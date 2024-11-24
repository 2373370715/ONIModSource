using System;
using UnityEngine;

// Token: 0x02001D52 RID: 7506
public class KleiPermitBuildingAnimateIn : MonoBehaviour
{
	// Token: 0x06009CCD RID: 40141 RVA: 0x003C5768 File Offset: 0x003C3968
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Parallel(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			this.extraUpdater
		});
	}

	// Token: 0x06009CCE RID: 40142 RVA: 0x001060B2 File Offset: 0x001042B2
	private void Update()
	{
		this.sourceAnimController.gameObject.SetActive(false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06009CCF RID: 40143 RVA: 0x001060D6 File Offset: 0x001042D6
	private void OnDisable()
	{
		this.sourceAnimController.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06009CD0 RID: 40144 RVA: 0x003C57F4 File Offset: 0x003C39F4
	public static KleiPermitBuildingAnimateIn MakeFor(KBatchedAnimController sourceAnimController, Updater extraUpdater = default(Updater))
	{
		sourceAnimController.gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController.gameObject.name = "KleiPermitBuildingAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController2.gameObject.name = "KleiPermitBuildingAnimateIn.colorAnimController";
		KAnimFileData data = sourceAnimController.AnimFiles[0].GetData();
		KAnim.Anim anim = data.GetAnim("idle");
		if (anim == null)
		{
			anim = data.GetAnim("off");
			if (anim == null)
			{
				anim = data.GetAnim(0);
			}
		}
		kbatchedAnimController2.initialAnim = anim.name;
		GameObject gameObject = new GameObject("KleiPermitBuildingAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		KleiPermitBuildingAnimateIn kleiPermitBuildingAnimateIn = gameObject.AddComponent<KleiPermitBuildingAnimateIn>();
		kleiPermitBuildingAnimateIn.sourceAnimController = sourceAnimController;
		kleiPermitBuildingAnimateIn.placeAnimController = kbatchedAnimController;
		kleiPermitBuildingAnimateIn.colorAnimController = kbatchedAnimController2;
		kleiPermitBuildingAnimateIn.extraUpdater = ((extraUpdater.fn == null) ? Updater.None() : extraUpdater);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return kleiPermitBuildingAnimateIn;
	}

	// Token: 0x06009CD1 RID: 40145 RVA: 0x003C591C File Offset: 0x003C3B1C
	public static Updater MakeAnimInUpdater(KBatchedAnimController sourceAnimController, KBatchedAnimController placeAnimController, KBatchedAnimController colorAnimController)
	{
		return Updater.Parallel(new Updater[]
		{
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.1f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(255f - alpha, 1f, 255f));
					colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.3f, Easing.CubicIn, -1f)
			}),
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1f, 1.012f, 0.2f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1.012f, 1f, 0.1f, Easing.CubicIn, -1f)
			})
		});
	}

	// Token: 0x04007AE1 RID: 31457
	private KBatchedAnimController sourceAnimController;

	// Token: 0x04007AE2 RID: 31458
	private KBatchedAnimController placeAnimController;

	// Token: 0x04007AE3 RID: 31459
	private KBatchedAnimController colorAnimController;

	// Token: 0x04007AE4 RID: 31460
	private Updater updater;

	// Token: 0x04007AE5 RID: 31461
	private Updater extraUpdater;
}
