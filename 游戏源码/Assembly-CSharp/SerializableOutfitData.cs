using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// Token: 0x02001857 RID: 6231
public static class SerializableOutfitData
{
	// Token: 0x060080CD RID: 32973 RVA: 0x00335E40 File Offset: 0x00334040
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

	// Token: 0x060080CE RID: 32974 RVA: 0x00335E78 File Offset: 0x00334078
	public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
	{
		int versionFrom = SerializableOutfitData.GetVersionFrom(jsonData);
		if (versionFrom == 1)
		{
			return SerializableOutfitData.Version2.FromVersion1(SerializableOutfitData.Version1.FromJson(jsonData));
		}
		if (versionFrom != 2)
		{
			DebugUtil.DevAssert(false, string.Format("Version {0} of OutfitData is not supported", versionFrom), null);
			return new SerializableOutfitData.Version2();
		}
		return SerializableOutfitData.Version2.FromJson(jsonData);
	}

	// Token: 0x060080CF RID: 32975 RVA: 0x000F4BCE File Offset: 0x000F2DCE
	public static JObject ToJson(SerializableOutfitData.Version2 data)
	{
		return SerializableOutfitData.Version2.ToJson(data);
	}

	// Token: 0x060080D0 RID: 32976 RVA: 0x00335EC8 File Offset: 0x003340C8
	public static string ToJsonString(JObject data)
	{
		string result;
		using (StringWriter stringWriter = new StringWriter())
		{
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
				result = stringWriter.ToString();
			}
		}
		return result;
	}

	// Token: 0x060080D1 RID: 32977 RVA: 0x00335F28 File Offset: 0x00334128
	public static void ToJsonString(JObject data, TextWriter textWriter)
	{
		using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
		{
			data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
		}
	}

	// Token: 0x040061AD RID: 25005
	public const string VERSION_KEY = "Version";

	// Token: 0x02001858 RID: 6232
	public class Version2
	{
		// Token: 0x060080D2 RID: 32978 RVA: 0x00335F64 File Offset: 0x00334164
		public static SerializableOutfitData.Version2 FromVersion1(SerializableOutfitData.Version1 data)
		{
			Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> dictionary = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
			foreach (KeyValuePair<string, string[]> keyValuePair in data.CustomOutfits)
			{
				string text;
				string[] array;
				keyValuePair.Deconstruct(out text, out array);
				string key = text;
				string[] itemIds = array;
				dictionary.Add(key, new SerializableOutfitData.Version2.CustomTemplateOutfitEntry
				{
					outfitType = "Clothing",
					itemIds = itemIds
				});
			}
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> keyValuePair2 in data.DuplicantOutfits)
			{
				string text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3;
				keyValuePair2.Deconstruct(out text, out dictionary3);
				string key2 = text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary4 = dictionary3;
				Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
				dictionary2[key2] = dictionary5;
				foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> keyValuePair3 in dictionary4)
				{
					ClothingOutfitUtility.OutfitType outfitType;
					keyValuePair3.Deconstruct(out outfitType, out text);
					ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
					string value = text;
					dictionary5.Add(Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfitType2), value);
				}
			}
			return new SerializableOutfitData.Version2
			{
				PersonalityIdToAssignedOutfits = dictionary2,
				OutfitIdToUserAuthoredTemplateOutfit = dictionary
			};
		}

		// Token: 0x060080D3 RID: 32979 RVA: 0x000F4BD6 File Offset: 0x000F2DD6
		public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
		{
			return jsonData.ToObject<SerializableOutfitData.Version2>(SerializableOutfitData.Version2.GetSerializer());
		}

		// Token: 0x060080D4 RID: 32980 RVA: 0x000F4BE3 File Offset: 0x000F2DE3
		public static JObject ToJson(SerializableOutfitData.Version2 data)
		{
			JObject jobject = JObject.FromObject(data, SerializableOutfitData.Version2.GetSerializer());
			jobject.AddFirst(new JProperty("Version", 2));
			return jobject;
		}

		// Token: 0x060080D5 RID: 32981 RVA: 0x000F4C06 File Offset: 0x000F2E06
		public static JsonSerializer GetSerializer()
		{
			if (SerializableOutfitData.Version2.s_serializer != null)
			{
				return SerializableOutfitData.Version2.s_serializer;
			}
			SerializableOutfitData.Version2.s_serializer = JsonSerializer.CreateDefault();
			SerializableOutfitData.Version2.s_serializer.Converters.Add(new StringEnumConverter());
			return SerializableOutfitData.Version2.s_serializer;
		}

		// Token: 0x040061AE RID: 25006
		public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x040061AF RID: 25007
		public Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();

		// Token: 0x040061B0 RID: 25008
		private static JsonSerializer s_serializer;

		// Token: 0x02001859 RID: 6233
		public class CustomTemplateOutfitEntry
		{
			// Token: 0x040061B1 RID: 25009
			public string outfitType;

			// Token: 0x040061B2 RID: 25010
			public string[] itemIds;
		}
	}

	// Token: 0x0200185A RID: 6234
	public class Version1
	{
		// Token: 0x060080D8 RID: 32984 RVA: 0x000F4C56 File Offset: 0x000F2E56
		public static JObject ToJson(SerializableOutfitData.Version1 data)
		{
			return JObject.FromObject(data);
		}

		// Token: 0x060080D9 RID: 32985 RVA: 0x003360D0 File Offset: 0x003342D0
		public static SerializableOutfitData.Version1 FromJson(JObject jsonData)
		{
			SerializableOutfitData.Version1 version = new SerializableOutfitData.Version1();
			SerializableOutfitData.Version1 result;
			using (JsonReader jsonReader = jsonData.CreateReader())
			{
				string a = null;
				string b = "DuplicantOutfits";
				string b2 = "CustomOutfits";
				while (jsonReader.Read())
				{
					JsonToken tokenType = jsonReader.TokenType;
					if (tokenType == JsonToken.PropertyName)
					{
						a = jsonReader.Value.ToString();
					}
					if (tokenType == JsonToken.StartObject && a == b)
					{
						ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								string key = jsonReader.Value.ToString();
								while (jsonReader.Read())
								{
									tokenType = jsonReader.TokenType;
									if (tokenType == JsonToken.EndObject)
									{
										break;
									}
									if (tokenType == JsonToken.PropertyName)
									{
										Enum.TryParse<ClothingOutfitUtility.OutfitType>(jsonReader.Value.ToString(), out outfitType);
										while (jsonReader.Read())
										{
											tokenType = jsonReader.TokenType;
											if (tokenType == JsonToken.String)
											{
												string value = jsonReader.Value.ToString();
												if (outfitType != ClothingOutfitUtility.OutfitType.LENGTH)
												{
													if (!version.DuplicantOutfits.ContainsKey(key))
													{
														version.DuplicantOutfits.Add(key, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
													}
													version.DuplicantOutfits[key][outfitType] = value;
													break;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
					else if (a == b2)
					{
						string text = null;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								text = jsonReader.Value.ToString();
							}
							if (tokenType == JsonToken.StartArray)
							{
								JArray jarray = JArray.Load(jsonReader);
								if (jarray != null)
								{
									string[] array = new string[jarray.Count];
									for (int i = 0; i < jarray.Count; i++)
									{
										array[i] = jarray[i].ToString();
									}
									if (text != null)
									{
										version.CustomOutfits[text] = array;
									}
								}
							}
						}
					}
				}
				result = version;
			}
			return result;
		}

		// Token: 0x040061B3 RID: 25011
		public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();

		// Token: 0x040061B4 RID: 25012
		public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();
	}
}
