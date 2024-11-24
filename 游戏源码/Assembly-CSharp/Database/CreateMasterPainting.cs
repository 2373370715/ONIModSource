using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021A4 RID: 8612
	public class CreateMasterPainting : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6F3 RID: 46835 RVA: 0x0045B6BC File Offset: 0x004598BC
		public override bool Success()
		{
			foreach (Painting painting in Components.Paintings.Items)
			{
				if (painting != null)
				{
					ArtableStage artableStage = Db.GetArtableStages().TryGet(painting.CurrentStage);
					if (artableStage != null && artableStage.statusItem == Db.Get().ArtableStatuses.LookingGreat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B6F4 RID: 46836 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B6F5 RID: 46837 RVA: 0x00115DAD File Offset: 0x00113FAD
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
		}
	}
}
