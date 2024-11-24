using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000B89 RID: 2953
[DebuggerDisplay("{IdHash}")]
public class ChoreType : Resource
{
	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06003863 RID: 14435 RVA: 0x000C483C File Offset: 0x000C2A3C
	// (set) Token: 0x06003864 RID: 14436 RVA: 0x000C4844 File Offset: 0x000C2A44
	public Urge urge { get; private set; }

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06003865 RID: 14437 RVA: 0x000C484D File Offset: 0x000C2A4D
	// (set) Token: 0x06003866 RID: 14438 RVA: 0x000C4855 File Offset: 0x000C2A55
	public ChoreGroup[] groups { get; private set; }

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06003867 RID: 14439 RVA: 0x000C485E File Offset: 0x000C2A5E
	// (set) Token: 0x06003868 RID: 14440 RVA: 0x000C4866 File Offset: 0x000C2A66
	public int priority { get; private set; }

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06003869 RID: 14441 RVA: 0x000C486F File Offset: 0x000C2A6F
	// (set) Token: 0x0600386A RID: 14442 RVA: 0x000C4877 File Offset: 0x000C2A77
	public int interruptPriority { get; set; }

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x0600386B RID: 14443 RVA: 0x000C4880 File Offset: 0x000C2A80
	// (set) Token: 0x0600386C RID: 14444 RVA: 0x000C4888 File Offset: 0x000C2A88
	public int explicitPriority { get; private set; }

	// Token: 0x0600386D RID: 14445 RVA: 0x000C4891 File Offset: 0x000C2A91
	private string ResolveStringCallback(string str, object data)
	{
		return ((Chore)data).ResolveString(str);
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x0021AF14 File Offset: 0x00219114
	public ChoreType(string id, ResourceSet parent, string[] chore_groups, string urge, string name, string status_message, string tooltip, IEnumerable<Tag> interrupt_exclusion, int implicit_priority, int explicit_priority) : base(id, parent, name)
	{
		this.statusItem = new StatusItem(id, status_message, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveStringCallback);
		this.tags.Add(TagManager.Create(id));
		this.interruptExclusion = new HashSet<Tag>(interrupt_exclusion);
		Db.Get().DuplicantStatusItems.Add(this.statusItem);
		List<ChoreGroup> list = new List<ChoreGroup>();
		for (int i = 0; i < chore_groups.Length; i++)
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.TryGet(chore_groups[i]);
			if (choreGroup != null)
			{
				if (!choreGroup.choreTypes.Contains(this))
				{
					choreGroup.choreTypes.Add(this);
				}
				list.Add(choreGroup);
			}
		}
		this.groups = list.ToArray();
		if (!string.IsNullOrEmpty(urge))
		{
			this.urge = Db.Get().Urges.Get(urge);
		}
		this.priority = implicit_priority;
		this.explicitPriority = explicit_priority;
	}

	// Token: 0x0400266D RID: 9837
	public StatusItem statusItem;

	// Token: 0x04002672 RID: 9842
	public HashSet<Tag> tags = new HashSet<Tag>();

	// Token: 0x04002673 RID: 9843
	public HashSet<Tag> interruptExclusion;

	// Token: 0x04002675 RID: 9845
	public string reportName;
}
