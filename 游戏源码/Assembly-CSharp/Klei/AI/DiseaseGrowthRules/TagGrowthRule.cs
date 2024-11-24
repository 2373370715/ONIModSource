using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003B9F RID: 15263
	public class TagGrowthRule : GrowthRule
	{
		// Token: 0x0600EB31 RID: 60209 RVA: 0x0013D3F3 File Offset: 0x0013B5F3
		public TagGrowthRule(Tag tag)
		{
			this.tag = tag;
		}

		// Token: 0x0600EB32 RID: 60210 RVA: 0x0013D402 File Offset: 0x0013B602
		public override bool Test(Element e)
		{
			return e.HasTag(this.tag);
		}

		// Token: 0x0600EB33 RID: 60211 RVA: 0x0013D410 File Offset: 0x0013B610
		public override string Name()
		{
			return this.tag.ProperName();
		}

		// Token: 0x0400E65E RID: 58974
		public Tag tag;
	}
}
