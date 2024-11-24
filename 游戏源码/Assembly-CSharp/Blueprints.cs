public class Blueprints
{
	public BlueprintCollection all = new BlueprintCollection();

	public BlueprintCollection skinsRelease = new BlueprintCollection();

	public BlueprintProvider[] skinsReleaseProviders = new BlueprintProvider[2]
	{
		new Blueprints_U51AndBefore(),
		new Blueprints_DlcPack2()
	};

	private static Blueprints instance;

	public static Blueprints Get()
	{
		if (instance == null)
		{
			instance = new Blueprints();
			instance.all.AddBlueprintsFrom(new Blueprints_Default());
			BlueprintProvider[] array = instance.skinsReleaseProviders;
			foreach (BlueprintProvider provider in array)
			{
				instance.skinsRelease.AddBlueprintsFrom(provider);
			}
			instance.all.AddBlueprintsFrom(instance.skinsRelease);
			instance.skinsRelease.PostProcess();
			instance.all.PostProcess();
		}
		return instance;
	}
}
