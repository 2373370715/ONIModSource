using System;

// Token: 0x02001F90 RID: 8080
public interface IMultiSliderControl
{
	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x0600AA94 RID: 43668
	string SidescreenTitleKey { get; }

	// Token: 0x0600AA95 RID: 43669
	bool SidescreenEnabled();

	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x0600AA96 RID: 43670
	ISliderControl[] sliderControls { get; }
}
