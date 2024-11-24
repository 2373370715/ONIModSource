using System;

// Token: 0x02000685 RID: 1669
public class DeliverFoodChore : Chore<DeliverFoodChore.StatesInstance>
{
	// Token: 0x06001E51 RID: 7761 RVA: 0x001B38C0 File Offset: 0x001B1AC0
	public DeliverFoodChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.DeliverFood, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DeliverFoodChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.IsChattable, target);
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x001B3914 File Offset: 0x001B1B14
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.requestedrationcount.Set(base.smi.GetComponent<StateMachineController>().GetSMI<RationMonitor.Instance>().GetRationsRemaining(), base.smi, false);
		base.smi.sm.ediblesource.Set(context.consumerState.gameObject.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible(), base.smi);
		base.smi.sm.deliverypoint.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x02000686 RID: 1670
	public class StatesInstance : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.GameInstance
	{
		// Token: 0x06001E53 RID: 7763 RVA: 0x000B405D File Offset: 0x000B225D
		public StatesInstance(DeliverFoodChore master) : base(master)
		{
		}
	}

	// Token: 0x02000687 RID: 1671
	public class States : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore>
	{
		// Token: 0x06001E54 RID: 7764 RVA: 0x001B39DC File Offset: 0x001B1BDC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			this.fetch.InitializeStates(this.deliverer, this.ediblesource, this.ediblechunk, this.requestedrationcount, this.actualrationcount, this.movetodeliverypoint, null);
			this.movetodeliverypoint.InitializeStates(this.deliverer, this.deliverypoint, this.drop, null, null, null);
			this.drop.InitializeStates(this.deliverer, this.ediblechunk, this.deliverypoint, this.success, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x0400135D RID: 4957
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverer;

		// Token: 0x0400135E RID: 4958
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblesource;

		// Token: 0x0400135F RID: 4959
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblechunk;

		// Token: 0x04001360 RID: 4960
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverypoint;

		// Token: 0x04001361 RID: 4961
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter requestedrationcount;

		// Token: 0x04001362 RID: 4962
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter actualrationcount;

		// Token: 0x04001363 RID: 4963
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FetchSubState fetch;

		// Token: 0x04001364 RID: 4964
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.ApproachSubState<Chattable> movetodeliverypoint;

		// Token: 0x04001365 RID: 4965
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.DropSubState drop;

		// Token: 0x04001366 RID: 4966
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.State success;
	}
}
