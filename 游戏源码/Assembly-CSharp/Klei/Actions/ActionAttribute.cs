using System;

namespace Klei.Actions
{
	// Token: 0x02003BAC RID: 15276
	[AttributeUsage(AttributeTargets.Class)]
	public class ActionAttribute : Attribute
	{
		// Token: 0x0600EB5F RID: 60255 RVA: 0x0013D5BB File Offset: 0x0013B7BB
		public ActionAttribute(string actionName)
		{
			this.ActionName = actionName;
		}

		// Token: 0x0400E674 RID: 58996
		public readonly string ActionName;
	}
}
