using System;

// Token: 0x020014AB RID: 5291
public interface ILogicEventSender : ILogicNetworkConnection
{
	// Token: 0x06006E21 RID: 28193
	void LogicTick();

	// Token: 0x06006E22 RID: 28194
	int GetLogicCell();

	// Token: 0x06006E23 RID: 28195
	int GetLogicValue();
}
