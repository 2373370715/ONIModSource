using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002142 RID: 8514
	public class MonumentParts : ResourceSet<MonumentPartResource>
	{
		// Token: 0x0600B584 RID: 46468 RVA: 0x00451C5C File Offset: 0x0044FE5C
		public MonumentParts(ResourceSet parent) : base("MonumentParts", parent)
		{
			base.Initialize();
			foreach (MonumentPartInfo monumentPartInfo in Blueprints.Get().all.monumentParts)
			{
				this.Add(monumentPartInfo.id, monumentPartInfo.name, monumentPartInfo.desc, monumentPartInfo.rarity, monumentPartInfo.animFile, monumentPartInfo.state, monumentPartInfo.symbolName, monumentPartInfo.part, monumentPartInfo.dlcIds);
			}
		}

		// Token: 0x0600B585 RID: 46469 RVA: 0x00451D00 File Offset: 0x0044FF00
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds)
		{
			MonumentPartResource item = new MonumentPartResource(id, name, desc, rarity, animFilename, state, symbolName, part, dlcIds);
			this.resources.Add(item);
		}

		// Token: 0x0600B586 RID: 46470 RVA: 0x00451D30 File Offset: 0x0044FF30
		public List<MonumentPartResource> GetParts(MonumentPartResource.Part part)
		{
			return this.resources.FindAll((MonumentPartResource mpr) => mpr.part == part);
		}
	}
}
