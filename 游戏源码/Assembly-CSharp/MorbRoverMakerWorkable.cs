using System;
using TUNING;

// Token: 0x020004BB RID: 1211
public class MorbRoverMakerWorkable : Workable
{
	// Token: 0x0600155C RID: 5468 RVA: 0x00193378 File Offset: 0x00191578
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerDoctorWorking;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerDoctorWorking);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = true;
		base.SetWorkTime(90f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x000AB70D File Offset: 0x000A990D
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x000AB715 File Offset: 0x000A9915
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x04000E6C RID: 3692
	public const float DOCTOR_WORKING_TIME = 90f;
}
