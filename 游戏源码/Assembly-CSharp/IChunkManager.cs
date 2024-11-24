using System;
using UnityEngine;

// Token: 0x02000B29 RID: 2857
public interface IChunkManager
{
	// Token: 0x06003658 RID: 13912
	SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);

	// Token: 0x06003659 RID: 13913
	SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);
}
