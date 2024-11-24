using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003BA3 RID: 15267
	public class ElementExposureRule : ExposureRule
	{
		// Token: 0x0600EB3F RID: 60223 RVA: 0x0013D44A File Offset: 0x0013B64A
		public ElementExposureRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x0600EB40 RID: 60224 RVA: 0x0013D459 File Offset: 0x0013B659
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x0600EB41 RID: 60225 RVA: 0x0013D469 File Offset: 0x0013B669
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x0400E66A RID: 58986
		public SimHashes element;
	}
}
