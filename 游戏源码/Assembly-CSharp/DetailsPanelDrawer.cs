using System.Collections.Generic;
using UnityEngine;

public class DetailsPanelDrawer
{
	private struct Label
	{
		public LocText text;

		public ToolTip tooltip;
	}

	private List<Label> labels = new List<Label>();

	private int activeLabelCount;

	private UIStringFormatter stringformatter;

	private UIFloatFormatter floatFormatter;

	private GameObject parent;

	private GameObject labelPrefab;

	public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
	{
		this.parent = parent;
		labelPrefab = label_prefab;
		stringformatter = new UIStringFormatter();
		floatFormatter = new UIFloatFormatter();
	}

	public DetailsPanelDrawer NewLabel(string text)
	{
		Label item = default(Label);
		if (activeLabelCount >= labels.Count)
		{
			item.text = Util.KInstantiate(labelPrefab, parent).GetComponent<LocText>();
			item.tooltip = item.text.GetComponent<ToolTip>();
			item.text.transform.localScale = new Vector3(1f, 1f, 1f);
			labels.Add(item);
		}
		else
		{
			item = labels[activeLabelCount];
		}
		activeLabelCount++;
		item.text.text = text;
		item.tooltip.toolTip = "";
		item.tooltip.OnToolTip = null;
		item.text.gameObject.SetActive(value: true);
		return this;
	}

	public DetailsPanelDrawer BeginDrawing()
	{
		return this;
	}

	public DetailsPanelDrawer EndDrawing()
	{
		return this;
	}
}
