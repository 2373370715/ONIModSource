using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003BA4 RID: 15268
	public class CompositeExposureRule
	{
		// Token: 0x0600EB42 RID: 60226 RVA: 0x0013D47B File Offset: 0x0013B67B
		public string Name()
		{
			return this.name;
		}

		// Token: 0x0600EB43 RID: 60227 RVA: 0x0013D483 File Offset: 0x0013B683
		public void Overlay(ExposureRule rule)
		{
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x0600EB44 RID: 60228 RVA: 0x0013D4B0 File Offset: 0x0013B6B0
		public float GetHalfLifeForCount(int count)
		{
			return this.populationHalfLife;
		}

		// Token: 0x0400E66B RID: 58987
		public string name;

		// Token: 0x0400E66C RID: 58988
		public float populationHalfLife;
	}
}
