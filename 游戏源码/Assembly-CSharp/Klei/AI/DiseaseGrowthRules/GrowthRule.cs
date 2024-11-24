using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003B9C RID: 15260
	public class GrowthRule
	{
		// Token: 0x0600EB27 RID: 60199 RVA: 0x004CC904 File Offset: 0x004CAB04
		public void Apply(ElemGrowthInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				Element element = elements[i];
				if (element.id != SimHashes.Vacuum && this.Test(element))
				{
					ElemGrowthInfo elemGrowthInfo = infoList[i];
					if (this.underPopulationDeathRate != null)
					{
						elemGrowthInfo.underPopulationDeathRate = this.underPopulationDeathRate.Value;
					}
					if (this.populationHalfLife != null)
					{
						elemGrowthInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					if (this.overPopulationHalfLife != null)
					{
						elemGrowthInfo.overPopulationHalfLife = this.overPopulationHalfLife.Value;
					}
					if (this.diffusionScale != null)
					{
						elemGrowthInfo.diffusionScale = this.diffusionScale.Value;
					}
					if (this.minCountPerKG != null)
					{
						elemGrowthInfo.minCountPerKG = this.minCountPerKG.Value;
					}
					if (this.maxCountPerKG != null)
					{
						elemGrowthInfo.maxCountPerKG = this.maxCountPerKG.Value;
					}
					if (this.minDiffusionCount != null)
					{
						elemGrowthInfo.minDiffusionCount = this.minDiffusionCount.Value;
					}
					if (this.minDiffusionInfestationTickCount != null)
					{
						elemGrowthInfo.minDiffusionInfestationTickCount = this.minDiffusionInfestationTickCount.Value;
					}
					infoList[i] = elemGrowthInfo;
				}
			}
		}

		// Token: 0x0600EB28 RID: 60200 RVA: 0x000A65EC File Offset: 0x000A47EC
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x0600EB29 RID: 60201 RVA: 0x000AD332 File Offset: 0x000AB532
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x0400E654 RID: 58964
		public float? underPopulationDeathRate;

		// Token: 0x0400E655 RID: 58965
		public float? populationHalfLife;

		// Token: 0x0400E656 RID: 58966
		public float? overPopulationHalfLife;

		// Token: 0x0400E657 RID: 58967
		public float? diffusionScale;

		// Token: 0x0400E658 RID: 58968
		public float? minCountPerKG;

		// Token: 0x0400E659 RID: 58969
		public float? maxCountPerKG;

		// Token: 0x0400E65A RID: 58970
		public int? minDiffusionCount;

		// Token: 0x0400E65B RID: 58971
		public byte? minDiffusionInfestationTickCount;
	}
}
