using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002045 RID: 8261
public class ValueTrendImageToggle : MonoBehaviour
{
	// Token: 0x0600AFDD RID: 45021 RVA: 0x0042246C File Offset: 0x0042066C
	public void SetValue(AmountInstance ainstance)
	{
		float delta = ainstance.GetDelta();
		Sprite sprite = null;
		if (ainstance.paused || delta == 0f)
		{
			this.targetImage.gameObject.SetActive(false);
		}
		else
		{
			this.targetImage.gameObject.SetActive(true);
			if (delta <= -ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Down_Three;
			}
			else if (delta <= -ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Down_Two;
			}
			else if (delta <= 0f)
			{
				sprite = this.Down_One;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Up_Three;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Up_Two;
			}
			else if (delta > 0f)
			{
				sprite = this.Up_One;
			}
		}
		this.targetImage.sprite = sprite;
	}

	// Token: 0x04008A94 RID: 35476
	public Image targetImage;

	// Token: 0x04008A95 RID: 35477
	public Sprite Up_One;

	// Token: 0x04008A96 RID: 35478
	public Sprite Up_Two;

	// Token: 0x04008A97 RID: 35479
	public Sprite Up_Three;

	// Token: 0x04008A98 RID: 35480
	public Sprite Down_One;

	// Token: 0x04008A99 RID: 35481
	public Sprite Down_Two;

	// Token: 0x04008A9A RID: 35482
	public Sprite Down_Three;

	// Token: 0x04008A9B RID: 35483
	public Sprite Zero;
}
