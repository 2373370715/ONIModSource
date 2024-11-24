using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Database;

// Token: 0x020010A6 RID: 4262
public class ColonyAchievementStatus
{
	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x0600576E RID: 22382 RVA: 0x000D90EA File Offset: 0x000D72EA
	public List<ColonyAchievementRequirement> Requirements
	{
		get
		{
			return this.m_achievement.requirementChecklist;
		}
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x002861AC File Offset: 0x002843AC
	public ColonyAchievementStatus(string achievementId)
	{
		this.m_achievement = Db.Get().ColonyAchievements.TryGet(achievementId);
		if (this.m_achievement == null)
		{
			this.m_achievement = new ColonyAchievement();
			return;
		}
		if (!this.m_achievement.IsValidForSave())
		{
			this.m_achievement.Disabled = true;
		}
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x00286204 File Offset: 0x00284404
	public void UpdateAchievement()
	{
		if (this.Requirements.Count <= 0)
		{
			return;
		}
		if (this.m_achievement.Disabled)
		{
			return;
		}
		this.success = true;
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in this.Requirements)
		{
			this.success &= colonyAchievementRequirement.Success();
			this.failed |= colonyAchievementRequirement.Fail();
		}
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x0028629C File Offset: 0x0028449C
	public static ColonyAchievementStatus Deserialize(IReader reader, string achievementId)
	{
		bool flag = reader.ReadByte() > 0;
		bool flag2 = reader.ReadByte() > 0;
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 22))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Type type = Type.GetType(reader.ReadKleiString());
				if (type != null)
				{
					AchievementRequirementSerialization_Deprecated achievementRequirementSerialization_Deprecated = FormatterServices.GetUninitializedObject(type) as AchievementRequirementSerialization_Deprecated;
					Debug.Assert(achievementRequirementSerialization_Deprecated != null, string.Format("Cannot deserialize old data for type {0}", type));
					achievementRequirementSerialization_Deprecated.Deserialize(reader);
				}
			}
		}
		return new ColonyAchievementStatus(achievementId)
		{
			success = flag,
			failed = flag2
		};
	}

	// Token: 0x06005772 RID: 22386 RVA: 0x000D90F7 File Offset: 0x000D72F7
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.success ? 1 : 0);
		writer.Write(this.failed ? 1 : 0);
	}

	// Token: 0x04003D08 RID: 15624
	public bool success;

	// Token: 0x04003D09 RID: 15625
	public bool failed;

	// Token: 0x04003D0A RID: 15626
	private ColonyAchievement m_achievement;
}
