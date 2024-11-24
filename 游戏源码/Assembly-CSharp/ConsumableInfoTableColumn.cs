using System;
using UnityEngine;

// Token: 0x02001DC4 RID: 7620
public class ConsumableInfoTableColumn : CheckboxTableColumn
{
	// Token: 0x06009F23 RID: 40739 RVA: 0x003CEC90 File Offset: 0x003CCE90
	public ConsumableInfoTableColumn(IConsumableUIItem consumable_info, Action<IAssignableIdentity, GameObject> load_value_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, Func<GameObject, string> get_header_label) : base(load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, () => DebugHandler.InstantBuildMode || ConsumerManager.instance.isDiscovered(consumable_info.ConsumableId.ToTag()))
	{
		this.consumable_info = consumable_info;
		this.get_header_label = get_header_label;
	}

	// Token: 0x06009F24 RID: 40740 RVA: 0x003CECDC File Offset: 0x003CCEDC
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject headerWidget = base.GetHeaderWidget(parent);
		if (headerWidget.GetComponentInChildren<LocText>() != null)
		{
			headerWidget.GetComponentInChildren<LocText>().text = this.get_header_label(headerWidget);
		}
		headerWidget.GetComponentInChildren<MultiToggle>().gameObject.SetActive(false);
		return headerWidget;
	}

	// Token: 0x04007CAE RID: 31918
	public IConsumableUIItem consumable_info;

	// Token: 0x04007CAF RID: 31919
	public Func<GameObject, string> get_header_label;
}
