using System;

namespace KMod
{
	// Token: 0x020021CF RID: 8655
	public class KModHeader
	{
		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x0600B776 RID: 46966 RVA: 0x001162BF File Offset: 0x001144BF
		// (set) Token: 0x0600B777 RID: 46967 RVA: 0x001162C7 File Offset: 0x001144C7
		public string staticID { get; set; }

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x0600B778 RID: 46968 RVA: 0x001162D0 File Offset: 0x001144D0
		// (set) Token: 0x0600B779 RID: 46969 RVA: 0x001162D8 File Offset: 0x001144D8
		public string title { get; set; }

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600B77A RID: 46970 RVA: 0x001162E1 File Offset: 0x001144E1
		// (set) Token: 0x0600B77B RID: 46971 RVA: 0x001162E9 File Offset: 0x001144E9
		public string description { get; set; }
	}
}
