using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021AA RID: 8618
	public class CreaturePoopKGProduction : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B70B RID: 46859 RVA: 0x00115E75 File Offset: 0x00114075
		public CreaturePoopKGProduction(Tag poopElement, float amountToPoop)
		{
			this.poopElement = poopElement;
			this.amountToPoop = amountToPoop;
		}

		// Token: 0x0600B70C RID: 46860 RVA: 0x0045BAA4 File Offset: 0x00459CA4
		public override bool Success()
		{
			return Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(this.poopElement) && Game.Instance.savedInfo.creaturePoopAmount[this.poopElement] >= this.amountToPoop;
		}

		// Token: 0x0600B70D RID: 46861 RVA: 0x0045BAF4 File Offset: 0x00459CF4
		public void Deserialize(IReader reader)
		{
			this.amountToPoop = reader.ReadSingle();
			string name = reader.ReadKleiString();
			this.poopElement = new Tag(name);
		}

		// Token: 0x0600B70E RID: 46862 RVA: 0x0045BB20 File Offset: 0x00459D20
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			Game.Instance.savedInfo.creaturePoopAmount.TryGetValue(this.poopElement, out num);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POOP_PRODUCTION, GameUtil.GetFormattedMass(complete ? this.amountToPoop : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
		}

		// Token: 0x0400951D RID: 38173
		private Tag poopElement;

		// Token: 0x0400951E RID: 38174
		private float amountToPoop;
	}
}
