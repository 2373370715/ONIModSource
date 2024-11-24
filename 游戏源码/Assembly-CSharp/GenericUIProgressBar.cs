using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CE1 RID: 7393
[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
	// Token: 0x06009A60 RID: 39520 RVA: 0x001046E6 File Offset: 0x001028E6
	public void SetMaxValue(float max)
	{
		this.maxValue = max;
	}

	// Token: 0x06009A61 RID: 39521 RVA: 0x003B8F10 File Offset: 0x003B7110
	public void SetFillPercentage(float value)
	{
		this.fill.fillAmount = value;
		this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
	}

	// Token: 0x0400787B RID: 30843
	public Image fill;

	// Token: 0x0400787C RID: 30844
	public LocText label;

	// Token: 0x0400787D RID: 30845
	private float maxValue;
}
