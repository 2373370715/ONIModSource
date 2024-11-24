using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F3A RID: 7994
public class ButtonMenuSideScreen : SideScreenContent
{
	// Token: 0x0600A8BB RID: 43195 RVA: 0x003FE15C File Offset: 0x003FC35C
	public override bool IsValidForTarget(GameObject target)
	{
		ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>();
		if (sidescreenButtonControl == null)
		{
			sidescreenButtonControl = target.GetSMI<ISidescreenButtonControl>();
		}
		return sidescreenButtonControl != null && sidescreenButtonControl.SidescreenEnabled();
	}

	// Token: 0x0600A8BC RID: 43196 RVA: 0x0010D94D File Offset: 0x0010BB4D
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].ButtonSideScreenSortOrder();
	}

	// Token: 0x0600A8BD RID: 43197 RVA: 0x0010D96B File Offset: 0x0010BB6B
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = new_target.GetAllSMI<ISidescreenButtonControl>();
		this.targets.AddRange(new_target.GetComponents<ISidescreenButtonControl>());
		this.Refresh();
	}

	// Token: 0x0600A8BE RID: 43198 RVA: 0x0010D9A4 File Offset: 0x0010BBA4
	public GameObject GetHorizontalGroup(int id)
	{
		if (!this.horizontalGroups.ContainsKey(id))
		{
			this.horizontalGroups.Add(id, Util.KInstantiateUI(this.horizontalGroupPrefab, this.buttonContainer.gameObject, true));
		}
		return this.horizontalGroups[id];
	}

	// Token: 0x0600A8BF RID: 43199 RVA: 0x003FE188 File Offset: 0x003FC388
	public void CopyLayoutSettings(LayoutElement to, LayoutElement from)
	{
		to.ignoreLayout = from.ignoreLayout;
		to.minWidth = from.minWidth;
		to.minHeight = from.minHeight;
		to.preferredWidth = from.preferredWidth;
		to.preferredHeight = from.preferredHeight;
		to.flexibleWidth = from.flexibleWidth;
		to.flexibleHeight = from.flexibleHeight;
		to.layoutPriority = from.layoutPriority;
	}

	// Token: 0x0600A8C0 RID: 43200 RVA: 0x003FE1F8 File Offset: 0x003FC3F8
	private void Refresh()
	{
		while (this.liveButtons.Count < this.targets.Count)
		{
			this.liveButtons.Add(Util.KInstantiateUI(this.buttonPrefab.gameObject, this.buttonContainer.gameObject, true));
		}
		foreach (int key in this.horizontalGroups.Keys)
		{
			this.horizontalGroups[key].SetActive(false);
		}
		for (int i = 0; i < this.liveButtons.Count; i++)
		{
			if (i >= this.targets.Count)
			{
				this.liveButtons[i].SetActive(false);
			}
			else
			{
				if (!this.liveButtons[i].activeSelf)
				{
					this.liveButtons[i].SetActive(true);
				}
				int num = this.targets[i].HorizontalGroupID();
				LayoutElement component = this.liveButtons[i].GetComponent<LayoutElement>();
				KButton componentInChildren = this.liveButtons[i].GetComponentInChildren<KButton>();
				ToolTip componentInChildren2 = this.liveButtons[i].GetComponentInChildren<ToolTip>();
				LocText componentInChildren3 = this.liveButtons[i].GetComponentInChildren<LocText>();
				if (num >= 0)
				{
					GameObject horizontalGroup = this.GetHorizontalGroup(num);
					horizontalGroup.SetActive(true);
					this.liveButtons[i].transform.SetParent(horizontalGroup.transform, false);
					this.CopyLayoutSettings(component, this.horizontalButtonPrefab);
				}
				else
				{
					this.liveButtons[i].transform.SetParent(this.buttonContainer, false);
					this.CopyLayoutSettings(component, this.buttonPrefab);
				}
				componentInChildren.isInteractable = this.targets[i].SidescreenButtonInteractable();
				componentInChildren.ClearOnClick();
				componentInChildren.onClick += this.targets[i].OnSidescreenButtonPressed;
				componentInChildren.onClick += this.Refresh;
				componentInChildren3.SetText(this.targets[i].SidescreenButtonText);
				componentInChildren2.SetSimpleTooltip(this.targets[i].SidescreenButtonTooltip);
			}
		}
	}

	// Token: 0x040084A8 RID: 33960
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	// Token: 0x040084A9 RID: 33961
	public LayoutElement buttonPrefab;

	// Token: 0x040084AA RID: 33962
	public LayoutElement horizontalButtonPrefab;

	// Token: 0x040084AB RID: 33963
	public GameObject horizontalGroupPrefab;

	// Token: 0x040084AC RID: 33964
	public RectTransform buttonContainer;

	// Token: 0x040084AD RID: 33965
	private List<GameObject> liveButtons = new List<GameObject>();

	// Token: 0x040084AE RID: 33966
	private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();

	// Token: 0x040084AF RID: 33967
	private List<ISidescreenButtonControl> targets;
}
