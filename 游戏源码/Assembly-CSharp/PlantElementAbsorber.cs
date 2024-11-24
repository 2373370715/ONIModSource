using System;

// Token: 0x020016A4 RID: 5796
public struct PlantElementAbsorber
{
	// Token: 0x060077B0 RID: 30640 RVA: 0x000EE916 File Offset: 0x000ECB16
	public void Clear()
	{
		this.storage = null;
		this.consumedElements = null;
	}

	// Token: 0x04005976 RID: 22902
	public Storage storage;

	// Token: 0x04005977 RID: 22903
	public PlantElementAbsorber.LocalInfo localInfo;

	// Token: 0x04005978 RID: 22904
	public HandleVector<int>.Handle[] accumulators;

	// Token: 0x04005979 RID: 22905
	public PlantElementAbsorber.ConsumeInfo[] consumedElements;

	// Token: 0x020016A5 RID: 5797
	public struct ConsumeInfo
	{
		// Token: 0x060077B1 RID: 30641 RVA: 0x000EE926 File Offset: 0x000ECB26
		public ConsumeInfo(Tag tag, float mass_consumption_rate)
		{
			this.tag = tag;
			this.massConsumptionRate = mass_consumption_rate;
		}

		// Token: 0x0400597A RID: 22906
		public Tag tag;

		// Token: 0x0400597B RID: 22907
		public float massConsumptionRate;
	}

	// Token: 0x020016A6 RID: 5798
	public struct LocalInfo
	{
		// Token: 0x0400597C RID: 22908
		public Tag tag;

		// Token: 0x0400597D RID: 22909
		public float massConsumptionRate;
	}
}
