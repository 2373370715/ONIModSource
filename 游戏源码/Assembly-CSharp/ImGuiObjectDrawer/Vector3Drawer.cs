using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F7 RID: 8439
	public sealed class Vector3Drawer : InlineDrawer
	{
		// Token: 0x0600B39D RID: 45981 RVA: 0x0011481F File Offset: 0x00112A1F
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector3;
		}

		// Token: 0x0600B39E RID: 45982 RVA: 0x0043AB1C File Offset: 0x00438D1C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector3 vector = (Vector3)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2} )", vector.x, vector.y, vector.z));
		}
	}
}
