using UnityEngine;

namespace ImGuiObjectDrawer;

public sealed class Vector3Drawer : InlineDrawer
{
	public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
	{
		return member.value is Vector3;
	}

	protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
	{
		Vector3 vector = (Vector3)member.value;
		ImGuiEx.SimpleField(member.name, $"( {vector.x}, {vector.y}, {vector.z} )");
	}
}
