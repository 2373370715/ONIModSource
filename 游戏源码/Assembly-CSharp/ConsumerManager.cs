using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001109 RID: 4361
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600596A RID: 22890 RVA: 0x000DA390 File Offset: 0x000D8590
	public static void DestroyInstance()
	{
		ConsumerManager.instance = null;
	}

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x0600596B RID: 22891 RVA: 0x00290CD0 File Offset: 0x0028EED0
	// (remove) Token: 0x0600596C RID: 22892 RVA: 0x00290D08 File Offset: 0x0028EF08
	public event Action<Tag> OnDiscover;

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x0600596D RID: 22893 RVA: 0x000DA398 File Offset: 0x000D8598
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x0600596E RID: 22894 RVA: 0x00290D40 File Offset: 0x0028EF40
	public List<Tag> StandardDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
			{
				list.Add(go.PrefabID());
			}
			return list;
		}
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x0600596F RID: 22895 RVA: 0x00290DA4 File Offset: 0x0028EFA4
	public List<Tag> BionicDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Edible))
			{
				list.Add(go.PrefabID());
			}
			return list;
		}
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x00290E08 File Offset: 0x0028F008
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConsumerManager.instance = this;
		this.RefreshDiscovered(null);
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x000DA3A0 File Offset: 0x000D85A0
	public bool isDiscovered(Tag id)
	{
		return !this.undiscoveredConsumableTags.Contains(id);
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x000DA3B1 File Offset: 0x000D85B1
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		if (this.undiscoveredConsumableTags.Contains(tag))
		{
			this.RefreshDiscovered(null);
		}
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x00290E5C File Offset: 0x0028F05C
	public void RefreshDiscovered(object data = null)
	{
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			if (!this.ShouldBeDiscovered(foodInfo.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Add(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover("UndiscoveredSomething".ToTag());
				}
			}
			else if (this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()) && this.ShouldBeDiscovered(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Remove(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover(foodInfo.Id.ToTag());
				}
				if (!DiscoveredResources.Instance.IsDiscovered(foodInfo.Id.ToTag()))
				{
					if (foodInfo.CaloriesPerUnit == 0f)
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.CookingIngredient);
					}
					else
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.Edible);
					}
				}
			}
		}
	}

	// Token: 0x06005974 RID: 22900 RVA: 0x00290FE0 File Offset: 0x0028F1E0
	private bool ShouldBeDiscovered(Tag food_id)
	{
		if (DiscoveredResources.Instance.IsDiscovered(food_id))
		{
			return true;
		}
		foreach (Recipe recipe in RecipeManager.Get().recipes)
		{
			if (recipe.Result == food_id)
			{
				foreach (string id in recipe.fabricators)
				{
					if (Db.Get().TechItems.IsTechItemComplete(id))
					{
						return true;
					}
				}
			}
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003F1F RID: 16159
	public static ConsumerManager instance;

	// Token: 0x04003F21 RID: 16161
	[Serialize]
	private List<Tag> undiscoveredConsumableTags = new List<Tag>();

	// Token: 0x04003F22 RID: 16162
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();
}
