using System;

namespace ImGuiObjectDrawer
{
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

		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			ArrayDrawer.<>c__DisplayClass2_0 CS$<>8__locals1 = new ArrayDrawer.<>c__DisplayClass2_0();
			CS$<>8__locals1.array = (Array)member.value;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.array.Length; i = i2)
			{
				int j = i;
				System.Action draw_tooltip;
				if ((draw_tooltip = CS$<>8__locals1.<>9__0) == null)
				{
					draw_tooltip = (CS$<>8__locals1.<>9__0 = delegate()
					{
						DrawerUtil.Tooltip(CS$<>8__locals1.array.GetType().GetElementType());
					});
				}
				visit(context, new CollectionDrawer.Element(j, draw_tooltip, () => new
				{
					value = CS$<>8__locals1.array.GetValue(i)
				}));
				i2 = i + 1;
			}
		}
	}
}
