using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/WearableAccessorizer")]
public class WearableAccessorizer : KMonoBehaviour
{
	public enum WearableType
	{
		Basic,
		CustomClothing,
		Outfit,
		Suit,
		CustomSuit
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public class Wearable
	{
		private List<KAnimFile> buildAnims;

		[Serialize]
		private List<string> animNames;

		[Serialize]
		public int buildOverridePriority;

		public List<KAnimFile> BuildAnims => buildAnims;

		public List<string> AnimNames => animNames;

		public Wearable(List<KAnimFile> buildAnims, int buildOverridePriority)
		{
			this.buildAnims = buildAnims;
			animNames = buildAnims.Select((KAnimFile animFile) => animFile.name).ToList();
			this.buildOverridePriority = buildOverridePriority;
		}

		public Wearable(KAnimFile buildAnim, int buildOverridePriority)
		{
			buildAnims = new List<KAnimFile> { buildAnim };
			animNames = new List<string> { buildAnim.name };
			this.buildOverridePriority = buildOverridePriority;
		}

		public Wearable(List<ResourceRef<ClothingItemResource>> items, int buildOverridePriority)
		{
			buildAnims = new List<KAnimFile>();
			animNames = new List<string>();
			this.buildOverridePriority = buildOverridePriority;
			foreach (ResourceRef<ClothingItemResource> item in items)
			{
				ClothingItemResource clothingItemResource = item.Get();
				buildAnims.Add(clothingItemResource.AnimFile);
				animNames.Add(clothingItemResource.animFilename);
			}
		}

		public void AddCustomItems(List<ResourceRef<ClothingItemResource>> items)
		{
			foreach (ResourceRef<ClothingItemResource> item in items)
			{
				ClothingItemResource clothingItemResource = item.Get();
				buildAnims.Add(clothingItemResource.AnimFile);
				animNames.Add(clothingItemResource.animFilename);
			}
		}

		public void Deserialize()
		{
			if (animNames == null)
			{
				return;
			}
			buildAnims = new List<KAnimFile>();
			for (int i = 0; i < animNames.Count; i++)
			{
				KAnimFile anim = null;
				if (Assets.TryGetAnim(animNames[i], out anim))
				{
					buildAnims.Add(anim);
				}
			}
		}

		public void AddAnim(KAnimFile animFile)
		{
			buildAnims.Add(animFile);
			animNames.Add(animFile.name);
		}

		public bool RemoveAnim(KAnimFile animFile)
		{
			return buildAnims.Remove(animFile) | animNames.Remove(animFile.name);
		}

		public void ClearAnims()
		{
			buildAnims.Clear();
			animNames.Clear();
		}
	}

	[MyCmpReq]
	private KAnimControllerBase animController;

	[Obsolete("Deprecated, use customOufitItems[ClothingOutfitUtility.OutfitType.Clothing]")]
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	[Serialize]
	private string joyResponsePermitId;

	[Serialize]
	private Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	private bool waitingForOutfitChangeFX;

	[Serialize]
	private Dictionary<WearableType, Wearable> wearables = new Dictionary<WearableType, Wearable>();

	private static string torso = "torso";

	private static string cropped = "_cropped";

	public Dictionary<WearableType, Wearable> Wearables => wearables;

	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> GetCustomClothingItems()
	{
		return customOutfitItems;
	}

