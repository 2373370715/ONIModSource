using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000260 RID: 608
[EntityConfigOrder(1)]
public class PacuTropicalConfig : IEntityConfig
{
	// Token: 0x060008C6 RID: 2246 RVA: 0x00163BC4 File Offset: 0x00161DC4
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuTropicalBaseTrait", name, desc, anim_file, is_baby, "trp_", 303.15f, 353.15f, 283.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
		gameObject.AddOrGet<DecorProvider>().SetValues(PacuTropicalConfig.DECOR);
		return gameObject;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00163C18 File Offset: 0x00161E18
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuTropicalConfig.CreatePacu("PacuTropical", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), "PacuTropicalEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuTropicalBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_TROPICAL, this.GetDlcIds(), 502, false, true, false, 0.75f, false);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400068D RID: 1677
	public const string ID = "PacuTropical";

	// Token: 0x0400068E RID: 1678
	public const string BASE_TRAIT_ID = "PacuTropicalBaseTrait";

	// Token: 0x0400068F RID: 1679
	public const string EGG_ID = "PacuTropicalEgg";

	// Token: 0x04000690 RID: 1680
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x04000691 RID: 1681
	public const int EGG_SORT_ORDER = 502;
}
