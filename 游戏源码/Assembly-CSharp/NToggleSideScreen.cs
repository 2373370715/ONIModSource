using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F93 RID: 8083
public class NToggleSideScreen : SideScreenContent
{
	// Token: 0x0600AAA2 RID: 43682 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AAA3 RID: 43683 RVA: 0x0010ED07 File Offset: 0x0010CF07
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<INToggleSideScreenControl>() != null;
	}

	// Token: 0x0600AAA4 RID: 43684 RVA: 0x004069A4 File Offset: 0x00404BA4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<INToggleSideScreenControl>();
		if (this.target == null)
		{
			return;
		}
		this.titleKey = this.target.SidescreenTitleKey;
		base.gameObject.SetActive(true);
		this.Refresh();
	}

	// Token: 0x0600AAA5 RID: 43685 RVA: 0x004069F0 File Offset: 0x00404BF0
	private void Refresh()
	{
		for (int i = 0; i < Mathf.Max(this.target.Options.Count, this.buttonList.Count); i++)
		{
			if (i >= this.target.Options.Count)
			{
				this.buttonList[i].gameObject.SetActive(false);
			}
			else
			{
				if (i >= this.buttonList.Count)
				{
					KToggle ktoggle = Util.KInstantiateUI<KToggle>(this.buttonPrefab.gameObject, this.ContentContainer, false);
					int idx = i;
					ktoggle.onClick += delegate()
					{
						this.target.QueueSelectedOption(idx);
						this.Refresh();
					};
					this.buttonList.Add(ktoggle);
				}
				this.buttonList[i].GetComponentInChildren<LocText>().text = this.target.Options[i];
				this.buttonList[i].GetComponentInChildren<ToolTip>().toolTip = this.target.Tooltips[i];
				if (this.target.SelectedOption == i && this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				else if (this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = true;
				}
				else
				{
					this.buttonList[i].isOn = false;
					foreach (ImageToggleState imageToggleState in this.buttonList[i].GetComponentsInChildren<ImageToggleState>())
					{
						imageToggleState.SetInactive();
						imageToggleState.SetInactive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				this.buttonList[i].gameObject.SetActive(true);
			}
		}
		this.description.text = this.target.Description;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(this.target.Description));
	}

	// Token: 0x0400861F RID: 34335
	[SerializeField]
	private KToggle buttonPrefab;

	// Token: 0x04008620 RID: 34336
	[SerializeField]
	private LocText description;

	// Token: 0x04008621 RID: 34337
	private INToggleSideScreenControl target;

	// Token: 0x04008622 RID: 34338
	private List<KToggle> buttonList = new List<KToggle>();
}
