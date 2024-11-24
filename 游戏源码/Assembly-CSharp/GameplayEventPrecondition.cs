using System;

// Token: 0x0200079E RID: 1950
public class GameplayEventPrecondition
{
	// Token: 0x04001724 RID: 5924
	public string description;

	// Token: 0x04001725 RID: 5925
	public GameplayEventPrecondition.PreconditionFn condition;

	// Token: 0x04001726 RID: 5926
	public bool required;

	// Token: 0x04001727 RID: 5927
	public int priorityModifier;

	// Token: 0x0200079F RID: 1951
	// (Invoke) Token: 0x06002312 RID: 8978
	public delegate bool PreconditionFn();
}
