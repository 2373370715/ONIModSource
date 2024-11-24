using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D40 RID: 7488
public class KleiInventoryUISubcategory : KMonoBehaviour
{
	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x06009C59 RID: 40025 RVA: 0x00105BDF File Offset: 0x00103DDF
	public bool IsOpen
	{
		get
		{
			return this.stateExpanded;
		}
	}

	// Token: 0x06009C5A RID: 40026 RVA: 0x00105BE7 File Offset: 0x00103DE7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.expandButton.onClick = delegate()
		{
			this.ToggleOpen(!this.stateExpanded);
		};
	}

	// Token: 0x06009C5B RID: 40027 RVA: 0x00105C06 File Offset: 0x00103E06
	public void SetIdentity(string label, Sprite icon)
	{
		this.label.SetText(label);
		this.icon.sprite = icon;
	}

	// Token: 0x06009C5C RID: 40028 RVA: 0x003C3A00 File Offset: 0x003C1C00
	public void RefreshDisplay()
	{
		foreach (GameObject gameObject in this.dummyItems)
		{
			gameObject.SetActive(false);
		}
		int num = 0;
		for (int i = 0; i < this.gridLayout.transform.childCount; i++)
		{
			if (this.gridLayout.transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		base.gameObject.SetActive(num != 0);
		int j = 0;
		int num2 = num % this.gridLayout.constraintCount;
		if (num2 > 0)
		{
			j = this.gridLayout.constraintCount - num2;
		}
		while (j > this.dummyItems.Count)
		{
			this.dummyItems.Add(Util.KInstantiateUI(this.dummyPrefab, this.gridLayout.gameObject, false));
		}
		for (int k = 0; k < j; k++)
		{
			this.dummyItems[k].SetActive(true);
			this.dummyItems[k].transform.SetAsLastSibling();
		}
		this.headerLayout.minWidth = base.transform.parent.rectTransform().rect.width - 8f;
	}

	// Token: 0x06009C5D RID: 40029 RVA: 0x00105C20 File Offset: 0x00103E20
	public void ToggleOpen(bool open)
	{
		this.gridLayout.gameObject.SetActive(open);
		this.stateExpanded = open;
		this.expandButton.ChangeState(this.stateExpanded ? 1 : 0);
	}

	// Token: 0x04007A90 RID: 31376
	[SerializeField]
	private GameObject dummyPrefab;

	// Token: 0x04007A91 RID: 31377
	public string subcategoryID;

	// Token: 0x04007A92 RID: 31378
	public GridLayoutGroup gridLayout;

	// Token: 0x04007A93 RID: 31379
	public List<GameObject> dummyItems;

	// Token: 0x04007A94 RID: 31380
	[SerializeField]
	private LayoutElement headerLayout;

	// Token: 0x04007A95 RID: 31381
	[SerializeField]
	private Image icon;

	// Token: 0x04007A96 RID: 31382
	[SerializeField]
	private LocText label;

	// Token: 0x04007A97 RID: 31383
	[SerializeField]
	private MultiToggle expandButton;

	// Token: 0x04007A98 RID: 31384
	private bool stateExpanded = true;
}
