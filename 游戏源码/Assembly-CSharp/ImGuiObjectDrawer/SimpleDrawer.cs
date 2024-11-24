namespace ImGuiObjectDrawer;

public class SimpleDrawer : InlineDrawer
{
	public override bool CanDrawAtDepth(int depth)
	{
		return true;
	}

	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		if (!member.type.IsPrimitive)
		{
			return member.CanAssignToType<string>();
		}
		return true;
	}

	protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
	{
		ImGuiEx.SimpleField(member.name, member.value.ToString());
	}
}
