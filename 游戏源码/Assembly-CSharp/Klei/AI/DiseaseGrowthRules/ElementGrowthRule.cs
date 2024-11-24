using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003B9E RID: 15262
	public class ElementGrowthRule : GrowthRule
	{
		// Token: 0x0600EB2E RID: 60206 RVA: 0x0013D3C2 File Offset: 0x0013B5C2
		public ElementGrowthRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x0600EB2F RID: 60207 RVA: 0x0013D3D1 File Offset: 0x0013B5D1
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x0600EB30 RID: 60208 RVA: 0x0013D3E1 File Offset: 0x0013B5E1
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x0400E65D RID: 58973
		public SimHashes element;
	}
}
