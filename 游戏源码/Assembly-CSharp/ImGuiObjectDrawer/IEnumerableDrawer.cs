using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002102 RID: 8450
	public sealed class IEnumerableDrawer : CollectionDrawer
	{
		// Token: 0x0600B3C2 RID: 46018 RVA: 0x001149C9 File Offset: 0x00112BC9
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IEnumerable>();
		}

		// Token: 0x0600B3C3 RID: 46019 RVA: 0x001149D1 File Offset: 0x00112BD1
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return !((IEnumerable)member.value).GetEnumerator().MoveNext();
		}

		// Token: 0x0600B3C4 RID: 46020 RVA: 0x0043AD7C File Offset: 0x00438F7C
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IEnumerable enumerable = (IEnumerable)member.value;
			int num = 0;
			using (IEnumerator enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object el = enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
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
	}
}
