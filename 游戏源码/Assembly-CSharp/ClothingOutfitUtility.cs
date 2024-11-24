using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Database;
using Newtonsoft.Json.Linq;
using STRINGS;

public static class ClothingOutfitUtility
{
	public enum OutfitType
	{
		Clothing,
		JoyResponse,
		AtmoSuit,
		LENGTH
	}

	public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_CLOTHING = new PermitCategory[4]
	{
		PermitCategory.DupeTops,
		PermitCategory.DupeGloves,
		PermitCategory.DupeBottoms,
		PermitCategory.DupeShoes
	};

	public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_ATMO_SUITS = new PermitCategory[5]
	{
		PermitCategory.AtmoSuitHelmet,
		PermitCategory.AtmoSuitBody,
		PermitCategory.AtmoSuitGloves,
		PermitCategory.AtmoSuitBelt,
		PermitCategory.AtmoSuitShoes
	};

	private static string OutfitFile_U44_to_U46 = "OutfitUserData.json";

	private static string OutfitFile_U47_to_Present = "OutfitUserData2.json";

	public static string GetName(this OutfitType self)
	{
		switch (self)
		{
		case OutfitType.Clothing:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_CLOTHING;
		case OutfitType.JoyResponse:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_JOY_RESPONSE;
		case OutfitType.AtmoSuit:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_ATMOSUIT;
		default:
			DebugUtil.DevAssert(test: false, $"Couldn't find name for outfit type: {self}");
			return self.ToString();
		}
	}

	public static bool SaveClothingOutfitData()
	{
		if (!Directory.Exists(Util.RootFolder()))
		{
			Directory.CreateDirectory(Util.RootFolder());
		}
		string text = Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string path = Path.Combine(text, OutfitFile_U47_to_Present);
		string data = SerializableOutfitData.ToJsonString(SerializableOutfitData.ToJson(CustomClothingOutfits.Instance.Internal_GetOutfitData()));
		return TryWriteTo(path, data);
	}

	public static void LoadClothingOutfitData(ClothingOutfits dbClothingOutfits)
	{
		string pathToJsonFile = GetPathToJsonFile(OutfitFile_U47_to_Present);
		if (!File.Exists(pathToJsonFile))
		{
			pathToJsonFile = GetPathToJsonFile(OutfitFile_U44_to_U46);
			if (!File.Exists(pathToJsonFile))
			{
				return;
			}
		}
		if (!TryReadFrom(pathToJsonFile, out var data))
		{
			return;
		}
		SerializableOutfitData.Version2 version = null;
		try
		{
			version = SerializableOutfitData.FromJson(JObject.Parse(data));
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(test: false, "ClothingOutfitData Parse failed: " + ex.ToString());
		}
		if (version == null)
		{
			return;
		}
		string key;
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> item in version.OutfitIdToUserAuthoredTemplateOutfit)
		{
			Util.Deconstruct(item, out key, out var value);
			string text = key;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry = value;
			ClothingOutfitResource clothingOutfitResource = dbClothingOutfits.TryGet(text);
			if (clothingOutfitResource != null)
			{
				DebugUtil.LogWarningArgs($"UserAuthored outfit with id \"{text}\" of type {customTemplateOutfitEntry.outfitType} conflicts with DatabaseAuthored outfit with id \"{clothingOutfitResource.Id}\" of type {clothingOutfitResource.outfitType}. This may result in weird behaviour with outfits.");
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Dictionary<string, string>> personalityIdToAssignedOutfit in version.PersonalityIdToAssignedOutfits)
		{
			Util.Deconstruct(personalityIdToAssignedOutfit, out key, out var _);
			string text2 = key;
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(text2);
			if (personalityFromNameStringKey.IsNullOrDestroyed())
			{
				DebugUtil.DevAssert(test: false, "<Loadings Outfit Error> Couldn't find personality \"" + text2 + "\" to apply outfit preferences");
			}
			else if (text2 != personalityFromNameStringKey.Id)
			{
				list.Add(text2);
			}
		}
		foreach (string item2 in list)
		{
			Personality personalityFromNameStringKey2 = Db.Get().Personalities.GetPersonalityFromNameStringKey(item2);
			if (personalityFromNameStringKey2.IsNullOrDestroyed() || !version.PersonalityIdToAssignedOutfits.ContainsKey(item2))
			{
				continue;
			}
			string id = personalityFromNameStringKey2.Id;
			Dictionary<string, string> dictionary = version.PersonalityIdToAssignedOutfits[item2];
			version.PersonalityIdToAssignedOutfits.Remove(item2);
			if (version.PersonalityIdToAssignedOutfits.TryGetValue(id, out var value3))
			{
				foreach (KeyValuePair<string, string> item3 in dictionary)
				{
					Util.Deconstruct(item3, out key, out var value4);
					string key2 = key;
					string value5 = value4;
					if (!value3.ContainsKey(key2))
					{
						value3[key2] = value5;
					}
				}
			}
			else
			{
				version.PersonalityIdToAssignedOutfits.Add(id, dictionary);
			}
		}
		CustomClothingOutfits.Instance.Internal_SetOutfitData(version);
	}

	public static string GetPathToJsonFile(string jsonFileName)
	{
		return Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName(), jsonFileName);
	}

	public static bool TryWriteTo(string path, string data)
	{
		bool result = false;
		try
		{
			using FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			fileStream.Write(bytes, 0, bytes.Length);
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(test: false, "ClothingOutfitData Write failed: " + ex.ToString());
		}
		return result;
	}

	public static bool TryReadFrom(string path, out string data)
	{
		data = null;
		bool result = false;
		try
		{
			using FileStream stream = File.Open(path, FileMode.Open);
			using StreamReader streamReader = new StreamReader(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
			data = streamReader.ReadToEnd();
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(test: false, "ClothingOutfitData Load failed: " + ex.ToString());
		}
		return result;
	}
}
