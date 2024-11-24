using System;
using STRINGS;

// Token: 0x020001B4 RID: 436
public class HiveHarvestMonitor : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>
{
	// Token: 0x060005F5 RID: 1525 RVA: 0x0015A8FC File Offset: 0x00158AFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.do_not_harvest;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.RefreshUserMenu, delegate(HiveHarvestMonitor.Instance smi)
		{
			smi.OnRefreshUserMenu();
		});
		this.do_not_harvest.ParamTransition<bool>(this.shouldHarvest, this.harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => bShouldHarvest);
		this.harvest.ParamTransition<bool>(this.shouldHarvest, this.do_not_harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => !bShouldHarvest).DefaultState(this.harvest.not_ready);
		this.harvest.not_ready.EventTransition(GameHashes.OnStorageChange, this.harvest.ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) >= smi.def.harvestThreshold);
		this.harvest.ready.ToggleChore((HiveHarvestMonitor.Instance smi) => smi.CreateHarvestChore(), this.harvest.not_ready).EventTransition(GameHashes.OnStorageChange, this.harvest.not_ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) < smi.def.harvestThreshold);
	}

	// Token: 0x04000452 RID: 1106
	public StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.BoolParameter shouldHarvest;

	// Token: 0x04000453 RID: 1107
	public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State do_not_harvest;

	// Token: 0x04000454 RID: 1108
	public HiveHarvestMonitor.HarvestStates harvest;

	// Token: 0x020001B5 RID: 437
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000455 RID: 1109
		public Tag producedOre;

		// Token: 0x04000456 RID: 1110
		public float harvestThreshold;
	}

	// Token: 0x020001B6 RID: 438
	public class HarvestStates : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State
	{
		// Token: 0x04000457 RID: 1111
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State not_ready;

		// Token: 0x04000458 RID: 1112
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State ready;
	}

	// Token: 0x020001B7 RID: 439
	public new class Instance : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.GameInstance
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x000A8A82 File Offset: 0x000A6C82
		public Instance(IStateMachineTarget master, HiveHarvestMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0015AA78 File Offset: 0x00158C78
		public void OnRefreshUserMenu()
		{
			if (base.sm.shouldHarvest.Get(this))
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.NAME, delegate()
				{
					base.sm.shouldHarvest.Set(false, this, false);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.TOOLTIP, true), 1f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYBEEHIVE.NAME, delegate()
			{
				base.sm.shouldHarvest.Set(true, this, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYBEEHIVE.TOOLTIP, true), 1f);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0015AB34 File Offset: 0x00158D34
		public Chore CreateHarvestChore()
		{
			return new WorkChore<HiveWorkableEmpty>(Db.Get().ChoreTypes.Ranch, base.master.GetComponent<HiveWorkableEmpty>(), null, true, new Action<Chore>(base.smi.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000A8A8C File Offset: 0x000A6C8C
		public void OnEmptyComplete(Chore chore)
		{
			base.smi.storage.Drop(base.smi.def.producedOre);
		}

		// Token: 0x04000459 RID: 1113
		[MyCmpReq]
		public Storage storage;
	}
}
