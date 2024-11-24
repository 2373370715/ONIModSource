using System;
using Database;

// Token: 0x02001A92 RID: 6802
public class EntityModifierSet : ModifierSet
{
	// Token: 0x06008E3A RID: 36410 RVA: 0x000FCE10 File Offset: 0x000FB010
	public override void Initialize()
	{
		base.Initialize();
		this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
		this.ChoreGroups = new ChoreGroups(this.Root);
		base.LoadTraits();
	}

	// Token: 0x04006B1F RID: 27423
	public DuplicantStatusItems DuplicantStatusItems;

	// Token: 0x04006B20 RID: 27424
	public ChoreGroups ChoreGroups;
}
