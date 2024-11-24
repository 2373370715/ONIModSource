using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class ContactConductivePipeBridgeConfig : IBuildingConfig
{
	public const float LIQUID_CAPACITY_KG = 10f;

	public const float GAS_CAPACITY_KG = 0.5f;

	public const float TEMPERATURE_EXCHANGE_WITH_STORAGE_MODIFIER = 50f;

	public const float BUILDING_TO_BUILDING_TEMPERATURE_SCALE = 0.001f;

	public const string ID = "ContactConductivePipeBridge";

	public const float NO_LIQUIDS_COOLDOWN = 1.5f;

	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("ContactConductivePipeBridge", 3, 1, "contactConductivePipeBridge_kanim", 30, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 2400f, BuildLocationRule.NoLiquidConduitAtOrigin, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.NONE);
		obj.Floodable = false;
		obj.Entombable = false;
		obj.Overheatable = false;
		obj.ViewMode = OverlayModes.LiquidConduits.ID;
		obj.ObjectLayer = ObjectLayer.LiquidConduitConnection;
		obj.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "small";
		obj.BaseTimeUntilRepair = -1f;
		obj.PermittedRotations = PermittedRotations.R360;
		obj.InputConduitType = ConduitType.Liquid;
		obj.OutputConduitType = ConduitType.Liquid;
		obj.UtilityInputOffset = new CellOffset(-1, 0);
		obj.UtilityOutputOffset = new CellOffset(1, 0);
		obj.UseStructureTemperature = true;
		obj.ReplacementTags = new List<Tag>();
		obj.ReplacementTags.Add(GameTags.Pipes);
		obj.ThermalConductivity = 2f;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "ContactConductivePipeBridge");
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<StructureToStructureTemperature>();
		ContactConductivePipeBridge.Def def = go.AddOrGetDef<ContactConductivePipeBridge.Def>();
		def.pumpKGRate = 10f;
		def.type = ConduitType.Liquid;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		Object.DestroyImmediate(go.GetComponent<RequireOutputs>());
		Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}
}
