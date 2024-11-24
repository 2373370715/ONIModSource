using System;
using UnityEngine;

// Token: 0x0200176D RID: 5997
public static class Blur
{
	// Token: 0x06007B69 RID: 31593 RVA: 0x000F12A4 File Offset: 0x000EF4A4
	public static RenderTexture Run(Texture2D image)
	{
		if (Blur.blurMaterial == null)
		{
			Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		}
		return null;
	}

	// Token: 0x04005C8A RID: 23690
	private static Material blurMaterial;
}
