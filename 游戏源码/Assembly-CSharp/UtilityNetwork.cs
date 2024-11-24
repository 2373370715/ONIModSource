using System;

// Token: 0x02001A0F RID: 6671
public class UtilityNetwork
{
	// Token: 0x06008AF5 RID: 35573 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void AddItem(object item)
	{
	}

	// Token: 0x06008AF6 RID: 35574 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void RemoveItem(object item)
	{
	}

	// Token: 0x06008AF7 RID: 35575 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void ConnectItem(object item)
	{
	}

	// Token: 0x06008AF8 RID: 35576 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void DisconnectItem(object item)
	{
	}

	// Token: 0x06008AF9 RID: 35577 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Reset(UtilityNetworkGridNode[] grid)
	{
	}

	// Token: 0x0400689E RID: 26782
	public int id;

	// Token: 0x0400689F RID: 26783
	public ConduitType conduitType;
}
