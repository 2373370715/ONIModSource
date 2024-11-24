using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

public class MorbRoverMaker : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>
{
	public class Def : BaseDef
	{
		public float FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;

		public Tag ROVER_PREFAB_ID;

		public float INITIAL_MORB_DEVELOPMENT_PERCENTAGE;

		public float ROVER_CRAFTING_DURATION;

		public float METAL_PER_ROVER;

		public long GERMS_PER_ROVER;

		public int MAX_GERMS_TAKEN_PER_PACKAGE;

		public int GERM_TYPE;

		public SimHashes ROVER_MATERIAL;

		public ConduitType GERM_INTAKE_CONDUIT_TYPE;

		public float GetConduitMaxPackageMass()
		{
			return GERM_INTAKE_CONDUIT_TYPE switch
			{
				ConduitType.Gas => 1f, 
				ConduitType.Liquid => 10f, 
				_ => 1f, 
			};
		}
	}

	public class CoverStates : State
	{
		public State idle;

		public State careOrderGiven;

		public State complete;
	}

	public class OperationalStates : State
	{
		public CoverStates covered;

		public State idle;

		public CraftingStates crafting;

		public State waitingForMorb;

		public DoctorStates doctor;

		public State finish;
	}

	public class DoctorStates : State
	{
		public State needed;

		public State working;
	}

	public class CraftingStates : State
	{
		public State conflict;

		public State pre;

		public State loop;

		public State pst;
	}

	public new class Instance : GameInstance, ISidescreenButtonControl
	{
		public Action<long> GermsAdded;

		public System.Action OnUncovered;

		public Action<GameObject> OnRoverSpawned;

		[MyCmpGet]
		private MorbRoverMakerRevealWorkable workable_reveal;

		[MyCmpGet]
		private MorbRoverMakerWorkable workable_release;

		[MyCmpGet]
		private Operational operational;

		[MyCmpGet]
		private KBatchedAnimController buildingAnimCtr;

		[MyCmpGet]
		private ManualDeliveryKG manualDelivery;

		[MyCmpGet]
		private Storage storage;

		[MyCmpGet]
		private MorbRoverMaker_Capsule capsule;

		[MyCmpGet]
		private KSelectable selectable;

		private MeterController RobotProgressMeter;

		private int inputCell = -1;

		private int outputCell = -1;

		private Chore workChore_revealMachine;

		private Chore workChore_releaseRover;

		[Serialize]
		private float lastastMaterialsConsumedTemp = -1f;

		[Serialize]
		private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;

		public float lastTimeGermsAdded = -1f;

		private Guid germsRequiredAlertStatusItemHandle;

		public long MorbDevelopment_GermsCollected => base.sm.Germs.Get(base.smi);

		public long MorbDevelopment_RemainingGerms => base.def.GERMS_PER_ROVER - MorbDevelopment_GermsCollected;

		public float MorbDevelopment_Progress => Mathf.Clamp((float)MorbDevelopment_GermsCollected / (float)base.def.GERMS_PER_ROVER, 0f, 1f);

		public bool HasMaterialsForRover => storage.GetMassAvailable(base.def.ROVER_MATERIAL) >= base.def.METAL_PER_ROVER;

		public float RoverDevelopment_Progress => base.sm.CraftProgress.Get(base.smi);

		public bool HasBeenRevealed => base.sm.WasUncoverByDuplicant.Get(base.smi);

		public bool CanPumpGerms
		{
			get
			{
				if ((bool)operational && MorbDevelopment_Progress < 1f)
				{
					return HasBeenRevealed;
				}
				return false;
			}
		}

		public string SidescreenButtonText => HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN);

