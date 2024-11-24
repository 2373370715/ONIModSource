using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001BC7 RID: 7111
public class DetailsPanelDrawer
{
	// Token: 0x060093D7 RID: 37847 RVA: 0x00100596 File Offset: 0x000FE796
	public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
	{
		this.parent = parent;
		this.labelPrefab = label_prefab;
		this.stringformatter = new UIStringFormatter();
		this.floatFormatter = new UIFloatFormatter();
	}

	// Token: 0x060093D8 RID: 37848 RVA: 0x00390B70 File Offset: 0x0038ED70
	public DetailsPanelDrawer NewLabel(string text)
	{
		DetailsPanelDrawer.Label label = default(DetailsPanelDrawer.Label);
		if (this.activeLabelCount >= this.labels.Count)
		{
			label.text = Util.KInstantiate(this.labelPrefab, this.parent, null).GetComponent<LocText>();
			label.tooltip = label.text.GetComponent<ToolTip>();
			label.text.transform.localScale = new Vector3(1f, 1f, 1f);
			this.labels.Add(label);
		}
		else
		{
			label = this.labels[this.activeLabelCount];
		}
		this.activeLabelCount++;
		label.text.text = text;
		label.tooltip.toolTip = "";
		label.tooltip.OnToolTip = null;
		label.text.gameObject.SetActive(true);
		return this;
	}

	// Token: 0x060093D9 RID: 37849 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public DetailsPanelDrawer BeginDrawing()
	{
		return this;
	}

	// Token: 0x060093DA RID: 37850 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public DetailsPanelDrawer EndDrawing()
	{
		return this;
	}

	// Token: 0x040072C4 RID: 29380
	private List<DetailsPanelDrawer.Label> labels = new List<DetailsPanelDrawer.Label>();

	// Token: 0x040072C5 RID: 29381
	private int activeLabelCount;

	// Token: 0x040072C6 RID: 29382
	private UIStringFormatter stringformatter;

	// Token: 0x040072C7 RID: 29383
	private UIFloatFormatter floatFormatter;

	// Token: 0x040072C8 RID: 29384
	private GameObject parent;

	// Token: 0x040072C9 RID: 29385
	private GameObject labelPrefab;

	// Token: 0x02001BC8 RID: 7112
	private struct Label
	{
		// Token: 0x040072CA RID: 29386
		public LocText text;

		// Token: 0x040072CB RID: 29387
		public ToolTip tooltip;
	}
}
