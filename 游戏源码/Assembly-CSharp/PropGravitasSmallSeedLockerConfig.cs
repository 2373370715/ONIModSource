using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class PropGravitasSmallSeedLockerConfig : IEntityConfig
{
	// Token: 0x060018C5 RID: 6341 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x001A0AF8 File Offset: 0x0019ECF8
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasSmallSeedLocker";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSMALLSEEDLOCKER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSMALLSEEDLOCKER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_medical_locker_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
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
		def.ObjectsToSpawn = new string[]
		{
			"EvilFlowerSeed",
			"EvilFlowerSeed"
		};
		def.LootSymbols = new string[]
		{
			"seed1",
			"seed2"
		};
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("story_trait_morbrover_locker", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.LOCKER.DESCRIPTION));
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x000B06FE File Offset: 0x000AE8FE
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<Workable>().SetOffsets(new CellOffset[]
		{
			new CellOffset(0, 0),
			CellOffset.down
		});
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}
}
