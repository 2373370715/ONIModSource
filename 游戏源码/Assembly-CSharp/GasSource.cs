using System;
using KSerialization;

// Token: 0x02000A63 RID: 2659
[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
	// Token: 0x060030FA RID: 12538 RVA: 0x000BFD41 File Offset: 0x000BDF41
	protected override CellOffset[] GetOffsetGroup()
	{
		return OffsetGroups.LiquidSource;
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x000BFD48 File Offset: 0x000BDF48
	protected override IChunkManager GetChunkManager()
	{
		return GasSourceManager.Instance;
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000BFD4F File Offset: 0x000BDF4F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}
}
