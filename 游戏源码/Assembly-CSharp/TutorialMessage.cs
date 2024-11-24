using System;
using KSerialization;

// Token: 0x02001E05 RID: 7685
public class TutorialMessage : GenericMessage
{
	// Token: 0x0600A0E5 RID: 41189 RVA: 0x001089EA File Offset: 0x00106BEA
	public TutorialMessage()
	{
	}

	// Token: 0x0600A0E6 RID: 41190 RVA: 0x003D63EC File Offset: 0x003D45EC
	public TutorialMessage(Tutorial.TutorialMessages messageId, string title, string body, string tooltip, string videoClipId = null, string videoOverlayName = null, string videoTitleText = null, string icon = "", string[] overrideDLCIDs = null) : base(title, body, tooltip, null)
	{
		this.messageId = messageId;
		this.videoClipId = videoClipId;
		this.videoOverlayName = videoOverlayName;
		this.videoTitleText = videoTitleText;
		this.icon = icon;
		if (overrideDLCIDs != null)
		{
			this.DLCIDs = overrideDLCIDs;
		}
	}

	// Token: 0x04007D9C RID: 32156
	[Serialize]
	public Tutorial.TutorialMessages messageId;

	// Token: 0x04007D9D RID: 32157
	public string videoClipId;

	// Token: 0x04007D9E RID: 32158
	public string videoOverlayName;

	// Token: 0x04007D9F RID: 32159
	public string videoTitleText;

	// Token: 0x04007DA0 RID: 32160
	public string icon;

	// Token: 0x04007DA1 RID: 32161
	public string[] DLCIDs = DlcManager.AVAILABLE_ALL_VERSIONS;
}
