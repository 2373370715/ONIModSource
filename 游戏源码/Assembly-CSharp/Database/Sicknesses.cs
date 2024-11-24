using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02002164 RID: 8548
	public class Sicknesses : ResourceSet<Sickness>
	{
		// Token: 0x0600B5E5 RID: 46565 RVA: 0x00455188 File Offset: 0x00453388
		public Sicknesses(ResourceSet parent) : base("Sicknesses", parent)
		{
			this.FoodSickness = base.Add(new FoodSickness());
			this.SlimeSickness = base.Add(new SlimeSickness());
			this.ZombieSickness = base.Add(new ZombieSickness());
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationSickness = base.Add(new RadiationSickness());
			}
			this.Allergies = base.Add(new Allergies());
			this.Sunburn = base.Add(new Sunburn());
		}

		// Token: 0x0600B5E6 RID: 46566 RVA: 0x00455210 File Offset: 0x00453410
		public static bool IsValidID(string id)
		{
			bool result = false;
			using (List<Sickness>.Enumerator enumerator = Db.Get().Sicknesses.resources.GetEnumerator())
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

		// Token: 0x04009422 RID: 37922
		public Sickness FoodSickness;

		// Token: 0x04009423 RID: 37923
		public Sickness SlimeSickness;

		// Token: 0x04009424 RID: 37924
		public Sickness ZombieSickness;

		// Token: 0x04009425 RID: 37925
		public Sickness Allergies;

		// Token: 0x04009426 RID: 37926
		public Sickness RadiationSickness;

		// Token: 0x04009427 RID: 37927
		public Sickness Sunburn;
	}
}
