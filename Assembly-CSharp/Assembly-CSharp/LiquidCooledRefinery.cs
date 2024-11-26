using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class LiquidCooledRefinery : ComplexFabricator {
    public static readonly Operational.Flag coolantOutputPipeEmpty
        = new Operational.Flag("coolantOutputPipeEmpty", Operational.Flag.Type.Requirement);

    private static readonly EventSystem.IntraObjectHandler<LiquidCooledRefinery> OnStorageChangeDelegate
        = new EventSystem.IntraObjectHandler<LiquidCooledRefinery>(delegate(LiquidCooledRefinery component,
                                                                            object               data) {
                                                                       component.OnStorageChange(data);
                                                                   });

    [MyCmpReq]
    private ConduitConsumer conduitConsumer;

    public  Tag             coolantTag;
    private MeterController meter_coolant;
    private MeterController meter_metal;
    public  float           minCoolantMass = 100f;
    private int             outputCell;
    public  float           outputTemperature = 313.15f;
    private StatesInstance  smi;
    public  float           thermalFudge = 0.8f;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(-1697596308, OnStorageChangeDelegate);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        var component = GetComponent<KBatchedAnimController>();
        meter_coolant = new MeterController(component,
                                            "meter_target",
                                            "meter_coolant",
                                            Meter.Offset.Infront,
                                            Grid.SceneLayer.NoLayer,
                                            Vector3.zero,
                                            null);

        meter_metal = new MeterController(component,
                                          "meter_target_metal",
                                          "meter_metal",
                                          Meter.Offset.Infront,
                                          Grid.SceneLayer.NoLayer,
                                          Vector3.zero,
                                          null);

        meter_metal.SetPositionPercent(1f);
        smi = new StatesInstance(this);
        smi.StartSM();
        Game.Instance.liquidConduitFlow.AddConduitUpdater(OnConduitUpdate);
        var component2 = GetComponent<Building>();
        outputCell = component2.GetUtilityOutputCell();
        workable.OnWorkTickActions = delegate {
                                         var percentComplete = workable.GetPercentComplete();
                                         meter_metal.SetPositionPercent(percentComplete);
                                     };
    }

    protected override void OnCleanUp() {
        Game.Instance.liquidConduitFlow.RemoveConduitUpdater(OnConduitUpdate);
        base.OnCleanUp();
    }

    private void OnConduitUpdate(float dt) {
        var flag = Game.Instance.liquidConduitFlow.GetContents(outputCell).mass > 0f;
        smi.sm.outputBlocked.Set(flag, smi);
        operational.SetFlag(coolantOutputPipeEmpty, !flag);
    }

    public bool HasEnoughCoolant() {
        return inStorage.GetAmountAvailable(coolantTag) + buildStorage.GetAmountAvailable(coolantTag) >= minCoolantMass;
    }

    private void OnStorageChange(object data) {
        var amountAvailable = inStorage.GetAmountAvailable(coolantTag);
        var capacityKG      = conduitConsumer.capacityKG;
        var positionPercent = Mathf.Clamp01(amountAvailable / capacityKG);
        if (meter_coolant != null) meter_coolant.SetPositionPercent(positionPercent);
    }

    protected override bool HasIngredients(ComplexRecipe recipe, Storage storage) {
        return storage.GetAmountAvailable(coolantTag) >= minCoolantMass && base.HasIngredients(recipe, storage);
    }

    protected override void TransferCurrentRecipeIngredientsForBuild() {
        base.TransferCurrentRecipeIngredientsForBuild();
        var num = minCoolantMass;
        while (buildStorage.GetAmountAvailable(coolantTag) < minCoolantMass &&
               inStorage.GetAmountAvailable(coolantTag)    > 0f             &&
               num                                         > 0f) {
            var num2 = inStorage.Transfer(buildStorage, coolantTag, num, false, true);
            num -= num2;
        }
    }

    protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe) {
        var list      = base.SpawnOrderProduct(recipe);
        var component = list[0].GetComponent<PrimaryElement>();
        component.Temperature = outputTemperature;
        var num = GameUtil.CalculateEnergyDeltaForElementChange(component.Element.specificHeatCapacity,
                                                                component.Mass,
                                                                component.Element.highTemp,
                                                                outputTemperature);

        var pooledList = ListPool<GameObject, LiquidCooledRefinery>.Allocate();
        buildStorage.Find(coolantTag, pooledList);
        var num2 = 0f;
        foreach (var gameObject in pooledList) {
            var component2                  = gameObject.GetComponent<PrimaryElement>();
            if (component2.Mass != 0f) num2 += component2.Mass * component2.Element.specificHeatCapacity;
        }

        foreach (var gameObject2 in pooledList) {
            var component3 = gameObject2.GetComponent<PrimaryElement>();
            if (component3.Mass != 0f) {
                var num3      = component3.Mass * component3.Element.specificHeatCapacity / num2;
                var kilowatts = -num                                                      * num3 * thermalFudge;
                var num4 = GameUtil.CalculateTemperatureChange(component3.Element.specificHeatCapacity,
                                                               component3.Mass,
                                                               kilowatts);

                var temperature = component3.Temperature;
                component3.Temperature += num4;
            }
        }

        buildStorage.Transfer(outStorage, coolantTag, float.MaxValue, false, true);
        pooledList.Recycle();
        return list;
    }

    public override List<Descriptor> GetDescriptors(GameObject go) {
        var descriptors = base.GetDescriptors(go);
        descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.COOLANT,
                                                     coolantTag.ProperName(),
                                                     GameUtil.GetFormattedMass(minCoolantMass)),
                                       string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.COOLANT,
                                                     coolantTag.ProperName(),
                                                     GameUtil.GetFormattedMass(minCoolantMass)),
                                       Descriptor.DescriptorType.Requirement));

        return descriptors;
    }

    public override List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe) {
        var    list           = base.AdditionalEffectsForRecipe(recipe);
        var    component      = Assets.GetPrefab(recipe.results[0].material).GetComponent<PrimaryElement>();
        var    primaryElement = inStorage.FindFirstWithMass(coolantTag);
        string format         = UI.BUILDINGEFFECTS.TOOLTIPS.REFINEMENT_ENERGY_HAS_COOLANT;
        if (primaryElement == null) {
            primaryElement = Assets.GetPrefab(GameTags.Water).GetComponent<PrimaryElement>();
            format         = UI.BUILDINGEFFECTS.TOOLTIPS.REFINEMENT_ENERGY_NO_COOLANT;
        }

        var num = -GameUtil.CalculateEnergyDeltaForElementChange(component.Element.specificHeatCapacity,
                                                                 recipe.results[0].amount,
                                                                 component.Element.highTemp,
                                                                 outputTemperature);

        var temp = GameUtil.CalculateTemperatureChange(primaryElement.Element.specificHeatCapacity,
                                                       minCoolantMass,
                                                       num * thermalFudge);

        list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.REFINEMENT_ENERGY, GameUtil.GetFormattedJoules(num)),
                                string.Format(format,
                                              GameUtil.GetFormattedJoules(num),
                                              primaryElement.GetProperName(),
                                              GameUtil.GetFormattedTemperature(temp,
                                                                               GameUtil.TimeSlice.None,
                                                                               GameUtil.TemperatureInterpretation
                                                                                   .Relative))));

        return list;
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, LiquidCooledRefinery, object>.GameInstance {
        public StatesInstance(LiquidCooledRefinery master) : base(master) { }
    }

    public class States : GameStateMachine<States, StatesInstance, LiquidCooledRefinery> {
        public static StatusItem    waitingForCoolantStatus;
        public        State         output_blocked;
        public        BoolParameter outputBlocked;
        public        State         ready;
        public        State         waiting_for_coolant;

        public override void InitializeStates(out BaseState default_state) {
            if (waitingForCoolantStatus == null) {
                waitingForCoolantStatus = new StatusItem("waitingForCoolantStatus",
                                                         BUILDING.STATUSITEMS.ENOUGH_COOLANT.NAME,
                                                         BUILDING.STATUSITEMS.ENOUGH_COOLANT.TOOLTIP,
                                                         "status_item_no_liquid_to_pump",
                                                         StatusItem.IconType.Custom,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID);

                waitingForCoolantStatus.resolveStringCallback = delegate(string str, object obj) {
                                                                    var liquidCooledRefinery
                                                                        = (LiquidCooledRefinery)obj;

                                                                    return string.Format(str,
                                                                     liquidCooledRefinery.coolantTag.ProperName(),
                                                                     GameUtil.GetFormattedMass(liquidCooledRefinery
                                                                         .minCoolantMass));
                                                                };
            }

            default_state = waiting_for_coolant;
            waiting_for_coolant.ToggleStatusItem(waitingForCoolantStatus, smi => smi.master)
                               .EventTransition(GameHashes.OnStorageChange, ready, smi => smi.master.HasEnoughCoolant())
                               .ParamTransition(outputBlocked, output_blocked, IsTrue);

            ready.EventTransition(GameHashes.OnStorageChange,
                                  waiting_for_coolant,
                                  smi => !smi.master.HasEnoughCoolant())
                 .ParamTransition(outputBlocked, output_blocked, IsTrue)
                 .Enter(delegate(StatesInstance smi) { smi.master.SetQueueDirty(); });

            output_blocked.ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, null)
                          .ParamTransition(outputBlocked, waiting_for_coolant, IsFalse);
        }
    }
}