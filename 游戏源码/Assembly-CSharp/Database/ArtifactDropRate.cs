using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002113 RID: 8467
	public class ArtifactDropRate : Resource
	{
		// Token: 0x0600B3F0 RID: 46064 RVA: 0x00114AE9 File Offset: 0x00112CE9
		public void AddItem(ArtifactTier tier, float weight)
		{
			this.rates.Add(new global::Tuple<ArtifactTier, float>(tier, weight));
			this.totalWeight += weight;
		}

		// Token: 0x0600B3F1 RID: 46065 RVA: 0x0043C3E4 File Offset: 0x0043A5E4
		public float GetTierWeight(ArtifactTier tier)
		{
			float result = 0f;
			foreach (global::Tuple<ArtifactTier, float> tuple in this.rates)
			{
				if (tuple.first == tier)
				{
					result = tuple.second;
				}
			}
			return result;
		}

		// Token: 0x04008E29 RID: 36393
		public List<global::Tuple<ArtifactTier, float>> rates = new List<global::Tuple<ArtifactTier, float>>();

		// Token: 0x04008E2A RID: 36394
		public float totalWeight;
	}
}
