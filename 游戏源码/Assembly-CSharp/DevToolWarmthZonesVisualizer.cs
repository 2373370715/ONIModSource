using UnityEngine;

public class DevToolWarmthZonesVisualizer : DevTool
{
	private const int MAX_COLOR_VARIANTS = 3;

	private Color WARM_CELL_COLOR = Color.red;

	private Color[] colors;

	private void SetupColors()
	{
		if (colors == null)
		{
			colors = new Color[3];
			for (int i = 1; i <= 3; i++)
			{
				colors[i - 1] = CreateColorForWarmthValue(i);
			}
		}
	}

	private Color CreateColorForWarmthValue(int warmValue)
	{
		float num = (float)Mathf.Clamp(warmValue, 1, 3) / 3f;
		Color result = WARM_CELL_COLOR * num;
		result.a = WARM_CELL_COLOR.a;
		return result;
	}

	private Color GetBorderColor(int warmValue)
	{
		int num = Mathf.Clamp(warmValue, 0, 3);
		return colors[num];
	}

	private Color GetFillColor(int warmValue)
	{
		Color borderColor = GetBorderColor(warmValue);
		borderColor.a = 0.3f;
		return borderColor;
	}

	protected override void RenderTo(DevPanel panel)
	{
		SetupColors();
		foreach (int key in WarmthProvider.WarmCells.Keys)
		{
			if (Grid.IsValidCell(key) && WarmthProvider.IsWarmCell(key))
			{
				int warmthValue = WarmthProvider.GetWarmthValue(key);
				Option<(Vector2, Vector2)> screenRect = new DevToolEntityTarget.ForSimCell(key).GetScreenRect();
				string text = warmthValue.ToString();
				DevToolEntity.DrawScreenRect(screenRect.Unwrap(), text, GetBorderColor(warmthValue - 1), GetFillColor(warmthValue - 1), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
			}
		}
	}
}
