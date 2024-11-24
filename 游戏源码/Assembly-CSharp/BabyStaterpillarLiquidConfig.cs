using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabyStaterpillarLiquidConfig : IEntityConfig
{
	public const string ID = "StaterpillarLiquidBaby";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.DESC, "baby_caterpillar_kanim", is_baby: true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid");
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", is_visible: false);
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
