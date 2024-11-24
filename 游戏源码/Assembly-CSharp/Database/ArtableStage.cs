using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200210F RID: 8463
	public class ArtableStage : PermitResource
	{
		// Token: 0x0600B3E5 RID: 46053 RVA: 0x0043C1DC File Offset: 0x0043A3DC
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x0600B3E6 RID: 46054 RVA: 0x0043C238 File Offset: 0x0043A438
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.animFile), "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(this.prefabId).GetProperName()).Replace("{ArtableQuality}", this.statusItem.GetName(null)));
			return result;
		}

		// Token: 0x04008E1E RID: 36382
		public string id;

		// Token: 0x04008E1F RID: 36383
		public string anim;

		// Token: 0x04008E20 RID: 36384
		public string animFile;

		// Token: 0x04008E21 RID: 36385
		public string prefabId;

		// Token: 0x04008E22 RID: 36386
		public string symbolName;

		// Token: 0x04008E23 RID: 36387
		public int decor;

		// Token: 0x04008E24 RID: 36388
		public bool cheerOnComplete;

		// Token: 0x04008E25 RID: 36389
		public ArtableStatusItem statusItem;
	}
}
