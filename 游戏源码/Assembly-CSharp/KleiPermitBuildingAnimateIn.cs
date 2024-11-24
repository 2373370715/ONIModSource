using UnityEngine;

public class KleiPermitBuildingAnimateIn : MonoBehaviour
{
	private KBatchedAnimController sourceAnimController;

	private KBatchedAnimController placeAnimController;

	private KBatchedAnimController colorAnimController;

	private Updater updater;

	private Updater extraUpdater;

	private void Awake()
	{
		placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		updater = Updater.Parallel(MakeAnimInUpdater(sourceAnimController, placeAnimController, colorAnimController), extraUpdater);
	}

	private void Update()
	{
		sourceAnimController.gameObject.SetActive(value: false);
		updater.Internal_Update(Time.unscaledDeltaTime);
	}

	private void OnDisable()
	{
		sourceAnimController.gameObject.SetActive(value: true);
		Object.Destroy(placeAnimController.gameObject);
		Object.Destroy(colorAnimController.gameObject);
		Object.Destroy(base.gameObject);
	}

	public static KleiPermitBuildingAnimateIn MakeFor(KBatchedAnimController sourceAnimController, Updater extraUpdater = default(Updater))
	{
		sourceAnimController.gameObject.SetActive(value: false);
		KBatchedAnimController kBatchedAnimController = Object.Instantiate(sourceAnimController, sourceAnimController.transform.parent, worldPositionStays: false);
		kBatchedAnimController.gameObject.name = "KleiPermitBuildingAnimateIn.placeAnimController";
		kBatchedAnimController.initialAnim = "place";
		KBatchedAnimController kBatchedAnimController2 = Object.Instantiate(sourceAnimController, sourceAnimController.transform.parent, worldPositionStays: false);
		kBatchedAnimController2.gameObject.name = "KleiPermitBuildingAnimateIn.colorAnimController";
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
		kBatchedAnimController2.initialAnim = anim.name;
		GameObject gameObject = new GameObject("KleiPermitBuildingAnimateIn");
		gameObject.SetActive(value: false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, worldPositionStays: false);
		KleiPermitBuildingAnimateIn kleiPermitBuildingAnimateIn = gameObject.AddComponent<KleiPermitBuildingAnimateIn>();
		kleiPermitBuildingAnimateIn.sourceAnimController = sourceAnimController;
		kleiPermitBuildingAnimateIn.placeAnimController = kBatchedAnimController;
		kleiPermitBuildingAnimateIn.colorAnimController = kBatchedAnimController2;
		kleiPermitBuildingAnimateIn.extraUpdater = ((extraUpdater.fn == null) ? Updater.None() : extraUpdater);
		kBatchedAnimController.gameObject.SetActive(value: true);
		kBatchedAnimController2.gameObject.SetActive(value: true);
		gameObject.SetActive(value: true);
		return kleiPermitBuildingAnimateIn;
	}

	public static Updater MakeAnimInUpdater(KBatchedAnimController sourceAnimController, KBatchedAnimController placeAnimController, KBatchedAnimController colorAnimController)
	{
		return Updater.Parallel(Updater.Series(Updater.Ease(delegate(float alpha)
		{
			placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
		}, 1f, 255f, 0.1f, Easing.CubicOut), Updater.Ease(delegate(float alpha)
		{
			placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(255f - alpha, 1f, 255f));
			colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
		}, 1f, 255f, 0.3f, Easing.CubicIn)), Updater.Series(Updater.Ease(delegate(float scale)
		{
			scale = sourceAnimController.transform.localScale.x * scale;
			placeAnimController.transform.localScale = Vector3.one * scale;
			colorAnimController.transform.localScale = Vector3.one * scale;
		}, 1f, 1.012f, 0.2f, Easing.CubicOut), Updater.Ease(delegate(float scale)
		{
			scale = sourceAnimController.transform.localScale.x * scale;
			placeAnimController.transform.localScale = Vector3.one * scale;
			colorAnimController.transform.localScale = Vector3.one * scale;
		}, 1.012f, 1f, 0.1f, Easing.CubicIn)));
	}
}
