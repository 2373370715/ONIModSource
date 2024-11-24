using System;
using UnityEngine;

namespace Database
{
	// Token: 0x0200213A RID: 8506
	public class EquippableFacadeResource : PermitResource
	{
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x0600B548 RID: 46408 RVA: 0x00114FF9 File Offset: 0x001131F9
		// (set) Token: 0x0600B549 RID: 46409 RVA: 0x00115001 File Offset: 0x00113201
		public string BuildOverride { get; private set; }

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600B54A RID: 46410 RVA: 0x0011500A File Offset: 0x0011320A
		// (set) Token: 0x0600B54B RID: 46411 RVA: 0x00115012 File Offset: 0x00113212
		public string DefID { get; private set; }

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600B54C RID: 46412 RVA: 0x0011501B File Offset: 0x0011321B
		// (set) Token: 0x0600B54D RID: 46413 RVA: 0x00115023 File Offset: 0x00113223
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x0600B54E RID: 46414 RVA: 0x0044E7F4 File Offset: 0x0044C9F4
		[Obsolete("Please use constructor with dlcIds parameter")]
		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile) : this(id, name, desc, rarity, buildOverride, defID, animFile, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x0600B54F RID: 46415 RVA: 0x0011502C File Offset: 0x0011322C
		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile, string[] dlcIds) : base(id, name, desc, PermitCategory.Equipment, rarity, dlcIds)
		{
			this.DefID = defID;
			this.BuildOverride = buildOverride;
			this.AnimFile = Assets.GetAnim(animFile);
		}

		// Token: 0x0600B550 RID: 46416 RVA: 0x0044E818 File Offset: 0x0044CA18
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			if (this.AnimFile == null)
			{
				global::Debug.LogError("Facade AnimFile is null: " + this.DefID);
			}
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
		}

		// Token: 0x0600B551 RID: 46417 RVA: 0x0044E878 File Offset: 0x0044CA78
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			GameObject gameObject = Assets.TryGetPrefab(this.DefID);
			if (gameObject == null || !gameObject)
			{
				result.SetFacadeForPrefabID(this.DefID);
			}
			else
			{
				result.SetFacadeForPrefabName(gameObject.GetProperName());
			}
			return result;
		}
	}
}
