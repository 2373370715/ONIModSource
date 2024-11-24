using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMenuSideScreen : SideScreenContent
{
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	public LayoutElement buttonPrefab;

	public LayoutElement horizontalButtonPrefab;

	public GameObject horizontalGroupPrefab;

	public RectTransform buttonContainer;

	private List<GameObject> liveButtons = new List<GameObject>();

	private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();

	private List<ISidescreenButtonControl> targets;

	public override bool IsValidForTarget(GameObject target)
	{
		ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>();
		if (sidescreenButtonControl == null)
		{
			sidescreenButtonControl = target.GetSMI<ISidescreenButtonControl>();
		}
		return sidescreenButtonControl?.SidescreenEnabled() ?? false;
	}

	public override int GetSideScreenSortOrder()
	{
		if (targets == null)
		{
			return 20;
		}
		return targets[0].ButtonSideScreenSortOrder();
	}

	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			Debug.LogError("Invalid gameObject received");
			return;
		}
		targets = new_target.GetAllSMI<ISidescreenButtonControl>();
		targets.AddRange(new_target.GetComponents<ISidescreenButtonControl>());
		Refresh();
	}

	public GameObject GetHorizontalGroup(int id)
	{
		if (!horizontalGroups.ContainsKey(id))
		{
			horizontalGroups.Add(id, Util.KInstantiateUI(horizontalGroupPrefab, buttonContainer.gameObject, force_active: true));
		}
		return horizontalGroups[id];
	}

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

	private void Refresh()
	{
		while (liveButtons.Count < targets.Count)
		{
			liveButtons.Add(Util.KInstantiateUI(buttonPrefab.gameObject, buttonContainer.gameObject, force_active: true));
		}
		foreach (int key in horizontalGroups.Keys)
		{
			horizontalGroups[key].SetActive(value: false);
		}
		for (int i = 0; i < liveButtons.Count; i++)
		{
			if (i >= targets.Count)
			{
				liveButtons[i].SetActive(value: false);
				continue;
			}
			if (!liveButtons[i].activeSelf)
			{
				liveButtons[i].SetActive(value: true);
			}
			int num = targets[i].HorizontalGroupID();
			LayoutElement component = liveButtons[i].GetComponent<LayoutElement>();
			KButton componentInChildren = liveButtons[i].GetComponentInChildren<KButton>();
			ToolTip componentInChildren2 = liveButtons[i].GetComponentInChildren<ToolTip>();
			LocText componentInChildren3 = liveButtons[i].GetComponentInChildren<LocText>();
			if (num >= 0)
			{
				GameObject horizontalGroup = GetHorizontalGroup(num);
				horizontalGroup.SetActive(value: true);
				liveButtons[i].transform.SetParent(horizontalGroup.transform, worldPositionStays: false);
				CopyLayoutSettings(component, horizontalButtonPrefab);
			}
			else
			{
				liveButtons[i].transform.SetParent(buttonContainer, worldPositionStays: false);
				CopyLayoutSettings(component, buttonPrefab);
			}
			componentInChildren.isInteractable = targets[i].SidescreenButtonInteractable();
			componentInChildren.ClearOnClick();
			componentInChildren.onClick += targets[i].OnSidescreenButtonPressed;
			componentInChildren.onClick += Refresh;
			componentInChildren3.SetText(targets[i].SidescreenButtonText);
			componentInChildren2.SetSimpleTooltip(targets[i].SidescreenButtonTooltip);
		}
	}
}
