using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A61 RID: 2657
public class FoodStorage : KMonoBehaviour
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060030EE RID: 12526 RVA: 0x000BFCD6 File Offset: 0x000BDED6
	// (set) Token: 0x060030EF RID: 12527 RVA: 0x000BFCDE File Offset: 0x000BDEDE
	public FilteredStorage FilteredStorage { get; set; }

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000BFCE7 File Offset: 0x000BDEE7
	// (set) Token: 0x060030F1 RID: 12529 RVA: 0x001FDE18 File Offset: 0x001FC018
	public bool SpicedFoodOnly
	{
		get
		{
			return this.onlyStoreSpicedFood;
		}
		set
		{
			this.onlyStoreSpicedFood = value;
			base.Trigger(1163645216, this.onlyStoreSpicedFood);
			if (this.onlyStoreSpicedFood)
			{
				this.FilteredStorage.AddForbiddenTag(GameTags.UnspicedFood);
				this.storage.DropHasTags(new Tag[]
				{
					GameTags.Edible,
					GameTags.UnspicedFood
				});
				return;
			}
			this.FilteredStorage.RemoveForbiddenTag(GameTags.UnspicedFood);
		}
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x000BFCEF File Offset: 0x000BDEEF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FoodStorage>(-905833192, FoodStorage.OnCopySettingsDelegate);
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x001FDE98 File Offset: 0x001FC098
	private void OnCopySettings(object data)
	{
		FoodStorage component = ((GameObject)data).GetComponent<FoodStorage>();
		if (component != null)
		{
			this.SpicedFoodOnly = component.SpicedFoodOnly;
		}
	}

	// Token: 0x04002112 RID: 8466
	[Serialize]
	private bool onlyStoreSpicedFood;

	// Token: 0x04002113 RID: 8467
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002115 RID: 8469
	private static readonly EventSystem.IntraObjectHandler<FoodStorage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FoodStorage>(delegate(FoodStorage component, object data)
	{
		component.OnCopySettings(data);
	});
}