		public string SidescreenButtonTooltip => HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP);

		public Workable GetWorkable_RevealMachine()
		{
			return workable_reveal;
		}

		public Workable GetWorkable_ReleaseRover()
		{
			return workable_release;
		}

		public void ShowGermRequiredStatusItemAlert()
		{
			if (germsRequiredAlertStatusItemHandle == default(Guid))
			{
				germsRequiredAlertStatusItemHandle = selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, base.smi);
			}
		}

		public void HideGermRequiredStatusItemAlert()
		{
			if (germsRequiredAlertStatusItemHandle != default(Guid))
			{
				selectable.RemoveStatusItem(germsRequiredAlertStatusItemHandle);
				germsRequiredAlertStatusItemHandle = default(Guid);
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			RobotProgressMeter = new MeterController(buildingAnimCtr, "meter_robot_target", "meter_robot", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront);
		}

		public override void StartSM()
		{
			Building component = GetComponent<Building>();
			inputCell = component.GetUtilityInputCell();
			outputCell = component.GetUtilityOutputCell();
			base.StartSM();
			if (!HasBeenRevealed)
			{
				base.sm.Germs.Set(0L, base.smi);
				AddGerms((long)((float)base.def.GERMS_PER_ROVER * base.def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), playAnimations: false);
			}
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(Flow);
			UpdateMeters();
		}

		public void AddGerms(long amount, bool playAnimations = true)
		{
			long value = MorbDevelopment_GermsCollected + amount;
			base.sm.Germs.Set(value, base.smi);
			UpdateMeters();
			if (amount > 0)
			{
				if (playAnimations)
				{
					capsule.PlayPumpGermsAnimation();
				}
				GermsAdded?.Invoke(amount);
				lastTimeGermsAdded = GameClock.Instance.GetTime();
			}
		}

		public long RemoveGerms(long amount)
		{
			long num = amount.Min(MorbDevelopment_GermsCollected);
			long value = MorbDevelopment_GermsCollected - num;
			base.sm.Germs.Set(value, base.smi);
			UpdateMeters();
			return num;
		}

		public void EnableManualDelivery(string reason)
		{
			manualDelivery.Pause(pause: false, reason);
		}

		public void DisableManualDelivery(string reason)
		{
			manualDelivery.Pause(pause: true, reason);
		}

		public void SetRoverDevelopmentProgress(float value)
		{
			base.sm.CraftProgress.Set(value, base.smi);
			UpdateMeters();
		}

		public void UpdateMeters()
		{
			RobotProgressMeter.SetPositionPercent(RoverDevelopment_Progress);
			capsule.SetMorbDevelopmentProgress(MorbDevelopment_Progress);
			capsule.SetGermMeterProgress(HasBeenRevealed ? MorbDevelopment_Progress : 0f);
		}

		public void Uncover()
		{
			base.sm.WasUncoverByDuplicant.Set(value: true, base.smi);
			OnUncovered?.Invoke();
		}

		public void CreateWorkChore_ReleaseRover()
		{
			if (workChore_releaseRover == null)
			{
				workChore_releaseRover = new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, workable_release);
			}
		}

		public void CancelWorkChore_ReleaseRover()
		{
			if (workChore_releaseRover != null)
			{
				workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
				workChore_releaseRover = null;
			}
		}

		public void CreateWorkChore_RevealMachine()
		{
			if (workChore_revealMachine == null)
			{
				workChore_revealMachine = new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, workable_reveal);
			}
		}

		public void CancelWorkChore_RevealMachine()
		{
			if (workChore_revealMachine != null)
			{
				workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
				workChore_revealMachine = null;
			}
		}

		public void ConsumeRoverBodyCraftingMaterials()
		{
			float amount_consumed = 0f;
			storage.ConsumeAndGetDisease(base.def.ROVER_MATERIAL.CreateTag(), base.def.METAL_PER_ROVER, out amount_consumed, out lastastMaterialsConsumedDiseases, out lastastMaterialsConsumedTemp);
		}

		public void SpawnRover()
		{
			if (RoverDevelopment_Progress == 1f)
			{
				RemoveGerms(base.def.GERMS_PER_ROVER);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(base.def.ROVER_PREFAB_ID), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Creatures);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (lastastMaterialsConsumedDiseases.idx != byte.MaxValue)
				{
					component.AddDisease(lastastMaterialsConsumedDiseases.idx, lastastMaterialsConsumedDiseases.count, "From the materials provided for its creation");
				}
				if (lastastMaterialsConsumedTemp > 0f)
				{
					component.SetMassTemperature(component.Mass, lastastMaterialsConsumedTemp);
				}
				gameObject.SetActive(value: true);
				SetRoverDevelopmentProgress(0f);
				OnRoverSpawned?.Invoke(gameObject);
			}
		}

		private void Flow(float dt)
		{
			if (!CanPumpGerms)
			{
				return;
			}
			ConduitFlow flowManager = Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE);
			int num = 0;
			if (flowManager.HasConduit(inputCell) && flowManager.HasConduit(outputCell))
			{
				ConduitFlow.ConduitContents contents = flowManager.GetContents(inputCell);
				ConduitFlow.ConduitContents contents2 = flowManager.GetContents(outputCell);
				float num2 = Mathf.Min(contents.mass, base.def.GetConduitMaxPackageMass() * dt);
				if (flowManager.CanMergeContents(contents, contents2, num2))
				{
					float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num2);
					if (amountAllowedForMerging > 0f)
					{
						ConduitFlow obj = ((base.def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow);
						int num3 = contents.diseaseCount;
						if (contents.diseaseIdx != byte.MaxValue && contents.diseaseIdx == base.def.GERM_TYPE)
						{
							num = (int)MorbDevelopment_RemainingGerms.Min(base.def.MAX_GERMS_TAKEN_PER_PACKAGE).Min(contents.diseaseCount);
							num3 -= num;
						}
						float num4 = obj.AddElement(outputCell, contents.element, amountAllowedForMerging, contents.temperature, contents.diseaseIdx, num3);
						if (amountAllowedForMerging != num4)
						{
							Debug.Log("[Morb Rover Maker] Mass Differs By: " + (amountAllowedForMerging - num4));
						}
						flowManager.RemoveElement(inputCell, num4);
					}
				}
			}
			if (num > 0)
			{
				AddGerms(num);
			}
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(Flow);
		}

		public bool SidescreenEnabled()
		{
			return true;
		}

		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		public int HorizontalGroupID()
		{
			return 0;
		}

		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		public void OnSidescreenButtonPressed()
		{
			if (HasBeenRevealed)
			{
				storage.DropAll();
				return;
			}
			bool flag = base.smi.sm.UncoverOrderRequested.Get(base.smi);
			base.smi.sm.UncoverOrderRequested.Set(!flag, base.smi);
		}
	}

	private const string ROBOT_PROGRESS_METER_TARGET_NAME = "meter_robot_target";

	private const string ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";

	private const string COVERED_IDLE_ANIM_NAME = "dusty";

	private const string IDLE_ANIM_NAME = "idle";

	private const string CRAFT_PRE_ANIM_NAME = "crafting_pre";

	private const string CRAFT_LOOP_ANIM_NAME = "crafting_loop";

	private const string CRAFT_PST_ANIM_NAME = "crafting_pst";

	private const string CRAFT_COMPLETED_ANIM_NAME = "crafting_complete";

	private const string WAITING_FOR_DOCTOR_ANIM_NAME = "waiting";

	public BoolParameter UncoverOrderRequested;

	public BoolParameter WasUncoverByDuplicant;

	public LongParameter Germs;

	public FloatParameter CraftProgress;

	public State no_operational;

	public OperationalStates operational;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = no_operational;
		root.Update(GermsRequiredFeedbackUpdate, UpdateRate.SIM_1000ms);
		no_operational.Enter(delegate(Instance smi)
		{
			DisableManualDelivery(smi, "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason");
		}).TagTransition(GameTags.Operational, operational);
		operational.TagTransition(GameTags.Operational, no_operational, on_remove: true).DefaultState(operational.covered);
		operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty).ParamTransition(WasUncoverByDuplicant, operational.idle, GameStateMachine<MorbRoverMaker, Instance, IStateMachineTarget, Def>.IsTrue).Enter(delegate(Instance smi)
		{
			DisableManualDelivery(smi, "Machine can't ask for materials if it has not been investigated by a dupe");
		})
			.DefaultState(operational.covered.idle);
		operational.covered.idle.PlayAnim("dusty").ParamTransition(UncoverOrderRequested, operational.covered.careOrderGiven, GameStateMachine<MorbRoverMaker, Instance, IStateMachineTarget, Def>.IsTrue);
		operational.covered.careOrderGiven.PlayAnim("dusty").Enter(StartWorkChore_RevealMachine).Exit(CancelWorkChore_RevealMachine)
			.WorkableCompleteTransition((Instance smi) => smi.GetWorkable_RevealMachine(), operational.covered.complete)
			.ParamTransition(UncoverOrderRequested, operational.covered.idle, GameStateMachine<MorbRoverMaker, Instance, IStateMachineTarget, Def>.IsFalse);
		operational.covered.complete.Enter(SetUncovered);
		operational.idle.Enter(delegate(Instance smi)
		{
			EnableManualDelivery(smi, "Operational and discovered");
		}).EnterTransition(operational.crafting, ShouldBeCrafting).EnterTransition(operational.waitingForMorb, IsCraftingCompleted)
			.EventTransition(GameHashes.OnStorageChange, operational.crafting, ShouldBeCrafting)
			.PlayAnim("idle")
			.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress);
		operational.crafting.DefaultState(operational.crafting.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody);
		operational.crafting.conflict.Enter(ResetRoverBodyCraftingProgress).GoTo(operational.idle);
		operational.crafting.pre.EventTransition(GameHashes.OnStorageChange, operational.crafting.conflict, GameStateMachine<MorbRoverMaker, Instance, IStateMachineTarget, Def>.Not(ShouldBeCrafting)).PlayAnim("crafting_pre").OnAnimQueueComplete(operational.crafting.loop);
		operational.crafting.loop.EventTransition(GameHashes.OnStorageChange, operational.crafting.conflict, GameStateMachine<MorbRoverMaker, Instance, IStateMachineTarget, Def>.Not(ShouldBeCrafting)).Update(CraftingUpdate).PlayAnim("crafting_loop", KAnim.PlayMode.Loop)
			.ParamTransition(CraftProgress, operational.crafting.pst, GameStateMachine<MorbRoverMaker, Instance, IStateMachineTarget, Def>.IsOne);
		operational.crafting.pst.Enter(ConsumeRoverBodyCraftingMaterials).PlayAnim("crafting_pst").OnAnimQueueComplete(operational.waitingForMorb);
		operational.waitingForMorb.PlayAnim("crafting_complete").ParamTransition(Germs, operational.doctor, HasEnoughGerms).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress);
		operational.doctor.Enter(StartWorkChore_ReleaseRover).Exit(CancelWorkChore_ReleaseRover).WorkableCompleteTransition((Instance smi) => smi.GetWorkable_ReleaseRover(), operational.finish)
			.DefaultState(operational.doctor.needed);
		operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop).WorkableStartTransition((Instance smi) => smi.GetWorkable_ReleaseRover(), operational.doctor.working).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor);
		operational.doctor.working.WorkableStopTransition((Instance smi) => smi.GetWorkable_ReleaseRover(), operational.doctor.needed);
		operational.finish.Enter(SpawnRover).GoTo(operational.idle);
	}

	public static bool ShouldBeCrafting(Instance smi)
	{
		if (smi.HasMaterialsForRover)
		{
			return smi.RoverDevelopment_Progress < 1f;
		}
		return false;
	}

	public static bool IsCraftingCompleted(Instance smi)
	{
		return smi.RoverDevelopment_Progress == 1f;
	}

	public static bool HasEnoughGerms(Instance smi, long germCount)
	{
		return germCount >= smi.def.GERMS_PER_ROVER;
	}

	public static void StartWorkChore_ReleaseRover(Instance smi)
	{
		smi.CreateWorkChore_ReleaseRover();
	}

	public static void CancelWorkChore_ReleaseRover(Instance smi)
	{
		smi.CancelWorkChore_ReleaseRover();
	}

	public static void StartWorkChore_RevealMachine(Instance smi)
	{
		smi.CreateWorkChore_RevealMachine();
	}

	public static void CancelWorkChore_RevealMachine(Instance smi)
	{
		smi.CancelWorkChore_RevealMachine();
	}

	public static void SetUncovered(Instance smi)
	{
		smi.Uncover();
	}

	public static void SpawnRover(Instance smi)
	{
		smi.SpawnRover();
	}

	public static void EnableManualDelivery(Instance smi, string reason)
	{
		smi.EnableManualDelivery(reason);
	}

	public static void DisableManualDelivery(Instance smi, string reason)
	{
		smi.DisableManualDelivery(reason);
	}

	public static void ConsumeRoverBodyCraftingMaterials(Instance smi)
	{
		smi.ConsumeRoverBodyCraftingMaterials();
	}

	public static void ResetRoverBodyCraftingProgress(Instance smi)
	{
		smi.SetRoverDevelopmentProgress(0f);
	}

	public static void CraftingUpdate(Instance smi, float dt)
	{
		float roverDevelopmentProgress = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) / smi.def.ROVER_CRAFTING_DURATION, 0f, 1f);
		smi.SetRoverDevelopmentProgress(roverDevelopmentProgress);
	}

	public static void GermsRequiredFeedbackUpdate(Instance smi, float dt)
	{
		if ((GameClock.Instance.GetTime() - smi.lastTimeGermsAdded > smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT) & (smi.MorbDevelopment_Progress < 1f) & !smi.IsInsideState(smi.sm.operational.doctor) & smi.HasBeenRevealed)
		{
			smi.ShowGermRequiredStatusItemAlert();
		}
		else
		{
			smi.HideGermRequiredStatusItemAlert();
		}
	}
}
