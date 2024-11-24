using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000759 RID: 1881
public class WaterCoolerChore : Chore<WaterCoolerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x0600215D RID: 8541 RVA: 0x001BFF1C File Offset: 0x001BE11C
	public WaterCoolerChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Relax, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new WaterCoolerChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x000B5CDC File Offset: 0x000B3EDC
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.drinker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x001BFFF0 File Offset: 0x001BE1F0
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04001603 RID: 5635
	public int basePriority = RELAXATION.PRIORITY.TIER2;

	// Token: 0x04001604 RID: 5636
	public string specificEffect = "Socialized";

	// Token: 0x04001605 RID: 5637
	public string trackingEffect = "RecentlySocialized";

	// Token: 0x0200075A RID: 1882
	public class States : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore>
	{
		// Token: 0x06002160 RID: 8544 RVA: 0x001C0050 File Offset: 0x001BE250
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.drink_move;
			base.Target(this.drinker);
			this.drink_move.InitializeStates(this.drinker, this.masterTarget, this.drink, null, null, null);
			this.drink.ToggleAnims(new Func<WaterCoolerChore.StatesInstance, KAnimFile>(WaterCoolerChore.States.GetAnimFileName)).DefaultState(this.drink.drink);
			this.drink.drink.Face(this.masterTarget, 0.5f).PlayAnim("working_pre").QueueAnim("working_loop", false, null).OnAnimQueueComplete(this.drink.post);
			this.drink.post.Enter("Drink", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.TriggerDrink)).Enter("Mark", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.MarkAsRecentlySocialized)).PlayAnim("working_pst").OnAnimQueueComplete(this.chat_move);
			this.chat_move.InitializeStates(this.drinker, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<SocialGatheringPointWorkable>(this.chitchatlocator, this.success, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x001C0190 File Offset: 0x001BE390
		public static KAnimFile GetAnimFileName(WaterCoolerChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.drinker.Get(smi);
			if (gameObject == null)
			{
				return Assets.GetAnim("anim_interacts_watercooler_kanim");
			}
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null && component.model == BionicMinionConfig.MODEL)
			{
				return Assets.GetAnim("anim_bionic_interacts_watercooler_kanim");
			}
			return Assets.GetAnim("anim_interacts_watercooler_kanim");
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x001C020C File Offset: 0x001BE40C
		private void MarkAsRecentlySocialized(WaterCoolerChore.StatesInstance smi)
		{
			Effects component = this.stateTarget.Get<WorkerBase>(smi).GetComponent<Effects>();
			if (!string.IsNullOrEmpty(smi.master.trackingEffect))
			{
				component.Add(smi.master.trackingEffect, true);
			}
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x001C0250 File Offset: 0x001BE450
		private void TriggerDrink(WaterCoolerChore.StatesInstance smi)
		{
			WorkerBase workerBase = this.stateTarget.Get<WorkerBase>(smi);
			smi.master.target.gameObject.GetSMI<WaterCooler.StatesInstance>().Drink(workerBase.gameObject, true);
		}

		// Token: 0x04001606 RID: 5638
		public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter drinker;

		// Token: 0x04001607 RID: 5639
		public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter chitchatlocator;

		// Token: 0x04001608 RID: 5640
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<WaterCooler> drink_move;

		// Token: 0x04001609 RID: 5641
		public WaterCoolerChore.States.DrinkStates drink;

		// Token: 0x0400160A RID: 5642
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x0400160B RID: 5643
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State chat;

		// Token: 0x0400160C RID: 5644
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State success;

		// Token: 0x0200075B RID: 1883
		public class DrinkStates : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State
		{
			// Token: 0x0400160D RID: 5645
			public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State drink;

			// Token: 0x0400160E RID: 5646
			public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State post;
		}
	}

	// Token: 0x0200075C RID: 1884
	public class StatesInstance : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.GameInstance
	{
		// Token: 0x06002166 RID: 8550 RVA: 0x000B5D1D File Offset: 0x000B3F1D
		public StatesInstance(WaterCoolerChore master) : base(master)
		{
		}
	}
}
