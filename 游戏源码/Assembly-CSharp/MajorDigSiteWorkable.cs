using System;

// Token: 0x02000403 RID: 1027
public class MajorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x0600115B RID: 4443 RVA: 0x000ADD17 File Offset: 0x000ABF17
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x000ADD2A File Offset: 0x000ABF2A
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MajorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x00183898 File Offset: 0x00181A98
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000BD4 RID: 3028
	private MajorFossilDigSite.Instance digsite;
}
