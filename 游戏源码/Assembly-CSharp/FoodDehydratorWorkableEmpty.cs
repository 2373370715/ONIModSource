using System;

// Token: 0x02000D83 RID: 3459
public class FoodDehydratorWorkableEmpty : Workable
{
	// Token: 0x060043D4 RID: 17364 RVA: 0x000CBDC3 File Offset: 0x000C9FC3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.workAnims = FoodDehydratorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = FoodDehydratorWorkableEmpty.WORK_ANIMS_PST;
		this.workingPstFailed = FoodDehydratorWorkableEmpty.WORK_ANIMS_FAIL_PST;
	}

	// Token: 0x04002E87 RID: 11911
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"empty_pre",
		"empty_loop"
	};

	// Token: 0x04002E88 RID: 11912
	private static readonly HashedString[] WORK_ANIMS_PST = new HashedString[]
	{
		"empty_pst"
	};

	// Token: 0x04002E89 RID: 11913
	private static readonly HashedString[] WORK_ANIMS_FAIL_PST = new HashedString[]
	{
		""
	};
}
