using System;
using UnityEngine;

// Token: 0x02001A89 RID: 6793
public class BloomEffect : MonoBehaviour
{
	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x06008DFD RID: 36349 RVA: 0x000FCB80 File Offset: 0x000FAD80
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.blurShader);
				this.m_Material.hideFlags = HideFlags.DontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06008DFE RID: 36350 RVA: 0x000FCBB4 File Offset: 0x000FADB4
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06008DFF RID: 36351 RVA: 0x0036F88C File Offset: 0x0036DA8C
	protected void Start()
	{
		if (!this.blurShader || !this.material.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.BloomMaskMaterial = new Material(Shader.Find("Klei/PostFX/BloomMask"));
		this.BloomCompositeMaterial = new Material(Shader.Find("Klei/PostFX/BloomComposite"));
	}

	// Token: 0x06008E00 RID: 36352 RVA: 0x0036F8EC File Offset: 0x0036DAEC
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06008E01 RID: 36353 RVA: 0x0036F958 File Offset: 0x0036DB58
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06008E02 RID: 36354 RVA: 0x0036F9BC File Offset: 0x0036DBBC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		temporary.name = "bloom_source";
		Graphics.Blit(source, temporary, this.BloomMaskMaterial);
		int width = Math.Max(source.width / 4, 4);
		int height = Math.Max(source.height / 4, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
		renderTexture.name = "bloom_downsampled";
		this.DownSample4x(temporary, renderTexture);
		RenderTexture.ReleaseTemporary(temporary);
		for (int i = 0; i < this.iterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
			temporary2.name = "bloom_blurred";
			this.FourTapCone(renderTexture, temporary2, i);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary2;
		}
		this.BloomCompositeMaterial.SetTexture("_BloomTex", renderTexture);
		Graphics.Blit(source, destination, this.BloomCompositeMaterial);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04006AB8 RID: 27320
	private Material BloomMaskMaterial;

	// Token: 0x04006AB9 RID: 27321
	private Material BloomCompositeMaterial;

	// Token: 0x04006ABA RID: 27322
	public int iterations = 3;

	// Token: 0x04006ABB RID: 27323
	public float blurSpread = 0.6f;

	// Token: 0x04006ABC RID: 27324
	public Shader blurShader;

	// Token: 0x04006ABD RID: 27325
	private Material m_Material;
}
