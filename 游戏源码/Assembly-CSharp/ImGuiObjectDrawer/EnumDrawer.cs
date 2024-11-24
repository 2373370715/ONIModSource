using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F3 RID: 8435
	public sealed class EnumDrawer : InlineDrawer
	{
		// Token: 0x0600B391 RID: 45969 RVA: 0x001147E2 File Offset: 0x001129E2
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsEnum;
		}

		// Token: 0x0600B392 RID: 45970 RVA: 0x0011478D File Offset: 0x0011298D
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
