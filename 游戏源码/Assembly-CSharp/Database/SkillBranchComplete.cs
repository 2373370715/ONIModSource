using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002194 RID: 8596
	public class SkillBranchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6B0 RID: 46768 RVA: 0x00115BDD File Offset: 0x00113DDD
		public SkillBranchComplete(List<Skill> skillsToMaster)
		{
			this.skillsToMaster = skillsToMaster;
		}

		// Token: 0x0600B6B1 RID: 46769 RVA: 0x0045A300 File Offset: 0x00458500
		public override bool Success()
		{
			foreach (MinionResume minionResume in Components.MinionResumes.Items)
			{
				foreach (Skill skill in this.skillsToMaster)
				{
					if (minionResume.HasMasteredSkill(skill.Id))
					{
						if (!minionResume.HasBeenGrantedSkill(skill))
						{
							return true;
						}
						List<Skill> allPriorSkills = Db.Get().Skills.GetAllPriorSkills(skill);
						bool flag = true;
						foreach (Skill skill2 in allPriorSkills)
						{
							flag = (flag && minionResume.HasMasteredSkill(skill2.Id));
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B6B2 RID: 46770 RVA: 0x0045A420 File Offset: 0x00458620
		public void Deserialize(IReader reader)
		{
			this.skillsToMaster = new List<Skill>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string id = reader.ReadKleiString();
				this.skillsToMaster.Add(Db.Get().Skills.Get(id));
			}
		}

		// Token: 0x0600B6B3 RID: 46771 RVA: 0x00115BEC File Offset: 0x00113DEC
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
		}

		// Token: 0x04009500 RID: 38144
		private List<Skill> skillsToMaster;
	}
}
