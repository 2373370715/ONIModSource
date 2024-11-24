using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003B9B RID: 15259
	public struct ElemGrowthInfo
	{
		// Token: 0x0600EB24 RID: 60196 RVA: 0x004CC7EC File Offset: 0x004CA9EC
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.underPopulationDeathRate);
			writer.Write(this.populationHalfLife);
			writer.Write(this.overPopulationHalfLife);
			writer.Write(this.diffusionScale);
			writer.Write(this.minCountPerKG);
			writer.Write(this.maxCountPerKG);
			writer.Write(this.minDiffusionCount);
			writer.Write(this.minDiffusionInfestationTickCount);
		}

		// Token: 0x0600EB25 RID: 60197 RVA: 0x004CC85C File Offset: 0x004CAA5C
		public static void SetBulk(ElemGrowthInfo[] info, Func<Element, bool> test, ElemGrowthInfo settings)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (test(elements[i]))
				{
					info[i] = settings;
				}
			}
		}

		// Token: 0x0600EB26 RID: 60198 RVA: 0x004CC898 File Offset: 0x004CAA98
		public float CalculateDiseaseCountDelta(int disease_count, float kg, float dt)
		{
			float num = this.minCountPerKG * kg;
			float num2 = this.maxCountPerKG * kg;
			float result;
			if (num <= (float)disease_count && (float)disease_count <= num2)
			{
				result = (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
			}
			else if ((float)disease_count < num)
			{
				result = -this.underPopulationDeathRate * dt;
			}
			else
			{
				result = (Disease.HalfLifeToGrowthRate(this.overPopulationHalfLife, dt) - 1f) * (float)disease_count;
			}
			return result;
		}

		// Token: 0x0400E64C RID: 58956
		public float underPopulationDeathRate;

		// Token: 0x0400E64D RID: 58957
		public float populationHalfLife;

		// Token: 0x0400E64E RID: 58958
		public float overPopulationHalfLife;

		// Token: 0x0400E64F RID: 58959
		public float diffusionScale;

		// Token: 0x0400E650 RID: 58960
		public float minCountPerKG;

		// Token: 0x0400E651 RID: 58961
		public float maxCountPerKG;

		// Token: 0x0400E652 RID: 58962
		public int minDiffusionCount;

		// Token: 0x0400E653 RID: 58963
		public byte minDiffusionInfestationTickCount;
	}
}
