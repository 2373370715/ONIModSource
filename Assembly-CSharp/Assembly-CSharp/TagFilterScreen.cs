﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class TagFilterScreen : SideScreenContent
{
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TreeFilterable>() != null;
	}

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

	protected override void OnActivate()
	{
		this.rootItem = this.BuildDisplay(this.rootTag);
		this.treeControl.SetUserItemRoot(this.rootItem);
		this.treeControl.root.opened = true;
		this.Filter(this.treeControl.root, this.acceptedTags, false);
	}

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

	public void SetRootTag(TagFilterScreen.TagEntry root_tag)
	{
		this.rootTag = root_tag;
	}

	public void Filter(HashSet<Tag> acceptedTags)
	{
		this.acceptedTags = acceptedTags;
	}

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

	[SerializeField]
	private KTreeControl treeControl;

	private KTreeControl.UserItem rootItem;

	private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;

	private HashSet<Tag> acceptedTags = new HashSet<Tag>();

	private TreeFilterable targetFilterable;

	public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry
	{
		name = "All",
		tag = default(Tag),
		children = new TagFilterScreen.TagEntry[0]
	};

	public class TagEntry
	{
		public string name;

		public Tag tag;

		public TagFilterScreen.TagEntry[] children;
	}
}
