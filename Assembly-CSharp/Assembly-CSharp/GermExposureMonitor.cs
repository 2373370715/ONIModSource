using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

public class GermExposureMonitor : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance> {
    public enum ExposureState {
        None,
        Contact,
        Exposed,
        Contracted,
        Sick
    }

    public override void InitializeStates(out BaseState default_state) {
        default_state = root;
        serializable = SerializeType.Never;
        root.Update(delegate(Instance smi, float dt) { smi.OnInhaleExposureTick(dt); }, UpdateRate.SIM_1000ms, true)
            .EventHandler(GameHashes.EatCompleteEater, delegate(Instance smi, object obj) { smi.OnEatComplete(obj); })
            .EventHandler(GameHashes.SicknessAdded, delegate(Instance smi, object data) { smi.OnSicknessAdded(data); })
            .EventHandler(GameHashes.SicknessCured, delegate(Instance smi, object data) { smi.OnSicknessCured(data); })
            .EventHandler(GameHashes.SleepFinished, delegate(Instance smi) { smi.OnSleepFinished(); });
    }

    public static float GetContractionChance(float rating) { return 0.5f - 0.5f * (float)Math.Tanh(0.25 * rating); }

    public class ExposureStatusData {
        public ExposureType exposure_type;
        public Instance     owner;
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    public new class Instance : GameInstance {
        private readonly Dictionary<string, Guid> contactStatusItemHandles = new Dictionary<string, Guid>();

        [Serialize]
        private readonly Dictionary<string, ExposureState> exposureStates = new Dictionary<string, ExposureState>();

        [Serialize]
        private readonly Dictionary<string, float> exposureTiers = new Dictionary<string, float>();

        private readonly Dictionary<HashedString, InhaleTickInfo> inhaleExposureTick;

        [Serialize]
        public Dictionary<HashedString, DiseaseSourceInfo> lastDiseaseSources;

        [Serialize]
        public Dictionary<HashedString, float> lastExposureTime;

        private readonly PrimaryElement           primaryElement;
        private readonly Sicknesses               sicknesses;
        private readonly Dictionary<string, Guid> statusItemHandles = new Dictionary<string, Guid>();
        private readonly Traits                   traits;

        public Instance(IStateMachineTarget master) : base(master) {
            sicknesses         = master.GetComponent<MinionModifiers>().sicknesses;
            primaryElement     = master.GetComponent<PrimaryElement>();
            traits             = master.GetComponent<Traits>();
            lastDiseaseSources = new Dictionary<HashedString, DiseaseSourceInfo>();
            lastExposureTime   = new Dictionary<HashedString, float>();
            inhaleExposureTick = new Dictionary<HashedString, InhaleTickInfo>();
            GameClock.Instance.Subscribe(-722330267, OnNightTime);
            var component = GetComponent<OxygenBreather>();
            component.onSimConsume
                = (Action<Sim.MassConsumedCallback>)Delegate.Combine(component.onSimConsume,
                                                                     new Action<Sim.
                                                                         MassConsumedCallback>(OnAirConsumed));
        }

        public override void StartSM() {
            base.StartSM();
            RefreshStatusItems();
        }

        public override void StopSM(string reason) {
            GameClock.Instance.Unsubscribe(-722330267, OnNightTime);
            foreach (var exposureType in GERM_EXPOSURE.TYPES) {
                Guid guid;
                statusItemHandles.TryGetValue(exposureType.germ_id, out guid);
                guid = GetComponent<KSelectable>().RemoveStatusItem(guid);
            }

            base.StopSM(reason);
        }

        public void OnEatComplete(object obj) {
            var edible = (Edible)obj;
            var handle = GameComps.DiseaseContainers.GetHandle(edible.gameObject);
            if (handle != HandleVector<int>.InvalidHandle) {
                var header = GameComps.DiseaseContainers.GetHeader(handle);
                if (header.diseaseIdx != 255) {
                    var disease = Db.Get().Diseases[header.diseaseIdx];
                    var num     = edible.unitsConsumed / (edible.unitsConsumed + edible.Units);
                    var num2    = Mathf.CeilToInt(header.diseaseCount * num);
                    GameComps.DiseaseContainers.ModifyDiseaseCount(handle, -num2);
                    var component = edible.GetComponent<KPrefabID>();
                    InjectDisease(disease, num2, component.PrefabID(), Sickness.InfectionVector.Digestion);
                }
            }
        }

        public void OnAirConsumed(Sim.MassConsumedCallback mass_cb_info) {
            if (mass_cb_info.diseaseIdx != 255) {
                var disease = Db.Get().Diseases[mass_cb_info.diseaseIdx];
                InjectDisease(disease,
                              mass_cb_info.diseaseCount,
                              ElementLoader.elements[mass_cb_info.elemIdx].tag,
                              Sickness.InfectionVector.Inhalation);
            }
        }

        public void OnInhaleExposureTick(float dt) {
            foreach (var keyValuePair in inhaleExposureTick)
                if (keyValuePair.Value.inhaled) {
                    keyValuePair.Value.inhaled = false;
                    keyValuePair.Value.ticks++;
                } else
                    keyValuePair.Value.ticks = Mathf.Max(0, keyValuePair.Value.ticks - 1);
        }

        public void TryInjectDisease(byte disease_idx, int count, Tag source, Sickness.InfectionVector vector) {
            if (disease_idx != 255) {
                var disease = Db.Get().Diseases[disease_idx];
                InjectDisease(disease, count, source, vector);
            }
        }

        public float GetGermResistance() {
            return Db.Get().Attributes.GermResistance.Lookup(gameObject).GetTotalValue();
        }

        public float GetResistanceToExposureType(ExposureType exposureType, float overrideExposureTier = -1f) {
            var num             = overrideExposureTier;
            if (num == -1f) num = GetExposureTier(exposureType.germ_id);
            num = Mathf.Clamp(num, 1f, 3f);
            var num2       = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)num - 1];
            var totalValue = Db.Get().Attributes.GermResistance.Lookup(gameObject).GetTotalValue();
            return exposureType.base_resistance + totalValue + num2;
        }

