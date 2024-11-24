using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001C2B RID: 7211
public class CodexEntry
{
	// Token: 0x06009621 RID: 38433 RVA: 0x003A00F8 File Offset: 0x0039E2F8
	public CodexEntry()
	{
	}

	// Token: 0x06009622 RID: 38434 RVA: 0x003A0150 File Offset: 0x0039E350
	public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
	{
		this.category = category;
		this.name = name;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x06009623 RID: 38435 RVA: 0x003A01D4 File Offset: 0x0039E3D4
	public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
	{
		this.category = category;
		this.title = titleKey;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(this.title);
		}
	}

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x06009624 RID: 38436 RVA: 0x00101C56 File Offset: 0x000FFE56
	// (set) Token: 0x06009625 RID: 38437 RVA: 0x00101C5E File Offset: 0x000FFE5E
	public List<ContentContainer> contentContainers
	{
		get
		{
			return this._contentContainers;
		}
		private set
		{
			this._contentContainers = value;
		}
	}

	// Token: 0x06009626 RID: 38438 RVA: 0x003A0260 File Offset: 0x0039E460
	public static List<string> ContentContainerDebug(List<ContentContainer> _contentContainers)
	{
		List<string> list = new List<string>();
		foreach (ContentContainer contentContainer in _contentContainers)
		{
			if (contentContainer != null)
			{
				string text = string.Concat(new string[]
				{
					"<b>",
					contentContainer.contentLayout.ToString(),
					" container: ",
					((contentContainer.content == null) ? 0 : contentContainer.content.Count).ToString(),
					" items</b>"
				});
				if (contentContainer.content != null)
				{
					text += "\n";
					for (int i = 0; i < contentContainer.content.Count; i++)
					{
						text = string.Concat(new string[]
						{
							text,
							"    • ",
							contentContainer.content[i].ToString(),
							": ",
							CodexEntry.GetContentWidgetDebugString(contentContainer.content[i]),
							"\n"
						});
					}
				}
				list.Add(text);
			}
			else
			{
				list.Add("null container");
			}
		}
		return list;
	}

	// Token: 0x06009627 RID: 38439 RVA: 0x003A03B8 File Offset: 0x0039E5B8
	private static string GetContentWidgetDebugString(ICodexWidget widget)
	{
		CodexText codexText = widget as CodexText;
		if (codexText != null)
		{
			return codexText.text;
		}
		CodexLabelWithIcon codexLabelWithIcon = widget as CodexLabelWithIcon;
		if (codexLabelWithIcon != null)
		{
			return codexLabelWithIcon.label.text + " / " + codexLabelWithIcon.icon.spriteName;
		}
		CodexImage codexImage = widget as CodexImage;
		if (codexImage != null)
		{
			return codexImage.spriteName;
		}
		CodexVideo codexVideo = widget as CodexVideo;
		if (codexVideo != null)
		{
			return codexVideo.name;
		}
		CodexIndentedLabelWithIcon codexIndentedLabelWithIcon = widget as CodexIndentedLabelWithIcon;
		if (codexIndentedLabelWithIcon != null)
		{
			return codexIndentedLabelWithIcon.label.text + " / " + codexIndentedLabelWithIcon.icon.spriteName;
		}
		return "";
	}

	// Token: 0x06009628 RID: 38440 RVA: 0x00101C67 File Offset: 0x000FFE67
	public void CreateContentContainerCollection()
	{
		this.contentContainers = new List<ContentContainer>();
	}

	// Token: 0x06009629 RID: 38441 RVA: 0x00101C74 File Offset: 0x000FFE74
	public void InsertContentContainer(int index, ContentContainer container)
	{
		this.contentContainers.Insert(index, container);
	}

	// Token: 0x0600962A RID: 38442 RVA: 0x00101C83 File Offset: 0x000FFE83
	public void RemoveContentContainerAt(int index)
	{
		this.contentContainers.RemoveAt(index);
	}

	// Token: 0x0600962B RID: 38443 RVA: 0x00101C91 File Offset: 0x000FFE91
	public void AddContentContainer(ContentContainer container)
	{
		this.contentContainers.Add(container);
	}

	// Token: 0x0600962C RID: 38444 RVA: 0x00101C9F File Offset: 0x000FFE9F
	public void AddContentContainerRange(IEnumerable<ContentContainer> containers)
	{
		this.contentContainers.AddRange(containers);
	}

	// Token: 0x0600962D RID: 38445 RVA: 0x00101CAD File Offset: 0x000FFEAD
	public void RemoveContentContainer(ContentContainer container)
	{
		this.contentContainers.Remove(container);
	}

