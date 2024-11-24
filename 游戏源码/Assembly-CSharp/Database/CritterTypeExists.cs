using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021A6 RID: 8614
	public class CritterTypeExists : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6FB RID: 46843 RVA: 0x00115DC5 File Offset: 0x00113FC5
		public CritterTypeExists(List<Tag> critterTypes)
		{
			this.critterTypes = critterTypes;
		}

		// Token: 0x0600B6FC RID: 46844 RVA: 0x0045B7C0 File Offset: 0x004599C0
		public override bool Success()
		{
			foreach (Capturable cmp in Components.Capturables.Items)
			{
				if (this.critterTypes.Contains(cmp.PrefabID()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B6FD RID: 46845 RVA: 0x0045B82C File Offset: 0x00459A2C
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.critterTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.critterTypes.Add(new Tag(name));
			}
		}

		// Token: 0x0600B6FE RID: 46846 RVA: 0x00115DDF File Offset: 0x00113FDF
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
		}

		// Token: 0x04009518 RID: 38168
		private List<Tag> critterTypes = new List<Tag>();
	}
}
