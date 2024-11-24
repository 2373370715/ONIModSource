using System;

namespace Database
{
	// Token: 0x02002133 RID: 8499
	public class Dreams : ResourceSet<Dream>
	{
		// Token: 0x0600B514 RID: 46356 RVA: 0x00114F21 File Offset: 0x00113121
		public Dreams(ResourceSet parent) : base("Dreams", parent)
		{
			this.CommonDream = new Dream("CommonDream", this, "dream_tear_swirly_kanim", new string[]
			{
				"dreamIcon_journal"
			});
		}

		// Token: 0x040091A0 RID: 37280
		public Dream CommonDream;
	}
}
