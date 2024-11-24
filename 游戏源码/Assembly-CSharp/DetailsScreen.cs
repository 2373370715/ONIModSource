using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CAB RID: 7339
public class DetailsScreen : KTabMenu
{
	// Token: 0x06009926 RID: 39206 RVA: 0x00103A0D File Offset: 0x00101C0D
	public static void DestroyInstance()
	{
		DetailsScreen.Instance = null;
	}

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x06009927 RID: 39207 RVA: 0x00103A15 File Offset: 0x00101C15
	// (set) Token: 0x06009928 RID: 39208 RVA: 0x00103A1D File Offset: 0x00101C1D
	public GameObject target { get; private set; }

	// Token: 0x06009929 RID: 39209 RVA: 0x003B3808 File Offset: 0x003B1A08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.SortScreenOrder();
		base.ConsumeMouseScroll = true;
		global::Debug.Assert(DetailsScreen.Instance == null);
		DetailsScreen.Instance = this;
		this.InitiateSidescreenTabs();
		this.DeactivateSideContent();
		this.Show(false);
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		this.tabHeader.Init();
	}

	// Token: 0x0600992A RID: 39210 RVA: 0x003B3880 File Offset: 0x003B1A80
	public bool CanObjectDisplayTabOfType(GameObject obj, DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab.ValidateTarget(obj);
			}
		}
		return false;
	}

	// Token: 0x0600992B RID: 39211 RVA: 0x003B38BC File Offset: 0x003B1ABC
	public DetailsScreen.SidescreenTab GetTabOfType(DetailsScreen.SidescreenTabTypes type)
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			if (sidescreenTab.type == type)
			{
				return sidescreenTab;
			}
		}
		return null;
	}

	// Token: 0x0600992C RID: 39212 RVA: 0x003B38F4 File Offset: 0x003B1AF4
	public void InitiateSidescreenTabs()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			sidescreenTab.Initiate(this.original_tab, this.original_tab_body, delegate(DetailsScreen.SidescreenTab _tab)
			{
				this.SelectSideScreenTab(_tab.type);
			});
			switch (sidescreenTab.type)
			{
			case DetailsScreen.SidescreenTabTypes.Errands:
				sidescreenTab.ValidateTargetCallback = ((GameObject target, DetailsScreen.SidescreenTab _tab) => target.GetComponent<MinionIdentity>() != null);
				break;
			case DetailsScreen.SidescreenTabTypes.Material:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					Reconstructable component = target.GetComponent<Reconstructable>();
					return component != null && component.AllowReconstruct;
				};
				break;
			case DetailsScreen.SidescreenTabTypes.Blueprints:
				sidescreenTab.ValidateTargetCallback = delegate(GameObject target, DetailsScreen.SidescreenTab _tab)
				{
					UnityEngine.Object component = target.GetComponent<MinionIdentity>();
					BuildingFacade component2 = target.GetComponent<BuildingFacade>();
					return component != null || component2 != null;
				};
				break;
			}
		}
	}

	// Token: 0x0600992D RID: 39213 RVA: 0x003B39D4 File Offset: 0x003B1BD4
	private void OnSelectObject(object data)
	{
		if (data == null)
		{
			this.previouslyActiveTab = -1;
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
			return;
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		if (!(component == null) && !(this.previousTargetID != component.PrefabID()))
		{
			this.SelectSideScreenTab(this.selectedSidescreenTabID);
			return;
		}
		if (component != null && component.GetComponent<MinionIdentity>())
		{
			this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Errands);
			return;
		}
		this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
	}

	// Token: 0x0600992E RID: 39214 RVA: 0x003B3A50 File Offset: 0x003B1C50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CodexEntryButton.onClick += this.CodexEntryButton_OnClick;
		this.PinResourceButton.onClick += this.PinResourceButton_OnClick;
		this.CloseButton.onClick += this.DeselectAndClose;
		this.TabTitle.OnNameChanged += this.OnNameChanged;
		this.TabTitle.OnStartedEditing += this.OnStartedEditing;
		this.sideScreen2.SetActive(false);
		base.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
	}

	// Token: 0x0600992F RID: 39215 RVA: 0x00103A26 File Offset: 0x00101C26
	private void OnStartedEditing()
	{
		base.isEditing = true;
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06009930 RID: 39216 RVA: 0x003B3AF4 File Offset: 0x003B1CF4
	private void OnNameChanged(string newName)
	{
		base.isEditing = false;
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		MinionIdentity component = this.target.GetComponent<MinionIdentity>();
		UserNameable component2 = this.target.GetComponent<UserNameable>();
		ClustercraftExteriorDoor component3 = this.target.GetComponent<ClustercraftExteriorDoor>();
		CommandModule component4 = this.target.GetComponent<CommandModule>();
		if (component != null)
		{
			component.SetName(newName);
		}
		else if (component4 != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
		}
		else if (component3 != null)
		{
			component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
		}
		else if (component2 != null)
		{
			component2.SetName(newName);
		}
		this.TabTitle.UpdateRenameTooltip(this.target);
	}

	// Token: 0x06009931 RID: 39217 RVA: 0x00103A39 File Offset: 0x00101C39
	protected override void OnDeactivate()
	{
		if (this.target != null && this.setRocketTitleHandle != -1)
		{
			this.target.Unsubscribe(this.setRocketTitleHandle);
		}
		this.setRocketTitleHandle = -1;
		this.DeactivateSideContent();
		base.OnDeactivate();
	}

	// Token: 0x06009932 RID: 39218 RVA: 0x00103A76 File Offset: 0x00101C76
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.DeactivateSideContent();
		}
		else
		{
			this.MaskSideContent(false);
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		}
		base.OnShow(show);
	}

	// Token: 0x06009933 RID: 39219 RVA: 0x00103AA6 File Offset: 0x00101CA6
	protected override void OnCmpDisable()
	{
		this.DeactivateSideContent();
		base.OnCmpDisable();
	}

	// Token: 0x06009934 RID: 39220 RVA: 0x00103AB4 File Offset: 0x00101CB4
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.isEditing && this.target != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.DeselectAndClose();
		}
	}

	// Token: 0x06009935 RID: 39221 RVA: 0x003B3BB4 File Offset: 0x003B1DB4
	private static Component GetComponent(GameObject go, string name)
	{
		Type type = Type.GetType(name);
		Component component;
		if (type != null)
		{
			component = go.GetComponent(type);
		}
		else
		{
			component = go.GetComponent(name);
		}
		return component;
	}

	// Token: 0x06009936 RID: 39222 RVA: 0x003B3BE8 File Offset: 0x003B1DE8
	private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
	{
		if (excluded_tags == null || excluded_tags.Length == 0)
		{
			return false;
		}
		bool result = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		foreach (Tag b in excluded_tags)
		{
			if (component.PrefabTag == b)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06009937 RID: 39223 RVA: 0x003B3C30 File Offset: 0x003B1E30
	private string CodexEntryButton_GetCodexId()
	{
		string text = "";
		global::Debug.Assert(this.target != null, "Details Screen has no target");
		KSelectable component = this.target.GetComponent<KSelectable>();
		DebugUtil.AssertArgs(component != null, new object[]
		{
			"Details Screen target is not a KSelectable",
			this.target
		});
		CellSelectionObject component2 = component.GetComponent<CellSelectionObject>();
		BuildingUnderConstruction component3 = component.GetComponent<BuildingUnderConstruction>();
		CreatureBrain component4 = component.GetComponent<CreatureBrain>();
		PlantableSeed component5 = component.GetComponent<PlantableSeed>();
		BudUprootedMonitor component6 = component.GetComponent<BudUprootedMonitor>();
		if (component2 != null)
		{
			text = CodexCache.FormatLinkID(component2.element.id.ToString());
		}
		else if (component3 != null)
		{
			text = CodexCache.FormatLinkID(component3.Def.PrefabID);
		}
		else if (component4 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("BABY", "");
		}
		else if (component5 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("SEED", "");
		}
		else if (component6 != null)
		{
			if (component6.parentObject.Get() != null)
			{
				text = CodexCache.FormatLinkID(component6.parentObject.Get().PrefabID().ToString());
			}
			else if (component6.GetComponent<TreeBud>() != null)
			{
				text = CodexCache.FormatLinkID(component6.GetComponent<TreeBud>().buddingTrunk.Get().PrefabID().ToString());
			}
		}
		else
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
		}
		if (CodexCache.entries.ContainsKey(text) || CodexCache.FindSubEntry(text) != null)
		{
			return text;
		}
		return "";
	}

	// Token: 0x06009938 RID: 39224 RVA: 0x003B3E28 File Offset: 0x003B2028
	private void CodexEntryButton_Refresh()
	{
		string a = this.CodexEntryButton_GetCodexId();
		this.CodexEntryButton.isInteractable = (a != "");
		this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip(this.CodexEntryButton.isInteractable ? UI.TOOLTIPS.OPEN_CODEX_ENTRY : UI.TOOLTIPS.NO_CODEX_ENTRY);
	}

	// Token: 0x06009939 RID: 39225 RVA: 0x003B3E80 File Offset: 0x003B2080
	public void CodexEntryButton_OnClick()
	{
		string text = this.CodexEntryButton_GetCodexId();
		if (text != "")
		{
			ManagementMenu.Instance.OpenCodexToEntry(text, null);
		}
	}

	// Token: 0x0600993A RID: 39226 RVA: 0x003B3EB0 File Offset: 0x003B20B0
	private bool PinResourceButton_TryGetResourceTagAndProperName(out Tag targetTag, out string targetProperName)
	{
		KPrefabID component = this.target.GetComponent<KPrefabID>();
		if (component != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component.PrefabTag))
		{
			targetTag = component.PrefabTag;
			targetProperName = component.GetProperName();
			return true;
		}
		CellSelectionObject component2 = this.target.GetComponent<CellSelectionObject>();
		if (component2 != null && DetailsScreen.<PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(component2.element.tag))
		{
			targetTag = component2.element.tag;
			targetProperName = component2.GetProperName();
			return true;
		}
		targetTag = null;
		targetProperName = null;
		return false;
	}

	// Token: 0x0600993B RID: 39227 RVA: 0x003B3F48 File Offset: 0x003B2148
	private void PinResourceButton_Refresh()
	{
		Tag tag;
		string arg;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out arg))
		{
			ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag);
			GameUtil.MeasureUnit measureUnit;
			if (!AllResourcesScreen.Instance.units.TryGetValue(tag, out measureUnit))
			{
				measureUnit = GameUtil.MeasureUnit.quantity;
			}
			string arg2;
			switch (measureUnit)
			{
			case GameUtil.MeasureUnit.mass:
				arg2 = GameUtil.GetFormattedMass(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				break;
			case GameUtil.MeasureUnit.kcal:
				arg2 = GameUtil.GetFormattedCalories(WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true), GameUtil.TimeSlice.None, true);
				break;
			case GameUtil.MeasureUnit.quantity:
				arg2 = GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, false), GameUtil.TimeSlice.None, true, "");
				break;
			default:
				arg2 = "";
				break;
			}
			this.PinResourceButton.gameObject.SetActive(true);
			this.PinResourceButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.TOOLTIPS.OPEN_RESOURCE_INFO, arg2, arg));
			return;
		}
		this.PinResourceButton.gameObject.SetActive(false);
	}

	// Token: 0x0600993C RID: 39228 RVA: 0x003B406C File Offset: 0x003B226C
	public void PinResourceButton_OnClick()
	{
		Tag tag;
		string text;
		if (this.PinResourceButton_TryGetResourceTagAndProperName(out tag, out text))
		{
			AllResourcesScreen.Instance.SetFilter(UI.StripLinkFormatting(text));
			AllResourcesScreen.Instance.Show(true);
		}
	}

	// Token: 0x0600993D RID: 39229 RVA: 0x003B40A0 File Offset: 0x003B22A0
	public void OnRefreshData(object obj)
	{
		this.RefreshTitle();
		for (int i = 0; i < this.tabs.Count; i++)
		{
			if (this.tabs[i].gameObject.activeInHierarchy)
			{
				this.tabs[i].Trigger(-1514841199, obj);
			}
		}
	}

	// Token: 0x0600993E RID: 39230 RVA: 0x003B40F8 File Offset: 0x003B22F8
	public void Refresh(GameObject go)
	{
		if (this.screens == null)
		{
			return;
		}
		if (this.target != go)
		{
			if (this.setRocketTitleHandle != -1)
			{
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			}
			if (this.target != null)
			{
				if (this.target.GetComponent<KPrefabID>() != null)
				{
					this.previousTargetID = this.target.GetComponent<KPrefabID>().PrefabID();
				}
				else
				{
					this.previousTargetID = null;
				}
			}
		}
		this.target = go;
		this.sortedSideScreens.Clear();
		CellSelectionObject component = this.target.GetComponent<CellSelectionObject>();
		if (component)
		{
			component.OnObjectSelected(null);
		}
		this.UpdateTitle();
		this.tabHeader.RefreshTabDisplayForTarget(this.target);
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			bool flag = false;
			foreach (DetailsScreen.SideScreenRef sideScreenRef in this.sideScreens)
			{
				if (!sideScreenRef.screenPrefab.IsValidForTarget(this.target))
				{
					if (sideScreenRef.screenInstance != null && sideScreenRef.screenInstance.gameObject.activeSelf)
					{
						sideScreenRef.screenInstance.gameObject.SetActive(false);
					}
				}
				else
				{
					flag = true;
					if (sideScreenRef.screenInstance == null)
					{
						DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(sideScreenRef.tab);
						sideScreenRef.screenInstance = global::Util.KInstantiateUI<SideScreenContent>(sideScreenRef.screenPrefab.gameObject, tabOfType.bodyInstance, false);
					}
					if (!this.sideScreen.activeSelf)
					{
						this.sideScreen.SetActive(true);
					}
					sideScreenRef.screenInstance.SetTarget(this.target);
					sideScreenRef.screenInstance.Show(true);
					int sideScreenSortOrder = sideScreenRef.screenInstance.GetSideScreenSortOrder();
					this.sortedSideScreens.Add(new KeyValuePair<DetailsScreen.SideScreenRef, int>(sideScreenRef, sideScreenSortOrder));
				}
			}
			if (!flag)
			{
				if (!this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Material) && !this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Blueprints))
				{
					this.sideScreen.SetActive(false);
				}
				else
				{
					this.sideScreen.SetActive(true);
				}
			}
		}
		this.sortedSideScreens.Sort(delegate(KeyValuePair<DetailsScreen.SideScreenRef, int> x, KeyValuePair<DetailsScreen.SideScreenRef, int> y)
		{
			if (x.Value <= y.Value)
			{
				return 1;
			}
			return -1;
		});
		for (int i = 0; i < this.sortedSideScreens.Count; i++)
		{
			this.sortedSideScreens[i].Key.screenInstance.transform.SetSiblingIndex(i);
		}
		for (int j = 0; j < this.sidescreenTabs.Length; j++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[j];
			tab.RepositionTitle();
			KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> t) => t.Key.tab == tab.type);
			tab.SetNoConfigMessageVisibility(keyValuePair.Key == null);
		}
		this.RefreshTitle();
	}

	// Token: 0x0600993F RID: 39231 RVA: 0x003B4424 File Offset: 0x003B2624
	public void RefreshTitle()
	{
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab tab = this.sidescreenTabs[i];
			if (tab.IsVisible)
			{
				KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((KeyValuePair<DetailsScreen.SideScreenRef, int> match) => match.Key.tab == tab.type);
				if (keyValuePair.Key != null)
				{
					tab.SetTitleVisibility(true);
					tab.SetTitle(keyValuePair.Key.screenInstance.GetTitle());
				}
				else
				{
					tab.SetTitle(UI.UISIDESCREENS.NOCONFIG.TITLE);
					tab.SetTitleVisibility(tab.type == DetailsScreen.SidescreenTabTypes.Config || tab.type == DetailsScreen.SidescreenTabTypes.Errands);
				}
			}
		}
	}

	// Token: 0x06009940 RID: 39232 RVA: 0x00103AE0 File Offset: 0x00101CE0
	private void SelectSideScreenTab(DetailsScreen.SidescreenTabTypes tabID)
	{
		this.selectedSidescreenTabID = tabID;
		this.RefreshSideScreenTabs();
	}

	// Token: 0x06009941 RID: 39233 RVA: 0x003B44F8 File Offset: 0x003B26F8
	private void RefreshSideScreenTabs()
	{
		int num = 1;
		for (int i = 0; i < this.sidescreenTabs.Length; i++)
		{
			DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[i];
			bool flag = sidescreenTab.ValidateTarget(this.target);
			sidescreenTab.SetVisible(flag);
			sidescreenTab.SetSelected(this.selectedSidescreenTabID == sidescreenTab.type);
			num += (flag ? 1 : 0);
		}
		this.RefreshTitle();
		DetailsScreen.SidescreenTabTypes sidescreenTabTypes = this.selectedSidescreenTabID;
		if (sidescreenTabTypes != DetailsScreen.SidescreenTabTypes.Material)
		{
			if (sidescreenTabTypes == DetailsScreen.SidescreenTabTypes.Blueprints)
			{
				CosmeticsPanel reference = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints).bodyInstance.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
				reference.SetTarget(this.target);
				reference.Refresh();
			}
		}
		else
		{
			this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material).bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(this.target);
		}
		this.sidescreenTabHeader.SetActive(num > 1);
	}

	// Token: 0x06009942 RID: 39234 RVA: 0x003B45C8 File Offset: 0x003B27C8
	public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
	{
		this.ClearSecondarySideScreen();
		if (this.instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
		{
			this.activeSideScreen2 = this.instantiatedSecondarySideScreens[secondaryPrefab];
			this.activeSideScreen2.gameObject.SetActive(true);
		}
		else
		{
			this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
			this.activeSideScreen2.Activate();
			this.instantiatedSecondarySideScreens.Add(secondaryPrefab, this.activeSideScreen2);
		}
		this.sideScreen2Title.text = title;
		this.sideScreen2.SetActive(true);
		return this.activeSideScreen2;
	}

	// Token: 0x06009943 RID: 39235 RVA: 0x00103AEF File Offset: 0x00101CEF
	public void ClearSecondarySideScreen()
	{
		if (this.activeSideScreen2 != null)
		{
			this.activeSideScreen2.gameObject.SetActive(false);
			this.activeSideScreen2 = null;
		}
		this.sideScreen2.SetActive(false);
	}

	// Token: 0x06009944 RID: 39236 RVA: 0x003B4668 File Offset: 0x003B2868
	public void DeactivateSideContent()
	{
		if (SideDetailsScreen.Instance != null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
		{
			SideDetailsScreen.Instance.Show(false);
		}
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			this.sideScreens.ForEach(delegate(DetailsScreen.SideScreenRef scn)
			{
				if (scn.screenInstance != null)
				{
					scn.screenInstance.ClearTarget();
					scn.screenInstance.Show(false);
				}
			});
		}
		DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material);
		DetailsScreen.SidescreenTab tabOfType2 = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
		tabOfType.bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(null);
		tabOfType2.bodyInstance.GetComponentInChildren<CosmeticsPanel>().SetTarget(null);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect, STOP_MODE.ALLOWFADEOUT);
		this.sideScreen.SetActive(false);
	}

	// Token: 0x06009945 RID: 39237 RVA: 0x00103B23 File Offset: 0x00101D23
	public void MaskSideContent(bool hide)
	{
		if (hide)
		{
			this.sideScreen.transform.localScale = Vector3.zero;
			return;
		}
		this.sideScreen.transform.localScale = Vector3.one;
	}

	// Token: 0x06009946 RID: 39238 RVA: 0x003B4730 File Offset: 0x003B2930
	public void DeselectAndClose()
	{
		if (base.gameObject.activeInHierarchy)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
		}
		if (this.GetActiveTab() != null)
		{
			this.GetActiveTab().SetTarget(null);
		}
		SelectTool.Instance.Select(null, false);
		ClusterMapSelectTool.Instance.Select(null, false);
		if (this.target == null)
		{
			return;
		}
		this.target = null;
		this.previousTargetID = null;
		this.DeactivateSideContent();
		this.Show(false);
	}

	// Token: 0x06009947 RID: 39239 RVA: 0x00103B53 File Offset: 0x00101D53
	private void SortScreenOrder()
	{
		Array.Sort<DetailsScreen.Screens>(this.screens, (DetailsScreen.Screens x, DetailsScreen.Screens y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority));
	}

	// Token: 0x06009948 RID: 39240 RVA: 0x003B47BC File Offset: 0x003B29BC
	public void UpdatePortrait(GameObject target)
	{
		KSelectable component = target.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		this.TabTitle.portrait.ClearPortrait();
		Building component2 = component.GetComponent<Building>();
		if (component2)
		{
			Sprite uisprite = component2.Def.GetUISprite("ui", false);
			if (uisprite != null)
			{
				this.TabTitle.portrait.SetPortrait(uisprite);
				return;
			}
		}
		if (target.GetComponent<MinionIdentity>())
		{
			this.TabTitle.SetPortrait(component.gameObject);
			return;
		}
		Edible component3 = target.GetComponent<Edible>();
		if (component3 != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim);
			return;
		}
		PrimaryElement component4 = target.GetComponent<PrimaryElement>();
		if (component4 != null)
		{
			this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim, "ui", false, ""));
			return;
		}
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		if (component5 != null)
		{
			string animName = component5.element.IsSolid ? "ui" : component5.element.substance.name;
			Sprite uispriteFromMultiObjectAnim2 = Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName, false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim2);
			return;
		}
	}

	// Token: 0x06009949 RID: 39241 RVA: 0x00103B7F File Offset: 0x00101D7F
	public bool CompareTargetWith(GameObject compare)
	{
		return this.target == compare;
	}

	// Token: 0x0600994A RID: 39242 RVA: 0x003B4940 File Offset: 0x003B2B40
	public void UpdateTitle()
	{
		this.CodexEntryButton_Refresh();
		this.PinResourceButton_Refresh();
		this.TabTitle.SetTitle(this.target.GetProperName());
		if (this.TabTitle != null)
		{
			this.TabTitle.SetTitle(this.target.GetProperName());
			MinionIdentity minionIdentity = null;
			UserNameable x = null;
			ClustercraftExteriorDoor clustercraftExteriorDoor = null;
			CommandModule commandModule = null;
			if (this.target != null)
			{
				minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
				x = this.target.gameObject.GetComponent<UserNameable>();
				clustercraftExteriorDoor = this.target.gameObject.GetComponent<ClustercraftExteriorDoor>();
				commandModule = this.target.gameObject.GetComponent<CommandModule>();
			}
			if (minionIdentity != null)
			{
				this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle(), "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (x != null)
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (commandModule != null)
			{
				this.TrySetRocketTitle(commandModule);
			}
			else if (clustercraftExteriorDoor != null)
			{
				this.TrySetRocketTitle(clustercraftExteriorDoor);
			}
			else
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(false);
			}
			this.TabTitle.UpdateRenameTooltip(this.target);
		}
	}

	// Token: 0x0600994B RID: 39243 RVA: 0x003B4AA4 File Offset: 0x003B2CA4
	private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor)
	{
		if (clusterCraftDoor2.HasTargetWorld())
		{
			WorldContainer targetWorld = clusterCraftDoor2.GetTargetWorld();
			this.TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
			this.TabTitle.SetUserEditable(true);
			this.TabTitle.SetSubText(this.target.GetProperName(), "");
			this.setRocketTitleHandle = -1;
			return;
		}
		if (this.setRocketTitleHandle == -1)
		{
			this.setRocketTitleHandle = this.target.Subscribe(-71801987, delegate(object clusterCraftDoor)
			{
				this.OnRefreshData(null);
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			});
		}
	}

	// Token: 0x0600994C RID: 39244 RVA: 0x003B4B30 File Offset: 0x003B2D30
	private void TrySetRocketTitle(CommandModule commandModule)
	{
		if (commandModule != null)
		{
			this.TabTitle.SetTitle(SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName());
			this.TabTitle.SetUserEditable(true);
		}
		this.TabTitle.SetSubText(this.target.GetProperName(), "");
	}

	// Token: 0x0600994D RID: 39245 RVA: 0x00103B8D File Offset: 0x00101D8D
	public TargetPanel GetActiveTab()
	{
		return this.tabHeader.ActivePanel;
	}

	// Token: 0x06009951 RID: 39249 RVA: 0x003B4B90 File Offset: 0x003B2D90
	[CompilerGenerated]
	internal static bool <PinResourceButton_TryGetResourceTagAndProperName>g__ShouldUse|51_0(Tag targetTag)
	{
		foreach (Tag tag in GameTags.MaterialCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag2 in GameTags.CalorieCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2).Contains(targetTag))
			{
				return true;
			}
		}
		foreach (Tag tag3 in GameTags.UnitCategories)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag3).Contains(targetTag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04007763 RID: 30563
	public static DetailsScreen Instance;

	// Token: 0x04007764 RID: 30564
	[SerializeField]
	private KButton CodexEntryButton;

	// Token: 0x04007765 RID: 30565
	[SerializeField]
	private KButton PinResourceButton;

	// Token: 0x04007766 RID: 30566
	[Header("Panels")]
	public Transform UserMenuPanel;

	// Token: 0x04007767 RID: 30567
	[Header("Name Editing (disabled)")]
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x04007768 RID: 30568
	[Header("Tabs")]
	[SerializeField]
	private DetailTabHeader tabHeader;

	// Token: 0x04007769 RID: 30569
	[SerializeField]
	private EditableTitleBar TabTitle;

	// Token: 0x0400776A RID: 30570
	[SerializeField]
	private DetailsScreen.Screens[] screens;

	// Token: 0x0400776B RID: 30571
	[SerializeField]
	private GameObject tabHeaderContainer;

	// Token: 0x0400776C RID: 30572
	[Header("Side Screen Tabs")]
	[SerializeField]
	private DetailsScreen.SidescreenTab[] sidescreenTabs;

	// Token: 0x0400776D RID: 30573
	[SerializeField]
	private GameObject sidescreenTabHeader;

	// Token: 0x0400776E RID: 30574
	[SerializeField]
	private GameObject original_tab;

	// Token: 0x0400776F RID: 30575
	[SerializeField]
	private GameObject original_tab_body;

	// Token: 0x04007770 RID: 30576
	[Header("Side Screens")]
	[SerializeField]
	private GameObject sideScreen;

	// Token: 0x04007771 RID: 30577
	[SerializeField]
	private List<DetailsScreen.SideScreenRef> sideScreens;

	// Token: 0x04007772 RID: 30578
	[SerializeField]
	private LayoutElement tabBodyLayoutElement;

	// Token: 0x04007773 RID: 30579
	[Header("Secondary Side Screens")]
	[SerializeField]
	private GameObject sideScreen2ContentBody;

	// Token: 0x04007774 RID: 30580
	[SerializeField]
	private GameObject sideScreen2;

	// Token: 0x04007775 RID: 30581
	[SerializeField]
	private LocText sideScreen2Title;

	// Token: 0x04007776 RID: 30582
	private KScreen activeSideScreen2;

	// Token: 0x04007778 RID: 30584
	private Tag previousTargetID = null;

	// Token: 0x04007779 RID: 30585
	private bool HasActivated;

	// Token: 0x0400777A RID: 30586
	private DetailsScreen.SidescreenTabTypes selectedSidescreenTabID;

	// Token: 0x0400777B RID: 30587
	private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();

	// Token: 0x0400777C RID: 30588
	private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>(delegate(DetailsScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x0400777D RID: 30589
	private List<KeyValuePair<DetailsScreen.SideScreenRef, int>> sortedSideScreens = new List<KeyValuePair<DetailsScreen.SideScreenRef, int>>();

	// Token: 0x0400777E RID: 30590
	private int setRocketTitleHandle = -1;

	// Token: 0x02001CAC RID: 7340
	[Serializable]
	private struct Screens
	{
		// Token: 0x0400777F RID: 30591
		public string name;

		// Token: 0x04007780 RID: 30592
		public string displayName;

		// Token: 0x04007781 RID: 30593
		public string tooltip;

		// Token: 0x04007782 RID: 30594
		public Sprite icon;

		// Token: 0x04007783 RID: 30595
		public TargetPanel screen;

		// Token: 0x04007784 RID: 30596
		public int displayOrderPriority;

		// Token: 0x04007785 RID: 30597
		public bool hideWhenDead;

		// Token: 0x04007786 RID: 30598
		public HashedString focusInViewMode;

		// Token: 0x04007787 RID: 30599
		[HideInInspector]
		public int tabIdx;
	}

	// Token: 0x02001CAD RID: 7341
	public enum SidescreenTabTypes
	{
		// Token: 0x04007789 RID: 30601
		Config,
		// Token: 0x0400778A RID: 30602
		Errands,
		// Token: 0x0400778B RID: 30603
		Material,
		// Token: 0x0400778C RID: 30604
		Blueprints
	}

	// Token: 0x02001CAE RID: 7342
	[Serializable]
	public class SidescreenTab
	{
		// Token: 0x06009953 RID: 39251 RVA: 0x00103C16 File Offset: 0x00101E16
		private void OnTabClicked()
		{
			System.Action onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked();
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06009955 RID: 39253 RVA: 0x00103C31 File Offset: 0x00101E31
		// (set) Token: 0x06009954 RID: 39252 RVA: 0x00103C28 File Offset: 0x00101E28
		public bool IsVisible { get; private set; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06009957 RID: 39255 RVA: 0x00103C42 File Offset: 0x00101E42
		// (set) Token: 0x06009956 RID: 39254 RVA: 0x00103C39 File Offset: 0x00101E39
		public bool IsSelected { get; private set; }

		// Token: 0x06009958 RID: 39256 RVA: 0x003B4C8C File Offset: 0x003B2E8C
		public void Initiate(GameObject originalTabInstance, GameObject originalBodyInstance, Action<DetailsScreen.SidescreenTab> on_tab_clicked_callback)
		{
			if (on_tab_clicked_callback != null)
			{
				this.OnClicked = delegate()
				{
					on_tab_clicked_callback(this);
				};
			}
			originalBodyInstance.gameObject.SetActive(false);
			if (this.OverrideBody == null)
			{
				this.bodyInstance = UnityEngine.Object.Instantiate<GameObject>(originalBodyInstance);
				this.bodyInstance.name = this.type.ToString() + " Tab - body instance";
				this.bodyInstance.SetActive(true);
				this.bodyInstance.transform.SetParent(originalBodyInstance.transform.parent, false);
			}
			else
			{
				this.bodyInstance = this.OverrideBody;
			}
			this.bodyReferences = this.bodyInstance.GetComponent<HierarchyReferences>();
			originalTabInstance.gameObject.SetActive(false);
			if (this.tabInstance == null)
			{
				this.tabInstance = UnityEngine.Object.Instantiate<GameObject>(originalTabInstance.gameObject).GetComponent<MultiToggle>();
				this.tabInstance.name = this.type.ToString() + " Tab Instance";
				this.tabInstance.gameObject.SetActive(true);
				this.tabInstance.transform.SetParent(originalTabInstance.transform.parent, false);
				MultiToggle multiToggle = this.tabInstance;
				multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnTabClicked));
				HierarchyReferences component = this.tabInstance.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("label").SetText(Strings.Get(this.Title_Key));
				component.GetReference<Image>("icon").sprite = this.Icon;
				this.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get(this.Tooltip_Key));
			}
		}

		// Token: 0x06009959 RID: 39257 RVA: 0x00103C4A File Offset: 0x00101E4A
		public void SetSelected(bool isSelected)
		{
			this.IsSelected = isSelected;
			this.tabInstance.ChangeState(isSelected ? 1 : 0);
			this.bodyInstance.SetActive(isSelected);
		}

		// Token: 0x0600995A RID: 39258 RVA: 0x00103C71 File Offset: 0x00101E71
		public void SetTitle(string title)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("TitleLabel"))
			{
				this.bodyReferences.GetReference<LocText>("TitleLabel").SetText(title);
			}
		}

		// Token: 0x0600995B RID: 39259 RVA: 0x00103CA9 File Offset: 0x00101EA9
		public void SetTitleVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("Title"))
			{
				this.bodyReferences.GetReference("Title").gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600995C RID: 39260 RVA: 0x00103CE6 File Offset: 0x00101EE6
		public void SetNoConfigMessageVisibility(bool visible)
		{
			if (this.bodyReferences != null && this.bodyReferences.HasReference("NoConfigMessage"))
			{
				this.bodyReferences.GetReference("NoConfigMessage").gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600995D RID: 39261 RVA: 0x003B4E68 File Offset: 0x003B3068
		public void RepositionTitle()
		{
			if (this.bodyReferences != null && this.bodyReferences.GetReference("Title") != null)
			{
				this.bodyReferences.GetReference("Title").transform.SetSiblingIndex(0);
			}
		}

		// Token: 0x0600995E RID: 39262 RVA: 0x00103D23 File Offset: 0x00101F23
		public void SetVisible(bool visible)
		{
			this.IsVisible = visible;
			this.tabInstance.gameObject.SetActive(visible);
			this.bodyInstance.SetActive(this.IsSelected && this.IsVisible);
		}

		// Token: 0x0600995F RID: 39263 RVA: 0x00103D59 File Offset: 0x00101F59
		public bool ValidateTarget(GameObject target)
		{
			return !(target == null) && (this.ValidateTargetCallback == null || this.ValidateTargetCallback(target, this));
		}

		// Token: 0x0400778D RID: 30605
		public DetailsScreen.SidescreenTabTypes type;

		// Token: 0x0400778E RID: 30606
		public string Title_Key;

		// Token: 0x0400778F RID: 30607
		public string Tooltip_Key;

		// Token: 0x04007790 RID: 30608
		public Sprite Icon;

		// Token: 0x04007791 RID: 30609
		public GameObject OverrideBody;

		// Token: 0x04007792 RID: 30610
		public Func<GameObject, DetailsScreen.SidescreenTab, bool> ValidateTargetCallback;

		// Token: 0x04007793 RID: 30611
		public System.Action OnClicked;

		// Token: 0x04007796 RID: 30614
		[NonSerialized]
		public MultiToggle tabInstance;

		// Token: 0x04007797 RID: 30615
		[NonSerialized]
		public GameObject bodyInstance;

		// Token: 0x04007798 RID: 30616
		private HierarchyReferences bodyReferences;

		// Token: 0x04007799 RID: 30617
		private const string bodyRef_Title = "Title";

		// Token: 0x0400779A RID: 30618
		private const string bodyRef_TitleLabel = "TitleLabel";

		// Token: 0x0400779B RID: 30619
		private const string bodyRef_NoConfigMessage = "NoConfigMessage";
	}

	// Token: 0x02001CB0 RID: 7344
	[Serializable]
	public class SideScreenRef
	{
		// Token: 0x0400779E RID: 30622
		public string name;

		// Token: 0x0400779F RID: 30623
		public SideScreenContent screenPrefab;

		// Token: 0x040077A0 RID: 30624
		public Vector2 offset;

		// Token: 0x040077A1 RID: 30625
		public DetailsScreen.SidescreenTabTypes tab;

		// Token: 0x040077A2 RID: 30626
		[HideInInspector]
		public SideScreenContent screenInstance;
	}
}
