using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001E60 RID: 7776
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class OldVersionMessageScreen : KModalScreen
{
	// Token: 0x0600A30E RID: 41742 RVA: 0x003DFF2C File Offset: 0x003DE12C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/140474-previous-update-steam-branch-access/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.quitButton.onClick += delegate()
		{
			App.Quit();
		};
	}

	// Token: 0x0600A30F RID: 41743 RVA: 0x003DFFAC File Offset: 0x003DE1AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.messageContainer.sizeDelta = new Vector2(Mathf.Max(384f, (float)Screen.width * 0.25f), this.messageContainer.sizeDelta.y);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04007F3D RID: 32573
	public KButton forumButton;

	// Token: 0x04007F3E RID: 32574
	public KButton confirmButton;

	// Token: 0x04007F3F RID: 32575
	public KButton quitButton;

	// Token: 0x04007F40 RID: 32576
	public LocText bodyText;

	// Token: 0x04007F41 RID: 32577
	public bool previewInEditor;

	// Token: 0x04007F42 RID: 32578
	public RectTransform messageContainer;
}
