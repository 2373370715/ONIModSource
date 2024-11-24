namespace ImGuiObjectDrawer;

public sealed class KAnimHashedStringDrawer : InlineDrawer
{
	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		return member.value is KAnimHashedString;
	}

	protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
	{
		KAnimHashedString kAnimHashedString = (KAnimHashedString)member.value;
		string text = kAnimHashedString.ToString();
		string text2 = "0x" + kAnimHashedString.HashValue.ToString("X");
		ImGuiEx.SimpleField(member.name, text + " (" + text2 + ")");
	}
}
