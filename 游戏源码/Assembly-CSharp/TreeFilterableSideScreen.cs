using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

public class TreeFilterableSideScreen : SideScreenContent
{
	private struct TagOrderInfo
	{
		public Tag tag;

		public string strippedName;
	}

	[SerializeField]
	private MultiToggle allCheckBox;

	[SerializeField]
	private LocText allCheckBoxLabel;

	[SerializeField]
	private GameObject specialItemsHeader;

	[SerializeField]
	private MultiToggle onlyAllowTransportItemsCheckBox;

	[SerializeField]
	private GameObject onlyallowTransportItemsRow;

	[SerializeField]
	private MultiToggle onlyAllowSpicedItemsCheckBox;

	[SerializeField]
	private GameObject onlyallowSpicedItemsRow;

	[SerializeField]
	private TreeFilterableSideScreenRow rowPrefab;

	[SerializeField]
	private GameObject rowGroup;

	[SerializeField]
	private TreeFilterableSideScreenElement elementPrefab;

	[SerializeField]
	private GameObject titlebar;

	[SerializeField]
	private GameObject contentMask;

	[SerializeField]
	private KInputTextField inputField;

	[SerializeField]
	private KButton clearButton;

	[SerializeField]
	private GameObject configurationRowsContainer;

	private GameObject target;

	private bool visualDirty;

	private bool initialized;

	private KImage onlyAllowTransportItemsImg;

	public UIPool<TreeFilterableSideScreenElement> elementPool;

	private UIPool<TreeFilterableSideScreenRow> rowPool;

	private TreeFilterable targetFilterable;

	private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();

	private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();

	private Storage storage;

	private bool InputFieldEmpty => inputField.text == "";

	public bool IsStorage => storage != null;

