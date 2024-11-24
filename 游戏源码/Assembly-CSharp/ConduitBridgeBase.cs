using System;

// Token: 0x02000CFD RID: 3325
public class ConduitBridgeBase : KMonoBehaviour
{
	// Token: 0x060040E5 RID: 16613 RVA: 0x000CA09A File Offset: 0x000C829A
	protected void SendEmptyOnMassTransfer()
	{
		if (this.OnMassTransfer != null)
		{
			this.OnMassTransfer(SimHashes.Void, 0f, 0f, 0, 0, null);
		}
	}

	// Token: 0x04002C55 RID: 11349
	public ConduitBridgeBase.DesiredMassTransfer desiredMassTransfer;

	// Token: 0x04002C56 RID: 11350
	public ConduitBridgeBase.ConduitBridgeEvent OnMassTransfer;

	// Token: 0x02000CFE RID: 3326
	// (Invoke) Token: 0x060040E8 RID: 16616
	public delegate float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);

	// Token: 0x02000CFF RID: 3327
	// (Invoke) Token: 0x060040EC RID: 16620
	public delegate void ConduitBridgeEvent(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);
}
