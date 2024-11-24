using System;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CD1 RID: 7377
public class FeedbackScreen : KModalScreen
{
	// Token: 0x06009A09 RID: 39433 RVA: 0x003B7D80 File Offset: 0x003B5F80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.bugForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		};
		this.suggestionForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/");
		};
		this.logsDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(Util.LogsFolder());
		};
		this.saveFilesDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		};
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.logsDirectoryButton.GetComponentInParent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
			this.saveFilesDirectoryButton.gameObject.SetActive(false);
			this.logsDirectoryButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400783B RID: 30779
	public LocText title;

	// Token: 0x0400783C RID: 30780
	public KButton dismissButton;

	// Token: 0x0400783D RID: 30781
	public KButton closeButton;

	// Token: 0x0400783E RID: 30782
	public KButton bugForumsButton;

	// Token: 0x0400783F RID: 30783
	public KButton suggestionForumsButton;

	// Token: 0x04007840 RID: 30784
	public KButton logsDirectoryButton;

	// Token: 0x04007841 RID: 30785
	public KButton saveFilesDirectoryButton;
}
