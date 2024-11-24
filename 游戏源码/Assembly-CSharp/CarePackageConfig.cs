using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class CarePackageConfig : IEntityConfig
{
	// Token: 0x060012F4 RID: 4852 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0018C328 File Offset: 0x0018A528
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, ITEMS.CARGO_CAPSULE.NAME, ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim("portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, 0, SimHashes.Creature, null);
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x000AE945 File Offset: 0x000ACB45
	public void OnPrefabInit(GameObject go)
	{
		go.AddOrGet<CarePackage>();
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D0E RID: 3342
	public static readonly string ID = "CarePackage";
}
