using Database;
using TUNING;
using UnityEngine;

public class MonumentBottomConfig : IBuildingConfig
{
	public const string ID = "MonumentBottom";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MonumentBottom", 5, 5, "monument_base_a_kanim", 1000, 60f, new float[2] { 7500f, 2500f }, new string[2]
		{
			SimHashes.Steel.ToString(),
			SimHashes.Obsidian.ToString()
		}, 9999f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
		BuildingTemplates.CreateMonumentBuildingDef(obj);
		obj.SceneLayer = Grid.SceneLayer.BuildingFront;
		obj.OverheatTemperature = 2273.15f;
		obj.Floodable = false;
		obj.AttachmentSlotTag = "MonumentBottom";
		obj.ObjectLayer = ObjectLayer.Building;
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.attachablePosition = new CellOffset(0, 0);
		obj.RequiresPowerInput = false;
		obj.CanMove = false;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), "MonumentMiddle", null)
		};
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Bottom;
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
			monumentPart.part = MonumentPartResource.Part.Bottom;
			monumentPart.stateUISymbol = "base";
		};
	}
}
