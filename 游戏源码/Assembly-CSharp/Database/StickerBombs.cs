using System;

namespace Database;

public class StickerBombs : ResourceSet<DbStickerBomb>
{
	public StickerBombs(ResourceSet parent)
		: base("StickerBombs", parent)
	{
		foreach (StickerBombFacadeInfo stickerBombFacade in Blueprints.Get().all.stickerBombFacades)
		{
			Add(stickerBombFacade.id, stickerBombFacade.name, stickerBombFacade.desc, stickerBombFacade.rarity, stickerBombFacade.animFile, stickerBombFacade.sticker, stickerBombFacade.dlcIds);
		}
	}

	[Obsolete("Please use Add(...) with dlcIds parameter")]
	private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName)
	{
		return Add(id, name, desc, rarity, animfilename, symbolName, DlcManager.AVAILABLE_ALL_VERSIONS);
	}

	private DbStickerBomb Add(string id, string name, string desc, PermitRarity rarity, string animfilename, string symbolName, string[] dlcIds)
	{
		DbStickerBomb dbStickerBomb = new DbStickerBomb(id, name, desc, rarity, animfilename, symbolName, dlcIds);
		resources.Add(dbStickerBomb);
		return dbStickerBomb;
	}

	public DbStickerBomb GetRandomSticker()
	{
		return resources.GetRandom();
	}
}
