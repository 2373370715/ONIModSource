using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003BA1 RID: 15265
	public struct ElemExposureInfo
	{
		// Token: 0x0600EB38 RID: 60216 RVA: 0x0013D425 File Offset: 0x0013B625
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.populationHalfLife);
		}

		// Token: 0x0600EB39 RID: 60217 RVA: 0x004CCBB0 File Offset: 0x004CADB0
		public static void SetBulk(ElemExposureInfo[] info, Func<Element, bool> test, ElemExposureInfo settings)
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

		// Token: 0x0600EB3A RID: 60218 RVA: 0x0013D433 File Offset: 0x0013B633
		public float CalculateExposureDiseaseCountDelta(int disease_count, float dt)
		{
			return (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
		}

		// Token: 0x0400E668 RID: 58984
		public float populationHalfLife;
	}
}
