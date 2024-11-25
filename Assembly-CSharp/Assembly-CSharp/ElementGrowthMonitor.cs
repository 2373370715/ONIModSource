using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class ElementGrowthMonitor
    : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget,
        ElementGrowthMonitor.Def> {
    private static readonly HashedString[] GROWTH_SYMBOL_NAMES = {
        "del_ginger1", "del_ginger2", "del_ginger3", "del_ginger4", "del_ginger5"
    };

    public State        fullyGrown;
    public GrowingState growing;
    public State        halted;
    public Tag[]        HungryTags = { GameTags.Creatures.Hungry };

    public override void InitializeStates(out BaseState default_state) {
        default_state = growing;
        root.Enter(delegate(Instance smi) { UpdateGrowth(smi, 0f); })
            .Update(UpdateGrowth, UpdateRate.SIM_1000ms)
            .EventHandler(GameHashes.EatSolidComplete,
                          delegate(Instance smi, object data) { smi.OnEatSolidComplete(data); });

        growing.DefaultState(growing.growing)
               .Transition(fullyGrown, IsFullyGrown, UpdateRate.SIM_1000ms)
               .TagTransition(HungryTags, halted);

        growing.growing.Transition(growing.stunted, Not(IsConsumedInTemperatureRange), UpdateRate.SIM_1000ms)
               .ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthGrowing, null)
               .Enter(ApplyModifier)
               .Exit(RemoveModifier);

        growing.stunted.Transition(growing.growing, IsConsumedInTemperatureRange, UpdateRate.SIM_1000ms)
               .ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthStunted, null)
               .Enter(ApplyModifier)
               .Exit(RemoveModifier);

        halted.TagTransition(HungryTags, growing, true)
              .ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthHalted, null);

        fullyGrown.ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthComplete, null)
                  .ToggleBehaviour(GameTags.Creatures.ScalesGrown, smi => smi.HasTag(GameTags.Creatures.CanMolt))
                  .Transition(growing, Not(IsFullyGrown), UpdateRate.SIM_1000ms);
    }

    private static bool IsConsumedInTemperatureRange(Instance smi) {
        return smi.lastConsumedTemperature == 0f ||
               (smi.lastConsumedTemperature >= smi.def.minTemperature &&
                smi.lastConsumedTemperature <= smi.def.maxTemperature);
    }

    private static bool IsFullyGrown(Instance smi) { return smi.elementGrowth.value >= smi.elementGrowth.GetMax(); }

    private static void ApplyModifier(Instance smi) {
        if (smi.IsInsideState(smi.sm.growing.growing)) {
            smi.elementGrowth.deltaAttribute.Add(smi.growingGrowthModifier);
            return;
        }

        if (smi.IsInsideState(smi.sm.growing.stunted)) smi.elementGrowth.deltaAttribute.Add(smi.stuntedGrowthModifier);
    }

    private static void RemoveModifier(Instance smi) {
        smi.elementGrowth.deltaAttribute.Remove(smi.growingGrowthModifier);
        smi.elementGrowth.deltaAttribute.Remove(smi.stuntedGrowthModifier);
    }

    private static void UpdateGrowth(Instance smi, float dt) {
        var num = (int)(smi.def.levelCount * smi.elementGrowth.value / 100f);
        if (smi.currentGrowthLevel != num) {
            var component = smi.GetComponent<KBatchedAnimController>();
            for (var i = 0; i < GROWTH_SYMBOL_NAMES.Length; i++) {
                var is_visible = i == num - 1;
                component.SetSymbolVisiblity(GROWTH_SYMBOL_NAMES[i], is_visible);
            }

            smi.currentGrowthLevel = num;
        }
    }

    public class Def : BaseDef, IGameObjectEffectDescriptor {
        public float defaultGrowthRate;
        public float dropMass;
        public Tag   itemDroppedOnShear;
        public int   levelCount;
        public float maxTemperature;
        public float minTemperature;

        public List<Descriptor> GetDescriptors(GameObject obj) {
            return new List<Descriptor> {
                new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_TEMP.Replace("{Item}", itemDroppedOnShear.ProperName())
                                 .Replace("{Amount}",  GameUtil.GetFormattedMass(dropMass))
                                 .Replace("{Time}",    GameUtil.GetFormattedCycles(1f / defaultGrowthRate))
                                 .Replace("{TempMin}", GameUtil.GetFormattedTemperature(minTemperature))
                                 .Replace("{TempMax}", GameUtil.GetFormattedTemperature(maxTemperature)),
                               UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_TEMP
                                 .Replace("{Item}",    itemDroppedOnShear.ProperName())
                                 .Replace("{Amount}",  GameUtil.GetFormattedMass(dropMass))
                                 .Replace("{Time}",    GameUtil.GetFormattedCycles(1f / defaultGrowthRate))
                                 .Replace("{TempMin}", GameUtil.GetFormattedTemperature(minTemperature))
                                 .Replace("{TempMax}", GameUtil.GetFormattedTemperature(maxTemperature)))
            };
        }

        public override void Configure(GameObject prefab) {
            prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
        }
    }

    public class GrowingState : State {
        public State growing;
        public State stunted;
    }

    public new class Instance : GameInstance, IShearable {
        public int               currentGrowthLevel = -1;
        public AmountInstance    elementGrowth;
        public AttributeModifier growingGrowthModifier;

        [Serialize]
        public SimHashes lastConsumedElement;

        [Serialize]
        public float lastConsumedTemperature;

        public AttributeModifier stuntedGrowthModifier;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            elementGrowth       = Db.Get().Amounts.ElementGrowth.Lookup(gameObject);
            elementGrowth.value = elementGrowth.GetMax();
            growingGrowthModifier = new AttributeModifier(elementGrowth.amount.deltaAttribute.Id,
                                                          def.defaultGrowthRate * 100f,
                                                          CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME);

            stuntedGrowthModifier = new AttributeModifier(elementGrowth.amount.deltaAttribute.Id,
                                                          def.defaultGrowthRate * 20f,
                                                          CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME);
        }

        public bool IsFullyGrown() { return currentGrowthLevel == def.levelCount; }

        public void Shear() {
            var component  = smi.GetComponent<PrimaryElement>();
            var gameObject = Util.KInstantiate(Assets.GetPrefab(def.itemDroppedOnShear));
            gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)),
                                                               Grid.SceneLayer.Ore));

            var component2 = gameObject.GetComponent<PrimaryElement>();
            component2.Temperature = component.Temperature;
            component2.Mass        = def.dropMass;
            component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
            gameObject.SetActive(true);
            var initial_velocity = new Vector2(Random.Range(-1f, 1f) * 1f, Random.value * 2f + 2f);
            if (GameComps.Fallers.Has(gameObject)) GameComps.Fallers.Remove(gameObject);
            GameComps.Fallers.Add(gameObject, initial_velocity);
            elementGrowth.value = 0f;
            UpdateGrowth(this, 0f);
        }

        public void OnEatSolidComplete(object data) {
            var kprefabID = (KPrefabID)data;
            if (kprefabID == null) return;

            var component = kprefabID.GetComponent<PrimaryElement>();
            lastConsumedElement     = component.ElementID;
            lastConsumedTemperature = component.Temperature;
        }
    }
}