using System;
using KSerialization;
using STRINGS;
using UnityEngine;

public class HEPBattery : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def> {
    public static readonly HashedString FIRE_PORT_ID = "HEPBatteryFire";
    public                 State        inoperational;
    public                 State        operational;

    public override void InitializeStates(out BaseState default_state) {
        default_state = inoperational;
        inoperational.PlayAnim("off")
                     .TagTransition(GameTags.Operational, operational)
                     .Update(delegate(Instance smi, float dt) {
                                 smi.DoConsumeParticlesWhileDisabled(dt);
                                 smi.UpdateDecayStatusItem(false);
                             });

        operational.Enter("SetActive(true)", delegate(Instance smi) { smi.operational.SetActive(true); })
                   .Exit("SetActive(false)", delegate(Instance smi) { smi.operational.SetActive(false); })
                   .PlayAnim("on", KAnim.PlayMode.Loop)
                   .TagTransition(GameTags.Operational, inoperational, true)
                   .Update(LauncherUpdate);
    }

    public void LauncherUpdate(Instance smi, float dt) {
        smi.UpdateDecayStatusItem(true);
        smi.UpdateMeter();
        smi.operational.SetActive(smi.particleStorage.Particles > 0f);
        smi.launcherTimer += dt;
        if (smi.launcherTimer < smi.def.minLaunchInterval || !smi.AllowSpawnParticles) return;

        if (smi.particleStorage.Particles >= smi.particleThreshold) {
            smi.launcherTimer = 0f;
            Fire(smi);
        }
    }

    public void Fire(Instance smi) {
        var highEnergyParticleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
        var gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"),
                                               Grid.CellToPosCCC(highEnergyParticleOutputCell,
                                                                 Grid.SceneLayer.FXFront2),
                                               Grid.SceneLayer.FXFront2);

        gameObject.SetActive(true);
        if (gameObject != null) {
            var component = gameObject.GetComponent<HighEnergyParticle>();
            component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
            component.SetDirection(smi.def.direction);
        }
    }

    public class Def : BaseDef {
        public EightDirection direction;
        public float          maxSlider;
        public float          minLaunchInterval;
        public float          minSlider;
        public float          particleDecayRate;
    }

    public new class Instance : GameInstance, ISingleSliderControl, ISliderControl {
        [MyCmpAdd]
        public CopyBuildingSettings copyBuildingSettings;

        [Serialize]
        public float launcherTimer;

        private          bool            m_skipFirstUpdate = true;
        private readonly MeterController meterController;

        [MyCmpGet]
        public Operational operational;

        [MyCmpReq]
        public HighEnergyParticleStorage particleStorage;

        [Serialize]
        public float particleThreshold = 50f;

        public  bool ShowWorkingStatus;
        private Guid statusHandle = Guid.Empty;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            Subscribe(-801688580, OnLogicValueChanged);
            Subscribe(-905833192, OnCopySettings);
            meterController = new MeterController(GetComponent<KBatchedAnimController>(),
                                                  "meter_target",
                                                  "meter",
                                                  Meter.Offset.Infront,
                                                  Grid.SceneLayer.NoLayer,
                                                  Array.Empty<string>());

            UpdateMeter();
        }

        public bool   AllowSpawnParticles => HasLogicWire && IsLogicActive;
        public bool   HasLogicWire { get; private set; }
        public bool   IsLogicActive { get; private set; }
        public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
        public string SliderUnits => UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
        public int    SliderDecimalPlaces(int index) { return 0; }
        public float  GetSliderMin(int        index) { return def.minSlider; }
        public float  GetSliderMax(int        index) { return def.maxSlider; }
        public float  GetSliderValue(int      index) { return particleThreshold; }
        public void   SetSliderValue(float    value, int index) { particleThreshold = value; }

        public string GetSliderTooltipKey(int index) {
            return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
        }

        string ISliderControl.GetSliderTooltip(int index) {
            return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"),
                                 particleThreshold);
        }

        public void DoConsumeParticlesWhileDisabled(float dt) {
            if (m_skipFirstUpdate) {
                m_skipFirstUpdate = false;
                return;
            }

            particleStorage.ConsumeAndGet(dt * def.particleDecayRate);
            UpdateMeter();
        }

        public void UpdateMeter(object data = null) {
            meterController.SetPositionPercent(particleStorage.Particles / particleStorage.Capacity());
        }

        public void UpdateDecayStatusItem(bool hasPower) {
            if (!hasPower) {
                if (particleStorage.Particles > 0f) {
                    if (statusHandle == Guid.Empty)
                        statusHandle = GetComponent<KSelectable>()
                            .AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts);
                } else if (statusHandle != Guid.Empty) {
                    GetComponent<KSelectable>().RemoveStatusItem(statusHandle);
                    statusHandle = Guid.Empty;
                }
            } else if (statusHandle != Guid.Empty) {
                GetComponent<KSelectable>().RemoveStatusItem(statusHandle);
                statusHandle = Guid.Empty;
            }
        }

        private LogicCircuitNetwork GetNetwork() {
            var portCell = GetComponent<LogicPorts>().GetPortCell(FIRE_PORT_ID);
            return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
        }

        private void OnLogicValueChanged(object data) {
            var logicValueChanged = (LogicValueChanged)data;
            if (logicValueChanged.portID == FIRE_PORT_ID) {
                IsLogicActive = logicValueChanged.newValue > 0;
                HasLogicWire  = GetNetwork()               != null;
            }
        }

        private void OnCopySettings(object data) {
            var gameObject = data as GameObject;
            if (gameObject != null) {
                var smi                            = gameObject.GetSMI<Instance>();
                if (smi != null) particleThreshold = smi.particleThreshold;
            }
        }
    }
}