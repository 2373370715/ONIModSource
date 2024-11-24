using System;
using STRINGS;

// Token: 0x02000D8A RID: 3466
public class Gantry : Switch
{
	// Token: 0x060043F4 RID: 17396 RVA: 0x002468EC File Offset: 0x00244AEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (Gantry.infoStatusItem == null)
		{
			Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Gantry.infoStatusItem.resolveStringCallback = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
		}
		base.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
		this.smi = new Gantry.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		base.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, this.smi);
	}

	// Token: 0x060043F5 RID: 17397 RVA: 0x000CBEAD File Offset: 0x000CA0AD
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		base.OnCleanUp();
	}

	// Token: 0x060043F6 RID: 17398 RVA: 0x000CBECD File Offset: 0x000CA0CD
	public void SetWalkable(bool active)
	{
		this.fakeFloorAdder.SetFloor(active);
	}

	// Token: 0x060043F7 RID: 17399 RVA: 0x000CBEDB File Offset: 0x000CA0DB
	protected override void Toggle()
	{
		base.Toggle();
		this.smi.SetSwitchState(this.switchedOn);
	}

	// Token: 0x060043F8 RID: 17400 RVA: 0x000CBEF4 File Offset: 0x000CA0F4
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x060043F9 RID: 17401 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void UpdateSwitchStatus()
	{
	}

	// Token: 0x060043FA RID: 17402 RVA: 0x0024698C File Offset: 0x00244B8C
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		Gantry.Instance instance = (Gantry.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL;
		string arg = instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.EXTENDED : BUILDING.STATUSITEMS.GANTRY.RETRACTED;
		return string.Format(format, arg);
	}

	// Token: 0x04002E9D RID: 11933
	public static readonly HashedString PORT_ID = "Gantry";

	// Token: 0x04002E9E RID: 11934
	[MyCmpReq]
	private Building building;

	// Token: 0x04002E9F RID: 11935
	[MyCmpReq]
	private FakeFloorAdder fakeFloorAdder;

	// Token: 0x04002EA0 RID: 11936
	private Gantry.Instance smi;

	// Token: 0x04002EA1 RID: 11937
	private static StatusItem infoStatusItem;

	// Token: 0x02000D8B RID: 3467
	public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
	{
		// Token: 0x060043FD RID: 17405 RVA: 0x002469DC File Offset: 0x00244BDC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.extended;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.retracted_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
			this.retracted.PlayAnim("off").ParamTransition<bool>(this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
			this.extended_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
			this.extended.Enter(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(false);
			}).PlayAnim("on").ParamTransition<bool>(this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse).ToggleTag(GameTags.GantryExtended);
		}

		// Token: 0x04002EA2 RID: 11938
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;

		// Token: 0x04002EA3 RID: 11939
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;

		// Token: 0x04002EA4 RID: 11940
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;

		// Token: 0x04002EA5 RID: 11941
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;

		// Token: 0x04002EA6 RID: 11942
		public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;
	}

	// Token: 0x02000D8D RID: 3469
	public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
	{
		// Token: 0x06004407 RID: 17415 RVA: 0x00246B60 File Offset: 0x00244D60
		public Instance(Gantry master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_extend.Set(true, base.smi, false);
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x000CBF5D File Offset: 0x000CA15D
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(Gantry.PORT_ID);
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x000CBF6F File Offset: 0x000CA16F
		public bool IsExtended()
		{
			if (!this.IsAutomated())
			{
				return this.manual_on;
			}
			return this.logic_on;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x000CBF86 File Offset: 0x000CA186
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldExtend();
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x000CBF95 File Offset: 0x000CA195
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x000CBFB0 File Offset: 0x000CA1B0
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldExtend();
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x00246BE8 File Offset: 0x00244DE8
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != Gantry.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldExtend();
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x00246C28 File Offset: 0x00244E28
		private void UpdateShouldExtend()
		{
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_extend.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_extend.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04002EAE RID: 11950
		private Operational operational;

		// Token: 0x04002EAF RID: 11951
		public LogicPorts logic;

		// Token: 0x04002EB0 RID: 11952
		public bool logic_on = true;

		// Token: 0x04002EB1 RID: 11953
		private bool manual_on;
	}
}
