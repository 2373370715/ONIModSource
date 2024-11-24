using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002110 RID: 8464
	public class ArtableStages : ResourceSet<ArtableStage>
	{
		// Token: 0x0600B3E7 RID: 46055 RVA: 0x0043C2B8 File Offset: 0x0043A4B8
		public ArtableStage Add(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname, string[] dlcIds)
		{
			ArtableStatusItem status_item = Db.Get().ArtableStatuses.Get(status_id);
			ArtableStage artableStage = new ArtableStage(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_item, prefabId, symbolname, dlcIds);
			this.resources.Add(artableStage);
			return artableStage;
		}

		// Token: 0x0600B3E8 RID: 46056 RVA: 0x0043C300 File Offset: 0x0043A500
		public ArtableStages(ResourceSet parent) : base("ArtableStages", parent)
		{
			foreach (ArtableInfo artableInfo in Blueprints.Get().all.artables)
			{
				this.Add(artableInfo.id, artableInfo.name, artableInfo.desc, artableInfo.rarity, artableInfo.animFile, artableInfo.anim, artableInfo.decor_value, artableInfo.cheer_on_complete, artableInfo.status_id, artableInfo.prefabId, artableInfo.symbolname, artableInfo.dlcIds);
			}
		}

		// Token: 0x0600B3E9 RID: 46057 RVA: 0x0043C3B0 File Offset: 0x0043A5B0
		public List<ArtableStage> GetPrefabStages(Tag prefab_id)
		{
			return this.resources.FindAll((ArtableStage stage) => stage.prefabId == prefab_id);
		}

		// Token: 0x0600B3EA RID: 46058 RVA: 0x00114A7F File Offset: 0x00112C7F
		public ArtableStage DefaultPrefabStage(Tag prefab_id)
		{
			return this.GetPrefabStages(prefab_id).Find((ArtableStage stage) => stage.statusItem == Db.Get().ArtableStatuses.AwaitingArting);
		}
	}
}
