using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GridLayouter
{
	public float minCellSize = -1f;

	public float maxCellSize = -1f;

	public List<GridLayoutGroup> targetGridLayouts;

	public RectTransform overrideParentForSizeReference;

	public System.Action OnSizeGridComplete;

	private Vector2 oldScreenSize;

	private float oldScreenScale;

	private int framesLeftToResizeGrid;

	[Conditional("UNITY_EDITOR")]
	private void ValidateImportantFieldsAreSet()
	{
		Debug.Assert(minCellSize >= 0f, string.Format("[{0} Error] Minimum cell size is invalid. Given: {1}", "GridLayouter", minCellSize));
		Debug.Assert(maxCellSize >= 0f, string.Format("[{0} Error] Maximum cell size is invalid. Given: {1}", "GridLayouter", maxCellSize));
		Debug.Assert(targetGridLayouts != null, string.Format("[{0} Error] Target grid layout is invalid. Given: {1}", "GridLayouter", targetGridLayouts));
	}

	public void CheckIfShouldResizeGrid()
	{
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		if (vector != oldScreenSize)
		{
			RequestGridResize();
		}
		oldScreenSize = vector;
		float @float = KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey);
		if (@float != oldScreenScale)
		{
			RequestGridResize();
		}
		oldScreenScale = @float;
		ResizeGridIfRequested();
	}

	public void RequestGridResize()
	{
		framesLeftToResizeGrid = 3;
	}

	private void ResizeGridIfRequested()
	{
		if (framesLeftToResizeGrid > 0)
		{
			ImmediateSizeGridToScreenResolution();
			framesLeftToResizeGrid--;
			if (framesLeftToResizeGrid == 0 && OnSizeGridComplete != null)
			{
				OnSizeGridComplete();
			}
		}
	}

	public void ImmediateSizeGridToScreenResolution()
	{
		foreach (GridLayoutGroup targetGridLayout in targetGridLayouts)
		{
			float workingWidth2 = ((overrideParentForSizeReference != null) ? overrideParentForSizeReference.rect.size.x : targetGridLayout.transform.parent.rectTransform().rect.size.x) - (float)targetGridLayout.padding.left - (float)targetGridLayout.padding.right;
			float x = targetGridLayout.spacing.x;
			int num = GetCellCountToFit(maxCellSize, x, workingWidth2) + 1;
			float num2;
			for (num2 = GetCellSize(workingWidth2, x, num); num2 < minCellSize; num2 = Mathf.Min(maxCellSize, GetCellSize(workingWidth2, x, num)))
			{
				num--;
				if (num <= 0)
				{
					num = 1;
					num2 = minCellSize;
					break;
				}
			}
			targetGridLayout.childAlignment = ((num == 1) ? TextAnchor.UpperCenter : TextAnchor.UpperLeft);
			targetGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			targetGridLayout.constraintCount = num;
			targetGridLayout.cellSize = Vector2.one * num2;
		}
		static int GetCellCountToFit(float cellSize, float spacingSize, float workingWidth)
		{
			int num3 = 0;
			for (float num4 = cellSize; num4 < workingWidth; num4 += cellSize + spacingSize)
			{
				num3++;
			}
			return num3;
		}
		static float GetCellSize(float workingWidth, float spacingSize, int count)
		{
			return (workingWidth - (spacingSize * (float)count - 1f)) / (float)count;
		}
	}
}
