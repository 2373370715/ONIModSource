using STRINGS;
using UnityEngine;

[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
	public const string ID = "Pacu";

	public const string BASE_TRAIT_ID = "PacuBaseTrait";

	public const string EGG_ID = "PacuEgg";

	public const int EGG_SORT_ORDER = 500;

	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		return EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f, 253.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, add_fixed_capturable_monitor: false);
	}

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(CreatePacu("Pacu", CREATURES.SPECIES.PACU.NAME, CREATURES.SPECIES.PACU.DESC, "pacu_kanim", is_baby: false), "PacuEgg", CREATURES.SPECIES.PACU.EGG_NAME, CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, GetDlcIds(), 500, is_ranchable: false, add_fish_overcrowding_monitor: true, add_fixed_capturable_monitor: false, 0.75f);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
