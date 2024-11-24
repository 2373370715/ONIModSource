using System;

// Token: 0x02000A84 RID: 2692
public class LongRangeSculpture : Sculpture
{
	// Token: 0x060031C2 RID: 12738 RVA: 0x000C0546 File Offset: 0x000BE746
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}
}
