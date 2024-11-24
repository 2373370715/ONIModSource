using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02001E88 RID: 7816
public class OutfitDesignerScreen_OutfitState
{
	// Token: 0x0600A3E0 RID: 41952 RVA: 0x003E3EFC File Offset: 0x003E20FC
	private OutfitDesignerScreen_OutfitState(ClothingOutfitUtility.OutfitType outfitType, ClothingOutfitTarget sourceTarget, ClothingOutfitTarget destinationTarget)
	{
		this.outfitType = outfitType;
		this.destinationTarget = destinationTarget;
		this.sourceTarget = sourceTarget;
		this.name = sourceTarget.ReadName();
		this.slots = OutfitDesignerScreen_OutfitState.Slots.For(outfitType);
		foreach (ClothingItemResource item in sourceTarget.ReadItemValues())
		{
			this.ApplyItem(item);
		}
	}

	// Token: 0x0600A3E1 RID: 41953 RVA: 0x0010A4F3 File Offset: 0x001086F3
	public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(ClothingOutfitTarget outfitTemplate)
	{
		global::Debug.Assert(outfitTemplate.IsTemplateOutfit());
		return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
	}

	// Token: 0x0600A3E2 RID: 41954 RVA: 0x0010A50F File Offset: 0x0010870F
	public static OutfitDesignerScreen_OutfitState ForMinionInstance(ClothingOutfitTarget sourceTarget, GameObject minionInstance)
	{
		return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
	}

	// Token: 0x0600A3E3 RID: 41955 RVA: 0x0010A52B File Offset: 0x0010872B
	public unsafe void ApplyItem(ClothingItemResource item)
	{
		*this.slots.GetItemSlotForCategory(item.Category) = item;
	}

	// Token: 0x0600A3E4 RID: 41956 RVA: 0x0010A549 File Offset: 0x00108749
	public unsafe Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
	{
		return *this.slots.GetItemSlotForCategory(category);
	}

	// Token: 0x0600A3E5 RID: 41957 RVA: 0x003E3F80 File Offset: 0x003E2180
	public unsafe void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
	{
		if (item.IsSome())
		{
			DebugUtil.DevAssert(item.Unwrap().outfitType == this.outfitType, string.Format("Tried to set clothing item with outfit type \"{0}\" to outfit of type \"{1}\"", item.Unwrap().outfitType, this.outfitType), null);
			DebugUtil.DevAssert(item.Unwrap().Category == category, string.Format("Tried to set clothing item with category \"{0}\" to slot with type \"{1}\"", item.Unwrap().Category, category), null);
		}
		*this.slots.GetItemSlotForCategory(category) = item;
	}

