using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001E13 RID: 7699
public abstract class MeterScreen_VTD_DuplicantIterator : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x0600A12E RID: 41262 RVA: 0x003D7218 File Offset: 0x003D5418
	protected virtual void UpdateDisplayInfo(BaseEventData base_ev_data, IList<MinionIdentity> minions)
	{
		PointerEventData pointerEventData = base_ev_data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		PointerEventData.InputButton button = pointerEventData.button;
		if (button != PointerEventData.InputButton.Left)
		{
			if (button != PointerEventData.InputButton.Right)
			{
				return;
			}
			this.lastSelectedDuplicantIndex = -1;
		}
		else
		{
			if (worldMinionIdentities.Count < this.lastSelectedDuplicantIndex)
			{
				this.lastSelectedDuplicantIndex = -1;
			}
			if (worldMinionIdentities.Count > 0)
			{
				this.lastSelectedDuplicantIndex = (this.lastSelectedDuplicantIndex + 1) % worldMinionIdentities.Count;
				MinionIdentity minionIdentity = minions[this.lastSelectedDuplicantIndex];
				SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), Vector3.zero);
				return;
			}
		}
	}

	// Token: 0x0600A12F RID: 41263 RVA: 0x003D72B0 File Offset: 0x003D54B0
	public override void OnClick(BaseEventData base_ev_data)
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		this.UpdateDisplayInfo(base_ev_data, worldMinionIdentities);
		this.OnTooltip();
		this.Tooltip.forceRefresh = true;
	}

	// Token: 0x0600A130 RID: 41264 RVA: 0x00108BE3 File Offset: 0x00106DE3
	protected void AddToolTipLine(string str, bool selected)
	{
		if (selected)
		{
			this.Tooltip.AddMultiStringTooltip("<color=#F0B310FF>" + str + "</color>", this.ToolTipStyle_Property);
			return;
		}
		this.Tooltip.AddMultiStringTooltip(str, this.ToolTipStyle_Property);
	}

	// Token: 0x0600A131 RID: 41265 RVA: 0x003D72E0 File Offset: 0x003D54E0
	protected void AddToolTipAmountPercentLine(AmountInstance amount, MinionIdentity id, bool selected)
	{
		string str = id.GetComponent<KSelectable>().GetName() + ":  " + Mathf.Round(amount.value).ToString() + "%";
		this.AddToolTipLine(str, selected);
	}

	// Token: 0x04007DC6 RID: 32198
	protected int lastSelectedDuplicantIndex = -1;
}
