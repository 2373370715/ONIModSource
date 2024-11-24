using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200039A RID: 922
public class MachinePartsConfig : IEntityConfig
{
	// Token: 0x06000F2E RID: 3886 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0017BF60 File Offset: 0x0017A160
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("MachineParts", ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim("buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AF4 RID: 2804
	public const string ID = "MachineParts";

	// Token: 0x04000AF5 RID: 2805
	public static readonly Tag TAG = TagManager.Create("MachineParts");

	// Token: 0x04000AF6 RID: 2806
	public const float MASS = 5f;
}
