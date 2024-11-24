using System;

namespace Database;

public class EquippableFacades : ResourceSet<EquippableFacadeResource>
{
	public EquippableFacades(ResourceSet parent)
		: base("EquippableFacades", parent)
	{
		Initialize();
		foreach (EquippableFacadeInfo equippableFacade in Blueprints.Get().all.equippableFacades)
		{
			Add(equippableFacade.id, equippableFacade.name, equippableFacade.desc, equippableFacade.rarity, equippableFacade.defID, equippableFacade.buildOverride, equippableFacade.animFile, equippableFacade.dlcIds);
		}
	}

	[Obsolete("Please use Add(...) with dlcIds parameter")]
	public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
	{
		Add(id, name, desc, rarity, defID, buildOverride, animFile, DlcManager.AVAILABLE_ALL_VERSIONS);
	}

	public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] dlcIds)
	{
		EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, dlcIds);
		resources.Add(item);
	}
}
