using System;

// Token: 0x020010DE RID: 4318
public interface IConduitFlow
{
	// Token: 0x060058A8 RID: 22696
	void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default);

	// Token: 0x060058A9 RID: 22697
	void RemoveConduitUpdater(Action<float> callback);

	// Token: 0x060058AA RID: 22698
	bool IsConduitEmpty(int cell);
}
