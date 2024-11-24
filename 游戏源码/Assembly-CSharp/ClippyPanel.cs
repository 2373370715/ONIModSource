using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002052 RID: 8274
public class ClippyPanel : KScreen
{
	// Token: 0x0600B031 RID: 45105 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600B032 RID: 45106 RVA: 0x00112832 File Offset: 0x00110A32
	protected override void OnActivate()
	{
		base.OnActivate();
		SpeedControlScreen.Instance.Pause(true, false);
		Game.Instance.Trigger(1634669191, null);
	}

	// Token: 0x0600B033 RID: 45107 RVA: 0x00112856 File Offset: 0x00110A56
	public void OnOk()
	{
		SpeedControlScreen.Instance.Unpause(true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04008AEC RID: 35564
	public Text title;

	// Token: 0x04008AED RID: 35565
	public Text detailText;

	// Token: 0x04008AEE RID: 35566
	public Text flavorText;

	// Token: 0x04008AEF RID: 35567
	public Image topicIcon;

	// Token: 0x04008AF0 RID: 35568
	private KButton okButton;
}
