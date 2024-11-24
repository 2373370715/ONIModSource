using System;

// Token: 0x02000B85 RID: 2949
public class Accessory : Resource
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06003847 RID: 14407 RVA: 0x000C4741 File Offset: 0x000C2941
	// (set) Token: 0x06003848 RID: 14408 RVA: 0x000C4749 File Offset: 0x000C2949
	public KAnim.Build.Symbol symbol { get; private set; }

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06003849 RID: 14409 RVA: 0x000C4752 File Offset: 0x000C2952
	// (set) Token: 0x0600384A RID: 14410 RVA: 0x000C475A File Offset: 0x000C295A
	public HashedString batchSource { get; private set; }

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x0600384B RID: 14411 RVA: 0x000C4763 File Offset: 0x000C2963
	// (set) Token: 0x0600384C RID: 14412 RVA: 0x000C476B File Offset: 0x000C296B
	public AccessorySlot slot { get; private set; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x0600384D RID: 14413 RVA: 0x000C4774 File Offset: 0x000C2974
	// (set) Token: 0x0600384E RID: 14414 RVA: 0x000C477C File Offset: 0x000C297C
	public KAnimFile animFile { get; private set; }

	// Token: 0x0600384F RID: 14415 RVA: 0x000C4785 File Offset: 0x000C2985
	public Accessory(string id, ResourceSet parent, AccessorySlot slot, HashedString batchSource, KAnim.Build.Symbol symbol, KAnimFile animFile = null, KAnimFile defaultAnimFile = null) : base(id, parent, null)
	{
		this.slot = slot;
		this.symbol = symbol;
		this.batchSource = batchSource;
		this.animFile = animFile;
	}

	// Token: 0x06003850 RID: 14416 RVA: 0x000C47AF File Offset: 0x000C29AF
	public bool IsDefault()
	{
		return this.animFile == this.slot.defaultAnimFile;
	}
}
