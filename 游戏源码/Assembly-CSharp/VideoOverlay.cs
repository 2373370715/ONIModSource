using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02002047 RID: 8263
[AddComponentMenu("KMonoBehaviour/scripts/VideoOverlay")]
public class VideoOverlay : KMonoBehaviour
{
	// Token: 0x0600AFE6 RID: 45030 RVA: 0x004225D4 File Offset: 0x004207D4
	public void SetText(List<string> strings)
	{
		if (strings.Count != this.textFields.Count)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				base.name,
				"expects",
				this.textFields.Count,
				"strings passed to it, got",
				strings.Count
			});
		}
		for (int i = 0; i < this.textFields.Count; i++)
		{
			this.textFields[i].text = strings[i];
		}
	}

	// Token: 0x04008A9E RID: 35486
	public List<LocText> textFields;
}
