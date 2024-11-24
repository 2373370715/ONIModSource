using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001FD1 RID: 8145
public abstract class SingleItemSelectionSideScreenBase : SideScreenContent
{
	// Token: 0x0600AC78 RID: 44152 RVA: 0x00110267 File Offset: 0x0010E467
	private static bool TagContainsSearchWord(Tag tag, string search)
	{
		return string.IsNullOrEmpty(search) || tag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
	}

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x0600AC7A RID: 44154 RVA: 0x00110292 File Offset: 0x0010E492
	// (set) Token: 0x0600AC79 RID: 44153 RVA: 0x00110289 File Offset: 0x0010E489
	private protected SingleItemSelectionRow CurrentSelectedItem { protected get; private set; }

	// Token: 0x0600AC7B RID: 44155 RVA: 0x0040E8C0 File Offset: 0x0040CAC0
	protected override void OnPrefabInit()
	{
		if (this.searchbar != null)
		{
			this.searchbar.EditingStateChanged = new Action<bool>(this.OnSearchbarEditStateChanged);
			this.searchbar.ValueChanged = new Action<string>(this.OnSearchBarValueChanged);
			this.activateOnSpawn = true;
		}
		base.OnPrefabInit();
	}

	// Token: 0x0600AC7C RID: 44156 RVA: 0x0011029A File Offset: 0x0010E49A
	protected virtual void OnSearchbarEditStateChanged(bool isEditing)
	{
		base.isEditing = isEditing;
	}

	// Token: 0x0600AC7D RID: 44157 RVA: 0x0040E918 File Offset: 0x0040CB18
	protected virtual void OnSearchBarValueChanged(string value)
	{
		foreach (Tag tag in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[tag];
			bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(tag, value);
			int num = category.FilterItemsBySearch(flag ? null : value);
			category.SetUnfoldedState((num > 0) ? SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded : SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
			category.SetVisibilityState(flag || num > 0);
		}
	}

	// Token: 0x0600AC7E RID: 44158 RVA: 0x001102A3 File Offset: 0x0010E4A3
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x0600AC7F RID: 44159 RVA: 0x001102B9 File Offset: 0x0010E4B9
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

	// Token: 0x0600AC80 RID: 44160 RVA: 0x001102B9 File Offset: 0x0010E4B9
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

	// Token: 0x0600AC81 RID: 44161 RVA: 0x0040E9A8 File Offset: 0x0040CBA8
	public virtual void SetData(Dictionary<Tag, HashSet<Tag>> data)
	{
		this.ProhibitAllCategories();
		foreach (Tag tag in data.Keys)
		{
			ICollection<Tag> items = data[tag];
			this.CreateCategoryWithItems(tag, items);
		}
		this.SortAll();
		if (this.searchbar != null && !string.IsNullOrEmpty(this.searchbar.CurrentSearchValue))
		{
			this.searchbar.ClearSearch();
		}
	}

	// Token: 0x0600AC82 RID: 44162 RVA: 0x0040EA3C File Offset: 0x0040CC3C
	public virtual SingleItemSelectionSideScreenBase.Category CreateCategoryWithItems(Tag categoryTag, ICollection<Tag> items)
	{
		SingleItemSelectionSideScreenBase.Category orCreateEmptyCategory = this.GetOrCreateEmptyCategory(categoryTag);
		if (!orCreateEmptyCategory.InitializeItemList(items.Count))
		{
			orCreateEmptyCategory.RemoveAllItems();
		}
		foreach (Tag itemTag in items)
		{
			SingleItemSelectionRow orCreateItemRow = this.GetOrCreateItemRow(itemTag);
			orCreateEmptyCategory.AddItem(orCreateItemRow);
		}
		return orCreateEmptyCategory;
	}

	// Token: 0x0600AC83 RID: 44163 RVA: 0x0040EAAC File Offset: 0x0040CCAC
	public virtual SingleItemSelectionSideScreenBase.Category GetOrCreateEmptyCategory(Tag categoryTag)
	{
		this.original_CategoryRow.gameObject.SetActive(false);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (!this.categories.TryGetValue(categoryTag, out category))
		{
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.original_CategoryRow.gameObject, this.original_CategoryRow.transform.parent.gameObject, false);
			hierarchyReferences.gameObject.SetActive(true);
			category = new SingleItemSelectionSideScreenBase.Category(hierarchyReferences, categoryTag);
			category.ItemRemoved = new Action<SingleItemSelectionRow>(this.RecycleItemRow);
			SingleItemSelectionSideScreenBase.Category category2 = category;
			category2.ToggleClicked = (Action<SingleItemSelectionSideScreenBase.Category>)Delegate.Combine(category2.ToggleClicked, new Action<SingleItemSelectionSideScreenBase.Category>(this.CategoryToggleClicked));
			this.categories.Add(categoryTag, category);
		}
		else
		{
			category.SetProihibedState(false);
			category.SetVisibilityState(true);
		}
		return category;
	}

