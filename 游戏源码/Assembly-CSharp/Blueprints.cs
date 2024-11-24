using System;

// Token: 0x0200097B RID: 2427
public class Blueprints
{
	// Token: 0x06002BE6 RID: 11238 RVA: 0x001E0578 File Offset: 0x001DE778
	public static Blueprints Get()
	{
		if (Blueprints.instance == null)
		{
			Blueprints.instance = new Blueprints();
			Blueprints.instance.all.AddBlueprintsFrom<Blueprints_Default>(new Blueprints_Default());
			foreach (BlueprintProvider provider in Blueprints.instance.skinsReleaseProviders)
			{
				Blueprints.instance.skinsRelease.AddBlueprintsFrom<BlueprintProvider>(provider);
			}
			Blueprints.instance.all.AddBlueprintsFrom(Blueprints.instance.skinsRelease);
			Blueprints.instance.skinsRelease.PostProcess();
			Blueprints.instance.all.PostProcess();
		}
		return Blueprints.instance;
	}

	// Token: 0x04001D98 RID: 7576
	public BlueprintCollection all = new BlueprintCollection();

	// Token: 0x04001D99 RID: 7577
	public BlueprintCollection skinsRelease = new BlueprintCollection();

	// Token: 0x04001D9A RID: 7578
	public BlueprintProvider[] skinsReleaseProviders = new BlueprintProvider[]
	{
		new Blueprints_U51AndBefore(),
		new Blueprints_DlcPack2()
	};

	// Token: 0x04001D9B RID: 7579
	private static Blueprints instance;
}
