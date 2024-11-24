using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropGravitasSmallSeedLockerConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("PropGravitasSmallSeedLocker", STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSMALLSEEDLOCKER.NAME, STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSMALLSEEDLOCKER.DESC, 50f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("gravitas_medical_locker_kanim"), initialAnim: "on", sceneLayer: Grid.SceneLayer.Building, width: 1, height: 1, permittedRotation: PermittedRotations.R90, orientation: Orientation.Neutral, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		GravitasLocker.Def def = gameObject.AddOrGetDef<GravitasLocker.Def>();
		def.CanBeClosed = false;
		def.SideScreen_OpenButtonText = UI.USERMENUACTIONS.EMPTYSTORAGE.NAME;
		def.SideScreen_OpenButtonTooltip = UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP;
		def.SideScreen_CancelOpenButtonText = UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF;
		def.SideScreen_CancelOpenButtonTooltip = UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF;
		def.SideScreen_CloseButtonText = UI.USERMENUACTIONS.CLOSESTORAGE.NAME;
		def.SideScreen_CloseButtonTooltip = UI.USERMENUACTIONS.CLOSESTORAGE.TOOLTIP;
		def.SideScreen_CancelCloseButtonText = UI.USERMENUACTIONS.CLOSESTORAGE.NAME_OFF;
		def.SideScreen_CancelCloseButtonTooltip = UI.USERMENUACTIONS.CLOSESTORAGE.TOOLTIP_OFF;
		def.ObjectsToSpawn = new string[2] { "EvilFlowerSeed", "EvilFlowerSeed" };
		def.LootSymbols = new string[2] { "seed1", "seed2" };
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("story_trait_morbrover_locker", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.LOCKER.DESCRIPTION));
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		gameObject.AddOrGet<Demolishable>();
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<Workable>().SetOffsets(new CellOffset[2]
		{
			new CellOffset(0, 0),
			CellOffset.down
		});
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
