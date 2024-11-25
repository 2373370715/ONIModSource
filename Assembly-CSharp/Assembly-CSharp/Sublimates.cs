using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/Sublimates")]
public class Sublimates : KMonoBehaviour, ISim200ms {
    private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate
        = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data) {
                                                             component.OnAbsorb(data);
                                                         });

    private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate
        = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data) {
                                                             component.OnSplitFromChunk(data);
                                                         });

    public  bool                     decayStorage;
    private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

    [SerializeField]
    public Info info;

    private EmitState lastEmitState = (EmitState)(-1);

    [MyCmpReq]
    private PrimaryElement primaryElement;

    [MyCmpReq]
    private KSelectable selectable;

    [SerializeField]
    public SpawnFXHashes spawnFXHash;

    [Serialize]
    private float sublimatedMass;

    public float Temperature => primaryElement.Temperature;

    public void Sim200ms(float dt) {
        var num = Grid.PosToCell(transform.GetPosition());
        if (!Grid.IsValidCell(num)) return;

        var flag      = this.HasTag(GameTags.Sealed);
        var component = GetComponent<Pickupable>();
        var storage   = component != null ? component.storage : null;
        if (flag && !decayStorage) return;

        if (flag && storage != null && storage.HasTag(GameTags.CorrosionProof)) return;

        var element = ElementLoader.FindElementByHash(info.sublimatedElement);
        if (primaryElement.Temperature <= element.lowTemp) {
            RefreshStatusItem(EmitState.BlockedOnTemperature);
            return;
        }

        var num2 = Grid.Mass[num];
        if (num2 < info.maxDestinationMass) {
            var num3 = primaryElement.Mass;
            if (num3 > 0f) {
                var num4 = Mathf.Pow(num3, info.massPower);
                var num5 = Mathf.Max(info.sublimationRate, info.sublimationRate * num4);
                num5           *= dt;
                num5           =  Mathf.Min(num5, num3);
                sublimatedMass += num5;
                num3           -= num5;
                if (sublimatedMass > info.minSublimationAmount) {
                    var  num6 = sublimatedMass / primaryElement.Mass;
                    byte diseaseIdx;
                    int  num7;
                    if (info.diseaseIdx == 255) {
                        diseaseIdx = primaryElement.DiseaseIdx;
                        num7       = (int)(primaryElement.DiseaseCount * num6);
                        primaryElement.ModifyDiseaseCount(-num7, "Sublimates.SimUpdate");
                    } else {
                        var num8 = sublimatedMass / info.sublimationRate;
                        diseaseIdx = info.diseaseIdx;
                        num7       = (int)(info.diseaseCount * num8);
                    }

                    var num9 = Mathf.Min(sublimatedMass, info.maxDestinationMass - num2);
                    if (num9 <= 0f) {
                        RefreshStatusItem(EmitState.BlockedOnPressure);
                        return;
                    }

                    Emit(num, num9, primaryElement.Temperature, diseaseIdx, num7);
                    sublimatedMass      = Mathf.Max(0f, sublimatedMass      - num9);
                    primaryElement.Mass = Mathf.Max(0f, primaryElement.Mass - num9);
                    UpdateStorage();
                    RefreshStatusItem(EmitState.Emitting);
                    if (flag && decayStorage && storage != null)
                        storage.Trigger(-794517298,
                                        new BuildingHP.DamageSourceInfo {
                                            damage               = 1,
                                            source               = BUILDINGS.DAMAGESOURCES.CORROSIVE_ELEMENT,
                                            popString            = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CORROSIVE_ELEMENT,
                                            fullDamageEffectName = "smoke_damage_kanim"
                                        });
                }
            } else if (sublimatedMass > 0f) {
                var num10 = Mathf.Min(sublimatedMass, info.maxDestinationMass - num2);
                if (num10 > 0f) {
                    Emit(num,
                         num10,
                         primaryElement.Temperature,
                         primaryElement.DiseaseIdx,
                         primaryElement.DiseaseCount);

                    sublimatedMass      = Mathf.Max(0f, sublimatedMass      - num10);
                    primaryElement.Mass = Mathf.Max(0f, primaryElement.Mass - num10);
                    UpdateStorage();
                    RefreshStatusItem(EmitState.Emitting);
                    return;
                }

                RefreshStatusItem(EmitState.BlockedOnPressure);
            } else if (!primaryElement.KeepZeroMassObject) Util.KDestroyGameObject(gameObject);
        } else
            RefreshStatusItem(EmitState.BlockedOnPressure);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(-2064133523, OnAbsorbDelegate);
        Subscribe(1335436905,  OnSplitFromChunkDelegate);
        simRenderLoadBalance = true;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", this);
        RefreshStatusItem(EmitState.Emitting);
    }

    protected override void OnCleanUp() {
        flowAccumulator = Game.Instance.accumulators.Remove(flowAccumulator);
        base.OnCleanUp();
    }

    private void OnAbsorb(object data) {
        var pickupable = (Pickupable)data;
        if (pickupable != null) {
            var component                         = pickupable.GetComponent<Sublimates>();
            if (component != null) sublimatedMass += component.sublimatedMass;
        }
    }

    private void OnSplitFromChunk(object data) {
        var pickupable     = data as Pickupable;
        var primaryElement = pickupable.PrimaryElement;
        var component      = pickupable.GetComponent<Sublimates>();
        if (component == null) return;

        var mass  = this.primaryElement.Mass;
        var mass2 = primaryElement.Mass;
        var num   = mass / (mass2 + mass);
        sublimatedMass = component.sublimatedMass * num;
        var num2 = 1f - num;
        component.sublimatedMass *= num2;
    }

    private void UpdateStorage() {
        var component = GetComponent<Pickupable>();
        if (component != null && component.storage != null) component.storage.Trigger(-1697596308, gameObject);
    }

    private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count) {
        SimMessages.AddRemoveSubstance(cell,
                                       info.sublimatedElement,
                                       CellEventLogger.Instance.SublimatesEmit,
                                       mass,
                                       temperature,
                                       disease_idx,
                                       disease_count);

        Game.Instance.accumulators.Accumulate(flowAccumulator, mass);
        if (spawnFXHash != SpawnFXHashes.None) {
            transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
            Game.Instance.SpawnFX(spawnFXHash, transform.GetPosition(), 0f);
        }
    }

    public float AvgFlowRate() { return Game.Instance.accumulators.GetAverageRate(flowAccumulator); }

    private void RefreshStatusItem(EmitState newEmitState) {
        if (newEmitState == lastEmitState) return;

        switch (newEmitState) {
            case EmitState.Emitting:
                if (info.sublimatedElement == SimHashes.Oxygen)
                    selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                             Db.Get().BuildingStatusItems.EmittingOxygenAvg,
                                             this);
                else
                    selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                             Db.Get().BuildingStatusItems.EmittingGasAvg,
                                             this);

                break;
            case EmitState.BlockedOnPressure:
                selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                         Db.Get().BuildingStatusItems.EmittingBlockedHighPressure,
                                         this);

                break;
            case EmitState.BlockedOnTemperature:
                selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                         Db.Get().BuildingStatusItems.EmittingBlockedLowTemperature,
                                         this);

                break;
        }

        lastEmitState = newEmitState;
    }

    [Serializable]
    public struct Info {
        public Info(float     rate,
                    float     min_amount,
                    float     max_destination_mass,
                    float     mass_power,
                    SimHashes element,
                    byte      disease_idx   = 255,
                    int       disease_count = 0) {
            sublimationRate      = rate;
            minSublimationAmount = min_amount;
            maxDestinationMass   = max_destination_mass;
            massPower            = mass_power;
            sublimatedElement    = element;
            diseaseIdx           = disease_idx;
            diseaseCount         = disease_count;
        }

        public float sublimationRate;
        public float minSublimationAmount;
        public float maxDestinationMass;
        public float massPower;
        public byte  diseaseIdx;
        public int   diseaseCount;

        [HashedEnum]
        public SimHashes sublimatedElement;
    }

    private enum EmitState {
        Emitting,
        BlockedOnPressure,
        BlockedOnTemperature
    }
}