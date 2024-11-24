using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FE9 RID: 8169
public class TreeFilterableSideScreen : SideScreenContent
{
	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x0600AD45 RID: 44357 RVA: 0x00110C13 File Offset: 0x0010EE13
	private bool InputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x0600AD46 RID: 44358 RVA: 0x00110C2A File Offset: 0x0010EE2A
	public bool IsStorage
	{
		get
		{
			return this.storage != null;
		}
	}

	// Token: 0x0600AD47 RID: 44359 RVA: 0x00110C38 File Offset: 0x0010EE38
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Initialize();
	}

	// Token: 0x0600AD48 RID: 44360 RVA: 0x0041160C File Offset: 0x0040F80C
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
		this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
		MultiToggle multiToggle = this.allCheckBox;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			TreeFilterableSideScreenRow.State allCheckboxState = this.GetAllCheckboxState();
			if (allCheckboxState > TreeFilterableSideScreenRow.State.Mixed)
			{
				if (allCheckboxState == TreeFilterableSideScreenRow.State.On)
				{
					this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
					return;
				}
			}
			else
			{
				this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
			}
		}));
		this.onlyAllowTransportItemsCheckBox.onClick = new System.Action(this.OnlyAllowTransportItemsClicked);
		this.onlyAllowSpicedItemsCheckBox.onClick = new System.Action(this.OnlyAllowSpicedItemsClicked);
		this.initialized = true;
	}

	// Token: 0x0600AD49 RID: 44361 RVA: 0x004116A0 File Offset: 0x0040F8A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
		this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
		this.onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
		this.inputField.ActivateInputField();
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.InitSearch();
	}

	// Token: 0x0600AD4A RID: 44362 RVA: 0x001102A3 File Offset: 0x0010E4A3
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x0600AD4B RID: 44363 RVA: 0x001102B9 File Offset: 0x0010E4B9
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600AD4C RID: 44364 RVA: 0x001102B9 File Offset: 0x0010E4B9
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600AD4D RID: 44365 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x0600AD4E RID: 44366 RVA: 0x00411754 File Offset: 0x0040F954
	private void UpdateAllCheckBoxVisualState()
	{
		switch (this.GetAllCheckboxState())
		{
		case TreeFilterableSideScreenRow.State.Off:
			this.allCheckBox.ChangeState(0);
			return;
		case TreeFilterableSideScreenRow.State.Mixed:
			this.allCheckBox.ChangeState(1);
			return;
		case TreeFilterableSideScreenRow.State.On:
			this.allCheckBox.ChangeState(2);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600AD4F RID: 44367 RVA: 0x004117A4 File Offset: 0x0040F9A4
	public void Update()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.visualDirty)
			{
				this.visualDirty = true;
				break;
			}
		}
		if (this.visualDirty)
		{
			foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
			{
				keyValuePair2.Value.RefreshRowElements();
				keyValuePair2.Value.UpdateCheckBoxVisualState();
			}
			this.UpdateAllCheckBoxVisualState();
			this.visualDirty = false;
		}
	}

	// Token: 0x0600AD50 RID: 44368 RVA: 0x00110C46 File Offset: 0x0010EE46
	private void OnlyAllowTransportItemsClicked()
	{
		this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
	}

	// Token: 0x0600AD51 RID: 44369 RVA: 0x00110C61 File Offset: 0x0010EE61
	private void OnlyAllowSpicedItemsClicked()
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		component.SpicedFoodOnly = !component.SpicedFoodOnly;
	}

	// Token: 0x0600AD52 RID: 44370 RVA: 0x00411870 File Offset: 0x0040FA70
	private TreeFilterableSideScreenRow.State GetAllCheckboxState()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.standardCommodity)
			{
				switch (keyValuePair.Value.GetState())
				{
				case TreeFilterableSideScreenRow.State.Off:
					flag2 = true;
					break;
				case TreeFilterableSideScreenRow.State.Mixed:
					flag3 = true;
					break;
				case TreeFilterableSideScreenRow.State.On:
					flag = true;
					break;
				}
			}
		}
		if (flag3)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
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
		return TreeFilterableSideScreenRow.State.Off;
	}

	// Token: 0x0600AD53 RID: 44371 RVA: 0x00411920 File Offset: 0x0040FB20
	private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.standardCommodity)
					{
						keyValuePair.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
					}
				}
				goto IL_AB;
			}
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			goto IL_AB;
		case TreeFilterableSideScreenRow.State.On:
			break;
		default:
			goto IL_AB;
		}
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
		{
			if (keyValuePair2.Value.standardCommodity)
			{
				keyValuePair2.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
			}
		}
		IL_AB:
		this.visualDirty = true;
	}

	// Token: 0x0600AD54 RID: 44372 RVA: 0x00110C7C File Offset: 0x0010EE7C
	public bool GetElementTagAcceptedState(Tag t)
	{
		return this.targetFilterable.ContainsTag(t);
	}

	// Token: 0x0600AD55 RID: 44373 RVA: 0x004119FC File Offset: 0x0040FBFC
	public override bool IsValidForTarget(GameObject target)
	{
		TreeFilterable component = target.GetComponent<TreeFilterable>();
		Storage component2 = target.GetComponent<Storage>();
		return component != null && target.GetComponent<FlatTagFilterable>() == null && component.showUserMenu && (component2 == null || component2.showInUI) && target.GetSMI<StorageTile.Instance>() == null;
	}

	// Token: 0x0600AD56 RID: 44374 RVA: 0x00110C8A File Offset: 0x0010EE8A
	private void ReconfigureForPreviousTarget()
	{
		global::Debug.Assert(this.target != null, "TreeFilterableSideScreen trying to restore null target.");
		this.SetTarget(this.target);
	}

	// Token: 0x0600AD57 RID: 44375 RVA: 0x00411A54 File Offset: 0x0040FC54
	public override void SetTarget(GameObject target)
	{
		this.Initialize();
		this.target = target;
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		this.contentMask.GetComponent<LayoutElement>().minHeight = (float)((this.targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall) ? 380 : 256);
		this.storage = this.targetFilterable.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		this.OnOnlyFetchMarkedItemsSettingChanged(null);
		this.OnOnlySpicedItemsSettingChanged(null);
		this.allCheckBoxLabel.SetText(this.targetFilterable.allResourceFilterLabelString);
		this.CreateCategories();
		this.CreateSpecialItemRows();
		this.titlebar.SetActive(false);
		if (this.storage.showSideScreenTitleBar)
		{
			this.titlebar.SetActive(true);
			this.titlebar.GetComponentInChildren<LocText>().SetText(this.storage.GetProperName());
		}
		if (!this.InputFieldEmpty)
		{
			this.ClearSearch();
		}
		this.ToggleSearchConfiguration(!this.InputFieldEmpty);
	}

	// Token: 0x0600AD58 RID: 44376 RVA: 0x00411BAC File Offset: 0x0040FDAC
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.onlyAllowTransportItemsCheckBox.ChangeState(this.storage.GetOnlyFetchMarkedItems() ? 1 : 0);
		if (this.storage.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyallowTransportItemsRow.SetActive(true);
			return;
		}
		this.onlyallowTransportItemsRow.SetActive(false);
	}

	// Token: 0x0600AD59 RID: 44377 RVA: 0x00411BFC File Offset: 0x0040FDFC
	private void OnOnlySpicedItemsSettingChanged(object data)
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		if (component != null)
		{
			this.onlyallowSpicedItemsRow.SetActive(true);
			this.onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
			return;
		}
		this.onlyallowSpicedItemsRow.SetActive(false);
	}

	// Token: 0x0600AD5A RID: 44378 RVA: 0x00110CAE File Offset: 0x0010EEAE
	public bool IsTagAllowed(Tag tag)
	{
		return this.targetFilterable.AcceptedTags.Contains(tag);
	}

	// Token: 0x0600AD5B RID: 44379 RVA: 0x00110CC1 File Offset: 0x0010EEC1
	public void AddTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.AddTagToFilter(tag);
	}

	// Token: 0x0600AD5C RID: 44380 RVA: 0x00110CDE File Offset: 0x0010EEDE
	public void RemoveTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.RemoveTagFromFilter(tag);
	}

	// Token: 0x0600AD5D RID: 44381 RVA: 0x00411C50 File Offset: 0x0040FE50
	private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(ICollection<Tag> tags)
	{
		List<TreeFilterableSideScreen.TagOrderInfo> list = new List<TreeFilterableSideScreen.TagOrderInfo>();
		foreach (Tag tag in tags)
		{
			list.Add(new TreeFilterableSideScreen.TagOrderInfo
			{
				tag = tag,
				strippedName = tag.ProperNameStripLink()
			});
		}
		list.Sort((TreeFilterableSideScreen.TagOrderInfo a, TreeFilterableSideScreen.TagOrderInfo b) => a.strippedName.CompareTo(b.strippedName));
		return list;
	}

	// Token: 0x0600AD5E RID: 44382 RVA: 0x00411CE4 File Offset: 0x0040FEE4
	private TreeFilterableSideScreenRow AddRow(Tag rowTag)
	{
		if (this.tagRowMap.ContainsKey(rowTag))
		{
			return this.tagRowMap[rowTag];
		}
		TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
		freeElement.Parent = this;
		freeElement.standardCommodity = !STORAGEFILTERS.SPECIAL_STORAGE.Contains(rowTag);
		this.tagRowMap.Add(rowTag, freeElement);
		Dictionary<Tag, bool> dictionary = new Dictionary<Tag, bool>();
		foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)))
		{
			dictionary.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
		}
		freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), dictionary);
		freeElement.transform.SetAsLastSibling();
		return freeElement;
	}

	// Token: 0x0600AD5F RID: 44383 RVA: 0x00110CFB File Offset: 0x0010EEFB
	public float GetAmountInStorage(Tag tag)
	{
		if (!this.IsStorage)
		{
			return 0f;
		}
		return this.storage.GetMassAvailable(tag);
	}

	// Token: 0x0600AD60 RID: 44384 RVA: 0x00411DE8 File Offset: 0x0040FFE8
	private void CreateCategories()
	{
		if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
		{
			bool flag = this.target.GetComponent<CreatureDeliveryPoint>() != null;
			foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(this.storage.storageFilters))
			{
				Tag tag = tagOrderInfo.tag;
				if (flag || DiscoveredResources.Instance.IsDiscovered(tag))
				{
					this.AddRow(tag);
				}
			}
			this.visualDirty = true;
			return;
		}
		global::Debug.LogError("If you're filtering, your storage filter should have the filters set on it");
	}

	// Token: 0x0600AD61 RID: 44385 RVA: 0x00411EA8 File Offset: 0x004100A8
	private void CreateSpecialItemRows()
	{
		this.specialItemsHeader.transform.SetAsLastSibling();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				keyValuePair.Value.transform.transform.SetAsLastSibling();
			}
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x0600AD62 RID: 44386 RVA: 0x00411F30 File Offset: 0x00410130
	private void RefreshSpecialItemsHeader()
	{
		bool active = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				active = true;
				break;
			}
		}
		this.specialItemsHeader.gameObject.SetActive(active);
	}

	// Token: 0x0600AD63 RID: 44387 RVA: 0x00110D17 File Offset: 0x0010EF17
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.target != null && (this.tagRowMap == null || this.tagRowMap.Count == 0))
		{
			this.ReconfigureForPreviousTarget();
		}
	}

	// Token: 0x0600AD64 RID: 44388 RVA: 0x00411FA4 File Offset: 0x004101A4
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.storage != null)
		{
			this.storage.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
			this.storage.Unsubscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		}
		this.rowPool.ClearAll();
		this.elementPool.ClearAll();
		this.tagRowMap.Clear();
	}

	// Token: 0x0600AD65 RID: 44389 RVA: 0x00412020 File Offset: 0x00410220
	private void RecordRowExpandedStatus()
	{
		this.rowExpandedStatusMemory.Clear();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			this.rowExpandedStatusMemory.Add(keyValuePair.Key, keyValuePair.Value.ArrowExpanded);
		}
	}

	// Token: 0x0600AD66 RID: 44390 RVA: 0x00412098 File Offset: 0x00410298
	private void RestoreRowExpandedStatus()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (this.rowExpandedStatusMemory.ContainsKey(keyValuePair.Key))
			{
				keyValuePair.Value.SetArrowToggleState(this.rowExpandedStatusMemory[keyValuePair.Key]);
			}
		}
	}

	// Token: 0x0600AD67 RID: 44391 RVA: 0x00412118 File Offset: 0x00410318
	private void InitSearch()
	{
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.RecordRowExpandedStatus();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
			KScreenManager.Instance.RefreshStack();
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			if (this.InputFieldEmpty)
			{
				this.RestoreRowExpandedStatus();
			}
			this.ToggleSearchConfiguration(!this.InputFieldEmpty);
			this.UpdateSearchFilter();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.clearButton.onClick += delegate()
		{
			if (!this.InputFieldEmpty)
			{
				this.ClearSearch();
			}
		};
	}

	// Token: 0x0600AD68 RID: 44392 RVA: 0x004121BC File Offset: 0x004103BC
	private void ToggleSearchConfiguration(bool searching)
	{
		this.configurationRowsContainer.gameObject.SetActive(!searching);
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.ShowToggleBox(!searching);
		}
		if (searching)
		{
			this.specialItemsHeader.gameObject.SetActive(false);
			return;
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x0600AD69 RID: 44393 RVA: 0x00110D48 File Offset: 0x0010EF48
	private void ClearSearch()
	{
		this.inputField.text = "";
		this.RestoreRowExpandedStatus();
		this.ToggleSearchConfiguration(false);
	}

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x0600AD6A RID: 44394 RVA: 0x00110D67 File Offset: 0x0010EF67
	public string CurrentSearchValue
	{
		get
		{
			if (this.inputField.text == null)
			{
				return "";
			}
			return this.inputField.text;
		}
	}

	// Token: 0x0600AD6B RID: 44395 RVA: 0x00412248 File Offset: 0x00410448
	private void UpdateSearchFilter()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.FilterAgainstSearch(keyValuePair.Key, this.CurrentSearchValue);
		}
	}

	// Token: 0x040087FF RID: 34815
	[SerializeField]
	private MultiToggle allCheckBox;

	// Token: 0x04008800 RID: 34816
	[SerializeField]
	private LocText allCheckBoxLabel;

	// Token: 0x04008801 RID: 34817
	[SerializeField]
	private GameObject specialItemsHeader;

	// Token: 0x04008802 RID: 34818
	[SerializeField]
	private MultiToggle onlyAllowTransportItemsCheckBox;

	// Token: 0x04008803 RID: 34819
	[SerializeField]
	private GameObject onlyallowTransportItemsRow;

	// Token: 0x04008804 RID: 34820
	[SerializeField]
	private MultiToggle onlyAllowSpicedItemsCheckBox;

	// Token: 0x04008805 RID: 34821
	[SerializeField]
	private GameObject onlyallowSpicedItemsRow;

	// Token: 0x04008806 RID: 34822
	[SerializeField]
	private TreeFilterableSideScreenRow rowPrefab;

	// Token: 0x04008807 RID: 34823
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x04008808 RID: 34824
	[SerializeField]
	private TreeFilterableSideScreenElement elementPrefab;

	// Token: 0x04008809 RID: 34825
	[SerializeField]
	private GameObject titlebar;

	// Token: 0x0400880A RID: 34826
	[SerializeField]
	private GameObject contentMask;

	// Token: 0x0400880B RID: 34827
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x0400880C RID: 34828
	[SerializeField]
	private KButton clearButton;

	// Token: 0x0400880D RID: 34829
	[SerializeField]
	private GameObject configurationRowsContainer;

	// Token: 0x0400880E RID: 34830
	private GameObject target;

	// Token: 0x0400880F RID: 34831
	private bool visualDirty;

	// Token: 0x04008810 RID: 34832
	private bool initialized;

	// Token: 0x04008811 RID: 34833
	private KImage onlyAllowTransportItemsImg;

	// Token: 0x04008812 RID: 34834
	public UIPool<TreeFilterableSideScreenElement> elementPool;

	// Token: 0x04008813 RID: 34835
	private UIPool<TreeFilterableSideScreenRow> rowPool;

	// Token: 0x04008814 RID: 34836
	private TreeFilterable targetFilterable;

	// Token: 0x04008815 RID: 34837
	private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();

	// Token: 0x04008816 RID: 34838
	private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();

	// Token: 0x04008817 RID: 34839
	private Storage storage;

	// Token: 0x02001FEA RID: 8170
	private struct TagOrderInfo
	{
		// Token: 0x04008818 RID: 34840
		public Tag tag;

		// Token: 0x04008819 RID: 34841
		public string strippedName;
	}
}
