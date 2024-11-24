using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001BCD RID: 7117
public class AllDiagnosticsScreen : ShowOptimizedKScreen, ISim4000ms, ISim1000ms
{
	// Token: 0x060093F9 RID: 37881 RVA: 0x0010065B File Offset: 0x000FE85B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AllDiagnosticsScreen.Instance = this;
		this.ConfigureDebugToggle();
	}

	// Token: 0x060093FA RID: 37882 RVA: 0x0010066F File Offset: 0x000FE86F
	protected override void OnForcedCleanUp()
	{
		AllDiagnosticsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060093FB RID: 37883 RVA: 0x00392BBC File Offset: 0x00390DBC
	private void ConfigureDebugToggle()
	{
		Game.Instance.Subscribe(1557339983, new Action<object>(this.DebugToggleRefresh));
		MultiToggle toggle = this.debugNotificationToggleCotainer.GetComponentInChildren<MultiToggle>();
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			DebugHandler.ToggleDisableNotifications();
			toggle.ChangeState(DebugHandler.NotificationsDisabled ? 1 : 0);
		}));
		this.DebugToggleRefresh(null);
		toggle.ChangeState(DebugHandler.NotificationsDisabled ? 1 : 0);
	}

	// Token: 0x060093FC RID: 37884 RVA: 0x0010067D File Offset: 0x000FE87D
	private void DebugToggleRefresh(object data = null)
	{
		this.debugNotificationToggleCotainer.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	// Token: 0x060093FD RID: 37885 RVA: 0x00392C40 File Offset: 0x00390E40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		Game.Instance.Subscribe(-1280433810, new Action<object>(this.Populate));
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		this.searchInputField.onValueChanged.AddListener(delegate(string value)
		{
			this.SearchFilter(value);
		});
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.Show(false);
	}

	// Token: 0x060093FE RID: 37886 RVA: 0x00100694 File Offset: 0x000FE894
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			ManagementMenu.Instance.CloseAll();
			AllResourcesScreen.Instance.Show(false);
			this.RefreshSubrows();
		}
	}

	// Token: 0x060093FF RID: 37887 RVA: 0x00392D30 File Offset: 0x00390F30
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009400 RID: 37888 RVA: 0x001006BB File Offset: 0x000FE8BB
	public int GetRowCount()
	{
		return this.diagnosticRows.Count;
	}

	// Token: 0x06009401 RID: 37889 RVA: 0x00392D84 File Offset: 0x00390F84
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06009402 RID: 37890 RVA: 0x000FD501 File Offset: 0x000FB701
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06009403 RID: 37891 RVA: 0x00392DD8 File Offset: 0x00390FD8
	public void Populate(object data = null)
	{
		this.SpawnRows();
		foreach (string s in this.diagnosticRows.Keys)
		{
			Tag key = s;
			this.currentlyDisplayedRows[key] = true;
		}
		this.SearchFilter(this.searchInputField.text);
		this.RefreshRows();
	}

	// Token: 0x06009404 RID: 37892 RVA: 0x00392E58 File Offset: 0x00391058
	private void SpawnRows()
	{
		foreach (KeyValuePair<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> keyValuePair in ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings)
		{
			foreach (KeyValuePair<string, ColonyDiagnosticUtility.DisplaySetting> keyValuePair2 in ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[keyValuePair.Key])
			{
				if (!this.diagnosticRows.ContainsKey(keyValuePair2.Key))
				{
					ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair2.Key, keyValuePair.Key);
					if (!(diagnostic is WorkTimeDiagnostic) && !(diagnostic is ChoreGroupDiagnostic))
					{
						this.SpawnRow(diagnostic, this.rootListContainer);
					}
				}
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.diagnosticRows)
		{
			list.Add(keyValuePair3.Key);
		}
		list.Sort((string a, string b) => UI.StripLinkFormatting(ColonyDiagnosticUtility.Instance.GetDiagnosticName(a)).CompareTo(UI.StripLinkFormatting(ColonyDiagnosticUtility.Instance.GetDiagnosticName(b))));
		foreach (string key in list)
		{
			this.diagnosticRows[key].transform.SetAsLastSibling();
		}
	}

	// Token: 0x06009405 RID: 37893 RVA: 0x0039300C File Offset: 0x0039120C
	private void SpawnRow(ColonyDiagnostic diagnostic, GameObject container)
	{
		if (diagnostic == null)
		{
			return;
		}
		if (!this.diagnosticRows.ContainsKey(diagnostic.id))
		{
			GameObject gameObject = Util.KInstantiateUI(this.diagnosticLinePrefab, container, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("NameLabel").SetText(diagnostic.name);
			string id2 = diagnostic.id;
			MultiToggle reference = component.GetReference<MultiToggle>("PinToggle");
			string id = diagnostic.id;
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnostic.id))
				{
					ColonyDiagnosticUtility.Instance.ClearDiagnosticTutorialSetting(diagnostic.id);
				}
				else
				{
					int activeWorldId = ClusterManager.Instance.activeWorldId;
					int num = ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[activeWorldId][id] - ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
					if (num < 0)
					{
						num = 2;
					}
					ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[activeWorldId][id] = (ColonyDiagnosticUtility.DisplaySetting)num;
				}
				this.RefreshRows();
				ColonyDiagnosticScreen.Instance.RefreshAll();
			}));
			GraphBase component2 = component.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>();
			component2.axis_x.min_value = 0f;
			component2.axis_x.max_value = 600f;
			component2.axis_x.guide_frequency = 120f;
			component2.RefreshGuides();
			this.diagnosticRows.Add(id2, gameObject);
			this.criteriaRows.Add(id2, new Dictionary<string, GameObject>());
			this.currentlyDisplayedRows.Add(id2, true);
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(diagnostic.icon);
			this.RefreshPinnedState(id2);
			RectTransform reference2 = component.GetReference<RectTransform>("SubRows");
			DiagnosticCriterion[] criteria = diagnostic.GetCriteria();
			for (int i = 0; i < criteria.Length; i++)
			{
				DiagnosticCriterion sub = criteria[i];
				GameObject gameObject2 = Util.KInstantiateUI(this.subDiagnosticLinePrefab, reference2.gameObject, true);
				gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.DIAGNOSTICS_SCREEN.CRITERIA_TOOLTIP, diagnostic.name, sub.name));
				HierarchyReferences component3 = gameObject2.GetComponent<HierarchyReferences>();
				component3.GetReference<LocText>("Label").SetText(sub.name);
				this.criteriaRows[diagnostic.id].Add(sub.id, gameObject2);
				MultiToggle reference3 = component3.GetReference<MultiToggle>("PinToggle");
				reference3.onClick = (System.Action)Delegate.Combine(reference3.onClick, new System.Action(delegate()
				{
					int activeWorldId = ClusterManager.Instance.activeWorldId;
					bool flag = ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(activeWorldId, diagnostic.id, sub.id);
					ColonyDiagnosticUtility.Instance.SetCriteriaEnabled(activeWorldId, diagnostic.id, sub.id, !flag);
					this.RefreshSubrows();
				}));
			}
			this.subrowContainerOpen.Add(diagnostic.id, false);
			MultiToggle reference4 = component.GetReference<MultiToggle>("SubrowToggle");
			MultiToggle multiToggle = reference4;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
			{
				this.subrowContainerOpen[diagnostic.id] = !this.subrowContainerOpen[diagnostic.id];
				this.RefreshSubrows();
			}));
			component.GetReference<MultiToggle>("MainToggle").onClick = reference4.onClick;
		}
	}

	// Token: 0x06009406 RID: 37894 RVA: 0x001006C8 File Offset: 0x000FE8C8
	private void FilterRowBySearch(Tag tag, string filter)
	{
		this.currentlyDisplayedRows[tag] = this.PassesSearchFilter(tag, filter);
	}

	// Token: 0x06009407 RID: 37895 RVA: 0x00393328 File Offset: 0x00391528
	private void SearchFilter(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			this.FilterRowBySearch(keyValuePair.Key, search);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.diagnosticRows)
		{
			this.currentlyDisplayedRows[keyValuePair2.Key] = this.PassesSearchFilter(keyValuePair2.Key, search);
		}
		this.SetRowsActive();
	}

	// Token: 0x06009408 RID: 37896 RVA: 0x003933F4 File Offset: 0x003915F4
	private bool PassesSearchFilter(Tag tag, string filter)
	{
		if (string.IsNullOrEmpty(filter))
		{
			return true;
		}
		filter = filter.ToUpper();
		string id = tag.ToString();
		if (ColonyDiagnosticUtility.Instance.GetDiagnosticName(id).ToUpper().Contains(filter) || tag.Name.ToUpper().Contains(filter))
		{
			return true;
		}
		ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(id, ClusterManager.Instance.activeWorldId);
		if (diagnostic == null)
		{
			return false;
		}
		DiagnosticCriterion[] criteria = diagnostic.GetCriteria();
		if (criteria == null)
		{
			return false;
		}
		DiagnosticCriterion[] array = criteria;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.ToUpper().Contains(filter))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009409 RID: 37897 RVA: 0x003934A4 File Offset: 0x003916A4
	private void RefreshPinnedState(string diagnosticID)
	{
		if (!ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId].ContainsKey(diagnosticID))
		{
			return;
		}
		MultiToggle reference = this.diagnosticRows[diagnosticID].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("PinToggle");
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnosticID))
		{
			reference.ChangeState(3);
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId][diagnosticID])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				reference.ChangeState(2);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				reference.ChangeState(1);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				reference.ChangeState(0);
				break;
			}
		}
		string simpleTooltip = "";
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnosticID))
		{
			simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.TUTORIAL_DISABLED;
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId][diagnosticID])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.NEVER;
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.ALWAYS;
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.ALERT_ONLY;
				break;
			}
		}
		reference.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
	}

	// Token: 0x0600940A RID: 37898 RVA: 0x003935D8 File Offset: 0x003917D8
	public void RefreshRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		if (this.allowRefresh)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
			{
				HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("AvailableLabel").SetText(keyValuePair.Key);
				component.GetReference<RectTransform>("SubRows").gameObject.SetActive(false);
				ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, ClusterManager.Instance.activeWorldId);
				if (diagnostic != null)
				{
					component.GetReference<LocText>("AvailableLabel").SetText(diagnostic.GetAverageValueString());
					component.GetReference<Image>("Indicator").color = diagnostic.colors[diagnostic.LatestResult.opinion];
					ToolTip reference = component.GetReference<ToolTip>("Tooltip");
					reference.refreshWhileHovering = true;
					reference.SetSimpleTooltip(Strings.Get(new StringKey("STRINGS.UI.COLONY_DIAGNOSTICS." + diagnostic.id.ToUpper() + ".TOOLTIP_NAME")) + "\n" + diagnostic.LatestResult.GetFormattedMessage());
				}
				this.RefreshPinnedState(keyValuePair.Key);
			}
		}
		this.SetRowsActive();
		this.RefreshSubrows();
	}

	// Token: 0x0600940B RID: 37899 RVA: 0x00393768 File Offset: 0x00391968
	private void RefreshSubrows()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
			component.GetReference<MultiToggle>("SubrowToggle").ChangeState((!this.subrowContainerOpen[keyValuePair.Key]) ? 0 : 1);
			component.GetReference<RectTransform>("SubRows").gameObject.SetActive(this.subrowContainerOpen[keyValuePair.Key]);
			int num = 0;
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.criteriaRows[keyValuePair.Key])
			{
				MultiToggle reference = keyValuePair2.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("PinToggle");
				int activeWorldId = ClusterManager.Instance.activeWorldId;
				string key = keyValuePair.Key;
				string key2 = keyValuePair2.Key;
				bool flag = ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(activeWorldId, key, key2);
				reference.ChangeState(flag ? 1 : 0);
				if (flag)
				{
					num++;
				}
			}
			component.GetReference<LocText>("SubrowHeaderLabel").SetText(string.Format(UI.DIAGNOSTICS_SCREEN.CRITERIA_ENABLED_COUNT, num, this.criteriaRows[keyValuePair.Key].Count));
		}
	}

	// Token: 0x0600940C RID: 37900 RVA: 0x00393920 File Offset: 0x00391B20
	private void RefreshCharts()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
			ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, ClusterManager.Instance.activeWorldId);
			if (diagnostic != null)
			{
				SparkLayer reference = component.GetReference<SparkLayer>("Chart");
				Tracker tracker = diagnostic.tracker;
				if (tracker != null)
				{
					float num = 3000f;
					global::Tuple<float, float>[] array = tracker.ChartableData(num);
					reference.graph.axis_x.max_value = array[array.Length - 1].first;
					reference.graph.axis_x.min_value = reference.graph.axis_x.max_value - num;
					reference.RefreshLine(array, "resourceAmount");
				}
			}
		}
	}

	// Token: 0x0600940D RID: 37901 RVA: 0x00393A1C File Offset: 0x00391C1C
	private void SetRowsActive()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			if (ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, ClusterManager.Instance.activeWorldId) == null)
			{
				this.currentlyDisplayedRows[keyValuePair.Key] = false;
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.diagnosticRows)
		{
			if (keyValuePair2.Value.activeSelf != this.currentlyDisplayedRows[keyValuePair2.Key])
			{
				keyValuePair2.Value.SetActive(this.currentlyDisplayedRows[keyValuePair2.Key]);
			}
		}
	}

	// Token: 0x0600940E RID: 37902 RVA: 0x001006DE File Offset: 0x000FE8DE
	public void Sim4000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshCharts();
	}

	// Token: 0x0600940F RID: 37903 RVA: 0x001006EF File Offset: 0x000FE8EF
	public void Sim1000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshRows();
	}

	// Token: 0x040072DE RID: 29406
	private Dictionary<string, GameObject> diagnosticRows = new Dictionary<string, GameObject>();

	// Token: 0x040072DF RID: 29407
	private Dictionary<string, Dictionary<string, GameObject>> criteriaRows = new Dictionary<string, Dictionary<string, GameObject>>();

	// Token: 0x040072E0 RID: 29408
	public GameObject rootListContainer;

	// Token: 0x040072E1 RID: 29409
	public GameObject diagnosticLinePrefab;

	// Token: 0x040072E2 RID: 29410
	public GameObject subDiagnosticLinePrefab;

	// Token: 0x040072E3 RID: 29411
	public KButton closeButton;

	// Token: 0x040072E4 RID: 29412
	public bool allowRefresh = true;

	// Token: 0x040072E5 RID: 29413
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x040072E6 RID: 29414
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x040072E7 RID: 29415
	public static AllDiagnosticsScreen Instance;

	// Token: 0x040072E8 RID: 29416
	public Dictionary<Tag, bool> currentlyDisplayedRows = new Dictionary<Tag, bool>();

	// Token: 0x040072E9 RID: 29417
	public Dictionary<Tag, bool> subrowContainerOpen = new Dictionary<Tag, bool>();

	// Token: 0x040072EA RID: 29418
	[SerializeField]
	private RectTransform debugNotificationToggleCotainer;
}
