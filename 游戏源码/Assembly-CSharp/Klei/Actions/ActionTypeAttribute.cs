using System;

namespace Klei.Actions
{
	// Token: 0x02003BAB RID: 15275
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ActionTypeAttribute : Attribute
	{
		// Token: 0x0600EB5A RID: 60250 RVA: 0x0013D581 File Offset: 0x0013B781
		public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
		{
			this.TypeName = typeName;
			this.GroupName = groupName;
			this.GenerateConfig = generateConfig;
		}

		// Token: 0x0600EB5B RID: 60251 RVA: 0x004CCD20 File Offset: 0x004CAF20
		public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			bool flag = object.Equals(lhs, null);
			bool flag2 = object.Equals(rhs, null);
			if (flag || flag2)
			{
				return flag == flag2;
			}
			return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
		}

		// Token: 0x0600EB5C RID: 60252 RVA: 0x0013D59E File Offset: 0x0013B79E
		public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600EB5D RID: 60253 RVA: 0x0013D5AA File Offset: 0x0013B7AA
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x0600EB5E RID: 60254 RVA: 0x0013D5B3 File Offset: 0x0013B7B3
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400E671 RID: 58993
		public readonly string TypeName;

		// Token: 0x0400E672 RID: 58994
		public readonly string GroupName;

		// Token: 0x0400E673 RID: 58995
		public readonly bool GenerateConfig;
	}
}
