using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001A2C RID: 6700
[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06008BB6 RID: 35766 RVA: 0x000AC786 File Offset: 0x000AA986
	private VerticalWindTunnelWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06008BB7 RID: 35767 RVA: 0x00360BF4 File Offset: 0x0035EDF4
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new WindTunnelWorkerStateMachine.StatesInstance(worker, this);
		return anim;
	}

	// Token: 0x06008BB8 RID: 35768 RVA: 0x000FB522 File Offset: 0x000F9722
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x06008BB9 RID: 35769 RVA: 0x000FB54A File Offset: 0x000F974A
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
	}

	// Token: 0x06008BBA RID: 35770 RVA: 0x000FB565 File Offset: 0x000F9765
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
	}

	// Token: 0x06008BBB RID: 35771 RVA: 0x000FB57E File Offset: 0x000F977E
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		component.Add(this.windTunnel.trackingEffect, true);
		component.Add(this.windTunnel.specificEffect, true);
	}

	// Token: 0x06008BBC RID: 35772 RVA: 0x00360C18 File Offset: 0x0035EE18
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.windTunnel.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.windTunnel.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.windTunnel.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400691E RID: 26910
	public VerticalWindTunnel windTunnel;

	// Token: 0x0400691F RID: 26911
	public HashedString overrideAnim;

	// Token: 0x04006920 RID: 26912
	public string[] preAnims;

	// Token: 0x04006921 RID: 26913
	public string loopAnim;

	// Token: 0x04006922 RID: 26914
	public string[] pstAnims;
}
