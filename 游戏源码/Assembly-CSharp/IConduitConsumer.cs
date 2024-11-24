using System;

// Token: 0x020010D2 RID: 4306
public interface IConduitConsumer
{
	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06005867 RID: 22631
	Storage Storage { get; }

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06005868 RID: 22632
	ConduitType ConduitType { get; }
}
