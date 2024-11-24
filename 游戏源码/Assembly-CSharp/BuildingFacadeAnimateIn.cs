using UnityEngine;

public class BuildingFacadeAnimateIn : MonoBehaviour
{
	private KBatchedAnimController sourceAnimController;

	private KBatchedAnimController placeAnimController;

	private KBatchedAnimController colorAnimController;

	private Updater updater;

	private void Awake()
	{
		placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		updater = Updater.Series(KleiPermitBuildingAnimateIn.MakeAnimInUpdater(sourceAnimController, placeAnimController, colorAnimController), Updater.Do(delegate
		{
			Object.Destroy(base.gameObject);
		}));
	}

	private void Update()
	{
		if (sourceAnimController.IsNullOrDestroyed())
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SetVisibilityOn(sourceAnimController, isVisible: false);
		updater.Internal_Update(Time.unscaledDeltaTime);
	}

	private void OnDisable()
	{
		if (!sourceAnimController.IsNullOrDestroyed())
		{
			SetVisibilityOn(sourceAnimController, isVisible: true);
		}
		Object.Destroy(placeAnimController.gameObject);
		Object.Destroy(colorAnimController.gameObject);
		Object.Destroy(base.gameObject);
	}

	public static BuildingFacadeAnimateIn MakeFor(KBatchedAnimController sourceAnimController)
	{
		SetVisibilityOn(sourceAnimController, isVisible: false);
		KBatchedAnimController kBatchedAnimController = SpawnAnimFrom(sourceAnimController);
		kBatchedAnimController.gameObject.name = "BuildingFacadeAnimateIn.placeAnimController";
		kBatchedAnimController.initialAnim = "place";
		KBatchedAnimController kBatchedAnimController2 = SpawnAnimFrom(sourceAnimController);
		kBatchedAnimController2.gameObject.name = "BuildingFacadeAnimateIn.colorAnimController";
		kBatchedAnimController2.initialAnim = ((sourceAnimController.CurrentAnim != null) ? sourceAnimController.CurrentAnim.name : sourceAnimController.AnimFiles[0].GetData().GetAnim(0).name);
		GameObject gameObject = new GameObject("BuildingFacadeAnimateIn");
		gameObject.SetActive(value: false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, worldPositionStays: false);
		BuildingFacadeAnimateIn buildingFacadeAnimateIn = gameObject.AddComponent<BuildingFacadeAnimateIn>();
		buildingFacadeAnimateIn.sourceAnimController = sourceAnimController;
		buildingFacadeAnimateIn.placeAnimController = kBatchedAnimController;
		buildingFacadeAnimateIn.colorAnimController = kBatchedAnimController2;
		kBatchedAnimController.gameObject.SetActive(value: true);
		kBatchedAnimController2.gameObject.SetActive(value: true);
		gameObject.SetActive(value: true);
		return buildingFacadeAnimateIn;
	}

	private static void SetVisibilityOn(KBatchedAnimController animController, bool isVisible)
	{
		animController.SetVisiblity(isVisible);
		KBatchedAnimController[] componentsInChildren = animController.GetComponentsInChildren<KBatchedAnimController>(includeInactive: true);
		foreach (KBatchedAnimController kBatchedAnimController in componentsInChildren)
		{
			if (kBatchedAnimController.batchGroupID == animController.batchGroupID)
			{
				kBatchedAnimController.SetVisiblity(isVisible);
			}
		}
	}

	private static KBatchedAnimController SpawnAnimFrom(KBatchedAnimController sourceAnimController)
	{
		GameObject obj = new GameObject();
		obj.SetActive(value: false);
		obj.transform.SetParent(sourceAnimController.transform.parent, worldPositionStays: false);
		obj.transform.localPosition = sourceAnimController.transform.localPosition;
		obj.transform.localRotation = sourceAnimController.transform.localRotation;
		obj.transform.localScale = sourceAnimController.transform.localScale;
		obj.layer = sourceAnimController.gameObject.layer;
		KBatchedAnimController kBatchedAnimController = obj.AddComponent<KBatchedAnimController>();
		kBatchedAnimController.materialType = sourceAnimController.materialType;
		kBatchedAnimController.initialMode = sourceAnimController.initialMode;
		kBatchedAnimController.AnimFiles = sourceAnimController.AnimFiles;
		kBatchedAnimController.Offset = sourceAnimController.Offset;
		kBatchedAnimController.animWidth = sourceAnimController.animWidth;
		kBatchedAnimController.animHeight = sourceAnimController.animHeight;
		kBatchedAnimController.animScale = sourceAnimController.animScale;
		kBatchedAnimController.sceneLayer = sourceAnimController.sceneLayer;
		kBatchedAnimController.fgLayer = sourceAnimController.fgLayer;
		kBatchedAnimController.FlipX = sourceAnimController.FlipX;
		kBatchedAnimController.FlipY = sourceAnimController.FlipY;
		kBatchedAnimController.Rotation = sourceAnimController.Rotation;
		kBatchedAnimController.Pivot = sourceAnimController.Pivot;
		return kBatchedAnimController;
	}
}
