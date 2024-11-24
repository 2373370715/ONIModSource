using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200204D RID: 8269
public class VirtualCursorOverlayFix : MonoBehaviour
{
	// Token: 0x0600B012 RID: 45074 RVA: 0x00423110 File Offset: 0x00421310
	private void Awake()
	{
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		this.cursorRendTex = new RenderTexture(width, height, 0);
		this.screenSpaceCamera.enabled = true;
		this.screenSpaceCamera.targetTexture = this.cursorRendTex;
		this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
		base.StartCoroutine(this.RenderVirtualCursor());
	}

	// Token: 0x0600B013 RID: 45075 RVA: 0x001126E5 File Offset: 0x001108E5
	private IEnumerator RenderVirtualCursor()
	{
		bool ShowCursor = KInputManager.currentControllerIsGamepad;
		while (Application.isPlaying)
		{
			ShowCursor = KInputManager.currentControllerIsGamepad;
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C))
			{
				ShowCursor = true;
			}
			this.screenSpaceCamera.enabled = true;
			if (!this.screenSpaceOverlayImage.enabled && ShowCursor)
			{
				yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
			}
			this.actualCursor.enabled = ShowCursor;
			this.screenSpaceOverlayImage.enabled = ShowCursor;
			this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04008AC7 RID: 35527
	private RenderTexture cursorRendTex;

	// Token: 0x04008AC8 RID: 35528
	public Camera screenSpaceCamera;

	// Token: 0x04008AC9 RID: 35529
	public Image screenSpaceOverlayImage;

	// Token: 0x04008ACA RID: 35530
	public RawImage actualCursor;
}
