using System;

// Token: 0x02001719 RID: 5913
public class Quest : Resource
{
	// Token: 0x060079DD RID: 31197 RVA: 0x00316C20 File Offset: 0x00314E20
	public Quest(string id, QuestCriteria[] criteria) : base(id, id)
	{
		Debug.Assert(criteria.Length != 0);
		this.Criteria = criteria;
		string str = "STRINGS.CODEX.QUESTS." + id.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(str + ".NAME", out stringEntry))
		{
			this.Title = stringEntry.String;
		}
		if (Strings.TryGet(str + ".COMPLETE", out stringEntry))
		{
			this.CompletionText = stringEntry.String;
		}
		for (int i = 0; i < this.Criteria.Length; i++)
		{
			this.Criteria[i].PopulateStrings("STRINGS.CODEX.QUESTS.");
		}
	}

	// Token: 0x04005B71 RID: 23409
	public const string STRINGS_PREFIX = "STRINGS.CODEX.QUESTS.";

	// Token: 0x04005B72 RID: 23410
	public readonly QuestCriteria[] Criteria;

	// Token: 0x04005B73 RID: 23411
	public readonly string Title;

	// Token: 0x04005B74 RID: 23412
	public readonly string CompletionText;

	// Token: 0x0200171A RID: 5914
	public struct ItemData
	{
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060079DE RID: 31198 RVA: 0x000F01CF File Offset: 0x000EE3CF
		// (set) Token: 0x060079DF RID: 31199 RVA: 0x000F01D9 File Offset: 0x000EE3D9
		public int ValueHandle
		{
			get
			{
				return this.valueHandle - 1;
			}
			set
			{
				this.valueHandle = value + 1;
			}
		}

		// Token: 0x04005B75 RID: 23413
		public int LocalCellId;

		// Token: 0x04005B76 RID: 23414
		public float CurrentValue;

		// Token: 0x04005B77 RID: 23415
		public Tag SatisfyingItem;

		// Token: 0x04005B78 RID: 23416
		public Tag QualifyingTag;

		// Token: 0x04005B79 RID: 23417
		public HashedString CriteriaId;

		// Token: 0x04005B7A RID: 23418
		private int valueHandle;
	}

	// Token: 0x0200171B RID: 5915
	public enum State
	{
		// Token: 0x04005B7C RID: 23420
		NotStarted,
		// Token: 0x04005B7D RID: 23421
		InProgress,
		// Token: 0x04005B7E RID: 23422
		Completed
	}
}
