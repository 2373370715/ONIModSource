using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CEB RID: 7403
public class BarLayer : GraphLayer
{
	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x06009A9E RID: 39582 RVA: 0x001048D0 File Offset: 0x00102AD0
	public int bar_count
	{
		get
		{
			return this.bars.Count;
		}
	}

	// Token: 0x06009A9F RID: 39583 RVA: 0x003BA78C File Offset: 0x003B898C
	public void NewBar(int[] values, float x_position, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
		if (ID == "")
		{
			ID = this.bars.Count.ToString();
		}
		gameObject.name = string.Format("bar_{0}", ID);
		GraphedBar component = gameObject.GetComponent<GraphedBar>();
		component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
		int[] array = new int[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = (int)(base.graph.rectTransform().rect.height * base.graph.GetRelativeSize(new Vector2(0f, (float)values[i])).y);
		}
		component.SetValues(array, base.graph.GetRelativePosition(new Vector2(x_position, 0f)).x);
		this.bars.Add(component);
	}

	// Token: 0x06009AA0 RID: 39584 RVA: 0x003BA88C File Offset: 0x003B8A8C
	public void ClearBars()
	{
		foreach (GraphedBar graphedBar in this.bars)
		{
			if (graphedBar != null && graphedBar.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedBar.gameObject);
			}
		}
		this.bars.Clear();
	}

	// Token: 0x040078CA RID: 30922
	public GameObject bar_container;

	// Token: 0x040078CB RID: 30923
	public GameObject prefab_bar;

	// Token: 0x040078CC RID: 30924
	public GraphedBarFormatting[] bar_formats;

	// Token: 0x040078CD RID: 30925
	private List<GraphedBar> bars = new List<GraphedBar>();
}
