using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003BA2 RID: 15266
	public class ExposureRule
	{
		// Token: 0x0600EB3B RID: 60219 RVA: 0x004CCBEC File Offset: 0x004CADEC
		public void Apply(ElemExposureInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (this.Test(elements[i]))
				{
					ElemExposureInfo elemExposureInfo = infoList[i];
					if (this.populationHalfLife != null)
					{
						elemExposureInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					infoList[i] = elemExposureInfo;
				}
			}
		}

		// Token: 0x0600EB3C RID: 60220 RVA: 0x000A65EC File Offset: 0x000A47EC
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x0600EB3D RID: 60221 RVA: 0x000AD332 File Offset: 0x000AB532
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x0400E669 RID: 58985
		public float? populationHalfLife;
	}
}
