using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001B4C RID: 6988
public class TagFilterScreen : SideScreenContent
{
	// Token: 0x060092CB RID: 37579 RVA: 0x000FFD9B File Offset: 0x000FDF9B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x060092CC RID: 37580 RVA: 0x0038A494 File Offset: 0x00388694
	public override void SetTarget(GameObject target)
	{
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
		if (!this.targetFilterable.showUserMenu)
		{
			return;
		}
		this.Filter(this.targetFilterable.AcceptedTags);
		base.Activate();
	}

	// Token: 0x060092CD RID: 37581 RVA: 0x0038A500 File Offset: 0x00388700
	protected override void OnActivate()
	{
		this.rootItem = this.BuildDisplay(this.rootTag);
		this.treeControl.SetUserItemRoot(this.rootItem);
		this.treeControl.root.opened = true;
		this.Filter(this.treeControl.root, this.acceptedTags, false);
	}

	// Token: 0x060092CE RID: 37582 RVA: 0x0038A55C File Offset: 0x0038875C
	public static List<Tag> GetAllTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (TagFilterScreen.TagEntry tagEntry in TagFilterScreen.defaultRootTag.children)
		{
			if (tagEntry.tag.IsValid)
			{
				list.Add(tagEntry.tag);
			}
		}
		return list;
	}

	// Token: 0x060092CF RID: 37583 RVA: 0x0038A5A8 File Offset: 0x003887A8
	private KTreeControl.UserItem BuildDisplay(TagFilterScreen.TagEntry root)
	{
		KTreeControl.UserItem userItem = null;
		if (root.name != null && root.name != "")
		{
			userItem = new KTreeControl.UserItem
			{
				text = root.name,
				userData = root.tag
			};
			List<KTreeControl.UserItem> list = new List<KTreeControl.UserItem>();
			if (root.children != null)
			{
				foreach (TagFilterScreen.TagEntry root2 in root.children)
				{
					list.Add(this.BuildDisplay(root2));
				}
			}
			userItem.children = list;
		}
		return userItem;
	}

	// Token: 0x060092D0 RID: 37584 RVA: 0x0038A634 File Offset: 0x00388834
	private static KTreeControl.UserItem CreateTree(string tree_name, Tag tree_tag, IList<Element> items)
	{
		KTreeControl.UserItem userItem = new KTreeControl.UserItem
		{
			text = tree_name,
			userData = tree_tag,
			children = new List<KTreeControl.UserItem>()
		};
		foreach (Element element in items)
		{
			KTreeControl.UserItem item = new KTreeControl.UserItem
			{
				text = element.name,
				userData = GameTagExtensions.Create(element.id)
			};
			userItem.children.Add(item);
		}
		return userItem;
	}

	// Token: 0x060092D1 RID: 37585 RVA: 0x000FFDA9 File Offset: 0x000FDFA9
	public void SetRootTag(TagFilterScreen.TagEntry root_tag)
	{
		this.rootTag = root_tag;
	}

	// Token: 0x060092D2 RID: 37586 RVA: 0x000FFDB2 File Offset: 0x000FDFB2
	public void Filter(HashSet<Tag> acceptedTags)
	{
		this.acceptedTags = acceptedTags;
	}

	// Token: 0x060092D3 RID: 37587 RVA: 0x0038A6D0 File Offset: 0x003888D0
	private void Filter(KTreeItem root, HashSet<Tag> acceptedTags, bool parentEnabled)
	{
		root.checkboxChecked = (parentEnabled || (root.userData != null && acceptedTags.Contains((Tag)root.userData)));
		foreach (KTreeItem root2 in root.children)
		{
			this.Filter(root2, acceptedTags, root.checkboxChecked);
		}
		if (!root.checkboxChecked && root.children.Count > 0)
		{
			bool checkboxChecked = true;
			using (IEnumerator<KTreeItem> enumerator = root.children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.checkboxChecked)
					{
						checkboxChecked = false;
						break;
					}
				}
			}
			root.checkboxChecked = checkboxChecked;
		}
	}

	// Token: 0x04006F0F RID: 28431
	[SerializeField]
	private KTreeControl treeControl;

	// Token: 0x04006F10 RID: 28432
	private KTreeControl.UserItem rootItem;

	// Token: 0x04006F11 RID: 28433
	private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;

	// Token: 0x04006F12 RID: 28434
	private HashSet<Tag> acceptedTags = new HashSet<Tag>();

	// Token: 0x04006F13 RID: 28435
	private TreeFilterable targetFilterable;

	// Token: 0x04006F14 RID: 28436
	public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry
	{
		name = "All",
		tag = default(Tag),
		children = new TagFilterScreen.TagEntry[0]
	};

	// Token: 0x02001B4D RID: 6989
	public class TagEntry
	{
		// Token: 0x04006F15 RID: 28437
		public string name;

		// Token: 0x04006F16 RID: 28438
		public Tag tag;

		// Token: 0x04006F17 RID: 28439
		public TagFilterScreen.TagEntry[] children;
	}
}
