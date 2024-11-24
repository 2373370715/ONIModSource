using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F55 RID: 8021
public class ConditionListSideScreen : SideScreenContent
{
	// Token: 0x0600A951 RID: 43345 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x0600A952 RID: 43346 RVA: 0x0010DFEC File Offset: 0x0010C1EC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target != null)
		{
			this.targetConditionSet = target.GetComponent<IProcessConditionSet>();
		}
	}

	// Token: 0x0600A953 RID: 43347 RVA: 0x0010E00A File Offset: 0x0010C20A
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600A954 RID: 43348 RVA: 0x004011E8 File Offset: 0x003FF3E8
	private void Refresh()
	{
		bool flag = false;
		List<ProcessCondition> conditionSet = this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All);
		foreach (ProcessCondition key in conditionSet)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			if (!conditionSet.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.Rebuild();
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair2 in this.rows)
		{
			ConditionListSideScreen.SetRowState(keyValuePair2.Value, keyValuePair2.Key);
		}
	}

	// Token: 0x0600A955 RID: 43349 RVA: 0x004012FC File Offset: 0x003FF4FC
	public static void SetRowState(GameObject row, ProcessCondition condition)
	{
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		ProcessCondition.Status status = condition.EvaluateCondition();
		component.GetReference<LocText>("Label").text = condition.GetStatusMessage(status);
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.failedColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.failedColor;
			break;
		case ProcessCondition.Status.Warning:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.warningColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.warningColor;
			break;
		case ProcessCondition.Status.Ready:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.readyColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.readyColor;
			break;
		}
		component.GetReference<Image>("Check").gameObject.SetActive(status == ProcessCondition.Status.Ready);
		component.GetReference<Image>("Dash").gameObject.SetActive(false);
		row.GetComponent<ToolTip>().SetSimpleTooltip(condition.GetStatusTooltip(status));
	}

	// Token: 0x0600A956 RID: 43350 RVA: 0x0010E01C File Offset: 0x0010C21C
	private void Rebuild()
	{
		this.ClearRows();
		this.BuildRows();
	}

	// Token: 0x0600A957 RID: 43351 RVA: 0x00401408 File Offset: 0x003FF608
	private void ClearRows()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
	}

	// Token: 0x0600A958 RID: 43352 RVA: 0x0040146C File Offset: 0x003FF66C
	private void BuildRows()
	{
		foreach (ProcessCondition processCondition in this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All))
		{
			if (processCondition.ShowInUI())
			{
				GameObject value = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(processCondition, value);
			}
		}
	}

	// Token: 0x04008532 RID: 34098
	public GameObject rowPrefab;

	// Token: 0x04008533 RID: 34099
	public GameObject rowContainer;

	// Token: 0x04008534 RID: 34100
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public static Color readyColor = Color.black;

	// Token: 0x04008535 RID: 34101
	public static Color failedColor = Color.red;

	// Token: 0x04008536 RID: 34102
	public static Color warningColor = new Color(1f, 0.3529412f, 0f, 1f);

	// Token: 0x04008537 RID: 34103
	private IProcessConditionSet targetConditionSet;

	// Token: 0x04008538 RID: 34104
	private Dictionary<ProcessCondition, GameObject> rows = new Dictionary<ProcessCondition, GameObject>();
}
