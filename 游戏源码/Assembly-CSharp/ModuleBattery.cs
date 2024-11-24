public class ModuleBattery : Battery
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}
}
