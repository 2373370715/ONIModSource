using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F1 RID: 8433
	public sealed class FallbackDrawer : SimpleDrawer
	{
		// Token: 0x0600B38B RID: 45963 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x0600B38C RID: 45964 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}
	}
}
