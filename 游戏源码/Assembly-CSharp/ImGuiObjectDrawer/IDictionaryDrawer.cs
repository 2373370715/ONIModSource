using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002100 RID: 8448
	public sealed class IDictionaryDrawer : CollectionDrawer
	{
		// Token: 0x0600B3BB RID: 46011 RVA: 0x0011495E File Offset: 0x00112B5E
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IDictionary>();
		}

		// Token: 0x0600B3BC RID: 46012 RVA: 0x00114966 File Offset: 0x00112B66
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((IDictionary)member.value).Count == 0;
		}

		// Token: 0x0600B3BD RID: 46013 RVA: 0x0043ACEC File Offset: 0x00438EEC
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IDictionary dictionary = (IDictionary)member.value;
			int num = 0;
			using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry kvp = (DictionaryEntry)enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(string.Format("{0} -> {1}", kvp.Key.GetType(), kvp.Value.GetType()));
					}, () => new
					{
						key = kvp.Key,
						value = kvp.Value
					}));
					num++;
				}
			}
		}
	}
}