	public string[] GetClothingItemsIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (customOutfitItems.ContainsKey(outfitType))
		{
			string[] array = new string[customOutfitItems[outfitType].Count];
			for (int i = 0; i < customOutfitItems[outfitType].Count; i++)
			{
				array[i] = customOutfitItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return new string[0];
	}

	public Option<string> GetJoyResponseId()
	{
		return joyResponsePermitId;
	}

	public void SetJoyResponseId(Option<string> joyResponsePermitId)
	{
		this.joyResponsePermitId = joyResponsePermitId.UnwrapOr(null);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (animController == null)
		{
			animController = GetComponent<KAnimControllerBase>();
		}
		Subscribe(-448952673, EquippedItem);
		Subscribe(-1285462312, UnequippedItem);
	}

	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		List<WearableType> list = new List<WearableType>();
		foreach (KeyValuePair<WearableType, Wearable> wearable in wearables)
		{
			wearable.Value.Deserialize();
			if (wearable.Value.BuildAnims == null || wearable.Value.BuildAnims.Count == 0)
			{
				list.Add(wearable.Key);
			}
		}
		foreach (WearableType item in list)
		{
			wearables.Remove(item);
		}
		if (clothingItems.Count > 0)
		{
			customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(clothingItems);
			clothingItems.Clear();
			if (!wearables.ContainsKey(WearableType.CustomClothing))
			{
				foreach (ResourceRef<ClothingItemResource> item2 in customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing])
				{
					Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType.Clothing, item2.Get());
				}
			}
		}
		ApplyWearable();
	}

	public void EquippedItem(object data)
	{
		KPrefabID kPrefabID = data as KPrefabID;
		if (kPrefabID != null)
		{
			Equippable component = kPrefabID.GetComponent<Equippable>();
			ApplyEquipment(component, component.GetBuildOverride());
		}
	}

	public void ApplyEquipment(Equippable equippable, KAnimFile animFile)
	{
		if (equippable != null && animFile != null && Enum.TryParse<WearableType>(equippable.def.Slot, out var result))
		{
			if (wearables.ContainsKey(result))
			{
				RemoveAnimBuild(wearables[result].BuildAnims[0], wearables[result].buildOverridePriority);
			}
			if (TryGetEquippableClothingType(equippable.def, out var outfitType) && customOutfitItems.ContainsKey(outfitType))
			{
				wearables[WearableType.CustomSuit] = new Wearable(animFile, equippable.def.BuildOverridePriority);
				wearables[WearableType.CustomSuit].AddCustomItems(customOutfitItems[outfitType]);
			}
			else
			{
				wearables[result] = new Wearable(animFile, equippable.def.BuildOverridePriority);
			}
			ApplyWearable();
		}
	}

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

	private Equippable GetSuitEquippable()
	{
		MinionIdentity component = GetComponent<MinionIdentity>();
		if (component != null && component.assignableProxy != null && component.assignableProxy.Get() != null)
		{
			Equipment equipment = component.GetEquipment();
			Assignable assignable = ((equipment != null) ? equipment.GetAssignable(Db.Get().AssignableSlots.Suit) : null);
			if (assignable != null)
			{
				return assignable.GetComponent<Equippable>();
			}
		}
		return null;
	}

	private WearableType GetHighestAccessory()
	{
		WearableType wearableType = WearableType.Basic;
		foreach (WearableType key in wearables.Keys)
		{
			if (key > wearableType)
			{
				wearableType = key;
			}
		}
		return wearableType;
	}

	private void ApplyWearable()
	{
		if (animController == null)
		{
			animController = GetComponent<KAnimControllerBase>();
			if (animController == null)
			{
				Debug.LogWarning("Missing animcontroller for WearableAccessorizer, bailing early to prevent a crash!");
				return;
			}
		}
		SymbolOverrideController component = GetComponent<SymbolOverrideController>();
		WearableType highestAccessory = GetHighestAccessory();
		foreach (WearableType value in Enum.GetValues(typeof(WearableType)))
		{
			if (!wearables.ContainsKey(value))
			{
				continue;
			}
			Wearable wearable = wearables[value];
			int buildOverridePriority = wearable.buildOverridePriority;
			foreach (KAnimFile buildAnim in wearable.BuildAnims)
			{
				KAnim.Build build = buildAnim.GetData().build;
				if (build == null)
				{
					continue;
				}
				for (int i = 0; i < build.symbols.Length; i++)
				{
					string text = HashCache.Get().Get(build.symbols[i].hash);
					if (value == highestAccessory)
					{
						component.AddSymbolOverride(text, build.symbols[i], buildOverridePriority);
						animController.SetSymbolVisiblity(text, is_visible: true);
					}
					else
					{
						component.RemoveSymbolOverride(text, buildOverridePriority);
					}
				}
			}
		}
		UpdateVisibleSymbols(highestAccessory);
	}

	public void UpdateVisibleSymbols(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (animController == null)
		{
			animController = GetComponent<KAnimControllerBase>();
		}
		UpdateVisibleSymbols(ConvertOutfitTypeToWearableType(outfitType));
	}

	private void UpdateVisibleSymbols(WearableType wearableType)
	{
		bool flag = wearableType == WearableType.Basic;
		bool hasHat = GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Hat) != null;
		bool flag2 = false;
		bool is_visible = false;
		bool is_visible2 = true;
		bool is_visible3 = wearableType == WearableType.Basic;
		bool is_visible4 = wearableType == WearableType.Basic;
		if (wearables.ContainsKey(wearableType))
		{
			List<KAnimHashedString> list = wearables[wearableType].BuildAnims.SelectMany((KAnimFile x) => x.GetData().build.symbols.Select((KAnim.Build.Symbol s) => s.hash)).ToList();
			flag = flag || list.Contains(Db.Get().AccessorySlots.Belt.targetSymbolId);
			flag2 = list.Contains(Db.Get().AccessorySlots.Skirt.targetSymbolId);
			is_visible = list.Contains(Db.Get().AccessorySlots.Necklace.targetSymbolId);
			is_visible2 = list.Contains(Db.Get().AccessorySlots.ArmLower.targetSymbolId) || (wearableType != 0 && !HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops));
			is_visible3 = list.Contains(Db.Get().AccessorySlots.Arm.targetSymbolId) || (wearableType != 0 && !HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops));
			is_visible4 = list.Contains(Db.Get().AccessorySlots.Leg.targetSymbolId) || (wearableType != 0 && !HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeBottoms));
		}
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Belt.targetSymbolId, flag);
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible);
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.ArmLower.targetSymbolId, is_visible2);
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Arm.targetSymbolId, is_visible3);
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, is_visible4);
		animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, flag2);
		if (flag2 || flag)
		{
			SkirtHACK(wearableType);
		}
		UpdateHairBasedOnHat(animController, hasHat);
	}

	private void SkirtHACK(WearableType wearable_type)
	{
		if (!wearables.ContainsKey(wearable_type))
		{
			return;
		}
		SymbolOverrideController component = GetComponent<SymbolOverrideController>();
		Wearable wearable = wearables[wearable_type];
		int buildOverridePriority = wearable.buildOverridePriority;
		foreach (KAnimFile buildAnim in wearable.BuildAnims)
		{
			KAnim.Build.Symbol[] symbols = buildAnim.GetData().build.symbols;
			foreach (KAnim.Build.Symbol symbol in symbols)
			{
				if (HashCache.Get().Get(symbol.hash).EndsWith(cropped))
				{
					component.AddSymbolOverride(torso, symbol, buildOverridePriority);
					break;
				}
			}
		}
	}

	public static void UpdateHairBasedOnHat(KAnimControllerBase kbac, bool hasHat)
	{
		if (hasHat)
		{
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, is_visible: false);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, is_visible: true);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, is_visible: true);
		}
		else
		{
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, is_visible: true);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, is_visible: false);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, is_visible: false);
		}
	}

	public static void SkirtAccessory(KAnimControllerBase kbac, bool show_skirt)
	{
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, show_skirt);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, !show_skirt);
	}

	private void RemoveAnimBuild(KAnimFile animFile, int override_priority)
	{
		SymbolOverrideController component = GetComponent<SymbolOverrideController>();
		KAnim.Build build = ((animFile != null) ? animFile.GetData().build : null);
		if (build != null)
		{
			for (int i = 0; i < build.symbols.Length; i++)
			{
				string text = HashCache.Get().Get(build.symbols[i].hash);
				component.RemoveSymbolOverride(text, override_priority);
			}
		}
	}

	private void UnequippedItem(object data)
	{
		KPrefabID kPrefabID = data as KPrefabID;
		if (kPrefabID != null)
		{
			Equippable component = kPrefabID.GetComponent<Equippable>();
			RemoveEquipment(component);
		}
	}

	public void RemoveEquipment(Equippable equippable)
	{
		if (!(equippable != null) || !Enum.TryParse<WearableType>(equippable.def.Slot, out var result))
		{
			return;
		}
		if (TryGetEquippableClothingType(equippable.def, out var outfitType) && customOutfitItems.ContainsKey(outfitType) && wearables.ContainsKey(WearableType.CustomSuit))
		{
			foreach (ResourceRef<ClothingItemResource> item in customOutfitItems[outfitType])
			{
				RemoveAnimBuild(item.Get().AnimFile, wearables[WearableType.CustomSuit].buildOverridePriority);
			}
			RemoveAnimBuild(equippable.GetBuildOverride(), wearables[WearableType.CustomSuit].buildOverridePriority);
			wearables.Remove(WearableType.CustomSuit);
		}
		if (wearables.ContainsKey(result))
		{
			RemoveAnimBuild(equippable.GetBuildOverride(), wearables[result].buildOverridePriority);
			wearables.Remove(result);
		}
		ApplyWearable();
	}

	public void ClearClothingItems(ClothingOutfitUtility.OutfitType? forOutfitType = null)
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItem in customOutfitItems)
		{
			Util.Deconstruct(customOutfitItem, out var key, out var _);
			ClothingOutfitUtility.OutfitType outfitType = key;
			if (forOutfitType.HasValue)
			{
				ClothingOutfitUtility.OutfitType? outfitType2 = forOutfitType;
				key = outfitType;
				if (!(outfitType2 == key))
				{
					continue;
				}
			}
			ApplyClothingItems(outfitType, Enumerable.Empty<ClothingItemResource>());
		}
	}

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
			return (resource.Category != PermitCategory.DupeShoes) ? 1 : 6;
		});
		if (customOutfitItems.ContainsKey(outfitType))
		{
			customOutfitItems[outfitType].Clear();
		}
		WearableType key = ConvertOutfitTypeToWearableType(outfitType);
		if (wearables.ContainsKey(key))
		{
			foreach (KAnimFile buildAnim in wearables[key].BuildAnims)
			{
				RemoveAnimBuild(buildAnim, wearables[key].buildOverridePriority);
			}
			wearables[key].ClearAnims();
			if (items.Count() <= 0)
			{
				wearables.Remove(key);
			}
		}
		foreach (ClothingItemResource item in items)
		{
			Internal_ApplyClothingItem(outfitType, item);
		}
		ApplyWearable();
		Equippable suitEquippable = GetSuitEquippable();
		ClothingOutfitUtility.OutfitType outfitType2;
		bool flag = (suitEquippable == null && outfitType == ClothingOutfitUtility.OutfitType.Clothing) || (suitEquippable != null && TryGetEquippableClothingType(suitEquippable.def, out outfitType2) && outfitType2 == outfitType);
		if (!GetComponent<MinionIdentity>().IsNullOrDestroyed() && animController.materialType != KAnimBatchGroup.MaterialType.UI && flag)
		{
			QueueOutfitChangedFX();
		}
	}

	private void Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothingItem)
	{
		WearableType wearableType = ConvertOutfitTypeToWearableType(outfitType);
		if (!customOutfitItems.ContainsKey(outfitType))
		{
			customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
		}
		if (!customOutfitItems[outfitType].Exists((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothingItem.IdHash))
		{
			if (wearables.ContainsKey(wearableType))
			{
				foreach (ResourceRef<ClothingItemResource> item in customOutfitItems[outfitType].FindAll((ResourceRef<ClothingItemResource> x) => x.Get().Category == clothingItem.Category))
				{
					Internal_RemoveClothingItem(outfitType, item.Get());
				}
			}
			customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
		}
		bool flag;
		if (GetComponent<MinionIdentity>().IsNullOrDestroyed() || animController.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			flag = true;
		}
		else if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			flag = true;
		}
		else
		{
			Equippable suitEquippable = GetSuitEquippable();
			flag = suitEquippable != null && TryGetEquippableClothingType(suitEquippable.def, out var outfitType2) && outfitType2 == outfitType;
		}
		if (flag)
		{
			if (!wearables.ContainsKey(wearableType))
			{
				int buildOverridePriority = ((wearableType == WearableType.CustomClothing) ? 4 : 6);
				wearables[wearableType] = new Wearable(new List<KAnimFile>(), buildOverridePriority);
			}
			wearables[wearableType].AddAnim(clothingItem.AnimFile);
		}
	}

	private void Internal_RemoveClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothing_item)
	{
		WearableType key = ConvertOutfitTypeToWearableType(outfitType);
		if (customOutfitItems.ContainsKey(outfitType))
		{
			customOutfitItems[outfitType].RemoveAll((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothing_item.IdHash);
		}
		if (wearables.ContainsKey(key))
		{
			if (wearables[key].RemoveAnim(clothing_item.AnimFile))
			{
				RemoveAnimBuild(clothing_item.AnimFile, wearables[key].buildOverridePriority);
			}
			if (wearables[key].BuildAnims.Count <= 0)
			{
				wearables.Remove(key);
			}
		}
	}

	private WearableType ConvertOutfitTypeToWearableType(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return WearableType.CustomClothing;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return WearableType.CustomSuit;
		default:
			Debug.LogWarning("Add a wearable type for clothing outfit type " + outfitType);
			return WearableType.Basic;
		}
	}

	public void RestoreWearables(Dictionary<WearableType, Wearable> stored_wearables, Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> clothing)
	{
		if (stored_wearables != null)
		{
			wearables = stored_wearables;
			foreach (KeyValuePair<WearableType, Wearable> wearable in wearables)
			{
				wearable.Value.Deserialize();
			}
		}
		if (clothing != null)
		{
			foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> item in clothing)
			{
				ApplyClothingItems(item.Key, item.Value.Select((ResourceRef<ClothingItemResource> i) => i.Get()));
			}
		}
		ApplyWearable();
	}

	public bool HasPermitCategoryItem(ClothingOutfitUtility.OutfitType wearable_type, PermitCategory category)
	{
		bool result = false;
		if (customOutfitItems.ContainsKey(wearable_type))
		{
			result = customOutfitItems[wearable_type].Exists((ResourceRef<ClothingItemResource> resource) => resource.Get().Category == category);
		}
		return result;
	}

	private void QueueOutfitChangedFX()
	{
		waitingForOutfitChangeFX = true;
	}

	private void Update()
	{
		if (waitingForOutfitChangeFX && !LockerNavigator.Instance.gameObject.activeInHierarchy)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MinionOutfitChanged, new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.FXFront)), 0f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Changed Clothes", base.transform, new Vector3(0f, 0.5f, 0f));
			KFMOD.PlayOneShot(GlobalAssets.GetSound("SupplyCloset_Dupe_Clothing_Change"), base.transform.position);
			waitingForOutfitChangeFX = false;
		}
	}
}
