using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;

public readonly struct ClothingOutfitTarget : IEquatable<ClothingOutfitTarget>
{
	public interface Implementation
	{
		string OutfitId { get; }

		ClothingOutfitUtility.OutfitType OutfitType { get; }

		bool CanWriteItems { get; }

		bool CanWriteName { get; }

		bool CanDelete { get; }

		string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

		void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

		string ReadName();

		void WriteName(string name);

		void Delete();

		bool DoesExist();
	}

	public readonly struct MinionInstance : Implementation
	{
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		public readonly GameObject minionInstance;

		public readonly WearableAccessorizer accessorizer;

		public bool CanWriteItems => true;

		public bool CanWriteName => false;

		public bool CanDelete => false;

		public string OutfitId => minionInstance.GetInstanceID() + "_outfit";

		public ClothingOutfitUtility.OutfitType OutfitType => m_outfitType;

		public bool DoesExist()
		{
			return !minionInstance.IsNullOrDestroyed();
		}

		public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			m_outfitType = outfitType;
			accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return accessorizer.GetClothingItemsIds(outfitType);
		}

		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			accessorizer.ClearClothingItems(outfitType);
			accessorizer.ApplyClothingItems(outfitType, items.Select((string i) => Db.Get().Permits.ClothingItems.Get(i)));
		}

		public string ReadName()
		{
			return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", minionInstance.GetProperName());
		}

		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
		}

		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a minion instance outfit");
		}
	}

	public readonly struct UserAuthoredTemplate : Implementation
	{
		private readonly string[] m_outfitId;

		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		public bool CanWriteItems => true;

		public bool CanWriteName => true;

		public bool CanDelete => true;

		public string OutfitId => m_outfitId[0];

		public ClothingOutfitUtility.OutfitType OutfitType => m_outfitType;

		public bool DoesExist()
		{
			return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(OutfitId);
		}

		public UserAuthoredTemplate(ClothingOutfitUtility.OutfitType outfitType, string outfitId)
		{
			m_outfitId = new string[1] { outfitId };
			m_outfitType = outfitType;
		}

		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(OutfitId, out var value))
			{
				Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(value.outfitType, ignoreCase: true, out var result) && result == m_outfitType);
				return value.itemIds;
			}
			return NO_ITEMS;
		}

		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, OutfitId, items);
		}

		public string ReadName()
		{
			return OutfitId;
		}

		public void WriteName(string name)
		{
			if (!(OutfitId == name))
			{
				if (DoesTemplateExist(name))
				{
					throw new Exception("Can not change outfit name from \"" + OutfitId + "\" to \"" + name + "\", \"" + name + "\" already exists");
				}
				if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(OutfitId))
				{
					CustomClothingOutfits.Instance.Internal_RenameOutfit(m_outfitType, OutfitId, name);
				}
				else
				{
					CustomClothingOutfits.Instance.Internal_EditOutfit(m_outfitType, name, NO_ITEMS);
				}
				m_outfitId[0] = name;
			}
		}

		public void Delete()
		{
			CustomClothingOutfits.Instance.Internal_RemoveOutfit(m_outfitType, OutfitId);
		}
	}

	public readonly struct DatabaseAuthoredTemplate : Implementation
	{
		public readonly ClothingOutfitResource resource;

		private readonly string m_outfitId;

		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		public bool CanWriteItems => false;

		public bool CanWriteName => false;

		public bool CanDelete => false;

		public string OutfitId => m_outfitId;

		public ClothingOutfitUtility.OutfitType OutfitType => m_outfitType;

		public bool DoesExist()
		{
			return true;
		}

		public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
		{
			m_outfitId = outfit.Id;
			m_outfitType = outfit.outfitType;
			resource = outfit;
		}

		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return resource.itemsInOutfit;
		}

		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			throw new InvalidOperationException("Can not set items on a Db authored outfit");
		}

		public string ReadName()
		{
			return resource.Name;
		}

		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not set name on a Db authored outfit");
		}

		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a Db authored outfit");
		}
	}

	public readonly Implementation impl;

	public static readonly string[] NO_ITEMS = new string[0];

	public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

	public string OutfitId => impl.OutfitId;

	public ClothingOutfitUtility.OutfitType OutfitType => impl.OutfitType;

	public bool CanWriteItems => impl.CanWriteItems;

	public bool CanWriteName => impl.CanWriteName;

	public bool CanDelete => impl.CanDelete;

	public string[] ReadItems()
	{
		return impl.ReadItems(OutfitType).Where(DoesClothingItemExist).ToArray();
	}

	public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
	{
		impl.WriteItems(outfitType, items);
	}

	public string ReadName()
	{
		return impl.ReadName();
	}

	public void WriteName(string name)
	{
		impl.WriteName(name);
	}

	public void Delete()
	{
		impl.Delete();
	}

	public bool DoesExist()
	{
		return impl.DoesExist();
	}

	public ClothingOutfitTarget(Implementation impl)
	{
		this.impl = impl;
	}

	public bool DoesContainLockedItems()
	{
		return DoesContainLockedItems(ReadItems());
	}

	public static bool DoesContainLockedItems(IList<string> itemIds)
	{
		foreach (string itemId in itemIds)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(itemId);
			if (permitResource != null && !permitResource.IsUnlocked())
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerable<ClothingItemResource> ReadItemValues()
	{
		return from i in ReadItems()
			select Db.Get().Permits.ClothingItems.Get(i);
	}

	public static bool DoesClothingItemExist(string clothingItemId)
	{
		return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
	}

	public bool Is<T>() where T : Implementation
	{
		return impl is T;
	}

	public bool Is<T>(out T value) where T : Implementation
	{
		if (impl is T val)
		{
			value = val;
			return true;
		}
		value = default(T);
		return false;
	}

	public bool IsTemplateOutfit()
	{
		if (!Is<DatabaseAuthoredTemplate>())
		{
			return Is<UserAuthoredTemplate>();
		}
		return true;
	}

	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		return new ClothingOutfitTarget(new UserAuthoredTemplate(outfitType, GetUniqueNameIdFrom(UI.OUTFIT_NAME.NEW)));
	}

	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType, string id)
	{
		if (DoesTemplateExist(id))
		{
			throw new ArgumentException("Can not create a new target with id " + id + ", an outfit with that id already exists");
		}
		return new ClothingOutfitTarget(new UserAuthoredTemplate(outfitType, id));
	}

	public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
	{
		return new ClothingOutfitTarget(new UserAuthoredTemplate(sourceTarget.OutfitType, GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
	}

	public static ClothingOutfitTarget FromMinion(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
	{
		return new ClothingOutfitTarget(new MinionInstance(outfitType, minionInstance));
	}

	public static ClothingOutfitTarget FromTemplateId(string outfitId)
	{
		return TryFromTemplateId(outfitId).Value;
	}

	public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
	{
		if (outfitId == null)
		{
			return Option.None;
		}
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out var value) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(value.outfitType, ignoreCase: true, out var result))
		{
			return new ClothingOutfitTarget(new UserAuthoredTemplate(result, outfitId));
		}
		ClothingOutfitResource clothingOutfitResource = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
		if (!clothingOutfitResource.IsNullOrDestroyed())
		{
			return new ClothingOutfitTarget(new DatabaseAuthoredTemplate(clothingOutfitResource));
		}
		return Option.None;
	}

	public static bool DoesTemplateExist(string outfitId)
	{
		if (Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null)
		{
			return true;
		}
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId))
		{
			return true;
		}
		return false;
	}

	public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
	{
		foreach (ClothingOutfitResource resource in Db.Get().Permits.ClothingOutfits.resources)
		{
			yield return new ClothingOutfitTarget(new DatabaseAuthoredTemplate(resource));
		}
		foreach (var (outfitId, customTemplateOutfitEntry2) in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
		{
			if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry2.outfitType, ignoreCase: true, out var result))
			{
				yield return new ClothingOutfitTarget(new UserAuthoredTemplate(result, outfitId));
			}
		}
	}

	public static ClothingOutfitTarget GetRandom()
	{
		return GetAllTemplates().GetRandom();
	}

	public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from t in GetAllTemplates()
			where t.OutfitType == onlyOfType
			select t;
		if (enumerable == null || enumerable.Count() == 0)
		{
			return Option.None;
		}
		return enumerable.GetRandom();
	}

	public static string GetUniqueNameIdFrom(string preferredName)
	{
		if (!DoesTemplateExist(preferredName))
		{
			return preferredName;
		}
		string replacement = "testOutfit";
		string text = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString());
		string text2 = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString());
		string text3 = ((!(text != text2)) ? "{OutfitName} ({ConflictNumber})" : ((string)UI.OUTFIT_NAME.RESOLVE_CONFLICT));
		for (int i = 1; i < 10000; i++)
		{
			string text4 = text3.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", i.ToString());
			if (!DoesTemplateExist(text4))
			{
				return text4;
			}
		}
		throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
	}

	public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return !a.Equals(b);
	}

	public override bool Equals(object obj)
	{
		if (obj is ClothingOutfitTarget other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(ClothingOutfitTarget other)
	{
		if (impl == null || other.impl == null)
		{
			return impl == null == (other.impl == null);
		}
		return OutfitId == other.OutfitId;
	}

	public override int GetHashCode()
	{
		return Hash.SDBMLower(impl.OutfitId);
	}
}
