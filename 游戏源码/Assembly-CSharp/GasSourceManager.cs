using System;
using UnityEngine;

// Token: 0x02000A64 RID: 2660
[AddComponentMenu("KMonoBehaviour/scripts/GasSourceManager")]
public class GasSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x060030FE RID: 12542 RVA: 0x000BFD5F File Offset: 0x000BDF5F
	protected override void OnPrefabInit()
	{
		GasSourceManager.Instance = this;
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x000BFD67 File Offset: 0x000BDF67
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x000BFD7D File Offset: 0x000BDF7D
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x04002117 RID: 8471
	public static GasSourceManager Instance;
}
