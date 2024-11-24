using System;
using Klei.AI;
using STRINGS;
using TUNING;

public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
	public class RancherChoreStates : GameStateMachine<RancherChoreStates, RancherChoreStates.Instance>
	{
		private class RanchState : State
		{
			public State callForCritter;

			public State working;

			public State pst;
		}

		public new class Instance : GameInstance
		{
			private const float WAIT_FOR_RANCHABLE_TIMEOUT = 2f;

			public RanchStation.Instance ranchStation;

			private float waitTime;

			public Instance(KPrefabID rancher_station)
				: base((IStateMachineTarget)rancher_station)
			{
				ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
			}

			public void WaitForAvailableRanchable(float dt)
			{
				waitTime += dt;
				State state = (ranchStation.IsCritterAvailableForRanching ? base.sm.ranchCritter : null);
				if (state != null || waitTime >= 2f)
				{
					waitTime = 0f;
					GoTo(state);
				}
			}
		}

		public TargetParameter rancher;

		private State moveToRanch;

		private RanchState ranchCritter;

		private State waitForAvailableRanchable;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = moveToRanch;
			Target(rancher);
			root.Exit("TriggerRanchStationNoLongerAvailable", delegate(Instance smi)
			{
				smi.ranchStation.TriggerRanchStationNoLongerAvailable();
			});
			moveToRanch.MoveTo((Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), waitForAvailableRanchable);
			waitForAvailableRanchable.Enter("FindRanchable", delegate(Instance smi)
			{
				smi.WaitForAvailableRanchable(0f);
			}).Update("FindRanchable", delegate(Instance smi, float dt)
			{
				smi.WaitForAvailableRanchable(dt);
			});
			ranchCritter.ScheduleGoTo(0.5f, ranchCritter.callForCritter).EventTransition(GameHashes.CreatureAbandonedRanchStation, waitForAvailableRanchable);
			ranchCritter.callForCritter.ToggleAnims("anim_interacts_rancherstation_kanim").PlayAnim("calling_loop", KAnim.PlayMode.Loop).ScheduleActionNextFrame("TellCreatureRancherIsReady", delegate(Instance smi)
			{
				smi.ranchStation.MessageRancherReady();
			})
				.Target(masterTarget)
				.EventTransition(GameHashes.CreatureArrivedAtRanchStation, ranchCritter.working);
			ranchCritter.working.ToggleWork<RancherWorkable>(masterTarget, ranchCritter.pst, waitForAvailableRanchable, null);
			ranchCritter.pst.ToggleAnims((Func<Instance, HashedString>)GetRancherInteractAnim).QueueAnim("wipe_brow").OnAnimQueueComplete(waitForAvailableRanchable);
		}

		private static HashedString GetRancherInteractAnim(Instance smi)
		{
			return smi.ranchStation.def.RancherInteractAnim;
		}

		public static bool TryRanchCreature(Instance smi)
		{
			Debug.Assert(smi.ranchStation != null, "smi.ranchStation was null");
			RanchedStates.Instance activeRanchable = smi.ranchStation.ActiveRanchable;
			if (activeRanchable.IsNullOrStopped())
			{
				return false;
			}
			KPrefabID component = activeRanchable.GetComponent<KPrefabID>();
			smi.sm.rancher.Get(smi).Trigger(937885943, component.PrefabTag.Name);
			smi.ranchStation.RanchCreature();
			return true;
		}
	}

	public class RancherWorkable : Workable
	{
		private RanchStation.Instance ranch;

		private KBatchedAnimController critterAnimController;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			ranch = base.gameObject.GetSMI<RanchStation.Instance>();
			overrideAnims = new KAnimFile[1] { Assets.GetAnim(ranch.def.RancherInteractAnim) };
			SetWorkTime(ranch.def.WorkTime);
			SetWorkerStatusItem(ranch.def.RanchingStatusItem);
			attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
			skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			lightEfficiencyBonus = false;
		}

		public override Klei.AI.Attribute GetWorkAttribute()
		{
			return Db.Get().Attributes.Ranching;
		}

		protected override void OnStartWork(Worker worker)
		{
			if (ranch != null)
			{
				critterAnimController = ranch.ActiveRanchable.AnimController;
				critterAnimController.Play(ranch.def.RanchedPreAnim);
				critterAnimController.Queue(ranch.def.RanchedLoopAnim, KAnim.PlayMode.Loop);
			}
		}

		protected override bool OnWorkTick(Worker worker, float dt)
		{
			if (ranch.def.OnRanchWorkTick != null)
			{
				ranch.def.OnRanchWorkTick(ranch.ActiveRanchable.gameObject, dt, this);
			}
			return base.OnWorkTick(worker, dt);
		}

		public override void OnPendingCompleteWork(Worker work)
		{
			RancherChoreStates.Instance sMI = base.gameObject.GetSMI<RancherChoreStates.Instance>();
			if (ranch != null && sMI != null && RancherChoreStates.TryRanchCreature(sMI))
			{
				critterAnimController.Play(ranch.def.RanchedPstAnim);
			}
		}

		protected override void OnAbortWork(Worker worker)
		{
			if (ranch != null && !(critterAnimController == null))
			{
				critterAnimController.Play(ranch.def.RanchedAbortAnim);
			}
		}
	}

	public Precondition IsOpenForRanching = new Precondition
	{
		id = "IsCreatureAvailableForRanching",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING,
		sortOrder = -3,
		fn = delegate(ref Precondition.Context context, object data)
		{
			RanchStation.Instance instance = data as RanchStation.Instance;
			return !instance.HasRancher && instance.IsCritterAvailableForRanching;
		}
	};

	public RancherChore(KPrefabID rancher_station)
		: base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget)rancher_station, (ChoreProvider)null, run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.basic, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		AddPrecondition(IsOpenForRanching, rancher_station.GetSMI<RanchStation.Instance>());
		SkillPerkMissingComplainer component = GetComponent<SkillPerkMissingComplainer>();
		AddPrecondition(ChorePreconditions.instance.HasSkillPerk, component.requiredSkillPerk);
		AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		AddPrecondition(ChorePreconditions.instance.CanMoveTo, rancher_station.GetComponent<Building>());
		Operational component2 = rancher_station.GetComponent<Operational>();
		AddPrecondition(ChorePreconditions.instance.IsOperational, component2);
		Deconstructable component3 = rancher_station.GetComponent<Deconstructable>();
		AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component3);
		BuildingEnabledButton component4 = rancher_station.GetComponent<BuildingEnabledButton>();
		AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component4);
		base.smi = new RancherChoreStates.Instance(rancher_station);
		SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
	}

	public override void Begin(Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi);
		base.Begin(context);
	}

	protected override void End(string reason)
	{
		base.End(reason);
		base.smi.sm.rancher.Set(null, base.smi);
	}
}
