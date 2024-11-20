using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using STRINGS;
using UnityEngine;
using Random = UnityEngine.Random;

public class Geyser : StateMachineComponent<Geyser.StatesInstance>, IGameObjectEffectDescriptor {
    public enum ModificationMethod {
        Values,
        Percentages
    }

    public enum Phase {
        Pre,
        On,
        Pst,
        Off,
        Any
    }

    public enum TimeShiftStep {
        ActiveState,
        DormantState,
        NextIteration,
        PreviousIteration
    }

    private const float              PRE_PCT                               = 0.1f;
    private const float              POST_PCT                              = 0.05f;
    public static ModificationMethod massModificationMethod                = ModificationMethod.Percentages;
    public static ModificationMethod temperatureModificationMethod         = ModificationMethod.Values;
    public static ModificationMethod IterationDurationModificationMethod   = ModificationMethod.Percentages;
    public static ModificationMethod IterationPercentageModificationMethod = ModificationMethod.Percentages;
    public static ModificationMethod yearDurationModificationMethod        = ModificationMethod.Percentages;
    public static ModificationMethod yearPercentageModificationMethod      = ModificationMethod.Percentages;
    public static ModificationMethod maxPressureModificationMethod         = ModificationMethod.Percentages;

    [Serialize]
    public GeyserConfigurator.GeyserInstanceConfiguration configuration;

    [MyCmpAdd]
    private ElementEmitter emitter;

    public  List<GeyserModification> modifications = new List<GeyserModification>();
    private GeyserModification       modifier;

    [MyCmpAdd]
    private UserNameable nameable;

    public Vector2I outputOffset;

    [MyCmpGet]
    private Studyable studyable;

    public float timeShift { get; private set; }

    public List<Descriptor> GetDescriptors(GameObject go) {
        var list     = new List<Descriptor>();
        var arg      = ElementLoader.FindElementByHash(configuration.GetElement()).tag.ProperName();
        var items    = Components.GeoTuners.GetItems(gameObject.GetMyWorldId());
        var instance = items.Find(g => g.GetAssignedGeyser()  == this);
        var num      = items.Count(x => x.GetAssignedGeyser() == this);
        var flag     = num > 0;
        var text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION,
                                 ElementLoader.FindElementByHash(configuration.GetElement()).name,
                                 GameUtil.GetFormattedMass(configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond),
                                 GameUtil.GetFormattedTemperature(configuration.GetTemperature()));

        if (flag) {
            Func<float, float> func = delegate(float emissionPerCycleModifier) {
                                          var num8 = 600f / configuration.GetIterationLength();
                                          return emissionPerCycleModifier / num8 / configuration.GetOnDuration();
                                      };

            var amountOfGeotunersAffectingThisGeyser = GetAmountOfGeotunersAffectingThisGeyser();
            var num2 = temperatureModificationMethod == ModificationMethod.Percentages
                           ? instance.currentGeyserModification.temperatureModifier *
                             configuration.geyserType.temperature
                           : instance.currentGeyserModification.temperatureModifier;

            var num3 = func(massModificationMethod == ModificationMethod.Percentages
                                ? instance.currentGeyserModification.massPerCycleModifier * configuration.scaledRate
                                : instance.currentGeyserModification.massPerCycleModifier);

            var num4 = amountOfGeotunersAffectingThisGeyser * num2;
            var num5 = amountOfGeotunersAffectingThisGeyser * num3;
            var arg2 = (num4 > 0f ? "+" : "") +
                       GameUtil.GetFormattedTemperature(num4,
                                                        GameUtil.TimeSlice.None,
                                                        GameUtil.TemperatureInterpretation.Relative);

            var arg3 = (num5 > 0f ? "+" : "") +
                       GameUtil.GetFormattedMass(num5,
                                                 GameUtil.TimeSlice.PerSecond,
                                                 GameUtil.MetricMassFormat.UseThreshold,
                                                 true,
                                                 "{0:0.##}");

            var str = (num2 > 0f ? "+" : "") +
                      GameUtil.GetFormattedTemperature(num2,
                                                       GameUtil.TimeSlice.None,
                                                       GameUtil.TemperatureInterpretation.Relative);

            var str2 = (num3 > 0f ? "+" : "") +
                       GameUtil.GetFormattedMass(num3,
                                                 GameUtil.TimeSlice.PerSecond,
                                                 GameUtil.MetricMassFormat.UseThreshold,
                                                 true,
                                                 "{0:0.##}");

            text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED,
                                 ElementLoader.FindElementByHash(configuration.GetElement()).name,
                                 GameUtil.GetFormattedMass(configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond),
                                 GameUtil.GetFormattedTemperature(configuration.GetTemperature()));

