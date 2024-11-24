using System;
using System.Collections.Generic;

// Token: 0x0200157D RID: 5501
public class ExposureType
{
	// Token: 0x04005576 RID: 21878
	public string germ_id;

	// Token: 0x04005577 RID: 21879
	public string sickness_id;

	// Token: 0x04005578 RID: 21880
	public string infection_effect;

	// Token: 0x04005579 RID: 21881
	public int exposure_threshold;

	// Token: 0x0400557A RID: 21882
	public bool infect_immediately;

	// Token: 0x0400557B RID: 21883
	public List<string> required_traits;

	// Token: 0x0400557C RID: 21884
	public List<string> excluded_traits;

	// Token: 0x0400557D RID: 21885
	public List<string> excluded_effects;

	// Token: 0x0400557E RID: 21886
	public int base_resistance;
}
