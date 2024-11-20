using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MorbRoverMakerConfig : IBuildingConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MorbRoverMaker";
		int width = 5;
		int height = 4;
		string anim = "gravitas_morb_tank_kanim";
		int hitpoints = 250;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 3200f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Overheatable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "medium";
		buildingDef.UseStructureTemperature = false;
		buildingDef.InputConduitType = this.GERM_INTAKE_CONDUIT_TYPE;
		buildingDef.OutputConduitType = this.GERM_INTAKE_CONDUIT_TYPE;
		buildingDef.UtilityInputOffset = new CellOffset(1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 3);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = ((this.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Gas) ? new List<Tag>(STORAGEFILTERS.GASES) : new List<Tag>(STORAGEFILTERS.LIQUIDS));
		storage.storageFilters.Add(MorbRoverMakerConfig.ROVER_MATERIAL_TAG.CreateTag());
		storage.allowItemRemoval = false;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = MorbRoverMakerConfig.ROVER_MATERIAL_TAG.CreateTag();
		manualDeliveryKG.capacity = 1800f;
		manualDeliveryKG.refillMass = 300f;
		manualDeliveryKG.MinimumMass = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		go.AddOrGet<Operational>();
		go.AddOrGet<Demolishable>().allowDemolition = true;
		go.AddOrGet<MorbRoverMakerWorkable>();
		go.AddOrGet<MorbRoverMakerRevealWorkable>();
		go.AddOrGet<MorbRoverMaker_Capsule>();
		MorbRoverMaker.Def def = go.AddOrGetDef<MorbRoverMaker.Def>();
		def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE = 0.5f;
		def.ROVER_PREFAB_ID = MorbRoverMakerConfig.ROVER_PREFAB_ID;
		def.ROVER_CRAFTING_DURATION = 15f;
		def.ROVER_MATERIAL = MorbRoverMakerConfig.ROVER_MATERIAL_TAG;
		def.METAL_PER_ROVER = 300f;
		def.GERMS_PER_ROVER = 9850000L;
		def.MAX_GERMS_TAKEN_PER_PACKAGE = 10000;
		def.GERM_TYPE = MorbRoverMakerConfig.GERM_TYPE;
		def.GERM_INTAKE_CONDUIT_TYPE = this.GERM_INTAKE_CONDUIT_TYPE;
		go.AddOrGetDef<MorbRoverMakerStorytrait.Def>();
		go.AddOrGetDef<MorbRoverMakerDisplay.Def>();
		go.AddOrGet<LoopingSounds>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<AutoDisinfectable>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<Disinfectable>());
	}

	public const string ID = "MorbRoverMaker";

	public const float TUNING_MAX_DESIRED_ROVERS_ALIVE_AT_ONCE = 6f;

	public const int TARGET_AMOUNT_FLOWERS = 10;

	public const float INITIAL_MORB_DEVELOPMENT_PERCENTAGE = 0.5f;

	public static Tag ROVER_PREFAB_ID = "MorbRover";

	public static SimHashes ROVER_MATERIAL_TAG = SimHashes.Steel;

	public const float MATERIAL_MASS_PER_ROVER = 300f;

	public const float ROVER_CRAFTING_DURATION = 15f;

	public const float INPUT_MATERIAL_STORAGE_CAPACITY = 1800f;

	public const int MAX_GERMS_TAKEN_PER_PACKAGE = 10000;

	public const long GERMS_PER_ROVER = 9850000L;

	public static int GERM_TYPE = (int)Db.Get().Diseases.GetIndex("ZombieSpores");

	public ConduitType GERM_INTAKE_CONDUIT_TYPE = ConduitType.Gas;

	public const float PREDICTED_DURATION_TO_GROW_MORB = 985f;
}
