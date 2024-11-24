using System;
using UnityEngine;

// Token: 0x02000768 RID: 1896
public class Chore<StateMachineInstanceType> : StandardChoreBase, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000B61A7 File Offset: 0x000B43A7
	// (set) Token: 0x060021F5 RID: 8693 RVA: 0x000B61AF File Offset: 0x000B43AF
	public StateMachineInstanceType smi { get; protected set; }

	// Token: 0x060021F6 RID: 8694 RVA: 0x000B61B8 File Offset: 0x000B43B8
	protected override StateMachine.Instance GetSMI()
	{
		return this.smi;
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000B61C5 File Offset: 0x000B43C5
	public int Subscribe(int hash, Action<object> handler)
	{
		return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000B61D4 File Offset: 0x000B43D4
	public void Unsubscribe(int hash, Action<object> handler)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x000B61E3 File Offset: 0x000B43E3
	public void Unsubscribe(int id)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(id);
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000B61F1 File Offset: 0x000B43F1
	public void Trigger(int hash, object data = null)
	{
		this.GetComponent<KPrefabID>().Trigger(hash, data);
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000B6200 File Offset: 0x000B4400
	public ComponentType GetComponent<ComponentType>()
	{
		return this.target.GetComponent<ComponentType>();
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060021FC RID: 8700 RVA: 0x000B620D File Offset: 0x000B440D
	public override GameObject gameObject
	{
		get
		{
			return this.target.gameObject;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060021FD RID: 8701 RVA: 0x000B621A File Offset: 0x000B441A
	public Transform transform
	{
		get
		{
			return this.target.gameObject.transform;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060021FE RID: 8702 RVA: 0x000B622C File Offset: 0x000B442C
	public string name
	{
		get
		{
			return this.gameObject.name;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060021FF RID: 8703 RVA: 0x000B6239 File Offset: 0x000B4439
	public override bool isNull
	{
		get
		{
			return this.target.isNull;
		}
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x001C10E0 File Offset: 0x001BF2E0
	public Chore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic, int master_priority_value = 5, bool is_preemptable = false, bool allow_in_context_menu = true, int priority_mod = 0, bool add_to_daily_report = false, ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
	{
		target.Subscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		this.reportType = report_type;
		this.addToDailyReport = add_to_daily_report;
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName(this, null));
		}
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000B6246 File Offset: 0x000B4446
	public override string ResolveString(string str)
	{
		if (!this.target.isNull)
		{
			str = str.Replace("{Target}", this.target.gameObject.GetProperName());
		}
		return base.ResolveString(str);
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000B6279 File Offset: 0x000B4479
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.target != null)
		{
			this.target.Unsubscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		}
		if (this.onCleanup != null)
		{
			this.onCleanup(this);
		}
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000B62B9 File Offset: 0x000B44B9
	private void OnTargetDestroyed(object data)
	{
		this.Cancel("Target Destroyed");
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x000B62C6 File Offset: 0x000B44C6
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return base.CanPreempt(context);
	}
}
