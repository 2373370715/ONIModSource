using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200204F RID: 8271
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownList")]
public class BreakdownList : KMonoBehaviour
{
	// Token: 0x0600B01B RID: 45083 RVA: 0x004232A8 File Offset: 0x004214A8
	public BreakdownListRow AddRow()
	{
		BreakdownListRow breakdownListRow;
		if (this.unusedListRows.Count > 0)
		{
			breakdownListRow = this.unusedListRows[0];
			this.unusedListRows.RemoveAt(0);
		}
		else
		{
			breakdownListRow = UnityEngine.Object.Instantiate<BreakdownListRow>(this.listRowTemplate);
		}
		breakdownListRow.gameObject.transform.SetParent(base.transform);
		breakdownListRow.gameObject.transform.SetAsLastSibling();
		this.listRows.Add(breakdownListRow);
		breakdownListRow.gameObject.SetActive(true);
		return breakdownListRow;
	}

	// Token: 0x0600B01C RID: 45084 RVA: 0x0011270B File Offset: 0x0011090B
	public GameObject AddCustomRow(GameObject newRow)
	{
		newRow.transform.SetParent(base.transform);
		newRow.gameObject.transform.SetAsLastSibling();
		this.customRows.Add(newRow);
		newRow.SetActive(true);
		return newRow;
	}

	// Token: 0x0600B01D RID: 45085 RVA: 0x0042332C File Offset: 0x0042152C
	public void ClearRows()
	{
		foreach (BreakdownListRow breakdownListRow in this.listRows)
		{
			this.unusedListRows.Add(breakdownListRow);
			breakdownListRow.gameObject.SetActive(false);
			breakdownListRow.ClearTooltip();
		}
		this.listRows.Clear();
		foreach (GameObject gameObject in this.customRows)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x0600B01E RID: 45086 RVA: 0x00112742 File Offset: 0x00110942
	public void SetTitle(string title)
	{
		this.headerTitle.text = title;
	}

	// Token: 0x0600B01F RID: 45087 RVA: 0x00112750 File Offset: 0x00110950
	public void SetDescription(string description)
	{
		if (description != null && description.Length >= 0)
		{
			this.infoTextLabel.gameObject.SetActive(true);
			this.infoTextLabel.text = description;
			return;
		}
		this.infoTextLabel.gameObject.SetActive(false);
	}

	// Token: 0x0600B020 RID: 45088 RVA: 0x0011278D File Offset: 0x0011098D
	public void SetIcon(Sprite icon)
	{
		this.headerIcon.sprite = icon;
	}

	// Token: 0x04008ACF RID: 35535
	public Image headerIcon;

	// Token: 0x04008AD0 RID: 35536
	public Sprite headerIconSprite;

	// Token: 0x04008AD1 RID: 35537
	public Image headerBar;

	// Token: 0x04008AD2 RID: 35538
	public LocText headerTitle;

	// Token: 0x04008AD3 RID: 35539
	public LocText headerValue;

	// Token: 0x04008AD4 RID: 35540
	public LocText infoTextLabel;

	// Token: 0x04008AD5 RID: 35541
	public BreakdownListRow listRowTemplate;

	// Token: 0x04008AD6 RID: 35542
	private List<BreakdownListRow> listRows = new List<BreakdownListRow>();

	// Token: 0x04008AD7 RID: 35543
	private List<BreakdownListRow> unusedListRows = new List<BreakdownListRow>();

	// Token: 0x04008AD8 RID: 35544
	private List<GameObject> customRows = new List<GameObject>();
}
