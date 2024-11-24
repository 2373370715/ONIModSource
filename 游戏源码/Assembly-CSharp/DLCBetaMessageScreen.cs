using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001C8F RID: 7311
public class DLCBetaMessageScreen : KModalScreen
{
	// Token: 0x0600986E RID: 39022 RVA: 0x003AFBCC File Offset: 0x003ADDCC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
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

	// Token: 0x0600986F RID: 39023 RVA: 0x003AFC20 File Offset: 0x003ADE20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.betaIsLive || (Application.isEditor && this.skipInEditor) || !DlcManager.GetActiveDLCIds().Contains("DLC2_ID"))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x06009870 RID: 39024 RVA: 0x0010326E File Offset: 0x0010146E
	private void Update()
	{
		this.logo.rectTransform().localPosition = new Vector3(0f, Mathf.Sin(Time.realtimeSinceStartup) * 7.5f);
	}

	// Token: 0x040076B3 RID: 30387
	public RectTransform logo;

	// Token: 0x040076B4 RID: 30388
	public KButton confirmButton;

	// Token: 0x040076B5 RID: 30389
	public KButton quitButton;

	// Token: 0x040076B6 RID: 30390
	public LocText bodyText;

	// Token: 0x040076B7 RID: 30391
	public RectTransform messageContainer;

	// Token: 0x040076B8 RID: 30392
	private bool betaIsLive;

	// Token: 0x040076B9 RID: 30393
	private bool skipInEditor;
}
