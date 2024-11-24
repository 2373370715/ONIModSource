using System;
using UnityEngine;

// Token: 0x02000C12 RID: 3090
[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000C6041 File Offset: 0x000C4241
	// (set) Token: 0x06003AED RID: 15085 RVA: 0x000C6048 File Offset: 0x000C4248
	public static AmbientSoundManager Instance { get; private set; }

	// Token: 0x06003AEE RID: 15086 RVA: 0x000C6050 File Offset: 0x000C4250
	public static void Destroy()
	{
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x000C6058 File Offset: 0x000C4258
	protected override void OnPrefabInit()
	{
		AmbientSoundManager.Instance = this;
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x000C6060 File Offset: 0x000C4260
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x04002835 RID: 10293
	[MyCmpAdd]
	private LoopingSounds loopingSounds;
}
