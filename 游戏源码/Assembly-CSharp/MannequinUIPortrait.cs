using UnityEngine;
using UnityEngine.UI;

public class MannequinUIPortrait : IEntityConfig
{
	public static string ID = "MannequinUIPortrait";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ID, ID);
		RectTransform rectTransform = gameObject.AddOrGet<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		LayoutElement layoutElement = gameObject.AddOrGet<LayoutElement>();
		layoutElement.preferredHeight = 100f;
		layoutElement.preferredWidth = 100f;
		gameObject.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
		gameObject.AddOrGet<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kBatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kBatchedAnimController.animScale = 0.5f;
		kBatchedAnimController.setScaleFromAnim = false;
		kBatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
		kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim("mannequin_kanim") };
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		MinionConfig.ConfigureSymbols(gameObject, show_defaults: false);
		return gameObject;
	}

	public void OnPrefabInit(GameObject go)
	{
	}

	public void OnSpawn(GameObject go)
	{
	}
}
