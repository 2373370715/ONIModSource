using System;

// Token: 0x0200104A RID: 4170
public interface IDisconnectable
{
	// Token: 0x0600550F RID: 21775
	bool Connect();

	// Token: 0x06005510 RID: 21776
	void Disconnect();

	// Token: 0x06005511 RID: 21777
	bool IsDisconnected();
}
