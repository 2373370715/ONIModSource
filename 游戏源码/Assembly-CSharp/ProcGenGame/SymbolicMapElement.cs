using System;

namespace ProcGenGame
{
	// Token: 0x0200209A RID: 8346
	public interface SymbolicMapElement
	{
		// Token: 0x0600B153 RID: 45395
		void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd);
	}
}
