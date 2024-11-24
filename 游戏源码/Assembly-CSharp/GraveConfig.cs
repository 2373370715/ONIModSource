using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class GraveConfig : IBuildingConfig
{
	// Token: 0x06000E4D RID: 3661 RVA: 0x00178604 File Offset: 0x00176804
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Grave";
		int width = 1;
		int height = 2;
		string anim = "gravestone_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		return buildingDef;
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00178670 File Offset: 0x00176870
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GraveConfig.STORAGE_OVERRIDE_ANIM_FILES = new KAnimFile[]
		{
			Assets.GetAnim("anim_bury_dupe_kanim")
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(GraveConfig.StorageModifiers);
		storage.overrideAnims = GraveConfig.STORAGE_OVERRIDE_ANIM_FILES;
		storage.workAnims = GraveConfig.STORAGE_WORK_ANIMS;
		storage.workingPstComplete = new HashedString[]
		{
			GraveConfig.STORAGE_PST_ANIM
		};
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		go.AddOrGet<Grave>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A5F RID: 2655
	public const string ID = "Grave";

	// Token: 0x04000A60 RID: 2656
	public const string AnimFile = "gravestone_kanim";

	// Token: 0x04000A61 RID: 2657
	private static KAnimFile[] STORAGE_OVERRIDE_ANIM_FILES;

	// Token: 0x04000A62 RID: 2658
	private static readonly HashedString[] STORAGE_WORK_ANIMS = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x04000A63 RID: 2659
	private static readonly HashedString STORAGE_PST_ANIM = HashedString.Invalid;

	// Token: 0x04000A64 RID: 2660
	private static readonly List<Storage.StoredItemModifier> StorageModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};
}
