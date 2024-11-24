using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EC9 RID: 7881
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
	// Token: 0x0600A59C RID: 42396 RVA: 0x003ED410 File Offset: 0x003EB610
	public void SetLine(ReportManager.ReportGroup reportGroup)
	{
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		component.minWidth = (component.preferredWidth = this.nameWidth);
		this.spacer.minWidth = this.groupSpacerWidth;
		this.name.text = reportGroup.stringKey;
	}

	// Token: 0x040081C5 RID: 33221
	[SerializeField]
	public new LocText name;

	// Token: 0x040081C6 RID: 33222
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x040081C7 RID: 33223
	[SerializeField]
	private Image bgImage;

	// Token: 0x040081C8 RID: 33224
	public float groupSpacerWidth;

	// Token: 0x040081C9 RID: 33225
	private float nameWidth = 164f;

	// Token: 0x040081CA RID: 33226
	[SerializeField]
	private Color oddRowColor;
}
