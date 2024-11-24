using System;

// Token: 0x02000EDB RID: 3803
public class ModuleBattery : Battery
{
	// Token: 0x06004CA8 RID: 19624 RVA: 0x000D1B33 File Offset: 0x000CFD33
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x06004CA9 RID: 19625 RVA: 0x0026317C File Offset: 0x0026137C
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}
}
