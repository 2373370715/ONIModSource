using System;
using UnityEngine;

// Token: 0x02000CB8 RID: 3256
public class BuildingFacadeAnimateIn : MonoBehaviour
{
	// Token: 0x06003F0A RID: 16138 RVA: 0x002361BC File Offset: 0x002343BC
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Series(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			Updater.Do(delegate()
			{
				UnityEngine.Object.Destroy(base.gameObject);
			})
		});
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x000C8F6E File Offset: 0x000C716E
	private void Update()
	{
		if (this.sourceAnimController.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x00236250 File Offset: 0x00234450
	private void OnDisable()
	{
		if (!this.sourceAnimController.IsNullOrDestroyed())
		{
			BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, true);
		}
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x002362A4 File Offset: 0x002344A4
	public static BuildingFacadeAnimateIn MakeFor(KBatchedAnimController sourceAnimController)
	{
		BuildingFacadeAnimateIn.SetVisibilityOn(sourceAnimController, false);
		KBatchedAnimController kbatchedAnimController = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController.gameObject.name = "BuildingFacadeAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController2.gameObject.name = "BuildingFacadeAnimateIn.colorAnimController";
		kbatchedAnimController2.initialAnim = ((sourceAnimController.CurrentAnim != null) ? sourceAnimController.CurrentAnim.name : sourceAnimController.AnimFiles[0].GetData().GetAnim(0).name);
		GameObject gameObject = new GameObject("BuildingFacadeAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		BuildingFacadeAnimateIn buildingFacadeAnimateIn = gameObject.AddComponent<BuildingFacadeAnimateIn>();
		buildingFacadeAnimateIn.sourceAnimController = sourceAnimController;
		buildingFacadeAnimateIn.placeAnimController = kbatchedAnimController;
		buildingFacadeAnimateIn.colorAnimController = kbatchedAnimController2;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return buildingFacadeAnimateIn;
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x00236388 File Offset: 0x00234588
	private static void SetVisibilityOn(KBatchedAnimController animController, bool isVisible)
	{
		animController.SetVisiblity(isVisible);
		foreach (KBatchedAnimController kbatchedAnimController in animController.GetComponentsInChildren<KBatchedAnimController>(true))
		{
			if (kbatchedAnimController.batchGroupID == animController.batchGroupID)
			{
				kbatchedAnimController.SetVisiblity(isVisible);
			}
		}
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x002363D0 File Offset: 0x002345D0
	private static KBatchedAnimController SpawnAnimFrom(KBatchedAnimController sourceAnimController)
	{
		GameObject gameObject = new GameObject();
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		gameObject.transform.localPosition = sourceAnimController.transform.localPosition;
		gameObject.transform.localRotation = sourceAnimController.transform.localRotation;
		gameObject.transform.localScale = sourceAnimController.transform.localScale;
		gameObject.layer = sourceAnimController.gameObject.layer;
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.materialType = sourceAnimController.materialType;
		kbatchedAnimController.initialMode = sourceAnimController.initialMode;
		kbatchedAnimController.AnimFiles = sourceAnimController.AnimFiles;
		kbatchedAnimController.Offset = sourceAnimController.Offset;
		kbatchedAnimController.animWidth = sourceAnimController.animWidth;
		kbatchedAnimController.animHeight = sourceAnimController.animHeight;
		kbatchedAnimController.animScale = sourceAnimController.animScale;
		kbatchedAnimController.sceneLayer = sourceAnimController.sceneLayer;
		kbatchedAnimController.fgLayer = sourceAnimController.fgLayer;
		kbatchedAnimController.FlipX = sourceAnimController.FlipX;
		kbatchedAnimController.FlipY = sourceAnimController.FlipY;
		kbatchedAnimController.Rotation = sourceAnimController.Rotation;
		kbatchedAnimController.Pivot = sourceAnimController.Pivot;
		return kbatchedAnimController;
	}

	// Token: 0x04002AFB RID: 11003
	private KBatchedAnimController sourceAnimController;

	// Token: 0x04002AFC RID: 11004
	private KBatchedAnimController placeAnimController;

	// Token: 0x04002AFD RID: 11005
	private KBatchedAnimController colorAnimController;

	// Token: 0x04002AFE RID: 11006
	private Updater updater;
}
