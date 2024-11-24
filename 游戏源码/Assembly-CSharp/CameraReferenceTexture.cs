using System;
using UnityEngine;

// Token: 0x0200176E RID: 5998
public class CameraReferenceTexture : MonoBehaviour
{
	// Token: 0x06007B6A RID: 31594 RVA: 0x0031B530 File Offset: 0x00319730
	private void OnPreCull()
	{
		if (this.quad == null)
		{
			this.quad = new FullScreenQuad("CameraReferenceTexture", base.GetComponent<Camera>(), this.referenceCamera.GetComponent<CameraRenderTexture>().ShouldFlip());
		}
		if (this.referenceCamera != null)
		{
			this.quad.Draw(this.referenceCamera.GetComponent<CameraRenderTexture>().GetTexture());
		}
	}

	// Token: 0x04005C8B RID: 23691
	public Camera referenceCamera;

	// Token: 0x04005C8C RID: 23692
	private FullScreenQuad quad;
}
