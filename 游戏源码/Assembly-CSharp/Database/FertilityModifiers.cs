using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02002106 RID: 8454
	public class FertilityModifiers : ResourceSet<FertilityModifier>
	{
		// Token: 0x0600B3D2 RID: 46034 RVA: 0x0043AEA0 File Offset: 0x004390A0
		public List<FertilityModifier> GetForTag(Tag searchTag)
		{
			List<FertilityModifier> list = new List<FertilityModifier>();
			foreach (FertilityModifier fertilityModifier in this.resources)
			{
				if (fertilityModifier.TargetTag == searchTag)
				{
					list.Add(fertilityModifier);
				}
			}
			return list;
		}
	}
}