        public int AssessDigestedGerms(ExposureType exposure_type, int count) {
            var exposure_threshold = exposure_type.exposure_threshold;
            var val                = count / exposure_threshold;
            return MathUtil.Clamp(1, 3, val);
        }

        public bool AssessInhaledGerms(ExposureType exposure_type) {
            InhaleTickInfo inhaleTickInfo;
            inhaleExposureTick.TryGetValue(exposure_type.germ_id, out inhaleTickInfo);
            if (inhaleTickInfo == null) {
                inhaleTickInfo                            = new InhaleTickInfo();
                inhaleExposureTick[exposure_type.germ_id] = inhaleTickInfo;
            }

            if (!inhaleTickInfo.inhaled) {
                var exposureTier = GetExposureTier(exposure_type.germ_id);
                inhaleTickInfo.inhaled = true;
                return inhaleTickInfo.ticks >= GERM_EXPOSURE.INHALE_TICK_THRESHOLD[(int)exposureTier];
            }

            return false;
        }

        public void InjectDisease(Disease disease, int count, Tag source, Sickness.InfectionVector vector) {
            foreach (var exposureType in GERM_EXPOSURE.TYPES)
                if (disease.id == exposureType.germ_id                &&
                    count      > exposureType.exposure_threshold      &&
                    HasMinExposurePeriodElapsed(exposureType.germ_id) &&
                    IsExposureValidForTraits(exposureType)) {
                    var sickness = exposureType.sickness_id != null
                                       ? Db.Get().Sicknesses.Get(exposureType.sickness_id)
                                       : null;

                    if (sickness == null || sickness.infectionVectors.Contains(vector)) {
                        var exposureState = GetExposureState(exposureType.germ_id);
                        var exposureTier  = GetExposureTier(exposureType.germ_id);
                        if (exposureState == ExposureState.None || exposureState == ExposureState.Contact) {
                            var contractionChance = GetContractionChance(GetResistanceToExposureType(exposureType));
                            SetExposureState(exposureType.germ_id, ExposureState.Contact);
                            if (contractionChance > 0f) {
                                lastDiseaseSources[disease.id]
                                    = new DiseaseSourceInfo(source, vector, contractionChance, transform.GetPosition());

                                if (exposureType.infect_immediately)
                                    InfectImmediately(exposureType);
                                else {
                                    var flag        = true;
                                    var flag2       = vector == Sickness.InfectionVector.Inhalation;
                                    var flag3       = vector == Sickness.InfectionVector.Digestion;
                                    var num         = 1;
                                    if (flag2) flag = AssessInhaledGerms(exposureType);
                                    if (flag3) num  = AssessDigestedGerms(exposureType, count);
                                    if (flag) {
                                        if (flag2) inhaleExposureTick[exposureType.germ_id].ticks = 0;
                                        SetExposureState(exposureType.germ_id, ExposureState.Exposed);
                                        SetExposureTier(exposureType.germ_id, num);
                                        var amount = Mathf.Clamp01(contractionChance);
                                        GermExposureTracker.Instance.AddExposure(exposureType, amount);
                                    }
                                }
                            }
                        } else if (exposureState == ExposureState.Exposed && exposureTier < 3f) {
                            var contractionChance2 = GetContractionChance(GetResistanceToExposureType(exposureType));
                            if (contractionChance2 > 0f) {
                                lastDiseaseSources[disease.id]
                                    = new DiseaseSourceInfo(source,
                                                            vector,
                                                            contractionChance2,
                                                            transform.GetPosition());

                                if (!exposureType.infect_immediately) {
                                    var flag4        = true;
                                    var flag5        = vector == Sickness.InfectionVector.Inhalation;
                                    var flag6        = vector == Sickness.InfectionVector.Digestion;
                                    var num2         = 1;
                                    if (flag5) flag4 = AssessInhaledGerms(exposureType);
                                    if (flag6) num2  = AssessDigestedGerms(exposureType, count);
                                    if (flag4) {
                                        if (flag5) inhaleExposureTick[exposureType.germ_id].ticks = 0;
                                        SetExposureTier(exposureType.germ_id,
                                                        GetExposureTier(exposureType.germ_id) + num2);

                                        var amount2
                                            = Mathf
                                                .Clamp01(GetContractionChance(GetResistanceToExposureType(exposureType)) -
                                                         contractionChance2);

                                        GermExposureTracker.Instance.AddExposure(exposureType, amount2);
                                    }
                                }
                            }
                        }
                    }
                }

            RefreshStatusItems();
        }