	// Token: 0x0600AC84 RID: 44164 RVA: 0x0040EB68 File Offset: 0x0040CD68
	public virtual SingleItemSelectionRow GetOrCreateItemRow(Tag itemTag)
	{
		this.original_ItemRow.gameObject.SetActive(false);
		SingleItemSelectionRow singleItemSelectionRow = null;
		if (!this.pooledRows.TryGetValue(itemTag, out singleItemSelectionRow))
		{
			singleItemSelectionRow = Util.KInstantiateUI<SingleItemSelectionRow>(this.original_ItemRow.gameObject, this.original_ItemRow.transform.parent.gameObject, false);
			UnityEngine.Object @object = singleItemSelectionRow;
			string str = "Item-";
			Tag tag = itemTag;
			@object.name = str + tag.ToString();
		}
		else
		{
			this.pooledRows.Remove(itemTag);
		}
		singleItemSelectionRow.gameObject.SetActive(true);
		singleItemSelectionRow.SetTag(itemTag);
		singleItemSelectionRow.Clicked = new Action<SingleItemSelectionRow>(this.ItemRowClicked);
		singleItemSelectionRow.SetVisibleState(true);
		return singleItemSelectionRow;
	}

	// Token: 0x0600AC85 RID: 44165 RVA: 0x0040EC1C File Offset: 0x0040CE1C
	public SingleItemSelectionSideScreenBase.Category GetCategoryWithItem(Tag itemTag, bool includeNotVisibleCategories = false)
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if ((includeNotVisibleCategories || category.IsVisible) && category.GetItem(itemTag) != null)
			{
				return category;
			}
		}
		return null;
	}

	// Token: 0x0600AC86 RID: 44166 RVA: 0x001102D3 File Offset: 0x0010E4D3
	public virtual void SetSelectedItem(SingleItemSelectionRow itemRow)
	{
		if (this.CurrentSelectedItem != null)
		{
			this.CurrentSelectedItem.SetSelected(false);
		}
		this.CurrentSelectedItem = itemRow;
		if (itemRow != null)
		{
			itemRow.SetSelected(true);
		}
	}

	// Token: 0x0600AC87 RID: 44167 RVA: 0x0040EC90 File Offset: 0x0040CE90
	public virtual bool SetSelectedItem(Tag itemTag)
	{
		foreach (Tag key in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[key];
			if (category.IsVisible)
			{
				SingleItemSelectionRow item = category.GetItem(itemTag);
				if (item != null)
				{
					this.SetSelectedItem(item);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600AC88 RID: 44168 RVA: 0x00110306 File Offset: 0x0010E506
	public virtual void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetSelectedItem(rowClicked);
	}

	// Token: 0x0600AC89 RID: 44169 RVA: 0x0011030F File Offset: 0x0010E50F
	public virtual void CategoryToggleClicked(SingleItemSelectionSideScreenBase.Category categoryClicked)
	{
		categoryClicked.ToggleUnfoldedState();
	}

	// Token: 0x0600AC8A RID: 44170 RVA: 0x0040ED18 File Offset: 0x0040CF18
	private void RecycleItemRow(SingleItemSelectionRow row)
	{
		if (this.pooledRows.ContainsKey(row.tag))
		{
			global::Debug.LogError(string.Format("Recycling an item row with tag {0} that was already in the recycle pool", row.tag));
		}
		if (this.CurrentSelectedItem == row)
		{
			this.SetSelectedItem(null);
		}
		row.Clicked = null;
		row.SetSelected(false);
		row.transform.SetParent(this.original_ItemRow.transform.parent.parent);
		row.gameObject.SetActive(false);
		this.pooledRows.Add(row.tag, row);
	}

	// Token: 0x0600AC8B RID: 44171 RVA: 0x0040EDB4 File Offset: 0x0040CFB4
	private void ProhibitAllCategories()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			category.SetProihibedState(true);
		}
	}

	// Token: 0x0600AC8C RID: 44172 RVA: 0x0040EE0C File Offset: 0x0040D00C
	public virtual void SortAll()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if (category.IsVisible)
			{
				category.Sort();
				category.SendToLastSibiling();
			}
		}
	}

	// Token: 0x0400877E RID: 34686
	[Space]
	[Header("Settings")]
	[SerializeField]
	private SearchBar searchbar;

	// Token: 0x0400877F RID: 34687
	[SerializeField]
	protected HierarchyReferences original_CategoryRow;

	// Token: 0x04008780 RID: 34688
	[SerializeField]
	protected SingleItemSelectionRow original_ItemRow;

	// Token: 0x04008781 RID: 34689
	protected SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category> categories = new SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category>(SingleItemSelectionSideScreenBase.categoryComparer);

	// Token: 0x04008782 RID: 34690
	private Dictionary<Tag, SingleItemSelectionRow> pooledRows = new Dictionary<Tag, SingleItemSelectionRow>();

	// Token: 0x04008783 RID: 34691
	private static TagNameComparer categoryComparer = new TagNameComparer(GameTags.Void);

	// Token: 0x04008784 RID: 34692
	private static SingleItemSelectionSideScreenBase.ItemComparer itemRowComparer = new SingleItemSelectionSideScreenBase.ItemComparer(GameTags.Void);

	// Token: 0x02001FD2 RID: 8146
	public class ItemComparer : IComparer<SingleItemSelectionRow>
	{
		// Token: 0x0600AC8F RID: 44175 RVA: 0x000A5E2C File Offset: 0x000A402C
		public ItemComparer()
		{
		}

		// Token: 0x0600AC90 RID: 44176 RVA: 0x0011035A File Offset: 0x0010E55A
		public ItemComparer(Tag firstTag)
		{
			this.firstTag = firstTag;
		}

		// Token: 0x0600AC91 RID: 44177 RVA: 0x0040EE74 File Offset: 0x0040D074
		public int Compare(SingleItemSelectionRow x, SingleItemSelectionRow y)
		{
			if (x == y)
			{
				return 0;
			}
			if (this.firstTag.IsValid)
			{
				if (x.tag == this.firstTag && y.tag != this.firstTag)
				{
					return 1;
				}
				if (x.tag != this.firstTag && y.tag == this.firstTag)
				{
					return -1;
				}
			}
			return x.tag.ProperNameStripLink().CompareTo(y.tag.ProperNameStripLink());
		}

		// Token: 0x04008786 RID: 34694
		private Tag firstTag;
	}

	// Token: 0x02001FD3 RID: 8147
	public class Category
	{
		// Token: 0x0600AC92 RID: 44178 RVA: 0x0040EF04 File Offset: 0x0040D104
		public virtual void ToggleUnfoldedState()
		{
			SingleItemSelectionSideScreenBase.Category.UnfoldedStates currentState = (SingleItemSelectionSideScreenBase.Category.UnfoldedStates)this.toggle.CurrentState;
			if (currentState == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded)
			{
				this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
				return;
			}
			if (currentState != SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded)
			{
				return;
			}
			this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
		}

		// Token: 0x0600AC93 RID: 44179 RVA: 0x00110369 File Offset: 0x0010E569
		public virtual void SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates new_state)
		{
			this.toggle.ChangeState((int)new_state);
			this.entries.gameObject.SetActive(new_state == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}

		// Token: 0x0600AC94 RID: 44180 RVA: 0x0011038B File Offset: 0x0010E58B
		public virtual void SetTitle(string text)
		{
			this.title.text = text;
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600AC96 RID: 44182 RVA: 0x001103A2 File Offset: 0x0010E5A2
		// (set) Token: 0x0600AC95 RID: 44181 RVA: 0x00110399 File Offset: 0x0010E599
		public Tag CategoryTag { get; protected set; }

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600AC98 RID: 44184 RVA: 0x001103B3 File Offset: 0x0010E5B3
		// (set) Token: 0x0600AC97 RID: 44183 RVA: 0x001103AA File Offset: 0x0010E5AA
		public bool IsProhibited { get; protected set; }

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600AC99 RID: 44185 RVA: 0x001103BB File Offset: 0x0010E5BB
		public bool IsVisible
		{
			get
			{
				return this.hierarchyReferences != null && this.hierarchyReferences.gameObject.activeSelf;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600AC9A RID: 44186 RVA: 0x001103DD File Offset: 0x0010E5DD
		protected RectTransform entries
		{
			get
			{
				return this.hierarchyReferences.GetReference<RectTransform>("Entries");
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600AC9B RID: 44187 RVA: 0x001103EF File Offset: 0x0010E5EF
		protected LocText title
		{
			get
			{
				return this.hierarchyReferences.GetReference<LocText>("Label");
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600AC9C RID: 44188 RVA: 0x00110401 File Offset: 0x0010E601
		protected MultiToggle toggle
		{
			get
			{
				return this.hierarchyReferences.GetReference<MultiToggle>("Toggle");
			}
		}

		// Token: 0x0600AC9D RID: 44189 RVA: 0x00110413 File Offset: 0x0010E613
		public Category(HierarchyReferences references, Tag categoryTag)
		{
			this.CategoryTag = categoryTag;
			this.hierarchyReferences = references;
			this.toggle.onClick = new System.Action(this.OnToggleClicked);
			this.SetTitle(categoryTag.ProperName());
		}

		// Token: 0x0600AC9E RID: 44190 RVA: 0x0011044D File Offset: 0x0010E64D
		public virtual void OnToggleClicked()
		{
			Action<SingleItemSelectionSideScreenBase.Category> toggleClicked = this.ToggleClicked;
			if (toggleClicked == null)
			{
				return;
			}
			toggleClicked(this);
		}

		// Token: 0x0600AC9F RID: 44191 RVA: 0x0040EF34 File Offset: 0x0040D134
		public virtual void AddItems(SingleItemSelectionRow[] _items)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(_items);
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				if (!this.items.Contains(_items[i]))
				{
					_items[i].transform.SetParent(this.entries, false);
					this.items.Add(_items[i]);
				}
			}
		}

		// Token: 0x0600ACA0 RID: 44192 RVA: 0x00110460 File Offset: 0x0010E660
		public virtual void AddItem(SingleItemSelectionRow item)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>();
			}
			item.transform.SetParent(this.entries, false);
			this.items.Add(item);
		}

		// Token: 0x0600ACA1 RID: 44193 RVA: 0x00110493 File Offset: 0x0010E693
		public virtual bool InitializeItemList(int size)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(size);
				return true;
			}
			return false;
		}

		// Token: 0x0600ACA2 RID: 44194 RVA: 0x001104AC File Offset: 0x0010E6AC
		public virtual void SetVisibilityState(bool isVisible)
		{
			this.hierarchyReferences.gameObject.SetActive(isVisible && !this.IsProhibited);
		}

		// Token: 0x0600ACA3 RID: 44195 RVA: 0x0040EF98 File Offset: 0x0040D198
		public virtual void RemoveAllItems()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				SingleItemSelectionRow obj = this.items[i];
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(obj);
				}
			}
			this.items.Clear();
			this.items = null;
		}

		// Token: 0x0600ACA4 RID: 44196 RVA: 0x0040EFEC File Offset: 0x0040D1EC
		public virtual SingleItemSelectionRow RemoveItem(Tag itemTag)
		{
			if (this.items != null)
			{
				SingleItemSelectionRow singleItemSelectionRow = this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
				if (singleItemSelectionRow != null)
				{
					Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
					if (itemRemoved != null)
					{
						itemRemoved(singleItemSelectionRow);
					}
					return singleItemSelectionRow;
				}
			}
			return null;
		}

		// Token: 0x0600ACA5 RID: 44197 RVA: 0x001104CD File Offset: 0x0010E6CD
		public virtual bool RemoveItem(SingleItemSelectionRow itemRow)
		{
			if (this.items != null && this.items.Remove(itemRow))
			{
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(itemRow);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600ACA6 RID: 44198 RVA: 0x0040F044 File Offset: 0x0040D244
		public SingleItemSelectionRow GetItem(Tag itemTag)
		{
			if (this.items == null)
			{
				return null;
			}
			return this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
		}

		// Token: 0x0600ACA7 RID: 44199 RVA: 0x0040F080 File Offset: 0x0040D280
		public int FilterItemsBySearch(string searchValue)
		{
			int num = 0;
			if (this.items != null)
			{
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(singleItemSelectionRow.tag, searchValue);
					singleItemSelectionRow.SetVisibleState(flag);
					if (flag)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600ACA8 RID: 44200 RVA: 0x0040F0F0 File Offset: 0x0040D2F0
		public void Sort()
		{
			if (this.items != null)
			{
				this.items.Sort(SingleItemSelectionSideScreenBase.itemRowComparer);
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					singleItemSelectionRow.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600ACA9 RID: 44201 RVA: 0x001104FA File Offset: 0x0010E6FA
		public void SendToLastSibiling()
		{
			this.hierarchyReferences.transform.SetAsLastSibling();
		}

		// Token: 0x0600ACAA RID: 44202 RVA: 0x0011050C File Offset: 0x0010E70C
		public void SetProihibedState(bool isPohibited)
		{
			this.IsProhibited = isPohibited;
			if (this.IsVisible && isPohibited)
			{
				this.SetVisibilityState(false);
			}
		}

		// Token: 0x04008787 RID: 34695
		public Action<SingleItemSelectionRow> ItemRemoved;

		// Token: 0x04008788 RID: 34696
		public Action<SingleItemSelectionSideScreenBase.Category> ToggleClicked;

		// Token: 0x0400878B RID: 34699
		protected HierarchyReferences hierarchyReferences;

		// Token: 0x0400878C RID: 34700
		protected List<SingleItemSelectionRow> items;

		// Token: 0x02001FD4 RID: 8148
		public enum UnfoldedStates
		{
			// Token: 0x0400878E RID: 34702
			Folded,
			// Token: 0x0400878F RID: 34703
			Unfolded
		}
	}
}
