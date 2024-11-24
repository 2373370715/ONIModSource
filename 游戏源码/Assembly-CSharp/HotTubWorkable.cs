using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020013FE RID: 5118
[AddComponentMenu("KMonoBehaviour/Workable/HotTubWorkable")]
public class HotTubWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600692A RID: 26922 RVA: 0x000AC786 File Offset: 0x000AA986
	private HotTubWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600692B RID: 26923 RVA: 0x000E4EDD File Offset: 0x000E30DD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x0600692C RID: 26924 RVA: 0x002D965C File Offset: 0x002D785C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new HotTubWorkerStateMachine.StatesInstance(worker);
		return anim;
	}

	// Token: 0x0600692D RID: 26925 RVA: 0x000E4F0C File Offset: 0x000E310C
	protected override void OnStartWork(WorkerBase worker)
	{
		this.faceLeft = (UnityEngine.Random.value > 0.5f);
		worker.GetComponent<Effects>().Add("HotTubRelaxing", false);
	}

	// Token: 0x0600692E RID: 26926 RVA: 0x000E4F36 File Offset: 0x000E3136
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("HotTubRelaxing");
	}

	// Token: 0x0600692F RID: 26927 RVA: 0x000E4F48 File Offset: 0x000E3148
	public override Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition() + (this.faceLeft ? Vector3.left : Vector3.right);
	}

	// Token: 0x06006930 RID: 26928 RVA: 0x002D9680 File Offset: 0x002D7880
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect))
		{
			component.Add(this.hotTub.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect))
		{
			component.Add(this.hotTub.specificEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
	}

	// Token: 0x06006931 RID: 26929 RVA: 0x002D96F4 File Offset: 0x002D78F4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.hotTub.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect) && component.HasEffect(this.hotTub.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect) && component.HasEffect(this.hotTub.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04004F61 RID: 20321
	public HotTub hotTub;

	// Token: 0x04004F62 RID: 20322
	private bool faceLeft;
}
