using TUNING;
using UnityEngine;

public class DevPumpSolidConfig : IBuildingConfig
{
	public const string ID = "DevPumpSolid";

	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	private ConduitPortInfo primaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(1, 1));

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("DevPumpSolid", 2, 2, "dev_pump_solid_kanim", 100, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.ALL_METALS, 9999f, BuildLocationRule.Anywhere, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.PENALTY.TIER1);
		obj.RequiresPowerInput = false;
		obj.OutputConduitType = ConduitType.Solid;
		obj.Floodable = false;
		obj.Invincible = true;
		obj.Overheatable = false;
		obj.Entombable = false;
		obj.ViewMode = OverlayModes.SolidConveyor.ID;
		obj.AudioCategory = "Metal";
		obj.UtilityOutputOffset = primaryPort.offset;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "DevPumpSolid");
		obj.DebugOnly = true;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		base.ConfigureBuildingTemplate(go, prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevPump>().elementState = Filterable.ElementState.Solid;
		go.AddOrGet<Storage>().capacityKg = 20f;
		go.AddTag(GameTags.CorrosionProof);
		SolidConduitDispenser solidConduitDispenser = go.AddOrGet<SolidConduitDispenser>();
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.elementFilter = null;
		go.AddOrGetDef<OperationalController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
	}
}
