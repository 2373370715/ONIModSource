using System;

// Token: 0x0200136F RID: 4975
public interface IEnergyProducer
{
	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06006623 RID: 26147
	float JoulesAvailable { get; }

	// Token: 0x06006624 RID: 26148
	void ConsumeEnergy(float joules);
}
