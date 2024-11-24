using System;

namespace Database;

public class DbStickerBomb : PermitResource
{
	public string id;

	public string sticker;

	public KAnimFile animFile;

	private const string stickerAnimPrefix = "idle_sticker";

	private const string stickerSymbolPrefix = "sticker";

	[Obsolete("Please use constructor with dlcIds parameter")]
	public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker)
		: this(id, name, desc, rarity, animfilename, sticker, DlcManager.AVAILABLE_ALL_VERSIONS)
	{
	}

	public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker, string[] dlcIds)
		: base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
	{
		this.id = id;
		this.sticker = sticker;
		animFile = Assets.GetAnim(animfilename);
	}

	public override PermitPresentationInfo GetPermitPresentationInfo()
	{
		PermitPresentationInfo result = default(PermitPresentationInfo);
		result.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, string.Format("{0}_{1}", "idle_sticker", sticker), centered: false, string.Format("{0}_{1}", "sticker", sticker));
		return result;
	}
}
