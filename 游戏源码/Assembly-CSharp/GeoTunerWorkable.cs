using TUNING;

public class GeoTunerWorkable : Workable
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SetWorkTime(30f);
		requiredSkillPerk = Db.Get().SkillPerks.AllowGeyserTuning.Id;
		SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Studying);
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_geotuner_kanim") };
		attributeConverter = Db.Get().AttributeConverters.GeotuningSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		lightEfficiencyBonus = true;
	}
}
