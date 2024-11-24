using System;
using UnityEngine;

// Token: 0x02001DA4 RID: 7588
public class CheckboxTableColumn : TableColumn
{
	// Token: 0x06009EA1 RID: 40609 RVA: 0x003CCA04 File Offset: 0x003CAC04
	public CheckboxTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparer, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, Func<bool> revealed = null) : base(on_load_action, sort_comparer, on_tooltip, on_sort_tooltip, revealed, false, "")
	{
		this.get_value_action = get_value_action;
		this.on_press_action = on_press_action;
		this.on_set_action = set_value_action;
	}

	// Token: 0x06009EA2 RID: 40610 RVA: 0x003CCA68 File Offset: 0x003CAC68
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x06009EA3 RID: 40611 RVA: 0x003CCAF8 File Offset: 0x003CACF8
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x06009EA4 RID: 40612 RVA: 0x003CCB88 File Offset: 0x003CAD88
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		ToolTip tooltip = null;
		GameObject widget_go = Util.KInstantiateUI(this.prefab_header_portrait_checkbox, parent, true);
		tooltip = widget_go.GetComponent<ToolTip>();
		HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
		if (tooltip == null && component != null && component.HasReference("ToolTip"))
		{
			tooltip = (component.GetReference("ToolTip") as ToolTip);
		}
		tooltip.OnToolTip = (() => this.GetTooltip(tooltip));
		MultiToggle multiToggle = component.GetReference("Toggle") as MultiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		MultiToggle componentInChildren = widget_go.GetComponentInChildren<MultiToggle>();
		MultiToggle multiToggle2 = componentInChildren;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.screen.SetSortComparison(this.sort_comparer, this);
			this.screen.SortRows();
		}));
		ToolTip sort_tooltip = componentInChildren.GetComponent<ToolTip>();
		sort_tooltip.OnToolTip = (() => this.GetSortTooltip(sort_tooltip));
		this.column_sort_toggle = widget_go.GetComponentInChildren<MultiToggle>();
		return widget_go;
	}

	// Token: 0x04007C5E RID: 31838
	public GameObject prefab_header_portrait_checkbox = Assets.UIPrefabs.TableScreenWidgets.TogglePortrait;

	// Token: 0x04007C5F RID: 31839
	public GameObject prefab_checkbox = Assets.UIPrefabs.TableScreenWidgets.Checkbox;

	// Token: 0x04007C60 RID: 31840
	public Action<GameObject> on_press_action;

	// Token: 0x04007C61 RID: 31841
	public Action<GameObject, TableScreen.ResultValues> on_set_action;

	// Token: 0x04007C62 RID: 31842
	public Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action;
}
