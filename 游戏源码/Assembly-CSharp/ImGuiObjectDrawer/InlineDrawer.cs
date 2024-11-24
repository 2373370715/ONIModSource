using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020EE RID: 8430
	public abstract class InlineDrawer : MemberDrawer
	{
		// Token: 0x0600B380 RID: 45952 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public sealed override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Inline;
		}

		// Token: 0x0600B381 RID: 45953 RVA: 0x0011473F File Offset: 0x0011293F
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			this.DrawInline(context, member);
		}
	}
}
