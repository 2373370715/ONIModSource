using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FED RID: 8173
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x0600AD82 RID: 44418 RVA: 0x00110F02 File Offset: 0x0010F102
	// (set) Token: 0x0600AD83 RID: 44419 RVA: 0x00110F0A File Offset: 0x0010F10A
	public bool ArrowExpanded { get; private set; }

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x0600AD84 RID: 44420 RVA: 0x00110F13 File Offset: 0x0010F113
	// (set) Token: 0x0600AD85 RID: 44421 RVA: 0x00110F1B File Offset: 0x0010F11B
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x0600AD86 RID: 44422 RVA: 0x004123F4 File Offset: 0x004105F4
	public TreeFilterableSideScreenRow.State GetState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			if (this.parent.GetElementTagAcceptedState(treeFilterableSideScreenElement.GetElementTag()))
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (this.rowElements.Count <= 0)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		return TreeFilterableSideScreenRow.State.On;
	}

	// Token: 0x0600AD87 RID: 44423 RVA: 0x00110F24 File Offset: 0x0010F124
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.checkBoxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (this.parent.CurrentSearchValue == "")
			{
				TreeFilterableSideScreenRow.State state = this.GetState();
				if (state > TreeFilterableSideScreenRow.State.Mixed)
				{
					if (state == TreeFilterableSideScreenRow.State.On)
					{
						this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
						return;
					}
				}
				else
				{
					this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
				}
			}
		}));
	}

	// Token: 0x0600AD88 RID: 44424 RVA: 0x00110F53 File Offset: 0x0010F153
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SetArrowToggleState(this.GetState() > TreeFilterableSideScreenRow.State.Off);
	}

	// Token: 0x0600AD89 RID: 44425 RVA: 0x00110F6A File Offset: 0x0010F16A
	protected override void OnCmpDisable()
	{
		this.SetArrowToggleState(false);
		base.OnCmpDisable();
	}

	// Token: 0x0600AD8A RID: 44426 RVA: 0x000BFD4F File Offset: 0x000BDF4F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600AD8B RID: 44427 RVA: 0x00110F79 File Offset: 0x0010F179
	public void UpdateCheckBoxVisualState()
	{
		this.checkBoxToggle.ChangeState((int)this.GetState());
		this.visualDirty = false;
	}

	// Token: 0x0600AD8C RID: 44428 RVA: 0x00412488 File Offset: 0x00410688
	public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			for (int i = 0; i < this.rowElements.Count; i++)
			{
				this.rowElements[i].SetCheckBox(false);
			}
			break;
		case TreeFilterableSideScreenRow.State.On:
			for (int j = 0; j < this.rowElements.Count; j++)
			{
				this.rowElements[j].SetCheckBox(true);
			}
			break;
		}
		this.visualDirty = true;
	}

	// Token: 0x0600AD8D RID: 44429 RVA: 0x00110F93 File Offset: 0x0010F193
	private void ArrowToggleClicked()
	{
		this.SetArrowToggleState(!this.ArrowExpanded);
		this.RefreshArrowToggleState();
	}

	// Token: 0x0600AD8E RID: 44430 RVA: 0x00110FAA File Offset: 0x0010F1AA
	public void SetArrowToggleState(bool state)
	{
		this.ArrowExpanded = state;
		this.RefreshArrowToggleState();
	}

	// Token: 0x0600AD8F RID: 44431 RVA: 0x00110FB9 File Offset: 0x0010F1B9
	private void RefreshArrowToggleState()
	{
		this.arrowToggle.ChangeState(this.ArrowExpanded ? 1 : 0);
		this.elementGroup.SetActive(this.ArrowExpanded);
		this.bgImg.enabled = this.ArrowExpanded;
	}

	// Token: 0x0600AD90 RID: 44432 RVA: 0x000FD595 File Offset: 0x000FB795
	private void ArrowToggleDisabledClick()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x0600AD91 RID: 44433 RVA: 0x00110FF4 File Offset: 0x0010F1F4
	public void ShowToggleBox(bool show)
	{
		this.checkBoxToggle.gameObject.SetActive(show);
	}

	// Token: 0x0600AD92 RID: 44434 RVA: 0x00111007 File Offset: 0x0010F207
	private void OnElementSelectionChanged(Tag t, bool state)
	{
		if (state)
		{
			this.parent.AddTag(t);
		}
		else
		{
			this.parent.RemoveTag(t);
		}
		this.visualDirty = true;
	}

	// Token: 0x0600AD93 RID: 44435 RVA: 0x00412504 File Offset: 0x00410704
	public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
	{
		this.subTags.Clear();
		this.rowElements.Clear();
		this.elementName.text = mainElementTag.ProperName();
		this.bgImg.enabled = false;
		string simpleTooltip = string.Format(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, mainElementTag.ProperName());
		this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		if (filterMap.Count == 0)
		{
			if (this.elementGroup.activeInHierarchy)
			{
				this.elementGroup.SetActive(false);
			}
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleDisabledClick);
			this.arrowToggle.ChangeState(0);
		}
		else
		{
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleClicked);
			this.arrowToggle.ChangeState(0);
			foreach (KeyValuePair<Tag, bool> keyValuePair in filterMap)
			{
				TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
				freeElement.Parent = this.parent;
				freeElement.SetTag(keyValuePair.Key);
				freeElement.SetCheckBox(keyValuePair.Value);
				freeElement.OnSelectionChanged = new Action<Tag, bool>(this.OnElementSelectionChanged);
				freeElement.SetCheckBox(this.parent.IsTagAllowed(keyValuePair.Key));
				this.rowElements.Add(freeElement);
				this.subTags.Add(keyValuePair.Key);
			}
		}
		this.UpdateCheckBoxVisualState();
	}

	// Token: 0x0600AD94 RID: 44436 RVA: 0x004126A4 File Offset: 0x004108A4
	public void RefreshRowElements()
	{
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			treeFilterableSideScreenElement.SetCheckBox(this.parent.IsTagAllowed(treeFilterableSideScreenElement.GetElementTag()));
		}
	}

	// Token: 0x0600AD95 RID: 44437 RVA: 0x00412708 File Offset: 0x00410908
	public void FilterAgainstSearch(Tag thisCategoryTag, string search)
	{
		bool flag = false;
		bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		search = search.ToUpper();
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			bool flag3 = flag2 || treeFilterableSideScreenElement.GetElementTag().ProperNameStripLink().ToUpper().Contains(search.ToUpper());
			treeFilterableSideScreenElement.gameObject.SetActive(flag3);
			flag = (flag || flag3);
		}
		base.gameObject.SetActive(flag);
		if (search != "" && flag && this.arrowToggle.CurrentState == 0)
		{
			this.SetArrowToggleState(true);
		}
	}

	// Token: 0x04008824 RID: 34852
	public bool visualDirty;

	// Token: 0x04008825 RID: 34853
	public bool standardCommodity = true;

	// Token: 0x04008826 RID: 34854
	[SerializeField]
	private LocText elementName;

	// Token: 0x04008827 RID: 34855
	[SerializeField]
	private GameObject elementGroup;

	// Token: 0x04008828 RID: 34856
	[SerializeField]
	private MultiToggle checkBoxToggle;

	// Token: 0x04008829 RID: 34857
	[SerializeField]
	private MultiToggle arrowToggle;

	// Token: 0x0400882A RID: 34858
	[SerializeField]
	private KImage bgImg;

	// Token: 0x0400882B RID: 34859
	private List<Tag> subTags = new List<Tag>();

	// Token: 0x0400882C RID: 34860
	private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();

	// Token: 0x0400882D RID: 34861
	private TreeFilterableSideScreen parent;

	// Token: 0x02001FEE RID: 8174
	public enum State
	{
		// Token: 0x04008830 RID: 34864
		Off,
		// Token: 0x04008831 RID: 34865
		Mixed,
		// Token: 0x04008832 RID: 34866
		On
	}
}
