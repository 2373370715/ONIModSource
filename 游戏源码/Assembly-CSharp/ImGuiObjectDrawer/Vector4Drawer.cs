using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F8 RID: 8440
	public sealed class Vector4Drawer : InlineDrawer
	{
		// Token: 0x0600B3A0 RID: 45984 RVA: 0x0011482F File Offset: 0x00112A2F
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector4;
		}

		// Token: 0x0600B3A1 RID: 45985 RVA: 0x0043AB6C File Offset: 0x00438D6C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector4 vector = (Vector4)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2}, {3} )", new object[]
			{
				vector.x,
				vector.y,
				vector.z,
				vector.w
			}));
		}
	}
}
