using System;
using UnityEngine;

// Token: 0x02002069 RID: 8297
[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
	// Token: 0x0600B08B RID: 45195 RVA: 0x004254D4 File Offset: 0x004236D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClick));
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace() && KPlayerPrefs.GetInt(this.saveStatePreferenceKey, 1) != 1 && this.toggle.CurrentState == 0)
		{
			this.OnClick();
		}
	}

	// Token: 0x0600B08C RID: 45196 RVA: 0x00425540 File Offset: 0x00423740
	private void OnClick()
	{
		this.toggle.NextState();
		this.content.SetActive(this.toggle.CurrentState == 0);
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace())
		{
			KPlayerPrefs.SetInt(this.saveStatePreferenceKey, (this.toggle.CurrentState == 0) ? 1 : 0);
		}
	}

	// Token: 0x04008B82 RID: 35714
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x04008B83 RID: 35715
	[SerializeField]
	public GameObject content;

	// Token: 0x04008B84 RID: 35716
	[SerializeField]
	private string saveStatePreferenceKey;

	// Token: 0x04008B85 RID: 35717
	private const int onState = 0;
}
