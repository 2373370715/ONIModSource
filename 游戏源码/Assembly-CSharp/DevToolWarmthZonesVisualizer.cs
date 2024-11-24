using System;
using UnityEngine;

// Token: 0x02000BE2 RID: 3042
public class DevToolWarmthZonesVisualizer : DevTool
{
	// Token: 0x06003A37 RID: 14903 RVA: 0x002268F8 File Offset: 0x00224AF8
	private void SetupColors()
	{
		if (this.colors == null)
		{
			this.colors = new Color[3];
			for (int i = 1; i <= 3; i++)
			{
				this.colors[i - 1] = this.CreateColorForWarmthValue(i);
			}
		}
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x0022693C File Offset: 0x00224B3C
	private Color CreateColorForWarmthValue(int warmValue)
	{
		float b = (float)Mathf.Clamp(warmValue, 1, 3) / 3f;
		Color result = this.WARM_CELL_COLOR * b;
		result.a = this.WARM_CELL_COLOR.a;
		return result;
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x0022697C File Offset: 0x00224B7C
	private Color GetBorderColor(int warmValue)
	{
		int num = Mathf.Clamp(warmValue, 0, 3);
		return this.colors[num];
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x002269A0 File Offset: 0x00224BA0
	private Color GetFillColor(int warmValue)
	{
		Color borderColor = this.GetBorderColor(warmValue);
		borderColor.a = 0.3f;
		return borderColor;
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x002269C4 File Offset: 0x00224BC4
	protected override void RenderTo(DevPanel panel)
	{
		this.SetupColors();
		foreach (int num in WarmthProvider.WarmCells.Keys)
		{
			if (Grid.IsValidCell(num) && WarmthProvider.IsWarmCell(num))
			{
				int warmthValue = WarmthProvider.GetWarmthValue(num);
				Option<ValueTuple<Vector2, Vector2>> screenRect = new DevToolEntityTarget.ForSimCell(num).GetScreenRect();
				string value = warmthValue.ToString();
				DevToolEntity.DrawScreenRect(screenRect.Unwrap(), value, this.GetBorderColor(warmthValue - 1), this.GetFillColor(warmthValue - 1), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
			}
		}
	}

	// Token: 0x040027AF RID: 10159
	private const int MAX_COLOR_VARIANTS = 3;

	// Token: 0x040027B0 RID: 10160
	private Color WARM_CELL_COLOR = Color.red;

	// Token: 0x040027B1 RID: 10161
	private Color[] colors;
}
