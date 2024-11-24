using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public static class SerializableOutfitData
{
	public class Version2
	{
		public class CustomTemplateOutfitEntry
		{
			public string outfitType;

			public string[] itemIds;
		}

		public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();

		public Dictionary<string, CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, CustomTemplateOutfitEntry>();

		private static JsonSerializer s_serializer;

		public static Version2 FromVersion1(Version1 data)
		{
			Dictionary<string, CustomTemplateOutfitEntry> dictionary = new Dictionary<string, CustomTemplateOutfitEntry>();
			string value;
			foreach (KeyValuePair<string, string[]> customOutfit in data.CustomOutfits)
			{
				Util.Deconstruct(customOutfit, out value, out var value2);
				string key = value;
				string[] itemIds = value2;
				dictionary.Add(key, new CustomTemplateOutfitEntry
				{
					outfitType = "Clothing",
					itemIds = itemIds
				});
			}
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> duplicantOutfit in data.DuplicantOutfits)
			{
				Util.Deconstruct(duplicantOutfit, out value, out var value3);
				string key2 = value;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3 = value3;
				Dictionary<string, string> dictionary5 = (dictionary2[key2] = new Dictionary<string, string>());
				foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> item in dictionary3)
				{
					Util.Deconstruct(item, out var key3, out value);
					ClothingOutfitUtility.OutfitType outfitType = key3;
					string value4 = value;
					dictionary5.Add(Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfitType), value4);
				}
			}
			return new Version2
			{
				PersonalityIdToAssignedOutfits = dictionary2,
				OutfitIdToUserAuthoredTemplateOutfit = dictionary
			};
		}

		public static Version2 FromJson(JObject jsonData)
		{
			return jsonData.ToObject<Version2>(GetSerializer());
		}

		public static JObject ToJson(Version2 data)
		{
			JObject jObject = JObject.FromObject(data, GetSerializer());
			jObject.AddFirst(new JProperty("Version", 2));
			return jObject;
		}

		public static JsonSerializer GetSerializer()
		{
			if (s_serializer != null)
			{
				return s_serializer;
			}
			s_serializer = JsonSerializer.CreateDefault();
			s_serializer.Converters.Add(new StringEnumConverter());
			return s_serializer;
		}
	}

	public class Version1
	{
		public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();

		public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();

		public static JObject ToJson(Version1 data)
		{
			return JObject.FromObject(data);
		}

		public static Version1 FromJson(JObject jsonData)
		{
			Version1 version = new Version1();
			using JsonReader jsonReader = jsonData.CreateReader();
			string text = null;
			string text2 = "DuplicantOutfits";
			string text3 = "CustomOutfits";
			while (jsonReader.Read())
			{
				JsonToken tokenType = jsonReader.TokenType;
				if (tokenType == JsonToken.PropertyName)
				{
					text = jsonReader.Value.ToString();
				}
				if (tokenType == JsonToken.StartObject && text == text2)
				{
					string text4 = null;
					ClothingOutfitUtility.OutfitType result = ClothingOutfitUtility.OutfitType.LENGTH;
					string text5 = null;
					while (jsonReader.Read())
					{
						switch (jsonReader.TokenType)
						{
						case JsonToken.PropertyName:
							text4 = jsonReader.Value.ToString();
							while (jsonReader.Read())
							{
								switch (jsonReader.TokenType)
								{
								case JsonToken.PropertyName:
									Enum.TryParse<ClothingOutfitUtility.OutfitType>(jsonReader.Value.ToString(), out result);
									while (jsonReader.Read())
									{
										tokenType = jsonReader.TokenType;
										if (tokenType != JsonToken.String)
										{
											continue;
										}
										text5 = jsonReader.Value.ToString();
										if (result != ClothingOutfitUtility.OutfitType.LENGTH)
										{
											if (!version.DuplicantOutfits.ContainsKey(text4))
											{
												version.DuplicantOutfits.Add(text4, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
											}
											version.DuplicantOutfits[text4][result] = text5;
										}
										break;
									}
									continue;
								default:
									continue;
								case JsonToken.EndObject:
									break;
								}
								break;
							}
							continue;
						default:
							continue;
						case JsonToken.EndObject:
							break;
						}
						break;
					}
				}
				else
				{
					if (!(text == text3))
					{
						continue;
					}
					string text6 = null;
					string[] array = null;
					while (jsonReader.Read())
					{
						tokenType = jsonReader.TokenType;
						if (tokenType == JsonToken.EndObject)
						{
							break;
						}
						if (tokenType == JsonToken.PropertyName)
						{
							text6 = jsonReader.Value.ToString();
						}
						if (tokenType != JsonToken.StartArray)
						{
							continue;
						}
						JArray jArray = JArray.Load(jsonReader);
						if (jArray != null)
						{
							array = new string[jArray.Count];
							for (int i = 0; i < jArray.Count; i++)
							{
								array[i] = jArray[i].ToString();
							}
							if (text6 != null)
							{
								version.CustomOutfits[text6] = array;
							}
						}
					}
				}
			}
			return version;
		}
	}

	public const string VERSION_KEY = "Version";

	public static int GetVersionFrom(JObject jsonData)
	{
		int result;
		if (jsonData["Version"] == null)
		{
			result = 1;
		}
		else
		{
			result = jsonData.Value<int>("Version");
			jsonData.Remove("Version");
		}
		return result;
	}

	public static Version2 FromJson(JObject jsonData)
	{
		int versionFrom = GetVersionFrom(jsonData);
		switch (versionFrom)
		{
		case 1:
			return Version2.FromVersion1(Version1.FromJson(jsonData));
		case 2:
			return Version2.FromJson(jsonData);
		default:
			DebugUtil.DevAssert(test: false, $"Version {versionFrom} of OutfitData is not supported");
			return new Version2();
		}
	}

	public static JObject ToJson(Version2 data)
	{
		return Version2.ToJson(data);
	}

	public static string ToJsonString(JObject data)
	{
		using StringWriter stringWriter = new StringWriter();
		using JsonTextWriter writer = new JsonTextWriter(stringWriter);
		data.WriteTo(writer);
		return stringWriter.ToString();
	}

	public static void ToJsonString(JObject data, TextWriter textWriter)
	{
		using JsonTextWriter writer = new JsonTextWriter(textWriter);
		data.WriteTo(writer);
	}
}
