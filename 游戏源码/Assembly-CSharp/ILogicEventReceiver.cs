using System;

// Token: 0x020014AC RID: 5292
public interface ILogicEventReceiver : ILogicNetworkConnection
{
	// Token: 0x06006E24 RID: 28196
	void ReceiveLogicEvent(int value);

	// Token: 0x06006E25 RID: 28197
	int GetLogicCell();
}
