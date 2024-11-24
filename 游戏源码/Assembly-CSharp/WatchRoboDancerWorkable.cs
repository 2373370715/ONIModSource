using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001042 RID: 4162
public class WatchRoboDancerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060054F2 RID: 21746 RVA: 0x0027CD14 File Offset: 0x0027AF14
	private WatchRoboDancerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x000D7784 File Offset: 0x000D5984
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(30f);
		this.showProgressBar = false;
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x0027CD7C File Offset: 0x0027AF7C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.TRACKING_EFFECT, true);
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.SPECIFIC_EFFECT, true);
		}
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x0027CDC4 File Offset: 0x0027AFC4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x060054F6 RID: 21750 RVA: 0x000D77B3 File Offset: 0x000D59B3
	protected override void OnStartWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x060054F7 RID: 21751 RVA: 0x000D77C7 File Offset: 0x000D59C7
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker.GetComponent<Facing>().Face(this.owner.transform.position.x);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060054F8 RID: 21752 RVA: 0x000D77F1 File Offset: 0x000D59F1
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Dancing");
		ChoreHelpers.DestroyLocator(base.gameObject);
	}

	// Token: 0x060054F9 RID: 21753 RVA: 0x0027CE20 File Offset: 0x0027B020
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04003B95 RID: 15253
	public GameObject owner;

	// Token: 0x04003B96 RID: 15254
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04003B97 RID: 15255
	public static string SPECIFIC_EFFECT = "SawRoboDancer";

	// Token: 0x04003B98 RID: 15256
	public static string TRACKING_EFFECT = "RecentlySawRoboDancer";

	// Token: 0x04003B99 RID: 15257
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance1_kanim")
		}
	};
}
