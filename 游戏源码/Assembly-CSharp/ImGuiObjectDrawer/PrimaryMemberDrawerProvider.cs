using System;
using System.Collections.Generic;

namespace ImGuiObjectDrawer
{
	// Token: 0x020020ED RID: 8429
	public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
	{
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600B37D RID: 45949 RVA: 0x000CECD9 File Offset: 0x000CCED9
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x0600B37E RID: 45950 RVA: 0x0043A978 File Offset: 0x00438B78
		public void AppendDrawersTo(List<MemberDrawer> drawers)
		{
			drawers.AddRange(new MemberDrawer[]
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
}
