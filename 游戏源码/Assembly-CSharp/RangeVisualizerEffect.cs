using System;
using Unity.Collections;
using UnityEngine;

public class RangeVisualizerEffect : MonoBehaviour
{
	private Material material;

	private Camera myCamera;

	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	private Texture2D OcclusionTex;

	private int LastVisibleTileCount;

	private void Start()
	{
		material = new Material(Shader.Find("Klei/PostFX/Range"));
	}

	private void OnPostRender()
	{
		RangeVisualizer rangeVisualizer = null;
		Vector2I vector2I = new Vector2I(0, 0);
		if (SelectTool.Instance.selected != null)
		{
			Grid.PosToXY(SelectTool.Instance.selected.transform.GetPosition(), out vector2I.x, out vector2I.y);
			rangeVisualizer = SelectTool.Instance.selected.GetComponent<RangeVisualizer>();
		}
		if (rangeVisualizer == null && BuildTool.Instance.visualizer != null)
		{
			Grid.PosToXY(BuildTool.Instance.visualizer.transform.GetPosition(), out vector2I.x, out vector2I.y);
			rangeVisualizer = BuildTool.Instance.visualizer.GetComponent<RangeVisualizer>();
		}
		if (!(rangeVisualizer != null))
		{
			return;
		}
		if (OcclusionTex == null)
		{
			OcclusionTex = new Texture2D(64, 64, TextureFormat.Alpha8, mipChain: false);
			OcclusionTex.filterMode = FilterMode.Point;
			OcclusionTex.wrapMode = TextureWrapMode.Clamp;
		}
		FindWorldBounds(out var world_min, out var world_max);
		Vector2I rangeMin = rangeVisualizer.RangeMin;
		Vector2I rangeMax = rangeVisualizer.RangeMax;
		Vector2I vector2I2 = rangeVisualizer.OriginOffset;
		if (rangeVisualizer.TryGetComponent<Rotatable>(out var component))
		{
			vector2I2 = component.GetRotatedOffset(vector2I2);
			Vector2I rotatedOffset = component.GetRotatedOffset(rangeMin);
			Vector2I rotatedOffset2 = component.GetRotatedOffset(rangeMax);
			rangeMin.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
			rangeMin.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			rangeMax.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
			rangeMax.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
		}
		Vector2I vector2I3 = vector2I + vector2I2;
		int width = OcclusionTex.width;
		NativeArray<byte> pixelData = OcclusionTex.GetPixelData<byte>(0);
		int num = 0;
		if (rangeVisualizer.TestLineOfSight)
		{
			for (int j = 0; j <= rangeMax.y - rangeMin.y; j++)
			{
				int num2 = vector2I3.y + rangeMin.y + j;
				for (int k = 0; k <= rangeMax.x - rangeMin.x; k++)
				{
					int num3 = vector2I3.x + rangeMin.x + k;
					Grid.XYToCell(num3, num2);
					bool flag = num3 > world_min.x && num3 < world_max.x && num2 > world_min.y && (num2 < world_max.y || rangeVisualizer.AllowLineOfSightInvalidCells) && Grid.TestLineOfSight(vector2I3.x, vector2I3.y, num3, num2, rangeVisualizer.BlockingCb, (rangeVisualizer.BlockingVisibleCb == null) ? ((Func<int, bool>)((int i) => rangeVisualizer.BlockingTileVisible)) : rangeVisualizer.BlockingVisibleCb, rangeVisualizer.AllowLineOfSightInvalidCells);
					pixelData[j * width + k] = (byte)(flag ? 255u : 0u);
					if (flag)
					{
						num++;
					}
				}
			}
		}
		else
		{
			for (int l = 0; l <= rangeMax.y - rangeMin.y; l++)
			{
				int num4 = vector2I3.y + rangeMin.y + l;
				for (int m = 0; m <= rangeMax.x - rangeMin.x; m++)
				{
					int num5 = vector2I3.x + rangeMin.x + m;
					int arg = Grid.XYToCell(num5, num4);
					bool flag2 = num5 > world_min.x && num5 < world_max.x && num4 > world_min.y && num4 < world_max.y && rangeVisualizer.BlockingCb(arg);
					pixelData[l * width + m] = (byte)((!flag2) ? 255u : 0u);
					if (!flag2)
					{
						num++;
					}
				}
			}
		}
		OcclusionTex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		Vector2I vector2I4 = rangeMin + vector2I3;
		Vector2I vector2I5 = rangeMax + vector2I3;
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
		value2.x = vector2I4.x;
		value2.y = vector2I4.y;
		value2.z = vector2I5.x + 1;
		value2.w = vector2I5.y + 1;
		material.SetVector("_RangeParams", value2);
		material.SetColor("_HighlightColor", highlightColor);
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
		if (LastVisibleTileCount != num)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement"), rangeVisualizer.transform.GetPosition());
			LastVisibleTileCount = num;
		}
	}

	private void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
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
}
