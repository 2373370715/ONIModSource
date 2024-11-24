using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

public class OutfitDesignerScreen_OutfitState
{
	public abstract class Slots
	{
		public class Clothing : Slots
		{
			public ref Option<ClothingItemResource> hatSlot => ref array[0];

			public ref Option<ClothingItemResource> topSlot => ref array[1];

			public ref Option<ClothingItemResource> glovesSlot => ref array[2];

			public ref Option<ClothingItemResource> bottomSlot => ref array[3];

			public ref Option<ClothingItemResource> shoesSlot => ref array[4];

			public ref Option<ClothingItemResource> accessorySlot => ref array[5];

			public Clothing()
				: base(6)
			{
			}

			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				return category switch
				{
					PermitCategory.DupeHats => ref hatSlot, 
					PermitCategory.DupeTops => ref topSlot, 
					PermitCategory.DupeGloves => ref glovesSlot, 
					PermitCategory.DupeBottoms => ref bottomSlot, 
					PermitCategory.DupeShoes => ref shoesSlot, 
					PermitCategory.DupeAccessories => ref accessorySlot, 
					_ => ref FallbackSlot(this, category), 
				};
			}
		}

		public class Atmosuit : Slots
		{
			public ref Option<ClothingItemResource> helmetSlot => ref array[0];

			public ref Option<ClothingItemResource> bodySlot => ref array[1];

			public ref Option<ClothingItemResource> glovesSlot => ref array[2];

			public ref Option<ClothingItemResource> beltSlot => ref array[3];

			public ref Option<ClothingItemResource> shoesSlot => ref array[4];

			public Atmosuit()
				: base(5)
			{
			}

			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				return category switch
				{
					PermitCategory.AtmoSuitHelmet => ref helmetSlot, 
					PermitCategory.AtmoSuitBody => ref bodySlot, 
					PermitCategory.AtmoSuitGloves => ref glovesSlot, 
					PermitCategory.AtmoSuitBelt => ref beltSlot, 
					PermitCategory.AtmoSuitShoes => ref shoesSlot, 
					_ => ref FallbackSlot(this, category), 
				};
			}
		}

		public Option<ClothingItemResource>[] array;

		private static Option<ClothingItemResource> dummySlot;

		private Slots(int slotsCount)
		{
			array = new Option<ClothingItemResource>[slotsCount];
		}

		public static Slots For(ClothingOutfitUtility.OutfitType outfitType)
		{
			return outfitType switch
			{
				ClothingOutfitUtility.OutfitType.Clothing => new Clothing(), 
				ClothingOutfitUtility.OutfitType.AtmoSuit => new Atmosuit(), 
				ClothingOutfitUtility.OutfitType.JoyResponse => throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead."), 
				_ => throw new NotImplementedException(), 
			};
		}

		public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

		private ref Option<ClothingItemResource> FallbackSlot(Slots self, PermitCategory category)
		{
			DebugUtil.DevAssert(test: false, string.Format("Couldn't get a {0}<{1}> for {2} \"{3}\" on {4}.{5}", "Option", "ClothingItemResource", "PermitCategory", category, "Slots", self.GetType().Name));
			return ref dummySlot;
		}
	}

	public string name;

	private Slots slots;

	public ClothingOutfitUtility.OutfitType outfitType;

	public ClothingOutfitTarget sourceTarget;

	public ClothingOutfitTarget destinationTarget;

	private OutfitDesignerScreen_OutfitState(ClothingOutfitUtility.OutfitType outfitType, ClothingOutfitTarget sourceTarget, ClothingOutfitTarget destinationTarget)
	{
		this.outfitType = outfitType;
		this.destinationTarget = destinationTarget;
		this.sourceTarget = sourceTarget;
		name = sourceTarget.ReadName();
		slots = Slots.For(outfitType);
		foreach (ClothingItemResource item in sourceTarget.ReadItemValues())
		{
			ApplyItem(item);
		}
	}

	public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(ClothingOutfitTarget outfitTemplate)
	{
		Debug.Assert(outfitTemplate.IsTemplateOutfit());
		return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
	}

	public static OutfitDesignerScreen_OutfitState ForMinionInstance(ClothingOutfitTarget sourceTarget, GameObject minionInstance)
	{
		return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
	}

	public void ApplyItem(ClothingItemResource item)
	{
		slots.GetItemSlotForCategory(item.Category) = item;
	}

	public Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
	{
		return slots.GetItemSlotForCategory(category);
	}

	public void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
	{
		if (item.IsSome())
		{
			DebugUtil.DevAssert(item.Unwrap().outfitType == outfitType, $"Tried to set clothing item with outfit type \"{item.Unwrap().outfitType}\" to outfit of type \"{outfitType}\"");
			DebugUtil.DevAssert(item.Unwrap().Category == category, $"Tried to set clothing item with category \"{item.Unwrap().Category}\" to slot with type \"{category}\"");
		}
		slots.GetItemSlotForCategory(category) = item;
	}

	public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
	{
		for (int i = 0; i < slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> reference = ref slots.array[i];
			if (reference.IsSome())
			{
				clothingItems.Add(reference.Unwrap());
			}
		}
	}

	public void AddItemsTo(ICollection<string> itemIds)
	{
		for (int i = 0; i < slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> reference = ref slots.array[i];
			if (reference.IsSome())
			{
				itemIds.Add(reference.Unwrap().Id);
			}
		}
	}

	public string[] GetItems()
	{
		List<string> list = new List<string>();
		AddItemsTo(list);
		return list.ToArray();
	}

	public bool DoesContainLockedItems()
	{
		using ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList itemIds = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>();
		AddItemsTo(itemIds);
		return ClothingOutfitTarget.DoesContainLockedItems(itemIds);
	}

	public bool IsDirty()
	{
		using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet pooledHashSet = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
		{
			AddItemsTo(pooledHashSet);
			string[] array = destinationTarget.ReadItems();
			if (pooledHashSet.Count != array.Length)
			{
				return true;
			}
			string[] array2 = array;
			foreach (string item in array2)
			{
				if (!pooledHashSet.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}
}