	// Token: 0x0600A3E6 RID: 41958 RVA: 0x003E4020 File Offset: 0x003E2220
	public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				clothingItems.Add(ptr.Unwrap());
			}
		}
	}

	// Token: 0x0600A3E7 RID: 41959 RVA: 0x003E406C File Offset: 0x003E226C
	public void AddItemsTo(ICollection<string> itemIds)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				itemIds.Add(ptr.Unwrap().Id);
			}
		}
	}

	// Token: 0x0600A3E8 RID: 41960 RVA: 0x003E40BC File Offset: 0x003E22BC
	public string[] GetItems()
	{
		List<string> list = new List<string>();
		this.AddItemsTo(list);
		return list.ToArray();
	}

	// Token: 0x0600A3E9 RID: 41961 RVA: 0x003E40DC File Offset: 0x003E22DC
	public bool DoesContainLockedItems()
	{
		bool result;
		using (ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList pooledList = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>())
		{
			this.AddItemsTo(pooledList);
			result = ClothingOutfitTarget.DoesContainLockedItems(pooledList);
		}
		return result;
	}

	// Token: 0x0600A3EA RID: 41962 RVA: 0x003E411C File Offset: 0x003E231C
	public bool IsDirty()
	{
		using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet pooledHashSet = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
		{
			this.AddItemsTo(pooledHashSet);
			string[] array = this.destinationTarget.ReadItems();
			if (pooledHashSet.Count != array.Length)
			{
				return true;
			}
			foreach (string item in array)
			{
				if (!pooledHashSet.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04008017 RID: 32791
	public string name;

	// Token: 0x04008018 RID: 32792
	private OutfitDesignerScreen_OutfitState.Slots slots;

	// Token: 0x04008019 RID: 32793
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x0400801A RID: 32794
	public ClothingOutfitTarget sourceTarget;

	// Token: 0x0400801B RID: 32795
	public ClothingOutfitTarget destinationTarget;

	// Token: 0x02001E89 RID: 7817
	public abstract class Slots
	{
		// Token: 0x0600A3EB RID: 41963 RVA: 0x0010A55C File Offset: 0x0010875C
		private Slots(int slotsCount)
		{
			this.array = new Option<ClothingItemResource>[slotsCount];
		}

		// Token: 0x0600A3EC RID: 41964 RVA: 0x0010A570 File Offset: 0x00108770
		public static OutfitDesignerScreen_OutfitState.Slots For(ClothingOutfitUtility.OutfitType outfitType)
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				return new OutfitDesignerScreen_OutfitState.Slots.Clothing();
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead.");
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				return new OutfitDesignerScreen_OutfitState.Slots.Atmosuit();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600A3ED RID: 41965
		public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

		// Token: 0x0600A3EE RID: 41966 RVA: 0x003E4198 File Offset: 0x003E2398
		private ref Option<ClothingItemResource> FallbackSlot(OutfitDesignerScreen_OutfitState.Slots self, PermitCategory category)
		{
			DebugUtil.DevAssert(false, string.Format("Couldn't get a {0}<{1}> for {2} \"{3}\" on {4}.{5}", new object[]
			{
				"Option",
				"ClothingItemResource",
				"PermitCategory",
				category,
				"Slots",
				self.GetType().Name
			}), null);
			return ref OutfitDesignerScreen_OutfitState.Slots.dummySlot;
		}

		// Token: 0x0400801C RID: 32796
		public Option<ClothingItemResource>[] array;

		// Token: 0x0400801D RID: 32797
		private static Option<ClothingItemResource> dummySlot;

		// Token: 0x02001E8A RID: 7818
		public class Clothing : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600A3EF RID: 41967 RVA: 0x0010A5A2 File Offset: 0x001087A2
			public Clothing() : base(6)
			{
			}

			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x0600A3F0 RID: 41968 RVA: 0x0010A5AB File Offset: 0x001087AB
			public ref Option<ClothingItemResource> hatSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000A83 RID: 2691
			// (get) Token: 0x0600A3F1 RID: 41969 RVA: 0x0010A5B9 File Offset: 0x001087B9
			public ref Option<ClothingItemResource> topSlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000A84 RID: 2692
			// (get) Token: 0x0600A3F2 RID: 41970 RVA: 0x0010A5C7 File Offset: 0x001087C7
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000A85 RID: 2693
			// (get) Token: 0x0600A3F3 RID: 41971 RVA: 0x0010A5D5 File Offset: 0x001087D5
			public ref Option<ClothingItemResource> bottomSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000A86 RID: 2694
			// (get) Token: 0x0600A3F4 RID: 41972 RVA: 0x0010A5E3 File Offset: 0x001087E3
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x17000A87 RID: 2695
			// (get) Token: 0x0600A3F5 RID: 41973 RVA: 0x0010A5F1 File Offset: 0x001087F1
			public ref Option<ClothingItemResource> accessorySlot
			{
				get
				{
					return ref this.array[5];
				}
			}

			// Token: 0x0600A3F6 RID: 41974 RVA: 0x003E41F8 File Offset: 0x003E23F8
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.DupeHats)
				{
					return this.hatSlot;
				}
				if (category == PermitCategory.DupeTops)
				{
					return this.topSlot;
				}
				if (category == PermitCategory.DupeGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.DupeBottoms)
				{
					return this.bottomSlot;
				}
				if (category == PermitCategory.DupeShoes)
				{
					return this.shoesSlot;
				}
				if (category == PermitCategory.DupeAccessories)
				{
					return this.accessorySlot;
				}
				return base.FallbackSlot(this, category);
			}
		}

		// Token: 0x02001E8B RID: 7819
		public class Atmosuit : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600A3F7 RID: 41975 RVA: 0x0010A5FF File Offset: 0x001087FF
			public Atmosuit() : base(5)
			{
			}

			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x0600A3F8 RID: 41976 RVA: 0x0010A5AB File Offset: 0x001087AB
			public ref Option<ClothingItemResource> helmetSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000A89 RID: 2697
			// (get) Token: 0x0600A3F9 RID: 41977 RVA: 0x0010A5B9 File Offset: 0x001087B9
			public ref Option<ClothingItemResource> bodySlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000A8A RID: 2698
			// (get) Token: 0x0600A3FA RID: 41978 RVA: 0x0010A5C7 File Offset: 0x001087C7
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000A8B RID: 2699
			// (get) Token: 0x0600A3FB RID: 41979 RVA: 0x0010A5D5 File Offset: 0x001087D5
			public ref Option<ClothingItemResource> beltSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000A8C RID: 2700
			// (get) Token: 0x0600A3FC RID: 41980 RVA: 0x0010A5E3 File Offset: 0x001087E3
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x0600A3FD RID: 41981 RVA: 0x003E4250 File Offset: 0x003E2450
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.AtmoSuitHelmet)
				{
					return this.helmetSlot;
				}
				if (category == PermitCategory.AtmoSuitBody)
				{
					return this.bodySlot;
				}
				if (category == PermitCategory.AtmoSuitGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.AtmoSuitBelt)
				{
					return this.beltSlot;
				}
				if (category == PermitCategory.AtmoSuitShoes)
				{
					return this.shoesSlot;
				}
				return base.FallbackSlot(this, category);
			}
		}
	}
}
