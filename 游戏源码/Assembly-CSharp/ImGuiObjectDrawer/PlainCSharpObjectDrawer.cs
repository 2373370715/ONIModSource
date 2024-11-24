using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002105 RID: 8453
	public class PlainCSharpObjectDrawer : MemberDrawer
	{
		// Token: 0x0600B3CC RID: 46028 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x0600B3CD RID: 46029 RVA: 0x000A65EC File Offset: 0x000A47EC
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Custom;
		}

		// Token: 0x0600B3CE RID: 46030 RVA: 0x00114A22 File Offset: 0x00112C22
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600B3CF RID: 46031 RVA: 0x0043AE58 File Offset: 0x00439058
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}

		// Token: 0x0600B3D0 RID: 46032 RVA: 0x00114A29 File Offset: 0x00112C29
		protected virtual void DrawContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			DrawerUtil.DrawObjectContents(member.value, context, depth + 1);
		}
	}
}