	// Token: 0x0600962E RID: 38446 RVA: 0x003A0458 File Offset: 0x0039E658
	public ICodexWidget GetFirstWidget()
	{
		for (int i = 0; i < this.contentContainers.Count; i++)
		{
			if (this.contentContainers[i].content != null)
			{
				for (int j = 0; j < this.contentContainers[i].content.Count; j++)
				{
					if (this.contentContainers[i].content[j] != null)
					{
						return this.contentContainers[i].content[j];
					}
				}
			}
		}
		return null;
	}

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x0600962F RID: 38447 RVA: 0x00101CBC File Offset: 0x000FFEBC
	// (set) Token: 0x06009630 RID: 38448 RVA: 0x00101CC4 File Offset: 0x000FFEC4
	public string[] dlcIds
	{
		get
		{
			return this._dlcIds;
		}
		set
		{
			this._dlcIds = value;
		}
	}

	// Token: 0x06009631 RID: 38449 RVA: 0x00101CBC File Offset: 0x000FFEBC
	public string[] GetDlcIds()
	{
		return this._dlcIds;
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06009632 RID: 38450 RVA: 0x00101CCD File Offset: 0x000FFECD
	// (set) Token: 0x06009633 RID: 38451 RVA: 0x003A04E4 File Offset: 0x0039E6E4
	public string[] forbiddenDLCIds
	{
		get
		{
			return this._forbiddenDLCIds;
		}
		set
		{
			this._forbiddenDLCIds = value;
			string str = "";
			for (int i = 0; i < value.Length; i++)
			{
				str += value[i];
				if (i != value.Length - 1)
				{
					str += "\n";
				}
			}
		}
	}

	// Token: 0x06009634 RID: 38452 RVA: 0x00101CD5 File Offset: 0x000FFED5
	public string[] GetForbiddenDLCs()
	{
		if (this._forbiddenDLCIds == null)
		{
			this._forbiddenDLCIds = this.NONE;
		}
		return this._forbiddenDLCIds;
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06009635 RID: 38453 RVA: 0x00101CF1 File Offset: 0x000FFEF1
	// (set) Token: 0x06009636 RID: 38454 RVA: 0x00101CF9 File Offset: 0x000FFEF9
	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x06009637 RID: 38455 RVA: 0x00101D02 File Offset: 0x000FFF02
	// (set) Token: 0x06009638 RID: 38456 RVA: 0x00101D0A File Offset: 0x000FFF0A
	public string parentId
	{
		get
		{
			return this._parentId;
		}
		set
		{
			this._parentId = value;
		}
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06009639 RID: 38457 RVA: 0x00101D13 File Offset: 0x000FFF13
	// (set) Token: 0x0600963A RID: 38458 RVA: 0x00101D1B File Offset: 0x000FFF1B
	public string category
	{
		get
		{
			return this._category;
		}
		set
		{
			this._category = value;
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x0600963B RID: 38459 RVA: 0x00101D24 File Offset: 0x000FFF24
	// (set) Token: 0x0600963C RID: 38460 RVA: 0x00101D2C File Offset: 0x000FFF2C
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			this._title = value;
		}
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x0600963D RID: 38461 RVA: 0x00101D35 File Offset: 0x000FFF35
	// (set) Token: 0x0600963E RID: 38462 RVA: 0x00101D3D File Offset: 0x000FFF3D
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			this._name = value;
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x0600963F RID: 38463 RVA: 0x00101D46 File Offset: 0x000FFF46
	// (set) Token: 0x06009640 RID: 38464 RVA: 0x00101D4E File Offset: 0x000FFF4E
	public string subtitle
	{
		get
		{
			return this._subtitle;
		}
		set
		{
			this._subtitle = value;
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x06009641 RID: 38465 RVA: 0x00101D57 File Offset: 0x000FFF57
	// (set) Token: 0x06009642 RID: 38466 RVA: 0x00101D5F File Offset: 0x000FFF5F
	public List<SubEntry> subEntries
	{
		get
		{
			return this._subEntries;
		}
		set
		{
			this._subEntries = value;
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x06009643 RID: 38467 RVA: 0x00101D68 File Offset: 0x000FFF68
	// (set) Token: 0x06009644 RID: 38468 RVA: 0x00101D70 File Offset: 0x000FFF70
	public List<CodexEntry_MadeAndUsed> contentMadeAndUsed
	{
		get
		{
			return this._contentMadeAndUsed;
		}
		set
		{
			this._contentMadeAndUsed = value;
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x06009645 RID: 38469 RVA: 0x00101D79 File Offset: 0x000FFF79
	// (set) Token: 0x06009646 RID: 38470 RVA: 0x00101D81 File Offset: 0x000FFF81
	public Sprite icon
	{
		get
		{
			return this._icon;
		}
		set
		{
			this._icon = value;
		}
	}

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x06009647 RID: 38471 RVA: 0x00101D8A File Offset: 0x000FFF8A
	// (set) Token: 0x06009648 RID: 38472 RVA: 0x00101D92 File Offset: 0x000FFF92
	public Color iconColor
	{
		get
		{
			return this._iconColor;
		}
		set
		{
			this._iconColor = value;
		}
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x06009649 RID: 38473 RVA: 0x00101D9B File Offset: 0x000FFF9B
	// (set) Token: 0x0600964A RID: 38474 RVA: 0x00101DA3 File Offset: 0x000FFFA3
	public string iconPrefabID
	{
		get
		{
			return this._iconPrefabID;
		}
		set
		{
			this._iconPrefabID = value;
		}
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x0600964B RID: 38475 RVA: 0x00101DAC File Offset: 0x000FFFAC
	// (set) Token: 0x0600964C RID: 38476 RVA: 0x00101DB4 File Offset: 0x000FFFB4
	public string iconLockID
	{
		get
		{
			return this._iconLockID;
		}
		set
		{
			this._iconLockID = value;
		}
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x0600964D RID: 38477 RVA: 0x00101DBD File Offset: 0x000FFFBD
	// (set) Token: 0x0600964E RID: 38478 RVA: 0x00101DC5 File Offset: 0x000FFFC5
	public string iconAssetName
	{
		get
		{
			return this._iconAssetName;
		}
		set
		{
			this._iconAssetName = value;
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x0600964F RID: 38479 RVA: 0x00101DCE File Offset: 0x000FFFCE
	// (set) Token: 0x06009650 RID: 38480 RVA: 0x00101DD6 File Offset: 0x000FFFD6
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			this._disabled = value;
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x06009651 RID: 38481 RVA: 0x00101DDF File Offset: 0x000FFFDF
	// (set) Token: 0x06009652 RID: 38482 RVA: 0x00101DE7 File Offset: 0x000FFFE7
	public bool searchOnly
	{
		get
		{
			return this._searchOnly;
		}
		set
		{
			this._searchOnly = value;
		}
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x06009653 RID: 38483 RVA: 0x00101DF0 File Offset: 0x000FFFF0
	// (set) Token: 0x06009654 RID: 38484 RVA: 0x00101DF8 File Offset: 0x000FFFF8
	public int customContentLength
	{
		get
		{
			return this._customContentLength;
		}
		set
		{
			this._customContentLength = value;
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x06009655 RID: 38485 RVA: 0x00101E01 File Offset: 0x00100001
	// (set) Token: 0x06009656 RID: 38486 RVA: 0x00101E09 File Offset: 0x00100009
	public string sortString
	{
		get
		{
			return this._sortString;
		}
		set
		{
			this._sortString = value;
		}
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x06009657 RID: 38487 RVA: 0x00101E12 File Offset: 0x00100012
	// (set) Token: 0x06009658 RID: 38488 RVA: 0x00101E1A File Offset: 0x0010001A
	public bool showBeforeGeneratedCategoryLinks
	{
		get
		{
			return this._showBeforeGeneratedCategoryLinks;
		}
		set
		{
			this._showBeforeGeneratedCategoryLinks = value;
		}
	}

	// Token: 0x04007494 RID: 29844
	public EntryDevLog log = new EntryDevLog();

	// Token: 0x04007495 RID: 29845
	private List<ContentContainer> _contentContainers = new List<ContentContainer>();

	// Token: 0x04007496 RID: 29846
	private string[] _dlcIds;

	// Token: 0x04007497 RID: 29847
	private string[] _forbiddenDLCIds;

	// Token: 0x04007498 RID: 29848
	private string[] NONE = new string[0];

	// Token: 0x04007499 RID: 29849
	private string _id;

	// Token: 0x0400749A RID: 29850
	private string _parentId;

	// Token: 0x0400749B RID: 29851
	private string _category;

	// Token: 0x0400749C RID: 29852
	private string _title;

	// Token: 0x0400749D RID: 29853
	private string _name;

	// Token: 0x0400749E RID: 29854
	private string _subtitle;

	// Token: 0x0400749F RID: 29855
	private List<SubEntry> _subEntries = new List<SubEntry>();

	// Token: 0x040074A0 RID: 29856
	private List<CodexEntry_MadeAndUsed> _contentMadeAndUsed = new List<CodexEntry_MadeAndUsed>();

	// Token: 0x040074A1 RID: 29857
	private Sprite _icon;

	// Token: 0x040074A2 RID: 29858
	private Color _iconColor = Color.white;

	// Token: 0x040074A3 RID: 29859
	private string _iconPrefabID;

	// Token: 0x040074A4 RID: 29860
	private string _iconLockID;

	// Token: 0x040074A5 RID: 29861
	private string _iconAssetName;

	// Token: 0x040074A6 RID: 29862
	private bool _disabled;

	// Token: 0x040074A7 RID: 29863
	private bool _searchOnly;

	// Token: 0x040074A8 RID: 29864
	private int _customContentLength;

	// Token: 0x040074A9 RID: 29865
	private string _sortString;

	// Token: 0x040074AA RID: 29866
	private bool _showBeforeGeneratedCategoryLinks;
}
