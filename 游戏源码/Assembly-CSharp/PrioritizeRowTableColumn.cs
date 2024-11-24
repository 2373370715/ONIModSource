using System;
using UnityEngine;

// Token: 0x02001DBE RID: 7614
public class PrioritizeRowTableColumn : TableColumn
{
	// Token: 0x06009EF8 RID: 40696 RVA: 0x001079D4 File Offset: 0x00105BD4
	public PrioritizeRowTableColumn(object user_data, Action<object, int> on_change_priority, Func<object, int, string> on_hover_widget) : base(null, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
	}

	// Token: 0x06009EF9 RID: 40697 RVA: 0x001079FC File Offset: 0x00105BFC
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06009EFA RID: 40698 RVA: 0x001079FC File Offset: 0x00105BFC
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06009EFB RID: 40699 RVA: 0x00107A05 File Offset: 0x00105C05
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowHeaderWidget, parent, true);
	}

	// Token: 0x06009EFC RID: 40700 RVA: 0x003CD698 File Offset: 0x003CB898
	private GameObject GetWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.ConfigureButton(component, "UpButton", 1, gameObject);
		this.ConfigureButton(component, "DownButton", -1, gameObject);
		return gameObject;
	}

	// Token: 0x06009EFD RID: 40701 RVA: 0x003CD6E0 File Offset: 0x003CB8E0
	private void ConfigureButton(HierarchyReferences refs, string ref_id, int delta, GameObject widget_go)
	{
		KButton kbutton = refs.GetReference(ref_id) as KButton;
		kbutton.onClick += delegate()
		{
			this.onChangePriority(widget_go, delta);
		};
		ToolTip component = kbutton.GetComponent<ToolTip>();
		if (component != null)
		{
			component.OnToolTip = (() => this.onHoverWidget(widget_go, delta));
		}
	}

	// Token: 0x04007CA2 RID: 31906
	public object userData;

	// Token: 0x04007CA3 RID: 31907
	private Action<object, int> onChangePriority;

	// Token: 0x04007CA4 RID: 31908
	private Func<object, int, string> onHoverWidget;
}
