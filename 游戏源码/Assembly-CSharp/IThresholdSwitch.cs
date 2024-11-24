using System;

// Token: 0x02000FDE RID: 4062
public interface IThresholdSwitch
{
	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x0600527D RID: 21117
	// (set) Token: 0x0600527E RID: 21118
	float Threshold { get; set; }

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x0600527F RID: 21119
	// (set) Token: 0x06005280 RID: 21120
	bool ActivateAboveThreshold { get; set; }

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06005281 RID: 21121
	float CurrentValue { get; }

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06005282 RID: 21122
	float RangeMin { get; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06005283 RID: 21123
	float RangeMax { get; }

	// Token: 0x06005284 RID: 21124
	float GetRangeMinInputField();

	// Token: 0x06005285 RID: 21125
	float GetRangeMaxInputField();

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06005286 RID: 21126
	LocString Title { get; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06005287 RID: 21127
	LocString ThresholdValueName { get; }

	// Token: 0x06005288 RID: 21128
	LocString ThresholdValueUnits();

	// Token: 0x06005289 RID: 21129
	string Format(float value, bool units);

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x0600528A RID: 21130
	string AboveToolTip { get; }

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x0600528B RID: 21131
	string BelowToolTip { get; }

	// Token: 0x0600528C RID: 21132
	float ProcessedSliderValue(float input);

	// Token: 0x0600528D RID: 21133
	float ProcessedInputValue(float input);

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x0600528E RID: 21134
	ThresholdScreenLayoutType LayoutType { get; }

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x0600528F RID: 21135
	int IncrementScale { get; }

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06005290 RID: 21136
	NonLinearSlider.Range[] GetRanges { get; }
}
