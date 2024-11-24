using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003BA0 RID: 15264
	public class CompositeGrowthRule
	{
		// Token: 0x0600EB34 RID: 60212 RVA: 0x0013D41D File Offset: 0x0013B61D
		public string Name()
		{
			return this.name;
		}

		// Token: 0x0600EB35 RID: 60213 RVA: 0x004CCA60 File Offset: 0x004CAC60
		public void Overlay(GrowthRule rule)
		{
			if (rule.underPopulationDeathRate != null)
			{
				this.underPopulationDeathRate = rule.underPopulationDeathRate.Value;
			}
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			if (rule.overPopulationHalfLife != null)
			{
				this.overPopulationHalfLife = rule.overPopulationHalfLife.Value;
			}
			if (rule.diffusionScale != null)
			{
				this.diffusionScale = rule.diffusionScale.Value;
			}
			if (rule.minCountPerKG != null)
			{
				this.minCountPerKG = rule.minCountPerKG.Value;
			}
			if (rule.maxCountPerKG != null)
			{
				this.maxCountPerKG = rule.maxCountPerKG.Value;
			}
			if (rule.minDiffusionCount != null)
			{
				this.minDiffusionCount = rule.minDiffusionCount.Value;
			}
			if (rule.minDiffusionInfestationTickCount != null)
			{
				this.minDiffusionInfestationTickCount = rule.minDiffusionInfestationTickCount.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x0600EB36 RID: 60214 RVA: 0x004CCB70 File Offset: 0x004CAD70
		public float GetHalfLifeForCount(int count, float kg)
		{
			int num = (int)(this.minCountPerKG * kg);
			int num2 = (int)(this.maxCountPerKG * kg);
			if (count < num)
			{
				return this.populationHalfLife;
			}
			if (count < num2)
			{
				return this.populationHalfLife;
			}
			return this.overPopulationHalfLife;
		}

		// Token: 0x0400E65F RID: 58975
		public string name;

		// Token: 0x0400E660 RID: 58976
		public float underPopulationDeathRate;

		// Token: 0x0400E661 RID: 58977
		public float populationHalfLife;

		// Token: 0x0400E662 RID: 58978
		public float overPopulationHalfLife;

		// Token: 0x0400E663 RID: 58979
		public float diffusionScale;

		// Token: 0x0400E664 RID: 58980
		public float minCountPerKG;

		// Token: 0x0400E665 RID: 58981
		public float maxCountPerKG;

		// Token: 0x0400E666 RID: 58982
		public int minDiffusionCount;

		// Token: 0x0400E667 RID: 58983
		public byte minDiffusionInfestationTickCount;
	}
}
