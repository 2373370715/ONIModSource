using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020EF RID: 8431
	public class NullDrawer : InlineDrawer
	{
		// Token: 0x0600B383 RID: 45955 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x0600B384 RID: 45956 RVA: 0x00114751 File Offset: 0x00112951
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value == null;
		}

		// Token: 0x0600B385 RID: 45957 RVA: 0x0011475C File Offset: 0x0011295C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, "null");
		}
	}
}
