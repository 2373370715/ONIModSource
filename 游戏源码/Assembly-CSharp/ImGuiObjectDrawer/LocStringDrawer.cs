using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F2 RID: 8434
	public sealed class LocStringDrawer : InlineDrawer
	{
		// Token: 0x0600B38E RID: 45966 RVA: 0x001147AD File Offset: 0x001129AD
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<LocString>();
		}

		// Token: 0x0600B38F RID: 45967 RVA: 0x001147B5 File Offset: 0x001129B5
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, string.Format("{0}({1})", member.value, ((LocString)member.value).text));
		}
	}
}
