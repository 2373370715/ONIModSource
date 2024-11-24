using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020017A5 RID: 6053
public class ResearchPointInventory
{
	// Token: 0x06007CA2 RID: 31906 RVA: 0x003224B4 File Offset: 0x003206B4
	public ResearchPointInventory()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.PointsByTypeID.Add(researchType.id, 0f);
		}
	}

	// Token: 0x06007CA3 RID: 31907 RVA: 0x00322530 File Offset: 0x00320730
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.PointsByTypeID.ContainsKey(researchTypeID))
		{
			Debug.LogWarning("Research inventory is missing research point key " + researchTypeID);
			return;
		}
		Dictionary<string, float> pointsByTypeID = this.PointsByTypeID;
		pointsByTypeID[researchTypeID] += points;
	}

	// Token: 0x06007CA4 RID: 31908 RVA: 0x000F20D6 File Offset: 0x000F02D6
	public void RemoveResearchPoints(string researchTypeID, float points)
	{
		this.AddResearchPoints(researchTypeID, -points);
	}

	// Token: 0x06007CA5 RID: 31909 RVA: 0x00322578 File Offset: 0x00320778
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (!this.PointsByTypeID.ContainsKey(researchType.id))
			{
				this.PointsByTypeID.Add(researchType.id, 0f);
			}
		}
	}

	// Token: 0x04005E55 RID: 24149
	public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();
}
