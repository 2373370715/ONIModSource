using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D80 RID: 7552
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicRibbonDisplayUI : KMonoBehaviour
{
	// Token: 0x06009DCF RID: 40399 RVA: 0x003C87B8 File Offset: 0x003C69B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.wire1.raycastTarget = false;
		this.wire2.raycastTarget = false;
		this.wire3.raycastTarget = false;
		this.wire4.raycastTarget = false;
	}

	// Token: 0x06009DD0 RID: 40400 RVA: 0x003C8844 File Offset: 0x003C6A44
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 color = this.colourDisconnected;
		List<Color32> list = new List<Color32>();
		for (int i = 0; i < this.bitDepth; i++)
		{
			list.Add((network == null) ? color : (network.IsBitActive(i) ? this.colourOn : this.colourOff));
		}
		if (this.wire1.color != list[0])
		{
			this.wire1.color = list[0];
		}
		if (this.wire2.color != list[1])
		{
			this.wire2.color = list[1];
		}
		if (this.wire3.color != list[2])
		{
			this.wire3.color = list[2];
		}
		if (this.wire4.color != list[3])
		{
			this.wire4.color = list[3];
		}
	}

	// Token: 0x04007BA3 RID: 31651
	[SerializeField]
	private Image wire1;

	// Token: 0x04007BA4 RID: 31652
	[SerializeField]
	private Image wire2;

	// Token: 0x04007BA5 RID: 31653
	[SerializeField]
	private Image wire3;

	// Token: 0x04007BA6 RID: 31654
	[SerializeField]
	private Image wire4;

	// Token: 0x04007BA7 RID: 31655
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04007BA8 RID: 31656
	private Color32 colourOn;

	// Token: 0x04007BA9 RID: 31657
	private Color32 colourOff;

	// Token: 0x04007BAA RID: 31658
	private Color32 colourDisconnected = new Color(255f, 255f, 255f, 255f);

	// Token: 0x04007BAB RID: 31659
	private int bitDepth = 4;
}
