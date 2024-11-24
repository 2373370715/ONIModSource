using System;
using System.Collections.Generic;

// Token: 0x0200078A RID: 1930
public class VoidChoreProvider : ChoreProvider
{
	// Token: 0x060022B4 RID: 8884 RVA: 0x000B690C File Offset: 0x000B4B0C
	public static void DestroyInstance()
	{
		VoidChoreProvider.Instance = null;
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000B6914 File Offset: 0x000B4B14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		VoidChoreProvider.Instance = this;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void AddChore(Chore chore)
	{
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void RemoveChore(Chore chore)
	{
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
	}

	// Token: 0x040016E0 RID: 5856
	public static VoidChoreProvider Instance;
}
