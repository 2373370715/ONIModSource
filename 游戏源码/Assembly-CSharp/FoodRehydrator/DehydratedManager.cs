using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace FoodRehydrator;

public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen
{
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	private Storage packages;

	private Storage water;

	private MeterController packagesMeter;

	private static string HASH_FOOD = "food";

	private KBatchedAnimController foodKBAC;

	private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DehydratedManager>(delegate(DehydratedManager component, object data)
	{
		component.OnCopySettings(data);
	});

	[Serialize]
	private Tag chosenContent = GameTags.Dehydrated;

	public Tag ChosenContent
	{
		get
		{
			return chosenContent;
		}
		set
		{
			if (!(chosenContent != value))
			{
				return;
			}
			GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
			chosenContent = value;
			packages.DropUnlessHasTag(chosenContent);
			if (chosenContent != GameTags.Dehydrated)
			{
				AccessabilityManager component = GetComponent<AccessabilityManager>();
				if (component != null)
				{
					component.CancelActiveWorkable();
				}
			}
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-905833192, OnCopySettingsDelegate);
	}

	public void SetFabricatedFoodSymbol(Tag material)
	{
		foodKBAC.gameObject.SetActive(value: true);
		GameObject prefab = Assets.GetPrefab(material);
		foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
		foodKBAC.Play("object", KAnim.PlayMode.Loop);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = GetComponents<Storage>();
		Debug.Assert(components.Length == 2);
		packages = components[0];
		water = components[1];
		packagesMeter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, "meter_target");
		Subscribe(-1697596308, StorageChangeHandler);
		SetupFoodSymbol();
		packagesMeter.SetPositionPercent((float)packages.items.Count / 5f);
	}

	public void ConsumeResourcesForRehydration(GameObject package, GameObject food)
	{
		Debug.Assert(packages.items.Contains(package));
		packages.ConsumeIgnoringDisease(package);
		water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out var _, out var disease_info, out var aggregate_temperature);
		PrimaryElement component = food.GetComponent<PrimaryElement>();
		if (component != null)
		{
			component.AddDisease(disease_info.idx, disease_info.count, "rehydrating");
			component.SetMassTemperature(component.Mass, component.Temperature * 0.125f + aggregate_temperature * 0.875f);
		}
	}

	private void StorageChangeHandler(object obj)
	{
		if (((GameObject)obj).GetComponent<DehydratedFoodPackage>() != null)
		{
			packagesMeter.SetPositionPercent((float)packages.items.Count / 5f);
		}
	}

	private void SetupFoodSymbol()
	{
		GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
		gameObject.SetActive(value: false);
		KBatchedAnimController component = GetComponent<KBatchedAnimController>();
		bool symbolVisible;
		Vector3 position = component.GetSymbolTransform(HASH_FOOD, out symbolVisible).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		gameObject.transform.SetPosition(position);
		foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
		foodKBAC.AnimFiles = new KAnimFile[1] { Assets.GetAnim("mushbar_kanim") };
		foodKBAC.initialAnim = "object";
		component.SetSymbolVisiblity(HASH_FOOD, is_visible: false);
		foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
		KBatchedAnimTracker kBatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kBatchedAnimTracker.symbol = new HashedString("food");
		kBatchedAnimTracker.offset = Vector3.zero;
	}

	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + discoveredResourcesFromTag.Count];
		array[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated, UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD, Def.GetUISprite("icon_category_food"));
		int num = 1;
		foreach (Tag item in discoveredResourcesFromTag)
		{
			array[num] = new FewOptionSideScreen.IFewOptionSideScreen.Option(item, item.ProperName(), Def.GetUISprite(item));
			num++;
		}
		return array;
	}

	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		ChosenContent = option.tag;
	}

	public Tag GetSelectedOption()
	{
		return chosenContent;
	}

	protected void OnCopySettings(object data)
	{
		if (data is GameObject gameObject)
		{
			DehydratedManager component = gameObject.GetComponent<DehydratedManager>();
			if (component != null)
			{
				ChosenContent = component.ChosenContent;
			}
		}
	}
}
