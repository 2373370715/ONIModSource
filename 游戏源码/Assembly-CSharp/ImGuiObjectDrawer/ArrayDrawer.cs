using System;

namespace ImGuiObjectDrawer;

public sealed class ArrayDrawer : CollectionDrawer
{
	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		return member.type.IsArray;
	}

	public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
	{
		return ((Array)member.value).Length == 0;
	}

	protected override void VisitElements(ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
	{
		Array array = (Array)member.value;
		int i = 0;
		while (i < array.Length)
		{
			visit(in context, new Element(i, delegate
			{
				DrawerUtil.Tooltip(array.GetType().GetElementType());
			}, () => new
			{
				value = array.GetValue(i)
			}));
			int num = i + 1;
			i = num;
		}
	}
}
