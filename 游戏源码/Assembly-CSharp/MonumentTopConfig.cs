using Database;
using TUNING;
using UnityEngine;

public class MonumentTopConfig : IBuildingConfig
{
	public const string ID = "MonumentTop";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MonumentTop", 5, 5, "monument_upper_a_kanim", 1000, 60f, new float[3] { 2500f, 2500f, 5000f }, new string[3]
		{
			SimHashes.Glass.ToString(),
			SimHashes.Diamond.ToString(),
			SimHashes.Steel.ToString()
		}, 9999f, BuildLocationRule.BuildingAttachPoint, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
		BuildingTemplates.CreateMonumentBuildingDef(obj);
		obj.SceneLayer = Grid.SceneLayer.BuildingFront;
		obj.OverheatTemperature = 2273.15f;
		obj.Floodable = false;
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.AttachmentSlotTag = "MonumentTop";
		obj.ObjectLayer = ObjectLayer.Building;
		obj.attachablePosition = new CellOffset(0, 0);
		obj.RequiresPowerInput = false;
		obj.CanMove = false;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Top;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Top;
			monumentPart.stateUISymbol = "upper";
		};
	}
}
