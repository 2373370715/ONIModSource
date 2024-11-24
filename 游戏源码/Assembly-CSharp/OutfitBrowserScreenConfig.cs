using System;
using UnityEngine;

public readonly struct OutfitBrowserScreenConfig
{
	public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;

	public readonly Option<ClothingOutfitTarget> selectedTarget;

	public readonly Option<Personality> minionPersonality;

	public readonly Option<GameObject> targetMinionInstance;

	public readonly bool isValid;

	public readonly bool isPickingOutfitForDupe;

	public OutfitBrowserScreenConfig(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType, Option<ClothingOutfitTarget> selectedTarget, Option<Personality> minionPersonality, Option<GameObject> minionInstance)
	{
		this.onlyShowOutfitType = onlyShowOutfitType;
		this.selectedTarget = selectedTarget;
		this.minionPersonality = minionPersonality;
		isPickingOutfitForDupe = minionPersonality.HasValue || minionInstance.HasValue;
		targetMinionInstance = minionInstance;
		isValid = true;
		if (minionPersonality.IsSome() || targetMinionInstance.IsSome())
		{
			Debug.Assert(onlyShowOutfitType.IsSome(), "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
		}
	}

	public OutfitBrowserScreenConfig WithOutfitType(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, selectedTarget, minionPersonality, targetMinionInstance);
	}

	public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, sourceTarget, minionPersonality, targetMinionInstance);
	}

	public string GetMinionName()
	{
		if (targetMinionInstance.HasValue)
		{
			return targetMinionInstance.Value.GetProperName();
		}
		if (minionPersonality.HasValue)
		{
			return minionPersonality.Value.Name;
		}
		return "-";
	}

	public static OutfitBrowserScreenConfig Mannequin()
	{
		return new OutfitBrowserScreenConfig(Option.None, Option.None, Option.None, Option.None);
	}

	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, Personality personality)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, Option.None, personality, Option.None);
	}

	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, GameObject minionInstance)
	{
		Personality personality = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), personality, minionInstance);
	}

	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, MinionBrowserScreen.GridItem item)
	{
		if (item is MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget)
		{
			return Minion(onlyShowOutfitType, personalityTarget.personality);
		}
		if (item is MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget)
		{
			return Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen);
	}
}
