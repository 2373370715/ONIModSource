using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F0 RID: 8432
	public class SimpleDrawer : InlineDrawer
	{
		// Token: 0x0600B387 RID: 45959 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x0600B388 RID: 45960 RVA: 0x00114776 File Offset: 0x00112976
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsPrimitive || member.CanAssignToType<string>();
		}

		// Token: 0x0600B389 RID: 45961 RVA: 0x0011478D File Offset: 0x0011298D
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
