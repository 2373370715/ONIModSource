using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x0200211D RID: 8477
	public class BuildingFacades : ResourceSet<BuildingFacadeResource>
	{
		// Token: 0x0600B40E RID: 46094 RVA: 0x0043DB80 File Offset: 0x0043BD80
		public BuildingFacades(ResourceSet parent) : base("BuildingFacades", parent)
		{
			base.Initialize();
			foreach (BuildingFacadeInfo buildingFacadeInfo in Blueprints.Get().all.buildingFacades)
			{
				this.Add(buildingFacadeInfo.id, buildingFacadeInfo.name, buildingFacadeInfo.desc, buildingFacadeInfo.rarity, buildingFacadeInfo.prefabId, buildingFacadeInfo.animFile, buildingFacadeInfo.dlcIds, buildingFacadeInfo.workables);
			}
		}

		// Token: 0x0600B40F RID: 46095 RVA: 0x0043DC28 File Offset: 0x0043BE28
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, LocString Name, LocString Desc, PermitRarity rarity, string prefabId, string animFile, Dictionary<string, string> workables = null)
		{
			this.Add(id, Name, Desc, rarity, prefabId, animFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables);
		}

		// Token: 0x0600B410 RID: 46096 RVA: 0x0043DC4C File Offset: 0x0043BE4C
		public void Add(string id, LocString Name, LocString Desc, PermitRarity rarity, string prefabId, string animFile, string[] dlcIds, Dictionary<string, string> workables = null)
		{
			BuildingFacadeResource item = new BuildingFacadeResource(id, Name, Desc, rarity, prefabId, animFile, dlcIds, workables);
			this.resources.Add(item);
		}

		// Token: 0x0600B411 RID: 46097 RVA: 0x0043DC84 File Offset: 0x0043BE84
		public void PostProcess()
		{
			foreach (BuildingFacadeResource buildingFacadeResource in this.resources)
			{
				buildingFacadeResource.Init();
			}
		}
	}
}
