using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001EB7 RID: 7863
public class ProgressBarsConfig : ScriptableObject
{
	// Token: 0x0600A53D RID: 42301 RVA: 0x0010B329 File Offset: 0x00109529
	public static void DestroyInstance()
	{
		ProgressBarsConfig.instance = null;
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x0600A53E RID: 42302 RVA: 0x0010B331 File Offset: 0x00109531
	public static ProgressBarsConfig Instance
	{
		get
		{
			if (ProgressBarsConfig.instance == null)
			{
				ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>("ProgressBarsConfig");
				ProgressBarsConfig.instance.Initialize();
			}
			return ProgressBarsConfig.instance;
		}
	}

	// Token: 0x0600A53F RID: 42303 RVA: 0x003EBCF8 File Offset: 0x003E9EF8
	public void Initialize()
	{
		foreach (ProgressBarsConfig.BarData barData in this.barColorDataList)
		{
			this.barColorMap.Add(barData.barName, barData);
		}
	}

	// Token: 0x0600A540 RID: 42304 RVA: 0x003EBD58 File Offset: 0x003E9F58
	public string GetBarDescription(string barName)
	{
		string result = "";
		if (this.IsBarNameValid(barName))
		{
			result = Strings.Get(this.barColorMap[barName].barDescriptionKey);
		}
		return result;
	}

	// Token: 0x0600A541 RID: 42305 RVA: 0x003EBD94 File Offset: 0x003E9F94
	public Color GetBarColor(string barName)
	{
		Color result = Color.clear;
		if (this.IsBarNameValid(barName))
		{
			result = this.barColorMap[barName].barColor;
		}
		return result;
	}

	// Token: 0x0600A542 RID: 42306 RVA: 0x0010B35E File Offset: 0x0010955E
	public bool IsBarNameValid(string barName)
	{
		if (string.IsNullOrEmpty(barName))
		{
			global::Debug.LogError("The barName provided was null or empty. Don't do that.");
			return false;
		}
		if (!this.barColorMap.ContainsKey(barName))
		{
			global::Debug.LogError(string.Format("No BarData found for the entry [ {0} ]", barName));
			return false;
		}
		return true;
	}

	// Token: 0x0400815E RID: 33118
	public GameObject progressBarPrefab;

	// Token: 0x0400815F RID: 33119
	public GameObject progressBarUIPrefab;

	// Token: 0x04008160 RID: 33120
	public GameObject healthBarPrefab;

	// Token: 0x04008161 RID: 33121
	public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();

	// Token: 0x04008162 RID: 33122
	public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();

	// Token: 0x04008163 RID: 33123
	private static ProgressBarsConfig instance;

	// Token: 0x02001EB8 RID: 7864
	[Serializable]
	public struct BarData
	{
		// Token: 0x04008164 RID: 33124
		public string barName;

		// Token: 0x04008165 RID: 33125
		public Color barColor;

		// Token: 0x04008166 RID: 33126
		public string barDescriptionKey;
	}
}
