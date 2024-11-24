using System;
using UnityEngine;

// Token: 0x02001B30 RID: 6960
public class LogicModeUI : ScriptableObject
{
	// Token: 0x04006E4F RID: 28239
	[Header("Base Assets")]
	public Sprite inputSprite;

	// Token: 0x04006E50 RID: 28240
	public Sprite outputSprite;

	// Token: 0x04006E51 RID: 28241
	public Sprite resetSprite;

	// Token: 0x04006E52 RID: 28242
	public GameObject prefab;

	// Token: 0x04006E53 RID: 28243
	public GameObject ribbonInputPrefab;

	// Token: 0x04006E54 RID: 28244
	public GameObject ribbonOutputPrefab;

	// Token: 0x04006E55 RID: 28245
	public GameObject controlInputPrefab;

	// Token: 0x04006E56 RID: 28246
	[Header("Colouring")]
	public Color32 colourOn = new Color32(0, byte.MaxValue, 0, 0);

	// Token: 0x04006E57 RID: 28247
	public Color32 colourOff = new Color32(byte.MaxValue, 0, 0, 0);

	// Token: 0x04006E58 RID: 28248
	public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04006E59 RID: 28249
	public Color32 colourOnProtanopia = new Color32(179, 204, 0, 0);

	// Token: 0x04006E5A RID: 28250
	public Color32 colourOffProtanopia = new Color32(166, 51, 102, 0);

	// Token: 0x04006E5B RID: 28251
	public Color32 colourOnDeuteranopia = new Color32(128, 0, 128, 0);

	// Token: 0x04006E5C RID: 28252
	public Color32 colourOffDeuteranopia = new Color32(byte.MaxValue, 153, 0, 0);

	// Token: 0x04006E5D RID: 28253
	public Color32 colourOnTritanopia = new Color32(51, 102, byte.MaxValue, 0);

	// Token: 0x04006E5E RID: 28254
	public Color32 colourOffTritanopia = new Color32(byte.MaxValue, 153, 0, 0);
}
