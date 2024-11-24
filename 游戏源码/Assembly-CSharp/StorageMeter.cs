using System;

public class StorageMeter : KMonoBehaviour
{
	[MyCmpGet]
	private Storage storage;

	private MeterController meter;

	private Func<float, int, float> interpolateFunction = MeterController.MinMaxStepLerp;

	public void SetInterpolateFunction(Func<float, int, float> func)
	{
		interpolateFunction = func;
		if (meter != null)
		{
			meter.interpolateFunction = interpolateFunction;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_target", "meter_frame", "meter_level");
		meter.interpolateFunction = interpolateFunction;
		UpdateMeter(null);
		Subscribe(-1697596308, UpdateMeter);
	}

	private void UpdateMeter(object data)
	{
		meter.SetPositionPercent(storage.MassStored() / storage.Capacity());
	}
}
