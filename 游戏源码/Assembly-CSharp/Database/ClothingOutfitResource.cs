using System;
using UnityEngine;

namespace Database
{
	// Token: 0x0200212D RID: 8493
	public class ClothingOutfitResource : Resource, IBlueprintDlcInfo
	{
		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x0600B4C7 RID: 46279 RVA: 0x00114E08 File Offset: 0x00113008
		// (set) Token: 0x0600B4C8 RID: 46280 RVA: 0x00114E10 File Offset: 0x00113010
		public string[] itemsInOutfit { get; private set; }

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x0600B4C9 RID: 46281 RVA: 0x00114E19 File Offset: 0x00113019
		// (set) Token: 0x0600B4CA RID: 46282 RVA: 0x00114E21 File Offset: 0x00113021
		public string[] dlcIds { get; set; } = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x0600B4CB RID: 46283 RVA: 0x00114E2A File Offset: 0x0011302A
		public ClothingOutfitResource(string id, string[] items_in_outfit, string name, ClothingOutfitUtility.OutfitType outfitType) : base(id, name)
		{
			this.itemsInOutfit = items_in_outfit;
			this.outfitType = outfitType;
		}

		// Token: 0x0600B4CC RID: 46284 RVA: 0x00114E4E File Offset: 0x0011304E
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x0600B4CD RID: 46285 RVA: 0x00114E79 File Offset: 0x00113079
		public string GetDlcIdFrom()
		{
			if (this.dlcIds == DlcManager.AVAILABLE_ALL_VERSIONS || this.dlcIds == DlcManager.AVAILABLE_VANILLA_ONLY)
			{
				return null;
			}
			if (this.dlcIds.Length == 0)
			{
				return null;
			}
			return this.dlcIds[0];
		}

		// Token: 0x040090F8 RID: 37112
		public ClothingOutfitUtility.OutfitType outfitType;
	}
}
