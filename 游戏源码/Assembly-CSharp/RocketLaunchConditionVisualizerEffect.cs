using Unity.Collections;
using UnityEngine;

public class RocketLaunchConditionVisualizerEffect : VisualizerEffect
{
	public enum EvaluationState : byte
	{
		NotEvaluated,
		Clear,
		Obstructed
	}

	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	public Color highlightColor2 = new Color(1f, 0.32f, 0f, 1f);

	private static EvaluationState[] clearPathToSpaceColumn = new EvaluationState[7];

	private static int clearPathToSpaceColumn_middleIndex = Mathf.FloorToInt(3.5f);

	protected override void SetupMaterial()
	{
		material = new Material(Shader.Find("Klei/PostFX/RocketLaunchCondition"));
	}

	protected override void SetupOcclusionTex()
	{
		OcclusionTex = new Texture2D(512, 1, TextureFormat.RGFloat, mipChain: false);
		OcclusionTex.filterMode = FilterMode.Point;
		OcclusionTex.wrapMode = TextureWrapMode.Clamp;
	}

	protected override void OnPostRender()
	{
		RocketLaunchConditionVisualizer rocketLaunchConditionVisualizer = null;
		if (SelectTool.Instance.selected != null)
		{
			rocketLaunchConditionVisualizer = SelectTool.Instance.selected.GetComponent<RocketLaunchConditionVisualizer>();
			if (rocketLaunchConditionVisualizer == null)
			{
				RocketModuleCluster component = SelectTool.Instance.selected.GetComponent<RocketModuleCluster>();
				if (component != null)
				{
					PassengerRocketModule passengerModule = component.CraftInterface.GetPassengerModule();
					if (passengerModule != null)
					{
						rocketLaunchConditionVisualizer = passengerModule.gameObject.GetComponent<RocketLaunchConditionVisualizer>();
					}
				}
			}
		}
		if (!(rocketLaunchConditionVisualizer != null))
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
		for (int j = 0; j < clearPathToSpaceColumn.Length; j++)
		{
			clearPathToSpaceColumn[j] = EvaluationState.NotEvaluated;
		}
		for (int k = 0; k < rocketLaunchConditionVisualizer.moduleVisualizeData.Length; k++)
		{
			ComputeVisibility(rocketLaunchConditionVisualizer.moduleVisualizeData[k], pixelData, world_min, world_max);
		}
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
	}

	private static void ComputeVisibility(RocketLaunchConditionVisualizer.RocketModuleVisualizeData moduleData, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max)
	{
		Vector2I vector2I = Grid.PosToXY(moduleData.Module.transform.GetPosition());
		int rangeMin = moduleData.RangeMin;
		int rangeMax = moduleData.RangeMax;
		Vector2I vector2I2 = vector2I + moduleData.OriginOffset;
		for (int num = 0; num >= rangeMin; num--)
		{
			int x_abs = vector2I2.x + num;
			int y = vector2I2.y;
			EvaluationState clearPathEvaluation = clearPathToSpaceColumn[clearPathToSpaceColumn_middleIndex + num];
			ComputeVisibility(x_abs, y, pixels, world_min, world_max, ref clearPathEvaluation);
			clearPathToSpaceColumn[clearPathToSpaceColumn_middleIndex + num] = clearPathEvaluation;
		}
		for (int i = 0; i <= rangeMax; i++)
		{
			int x_abs2 = vector2I2.x + i;
			int y2 = vector2I2.y;
			EvaluationState clearPathEvaluation2 = clearPathToSpaceColumn[clearPathToSpaceColumn_middleIndex + i];
			ComputeVisibility(x_abs2, y2, pixels, world_min, world_max, ref clearPathEvaluation2);
			clearPathToSpaceColumn[clearPathToSpaceColumn_middleIndex + i] = clearPathEvaluation2;
		}
	}

	private static void ComputeVisibility(int x_abs, int y_abs, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max, ref EvaluationState clearPathEvaluation)
	{
		int num = x_abs - world_min.x;
		if (x_abs < world_min.x || x_abs > world_max.x || y_abs < world_min.y || y_abs >= world_max.y)
		{
			return;
		}
		int cell = Grid.XYToCell(x_abs, y_abs);
		if (clearPathEvaluation == EvaluationState.NotEvaluated)
		{
			clearPathEvaluation = (HasClearPathToSpace(cell, world_max) ? EvaluationState.Clear : EvaluationState.Obstructed);
		}
		bool flag = clearPathEvaluation == EvaluationState.Clear;
		if (pixels[2 * num] != 2f)
		{
			pixels[2 * num] = ((!flag) ? 1 : 2);
			if (pixels[2 * num] == 1f && pixels[2 * num + 1] != 0f)
			{
				pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], y_abs);
			}
			else
			{
				pixels[2 * num + 1] = y_abs;
			}
		}
		else if (flag)
		{
			pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], y_abs);
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

	private static bool HasClearPathToSpace(int cell, Vector2I worldMax)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int cell2 = cell;
		while (!Grid.IsSolidCell(cell2) && Grid.CellToXY(cell2).y < worldMax.y)
		{
			cell2 = Grid.CellAbove(cell2);
		}
		if (!Grid.IsSolidCell(cell2) && Grid.CellToXY(cell2).y == worldMax.y)
		{
			return true;
		}
		return false;
	}
}
