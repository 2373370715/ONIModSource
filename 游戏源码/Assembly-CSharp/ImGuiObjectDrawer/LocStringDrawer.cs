namespace ImGuiObjectDrawer;

public sealed class LocStringDrawer : InlineDrawer
{
	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		return member.CanAssignToType<LocString>();
	}

	protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
	{
		ImGuiEx.SimpleField(member.name, $"{member.value}({((LocString)member.value).text})");
	}
}
