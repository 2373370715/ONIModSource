using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000C42 RID: 3138
[AddComponentMenu("KMonoBehaviour/Workable/BeachChairWorkable")]
public class BeachChairWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003C22 RID: 15394 RVA: 0x0022D460 File Offset: 0x0022B660
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_beach_chair_kanim")
		};
		this.workAnims = null;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.lightEfficiencyBonus = false;
		base.SetWorkTime(150f);
		this.beachChair = base.GetComponent<BeachChair>();
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x000C6D23 File Offset: 0x000C4F23
	protected override void OnStartWork(WorkerBase worker)
	{
		this.timeLit = 0f;
		this.beachChair.SetWorker(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("BeachChairRelaxing", false);
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0022D4E4 File Offset: 0x0022B6E4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		int i = Grid.PosToCell(base.gameObject);
		bool flag = (float)Grid.LightIntensity[i] >= (float)BeachChairConfig.TAN_LUX - 1f;
		this.beachChair.SetLit(flag);
		if (flag)
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 1f);
			this.timeLit += dt;
		}
		else
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 0f);
		}
		return false;
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x0022D574 File Offset: 0x0022B774
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.timeLit / this.workTime >= 0.75f)
		{
			component.Add(this.beachChair.specificEffectLit, true);
			component.Remove(this.beachChair.specificEffectUnlit);
		}
		else
		{
			component.Add(this.beachChair.specificEffectUnlit, true);
			component.Remove(this.beachChair.specificEffectLit);
		}
		component.Add(this.beachChair.trackingEffect, true);
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x000C6D5B File Offset: 0x000C4F5B
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("BeachChairRelaxing");
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0022D5FC File Offset: 0x0022B7FC
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.beachChair.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.beachChair.specificEffectLit) || component.HasEffect(this.beachChair.specificEffectUnlit))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002926 RID: 10534
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002927 RID: 10535
	private float timeLit;

	// Token: 0x04002928 RID: 10536
	public string soundPath = GlobalAssets.GetSound("BeachChair_music_lp", false);

	// Token: 0x04002929 RID: 10537
	public HashedString BEACH_CHAIR_LIT_PARAMETER = "beachChair_lit";

	// Token: 0x0400292A RID: 10538
	public int basePriority;

	// Token: 0x0400292B RID: 10539
	private BeachChair beachChair;
}
