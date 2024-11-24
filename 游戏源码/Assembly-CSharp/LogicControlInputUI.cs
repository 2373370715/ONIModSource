using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D7F RID: 7551
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicControlInputUI : KMonoBehaviour
{
	// Token: 0x06009DCC RID: 40396 RVA: 0x003C86E4 File Offset: 0x003C68E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.colourDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
		this.icon.raycastTarget = false;
		this.border.raycastTarget = false;
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x003C876C File Offset: 0x003C696C
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 c = (network == null) ? GlobalAssets.Instance.colorSet.logicDisconnected : (network.IsBitActive(0) ? this.colourOn : this.colourOff);
		this.icon.color = c;
	}

	// Token: 0x04007B9D RID: 31645
	[SerializeField]
	private Image icon;

	// Token: 0x04007B9E RID: 31646
	[SerializeField]
	private Image border;

	// Token: 0x04007B9F RID: 31647
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04007BA0 RID: 31648
	private Color32 colourOn;

	// Token: 0x04007BA1 RID: 31649
	private Color32 colourOff;

	// Token: 0x04007BA2 RID: 31650
	private Color32 colourDisconnected;
}
