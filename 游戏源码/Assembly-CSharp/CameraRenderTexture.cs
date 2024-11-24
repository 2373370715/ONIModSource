using System;
using UnityEngine;

// Token: 0x0200176F RID: 5999
public class CameraRenderTexture : MonoBehaviour
{
	// Token: 0x06007B6C RID: 31596 RVA: 0x000F12C8 File Offset: 0x000EF4C8
	private void Awake()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
	}

	// Token: 0x06007B6D RID: 31597 RVA: 0x000F12DF File Offset: 0x000EF4DF
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.OnResize();
	}

	// Token: 0x06007B6E RID: 31598 RVA: 0x0031B594 File Offset: 0x00319794
	private void OnResize()
	{
		if (this.resultTexture != null)
		{
			this.resultTexture.DestroyRenderTexture();
		}
		this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		this.resultTexture.name = base.name;
		this.resultTexture.filterMode = FilterMode.Point;
		this.resultTexture.autoGenerateMips = false;
		if (this.TextureName != "")
		{
			Shader.SetGlobalTexture(this.TextureName, this.resultTexture);
		}
	}

	// Token: 0x06007B6F RID: 31599 RVA: 0x000F131A File Offset: 0x000EF51A
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.resultTexture, this.material);
	}

	// Token: 0x06007B70 RID: 31600 RVA: 0x000F132E File Offset: 0x000EF52E
	public RenderTexture GetTexture()
	{
		return this.resultTexture;
	}

	// Token: 0x06007B71 RID: 31601 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool ShouldFlip()
	{
		return false;
	}

	// Token: 0x04005C8D RID: 23693
	public string TextureName;

	// Token: 0x04005C8E RID: 23694
	private RenderTexture resultTexture;

	// Token: 0x04005C8F RID: 23695
	private Material material;
}
