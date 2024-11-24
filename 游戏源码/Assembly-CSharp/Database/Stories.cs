using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace Database;

public class Stories : ResourceSet<Story>
{
	public Story MegaBrainTank;

	public Story CreatureManipulator;

	public Story LonelyMinion;

	public Story FossilHunt;

	public Story MorbRoverMaker;

	public Stories(ResourceSet parent)
		: base("Stories", parent)
	{
		MegaBrainTank = Add(new Story("MegaBrainTank", "storytraits/MegaBrainTank", 0, 1, 43, "storytraits/mega_brain_tank").SetKeepsake("keepsake_megabrain"));
		CreatureManipulator = Add(new Story("CreatureManipulator", "storytraits/CritterManipulator", 1, 2, 43, "storytraits/creature_manipulator_retrofit").SetKeepsake("keepsake_crittermanipulator"));
		LonelyMinion = Add(new Story("LonelyMinion", "storytraits/LonelyMinion", 2, 3, 44, "storytraits/lonelyminion_retrofit").SetKeepsake("keepsake_lonelyminion"));
		FossilHunt = Add(new Story("FossilHunt", "storytraits/FossilHunt", 3, 4, 44, "storytraits/fossil_hunt_retrofit").SetKeepsake("keepsake_fossilhunt"));
		MorbRoverMaker = Add(new Story("MorbRoverMaker", "storytraits/MorbRoverMaker", 4, 5, 50, "storytraits/morb_rover_maker_retrofit").SetKeepsake("keepsake_morbrovermaker"));
		resources.Sort();
	}

	public void AddStoryMod(Story mod)
	{
		mod.kleiUseOnlyCoordinateOrder = -1;
		Add(mod);
		resources.Sort();
	}

	public int GetHighestCoordinate()
	{
		int num = 0;
		foreach (Story resource in resources)
		{
			num = Mathf.Max(num, resource.kleiUseOnlyCoordinateOrder);
		}
		return num;
	}

	public WorldTrait GetStoryTrait(string id, bool assertMissingTrait = false)
	{
		Story story = resources.Find((Story x) => x.Id == id);
		if (story != null)
		{
			return SettingsCache.GetCachedStoryTrait(story.worldgenStoryTraitKey, assertMissingTrait);
		}
		return null;
	}

	public Story GetStoryFromStoryTrait(string storyTraitTemplate)
	{
		return resources.Find((Story x) => x.worldgenStoryTraitKey == storyTraitTemplate);
	}

	public List<Story> GetStoriesSortedByCoordinateOrder()
	{
		List<Story> list = new List<Story>(resources);
		list.Sort((Story s1, Story s2) => s1.kleiUseOnlyCoordinateOrder.CompareTo(s2.kleiUseOnlyCoordinateOrder));
		return list;
	}
}
