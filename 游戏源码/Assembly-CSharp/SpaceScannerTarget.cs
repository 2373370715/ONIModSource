public readonly struct SpaceScannerTarget
{
	public readonly string id;

	private SpaceScannerTarget(string id)
	{
		this.id = id;
	}

	public static SpaceScannerTarget MeteorShower()
	{
		return new SpaceScannerTarget("meteor_shower");
	}

	public static SpaceScannerTarget BallisticObject()
	{
		return new SpaceScannerTarget("ballistic_object");
	}

	public static SpaceScannerTarget RocketBaseGame(LaunchConditionManager rocket)
	{
		return new SpaceScannerTarget($"rocket_base_game::{rocket.GetComponent<KPrefabID>().InstanceID}");
	}

	public static SpaceScannerTarget RocketDlc1(Clustercraft rocket)
	{
		return new SpaceScannerTarget($"rocket_dlc1::{rocket.GetComponent<KPrefabID>().InstanceID}");
	}
}
