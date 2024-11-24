using TUNING;
using UnityEngine;

public class LuxuryBedConfig : IBuildingConfig
{
	public const string ID = "LuxuryBed";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("LuxuryBed", 4, 2, "elegantbed_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.BONUS.TIER2);
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LuxuryBedType);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Bed bed = go.AddOrGet<Bed>();
		bed.effects = new string[2] { "LuxuryBedStamina", "BedHealth" };
		bed.workLayer = Grid.SceneLayer.BuildingFront;
		Sleepable sleepable = go.AddOrGet<Sleepable>();
		sleepable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_sleep_bed_kanim") };
		sleepable.workLayer = Grid.SceneLayer.BuildingFront;
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}
}
