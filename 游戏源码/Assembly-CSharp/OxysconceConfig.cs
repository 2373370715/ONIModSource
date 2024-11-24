using TUNING;
using UnityEngine;

public class OxysconceConfig : IBuildingConfig
{
	public const string ID = "Oxysconce";

	private const float OXYLITE_STORAGE = 240f;

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("Oxysconce", 1, 1, "oxy_sconce_kanim", 10, 3f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.ALL_METALS, 800f, BuildLocationRule.Anywhere, noise: NOISE_POLLUTION.NOISY.TIER0, decor: BUILDINGS.DECOR.BONUS.TIER0);
		obj.RequiresPowerInput = false;
		obj.ExhaustKilowattsWhenActive = 0f;
		obj.SelfHeatKilowattsWhenActive = 0f;
		obj.ViewMode = OverlayModes.Oxygen.ID;
		obj.AudioCategory = "HollowMetal";
		obj.Breakable = true;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		new CellOffset(0, 0);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 240f;
		storage.showInUI = true;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.OxyRock.CreateTag();
		manualDeliveryKG.capacity = 240f;
		manualDeliveryKG.refillMass = 96f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<StorageMeter>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			Tutorial.Instance.oxygenGenerators.Add(game_object);
		};
	}
}