        public ExposureState GetExposureState(string germ_id) {
            ExposureState result;
            exposureStates.TryGetValue(germ_id, out result);
            return result;
        }

        public float GetExposureTier(string germ_id) {
            var value = 1f;
            exposureTiers.TryGetValue(germ_id, out value);
            return Mathf.Clamp(value, 1f, 3f);
        }

        public void SetExposureState(string germ_id, ExposureState exposure_state) {
            exposureStates[germ_id] = exposure_state;
            RefreshStatusItems();
        }

        public void SetExposureTier(string germ_id, float tier) {
            tier                   = Mathf.Clamp(tier, 0f, 3f);
            exposureTiers[germ_id] = tier;
            RefreshStatusItems();
        }

        public void ContractGerms(string germ_id) {
            DebugUtil.DevAssert(GetExposureState(germ_id) == ExposureState.Exposed,
                                "Duplicant is contracting a sickness but was never exposed to it!");

            SetExposureState(germ_id, ExposureState.Contracted);
        }

        public void OnSicknessAdded(object sickness_instance_data) {
            var sicknessInstance = (SicknessInstance)sickness_instance_data;
            foreach (var exposureType in GERM_EXPOSURE.TYPES)
                if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
                    SetExposureState(exposureType.germ_id, ExposureState.Sick);
        }

        public void OnSicknessCured(object sickness_instance_data) {
            var sicknessInstance = (SicknessInstance)sickness_instance_data;
            foreach (var exposureType in GERM_EXPOSURE.TYPES)
                if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
                    SetExposureState(exposureType.germ_id, ExposureState.None);
        }

        private bool IsExposureValidForTraits(ExposureType exposure_type) {
            if (exposure_type.required_traits != null && exposure_type.required_traits.Count > 0)
                foreach (var trait_id in exposure_type.required_traits)
                    if (!traits.HasTrait(trait_id))
                        return false;

            if (exposure_type.excluded_traits != null && exposure_type.excluded_traits.Count > 0)
                foreach (var trait_id2 in exposure_type.excluded_traits)
                    if (traits.HasTrait(trait_id2))
                        return false;

            if (exposure_type.excluded_effects != null && exposure_type.excluded_effects.Count > 0) {
                var component = master.GetComponent<Effects>();
                foreach (var effect_id in exposure_type.excluded_effects)
                    if (component.HasEffect(effect_id))
                        return false;
            }

            return true;
        }

        private bool HasMinExposurePeriodElapsed(string germ_id) {
            float num;
            lastExposureTime.TryGetValue(germ_id, out num);
            return num == 0f || GameClock.Instance.GetTime() - num > 540f;
        }

