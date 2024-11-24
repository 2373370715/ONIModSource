using System;
using KSerialization;
using TMPro;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class RailModUploadScreen : KModalScreen
{
	// Token: 0x04000009 RID: 9
	[SerializeField]
	private KButton[] closeButtons;

	// Token: 0x0400000A RID: 10
	[SerializeField]
	private KButton submitButton;

	// Token: 0x0400000B RID: 11
	[SerializeField]
	private ToolTip submitButtonTooltip;

	// Token: 0x0400000C RID: 12
	[SerializeField]
	private TMP_InputField modName;

	// Token: 0x0400000D RID: 13
	[SerializeField]
	private TMP_InputField modDesc;

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private TMP_InputField modVersion;

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private TMP_InputField contentFolder;

	// Token: 0x04000010 RID: 16
	[SerializeField]
	private TMP_InputField previewImage;

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private MultiToggle[] shareTypeToggles;

	// Token: 0x04000012 RID: 18
	[Serialize]
	private string previousFolderPath;
}
