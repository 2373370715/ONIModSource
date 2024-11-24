using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02001E06 RID: 7686
public class TutorialMessageDialog : MessageDialog
{
	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x0600A0E7 RID: 41191 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool CanDontShowAgain
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600A0E8 RID: 41192 RVA: 0x001089FD File Offset: 0x00106BFD
	public override bool CanDisplay(Message message)
	{
		return typeof(TutorialMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A0E9 RID: 41193 RVA: 0x003D6444 File Offset: 0x003D4644
	public override void SetMessage(Message base_message)
	{
		this.message = (base_message as TutorialMessage);
		this.description.text = this.message.GetMessageBody();
		if (!string.IsNullOrEmpty(this.message.videoClipId))
		{
			VideoClip video = Assets.GetVideo(this.message.videoClipId);
			this.SetVideo(video, this.message.videoOverlayName, this.message.videoTitleText);
		}
	}

	// Token: 0x0600A0EA RID: 41194 RVA: 0x003D64B4 File Offset: 0x003D46B4
	public void SetVideo(VideoClip clip, string overlayName, string titleText)
	{
		if (this.videoWidget == null)
		{
			this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, base.transform.gameObject, true).GetComponent<VideoWidget>();
			this.videoWidget.transform.SetAsFirstSibling();
		}
		this.videoWidget.SetClip(clip, overlayName, new List<string>
		{
			titleText,
			VIDEOS.TUTORIAL_HEADER
		});
	}

	// Token: 0x0600A0EB RID: 41195 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void OnClickAction()
	{
	}

	// Token: 0x0600A0EC RID: 41196 RVA: 0x00108A14 File Offset: 0x00106C14
	public override void OnDontShowAgain()
	{
		Tutorial.Instance.HideTutorialMessage(this.message.messageId);
	}

	// Token: 0x04007DA2 RID: 32162
	[SerializeField]
	private LocText description;

	// Token: 0x04007DA3 RID: 32163
	private TutorialMessage message;

	// Token: 0x04007DA4 RID: 32164
	[SerializeField]
	private GameObject videoWidgetPrefab;

	// Token: 0x04007DA5 RID: 32165
	private VideoWidget videoWidget;
}
