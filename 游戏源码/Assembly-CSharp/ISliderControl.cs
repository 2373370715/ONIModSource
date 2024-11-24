using System;

// Token: 0x02001FD8 RID: 8152
public interface ISliderControl
{
	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x0600ACB7 RID: 44215
	string SliderTitleKey { get; }

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x0600ACB8 RID: 44216
	string SliderUnits { get; }

	// Token: 0x0600ACB9 RID: 44217
	int SliderDecimalPlaces(int index);

	// Token: 0x0600ACBA RID: 44218
	float GetSliderMin(int index);

	// Token: 0x0600ACBB RID: 44219
	float GetSliderMax(int index);

	// Token: 0x0600ACBC RID: 44220
	float GetSliderValue(int index);

	// Token: 0x0600ACBD RID: 44221
	void SetSliderValue(float percent, int index);

	// Token: 0x0600ACBE RID: 44222
	string GetSliderTooltipKey(int index);

	// Token: 0x0600ACBF RID: 44223
	string GetSliderTooltip(int index);
}
