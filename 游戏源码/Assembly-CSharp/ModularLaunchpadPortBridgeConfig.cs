using TUNING;
using UnityEngine;

public class ModularLaunchpadPortBridgeConfig : IBuildingConfig
{
	public const string ID = "ModularLaunchpadPortBridge";

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("ModularLaunchpadPortBridge", 1, 2, "rocket_loader_extension_kanim", 1000, 60f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 9999f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER0, decor: BUILDINGS.DECOR.NONE);
		obj.SceneLayer = Grid.SceneLayer.BuildingBack;
		obj.OverheatTemperature = 2273.15f;
		obj.Floodable = false;
		obj.Entombable = false;
		obj.DefaultAnimState = "idle";
		obj.AudioCategory = "Metal";
		obj.AudioSize = "medium";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.ModularConduitPort);
		component.AddTag(GameTags.NotRocketInteriorBuilding);
		component.AddTag(BaseModularLaunchpadPortConfig.LinkTag);
		ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
		def.headBuildingTag = "LaunchPad".ToTag();
		def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
		def.objectLayer = ObjectLayer.Building;
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[1]
		{
			new CellOffset(0, 1)
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
