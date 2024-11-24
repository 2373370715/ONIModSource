using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200023D RID: 573
[EntityConfigOrder(2)]
public class BabyHatchVeggieConfig : IEntityConfig
{
	// Token: 0x060007FA RID: 2042 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x000A9CD5 File Offset: 0x000A7ED5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchVeggieConfig.CreateHatch("HatchVeggieBaby", CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchVeggie", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000603 RID: 1539
	public const string ID = "HatchVeggieBaby";
}
