using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x0200215B RID: 8539
	public class Quests : ResourceSet<Quest>
	{
		// Token: 0x0600B5C8 RID: 46536 RVA: 0x00453548 File Offset: 0x00451748
		public Quests(ResourceSet parent) : base("Quests", parent)
		{
			this.LonelyMinionGreetingQuest = base.Add(new Quest("KnockQuest", new QuestCriteria[]
			{
				new QuestCriteria("Neighbor", null, 1, null, QuestCriteria.BehaviorFlags.None)
			}));
			this.LonelyMinionFoodQuest = base.Add(new Quest("FoodQuest", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("FoodQuality", new float[]
				{
					4f
				}, 3, new HashSet<Tag>
				{
					GameTags.Edible
				}, QuestCriteria.BehaviorFlags.UniqueItems)
			}));
			this.LonelyMinionPowerQuest = base.Add(new Quest("PluggedIn", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("SuppliedPower", new float[]
				{
					3000f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
			this.LonelyMinionDecorQuest = base.Add(new Quest("HighDecor", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("Decor", new float[]
				{
					120f
				}, 1, null, (QuestCriteria.BehaviorFlags)6)
			}));
			this.FossilHuntQuest = base.Add(new Quest("FossilHuntQuest", new QuestCriteria[]
			{
				new QuestCriteria_Equals("LostSpecimen", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostIceFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostResinFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostRockFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
		}

		// Token: 0x040093D9 RID: 37849
		public Quest LonelyMinionGreetingQuest;

		// Token: 0x040093DA RID: 37850
		public Quest LonelyMinionFoodQuest;

		// Token: 0x040093DB RID: 37851
		public Quest LonelyMinionPowerQuest;

		// Token: 0x040093DC RID: 37852
		public Quest LonelyMinionDecorQuest;

		// Token: 0x040093DD RID: 37853
		public Quest FossilHuntQuest;
	}
}
