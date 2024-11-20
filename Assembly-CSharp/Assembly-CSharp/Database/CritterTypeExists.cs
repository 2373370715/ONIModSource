using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	public class CritterTypeExists : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		public CritterTypeExists(List<Tag> critterTypes)
		{
			this.critterTypes = critterTypes;
		}

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

		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
		}

		private List<Tag> critterTypes = new List<Tag>();
	}
}
