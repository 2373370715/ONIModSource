using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021A5 RID: 8613
	public class ActivateLorePOI : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6F7 RID: 46839 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B6F8 RID: 46840 RVA: 0x0045B748 File Offset: 0x00459948
		public override bool Success()
		{
			foreach (BuildingComplete buildingComplete in Components.TemplateBuildings.Items)
			{
				if (!(buildingComplete == null))
				{
					Unsealable component = buildingComplete.GetComponent<Unsealable>();
					if (component != null && component.unsealed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B6F9 RID: 46841 RVA: 0x00115DB9 File Offset: 0x00113FB9
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
		}
	}
}
