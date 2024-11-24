using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000C1E RID: 3102
[AddComponentMenu("KMonoBehaviour/Workable/ArcadeMachineWorkable")]
public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003B21 RID: 15137 RVA: 0x000C627A File Offset: 0x000C447A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x000C62AA File Offset: 0x000C44AA
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("ArcadePlaying", false);
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x000C62C5 File Offset: 0x000C44C5
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("ArcadePlaying");
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x00229DD0 File Offset: 0x00227FD0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
		{
			component.Add(ArcadeMachineWorkable.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
		{
			component.Add(ArcadeMachineWorkable.specificEffect, true);
		}
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x00229E18 File Offset: 0x00228018
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002870 RID: 10352
	public ArcadeMachine owner;

	// Token: 0x04002871 RID: 10353
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04002872 RID: 10354
	private static string specificEffect = "PlayedArcade";

	// Token: 0x04002873 RID: 10355
	private static string trackingEffect = "RecentlyPlayedArcade";
}
