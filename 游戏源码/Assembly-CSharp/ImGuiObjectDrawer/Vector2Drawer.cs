using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F6 RID: 8438
	public sealed class Vector2Drawer : InlineDrawer
	{
		// Token: 0x0600B39A RID: 45978 RVA: 0x0011480F File Offset: 0x00112A0F
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector2;
		}

		// Token: 0x0600B39B RID: 45979 RVA: 0x0043AAD8 File Offset: 0x00438CD8
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector2 vector = (Vector2)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1} )", vector.x, vector.y));
		}
	}
}
