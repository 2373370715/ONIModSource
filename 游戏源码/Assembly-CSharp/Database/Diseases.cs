using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02002132 RID: 8498
	public class Diseases : ResourceSet<Disease>
	{
		// Token: 0x0600B510 RID: 46352 RVA: 0x0044B390 File Offset: 0x00449590
		public Diseases(ResourceSet parent, bool statsOnly = false) : base("Diseases", parent)
		{
			this.FoodGerms = base.Add(new FoodGerms(statsOnly));
			this.SlimeGerms = base.Add(new SlimeGerms(statsOnly));
			this.PollenGerms = base.Add(new PollenGerms(statsOnly));
			this.ZombieSpores = base.Add(new ZombieSpores(statsOnly));
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationPoisoning = base.Add(new RadiationPoisoning(statsOnly));
			}
		}

		// Token: 0x0600B511 RID: 46353 RVA: 0x0044B40C File Offset: 0x0044960C
		public bool IsValidID(string id)
		{
			bool result = false;
			using (List<Disease>.Enumerator enumerator = this.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == id)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600B512 RID: 46354 RVA: 0x0044B46C File Offset: 0x0044966C
		public byte GetIndex(int hash)
		{
			byte b = 0;
			while ((int)b < this.resources.Count)
			{
				Disease disease = this.resources[(int)b];
				if (hash == disease.id.GetHashCode())
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x0600B513 RID: 46355 RVA: 0x00114F0C File Offset: 0x0011310C
		public byte GetIndex(HashedString id)
		{
			return this.GetIndex(id.GetHashCode());
		}

		// Token: 0x0400919B RID: 37275
		public Disease FoodGerms;

		// Token: 0x0400919C RID: 37276
		public Disease SlimeGerms;

		// Token: 0x0400919D RID: 37277
		public Disease PollenGerms;

		// Token: 0x0400919E RID: 37278
		public Disease ZombieSpores;

		// Token: 0x0400919F RID: 37279
		public Disease RadiationPoisoning;
	}
}
