using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/AirConditioner")]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor, ISim200ms {
    private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate
        = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data) {
                                                                 component.OnOperationalChanged(data);
                                                             });

    private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate
        = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data) {
                                                                 component.OnActiveChanged(data);
                                                             });

    private static readonly Func<int, object, bool> UpdateStateCbDelegate = (cell, data) => UpdateStateCb(cell, data);

    [MyCmpReq]
    private BuildingComplete building;

    private int cellCount;

    [MyCmpReq]
    private ConduitConsumer consumer;

    private int   cooledAirOutputCell = -1;
    private float envTemp;

    [MyCmpGet]
    private KBatchedAnimHeatPostProcessingEffect heatEffect;

    public  bool  isLiquidConditioner;
    private float lastSampleTime = -1f;
    private float lowTempLag;
    public  float maxEnvironmentDelta = -50f;

    [MyCmpGet]
    private OccupyArea occupyArea;

    [MyCmpReq]
    protected Operational operational;

    [MyCmpReq]
    private KSelectable selectable;

    private bool showingHotEnv;
    private bool showingLowTemp;
    private Guid statusHandle;

    [MyCmpReq]
    protected Storage storage;

    private HandleVector<int>.Handle structureTemperature;
    public  float                    temperatureDelta = -14f;
    public  float                    lastEnvTemp { get; private set; }
    public  float                    lastGasTemp { get; private set; }

    [field: Serialize]
    public float TargetTemperature { get; }

    public List<Descriptor> GetDescriptors(GameObject go) {
        var list = new List<Descriptor>();
        var formattedTemperature
            = GameUtil.GetFormattedTemperature(temperatureDelta,
                                               GameUtil.TimeSlice.None,
                                               GameUtil.TemperatureInterpretation.Relative);

        var   element = ElementLoader.FindElementByName(isLiquidConditioner ? "Water" : "Oxygen");
        float num;
        if (isLiquidConditioner)
            num = Mathf.Abs(temperatureDelta * element.specificHeatCapacity * 10000f);
        else
            num = Mathf.Abs(temperatureDelta * element.specificHeatCapacity * 1000f);

        var dtu  = num * 1f;
        var item = default(Descriptor);
        var txt
            = string.Format(isLiquidConditioner
                                ? UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER
                                : UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER,
                            GameUtil.GetFormattedHeatEnergy(dtu),
                            GameUtil.GetFormattedTemperature(Mathf.Abs(temperatureDelta),
                                                             GameUtil.TimeSlice.None,
                                                             GameUtil.TemperatureInterpretation.Relative));

        var tooltip
            = string.Format(isLiquidConditioner
                                ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER
                                : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER,
                            GameUtil.GetFormattedHeatEnergy(dtu),
                            GameUtil.GetFormattedTemperature(Mathf.Abs(temperatureDelta),
                                                             GameUtil.TimeSlice.None,
                                                             GameUtil.TemperatureInterpretation.Relative));

        item.SetupDescriptor(txt, tooltip);
        list.Add(item);
        var item2 = default(Descriptor);
        item2.SetupDescriptor(string.Format(isLiquidConditioner
                                                ? UI.BUILDINGEFFECTS.LIQUIDCOOLING
                                                : UI.BUILDINGEFFECTS.GASCOOLING,
                                            formattedTemperature),
                              string.Format(isLiquidConditioner
                                                ? UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING
                                                : UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING,
                                            formattedTemperature));

        list.Add(item2);
        return list;
    }

    public void Sim200ms(float dt) {
        if (operational != null && !operational.IsOperational) {
            operational.SetActive(false);
            return;
        }

        UpdateState(dt);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(-592767678, OnOperationalChangedDelegate);
        Subscribe(824508782,  OnActiveChangedDelegate);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        GameScheduler.Instance.Schedule("InsulationTutorial",
                                        2f,
                                        delegate {
                                            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation);
                                        });

        structureTemperature = GameComps.StructureTemperatures.GetHandle(gameObject);
        gameObject.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
        cooledAirOutputCell = building.GetUtilityOutputCell();
    }

    private static bool UpdateStateCb(int cell, object data) {
        var airConditioner = data as AirConditioner;
        airConditioner.cellCount++;
        airConditioner.envTemp += Grid.Temperature[cell];
        return true;
    }

    private void UpdateState(float dt) {
        var value = consumer.IsSatisfied;
        envTemp   = 0f;
        cellCount = 0;
        if (occupyArea != null && gameObject != null) {
            occupyArea.TestArea(Grid.PosToCell(gameObject), this, UpdateStateCbDelegate);
            envTemp /= cellCount;
        }

        lastEnvTemp = envTemp;
        var items = storage.items;
        for (var i = 0; i < items.Count; i++) {
            var component = items[i].GetComponent<PrimaryElement>();
            if (component.Mass > 0f                                &&
                (!isLiquidConditioner || !component.Element.IsGas) &&
                (isLiquidConditioner  || !component.Element.IsLiquid)) {
                value       = true;
                lastGasTemp = component.Temperature;
                var num = component.Temperature + temperatureDelta;
                if (num < 1f) {
                    num        = 1f;
                    lowTempLag = Mathf.Min(lowTempLag + dt / 5f, 1f);
                } else
                    lowTempLag = Mathf.Min(lowTempLag - dt / 5f, 0f);

                var num2
                    = (isLiquidConditioner ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow)
                    .AddElement(cooledAirOutputCell,
                                component.ElementID,
                                component.Mass,
                                num,
                                component.DiseaseIdx,
                                component.DiseaseCount);

                component.KeepZeroMassObject = true;
                var num3 = num2 / component.Mass;
                var num4 = (int)(component.DiseaseCount * num3);
                component.Mass -= num2;
                component.ModifyDiseaseCount(-num4, "AirConditioner.UpdateState");
                var num5       = (num - component.Temperature) * component.Element.specificHeatCapacity * num2;
                var display_dt = lastSampleTime > 0f ? Time.time - lastSampleTime : 1f;
                lastSampleTime = Time.time;
                heatEffect.SetHeatBeingProducedValue(Mathf.Abs(num5));
                GameComps.StructureTemperatures.ProduceEnergy(structureTemperature,
                                                              -num5,
                                                              BUILDING.STATUSITEMS.OPERATINGENERGY
                                                                      .PIPECONTENTS_TRANSFER,
                                                              display_dt);

                break;
            }
        }

        if (Time.time - lastSampleTime > 2f) {
            GameComps.StructureTemperatures.ProduceEnergy(structureTemperature,
                                                          0f,
                                                          BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER,
                                                          Time.time - lastSampleTime);

            lastSampleTime = Time.time;
        }

        operational.SetActive(value);
        UpdateStatus();
    }

    private void OnOperationalChanged(object data) {
        if (operational.IsOperational) UpdateState(0f);
    }

    private void OnActiveChanged(object data) {
        UpdateStatus();
        if (operational.IsActive) {
            heatEffect.enabled = true;
            return;
        }

        heatEffect.enabled = false;
    }

    private void UpdateStatus() {
        if (operational.IsActive) {
            if (lowTempLag >= 1f && !showingLowTemp) {
                statusHandle = isLiquidConditioner
                                   ? selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                                              Db.Get().BuildingStatusItems.CoolingStalledColdLiquid,
                                                              this)
                                   : selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                                              Db.Get().BuildingStatusItems.CoolingStalledColdGas,
                                                              this);

                showingLowTemp = true;
                showingHotEnv  = false;
                return;
            }

            if (lowTempLag <= 0f && (showingHotEnv || showingLowTemp)) {
                statusHandle
                    = selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                               Db.Get().BuildingStatusItems.Cooling);

                showingLowTemp = false;
                showingHotEnv  = false;
                return;
            }

            if (statusHandle == Guid.Empty) {
                statusHandle
                    = selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                               Db.Get().BuildingStatusItems.Cooling);

                showingLowTemp = false;
                showingHotEnv  = false;
            }
        } else
            statusHandle = selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null);
    }
}