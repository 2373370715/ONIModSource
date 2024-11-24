using System;
using System.Collections.Generic;
using FMODUnity;

namespace Database;

public class ColonyAchievement : Resource
{
	public string description;

	public bool isVictoryCondition;

	public string messageTitle;

	public string messageBody;

	public string shortVideoName;

	public string loopVideoName;

	public string platformAchievementId;

	public string icon;

	public string clusterTag;

	public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();

	public Action<KMonoBehaviour> victorySequence;

	public string[] dlcIds;

	public string dlcIdFrom;

	public EventReference victoryNISSnapshot { get; private set; }

	public ColonyAchievement(string Id, string platformAchievementId, string Name, string description, bool isVictoryCondition, List<ColonyAchievementRequirement> requirementChecklist, string messageTitle = "", string messageBody = "", string videoDataName = "", string victoryLoopVideo = "", Action<KMonoBehaviour> VictorySequence = null, EventReference victorySnapshot = default(EventReference), string icon = "", string[] dlcIds = null, string dlcIdFrom = null, string clusterTag = null)
		: base(Id, Name)
	{
		base.Id = Id;
		this.platformAchievementId = platformAchievementId;
		base.Name = Name;
		this.description = description;
		this.isVictoryCondition = isVictoryCondition;
		this.requirementChecklist = requirementChecklist;
		this.messageTitle = messageTitle;
		this.messageBody = messageBody;
		shortVideoName = videoDataName;
		loopVideoName = victoryLoopVideo;
		victorySequence = VictorySequence;
		victoryNISSnapshot = (victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot);
		this.icon = icon;
		this.clusterTag = clusterTag;
		this.dlcIds = dlcIds;
		if (this.dlcIds == null)
		{
			this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
		}
		this.dlcIdFrom = dlcIdFrom;
	}

	public bool IsValidForSave()
	{
		if (clusterTag.IsNullOrWhiteSpace())
		{
			return true;
		}
		DebugUtil.Assert(CustomGameSettings.Instance != null, "IsValidForSave called when CustomGamesSettings is not initialized.");
		return CustomGameSettings.Instance.GetCurrentClusterLayout()?.clusterTags.Contains(clusterTag) ?? false;
	}
}
