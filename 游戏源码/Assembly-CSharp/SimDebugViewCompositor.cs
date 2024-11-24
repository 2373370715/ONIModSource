using System;
using UnityEngine;

// Token: 0x02001790 RID: 6032
public class SimDebugViewCompositor : MonoBehaviour
{
	// Token: 0x06007C16 RID: 31766 RVA: 0x000F1C21 File Offset: 0x000EFE21
	private void Awake()
	{
		SimDebugViewCompositor.Instance = this;
	}

	// Token: 0x06007C17 RID: 31767 RVA: 0x000F1C29 File Offset: 0x000EFE29
	private void OnDestroy()
	{
		SimDebugViewCompositor.Instance = null;
	}

	// Token: 0x06007C18 RID: 31768 RVA: 0x000F1C31 File Offset: 0x000EFE31
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
		this.Toggle(false);
	}

	// Token: 0x06007C19 RID: 31769 RVA: 0x000F1C4F File Offset: 0x000EFE4F
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen.Instance.RunPostProcessEffects(src, dest);
		}
	}

	// Token: 0x06007C1A RID: 31770 RVA: 0x000F1C77 File Offset: 0x000EFE77
	public void Toggle(bool is_on)
	{
		base.enabled = is_on;
	}

	// Token: 0x04005DD1 RID: 24017
	public Material material;

	// Token: 0x04005DD2 RID: 24018
	public static SimDebugViewCompositor Instance;
}
