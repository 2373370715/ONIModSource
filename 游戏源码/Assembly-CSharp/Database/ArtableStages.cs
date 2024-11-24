using System.Collections.Generic;

namespace Database;

public class ArtableStages : ResourceSet<ArtableStage>
{
	public ArtableStage Add(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname, string[] dlcIds)
	{
		ArtableStatusItem status_item = Db.Get().ArtableStatuses.Get(status_id);
		ArtableStage artableStage = new ArtableStage(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_item, prefabId, symbolname, dlcIds);
		resources.Add(artableStage);
		return artableStage;
	}

	public ArtableStages(ResourceSet parent)
		: base("ArtableStages", parent)
	{
		foreach (ArtableInfo artable in Blueprints.Get().all.artables)
		{
			Add(artable.id, artable.name, artable.desc, artable.rarity, artable.animFile, artable.anim, artable.decor_value, artable.cheer_on_complete, artable.status_id, artable.prefabId, artable.symbolname, artable.dlcIds);
		}
	}

	public List<ArtableStage> GetPrefabStages(Tag prefab_id)
	{
		return resources.FindAll((ArtableStage stage) => stage.prefabId == prefab_id);
	}

	public ArtableStage DefaultPrefabStage(Tag prefab_id)
	{
		return GetPrefabStages(prefab_id).Find((ArtableStage stage) => stage.statusItem == Db.Get().ArtableStatuses.AwaitingArting);
	}
}
