using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02002006 RID: 8198
[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour
{
	// Token: 0x0600AE4F RID: 44623 RVA: 0x001116D2 File Offset: 0x0010F8D2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
	}

	// Token: 0x0600AE50 RID: 44624 RVA: 0x00418A64 File Offset: 0x00416C64
	public void UpdateSliderLabel(float newValue)
	{
		if (this.isPercentValue)
		{
			this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
			return;
		}
		this.valueLabel.text = newValue.ToString();
	}

	// Token: 0x0400890C RID: 35084
	public bool isPercentValue = true;

	// Token: 0x0400890D RID: 35085
	public KSlider slider;

	// Token: 0x0400890E RID: 35086
	public LocText nameLabel;

	// Token: 0x0400890F RID: 35087
	public LocText valueLabel;
}
