using System;

// Token: 0x02000794 RID: 1940
public class GameplayEventMinionFilter
{
	// Token: 0x04001716 RID: 5910
	public string id;

	// Token: 0x04001717 RID: 5911
	public GameplayEventMinionFilter.FilterFn filter;

	// Token: 0x02000795 RID: 1941
	// (Invoke) Token: 0x060022F5 RID: 8949
	public delegate bool FilterFn(MinionIdentity minion);
}
