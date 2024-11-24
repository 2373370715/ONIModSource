using TUNING;

public class MorbRoverMakerWorkable : Workable
{
	public const float DOCTOR_WORKING_TIME = 90f;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerDoctorWorking;
		SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerDoctorWorking);
		requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim") };
		attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		lightEfficiencyBonus = true;
		synchronizeAnims = true;
		shouldShowSkillPerkStatusItem = true;
		SetWorkTime(90f);
		resetProgressOnStop = true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
	}
}
