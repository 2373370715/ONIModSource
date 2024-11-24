using System;
using UnityEngine;

// Token: 0x02000A7E RID: 2686
[AddComponentMenu("KMonoBehaviour/scripts/LiquidSourceManager")]
public class LiquidSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x06003193 RID: 12691 RVA: 0x000C03B4 File Offset: 0x000BE5B4
	protected override void OnPrefabInit()
	{
		LiquidSourceManager.Instance = this;
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x000C03BC File Offset: 0x000BE5BC
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x000BFD7D File Offset: 0x000BDF7D
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x04002152 RID: 8530
	public static LiquidSourceManager Instance;
}
