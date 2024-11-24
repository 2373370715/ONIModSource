using System;
using UnityEngine;

// Token: 0x02001778 RID: 6008
public class GridCompositor : MonoBehaviour
{
	// Token: 0x06007B8C RID: 31628 RVA: 0x000F1459 File Offset: 0x000EF659
	public static void DestroyInstance()
	{
		GridCompositor.Instance = null;
	}

	// Token: 0x06007B8D RID: 31629 RVA: 0x000F1461 File Offset: 0x000EF661
	private void Awake()
	{
		GridCompositor.Instance = this;
		base.enabled = false;
	}

	// Token: 0x06007B8E RID: 31630 RVA: 0x000F1470 File Offset: 0x000EF670
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/GridCompositor"));
	}

	// Token: 0x06007B8F RID: 31631 RVA: 0x000F1487 File Offset: 0x000EF687
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x06007B90 RID: 31632 RVA: 0x000F1496 File Offset: 0x000EF696
	public void ToggleMajor(bool on)
	{
		this.onMajor = on;
		this.Refresh();
	}

	// Token: 0x06007B91 RID: 31633 RVA: 0x000F14A5 File Offset: 0x000EF6A5
	public void ToggleMinor(bool on)
	{
		this.onMinor = on;
		this.Refresh();
	}

	// Token: 0x06007B92 RID: 31634 RVA: 0x000F14B4 File Offset: 0x000EF6B4
	private void Refresh()
	{
		base.enabled = (this.onMinor || this.onMajor);
	}

	// Token: 0x04005CA9 RID: 23721
	public Material material;

	// Token: 0x04005CAA RID: 23722
	public static GridCompositor Instance;

	// Token: 0x04005CAB RID: 23723
	private bool onMajor;

	// Token: 0x04005CAC RID: 23724
	private bool onMinor;
}
