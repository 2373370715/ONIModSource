using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class DetailTabHeader : KMonoBehaviour
{
	private Dictionary<string, MultiToggle> tabs = new Dictionary<string, MultiToggle>();

	private string selectedTabID;

	[SerializeField]
	private GameObject tabPrefab;

	[SerializeField]
	private GameObject tabContainer;

	[SerializeField]
	private GameObject panelContainer;

	[Header("Screen Prefabs")]
	[SerializeField]
	private GameObject simpleInfoScreen;

	[SerializeField]
	private GameObject minionPersonalityPanel;

	[SerializeField]
	private GameObject buildingInfoPanel;

	[SerializeField]
	private GameObject additionalDetailsPanel;

	[SerializeField]
	private GameObject cosmeticsPanel;

	[SerializeField]
	private GameObject materialPanel;

	private DetailsScreen detailsScreen;

	private Dictionary<string, TargetPanel> tabPanels = new Dictionary<string, TargetPanel>();

	public TargetPanel ActivePanel
	{
		get
		{
			if (tabPanels.ContainsKey(selectedTabID))
			{
				return tabPanels[selectedTabID];
			}
			return null;
		}
	}

	public void Init()
	{
		detailsScreen = DetailsScreen.Instance;
		MakeTab("SIMPLEINFO", UI.DETAILTABS.SIMPLEINFO.NAME, Assets.GetSprite("icon_display_screen_status"), UI.DETAILTABS.SIMPLEINFO.TOOLTIP, simpleInfoScreen);
		MakeTab("PERSONALITY", UI.DETAILTABS.PERSONALITY.NAME, Assets.GetSprite("icon_display_screen_bio"), UI.DETAILTABS.PERSONALITY.TOOLTIP, minionPersonalityPanel);
		MakeTab("BUILDINGCHORES", UI.DETAILTABS.BUILDING_CHORES.NAME, Assets.GetSprite("icon_display_screen_errands"), UI.DETAILTABS.BUILDING_CHORES.TOOLTIP, buildingInfoPanel);
		MakeTab("DETAILS", UI.DETAILTABS.DETAILS.NAME, Assets.GetSprite("icon_display_screen_properties"), UI.DETAILTABS.DETAILS.TOOLTIP, additionalDetailsPanel);
		ChangeToDefaultTab();
	}

	private void MakeTabContents(GameObject panelToActivate)
	{
	}

	private void MakeTab(string id, string label, Sprite sprite, string tooltip, GameObject panelToActivate)
	{
		GameObject obj = Util.KInstantiateUI(tabPrefab, tabContainer, force_active: true);
		obj.name = "tab: " + id;
		obj.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
		HierarchyReferences component = obj.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = sprite;
		component.GetReference<LocText>("label").text = label;
		MultiToggle component2 = obj.GetComponent<MultiToggle>();
		GameObject obj2 = Util.KInstantiateUI(panelToActivate, panelContainer.gameObject, force_active: true);
		TargetPanel component3 = obj2.GetComponent<TargetPanel>();
		component3.SetTarget(detailsScreen.target);
		tabPanels.Add(id, component3);
		string targetTab = id;
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, (System.Action)delegate
		{
			ChangeTab(targetTab);
		});
		tabs.Add(id, component2);
		obj2.SetActive(value: false);
	}

	private void ChangeTab(string id)
	{
		selectedTabID = id;
		foreach (KeyValuePair<string, MultiToggle> tab in tabs)
		{
			tab.Value.ChangeState((tab.Key == selectedTabID) ? 1 : 0);
		}
		foreach (KeyValuePair<string, TargetPanel> tabPanel in tabPanels)
		{
			if (tabPanel.Key == id)
			{
				tabPanel.Value.gameObject.SetActive(value: true);
				tabPanel.Value.SetTarget(detailsScreen.target);
			}
			else
			{
				tabPanel.Value.SetTarget(null);
				tabPanel.Value.gameObject.SetActive(value: false);
			}
		}
	}

	private void ChangeToDefaultTab()
	{
		ChangeTab("SIMPLEINFO");
	}

	public void RefreshTabDisplayForTarget(GameObject target)
	{
		foreach (KeyValuePair<string, TargetPanel> tabPanel in tabPanels)
		{
			tabs[tabPanel.Key].gameObject.SetActive(tabPanel.Value.IsValidForTarget(target));
		}
		if (tabPanels[selectedTabID].IsValidForTarget(target))
		{
			ChangeTab(selectedTabID);
		}
		else
		{
			ChangeToDefaultTab();
		}
	}
}
