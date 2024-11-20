using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ScannerNetworkVisualizerEffect : VisualizerEffect
{
	protected override void SetupMaterial()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/ScannerNetwork"));
	}

	protected override void SetupOcclusionTex()
	{
		this.OcclusionTex = new Texture2D(512, 1, TextureFormat.RGFloat, false);
		this.OcclusionTex.filterMode = FilterMode.Point;
		this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
	}

	protected override void OnPostRender()
	{
		ScannerNetworkVisualizer scannerNetworkVisualizer = null;
		if (SelectTool.Instance.selected != null)
		{
			scannerNetworkVisualizer = SelectTool.Instance.selected.GetComponent<ScannerNetworkVisualizer>();
		}
		if (scannerNetworkVisualizer == null && BuildTool.Instance.visualizer != null)
		{
			scannerNetworkVisualizer = BuildTool.Instance.visualizer.GetComponent<ScannerNetworkVisualizer>();
		}
		if (scannerNetworkVisualizer != null)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			ScannerNetworkVisualizerEffect.FindWorldBounds(out vector2I, out vector2I2);
			if (vector2I2.x - vector2I.x > this.OcclusionTex.width)
			{
				return;
			}
			NativeArray<float> pixelData = this.OcclusionTex.GetPixelData<float>(0);
			for (int i = 0; i < this.OcclusionTex.width; i++)
			{
				pixelData[2 * i] = 0f;
				pixelData[2 * i + 1] = 0f;
			}
			int num = 0;
			List<ScannerNetworkVisualizer> items = Components.ScannerVisualizers.GetItems(scannerNetworkVisualizer.GetMyWorldId());
			for (int j = 0; j < items.Count; j++)
			{
				ScannerNetworkVisualizer scannerNetworkVisualizer2 = items[j];
				if (scannerNetworkVisualizer2 != scannerNetworkVisualizer)
				{
					ScannerNetworkVisualizerEffect.ComputeVisibility(scannerNetworkVisualizer2, pixelData, vector2I, vector2I2, ref num);
				}
			}
			ScannerNetworkVisualizerEffect.ComputeVisibility(scannerNetworkVisualizer, pixelData, vector2I, vector2I2, ref num);
			this.OcclusionTex.Apply(false, false);
			Vector2I vector2I3 = vector2I;
			Vector2I vector2I4 = vector2I2;
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
			value.x = (float)vector2I3.x;
			value.y = (float)vector2I3.y;
			value.z = (float)vector2I4.x;
			value.w = (float)vector2I4.y;
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
				SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement", false), scannerNetworkVisualizer.transform.GetPosition(), 1f);
				this.LastVisibleColumnCount = num;
			}
		}
	}

	private static void ComputeVisibility(ScannerNetworkVisualizer scan, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max, ref int visible_column_count)
	{
		Vector2I u = Grid.PosToXY(scan.transform.GetPosition());
		int rangeMin = scan.RangeMin;
		int rangeMax = scan.RangeMax;
		Vector2I vector2I = u + scan.OriginOffset;
		bool flag = true;
		for (int i = 0; i >= rangeMin; i--)
		{
			int x_abs = vector2I.x + i;
			int y_abs = vector2I.y + Mathf.Abs(i);
			ScannerNetworkVisualizerEffect.ComputeVisibility(x_abs, y_abs, pixels, world_min, world_max, ref flag);
			if (flag)
			{
				visible_column_count++;
			}
		}
		flag = true;
		for (int j = 0; j <= rangeMax; j++)
		{
			int x_abs2 = vector2I.x + j;
			int y_abs2 = vector2I.y + Mathf.Abs(j);
			ScannerNetworkVisualizerEffect.ComputeVisibility(x_abs2, y_abs2, pixels, world_min, world_max, ref flag);
			if (flag)
			{
				visible_column_count++;
			}
		}
	}

	private static void ComputeVisibility(int x_abs, int y_abs, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max, ref bool visible)
	{
		int num = x_abs - world_min.x;
		if (x_abs < world_min.x || x_abs > world_max.x || y_abs < world_min.y || y_abs >= world_max.y)
		{
			return;
		}
		int cell = Grid.XYToCell(x_abs, y_abs);
		visible &= ScannerNetworkVisualizerEffect.HasSkyVisibility(cell);
		if (pixels[2 * num] == 2f)
		{
			if (visible)
			{
				pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], (float)(y_abs + 1));
			}
			return;
		}
		pixels[2 * num] = (float)(visible ? 2 : 1);
		if (pixels[2 * num] == 1f && pixels[2 * num + 1] != 0f)
		{
			pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], (float)(y_abs + 1));
			return;
		}
		pixels[2 * num + 1] = (float)(y_abs + 1);
	}

	private static void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
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

	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}

	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	public Color highlightColor2 = new Color(1f, 0.32f, 0f, 1f);

	private int LastVisibleColumnCount;
}
