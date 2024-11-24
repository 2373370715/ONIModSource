using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class KeepsakeConfig : IMultiEntityConfig
{
	public delegate void PostInitFn(GameObject gameObject);

	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> obj = new List<GameObject>
		{
			CreateKeepsake("MegaBrain", UI.KEEPSAKES.MEGA_BRAIN.NAME, UI.KEEPSAKES.MEGA_BRAIN.DESCRIPTION, "keepsake_mega_brain_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS),
			CreateKeepsake("CritterManipulator", UI.KEEPSAKES.CRITTER_MANIPULATOR.NAME, UI.KEEPSAKES.CRITTER_MANIPULATOR.DESCRIPTION, "keepsake_critter_manipulator_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS),
			CreateKeepsake("LonelyMinion", UI.KEEPSAKES.LONELY_MINION.NAME, UI.KEEPSAKES.LONELY_MINION.DESCRIPTION, "keepsake_lonelyminion_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS),
			CreateKeepsake("FossilHunt", UI.KEEPSAKES.FOSSIL_HUNT.NAME, UI.KEEPSAKES.FOSSIL_HUNT.DESCRIPTION, "keepsake_fossil_dig_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS),
			CreateKeepsake("GeothermalPlant", UI.KEEPSAKES.GEOTHERMAL_PLANT.NAME, UI.KEEPSAKES.GEOTHERMAL_PLANT.DESCRIPTION, "keepsake_geothermal_vent_kanim", "idle", "ui", DlcManager.AVAILABLE_DLC_2)
		};
		GameObject gameObject = CreateKeepsake("MorbRoverMaker", UI.KEEPSAKES.MORB_ROVER_MAKER.NAME, UI.KEEPSAKES.MORB_ROVER_MAKER.DESCRIPTION, "keepsake_morb_tank_kanim", "idle", "ui", DlcManager.AVAILABLE_ALL_VERSIONS);
		gameObject.AddOrGetDef<MorbRoverMakerKeepsake.Def>();
		obj.Add(gameObject);
		obj.RemoveAll((GameObject x) => x == null);
		return obj;
	}

	public static GameObject CreateKeepsake(string id, string name, string desc, string animFile, string initial_anim = "idle", string ui_anim = "ui", string[] dlcIDs = null, PostInitFn postInitFn = null, SimHashes element = SimHashes.Creature)
	{
		if (dlcIDs == null)
		{
			dlcIDs = DlcManager.AVAILABLE_ALL_VERSIONS;
		}
		if (!DlcManager.IsDlcListValidForCurrentContent(dlcIDs))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity("keepsake_" + id.ToLower(), name, desc, 25f, unitMass: true, Assets.GetAnim(animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, isPickupable: true, SORTORDER.KEEPSAKES, element, new List<Tag> { GameTags.MiscPickupable });
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.GetProperName();
		gameObject.AddOrGet<KSelectable>();
		gameObject.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
		postInitFn?.Invoke(gameObject);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable);
		component.AddTag(GameTags.Keepsake);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
