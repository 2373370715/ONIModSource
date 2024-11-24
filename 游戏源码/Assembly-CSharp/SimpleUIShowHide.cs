using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
	[MyCmpReq]
	private MultiToggle toggle;

	[SerializeField]
	public GameObject content;

	[SerializeField]
	private string saveStatePreferenceKey;

	private const int onState = 0;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(OnClick));
		if (!saveStatePreferenceKey.IsNullOrWhiteSpace() && KPlayerPrefs.GetInt(saveStatePreferenceKey, 1) != 1 && toggle.CurrentState == 0)
		{
			OnClick();
		}
	}

	private void OnClick()
	{
		toggle.NextState();
		content.SetActive(toggle.CurrentState == 0);
		if (!saveStatePreferenceKey.IsNullOrWhiteSpace())
		{
			KPlayerPrefs.SetInt(saveStatePreferenceKey, (toggle.CurrentState == 0) ? 1 : 0);
		}
	}
}
