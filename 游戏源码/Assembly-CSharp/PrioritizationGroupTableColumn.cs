using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DBB RID: 7611
public class PrioritizationGroupTableColumn : TableColumn
{
	// Token: 0x06009EEB RID: 40683 RVA: 0x003CD468 File Offset: 0x003CB668
	public PrioritizationGroupTableColumn(object user_data, Action<IAssignableIdentity, GameObject> on_load_action, Action<object, int> on_change_priority, Func<object, string> on_hover_widget, Action<object, int> on_change_header_priority, Func<object, string> on_hover_header_option_selector, Action<object> on_sort_clicked, Func<object, string> on_sort_hovered) : base(on_load_action, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
		this.onHoverHeaderOptionSelector = on_hover_header_option_selector;
		this.onSortClicked = on_sort_clicked;
		this.onSortHovered = on_sort_hovered;
	}

	// Token: 0x06009EEC RID: 40684 RVA: 0x0010793F File Offset: 0x00105B3F
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06009EED RID: 40685 RVA: 0x0010793F File Offset: 0x00105B3F
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06009EEE RID: 40686 RVA: 0x003CD4B4 File Offset: 0x003CB6B4
	private GameObject GetWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PriorityGroupSelector, parent, true);
		OptionSelector component = widget_go.GetComponent<OptionSelector>();
		component.Initialize(widget_go);
		component.OnChangePriority = delegate(object widget, int delta)
		{
			this.onChangePriority(widget, delta);
		};
		ToolTip[] componentsInChildren = widget_go.transform.GetComponentsInChildren<ToolTip>();
		if (componentsInChildren != null)
		{
			Func<string> <>9__1;
			foreach (ToolTip toolTip in componentsInChildren)
			{
				Func<string> onToolTip;
				if ((onToolTip = <>9__1) == null)
				{
					onToolTip = (<>9__1 = (() => this.onHoverWidget(widget_go)));
				}
				toolTip.OnToolTip = onToolTip;
			}
		}
		return widget_go;
	}

	// Token: 0x06009EEF RID: 40687 RVA: 0x003CD568 File Offset: 0x003CB768
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PriorityGroupSelectorHeader, parent, true);
		HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
		LayoutElement component2 = widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component2.preferredWidth = (component2.minWidth = 63f);
		Component reference = component.GetReference("Label");
		reference.GetComponent<LocText>().raycastTarget = true;
		ToolTip component3 = reference.GetComponent<ToolTip>();
		if (component3 != null)
		{
			component3.OnToolTip = (() => this.onHoverWidget(widget_go));
		}
		MultiToggle componentInChildren = widget_go.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.onSortClicked(widget_go);
		}));
		ToolTip component4 = componentInChildren.GetComponent<ToolTip>();
		if (component4 != null)
		{
			component4.OnToolTip = (() => this.onSortHovered(widget_go));
		}
		ToolTip component5 = (component.GetReference("PrioritizeButton") as KButton).GetComponent<ToolTip>();
		if (component5 != null)
		{
			component5.OnToolTip = (() => this.onHoverHeaderOptionSelector(widget_go));
		}
		return widget_go;
	}

	// Token: 0x04007C97 RID: 31895
	public object userData;

	// Token: 0x04007C98 RID: 31896
	private Action<object, int> onChangePriority;

	// Token: 0x04007C99 RID: 31897
	private Func<object, string> onHoverWidget;

	// Token: 0x04007C9A RID: 31898
	private Func<object, string> onHoverHeaderOptionSelector;

	// Token: 0x04007C9B RID: 31899
	private Action<object> onSortClicked;

	// Token: 0x04007C9C RID: 31900
	private Func<object, string> onSortHovered;
}
