using System;
using UnityEngine;

public readonly struct OutfitDesignerScreenConfig
{
	public readonly ClothingOutfitTarget sourceTarget;

	public readonly Option<ClothingOutfitTarget> outfitTemplate;

	public readonly Option<Personality> minionPersonality;

	public readonly Option<GameObject> targetMinionInstance;

	public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;

	public readonly bool isValid;

	public OutfitDesignerScreenConfig(ClothingOutfitTarget sourceTarget, Option<Personality> minionPersonality, Option<GameObject> targetMinionInstance, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
	{
		this.sourceTarget = sourceTarget;
		outfitTemplate = (sourceTarget.IsTemplateOutfit() ? Option.Some(sourceTarget) : ((Option<ClothingOutfitTarget>)Option.None));
		this.minionPersonality = minionPersonality;
		this.targetMinionInstance = targetMinionInstance;
		this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
		isValid = true;
		if (sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out var value))
		{
			Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == value.minionInstance);
		}
	}

	public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, minionPersonality, targetMinionInstance, onWriteToOutfitTargetFn);
	}

	public OutfitDesignerScreenConfig OnWriteToOutfitTarget(Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, minionPersonality, targetMinionInstance, onWriteToOutfitTargetFn);
	}

	public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
	{
		return new OutfitDesignerScreenConfig(outfit, Option.None, Option.None);
	}

	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, Personality personality)
	{
		return new OutfitDesignerScreenConfig(outfit, personality, Option.None);
	}

	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, GameObject targetMinionInstance)
	{
		Personality personality = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out var value));
		Debug.Assert(value.minionInstance == targetMinionInstance);
		return new OutfitDesignerScreenConfig(outfit, personality, targetMinionInstance);
	}

	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, MinionBrowserScreen.GridItem item)
	{
		if (item is MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget)
		{
			return Minion(outfit, personalityTarget.personality);
		}
		if (item is MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget)
		{
			return Minion(outfit, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen);
	}
}
