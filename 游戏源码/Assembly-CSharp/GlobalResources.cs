using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02001A93 RID: 6803
public class GlobalResources : ScriptableObject
{
	// Token: 0x06008E3C RID: 36412 RVA: 0x000FCE48 File Offset: 0x000FB048
	public static GlobalResources Instance()
	{
		if (GlobalResources._Instance == null)
		{
			GlobalResources._Instance = Resources.Load<GlobalResources>("GlobalResources");
		}
		return GlobalResources._Instance;
	}

	// Token: 0x04006B21 RID: 27425
	public Material AnimMaterial;

	// Token: 0x04006B22 RID: 27426
	public Material AnimUIMaterial;

	// Token: 0x04006B23 RID: 27427
	public Material AnimPlaceMaterial;

	// Token: 0x04006B24 RID: 27428
	public Material AnimMaterialUIDesaturated;

	// Token: 0x04006B25 RID: 27429
	public Material AnimSimpleMaterial;

	// Token: 0x04006B26 RID: 27430
	public Material AnimOverlayMaterial;

	// Token: 0x04006B27 RID: 27431
	public Texture2D WhiteTexture;

	// Token: 0x04006B28 RID: 27432
	public EventReference ConduitOverlaySoundLiquid;

	// Token: 0x04006B29 RID: 27433
	public EventReference ConduitOverlaySoundGas;

	// Token: 0x04006B2A RID: 27434
	public EventReference ConduitOverlaySoundSolid;

	// Token: 0x04006B2B RID: 27435
	public EventReference AcousticDisturbanceSound;

	// Token: 0x04006B2C RID: 27436
	public EventReference AcousticDisturbanceBubbleSound;

	// Token: 0x04006B2D RID: 27437
	public EventReference WallDamageLayerSound;

	// Token: 0x04006B2E RID: 27438
	public Sprite sadDupeAudio;

	// Token: 0x04006B2F RID: 27439
	public Sprite sadDupe;

	// Token: 0x04006B30 RID: 27440
	public Sprite baseGameLogoSmall;

	// Token: 0x04006B31 RID: 27441
	public Sprite expansion1LogoSmall;

	// Token: 0x04006B32 RID: 27442
	private static GlobalResources _Instance;
}
