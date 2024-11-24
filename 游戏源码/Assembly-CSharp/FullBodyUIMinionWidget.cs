﻿using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02001CDC RID: 7388
public class FullBodyUIMinionWidget : KMonoBehaviour
{
	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06009A33 RID: 39475 RVA: 0x00104554 File Offset: 0x00102754
	// (set) Token: 0x06009A34 RID: 39476 RVA: 0x0010455C File Offset: 0x0010275C
	public KBatchedAnimController animController { get; private set; }

	// Token: 0x06009A35 RID: 39477 RVA: 0x00104565 File Offset: 0x00102765
	protected override void OnSpawn()
	{
		this.TrySpawnDisplayMinion();
	}

	// Token: 0x06009A36 RID: 39478 RVA: 0x003B84F4 File Offset: 0x003B66F4
	private void TrySpawnDisplayMinion()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), this.duplicantAnimAnchor.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
		}
	}

	// Token: 0x06009A37 RID: 39479 RVA: 0x003B855C File Offset: 0x003B675C
	private void InitializeAnimator()
	{
		this.TrySpawnDisplayMinion();
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		for (int i = component.GetAccessories().Count - 1; i >= 0; i--)
		{
			component.RemoveAccessory(component.GetAccessories()[i].Get());
		}
	}

	// Token: 0x06009A38 RID: 39480 RVA: 0x003B85CC File Offset: 0x003B67CC
	public void SetDefaultPortraitAnimator()
	{
		MinionIdentity minionIdentity = (Components.MinionIdentities.Count > 0) ? Components.MinionIdentities[0] : null;
		HashedString id = (minionIdentity != null) ? minionIdentity.personalityResourceId : Db.Get().Personalities.resources.GetRandom<Personality>().Id;
		this.InitializeAnimator();
		this.animController.GetComponent<Accessorizer>().ApplyMinionPersonality(Db.Get().Personalities.Get(id));
		Accessorizer accessorizer = (minionIdentity != null) ? minionIdentity.GetComponent<Accessorizer>() : null;
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (accessorizer)
		{
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(null, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, null);
	}

	// Token: 0x06009A39 RID: 39481 RVA: 0x003B86F4 File Offset: 0x003B68F4
	public void SetPortraitAnimator(IAssignableIdentity assignableIdentity)
	{
		if (assignableIdentity == null || assignableIdentity.IsNull())
		{
			this.SetDefaultPortraitAnimator();
			return;
		}
		this.InitializeAnimator();
		string current_hat = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(assignableIdentity, out minionIdentity, out storedMinionIdentity);
		Accessorizer accessorizer = null;
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (minionIdentity != null)
		{
			accessorizer = minionIdentity.GetComponent<Accessorizer>();
			foreach (ResourceRef<Accessory> resourceRef in accessorizer.GetAccessories())
			{
				component.AddAccessory(resourceRef.Get());
			}
			current_hat = minionIdentity.GetComponent<MinionResume>().CurrentHat;
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		else if (storedMinionIdentity != null)
		{
			foreach (ResourceRef<Accessory> resourceRef2 in storedMinionIdentity.accessories)
			{
				component.AddAccessory(resourceRef2.Get());
			}
			current_hat = storedMinionIdentity.currentHat;
			hair_symbol = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(current_hat, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, storedMinionIdentity);
	}

	// Token: 0x06009A3A RID: 39482 RVA: 0x003B8904 File Offset: 0x003B6B04
	private void UpdateHatOverride(string current_hat, KAnim.Build.Symbol hair_symbol, KAnim.Build.Symbol hat_hair_symbol)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		this.animController.SetSymbolVisiblity(hat.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		SymbolOverrideController component = this.animController.GetComponent<SymbolOverrideController>();
		if (hair_symbol != null)
		{
			component.AddSymbolOverride("snapto_hair_always", hair_symbol, 1);
		}
		if (hat_hair_symbol != null)
		{
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, hat_hair_symbol, 1);
		}
	}

	// Token: 0x06009A3B RID: 39483 RVA: 0x003B89DC File Offset: 0x003B6BDC
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
			this.animController.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in array
			select Db.Get().Permits.ClothingItems.Get(i));
		}
	}

	// Token: 0x06009A3C RID: 39484 RVA: 0x0010456D File Offset: 0x0010276D
	public void UpdateEquipment(Equippable equippable, KAnimFile animFile)
	{
		this.animController.GetComponent<WearableAccessorizer>().ApplyEquipment(equippable, animFile);
	}

	// Token: 0x06009A3D RID: 39485 RVA: 0x00104581 File Offset: 0x00102781
	public void RemoveEquipment(Equippable equippable)
	{
		this.animController.GetComponent<WearableAccessorizer>().RemoveEquipment(equippable);
	}

	// Token: 0x06009A3E RID: 39486 RVA: 0x00104594 File Offset: 0x00102794
	private void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			return;
		}
		minionIdentity = (assignableIdentity as MinionIdentity);
		storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
	}

	// Token: 0x04007860 RID: 30816
	[SerializeField]
	private GameObject duplicantAnimAnchor;

	// Token: 0x04007862 RID: 30818
	public const float UI_MINION_PORTRAIT_ANIM_SCALE = 0.38f;
}
