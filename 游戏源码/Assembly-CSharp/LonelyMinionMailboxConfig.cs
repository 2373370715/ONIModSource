using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LonelyMinionMailboxConfig : IBuildingConfig
{
	public const string ID = "LonelyMailBox";

	public static readonly HashedString IdHash = "LonelyMailBox";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("LonelyMailBox", 2, 2, "parcel_delivery_kanim", 10, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER2);
		obj.SceneLayer = Grid.SceneLayer.BuildingBack;
		obj.DefaultAnimState = "idle";
		obj.Floodable = false;
		obj.Overheatable = false;
		obj.ShowInBuildMenu = false;
		obj.ViewMode = OverlayModes.None.ID;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "small";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SingleEntityReceptacle singleEntityReceptacle = go.AddComponent<SingleEntityReceptacle>();
		singleEntityReceptacle.AddDepositTag(GameTags.Edible);
		singleEntityReceptacle.enabled = false;
		go.AddComponent<LonelyMinionMailbox>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Preserve
		});
		Prioritizable.AddRef(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
