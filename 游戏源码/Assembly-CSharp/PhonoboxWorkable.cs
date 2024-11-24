using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020016A3 RID: 5795
[AddComponentMenu("KMonoBehaviour/Workable/PhonoboxWorkable")]
public class PhonoboxWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060077A9 RID: 30633 RVA: 0x0030EC60 File Offset: 0x0030CE60
	private PhonoboxWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060077AA RID: 30634 RVA: 0x000EE8B0 File Offset: 0x000ECAB0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x060077AB RID: 30635 RVA: 0x0030ECFC File Offset: 0x0030CEFC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect))
		{
			component.Add(this.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x060077AC RID: 30636 RVA: 0x0030ED48 File Offset: 0x0030CF48
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

	// Token: 0x060077AD RID: 30637 RVA: 0x000EE8D8 File Offset: 0x000ECAD8
	protected override void OnStartWork(WorkerBase worker)
	{
		this.owner.AddWorker(worker);
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x060077AE RID: 30638 RVA: 0x000EE8F8 File Offset: 0x000ECAF8
	protected override void OnStopWork(WorkerBase worker)
	{
		this.owner.RemoveWorker(worker);
		worker.GetComponent<Effects>().Remove("Dancing");
	}

	// Token: 0x060077AF RID: 30639 RVA: 0x0030EDA8 File Offset: 0x0030CFA8
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04005971 RID: 22897
	public Phonobox owner;

	// Token: 0x04005972 RID: 22898
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04005973 RID: 22899
	public string specificEffect = "Danced";

	// Token: 0x04005974 RID: 22900
	public string trackingEffect = "RecentlyDanced";

	// Token: 0x04005975 RID: 22901
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
		}
	};
}
