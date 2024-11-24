using System;
using UnityEngine;

// Token: 0x02001300 RID: 4864
[AddComponentMenu("KMonoBehaviour/scripts/FlowOffsetRenderer")]
public class FlowOffsetRenderer : KMonoBehaviour
{
	// Token: 0x060063D9 RID: 25561 RVA: 0x002BD000 File Offset: 0x002BB200
	protected override void OnSpawn()
	{
		this.FlowMaterial = new Material(Shader.Find("Klei/Flow"));
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.DoUpdate(0.1f);
	}

	// Token: 0x060063DA RID: 25562 RVA: 0x002BD05C File Offset: 0x002BB25C
	private void OnResize()
	{
		for (int i = 0; i < this.OffsetTextures.Length; i++)
		{
			if (this.OffsetTextures[i] != null)
			{
				this.OffsetTextures[i].DestroyRenderTexture();
			}
			this.OffsetTextures[i] = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
			this.OffsetTextures[i].filterMode = FilterMode.Bilinear;
			this.OffsetTextures[i].name = "FlowOffsetTexture";
		}
	}

	// Token: 0x060063DB RID: 25563 RVA: 0x002BD0D8 File Offset: 0x002BB2D8
	private void LateUpdate()
	{
		if ((Time.deltaTime > 0f && Time.timeScale > 0f) || this.forceUpdate)
		{
			float num = Time.deltaTime / Time.timeScale;
			this.DoUpdate(num * Time.timeScale / 4f + num * 0.5f);
		}
	}

	// Token: 0x060063DC RID: 25564 RVA: 0x002BD12C File Offset: 0x002BB32C
	private void DoUpdate(float dt)
	{
		this.CurrentTime += dt;
		float num = this.CurrentTime * this.PhaseMultiplier;
		num -= (float)((int)num);
		float num2 = num - (float)((int)num);
		float y = 1f;
		if (num2 <= this.GasPhase0)
		{
			y = 0f;
		}
		this.GasPhase0 = num2;
		float z = 1f;
		float num3 = num + 0.5f - (float)((int)(num + 0.5f));
		if (num3 <= this.GasPhase1)
		{
			z = 0f;
		}
		this.GasPhase1 = num3;
		Shader.SetGlobalVector(this.ParametersName, new Vector4(this.GasPhase0, 0f, 0f, 0f));
		Shader.SetGlobalVector("_NoiseParameters", new Vector4(this.NoiseInfluence, this.NoiseScale, 0f, 0f));
		RenderTexture renderTexture = this.OffsetTextures[this.OffsetIdx];
		this.OffsetIdx = (this.OffsetIdx + 1) % 2;
		RenderTexture renderTexture2 = this.OffsetTextures[this.OffsetIdx];
		Material flowMaterial = this.FlowMaterial;
		flowMaterial.SetTexture("_PreviousOffsetTex", renderTexture);
		flowMaterial.SetVector("_FlowParameters", new Vector4(Time.deltaTime * this.OffsetSpeed, y, z, 0f));
		flowMaterial.SetVector("_MinFlow", new Vector4(this.MinFlow0.x, this.MinFlow0.y, this.MinFlow1.x, this.MinFlow1.y));
		flowMaterial.SetVector("_VisibleArea", new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells));
		flowMaterial.SetVector("_LiquidGasMask", new Vector4(this.LiquidGasMask.x, this.LiquidGasMask.y, 0f, 0f));
		Graphics.Blit(renderTexture, renderTexture2, flowMaterial);
		Shader.SetGlobalTexture(this.OffsetTextureName, renderTexture2);
	}

	// Token: 0x04004759 RID: 18265
	private float GasPhase0;

	// Token: 0x0400475A RID: 18266
	private float GasPhase1;

	// Token: 0x0400475B RID: 18267
	public float PhaseMultiplier;

	// Token: 0x0400475C RID: 18268
	public float NoiseInfluence;

	// Token: 0x0400475D RID: 18269
	public float NoiseScale;

	// Token: 0x0400475E RID: 18270
	public float OffsetSpeed;

	// Token: 0x0400475F RID: 18271
	public string OffsetTextureName;

	// Token: 0x04004760 RID: 18272
	public string ParametersName;

	// Token: 0x04004761 RID: 18273
	public Vector2 MinFlow0;

	// Token: 0x04004762 RID: 18274
	public Vector2 MinFlow1;

	// Token: 0x04004763 RID: 18275
	public Vector2 LiquidGasMask;

	// Token: 0x04004764 RID: 18276
	[SerializeField]
	private Material FlowMaterial;

	// Token: 0x04004765 RID: 18277
	[SerializeField]
	private bool forceUpdate;

	// Token: 0x04004766 RID: 18278
	private TextureLerper FlowLerper;

	// Token: 0x04004767 RID: 18279
	public RenderTexture[] OffsetTextures = new RenderTexture[2];

	// Token: 0x04004768 RID: 18280
	private int OffsetIdx;

	// Token: 0x04004769 RID: 18281
	private float CurrentTime;
}
