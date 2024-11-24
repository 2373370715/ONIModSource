using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02002144 RID: 8516
	public class MonumentPartResource : PermitResource
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600B589 RID: 46473 RVA: 0x00115109 File Offset: 0x00113309
		// (set) Token: 0x0600B58A RID: 46474 RVA: 0x00115111 File Offset: 0x00113311
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600B58B RID: 46475 RVA: 0x0011511A File Offset: 0x0011331A
		// (set) Token: 0x0600B58C RID: 46476 RVA: 0x00115122 File Offset: 0x00113322
		public string SymbolName { get; private set; }

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600B58D RID: 46477 RVA: 0x0011512B File Offset: 0x0011332B
		// (set) Token: 0x0600B58E RID: 46478 RVA: 0x00115133 File Offset: 0x00113333
		public string State { get; private set; }

		// Token: 0x0600B58F RID: 46479 RVA: 0x0011513C File Offset: 0x0011333C
		public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

		// Token: 0x0600B590 RID: 46480 RVA: 0x00114E4E File Offset: 0x0011304E
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x0600B591 RID: 46481 RVA: 0x00451D64 File Offset: 0x0044FF64
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			result.SetFacadeForText("_monument part");
			return result;
		}

		// Token: 0x04009357 RID: 37719
		public MonumentPartResource.Part part;

		// Token: 0x02002145 RID: 8517
		public enum Part
		{
			// Token: 0x04009359 RID: 37721
			Bottom,
			// Token: 0x0400935A RID: 37722
			Middle,
			// Token: 0x0400935B RID: 37723
			Top
		}
	}
}
