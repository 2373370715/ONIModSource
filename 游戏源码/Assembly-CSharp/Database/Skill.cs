using System;
using System.Collections.Generic;
using TUNING;

namespace Database
{
	// Token: 0x020021C3 RID: 8643
	public class Skill : Resource
	{
		// Token: 0x0600B75D RID: 46941 RVA: 0x0045F4B0 File Offset: 0x0045D6B0
		public Skill(string id, string name, string description, string dlcId, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null) : base(id, name)
		{
			this.description = description;
			this.dlcId = dlcId;
			this.tier = tier;
			this.hat = hat;
			this.badge = badge;
			this.skillGroup = skillGroup;
			this.perks = perks;
			if (this.perks == null)
			{
				this.perks = new List<SkillPerk>();
			}
			this.priorSkills = priorSkills;
			if (this.priorSkills == null)
			{
				this.priorSkills = new List<string>();
			}
		}

		// Token: 0x0600B75E RID: 46942 RVA: 0x00116232 File Offset: 0x00114432
		public int GetMoraleExpectation()
		{
			return SKILLS.SKILL_TIER_MORALE_COST[this.tier];
		}

		// Token: 0x0600B75F RID: 46943 RVA: 0x00116240 File Offset: 0x00114440
		public bool GivesPerk(SkillPerk perk)
		{
			return this.perks.Contains(perk);
		}

		// Token: 0x0600B760 RID: 46944 RVA: 0x0045F52C File Offset: 0x0045D72C
		public bool GivesPerk(HashedString perkId)
		{
			using (List<SkillPerk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IdHash == perkId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040095D1 RID: 38353
		public string description;

		// Token: 0x040095D2 RID: 38354
		public string dlcId;

		// Token: 0x040095D3 RID: 38355
		public string skillGroup;

		// Token: 0x040095D4 RID: 38356
		public string hat;

		// Token: 0x040095D5 RID: 38357
		public string badge;

		// Token: 0x040095D6 RID: 38358
		public int tier;

		// Token: 0x040095D7 RID: 38359
		public bool deprecated;

		// Token: 0x040095D8 RID: 38360
		public List<SkillPerk> perks;

		// Token: 0x040095D9 RID: 38361
		public List<string> priorSkills;
	}
}
