using System;

// Token: 0x02001F21 RID: 7969
public interface IActivationRangeTarget
{
	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x0600A80D RID: 43021
	// (set) Token: 0x0600A80E RID: 43022
	float ActivateValue { get; set; }

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x0600A80F RID: 43023
	// (set) Token: 0x0600A810 RID: 43024
	float DeactivateValue { get; set; }

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x0600A811 RID: 43025
	float MinValue { get; }

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x0600A812 RID: 43026
	float MaxValue { get; }

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x0600A813 RID: 43027
	bool UseWholeNumbers { get; }

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x0600A814 RID: 43028
	string ActivationRangeTitleText { get; }

	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x0600A815 RID: 43029
	string ActivateSliderLabelText { get; }

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x0600A816 RID: 43030
	string DeactivateSliderLabelText { get; }

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x0600A817 RID: 43031
	string ActivateTooltip { get; }

	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x0600A818 RID: 43032
	string DeactivateTooltip { get; }
}
