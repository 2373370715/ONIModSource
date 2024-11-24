using System;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class BionicMassOxygenAbsorbChore : Chore<BionicMassOxygenAbsorbChore.Instance>
{
	// Token: 0x06001DF3 RID: 7667 RVA: 0x001B11E8 File Offset: 0x001AF3E8
	public BionicMassOxygenAbsorbChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.BionicAbsorbOxygen, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicMassOxygenAbsorbChore.Instance(this, target.gameObject);
		Func<int> data = new Func<int>(base.smi.UpdateTargetCell);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCellUntilBegun, data);
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x001B1264 File Offset: 0x001AF464
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null context.consumer");
			return;
		}
		if (context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>() == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000B3C69 File Offset: 0x000B1E69
	public static void ResetOxygenTimer(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.sm.SecondsPassedWithoutOxygen.Set(0f, smi, false);
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000B3C83 File Offset: 0x000B1E83
	public static void RefreshTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.UpdateTargetCell();
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x000B3C8C File Offset: 0x000B1E8C
	public static void UpdateTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x000B3C94 File Offset: 0x000B1E94
	public static bool HasSpaceInOxygenTank(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return smi.oxygenTankMonitor.SpaceAvailableInTank > 0f;
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x001B12DC File Offset: 0x001AF4DC
	public static void AbsorbUpdate(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		float mass = Mathf.Min(dt * BionicMassOxygenAbsorbChore.ABSORB_RATE, smi.oxygenTankMonitor.SpaceAvailableInTank);
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = new BionicMassOxygenAbsorbChore.AbsorbUpdateData(smi, dt);
		int gameCell;
		SimHashes nearBreathableElement = BionicMassOxygenAbsorbChore.GetNearBreathableElement(gameCell = Grid.PosToCell(smi.sm.dupe.Get(smi)), BionicMassOxygenAbsorbChore.ABSORB_RANGE, out gameCell);
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(BionicMassOxygenAbsorbChore.OnSimConsumeCallback), absorbUpdateData, "BionicMassOxygenAbsorbChore");
		SimMessages.ConsumeMass(gameCell, nearBreathableElement, mass, 3, handle.index);
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x001B1368 File Offset: 0x001AF568
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = (BionicMassOxygenAbsorbChore.AbsorbUpdateData)data;
		absorbUpdateData.smi.OnSimConsume(mass_cb_info, absorbUpdateData.dt);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x001B1390 File Offset: 0x001AF590
	public static SimHashes GetNearBreathableElement(int centralCell, CellOffset[] range, out int elementCell)
	{
		float num = 0f;
		int num2 = centralCell;
		SimHashes simHashes = SimHashes.Vacuum;
		foreach (CellOffset offset in range)
		{
			int num3 = Grid.OffsetCell(centralCell, offset);
			SimHashes simHashes2 = SimHashes.Vacuum;
			float breathableMassInCell = BionicMassOxygenAbsorbChore.GetBreathableMassInCell(num3, out simHashes2);
			if (breathableMassInCell > Mathf.Epsilon && (simHashes == SimHashes.Vacuum || breathableMassInCell > num))
			{
				simHashes = simHashes2;
				num = breathableMassInCell;
				num2 = num3;
			}
		}
		elementCell = num2;
		return simHashes;
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x001B1408 File Offset: 0x001AF608
	private static float GetBreathableMassInCell(int cell, out SimHashes elementID)
	{
		if (Grid.IsValidCell(cell))
		{
			Element element = Grid.Element[cell];
			if (element.HasTag(GameTags.Breathable))
			{
				elementID = element.id;
				return Grid.Mass[cell];
			}
		}
		elementID = SimHashes.Vacuum;
		return 0f;
	}

	// Token: 0x040012D3 RID: 4819
	public static CellOffset[] ABSORB_RANGE = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x040012D4 RID: 4820
	public const float GIVE_UP_DURATION = 2f;

	// Token: 0x040012D5 RID: 4821
	public const float ABSORB_RATE_IDEAL_CHORE_DURATION = 30f;

	// Token: 0x040012D6 RID: 4822
	public static readonly float ABSORB_RATE = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG / 30f;

	// Token: 0x040012D7 RID: 4823
	public const string ABSORB_ANIM_FILE = "anim_banshee_kanim";

	// Token: 0x040012D8 RID: 4824
	public const string ABSORB_PRE_ANIM_NAME = "working_pre";

	// Token: 0x040012D9 RID: 4825
	public const string ABSORB_LOOP_ANIM_NAME = "working_loop";

	// Token: 0x040012DA RID: 4826
	public const string ABSORB_PST_ANIM_NAME = "working_pst";

	// Token: 0x040012DB RID: 4827
	public static CellOffset MouthCellOffset = new CellOffset(0, 1);

	// Token: 0x02000676 RID: 1654
	public class States : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore>
	{
		// Token: 0x06001DFE RID: 7678 RVA: 0x001B14DC File Offset: 0x001AF6DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.dupe);
			this.move.DefaultState(this.move.onGoing);
			this.move.onGoing.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.RefreshTargetSafeCell)).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.UpdateTargetSafeCell), UpdateRate.SIM_200ms, false).MoveTo((BionicMassOxygenAbsorbChore.Instance smi) => smi.targetCell, this.absorb, this.move.fail, true);
			this.move.fail.ReturnFailure();
			this.absorb.ToggleTag(GameTags.RecoveringBreath).ToggleAnims("anim_banshee_kanim", 0f).DefaultState(this.absorb.pre);
			this.absorb.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.absorb.loop).ScheduleGoTo(3f, this.absorb.loop).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer));
			this.absorb.loop.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer)).ParamTransition<float>(this.SecondsPassedWithoutOxygen, this.absorb.pst, (BionicMassOxygenAbsorbChore.Instance smi, float secondsPassed) => secondsPassed > 2f).OnSignal(this.TankFilledSignal, this.absorb.pst).PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.AbsorbUpdate), UpdateRate.SIM_200ms, false);
			this.absorb.pst.PlayAnim("working_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040012DC RID: 4828
		public BionicMassOxygenAbsorbChore.States.MoveStates move;

		// Token: 0x040012DD RID: 4829
		public BionicMassOxygenAbsorbChore.States.MassAbsorbStates absorb;

		// Token: 0x040012DE RID: 4830
		public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State complete;

		// Token: 0x040012DF RID: 4831
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.FloatParameter SecondsPassedWithoutOxygen;

		// Token: 0x040012E0 RID: 4832
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.TargetParameter dupe;

		// Token: 0x040012E1 RID: 4833
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Signal TankFilledSignal;

		// Token: 0x02000677 RID: 1655
		public class MoveStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x040012E2 RID: 4834
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State onGoing;

			// Token: 0x040012E3 RID: 4835
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;
		}

		// Token: 0x02000678 RID: 1656
		public class MassAbsorbStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x040012E4 RID: 4836
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;

			// Token: 0x040012E5 RID: 4837
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;

			// Token: 0x040012E6 RID: 4838
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;
		}
	}

	// Token: 0x0200067A RID: 1658
	public struct AbsorbUpdateData
	{
		// Token: 0x06001E06 RID: 7686 RVA: 0x000B3CD6 File Offset: 0x000B1ED6
		public AbsorbUpdateData(BionicMassOxygenAbsorbChore.Instance smi, float dt)
		{
			this.smi = smi;
			this.dt = dt;
		}

		// Token: 0x040012EA RID: 4842
		public BionicMassOxygenAbsorbChore.Instance smi;

		// Token: 0x040012EB RID: 4843
		public float dt;
	}

	// Token: 0x0200067B RID: 1659
	public class Instance : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.GameInstance
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x000B3CE6 File Offset: 0x000B1EE6
		public float OXYGEN_MASS_GIVE_UP_TRESHOLD
		{
			get
			{
				return this.oxygenBreather.ConsumptionRate;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x000B3CFC File Offset: 0x000B1EFC
		// (set) Token: 0x06001E08 RID: 7688 RVA: 0x000B3CF3 File Offset: 0x000B1EF3
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06001E0A RID: 7690 RVA: 0x001B16CC File Offset: 0x001AF8CC
		public Instance(BionicMassOxygenAbsorbChore master, GameObject duplicant) : base(master)
		{
			this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = duplicant.GetComponent<Navigator>();
			base.sm.dupe.Set(duplicant, base.smi, false);
			this.dupePrefabID = duplicant.GetComponent<KPrefabID>();
			this.oxygenTankMonitor = duplicant.GetSMI<BionicOxygenTankMonitor.Instance>();
			this.oxygenBreather = duplicant.GetComponent<OxygenBreather>();
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x001B175C File Offset: 0x001AF95C
		public int UpdateTargetCell()
		{
			this.query.Reset();
			this.navigator.RunQuery(base.smi.query);
			int cell = base.smi.query.GetResultCell();
			if (!this.oxygenBreather.IsBreathableElementAtCell(cell, null))
			{
				cell = PathFinder.InvalidCell;
			}
			this.targetCell = cell;
			return this.targetCell;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x001B17C0 File Offset: 0x001AF9C0
		public void OnSimConsume(Sim.MassConsumedCallback mass_cb_info, float dt)
		{
			if (this.dupePrefabID == null || this.oxygenBreather == null || this.oxygenTankMonitor == null || this.dupePrefabID.HasTag(GameTags.Dead))
			{
				return;
			}
			GameObject gameObject = this.dupePrefabID.gameObject;
			if (mass_cb_info.mass <= this.OXYGEN_MASS_GIVE_UP_TRESHOLD * dt)
			{
				base.sm.SecondsPassedWithoutOxygen.Set(base.sm.SecondsPassedWithoutOxygen.Get(base.smi) + dt, base.smi, false);
				return;
			}
			if (ElementLoader.elements[(int)mass_cb_info.elemIdx].id == SimHashes.ContaminatedOxygen)
			{
				this.oxygenBreather.Trigger(-935848905, mass_cb_info);
			}
			float num = this.oxygenTankMonitor.AddGas(mass_cb_info);
			if (num > Mathf.Epsilon)
			{
				SimMessages.EmitMass(Grid.PosToCell(gameObject), mass_cb_info.elemIdx, num, mass_cb_info.temperature, byte.MaxValue, 0, -1);
			}
			if (!BionicMassOxygenAbsorbChore.HasSpaceInOxygenTank(this))
			{
				base.sm.TankFilledSignal.Trigger(this);
			}
			BionicMassOxygenAbsorbChore.ResetOxygenTimer(base.smi);
		}

		// Token: 0x040012EC RID: 4844
		public Navigator navigator;

		// Token: 0x040012ED RID: 4845
		public SafetyQuery query;

		// Token: 0x040012EF RID: 4847
		public KPrefabID dupePrefabID;

		// Token: 0x040012F0 RID: 4848
		public int targetCell = Grid.InvalidCell;

		// Token: 0x040012F1 RID: 4849
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor;
	}
}
