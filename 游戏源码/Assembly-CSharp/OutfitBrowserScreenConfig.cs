using System;
using UnityEngine;

// Token: 0x02001E69 RID: 7785
public readonly struct OutfitBrowserScreenConfig
{
	// Token: 0x0600A334 RID: 41780 RVA: 0x003E0394 File Offset: 0x003DE594
	public OutfitBrowserScreenConfig(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType, Option<ClothingOutfitTarget> selectedTarget, Option<Personality> minionPersonality, Option<GameObject> minionInstance)
	{
		this.onlyShowOutfitType = onlyShowOutfitType;
		this.selectedTarget = selectedTarget;
		this.minionPersonality = minionPersonality;
		this.isPickingOutfitForDupe = (minionPersonality.HasValue || minionInstance.HasValue);
		this.targetMinionInstance = minionInstance;
		this.isValid = true;
		if (minionPersonality.IsSome() || this.targetMinionInstance.IsSome())
		{
			global::Debug.Assert(onlyShowOutfitType.IsSome(), "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
		}
	}

	// Token: 0x0600A335 RID: 41781 RVA: 0x00109DF2 File Offset: 0x00107FF2
	public OutfitBrowserScreenConfig WithOutfitType(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, this.selectedTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x0600A336 RID: 41782 RVA: 0x00109E0C File Offset: 0x0010800C
	public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
	{
		return new OutfitBrowserScreenConfig(this.onlyShowOutfitType, sourceTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x0600A337 RID: 41783 RVA: 0x003E0408 File Offset: 0x003DE608
	public string GetMinionName()
	{
		if (this.targetMinionInstance.HasValue)
		{
			return this.targetMinionInstance.Value.GetProperName();
		}
		if (this.minionPersonality.HasValue)
		{
			return this.minionPersonality.Value.Name;
		}
		return "-";
	}

	// Token: 0x0600A338 RID: 41784 RVA: 0x00109E26 File Offset: 0x00108026
	public static OutfitBrowserScreenConfig Mannequin()
	{
		return new OutfitBrowserScreenConfig(Option.None, Option.None, Option.None, Option.None);
	}

	// Token: 0x0600A339 RID: 41785 RVA: 0x00109E55 File Offset: 0x00108055
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, Personality personality)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, Option.None, personality, Option.None);
	}

	// Token: 0x0600A33A RID: 41786 RVA: 0x003E0458 File Offset: 0x003DE658
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, GameObject minionInstance)
	{
		Personality value = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), value, minionInstance);
	}

	// Token: 0x0600A33B RID: 41787 RVA: 0x003E04A4 File Offset: 0x003DE6A4
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x0600A33C RID: 41788 RVA: 0x00109E7C File Offset: 0x0010807C
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen, null);
	}

	// Token: 0x04007F60 RID: 32608
	public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;

	// Token: 0x04007F61 RID: 32609
	public readonly Option<ClothingOutfitTarget> selectedTarget;

	// Token: 0x04007F62 RID: 32610
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04007F63 RID: 32611
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04007F64 RID: 32612
	public readonly bool isValid;

	// Token: 0x04007F65 RID: 32613
	public readonly bool isPickingOutfitForDupe;
}