	public string CurrentSearchValue
	{
		get
		{
			if (inputField.text == null)
			{
				return "";
			}
			return inputField.text;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Initialize();
	}

	private void Initialize()
	{
		if (initialized)
		{
			return;
		}
		rowPool = new UIPool<TreeFilterableSideScreenRow>(rowPrefab);
		elementPool = new UIPool<TreeFilterableSideScreenElement>(elementPrefab);
		MultiToggle multiToggle = allCheckBox;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			switch (GetAllCheckboxState())
			{
			case TreeFilterableSideScreenRow.State.On:
				SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
				break;
			case TreeFilterableSideScreenRow.State.Off:
			case TreeFilterableSideScreenRow.State.Mixed:
				SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
				break;
			}
		});
		onlyAllowTransportItemsCheckBox.onClick = OnlyAllowTransportItemsClicked;
		onlyAllowSpicedItemsCheckBox.onClick = OnlyAllowSpicedItemsClicked;
		initialized = true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
		onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
		onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
		inputField.ActivateInputField();
		inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		InitSearch();
	}

	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && base.isEditing)
		{
			e.Consumed = true;
		}
	}

	public override void OnKeyUp(KButtonEvent e)
	{
		if (!e.Consumed && base.isEditing)
		{
			e.Consumed = true;
		}
	}

	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	private void UpdateAllCheckBoxVisualState()
	{
		switch (GetAllCheckboxState())
		{
		case TreeFilterableSideScreenRow.State.Off:
			allCheckBox.ChangeState(0);
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			allCheckBox.ChangeState(1);
			break;
		case TreeFilterableSideScreenRow.State.On:
			allCheckBox.ChangeState(2);
			break;
		}
	}

	public void Update()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			if (item.Value.visualDirty)
			{
				visualDirty = true;
				break;
			}
		}
		if (!visualDirty)
		{
			return;
		}
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item2 in tagRowMap)
		{
			item2.Value.RefreshRowElements();
			item2.Value.UpdateCheckBoxVisualState();
		}
		UpdateAllCheckBoxVisualState();
		visualDirty = false;
	}

	private void OnlyAllowTransportItemsClicked()
	{
		storage.SetOnlyFetchMarkedItems(!storage.GetOnlyFetchMarkedItems());
	}

	private void OnlyAllowSpicedItemsClicked()
	{
		FoodStorage component = storage.GetComponent<FoodStorage>();
		component.SpicedFoodOnly = !component.SpicedFoodOnly;
	}

	private TreeFilterableSideScreenRow.State GetAllCheckboxState()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			if (item.Value.standardCommodity)
			{
				switch (item.Value.GetState())
				{
				case TreeFilterableSideScreenRow.State.Mixed:
					flag3 = true;
					break;
				case TreeFilterableSideScreenRow.State.On:
					flag = true;
					break;
				case TreeFilterableSideScreenRow.State.Off:
					flag2 = true;
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

	private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
			{
				if (item.Value.standardCommodity)
				{
					item.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
				}
			}
			break;
		case TreeFilterableSideScreenRow.State.On:
			foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item2 in tagRowMap)
			{
				if (item2.Value.standardCommodity)
				{
					item2.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
				}
			}
			break;
		}
		visualDirty = true;
	}

	public bool GetElementTagAcceptedState(Tag t)
	{
		return targetFilterable.ContainsTag(t);
	}

	public override bool IsValidForTarget(GameObject target)
	{
		TreeFilterable component = target.GetComponent<TreeFilterable>();
		Storage component2 = target.GetComponent<Storage>();
		if (component != null && target.GetComponent<FlatTagFilterable>() == null && component.showUserMenu && (component2 == null || component2.showInUI))
		{
			return target.GetSMI<StorageTile.Instance>() == null;
		}
		return false;
	}

	private void ReconfigureForPreviousTarget()
	{
		Debug.Assert(target != null, "TreeFilterableSideScreen trying to restore null target.");
		SetTarget(target);
	}

	public override void SetTarget(GameObject target)
	{
		Initialize();
		this.target = target;
		if (target == null)
		{
			Debug.LogError("The target object provided was null");
			return;
		}
		targetFilterable = target.GetComponent<TreeFilterable>();
		if (targetFilterable == null)
		{
			Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		contentMask.GetComponent<LayoutElement>().minHeight = ((targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall) ? 380 : 256);
		storage = targetFilterable.GetComponent<Storage>();
		storage.Subscribe(644822890, OnOnlyFetchMarkedItemsSettingChanged);
		storage.Subscribe(1163645216, OnOnlySpicedItemsSettingChanged);
		OnOnlyFetchMarkedItemsSettingChanged(null);
		OnOnlySpicedItemsSettingChanged(null);
		allCheckBoxLabel.SetText(targetFilterable.allResourceFilterLabelString);
		CreateCategories();
		CreateSpecialItemRows();
		titlebar.SetActive(value: false);
		if (storage.showSideScreenTitleBar)
		{
			titlebar.SetActive(value: true);
			titlebar.GetComponentInChildren<LocText>().SetText(storage.GetProperName());
		}
		if (!InputFieldEmpty)
		{
			ClearSearch();
		}
		ToggleSearchConfiguration(!InputFieldEmpty);
	}

	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		onlyAllowTransportItemsCheckBox.ChangeState(storage.GetOnlyFetchMarkedItems() ? 1 : 0);
		if (storage.allowSettingOnlyFetchMarkedItems)
		{
			onlyallowTransportItemsRow.SetActive(value: true);
		}
		else
		{
			onlyallowTransportItemsRow.SetActive(value: false);
		}
	}

	private void OnOnlySpicedItemsSettingChanged(object data)
	{
		FoodStorage component = storage.GetComponent<FoodStorage>();
		if (component != null)
		{
			onlyallowSpicedItemsRow.SetActive(value: true);
			onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
		}
		else
		{
			onlyallowSpicedItemsRow.SetActive(value: false);
		}
	}

	public bool IsTagAllowed(Tag tag)
	{
		return targetFilterable.AcceptedTags.Contains(tag);
	}

	public void AddTag(Tag tag)
	{
		if (!(targetFilterable == null))
		{
			targetFilterable.AddTagToFilter(tag);
		}
	}

	public void RemoveTag(Tag tag)
	{
		if (!(targetFilterable == null))
		{
			targetFilterable.RemoveTagFromFilter(tag);
		}
	}

	private List<TagOrderInfo> GetTagsSortedAlphabetically(ICollection<Tag> tags)
	{
		List<TagOrderInfo> list = new List<TagOrderInfo>();
		foreach (Tag tag in tags)
		{
			list.Add(new TagOrderInfo
			{
				tag = tag,
				strippedName = tag.ProperNameStripLink()
			});
		}
		list.Sort((TagOrderInfo a, TagOrderInfo b) => a.strippedName.CompareTo(b.strippedName));
		return list;
	}

	private TreeFilterableSideScreenRow AddRow(Tag rowTag)
	{
		if (tagRowMap.ContainsKey(rowTag))
		{
			return tagRowMap[rowTag];
		}
		TreeFilterableSideScreenRow freeElement = rowPool.GetFreeElement(rowGroup, forceActive: true);
		freeElement.Parent = this;
		freeElement.standardCommodity = !STORAGEFILTERS.SPECIAL_STORAGE.Contains(rowTag);
		tagRowMap.Add(rowTag, freeElement);
		Dictionary<Tag, bool> dictionary = new Dictionary<Tag, bool>();
		foreach (TagOrderInfo item in GetTagsSortedAlphabetically(DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)))
		{
			dictionary.Add(item.tag, targetFilterable.ContainsTag(item.tag) || targetFilterable.ContainsTag(rowTag));
		}
		freeElement.SetElement(rowTag, targetFilterable.ContainsTag(rowTag), dictionary);
		freeElement.transform.SetAsLastSibling();
		return freeElement;
	}

	public float GetAmountInStorage(Tag tag)
	{
		if (!IsStorage)
		{
			return 0f;
		}
		return storage.GetMassAvailable(tag);
	}

	private void CreateCategories()
	{
		if (storage.storageFilters != null && storage.storageFilters.Count >= 1)
		{
			bool flag = target.GetComponent<CreatureDeliveryPoint>() != null;
			foreach (TagOrderInfo item in GetTagsSortedAlphabetically(storage.storageFilters))
			{
				Tag rowTag = item.tag;
				if (flag || DiscoveredResources.Instance.IsDiscovered(rowTag))
				{
					AddRow(rowTag);
				}
			}
			visualDirty = true;
		}
		else
		{
			Debug.LogError("If you're filtering, your storage filter should have the filters set on it");
		}
	}

	private void CreateSpecialItemRows()
	{
		specialItemsHeader.transform.SetAsLastSibling();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			if (!item.Value.standardCommodity)
			{
				item.Value.transform.transform.SetAsLastSibling();
			}
		}
		RefreshSpecialItemsHeader();
	}

	private void RefreshSpecialItemsHeader()
	{
		bool active = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			if (!item.Value.standardCommodity)
			{
				active = true;
				break;
			}
		}
		specialItemsHeader.gameObject.SetActive(active);
	}

	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (target != null && (tagRowMap == null || tagRowMap.Count == 0))
		{
			ReconfigureForPreviousTarget();
		}
	}

	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (storage != null)
		{
			storage.Unsubscribe(644822890, OnOnlyFetchMarkedItemsSettingChanged);
			storage.Unsubscribe(1163645216, OnOnlySpicedItemsSettingChanged);
		}
		rowPool.ClearAll();
		elementPool.ClearAll();
		tagRowMap.Clear();
	}

	private void RecordRowExpandedStatus()
	{
		rowExpandedStatusMemory.Clear();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			rowExpandedStatusMemory.Add(item.Key, item.Value.ArrowExpanded);
		}
	}

	private void RestoreRowExpandedStatus()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			if (rowExpandedStatusMemory.ContainsKey(item.Key))
			{
				item.Value.SetArrowToggleState(rowExpandedStatusMemory[item.Key]);
			}
		}
	}

	private void InitSearch()
	{
		KInputTextField kInputTextField = inputField;
		kInputTextField.onFocus = (System.Action)Delegate.Combine(kInputTextField.onFocus, (System.Action)delegate
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			RecordRowExpandedStatus();
		});
		inputField.onEndEdit.AddListener(delegate
		{
			base.isEditing = false;
			KScreenManager.Instance.RefreshStack();
		});
		inputField.onValueChanged.AddListener(delegate
		{
			if (InputFieldEmpty)
			{
				RestoreRowExpandedStatus();
			}
			ToggleSearchConfiguration(!InputFieldEmpty);
			UpdateSearchFilter();
		});
		inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		clearButton.onClick += delegate
		{
			if (!InputFieldEmpty)
			{
				ClearSearch();
			}
		};
	}

	private void ToggleSearchConfiguration(bool searching)
	{
		configurationRowsContainer.gameObject.SetActive(!searching);
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			item.Value.ShowToggleBox(!searching);
		}
		if (searching)
		{
			specialItemsHeader.gameObject.SetActive(value: false);
		}
		else
		{
			RefreshSpecialItemsHeader();
		}
	}

	private void ClearSearch()
	{
		inputField.text = "";
		RestoreRowExpandedStatus();
		ToggleSearchConfiguration(searching: false);
	}

	private void UpdateSearchFilter()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> item in tagRowMap)
		{
			item.Value.FilterAgainstSearch(item.Key, CurrentSearchValue);
		}
	}
}
