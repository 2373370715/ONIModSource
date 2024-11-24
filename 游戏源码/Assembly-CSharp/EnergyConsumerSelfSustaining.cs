using System;
using System.Diagnostics;
using KSerialization;

// Token: 0x02001292 RID: 4754
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060061C1 RID: 25025 RVA: 0x002B4084 File Offset: 0x002B2284
	// (remove) Token: 0x060061C2 RID: 25026 RVA: 0x002B40BC File Offset: 0x002B22BC
	public event System.Action OnConnectionChanged;

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x060061C3 RID: 25027 RVA: 0x000DFD00 File Offset: 0x000DDF00
	public override bool IsPowered
	{
		get
		{
			return this.isSustained || this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x060061C4 RID: 25028 RVA: 0x000DFD15 File Offset: 0x000DDF15
	public bool IsExternallyPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x060061C5 RID: 25029 RVA: 0x000DFD20 File Offset: 0x000DDF20
	public void SetSustained(bool isSustained)
	{
		this.isSustained = isSustained;
	}

	// Token: 0x060061C6 RID: 25030 RVA: 0x002B40F4 File Offset: 0x002B22F4
	public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		CircuitManager.ConnectionStatus connectionStatus = this.connectionStatus;
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.connectionStatus = CircuitManager.ConnectionStatus.NotConnected;
			break;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.connectionStatus == CircuitManager.ConnectionStatus.Powered && base.GetComponent<Battery>() == null)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Unpowered;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (this.connectionStatus != CircuitManager.ConnectionStatus.Powered)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Powered;
			}
			break;
		}
		this.UpdatePoweredStatus();
		if (connectionStatus != this.connectionStatus && this.OnConnectionChanged != null)
		{
			this.OnConnectionChanged();
		}
	}

	// Token: 0x060061C7 RID: 25031 RVA: 0x000DFD29 File Offset: 0x000DDF29
	public void UpdatePoweredStatus()
	{
		this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
	}

	// Token: 0x04004591 RID: 17809
	private bool isSustained;

	// Token: 0x04004592 RID: 17810
	private CircuitManager.ConnectionStatus connectionStatus;
}
