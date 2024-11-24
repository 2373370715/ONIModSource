using System;

namespace Database
{
	// Token: 0x0200214F RID: 8527
	public abstract class PermitResource : Resource
	{
		// Token: 0x0600B5A5 RID: 46501 RVA: 0x00452718 File Offset: 0x00450918
		public PermitResource(string id, string Name, string Desc, PermitCategory permitCategory, PermitRarity rarity, string[] DLCIds) : base(id, Name)
		{
			DebugUtil.DevAssert(Name != null, "Name must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			DebugUtil.DevAssert(Desc != null, "Description must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			this.Description = Desc;
			this.Category = permitCategory;
			this.Rarity = rarity;
			this.DlcIds = DLCIds;
		}

		// Token: 0x0600B5A6 RID: 46502
		public abstract PermitPresentationInfo GetPermitPresentationInfo();

		// Token: 0x0600B5A7 RID: 46503 RVA: 0x0011523E File Offset: 0x0011343E
		public bool IsOwnableOnServer()
		{
			return this.Rarity != PermitRarity.Universal && this.Rarity != PermitRarity.UniversalLocked;
		}

		// Token: 0x0600B5A8 RID: 46504 RVA: 0x00115257 File Offset: 0x00113457
		public bool IsUnlocked()
		{
			return this.Rarity == PermitRarity.Universal || PermitItems.IsPermitUnlocked(this);
		}

		// Token: 0x0600B5A9 RID: 46505 RVA: 0x0011526A File Offset: 0x0011346A
		public string GetDlcIdFrom()
		{
			if (this.DlcIds == DlcManager.AVAILABLE_ALL_VERSIONS || this.DlcIds == DlcManager.AVAILABLE_VANILLA_ONLY)
			{
				return null;
			}
			if (this.DlcIds.Length == 0)
			{
				return null;
			}
			return this.DlcIds[0];
		}

		// Token: 0x0400939B RID: 37787
		public string Description;

		// Token: 0x0400939C RID: 37788
		public PermitCategory Category;

		// Token: 0x0400939D RID: 37789
		public PermitRarity Rarity;

		// Token: 0x0400939E RID: 37790
		public string[] DlcIds;
	}
}
