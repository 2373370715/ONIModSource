using System;

namespace Database
{
	// Token: 0x0200216E RID: 8558
	public class DbStickerBomb : PermitResource
	{
		// Token: 0x0600B607 RID: 46599 RVA: 0x001154B6 File Offset: 0x001136B6
		[Obsolete("Please use constructor with dlcIds parameter")]
		public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker) : this(id, name, desc, rarity, animfilename, sticker, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x0600B608 RID: 46600 RVA: 0x001154CC File Offset: 0x001136CC
		public DbStickerBomb(string id, string name, string desc, PermitRarity rarity, string animfilename, string sticker, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.id = id;
			this.sticker = sticker;
			this.animFile = Assets.GetAnim(animfilename);
		}

		// Token: 0x0600B609 RID: 46601 RVA: 0x00456868 File Offset: 0x00454A68
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			return new PermitPresentationInfo
			{
				sprite = Def.GetUISpriteFromMultiObjectAnim(this.animFile, string.Format("{0}_{1}", "idle_sticker", this.sticker), false, string.Format("{0}_{1}", "sticker", this.sticker))
			};
		}

		// Token: 0x04009481 RID: 38017
		public string id;

		// Token: 0x04009482 RID: 38018
		public string sticker;

		// Token: 0x04009483 RID: 38019
		public KAnimFile animFile;

		// Token: 0x04009484 RID: 38020
		private const string stickerAnimPrefix = "idle_sticker";

		// Token: 0x04009485 RID: 38021
		private const string stickerSymbolPrefix = "sticker";
	}
}
