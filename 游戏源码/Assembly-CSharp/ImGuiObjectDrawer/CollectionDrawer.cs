using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020F9 RID: 8441
	public abstract class CollectionDrawer : MemberDrawer
	{
		// Token: 0x0600B3A3 RID: 45987
		public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

		// Token: 0x0600B3A4 RID: 45988 RVA: 0x0011483F File Offset: 0x00112A3F
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			if (this.IsEmpty(context, member))
			{
				return MemberDrawType.Inline;
			}
			return MemberDrawType.Custom;
		}

		// Token: 0x0600B3A5 RID: 45989 RVA: 0x0011484E File Offset: 0x00112A4E
		protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Debug.Assert(this.IsEmpty(context, member));
			this.DrawEmpty(context, member);
		}

		// Token: 0x0600B3A6 RID: 45990 RVA: 0x00114865 File Offset: 0x00112A65
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			Debug.Assert(!this.IsEmpty(context, member));
			this.DrawWithContents(context, member, depth);
		}

		// Token: 0x0600B3A7 RID: 45991 RVA: 0x00114880 File Offset: 0x00112A80
		private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			ImGui.Text(member.name + "(empty)");
		}

		// Token: 0x0600B3A8 RID: 45992 RVA: 0x0043ABD8 File Offset: 0x00438DD8
		private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			CollectionDrawer.<>c__DisplayClass5_0 CS$<>8__locals1 = new CollectionDrawer.<>c__DisplayClass5_0();
			CS$<>8__locals1.depth = depth;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && CS$<>8__locals1.depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.VisitElements(new CollectionDrawer.ElementVisitor(CS$<>8__locals1.<DrawWithContents>g__Visitor|0), context, member);
				ImGui.TreePop();
			}
		}

		// Token: 0x0600B3A9 RID: 45993
		protected abstract void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member);

		// Token: 0x020020FA RID: 8442
		// (Invoke) Token: 0x0600B3AC RID: 45996
		protected delegate void ElementVisitor(in MemberDrawContext context, CollectionDrawer.Element element);

		// Token: 0x020020FB RID: 8443
		protected struct Element
		{
			// Token: 0x0600B3AF RID: 45999 RVA: 0x00114897 File Offset: 0x00112A97
			public Element(string node_name, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this.node_name = node_name;
				this.draw_tooltip = draw_tooltip;
				this.get_object_to_inspect = get_object_to_inspect;
			}

			// Token: 0x0600B3B0 RID: 46000 RVA: 0x001148AE File Offset: 0x00112AAE
			public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this = new CollectionDrawer.Element(string.Format("[{0}]", index), draw_tooltip, get_object_to_inspect);
			}

			// Token: 0x04008DCB RID: 36299
			public readonly string node_name;

			// Token: 0x04008DCC RID: 36300
			public readonly System.Action draw_tooltip;

			// Token: 0x04008DCD RID: 36301
			public readonly Func<object> get_object_to_inspect;
		}
	}
}
