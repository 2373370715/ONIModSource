using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02002039 RID: 8249
public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x0600AF90 RID: 44944 RVA: 0x00112122 File Offset: 0x00110322
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x0600AF91 RID: 44945 RVA: 0x00421628 File Offset: 0x0041F828
	public Option<Personality> Personality
	{
		get
		{
			return default(Option<Personality>);
		}
	}

	// Token: 0x0600AF92 RID: 44946 RVA: 0x0011213E File Offset: 0x0011033E
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600AF93 RID: 44947 RVA: 0x00421640 File Offset: 0x0041F840
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MannequinUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.LoadAnims();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle", KAnim.PlayMode.Paused, 1f, 0f);
			this.spawn = this.animController.gameObject;
			BaseMinionConfig.ConfigureSymbols(this.spawn, false);
			base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = Option.None;
		}
	}

	// Token: 0x0600AF94 RID: 44948 RVA: 0x00421708 File Offset: 0x0041F908
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		bool flag = outfit.Count<ClothingItemResource>() == 0;
		if (this.shouldShowOutfitWithDefaultItems)
		{
			outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		}
		this.SpawnedAvatar.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides(0);
		BaseMinionConfig.ConfigureSymbols(this.SpawnedAvatar, false);
		Accessorizer component = this.SpawnedAvatar.GetComponent<Accessorizer>();
		WearableAccessorizer component2 = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ApplyMinionPersonality(this.personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE"), null));
		component2.ClearClothingItems(null);
		component2.ApplyClothingItems(outfitType, outfit);
		List<KAnimHashedString> list = new List<KAnimHashedString>(32);
		if (this.shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			list.Add("foot");
			list.Add("hand_paint");
			if (flag)
			{
				list.Add("belt");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeTops))
			{
				list.Add("torso");
				list.Add("neck");
				list.Add("arm_lower");
				list.Add("arm_lower_sleeve");
				list.Add("arm_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeGloves))
			{
				list.Add("arm_lower_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeBottoms))
			{
				list.Add("leg");
				list.Add("pelvis");
			}
		}
		KAnimHashedString[] source = outfit.SelectMany((ClothingItemResource item) => from s in item.AnimFile.GetData().build.symbols
		select s.hash).Concat(list).ToArray<KAnimHashedString>();
		foreach (KAnim.Build.Symbol symbol in this.animController.AnimFiles[0].GetData().build.symbols)
		{
			if (symbol.hash == "mannequin_arm" || symbol.hash == "mannequin_body" || symbol.hash == "mannequin_headshape" || symbol.hash == "mannequin_leg")
			{
				this.animController.SetSymbolVisiblity(symbol.hash, true);
			}
			else
			{
				this.animController.SetSymbolVisiblity(symbol.hash, source.Contains(symbol.hash));
			}
		}
	}

	// Token: 0x0600AF95 RID: 44949 RVA: 0x00421A18 File Offset: 0x0041FC18
	private static ClothingItemResource GetItemForCategory(PermitCategory category, IEnumerable<ClothingItemResource> outfit)
	{
		foreach (ClothingItemResource clothingItemResource in outfit)
		{
			if (clothingItemResource.Category == category)
			{
				return clothingItemResource;
			}
		}
		return null;
	}

	// Token: 0x0600AF96 RID: 44950 RVA: 0x00112146 File Offset: 0x00110346
	public void React(UIMinionOrMannequinReactSource source)
	{
		this.animController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04008A69 RID: 35433
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04008A6A RID: 35434
	private KBatchedAnimController animController;

	// Token: 0x04008A6B RID: 35435
	private GameObject spawn;

	// Token: 0x04008A6C RID: 35436
	public bool shouldShowOutfitWithDefaultItems = true;

	// Token: 0x04008A6D RID: 35437
	public Option<Personality> personalityToUseForDefaultClothing;
}
