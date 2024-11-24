using System;

// Token: 0x02000FC1 RID: 4033
public class StorageMeter : KMonoBehaviour
{
	// Token: 0x060051A9 RID: 20905 RVA: 0x000D5336 File Offset: 0x000D3536
	public void SetInterpolateFunction(Func<float, int, float> func)
	{
		this.interpolateFunction = func;
		if (this.meter != null)
		{
			this.meter.interpolateFunction = this.interpolateFunction;
		}
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x0027264C File Offset: 0x0027084C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_frame",
			"meter_level"
		});
		this.meter.interpolateFunction = this.interpolateFunction;
		this.UpdateMeter(null);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x000D5358 File Offset: 0x000D3558
	private void UpdateMeter(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x04003920 RID: 14624
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003921 RID: 14625
	private MeterController meter;

	// Token: 0x04003922 RID: 14626
	private Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);
}
