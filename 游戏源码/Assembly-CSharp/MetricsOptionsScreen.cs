using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class MetricsOptionsScreen : KModalScreen
{
	public LocText title;

	public KButton dismissButton;

	public KButton closeButton;

	public GameObject enableButton;

	public Button descriptionButton;

	public LocText restartWarningText;

	private bool disableDataCollection;

	private bool IsSettingsDirty()
	{
		return disableDataCollection != KPrivacyPrefs.instance.disableDataCollection;
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if ((e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight)) && !IsSettingsDirty())
		{
			Show(show: false);
		}
		base.OnKeyDown(e);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		disableDataCollection = KPrivacyPrefs.instance.disableDataCollection;
		title.SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
		GameObject obj = enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
		obj.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
		obj.GetComponent<KButton>().onClick += delegate
		{
			OnClickToggle();
		};
		enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
		dismissButton.onClick += delegate
		{
			if (IsSettingsDirty())
			{
				ApplySettingsAndDoRestart();
			}
			else
			{
				Deactivate();
			}
		};
		closeButton.onClick += delegate
		{
			Deactivate();
		};
		descriptionButton.onClick.AddListener(delegate
		{
			App.OpenWebURL("https://www.kleientertainment.com/privacy-policy");
		});
		Refresh();
	}

	private void OnClickToggle()
	{
		disableDataCollection = !disableDataCollection;
		enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(disableDataCollection);
		Refresh();
	}

	private void ApplySettingsAndDoRestart()
	{
		KPrivacyPrefs.instance.disableDataCollection = disableDataCollection;
		KPrivacyPrefs.Save();
		KPlayerPrefs.SetString("DisableDataCollection", KPrivacyPrefs.instance.disableDataCollection ? "yes" : "no");
		KPlayerPrefs.Save();
		ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
		enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
		App.instance.Restart();
	}

	private void Refresh()
	{
		enableButton.GetComponent<HierarchyReferences>().GetReference("Button").transform.GetChild(0).gameObject.SetActive(!disableDataCollection);
		closeButton.isInteractable = !IsSettingsDirty();
		restartWarningText.gameObject.SetActive(IsSettingsDirty());
		if (IsSettingsDirty())
		{
			dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.RESTART_BUTTON;
		}
		else
		{
			dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.DONE_BUTTON;
		}
	}
}
