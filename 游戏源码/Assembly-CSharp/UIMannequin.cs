using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	public const float ANIM_SCALE = 0.38f;

	private KBatchedAnimController animController;

	private GameObject spawn;

	public bool shouldShowOutfitWithDefaultItems = true;

	public Option<Personality> personalityToUseForDefaultClothing;

	public GameObject SpawnedAvatar
	{
		get
		{
			if (spawn == null)
			{
				TrySpawn();
			}
			return spawn;
		}
	}

	public Option<Personality> Personality => default(Option<Personality>);

	protected override void OnSpawn()
	{
		TrySpawn();
	}

	public void TrySpawn()
	{
		if (animController == null)
		{
			animController = Util.KInstantiateUI(Assets.GetPrefab(MannequinUIPortrait.ID), base.gameObject).GetComponent<KBatchedAnimController>();
			animController.LoadAnims();
			animController.gameObject.SetActive(value: true);
			animController.animScale = 0.38f;
			animController.Play("idle", KAnim.PlayMode.Paused);
			spawn = animController.gameObject;
			MinionConfig.ConfigureSymbols(spawn, show_defaults: false);
			base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = Option.None;
		}
	}

	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		bool flag = outfit.Count() == 0;
		if (shouldShowOutfitWithDefaultItems)
		{
			outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		}
		SpawnedAvatar.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides();
		MinionConfig.ConfigureSymbols(SpawnedAvatar, show_defaults: false);
		Accessorizer component = SpawnedAvatar.GetComponent<Accessorizer>();
		WearableAccessorizer component2 = SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ApplyMinionPersonality(personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE")));
		component2.ClearClothingItems();
		component2.ApplyClothingItems(outfitType, outfit);
		List<KAnimHashedString> list = new List<KAnimHashedString>(32);
		if (shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			list.Add("foot");
			list.Add("hand_paint");
			if (flag)
			{
				list.Add("belt");
			}
			if (!outfit.Select((ClothingItemResource item) => item.Category).Contains(PermitCategory.DupeTops))
			{
				list.Add("torso");
				list.Add("neck");
				list.Add("arm_lower");
				list.Add("arm_lower_sleeve");
				list.Add("arm_sleeve");
				list.Add("cuff");
			}
			if (!outfit.Select((ClothingItemResource item) => item.Category).Contains(PermitCategory.DupeGloves))
			{
				list.Add("arm_lower_sleeve");
				list.Add("cuff");
			}
			if (!outfit.Select((ClothingItemResource item) => item.Category).Contains(PermitCategory.DupeBottoms))
			{
				list.Add("leg");
				list.Add("pelvis");
			}
		}
		KAnimHashedString[] source = outfit.SelectMany((ClothingItemResource item) => item.AnimFile.GetData().build.symbols.Select((KAnim.Build.Symbol s) => s.hash)).Concat(list).ToArray();
		KAnim.Build.Symbol[] symbols = animController.AnimFiles[0].GetData().build.symbols;
		foreach (KAnim.Build.Symbol symbol in symbols)
		{
			if (symbol.hash == (KAnimHashedString)"mannequin_arm" || symbol.hash == (KAnimHashedString)"mannequin_body" || symbol.hash == (KAnimHashedString)"mannequin_headshape" || symbol.hash == (KAnimHashedString)"mannequin_leg")
			{
				animController.SetSymbolVisiblity(symbol.hash, is_visible: true);
			}
			else
			{
				animController.SetSymbolVisiblity(symbol.hash, source.Contains(symbol.hash));
			}
		}
	}

	private static ClothingItemResource GetItemForCategory(PermitCategory category, IEnumerable<ClothingItemResource> outfit)
	{
		foreach (ClothingItemResource item in outfit)
		{
			if (item.Category == category)
			{
				return item;
			}
		}
		return null;
	}

	public void React(UIMinionOrMannequinReactSource source)
	{
		animController.Play("idle");
	}
}
