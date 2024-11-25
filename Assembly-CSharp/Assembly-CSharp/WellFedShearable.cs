using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class WellFedShearable
    : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def> {
    public State fullyGrown;
    public State growing;

    public override void InitializeStates(out BaseState default_state) {
        default_state = growing;
        root.Enter(delegate(Instance smi) { UpdateScales(smi, 0f); })
            .Enter(delegate(Instance smi) {
                       if (smi.def.hideSymbols != null)
                           foreach (var symbol in smi.def.hideSymbols)
                               smi.animController.SetSymbolVisiblity(symbol, false);
                   })
            .Update(UpdateScales, UpdateRate.SIM_1000ms)
            .EventHandler(GameHashes.CaloriesConsumed,
                          delegate(Instance smi, object data) { smi.OnCaloriesConsumed(data); });

        growing.Enter(delegate(Instance smi) { UpdateScales(smi, 0f); })
               .Transition(fullyGrown, AreScalesFullyGrown, UpdateRate.SIM_1000ms);

        fullyGrown.Enter(delegate(Instance smi) { UpdateScales(smi, 0f); })
                  .ToggleBehaviour(GameTags.Creatures.ScalesGrown, smi => smi.HasTag(GameTags.Creatures.CanMolt))
                  .EventTransition(GameHashes.Molt, growing, Not(AreScalesFullyGrown))
                  .Transition(growing, Not(AreScalesFullyGrown), UpdateRate.SIM_1000ms);
    }

    private static bool AreScalesFullyGrown(Instance smi) { return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax(); }

    private static void UpdateScales(Instance smi, float dt) {
        var num = (int)(smi.def.levelCount * smi.scaleGrowth.value / 100f);
        if (smi.currentScaleLevel != num) {
            for (var i = 0; i < smi.def.scaleGrowthSymbols.Length; i++) {
                var is_visible = i <= num - 1;
                smi.animController.SetSymbolVisiblity(smi.def.scaleGrowthSymbols[i], is_visible);
            }

            smi.currentScaleLevel = num;
        }
    }

    public class Def : BaseDef, IGameObjectEffectDescriptor {
        public static KAnimHashedString[] SCALE_SYMBOL_NAMES = {
            "scale_0", "scale_1", "scale_2", "scale_3", "scale_4"
        };

        public float               caloriesPerCycle;
        public float               dropMass;
        public string              effectId;
        public float               growthDurationCycles;
        public KAnimHashedString[] hideSymbols;
        public Tag                 itemDroppedOnShear;
        public int                 levelCount;
        public Tag                 requiredDiet       = null;
        public KAnimHashedString[] scaleGrowthSymbols = SCALE_SYMBOL_NAMES;

        public List<Descriptor> GetDescriptors(GameObject obj) {
            return new List<Descriptor> {
                new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", itemDroppedOnShear.ProperName())
                                 .Replace("{Amount}", GameUtil.GetFormattedMass(dropMass))
                                 .Replace("{Time}",   GameUtil.GetFormattedCycles(growthDurationCycles * 600f)),
                               UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED
                                 .Replace("{Item}",   itemDroppedOnShear.ProperName())
                                 .Replace("{Amount}", GameUtil.GetFormattedMass(dropMass))
                                 .Replace("{Time}",   GameUtil.GetFormattedCycles(growthDurationCycles * 600f)))
            };
        }

        public override void Configure(GameObject prefab) {
            prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
        }
    }

    public new class Instance : GameInstance, IShearable {
        [MyCmpGet]
        public KBatchedAnimController animController;

        public int currentScaleLevel = -1;

        [MyCmpGet]
        private Effects effects;

        public AmountInstance scaleGrowth;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            scaleGrowth       = Db.Get().Amounts.ScaleGrowth.Lookup(gameObject);
            scaleGrowth.value = scaleGrowth.GetMax();
        }

        public bool IsFullyGrown() { return currentScaleLevel == def.levelCount; }

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
            scaleGrowth.value = 0f;
            UpdateScales(this, 0f);
        }

        public void OnCaloriesConsumed(object data) {
            var caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
            if (def.requiredDiet != null && caloriesConsumedEvent.tag != def.requiredDiet) return;

            var effectInstance                         = effects.Get(smi.def.effectId);
            if (effectInstance == null) effectInstance = effects.Add(smi.def.effectId, true);
            effectInstance.timeRemaining += caloriesConsumedEvent.calories / smi.def.caloriesPerCycle * 600f;
        }
    }
}