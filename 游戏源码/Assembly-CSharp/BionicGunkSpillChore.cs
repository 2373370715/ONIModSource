using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000670 RID: 1648
public class BionicGunkSpillChore : Chore<BionicGunkSpillChore.StatesInstance>
{
	// Token: 0x06001DE7 RID: 7655 RVA: 0x001B0F70 File Offset: 0x001AF170
	public static void ExpellOilUpdate(BionicGunkSpillChore.StatesInstance smi, float dt)
	{
		float num = GunkMonitor.GUNK_CAPACITY * (dt / 10f);
		if (num >= smi.gunkMonitor.CurrentGunkMass)
		{
			smi.GoTo(smi.sm.pst);
			return;
		}
		smi.gunkMonitor.ExpellGunk(num, null);
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x001B0FB8 File Offset: 0x001AF1B8
	public BionicGunkSpillChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ExpellGunk, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicGunkSpillChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x040012C3 RID: 4803
	public const float EVENT_DURATION = 10f;

	// Token: 0x040012C4 RID: 4804
	public const string PRE_ANIM_NAME = "oiloverload_pre";

	// Token: 0x040012C5 RID: 4805
	public const string LOOP_ANIM_NAME = "oiloverload_loop";

	// Token: 0x040012C6 RID: 4806
	public const string PST_ANIM_NAME = "overload_pst";

	// Token: 0x02000671 RID: 1649
	public class States : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore>
	{
		// Token: 0x06001DE9 RID: 7657 RVA: 0x001B1000 File Offset: 0x001AF200
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			base.Target(this.worker);
			this.root.ToggleAnims("anim_bionic_oil_overload_kanim", 0f).ToggleEffect("ExpellingGunk").ToggleTag(GameTags.MakingMess).DoNotification((BionicGunkSpillChore.StatesInstance smi) => smi.stressfullyEmptyingGunk).Enter(delegate(BionicGunkSpillChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			});
			this.enter.PlayAnim("oiloverload_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.running);
			this.running.PlayAnim("oiloverload_loop", KAnim.PlayMode.Loop).Update(new Action<BionicGunkSpillChore.StatesInstance, float>(BionicGunkSpillChore.ExpellOilUpdate), UpdateRate.SIM_200ms, false);
			this.pst.PlayAnim("overload_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040012C7 RID: 4807
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State enter;

		// Token: 0x040012C8 RID: 4808
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State running;

		// Token: 0x040012C9 RID: 4809
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State pst;

		// Token: 0x040012CA RID: 4810
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State complete;

		// Token: 0x040012CB RID: 4811
		public StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.TargetParameter worker;
	}

	// Token: 0x02000673 RID: 1651
	public class StatesInstance : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.GameInstance
	{
		// Token: 0x06001DEF RID: 7663 RVA: 0x001B116C File Offset: 0x001AF36C
		public StatesInstance(BionicGunkSpillChore master, GameObject worker) : base(master)
		{
			this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x040012CF RID: 4815
		public Notification stressfullyEmptyingGunk = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x040012D0 RID: 4816
		public GunkMonitor.Instance gunkMonitor;
	}
}
