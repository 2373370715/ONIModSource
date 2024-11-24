using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Database;
using Newtonsoft.Json.Linq;
using STRINGS;

// Token: 0x0200107C RID: 4220
public static class ClothingOutfitUtility
{
	// Token: 0x06005648 RID: 22088 RVA: 0x00282178 File Offset: 0x00280378
	public static string GetName(this ClothingOutfitUtility.OutfitType self)
	{
		switch (self)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_CLOTHING;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_JOY_RESPONSE;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_ATMOSUIT;
		default:
			DebugUtil.DevAssert(false, string.Format("Couldn't find name for outfit type: {0}", self), null);
			return self.ToString();
		}
	}

	// Token: 0x06005649 RID: 22089 RVA: 0x002821E0 File Offset: 0x002803E0
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
		string path = Path.Combine(text, ClothingOutfitUtility.OutfitFile_U47_to_Present);
		string data = SerializableOutfitData.ToJsonString(SerializableOutfitData.ToJson(CustomClothingOutfits.Instance.Internal_GetOutfitData()));
		return ClothingOutfitUtility.TryWriteTo(path, data);
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x0028224C File Offset: 0x0028044C
	public static void LoadClothingOutfitData(ClothingOutfits dbClothingOutfits)
	{
		string pathToJsonFile = ClothingOutfitUtility.GetPathToJsonFile(ClothingOutfitUtility.OutfitFile_U47_to_Present);
		if (!File.Exists(pathToJsonFile))
		{
			pathToJsonFile = ClothingOutfitUtility.GetPathToJsonFile(ClothingOutfitUtility.OutfitFile_U44_to_U46);
			if (!File.Exists(pathToJsonFile))
			{
				return;
			}
		}
		string json;
		if (!ClothingOutfitUtility.TryReadFrom(pathToJsonFile, out json))
		{
			return;
		}
		SerializableOutfitData.Version2 version = null;
		try
		{
			version = SerializableOutfitData.FromJson(JObject.Parse(json));
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(false, "ClothingOutfitData Parse failed: " + ex.ToString(), null);
		}
		if (version == null)
		{
			return;
		}
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in version.OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			keyValuePair.Deconstruct(out text, out customTemplateOutfitEntry);
			string text2 = text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry2 = customTemplateOutfitEntry;
			ClothingOutfitResource clothingOutfitResource = dbClothingOutfits.TryGet(text2);
			if (clothingOutfitResource != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("UserAuthored outfit with id \"{0}\" of type {1} conflicts with DatabaseAuthored outfit with id \"{2}\" of type {3}. This may result in weird behaviour with outfits.", new object[]
					{
						text2,
						customTemplateOutfitEntry2.outfitType,
						clothingOutfitResource.Id,
						clothingOutfitResource.outfitType
					})
				});
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair2 in version.PersonalityIdToAssignedOutfits)
		{
			string text;
			Dictionary<string, string> dictionary;
			keyValuePair2.Deconstruct(out text, out dictionary);
			string text3 = text;
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(text3);
			if (personalityFromNameStringKey.IsNullOrDestroyed())
			{
				DebugUtil.DevAssert(false, "<Loadings Outfit Error> Couldn't find personality \"" + text3 + "\" to apply outfit preferences", null);
			}
			else if (text3 != personalityFromNameStringKey.Id)
			{
				list.Add(text3);
			}
		}
		foreach (string text4 in list)
		{
			Personality personalityFromNameStringKey2 = Db.Get().Personalities.GetPersonalityFromNameStringKey(text4);
			if (!personalityFromNameStringKey2.IsNullOrDestroyed() && version.PersonalityIdToAssignedOutfits.ContainsKey(text4))
			{
				string id = personalityFromNameStringKey2.Id;
				Dictionary<string, string> dictionary2 = version.PersonalityIdToAssignedOutfits[text4];
				version.PersonalityIdToAssignedOutfits.Remove(text4);
				Dictionary<string, string> dictionary3;
				if (version.PersonalityIdToAssignedOutfits.TryGetValue(id, out dictionary3))
				{
					using (Dictionary<string, string>.Enumerator enumerator4 = dictionary2.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							KeyValuePair<string, string> keyValuePair3 = enumerator4.Current;
							string text;
							string text5;
							keyValuePair3.Deconstruct(out text, out text5);
							string key = text;
							string value = text5;
							if (!dictionary3.ContainsKey(key))
							{
								dictionary3[key] = value;
							}
						}
						continue;
					}
				}
				version.PersonalityIdToAssignedOutfits.Add(id, dictionary2);
			}
		}
		CustomClothingOutfits.Instance.Internal_SetOutfitData(version);
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x000D857E File Offset: 0x000D677E
	public static string GetPathToJsonFile(string jsonFileName)
	{
		return Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName(), jsonFileName);
	}

	// Token: 0x0600564C RID: 22092 RVA: 0x00282534 File Offset: 0x00280734
	public static bool TryWriteTo(string path, string data)
	{
		bool result = false;
		try
		{
			using (FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(data);
				fileStream.Write(bytes, 0, bytes.Length);
				result = true;
			}
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(false, "ClothingOutfitData Write failed: " + ex.ToString(), null);
		}
		return result;
	}

	// Token: 0x0600564D RID: 22093 RVA: 0x002825AC File Offset: 0x002807AC
	public static bool TryReadFrom(string path, out string data)
	{
		data = null;
		bool result = false;
		try
		{
			using (FileStream fileStream = File.Open(path, FileMode.Open))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, new UTF8Encoding(false, true)))
				{
					data = streamReader.ReadToEnd();
					result = true;
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(false, "ClothingOutfitData Load failed: " + ex.ToString(), null);
		}
		return result;
	}

	// Token: 0x04003C87 RID: 15495
	public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_CLOTHING = new PermitCategory[]
	{
		PermitCategory.DupeTops,
		PermitCategory.DupeGloves,
		PermitCategory.DupeBottoms,
		PermitCategory.DupeShoes
	};

	// Token: 0x04003C88 RID: 15496
	public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_ATMO_SUITS = new PermitCategory[]
	{
		PermitCategory.AtmoSuitHelmet,
		PermitCategory.AtmoSuitBody,
		PermitCategory.AtmoSuitGloves,
		PermitCategory.AtmoSuitBelt,
		PermitCategory.AtmoSuitShoes
	};

	// Token: 0x04003C89 RID: 15497
	private static string OutfitFile_U44_to_U46 = "OutfitUserData.json";

	// Token: 0x04003C8A RID: 15498
	private static string OutfitFile_U47_to_Present = "OutfitUserData2.json";

	// Token: 0x0200107D RID: 4221
	public enum OutfitType
	{
		// Token: 0x04003C8C RID: 15500
		Clothing,
		// Token: 0x04003C8D RID: 15501
		JoyResponse,
		// Token: 0x04003C8E RID: 15502
		AtmoSuit,
		// Token: 0x04003C8F RID: 15503
		LENGTH
	}
}
