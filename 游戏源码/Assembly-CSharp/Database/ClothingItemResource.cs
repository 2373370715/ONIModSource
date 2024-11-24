using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200212A RID: 8490
	public class ClothingItemResource : PermitResource
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600B4BB RID: 46267 RVA: 0x00114D8F File Offset: 0x00112F8F
		// (set) Token: 0x0600B4BC RID: 46268 RVA: 0x00114D97 File Offset: 0x00112F97
		public string animFilename { get; private set; }

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600B4BD RID: 46269 RVA: 0x00114DA0 File Offset: 0x00112FA0
		// (set) Token: 0x0600B4BE RID: 46270 RVA: 0x00114DA8 File Offset: 0x00112FA8
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x0600B4BF RID: 46271 RVA: 0x00114DB1 File Offset: 0x00112FB1
		// (set) Token: 0x0600B4C0 RID: 46272 RVA: 0x00114DB9 File Offset: 0x00112FB9
		public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

		// Token: 0x0600B4C1 RID: 46273 RVA: 0x004486C4 File Offset: 0x004468C4
		[Obsolete("Please use constructor with dlcIds parameter")]
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile) : this(id, name, desc, outfitType, category, rarity, animFile, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x0600B4C2 RID: 46274 RVA: 0x00114DC2 File Offset: 0x00112FC2
		public ClothingItemResource(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] dlcIds) : base(id, name, desc, category, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.outfitType = outfitType;
		}

		// Token: 0x0600B4C3 RID: 46275 RVA: 0x004486E8 File Offset: 0x004468E8
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			if (this.AnimFile == null)
			{
				Debug.LogError("Clothing kanim is missing from bundle: " + this.animFilename);
			}
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.CLOTHING_ITEM_FACADE_FOR);
			return result;
		}
	}
}
