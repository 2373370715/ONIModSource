using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001AE8 RID: 6888
public class ManagementMenu : KIconToggleMenu
{
	// Token: 0x06009065 RID: 36965 RVA: 0x000FE618 File Offset: 0x000FC818
	public static void DestroyInstance()
	{
		ManagementMenu.Instance = null;
	}

	// Token: 0x06009066 RID: 36966 RVA: 0x000FE620 File Offset: 0x000FC820
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06009067 RID: 36967 RVA: 0x0037B8EC File Offset: 0x00379AEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ManagementMenu.Instance = this;
		this.notificationDisplayer.onNotificationsChanged += this.OnNotificationsChanged;
		CodexCache.CodexCacheInit();
		ScheduledUIInstantiation component = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
		this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
		this.clusterMapScreen = component.GetInstantiatedObject<ClusterMapScreen>();
		this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
		this.researchScreen = component.GetInstantiatedObject<ResearchScreen>();
		this.fullscreenUIs = new ManagementMenu.ManagementMenuToggleInfo[]
		{
			this.researchInfo,
			this.skillsInfo,
			this.starmapInfo,
			this.clusterMapInfo
		};
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		this.consumablesInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CONSUMABLES, "OverviewUI_consumables_icon", null, global::Action.ManageConsumables, UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, "");
		this.AddToggleTooltip(this.consumablesInfo, null);
		this.vitalsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.VITALS, "OverviewUI_vitals_icon", null, global::Action.ManageVitals, UI.TOOLTIPS.MANAGEMENTMENU_VITALS, "");
		this.AddToggleTooltip(this.vitalsInfo, null);
		this.researchInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.RESEARCH, "OverviewUI_research_nav_icon", null, global::Action.ManageResearch, UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, "");
		this.AddToggleTooltipForResearch(this.researchInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH);
		this.researchInfo.prefabOverride = this.researchButtonPrefab;
		this.jobsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.JOBS, "OverviewUI_priority_icon", null, global::Action.ManagePriorities, UI.TOOLTIPS.MANAGEMENTMENU_JOBS, "");
		this.AddToggleTooltip(this.jobsInfo, null);
		this.skillsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SKILLS, "OverviewUI_jobs_icon", null, global::Action.ManageSkills, UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, "");
		this.AddToggleTooltip(this.skillsInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION);
		this.starmapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.starmapInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE);
		this.clusterMapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.clusterMapInfo, null);
		this.scheduleInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SCHEDULE, "OverviewUI_schedule2_icon", null, global::Action.ManageSchedule, UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, "");
		this.AddToggleTooltip(this.scheduleInfo, null);
		this.reportsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.REPORT, "OverviewUI_reports_icon", null, global::Action.ManageReport, UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, "");
		this.AddToggleTooltip(this.reportsInfo, null);
		this.reportsInfo.prefabOverride = this.smallPrefab;
		this.codexInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", null, global::Action.ManageDatabase, UI.TOOLTIPS.MANAGEMENTMENU_CODEX, "");
		this.AddToggleTooltip(this.codexInfo, null);
		this.codexInfo.prefabOverride = this.smallPrefab;
		this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData
		{
			screen = this.consumablesScreen,
			tabIdx = 3,
			toggleInfo = this.consumablesInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData
		{
			screen = this.vitalsScreen,
			tabIdx = 2,
			toggleInfo = this.vitalsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData
		{
			screen = this.reportsScreen,
			tabIdx = 4,
			toggleInfo = this.reportsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData
		{
			screen = this.jobsScreen,
			tabIdx = 1,
			toggleInfo = this.jobsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData
		{
			screen = this.skillsScreen,
			tabIdx = 0,
			toggleInfo = this.skillsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData
		{
			screen = this.codexScreen,
			tabIdx = 6,
			toggleInfo = this.codexInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData
		{
			screen = this.scheduleScreen,
			tabIdx = 7,
			toggleInfo = this.scheduleInfo,
			cancelHandler = null
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ScreenInfoMatch.Add(this.clusterMapInfo, new ManagementMenu.ScreenData
			{
				screen = this.clusterMapScreen,
				tabIdx = 7,
				toggleInfo = this.clusterMapInfo,
				cancelHandler = new Func<bool>(this.clusterMapScreen.TryHandleCancel)
			});
		}
		else
		{
			this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData
			{
				screen = this.starmapScreen,
				tabIdx = 7,
				toggleInfo = this.starmapInfo,
				cancelHandler = null
			});
		}
		this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData
		{
			screen = this.researchScreen,
			tabIdx = 5,
			toggleInfo = this.researchInfo,
			cancelHandler = null
		});
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		list.Add(this.vitalsInfo);
		list.Add(this.consumablesInfo);
		list.Add(this.scheduleInfo);
		list.Add(this.jobsInfo);
		list.Add(this.skillsInfo);
		list.Add(this.researchInfo);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(this.clusterMapInfo);
		}
		else
		{
			list.Add(this.starmapInfo);
		}
		list.Add(this.reportsInfo);
		list.Add(this.codexInfo);
		base.Setup(list);
		base.onSelect += this.OnButtonClick;
		this.PauseMenuButton.onClick += this.OnPauseMenuClicked;
		this.PauseMenuButton.transform.SetAsLastSibling();
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChanged));
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearch);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearch);
		Components.RoleStations.OnAdd += new Action<RoleStation>(this.CheckSkills);
		Components.RoleStations.OnRemove += new Action<RoleStation>(this.CheckSkills);
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckResearch));
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckSkills));
		Game.Instance.Subscribe(445618876, new Action<object>(this.OnResolutionChanged));
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			Components.Telescopes.OnAdd += new Action<Telescope>(this.CheckStarmap);
			Components.Telescopes.OnRemove += new Action<Telescope>(this.CheckStarmap);
		}
		this.CheckResearch(null);
		this.CheckSkills(null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.CheckStarmap(null);
		}
		this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (() => this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
			ktoggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
		}
		this.OnResolutionChanged(null);
	}

	// Token: 0x06009068 RID: 36968 RVA: 0x000FE627 File Offset: 0x000FC827
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mutuallyExclusiveScreens.Add(AllResourcesScreen.Instance);
		this.mutuallyExclusiveScreens.Add(AllDiagnosticsScreen.Instance);
		this.OnNotificationsChanged();
	}

	// Token: 0x06009069 RID: 36969 RVA: 0x000FE655 File Offset: 0x000FC855
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChanged));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600906A RID: 36970 RVA: 0x0037C134 File Offset: 0x0037A334
	private void OnInputChanged()
	{
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		this.consumablesInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, this.consumablesInfo.hotKey);
		this.vitalsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_VITALS, this.vitalsInfo.hotKey);
		this.researchInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, this.researchInfo.hotKey);
		this.jobsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_JOBS, this.jobsInfo.hotKey);
		this.skillsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, this.skillsInfo.hotKey);
		this.starmapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.starmapInfo.hotKey);
		this.clusterMapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.clusterMapInfo.hotKey);
		this.scheduleInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, this.scheduleInfo.hotKey);
		this.reportsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, this.reportsInfo.hotKey);
		this.codexInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CODEX, this.codexInfo.hotKey);
	}

	// Token: 0x0600906B RID: 36971 RVA: 0x0037C2D4 File Offset: 0x0037A4D4
	private void OnResolutionChanged(object data = null)
	{
		bool flag = (float)Screen.width < 1300f;
		foreach (KToggle ktoggle in this.toggles)
		{
			HierarchyReferences component = ktoggle.GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				RectTransform reference = component.GetReference<RectTransform>("TextContainer");
				if (!(reference == null))
				{
					reference.gameObject.SetActive(!flag);
				}
			}
		}
	}

	// Token: 0x0600906C RID: 36972 RVA: 0x0037C360 File Offset: 0x0037A560
	private void OnNotificationsChanged()
	{
		foreach (KeyValuePair<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> keyValuePair in this.ScreenInfoMatch)
		{
			keyValuePair.Key.SetNotificationDisplay(false, false, null, this.noAlertColorStyle);
		}
	}

	// Token: 0x0600906D RID: 36973 RVA: 0x000FE673 File Offset: 0x000FC873
	private ToolTip.ComplexTooltipDelegate CreateToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		return delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x0600906E RID: 36974 RVA: 0x000FE693 File Offset: 0x000FC893
	private void AddToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = this.CreateToggleTooltip(toggleInfo, disabledTooltip);
	}

	// Token: 0x0600906F RID: 36975 RVA: 0x0037C3C4 File Offset: 0x0037A5C4
	private void AddToggleTooltipForResearch(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			string a = (activeResearch == null) ? UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_NO_RESEARCH : string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_CARD_NAME, activeResearch.tech.Name);
			list.Add(new global::Tuple<string, TextStyleSetting>(a, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			if (activeResearch != null)
			{
				string text = "";
				for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
				{
					TechItem techItem = activeResearch.tech.unlockedItems[i];
					text = text + "\n" + string.Format(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_ITEM_LINE, techItem.Name);
				}
				list.Add(new global::Tuple<string, TextStyleSetting>(text, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>("\n" + toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x06009070 RID: 36976 RVA: 0x0037C3FC File Offset: 0x0037A5FC
	public bool IsFullscreenUIActive()
	{
		if (this.activeScreen == null)
		{
			return false;
		}
		foreach (ManagementMenu.ManagementMenuToggleInfo managementMenuToggleInfo in this.fullscreenUIs)
		{
			if (this.activeScreen.toggleInfo == managementMenuToggleInfo)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009071 RID: 36977 RVA: 0x000FE6A3 File Offset: 0x000FC8A3
	private void OnPauseMenuClicked()
	{
		PauseScreen.Instance.Show(true);
		this.PauseMenuButton.isOn = false;
	}

	// Token: 0x06009072 RID: 36978 RVA: 0x000FE6BC File Offset: 0x000FC8BC
	public void Refresh()
	{
		this.CheckResearch(null);
		this.CheckSkills(null);
		this.CheckStarmap(null);
	}

	// Token: 0x06009073 RID: 36979 RVA: 0x0037C440 File Offset: 0x0037A640
	public void CheckResearch(object o)
	{
		if (this.researchInfo.toggle == null)
		{
			return;
		}
		bool flag = Components.ResearchCenters.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = !flag && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
		this.ConfigureToggle(this.researchInfo.toggle, flag, active);
	}

	// Token: 0x06009074 RID: 36980 RVA: 0x0037C4BC File Offset: 0x0037A6BC
	public void CheckSkills(object o = null)
	{
		if (this.skillsInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo;
		this.ConfigureToggle(this.skillsInfo.toggle, disabled, active);
	}

	// Token: 0x06009075 RID: 36981 RVA: 0x0037C534 File Offset: 0x0037A734
	public void CheckStarmap(object o = null)
	{
		if (this.starmapInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo;
		this.ConfigureToggle(this.starmapInfo.toggle, disabled, active);
	}

	// Token: 0x06009076 RID: 36982 RVA: 0x000FE6D3 File Offset: 0x000FC8D3
	private void ConfigureToggle(KToggle toggle, bool disabled, bool active)
	{
		toggle.interactable = !disabled;
		if (disabled)
		{
			toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
			return;
		}
		toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
	}

	// Token: 0x06009077 RID: 36983 RVA: 0x000FE6FA File Offset: 0x000FC8FA
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.activeScreen != null && e.TryConsume(global::Action.Escape))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06009078 RID: 36984 RVA: 0x000FE728 File Offset: 0x000FC928
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06009079 RID: 36985 RVA: 0x000FE75B File Offset: 0x000FC95B
	private void ToggleIfCancelUnhandled(ManagementMenu.ScreenData screenData)
	{
		if (screenData.cancelHandler == null || !screenData.cancelHandler())
		{
			this.ToggleScreen(screenData);
		}
	}

	// Token: 0x0600907A RID: 36986 RVA: 0x000FE779 File Offset: 0x000FC979
	private bool ResearchAvailable()
	{
		return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x0600907B RID: 36987 RVA: 0x000FE79B File Offset: 0x000FC99B
	private bool SkillsAvailable()
	{
		return Components.RoleStations.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x0600907C RID: 36988 RVA: 0x000FE7BD File Offset: 0x000FC9BD
	public static bool StarmapAvailable()
	{
		return Components.Telescopes.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
	}

	// Token: 0x0600907D RID: 36989 RVA: 0x000FE7DF File Offset: 0x000FC9DF
	public void CloseAll()
	{
		if (this.activeScreen == null)
		{
			return;
		}
		if (this.activeScreen.toggleInfo != null)
		{
			this.ToggleScreen(this.activeScreen);
		}
		this.CloseActive();
		this.ClearSelection();
	}

	// Token: 0x0600907E RID: 36990 RVA: 0x000FE80F File Offset: 0x000FCA0F
	private void OnUIClear(object data)
	{
		this.CloseAll();
	}

	// Token: 0x0600907F RID: 36991 RVA: 0x0037C5AC File Offset: 0x0037A7AC
	public void ToggleScreen(ManagementMenu.ScreenData screenData)
	{
		if (screenData == null)
		{
			return;
		}
		if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
		{
			this.CheckResearch(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
		{
			this.CheckSkills(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.starmapInfo && !ManagementMenu.StarmapAvailable())
		{
			this.CheckStarmap(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
		{
			return;
		}
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
		}
		if (this.activeScreen != screenData)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			if (this.activeScreen != null)
			{
				this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
			screenData.toggleInfo.toggle.ActivateFlourish(true);
			screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
			this.CloseActive();
			this.activeScreen = screenData;
			if (!this.activeScreen.screen.IsActive())
			{
				this.activeScreen.screen.Activate();
			}
			this.activeScreen.screen.Show(true);
			foreach (ManagementMenuNotification managementMenuNotification in this.notificationDisplayer.GetNotificationsForAction(screenData.toggleInfo.hotKey))
			{
				if (managementMenuNotification.customClickCallback != null)
				{
					managementMenuNotification.customClickCallback(managementMenuNotification.customClickData);
					break;
				}
			}
			using (List<KScreen>.Enumerator enumerator2 = this.mutuallyExclusiveScreens.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KScreen kscreen = enumerator2.Current;
					kscreen.Show(false);
				}
				return;
			}
		}
		this.activeScreen.screen.Show(false);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenMigrated, STOP_MODE.ALLOWFADEOUT);
		this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
		this.activeScreen = null;
		screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
	}

	// Token: 0x06009080 RID: 36992 RVA: 0x000FE817 File Offset: 0x000FCA17
	public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.ToggleScreen(this.ScreenInfoMatch[(ManagementMenu.ManagementMenuToggleInfo)toggle_info]);
	}

	// Token: 0x06009081 RID: 36993 RVA: 0x000FE830 File Offset: 0x000FCA30
	private void CloseActive()
	{
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.screen.Show(false);
			this.activeScreen = null;
		}
	}

	// Token: 0x06009082 RID: 36994 RVA: 0x0037C864 File Offset: 0x0037AA64
	public void ToggleResearch()
	{
		if ((this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]) && this.researchInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
	}

	// Token: 0x06009083 RID: 36995 RVA: 0x000FE868 File Offset: 0x000FCA68
	public void ToggleCodex()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
	}

	// Token: 0x06009084 RID: 36996 RVA: 0x0037C8BC File Offset: 0x0037AABC
	public void OpenCodexToLockId(string lockId, bool focusContent = false)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockId);
		if (entryForLock == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Could not open codex to lockId \"" + lockId + "\", couldn't find an entry that contained that lockId"
			});
			return;
		}
		ContentContainer contentContainer = null;
		if (focusContent)
		{
			CodexEntry codexEntry = CodexCache.FindEntry(entryForLock);
			int num = 0;
			while (contentContainer == null && num < codexEntry.contentContainers.Count)
			{
				if (!(codexEntry.contentContainers[num].lockID != lockId))
				{
					contentContainer = codexEntry.contentContainers[num];
				}
				num++;
			}
		}
		this.OpenCodexToEntry(entryForLock, contentContainer);
	}

	// Token: 0x06009085 RID: 36997 RVA: 0x0037C948 File Offset: 0x0037AB48
	public void OpenCodexToEntry(string id, ContentContainer targetContainer = null)
	{
		if (!this.codexScreen.gameObject.activeInHierarchy)
		{
			this.ToggleCodex();
		}
		this.codexScreen.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		this.codexScreen.FocusContainer(targetContainer);
	}

	// Token: 0x06009086 RID: 36998 RVA: 0x0037C990 File Offset: 0x0037AB90
	public void ToggleSkills()
	{
		if ((this.SkillsAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]) && this.skillsInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06009087 RID: 36999 RVA: 0x000FE885 File Offset: 0x000FCA85
	public void ToggleStarmap()
	{
		if (this.starmapInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x06009088 RID: 37000 RVA: 0x000FE8AA File Offset: 0x000FCAAA
	public void ToggleClusterMap()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
	}

	// Token: 0x06009089 RID: 37001 RVA: 0x000FE8C7 File Offset: 0x000FCAC7
	public void TogglePriorities()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
	}

	// Token: 0x0600908A RID: 37002 RVA: 0x0037C9E8 File Offset: 0x0037ABE8
	public void OpenReports(int day)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
		}
		ReportScreen.Instance.ShowReport(day);
	}

	// Token: 0x0600908B RID: 37003 RVA: 0x0037CA38 File Offset: 0x0037AC38
	public void OpenResearch(string zoomToTech = null)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
		if (zoomToTech != null)
		{
			this.researchScreen.ZoomToTech(zoomToTech);
		}
	}

	// Token: 0x0600908C RID: 37004 RVA: 0x000FE8E4 File Offset: 0x000FCAE4
	public void OpenStarmap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x0600908D RID: 37005 RVA: 0x000FE91E File Offset: 0x000FCB1E
	public void OpenClusterMap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x0600908E RID: 37006 RVA: 0x000FE958 File Offset: 0x000FCB58
	public void CloseClusterMap()
	{
		if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x0600908F RID: 37007 RVA: 0x0037CA8C File Offset: 0x0037AC8C
	public void OpenSkills(MinionIdentity minionIdentity)
	{
		this.skillsScreen.CurrentlySelectedMinion = minionIdentity;
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06009090 RID: 37008 RVA: 0x000FE992 File Offset: 0x000FCB92
	public bool IsScreenOpen(KScreen screen)
	{
		return this.activeScreen != null && this.activeScreen.screen == screen;
	}

	// Token: 0x04006D16 RID: 27926
	private const float UI_WIDTH_COMPRESS_THRESHOLD = 1300f;

	// Token: 0x04006D17 RID: 27927
	[MyCmpReq]
	public ManagementMenuNotificationDisplayer notificationDisplayer;

	// Token: 0x04006D18 RID: 27928
	public static ManagementMenu Instance;

	// Token: 0x04006D19 RID: 27929
	[Header("Management Menu Specific")]
	[SerializeField]
	private KToggle smallPrefab;

	// Token: 0x04006D1A RID: 27930
	[SerializeField]
	private KToggle researchButtonPrefab;

	// Token: 0x04006D1B RID: 27931
	public KToggle PauseMenuButton;

	// Token: 0x04006D1C RID: 27932
	[Header("Top Right Screen References")]
	public JobsTableScreen jobsScreen;

	// Token: 0x04006D1D RID: 27933
	public VitalsTableScreen vitalsScreen;

	// Token: 0x04006D1E RID: 27934
	public ScheduleScreen scheduleScreen;

	// Token: 0x04006D1F RID: 27935
	public ReportScreen reportsScreen;

	// Token: 0x04006D20 RID: 27936
	public CodexScreen codexScreen;

	// Token: 0x04006D21 RID: 27937
	public ConsumablesTableScreen consumablesScreen;

	// Token: 0x04006D22 RID: 27938
	private StarmapScreen starmapScreen;

	// Token: 0x04006D23 RID: 27939
	private ClusterMapScreen clusterMapScreen;

	// Token: 0x04006D24 RID: 27940
	private SkillsScreen skillsScreen;

	// Token: 0x04006D25 RID: 27941
	private ResearchScreen researchScreen;

	// Token: 0x04006D26 RID: 27942
	[Header("Notification Styles")]
	public ColorStyleSetting noAlertColorStyle;

	// Token: 0x04006D27 RID: 27943
	public List<ColorStyleSetting> alertColorStyle;

	// Token: 0x04006D28 RID: 27944
	public List<TextStyleSetting> alertTextStyle;

	// Token: 0x04006D29 RID: 27945
	private ManagementMenu.ManagementMenuToggleInfo jobsInfo;

	// Token: 0x04006D2A RID: 27946
	private ManagementMenu.ManagementMenuToggleInfo consumablesInfo;

	// Token: 0x04006D2B RID: 27947
	private ManagementMenu.ManagementMenuToggleInfo scheduleInfo;

	// Token: 0x04006D2C RID: 27948
	private ManagementMenu.ManagementMenuToggleInfo vitalsInfo;

	// Token: 0x04006D2D RID: 27949
	private ManagementMenu.ManagementMenuToggleInfo reportsInfo;

	// Token: 0x04006D2E RID: 27950
	private ManagementMenu.ManagementMenuToggleInfo researchInfo;

	// Token: 0x04006D2F RID: 27951
	private ManagementMenu.ManagementMenuToggleInfo codexInfo;

	// Token: 0x04006D30 RID: 27952
	private ManagementMenu.ManagementMenuToggleInfo starmapInfo;

	// Token: 0x04006D31 RID: 27953
	private ManagementMenu.ManagementMenuToggleInfo clusterMapInfo;

	// Token: 0x04006D32 RID: 27954
	private ManagementMenu.ManagementMenuToggleInfo skillsInfo;

	// Token: 0x04006D33 RID: 27955
	private ManagementMenu.ManagementMenuToggleInfo[] fullscreenUIs;

	// Token: 0x04006D34 RID: 27956
	private Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData>();

	// Token: 0x04006D35 RID: 27957
	private ManagementMenu.ScreenData activeScreen;

	// Token: 0x04006D36 RID: 27958
	private KButton activeButton;

	// Token: 0x04006D37 RID: 27959
	private string skillsTooltip;

	// Token: 0x04006D38 RID: 27960
	private string skillsTooltipDisabled;

	// Token: 0x04006D39 RID: 27961
	private string researchTooltip;

	// Token: 0x04006D3A RID: 27962
	private string researchTooltipDisabled;

	// Token: 0x04006D3B RID: 27963
	private string starmapTooltip;

	// Token: 0x04006D3C RID: 27964
	private string starmapTooltipDisabled;

	// Token: 0x04006D3D RID: 27965
	private string clusterMapTooltip;

	// Token: 0x04006D3E RID: 27966
	private string clusterMapTooltipDisabled;

	// Token: 0x04006D3F RID: 27967
	private List<KScreen> mutuallyExclusiveScreens = new List<KScreen>();

	// Token: 0x02001AE9 RID: 6889
	public class ScreenData
	{
		// Token: 0x04006D40 RID: 27968
		public KScreen screen;

		// Token: 0x04006D41 RID: 27969
		public ManagementMenu.ManagementMenuToggleInfo toggleInfo;

		// Token: 0x04006D42 RID: 27970
		public Func<bool> cancelHandler;

		// Token: 0x04006D43 RID: 27971
		public int tabIdx;
	}

	// Token: 0x02001AEA RID: 6890
	public class ManagementMenuToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x06009094 RID: 37012 RVA: 0x000FE9F6 File Offset: 0x000FCBF6
		public ManagementMenuToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon, user_data, hotkey, tooltip, tooltip_header)
		{
			this.tooltip = GameUtil.ReplaceHotkeyString(this.tooltip, this.hotKey);
		}

		// Token: 0x06009095 RID: 37013 RVA: 0x0037CAE0 File Offset: 0x0037ACE0
		public void SetNotificationDisplay(bool showAlertImage, bool showGlow, ColorStyleSetting buttonColorStyle, ColorStyleSetting alertColorStyle)
		{
			ImageToggleState component = this.toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				if (buttonColorStyle != null)
				{
					component.SetColorStyle(buttonColorStyle);
				}
				else
				{
					component.SetColorStyle(this.originalButtonSetting);
				}
			}
			if (this.alertImage != null)
			{
				this.alertImage.gameObject.SetActive(showAlertImage);
				this.alertImage.SetColorStyle(alertColorStyle);
			}
			if (this.glowImage != null)
			{
				this.glowImage.gameObject.SetActive(showGlow);
				if (buttonColorStyle != null)
				{
					this.glowImage.SetColorStyle(buttonColorStyle);
				}
			}
		}

		// Token: 0x06009096 RID: 37014 RVA: 0x0037CB80 File Offset: 0x0037AD80
		public override void SetToggle(KToggle toggle)
		{
			base.SetToggle(toggle);
			ImageToggleState component = toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				this.originalButtonSetting = component.colorStyleSetting;
			}
			HierarchyReferences component2 = toggle.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				this.alertImage = component2.GetReference<ImageToggleState>("AlertImage");
				this.glowImage = component2.GetReference<ImageToggleState>("GlowImage");
			}
		}

		// Token: 0x04006D44 RID: 27972
		public ImageToggleState alertImage;

		// Token: 0x04006D45 RID: 27973
		public ImageToggleState glowImage;

		// Token: 0x04006D46 RID: 27974
		private ColorStyleSetting originalButtonSetting;
	}
}