            text += "\n";
            text = text +
                   "\n" +
                   string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_COUNT,
                                 amountOfGeotunersAffectingThisGeyser.ToString(),
                                 num.ToString());

            text += "\n";
            text = text +
                   "\n" +
                   string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_TOTAL, arg3, arg2);

            for (var i = 0; i < amountOfGeotunersAffectingThisGeyser; i++) {
                var text2 = "\n    • " +
                            UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE;

                text2 =  text2 + str2 + " " + str;
                text  += text2;
            }
        }

        list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PRODUCTION,
                                              arg,
                                              GameUtil.GetFormattedMass(configuration.GetEmitRate(),
                                                                        GameUtil.TimeSlice.PerSecond),
                                              GameUtil.GetFormattedTemperature(configuration.GetTemperature())),
                                text));

        if (configuration.GetDiseaseIdx() != 255)
            list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_DISEASE,
                                                  GameUtil.GetFormattedDiseaseName(configuration.GetDiseaseIdx())),
                                    string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_DISEASE,
                                                  GameUtil.GetFormattedDiseaseName(configuration.GetDiseaseIdx()))));

        list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PERIOD,
                                              GameUtil.GetFormattedTime(configuration.GetOnDuration()),
                                              GameUtil.GetFormattedTime(configuration.GetIterationLength())),
                                string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PERIOD,
                                              GameUtil.GetFormattedTime(configuration.GetOnDuration()),
                                              GameUtil.GetFormattedTime(configuration.GetIterationLength()))));

        var component = GetComponent<Studyable>();
        if (component && !component.Studied) {
            list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()),
                                    string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_UNSTUDIED,
                                                  Array.Empty<object>())));

            list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED,
                                                  Array.Empty<object>()),
                                    string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED,
                                                  Array.Empty<object>())));
        } else {
            list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_PERIOD,
                                                  GameUtil.GetFormattedCycles(configuration.GetYearOnDuration()),
                                                  GameUtil.GetFormattedCycles(configuration.GetYearLength())),
                                    string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_PERIOD,
                                                  GameUtil.GetFormattedCycles(configuration.GetYearOnDuration()),
                                                  GameUtil.GetFormattedCycles(configuration.GetYearLength()))));

            if (smi.IsInsideState(smi.sm.dormant))
                list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_ACTIVE,
                                                      GameUtil.GetFormattedCycles(RemainingDormantTime())),
                                        string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_ACTIVE,
                                                      GameUtil.GetFormattedCycles(RemainingDormantTime()))));
            else
                list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_DORMANT,
                                                      GameUtil.GetFormattedCycles(RemainingActiveTime())),
                                        string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_DORMANT,
                                                      GameUtil.GetFormattedCycles(RemainingActiveTime()))));

            var text3 = UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT
                          .Replace("{average}",
                                   GameUtil.GetFormattedMass(configuration.GetAverageEmission(),
                                                             GameUtil.TimeSlice.PerSecond))
                          .Replace("{element}", configuration.geyserType.element.CreateTag().ProperName());

            if (flag) {
                text3 += "\n";
                text3 =  text3 + "\n" + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE;
                var amountOfGeotunersAffectingThisGeyser2 = GetAmountOfGeotunersAffectingThisGeyser();
                var num6 = massModificationMethod == ModificationMethod.Percentages
                               ? instance.currentGeyserModification.massPerCycleModifier * 100f
                               : instance.currentGeyserModification.massPerCycleModifier *
                                 100f /
                                 configuration.scaledRate;

                var num7 = num6 * amountOfGeotunersAffectingThisGeyser2;
                text3 = text3 + GameUtil.AddPositiveSign(num7.ToString("0.0"), num7 > 0f) + "%";
                for (var j = 0; j < amountOfGeotunersAffectingThisGeyser2; j++) {
                    var text4 = "\n    • " + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW;
                    text4 =  text4 + GameUtil.AddPositiveSign(num6.ToString("0.0"), num6 > 0f) + "%";
                    text3 += text4;
                }
            }

            list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT,
                                                  GameUtil.GetFormattedMass(configuration.GetAverageEmission(),
                                                                            GameUtil.TimeSlice.PerSecond)),
                                    text3));
        }

        return list;
    }

    public float GetCurrentLifeTime() { return GameClock.Instance.GetTime() + timeShift; }

    public void AlterTime(float timeOffset) {
        timeShift = Mathf.Max(timeOffset, -GameClock.Instance.GetTime());
        var num  = RemainingEruptTime();
        var num2 = RemainingNonEruptTime();
        var num3 = RemainingActiveTime();
        var num4 = RemainingDormantTime();
        configuration.GetYearLength();
        if (num2 == 0f) {
            if ((num4 == 0f && configuration.GetYearOnDuration()  - num3 < configuration.GetOnDuration()  - num) |
                (num3 == 0f && configuration.GetYearOffDuration() - num4 >= configuration.GetOnDuration() - num)) {
                smi.GoTo(smi.sm.dormant);
                return;
            }

            smi.GoTo(smi.sm.erupt);
            return;
        }

        var flag
            = (num4 == 0f && configuration.GetYearOnDuration()  - num3 < configuration.GetIterationLength()  - num2) |
              (num3 == 0f && configuration.GetYearOffDuration() - num4 >= configuration.GetIterationLength() - num2);

        var num5 = RemainingEruptPreTime();
        if (flag && num5 <= 0f) {
            smi.GoTo(smi.sm.dormant);
            return;
        }

        if (num5 <= 0f) {
            smi.GoTo(smi.sm.idle);
            return;
        }

        var num6 = PreDuration() - num5;
        if (num3                                            == 0f
                ? configuration.GetYearOffDuration() - num4 > num6
                : num6                                      > configuration.GetYearOnDuration() - num3) {
            smi.GoTo(smi.sm.dormant);
            return;
        }

        smi.GoTo(smi.sm.pre_erupt);
    }

    public void ShiftTimeTo(TimeShiftStep step) {
        var num        = RemainingEruptTime();
        var num2       = RemainingNonEruptTime();
        var num3       = RemainingActiveTime();
        var num4       = RemainingDormantTime();
        var yearLength = configuration.GetYearLength();
        switch (step) {
            case TimeShiftStep.ActiveState: {
                var num5 = num3 > 0f ? configuration.GetYearOnDuration() - num3 : yearLength - num4;
                AlterTime(timeShift - num5);
                return;
            }
            case TimeShiftStep.DormantState: {
                var num6 = num3 > 0f ? num3 : -(configuration.GetYearOffDuration() - num4);
                AlterTime(timeShift + num6);
                return;
            }
            case TimeShiftStep.NextIteration: {
                var num7 = num > 0f ? num + configuration.GetOffDuration() : num2;
                AlterTime(timeShift + num7);
                return;
            }
            case TimeShiftStep.PreviousIteration: {
                var num8 = num > 0f
                               ? -(configuration.GetOnDuration()      - num)
                               : -(configuration.GetIterationLength() - num2);

                if (num > 0f && Mathf.Abs(num8) < configuration.GetOnDuration() * 0.05f)
                    num8 -= configuration.GetIterationLength();

                AlterTime(timeShift + num8);
                return;
            }
            default:
                return;
        }
    }

    public void AddModification(GeyserModification modification) {
        modifications.Add(modification);
        UpdateModifier();
    }

    public void RemoveModification(GeyserModification modification) {
        modifications.Remove(modification);
        UpdateModifier();
    }

    private void UpdateModifier() {
        modifier.Clear();
        foreach (var modification in modifications) modifier.AddValues(modification);
        configuration.SetModifier(modifier);
        ApplyConfigurationEmissionValues(configuration);
        RefreshGeotunerFeedback();
    }

    public void RefreshGeotunerFeedback() {
        RefreshGeotunerStatusItem();
        RefreshStudiedMeter();
    }

    private void RefreshGeotunerStatusItem() {
        var component = gameObject.GetComponent<KSelectable>();
        if (GetAmountOfGeotunersPointingThisGeyser() > 0) {
            component.AddStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
            return;
        }

        component.RemoveStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
    }

    private void RefreshStudiedMeter() {
        if (studyable.Studied) {
            var flag                  = GetAmountOfGeotunersPointingThisGeyser() > 0;
            var trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.tracker;
            if (flag) {
                trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker;
                var amountOfGeotunersAffectingThisGeyser = GetAmountOfGeotunersAffectingThisGeyser();
                if (amountOfGeotunersAffectingThisGeyser > 0)
                    trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_minor;

                if (amountOfGeotunersAffectingThisGeyser >= 5)
                    trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_major;
            }

            studyable.studiedIndicator.meterController.Play(trackerMeterAnimNames.ToString(), KAnim.PlayMode.Loop);
        }
    }

    public int GetAmountOfGeotunersPointingThisGeyser() {
        return Components.GeoTuners.GetItems(gameObject.GetMyWorldId()).Count(x => x.GetAssignedGeyser() == this);
    }

    public int GetAmountOfGeotunersPointingOrWillPointAtThisGeyser() {
        return Components.GeoTuners.GetItems(gameObject.GetMyWorldId())
                         .Count(x => x.GetAssignedGeyser() == this || x.GetFutureGeyser() == this);
    }

    public int GetAmountOfGeotunersAffectingThisGeyser() {
        var num = 0;
        for (var i = 0; i < modifications.Count; i++)
            if (modifications[i].originID.Contains("GeoTuner"))
                num++;

        return num;
    }

    private void OnGeotunerChanged(object o) { RefreshGeotunerFeedback(); }

    protected override void OnSpawn() {
        base.OnSpawn();
        Prioritizable.AddRef(gameObject);
        if (configuration == null || configuration.typeId == HashedString.Invalid)
            configuration = GetComponent<GeyserConfigurator>().MakeConfiguration();
        else {
            var component = gameObject.GetComponent<PrimaryElement>();
            if (configuration.geyserType.geyserTemperature - component.Temperature != 0f) {
                var component2 = gameObject.GetComponent<SimTemperatureTransfer>();
                component2.onSimRegistered
                    = (Action<SimTemperatureTransfer>)Delegate.Combine(component2.onSimRegistered,
                                                                       new Action<
                                                                           SimTemperatureTransfer>(OnSimRegistered));
            }
        }

        ApplyConfigurationEmissionValues(configuration);
        GenerateName();
        smi.StartSM();
        Workable component3                                      = GetComponent<Studyable>();
        if (component3 != null) component3.alwaysShowProgressBar = true;
        Components.Geysers.Add(gameObject.GetMyWorldId(), this);
        gameObject.Subscribe(1763323737, OnGeotunerChanged);
        RefreshStudiedMeter();
        UpdateModifier();
    }

    private void GenerateName() {
        var key = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + configuration.geyserType.id.ToUpper() + ".NAME");
        if (nameable.savedName == Strings.Get(key)) {
            var cell           = Grid.PosToCell(gameObject);
            var quadrantOfCell = gameObject.GetMyWorld().GetQuadrantOfCell(cell, 2);
            var num            = (int)quadrantOfCell[0];
            var str            = num.ToString();
            num = (int)quadrantOfCell[1];
            var text  = str + num;
            var array = NAMEGEN.GEYSER_IDS.IDs.ToString().Split('\n');
            var text2 = array[Random.Range(0, array.Length)];
            var name = string.Concat(UI.StripLinkFormatting(gameObject.GetProperName()),
                                     " ",
                                     text2,
                                     text,
                                     "‑",
                                     Random.Range(0, 10).ToString());

            nameable.SetName(name);
        }
    }

    public void ApplyConfigurationEmissionValues(GeyserConfigurator.GeyserInstanceConfiguration config) {
        emitter.emitRange   = 2;
        emitter.maxPressure = config.GetMaxPressure();
        emitter.outputElement = new ElementConverter.OutputElement(config.GetEmitRate(),
                                                                   config.GetElement(),
                                                                   config.GetTemperature(),
                                                                   false,
                                                                   false,
                                                                   outputOffset.x,
                                                                   outputOffset.y,
                                                                   1f,
                                                                   config.GetDiseaseIdx(),
                                                                   Mathf.RoundToInt(config.GetDiseaseCount() *
                                                                       config.GetEmitRate()));

        if (emitter.IsSimActive) emitter.SetSimActive(true);
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        gameObject.Unsubscribe(1763323737, OnGeotunerChanged);
        Components.Geysers.Remove(gameObject.GetMyWorldId(), this);
    }

    private void OnSimRegistered(SimTemperatureTransfer stt) {
        var component = gameObject.GetComponent<PrimaryElement>();
        if (configuration.geyserType.geyserTemperature - component.Temperature != 0f)
            component.Temperature = configuration.geyserType.geyserTemperature;

        stt.onSimRegistered
            = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered,
                                                              new Action<SimTemperatureTransfer>(OnSimRegistered));
    }

    public float RemainingPhaseTimeFrom2(float onDuration, float offDuration, float time, Phase expectedPhase) {
        var   num  = onDuration + offDuration;
        var   num2 = time % num;
        float result;
        Phase phase;
        if (num2 < onDuration) {
            result = Mathf.Max(onDuration - num2, 0f);
            phase  = Phase.On;
        } else {
            result = Mathf.Max(onDuration + offDuration - num2, 0f);
            phase  = Phase.Off;
        }

        if (expectedPhase != Phase.Any && phase != expectedPhase) return 0f;

        return result;
    }

    public float RemainingPhaseTimeFrom4(float onDuration,
                                         float pstDuration,
                                         float offDuration,
                                         float preDuration,
                                         float time,
                                         Phase expectedPhase) {
        var   num  = onDuration + pstDuration + offDuration + preDuration;
        var   num2 = time % num;
        float result;
        Phase phase;
        if (num2 < onDuration) {
            result = onDuration - num2;
            phase  = Phase.On;
        } else if (num2 < onDuration + pstDuration) {
            result = onDuration + pstDuration - num2;
            phase  = Phase.Pst;
        } else if (num2 < onDuration + pstDuration + offDuration) {
            result = onDuration + pstDuration + offDuration - num2;
            phase  = Phase.Off;
        } else {
            result = onDuration + pstDuration + offDuration + preDuration - num2;
            phase  = Phase.Pre;
        }

        if (expectedPhase != Phase.Any && phase != expectedPhase) return 0f;

        return result;
    }

    private float IdleDuration()    { return configuration.GetOffDuration() * 0.84999996f; }
    private float PreDuration()     { return configuration.GetOffDuration() * 0.1f; }
    private float PostDuration()    { return configuration.GetOffDuration() * 0.05f; }
    private float EruptDuration()   { return configuration.GetOnDuration(); }
    public  bool  ShouldGoDormant() { return RemainingActiveTime() <= 0f; }

    public float RemainingIdleTime() {
        return RemainingPhaseTimeFrom4(EruptDuration(),
                                       PostDuration(),
                                       IdleDuration(),
                                       PreDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.Off);
    }

    public float RemainingEruptPreTime() {
        return RemainingPhaseTimeFrom4(EruptDuration(),
                                       PostDuration(),
                                       IdleDuration(),
                                       PreDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.Pre);
    }

    public float RemainingEruptTime() {
        return RemainingPhaseTimeFrom2(configuration.GetOnDuration(),
                                       configuration.GetOffDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.On);
    }

    public float RemainingEruptPostTime() {
        return RemainingPhaseTimeFrom4(EruptDuration(),
                                       PostDuration(),
                                       IdleDuration(),
                                       PreDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.Pst);
    }

    public float RemainingNonEruptTime() {
        return RemainingPhaseTimeFrom2(configuration.GetOnDuration(),
                                       configuration.GetOffDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.Off);
    }

    public float RemainingDormantTime() {
        return RemainingPhaseTimeFrom2(configuration.GetYearOnDuration(),
                                       configuration.GetYearOffDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.Off);
    }

    public float RemainingActiveTime() {
        return RemainingPhaseTimeFrom2(configuration.GetYearOnDuration(),
                                       configuration.GetYearOffDuration(),
                                       GetCurrentLifeTime(),
                                       Phase.On);
    }

    public struct GeyserModification {
        public void Clear() {
            massPerCycleModifier        = 0f;
            temperatureModifier         = 0f;
            iterationDurationModifier   = 0f;
            iterationPercentageModifier = 0f;
            yearDurationModifier        = 0f;
            yearPercentageModifier      = 0f;
            maxPressureModifier         = 0f;
            modifyElement               = false;
            newElement                  = 0;
        }

        public void AddValues(GeyserModification modification) {
            massPerCycleModifier        += modification.massPerCycleModifier;
            temperatureModifier         += modification.temperatureModifier;
            iterationDurationModifier   += modification.iterationDurationModifier;
            iterationPercentageModifier += modification.iterationPercentageModifier;
            yearDurationModifier        += modification.yearDurationModifier;
            yearPercentageModifier      += modification.yearPercentageModifier;
            maxPressureModifier         += modification.maxPressureModifier;
            modifyElement               |= modification.modifyElement;
            newElement                  =  modification.newElement == 0 ? newElement : modification.newElement;
        }

        public bool      IsNewElementInUse() { return modifyElement && newElement > 0; }
        public string    originID;
        public float     massPerCycleModifier;
        public float     temperatureModifier;
        public float     iterationDurationModifier;
        public float     iterationPercentageModifier;
        public float     yearDurationModifier;
        public float     yearPercentageModifier;
        public float     maxPressureModifier;
        public bool      modifyElement;
        public SimHashes newElement;
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, Geyser, object>.GameInstance {
        public StatesInstance(Geyser smi) : base(smi) { }
    }

    public class States : GameStateMachine<States, StatesInstance, Geyser> {
        public State      dormant;
        public EruptState erupt;
        public State      idle;
        public State      post_erupt;
        public State      pre_erupt;

        public override void InitializeStates(out BaseState default_state) {
            default_state = idle;
            serializable  = SerializeType.Both_DEPRECATED;
            root.DefaultState(idle).Enter(delegate(StatesInstance smi) { smi.master.emitter.SetEmitting(false); });
            dormant.PlayAnim("inactive", KAnim.PlayMode.Loop)
                   .ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutDormant)
                   .ScheduleGoTo(smi => smi.master.RemainingDormantTime(), pre_erupt);

            idle.PlayAnim("inactive", KAnim.PlayMode.Loop)
                .ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle)
                .Enter(delegate(StatesInstance smi) {
                           if (smi.master.ShouldGoDormant()) smi.GoTo(dormant);
                       })
                .ScheduleGoTo(smi => smi.master.RemainingIdleTime(), pre_erupt);

            pre_erupt.PlayAnim("shake", KAnim.PlayMode.Loop)
                     .ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding)
                     .ScheduleGoTo(smi => smi.master.RemainingEruptPreTime(), erupt);

            erupt.TriggerOnEnter(GameHashes.GeyserEruption, smi => true)
                 .TriggerOnExit(GameHashes.GeyserEruption, smi => false)
                 .DefaultState(erupt.erupting)
                 .ScheduleGoTo(smi => smi.master.RemainingEruptTime(), post_erupt)
                 .Enter(delegate(StatesInstance smi) { smi.master.emitter.SetEmitting(true); })
                 .Exit(delegate(StatesInstance  smi) { smi.master.emitter.SetEmitting(false); });

            erupt.erupting
                 .EventTransition(GameHashes.EmitterBlocked,
                                  erupt.overpressure,
                                  smi => smi.GetComponent<ElementEmitter>().isEmitterBlocked)
                 .PlayAnim("erupt", KAnim.PlayMode.Loop);

            erupt.overpressure
                 .EventTransition(GameHashes.EmitterUnblocked,
                                  erupt.erupting,
                                  smi => !smi.GetComponent<ElementEmitter>().isEmitterBlocked)
                 .ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure)
                 .PlayAnim("inactive", KAnim.PlayMode.Loop);

            post_erupt.PlayAnim("shake", KAnim.PlayMode.Loop)
                      .ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle)
                      .ScheduleGoTo(smi => smi.master.RemainingEruptPostTime(), idle);
        }

        public class EruptState : State {
            public State erupting;
            public State overpressure;
        }
    }
}