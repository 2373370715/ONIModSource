using System;

namespace Database
{
	// Token: 0x02002163 RID: 8547
	public class Shirts : ResourceSet<Shirt>
	{
		// Token: 0x0600B5E4 RID: 46564 RVA: 0x001153AB File Offset: 0x001135AB
		public Shirts()
		{
			this.Hot00 = base.Add(new Shirt("body_shirt_hot_shearling"));
			this.Decor00 = base.Add(new Shirt("body_shirt_decor01"));
		}

		// Token: 0x04009420 RID: 37920
		public Shirt Hot00;

		// Token: 0x04009421 RID: 37921
		public Shirt Decor00;
	}
}
