using System.Collections;

namespace ImGuiObjectDrawer;

public sealed class IDictionaryDrawer : CollectionDrawer
{
	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		return member.CanAssignToType<IDictionary>();
	}

	public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
	{
		return ((IDictionary)member.value).Count == 0;
	}

	protected override void VisitElements(ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
	{
		IDictionary obj = (IDictionary)member.value;
		int num = 0;
		foreach (DictionaryEntry kvp in obj)
		{
			visit(in context, new Element(num, delegate
			{
				DrawerUtil.Tooltip($"{kvp.Key.GetType()} -> {kvp.Value.GetType()}");
			}, () => new
			{
				key = kvp.Key,
				value = kvp.Value
			}));
			num++;
		}
	}
}
