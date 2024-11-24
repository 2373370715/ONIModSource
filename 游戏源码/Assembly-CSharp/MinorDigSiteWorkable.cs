using System;

// Token: 0x0200042B RID: 1067
public class MinorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06001234 RID: 4660 RVA: 0x000ADD17 File Offset: 0x000ABF17
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x000AE237 File Offset: 0x000AC437
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MinorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x00187B44 File Offset: 0x00185D44
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000C6E RID: 3182
	private MinorFossilDigSite.Instance digsite;
}
