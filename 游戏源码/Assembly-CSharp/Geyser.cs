using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using STRINGS;
using UnityEngine;

public class Geyser : StateMachineComponent<Geyser.StatesInstance>, IGameObjectEffectDescriptor
{
	public enum ModificationMethod
	{
		Values,
		Percentages
	}

	public struct GeyserModification
	{
		public string originID;

		public float massPerCycleModifier;

		public float temperatureModifier;

		public float iterationDurationModifier;

		public float iterationPercentageModifier;

		public float yearDurationModifier;

		public float yearPercentageModifier;

		public float maxPressureModifier;

		public bool modifyElement;

		public SimHashes newElement;

		public void Clear()
		{
			massPerCycleModifier = 0f;
			temperatureModifier = 0f;
			iterationDurationModifier = 0f;
			iterationPercentageModifier = 0f;
			yearDurationModifier = 0f;
			yearPercentageModifier = 0f;
			maxPressureModifier = 0f;
			modifyElement = false;
			newElement = (SimHashes)0;
		}

		public void AddValues(GeyserModification modification)
		{
			massPerCycleModifier += modification.massPerCycleModifier;
			temperatureModifier += modification.temperatureModifier;
			iterationDurationModifier += modification.iterationDurationModifier;
			iterationPercentageModifier += modification.iterationPercentageModifier;
			yearDurationModifier += modification.yearDurationModifier;
			yearPercentageModifier += modification.yearPercentageModifier;
			maxPressureModifier += modification.maxPressureModifier;
			modifyElement |= modification.modifyElement;
			newElement = ((modification.newElement == (SimHashes)0) ? newElement : modification.newElement);
		}

