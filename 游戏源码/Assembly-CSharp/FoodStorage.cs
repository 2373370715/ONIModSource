using KSerialization;
using UnityEngine;

public class FoodStorage : KMonoBehaviour
{
	[Serialize]
	private bool onlyStoreSpicedFood;

	[MyCmpReq]
	public Storage storage;

	private static readonly EventSystem.IntraObjectHandler<FoodStorage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FoodStorage>(delegate(FoodStorage component, object data)
	{
		component.OnCopySettings(data);
	});

	public FilteredStorage FilteredStorage { get; set; }

	public bool SpicedFoodOnly
	{
		get
		{
			return onlyStoreSpicedFood;
		}
		set
		{
			onlyStoreSpicedFood = value;
			Trigger(1163645216, onlyStoreSpicedFood);
			if (onlyStoreSpicedFood)
			{
				FilteredStorage.AddForbiddenTag(GameTags.UnspicedFood);
				storage.DropHasTags(new Tag[2]
				{
					GameTags.Edible,
					GameTags.UnspicedFood
				});
			}
			else
			{
				FilteredStorage.RemoveForbiddenTag(GameTags.UnspicedFood);
			}
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-905833192, OnCopySettingsDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	private void OnCopySettings(object data)
	{
		FoodStorage component = ((GameObject)data).GetComponent<FoodStorage>();
		if (component != null)
		{
			SpicedFoodOnly = component.SpicedFoodOnly;
		}
	}
}
