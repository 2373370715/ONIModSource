using System;
using Unity.Collections;
using UnityEngine;

// Token: 0x02001A84 RID: 6788
public class SkyVisibilityVisualizerEffect : MonoBehaviour
{
	// Token: 0x06008DF2 RID: 36338 RVA: 0x000FCB4F File Offset: 0x000FAD4F
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/SkyVisibility"));
	}

	// Token: 0x06008DF3 RID: 36339 RVA: 0x0036F000 File Offset: 0x0036D200
	private void OnPostRender()
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = null;
		Vector2I u = new Vector2I(0, 0);
		if (SelectTool.Instance.selected != null)
		{
			Grid.PosToXY(SelectTool.Instance.selected.transform.GetPosition(), out u.x, out u.y);
			skyVisibilityVisualizer = SelectTool.Instance.selected.GetComponent<SkyVisibilityVisualizer>();
		}
		if (skyVisibilityVisualizer == null && BuildTool.Instance.visualizer != null)
		{
			Grid.PosToXY(BuildTool.Instance.visualizer.transform.GetPosition(), out u.x, out u.y);
			skyVisibilityVisualizer = BuildTool.Instance.visualizer.GetComponent<SkyVisibilityVisualizer>();
		}
		if (skyVisibilityVisualizer != null)
		{
			if (skyVisibilityVisualizer.SkipOnModuleInteriors && ClusterManager.Instance != null)
			{
				WorldContainer myWorld = skyVisibilityVisualizer.GetMyWorld();
				if (myWorld != null && myWorld.IsModuleInterior)
				{
					return;
				}
			}
			if (this.OcclusionTex == null)
			{
				this.OcclusionTex = new Texture2D(64, 1, TextureFormat.RGFloat, false);
				this.OcclusionTex.filterMode = FilterMode.Point;
				this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
			}
			Vector2I vector2I;
			Vector2I vector2I2;
			this.FindWorldBounds(out vector2I, out vector2I2);
			int rangeMin = skyVisibilityVisualizer.RangeMin;
			int rangeMax = skyVisibilityVisualizer.RangeMax;
			Vector2I originOffset = skyVisibilityVisualizer.OriginOffset;
			Vector2I vector2I3 = u + originOffset;
			NativeArray<float> pixelData = this.OcclusionTex.GetPixelData<float>(0);
			int num = 0;
			bool flag = true;
			int num2 = vector2I3.x + rangeMin;
			int num3 = vector2I3.x + rangeMax;
			bool flag2 = true;
			for (int i = vector2I3.x; i >= num2; i--)
			{
				int num4 = vector2I3.y + (vector2I3.x - i) * skyVisibilityVisualizer.ScanVerticalStep;
				int arg = Grid.XYToCell(i, num4);
				flag2 &= (i > vector2I.x && i < vector2I2.x && num4 > vector2I.y && num4 < vector2I2.y && skyVisibilityVisualizer.SkyVisibilityCb(arg));
				int num5 = i - num2;
				if (!skyVisibilityVisualizer.AllOrNothingVisibility)
				{
					pixelData[2 * num5] = (float)(flag2 ? 1 : 0);
				}
				pixelData[2 * num5 + 1] = (float)(num4 + 1);
				if (flag2)
				{
					num++;
				}
			}
			flag = (flag && flag2);
			Vector2I vector2I4 = vector2I3;
			if (skyVisibilityVisualizer.TwoWideOrgin)
			{
				vector2I4.x++;
			}
			flag2 = true;
			for (int j = vector2I4.x; j <= num3; j++)
			{
				int num6 = vector2I4.y + (j - vector2I4.x) * skyVisibilityVisualizer.ScanVerticalStep;
				int arg2 = Grid.XYToCell(j, num6);
				flag2 &= (j > vector2I.x && j < vector2I2.x && num6 > vector2I.y && num6 < vector2I2.y && skyVisibilityVisualizer.SkyVisibilityCb(arg2));
				int num7 = j - num2;
				if (!skyVisibilityVisualizer.AllOrNothingVisibility)
				{
					pixelData[2 * num7] = (float)(flag2 ? 1 : 0);
				}
				pixelData[2 * num7 + 1] = (float)(num6 + 1);
				if (flag2)
				{
					num++;
				}
			}
			flag = (flag && flag2);
			if (skyVisibilityVisualizer.AllOrNothingVisibility)
			{
				for (int k = 0; k <= rangeMax - rangeMin; k++)
				{
					pixelData[2 * k] = (float)(flag ? 1 : 0);
				}
			}
			this.OcclusionTex.Apply(false, false);
			Vector2I vector2I5 = vector2I3 + new Vector2I(rangeMin, 0);
			Vector2I vector2I6 = new Vector2I(vector2I3.x + rangeMax, vector2I2.y);
			if (this.myCamera == null)
			{
				this.myCamera = base.GetComponent<Camera>();
				if (this.myCamera == null)
				{
					return;
				}
			}
			Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
			float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
			Vector3 point = ray.GetPoint(distance);
			Vector4 vector;
			vector.x = point.x;
			vector.y = point.y;
			ray = this.myCamera.ViewportPointToRay(Vector3.one);
			distance = Mathf.Abs(ray.origin.z / ray.direction.z);
			point = ray.GetPoint(distance);
			vector.z = point.x - vector.x;
			vector.w = point.y - vector.y;
			this.material.SetVector("_UVOffsetScale", vector);
			Vector4 value;
			value.x = (float)vector2I5.x;
			value.y = (float)vector2I5.y;
			value.z = (float)(vector2I6.x + 1);
			value.w = (float)(vector2I6.y + 1);
			this.material.SetVector("_RangeParams", value);
			this.material.SetColor("_HighlightColor", this.highlightColor);
			this.material.SetColor("_HighlightColor2", this.highlightColor2);
			Vector4 value2;
			value2.x = 1f / (float)this.OcclusionTex.width;
			value2.y = 1f / (float)this.OcclusionTex.height;
			value2.z = 0f;
			value2.w = 0f;
			this.material.SetVector("_OcclusionParams", value2);
			this.material.SetTexture("_OcclusionTex", this.OcclusionTex);
			Vector4 value3;
			value3.x = (float)Grid.WidthInCells;
			value3.y = (float)Grid.HeightInCells;
			value3.z = 1f / (float)Grid.WidthInCells;
			value3.w = 1f / (float)Grid.HeightInCells;
			this.material.SetVector("_WorldParams", value3);
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(5);
			GL.Color(Color.white);
			GL.Vertex3(0f, 0f, 0f);
			GL.Vertex3(0f, 1f, 0f);
			GL.Vertex3(1f, 0f, 0f);
			GL.Vertex3(1f, 1f, 0f);
			GL.End();
			GL.PopMatrix();
			if (this.LastVisibleColumnCount != num)
			{
				SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement", false), skyVisibilityVisualizer.transform.GetPosition(), 1f);
				this.LastVisibleColumnCount = num;
			}
		}
	}

	// Token: 0x06008DF4 RID: 36340 RVA: 0x0036E250 File Offset: 0x0036C450
	private void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			world_min = activeWorld.WorldOffset;
			world_max = activeWorld.WorldOffset + activeWorld.WorldSize;
			return;
		}
		world_min.x = 0;
		world_min.y = 0;
		world_max.x = Grid.WidthInCells;
		world_max.y = Grid.HeightInCells;
	}

	// Token: 0x04006AA2 RID: 27298
	private Material material;

	// Token: 0x04006AA3 RID: 27299
	private Camera myCamera;

	// Token: 0x04006AA4 RID: 27300
	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	// Token: 0x04006AA5 RID: 27301
	public Color highlightColor2 = new Color(1f, 0.32f, 0f, 1f);

	// Token: 0x04006AA6 RID: 27302
	private Texture2D OcclusionTex;

	// Token: 0x04006AA7 RID: 27303
	private int LastVisibleColumnCount;
}
