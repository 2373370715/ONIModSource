using System;

// Token: 0x0200186B RID: 6251
public class SimData
{
	// Token: 0x040061E6 RID: 25062
	public unsafe Sim.EmittedMassInfo* emittedMassEntries;

	// Token: 0x040061E7 RID: 25063
	public unsafe Sim.ElementChunkInfo* elementChunks;

	// Token: 0x040061E8 RID: 25064
	public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

	// Token: 0x040061E9 RID: 25065
	public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

	// Token: 0x040061EA RID: 25066
	public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;
}
