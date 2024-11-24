namespace FoodRehydrator;

public class ResourceRequirementMonitor : KMonoBehaviour
{
	[MyCmpReq]
	private Operational operational;

	private Storage packages;

	private Storage water;

	private static readonly Operational.Flag flag = new Operational.Flag("HasSufficientResources", Operational.Flag.Type.Requirement);

	private static readonly EventSystem.IntraObjectHandler<ResourceRequirementMonitor> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ResourceRequirementMonitor>(delegate(ResourceRequirementMonitor component, object data)
	{
		component.OnStorageChanged(data);
	});

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = GetComponents<Storage>();
		DebugUtil.DevAssert(components.Length == 2, "Incorrect number of storages on foodrehydrator");
		packages = components[0];
		water = components[1];
		Subscribe(-1697596308, OnStorageChangedDelegate);
	}

	protected float GetAvailableWater()
	{
		return water.GetMassAvailable(GameTags.Water);
	}

	protected bool HasSufficientResources()
	{
		if (packages.items.Count > 0)
		{
			return GetAvailableWater() > 1f;
		}
		return false;
	}

	protected void OnStorageChanged(object _)
	{
		operational.SetFlag(flag, HasSufficientResources());
	}
}