		public bool IsNewElementInUse()
		{
			if (modifyElement)
			{
				return newElement != (SimHashes)0;
			}
			return false;
		}
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, Geyser, object>.GameInstance
	{
		public StatesInstance(Geyser smi)
			: base(smi)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, Geyser>
	{
		public class EruptState : State
		{
			public State erupting;

			public State overpressure;
		}

		public State dormant;

		public State idle;

		public State pre_erupt;

		public EruptState erupt;

		public State post_erupt;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;
			base.serializable = SerializeType.Both_DEPRECATED;
			root.DefaultState(idle).Enter(delegate(StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(emitting: false);
			});
			dormant.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutDormant).ScheduleGoTo((StatesInstance smi) => smi.master.RemainingDormantTime(), pre_erupt);
			idle.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle).Enter(delegate(StatesInstance smi)
			{
				if (smi.master.ShouldGoDormant())
				{
					smi.GoTo(dormant);
				}
			})
				.ScheduleGoTo((StatesInstance smi) => smi.master.RemainingIdleTime(), pre_erupt);
			pre_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding).ScheduleGoTo((StatesInstance smi) => smi.master.RemainingEruptPreTime(), erupt);
			erupt.TriggerOnEnter(GameHashes.GeyserEruption, (StatesInstance smi) => true).TriggerOnExit(GameHashes.GeyserEruption, (StatesInstance smi) => false).DefaultState(erupt.erupting)
				.ScheduleGoTo((StatesInstance smi) => smi.master.RemainingEruptTime(), post_erupt)
				.Enter(delegate(StatesInstance smi)
				{
					smi.master.emitter.SetEmitting(emitting: true);
				})
				.Exit(delegate(StatesInstance smi)
				{
					smi.master.emitter.SetEmitting(emitting: false);
				});
			erupt.erupting.EventTransition(GameHashes.EmitterBlocked, erupt.overpressure, (StatesInstance smi) => smi.GetComponent<ElementEmitter>().isEmitterBlocked).PlayAnim("erupt", KAnim.PlayMode.Loop);
			erupt.overpressure.EventTransition(GameHashes.EmitterUnblocked, erupt.erupting, (StatesInstance smi) => !smi.GetComponent<ElementEmitter>().isEmitterBlocked).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure).PlayAnim("inactive", KAnim.PlayMode.Loop);
			post_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle).ScheduleGoTo((StatesInstance smi) => smi.master.RemainingEruptPostTime(), idle);
		}
	}

	public enum TimeShiftStep
	{
		ActiveState,
		DormantState,
		NextIteration,
		PreviousIteration
	}

	public enum Phase
	{
		Pre,
		On,
		Pst,
		Off,
		Any
	}

	public static ModificationMethod massModificationMethod = ModificationMethod.Percentages;

	public static ModificationMethod temperatureModificationMethod = ModificationMethod.Values;

	public static ModificationMethod IterationDurationModificationMethod = ModificationMethod.Percentages;

	public static ModificationMethod IterationPercentageModificationMethod = ModificationMethod.Percentages;

	public static ModificationMethod yearDurationModificationMethod = ModificationMethod.Percentages;

	public static ModificationMethod yearPercentageModificationMethod = ModificationMethod.Percentages;

	public static ModificationMethod maxPressureModificationMethod = ModificationMethod.Percentages;

	[MyCmpAdd]
	private ElementEmitter emitter;

	[MyCmpAdd]
	private UserNameable nameable;

	[MyCmpGet]
	private Studyable studyable;

	[Serialize]
	public GeyserConfigurator.GeyserInstanceConfiguration configuration;

	public Vector2I outputOffset;

	public List<GeyserModification> modifications = new List<GeyserModification>();

	private GeyserModification modifier;

	private const float PRE_PCT = 0.1f;

	private const float POST_PCT = 0.05f;

	public float timeShift { get; private set; }

	public float GetCurrentLifeTime()
	{
		return GameClock.Instance.GetTime() + timeShift;
	}

	public void AlterTime(float timeOffset)
	{
		timeShift = Mathf.Max(timeOffset, 0f - GameClock.Instance.GetTime());
		float num = RemainingEruptTime();
		float num2 = RemainingNonEruptTime();
		float num3 = RemainingActiveTime();
		float num4 = RemainingDormantTime();
		configuration.GetYearLength();
		if (num2 == 0f)
		{
			if ((num4 == 0f && configuration.GetYearOnDuration() - num3 < configuration.GetOnDuration() - num) | (num3 == 0f && configuration.GetYearOffDuration() - num4 >= configuration.GetOnDuration() - num))
			{
				base.smi.GoTo(base.smi.sm.dormant);
			}
			else
			{
				base.smi.GoTo(base.smi.sm.erupt);
			}
			return;
		}
		bool num5 = (num4 == 0f && configuration.GetYearOnDuration() - num3 < configuration.GetIterationLength() - num2) | (num3 == 0f && configuration.GetYearOffDuration() - num4 >= configuration.GetIterationLength() - num2);
		float num6 = RemainingEruptPreTime();
		if (num5 && num6 <= 0f)
		{
			base.smi.GoTo(base.smi.sm.dormant);
			return;
		}
		if (num6 <= 0f)
		{
			base.smi.GoTo(base.smi.sm.idle);
			return;
		}
		float num7 = PreDuration() - num6;
		if ((num3 == 0f) ? (configuration.GetYearOffDuration() - num4 > num7) : (num7 > configuration.GetYearOnDuration() - num3))
		{
			base.smi.GoTo(base.smi.sm.dormant);
		}
		else
		{
			base.smi.GoTo(base.smi.sm.pre_erupt);
		}
	}

	public void ShiftTimeTo(TimeShiftStep step)
	{
		float num = RemainingEruptTime();
		float num2 = RemainingNonEruptTime();
		float num3 = RemainingActiveTime();
		float num4 = RemainingDormantTime();
		float yearLength = configuration.GetYearLength();
		switch (step)
		{
		case TimeShiftStep.ActiveState:
		{
			float num6 = ((num3 > 0f) ? (configuration.GetYearOnDuration() - num3) : (yearLength - num4));
			AlterTime(timeShift - num6);
			break;
		}
		case TimeShiftStep.DormantState:
		{
			float num8 = ((num3 > 0f) ? num3 : (0f - (configuration.GetYearOffDuration() - num4)));
			AlterTime(timeShift + num8);
			break;
		}
		case TimeShiftStep.NextIteration:
		{
			float num7 = ((num > 0f) ? (num + configuration.GetOffDuration()) : num2);
			AlterTime(timeShift + num7);
			break;
		}
		case TimeShiftStep.PreviousIteration:
		{
			float num5 = ((num > 0f) ? (0f - (configuration.GetOnDuration() - num)) : (0f - (configuration.GetIterationLength() - num2)));
			if (num > 0f && Mathf.Abs(num5) < configuration.GetOnDuration() * 0.05f)
			{
				num5 -= configuration.GetIterationLength();
			}
			AlterTime(timeShift + num5);
			break;
		}
		}
	}

	public void AddModification(GeyserModification modification)
	{
		modifications.Add(modification);
		UpdateModifier();
	}

	public void RemoveModification(GeyserModification modification)
	{
		modifications.Remove(modification);
		UpdateModifier();
	}

	private void UpdateModifier()
	{
		modifier.Clear();
		foreach (GeyserModification modification in modifications)
		{
			modifier.AddValues(modification);
		}
		configuration.SetModifier(modifier);
		ApplyConfigurationEmissionValues(configuration);
		RefreshGeotunerFeedback();
	}

	public void RefreshGeotunerFeedback()
	{
		RefreshGeotunerStatusItem();
		RefreshStudiedMeter();
	}

	private void RefreshGeotunerStatusItem()
	{
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (GetAmountOfGeotunersPointingThisGeyser() > 0)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
		}
		else
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
		}
	}

	private void RefreshStudiedMeter()
	{
		if (!studyable.Studied)
		{
			return;
		}
		bool num = GetAmountOfGeotunersPointingThisGeyser() > 0;
		GeyserConfig.TrackerMeterAnimNames trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.tracker;
		if (num)
		{
			trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker;
			int amountOfGeotunersAffectingThisGeyser = GetAmountOfGeotunersAffectingThisGeyser();
			if (amountOfGeotunersAffectingThisGeyser > 0)
			{
				trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_minor;
			}
			if (amountOfGeotunersAffectingThisGeyser >= 5)
			{
				trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_major;
			}
		}
		studyable.studiedIndicator.meterController.Play(trackerMeterAnimNames.ToString(), KAnim.PlayMode.Loop);
	}

	public int GetAmountOfGeotunersPointingThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
	}

	public int GetAmountOfGeotunersPointingOrWillPointAtThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this || x.GetFutureGeyser() == this);
	}

	public int GetAmountOfGeotunersAffectingThisGeyser()
	{
		int num = 0;
		for (int i = 0; i < modifications.Count; i++)
		{
			if (modifications[i].originID.Contains("GeoTuner"))
			{
				num++;
			}
		}
		return num;
	}

	private void OnGeotunerChanged(object o)
	{
		RefreshGeotunerFeedback();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		if (configuration == null || configuration.typeId == HashedString.Invalid)
		{
			configuration = GetComponent<GeyserConfigurator>().MakeConfiguration();
		}
		else
		{
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			if (configuration.geyserType.geyserTemperature - component.Temperature != 0f)
			{
				SimTemperatureTransfer component2 = base.gameObject.GetComponent<SimTemperatureTransfer>();
				component2.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(component2.onSimRegistered, new Action<SimTemperatureTransfer>(OnSimRegistered));
			}
		}
		ApplyConfigurationEmissionValues(configuration);
		GenerateName();
		base.smi.StartSM();
		Workable component3 = GetComponent<Studyable>();
		if (component3 != null)
		{
			component3.alwaysShowProgressBar = true;
		}
		Components.Geysers.Add(base.gameObject.GetMyWorldId(), this);
		base.gameObject.Subscribe(1763323737, OnGeotunerChanged);
		RefreshStudiedMeter();
		UpdateModifier();
	}

	private void GenerateName()
	{
		StringKey key = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + configuration.geyserType.id.ToUpper() + ".NAME");
		if (nameable.savedName == Strings.Get(key))
		{
			int cell = Grid.PosToCell(base.gameObject);
			Quadrant[] quadrantOfCell = base.gameObject.GetMyWorld().GetQuadrantOfCell(cell, 2);
			int num = (int)quadrantOfCell[0];
			string text = num.ToString();
			num = (int)quadrantOfCell[1];
			string text2 = text + num;
			string[] array = NAMEGEN.GEYSER_IDS.IDs.ToString().Split('\n');
			string text3 = array[UnityEngine.Random.Range(0, array.Length)];
			string text4 = UI.StripLinkFormatting(base.gameObject.GetProperName()) + " " + text3 + text2 + "‑" + UnityEngine.Random.Range(0, 10);
			nameable.SetName(text4);
		}
	}

	public void ApplyConfigurationEmissionValues(GeyserConfigurator.GeyserInstanceConfiguration config)
	{
		emitter.emitRange = 2;
		emitter.maxPressure = config.GetMaxPressure();
		emitter.outputElement = new ElementConverter.OutputElement(config.GetEmitRate(), config.GetElement(), config.GetTemperature(), useEntityTemperature: false, storeOutput: false, outputOffset.x, outputOffset.y, 1f, config.GetDiseaseIdx(), Mathf.RoundToInt((float)config.GetDiseaseCount() * config.GetEmitRate()));
		if (emitter.IsSimActive)
		{
			emitter.SetSimActive(active: true);
		}
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.gameObject.Unsubscribe(1763323737, OnGeotunerChanged);
		Components.Geysers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	private void OnSimRegistered(SimTemperatureTransfer stt)
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		if (configuration.geyserType.geyserTemperature - component.Temperature != 0f)
		{
			component.Temperature = configuration.geyserType.geyserTemperature;
		}
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(OnSimRegistered));
	}

	public float RemainingPhaseTimeFrom2(float onDuration, float offDuration, float time, Phase expectedPhase)
	{
		float num = onDuration + offDuration;
		float num2 = time % num;
		float result;
		Phase phase;
		if (num2 < onDuration)
		{
			result = Mathf.Max(onDuration - num2, 0f);
			phase = Phase.On;
		}
		else
		{
			result = Mathf.Max(onDuration + offDuration - num2, 0f);
			phase = Phase.Off;
		}
		if (expectedPhase != Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	public float RemainingPhaseTimeFrom4(float onDuration, float pstDuration, float offDuration, float preDuration, float time, Phase expectedPhase)
	{
		float num = onDuration + pstDuration + offDuration + preDuration;
		float num2 = time % num;
		float result;
		Phase phase;
		if (num2 < onDuration)
		{
			result = onDuration - num2;
			phase = Phase.On;
		}
		else if (num2 < onDuration + pstDuration)
		{
			result = onDuration + pstDuration - num2;
			phase = Phase.Pst;
		}
		else if (num2 < onDuration + pstDuration + offDuration)
		{
			result = onDuration + pstDuration + offDuration - num2;
			phase = Phase.Off;
		}
		else
		{
			result = onDuration + pstDuration + offDuration + preDuration - num2;
			phase = Phase.Pre;
		}
		if (expectedPhase != Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	private float IdleDuration()
	{
		return configuration.GetOffDuration() * 0.84999996f;
	}

	private float PreDuration()
	{
		return configuration.GetOffDuration() * 0.1f;
	}

	private float PostDuration()
	{
		return configuration.GetOffDuration() * 0.05f;
	}

	private float EruptDuration()
	{
		return configuration.GetOnDuration();
	}

	public bool ShouldGoDormant()
	{
		return RemainingActiveTime() <= 0f;
	}

	public float RemainingIdleTime()
	{
		return RemainingPhaseTimeFrom4(EruptDuration(), PostDuration(), IdleDuration(), PreDuration(), GetCurrentLifeTime(), Phase.Off);
	}

	public float RemainingEruptPreTime()
	{
		return RemainingPhaseTimeFrom4(EruptDuration(), PostDuration(), IdleDuration(), PreDuration(), GetCurrentLifeTime(), Phase.Pre);
	}

	public float RemainingEruptTime()
	{
		return RemainingPhaseTimeFrom2(configuration.GetOnDuration(), configuration.GetOffDuration(), GetCurrentLifeTime(), Phase.On);
	}

	public float RemainingEruptPostTime()
	{
		return RemainingPhaseTimeFrom4(EruptDuration(), PostDuration(), IdleDuration(), PreDuration(), GetCurrentLifeTime(), Phase.Pst);
	}

	public float RemainingNonEruptTime()
	{
		return RemainingPhaseTimeFrom2(configuration.GetOnDuration(), configuration.GetOffDuration(), GetCurrentLifeTime(), Phase.Off);
	}

	public float RemainingDormantTime()
	{
		return RemainingPhaseTimeFrom2(configuration.GetYearOnDuration(), configuration.GetYearOffDuration(), GetCurrentLifeTime(), Phase.Off);
	}

	public float RemainingActiveTime()
	{
		return RemainingPhaseTimeFrom2(configuration.GetYearOnDuration(), configuration.GetYearOffDuration(), GetCurrentLifeTime(), Phase.On);
	}

	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(configuration.GetElement()).tag.ProperName();
		List<GeoTuner.Instance> items = Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId());
		GeoTuner.Instance instance = items.Find((GeoTuner.Instance g) => g.GetAssignedGeyser() == this);
		int num = items.Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
		bool flag = num > 0;
		string text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION, ElementLoader.FindElementByHash(configuration.GetElement()).name, GameUtil.GetFormattedMass(configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond), GameUtil.GetFormattedTemperature(configuration.GetTemperature()));
		if (flag)
		{
			Func<float, float> obj = delegate(float emissionPerCycleModifier)
			{
				float num8 = 600f / configuration.GetIterationLength();
				return emissionPerCycleModifier / num8 / configuration.GetOnDuration();
			};
			int amountOfGeotunersAffectingThisGeyser = GetAmountOfGeotunersAffectingThisGeyser();
			float num2 = ((temperatureModificationMethod == ModificationMethod.Percentages) ? (instance.currentGeyserModification.temperatureModifier * configuration.geyserType.temperature) : instance.currentGeyserModification.temperatureModifier);
			float num3 = obj((massModificationMethod == ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * configuration.scaledRate) : instance.currentGeyserModification.massPerCycleModifier);
			float num4 = (float)amountOfGeotunersAffectingThisGeyser * num2;
			float num5 = (float)amountOfGeotunersAffectingThisGeyser * num3;
			string arg2 = ((num4 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num4, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative);
			string arg3 = ((num5 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num5, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, includeSuffix: true, "{0:0.##}");
			string text2 = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative);
			string text3 = ((num3 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num3, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, includeSuffix: true, "{0:0.##}");
			text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED, ElementLoader.FindElementByHash(configuration.GetElement()).name, GameUtil.GetFormattedMass(configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond), GameUtil.GetFormattedTemperature(configuration.GetTemperature()));
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_COUNT, amountOfGeotunersAffectingThisGeyser.ToString(), num.ToString());
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_TOTAL, arg3, arg2);
			for (int i = 0; i < amountOfGeotunersAffectingThisGeyser; i++)
			{
				string text4 = "\n    • " + UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE.ToString();
				text4 = text4 + text3 + " " + text2;
				text += text4;
			}
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, arg, GameUtil.GetFormattedMass(configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond), GameUtil.GetFormattedTemperature(configuration.GetTemperature())), text));
		if (configuration.GetDiseaseIdx() != byte.MaxValue)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(configuration.GetDiseaseIdx())), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(configuration.GetDiseaseIdx()))));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PERIOD, GameUtil.GetFormattedTime(configuration.GetOnDuration()), GameUtil.GetFormattedTime(configuration.GetIterationLength())), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PERIOD, GameUtil.GetFormattedTime(configuration.GetOnDuration()), GameUtil.GetFormattedTime(configuration.GetIterationLength()))));
		Studyable component = GetComponent<Studyable>();
		if ((bool)component && !component.Studied)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_UNSTUDIED), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_UNSTUDIED)));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED)));
		}
		else
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(configuration.GetYearOnDuration()), GameUtil.GetFormattedCycles(configuration.GetYearLength())), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(configuration.GetYearOnDuration()), GameUtil.GetFormattedCycles(configuration.GetYearLength()))));
			if (base.smi.IsInsideState(base.smi.sm.dormant))
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(RemainingDormantTime())), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(RemainingDormantTime()))));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(RemainingActiveTime())), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(RemainingActiveTime()))));
			}
			string text5 = UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT.Replace("{average}", GameUtil.GetFormattedMass(configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond)).Replace("{element}", configuration.geyserType.element.CreateTag().ProperName());
			if (flag)
			{
				text5 += "\n";
				text5 = text5 + "\n" + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE;
				int amountOfGeotunersAffectingThisGeyser2 = GetAmountOfGeotunersAffectingThisGeyser();
				float num6 = ((massModificationMethod == ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * 100f) : (instance.currentGeyserModification.massPerCycleModifier * 100f / configuration.scaledRate));
				float num7 = num6 * (float)amountOfGeotunersAffectingThisGeyser2;
				text5 = text5 + GameUtil.AddPositiveSign(num7.ToString("0.0"), num7 > 0f) + "%";
				for (int j = 0; j < amountOfGeotunersAffectingThisGeyser2; j++)
				{
					string text6 = "\n    • " + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW.ToString();
					text6 = text6 + GameUtil.AddPositiveSign(num6.ToString("0.0"), num6 > 0f) + "%";
					text5 += text6;
				}
			}
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT, GameUtil.GetFormattedMass(configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond)), text5));
		}
		return list;
	}
}
