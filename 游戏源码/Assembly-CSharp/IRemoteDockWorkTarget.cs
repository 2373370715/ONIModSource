using System;

// Token: 0x02001749 RID: 5961
public interface IRemoteDockWorkTarget
{
	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06007AC2 RID: 31426
	Chore RemoteDockChore { get; }

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x06007AC3 RID: 31427
	IApproachable Approachable { get; }
}
