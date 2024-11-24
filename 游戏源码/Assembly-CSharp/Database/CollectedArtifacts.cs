using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002188 RID: 8584
	public class CollectedArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B673 RID: 46707 RVA: 0x00459F14 File Offset: 0x00458114
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x0600B674 RID: 46708 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B675 RID: 46709 RVA: 0x00115A0E File Offset: 0x00113C0E
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount >= 10;
		}

		// Token: 0x0600B676 RID: 46710 RVA: 0x00115A21 File Offset: 0x00113C21
		private int GetStudiedArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount;
		}

		// Token: 0x0600B677 RID: 46711 RVA: 0x00459F58 File Offset: 0x00458158
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x040094F9 RID: 38137
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
