using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002199 RID: 8601
	public class NoFarmables : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6C5 RID: 46789 RVA: 0x0045AA30 File Offset: 0x00458C30
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(worldContainer.id))
				{
					if (plantablePlot.Occupant != null)
					{
						using (IEnumerator<Tag> enumerator3 = plantablePlot.possibleDepositObjectTags.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current != GameTags.DecorSeed)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600B6C6 RID: 46790 RVA: 0x001158A9 File Offset: 0x00113AA9
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600B6C7 RID: 46791 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B6C8 RID: 46792 RVA: 0x00115C66 File Offset: 0x00113E66
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_FARM_TILES;
		}
	}
}
