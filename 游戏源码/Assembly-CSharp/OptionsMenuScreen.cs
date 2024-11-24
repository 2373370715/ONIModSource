using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001E68 RID: 7784
public class OptionsMenuScreen : KModalButtonMenu
{
	// Token: 0x0600A327 RID: 41767 RVA: 0x003E0220 File Offset: 0x003DE420
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepMenuOpen = true;
		this.buttons = new List<KButtonMenu.ButtonInfo>
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GRAPHICS, global::Action.NumActions, new UnityAction(this.OnGraphicsOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.AUDIO, global::Action.NumActions, new UnityAction(this.OnAudioOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GAME, global::Action.NumActions, new UnityAction(this.OnGameOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.METRICS, global::Action.NumActions, new UnityAction(this.OnMetrics), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.FEEDBACK, global::Action.NumActions, new UnityAction(this.OnFeedback), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.CREDITS, global::Action.NumActions, new UnityAction(this.OnCredits), null, null)
		};
		this.closeButton.onClick += this.Deactivate;
		this.backButton.onClick += this.Deactivate;
	}

	// Token: 0x0600A328 RID: 41768 RVA: 0x00109D4D File Offset: 0x00107F4D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.OPTIONS_SCREEN.TITLE);
		this.backButton.transform.SetAsLastSibling();
	}

	// Token: 0x0600A329 RID: 41769 RVA: 0x003E0368 File Offset: 0x003DE568
	protected override void OnActivate()
	{
		base.OnActivate();
		foreach (GameObject gameObject in this.buttonObjects)
		{
		}
	}

	// Token: 0x0600A32A RID: 41770 RVA: 0x0010463C File Offset: 0x0010283C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A32B RID: 41771 RVA: 0x00109D7A File Offset: 0x00107F7A
	private void OnGraphicsOptions()
	{
		base.ActivateChildScreen(this.graphicsOptionsScreenPrefab.gameObject);
	}

	// Token: 0x0600A32C RID: 41772 RVA: 0x00109D8E File Offset: 0x00107F8E
	private void OnAudioOptions()
	{
		base.ActivateChildScreen(this.audioOptionsScreenPrefab.gameObject);
	}

	// Token: 0x0600A32D RID: 41773 RVA: 0x00109DA2 File Offset: 0x00107FA2
	private void OnGameOptions()
	{
		base.ActivateChildScreen(this.gameOptionsScreenPrefab.gameObject);
	}

	// Token: 0x0600A32E RID: 41774 RVA: 0x00109DB6 File Offset: 0x00107FB6
	private void OnMetrics()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x0600A32F RID: 41775 RVA: 0x00109DB6 File Offset: 0x00107FB6
	public void ShowMetricsScreen()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x0600A330 RID: 41776 RVA: 0x00109DCA File Offset: 0x00107FCA
	private void OnFeedback()
	{
		base.ActivateChildScreen(this.feedbackScreenPrefab.gameObject);
	}

	// Token: 0x0600A331 RID: 41777 RVA: 0x00109DDE File Offset: 0x00107FDE
	private void OnCredits()
	{
		base.ActivateChildScreen(this.creditsScreenPrefab.gameObject);
	}

	// Token: 0x0600A332 RID: 41778 RVA: 0x001047BE File Offset: 0x001029BE
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x04007F57 RID: 32599
	[SerializeField]
	private GameOptionsScreen gameOptionsScreenPrefab;

	// Token: 0x04007F58 RID: 32600
	[SerializeField]
	private AudioOptionsScreen audioOptionsScreenPrefab;

	// Token: 0x04007F59 RID: 32601
	[SerializeField]
	private GraphicsOptionsScreen graphicsOptionsScreenPrefab;

	// Token: 0x04007F5A RID: 32602
	[SerializeField]
	private CreditsScreen creditsScreenPrefab;

	// Token: 0x04007F5B RID: 32603
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007F5C RID: 32604
	[SerializeField]
	private MetricsOptionsScreen metricsScreenPrefab;

	// Token: 0x04007F5D RID: 32605
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x04007F5E RID: 32606
	[SerializeField]
	private LocText title;

	// Token: 0x04007F5F RID: 32607
	[SerializeField]
	private KButton backButton;
}
