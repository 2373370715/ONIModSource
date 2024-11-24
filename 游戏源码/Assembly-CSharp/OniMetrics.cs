using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A69 RID: 6761
public class OniMetrics : MonoBehaviour
{
	// Token: 0x06008D6A RID: 36202 RVA: 0x0036B46C File Offset: 0x0036966C
	private static void EnsureMetrics()
	{
		if (OniMetrics.Metrics != null)
		{
			return;
		}
		OniMetrics.Metrics = new List<Dictionary<string, object>>(2);
		for (int i = 0; i < 2; i++)
		{
			OniMetrics.Metrics.Add(null);
		}
	}

	// Token: 0x06008D6B RID: 36203 RVA: 0x000FC564 File Offset: 0x000FA764
	public static void LogEvent(OniMetrics.Event eventType, string key, object data)
	{
		OniMetrics.EnsureMetrics();
		if (OniMetrics.Metrics[(int)eventType] == null)
		{
			OniMetrics.Metrics[(int)eventType] = new Dictionary<string, object>();
		}
		OniMetrics.Metrics[(int)eventType][key] = data;
	}

	// Token: 0x06008D6C RID: 36204 RVA: 0x0036B4A4 File Offset: 0x003696A4
	public static void SendEvent(OniMetrics.Event eventType, string debugName)
	{
		if (OniMetrics.Metrics[(int)eventType] == null || OniMetrics.Metrics[(int)eventType].Count == 0)
		{
			return;
		}
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(OniMetrics.Metrics[(int)eventType], debugName);
		OniMetrics.Metrics[(int)eventType].Clear();
	}

	// Token: 0x04006A40 RID: 27200
	private static List<Dictionary<string, object>> Metrics;

	// Token: 0x02001A6A RID: 6762
	public enum Event : short
	{
		// Token: 0x04006A42 RID: 27202
		NewSave,
		// Token: 0x04006A43 RID: 27203
		EndOfCycle,
		// Token: 0x04006A44 RID: 27204
		NumEvents
	}
}
