using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F5 RID: 8437
	public sealed class KAnimHashedStringDrawer : InlineDrawer
	{
		// Token: 0x0600B397 RID: 45975 RVA: 0x001147FF File Offset: 0x001129FF
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is KAnimHashedString;
		}

		// Token: 0x0600B398 RID: 45976 RVA: 0x0043AA74 File Offset: 0x00438C74
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			KAnimHashedString kanimHashedString = (KAnimHashedString)member.value;
			string str = kanimHashedString.ToString();
			string str2 = "0x" + kanimHashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
