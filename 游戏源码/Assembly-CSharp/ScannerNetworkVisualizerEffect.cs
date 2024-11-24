using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ScannerNetworkVisualizerEffect : VisualizerEffect
{
	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	public Color highlightColor2 = new Color(1f, 0.32f, 0f, 1f);

	private int LastVisibleColumnCount;

	protected override void SetupMaterial()
	{
		material = new Material(Shader.Find("Klei/PostFX/ScannerNetwork"));
	}

	protected override void SetupOcclusionTex()
	{
		OcclusionTex = new Texture2D(512, 1, TextureFormat.RGFloat, mipChain: false);
		OcclusionTex.filterMode = FilterMode.Point;
		OcclusionTex.wrapMode = TextureWrapMode.Clamp;
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
		if (!(scannerNetworkVisualizer != null))
		{
			return;
		}
		FindWorldBounds(out var world_min, out var world_max);
		if (world_max.x - world_min.x > OcclusionTex.width)
		{
			return;
		}
		NativeArray<float> pixelData = OcclusionTex.GetPixelData<float>(0);
		for (int i = 0; i < OcclusionTex.width; i++)
		{
			pixelData[2 * i] = 0f;
			pixelData[2 * i + 1] = 0f;
		}
		int visible_column_count = 0;
		List<ScannerNetworkVisualizer> items = Components.ScannerVisualizers.GetItems(scannerNetworkVisualizer.GetMyWorldId());
		for (int j = 0; j < items.Count; j++)
		{
			ScannerNetworkVisualizer scannerNetworkVisualizer2 = items[j];
			if (scannerNetworkVisualizer2 != scannerNetworkVisualizer)
			{
				ComputeVisibility(scannerNetworkVisualizer2, pixelData, world_min, world_max, ref visible_column_count);
			}
		}
		ComputeVisibility(scannerNetworkVisualizer, pixelData, world_min, world_max, ref visible_column_count);
		OcclusionTex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		Vector2I vector2I = world_min;
		Vector2I vector2I2 = world_max;
		if (myCamera == null)
		{
			myCamera = GetComponent<Camera>();
			if (myCamera == null)
			{
				return;
			}
		}
		Ray ray = myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 value = default(Vector4);
		value.x = point.x;
		value.y = point.y;
		ray = myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		value.z = point.x - value.x;
		value.w = point.y - value.y;
		material.SetVector("_UVOffsetScale", value);
		Vector4 value2 = default(Vector4);
		value2.x = vector2I.x;
		value2.y = vector2I.y;
		value2.z = vector2I2.x;
		value2.w = vector2I2.y;
		material.SetVector("_RangeParams", value2);
		material.SetColor("_HighlightColor", highlightColor);
		material.SetColor("_HighlightColor2", highlightColor2);
		Vector4 value3 = default(Vector4);
		value3.x = 1f / (float)OcclusionTex.width;
		value3.y = 1f / (float)OcclusionTex.height;
		value3.z = 0f;
		value3.w = 0f;
		material.SetVector("_OcclusionParams", value3);
		material.SetTexture("_OcclusionTex", OcclusionTex);
		Vector4 value4 = default(Vector4);
		value4.x = Grid.WidthInCells;
		value4.y = Grid.HeightInCells;
		value4.z = 1f / (float)Grid.WidthInCells;
		value4.w = 1f / (float)Grid.HeightInCells;
		material.SetVector("_WorldParams", value4);
		GL.PushMatrix();
		material.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(5);
		GL.Color(Color.white);
		GL.Vertex3(0f, 0f, 0f);
		GL.Vertex3(0f, 1f, 0f);
		GL.Vertex3(1f, 0f, 0f);
		GL.Vertex3(1f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
		if (LastVisibleColumnCount != visible_column_count)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement"), scannerNetworkVisualizer.transform.GetPosition());
			LastVisibleColumnCount = visible_column_count;
		}
	}

	private static void ComputeVisibility(ScannerNetworkVisualizer scan, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max, ref int visible_column_count)
	{
		Vector2I vector2I = Grid.PosToXY(scan.transform.GetPosition());
		int rangeMin = scan.RangeMin;
		int rangeMax = scan.RangeMax;
		Vector2I vector2I2 = vector2I + scan.OriginOffset;
		bool visible = true;
		for (int num = 0; num >= rangeMin; num--)
		{
			int x_abs = vector2I2.x + num;
			int y_abs = vector2I2.y + Mathf.Abs(num);
			ComputeVisibility(x_abs, y_abs, pixels, world_min, world_max, ref visible);
			if (visible)
			{
				visible_column_count++;
			}
		}
		visible = true;
		for (int i = 0; i <= rangeMax; i++)
		{
			int x_abs2 = vector2I2.x + i;
			int y_abs2 = vector2I2.y + Mathf.Abs(i);
			ComputeVisibility(x_abs2, y_abs2, pixels, world_min, world_max, ref visible);
			if (visible)
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
		visible &= HasSkyVisibility(cell);
		if (pixels[2 * num] != 2f)
		{
			pixels[2 * num] = ((!visible) ? 1 : 2);
			if (pixels[2 * num] == 1f && pixels[2 * num + 1] != 0f)
			{
				pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], y_abs + 1);
			}
			else
			{
				pixels[2 * num + 1] = y_abs + 1;
			}
		}
		else if (visible)
		{
			pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], y_abs + 1);
		}
	}

	private static void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			world_min = activeWorld.WorldOffset;
			world_max = activeWorld.WorldOffset + activeWorld.WorldSize;
		}
		else
		{
			world_min.x = 0;
			world_min.y = 0;
			world_max.x = Grid.WidthInCells;
			world_max.y = Grid.HeightInCells;
		}
	}

	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}
}
