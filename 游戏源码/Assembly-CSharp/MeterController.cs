using System;
using UnityEngine;

// Token: 0x02000EAB RID: 3755
public class MeterController
{
	// Token: 0x06004BA7 RID: 19367 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public static float StandardLerp(float percentage, int frames)
	{
		return percentage;
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x000D0D72 File Offset: 0x000CEF72
	public static float MinMaxStepLerp(float percentage, int frames)
	{
		if ((double)percentage <= 0.0 || frames <= 1)
		{
			return 0f;
		}
		if ((double)percentage >= 1.0 || frames == 2)
		{
			return 1f;
		}
		return (1f + percentage * (float)(frames - 2)) / (float)frames;
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x000D0DB1 File Offset: 0x000CEFB1
	// (set) Token: 0x06004BAA RID: 19370 RVA: 0x000D0DB9 File Offset: 0x000CEFB9
	public KBatchedAnimController meterController { get; private set; }

	// Token: 0x06004BAB RID: 19371 RVA: 0x0025F4D4 File Offset: 0x0025D6D4
	public MeterController(KMonoBehaviour target, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		string[] array = new string[symbols_to_hide.Length + 1];
		Array.Copy(symbols_to_hide, array, symbols_to_hide.Length);
		array[array.Length - 1] = "meter_target";
		KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
		this.Initialize(component, "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, array);
	}

	// Token: 0x06004BAC RID: 19372 RVA: 0x000D0DC2 File Offset: 0x000CEFC2
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
	}

	// Token: 0x06004BAD RID: 19373 RVA: 0x000D0DF0 File Offset: 0x000CEFF0
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x0025F540 File Offset: 0x0025D740
	private void Initialize(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		if (building_controller.HasAnimation(meter_animation + "_cb") && !GlobalAssets.Instance.colorSet.IsDefaultColorSet())
		{
			meter_animation += "_cb";
		}
		string name = building_controller.name + "." + meter_animation;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(MeterConfig.ID));
		gameObject.name = name;
		gameObject.SetActive(false);
		gameObject.transform.parent = building_controller.transform;
		this.gameObject = gameObject;
		gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
		Vector3 position = building_controller.transform.GetPosition();
		switch (front_back)
		{
		case Meter.Offset.Infront:
			position.z -= 0.1f;
			break;
		case Meter.Offset.Behind:
			position.z += 0.1f;
			break;
		case Meter.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(user_specified_render_layer);
			break;
		}
		gameObject.transform.SetPosition(position);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.AnimFiles = new KAnimFile[]
		{
			building_controller.AnimFiles[0]
		};
		component.initialAnim = meter_animation;
		component.fgLayer = Grid.SceneLayer.NoLayer;
		component.initialMode = KAnim.PlayMode.Paused;
		component.isMovable = true;
		component.FlipX = building_controller.FlipX;
		component.FlipY = building_controller.FlipY;
		if (Meter.Offset.UserSpecified == front_back)
		{
			component.sceneLayer = user_specified_render_layer;
		}
		this.meterController = component;
		KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
		component2.offset = tracker_offset;
		component2.symbol = new HashedString(meter_target);
		gameObject.SetActive(true);
		building_controller.SetSymbolVisiblity(meter_target, false);
		if (symbols_to_hide != null)
		{
			for (int i = 0; i < symbols_to_hide.Length; i++)
			{
				building_controller.SetSymbolVisiblity(symbols_to_hide[i], false);
			}
		}
		this.link = new KAnimLink(building_controller, component);
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x0025F710 File Offset: 0x0025D910
	public MeterController(KAnimControllerBase building_controller, KBatchedAnimController meter_controller, params string[] symbol_names)
	{
		if (meter_controller == null)
		{
			return;
		}
		this.meterController = meter_controller;
		this.link = new KAnimLink(building_controller, meter_controller);
		for (int i = 0; i < symbol_names.Length; i++)
		{
			building_controller.SetSymbolVisiblity(symbol_names[i], false);
		}
		this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
	}

	// Token: 0x06004BB0 RID: 19376 RVA: 0x000D0E1B File Offset: 0x000CF01B
	public void SetPositionPercent(float percent_full)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.SetPositionPercent(this.interpolateFunction(percent_full, this.meterController.GetCurrentNumFrames()));
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x000D0E4E File Offset: 0x000CF04E
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.meterController != null)
		{
			this.meterController.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x000D0E70 File Offset: 0x000CF070
	public void SetRotation(float rot)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.Rotation = rot;
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x000D0E8D File Offset: 0x000CF08D
	public void Unlink()
	{
		if (this.link != null)
		{
			this.link.Unregister();
			this.link = null;
		}
	}

	// Token: 0x04003475 RID: 13429
	public GameObject gameObject;

	// Token: 0x04003476 RID: 13430
	public Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);

	// Token: 0x04003477 RID: 13431
	private KAnimLink link;
}
