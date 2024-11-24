using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000B70 RID: 2928
[AddComponentMenu("KMonoBehaviour/scripts/WearableAccessorizer")]
public class WearableAccessorizer : KMonoBehaviour
{
	// Token: 0x060037A2 RID: 14242 RVA: 0x000C414B File Offset: 0x000C234B
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> GetCustomClothingItems()
	{
		return this.customOutfitItems;
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060037A3 RID: 14243 RVA: 0x000C4153 File Offset: 0x000C2353
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> Wearables
	{
		get
		{
			return this.wearables;
		}
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x00218594 File Offset: 0x00216794
	public string[] GetClothingItemsIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customOutfitItems[outfitType].Count];
			for (int i = 0; i < this.customOutfitItems[outfitType].Count; i++)
			{
				array[i] = this.customOutfitItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return new string[0];
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x000C415B File Offset: 0x000C235B
	public Option<string> GetJoyResponseId()
	{
		return this.joyResponsePermitId;
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x000C4168 File Offset: 0x000C2368
	public void SetJoyResponseId(Option<string> joyResponsePermitId)
	{
		this.joyResponsePermitId = joyResponsePermitId.UnwrapOr(null, null);
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x0021860C File Offset: 0x0021680C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		base.Subscribe(-448952673, new Action<object>(this.EquippedItem));
		base.Subscribe(-1285462312, new Action<object>(this.UnequippedItem));
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x0021866C File Offset: 0x0021686C
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		List<WearableAccessorizer.WearableType> list = new List<WearableAccessorizer.WearableType>();
		foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
		{
			keyValuePair.Value.Deserialize();
			if (keyValuePair.Value.BuildAnims == null || keyValuePair.Value.BuildAnims.Count == 0)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (WearableAccessorizer.WearableType key in list)
		{
			this.wearables.Remove(key);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
			if (!this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomClothing))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing])
				{
					this.Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType.Clothing, resourceRef.Get());
				}
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x002187D4 File Offset: 0x002169D4
	public void EquippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.ApplyEquipment(component, component.GetBuildOverride());
		}
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x00218808 File Offset: 0x00216A08
	public void ApplyEquipment(Equippable equippable, KAnimFile animFile)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && animFile != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(this.wearables[key].BuildAnims[0], this.wearables[key].buildOverridePriority);
			}
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2))
			{
				this.wearables[WearableAccessorizer.WearableType.CustomSuit] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
				this.wearables[WearableAccessorizer.WearableType.CustomSuit].AddCustomItems(this.customOutfitItems[key2]);
			}
			else
			{
				this.wearables[key] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x000C4179 File Offset: 0x000C2379
	private bool TryGetEquippableClothingType(EquipmentDef equipment, out ClothingOutfitUtility.OutfitType outfitType)
	{
		if (equipment.Id == "Atmo_Suit")
		{
			outfitType = ClothingOutfitUtility.OutfitType.AtmoSuit;
			return true;
		}
		outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
		return false;
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x00218900 File Offset: 0x00216B00
	private Equippable GetSuitEquippable()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null && component.assignableProxy != null && component.assignableProxy.Get() != null)
		{
			Equipment equipment = component.GetEquipment();
			Assignable assignable = (equipment != null) ? equipment.GetAssignable(Db.Get().AssignableSlots.Suit) : null;
			if (assignable != null)
			{
				return assignable.GetComponent<Equippable>();
			}
		}
		return null;
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x00218974 File Offset: 0x00216B74
	private WearableAccessorizer.WearableType GetHighestAccessory()
	{
		WearableAccessorizer.WearableType wearableType = WearableAccessorizer.WearableType.Basic;
		foreach (WearableAccessorizer.WearableType wearableType2 in this.wearables.Keys)
		{
			if (wearableType2 > wearableType)
			{
				wearableType = wearableType2;
			}
		}
		return wearableType;
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x002189D0 File Offset: 0x00216BD0
	private void ApplyWearable()
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
			if (this.animController == null)
			{
				global::Debug.LogWarning("Missing animcontroller for WearableAccessorizer, bailing early to prevent a crash!");
				return;
			}
		}
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		WearableAccessorizer.WearableType highestAccessory = this.GetHighestAccessory();
		foreach (object obj in Enum.GetValues(typeof(WearableAccessorizer.WearableType)))
		{
			WearableAccessorizer.WearableType wearableType = (WearableAccessorizer.WearableType)obj;
			if (this.wearables.ContainsKey(wearableType))
			{
				WearableAccessorizer.Wearable wearable = this.wearables[wearableType];
				int buildOverridePriority = wearable.buildOverridePriority;
				foreach (KAnimFile kanimFile in wearable.BuildAnims)
				{
					KAnim.Build build = kanimFile.GetData().build;
					if (build != null)
					{
						for (int i = 0; i < build.symbols.Length; i++)
						{
							string text = HashCache.Get().Get(build.symbols[i].hash);
							if (wearableType == highestAccessory)
							{
								component.AddSymbolOverride(text, build.symbols[i], buildOverridePriority);
								this.animController.SetSymbolVisiblity(text, true);
							}
							else
							{
								component.RemoveSymbolOverride(text, buildOverridePriority);
							}
						}
					}
				}
			}
		}
		this.UpdateVisibleSymbols(highestAccessory);
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x000C4196 File Offset: 0x000C2396
	public void UpdateVisibleSymbols(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		this.UpdateVisibleSymbols(this.ConvertOutfitTypeToWearableType(outfitType));
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x00218B68 File Offset: 0x00216D68
	private void UpdateVisibleSymbols(WearableAccessorizer.WearableType wearableType)
	{
		bool flag = wearableType == WearableAccessorizer.WearableType.Basic;
		bool hasHat = base.GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Hat) != null;
		bool flag2 = false;
		bool is_visible = false;
		bool is_visible2 = true;
		bool is_visible3 = wearableType == WearableAccessorizer.WearableType.Basic;
		bool is_visible4 = wearableType == WearableAccessorizer.WearableType.Basic;
		if (this.wearables.ContainsKey(wearableType))
		{
			List<KAnimHashedString> list = this.wearables[wearableType].BuildAnims.SelectMany((KAnimFile x) => from s in x.GetData().build.symbols
			select s.hash).ToList<KAnimHashedString>();
			flag = (flag || list.Contains(Db.Get().AccessorySlots.Belt.targetSymbolId));
			flag2 = list.Contains(Db.Get().AccessorySlots.Skirt.targetSymbolId);
			is_visible = list.Contains(Db.Get().AccessorySlots.Necklace.targetSymbolId);
			is_visible2 = (list.Contains(Db.Get().AccessorySlots.ArmLower.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			is_visible3 = (list.Contains(Db.Get().AccessorySlots.Arm.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			is_visible4 = (list.Contains(Db.Get().AccessorySlots.Leg.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeBottoms)));
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Belt.targetSymbolId, flag);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.ArmLower.targetSymbolId, is_visible2);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Arm.targetSymbolId, is_visible3);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, is_visible4);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, flag2);
		if (flag2 || flag)
		{
			this.SkirtHACK(wearableType);
		}
		WearableAccessorizer.UpdateHairBasedOnHat(this.animController, hasHat);
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x00218DC8 File Offset: 0x00216FC8
	private void SkirtHACK(WearableAccessorizer.WearableType wearable_type)
	{
		if (this.wearables.ContainsKey(wearable_type))
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			WearableAccessorizer.Wearable wearable = this.wearables[wearable_type];
			int buildOverridePriority = wearable.buildOverridePriority;
			foreach (KAnimFile kanimFile in wearable.BuildAnims)
			{
				foreach (KAnim.Build.Symbol symbol in kanimFile.GetData().build.symbols)
				{
					if (HashCache.Get().Get(symbol.hash).EndsWith(WearableAccessorizer.cropped))
					{
						component.AddSymbolOverride(WearableAccessorizer.torso, symbol, buildOverridePriority);
						break;
					}
				}
			}
		}
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x00218E98 File Offset: 0x00217098
	public static void UpdateHairBasedOnHat(KAnimControllerBase kbac, bool hasHat)
	{
		if (hasHat)
		{
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);
			return;
		}
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x000C41BF File Offset: 0x000C23BF
	public static void SkirtAccessory(KAnimControllerBase kbac, bool show_skirt)
	{
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, show_skirt);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, !show_skirt);
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x00218F4C File Offset: 0x0021714C
	private void RemoveAnimBuild(KAnimFile animFile, int override_priority)
	{
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		KAnim.Build build = (animFile != null) ? animFile.GetData().build : null;
		if (build != null)
		{
			for (int i = 0; i < build.symbols.Length; i++)
			{
				string s = HashCache.Get().Get(build.symbols[i].hash);
				component.RemoveSymbolOverride(s, override_priority);
			}
		}
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x00218FB4 File Offset: 0x002171B4
	private void UnequippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.RemoveEquipment(component);
		}
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x00218FE0 File Offset: 0x002171E0
	public void RemoveEquipment(Equippable equippable)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2) && this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomSuit))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[key2])
				{
					this.RemoveAnimBuild(resourceRef.Get().AnimFile, this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				}
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				this.wearables.Remove(WearableAccessorizer.WearableType.CustomSuit);
			}
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[key].buildOverridePriority);
				this.wearables.Remove(key);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x00219114 File Offset: 0x00217314
	public void ClearClothingItems(ClothingOutfitUtility.OutfitType? forOutfitType = null)
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair in this.customOutfitItems)
		{
			ClothingOutfitUtility.OutfitType outfitType;
			List<ResourceRef<ClothingItemResource>> list;
			keyValuePair.Deconstruct(out outfitType, out list);
			ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
			if (forOutfitType != null)
			{
				ClothingOutfitUtility.OutfitType? outfitType3 = forOutfitType;
				outfitType = outfitType2;
				if (!(outfitType3.GetValueOrDefault() == outfitType & outfitType3 != null))
				{
					continue;
				}
			}
			this.ApplyClothingItems(outfitType2, Enumerable.Empty<ClothingItemResource>());
		}
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x0021919C File Offset: 0x0021739C
	public void ApplyClothingItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> items)
	{
		items = items.StableSort(delegate(ClothingItemResource resource)
		{
			if (resource.Category == PermitCategory.DupeTops)
			{
				return 10;
			}
			if (resource.Category == PermitCategory.DupeGloves)
			{
				return 8;
			}
			if (resource.Category == PermitCategory.DupeBottoms)
			{
				return 7;
			}
			if (resource.Category == PermitCategory.DupeShoes)
			{
				return 6;
			}
			return 1;
		});
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].Clear();
		}
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.wearables.ContainsKey(key))
		{
			foreach (KAnimFile animFile in this.wearables[key].BuildAnims)
			{
				this.RemoveAnimBuild(animFile, this.wearables[key].buildOverridePriority);
			}
			this.wearables[key].ClearAnims();
			if (items.Count<ClothingItemResource>() <= 0)
			{
				this.wearables.Remove(key);
			}
		}
		foreach (ClothingItemResource clothingItem in items)
		{
			this.Internal_ApplyClothingItem(outfitType, clothingItem);
		}
		this.ApplyWearable();
		Equippable suitEquippable = this.GetSuitEquippable();
		ClothingOutfitUtility.OutfitType outfitType2;
		bool flag = (suitEquippable == null && outfitType == ClothingOutfitUtility.OutfitType.Clothing) || (suitEquippable != null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2) && outfitType2 == outfitType);
		if (!base.GetComponent<MinionIdentity>().IsNullOrDestroyed() && this.animController.materialType != KAnimBatchGroup.MaterialType.UI && flag)
		{
			this.QueueOutfitChangedFX();
		}
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x0021933C File Offset: 0x0021753C
	private void Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothingItem)
	{
		WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
		if (!this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
		}
		if (!this.customOutfitItems[outfitType].Exists((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothingItem.IdHash))
		{
			if (this.wearables.ContainsKey(wearableType))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType].FindAll((ResourceRef<ClothingItemResource> x) => x.Get().Category == clothingItem.Category))
				{
					this.Internal_RemoveClothingItem(outfitType, resourceRef.Get());
				}
			}
			this.customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
		}
		bool flag;
		if (base.GetComponent<MinionIdentity>().IsNullOrDestroyed() || this.animController.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			flag = true;
		}
		else if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			flag = true;
		}
		else
		{
			Equippable suitEquippable = this.GetSuitEquippable();
			ClothingOutfitUtility.OutfitType outfitType2;
			flag = (suitEquippable != null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2) && outfitType2 == outfitType);
		}
		if (flag)
		{
			if (!this.wearables.ContainsKey(wearableType))
			{
				int buildOverridePriority = (wearableType == WearableAccessorizer.WearableType.CustomClothing) ? 4 : 6;
				this.wearables[wearableType] = new WearableAccessorizer.Wearable(new List<KAnimFile>(), buildOverridePriority);
			}
			this.wearables[wearableType].AddAnim(clothingItem.AnimFile);
		}
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x002194D4 File Offset: 0x002176D4
	private void Internal_RemoveClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothing_item)
	{
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].RemoveAll((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothing_item.IdHash);
		}
		if (this.wearables.ContainsKey(key))
		{
			if (this.wearables[key].RemoveAnim(clothing_item.AnimFile))
			{
				this.RemoveAnimBuild(clothing_item.AnimFile, this.wearables[key].buildOverridePriority);
			}
			if (this.wearables[key].BuildAnims.Count <= 0)
			{
				this.wearables.Remove(key);
			}
		}
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x000C41FA File Offset: 0x000C23FA
	private WearableAccessorizer.WearableType ConvertOutfitTypeToWearableType(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			return WearableAccessorizer.WearableType.CustomClothing;
		}
		if (outfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
		{
			global::Debug.LogWarning("Add a wearable type for clothing outfit type " + outfitType.ToString());
			return WearableAccessorizer.WearableType.Basic;
		}
		return WearableAccessorizer.WearableType.CustomSuit;
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x00219598 File Offset: 0x00217798
	public void RestoreWearables(Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> stored_wearables, Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> clothing)
	{
		if (stored_wearables != null)
		{
			this.wearables = stored_wearables;
			foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
			{
				keyValuePair.Value.Deserialize();
			}
		}
		if (clothing != null)
		{
			foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair2 in clothing)
			{
				this.ApplyClothingItems(keyValuePair2.Key, from i in keyValuePair2.Value
				select i.Get());
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x00219674 File Offset: 0x00217874
	public bool HasPermitCategoryItem(ClothingOutfitUtility.OutfitType wearable_type, PermitCategory category)
	{
		bool result = false;
		if (this.customOutfitItems.ContainsKey(wearable_type))
		{
			result = this.customOutfitItems[wearable_type].Exists((ResourceRef<ClothingItemResource> resource) => resource.Get().Category == category);
		}
		return result;
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x000C4226 File Offset: 0x000C2426
	private void QueueOutfitChangedFX()
	{
		this.waitingForOutfitChangeFX = true;
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x002196C0 File Offset: 0x002178C0
	private void Update()
	{
		if (this.waitingForOutfitChangeFX && !LockerNavigator.Instance.gameObject.activeInHierarchy)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MinionOutfitChanged, new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.FXFront)), 0f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Changed Clothes", base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			KFMOD.PlayOneShot(GlobalAssets.GetSound("SupplyCloset_Dupe_Clothing_Change", false), base.transform.position, 1f);
			this.waitingForOutfitChangeFX = false;
		}
	}

	// Token: 0x040025CE RID: 9678
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x040025CF RID: 9679
	[Obsolete("Deprecated, use customOufitItems[ClothingOutfitUtility.OutfitType.Clothing]")]
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x040025D0 RID: 9680
	[Serialize]
	private string joyResponsePermitId;

	// Token: 0x040025D1 RID: 9681
	[Serialize]
	private Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x040025D2 RID: 9682
	private bool waitingForOutfitChangeFX;

	// Token: 0x040025D3 RID: 9683
	[Serialize]
	private Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x040025D4 RID: 9684
	private static string torso = "torso";

	// Token: 0x040025D5 RID: 9685
	private static string cropped = "_cropped";

	// Token: 0x02000B71 RID: 2929
	public enum WearableType
	{
		// Token: 0x040025D7 RID: 9687
		Basic,
		// Token: 0x040025D8 RID: 9688
		CustomClothing,
		// Token: 0x040025D9 RID: 9689
		Outfit,
		// Token: 0x040025DA RID: 9690
		Suit,
		// Token: 0x040025DB RID: 9691
		CustomSuit
	}

	// Token: 0x02000B72 RID: 2930
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Wearable
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060037C2 RID: 14274 RVA: 0x000C426E File Offset: 0x000C246E
		public List<KAnimFile> BuildAnims
		{
			get
			{
				return this.buildAnims;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000C4276 File Offset: 0x000C2476
		public List<string> AnimNames
		{
			get
			{
				return this.animNames;
			}
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x00219790 File Offset: 0x00217990
		public Wearable(List<KAnimFile> buildAnims, int buildOverridePriority)
		{
			this.buildAnims = buildAnims;
			this.animNames = (from animFile in buildAnims
			select animFile.name).ToList<string>();
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000C427E File Offset: 0x000C247E
		public Wearable(KAnimFile buildAnim, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>
			{
				buildAnim
			};
			this.animNames = new List<string>
			{
				buildAnim.name
			};
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x002197E4 File Offset: 0x002179E4
		public Wearable(List<ResourceRef<ClothingItemResource>> items, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>();
			this.animNames = new List<string>();
			this.buildOverridePriority = buildOverridePriority;
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x00219878 File Offset: 0x00217A78
		public void AddCustomItems(List<ResourceRef<ClothingItemResource>> items)
		{
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x002198E8 File Offset: 0x00217AE8
		public void Deserialize()
		{
			if (this.animNames != null)
			{
				this.buildAnims = new List<KAnimFile>();
				for (int i = 0; i < this.animNames.Count; i++)
				{
					KAnimFile item = null;
					if (Assets.TryGetAnim(this.animNames[i], out item))
					{
						this.buildAnims.Add(item);
					}
				}
			}
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000C42B6 File Offset: 0x000C24B6
		public void AddAnim(KAnimFile animFile)
		{
			this.buildAnims.Add(animFile);
			this.animNames.Add(animFile.name);
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000C42D5 File Offset: 0x000C24D5
		public bool RemoveAnim(KAnimFile animFile)
		{
			return this.buildAnims.Remove(animFile) | this.animNames.Remove(animFile.name);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000C42F5 File Offset: 0x000C24F5
		public void ClearAnims()
		{
			this.buildAnims.Clear();
			this.animNames.Clear();
		}

		// Token: 0x040025DC RID: 9692
		private List<KAnimFile> buildAnims;

		// Token: 0x040025DD RID: 9693
		[Serialize]
		private List<string> animNames;

		// Token: 0x040025DE RID: 9694
		[Serialize]
		public int buildOverridePriority;
	}
}
