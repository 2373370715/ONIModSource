using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class CodexEntryGenerator_Elements
{
	public class ConversionEntry
	{
		public string title;

		public GameObject prefab;

		public HashSet<ElementUsage> inSet = new HashSet<ElementUsage>();

		public HashSet<ElementUsage> outSet = new HashSet<ElementUsage>();
	}

	public class CodexElementMap
	{
		public Dictionary<Tag, List<ConversionEntry>> map = new Dictionary<Tag, List<ConversionEntry>>();

		public void Add(Tag t, ConversionEntry ce)
		{
			if (map.TryGetValue(t, out var value))
			{
				value.Add(ce);
				return;
			}
			map[t] = new List<ConversionEntry> { ce };
		}
	}

	public class ElementEntryContext
	{
		public CodexElementMap madeMap = new CodexElementMap();

		public CodexElementMap usedMap = new CodexElementMap();
	}

	public static string ELEMENTS_ID = CodexCache.FormatLinkID("ELEMENTS");

	public static string ELEMENTS_SOLIDS_ID = CodexCache.FormatLinkID("ELEMENTS_SOLID");

	public static string ELEMENTS_LIQUIDS_ID = CodexCache.FormatLinkID("ELEMENTS_LIQUID");

	public static string ELEMENTS_GASES_ID = CodexCache.FormatLinkID("ELEMENTS_GAS");

	public static string ELEMENTS_OTHER_ID = CodexCache.FormatLinkID("ELEMENTS_OTHER");

	private static ElementEntryContext contextInstance;

	public static Dictionary<string, CodexEntry> GenerateEntries()
	{
		Dictionary<string, CodexEntry> entriesElements = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary2 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary3 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary4 = new Dictionary<string, CodexEntry>();
		AddCategoryEntry(ELEMENTS_SOLIDS_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSSOLID, Assets.GetSprite("ui_elements-solid"), dictionary);
		AddCategoryEntry(ELEMENTS_LIQUIDS_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID, Assets.GetSprite("ui_elements-liquids"), dictionary2);
		AddCategoryEntry(ELEMENTS_GASES_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSGAS, Assets.GetSprite("ui_elements-gases"), dictionary3);
		AddCategoryEntry(ELEMENTS_OTHER_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSOTHER, Assets.GetSprite("ui_elements-other"), dictionary4);
		foreach (Element element in ElementLoader.elements)
		{
			if (element.disabled)
			{
				continue;
			}
			bool flag = false;
			Tag[] oreTags = element.oreTags;
			for (int i = 0; i < oreTags.Length; i++)
			{
				if (oreTags[i] == GameTags.HideFromCodex)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			Tuple<Sprite, Color> tuple = Def.GetUISprite(element);
			if (tuple.first == null)
			{
				if (element.id == SimHashes.Void)
				{
					tuple = new Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-void"), Color.white);
				}
				else if (element.id == SimHashes.Vacuum)
				{
					tuple = new Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-vacuum"), Color.white);
				}
			}
			List<ContentContainer> list = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(element.name, list);
			CodexEntryGenerator.GenerateImageContainers(new Tuple<Sprite, Color>[1] { tuple }, list, ContentContainer.ContentLayout.Horizontal);
			GenerateElementDescriptionContainers(element, list);
			string text;
			Dictionary<string, CodexEntry> dictionary5;
			if (element.IsSolid)
			{
				text = ELEMENTS_SOLIDS_ID;
				dictionary5 = dictionary;
			}
			else if (element.IsLiquid)
			{
				text = ELEMENTS_LIQUIDS_ID;
				dictionary5 = dictionary2;
			}
			else if (element.IsGas)
			{
				text = ELEMENTS_GASES_ID;
				dictionary5 = dictionary3;
			}
			else
			{
				text = ELEMENTS_OTHER_ID;
				dictionary5 = dictionary4;
			}
			string text2 = element.id.ToString();
			CodexEntry codexEntry = new CodexEntry(text, list, element.name);
			codexEntry.parentId = text;
			codexEntry.icon = tuple.first;
			codexEntry.iconColor = tuple.second;
			CodexCache.AddEntry(text2, codexEntry);
			dictionary5.Add(text2, codexEntry);
		}
		string text3 = "IceBellyPoop";
		GameObject gameObject = Assets.TryGetPrefab(text3);
		if (gameObject != null)
		{
			string eLEMENTS_SOLIDS_ID = ELEMENTS_SOLIDS_ID;
			Dictionary<string, CodexEntry> dictionary6 = dictionary;
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			InfoDescription component2 = gameObject.GetComponent<InfoDescription>();
			string properName = gameObject.GetProperName();
			string description = component2.description;
			Tuple<Sprite, Color> uISprite = Def.GetUISprite(gameObject);
			List<ContentContainer> list2 = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(properName, list2);
			CodexEntryGenerator.GenerateImageContainers(new Tuple<Sprite, Color>[1] { uISprite }, list2, ContentContainer.ContentLayout.Horizontal);
			GenerateMadeAndUsedContainers(component.PrefabTag, list2);
			list2.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(description),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical));
			CodexEntry codexEntry2 = new CodexEntry(eLEMENTS_SOLIDS_ID, list2, properName);
			codexEntry2.parentId = eLEMENTS_SOLIDS_ID;
			codexEntry2.icon = uISprite.first;
			codexEntry2.iconColor = uISprite.second;
			CodexCache.AddEntry(text3, codexEntry2);
			dictionary6.Add(text3, codexEntry2);
		}
		CodexEntryGenerator.PopulateCategoryEntries(entriesElements);
		return entriesElements;
		void AddCategoryEntry(string categoryId, string name, Sprite icon, Dictionary<string, CodexEntry> entries)
		{
			CodexEntry codexEntry3 = CodexEntryGenerator.GenerateCategoryEntry(categoryId, name, entries, icon);
			codexEntry3.parentId = ELEMENTS_ID;
			codexEntry3.category = ELEMENTS_ID;
			entriesElements.Add(categoryId, codexEntry3);
		}
	}

	public static void GenerateElementDescriptionContainers(Element element, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		if (element.highTempTransition != null)
		{
			list.Add(new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.HEAT));
		}
		if (element.lowTempTransition != null)
		{
			list.Add(new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.COOL));
		}
		foreach (Element element2 in ElementLoader.elements)
		{
			if (!element2.disabled)
			{
				if (element2.highTempTransition == element || ElementLoader.FindElementByHash(element2.highTempTransitionOreID) == element)
				{
					list2.Add(new CodexTemperatureTransitionPanel(element2, CodexTemperatureTransitionPanel.TransitionType.HEAT));
				}
				if (element2.lowTempTransition == element || ElementLoader.FindElementByHash(element2.lowTempTransitionOreID) == element)
				{
					list2.Add(new CodexTemperatureTransitionPanel(element2, CodexTemperatureTransitionPanel.TransitionType.COOL));
				}
			}
		}
		if (list.Count > 0)
		{
			ContentContainer contentContainer = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTTRANSITIONSTO, contentContainer)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer);
		}
		if (list2.Count > 0)
		{
			ContentContainer contentContainer2 = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTTRANSITIONSFROM, contentContainer2)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer2);
		}
		GenerateMadeAndUsedContainers(element.tag, containers);
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexSpacer(),
			new CodexText(element.FullDescription()),
			new CodexSpacer()
		}, ContentContainer.ContentLayout.Vertical));
	}

	public static void GenerateMadeAndUsedContainers(Tag tag, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		foreach (ComplexRecipe recipe in ComplexRecipeManager.Get().recipes)
		{
			if (recipe.ingredients.Any((ComplexRecipe.RecipeElement i) => i.material == tag))
			{
				list.Add(new CodexRecipePanel(recipe));
			}
			if (recipe.results.Any((ComplexRecipe.RecipeElement i) => i.material == tag))
			{
				list2.Add(new CodexRecipePanel(recipe, shouldUseFabricatorForTitle: true));
			}
		}
		if (GetElementEntryContext().usedMap.map.TryGetValue(tag, out var value))
		{
			foreach (ConversionEntry item in value)
			{
				list.Add(new CodexConversionPanel(item.title, item.inSet.ToArray(), item.outSet.ToArray(), item.prefab));
			}
		}
		if (GetElementEntryContext().madeMap.map.TryGetValue(tag, out var value2))
		{
			foreach (ConversionEntry item2 in value2)
			{
				list2.Add(new CodexConversionPanel(item2.title, item2.inSet.ToArray(), item2.outSet.ToArray(), item2.prefab));
			}
		}
		ContentContainer contentContainer = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
		ContentContainer contentContainer2 = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
		if (list.Count > 0)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTCONSUMEDBY, contentContainer)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer);
		}
		if (list2.Count > 0)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTPRODUCEDBY, contentContainer2)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer2);
		}
	}

	public static ElementEntryContext GetElementEntryContext()
	{
		CodexElementMap madeMap;
		Tag waterTag;
		Tag dirtyWaterTag;
		if (contextInstance == null)
		{
			CodexElementMap codexElementMap = new CodexElementMap();
			madeMap = new CodexElementMap();
			waterTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
			dirtyWaterTag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
			foreach (PlanScreen.PlanInfo item2 in TUNING.BUILDINGS.PLANORDER)
			{
				foreach (KeyValuePair<string, string> buildingAndSubcategoryDatum in item2.buildingAndSubcategoryData)
				{
					BuildingDef buildingDef = Assets.GetBuildingDef(buildingAndSubcategoryDatum.Key);
					if (buildingDef == null)
					{
						Debug.LogError("Building def for id " + buildingAndSubcategoryDatum.Key + " is null");
					}
					if (!buildingDef.Deprecated && !buildingDef.BuildingComplete.HasTag(GameTags.DevBuilding))
					{
						CheckPrefab(buildingDef.BuildingComplete, codexElementMap, madeMap);
					}
				}
			}
			HashSet<GameObject> hashSet = new HashSet<GameObject>(Assets.GetPrefabsWithComponent<Harvestable>());
			foreach (GameObject item3 in Assets.GetPrefabsWithComponent<WiltCondition>())
			{
				hashSet.Add(item3);
			}
			foreach (GameObject item4 in hashSet)
			{
				if (!(item4.GetComponent<BudUprootedMonitor>() != null))
				{
					CheckPrefab(item4, codexElementMap, madeMap);
				}
			}
			foreach (GameObject item5 in Assets.GetPrefabsWithComponent<CreatureBrain>())
			{
				if (item5.GetDef<BabyMonitor.Def>() == null)
				{
					CheckPrefab(item5, codexElementMap, madeMap);
				}
			}
			foreach (KeyValuePair<Tag, Diet> item6 in DietManager.CollectSaveDiets(null))
			{
				GameObject gameObject = Assets.GetPrefab(item6.Key).gameObject;
				if (gameObject.GetDef<BabyMonitor.Def>() != null)
				{
					continue;
				}
				float num = 0f;
				foreach (AttributeModifier selfModifier in Db.Get().traits.Get(gameObject.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
				{
					if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
					{
						num = selfModifier.Value;
					}
				}
				Diet value = item6.Value;
				Diet.Info[] infos = value.infos;
				foreach (Diet.Info info in infos)
				{
					float num2 = (0f - num) / info.caloriesPerKg;
					float amount = num2 * info.producedConversionRate;
					foreach (Tag consumedTag in info.consumedTags)
					{
						ElementUsage item = (value.IsConsumedTagAbleToBeEatenDirectly(consumedTag) ? new ElementUsage(consumedTag, num2, continuous: true, GameUtil.GetFormattedPlantConsumptionValuePerCycle) : new ElementUsage(consumedTag, num2, continuous: true));
						ConversionEntry conversionEntry = new ConversionEntry();
						conversionEntry.title = gameObject.GetProperName();
						conversionEntry.prefab = gameObject;
						conversionEntry.inSet = new HashSet<ElementUsage>();
						conversionEntry.inSet.Add(item);
						conversionEntry.outSet = new HashSet<ElementUsage>();
						conversionEntry.outSet.Add(new ElementUsage(info.producedElement, amount, continuous: true));
						codexElementMap.Add(consumedTag, conversionEntry);
						madeMap.Add(info.producedElement, conversionEntry);
					}
				}
			}
			contextInstance = new ElementEntryContext
			{
				usedMap = codexElementMap,
				madeMap = madeMap
			};
		}
		return contextInstance;
		void CheckPrefab(GameObject prefab, CodexElementMap usedMap, CodexElementMap made)
		{
			HashSet<ElementUsage> hashSet2 = new HashSet<ElementUsage>();
			HashSet<ElementUsage> hashSet3 = new HashSet<ElementUsage>();
			EnergyGenerator component = prefab.GetComponent<EnergyGenerator>();
			if ((bool)component)
			{
				IEnumerable<EnergyGenerator.InputItem> inputs = component.formula.inputs;
				foreach (EnergyGenerator.InputItem item7 in inputs ?? Enumerable.Empty<EnergyGenerator.InputItem>())
				{
					hashSet2.Add(new ElementUsage(item7.tag, item7.consumptionRate, continuous: true));
				}
				IEnumerable<EnergyGenerator.OutputItem> outputs = component.formula.outputs;
				foreach (EnergyGenerator.OutputItem item8 in outputs ?? Enumerable.Empty<EnergyGenerator.OutputItem>())
				{
					Tag tag = ElementLoader.FindElementByHash(item8.element).tag;
					hashSet3.Add(new ElementUsage(tag, item8.creationRate, continuous: true));
				}
			}
			IEnumerable<ElementConverter> components = prefab.GetComponents<ElementConverter>();
			foreach (ElementConverter item9 in components ?? Enumerable.Empty<ElementConverter>())
			{
				IEnumerable<ElementConverter.ConsumedElement> consumedElements = item9.consumedElements;
				foreach (ElementConverter.ConsumedElement item10 in consumedElements ?? Enumerable.Empty<ElementConverter.ConsumedElement>())
				{
					hashSet2.Add(new ElementUsage(item10.Tag, item10.MassConsumptionRate, continuous: true));
				}
				IEnumerable<ElementConverter.OutputElement> outputElements = item9.outputElements;
				foreach (ElementConverter.OutputElement item11 in outputElements ?? Enumerable.Empty<ElementConverter.OutputElement>())
				{
					Tag tag2 = ElementLoader.FindElementByHash(item11.elementHash).tag;
					hashSet3.Add(new ElementUsage(tag2, item11.massGenerationRate, continuous: true));
				}
			}
			IEnumerable<ElementConsumer> components2 = prefab.GetComponents<ElementConsumer>();
			foreach (ElementConsumer item12 in components2 ?? Enumerable.Empty<ElementConsumer>())
			{
				if (!item12.storeOnConsume)
				{
					Tag tag3 = ElementLoader.FindElementByHash(item12.elementToConsume).tag;
					hashSet2.Add(new ElementUsage(tag3, item12.consumptionRate, continuous: true));
				}
			}
			IrrigationMonitor.Def def = prefab.GetDef<IrrigationMonitor.Def>();
			if (def != null)
			{
				PlantElementAbsorber.ConsumeInfo[] consumedElements2 = def.consumedElements;
				for (int j = 0; j < consumedElements2.Length; j++)
				{
					PlantElementAbsorber.ConsumeInfo consumeInfo = consumedElements2[j];
					hashSet2.Add(new ElementUsage(consumeInfo.tag, consumeInfo.massConsumptionRate, continuous: true));
				}
			}
			FertilizationMonitor.Def def2 = prefab.GetDef<FertilizationMonitor.Def>();
			if (def2 != null)
			{
				PlantElementAbsorber.ConsumeInfo[] consumedElements2 = def2.consumedElements;
				for (int j = 0; j < consumedElements2.Length; j++)
				{
					PlantElementAbsorber.ConsumeInfo consumeInfo2 = consumedElements2[j];
					hashSet2.Add(new ElementUsage(consumeInfo2.tag, consumeInfo2.massConsumptionRate, continuous: true));
				}
			}
			Crop component2 = prefab.GetComponent<Crop>();
			if (component2 != null)
			{
				hashSet3.Add(new ElementUsage(component2.cropId, (float)component2.cropVal.numProduced / component2.cropVal.cropDuration, continuous: true));
			}
			FlushToilet component3 = prefab.GetComponent<FlushToilet>();
			if ((bool)component3)
			{
				hashSet2.Add(new ElementUsage(waterTag, component3.massConsumedPerUse, continuous: false));
				hashSet3.Add(new ElementUsage(dirtyWaterTag, component3.massEmittedPerUse, continuous: false));
			}
			HandSanitizer component4 = prefab.GetComponent<HandSanitizer>();
			if ((bool)component4)
			{
				Tag tag4 = ElementLoader.FindElementByHash(component4.consumedElement).tag;
				hashSet2.Add(new ElementUsage(tag4, component4.massConsumedPerUse, continuous: false));
				if (component4.outputElement != SimHashes.Vacuum)
				{
					Tag tag5 = ElementLoader.FindElementByHash(component4.outputElement).tag;
					hashSet3.Add(new ElementUsage(tag5, component4.massConsumedPerUse, continuous: false));
				}
			}
			if (prefab.IsPrefabID("Moo"))
			{
				hashSet2.Add(new ElementUsage("GasGrass", MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE, continuous: false));
				hashSet3.Add(new ElementUsage(ElementLoader.FindElementByHash(SimHashes.Milk).tag, MooTuning.MILK_PER_CYCLE, continuous: false));
			}
			ConversionEntry ce = new ConversionEntry
			{
				title = prefab.GetProperName(),
				prefab = prefab,
				inSet = hashSet2,
				outSet = hashSet3
			};
			if (hashSet2.Count > 0 && hashSet3.Count > 0)
			{
				usedMap.Add(prefab.PrefabID(), ce);
			}
			foreach (ElementUsage item13 in hashSet2)
			{
				usedMap.Add(item13.tag, ce);
			}
			foreach (ElementUsage item14 in hashSet3)
			{
				madeMap.Add(item14.tag, ce);
			}
			PlantBranchGrower.Def def3 = prefab.GetDef<PlantBranchGrower.Def>();
			if (def3 != null)
			{
				GameObject prefab2 = Assets.GetPrefab(def3.BRANCH_PREFAB_NAME);
				if (prefab2 != null)
				{
					Crop component5 = prefab2.GetComponent<Crop>();
					if (component5 != null && (component2 == null || component5.cropId != component2.cropId || component5.cropVal.numProduced != component2.cropVal.numProduced))
					{
						ConversionEntry conversionEntry2 = new ConversionEntry
						{
							title = prefab2.GetProperName(),
							prefab = prefab
						};
						usedMap.Add(prefab.PrefabID(), conversionEntry2);
						conversionEntry2.inSet = new HashSet<ElementUsage>();
						conversionEntry2.outSet = new HashSet<ElementUsage>();
						conversionEntry2.outSet.Add(new ElementUsage(component5.cropId, component5.cropVal.numProduced, continuous: false));
						madeMap.Add(component5.cropId, conversionEntry2);
					}
				}
			}
			ScaleGrowthMonitor.Def def4 = prefab.GetDef<ScaleGrowthMonitor.Def>();
			if (def4 != null)
			{
				ConversionEntry conversionEntry3 = new ConversionEntry();
				conversionEntry3.title = Assets.GetPrefab("ShearingStation").GetProperName();
				conversionEntry3.prefab = Assets.GetPrefab("ShearingStation");
				conversionEntry3.inSet = new HashSet<ElementUsage>();
				conversionEntry3.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, continuous: false));
				usedMap.Add(prefab.PrefabID(), conversionEntry3);
				conversionEntry3.outSet = new HashSet<ElementUsage>();
				conversionEntry3.outSet.Add(new ElementUsage(def4.itemDroppedOnShear, def4.dropMass, continuous: false));
				madeMap.Add(def4.itemDroppedOnShear, conversionEntry3);
			}
			WellFedShearable.Def def5 = prefab.GetDef<WellFedShearable.Def>();
			if (def5 != null)
			{
				ConversionEntry conversionEntry4 = new ConversionEntry();
				conversionEntry4.title = Assets.GetPrefab("ShearingStation").GetProperName();
				conversionEntry4.prefab = Assets.GetPrefab("ShearingStation");
				conversionEntry4.inSet = new HashSet<ElementUsage>();
				conversionEntry4.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, continuous: false));
				usedMap.Add(prefab.PrefabID(), conversionEntry4);
				conversionEntry4.outSet = new HashSet<ElementUsage>();
				conversionEntry4.outSet.Add(new ElementUsage(def5.itemDroppedOnShear, def5.dropMass, continuous: false));
				madeMap.Add(def5.itemDroppedOnShear, conversionEntry4);
			}
			Butcherable component6 = prefab.GetComponent<Butcherable>();
			if (component6 != null)
			{
				ConversionEntry conversionEntry5 = new ConversionEntry
				{
					title = prefab.GetProperName(),
					prefab = prefab
				};
				usedMap.Add(prefab.PrefabID(), conversionEntry5);
				conversionEntry5.outSet = new HashSet<ElementUsage>();
				Dictionary<string, float> dictionary = new Dictionary<string, float>();
				string[] drops = component6.drops;
				foreach (string text in drops)
				{
					dictionary.TryGetValue(text, out var value2);
					dictionary[text] = value2 + Assets.GetPrefab(text).GetComponent<PrimaryElement>().Mass;
				}
				foreach (var (text3, amount2) in dictionary)
				{
					conversionEntry5.outSet.Add(new ElementUsage(text3, amount2, continuous: false));
					madeMap.Add(text3, conversionEntry5);
				}
			}
		}
	}
}
