using System;
using System.Collections.Generic;

public class CustomClothingOutfits
{
	private static CustomClothingOutfits _instance;

	private SerializableOutfitData.Version2 serializableOutfitData = new SerializableOutfitData.Version2();

	public static CustomClothingOutfits Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CustomClothingOutfits();
			}
			return _instance;
		}
	}

	public SerializableOutfitData.Version2 Internal_GetOutfitData()
	{
		return serializableOutfitData;
	}

	public void Internal_SetOutfitData(SerializableOutfitData.Version2 data)
	{
		serializableOutfitData = data;
	}

	public void Internal_EditOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name, string[] outfit_items)
	{
		if (!serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfit_name, out var value))
		{
			value = new SerializableOutfitData.Version2.CustomTemplateOutfitEntry();
			value.outfitType = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			value.itemIds = outfit_items;
			serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[outfit_name] = value;
		}
		else
		{
			if (!Enum.TryParse<ClothingOutfitUtility.OutfitType>(value.outfitType, ignoreCase: true, out var result))
			{
				throw new NotSupportedException("Cannot edit outfit \"" + outfit_name + "\" of unknown outfit type \"" + value.outfitType + "\"");
			}
			if (result != outfit_type)
			{
				throw new NotSupportedException($"Cannot edit outfit \"{outfit_name}\" of outfit type \"{value.outfitType}\" to be an outfit of type \"{outfit_type}\"");
			}
			value.itemIds = outfit_items;
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	public void Internal_RenameOutfit(ClothingOutfitUtility.OutfitType outfit_type, string old_outfit_name, string new_outfit_name)
	{
		if (!serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(old_outfit_name))
		{
			throw new ArgumentException("Can't rename outfit \"" + old_outfit_name + "\" to \"" + new_outfit_name + "\": missing \"" + old_outfit_name + "\" entry");
		}
		if (serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(new_outfit_name))
		{
			throw new ArgumentException("Can't rename outfit \"" + old_outfit_name + "\" to \"" + new_outfit_name + "\": entry \"" + new_outfit_name + "\" already exists");
		}
		serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Add(new_outfit_name, serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[old_outfit_name]);
		foreach (KeyValuePair<string, Dictionary<string, string>> personalityIdToAssignedOutfit in serializableOutfitData.PersonalityIdToAssignedOutfits)
		{
			Util.Deconstruct(personalityIdToAssignedOutfit, out var key, out var value);
			Dictionary<string, string> dictionary = value;
			if (dictionary == null)
			{
				continue;
			}
			using ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>();
			foreach (KeyValuePair<string, string> item2 in dictionary)
			{
				Util.Deconstruct(item2, out key, out var value2);
				string item = key;
				if (value2 == old_outfit_name)
				{
					pooledList.Add(item);
				}
			}
			foreach (string item3 in pooledList)
			{
				dictionary[item3] = new_outfit_name;
			}
		}
		serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(old_outfit_name);
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	public void Internal_RemoveOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name)
	{
		if (!serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(outfit_name))
		{
			return;
		}
		foreach (KeyValuePair<string, Dictionary<string, string>> personalityIdToAssignedOutfit in serializableOutfitData.PersonalityIdToAssignedOutfits)
		{
			Util.Deconstruct(personalityIdToAssignedOutfit, out var key, out var value);
			Dictionary<string, string> dictionary = value;
			if (dictionary == null)
			{
				continue;
			}
			using ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>();
			foreach (KeyValuePair<string, string> item2 in dictionary)
			{
				Util.Deconstruct(item2, out key, out var value2);
				string item = key;
				if (value2 == outfit_name)
				{
					pooledList.Add(item);
				}
			}
			foreach (string item3 in pooledList)
			{
				dictionary.Remove(item3);
			}
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	public bool Internal_TryGetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, out string outfitId)
	{
		if (serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
		{
			string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			if (serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId].ContainsKey(name))
			{
				outfitId = serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name];
				return true;
			}
		}
		outfitId = null;
		return false;
	}

	public void Internal_SetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, Option<string> outfit_id)
	{
		string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
		Dictionary<string, string> value;
		if (outfit_id.HasValue)
		{
			if (!serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
			{
				serializableOutfitData.PersonalityIdToAssignedOutfits.Add(personalityId, new Dictionary<string, string>());
			}
			serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name] = outfit_id.Value;
		}
		else if (serializableOutfitData.PersonalityIdToAssignedOutfits.TryGetValue(personalityId, out value))
		{
			value.Remove(name);
			if (value.Count == 0)
			{
				serializableOutfitData.PersonalityIdToAssignedOutfits.Remove(personalityId);
			}
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}
}
