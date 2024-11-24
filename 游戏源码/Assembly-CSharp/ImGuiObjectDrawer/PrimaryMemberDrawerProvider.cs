using System.Collections.Generic;

namespace ImGuiObjectDrawer;

public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
{
	public int Priority => 100;

	public void AppendDrawersTo(List<MemberDrawer> drawers)
	{
		drawers.AddRange(new MemberDrawer[15]
		{
			new NullDrawer(),
			new SimpleDrawer(),
			new LocStringDrawer(),
			new EnumDrawer(),
			new HashedStringDrawer(),
			new KAnimHashedStringDrawer(),
			new Vector2Drawer(),
			new Vector3Drawer(),
			new Vector4Drawer(),
			new UnityObjectDrawer(),
			new ArrayDrawer(),
			new IDictionaryDrawer(),
			new IEnumerableDrawer(),
			new PlainCSharpObjectDrawer(),
			new FallbackDrawer()
		});
	}
}
