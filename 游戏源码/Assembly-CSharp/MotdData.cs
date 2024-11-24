using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class MotdData
{
	public int liveVersion;

	public List<MotdData_Box> boxesLive = new List<MotdData_Box>();

	public static MotdData Parse(string inputStr)
	{
		try
		{
			MotdData motdData = new MotdData();
			JObject jObject = JObject.Parse(inputStr);
			motdData.liveVersion = int.Parse(jObject["live-version"].Value<string>());
			foreach (JProperty item in (IEnumerable<JToken>)jObject["boxes-live"][0]["Category"])
			{
				string name = item.Name;
				foreach (JObject item2 in (IEnumerable<JToken>)item.Value)
				{
					MotdData_Box motdData_Box = new MotdData_Box
					{
						category = name,
						guid = item2.Value<string>("guid"),
						startTime = 0L,
						finishTime = 0L,
						title = item2.Value<string>("title"),
						text = item2.Value<string>("text"),
						image = item2.Value<string>("image"),
						href = item2.Value<string>("href")
					};
					if (long.TryParse(item2.Value<string>("start-time"), out var result))
					{
						motdData_Box.startTime = result;
					}
					if (long.TryParse(item2.Value<string>("finish-time"), out var result2))
					{
						motdData_Box.finishTime = result2;
					}
					motdData.boxesLive.Add(motdData_Box);
				}
			}
			return motdData;
		}
		catch (Exception arg)
		{
			Debug.LogWarning($"Motd Parse Error:\n--------------------\n{inputStr}\n--------------------\n{arg}");
			return null;
		}
	}
}
