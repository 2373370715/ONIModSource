using System;

namespace Database
{
	// Token: 0x0200216F RID: 8559
	public class StickerBombs : ResourceSet<DbStickerBomb>
	{
		// Token: 0x0600B60A RID: 46602 RVA: 0x004568BC File Offset: 0x00454ABC
		public StickerBombs(ResourceSet parent) : base("StickerBombs", parent)
		{
			foreach (StickerBombFacadeInfo stickerBombFacadeInfo in Blueprints.Get().all.stickerBombFacades)
			{
				this.Add(stickerBombFacadeInfo.id, stickerBombFacadeInfo.name, stickerBombFacadeInfo.desc, stickerBombFacadeInfo.rarity, stickerBombFacadeInfo.animFile, stickerBombFacadeInfo.sticker, stickerBombFacadeInfo.dlcIds);
			}
		}

		// Token: 0x0600B60B RID: 46603 RVA: 0x001154FE File Offset: 0x001136FE
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName)
		{
			return this.Add(id, name, desc, rarity, animfilename, symbolName, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600B60C RID: 46604 RVA: 0x00456950 File Offset: 0x00454B50
		private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName, string[] dlcIds)
		{
			DbStickerBomb dbStickerBomb = new DbStickerBomb(id, name, desc, rarity, animfilename, symbolName, dlcIds);
			this.resources.Add(dbStickerBomb);
			return dbStickerBomb;
		}

		// Token: 0x0600B60D RID: 46605 RVA: 0x00115514 File Offset: 0x00113714
		public DbStickerBomb GetRandomSticker()
		{
			return this.resources.GetRandom<DbStickerBomb>();
		}
	}
}
