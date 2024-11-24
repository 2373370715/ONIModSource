using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CA6 RID: 7334
public class DetailTabHeader : KMonoBehaviour
{
	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x06009909 RID: 39177 RVA: 0x001038FE File Offset: 0x00101AFE
	public TargetPanel ActivePanel
	{
		get
		{
			if (this.tabPanels.ContainsKey(this.selectedTabID))
			{
				return this.tabPanels[this.selectedTabID];
			}
			return null;
		}
	}

	// Token: 0x0600990A RID: 39178 RVA: 0x003B3264 File Offset: 0x003B1464
	public void Init()
	{
		this.detailsScreen = DetailsScreen.Instance;
		this.MakeTab("SIMPLEINFO", UI.DETAILTABS.SIMPLEINFO.NAME, Assets.GetSprite("icon_display_screen_status"), UI.DETAILTABS.SIMPLEINFO.TOOLTIP, this.simpleInfoScreen);
		this.MakeTab("PERSONALITY", UI.DETAILTABS.PERSONALITY.NAME, Assets.GetSprite("icon_display_screen_bio"), UI.DETAILTABS.PERSONALITY.TOOLTIP, this.minionPersonalityPanel);
		this.MakeTab("BUILDINGCHORES", UI.DETAILTABS.BUILDING_CHORES.NAME, Assets.GetSprite("icon_display_screen_errands"), UI.DETAILTABS.BUILDING_CHORES.TOOLTIP, this.buildingInfoPanel);
		this.MakeTab("DETAILS", UI.DETAILTABS.DETAILS.NAME, Assets.GetSprite("icon_display_screen_properties"), UI.DETAILTABS.DETAILS.TOOLTIP, this.additionalDetailsPanel);
		this.ChangeToDefaultTab();
	}

	// Token: 0x0600990B RID: 39179 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void MakeTabContents(GameObject panelToActivate)
	{
	}

	// Token: 0x0600990C RID: 39180 RVA: 0x003B3354 File Offset: 0x003B1554
	private void MakeTab(string id, string label, Sprite sprite, string tooltip, GameObject panelToActivate)
	{
		GameObject gameObject = Util.KInstantiateUI(this.tabPrefab, this.tabContainer, true);
		gameObject.name = "tab: " + id;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = sprite;
		component.GetReference<LocText>("label").text = label;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		GameObject gameObject2 = Util.KInstantiateUI(panelToActivate, this.panelContainer.gameObject, true);
		TargetPanel component3 = gameObject2.GetComponent<TargetPanel>();
		component3.SetTarget(this.detailsScreen.target);
		this.tabPanels.Add(id, component3);
		string targetTab = id;
		MultiToggle multiToggle = component2;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ChangeTab(targetTab);
		}));
		this.tabs.Add(id, component2);
		gameObject2.SetActive(false);
	}

	// Token: 0x0600990D RID: 39181 RVA: 0x003B3440 File Offset: 0x003B1640
	private void ChangeTab(string id)
	{
		this.selectedTabID = id;
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.tabs)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.selectedTabID) ? 1 : 0);
		}
		foreach (KeyValuePair<string, TargetPanel> keyValuePair2 in this.tabPanels)
		{
			if (keyValuePair2.Key == id)
			{
				keyValuePair2.Value.gameObject.SetActive(true);
				keyValuePair2.Value.SetTarget(this.detailsScreen.target);
			}
			else
			{
				keyValuePair2.Value.SetTarget(null);
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600990E RID: 39182 RVA: 0x00103926 File Offset: 0x00101B26
	private void ChangeToDefaultTab()
	{
		this.ChangeTab("SIMPLEINFO");
	}

	// Token: 0x0600990F RID: 39183 RVA: 0x003B354C File Offset: 0x003B174C
	public void RefreshTabDisplayForTarget(GameObject target)
	{
		foreach (KeyValuePair<string, TargetPanel> keyValuePair in this.tabPanels)
		{
			this.tabs[keyValuePair.Key].gameObject.SetActive(keyValuePair.Value.IsValidForTarget(target));
		}
		if (this.tabPanels[this.selectedTabID].IsValidForTarget(target))
		{
			this.ChangeTab(this.selectedTabID);
			return;
		}
		this.ChangeToDefaultTab();
	}

	// Token: 0x04007749 RID: 30537
	private Dictionary<string, MultiToggle> tabs = new Dictionary<string, MultiToggle>();

	// Token: 0x0400774A RID: 30538
	private string selectedTabID;

	// Token: 0x0400774B RID: 30539
	[SerializeField]
	private GameObject tabPrefab;

	// Token: 0x0400774C RID: 30540
	[SerializeField]
	private GameObject tabContainer;

	// Token: 0x0400774D RID: 30541
	[SerializeField]
	private GameObject panelContainer;

	// Token: 0x0400774E RID: 30542
	[Header("Screen Prefabs")]
	[SerializeField]
	private GameObject simpleInfoScreen;

	// Token: 0x0400774F RID: 30543
	[SerializeField]
	private GameObject minionPersonalityPanel;

	// Token: 0x04007750 RID: 30544
	[SerializeField]
	private GameObject buildingInfoPanel;

	// Token: 0x04007751 RID: 30545
	[SerializeField]
	private GameObject additionalDetailsPanel;

	// Token: 0x04007752 RID: 30546
	[SerializeField]
	private GameObject cosmeticsPanel;

	// Token: 0x04007753 RID: 30547
	[SerializeField]
	private GameObject materialPanel;

	// Token: 0x04007754 RID: 30548
	private DetailsScreen detailsScreen;

	// Token: 0x04007755 RID: 30549
	private Dictionary<string, TargetPanel> tabPanels = new Dictionary<string, TargetPanel>();
}
