using System.Collections.Generic;

namespace Database;

public class Quests : ResourceSet<Quest>
{
	public Quest LonelyMinionGreetingQuest;

	public Quest LonelyMinionFoodQuest;

	public Quest LonelyMinionPowerQuest;

	public Quest LonelyMinionDecorQuest;

	public Quest FossilHuntQuest;

	public Quests(ResourceSet parent)
		: base("Quests", parent)
	{
		LonelyMinionGreetingQuest = Add(new Quest("KnockQuest", new QuestCriteria[1]
		{
			new QuestCriteria("Neighbor")
		}));
		LonelyMinionFoodQuest = Add(new Quest("FoodQuest", new QuestCriteria[1]
		{
			new QuestCriteria_GreaterOrEqual("FoodQuality", new float[1] { 4f }, 3, new HashSet<Tag> { GameTags.Edible }, QuestCriteria.BehaviorFlags.UniqueItems)
		}));
		LonelyMinionPowerQuest = Add(new Quest("PluggedIn", new QuestCriteria[1]
		{
			new QuestCriteria_GreaterOrEqual("SuppliedPower", new float[1] { 3000f })
		}));
		LonelyMinionDecorQuest = Add(new Quest("HighDecor", new QuestCriteria[1]
		{
			new QuestCriteria_GreaterOrEqual("Decor", new float[1] { 120f }, 1, null, (QuestCriteria.BehaviorFlags)6)
		}));
		FossilHuntQuest = Add(new Quest("FossilHuntQuest", new QuestCriteria[4]
		{
			new QuestCriteria_Equals("LostSpecimen", new float[1] { 1f }),
			new QuestCriteria_Equals("LostIceFossil", new float[1] { 1f }),
			new QuestCriteria_Equals("LostResinFossil", new float[1] { 1f }),
			new QuestCriteria_Equals("LostRockFossil", new float[1] { 1f })
		}));
	}
}
