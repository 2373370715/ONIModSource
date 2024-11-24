using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001DD2 RID: 7634
public class TableColumn : IRender1000ms
{
	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x06009F8A RID: 40842 RVA: 0x00107C75 File Offset: 0x00105E75
	public bool isRevealed
	{
		get
		{
			return this.revealed == null || this.revealed();
		}
	}

	// Token: 0x06009F8B RID: 40843 RVA: 0x003D1ACC File Offset: 0x003CFCCC
	public TableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip = null, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip = null, Func<bool> revealed = null, bool should_refresh_columns = false, string scrollerID = "")
	{
		this.on_load_action = on_load_action;
		this.sort_comparer = sort_comparison;
		this.on_tooltip = on_tooltip;
		this.on_sort_tooltip = on_sort_tooltip;
		this.revealed = revealed;
		this.scrollerID = scrollerID;
		if (should_refresh_columns)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
	}

	// Token: 0x06009F8C RID: 40844 RVA: 0x003D1B28 File Offset: 0x003CFD28
	protected string GetTooltip(ToolTip tool_tip_instance)
	{
		GameObject gameObject = tool_tip_instance.gameObject;
		HierarchyReferences component = tool_tip_instance.GetComponent<HierarchyReferences>();
		if (component != null && component.HasReference("Widget"))
		{
			gameObject = component.GetReference("Widget").gameObject;
		}
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_tooltip != null)
		{
			this.on_tooltip(tableRow.GetIdentity(), gameObject, tool_tip_instance);
		}
		return "";
	}

	// Token: 0x06009F8D RID: 40845 RVA: 0x003D1BF0 File Offset: 0x003CFDF0
	protected string GetSortTooltip(ToolTip sort_tooltip_instance)
	{
		GameObject gameObject = sort_tooltip_instance.transform.parent.gameObject;
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_sort_tooltip != null)
		{
			this.on_sort_tooltip(tableRow.GetIdentity(), gameObject, sort_tooltip_instance);
		}
		return "";
	}

	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x06009F8E RID: 40846 RVA: 0x00107C8C File Offset: 0x00105E8C
	public bool isDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x06009F8F RID: 40847 RVA: 0x00107C94 File Offset: 0x00105E94
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets_by_row.ContainsValue(widget);
	}

	// Token: 0x06009F90 RID: 40848 RVA: 0x00107CA2 File Offset: 0x00105EA2
	public virtual GameObject GetMinionWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06009F91 RID: 40849 RVA: 0x00107CA2 File Offset: 0x00105EA2
	public virtual GameObject GetHeaderWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06009F92 RID: 40850 RVA: 0x00107CA2 File Offset: 0x00105EA2
	public virtual GameObject GetDefaultWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06009F93 RID: 40851 RVA: 0x00107CAF File Offset: 0x00105EAF
	public void Render1000ms(float dt)
	{
		this.MarkDirty(null, TableScreen.ResultValues.False);
	}

	// Token: 0x06009F94 RID: 40852 RVA: 0x00107CB9 File Offset: 0x00105EB9
	public void MarkDirty(GameObject triggering_obj = null, TableScreen.ResultValues triggering_object_state = TableScreen.ResultValues.False)
	{
		this.dirty = true;
	}

	// Token: 0x06009F95 RID: 40853 RVA: 0x00107CC2 File Offset: 0x00105EC2
	public void MarkClean()
	{
		this.dirty = false;
	}

	// Token: 0x04007CD7 RID: 31959
	public Action<IAssignableIdentity, GameObject> on_load_action;

	// Token: 0x04007CD8 RID: 31960
	public Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip;

	// Token: 0x04007CD9 RID: 31961
	public Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip;

	// Token: 0x04007CDA RID: 31962
	public Comparison<IAssignableIdentity> sort_comparer;

	// Token: 0x04007CDB RID: 31963
	public Dictionary<TableRow, GameObject> widgets_by_row = new Dictionary<TableRow, GameObject>();

	// Token: 0x04007CDC RID: 31964
	public string scrollerID;

	// Token: 0x04007CDD RID: 31965
	public TableScreen screen;

	// Token: 0x04007CDE RID: 31966
	public MultiToggle column_sort_toggle;

	// Token: 0x04007CDF RID: 31967
	private Func<bool> revealed;

	// Token: 0x04007CE0 RID: 31968
	protected bool dirty;
}
