using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
	// Token: 0x020021AD RID: 8621
	public class DupesCompleteChoreInExoSuitForCycles : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B716 RID: 46870 RVA: 0x00115ED8 File Offset: 0x001140D8
		public DupesCompleteChoreInExoSuitForCycles(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x0600B717 RID: 46871 RVA: 0x0045BC48 File Offset: 0x00459E48
		public override bool Success()
		{
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.ColonyAchievementTracker.dupesCompleteChoresInSuits;
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				KPrefabID component = minionIdentity.GetComponent<KPrefabID>();
				if (!component.HasTag(GameTags.Dead))
				{
					dictionary.Add(component.InstanceID, minionIdentity.arrivalTime);
				}
			}
			int num = 0;
			int num2 = Math.Min(dupesCompleteChoresInSuits.Count, this.numCycles);
			for (int i = GameClock.Instance.GetCycle() - num2; i <= GameClock.Instance.GetCycle(); i++)
			{
				if (dupesCompleteChoresInSuits.ContainsKey(i))
				{
					List<int> list = dictionary.Keys.Except(dupesCompleteChoresInSuits[i]).ToList<int>();
					bool flag = true;
					foreach (int key in list)
					{
						if (dictionary[key] < (float)i)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num++;
					}
					else if (i != GameClock.Instance.GetCycle())
					{
						num = 0;
					}
					this.currentCycleStreak = num;
					if (num >= this.numCycles)
					{
						this.currentCycleStreak = this.numCycles;
						return true;
					}
				}
				else
				{
					this.currentCycleStreak = Math.Max(this.currentCycleStreak, num);
					num = 0;
				}
			}
			return false;
		}

		// Token: 0x0600B718 RID: 46872 RVA: 0x00115EE7 File Offset: 0x001140E7
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
		}

		// Token: 0x0600B719 RID: 46873 RVA: 0x0045BDD8 File Offset: 0x00459FD8
		public int GetNumberOfDupesForCycle(int cycle)
		{
			int result = 0;
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.ColonyAchievementTracker.dupesCompleteChoresInSuits;
			if (dupesCompleteChoresInSuits.ContainsKey(GameClock.Instance.GetCycle()))
			{
				result = dupesCompleteChoresInSuits[GameClock.Instance.GetCycle()].Count;
			}
			return result;
		}

		// Token: 0x04009523 RID: 38179
		public int currentCycleStreak;

		// Token: 0x04009524 RID: 38180
		public int numCycles;
	}
}
