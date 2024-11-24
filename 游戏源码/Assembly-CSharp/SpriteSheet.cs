using System;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
[Serializable]
public struct SpriteSheet
{
	// Token: 0x04002409 RID: 9225
	public string name;

	// Token: 0x0400240A RID: 9226
	public int numFrames;

	// Token: 0x0400240B RID: 9227
	public int numXFrames;

	// Token: 0x0400240C RID: 9228
	public Vector2 uvFrameSize;

	// Token: 0x0400240D RID: 9229
	public int renderLayer;

	// Token: 0x0400240E RID: 9230
	public Material material;

	// Token: 0x0400240F RID: 9231
	public Texture2D texture;
}
