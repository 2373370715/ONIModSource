using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200177C RID: 6012
public class LightBuffer : MonoBehaviour
{
	// Token: 0x06007BC3 RID: 31683 RVA: 0x0031CC20 File Offset: 0x0031AE20
	private void Awake()
	{
		LightBuffer.Instance = this;
		this.ColorRangeTag = Shader.PropertyToID("_ColorRange");
		this.LightPosTag = Shader.PropertyToID("_LightPos");
		this.LightDirectionAngleTag = Shader.PropertyToID("_LightDirectionAngle");
		this.TintColorTag = Shader.PropertyToID("_TintColor");
		this.Camera = base.GetComponent<Camera>();
		this.Layer = LayerMask.NameToLayer("Lights");
		this.Mesh = new Mesh();
		this.Mesh.name = "Light Mesh";
		this.Mesh.vertices = new Vector3[]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f)
		};
		this.Mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};
		this.Mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
		this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
		this.Texture.name = "LightBuffer";
		this.Camera.targetTexture = this.Texture;
	}

	// Token: 0x06007BC4 RID: 31684 RVA: 0x0031CE10 File Offset: 0x0031B010
	private void LateUpdate()
	{
		if (PropertyTextures.instance == null)
		{
			return;
		}
		if (this.Texture.width != Screen.width || this.Texture.height != Screen.height)
		{
			this.Texture.DestroyRenderTexture();
			this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
			this.Texture.name = "LightBuffer";
			this.Camera.targetTexture = this.Texture;
		}
		Matrix4x4 matrix = default(Matrix4x4);
		this.WorldLight = PropertyTextures.instance.GetTexture(PropertyTextures.Property.WorldLight);
		this.Material.SetTexture("_PropertyWorldLight", this.WorldLight);
		this.CircleMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
		this.ConeMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
		List<Light2D> list = Components.Light2Ds.Items;
		if (ClusterManager.Instance != null)
		{
			list = Components.Light2Ds.GetWorldItems(ClusterManager.Instance.activeWorldId, false);
		}
		if (list == null)
		{
			return;
		}
		foreach (Light2D light2D in list)
		{
			if (!(light2D == null) && light2D.enabled)
			{
				MaterialPropertyBlock materialPropertyBlock = light2D.materialPropertyBlock;
				materialPropertyBlock.SetVector(this.ColorRangeTag, new Vector4(light2D.Color.r * light2D.IntensityAnimation, light2D.Color.g * light2D.IntensityAnimation, light2D.Color.b * light2D.IntensityAnimation, light2D.Range));
				Vector3 position = light2D.transform.GetPosition();
				position.x += light2D.Offset.x;
				position.y += light2D.Offset.y;
				materialPropertyBlock.SetVector(this.LightPosTag, new Vector4(position.x, position.y, 0f, 0f));
				Vector2 normalized = light2D.Direction.normalized;
				materialPropertyBlock.SetVector(this.LightDirectionAngleTag, new Vector4(normalized.x, normalized.y, 0f, light2D.Angle));
				Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, materialPropertyBlock, false, false);
				if (light2D.drawOverlay)
				{
					materialPropertyBlock.SetColor(this.TintColorTag, light2D.overlayColour);
					global::LightShape shape = light2D.shape;
					if (shape != global::LightShape.Circle)
					{
						if (shape == global::LightShape.Cone)
						{
							matrix.SetTRS(position - Vector3.up * (light2D.Range * 0.5f), Quaternion.identity, new Vector3(1f, 0.5f, 1f) * light2D.Range);
							Graphics.DrawMesh(this.Mesh, matrix, this.ConeMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
						}
					}
					else
					{
						matrix.SetTRS(position, Quaternion.identity, Vector3.one * light2D.Range);
						Graphics.DrawMesh(this.Mesh, matrix, this.CircleMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
					}
				}
			}
		}
	}

	// Token: 0x06007BC5 RID: 31685 RVA: 0x000F1865 File Offset: 0x000EFA65
	private void OnDestroy()
	{
		LightBuffer.Instance = null;
	}

	// Token: 0x04005CC3 RID: 23747
	private Mesh Mesh;

	// Token: 0x04005CC4 RID: 23748
	private Camera Camera;

	// Token: 0x04005CC5 RID: 23749
	[NonSerialized]
	public Material Material;

	// Token: 0x04005CC6 RID: 23750
	[NonSerialized]
	public Material CircleMaterial;

	// Token: 0x04005CC7 RID: 23751
	[NonSerialized]
	public Material ConeMaterial;

	// Token: 0x04005CC8 RID: 23752
	private int ColorRangeTag;

	// Token: 0x04005CC9 RID: 23753
	private int LightPosTag;

	// Token: 0x04005CCA RID: 23754
	private int LightDirectionAngleTag;

	// Token: 0x04005CCB RID: 23755
	private int TintColorTag;

	// Token: 0x04005CCC RID: 23756
	private int Layer;

	// Token: 0x04005CCD RID: 23757
	public RenderTexture Texture;

	// Token: 0x04005CCE RID: 23758
	public Texture WorldLight;

	// Token: 0x04005CCF RID: 23759
	public static LightBuffer Instance;

	// Token: 0x04005CD0 RID: 23760
	private const RenderTextureFormat RTFormat = RenderTextureFormat.ARGBHalf;
}
