using System;
using System.Collections.Generic;

// Token: 0x02001BC3 RID: 7107
public class UIFloatFormatter
{
	// Token: 0x060093D1 RID: 37841 RVA: 0x00100558 File Offset: 0x000FE758
	public string Format(string format, float value)
	{
		return this.Replace(format, "{0}", value);
	}

	// Token: 0x060093D2 RID: 37842 RVA: 0x00390A78 File Offset: 0x0038EC78
	private string Replace(string format, string key, float value)
	{
		UIFloatFormatter.Entry entry = default(UIFloatFormatter.Entry);
		if (this.activeStringCount >= this.entries.Count)
		{
			entry.format = format;
			entry.key = key;
			entry.value = value;
			entry.result = entry.format.Replace(key, value.ToString());
			this.entries.Add(entry);
		}
		else
		{
			entry = this.entries[this.activeStringCount];
			if (entry.format != format || entry.key != key || entry.value != value)
			{
				entry.format = format;
				entry.key = key;
				entry.value = value;
				entry.result = entry.format.Replace(key, value.ToString());
				this.entries[this.activeStringCount] = entry;
			}
		}
		this.activeStringCount++;
		return entry.result;
	}

	// Token: 0x060093D3 RID: 37843 RVA: 0x00100567 File Offset: 0x000FE767
	public void BeginDrawing()
	{
		this.activeStringCount = 0;
	}

	// Token: 0x060093D4 RID: 37844 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void EndDrawing()
	{
	}

	// Token: 0x040072B9 RID: 29369
	private int activeStringCount;

	// Token: 0x040072BA RID: 29370
	private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();

	// Token: 0x02001BC4 RID: 7108
	private struct Entry
	{
		// Token: 0x040072BB RID: 29371
		public string format;

		// Token: 0x040072BC RID: 29372
		public string key;

		// Token: 0x040072BD RID: 29373
		public float value;

		// Token: 0x040072BE RID: 29374
		public string result;
	}
}
