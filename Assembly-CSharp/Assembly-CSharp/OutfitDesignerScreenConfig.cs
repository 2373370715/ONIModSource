﻿using System;
using UnityEngine;

public readonly struct OutfitDesignerScreenConfig
{
	public OutfitDesignerScreenConfig(ClothingOutfitTarget sourceTarget, Option<Personality> minionPersonality, Option<GameObject> targetMinionInstance, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
	{
		this.sourceTarget = sourceTarget;
		this.outfitTemplate = (sourceTarget.IsTemplateOutfit() ? Option.Some<ClothingOutfitTarget>(sourceTarget) : Option.None);
		this.minionPersonality = minionPersonality;
		this.targetMinionInstance = targetMinionInstance;
		this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
		this.isValid = true;
		ClothingOutfitTarget.MinionInstance minionInstance;
		if (sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance))
		{
			global::Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == minionInstance.minionInstance);
		}
	}

	public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, this.minionPersonality, this.targetMinionInstance, this.onWriteToOutfitTargetFn);
	}

	public OutfitDesignerScreenConfig OnWriteToOutfitTarget(Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		return new OutfitDesignerScreenConfig(this.sourceTarget, this.minionPersonality, this.targetMinionInstance, onWriteToOutfitTargetFn);
	}

	public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
	{
		return new OutfitDesignerScreenConfig(outfit, Option.None, Option.None, null);
	}

	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, Personality personality)
	{
		return new OutfitDesignerScreenConfig(outfit, personality, Option.None, null);
	}

	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, GameObject targetMinionInstance)
	{
		Personality value = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		ClothingOutfitTarget.MinionInstance minionInstance;
		global::Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance));
		global::Debug.Assert(minionInstance.minionInstance == targetMinionInstance);
		return new OutfitDesignerScreenConfig(outfit, value, targetMinionInstance, null);
	}

	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen, null);
	}

	public readonly ClothingOutfitTarget sourceTarget;

	public readonly Option<ClothingOutfitTarget> outfitTemplate;

	public readonly Option<Personality> minionPersonality;

	public readonly Option<GameObject> targetMinionInstance;

	public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;

	public readonly bool isValid;
}
