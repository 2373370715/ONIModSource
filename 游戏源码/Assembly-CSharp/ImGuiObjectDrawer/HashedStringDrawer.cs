using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F4 RID: 8436
	public sealed class HashedStringDrawer : InlineDrawer
	{
		// Token: 0x0600B394 RID: 45972 RVA: 0x001147EF File Offset: 0x001129EF
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is HashedString;
		}

		// Token: 0x0600B395 RID: 45973 RVA: 0x0043AA10 File Offset: 0x00438C10
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			HashedString hashedString = (HashedString)member.value;
			string str = hashedString.ToString();
			string str2 = "0x" + hashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
