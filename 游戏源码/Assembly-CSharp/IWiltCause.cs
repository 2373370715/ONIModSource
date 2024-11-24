using System;

// Token: 0x020011F9 RID: 4601
public interface IWiltCause
{
	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06005DBB RID: 23995
	string WiltStateString { get; }

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06005DBC RID: 23996
	WiltCondition.Condition[] Conditions { get; }
}
