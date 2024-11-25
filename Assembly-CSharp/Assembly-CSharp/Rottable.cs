using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def> {
    public enum RotAtmosphereQuality {
        Normal,
        Sterilizing,
        Contaminating
    }

    public enum RotRefrigerationLevel {
        Normal,
        Refrigerated,
        Frozen
    }

    private static readonly Tag[] PRESERVED_TAGS = { GameTags.Preserved, GameTags.Dehydrated, GameTags.Entombed };
    private static readonly RotCB rotCB          = new RotCB();

    public static Dictionary<int, RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, RotAtmosphereQuality> {
        { 721531317, RotAtmosphereQuality.Contaminating },
        { 1887387588, RotAtmosphereQuality.Contaminating },
        { -1528777920, RotAtmosphereQuality.Normal },
        { 1836671383, RotAtmosphereQuality.Normal },
        { 1960575215, RotAtmosphereQuality.Sterilizing },
        { -899515856, RotAtmosphereQuality.Sterilizing },
        { -1554872654, RotAtmosphereQuality.Sterilizing },
        { -1858722091, RotAtmosphereQuality.Sterilizing },
        { 758759285, RotAtmosphereQuality.Sterilizing },
        { -1046145888, RotAtmosphereQuality.Sterilizing },
        { -1324664829, RotAtmosphereQuality.Sterilizing },
        { -1406916018, RotAtmosphereQuality.Sterilizing },
        { -432557516, RotAtmosphereQuality.Sterilizing },
        { -805366663, RotAtmosphereQuality.Sterilizing },
        { 1966552544, RotAtmosphereQuality.Sterilizing }
    };

    public State          Fresh;
    public State          Preserved;
    public FloatParameter rotParameter;
    public State          Spoiled;
    public State          Stale;
    public State          Stale_Pre;

    public override void InitializeStates(out BaseState default_state) {
        default_state = Fresh;
        serializable  = SerializeType.Both_DEPRECATED;
        root.TagTransition(GameTags.Preserved, Preserved).TagTransition(GameTags.Entombed, Preserved);
        Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, smi => smi)
             .ParamTransition(rotParameter,
                              Stale_Pre,
                              (smi, p) => p <= smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime))
             .Update(delegate(Instance smi, float dt) { smi.sm.rotParameter.Set(smi.RotValue, smi); },
                     UpdateRate.SIM_1000ms,
                     true)
             .FastUpdate("Rot", rotCB, UpdateRate.SIM_1000ms, true);

        Preserved.TagTransition(PRESERVED_TAGS, Fresh, true)
                 .Enter("RefreshModifiers", delegate(Instance smi) { smi.RefreshModifiers(0f); });

        Stale_Pre.Enter(delegate(Instance smi) { smi.GoTo(Stale); });
        Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, smi => smi)
             .ParamTransition(rotParameter,
                              Fresh,
                              (smi, p) => p > smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime))
             .ParamTransition(rotParameter, Spoiled, IsLTEZero)
             .Update(delegate(Instance smi, float dt) { smi.sm.rotParameter.Set(smi.RotValue, smi); },
                     UpdateRate.SIM_1000ms)
             .FastUpdate("Rot", rotCB, UpdateRate.SIM_1000ms);

        Spoiled.Enter(delegate(Instance smi) {
                          var gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile");
                          gameObject.gameObject.GetComponent<KSelectable>()
                                    .SetName(UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());

                          gameObject.transform.SetPosition(smi.master.transform.GetPosition());
                          gameObject.GetComponent<PrimaryElement>().Mass
                              = smi.master.GetComponent<PrimaryElement>().Mass;

                          gameObject.GetComponent<PrimaryElement>().Temperature
                              = smi.master.GetComponent<PrimaryElement>().Temperature;

                          gameObject.SetActive(true);
                          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource,
                                                        ITEMS.FOOD.ROTPILE.NAME,
                                                        gameObject.transform);

                          var component = smi.GetComponent<Edible>();
                          if (component != null) {
                              if (component.worker != null) {
                                  var component2 = component.worker.GetComponent<ChoreDriver>();
                                  if (component2 != null && component2.GetCurrentChore() != null)
                                      component2.GetCurrentChore().Fail("food rotted");
                              }

                              ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated,
                                                                 -component.Calories,
                                                                 StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.ROTTED,
                                                                  "{0}",
                                                                  smi.gameObject.GetProperName()),
                                                                 UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
                          }

                          Util.KDestroyGameObject(smi.gameObject);
                      });
    }

    private static string OnStaleTooltip(List<Notification> notifications, object data) {
        var text = "\n";
        foreach (var notification in notifications)
            if (notification.tooltipData != null) {
                var gameObject               = (GameObject)notification.tooltipData;
                if (gameObject != null) text = text + "\n" + gameObject.GetProperName();
            }

        return string.Format(MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, text);
    }

    public static void SetStatusItems(IRottable rottable) {
        Grid.PosToCell(rottable.gameObject);
        var component             = rottable.gameObject.GetComponent<KSelectable>();
        var rotRefrigerationLevel = RefrigerationLevel(rottable);
        if (rotRefrigerationLevel != RotRefrigerationLevel.Refrigerated) {
            if (rotRefrigerationLevel == RotRefrigerationLevel.Frozen)
                component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature,
                                        Db.Get().CreatureStatusItems.RefrigeratedFrozen,
                                        rottable);
            else
                component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature,
                                        Db.Get().CreatureStatusItems.Unrefrigerated,
                                        rottable);
        } else
            component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature,
                                    Db.Get().CreatureStatusItems.Refrigerated,
                                    rottable);

        var rotAtmosphereQuality = AtmosphereQuality(rottable);
        if (rotAtmosphereQuality == RotAtmosphereQuality.Sterilizing) {
            component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere,
                                    Db.Get().CreatureStatusItems.SterilizingAtmosphere);

            return;
        }

        if (rotAtmosphereQuality == RotAtmosphereQuality.Contaminating) {
            component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere,
                                    Db.Get().CreatureStatusItems.ContaminatedAtmosphere);

            return;
        }

        component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, null);
    }

    public static bool IsInActiveFridge(IRottable rottable) {
        var component = rottable.gameObject.GetComponent<Pickupable>();
        if (component != null && component.storage != null) {
            var component2 = component.storage.GetComponent<Refrigerator>();
            return component2 != null && component2.IsActive();
        }

        return false;
    }

    public static RotRefrigerationLevel RefrigerationLevel(IRottable rottable) {
        var num       = Grid.PosToCell(rottable.gameObject);
        var smi       = rottable.gameObject.GetSMI<Instance>();
        var component = rottable.gameObject.GetComponent<PrimaryElement>();
        var num2      = component.Temperature;
        var flag      = false;
        if (!Grid.IsValidCell(num)) {
            if (!smi.IsRottableInSpace()) return RotRefrigerationLevel.Normal;

            flag = true;
        }

        if (!flag && Grid.Element[num].id != SimHashes.Vacuum)
            num2 = Mathf.Min(Grid.Temperature[num], component.Temperature);

        if (num2 < rottable.PreserveTemperature) return RotRefrigerationLevel.Frozen;

        if (num2 < rottable.RotTemperature || IsInActiveFridge(rottable)) return RotRefrigerationLevel.Refrigerated;

        return RotRefrigerationLevel.Normal;
    }

    public static RotAtmosphereQuality AtmosphereQuality(IRottable rottable) {
        var num  = Grid.PosToCell(rottable.gameObject);
        var num2 = Grid.CellAbove(num);
        if (!Grid.IsValidCell(num)) {
            if (rottable.gameObject.GetSMI<Instance>().IsRottableInSpace()) return RotAtmosphereQuality.Sterilizing;

            return RotAtmosphereQuality.Normal;
        }

        var id                   = Grid.Element[num].id;
        var rotAtmosphereQuality = RotAtmosphereQuality.Normal;
        AtmosphereModifier.TryGetValue((int)id, out rotAtmosphereQuality);
        var rotAtmosphereQuality2 = RotAtmosphereQuality.Normal;
        if (Grid.IsValidCell(num2)) {
            var id2 = Grid.Element[num2].id;
            if (!AtmosphereModifier.TryGetValue((int)id2, out rotAtmosphereQuality2))
                rotAtmosphereQuality2 = rotAtmosphereQuality;
        } else
            rotAtmosphereQuality2 = rotAtmosphereQuality;

        if (rotAtmosphereQuality == rotAtmosphereQuality2) return rotAtmosphereQuality;

        if (rotAtmosphereQuality  == RotAtmosphereQuality.Contaminating ||
            rotAtmosphereQuality2 == RotAtmosphereQuality.Contaminating)
            return RotAtmosphereQuality.Contaminating;

        if (rotAtmosphereQuality == RotAtmosphereQuality.Normal || rotAtmosphereQuality2 == RotAtmosphereQuality.Normal)
            return RotAtmosphereQuality.Normal;

        return RotAtmosphereQuality.Sterilizing;
    }

    public class Def : BaseDef {
        public float preserveTemperature = 255.15f;
        public float rotTemperature      = 277.15f;
        public float spoilTime;
        public float staleTime;
    }

    private class RotCB : UpdateBucketWithUpdater<Instance>.IUpdater {
        public void Update(Instance smi, float dt) { smi.Rot(smi, dt); }
    }

    public new class Instance : GameInstance, IRottable {
        private static   AttributeModifier unrefrigeratedModifier;
        private static   AttributeModifier refrigeratedModifier;
        private static   AttributeModifier frozenModifier;
        private static   AttributeModifier contaminatedAtmosphereModifier;
        private static   AttributeModifier normalAtmosphereModifier;
        private static   AttributeModifier sterileAtmosphereModifier;
        public           Pickupable        pickupable;
        public           PrimaryElement    primaryElement;
        private readonly AmountInstance    rotAmountInstance;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            pickupable = gameObject.RequireComponent<Pickupable>();
            this.master.Subscribe(-2064133523, OnAbsorb);
            this.master.Subscribe(1335436905,  OnSplitFromChunk);
            primaryElement = gameObject.GetComponent<PrimaryElement>();
            var amounts = master.gameObject.GetAmounts();
            rotAmountInstance = amounts.Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
            rotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime));
            rotAmountInstance.SetValue(def.spoilTime);
            sm.rotParameter.Set(rotAmountInstance.value, smi);
            if (unrefrigeratedModifier == null) {
                unrefrigeratedModifier = new AttributeModifier(rotAmountInstance.amount.Id,
                                                               -0.7f,
                                                               DUPLICANTS.MODIFIERS.ROTTEMPERATURE.UNREFRIGERATED);

                refrigeratedModifier = new AttributeModifier(rotAmountInstance.amount.Id,
                                                             -0.2f,
                                                             DUPLICANTS.MODIFIERS.ROTTEMPERATURE.REFRIGERATED);

                frozenModifier = new AttributeModifier(rotAmountInstance.amount.Id,
                                                       --0f,
                                                       DUPLICANTS.MODIFIERS.ROTTEMPERATURE.FROZEN);

                contaminatedAtmosphereModifier
                    = new AttributeModifier(rotAmountInstance.amount.Id,
                                            -1f,
                                            DUPLICANTS.MODIFIERS.ROTATMOSPHERE.CONTAMINATED);

                normalAtmosphereModifier
                    = new AttributeModifier(rotAmountInstance.amount.Id,
                                            -0.3f,
                                            DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NORMAL);

                sterileAtmosphereModifier
                    = new AttributeModifier(rotAmountInstance.amount.Id,
                                            --0f,
                                            DUPLICANTS.MODIFIERS.ROTATMOSPHERE.STERILE);
            }

            RefreshModifiers(0f);
        }

        public float RotValue {
            get => rotAmountInstance.value;
            set {
                sm.rotParameter.Set(value, this);
                rotAmountInstance.SetValue(value);
            }
        }

        public float RotConstitutionPercentage => RotValue / def.spoilTime;
        public float RotTemperature            => def.rotTemperature;
        public float PreserveTemperature       => def.preserveTemperature;

        [OnDeserialized]
        private void OnDeserialized() {
            if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 23))
                rotAmountInstance.SetValue(rotAmountInstance.value * 2f);
        }

        public string StateString() {
            var result = "";
            if (smi.GetCurrentState() == sm.Fresh)
                result = Db.Get()
                           .CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.FRESH.NAME, this);

            if (smi.GetCurrentState() == sm.Stale)
                result = Db.Get()
                           .CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.STALE.NAME, this);

            return result;
        }

        public void Rot(Instance smi, float deltaTime) {
            RefreshModifiers(deltaTime);
            if (smi.pickupable.storage != null) smi.pickupable.storage.Trigger(-1197125120);
        }

        public bool IsRottableInSpace() {
            if (gameObject.GetMyWorld() == null) {
                var component = GetComponent<Pickupable>();
                if (component != null &&
                    component.storage &&
                    (component.storage.GetComponent<RocketModuleCluster>() ||
                     component.storage.GetComponent<ClusterTraveler>()))
                    return true;
            }

            return false;
        }

        public void RefreshModifiers(float dt) {
            if (GetMaster().isNull) return;

            if (!Grid.IsValidCell(Grid.PosToCell(gameObject)) && !IsRottableInSpace()) return;

            rotAmountInstance.deltaAttribute.ClearModifiers();
            var component = GetComponent<KPrefabID>();
            if (!component.HasAnyTags(PRESERVED_TAGS)) {
                var rotRefrigerationLevel = RefrigerationLevel(this);
                if (rotRefrigerationLevel != RotRefrigerationLevel.Refrigerated) {
                    if (rotRefrigerationLevel == RotRefrigerationLevel.Frozen)
                        rotAmountInstance.deltaAttribute.Add(frozenModifier);
                    else
                        rotAmountInstance.deltaAttribute.Add(unrefrigeratedModifier);
                } else
                    rotAmountInstance.deltaAttribute.Add(refrigeratedModifier);

                var rotAtmosphereQuality = AtmosphereQuality(this);
                if (rotAtmosphereQuality != RotAtmosphereQuality.Sterilizing) {
                    if (rotAtmosphereQuality == RotAtmosphereQuality.Contaminating)
                        rotAmountInstance.deltaAttribute.Add(contaminatedAtmosphereModifier);
                    else
                        rotAmountInstance.deltaAttribute.Add(normalAtmosphereModifier);
                } else
                    rotAmountInstance.deltaAttribute.Add(sterileAtmosphereModifier);
            }

            if (component.HasTag(Db.Get().Spices.PreservingSpice.Id))
                rotAmountInstance.deltaAttribute.Add(Db.Get().Spices.PreservingSpice.FoodModifier);

            SetStatusItems(this);
        }

        private void OnAbsorb(object data) {
            var pickupable = (Pickupable)data;
            if (pickupable != null) {
                var component      = gameObject.GetComponent<PrimaryElement>();
                var primaryElement = pickupable.PrimaryElement;
                var smi            = pickupable.gameObject.GetSMI<Instance>();
                if (component != null && primaryElement != null && smi != null) {
                    var num   = component.Units      * sm.rotParameter.Get(this.smi);
                    var num2  = primaryElement.Units * sm.rotParameter.Get(smi);
                    var value = (num + num2)         / (component.Units + primaryElement.Units);
                    sm.rotParameter.Set(value, this.smi);
                }
            }
        }

        public bool IsRotLevelStackable(Instance other) {
            return Mathf.Abs(RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.1f;
        }

        public string GetToolTip() { return rotAmountInstance.GetTooltip(); }

        private void OnSplitFromChunk(object data) {
            var pickupable = (Pickupable)data;
            if (pickupable != null) {
                var smi                   = pickupable.GetSMI<Instance>();
                if (smi != null) RotValue = smi.RotValue;
            }
        }

        public void OnPreserved(object data) {
            if ((bool)data) {
                smi.GoTo(sm.Preserved);
                return;
            }

            smi.GoTo(sm.Fresh);
        }
    }
}