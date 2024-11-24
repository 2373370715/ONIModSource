using Unity.Collections;
using UnityEngine;

public class SkyVisibilityVisualizerEffect : MonoBehaviour
{
	private Material material;

	private Camera myCamera;

	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	public Color highlightColor2 = new Color(1f, 0.32f, 0f, 1f);

	private Texture2D OcclusionTex;

	private int LastVisibleColumnCount;

	private void Start()
	{
		material = new Material(Shader.Find("Klei/PostFX/SkyVisibility"));
	}

	private void OnPostRender()
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = null;
		Vector2I vector2I = new Vector2I(0, 0);
		if (SelectTool.Instance.selected != null)
		{
			Grid.PosToXY(SelectTool.Instance.selected.transform.GetPosition(), out vector2I.x, out vector2I.y);
			skyVisibilityVisualizer = SelectTool.Instance.selected.GetComponent<SkyVisibilityVisualizer>();
		}
		if (skyVisibilityVisualizer == null && BuildTool.Instance.visualizer != null)
		{
			Grid.PosToXY(BuildTool.Instance.visualizer.transform.GetPosition(), out vector2I.x, out vector2I.y);
			skyVisibilityVisualizer = BuildTool.Instance.visualizer.GetComponent<SkyVisibilityVisualizer>();
		}
		if (!(skyVisibilityVisualizer != null))
		{
			return;
		}
		if (skyVisibilityVisualizer.SkipOnModuleInteriors && ClusterManager.Instance != null)
		{
			WorldContainer myWorld = skyVisibilityVisualizer.GetMyWorld();
			if (myWorld != null && myWorld.IsModuleInterior)
			{
				return;
			}
		}
		if (OcclusionTex == null)
		{
			OcclusionTex = new Texture2D(64, 1, TextureFormat.RGFloat, mipChain: false);
			OcclusionTex.filterMode = FilterMode.Point;
			OcclusionTex.wrapMode = TextureWrapMode.Clamp;
		}
		FindWorldBounds(out var world_min, out var world_max);
		int rangeMin = skyVisibilityVisualizer.RangeMin;
		int rangeMax = skyVisibilityVisualizer.RangeMax;
		Vector2I originOffset = skyVisibilityVisualizer.OriginOffset;
		Vector2I vector2I2 = vector2I + originOffset;
		NativeArray<float> pixelData = OcclusionTex.GetPixelData<float>(0);
		int num = 0;
		bool flag = true;
		int num2 = vector2I2.x + rangeMin;
		int num3 = vector2I2.x + rangeMax;
		bool flag2 = true;
		for (int num4 = vector2I2.x; num4 >= num2; num4--)
		{
			int num5 = vector2I2.y + (vector2I2.x - num4) * skyVisibilityVisualizer.ScanVerticalStep;
			int arg = Grid.XYToCell(num4, num5);
			flag2 &= num4 > world_min.x && num4 < world_max.x && num5 > world_min.y && num5 < world_max.y && skyVisibilityVisualizer.SkyVisibilityCb(arg);
			int num6 = num4 - num2;
			if (!skyVisibilityVisualizer.AllOrNothingVisibility)
			{
				pixelData[2 * num6] = (flag2 ? 1 : 0);
			}
			pixelData[2 * num6 + 1] = num5 + 1;
			if (flag2)
			{
				num++;
			}
		}
		flag = flag && flag2;
		Vector2I vector2I3 = vector2I2;
		if (skyVisibilityVisualizer.TwoWideOrgin)
		{
			vector2I3.x++;
		}
		flag2 = true;
		for (int i = vector2I3.x; i <= num3; i++)
		{
			int num7 = vector2I3.y + (i - vector2I3.x) * skyVisibilityVisualizer.ScanVerticalStep;
			int arg2 = Grid.XYToCell(i, num7);
			flag2 &= i > world_min.x && i < world_max.x && num7 > world_min.y && num7 < world_max.y && skyVisibilityVisualizer.SkyVisibilityCb(arg2);
			int num8 = i - num2;
			if (!skyVisibilityVisualizer.AllOrNothingVisibility)
			{
				pixelData[2 * num8] = (flag2 ? 1 : 0);
			}
			pixelData[2 * num8 + 1] = num7 + 1;
			if (flag2)
			{
				num++;
			}
		}
		flag = flag && flag2;
		if (skyVisibilityVisualizer.AllOrNothingVisibility)
		{
			for (int j = 0; j <= rangeMax - rangeMin; j++)
			{
				pixelData[2 * j] = (flag ? 1 : 0);
			}
		}
		OcclusionTex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		Vector2I vector2I4 = vector2I2 + new Vector2I(rangeMin, 0);
		Vector2I vector2I5 = new Vector2I(vector2I2.x + rangeMax, world_max.y);
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
		if (LastVisibleColumnCount != num)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement"), skyVisibilityVisualizer.transform.GetPosition());
			LastVisibleColumnCount = num;
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
