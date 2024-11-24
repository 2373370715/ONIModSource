using System;
using UnityEngine;

// Token: 0x02001B33 RID: 6963
public class PatchNotesScreen : KModalScreen
{
	// Token: 0x06009217 RID: 37399 RVA: 0x00385C50 File Offset: 0x00383E50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		this.closeButton.onClick += this.MarkAsReadAndClose;
		this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
		this.okButton.onClick += this.MarkAsReadAndClose;
		this.previousVersion.onClick += delegate()
		{
			App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
		};
		this.fullPatchNotes.onClick += this.OnPatchNotesClick;
		PatchNotesScreen.instance = this;
	}

	// Token: 0x06009218 RID: 37400 RVA: 0x000FF69F File Offset: 0x000FD89F
	protected override void OnCleanUp()
	{
		PatchNotesScreen.instance = null;
	}

	// Token: 0x06009219 RID: 37401 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public static bool ShouldShowScreen()
	{
		return false;
	}

	// Token: 0x0600921A RID: 37402 RVA: 0x000FF6A7 File Offset: 0x000FD8A7
	private void MarkAsReadAndClose()
	{
		KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
		this.Deactivate();
	}

	// Token: 0x0600921B RID: 37403 RVA: 0x000FF6BE File Offset: 0x000FD8BE
	public static void UpdatePatchNotes(string patchNotesSummary, string url)
	{
		PatchNotesScreen.m_patchNotesUrl = url;
		PatchNotesScreen.m_patchNotesText = patchNotesSummary;
		if (PatchNotesScreen.instance != null)
		{
			PatchNotesScreen.instance.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		}
	}

	// Token: 0x0600921C RID: 37404 RVA: 0x000FF6ED File Offset: 0x000FD8ED
	private void OnPatchNotesClick()
	{
		App.OpenWebURL(PatchNotesScreen.m_patchNotesUrl);
	}

	// Token: 0x0600921D RID: 37405 RVA: 0x000FF6F9 File Offset: 0x000FD8F9
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.MarkAsReadAndClose();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04006E83 RID: 28291
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006E84 RID: 28292
	[SerializeField]
	private KButton okButton;

	// Token: 0x04006E85 RID: 28293
	[SerializeField]
	private KButton fullPatchNotes;

	// Token: 0x04006E86 RID: 28294
	[SerializeField]
	private KButton previousVersion;

	// Token: 0x04006E87 RID: 28295
	[SerializeField]
	private LocText changesLabel;

	// Token: 0x04006E88 RID: 28296
	private static string m_patchNotesUrl;

	// Token: 0x04006E89 RID: 28297
	private static string m_patchNotesText;

	// Token: 0x04006E8A RID: 28298
	private static int PatchNotesVersion = 9;

	// Token: 0x04006E8B RID: 28299
	private static PatchNotesScreen instance;
}