        private void RefreshStatusItems() {
            foreach (var exposureType in GERM_EXPOSURE.TYPES) {
                Guid guid;
                contactStatusItemHandles.TryGetValue(exposureType.germ_id, out guid);
                Guid guid2;
                statusItemHandles.TryGetValue(exposureType.germ_id, out guid2);
                var exposureState = GetExposureState(exposureType.germ_id);
                if (guid2 == Guid.Empty                                                                   &&
                    (exposureState == ExposureState.Exposed || exposureState == ExposureState.Contracted) &&
                    !string.IsNullOrEmpty(exposureType.sickness_id))
                    guid2 = GetComponent<KSelectable>()
                        .AddStatusItem(Db.Get().DuplicantStatusItems.ExposedToGerms,
                                       new ExposureStatusData { exposure_type = exposureType, owner = this });
                else if (guid2         != Guid.Empty            &&
                         exposureState != ExposureState.Exposed &&
                         exposureState != ExposureState.Contracted)
                    guid2 = GetComponent<KSelectable>().RemoveStatusItem(guid2);

                statusItemHandles[exposureType.germ_id] = guid2;
                if (guid == Guid.Empty && exposureState == ExposureState.Contact) {
                    if (!string.IsNullOrEmpty(exposureType.sickness_id))
                        guid = GetComponent<KSelectable>()
                            .AddStatusItem(Db.Get().DuplicantStatusItems.ContactWithGerms,
                                           new ExposureStatusData { exposure_type = exposureType, owner = this });
                } else if (guid != Guid.Empty && exposureState != ExposureState.Contact)
                    guid = GetComponent<KSelectable>().RemoveStatusItem(guid);

                contactStatusItemHandles[exposureType.germ_id] = guid;
            }
        }

        private void OnNightTime(object data) { UpdateReports(); }

        private void UpdateReports() {
            ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseStatus,
                                               primaryElement.DiseaseCount,
                                               StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.GERMS,
                                                                       "{0}",
                                                                       master.name),
                                               master.gameObject.GetProperName());
        }

        public void InfectImmediately(ExposureType exposure_type) {
            if (exposure_type.infection_effect != null)
                master.GetComponent<Effects>().Add(exposure_type.infection_effect, true);

            if (exposure_type.sickness_id != null) {
                var lastDiseaseSource = GetLastDiseaseSource(exposure_type.germ_id);
                var exposure_info     = new SicknessExposureInfo(exposure_type.sickness_id, lastDiseaseSource);
                sicknesses.Infect(exposure_info);
            }
        }

        public void OnSleepFinished() {
            foreach (var exposureType in GERM_EXPOSURE.TYPES)
                if (!exposureType.infect_immediately && exposureType.sickness_id != null) {
                    var exposureState = GetExposureState(exposureType.germ_id);
                    if (exposureState == ExposureState.Exposed)
                        SetExposureState(exposureType.germ_id, ExposureState.None);

                    if (exposureState == ExposureState.Contracted) {
                        SetExposureState(exposureType.germ_id, ExposureState.Sick);
                        var lastDiseaseSource = GetLastDiseaseSource(exposureType.germ_id);
                        var exposure_info     = new SicknessExposureInfo(exposureType.sickness_id, lastDiseaseSource);
                        sicknesses.Infect(exposure_info);
                    }

                    SetExposureTier(exposureType.germ_id, 0f);
                }
        }

        public string GetLastDiseaseSource(string id) {
            DiseaseSourceInfo diseaseSourceInfo;
            string            result;
            if (lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
                switch (diseaseSourceInfo.vector) {
                    case Sickness.InfectionVector.Contact:
                        result = DUPLICANTS.DISEASES.INFECTIONSOURCES.SKIN;
                        break;
                    case Sickness.InfectionVector.Digestion:
                        result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.FOOD,
                                               diseaseSourceInfo.sourceObject.ProperName());

                        break;
                    case Sickness.InfectionVector.Inhalation:
                        result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.AIR,
                                               diseaseSourceInfo.sourceObject.ProperName());

                        break;
                    default:
                        result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
                        break;
                }
            else
                result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;

            return result;
        }

        public Vector3 GetLastExposurePosition(string germ_id) {
            DiseaseSourceInfo diseaseSourceInfo;
            if (lastDiseaseSources.TryGetValue(germ_id, out diseaseSourceInfo)) return diseaseSourceInfo.position;

            return transform.GetPosition();
        }

        public float GetExposureWeight(string id) {
            var               exposureTier = GetExposureTier(id);
            DiseaseSourceInfo diseaseSourceInfo;
            if (lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
                return diseaseSourceInfo.factor * exposureTier;

            return 0f;
        }

        [Serializable]
        public class DiseaseSourceInfo {
            public float                    factor;
            public Vector3                  position;
            public Tag                      sourceObject;
            public Sickness.InfectionVector vector;

            public DiseaseSourceInfo(Tag                      sourceObject,
                                     Sickness.InfectionVector vector,
                                     float                    factor,
                                     Vector3                  position) {
                this.sourceObject = sourceObject;
                this.vector       = vector;
                this.factor       = factor;
                this.position     = position;
            }
        }

        public class InhaleTickInfo {
            public bool inhaled;
            public int  ticks;
        }
    }
}