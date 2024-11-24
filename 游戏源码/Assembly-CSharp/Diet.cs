using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020009DD RID: 2525
public class Diet
{
	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06002E64 RID: 11876 RVA: 0x000BE105 File Offset: 0x000BC305
	// (set) Token: 0x06002E65 RID: 11877 RVA: 0x000BE10D File Offset: 0x000BC30D
	public Diet.Info[] infos { get; private set; }

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06002E66 RID: 11878 RVA: 0x000BE116 File Offset: 0x000BC316
	// (set) Token: 0x06002E67 RID: 11879 RVA: 0x000BE11E File Offset: 0x000BC31E
	public Diet.Info[] noPlantInfos { get; private set; }

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06002E68 RID: 11880 RVA: 0x000BE127 File Offset: 0x000BC327
	// (set) Token: 0x06002E69 RID: 11881 RVA: 0x000BE12F File Offset: 0x000BC32F
	public Diet.Info[] directlyEatenPlantInfos { get; private set; }

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06002E6A RID: 11882 RVA: 0x000BE138 File Offset: 0x000BC338
	public bool CanEatAnyNonDirectlyEdiblePlant
	{
		get
		{
			return this.noPlantInfos != null && this.noPlantInfos.Length != 0;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06002E6B RID: 11883 RVA: 0x000BE14E File Offset: 0x000BC34E
	public bool CanEatAnyPlantDirectly
	{
		get
		{
			return this.directlyEatenPlantInfos != null && this.directlyEatenPlantInfos.Length != 0;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06002E6C RID: 11884 RVA: 0x000BE164 File Offset: 0x000BC364
	public bool AllConsumablesAreDirectlyEdiblePlants
	{
		get
		{
			return this.CanEatAnyPlantDirectly && (this.noPlantInfos == null || this.noPlantInfos.Length == 0);
		}
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x001F48B4 File Offset: 0x001F2AB4
	public bool IsConsumedTagAbleToBeEatenDirectly(Tag tag)
	{
		if (this.directlyEatenPlantInfos == null)
		{
			return false;
		}
		for (int i = 0; i < this.directlyEatenPlantInfos.Length; i++)
		{
			if (this.directlyEatenPlantInfos[i].consumedTags.Contains(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x001F48F8 File Offset: 0x001F2AF8
	private void UpdateSecondaryInfoArrays()
	{
		Diet.Info[] directlyEatenPlantInfos;
		if (this.infos != null)
		{
			directlyEatenPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatPlantDirectly || i.foodType == Diet.Info.FoodType.EatPlantStorage
			select i).ToArray<Diet.Info>();
		}
		else
		{
			directlyEatenPlantInfos = null;
		}
		this.directlyEatenPlantInfos = directlyEatenPlantInfos;
		Diet.Info[] noPlantInfos;
		if (this.infos != null)
		{
			noPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatSolid
			select i).ToArray<Diet.Info>();
		}
		else
		{
			noPlantInfos = null;
		}
		this.noPlantInfos = noPlantInfos;
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x001F4988 File Offset: 0x001F2B88
	public Diet(params Diet.Info[] infos)
	{
		this.infos = infos;
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		for (int i = 0; i < infos.Length; i++)
		{
			Diet.Info info = infos[i];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x001F4B2C File Offset: 0x001F2D2C
	public Diet(Diet diet)
	{
		this.infos = new Diet.Info[diet.infos.Length];
		for (int i = 0; i < diet.infos.Length; i++)
		{
			this.infos[i] = new Diet.Info(diet.infos[i]);
		}
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		Diet.Info[] infos = this.infos;
		for (int j = 0; j < infos.Length; j++)
		{
			Diet.Info info = infos[j];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x06002E71 RID: 11889 RVA: 0x001F4D08 File Offset: 0x001F2F08
	public Diet.Info GetDietInfo(Tag tag)
	{
		Diet.Info result = null;
		this.consumedTagToInfo.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06002E72 RID: 11890 RVA: 0x001F4D28 File Offset: 0x001F2F28
	public void FilterDLC()
	{
		foreach (Diet.Info info in this.infos)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag tag in info.consumedTags)
			{
				GameObject prefab = Assets.GetPrefab(tag);
				if (!SaveLoader.Instance.IsDlcListActiveForCurrentSave(prefab.GetComponent<KPrefabID>().requiredDlcIds))
				{
					list.Add(tag);
				}
			}
			using (List<Tag>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Tag invalid_tag = enumerator2.Current;
					info.consumedTags.Remove(invalid_tag);
					this.consumedTags.RemoveAll((KeyValuePair<Tag, float> t) => t.Key == invalid_tag);
					this.consumedTagToInfo.Remove(invalid_tag);
				}
			}
			GameObject gameObject = (info.producedElement != Tag.Invalid) ? Assets.GetPrefab(info.producedElement) : null;
			if (gameObject != null && !SaveLoader.Instance.IsDlcListActiveForCurrentSave(gameObject.GetComponent<KPrefabID>().requiredDlcIds))
			{
				info.consumedTags.Clear();
			}
		}
		this.infos = (from i in this.infos
		where i.consumedTags.Count > 0
		select i).ToArray<Diet.Info>();
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x04001F2E RID: 7982
	public List<KeyValuePair<Tag, float>> consumedTags;

	// Token: 0x04001F2F RID: 7983
	public List<KeyValuePair<Tag, float>> producedTags;

	// Token: 0x04001F30 RID: 7984
	private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

	// Token: 0x020009DE RID: 2526
	public class Info
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06002E73 RID: 11891 RVA: 0x000BE184 File Offset: 0x000BC384
		// (set) Token: 0x06002E74 RID: 11892 RVA: 0x000BE18C File Offset: 0x000BC38C
		public HashSet<Tag> consumedTags { get; private set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06002E75 RID: 11893 RVA: 0x000BE195 File Offset: 0x000BC395
		// (set) Token: 0x06002E76 RID: 11894 RVA: 0x000BE19D File Offset: 0x000BC39D
		public Tag producedElement { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06002E77 RID: 11895 RVA: 0x000BE1A6 File Offset: 0x000BC3A6
		// (set) Token: 0x06002E78 RID: 11896 RVA: 0x000BE1AE File Offset: 0x000BC3AE
		public float caloriesPerKg { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06002E79 RID: 11897 RVA: 0x000BE1B7 File Offset: 0x000BC3B7
		// (set) Token: 0x06002E7A RID: 11898 RVA: 0x000BE1BF File Offset: 0x000BC3BF
		public float producedConversionRate { get; private set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06002E7B RID: 11899 RVA: 0x000BE1C8 File Offset: 0x000BC3C8
		// (set) Token: 0x06002E7C RID: 11900 RVA: 0x000BE1D0 File Offset: 0x000BC3D0
		public byte diseaseIdx { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x000BE1D9 File Offset: 0x000BC3D9
		// (set) Token: 0x06002E7E RID: 11902 RVA: 0x000BE1E1 File Offset: 0x000BC3E1
		public float diseasePerKgProduced { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06002E7F RID: 11903 RVA: 0x000BE1EA File Offset: 0x000BC3EA
		// (set) Token: 0x06002E80 RID: 11904 RVA: 0x000BE1F2 File Offset: 0x000BC3F2
		public bool emmitDiseaseOnCell { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06002E81 RID: 11905 RVA: 0x000BE1FB File Offset: 0x000BC3FB
		// (set) Token: 0x06002E82 RID: 11906 RVA: 0x000BE203 File Offset: 0x000BC403
		public bool produceSolidTile { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06002E83 RID: 11907 RVA: 0x000BE20C File Offset: 0x000BC40C
		// (set) Token: 0x06002E84 RID: 11908 RVA: 0x000BE214 File Offset: 0x000BC414
		public Diet.Info.FoodType foodType { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06002E85 RID: 11909 RVA: 0x000BE21D File Offset: 0x000BC41D
		// (set) Token: 0x06002E86 RID: 11910 RVA: 0x000BE225 File Offset: 0x000BC425
		public string[] eatAnims { get; set; }

		// Token: 0x06002E87 RID: 11911 RVA: 0x001F4ED4 File Offset: 0x001F30D4
		public Info(HashSet<Tag> consumed_tags, Tag produced_element, float calories_per_kg, float produced_conversion_rate = 1f, string disease_id = null, float disease_per_kg_produced = 0f, bool produce_solid_tile = false, Diet.Info.FoodType food_type = Diet.Info.FoodType.EatSolid, bool emmit_disease_on_cell = false, string[] eat_anims = null)
		{
			this.consumedTags = consumed_tags;
			this.producedElement = produced_element;
			this.caloriesPerKg = calories_per_kg;
			this.producedConversionRate = produced_conversion_rate;
			if (!string.IsNullOrEmpty(disease_id))
			{
				this.diseaseIdx = Db.Get().Diseases.GetIndex(disease_id);
			}
			else
			{
				this.diseaseIdx = byte.MaxValue;
			}
			this.diseasePerKgProduced = disease_per_kg_produced;
			this.emmitDiseaseOnCell = emmit_disease_on_cell;
			this.produceSolidTile = produce_solid_tile;
			this.foodType = food_type;
			if (eat_anims == null)
			{
				eat_anims = new string[]
				{
					"eat_pre",
					"eat_loop",
					"eat_pst"
				};
			}
			this.eatAnims = eat_anims;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x001F4F84 File Offset: 0x001F3184
		public Info(Diet.Info info)
		{
			this.consumedTags = new HashSet<Tag>(info.consumedTags);
			this.producedElement = info.producedElement;
			this.caloriesPerKg = info.caloriesPerKg;
			this.producedConversionRate = info.producedConversionRate;
			this.diseaseIdx = info.diseaseIdx;
			this.diseasePerKgProduced = info.diseasePerKgProduced;
			this.emmitDiseaseOnCell = info.emmitDiseaseOnCell;
			this.produceSolidTile = info.produceSolidTile;
			this.foodType = info.foodType;
			this.eatAnims = info.eatAnims;
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000BE22E File Offset: 0x000BC42E
		public bool IsMatch(Tag tag)
		{
			return this.consumedTags.Contains(tag);
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x001F5014 File Offset: 0x001F3214
		public bool IsMatch(HashSet<Tag> tags)
		{
			if (tags.Count < this.consumedTags.Count)
			{
				foreach (Tag item in tags)
				{
					if (this.consumedTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (Tag item2 in this.consumedTags)
			{
				if (tags.Contains(item2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000BE23C File Offset: 0x000BC43C
		public float ConvertCaloriesToConsumptionMass(float calories)
		{
			return calories / this.caloriesPerKg;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000BE246 File Offset: 0x000BC446
		public float ConvertConsumptionMassToCalories(float mass)
		{
			return this.caloriesPerKg * mass;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000BE250 File Offset: 0x000BC450
		public float ConvertConsumptionMassToProducedMass(float consumed_mass)
		{
			return consumed_mass * this.producedConversionRate;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000BE25A File Offset: 0x000BC45A
		public float ConvertProducedMassToConsumptionMass(float produced_mass)
		{
			return produced_mass / this.producedConversionRate;
		}

		// Token: 0x020009DF RID: 2527
		public enum FoodType
		{
			// Token: 0x04001F3C RID: 7996
			EatSolid,
			// Token: 0x04001F3D RID: 7997
			EatPlantDirectly,
			// Token: 0x04001F3E RID: 7998
			EatPlantStorage
		}
	}
}
