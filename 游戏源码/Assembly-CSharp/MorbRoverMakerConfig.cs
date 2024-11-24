using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MorbRoverMakerConfig : IBuildingConfig
{
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

	public static int GERM_TYPE = Db.Get().Diseases.GetIndex("ZombieSpores");

	public ConduitType GERM_INTAKE_CONDUIT_TYPE = ConduitType.Gas;

	public const float PREDICTED_DURATION_TO_GROW_MORB = 985f;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MorbRoverMaker", 5, 4, "gravitas_morb_tank_kanim", 250, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.REFINED_METALS, 3200f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.BONUS.TIER1);
		obj.Floodable = true;
		obj.Entombable = true;
		obj.ShowInBuildMenu = false;
		obj.Overheatable = false;
		obj.ObjectLayer = ObjectLayer.Building;
		obj.SceneLayer = Grid.SceneLayer.Building;
		obj.AudioCategory = "Glass";
		obj.AudioSize = "medium";
		obj.UseStructureTemperature = false;
		obj.InputConduitType = GERM_INTAKE_CONDUIT_TYPE;
		obj.OutputConduitType = GERM_INTAKE_CONDUIT_TYPE;
		obj.UtilityInputOffset = new CellOffset(1, 0);
		obj.UtilityOutputOffset = new CellOffset(2, 3);
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel);
		component.Temperature = 294.15f;
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = ((GERM_INTAKE_CONDUIT_TYPE == ConduitType.Gas) ? new List<Tag>(STORAGEFILTERS.GASES) : new List<Tag>(STORAGEFILTERS.LIQUIDS));
		storage.storageFilters.Add(ROVER_MATERIAL_TAG.CreateTag());
		storage.allowItemRemoval = false;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = ROVER_MATERIAL_TAG.CreateTag();
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
		def.ROVER_PREFAB_ID = ROVER_PREFAB_ID;
		def.ROVER_CRAFTING_DURATION = 15f;
		def.ROVER_MATERIAL = ROVER_MATERIAL_TAG;
		def.METAL_PER_ROVER = 300f;
		def.GERMS_PER_ROVER = 9850000L;
		def.MAX_GERMS_TAKEN_PER_PACKAGE = 10000;
		def.GERM_TYPE = GERM_TYPE;
		def.GERM_INTAKE_CONDUIT_TYPE = GERM_INTAKE_CONDUIT_TYPE;
		go.AddOrGetDef<MorbRoverMakerStorytrait.Def>();
		go.AddOrGetDef<MorbRoverMakerDisplay.Def>();
		go.AddOrGet<LoopingSounds>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		Object.DestroyImmediate(go.GetComponent<AutoDisinfectable>());
		Object.DestroyImmediate(go.GetComponent<Disinfectable>());
	}
}
