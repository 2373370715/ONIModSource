using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei.AI;
using ProcGen;
using TUNING;
using UnityEngine;

// Token: 0x0200125C RID: 4700
public class EconomyDetails
{
	// Token: 0x06006048 RID: 24648 RVA: 0x002AD544 File Offset: 0x002AB744
	public EconomyDetails()
	{
		this.massResourceType = new EconomyDetails.Resource.Type("Mass", "kg");
		this.heatResourceType = new EconomyDetails.Resource.Type("Heat Energy", "kdtu");
		this.energyResourceType = new EconomyDetails.Resource.Type("Energy", "joules");
		this.timeResourceType = new EconomyDetails.Resource.Type("Time", "seconds");
		this.attributeResourceType = new EconomyDetails.Resource.Type("Attribute", "units");
		this.caloriesResourceType = new EconomyDetails.Resource.Type("Calories", "kcal");
		this.amountResourceType = new EconomyDetails.Resource.Type("Amount", "units");
		this.buildingTransformationType = new EconomyDetails.Transformation.Type("Building");
		this.foodTransformationType = new EconomyDetails.Transformation.Type("Food");
		this.plantTransformationType = new EconomyDetails.Transformation.Type("Plant");
		this.creatureTransformationType = new EconomyDetails.Transformation.Type("Creature");
		this.dupeTransformationType = new EconomyDetails.Transformation.Type("Duplicant");
		this.referenceTransformationType = new EconomyDetails.Transformation.Type("Reference");
		this.effectTransformationType = new EconomyDetails.Transformation.Type("Effect");
		this.geyserActivePeriodTransformationType = new EconomyDetails.Transformation.Type("GeyserActivePeriod");
		this.geyserLifetimeTransformationType = new EconomyDetails.Transformation.Type("GeyserLifetime");
		this.energyResource = this.CreateResource(TagManager.Create("Energy"), this.energyResourceType);
		this.heatResource = this.CreateResource(TagManager.Create("Heat"), this.heatResourceType);
		this.duplicantTimeResource = this.CreateResource(TagManager.Create("DupeTime"), this.timeResourceType);
		this.caloriesResource = this.CreateResource(new Tag(Db.Get().Amounts.Calories.deltaAttribute.Id), this.caloriesResourceType);
		this.fixedCaloriesResource = this.CreateResource(new Tag(Db.Get().Amounts.Calories.Id), this.caloriesResourceType);
		foreach (Element element in ElementLoader.elements)
		{
			this.CreateResource(element);
		}
		foreach (Tag tag in new List<Tag>
		{
			GameTags.CombustibleLiquid,
			GameTags.CombustibleGas,
			GameTags.CombustibleSolid
		})
		{
			this.CreateResource(tag, this.massResourceType);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.CreateResource(foodInfo.Id.ToTag(), this.amountResourceType);
		}
		this.GatherStartingBiomeAmounts();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			this.CreateTransformation(kprefabID, kprefabID.PrefabTag);
			if (kprefabID.GetComponent<GeyserConfigurator>() != null)
			{
				KPrefabID prefab_id = kprefabID;
				Tag prefabTag = kprefabID.PrefabTag;
				this.CreateTransformation(prefab_id, prefabTag.ToString() + "_ActiveOnly");
			}
		}
		foreach (Effect effect in Db.Get().effects.resources)
		{
			this.CreateTransformation(effect);
		}
		EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(TagManager.Create("Duplicant"), this.dupeTransformationType, 1f, false);
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * Assets.GetPrefab(MinionConfig.ID).GetComponent<OxygenBreather>().O2toCO2conversion));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, 0.875f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, DUPLICANTSTATS.STANDARD.BaseStats.GUESSTIMATE_CALORIES_BURNED_PER_SECOND * 0.001f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND));
		this.transformations.Add(transformation);
		EconomyDetails.Transformation transformation2 = new EconomyDetails.Transformation(TagManager.Create("Electrolysis"), this.referenceTransformationType, 1f, false);
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), 1.7777778f));
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Hydrogen), 0.22222222f));
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Water), -2f));
		this.transformations.Add(transformation2);
		EconomyDetails.Transformation transformation3 = new EconomyDetails.Transformation(TagManager.Create("MethaneCombustion"), this.referenceTransformationType, 1f, false);
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Methane), -1f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -4f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 2.75f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Water), 2.25f));
		this.transformations.Add(transformation3);
		EconomyDetails.Transformation transformation4 = new EconomyDetails.Transformation(TagManager.Create("CoalCombustion"), this.referenceTransformationType, 1f, false);
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Carbon), -1f));
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -2.6666667f));
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 3.6666667f));
		this.transformations.Add(transformation4);
	}

	// Token: 0x06006049 RID: 24649 RVA: 0x000DEC15 File Offset: 0x000DCE15
	private static void WriteProduct(StreamWriter o, string a, string b)
	{
		o.Write(string.Concat(new string[]
		{
			"\"=PRODUCT(",
			a,
			", ",
			b,
			")\""
		}));
	}

	// Token: 0x0600604A RID: 24650 RVA: 0x000DEC48 File Offset: 0x000DCE48
	private static void WriteProduct(StreamWriter o, string a, string b, string c)
	{
		o.Write(string.Concat(new string[]
		{
			"\"=PRODUCT(",
			a,
			", ",
			b,
			", ",
			c,
			")\""
		}));
	}

	// Token: 0x0600604B RID: 24651 RVA: 0x002ADBC8 File Offset: 0x002ABDC8
	public void DumpTransformations(EconomyDetails.Scenario scenario, StreamWriter o)
	{
		List<EconomyDetails.Resource> used_resources = new List<EconomyDetails.Resource>();
		foreach (EconomyDetails.Transformation transformation in this.transformations)
		{
			if (scenario.IncludesTransformation(transformation))
			{
				foreach (EconomyDetails.Transformation.Delta delta in transformation.deltas)
				{
					if (!used_resources.Contains(delta.resource))
					{
						used_resources.Add(delta.resource);
					}
				}
			}
		}
		used_resources.Sort((EconomyDetails.Resource x, EconomyDetails.Resource y) => x.tag.Name.CompareTo(y.tag.Name));
		List<EconomyDetails.Ratio> list = new List<EconomyDetails.Ratio>();
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Algae), this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.energyResource, this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Oxygen), this.energyResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Water), this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.DirtyWater), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Water), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Fertilizer), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.energyResource, this.CreateResource(new Tag(Db.Get().Amounts.Stress.deltaAttribute.Id), this.amountResourceType), true));
		list.RemoveAll((EconomyDetails.Ratio x) => !used_resources.Contains(x.input) || !used_resources.Contains(x.output));
		o.Write("Id");
		o.Write(",Count");
		o.Write(",Type");
		o.Write(",Time(s)");
		int num = 4;
		foreach (EconomyDetails.Resource resource in used_resources)
		{
			o.Write(string.Concat(new string[]
			{
				", ",
				resource.tag.Name,
				"(",
				resource.type.unit,
				")"
			}));
			num++;
		}
		o.Write(",MassDelta");
		foreach (EconomyDetails.Ratio ratio in list)
		{
			o.Write(string.Concat(new string[]
			{
				", ",
				ratio.output.tag.Name,
				"(",
				ratio.output.type.unit,
				")/",
				ratio.input.tag.Name,
				"(",
				ratio.input.type.unit,
				")"
			}));
			num++;
		}
		string str = "B";
		o.Write("\n");
		int num2 = 1;
		this.transformations.Sort((EconomyDetails.Transformation x, EconomyDetails.Transformation y) => x.tag.Name.CompareTo(y.tag.Name));
		for (int i = 0; i < this.transformations.Count; i++)
		{
			EconomyDetails.Transformation transformation2 = this.transformations[i];
			if (scenario.IncludesTransformation(transformation2))
			{
				num2++;
			}
		}
		string text = "B" + (num2 + 4).ToString();
		int num3 = 1;
		for (int j = 0; j < this.transformations.Count; j++)
		{
			EconomyDetails.Transformation transformation3 = this.transformations[j];
			if (scenario.IncludesTransformation(transformation3))
			{
				if (transformation3.tag == new Tag(EconomyDetails.debugTag))
				{
					int num4 = 0 + 1;
				}
				num3++;
				o.Write("\"" + transformation3.tag.Name + "\"");
				o.Write("," + scenario.GetCount(transformation3.tag).ToString());
				o.Write(",\"" + transformation3.type.id + "\"");
				if (!transformation3.timeInvariant)
				{
					o.Write(",\"" + transformation3.timeInSeconds.ToString("0.00") + "\"");
				}
				else
				{
					o.Write(",\"invariant\"");
				}
				string a = str + num3.ToString();
				float num5 = 0f;
				bool flag = false;
				foreach (EconomyDetails.Resource resource2 in used_resources)
				{
					EconomyDetails.Transformation.Delta delta2 = null;
					foreach (EconomyDetails.Transformation.Delta delta3 in transformation3.deltas)
					{
						if (delta3.resource.tag == resource2.tag)
						{
							delta2 = delta3;
							break;
						}
					}
					o.Write(",");
					if (delta2 != null && delta2.amount != 0f)
					{
						if (delta2.resource.type == this.massResourceType)
						{
							flag = true;
							num5 += delta2.amount;
						}
						if (!transformation3.timeInvariant)
						{
							EconomyDetails.WriteProduct(o, a, (delta2.amount / transformation3.timeInSeconds).ToString("0.00000"), text);
						}
						else
						{
							EconomyDetails.WriteProduct(o, a, delta2.amount.ToString("0.00000"));
						}
					}
				}
				o.Write(",");
				if (flag)
				{
					num5 /= transformation3.timeInSeconds;
					EconomyDetails.WriteProduct(o, a, num5.ToString("0.00000"), text);
				}
				foreach (EconomyDetails.Ratio ratio2 in list)
				{
					o.Write(", ");
					EconomyDetails.Transformation.Delta delta4 = transformation3.GetDelta(ratio2.input);
					EconomyDetails.Transformation.Delta delta5 = transformation3.GetDelta(ratio2.output);
					if (delta5 != null && delta4 != null && delta4.amount < 0f && (delta5.amount > 0f || ratio2.allowNegativeOutput))
					{
						o.Write(delta5.amount / Mathf.Abs(delta4.amount));
					}
				}
				o.Write("\n");
			}
		}
		int num6 = 4;
		for (int k = 0; k < num; k++)
		{
			if (k >= num6 && k < num6 + used_resources.Count)
			{
				string text2 = ((char)(65 + k % 26)).ToString();
				int num7 = Mathf.FloorToInt((float)k / 26f);
				if (num7 > 0)
				{
					text2 = ((char)(65 + num7 - 1)).ToString() + text2;
				}
				o.Write(string.Concat(new string[]
				{
					"\"=SUM(",
					text2,
					"2: ",
					text2,
					num2.ToString(),
					")\""
				}));
			}
			o.Write(",");
		}
		string str2 = "B" + (num2 + 5).ToString();
		o.Write("\n");
		o.Write("\nTiming:");
		o.Write("\nTimeInSeconds:," + scenario.timeInSeconds.ToString());
		o.Write("\nSecondsPerCycle:," + 600f.ToString());
		o.Write("\nCycles:,=" + text + "/" + str2);
	}

	// Token: 0x0600604C RID: 24652 RVA: 0x002AE4F4 File Offset: 0x002AC6F4
	public EconomyDetails.Resource CreateResource(Tag tag, EconomyDetails.Resource.Type resource_type)
	{
		foreach (EconomyDetails.Resource resource in this.resources)
		{
			if (resource.tag == tag)
			{
				return resource;
			}
		}
		EconomyDetails.Resource resource2 = new EconomyDetails.Resource(tag, resource_type);
		this.resources.Add(resource2);
		return resource2;
	}

	// Token: 0x0600604D RID: 24653 RVA: 0x000DEC87 File Offset: 0x000DCE87
	public EconomyDetails.Resource CreateResource(Element element)
	{
		return this.CreateResource(element.tag, this.massResourceType);
	}

	// Token: 0x0600604E RID: 24654 RVA: 0x002AE56C File Offset: 0x002AC76C
	public EconomyDetails.Transformation CreateTransformation(Effect effect)
	{
		EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(new Tag(effect.Id), this.effectTransformationType, 1f, false);
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			EconomyDetails.Resource resource = this.CreateResource(new Tag(attributeModifier.AttributeId), this.attributeResourceType);
			transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource, attributeModifier.Value));
		}
		this.transformations.Add(transformation);
		return transformation;
	}

	// Token: 0x0600604F RID: 24655 RVA: 0x002AE60C File Offset: 0x002AC80C
	public EconomyDetails.Transformation GetTransformation(Tag tag)
	{
		foreach (EconomyDetails.Transformation transformation in this.transformations)
		{
			if (transformation.tag == tag)
			{
				return transformation;
			}
		}
		return null;
	}

	// Token: 0x06006050 RID: 24656 RVA: 0x002AE670 File Offset: 0x002AC870
	public EconomyDetails.Transformation CreateTransformation(KPrefabID prefab_id, Tag tag)
	{
		if (tag == new Tag(EconomyDetails.debugTag))
		{
			int num = 0 + 1;
		}
		Building component = prefab_id.GetComponent<Building>();
		ElementConverter component2 = prefab_id.GetComponent<ElementConverter>();
		EnergyConsumer component3 = prefab_id.GetComponent<EnergyConsumer>();
		ElementConsumer component4 = prefab_id.GetComponent<ElementConsumer>();
		BuildingElementEmitter component5 = prefab_id.GetComponent<BuildingElementEmitter>();
		Generator component6 = prefab_id.GetComponent<Generator>();
		EnergyGenerator component7 = prefab_id.GetComponent<EnergyGenerator>();
		ManualGenerator component8 = prefab_id.GetComponent<ManualGenerator>();
		ManualDeliveryKG[] components = prefab_id.GetComponents<ManualDeliveryKG>();
		StateMachineController component9 = prefab_id.GetComponent<StateMachineController>();
		Edible component10 = prefab_id.GetComponent<Edible>();
		Crop component11 = prefab_id.GetComponent<Crop>();
		Uprootable component12 = prefab_id.GetComponent<Uprootable>();
		ComplexRecipe complexRecipe = ComplexRecipeManager.Get().recipes.Find((ComplexRecipe r) => r.FirstResult == prefab_id.PrefabTag);
		List<FertilizationMonitor.Def> list = null;
		List<IrrigationMonitor.Def> list2 = null;
		GeyserConfigurator component13 = prefab_id.GetComponent<GeyserConfigurator>();
		Toilet component14 = prefab_id.GetComponent<Toilet>();
		FlushToilet component15 = prefab_id.GetComponent<FlushToilet>();
		RelaxationPoint component16 = prefab_id.GetComponent<RelaxationPoint>();
		CreatureCalorieMonitor.Def def = prefab_id.gameObject.GetDef<CreatureCalorieMonitor.Def>();
		if (component9 != null)
		{
			list = component9.GetDefs<FertilizationMonitor.Def>();
			list2 = component9.GetDefs<IrrigationMonitor.Def>();
		}
		EconomyDetails.Transformation transformation = null;
		float time_in_seconds = 1f;
		if (component10 != null)
		{
			transformation = new EconomyDetails.Transformation(tag, this.foodTransformationType, time_in_seconds, complexRecipe != null);
		}
		else if (component2 != null || component3 != null || component4 != null || component5 != null || component6 != null || component7 != null || component12 != null || component13 != null || component14 != null || component15 != null || component16 != null || def != null)
		{
			if (component12 != null || component11 != null)
			{
				if (component11 != null)
				{
					time_in_seconds = component11.cropVal.cropDuration;
				}
				transformation = new EconomyDetails.Transformation(tag, this.plantTransformationType, time_in_seconds, false);
			}
			else if (def != null)
			{
				transformation = new EconomyDetails.Transformation(tag, this.creatureTransformationType, time_in_seconds, false);
			}
			else if (component13 != null)
			{
				GeyserConfigurator.GeyserInstanceConfiguration geyserInstanceConfiguration = new GeyserConfigurator.GeyserInstanceConfiguration
				{
					typeId = component13.presetType,
					rateRoll = 0.5f,
					iterationLengthRoll = 0.5f,
					iterationPercentRoll = 0.5f,
					yearLengthRoll = 0.5f,
					yearPercentRoll = 0.5f
				};
				if (tag.Name.Contains("_ActiveOnly"))
				{
					float iterationLength = geyserInstanceConfiguration.GetIterationLength();
					transformation = new EconomyDetails.Transformation(tag, this.geyserActivePeriodTransformationType, iterationLength, false);
				}
				else
				{
					float yearLength = geyserInstanceConfiguration.GetYearLength();
					transformation = new EconomyDetails.Transformation(tag, this.geyserLifetimeTransformationType, yearLength, false);
				}
			}
			else
			{
				if (component14 != null || component15 != null)
				{
					time_in_seconds = 600f;
				}
				transformation = new EconomyDetails.Transformation(tag, this.buildingTransformationType, time_in_seconds, false);
			}
		}
		if (transformation != null)
		{
			if (component2 != null && component2.consumedElements != null)
			{
				foreach (ElementConverter.ConsumedElement consumedElement in component2.consumedElements)
				{
					EconomyDetails.Resource resource = this.CreateResource(consumedElement.Tag, this.massResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource, -consumedElement.MassConsumptionRate));
				}
				if (component2.outputElements != null)
				{
					foreach (ElementConverter.OutputElement outputElement in component2.outputElements)
					{
						Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
						EconomyDetails.Resource resource2 = this.CreateResource(element.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource2, outputElement.massGenerationRate));
					}
				}
			}
			if (component4 != null && component7 == null && (component2 == null || prefab_id.GetComponent<AlgaeHabitat>() != null))
			{
				EconomyDetails.Resource resource3 = this.GetResource(ElementLoader.FindElementByHash(component4.elementToConsume).tag);
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource3, -component4.consumptionRate));
			}
			if (component3 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.energyResource, -component3.WattsNeededWhenActive));
			}
			if (component5 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(component5.element), component5.emitRate));
			}
			if (component6 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.energyResource, component6.GetComponent<Building>().Def.GeneratorWattageRating));
			}
			if (component7 != null)
			{
				if (component7.formula.inputs != null)
				{
					foreach (EnergyGenerator.InputItem inputItem in component7.formula.inputs)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(inputItem.tag), -inputItem.consumptionRate));
					}
				}
				if (component7.formula.outputs != null)
				{
					foreach (EnergyGenerator.OutputItem outputItem in component7.formula.outputs)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(outputItem.element), outputItem.creationRate));
					}
				}
			}
			if (component)
			{
				BuildingDef def2 = component.Def;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.heatResource, def2.SelfHeatKilowattsWhenActive + def2.ExhaustKilowattsWhenActive));
			}
			if (component8)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, -1f));
			}
			if (component10)
			{
				EdiblesManager.FoodInfo foodInfo = component10.FoodInfo;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.fixedCaloriesResource, foodInfo.CaloriesPerUnit * 0.001f));
				ComplexRecipeManager.Get().recipes.Find((ComplexRecipe a) => a.FirstResult == tag);
			}
			if (component11 != null)
			{
				EconomyDetails.Resource resource4 = this.CreateResource(TagManager.Create(component11.cropVal.cropId), this.amountResourceType);
				float num2 = (float)component11.cropVal.numProduced;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource4, num2));
				GameObject prefab = Assets.GetPrefab(new Tag(component11.cropVal.cropId));
				if (prefab != null)
				{
					Edible component17 = prefab.GetComponent<Edible>();
					if (component17 != null)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, component17.FoodInfo.CaloriesPerUnit * num2 * 0.001f));
					}
				}
			}
			if (complexRecipe != null)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					this.CreateResource(recipeElement.material, this.amountResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(recipeElement.material), -recipeElement.amount));
				}
				foreach (ComplexRecipe.RecipeElement recipeElement2 in complexRecipe.results)
				{
					this.CreateResource(recipeElement2.material, this.amountResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(recipeElement2.material), recipeElement2.amount));
				}
			}
			if (components != null)
			{
				for (int j = 0; j < components.Length; j++)
				{
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, -0.1f * transformation.timeInSeconds));
				}
			}
			if (list != null && list.Count > 0)
			{
				foreach (FertilizationMonitor.Def def3 in list)
				{
					foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in def3.consumedElements)
					{
						EconomyDetails.Resource resource5 = this.CreateResource(consumeInfo.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource5, -consumeInfo.massConsumptionRate * transformation.timeInSeconds));
					}
				}
			}
			if (list2 != null && list2.Count > 0)
			{
				foreach (IrrigationMonitor.Def def4 in list2)
				{
					foreach (PlantElementAbsorber.ConsumeInfo consumeInfo2 in def4.consumedElements)
					{
						EconomyDetails.Resource resource6 = this.CreateResource(consumeInfo2.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource6, -consumeInfo2.massConsumptionRate * transformation.timeInSeconds));
					}
				}
			}
			if (component13 != null)
			{
				GeyserConfigurator.GeyserInstanceConfiguration geyserInstanceConfiguration2 = new GeyserConfigurator.GeyserInstanceConfiguration
				{
					typeId = component13.presetType,
					rateRoll = 0.5f,
					iterationLengthRoll = 0.5f,
					iterationPercentRoll = 0.5f,
					yearLengthRoll = 0.5f,
					yearPercentRoll = 0.5f
				};
				if (tag.Name.Contains("_ActiveOnly"))
				{
					float amount = geyserInstanceConfiguration2.GetMassPerCycle() / 600f * geyserInstanceConfiguration2.GetIterationLength();
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(geyserInstanceConfiguration2.GetElement().CreateTag(), this.massResourceType), amount));
				}
				else
				{
					float amount2 = geyserInstanceConfiguration2.GetMassPerCycle() / 600f * geyserInstanceConfiguration2.GetYearLength() * geyserInstanceConfiguration2.GetYearPercent();
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(geyserInstanceConfiguration2.GetElement().CreateTag(), this.massResourceType), amount2));
				}
			}
			if (component14 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), -DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.Dirt), -component14.solidWastePerUse.mass));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(component14.solidWastePerUse.elementID), component14.solidWastePerUse.mass));
			}
			if (component15 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), -DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.Water), -component15.massConsumedPerUse));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.DirtyWater), component15.massEmittedPerUse));
			}
			if (component16 != null)
			{
				foreach (AttributeModifier attributeModifier in component16.CreateEffect().SelfModifiers)
				{
					EconomyDetails.Resource resource7 = this.CreateResource(new Tag(attributeModifier.AttributeId), this.attributeResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource7, attributeModifier.Value));
				}
			}
			if (def != null)
			{
				this.CollectDietTransformations(prefab_id);
			}
			this.transformations.Add(transformation);
		}
		return transformation;
	}

	// Token: 0x06006051 RID: 24657 RVA: 0x002AF270 File Offset: 0x002AD470
	private void CollectDietTransformations(KPrefabID prefab_id)
	{
		Trait trait = Db.Get().traits.Get(prefab_id.GetComponent<Modifiers>().initialTraits[0]);
		CreatureCalorieMonitor.Def def = prefab_id.gameObject.GetDef<CreatureCalorieMonitor.Def>();
		WildnessMonitor.Def def2 = prefab_id.gameObject.GetDef<WildnessMonitor.Def>();
		List<AttributeModifier> list = new List<AttributeModifier>();
		list.AddRange(trait.SelfModifiers);
		list.AddRange(def2.tameEffect.SelfModifiers);
		float num = 0f;
		float num2 = 0f;
		foreach (AttributeModifier attributeModifier in list)
		{
			if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.maxAttribute.Id)
			{
				num = attributeModifier.Value;
			}
			if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
			{
				num2 = attributeModifier.Value;
			}
		}
		foreach (Diet.Info info in def.diet.infos)
		{
			foreach (Tag tag in info.consumedTags)
			{
				float time_in_seconds = Mathf.Abs(num / num2);
				float num3 = num / info.caloriesPerKg;
				float amount = num3 * info.producedConversionRate;
				EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(new Tag(prefab_id.PrefabTag.Name + "Diet" + tag.Name), this.creatureTransformationType, time_in_seconds, false);
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(tag, this.massResourceType), -num3));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(info.producedElement.ToString()), this.massResourceType), amount));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, num));
				this.transformations.Add(transformation);
			}
		}
	}

	// Token: 0x06006052 RID: 24658 RVA: 0x002AF4B8 File Offset: 0x002AD6B8
	private static void CollectDietScenarios(List<EconomyDetails.Scenario> scenarios)
	{
		EconomyDetails.Scenario scenario = new EconomyDetails.Scenario("diets/all", 0f, null);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.gameObject.GetDef<CreatureCalorieMonitor.Def>();
			if (def != null)
			{
				EconomyDetails.Scenario scenario2 = new EconomyDetails.Scenario("diets/" + kprefabID.name, 0f, null);
				Diet.Info[] infos = def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					foreach (Tag tag in infos[i].consumedTags)
					{
						Tag tag2 = kprefabID.PrefabTag.Name + "Diet" + tag.Name;
						scenario2.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
						scenario.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
					}
				}
				scenarios.Add(scenario2);
			}
		}
		scenarios.Add(scenario);
	}

	// Token: 0x06006053 RID: 24659 RVA: 0x002AF608 File Offset: 0x002AD808
	public void GatherStartingBiomeAmounts()
	{
		for (int i = 0; i < Grid.CellCount; i++)
		{
			if (global::World.Instance.zoneRenderData.worldZoneTypes[i] == SubWorld.ZoneType.Sandstone)
			{
				Element key = Grid.Element[i];
				float num = 0f;
				this.startingBiomeAmounts.TryGetValue(key, out num);
				this.startingBiomeAmounts[key] = num + Grid.Mass[i];
				this.startingBiomeCellCount++;
			}
		}
	}

	// Token: 0x06006054 RID: 24660 RVA: 0x000DEC9B File Offset: 0x000DCE9B
	public EconomyDetails.Resource GetResource(SimHashes element)
	{
		return this.GetResource(ElementLoader.FindElementByHash(element).tag);
	}

	// Token: 0x06006055 RID: 24661 RVA: 0x002AF680 File Offset: 0x002AD880
	public EconomyDetails.Resource GetResource(Tag tag)
	{
		foreach (EconomyDetails.Resource resource in this.resources)
		{
			if (resource.tag == tag)
			{
				return resource;
			}
		}
		DebugUtil.LogErrorArgs(new object[]
		{
			"Found a tag without a matching resource!",
			tag
		});
		return null;
	}

	// Token: 0x06006056 RID: 24662 RVA: 0x000DECAE File Offset: 0x000DCEAE
	private float GetDupeBreathingPerSecond(EconomyDetails details)
	{
		return details.GetTransformation(TagManager.Create("Duplicant")).GetDelta(details.GetResource(GameTags.Oxygen)).amount;
	}

	// Token: 0x06006057 RID: 24663 RVA: 0x002AF700 File Offset: 0x002AD900
	private EconomyDetails.BiomeTransformation CreateBiomeTransformationFromTransformation(EconomyDetails details, Tag transformation_tag, Tag input_resource_tag, Tag output_resource_tag)
	{
		EconomyDetails.Resource resource = details.GetResource(input_resource_tag);
		EconomyDetails.Resource resource2 = details.GetResource(output_resource_tag);
		EconomyDetails.Transformation transformation = details.GetTransformation(transformation_tag);
		float num = transformation.GetDelta(resource2).amount / -transformation.GetDelta(resource).amount;
		float num2 = this.GetDupeBreathingPerSecond(details) * 600f;
		return new EconomyDetails.BiomeTransformation((transformation_tag.Name + input_resource_tag.Name + "Cycles").ToTag(), resource, num / -num2);
	}

	// Token: 0x06006058 RID: 24664 RVA: 0x002AF778 File Offset: 0x002AD978
	private static void DumpEconomyDetails()
	{
		global::Debug.Log("Starting Economy Details Dump...");
		EconomyDetails details = new EconomyDetails();
		List<EconomyDetails.Scenario> list = new List<EconomyDetails.Scenario>();
		EconomyDetails.Scenario item = new EconomyDetails.Scenario("default", 1f, (EconomyDetails.Transformation t) => true);
		list.Add(item);
		EconomyDetails.Scenario item2 = new EconomyDetails.Scenario("all_buildings", 1f, (EconomyDetails.Transformation t) => t.type == details.buildingTransformationType);
		list.Add(item2);
		EconomyDetails.Scenario item3 = new EconomyDetails.Scenario("all_plants", 1f, (EconomyDetails.Transformation t) => t.type == details.plantTransformationType);
		list.Add(item3);
		EconomyDetails.Scenario item4 = new EconomyDetails.Scenario("all_creatures", 1f, (EconomyDetails.Transformation t) => t.type == details.creatureTransformationType);
		list.Add(item4);
		EconomyDetails.Scenario item5 = new EconomyDetails.Scenario("all_stress", 1f, (EconomyDetails.Transformation t) => t.GetDelta(details.GetResource(new Tag(Db.Get().Amounts.Stress.deltaAttribute.Id))) != null);
		list.Add(item5);
		EconomyDetails.Scenario item6 = new EconomyDetails.Scenario("all_foods", 1f, (EconomyDetails.Transformation t) => t.type == details.foodTransformationType);
		list.Add(item6);
		EconomyDetails.Scenario item7 = new EconomyDetails.Scenario("geysers/geysers_active_period_only", 1f, (EconomyDetails.Transformation t) => t.type == details.geyserActivePeriodTransformationType);
		list.Add(item7);
		EconomyDetails.Scenario item8 = new EconomyDetails.Scenario("geysers/geysers_whole_lifetime", 1f, (EconomyDetails.Transformation t) => t.type == details.geyserLifetimeTransformationType);
		list.Add(item8);
		EconomyDetails.Scenario scenario = new EconomyDetails.Scenario("oxygen/algae_distillery", 0f, null);
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("AlgaeDistillery"), 3f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("AlgaeHabitat"), 22f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 9f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		list.Add(scenario);
		EconomyDetails.Scenario scenario2 = new EconomyDetails.Scenario("oxygen/algae_habitat_electrolyzer", 0f, null);
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("AlgaeHabitat", 1f));
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("Duplicant", 1f));
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("Electrolyzer", 1f));
		list.Add(scenario2);
		EconomyDetails.Scenario scenario3 = new EconomyDetails.Scenario("oxygen/electrolyzer", 0f, null);
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 9f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		list.Add(scenario3);
		EconomyDetails.Scenario scenario4 = new EconomyDetails.Scenario("purifiers/methane_generator", 0f, null);
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MethaneGenerator"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FertilizerMaker"), 3f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 2f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PrickleFlower"), 0f));
		list.Add(scenario4);
		EconomyDetails.Scenario scenario5 = new EconomyDetails.Scenario("purifiers/water_purifier", 0f, null);
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Compost"), 2f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 2f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PrickleFlower"), 29f));
		list.Add(scenario5);
		EconomyDetails.Scenario scenario6 = new EconomyDetails.Scenario("energy/petroleum_generator", 0f, null);
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PetroleumGenerator"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("OilRefinery"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("CO2Scrubber"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MethaneGenerator"), 1f));
		list.Add(scenario6);
		EconomyDetails.Scenario scenario7 = new EconomyDetails.Scenario("energy/coal_generator", 0f, (EconomyDetails.Transformation t) => t.tag.Name.Contains("Hatch"));
		scenario7.AddEntry(new EconomyDetails.Scenario.Entry("Generator", 1f));
		list.Add(scenario7);
		EconomyDetails.Scenario scenario8 = new EconomyDetails.Scenario("waste/outhouse", 0f, null);
		scenario8.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Outhouse"), 1f));
		scenario8.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Compost"), 1f));
		list.Add(scenario8);
		EconomyDetails.Scenario scenario9 = new EconomyDetails.Scenario("stress/massage_table", 0f, null);
		scenario9.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MassageTable"), 1f));
		scenario9.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("ManualGenerator"), 1f));
		list.Add(scenario9);
		EconomyDetails.Scenario scenario10 = new EconomyDetails.Scenario("waste/flush_toilet", 0f, null);
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FlushToilet"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FertilizerMaker"), 1f));
		list.Add(scenario10);
		EconomyDetails.CollectDietScenarios(list);
		foreach (EconomyDetails.Transformation transformation in details.transformations)
		{
			EconomyDetails.Transformation transformation_iter = transformation;
			EconomyDetails.Scenario item9 = new EconomyDetails.Scenario("transformations/" + transformation.tag.Name, 1f, (EconomyDetails.Transformation t) => transformation_iter == t);
			list.Add(item9);
		}
		foreach (EconomyDetails.Transformation transformation2 in details.transformations)
		{
			EconomyDetails.Scenario scenario11 = new EconomyDetails.Scenario("transformation_groups/" + transformation2.tag.Name, 0f, null);
			scenario11.AddEntry(new EconomyDetails.Scenario.Entry(transformation2.tag, 1f));
			foreach (EconomyDetails.Transformation transformation3 in details.transformations)
			{
				bool flag = false;
				foreach (EconomyDetails.Transformation.Delta delta in transformation2.deltas)
				{
					if (delta.resource.type != details.energyResourceType)
					{
						foreach (EconomyDetails.Transformation.Delta delta2 in transformation3.deltas)
						{
							if (delta.resource == delta2.resource)
							{
								scenario11.AddEntry(new EconomyDetails.Scenario.Entry(transformation3.tag, 0f));
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			list.Add(scenario11);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			EconomyDetails.Scenario scenario12 = new EconomyDetails.Scenario("food/" + foodInfo.Id, 0f, null);
			Tag tag2 = TagManager.Create(foodInfo.Id);
			scenario12.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
			scenario12.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 1f));
			List<Tag> list2 = new List<Tag>();
			list2.Add(tag2);
			while (list2.Count > 0)
			{
				Tag tag = list2[0];
				list2.RemoveAt(0);
				ComplexRecipe complexRecipe = ComplexRecipeManager.Get().recipes.Find((ComplexRecipe a) => a.FirstResult == tag);
				if (complexRecipe != null)
				{
					foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
					{
						scenario12.AddEntry(new EconomyDetails.Scenario.Entry(recipeElement.material, 1f));
						list2.Add(recipeElement.material);
					}
				}
				foreach (KPrefabID kprefabID in Assets.Prefabs)
				{
					Crop component = kprefabID.GetComponent<Crop>();
					if (component != null && component.cropVal.cropId == tag.Name)
					{
						scenario12.AddEntry(new EconomyDetails.Scenario.Entry(kprefabID.PrefabTag, 1f));
						list2.Add(kprefabID.PrefabTag);
					}
				}
			}
			list.Add(scenario12);
		}
		if (!Directory.Exists("assets/Tuning/Economy"))
		{
			Directory.CreateDirectory("assets/Tuning/Economy");
		}
		foreach (EconomyDetails.Scenario scenario13 in list)
		{
			string path = "assets/Tuning/Economy/" + scenario13.name + ".csv";
			if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				details.DumpTransformations(scenario13, streamWriter);
			}
		}
		float dupeBreathingPerSecond = details.GetDupeBreathingPerSecond(details);
		List<EconomyDetails.BiomeTransformation> list3 = new List<EconomyDetails.BiomeTransformation>();
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "MineralDeoxidizer".ToTag(), GameTags.Algae, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "AlgaeHabitat".ToTag(), GameTags.Algae, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "AlgaeHabitat".ToTag(), GameTags.Water, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "Electrolyzer".ToTag(), GameTags.Water, GameTags.Oxygen));
		list3.Add(new EconomyDetails.BiomeTransformation("StartingOxygenCycles".ToTag(), details.GetResource(GameTags.Oxygen), 1f / -(dupeBreathingPerSecond * 600f)));
		list3.Add(new EconomyDetails.BiomeTransformation("StartingOxyliteCycles".ToTag(), details.CreateResource(GameTags.OxyRock, details.massResourceType), 1f / -(dupeBreathingPerSecond * 600f)));
		string path2 = "assets/Tuning/Economy/biomes/starting_amounts.csv";
		if (!Directory.Exists(System.IO.Path.GetDirectoryName(path2)))
		{
			Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path2));
		}
		using (StreamWriter streamWriter2 = new StreamWriter(path2))
		{
			streamWriter2.Write("Resource,Amount");
			foreach (EconomyDetails.BiomeTransformation biomeTransformation in list3)
			{
				streamWriter2.Write("," + biomeTransformation.tag.ToString());
			}
			streamWriter2.Write("\n");
			streamWriter2.Write("Cells, " + details.startingBiomeCellCount.ToString() + "\n");
			foreach (KeyValuePair<Element, float> keyValuePair in details.startingBiomeAmounts)
			{
				streamWriter2.Write(keyValuePair.Key.id.ToString() + ", " + keyValuePair.Value.ToString());
				foreach (EconomyDetails.BiomeTransformation biomeTransformation2 in list3)
				{
					streamWriter2.Write(",");
					float num = biomeTransformation2.Transform(keyValuePair.Key, keyValuePair.Value);
					if (num > 0f)
					{
						streamWriter2.Write(num);
					}
				}
				streamWriter2.Write("\n");
			}
		}
		global::Debug.Log("Completed economy details dump!!");
	}

	// Token: 0x06006059 RID: 24665 RVA: 0x002B0700 File Offset: 0x002AE900
	private static void DumpNameMapping()
	{
		string path = "assets/Tuning/Economy/name_mapping.csv";
		if (!Directory.Exists("assets/Tuning/Economy"))
		{
			Directory.CreateDirectory("assets/Tuning/Economy");
		}
		using (StreamWriter streamWriter = new StreamWriter(path))
		{
			streamWriter.Write("Game Name, Prefab Name, Anim Files\n");
			foreach (KPrefabID kprefabID in Assets.Prefabs)
			{
				string text = TagManager.StripLinkFormatting(kprefabID.GetProperName());
				Tag tag = kprefabID.PrefabID();
				if (!text.IsNullOrWhiteSpace() && !tag.Name.Contains("UnderConstruction") && !tag.Name.Contains("Preview"))
				{
					streamWriter.Write(text);
					TextWriter textWriter = streamWriter;
					string str = ",";
					Tag tag2 = tag;
					textWriter.Write(str + tag2.ToString());
					KAnimControllerBase component = kprefabID.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						foreach (KAnimFile kanimFile in component.AnimFiles)
						{
							streamWriter.Write("," + kanimFile.name);
						}
					}
					else
					{
						streamWriter.Write(",");
					}
					streamWriter.Write("\n");
				}
			}
			using (List<PermitResource>.Enumerator enumerator2 = Db.Get().Permits.resources.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PermitResource permit = enumerator2.Current;
					if (Blueprints.Get().skinsRelease.buildingFacades.Any((BuildingFacadeInfo info) => info.id == permit.Id) || Blueprints.Get().skinsRelease.clothingItems.Any((ClothingItemInfo info) => info.id == permit.Id) || Blueprints.Get().skinsRelease.artables.Any((ArtableInfo info) => info.id == permit.Id))
					{
						string value = TagManager.StripLinkFormatting(permit.Name);
						streamWriter.Write(value);
						string id = permit.Id;
						streamWriter.Write("," + id);
						BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
						string str2;
						if (buildingFacadeResource != null)
						{
							str2 = buildingFacadeResource.AnimFile;
						}
						else
						{
							ClothingItemResource clothingItemResource = permit as ClothingItemResource;
							if (clothingItemResource != null)
							{
								str2 = clothingItemResource.AnimFile.name;
							}
							else
							{
								ArtableStage artableStage = permit as ArtableStage;
								if (artableStage != null)
								{
									str2 = artableStage.animFile;
								}
								else
								{
									str2 = "";
								}
							}
						}
						streamWriter.Write("," + str2);
						streamWriter.Write("\n");
					}
				}
			}
		}
	}

	// Token: 0x04004448 RID: 17480
	private List<EconomyDetails.Transformation> transformations = new List<EconomyDetails.Transformation>();

	// Token: 0x04004449 RID: 17481
	private List<EconomyDetails.Resource> resources = new List<EconomyDetails.Resource>();

	// Token: 0x0400444A RID: 17482
	public Dictionary<Element, float> startingBiomeAmounts = new Dictionary<Element, float>();

	// Token: 0x0400444B RID: 17483
	public int startingBiomeCellCount;

	// Token: 0x0400444C RID: 17484
	public EconomyDetails.Resource energyResource;

	// Token: 0x0400444D RID: 17485
	public EconomyDetails.Resource heatResource;

	// Token: 0x0400444E RID: 17486
	public EconomyDetails.Resource duplicantTimeResource;

	// Token: 0x0400444F RID: 17487
	public EconomyDetails.Resource caloriesResource;

	// Token: 0x04004450 RID: 17488
	public EconomyDetails.Resource fixedCaloriesResource;

	// Token: 0x04004451 RID: 17489
	public EconomyDetails.Resource.Type massResourceType;

	// Token: 0x04004452 RID: 17490
	public EconomyDetails.Resource.Type heatResourceType;

	// Token: 0x04004453 RID: 17491
	public EconomyDetails.Resource.Type energyResourceType;

	// Token: 0x04004454 RID: 17492
	public EconomyDetails.Resource.Type timeResourceType;

	// Token: 0x04004455 RID: 17493
	public EconomyDetails.Resource.Type attributeResourceType;

	// Token: 0x04004456 RID: 17494
	public EconomyDetails.Resource.Type caloriesResourceType;

	// Token: 0x04004457 RID: 17495
	public EconomyDetails.Resource.Type amountResourceType;

	// Token: 0x04004458 RID: 17496
	public EconomyDetails.Transformation.Type buildingTransformationType;

	// Token: 0x04004459 RID: 17497
	public EconomyDetails.Transformation.Type foodTransformationType;

	// Token: 0x0400445A RID: 17498
	public EconomyDetails.Transformation.Type plantTransformationType;

	// Token: 0x0400445B RID: 17499
	public EconomyDetails.Transformation.Type creatureTransformationType;

	// Token: 0x0400445C RID: 17500
	public EconomyDetails.Transformation.Type dupeTransformationType;

	// Token: 0x0400445D RID: 17501
	public EconomyDetails.Transformation.Type referenceTransformationType;

	// Token: 0x0400445E RID: 17502
	public EconomyDetails.Transformation.Type effectTransformationType;

	// Token: 0x0400445F RID: 17503
	private const string GEYSER_ACTIVE_SUFFIX = "_ActiveOnly";

	// Token: 0x04004460 RID: 17504
	public EconomyDetails.Transformation.Type geyserActivePeriodTransformationType;

	// Token: 0x04004461 RID: 17505
	public EconomyDetails.Transformation.Type geyserLifetimeTransformationType;

	// Token: 0x04004462 RID: 17506
	private static string debugTag = "CO2Scrubber";

	// Token: 0x0200125D RID: 4701
	public class Resource
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x000DECE1 File Offset: 0x000DCEE1
		// (set) Token: 0x0600605C RID: 24668 RVA: 0x000DECE9 File Offset: 0x000DCEE9
		public Tag tag { get; private set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600605D RID: 24669 RVA: 0x000DECF2 File Offset: 0x000DCEF2
		// (set) Token: 0x0600605E RID: 24670 RVA: 0x000DECFA File Offset: 0x000DCEFA
		public EconomyDetails.Resource.Type type { get; private set; }

		// Token: 0x0600605F RID: 24671 RVA: 0x000DED03 File Offset: 0x000DCF03
		public Resource(Tag tag, EconomyDetails.Resource.Type type)
		{
			this.tag = tag;
			this.type = type;
		}

		// Token: 0x0200125E RID: 4702
		public class Type
		{
			// Token: 0x170005C7 RID: 1479
			// (get) Token: 0x06006060 RID: 24672 RVA: 0x000DED19 File Offset: 0x000DCF19
			// (set) Token: 0x06006061 RID: 24673 RVA: 0x000DED21 File Offset: 0x000DCF21
			public string id { get; private set; }

			// Token: 0x170005C8 RID: 1480
			// (get) Token: 0x06006062 RID: 24674 RVA: 0x000DED2A File Offset: 0x000DCF2A
			// (set) Token: 0x06006063 RID: 24675 RVA: 0x000DED32 File Offset: 0x000DCF32
			public string unit { get; private set; }

			// Token: 0x06006064 RID: 24676 RVA: 0x000DED3B File Offset: 0x000DCF3B
			public Type(string id, string unit)
			{
				this.id = id;
				this.unit = unit;
			}
		}
	}

	// Token: 0x0200125F RID: 4703
	public class BiomeTransformation
	{
		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06006065 RID: 24677 RVA: 0x000DED51 File Offset: 0x000DCF51
		// (set) Token: 0x06006066 RID: 24678 RVA: 0x000DED59 File Offset: 0x000DCF59
		public Tag tag { get; private set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06006067 RID: 24679 RVA: 0x000DED62 File Offset: 0x000DCF62
		// (set) Token: 0x06006068 RID: 24680 RVA: 0x000DED6A File Offset: 0x000DCF6A
		public EconomyDetails.Resource resource { get; private set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06006069 RID: 24681 RVA: 0x000DED73 File Offset: 0x000DCF73
		// (set) Token: 0x0600606A RID: 24682 RVA: 0x000DED7B File Offset: 0x000DCF7B
		public float ratio { get; private set; }

		// Token: 0x0600606B RID: 24683 RVA: 0x000DED84 File Offset: 0x000DCF84
		public BiomeTransformation(Tag tag, EconomyDetails.Resource resource, float ratio)
		{
			this.tag = tag;
			this.resource = resource;
			this.ratio = ratio;
		}

		// Token: 0x0600606C RID: 24684 RVA: 0x000DEDA1 File Offset: 0x000DCFA1
		public float Transform(Element element, float amount)
		{
			if (this.resource.tag == element.tag)
			{
				return this.ratio * amount;
			}
			return 0f;
		}
	}

	// Token: 0x02001260 RID: 4704
	public class Ratio
	{
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x000DEDC9 File Offset: 0x000DCFC9
		// (set) Token: 0x0600606E RID: 24686 RVA: 0x000DEDD1 File Offset: 0x000DCFD1
		public EconomyDetails.Resource input { get; private set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600606F RID: 24687 RVA: 0x000DEDDA File Offset: 0x000DCFDA
		// (set) Token: 0x06006070 RID: 24688 RVA: 0x000DEDE2 File Offset: 0x000DCFE2
		public EconomyDetails.Resource output { get; private set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06006071 RID: 24689 RVA: 0x000DEDEB File Offset: 0x000DCFEB
		// (set) Token: 0x06006072 RID: 24690 RVA: 0x000DEDF3 File Offset: 0x000DCFF3
		public bool allowNegativeOutput { get; private set; }

		// Token: 0x06006073 RID: 24691 RVA: 0x000DEDFC File Offset: 0x000DCFFC
		public Ratio(EconomyDetails.Resource input, EconomyDetails.Resource output, bool allow_negative_output)
		{
			this.input = input;
			this.output = output;
			this.allowNegativeOutput = allow_negative_output;
		}
	}

	// Token: 0x02001261 RID: 4705
	public class Scenario
	{
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06006074 RID: 24692 RVA: 0x000DEE19 File Offset: 0x000DD019
		// (set) Token: 0x06006075 RID: 24693 RVA: 0x000DEE21 File Offset: 0x000DD021
		public string name { get; private set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x000DEE2A File Offset: 0x000DD02A
		// (set) Token: 0x06006077 RID: 24695 RVA: 0x000DEE32 File Offset: 0x000DD032
		public float defaultCount { get; private set; }

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x000DEE3B File Offset: 0x000DD03B
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x000DEE43 File Offset: 0x000DD043
		public float timeInSeconds { get; set; }

		// Token: 0x0600607A RID: 24698 RVA: 0x000DEE4C File Offset: 0x000DD04C
		public Scenario(string name, float default_count, Func<EconomyDetails.Transformation, bool> filter)
		{
			this.name = name;
			this.defaultCount = default_count;
			this.filter = filter;
			this.timeInSeconds = 600f;
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x000DEE7F File Offset: 0x000DD07F
		public void AddEntry(EconomyDetails.Scenario.Entry entry)
		{
			this.entries.Add(entry);
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x002B0A14 File Offset: 0x002AEC14
		public float GetCount(Tag tag)
		{
			foreach (EconomyDetails.Scenario.Entry entry in this.entries)
			{
				if (entry.tag == tag)
				{
					return entry.count;
				}
			}
			return this.defaultCount;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x002B0A80 File Offset: 0x002AEC80
		public bool IncludesTransformation(EconomyDetails.Transformation transformation)
		{
			if (this.filter != null && this.filter(transformation))
			{
				return true;
			}
			using (List<EconomyDetails.Scenario.Entry>.Enumerator enumerator = this.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.tag == transformation.tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004470 RID: 17520
		private Func<EconomyDetails.Transformation, bool> filter;

		// Token: 0x04004471 RID: 17521
		private List<EconomyDetails.Scenario.Entry> entries = new List<EconomyDetails.Scenario.Entry>();

		// Token: 0x02001262 RID: 4706
		public class Entry
		{
			// Token: 0x170005D2 RID: 1490
			// (get) Token: 0x0600607E RID: 24702 RVA: 0x000DEE8D File Offset: 0x000DD08D
			// (set) Token: 0x0600607F RID: 24703 RVA: 0x000DEE95 File Offset: 0x000DD095
			public Tag tag { get; private set; }

			// Token: 0x170005D3 RID: 1491
			// (get) Token: 0x06006080 RID: 24704 RVA: 0x000DEE9E File Offset: 0x000DD09E
			// (set) Token: 0x06006081 RID: 24705 RVA: 0x000DEEA6 File Offset: 0x000DD0A6
			public float count { get; private set; }

			// Token: 0x06006082 RID: 24706 RVA: 0x000DEEAF File Offset: 0x000DD0AF
			public Entry(Tag tag, float count)
			{
				this.tag = tag;
				this.count = count;
			}
		}
	}

	// Token: 0x02001263 RID: 4707
	public class Transformation
	{
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06006083 RID: 24707 RVA: 0x000DEEC5 File Offset: 0x000DD0C5
		// (set) Token: 0x06006084 RID: 24708 RVA: 0x000DEECD File Offset: 0x000DD0CD
		public Tag tag { get; private set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06006085 RID: 24709 RVA: 0x000DEED6 File Offset: 0x000DD0D6
		// (set) Token: 0x06006086 RID: 24710 RVA: 0x000DEEDE File Offset: 0x000DD0DE
		public EconomyDetails.Transformation.Type type { get; private set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06006087 RID: 24711 RVA: 0x000DEEE7 File Offset: 0x000DD0E7
		// (set) Token: 0x06006088 RID: 24712 RVA: 0x000DEEEF File Offset: 0x000DD0EF
		public float timeInSeconds { get; private set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06006089 RID: 24713 RVA: 0x000DEEF8 File Offset: 0x000DD0F8
		// (set) Token: 0x0600608A RID: 24714 RVA: 0x000DEF00 File Offset: 0x000DD100
		public bool timeInvariant { get; private set; }

		// Token: 0x0600608B RID: 24715 RVA: 0x000DEF09 File Offset: 0x000DD109
		public Transformation(Tag tag, EconomyDetails.Transformation.Type type, float time_in_seconds, bool timeInvariant = false)
		{
			this.tag = tag;
			this.type = type;
			this.timeInSeconds = time_in_seconds;
			this.timeInvariant = timeInvariant;
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x000DEF39 File Offset: 0x000DD139
		public void AddDelta(EconomyDetails.Transformation.Delta delta)
		{
			global::Debug.Assert(delta.resource != null);
			this.deltas.Add(delta);
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x002B0AFC File Offset: 0x002AECFC
		public EconomyDetails.Transformation.Delta GetDelta(EconomyDetails.Resource resource)
		{
			foreach (EconomyDetails.Transformation.Delta delta in this.deltas)
			{
				if (delta.resource == resource)
				{
					return delta;
				}
			}
			return null;
		}

		// Token: 0x04004475 RID: 17525
		public List<EconomyDetails.Transformation.Delta> deltas = new List<EconomyDetails.Transformation.Delta>();

		// Token: 0x02001264 RID: 4708
		public class Delta
		{
			// Token: 0x170005D8 RID: 1496
			// (get) Token: 0x0600608E RID: 24718 RVA: 0x000DEF55 File Offset: 0x000DD155
			// (set) Token: 0x0600608F RID: 24719 RVA: 0x000DEF5D File Offset: 0x000DD15D
			public EconomyDetails.Resource resource { get; private set; }

			// Token: 0x170005D9 RID: 1497
			// (get) Token: 0x06006090 RID: 24720 RVA: 0x000DEF66 File Offset: 0x000DD166
			// (set) Token: 0x06006091 RID: 24721 RVA: 0x000DEF6E File Offset: 0x000DD16E
			public float amount { get; set; }

			// Token: 0x06006092 RID: 24722 RVA: 0x000DEF77 File Offset: 0x000DD177
			public Delta(EconomyDetails.Resource resource, float amount)
			{
				this.resource = resource;
				this.amount = amount;
			}
		}

		// Token: 0x02001265 RID: 4709
		public class Type
		{
			// Token: 0x170005DA RID: 1498
			// (get) Token: 0x06006093 RID: 24723 RVA: 0x000DEF8D File Offset: 0x000DD18D
			// (set) Token: 0x06006094 RID: 24724 RVA: 0x000DEF95 File Offset: 0x000DD195
			public string id { get; private set; }

			// Token: 0x06006095 RID: 24725 RVA: 0x000DEF9E File Offset: 0x000DD19E
			public Type(string id)
			{
				this.id = id;
			}
		}
	}
}
