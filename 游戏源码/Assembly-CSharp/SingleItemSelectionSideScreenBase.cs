using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleItemSelectionSideScreenBase : SideScreenContent
{
	public class ItemComparer : IComparer<SingleItemSelectionRow>
	{
		private Tag firstTag;

		public ItemComparer()
		{
		}

		public ItemComparer(Tag firstTag)
		{
			this.firstTag = firstTag;
		}

		public int Compare(SingleItemSelectionRow x, SingleItemSelectionRow y)
		{
			if (x == y)
			{
				return 0;
			}
			if (firstTag.IsValid)
			{
				if (x.tag == firstTag && y.tag != firstTag)
				{
					return 1;
				}
				if (x.tag != firstTag && y.tag == firstTag)
				{
					return -1;
				}
			}
			return x.tag.ProperNameStripLink().CompareTo(y.tag.ProperNameStripLink());
		}
	}

	public class Category
	{
		public enum UnfoldedStates
		{
			Folded,
			Unfolded
		}

		public Action<SingleItemSelectionRow> ItemRemoved;

		public Action<Category> ToggleClicked;

		protected HierarchyReferences hierarchyReferences;

		protected List<SingleItemSelectionRow> items;

		public Tag CategoryTag { get; protected set; }

		public bool IsProhibited { get; protected set; }

		public bool IsVisible
		{
			get
			{
				if (hierarchyReferences != null)
				{
					return hierarchyReferences.gameObject.activeSelf;
				}
				return false;
			}
		}

		protected RectTransform entries => hierarchyReferences.GetReference<RectTransform>("Entries");

		protected LocText title => hierarchyReferences.GetReference<LocText>("Label");

		protected MultiToggle toggle => hierarchyReferences.GetReference<MultiToggle>("Toggle");

		public virtual void ToggleUnfoldedState()
		{
			switch ((UnfoldedStates)toggle.CurrentState)
			{
			case UnfoldedStates.Folded:
				SetUnfoldedState(UnfoldedStates.Unfolded);
				break;
			case UnfoldedStates.Unfolded:
				SetUnfoldedState(UnfoldedStates.Folded);
				break;
			}
		}

		public virtual void SetUnfoldedState(UnfoldedStates new_state)
		{
			toggle.ChangeState((int)new_state);
			entries.gameObject.SetActive(new_state == UnfoldedStates.Unfolded);
		}

		public virtual void SetTitle(string text)
		{
			title.text = text;
		}

		public Category(HierarchyReferences references, Tag categoryTag)
		{
			CategoryTag = categoryTag;
			hierarchyReferences = references;
			toggle.onClick = OnToggleClicked;
			SetTitle(categoryTag.ProperName());
		}

		public virtual void OnToggleClicked()
		{
			ToggleClicked?.Invoke(this);
		}

		public virtual void AddItems(SingleItemSelectionRow[] _items)
		{
			if (items == null)
			{
				items = new List<SingleItemSelectionRow>(_items);
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				if (!items.Contains(_items[i]))
				{
					_items[i].transform.SetParent(entries, worldPositionStays: false);
					items.Add(_items[i]);
				}
			}
		}

		public virtual void AddItem(SingleItemSelectionRow item)
		{
			if (items == null)
			{
				items = new List<SingleItemSelectionRow>();
			}
			item.transform.SetParent(entries, worldPositionStays: false);
			items.Add(item);
		}

		public virtual bool InitializeItemList(int size)
		{
			if (items == null)
			{
				items = new List<SingleItemSelectionRow>(size);
				return true;
			}
			return false;
		}

		public virtual void SetVisibilityState(bool isVisible)
		{
			hierarchyReferences.gameObject.SetActive(isVisible && !IsProhibited);
		}

		public virtual void RemoveAllItems()
		{
			for (int i = 0; i < items.Count; i++)
			{
				SingleItemSelectionRow obj = items[i];
				ItemRemoved?.Invoke(obj);
			}
			items.Clear();
			items = null;
		}

		public virtual SingleItemSelectionRow RemoveItem(Tag itemTag)
		{
			if (items != null)
			{
				SingleItemSelectionRow singleItemSelectionRow = items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
				if (singleItemSelectionRow != null)
				{
					ItemRemoved?.Invoke(singleItemSelectionRow);
					return singleItemSelectionRow;
				}
			}
			return null;
		}

		public virtual bool RemoveItem(SingleItemSelectionRow itemRow)
		{
			if (items != null && items.Remove(itemRow))
			{
				ItemRemoved?.Invoke(itemRow);
				return true;
			}
			return false;
		}

		public SingleItemSelectionRow GetItem(Tag itemTag)
		{
			if (items == null)
			{
				return null;
			}
			return items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
		}

		public int FilterItemsBySearch(string searchValue)
		{
			int num = 0;
			if (items != null)
			{
				foreach (SingleItemSelectionRow item in items)
				{
					bool flag = TagContainsSearchWord(item.tag, searchValue);
					item.SetVisibleState(flag);
					if (flag)
					{
						num++;
					}
				}
			}
			return num;
		}

		public void Sort()
		{
			if (items == null)
			{
				return;
			}
			items.Sort(itemRowComparer);
			foreach (SingleItemSelectionRow item in items)
			{
				item.transform.SetAsLastSibling();
			}
		}

		public void SendToLastSibiling()
		{
			hierarchyReferences.transform.SetAsLastSibling();
		}

		public void SetProihibedState(bool isPohibited)
		{
			IsProhibited = isPohibited;
			if (IsVisible && isPohibited)
			{
				SetVisibilityState(isVisible: false);
			}
		}
	}

	[Space]
	[Header("Settings")]
	[SerializeField]
	private SearchBar searchbar;

	[SerializeField]
	protected HierarchyReferences original_CategoryRow;

	[SerializeField]
	protected SingleItemSelectionRow original_ItemRow;

	protected SortedDictionary<Tag, Category> categories = new SortedDictionary<Tag, Category>(categoryComparer);

	private Dictionary<Tag, SingleItemSelectionRow> pooledRows = new Dictionary<Tag, SingleItemSelectionRow>();

	private static TagNameComparer categoryComparer = new TagNameComparer(GameTags.Void);

	private static ItemComparer itemRowComparer = new ItemComparer(GameTags.Void);

	protected SingleItemSelectionRow CurrentSelectedItem { get; private set; }

	private static bool TagContainsSearchWord(Tag tag, string search)
	{
		if (!string.IsNullOrEmpty(search))
		{
			return tag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		}
		return true;
	}

	protected override void OnPrefabInit()
	{
		if (searchbar != null)
		{
			searchbar.EditingStateChanged = OnSearchbarEditStateChanged;
			searchbar.ValueChanged = OnSearchBarValueChanged;
			activateOnSpawn = true;
		}
		base.OnPrefabInit();
	}

	protected virtual void OnSearchbarEditStateChanged(bool isEditing)
	{
		base.isEditing = isEditing;
	}

	protected virtual void OnSearchBarValueChanged(string value)
	{
		foreach (Tag key in categories.Keys)
		{
			Category category = categories[key];
			bool flag = TagContainsSearchWord(key, value);
			int num = category.FilterItemsBySearch(flag ? null : value);
			category.SetUnfoldedState((num > 0) ? Category.UnfoldedStates.Unfolded : Category.UnfoldedStates.Folded);
			category.SetVisibilityState(flag || num > 0);
		}
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

	public virtual void SetData(Dictionary<Tag, HashSet<Tag>> data)
	{
		ProhibitAllCategories();
		foreach (Tag key in data.Keys)
		{
			ICollection<Tag> items = data[key];
			CreateCategoryWithItems(key, items);
		}
		SortAll();
		if (searchbar != null && !string.IsNullOrEmpty(searchbar.CurrentSearchValue))
		{
			searchbar.ClearSearch();
		}
	}

	public virtual Category CreateCategoryWithItems(Tag categoryTag, ICollection<Tag> items)
	{
		Category orCreateEmptyCategory = GetOrCreateEmptyCategory(categoryTag);
		if (!orCreateEmptyCategory.InitializeItemList(items.Count))
		{
			orCreateEmptyCategory.RemoveAllItems();
		}
		foreach (Tag item in items)
		{
			SingleItemSelectionRow orCreateItemRow = GetOrCreateItemRow(item);
			orCreateEmptyCategory.AddItem(orCreateItemRow);
		}
		return orCreateEmptyCategory;
	}

	public virtual Category GetOrCreateEmptyCategory(Tag categoryTag)
	{
		original_CategoryRow.gameObject.SetActive(value: false);
		Category value = null;
		if (!categories.TryGetValue(categoryTag, out value))
		{
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(original_CategoryRow.gameObject, original_CategoryRow.transform.parent.gameObject);
			hierarchyReferences.gameObject.SetActive(value: true);
			value = new Category(hierarchyReferences, categoryTag);
			value.ItemRemoved = RecycleItemRow;
			Category category = value;
			category.ToggleClicked = (Action<Category>)Delegate.Combine(category.ToggleClicked, new Action<Category>(CategoryToggleClicked));
			categories.Add(categoryTag, value);
		}
		else
		{
			value.SetProihibedState(isPohibited: false);
			value.SetVisibilityState(isVisible: true);
		}
		return value;
	}

	public virtual SingleItemSelectionRow GetOrCreateItemRow(Tag itemTag)
	{
		original_ItemRow.gameObject.SetActive(value: false);
		SingleItemSelectionRow value = null;
		if (!pooledRows.TryGetValue(itemTag, out value))
		{
			value = Util.KInstantiateUI<SingleItemSelectionRow>(original_ItemRow.gameObject, original_ItemRow.transform.parent.gameObject);
			SingleItemSelectionRow singleItemSelectionRow = value;
			Tag tag = itemTag;
			singleItemSelectionRow.name = "Item-" + tag.ToString();
		}
		else
		{
			pooledRows.Remove(itemTag);
		}
		value.gameObject.SetActive(value: true);
		value.SetTag(itemTag);
		value.Clicked = ItemRowClicked;
		value.SetVisibleState(isVisible: true);
		return value;
	}

	public Category GetCategoryWithItem(Tag itemTag, bool includeNotVisibleCategories = false)
	{
		foreach (Category value in categories.Values)
		{
			if ((includeNotVisibleCategories || value.IsVisible) && value.GetItem(itemTag) != null)
			{
				return value;
			}
		}
		return null;
	}

	public virtual void SetSelectedItem(SingleItemSelectionRow itemRow)
	{
		if (CurrentSelectedItem != null)
		{
			CurrentSelectedItem.SetSelected(selected: false);
		}
		CurrentSelectedItem = itemRow;
		if (itemRow != null)
		{
			itemRow.SetSelected(selected: true);
		}
	}

	public virtual bool SetSelectedItem(Tag itemTag)
	{
		foreach (Tag key in categories.Keys)
		{
			Category category = categories[key];
			if (category.IsVisible)
			{
				SingleItemSelectionRow item = category.GetItem(itemTag);
				if (item != null)
				{
					SetSelectedItem(item);
					return true;
				}
			}
		}
		return false;
	}

	public virtual void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		SetSelectedItem(rowClicked);
	}

	public virtual void CategoryToggleClicked(Category categoryClicked)
	{
		categoryClicked.ToggleUnfoldedState();
	}

	private void RecycleItemRow(SingleItemSelectionRow row)
	{
		if (pooledRows.ContainsKey(row.tag))
		{
			Debug.LogError($"Recycling an item row with tag {row.tag} that was already in the recycle pool");
		}
		if (CurrentSelectedItem == row)
		{
			SetSelectedItem((SingleItemSelectionRow)null);
		}
		row.Clicked = null;
		row.SetSelected(selected: false);
		row.transform.SetParent(original_ItemRow.transform.parent.parent);
		row.gameObject.SetActive(value: false);
		pooledRows.Add(row.tag, row);
	}

	private void ProhibitAllCategories()
	{
		foreach (Category value in categories.Values)
		{
			value.SetProihibedState(isPohibited: true);
		}
	}

	public virtual void SortAll()
	{
		foreach (Category value in categories.Values)
		{
			if (value.IsVisible)
			{
				value.Sort();
				value.SendToLastSibiling();
			}
		}
	}
}
