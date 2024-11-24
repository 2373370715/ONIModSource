using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02002186 RID: 8582
	public class MinimumMorale : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B667 RID: 46695 RVA: 0x00115917 File Offset: 0x00113B17
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, this.minimumMorale);
		}

		// Token: 0x0600B668 RID: 46696 RVA: 0x00115933 File Offset: 0x00113B33
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, this.minimumMorale);
		}

		// Token: 0x0600B669 RID: 46697 RVA: 0x0011594F File Offset: 0x00113B4F
		public MinimumMorale(int minimumMorale = 16)
		{
			this.minimumMorale = minimumMorale;
		}

		// Token: 0x0600B66A RID: 46698 RVA: 0x00459D58 File Offset: 0x00457F58
		public override bool Success()
		{
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
				if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
				{
					AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
					flag = (attributeInstance != null && attributeInstance.GetTotalValue() >= (float)this.minimumMorale && flag);
				}
			}
			return flag;
		}

		// Token: 0x0600B66B RID: 46699 RVA: 0x0011595E File Offset: 0x00113B5E
		public void Deserialize(IReader reader)
		{
			this.minimumMorale = reader.ReadInt32();
		}

		// Token: 0x0600B66C RID: 46700 RVA: 0x0011596C File Offset: 0x00113B6C
		public override string GetProgress(bool complete)
		{
			return this.Description();
		}

		// Token: 0x040094F7 RID: 38135
		public int minimumMorale;
	}
}
