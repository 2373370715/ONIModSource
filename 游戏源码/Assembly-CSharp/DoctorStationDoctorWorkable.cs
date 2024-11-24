using System;
using TUNING;
using UnityEngine;

// Token: 0x02001249 RID: 4681
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStationDoctorWorkable")]
public class DoctorStationDoctorWorkable : Workable
{
	// Token: 0x06005FE4 RID: 24548 RVA: 0x000C9266 File Offset: 0x000C7466
	private DoctorStationDoctorWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06005FE5 RID: 24549 RVA: 0x00237470 File Offset: 0x00235670
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x06005FE6 RID: 24550 RVA: 0x000AB70D File Offset: 0x000A990D
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005FE7 RID: 24551 RVA: 0x000DE7B2 File Offset: 0x000DC9B2
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.station.SetHasDoctor(true);
	}

	// Token: 0x06005FE8 RID: 24552 RVA: 0x000DE7C7 File Offset: 0x000DC9C7
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.station.SetHasDoctor(false);
	}

	// Token: 0x06005FE9 RID: 24553 RVA: 0x000DE7DC File Offset: 0x000DC9DC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.station.CompleteDoctoring();
	}

	// Token: 0x04004401 RID: 17409
	[MyCmpReq]
	private DoctorStation station;
}
