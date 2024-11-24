﻿using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02001CEF RID: 7407
[AddComponentMenu("KMonoBehaviour/scripts/GraphedLine")]
[Serializable]
public class GraphedLine : KMonoBehaviour
{
	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x06009AA8 RID: 39592 RVA: 0x0010492E File Offset: 0x00102B2E
	public int PointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x06009AA9 RID: 39593 RVA: 0x00104938 File Offset: 0x00102B38
	public void SetPoints(Vector2[] points)
	{
		this.points = points;
		this.UpdatePoints();
	}

	// Token: 0x06009AAA RID: 39594 RVA: 0x003BAA44 File Offset: 0x003B8C44
	private void UpdatePoints()
	{
		Vector2[] array = new Vector2[this.points.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.layer.graph.GetRelativePosition(this.points[i]);
		}
		this.line_renderer.Points = array;
	}

	// Token: 0x06009AAB RID: 39595 RVA: 0x003BAA9C File Offset: 0x003B8C9C
	public Vector2 GetClosestDataToPointOnXAxis(Vector2 toPoint)
	{
		float num = toPoint.x / this.layer.graph.rectTransform().sizeDelta.x;
		float num2 = this.layer.graph.axis_x.min_value + this.layer.graph.axis_x.range * num;
		Vector2 vector = Vector2.zero;
		foreach (Vector2 vector2 in this.points)
		{
			if (Mathf.Abs(vector2.x - num2) < Mathf.Abs(vector.x - num2))
			{
				vector = vector2;
			}
		}
		return vector;
	}

	// Token: 0x06009AAC RID: 39596 RVA: 0x00104947 File Offset: 0x00102B47
	public void HidePointHighlight()
	{
		if (this.highlightPoint != null)
		{
			this.highlightPoint.SetActive(false);
		}
	}

	// Token: 0x06009AAD RID: 39597 RVA: 0x003BAB44 File Offset: 0x003B8D44
	public void SetPointHighlight(Vector2 point)
	{
		if (this.highlightPoint == null)
		{
			return;
		}
		this.highlightPoint.SetActive(true);
		Vector2 relativePosition = this.layer.graph.GetRelativePosition(point);
		this.highlightPoint.rectTransform().SetLocalPosition(new Vector2(relativePosition.x * this.layer.graph.rectTransform().sizeDelta.x - this.layer.graph.rectTransform().sizeDelta.x / 2f, relativePosition.y * this.layer.graph.rectTransform().sizeDelta.y - this.layer.graph.rectTransform().sizeDelta.y / 2f));
		ToolTip component = this.layer.graph.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		component.tooltipPositionOffset = new Vector2(this.highlightPoint.rectTransform().localPosition.x, this.layer.graph.rectTransform().rect.height / 2f - 12f);
		component.SetSimpleTooltip(string.Concat(new string[]
		{
			this.layer.graph.axis_x.name,
			" ",
			point.x.ToString(),
			", ",
			Mathf.RoundToInt(point.y).ToString(),
			" ",
			this.layer.graph.axis_y.name
		}));
		ToolTipScreen.Instance.SetToolTip(component);
	}

	// Token: 0x040078D5 RID: 30933
	public UILineRenderer line_renderer;

	// Token: 0x040078D6 RID: 30934
	public LineLayer layer;

	// Token: 0x040078D7 RID: 30935
	private Vector2[] points = new Vector2[0];

	// Token: 0x040078D8 RID: 30936
	[SerializeField]
	private GameObject highlightPoint;
}
