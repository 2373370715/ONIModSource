using System;
using System.Collections.Generic;
using FMODUnity;
using ProcGen;

namespace Database
{
	// Token: 0x020021BB RID: 8635
	public class ColonyAchievement : Resource
	{
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x0600B741 RID: 46913 RVA: 0x001160F5 File Offset: 0x001142F5
		// (set) Token: 0x0600B742 RID: 46914 RVA: 0x001160FD File Offset: 0x001142FD
		public EventReference victoryNISSnapshot { get; private set; }

		// Token: 0x0600B743 RID: 46915 RVA: 0x0045DCE0 File Offset: 0x0045BEE0
		public ColonyAchievement()
		{
			this.Id = "Disabled";
			this.platformAchievementId = "Disabled";
			this.Name = "Disabled";
			this.description = "Disabled";
			this.isVictoryCondition = false;
			this.requirementChecklist = new List<ColonyAchievementRequirement>();
			this.messageTitle = string.Empty;
			this.messageBody = string.Empty;
			this.shortVideoName = string.Empty;
			this.loopVideoName = string.Empty;
			this.platformAchievementId = string.Empty;
			this.icon = string.Empty;
			this.clusterTag = string.Empty;
			this.Disabled = true;
		}

		// Token: 0x0600B744 RID: 46916 RVA: 0x0045DD90 File Offset: 0x0045BF90
		public ColonyAchievement(string Id, string platformAchievementId, string Name, string description, bool isVictoryCondition, List<ColonyAchievementRequirement> requirementChecklist, string messageTitle = "", string messageBody = "", string videoDataName = "", string victoryLoopVideo = "", Action<KMonoBehaviour> VictorySequence = null, EventReference victorySnapshot = default(EventReference), string icon = "", string[] dlcIds = null, string dlcIdFrom = null, string clusterTag = null) : base(Id, Name)
		{
			this.Id = Id;
			this.platformAchievementId = platformAchievementId;
			this.Name = Name;
			this.description = description;
			this.isVictoryCondition = isVictoryCondition;
			this.requirementChecklist = requirementChecklist;
			this.messageTitle = messageTitle;
			this.messageBody = messageBody;
			this.shortVideoName = videoDataName;
			this.loopVideoName = victoryLoopVideo;
			this.victorySequence = VictorySequence;
			this.victoryNISSnapshot = (victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot);
			this.icon = icon;
			this.clusterTag = clusterTag;
			this.dlcIds = dlcIds;
			if (this.dlcIds == null)
			{
				this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			}
			this.dlcIdFrom = dlcIdFrom;
		}

		// Token: 0x0600B745 RID: 46917 RVA: 0x0045DE58 File Offset: 0x0045C058
		public bool IsValidForSave()
		{
			if (this.clusterTag.IsNullOrWhiteSpace())
			{
				return true;
			}
			DebugUtil.Assert(CustomGameSettings.Instance != null, "IsValidForSave called when CustomGamesSettings is not initialized.");
			ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(this.clusterTag);
		}

		// Token: 0x04009559 RID: 38233
		public string description;

		// Token: 0x0400955A RID: 38234
		public bool isVictoryCondition;

		// Token: 0x0400955B RID: 38235
		public string messageTitle;

		// Token: 0x0400955C RID: 38236
		public string messageBody;

		// Token: 0x0400955D RID: 38237
		public string shortVideoName;

		// Token: 0x0400955E RID: 38238
		public string loopVideoName;

		// Token: 0x0400955F RID: 38239
		public string platformAchievementId;

		// Token: 0x04009560 RID: 38240
		public string icon;

		// Token: 0x04009561 RID: 38241
		public string clusterTag;

		// Token: 0x04009562 RID: 38242
		public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();

		// Token: 0x04009563 RID: 38243
		public Action<KMonoBehaviour> victorySequence;

		// Token: 0x04009565 RID: 38245
		public string[] dlcIds;

		// Token: 0x04009566 RID: 38246
		public string dlcIdFrom;
	}
}
