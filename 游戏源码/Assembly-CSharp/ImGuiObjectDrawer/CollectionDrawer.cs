using System;
using ImGuiNET;

namespace ImGuiObjectDrawer;

public abstract class CollectionDrawer : MemberDrawer
{
	protected delegate void ElementVisitor(in MemberDrawContext context, Element element);

	protected struct Element
	{
		public readonly string node_name;

		public readonly System.Action draw_tooltip;

		public readonly Func<object> get_object_to_inspect;

		public Element(string node_name, System.Action draw_tooltip, Func<object> get_object_to_inspect)
		{
			this.node_name = node_name;
			this.draw_tooltip = draw_tooltip;
			this.get_object_to_inspect = get_object_to_inspect;
		}

		public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			: this($"[{index}]", draw_tooltip, get_object_to_inspect)
		{
		}
	}

	public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

	public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
	{
		if (IsEmpty(in context, in member))
		{
			return MemberDrawType.Inline;
		}
		return MemberDrawType.Custom;
	}

	protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
	{
		Debug.Assert(IsEmpty(in context, in member));
		DrawEmpty(in context, in member);
	}

	protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
	{
		Debug.Assert(!IsEmpty(in context, in member));
		DrawWithContents(in context, in member, depth);
	}

	private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
	{
		ImGui.Text(member.name + "(empty)");
	}

	private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
	{
		ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
		if (context.default_open && depth <= 0)
		{
			imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
		}
		bool num = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
		DrawerUtil.Tooltip(member.type);
		if (num)
		{
			VisitElements(Visitor, in context, in member);
			ImGui.TreePop();
		}
		void Visitor(in MemberDrawContext context, Element element)
		{
			bool num2 = ImGui.TreeNode(element.node_name);
			element.draw_tooltip();
			if (num2)
			{
				DrawerUtil.DrawObjectContents(element.get_object_to_inspect(), in context, depth + 1);
				ImGui.TreePop();
			}
		}
	}

	protected abstract void VisitElements(ElementVisitor visit, in MemberDrawContext context, in MemberDetails member);
}
