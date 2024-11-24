using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021A2 RID: 8610
	public class UpgradeAllBasicBuildings : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6EB RID: 46827 RVA: 0x00115D8B File Offset: 0x00113F8B
		public UpgradeAllBasicBuildings(Tag basicBuilding, Tag upgradeBuilding)
		{
			this.basicBuilding = basicBuilding;
			this.upgradeBuilding = upgradeBuilding;
		}

		// Token: 0x0600B6EC RID: 46828 RVA: 0x0045B414 File Offset: 0x00459614
		public override bool Success()
		{
			bool result = false;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (component.HasTag(this.basicBuilding))
				{
					return false;
				}
				if (component.HasTag(this.upgradeBuilding))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600B6ED RID: 46829 RVA: 0x0045B498 File Offset: 0x00459698
		public void Deserialize(IReader reader)
		{
			string name = reader.ReadKleiString();
			this.basicBuilding = new Tag(name);
			string name2 = reader.ReadKleiString();
			this.upgradeBuilding = new Tag(name2);
		}

		// Token: 0x0600B6EE RID: 46830 RVA: 0x0045B4CC File Offset: 0x004596CC
		public override string GetProgress(bool complete)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(this.basicBuilding.Name);
			BuildingDef buildingDef2 = Assets.GetBuildingDef(this.upgradeBuilding.Name);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.UPGRADE_ALL_BUILDINGS, buildingDef.Name, buildingDef2.Name);
		}

		// Token: 0x04009516 RID: 38166
		private Tag basicBuilding;

		// Token: 0x04009517 RID: 38167
		private Tag upgradeBuilding;
	}
}
