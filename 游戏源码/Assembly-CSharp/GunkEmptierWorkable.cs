using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class GunkEmptierWorkable : Workable
{
	// Token: 0x06000E88 RID: 3720 RVA: 0x000AC786 File Offset: 0x000AA986
	private GunkEmptierWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x001793E0 File Offset: 0x001775E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gunkdump_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		this.storage = base.GetComponent<Storage>();
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0017944C File Offset: 0x0017764C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float mass = Mathf.Min(new float[]
		{
			dt / this.workTime * GunkMonitor.GUNK_CAPACITY,
			this.gunkMonitor.CurrentGunkMass,
			this.storage.RemainingCapacity()
		});
		this.gunkMonitor.ExpellGunk(mass, this.storage);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x001794AC File Offset: 0x001776AC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		this.TriggerRoomEffects();
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x00179520 File Offset: 0x00177720
	private void TriggerRoomEffects()
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), base.worker.GetComponent<Effects>());
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x000AC796 File Offset: 0x000AA996
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.gunkMonitor != null)
		{
			this.gunkMonitor.ExpellAllGunk(this.storage);
		}
		this.gunkMonitor = null;
		base.OnCompleteWork(worker);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x000AC7BF File Offset: 0x000AA9BF
	protected override void OnStopWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnStopWork(worker);
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x000AC7CE File Offset: 0x000AA9CE
	protected override void OnAbortWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnAbortWork(worker);
		this.gunkMonitor = null;
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x000AC7E4 File Offset: 0x000AA9E4
	private void RemoveExpellingRadStatusItem()
	{
		if (Sim.IsRadiationEnabled())
		{
			base.worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
	}

	// Token: 0x04000A79 RID: 2681
	private Storage storage;

	// Token: 0x04000A7A RID: 2682
	private GunkMonitor.Instance gunkMonitor;
}
