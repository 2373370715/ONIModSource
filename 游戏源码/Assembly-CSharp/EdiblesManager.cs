using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200126D RID: 4717
[AddComponentMenu("KMonoBehaviour/scripts/EdiblesManager")]
public class EdiblesManager : KMonoBehaviour
{
	// Token: 0x060060B1 RID: 24753 RVA: 0x000DF127 File Offset: 0x000DD327
	public static List<EdiblesManager.FoodInfo> GetAllLoadedFoodTypes()
	{
		return (from x in EdiblesManager.s_allFoodTypes
		where DlcManager.IsContentSubscribed(x.DlcId)
		select x).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x002B0BE0 File Offset: 0x002AEDE0
	public static List<EdiblesManager.FoodInfo> GetAllFoodTypes()
	{
		global::Debug.Assert(SaveLoader.Instance != null, "Call GetAllLoadedFoodTypes from the frontend");
		return (from x in EdiblesManager.s_allFoodTypes
		where SaveLoader.Instance.IsDLCActiveForCurrentSave(x.DlcId)
		select x).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x002B0C30 File Offset: 0x002AEE30
	public static EdiblesManager.FoodInfo GetFoodInfo(string foodID)
	{
		string key = foodID.Replace("Compost", "");
		EdiblesManager.FoodInfo result = null;
		EdiblesManager.s_allFoodMap.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x060060B4 RID: 24756 RVA: 0x000DF157 File Offset: 0x000DD357
	public static bool TryGetFoodInfo(string foodID, out EdiblesManager.FoodInfo info)
	{
		info = null;
		if (string.IsNullOrEmpty(foodID))
		{
			return false;
		}
		info = EdiblesManager.GetFoodInfo(foodID);
		return info != null;
	}

	// Token: 0x04004488 RID: 17544
	private static List<EdiblesManager.FoodInfo> s_allFoodTypes = new List<EdiblesManager.FoodInfo>();

	// Token: 0x04004489 RID: 17545
	private static Dictionary<string, EdiblesManager.FoodInfo> s_allFoodMap = new Dictionary<string, EdiblesManager.FoodInfo>();

	// Token: 0x0200126E RID: 4718
	public class FoodInfo : IConsumableUIItem
	{
		// Token: 0x060060B7 RID: 24759 RVA: 0x002B0C60 File Offset: 0x002AEE60
		public FoodInfo(string id, string dlcId, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot)
		{
			this.Id = id;
			this.DlcId = dlcId;
			this.CaloriesPerUnit = caloriesPerUnit;
			this.Quality = quality;
			this.PreserveTemperature = preserveTemperatue;
			this.RotTemperature = rotTemperature;
			this.StaleTime = spoilTime / 2f;
			this.SpoilTime = spoilTime;
			this.CanRot = can_rot;
			this.Name = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".NAME");
			this.Description = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".DESC");
			this.Effects = new List<string>();
			EdiblesManager.s_allFoodTypes.Add(this);
			EdiblesManager.s_allFoodMap[this.Id] = this;
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x000DF189 File Offset: 0x000DD389
		public EdiblesManager.FoodInfo AddEffects(List<string> effects, string[] dlcIds)
		{
			if (DlcManager.IsDlcListValidForCurrentContent(dlcIds))
			{
				this.Effects.AddRange(effects);
			}
			return this;
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060060B9 RID: 24761 RVA: 0x000DF1A0 File Offset: 0x000DD3A0
		public string ConsumableId
		{
			get
			{
				return this.Id;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060060BA RID: 24762 RVA: 0x000DF1A8 File Offset: 0x000DD3A8
		public string ConsumableName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060060BB RID: 24763 RVA: 0x000DF1B0 File Offset: 0x000DD3B0
		public int MajorOrder
		{
			get
			{
				return this.Quality;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060060BC RID: 24764 RVA: 0x000DF1B8 File Offset: 0x000DD3B8
		public int MinorOrder
		{
			get
			{
				return (int)this.CaloriesPerUnit;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060060BD RID: 24765 RVA: 0x000DF1C1 File Offset: 0x000DD3C1
		public bool Display
		{
			get
			{
				return this.CaloriesPerUnit != 0f;
			}
		}

		// Token: 0x0400448A RID: 17546
		public string Id;

		// Token: 0x0400448B RID: 17547
		public string DlcId;

		// Token: 0x0400448C RID: 17548
		public string Name;

		// Token: 0x0400448D RID: 17549
		public string Description;

		// Token: 0x0400448E RID: 17550
		public float CaloriesPerUnit;

		// Token: 0x0400448F RID: 17551
		public float PreserveTemperature;

		// Token: 0x04004490 RID: 17552
		public float RotTemperature;

		// Token: 0x04004491 RID: 17553
		public float StaleTime;

		// Token: 0x04004492 RID: 17554
		public float SpoilTime;

		// Token: 0x04004493 RID: 17555
		public bool CanRot;

		// Token: 0x04004494 RID: 17556
		public int Quality;

		// Token: 0x04004495 RID: 17557
		public List<string> Effects;
	}
}
