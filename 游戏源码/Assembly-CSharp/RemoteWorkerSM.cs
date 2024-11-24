using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02001758 RID: 5976
public class RemoteWorkerSM : StateMachineComponent<RemoteWorkerSM.StatesInstance>
{
	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06007AF8 RID: 31480 RVA: 0x000F0D54 File Offset: 0x000EEF54
	// (set) Token: 0x06007AF9 RID: 31481 RVA: 0x000F0D5C File Offset: 0x000EEF5C
	public bool Docked
	{
		get
		{
			return this.docked;
		}
		set
		{
			this.docked = value;
		}
	}

	// Token: 0x06007AFA RID: 31482 RVA: 0x000F0D65 File Offset: 0x000EEF65
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007AFB RID: 31483 RVA: 0x000F0D78 File Offset: 0x000EEF78
	public void SetNextChore(Chore.Precondition.Context next)
	{
		this.nextChore = new Chore.Precondition.Context?(next);
	}

	// Token: 0x06007AFC RID: 31484 RVA: 0x000F0D86 File Offset: 0x000EEF86
	public void StartNextChore()
	{
		if (this.nextChore != null)
		{
			this.driver.SetChore(this.nextChore.Value);
			this.nextChore = null;
		}
	}

	// Token: 0x06007AFD RID: 31485 RVA: 0x000F0DB7 File Offset: 0x000EEFB7
	public bool HasChoreQueued()
	{
		return this.nextChore != null;
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x06007AFE RID: 31486 RVA: 0x000F0DC4 File Offset: 0x000EEFC4
	// (set) Token: 0x06007AFF RID: 31487 RVA: 0x000F0DD7 File Offset: 0x000EEFD7
	public RemoteWorkerDock HomeDepot
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.homeDepot;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			this.homeDepot = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06007B00 RID: 31488 RVA: 0x000F0DE5 File Offset: 0x000EEFE5
	// (set) Token: 0x06007B01 RID: 31489 RVA: 0x000F0DED File Offset: 0x000EEFED
	public bool ActivelyControlled { get; set; }

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x06007B02 RID: 31490 RVA: 0x000F0DF6 File Offset: 0x000EEFF6
	// (set) Token: 0x06007B03 RID: 31491 RVA: 0x000F0DFE File Offset: 0x000EEFFE
	public bool ActivelyWorking { get; set; }

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x06007B04 RID: 31492 RVA: 0x000F0E07 File Offset: 0x000EF007
	// (set) Token: 0x06007B05 RID: 31493 RVA: 0x000F0E0F File Offset: 0x000EF00F
	public bool Available { get; set; }

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x06007B06 RID: 31494 RVA: 0x000F0E18 File Offset: 0x000EF018
	public bool RequiresMaintnence
	{
		get
		{
			return this.power.IsLowPower;
		}
	}

	// Token: 0x06007B07 RID: 31495 RVA: 0x00319FFC File Offset: 0x003181FC
	public void TickResources(float dt)
	{
		this.power.ApplyDeltaEnergy(-0.1f * dt);
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		this.storage.ConsumeAndGetDisease(GameTags.LubricatingOil, 0.1f * dt, out num, out diseaseInfo, out temperature);
		if (num > 0f)
		{
			this.storage.AddElement(SimHashes.LiquidGunk, num, temperature, diseaseInfo.idx, diseaseInfo.count, true, true);
		}
	}

	// Token: 0x06007B08 RID: 31496 RVA: 0x000F0E25 File Offset: 0x000EF025
	public GameObject FindStation()
	{
		if (Components.ComplexFabricators.Count == 0)
		{
			return null;
		}
		return Components.ComplexFabricators[0].gameObject;
	}

	// Token: 0x06007B09 RID: 31497 RVA: 0x000F0E45 File Offset: 0x000EF045
	public bool HasHomeDepot()
	{
		return !this.HomeDepot.IsNullOrDestroyed();
	}

	// Token: 0x04005C2E RID: 23598
	[MyCmpAdd]
	private RemoteWorkerCapacitor power;

	// Token: 0x04005C2F RID: 23599
	[MyCmpAdd]
	private RemoteWorkerGunkMonitor gunk;

	// Token: 0x04005C30 RID: 23600
	[MyCmpAdd]
	private RemoteWorkerOilMonitor oil;

	// Token: 0x04005C31 RID: 23601
	[MyCmpAdd]
	private ChoreDriver driver;

	// Token: 0x04005C32 RID: 23602
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005C33 RID: 23603
	public bool playNewWorker;

	// Token: 0x04005C34 RID: 23604
	[Serialize]
	private bool docked = true;

	// Token: 0x04005C35 RID: 23605
	private Chore.Precondition.Context? nextChore;

	// Token: 0x04005C36 RID: 23606
	private const string LostAnim_pre = "incapacitate_pre";

	// Token: 0x04005C37 RID: 23607
	private const string LostAnim_loop = "incapacitate_loop";

	// Token: 0x04005C38 RID: 23608
	private const string DeathAnim = "incapacitate_death";

	// Token: 0x04005C39 RID: 23609
	[Serialize]
	private Ref<RemoteWorkerDock> homeDepot;

	// Token: 0x04005C3A RID: 23610
	private Chore.Precondition.Context enterDockContext;

	// Token: 0x04005C3B RID: 23611
	private Chore.Precondition.Context exitDockContext;

	// Token: 0x02001759 RID: 5977
	public class StatesInstance : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.GameInstance
	{
		// Token: 0x06007B0B RID: 31499 RVA: 0x000F0E64 File Offset: 0x000EF064
		public StatesInstance(RemoteWorkerSM master) : base(master)
		{
			base.sm.homedock.Set(base.smi.master.HomeDepot, base.smi);
		}
	}

	// Token: 0x0200175A RID: 5978
	public class States : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM>
	{
		// Token: 0x06007B0C RID: 31500 RVA: 0x0031A064 File Offset: 0x00318264
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.uncontrolled;
			this.controlled.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			}).EnterTransition(this.controlled.exit_dock, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition(this.controlled.working, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition(this.uncontrolled, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator)), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms).Update(new Action<RemoteWorkerSM.StatesInstance, float>(RemoteWorkerSM.States.TickResources), UpdateRate.RENDER_200ms, false);
			this.controlled.exit_dock.ToggleWork<ExitableDock>(this.homedock, this.controlled.working, this.controlled.working, (RemoteWorkerSM.StatesInstance _) => true);
			this.controlled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = true;
			}).Exit(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = false;
			}).DefaultState(this.controlled.working.find_work);
			this.controlled.working.find_work.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				if (RemoteWorkerSM.States.HasChore(smi))
				{
					smi.GoTo(this.controlled.working.do_work);
					return;
				}
				RemoteWorkerSM.States.SetNextChore(smi);
				smi.GoTo(RemoteWorkerSM.States.HasChore(smi) ? this.controlled.working.do_work : this.controlled.no_work);
			});
			this.controlled.working.do_work.Exit(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).Transition(this.controlled.working.find_work, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore)), UpdateRate.SIM_200ms);
			this.controlled.no_work.Transition(this.controlled.working.do_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore), UpdateRate.SIM_200ms).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChoreQueued), UpdateRate.SIM_200ms);
			this.uncontrolled.EnterTransition(this.uncontrolled.working.new_worker, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)).EnterTransition(this.uncontrolled.idle, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition(this.uncontrolled.approach_dock, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.uncontrolled.approach_dock.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).MoveTo<IApproachable>(this.homedock, this.uncontrolled.working.enter, this.uncontrolled.idle, null, null);
			this.uncontrolled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			});
			this.uncontrolled.working.new_worker.ToggleWork<NewWorker>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.working.recharge, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.enter.ToggleWork<EnterableDock>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge.ToggleWork<WorkerRecharger>(this.homedock, this.uncontrolled.working.recharge_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge_pst.OnAnimQueueComplete(this.uncontrolled.working.drain_gunk).ScheduleGoTo(1f, this.uncontrolled.working.drain_gunk);
			this.uncontrolled.working.drain_gunk.ToggleWork<WorkerGunkRemover>(this.homedock, this.uncontrolled.working.drain_gunk_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.drain_gunk_pst.OnAnimQueueComplete(this.uncontrolled.working.fill_oil).ScheduleGoTo(1f, this.uncontrolled.working.fill_oil);
			this.uncontrolled.working.fill_oil.ToggleWork<WorkerOilRefiller>(this.homedock, this.uncontrolled.working.fill_oil_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.fill_oil_pst.OnAnimQueueComplete(this.uncontrolled.idle).ScheduleGoTo(1f, this.uncontrolled.idle);
			this.uncontrolled.idle.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).PlayAnim(RemoteWorkerConfig.IDLE_IN_DOCK_ANIM, KAnim.PlayMode.Loop).Transition(this.uncontrolled.working.recharge, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.RequiresMaintnence), new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.DockIsOperational)), UpdateRate.SIM_1000ms);
			this.incapacitated.ToggleAnims("anim_incapacitated_kanim", 0f);
			this.incapacitated.lost.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.Play("incapacitate_pre", KAnim.PlayMode.Once);
				smi.Queue("incapacitate_loop", KAnim.PlayMode.Loop);
				RemoteWorkerSM.States.ClearChore(smi);
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.UnreachableDock, null).Transition(this.controlled, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.incapacitated.die.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).PlayAnim("incapacitate_death").OnAnimQueueComplete(this.incapacitated.explode).ToggleStatusItem(Db.Get().DuplicantStatusItems.NoHomeDock, null);
			this.incapacitated.explode.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.Explode));
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x000F0E93 File Offset: 0x000EF093
		public static bool IsNewWorker(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.playNewWorker;
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x000F0EA0 File Offset: 0x000EF0A0
		public static void SetNextChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.StartNextChore();
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x000F0EAD File Offset: 0x000EF0AD
		public static void ClearChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.driver.StopChore();
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x000F0EBF File Offset: 0x000EF0BF
		public static bool HasChore(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.driver.HasChore();
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x000F0ED1 File Offset: 0x000EF0D1
		public static bool HasChoreQueued(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HasChoreQueued();
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x0031A7F0 File Offset: 0x003189F0
		public static bool CanReachDepot(RemoteWorkerSM.StatesInstance smi)
		{
			int depotCell = RemoteWorkerSM.States.GetDepotCell(smi);
			return depotCell != Grid.InvalidCell && smi.master.GetComponent<Navigator>().CanReach(depotCell);
		}

		// Token: 0x06007B13 RID: 31507 RVA: 0x0031A820 File Offset: 0x00318A20
		public static int GetDepotCell(RemoteWorkerSM.StatesInstance smi)
		{
			RemoteWorkerDock homeDepot = smi.master.HomeDepot;
			if (homeDepot == null)
			{
				return Grid.InvalidCell;
			}
			return Grid.PosToCell(homeDepot);
		}

		// Token: 0x06007B14 RID: 31508 RVA: 0x000F0EDE File Offset: 0x000EF0DE
		public static bool HasRemoteOperator(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.ActivelyControlled;
		}

		// Token: 0x06007B15 RID: 31509 RVA: 0x000F0EEB File Offset: 0x000EF0EB
		public static bool RequiresMaintnence(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.RequiresMaintnence;
		}

		// Token: 0x06007B16 RID: 31510 RVA: 0x000F0EF8 File Offset: 0x000EF0F8
		public static bool DockIsOperational(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HomeDepot != null && smi.master.HomeDepot.IsOperational;
		}

		// Token: 0x06007B17 RID: 31511 RVA: 0x000F0F1F File Offset: 0x000EF11F
		public static bool HasHomeDepot(RemoteWorkerSM.StatesInstance smi)
		{
			return RemoteWorkerSM.States.GetDepotCell(smi) != Grid.InvalidCell;
		}

		// Token: 0x06007B18 RID: 31512 RVA: 0x000F0F31 File Offset: 0x000EF131
		public static void StopWork(RemoteWorkerSM.StatesInstance smi)
		{
			if (smi.master.driver.HasChore())
			{
				smi.master.driver.StopChore();
			}
		}

		// Token: 0x06007B19 RID: 31513 RVA: 0x000F0F55 File Offset: 0x000EF155
		public static bool IsInsideDock(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.Docked;
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x0031A850 File Offset: 0x00318A50
		public static void Explode(RemoteWorkerSM.StatesInstance smi)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0f);
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), 42f, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, false, false);
			Util.KDestroyGameObject(smi.master.gameObject);
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x000F0F62 File Offset: 0x000EF162
		public static void TickResources(RemoteWorkerSM.StatesInstance smi, float dt)
		{
			if (dt > 0f)
			{
				smi.master.TickResources(dt);
			}
		}

		// Token: 0x04005C3F RID: 23615
		public RemoteWorkerSM.States.ControlledStates controlled;

		// Token: 0x04005C40 RID: 23616
		public RemoteWorkerSM.States.UncontrolledStates uncontrolled;

		// Token: 0x04005C41 RID: 23617
		public RemoteWorkerSM.States.IncapacitatedStates incapacitated;

		// Token: 0x04005C42 RID: 23618
		public StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.TargetParameter homedock;

		// Token: 0x0200175B RID: 5979
		public class ControlledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x04005C43 RID: 23619
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State exit_dock;

			// Token: 0x04005C44 RID: 23620
			public RemoteWorkerSM.States.ControlledStates.WorkingStates working;

			// Token: 0x04005C45 RID: 23621
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State no_work;

			// Token: 0x0200175C RID: 5980
			public class WorkingStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x04005C46 RID: 23622
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State find_work;

				// Token: 0x04005C47 RID: 23623
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State do_work;
			}
		}

		// Token: 0x0200175D RID: 5981
		public class UncontrolledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x04005C48 RID: 23624
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State approach_dock;

			// Token: 0x04005C49 RID: 23625
			public RemoteWorkerSM.States.UncontrolledStates.WorkingDockStates working;

			// Token: 0x04005C4A RID: 23626
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State idle;

			// Token: 0x0200175E RID: 5982
			public class WorkingDockStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x04005C4B RID: 23627
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State new_worker;

				// Token: 0x04005C4C RID: 23628
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State enter;

				// Token: 0x04005C4D RID: 23629
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge;

				// Token: 0x04005C4E RID: 23630
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge_pst;

				// Token: 0x04005C4F RID: 23631
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk;

				// Token: 0x04005C50 RID: 23632
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk_pst;

				// Token: 0x04005C51 RID: 23633
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil;

				// Token: 0x04005C52 RID: 23634
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil_pst;
			}
		}

		// Token: 0x0200175F RID: 5983
		public class IncapacitatedStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x04005C53 RID: 23635
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost;

			// Token: 0x04005C54 RID: 23636
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State die;

			// Token: 0x04005C55 RID: 23637
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State explode;
		}
	}
}
