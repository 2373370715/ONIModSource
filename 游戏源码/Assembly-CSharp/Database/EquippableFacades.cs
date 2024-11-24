using System;

namespace Database
{
	// Token: 0x02002139 RID: 8505
	public class EquippableFacades : ResourceSet<EquippableFacadeResource>
	{
		// Token: 0x0600B545 RID: 46405 RVA: 0x0044E704 File Offset: 0x0044C904
		public EquippableFacades(ResourceSet parent) : base("EquippableFacades", parent)
		{
			base.Initialize();
			foreach (EquippableFacadeInfo equippableFacadeInfo in Blueprints.Get().all.equippableFacades)
			{
				this.Add(equippableFacadeInfo.id, equippableFacadeInfo.name, equippableFacadeInfo.desc, equippableFacadeInfo.rarity, equippableFacadeInfo.defID, equippableFacadeInfo.buildOverride, equippableFacadeInfo.animFile, equippableFacadeInfo.dlcIds);
			}
		}

		// Token: 0x0600B546 RID: 46406 RVA: 0x0044E7A4 File Offset: 0x0044C9A4
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
		{
			this.Add(id, name, desc, rarity, defID, buildOverride, animFile, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600B547 RID: 46407 RVA: 0x0044E7C8 File Offset: 0x0044C9C8
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] dlcIds)
		{
			EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, dlcIds);
			this.resources.Add(item);
		}
	}
}
