using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017AB RID: 6059
public class TechInstance
{
	// Token: 0x06007CC5 RID: 31941 RVA: 0x000F21D8 File Offset: 0x000F03D8
	public TechInstance(Tech tech)
	{
		this.tech = tech;
	}

	// Token: 0x06007CC6 RID: 31942 RVA: 0x000F21FD File Offset: 0x000F03FD
	public bool IsComplete()
	{
		return this.complete;
	}

	// Token: 0x06007CC7 RID: 31943 RVA: 0x000F2205 File Offset: 0x000F0405
	public void Purchased()
	{
		if (!this.complete)
		{
			this.complete = true;
		}
	}

	// Token: 0x06007CC8 RID: 31944 RVA: 0x00322DC4 File Offset: 0x00320FC4
	public void UnlockPOITech(string tech_id)
	{
		TechItem techItem = Db.Get().TechItems.Get(tech_id);
		if (techItem == null || !techItem.isPOIUnlock)
		{
			return;
		}
		if (!this.UnlockedPOITechIds.Contains(tech_id))
		{
			this.UnlockedPOITechIds.Add(tech_id);
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef != null)
			{
				Game.Instance.Trigger(-107300940, buildingDef);
			}
		}
	}

	// Token: 0x06007CC9 RID: 31945 RVA: 0x00322E30 File Offset: 0x00321030
	public float GetTotalPercentageComplete()
	{
		float num = 0f;
		int num2 = 0;
		foreach (string type in this.progressInventory.PointsByTypeID.Keys)
		{
			if (this.tech.RequiresResearchType(type))
			{
				num += this.PercentageCompleteResearchType(type);
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x06007CCA RID: 31946 RVA: 0x000F2216 File Offset: 0x000F0416
	public float PercentageCompleteResearchType(string type)
	{
		if (!this.tech.RequiresResearchType(type))
		{
			return 1f;
		}
		return Mathf.Clamp01(this.progressInventory.PointsByTypeID[type] / this.tech.costsByResearchTypeID[type]);
	}

	// Token: 0x06007CCB RID: 31947 RVA: 0x00322EB0 File Offset: 0x003210B0
	public TechInstance.SaveData Save()
	{
		string[] array = new string[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Keys.CopyTo(array, 0);
		float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
		string[] unlockedPOIIDs = this.UnlockedPOITechIds.ToArray();
		return new TechInstance.SaveData
		{
			techId = this.tech.Id,
			complete = this.complete,
			inventoryIDs = array,
			inventoryValues = array2,
			unlockedPOIIDs = unlockedPOIIDs
		};
	}

	// Token: 0x06007CCC RID: 31948 RVA: 0x00322F64 File Offset: 0x00321164
	public void Load(TechInstance.SaveData save_data)
	{
		this.complete = save_data.complete;
		for (int i = 0; i < save_data.inventoryIDs.Length; i++)
		{
			this.progressInventory.AddResearchPoints(save_data.inventoryIDs[i], save_data.inventoryValues[i]);
		}
		if (save_data.unlockedPOIIDs != null)
		{
			this.UnlockedPOITechIds = new List<string>(save_data.unlockedPOIIDs);
		}
	}

	// Token: 0x04005E6D RID: 24173
	public Tech tech;

	// Token: 0x04005E6E RID: 24174
	private bool complete;

	// Token: 0x04005E6F RID: 24175
	public ResearchPointInventory progressInventory = new ResearchPointInventory();

	// Token: 0x04005E70 RID: 24176
	public List<string> UnlockedPOITechIds = new List<string>();

	// Token: 0x020017AC RID: 6060
	public struct SaveData
	{
		// Token: 0x04005E71 RID: 24177
		public string techId;

		// Token: 0x04005E72 RID: 24178
		public bool complete;

		// Token: 0x04005E73 RID: 24179
		public string[] inventoryIDs;

		// Token: 0x04005E74 RID: 24180
		public float[] inventoryValues;

		// Token: 0x04005E75 RID: 24181
		public string[] unlockedPOIIDs;
	}
}
