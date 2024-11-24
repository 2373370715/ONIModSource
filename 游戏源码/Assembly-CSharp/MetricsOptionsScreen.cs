using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E16 RID: 7702
public class MetricsOptionsScreen : KModalScreen
{
	// Token: 0x0600A13E RID: 41278 RVA: 0x00108C7B File Offset: 0x00106E7B
	private bool IsSettingsDirty()
	{
		return this.disableDataCollection != KPrivacyPrefs.instance.disableDataCollection;
	}

	// Token: 0x0600A13F RID: 41279 RVA: 0x00108C92 File Offset: 0x00106E92
	public override void OnKeyDown(KButtonEvent e)
	{
		if ((e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)) && !this.IsSettingsDirty())
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A140 RID: 41280 RVA: 0x003D7378 File Offset: 0x003D5578
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.disableDataCollection = KPrivacyPrefs.instance.disableDataCollection;
		this.title.SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
		GameObject gameObject = this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.OnClickToggle();
		};
		this.enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
		this.dismissButton.onClick += delegate()
		{
			if (this.IsSettingsDirty())
			{
				this.ApplySettingsAndDoRestart();
				return;
			}
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.descriptionButton.onClick.AddListener(delegate()
		{
			App.OpenWebURL("https://www.kleientertainment.com/privacy-policy");
		});
		this.Refresh();
	}

	// Token: 0x0600A141 RID: 41281 RVA: 0x00108CBC File Offset: 0x00106EBC
	private void OnClickToggle()
	{
		this.disableDataCollection = !this.disableDataCollection;
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(this.disableDataCollection);
		this.Refresh();
	}

	// Token: 0x0600A142 RID: 41282 RVA: 0x003D747C File Offset: 0x003D567C
	private void ApplySettingsAndDoRestart()
	{
		KPrivacyPrefs.instance.disableDataCollection = this.disableDataCollection;
		KPrivacyPrefs.Save();
		KPlayerPrefs.SetString("DisableDataCollection", KPrivacyPrefs.instance.disableDataCollection ? "yes" : "no");
		KPlayerPrefs.Save();
		ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
		App.instance.Restart();
	}

	// Token: 0x0600A143 RID: 41283 RVA: 0x003D7510 File Offset: 0x003D5710
	private void Refresh()
	{
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").transform.GetChild(0).gameObject.SetActive(!this.disableDataCollection);
		this.closeButton.isInteractable = !this.IsSettingsDirty();
		this.restartWarningText.gameObject.SetActive(this.IsSettingsDirty());
		if (this.IsSettingsDirty())
		{
			this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.RESTART_BUTTON;
			return;
		}
		this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.DONE_BUTTON;
	}

	// Token: 0x04007DCF RID: 32207
	public LocText title;

	// Token: 0x04007DD0 RID: 32208
	public KButton dismissButton;

	// Token: 0x04007DD1 RID: 32209
	public KButton closeButton;

	// Token: 0x04007DD2 RID: 32210
	public GameObject enableButton;

	// Token: 0x04007DD3 RID: 32211
	public Button descriptionButton;

	// Token: 0x04007DD4 RID: 32212
	public LocText restartWarningText;

	// Token: 0x04007DD5 RID: 32213
	private bool disableDataCollection;
}
