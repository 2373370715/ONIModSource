using System.Collections.Generic;
using UnityEngine;

public class TechInstance
{
	public struct SaveData
	{
		public string techId;

		public bool complete;

		public string[] inventoryIDs;

		public float[] inventoryValues;

		public string[] unlockedPOIIDs;
	}

	public Tech tech;

	private bool complete;

	public ResearchPointInventory progressInventory = new ResearchPointInventory();

	public List<string> UnlockedPOITechIds = new List<string>();

	public TechInstance(Tech tech)
	{
		this.tech = tech;
	}

	public bool IsComplete()
	{
		return complete;
	}

	public void Purchased()
	{
		if (!complete)
		{
			complete = true;
		}
	}

	public void UnlockPOITech(string tech_id)
	{
		TechItem techItem = Db.Get().TechItems.Get(tech_id);
		if (techItem != null && techItem.isPOIUnlock && !UnlockedPOITechIds.Contains(tech_id))
		{
			UnlockedPOITechIds.Add(tech_id);
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef != null)
			{
				Game.Instance.Trigger(-107300940, buildingDef);
			}
		}
	}

	public float GetTotalPercentageComplete()
	{
		float num = 0f;
		int num2 = 0;
		foreach (string key in progressInventory.PointsByTypeID.Keys)
		{
			if (tech.RequiresResearchType(key))
			{
				num += PercentageCompleteResearchType(key);
				num2++;
			}
		}
		return num / (float)num2;
	}

	public float PercentageCompleteResearchType(string type)
	{
		if (!tech.RequiresResearchType(type))
		{
			return 1f;
		}
		return Mathf.Clamp01(progressInventory.PointsByTypeID[type] / tech.costsByResearchTypeID[type]);
	}

	public SaveData Save()
	{
		string[] array = new string[progressInventory.PointsByTypeID.Count];
		progressInventory.PointsByTypeID.Keys.CopyTo(array, 0);
		float[] array2 = new float[progressInventory.PointsByTypeID.Count];
		progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
		string[] unlockedPOIIDs = UnlockedPOITechIds.ToArray();
		SaveData result = default(SaveData);
		result.techId = tech.Id;
		result.complete = complete;
		result.inventoryIDs = array;
		result.inventoryValues = array2;
		result.unlockedPOIIDs = unlockedPOIIDs;
		return result;
	}

	public void Load(SaveData save_data)
	{
		complete = save_data.complete;
		for (int i = 0; i < save_data.inventoryIDs.Length; i++)
		{
			progressInventory.AddResearchPoints(save_data.inventoryIDs[i], save_data.inventoryValues[i]);
		}
		if (save_data.unlockedPOIIDs != null)
		{
			UnlockedPOITechIds = new List<string>(save_data.unlockedPOIIDs);
		}
	}
}
