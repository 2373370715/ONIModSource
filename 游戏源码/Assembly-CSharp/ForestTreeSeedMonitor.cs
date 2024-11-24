using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200029C RID: 668
public class ForestTreeSeedMonitor : KMonoBehaviour
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x000AAA27 File Offset: 0x000A8C27
	public bool ExtraSeedAvailable
	{
		get
		{
			return this.hasExtraSeedAvailable;
		}
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00168D14 File Offset: 0x00166F14
	public void ExtractExtraSeed()
	{
		if (!this.hasExtraSeedAvailable)
		{
			return;
		}
		this.hasExtraSeedAvailable = false;
		Vector3 position = base.transform.position;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		Util.KInstantiate(Assets.GetPrefab("ForestTreeSeed"), position).SetActive(true);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x000AAA2F File Offset: 0x000A8C2F
	public void TryRollNewSeed()
	{
		if (!this.hasExtraSeedAvailable && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.hasExtraSeedAvailable = true;
		}
	}

	// Token: 0x04000780 RID: 1920
	[Serialize]
	private bool hasExtraSeedAvailable;
}
