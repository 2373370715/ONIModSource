using UnityEngine;

public class DLCBetaMessageScreen : KModalScreen
{
	public RectTransform logo;

	public KButton confirmButton;

	public KButton quitButton;

	public LocText bodyText;

	public RectTransform messageContainer;

	private bool betaIsLive;

	private bool skipInEditor;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		confirmButton.onClick += delegate
		{
			base.gameObject.SetActive(value: false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
		};
		quitButton.onClick += delegate
		{
			App.Quit();
		};
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!betaIsLive || (Application.isEditor && skipInEditor) || !DlcManager.GetActiveDLCIds().Contains("DLC2_ID"))
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
		}
	}

	private void Update()
	{
		logo.rectTransform().localPosition = new Vector3(0f, Mathf.Sin(Time.realtimeSinceStartup) * 7.5f);
	}
}
