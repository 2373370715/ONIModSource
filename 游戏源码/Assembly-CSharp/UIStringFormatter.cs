using System;
using System.Collections.Generic;

// Token: 0x02001BC5 RID: 7109
public class UIStringFormatter
{
	// Token: 0x040072BF RID: 29375
	private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();

	// Token: 0x02001BC6 RID: 7110
	private struct Entry
	{
		// Token: 0x040072C0 RID: 29376
		public string format;

		// Token: 0x040072C1 RID: 29377
		public string key;

		// Token: 0x040072C2 RID: 29378
		public string value;

		// Token: 0x040072C3 RID: 29379
		public string result;
	}
}
