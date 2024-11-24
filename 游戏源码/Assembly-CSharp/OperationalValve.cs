using System;
using KSerialization;

// Token: 0x02000EFA RID: 3834
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
	// Token: 0x06004D62 RID: 19810 RVA: 0x000D241D File Offset: 0x000D061D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x000D2436 File Offset: 0x000D0636
	protected override void OnSpawn()
	{
		this.OnOperationalChanged(this.operational.IsOperational);
		base.OnSpawn();
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x000D2454 File Offset: 0x000D0654
	protected override void OnCleanUp()
	{
		base.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x00264E88 File Offset: 0x00263088
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		if (flag)
		{
			base.CurrentFlow = base.MaxFlow;
		}
		else
		{
			base.CurrentFlow = 0f;
		}
		this.operational.SetActive(flag, false);
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x000D246D File Offset: 0x000D066D
	protected override void OnMassTransfer(float amount)
	{
		this.isDispensing = (amount > 0f);
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x00264EC8 File Offset: 0x002630C8
	public override void UpdateAnim()
	{
		if (!this.operational.IsOperational)
		{
			this.controller.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.isDispensing)
		{
			this.controller.Queue("on_flow", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.controller.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x040035C5 RID: 13765
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040035C6 RID: 13766
	private bool isDispensing;

	// Token: 0x040035C7 RID: 13767
	private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>(delegate(OperationalValve component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
