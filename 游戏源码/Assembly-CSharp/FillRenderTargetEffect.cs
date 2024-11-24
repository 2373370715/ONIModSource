using System;
using UnityEngine;

// Token: 0x02001A8A RID: 6794
public class FillRenderTargetEffect : MonoBehaviour
{
	// Token: 0x06008E04 RID: 36356 RVA: 0x000FCBE8 File Offset: 0x000FADE8
	public void SetFillTexture(Texture tex)
	{
		this.fillTexture = tex;
	}

	// Token: 0x06008E05 RID: 36357 RVA: 0x000FCBF1 File Offset: 0x000FADF1
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(this.fillTexture, null);
	}

	// Token: 0x04006ABE RID: 27326
	private Texture fillTexture;
}
