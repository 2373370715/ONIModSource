using System;
using TUNING;
using UnityEngine;

// Token: 0x0200053B RID: 1339
public class ReanimateBionicWorkable : Workable
{
	// Token: 0x060017AA RID: 6058 RVA: 0x0019B144 File Offset: 0x00199344
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = new HashedString[]
		{
			"offline_battery_change_pre",
			"offline_battery_change_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"offline_battery_change_pst"
		};
		base.SetWorkTime(30f);
		this.readyForSkillWorkStatusItem = Db.Get().DuplicantStatusItems.BionicRequiresSkillPerk;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.InstallingElectrobank);
		this.workingStatusItem = Db.Get().DuplicantStatusItems.BionicBeingRebooted;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bionic_kanim")
		};
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x0019B238 File Offset: 0x00199438
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Vector3 position = worker.transform.GetPosition();
		position.x = base.transform.GetPosition().x;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		worker.transform.SetPosition(position);
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x0019B28C File Offset: 0x0019948C
	protected override void OnStopWork(WorkerBase worker)
	{
		Vector3 position = worker.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		worker.transform.SetPosition(position);
		base.OnStopWork(worker);
	}
}
