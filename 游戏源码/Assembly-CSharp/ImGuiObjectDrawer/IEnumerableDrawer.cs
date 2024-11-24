using System.Collections;

namespace ImGuiObjectDrawer;

public sealed class IEnumerableDrawer : CollectionDrawer
{
	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		return member.CanAssignToType<IEnumerable>();
	}

	public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
	{
		return !((IEnumerable)member.value).GetEnumerator().MoveNext();
	}

	protected override void VisitElements(ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
	{
		IEnumerable obj = (IEnumerable)member.value;
		int num = 0;
		foreach (object el in obj)
		{
			visit(in context, new Element(num, delegate
			{
				DrawerUtil.Tooltip(el.GetType());
			}, () => new
			{
				value = el
			}));
			num++;
		}
	}
}
