using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002189 RID: 8585
	public class CollectedSpaceArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B679 RID: 46713 RVA: 0x00459F84 File Offset: 0x00458184
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_SPACE_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedSpaceArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x0600B67A RID: 46714 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B67B RID: 46715 RVA: 0x00115A2D File Offset: 0x00113C2D
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount >= 10;
		}

		// Token: 0x0600B67C RID: 46716 RVA: 0x00115A40 File Offset: 0x00113C40
		private int GetStudiedSpaceArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount;
		}

		// Token: 0x0600B67D RID: 46717 RVA: 0x00459FC8 File Offset: 0x004581C8
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_SPACE_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x040094FA RID: 38138
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
