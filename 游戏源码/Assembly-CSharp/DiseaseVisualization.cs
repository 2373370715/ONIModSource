using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001773 RID: 6003
public class DiseaseVisualization : ScriptableObject
{
	// Token: 0x06007B7D RID: 31613 RVA: 0x0031C55C File Offset: 0x0031A75C
	public DiseaseVisualization.Info GetInfo(HashedString id)
	{
		foreach (DiseaseVisualization.Info info in this.info)
		{
			if (id == info.name)
			{
				return info;
			}
		}
		return default(DiseaseVisualization.Info);
	}

	// Token: 0x04005C9E RID: 23710
	public Sprite overlaySprite;

	// Token: 0x04005C9F RID: 23711
	public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();

	// Token: 0x02001774 RID: 6004
	[Serializable]
	public struct Info
	{
		// Token: 0x06007B7F RID: 31615 RVA: 0x000F13B1 File Offset: 0x000EF5B1
		public Info(string name)
		{
			this.name = name;
			this.overlayColourName = "germFoodPoisoning";
		}

		// Token: 0x04005CA0 RID: 23712
		public string name;

		// Token: 0x04005CA1 RID: 23713
		public string overlayColourName;
	}
}
