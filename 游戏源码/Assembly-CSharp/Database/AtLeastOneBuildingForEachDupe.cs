using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021A1 RID: 8609
	public class AtLeastOneBuildingForEachDupe : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6E6 RID: 46822 RVA: 0x00115D71 File Offset: 0x00113F71
		public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes)
		{
			this.validBuildingTypes = validBuildingTypes;
		}

		// Token: 0x0600B6E7 RID: 46823 RVA: 0x0045B228 File Offset: 0x00459428
		public override bool Success()
		{
			if (Components.LiveMinionIdentities.Items.Count <= 0)
			{
				return false;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				Tag prefabTag = basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag;
				if (this.validBuildingTypes.Contains(prefabTag))
				{
					num++;
					if (prefabTag == "FlushToilet" || prefabTag == "Outhouse")
					{
						return true;
					}
				}
			}
			return num >= Components.LiveMinionIdentities.Items.Count;
		}

		// Token: 0x0600B6E8 RID: 46824 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public override bool Fail()
		{
			return false;
		}

		// Token: 0x0600B6E9 RID: 46825 RVA: 0x0045B2F0 File Offset: 0x004594F0
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.validBuildingTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.validBuildingTypes.Add(new Tag(name));
			}
		}

		// Token: 0x0600B6EA RID: 46826 RVA: 0x0045B334 File Offset: 0x00459534
		public override string GetProgress(bool complete)
		{
			if (this.validBuildingTypes.Contains("FlushToilet"))
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
			}
			if (complete)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (this.validBuildingTypes.Contains(component.PrefabTag))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, complete ? Components.LiveMinionIdentities.Items.Count : num, Components.LiveMinionIdentities.Items.Count);
		}

		// Token: 0x04009515 RID: 38165
		private List<Tag> validBuildingTypes = new List<Tag>();
	}
}
