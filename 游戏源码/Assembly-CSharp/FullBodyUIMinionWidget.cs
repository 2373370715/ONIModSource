using System.Linq;
using UnityEngine;

public class FullBodyUIMinionWidget : KMonoBehaviour
{
	[SerializeField]
	private GameObject duplicantAnimAnchor;

	public const float UI_MINION_PORTRAIT_ANIM_SCALE = 0.38f;

	public KBatchedAnimController animController { get; private set; }

	protected override void OnSpawn()
	{
		TrySpawnDisplayMinion();
	}

	private void TrySpawnDisplayMinion()
	{
		if (animController == null)
		{
			animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), duplicantAnimAnchor.gameObject).GetComponent<KBatchedAnimController>();
			animController.gameObject.SetActive(value: true);
			animController.animScale = 0.38f;
		}
	}

	private void InitializeAnimator()
	{
		TrySpawnDisplayMinion();
		animController.Queue("idle_default", KAnim.PlayMode.Loop);
		Accessorizer component = animController.GetComponent<Accessorizer>();
		for (int num = component.GetAccessories().Count - 1; num >= 0; num--)
		{
			component.RemoveAccessory(component.GetAccessories()[num].Get());
		}
	}

	public void SetDefaultPortraitAnimator()
	{
		MinionIdentity minionIdentity = ((Components.MinionIdentities.Count > 0) ? Components.MinionIdentities[0] : null);
		HashedString id = ((minionIdentity != null) ? minionIdentity.personalityResourceId : ((HashedString)Db.Get().Personalities.resources.GetRandom().Id));
		InitializeAnimator();
		animController.GetComponent<Accessorizer>().ApplyMinionPersonality(Db.Get().Personalities.Get(id));
		Accessorizer accessorizer = ((minionIdentity != null) ? minionIdentity.GetComponent<Accessorizer>() : null);
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if ((bool)accessorizer)
		{
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		UpdateHatOverride(null, hair_symbol, hat_hair_symbol);
		UpdateClothingOverride(animController.GetComponent<SymbolOverrideController>(), minionIdentity, null);
	}

	public void SetPortraitAnimator(IAssignableIdentity assignableIdentity)
	{
		if (assignableIdentity == null || assignableIdentity.IsNull())
		{
			SetDefaultPortraitAnimator();
			return;
		}
		InitializeAnimator();
		string current_hat = "";
		GetMinionIdentity(assignableIdentity, out var minionIdentity, out var storedMinionIdentity);
		Accessorizer accessorizer = null;
		Accessorizer component = animController.GetComponent<Accessorizer>();
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (minionIdentity != null)
		{
			accessorizer = minionIdentity.GetComponent<Accessorizer>();
			foreach (ResourceRef<Accessory> accessory in accessorizer.GetAccessories())
			{
				component.AddAccessory(accessory.Get());
			}
			current_hat = minionIdentity.GetComponent<MinionResume>().CurrentHat;
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		else if (storedMinionIdentity != null)
		{
			foreach (ResourceRef<Accessory> accessory2 in storedMinionIdentity.accessories)
			{
				component.AddAccessory(accessory2.Get());
			}
			current_hat = storedMinionIdentity.currentHat;
			hair_symbol = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		UpdateHatOverride(current_hat, hair_symbol, hat_hair_symbol);
		UpdateClothingOverride(animController.GetComponent<SymbolOverrideController>(), minionIdentity, storedMinionIdentity);
	}

	private void UpdateHatOverride(string current_hat, KAnim.Build.Symbol hair_symbol, KAnim.Build.Symbol hat_hair_symbol)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		animController.SetSymbolVisiblity(hat.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(current_hat) ? true : false);
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		SymbolOverrideController component = animController.GetComponent<SymbolOverrideController>();
		if (hair_symbol != null)
		{
			component.AddSymbolOverride("snapto_hair_always", hair_symbol, 1);
		}
		if (hat_hair_symbol != null)
		{
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, hat_hair_symbol, 1);
		}
	}

	private void UpdateClothingOverride(SymbolOverrideController symbolOverrideController, MinionIdentity identity, StoredMinionIdentity storedMinionIdentity)
	{
		string[] array = null;
		if (identity != null)
		{
			array = identity.GetComponent<WearableAccessorizer>().GetClothingItemsIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		else if (storedMinionIdentity != null)
		{
			array = storedMinionIdentity.GetClothingItemIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		if (array != null)
		{
			animController.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, array.Select((string i) => Db.Get().Permits.ClothingItems.Get(i)));
		}
	}

	public void UpdateEquipment(Equippable equippable, KAnimFile animFile)
	{
		animController.GetComponent<WearableAccessorizer>().ApplyEquipment(equippable, animFile);
	}

	public void RemoveEquipment(Equippable equippable)
	{
		animController.GetComponent<WearableAccessorizer>().RemoveEquipment(equippable);
	}

	private void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
		}
		else
		{
			minionIdentity = assignableIdentity as MinionIdentity;
			storedMinionIdentity = assignableIdentity as StoredMinionIdentity;
		}
	}
}
