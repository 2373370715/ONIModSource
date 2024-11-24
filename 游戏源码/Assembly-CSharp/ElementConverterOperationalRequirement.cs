using System;
using KSerialization;

// Token: 0x02001289 RID: 4745
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverterOperationalRequirement : KMonoBehaviour
{
	// Token: 0x06006162 RID: 24930 RVA: 0x000DF856 File Offset: 0x000DDA56
	private void onStorageChanged(object _)
	{
		this.operational.SetFlag(this.sufficientResources, this.converter.HasEnoughMassToStartConverting(false));
	}

	// Token: 0x06006163 RID: 24931 RVA: 0x000DF875 File Offset: 0x000DDA75
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sufficientResources = new Operational.Flag("sufficientResources", this.operationalReq);
		base.Subscribe(-1697596308, new Action<object>(this.onStorageChanged));
		this.onStorageChanged(null);
	}

	// Token: 0x04004562 RID: 17762
	[MyCmpReq]
	private ElementConverter converter;

	// Token: 0x04004563 RID: 17763
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04004564 RID: 17764
	private Operational.Flag.Type operationalReq;

	// Token: 0x04004565 RID: 17765
	private Operational.Flag sufficientResources;
}
