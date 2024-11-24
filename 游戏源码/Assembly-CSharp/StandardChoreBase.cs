using System;
using System.Collections.Generic;

// Token: 0x02000766 RID: 1894
public abstract class StandardChoreBase : Chore
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060021BD RID: 8637 RVA: 0x000B5F33 File Offset: 0x000B4133
	// (set) Token: 0x060021BE RID: 8638 RVA: 0x000B5F3B File Offset: 0x000B413B
	public override int id { get; protected set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060021BF RID: 8639 RVA: 0x000B5F44 File Offset: 0x000B4144
	// (set) Token: 0x060021C0 RID: 8640 RVA: 0x000B5F4C File Offset: 0x000B414C
	public override int priorityMod { get; protected set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060021C1 RID: 8641 RVA: 0x000B5F55 File Offset: 0x000B4155
	// (set) Token: 0x060021C2 RID: 8642 RVA: 0x000B5F5D File Offset: 0x000B415D
	public override ChoreType choreType { get; protected set; }

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060021C3 RID: 8643 RVA: 0x000B5F66 File Offset: 0x000B4166
	// (set) Token: 0x060021C4 RID: 8644 RVA: 0x000B5F6E File Offset: 0x000B416E
	public override ChoreDriver driver { get; protected set; }

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060021C5 RID: 8645 RVA: 0x000B5F77 File Offset: 0x000B4177
	// (set) Token: 0x060021C6 RID: 8646 RVA: 0x000B5F7F File Offset: 0x000B417F
	public override ChoreDriver lastDriver { get; protected set; }

	// Token: 0x060021C7 RID: 8647 RVA: 0x000B5F88 File Offset: 0x000B4188
	public override bool SatisfiesUrge(Urge urge)
	{
		return urge == this.choreType.urge;
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x000B5F98 File Offset: 0x000B4198
	public override bool IsValid()
	{
		return this.provider != null && this.gameObject.GetMyWorldId() != -1;
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060021C9 RID: 8649 RVA: 0x000B5FBB File Offset: 0x000B41BB
	// (set) Token: 0x060021CA RID: 8650 RVA: 0x000B5FC3 File Offset: 0x000B41C3
	public override IStateMachineTarget target { get; protected set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060021CB RID: 8651 RVA: 0x000B5FCC File Offset: 0x000B41CC
	// (set) Token: 0x060021CC RID: 8652 RVA: 0x000B5FD4 File Offset: 0x000B41D4
	public override bool isComplete { get; protected set; }

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060021CD RID: 8653 RVA: 0x000B5FDD File Offset: 0x000B41DD
	// (set) Token: 0x060021CE RID: 8654 RVA: 0x000B5FE5 File Offset: 0x000B41E5
	public override bool IsPreemptable { get; protected set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060021CF RID: 8655 RVA: 0x000B5FEE File Offset: 0x000B41EE
	// (set) Token: 0x060021D0 RID: 8656 RVA: 0x000B5FF6 File Offset: 0x000B41F6
	public override ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000B5FFF File Offset: 0x000B41FF
	// (set) Token: 0x060021D2 RID: 8658 RVA: 0x000B6007 File Offset: 0x000B4207
	public override Prioritizable prioritizable { get; protected set; }

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060021D3 RID: 8659 RVA: 0x000B6010 File Offset: 0x000B4210
	// (set) Token: 0x060021D4 RID: 8660 RVA: 0x000B6018 File Offset: 0x000B4218
	public override ChoreProvider provider { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060021D5 RID: 8661 RVA: 0x000B6021 File Offset: 0x000B4221
	// (set) Token: 0x060021D6 RID: 8662 RVA: 0x000B6029 File Offset: 0x000B4229
	public override bool runUntilComplete { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060021D7 RID: 8663 RVA: 0x000B6032 File Offset: 0x000B4232
	// (set) Token: 0x060021D8 RID: 8664 RVA: 0x000B603A File Offset: 0x000B423A
	public override bool isExpanded { get; protected set; }

	// Token: 0x060021D9 RID: 8665 RVA: 0x000B6043 File Offset: 0x000B4243
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return this.IsPreemptable;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000B604B File Offset: 0x000B424B
	public override string GetReportName(string context = null)
	{
		if (context == null || this.choreType.reportName == null)
		{
			return this.choreType.Name;
		}
		return string.Format(this.choreType.reportName, context);
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x001C0B58 File Offset: 0x001BED58
	public override void Cancel(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000B607A File Offset: 0x000B427A
	public override void Cleanup()
	{
		this.ClearPrioritizable();
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000B6082 File Offset: 0x000B4282
	public override ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x001C0BD0 File Offset: 0x001BEDD0
	public override void AddPrecondition(Chore.Precondition precondition, object data = null)
	{
		this.arePreconditionsDirty = true;
		this.preconditions.Add(new Chore.PreconditionInstance
		{
			condition = precondition,
			data = data
		});
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x001C0C08 File Offset: 0x001BEE08
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context item = new Chore.Precondition.Context(this, consumer_state, is_attempting_override, null);
		item.RunPreconditions();
		if (!item.IsComplete())
		{
			incomplete_contexts.Add(item);
			return;
		}
		if (item.IsSuccess())
		{
			succeeded_contexts.Add(item);
			return;
		}
		failed_contexts.Add(item);
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000B608A File Offset: 0x000B428A
	public override void Fail(string reason)
	{
		if (this.provider == null)
		{
			return;
		}
		if (this.driver == null)
		{
			return;
		}
		if (!this.runUntilComplete)
		{
			this.Cancel(reason);
			return;
		}
		this.End(reason);
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x001C0C54 File Offset: 0x001BEE54
	public override void Begin(Chore.Precondition.Context context)
	{
		if (this.driver != null)
		{
			Debug.LogErrorFormat("Chore.Begin driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				context.consumerState.choreDriver
			});
		}
		if (this.provider == null)
		{
			Debug.LogErrorFormat("Chore.Begin provider is null {0} {1} {2}, provider {3}, driver {4}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver
			});
		}
		this.driver = context.consumerState.choreDriver;
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		KSelectable component = this.driver.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.GetStatusItem(), this);
		}
		smi.StartSM();
		if (this.onBegin != null)
		{
			this.onBegin(this);
		}
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000B60C1 File Offset: 0x000B42C1
	public override bool InProgress()
	{
		return this.driver != null;
	}

	// Token: 0x060021E4 RID: 8676
	protected abstract StateMachine.Instance GetSMI();

	// Token: 0x060021E5 RID: 8677 RVA: 0x001C0DA0 File Offset: 0x001BEFA0
	public StandardChoreBase(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete, Action<Chore> on_complete, Action<Chore> on_begin, Action<Chore> on_end, PriorityScreen.PriorityClass priority_class, int priority_value, bool is_preemptable, bool allow_in_context_menu, int priority_mod, bool add_to_daily_report, ReportManager.ReportType report_type)
	{
		this.target = target;
		if (priority_value == 2147483647)
		{
			priority_class = PriorityScreen.PriorityClass.topPriority;
			priority_value = 2;
		}
		if (priority_value < 1 || priority_value > 9)
		{
			Debug.LogErrorFormat("Priority Value Out Of Range: {0}", new object[]
			{
				priority_value
			});
		}
		this.masterPriority = new PrioritySetting(priority_class, priority_value);
		this.priorityMod = priority_mod;
		this.id = Chore.GetNextChoreID();
		if (chore_provider == null)
		{
			chore_provider = GlobalChoreProvider.Instance;
			DebugUtil.Assert(chore_provider != null);
		}
		this.choreType = chore_type;
		this.runUntilComplete = run_until_complete;
		this.onComplete = on_complete;
		this.onEnd = on_end;
		this.onBegin = on_begin;
		this.IsPreemptable = is_preemptable;
		this.AddPrecondition(ChorePreconditions.instance.IsValid, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPermitted, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPreemptable, null);
		this.AddPrecondition(ChorePreconditions.instance.HasUrge, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingEarly, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingLate, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOverrideTargetNullOrMe, null);
		chore_provider.AddChore(this);
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000B60CF File Offset: 0x000B42CF
	public virtual void SetPriorityMod(int priorityMod)
	{
		this.priorityMod = priorityMod;
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x001C0EE4 File Offset: 0x001BF0E4
	public override List<Chore.PreconditionInstance> GetPreconditions()
	{
		if (this.arePreconditionsDirty)
		{
			this.preconditions.Sort((Chore.PreconditionInstance x, Chore.PreconditionInstance y) => x.condition.sortOrder.CompareTo(y.condition.sortOrder));
			this.arePreconditionsDirty = false;
		}
		return this.preconditions;
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x001C0F30 File Offset: 0x001BF130
	protected void SetPrioritizable(Prioritizable prioritizable)
	{
		if (prioritizable != null && prioritizable.IsPrioritizable())
		{
			this.prioritizable = prioritizable;
			this.masterPriority = prioritizable.GetMasterPriority();
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000B60D8 File Offset: 0x000B42D8
	private void ClearPrioritizable()
	{
		if (this.prioritizable != null)
		{
			Prioritizable prioritizable = this.prioritizable;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x000B610F File Offset: 0x000B430F
	private void OnMasterPriorityChanged(PrioritySetting priority)
	{
		this.masterPriority = priority;
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000B6118 File Offset: 0x000B4318
	public void SetOverrideTarget(ChoreConsumer chore_consumer)
	{
		if (chore_consumer != null)
		{
			string name = chore_consumer.name;
		}
		this.overrideTarget = chore_consumer;
		this.Fail("New override target");
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x001C0F84 File Offset: 0x001BF184
	protected virtual void End(string reason)
	{
		if (this.driver != null)
		{
			KSelectable component = this.driver.GetComponent<KSelectable>();
			if (component != null)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
			}
		}
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		smi.StopSM(reason);
		if (this.driver == null)
		{
			return;
		}
		this.lastDriver = this.driver;
		this.driver = null;
		if (this.onEnd != null)
		{
			this.onEnd(this);
		}
		if (this.onExit != null)
		{
			this.onExit(this);
		}
		this.driver = null;
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x001C104C File Offset: 0x001BF24C
	protected void Succeed(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		this.isComplete = true;
		if (this.onComplete != null)
		{
			this.onComplete(this);
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000B613C File Offset: 0x000B433C
	protected virtual StatusItem GetStatusItem()
	{
		return this.choreType.statusItem;
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000B6149 File Offset: 0x000B4349
	protected virtual void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			this.Succeed(reason);
			return;
		}
		this.Fail(reason);
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000B615E File Offset: 0x000B435E
	private bool RemoveFromProvider()
	{
		if (this.provider != null)
		{
			this.provider.RemoveChore(this);
			return true;
		}
		return false;
	}

	// Token: 0x0400164F RID: 5711
	private Action<Chore> onBegin;

	// Token: 0x04001650 RID: 5712
	private Action<Chore> onEnd;

	// Token: 0x04001651 RID: 5713
	public Action<Chore> onCleanup;

	// Token: 0x04001652 RID: 5714
	private List<Chore.PreconditionInstance> preconditions = new List<Chore.PreconditionInstance>();

	// Token: 0x04001653 RID: 5715
	private bool arePreconditionsDirty;

	// Token: 0x04001654 RID: 5716
	public bool addToDailyReport;

	// Token: 0x04001655 RID: 5717
	public ReportManager.ReportType reportType;
}
