using System;
using KSerialization;

// Token: 0x02000EF8 RID: 3832
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
	// Token: 0x06004D5A RID: 19802 RVA: 0x000D23BC File Offset: 0x000D05BC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.manuallyControlled = false;
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x000D23CB File Offset: 0x000D05CB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x00264E6C File Offset: 0x0026306C
	private void OnOperationalChanged(object data)
	{
		bool state = (bool)data;
		this.SetState(state);
	}

	// Token: 0x040035C3 RID: 13763
	private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>(delegate(OperationalControlledSwitch component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
