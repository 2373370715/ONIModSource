using System;
using UnityEngine;

// Token: 0x020017B3 RID: 6067
[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
	// Token: 0x06007CF9 RID: 31993 RVA: 0x00323A9C File Offset: 0x00321C9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_fill",
			"meter_OL"
		});
		base.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
		this.OnStorageChange(null);
	}

	// Token: 0x06007CFA RID: 31994 RVA: 0x000F2477 File Offset: 0x000F0677
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x04005E8D RID: 24205
	private MeterController meter;

	// Token: 0x04005E8E RID: 24206
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005E8F RID: 24207
	private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>(delegate(Reservoir component, object data)
	{
		component.OnStorageChange(data);
	});
}
