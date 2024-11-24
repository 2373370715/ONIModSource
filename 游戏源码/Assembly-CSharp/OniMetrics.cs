using System.Collections.Generic;
using UnityEngine;

public class OniMetrics : MonoBehaviour
{
	public enum Event : short
	{
		NewSave,
		EndOfCycle,
		NumEvents
	}

	private static List<Dictionary<string, object>> Metrics;

	private static void EnsureMetrics()
	{
		if (Metrics == null)
		{
			Metrics = new List<Dictionary<string, object>>(2);
			for (int i = 0; i < 2; i++)
			{
				Metrics.Add(null);
			}
		}
	}

	public static void LogEvent(Event eventType, string key, object data)
	{
		EnsureMetrics();
		if (Metrics[(int)eventType] == null)
		{
			Metrics[(int)eventType] = new Dictionary<string, object>();
		}
		Metrics[(int)eventType][key] = data;
	}

	public static void SendEvent(Event eventType, string debugName)
	{
		if (Metrics[(int)eventType] != null && Metrics[(int)eventType].Count != 0)
		{
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(Metrics[(int)eventType], debugName);
			Metrics[(int)eventType].Clear();
		}
	}
}
