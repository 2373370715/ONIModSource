using System.Collections.Generic;
using System.Collections.ObjectModel;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using UnityEngine;

public class AdditionalDetailsPanel : DetailScreenTab
{
	public GameObject attributesLabelTemplate;

	private CollapsibleDetailContentPanel detailsPanel;

	private DetailsPanelDrawer drawer;

	private CollapsibleDetailContentPanel immuneSystemPanel;

	private CollapsibleDetailContentPanel diseaseSourcePanel;

	private CollapsibleDetailContentPanel currentGermsPanel;

	private CollapsibleDetailContentPanel overviewPanel;

	private CollapsibleDetailContentPanel generatorsPanel;

	private CollapsibleDetailContentPanel consumersPanel;

	private CollapsibleDetailContentPanel batteriesPanel;

	private static readonly EventSystem.IntraObjectHandler<AdditionalDetailsPanel> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<AdditionalDetailsPanel>(delegate(AdditionalDetailsPanel component, object data)
	{
		component.OnRefreshData(data);
	});

	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		detailsPanel = CreateCollapsableSection(UI.DETAILTABS.DETAILS.GROUPNAME_DETAILS);
		drawer = new DetailsPanelDrawer(attributesLabelTemplate, detailsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
		immuneSystemPanel = CreateCollapsableSection(UI.DETAILTABS.DISEASE.CONTRACTION_RATES);
		diseaseSourcePanel = CreateCollapsableSection(UI.DETAILTABS.DISEASE.DISEASE_SOURCE);
		currentGermsPanel = CreateCollapsableSection(UI.DETAILTABS.DISEASE.CURRENT_GERMS);
		overviewPanel = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.CIRCUITOVERVIEW);
		generatorsPanel = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.GENERATORS);
		consumersPanel = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.CONSUMERS);
		batteriesPanel = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.BATTERIES);
		Subscribe(-1514841199, OnRefreshDataDelegate);
	}

	private void OnRefreshData(object obj)
	{
		Refresh();
	}

	private void Update()
	{
		Refresh();
	}

	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		Refresh();
	}

	private void Refresh()
	{
		RefreshDetailsPanel(detailsPanel, selectedTarget);
		RefreshImuneSystemPanel(immuneSystemPanel, selectedTarget);
		RefreshCurrentGermsPanel(currentGermsPanel, selectedTarget);
		RefreshDiseaseSourcePanel(diseaseSourcePanel, selectedTarget);
		RefreshEnergyOverviewPanel(overviewPanel, selectedTarget);
		RefreshEnergyGeneratorPanel(generatorsPanel, selectedTarget);
		RefreshEnergyConsumerPanel(consumersPanel, selectedTarget);
		RefreshEnergyBatteriesPanel(batteriesPanel, selectedTarget);
	}

	private static void RefreshDetailsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		PrimaryElement component = targetEntity.GetComponent<PrimaryElement>();
		CellSelectionObject component2 = targetEntity.GetComponent<CellSelectionObject>();
		float mass;
		float temperature;
		Element element;
		byte diseaseIdx;
		int diseaseCount;
		if (component != null)
		{
			mass = component.Mass;
			temperature = component.Temperature;
			element = component.Element;
			diseaseIdx = component.DiseaseIdx;
			diseaseCount = component.DiseaseCount;
		}
		else
		{
			if (!(component2 != null))
			{
				return;
			}
			mass = component2.Mass;
			temperature = component2.temperature;
			element = component2.element;
			diseaseIdx = component2.diseaseIdx;
			diseaseCount = component2.diseaseCount;
		}
		bool flag = element.id == SimHashes.Vacuum || element.id == SimHashes.Void;
		float specificHeatCapacity = element.specificHeatCapacity;
		float highTemp = element.highTemp;
		float lowTemp = element.lowTemp;
		BuildingComplete component3 = targetEntity.GetComponent<BuildingComplete>();
		float num = ((!(component3 != null)) ? (-1f) : component3.creationTime);
		LogicPorts component4 = targetEntity.GetComponent<LogicPorts>();
		EnergyConsumer component5 = targetEntity.GetComponent<EnergyConsumer>();
		Operational component6 = targetEntity.GetComponent<Operational>();
		Battery component7 = targetEntity.GetComponent<Battery>();
		targetPanel.SetLabel("element_name", string.Format(UI.ELEMENTAL.PRIMARYELEMENT.NAME, element.name), string.Format(UI.ELEMENTAL.PRIMARYELEMENT.TOOLTIP, element.name));
		targetPanel.SetLabel("element_mass", string.Format(UI.ELEMENTAL.MASS.NAME, GameUtil.GetFormattedMass(mass)), string.Format(UI.ELEMENTAL.MASS.TOOLTIP, GameUtil.GetFormattedMass(mass)));
		if (num > 0f)
		{
			targetPanel.SetLabel("element_age", string.Format(UI.ELEMENTAL.AGE.NAME, Util.FormatTwoDecimalPlace((GameClock.Instance.GetTime() - num) / 600f)), string.Format(UI.ELEMENTAL.AGE.TOOLTIP, Util.FormatTwoDecimalPlace((GameClock.Instance.GetTime() - num) / 600f)));
		}
		int num_cycles = 5;
		float num2;
		float num3;
		float num4;
		if (component6 != null && (component4 != null || component5 != null || component7 != null))
		{
			num2 = component6.GetCurrentCycleUptime();
			num3 = component6.GetLastCycleUptime();
			num4 = component6.GetUptimeOverCycles(num_cycles);
		}
		else
		{
			num2 = -1f;
			num3 = -1f;
			num4 = -1f;
		}
		if (num2 >= 0f)
		{
			string text = UI.ELEMENTAL.UPTIME.NAME;
			text = text.Replace("{0}", "    • ");
			text = text.Replace("{1}", UI.ELEMENTAL.UPTIME.THIS_CYCLE);
			text = text.Replace("{2}", GameUtil.GetFormattedPercent(num2 * 100f));
			text = text.Replace("{3}", UI.ELEMENTAL.UPTIME.LAST_CYCLE);
			text = text.Replace("{4}", GameUtil.GetFormattedPercent(num3 * 100f));
			text = text.Replace("{5}", UI.ELEMENTAL.UPTIME.LAST_X_CYCLES.Replace("{0}", num_cycles.ToString()));
			text = text.Replace("{6}", GameUtil.GetFormattedPercent(num4 * 100f));
			targetPanel.SetLabel("uptime_name", text, "");
		}
		if (!flag)
		{
			bool flag2 = false;
			float num5 = element.thermalConductivity;
			Building component8 = targetEntity.GetComponent<Building>();
			if (component8 != null)
			{
				num5 *= component8.Def.ThermalConductivity;
				flag2 = component8.Def.ThermalConductivity < 1f;
			}
			string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
			float shc = specificHeatCapacity * 1f;
			string text2 = string.Format(UI.ELEMENTAL.SHC.NAME, GameUtil.GetDisplaySHC(shc).ToString("0.000"));
			string text3 = UI.ELEMENTAL.SHC.TOOLTIP;
			text3 = text3.Replace("{SPECIFIC_HEAT_CAPACITY}", text2 + GameUtil.GetSHCSuffix());
			text3 = text3.Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
			string text4 = string.Format(UI.ELEMENTAL.THERMALCONDUCTIVITY.NAME, GameUtil.GetDisplayThermalConductivity(num5).ToString("0.000"));
			string text5 = UI.ELEMENTAL.THERMALCONDUCTIVITY.TOOLTIP;
			text5 = text5.Replace("{THERMAL_CONDUCTIVITY}", text4 + GameUtil.GetThermalConductivitySuffix());
			text5 = text5.Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
			targetPanel.SetLabel("temperature", string.Format(UI.ELEMENTAL.TEMPERATURE.NAME, GameUtil.GetFormattedTemperature(temperature)), string.Format(UI.ELEMENTAL.TEMPERATURE.TOOLTIP, GameUtil.GetFormattedTemperature(temperature)));
			targetPanel.SetLabel("disease", string.Format(UI.ELEMENTAL.DISEASE.NAME, GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount)), string.Format(UI.ELEMENTAL.DISEASE.TOOLTIP, GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount, color: true)));
			targetPanel.SetLabel("shc", text2, text3);
			targetPanel.SetLabel("tc", text4, text5);
			if (flag2)
			{
				targetPanel.SetLabel("insulated", UI.GAMEOBJECTEFFECTS.INSULATED.NAME, UI.GAMEOBJECTEFFECTS.INSULATED.TOOLTIP);
			}
		}
		if (element.IsSolid)
		{
			targetPanel.SetLabel("melting_point", string.Format(UI.ELEMENTAL.MELTINGPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp)), string.Format(UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp)));
			targetPanel.SetLabel("melting_point", string.Format(UI.ELEMENTAL.MELTINGPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp)), string.Format(UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp)));
			if (targetEntity.GetComponent<ElementChunk>() != null)
			{
				AttributeModifier attributeModifier = component.Element.attributeModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().BuildingAttributes.OverheatTemperature.Id);
				if (attributeModifier != null)
				{
					targetPanel.SetLabel("overheat", string.Format(UI.ELEMENTAL.OVERHEATPOINT.NAME, attributeModifier.GetFormattedString()), string.Format(UI.ELEMENTAL.OVERHEATPOINT.TOOLTIP, attributeModifier.GetFormattedString()));
				}
			}
		}
		else if (element.IsLiquid)
		{
			targetPanel.SetLabel("freezepoint", string.Format(UI.ELEMENTAL.FREEZEPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp)), string.Format(UI.ELEMENTAL.FREEZEPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(lowTemp)));
			targetPanel.SetLabel("vapourizationpoint", string.Format(UI.ELEMENTAL.VAPOURIZATIONPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp)), string.Format(UI.ELEMENTAL.VAPOURIZATIONPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp)));
		}
		else if (!flag)
		{
			targetPanel.SetLabel("dewpoint", string.Format(UI.ELEMENTAL.DEWPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp)), string.Format(UI.ELEMENTAL.DEWPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(lowTemp)));
		}
		if (DlcManager.FeatureRadiationEnabled())
		{
			string formattedPercent = GameUtil.GetFormattedPercent(GameUtil.GetRadiationAbsorptionPercentage(Grid.PosToCell(targetEntity)) * 100f);
			targetPanel.SetLabel("radiationabsorption", string.Format(UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.NAME, formattedPercent), string.Format(UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.TOOLTIP, formattedPercent));
		}
		Attributes attributes = targetEntity.GetAttributes();
		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				AttributeInstance attributeInstance = attributes.AttributeTable[i];
				if (DlcManager.IsDlcListValidForCurrentContent(attributeInstance.Attribute.DLCIds) && (attributeInstance.Attribute.ShowInUI == Attribute.Display.Details || attributeInstance.Attribute.ShowInUI == Attribute.Display.Expectation))
				{
					targetPanel.SetLabel(attributeInstance.modifier.Id, attributeInstance.modifier.Name + ": " + attributeInstance.GetFormattedValue(), attributeInstance.GetAttributeValueTooltip());
				}
			}
		}
		List<Descriptor> detailDescriptors = GameUtil.GetDetailDescriptors(GameUtil.GetAllDescriptors(targetEntity));
		for (int j = 0; j < detailDescriptors.Count; j++)
		{
			Descriptor descriptor = detailDescriptors[j];
			targetPanel.SetLabel("descriptor_" + j, descriptor.text, descriptor.tooltipText);
		}
		targetPanel.Commit();
	}

	private static void RefreshDiseaseSourcePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(targetEntity, simpleInfoScreen: true);
		Sicknesses sicknesses = targetEntity.GetSicknesses();
		if (sicknesses != null)
		{
			for (int i = 0; i < sicknesses.Count; i++)
			{
				allDescriptors.AddRange(sicknesses[i].GetDescriptors());
			}
		}
		allDescriptors = allDescriptors.FindAll((Descriptor e) => e.type == Descriptor.DescriptorType.DiseaseSource);
		if (allDescriptors.Count > 0)
		{
			for (int j = 0; j < allDescriptors.Count; j++)
			{
				targetPanel.SetLabel("source_" + j, allDescriptors[j].text, allDescriptors[j].tooltipText);
			}
		}
		targetPanel.Commit();
	}

	private static void RefreshCurrentGermsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity != null)
		{
			CellSelectionObject component = targetEntity.GetComponent<CellSelectionObject>();
			if (component != null)
			{
				if (component.diseaseIdx != byte.MaxValue && component.diseaseCount > 0)
				{
					Disease disease = Db.Get().Diseases[component.diseaseIdx];
					BuildFactorsStrings(targetPanel, component.diseaseCount, component.element.idx, component.SelectedCell, component.Mass, component.temperature, null, disease, isCell: true);
				}
				else
				{
					targetPanel.SetLabel("currentgerms", UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
				}
			}
			else
			{
				PrimaryElement component2 = targetEntity.GetComponent<PrimaryElement>();
				if (component2 != null)
				{
					if (component2.DiseaseIdx != byte.MaxValue && component2.DiseaseCount > 0)
					{
						Disease disease2 = Db.Get().Diseases[component2.DiseaseIdx];
						int environmentCell = Grid.PosToCell(component2.transform.GetPosition());
						KPrefabID component3 = component2.GetComponent<KPrefabID>();
						BuildFactorsStrings(targetPanel, component2.DiseaseCount, component2.Element.idx, environmentCell, component2.Mass, component2.Temperature, component3.Tags, disease2);
					}
					else
					{
						targetPanel.SetLabel("currentgerms", UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
					}
				}
			}
		}
		targetPanel.Commit();
	}

	private static void RefreshImuneSystemPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		GermExposureMonitor.Instance sMI = targetEntity.GetSMI<GermExposureMonitor.Instance>();
		if (sMI != null)
		{
			targetPanel.SetLabel("germ_resistance", Db.Get().Attributes.GermResistance.Name + ": " + sMI.GetGermResistance(), DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.DESC);
			for (int i = 0; i < Db.Get().Diseases.Count; i++)
			{
				Disease disease = Db.Get().Diseases[i];
				ExposureType exposureTypeForDisease = GameUtil.GetExposureTypeForDisease(disease);
				Sickness sicknessForDisease = GameUtil.GetSicknessForDisease(disease);
				if (sicknessForDisease == null)
				{
					continue;
				}
				bool flag = true;
				List<string> list = new List<string>();
				if (exposureTypeForDisease.required_traits != null && exposureTypeForDisease.required_traits.Count > 0)
				{
					for (int j = 0; j < exposureTypeForDisease.required_traits.Count; j++)
					{
						if (!targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.required_traits[j]))
						{
							list.Add(exposureTypeForDisease.required_traits[j]);
						}
					}
					if (list.Count > 0)
					{
						flag = false;
					}
				}
				bool flag2 = false;
				List<string> list2 = new List<string>();
				if (exposureTypeForDisease.excluded_effects != null && exposureTypeForDisease.excluded_effects.Count > 0)
				{
					for (int k = 0; k < exposureTypeForDisease.excluded_effects.Count; k++)
					{
						if (targetEntity.GetComponent<Effects>().HasEffect(exposureTypeForDisease.excluded_effects[k]))
						{
							list2.Add(exposureTypeForDisease.excluded_effects[k]);
						}
					}
					if (list2.Count > 0)
					{
						flag2 = true;
					}
				}
				bool flag3 = false;
				List<string> list3 = new List<string>();
				if (exposureTypeForDisease.excluded_traits != null && exposureTypeForDisease.excluded_traits.Count > 0)
				{
					for (int l = 0; l < exposureTypeForDisease.excluded_traits.Count; l++)
					{
						if (targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.excluded_traits[l]))
						{
							list3.Add(exposureTypeForDisease.excluded_traits[l]);
						}
					}
					if (list3.Count > 0)
					{
						flag3 = true;
					}
				}
				string text = "";
				float num;
				if (!flag)
				{
					num = 0f;
					string text2 = "";
					for (int m = 0; m < list.Count; m++)
					{
						if (text2 != "")
						{
							text2 += ", ";
						}
						text2 += Db.Get().traits.Get(list[m]).Name;
					}
					text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_MISSING_REQUIRED_TRAIT, text2);
				}
				else if (flag3)
				{
					num = 0f;
					string text3 = "";
					for (int n = 0; n < list3.Count; n++)
					{
						if (text3 != "")
						{
							text3 += ", ";
						}
						text3 += Db.Get().traits.Get(list3[n]).Name;
					}
					if (text != "")
					{
						text += "\n";
					}
					text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT, text3);
				}
				else if (!flag2)
				{
					num = ((!exposureTypeForDisease.infect_immediately) ? GermExposureMonitor.GetContractionChance(sMI.GetResistanceToExposureType(exposureTypeForDisease, 3f)) : 1f);
				}
				else
				{
					num = 0f;
					string text4 = "";
					for (int num2 = 0; num2 < list2.Count; num2++)
					{
						if (text4 != "")
						{
							text4 += ", ";
						}
						text4 += Db.Get().effects.Get(list2[num2]).Name;
					}
					if (text != "")
					{
						text += "\n";
					}
					text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXCLUDED_EFFECT, text4);
				}
				string arg = ((text != "") ? text : string.Format(DUPLICANTS.DISEASES.CONTRACTION_PROBABILITY, GameUtil.GetFormattedPercent(num * 100f), targetEntity.GetProperName(), sicknessForDisease.Name));
				targetPanel.SetLabel("disease_" + disease.Id, "    • " + disease.Name + ": " + GameUtil.GetFormattedPercent(num * 100f), string.Format(DUPLICANTS.DISEASES.RESISTANCES_PANEL_TOOLTIP, arg, sicknessForDisease.Name));
			}
		}
		targetPanel.Commit();
	}

	private static string GetFormattedHalfLife(float hl)
	{
		return GetFormattedGrowthRate(Disease.HalfLifeToGrowthRate(hl, 600f));
	}

	private static string GetFormattedGrowthRate(float rate)
	{
		if (rate < 1f)
		{
			return string.Format(UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT, GameUtil.GetFormattedPercent(100f * (1f - rate)), UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT_TOOLTIP);
		}
		if (rate > 1f)
		{
			return string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT, GameUtil.GetFormattedPercent(100f * (rate - 1f)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT_TOOLTIP);
		}
		return string.Format(UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT, UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT_TOOLTIP);
	}

	private static string GetFormattedGrowthEntry(string name, float halfLife, string dyingFormat, string growingFormat, string neutralFormat)
	{
		string format = ((halfLife == float.PositiveInfinity) ? neutralFormat : ((!(halfLife > 0f)) ? growingFormat : dyingFormat));
		return string.Format(format, name, GetFormattedHalfLife(halfLife));
	}

	private static void BuildFactorsStrings(CollapsibleDetailContentPanel targetPanel, int diseaseCount, ushort elementIdx, int environmentCell, float environmentMass, float temperature, HashSet<Tag> tags, Disease disease, bool isCell = false)
	{
		targetPanel.SetTitle(string.Format(UI.DETAILTABS.DISEASE.CURRENT_GERMS, disease.Name.ToUpper()));
		targetPanel.SetLabel("currentgerms", string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT, disease.Name, GameUtil.GetFormattedDiseaseAmount(diseaseCount)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT_TOOLTIP, GameUtil.GetFormattedDiseaseAmount(diseaseCount)));
		Element e = ElementLoader.elements[elementIdx];
		CompositeGrowthRule growthRuleForElement = disease.GetGrowthRuleForElement(e);
		float tags_multiplier_base = 1f;
		if (tags != null && tags.Count > 0)
		{
			tags_multiplier_base = disease.GetGrowthRateForTags(tags, (float)diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass);
		}
		float num = DiseaseContainers.CalculateDelta(diseaseCount, elementIdx, environmentMass, environmentCell, temperature, tags_multiplier_base, disease, 1f, Sim.IsRadiationEnabled());
		targetPanel.SetLabel("finaldelta", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE, GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE_TOOLTIP, GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")));
		float num2 = Disease.GrowthRateToHalfLife(1f - num / (float)diseaseCount);
		if (num2 > 0f)
		{
			targetPanel.SetLabel("finalhalflife", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG, GameUtil.GetFormattedCycles(num2)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG_TOOLTIP, GameUtil.GetFormattedCycles(num2)));
		}
		else if (num2 < 0f)
		{
			targetPanel.SetLabel("finalhalflife", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS, GameUtil.GetFormattedCycles(0f - num2)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS_TOOLTIP, GameUtil.GetFormattedCycles(num2)));
		}
		else
		{
			targetPanel.SetLabel("finalhalflife", UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL_TOOLTIP);
		}
		targetPanel.SetLabel("factors", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TITLE), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TOOLTIP);
		bool flag = false;
		if ((float)diseaseCount < growthRuleForElement.minCountPerKG * environmentMass)
		{
			targetPanel.SetLabel("critical_status", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TITLE, GetFormattedGrowthRate(0f - growthRuleForElement.underPopulationDeathRate)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TOOLTIP, GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.minCountPerKG * environmentMass)), GameUtil.GetFormattedMass(environmentMass), growthRuleForElement.minCountPerKG));
			flag = true;
		}
		else if ((float)diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass)
		{
			targetPanel.SetLabel("critical_status", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TITLE, GetFormattedHalfLife(growthRuleForElement.overPopulationHalfLife)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TOOLTIP, GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.maxCountPerKG * environmentMass)), GameUtil.GetFormattedMass(environmentMass), growthRuleForElement.maxCountPerKG));
			flag = true;
		}
		if (!flag)
		{
			targetPanel.SetLabel("substrate", GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
		}
		int num3 = 0;
		if (tags != null)
		{
			foreach (Tag tag in tags)
			{
				TagGrowthRule growthRuleForTag = disease.GetGrowthRuleForTag(tag);
				if (growthRuleForTag != null)
				{
					targetPanel.SetLabel("tag_" + num3, GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
				}
				num3++;
			}
		}
		if (Grid.IsValidCell(environmentCell))
		{
			if (!isCell)
			{
				CompositeExposureRule exposureRuleForElement = disease.GetExposureRuleForElement(Grid.Element[environmentCell]);
				if (exposureRuleForElement != null && exposureRuleForElement.populationHalfLife != float.PositiveInfinity)
				{
					if (exposureRuleForElement.GetHalfLifeForCount(diseaseCount) > 0f)
					{
						targetPanel.SetLabel("environment", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, exposureRuleForElement.Name(), GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.DIE_TOOLTIP);
					}
					else
					{
						targetPanel.SetLabel("environment", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, exposureRuleForElement.Name(), GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.GROW_TOOLTIP);
					}
				}
			}
			if (Sim.IsRadiationEnabled())
			{
				float num4 = Grid.Radiation[environmentCell];
				if (num4 > 0f)
				{
					float num5 = disease.radiationKillRate * num4;
					float hl = (float)diseaseCount * 0.5f / num5;
					targetPanel.SetLabel("radiation", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.TITLE, Mathf.RoundToInt(num4), GetFormattedHalfLife(hl)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.DIE_TOOLTIP);
				}
			}
		}
		float num6 = disease.CalculateTemperatureHalfLife(temperature);
		if (num6 != float.PositiveInfinity)
		{
			if (num6 > 0f)
			{
				targetPanel.SetLabel("temperature", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, GameUtil.GetFormattedTemperature(temperature), GetFormattedHalfLife(num6)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.DIE_TOOLTIP);
			}
			else
			{
				targetPanel.SetLabel("temperature", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, GameUtil.GetFormattedTemperature(temperature), GetFormattedHalfLife(num6)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.GROW_TOOLTIP);
			}
		}
	}

	private static void RefreshEnergyOverviewPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		if (targetEntity.GetComponent<ICircuitConnected>() != null || targetEntity.GetComponent<Wire>() != null)
		{
			ushort selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
			if (selectedTargetCircuitID == ushort.MaxValue)
			{
				targetPanel.SetLabel("nocircuit", UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED, UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED);
			}
			else
			{
				float joulesAvailableOnCircuit = Game.Instance.circuitManager.GetJoulesAvailableOnCircuit(selectedTargetCircuitID);
				targetPanel.SetLabel("joulesAvailable", string.Format(UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES, GameUtil.GetFormattedJoules(joulesAvailableOnCircuit)), UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES_TOOLTIP);
				float wattsGeneratedByCircuit = Game.Instance.circuitManager.GetWattsGeneratedByCircuit(selectedTargetCircuitID);
				float potentialWattsGeneratedByCircuit = Game.Instance.circuitManager.GetPotentialWattsGeneratedByCircuit(selectedTargetCircuitID);
				string text = null;
				targetPanel.SetLabel("wattageGenerated", string.Format(arg0: (wattsGeneratedByCircuit != potentialWattsGeneratedByCircuit) ? $"{GameUtil.GetFormattedWattage(wattsGeneratedByCircuit)} / {GameUtil.GetFormattedWattage(potentialWattsGeneratedByCircuit)}" : GameUtil.GetFormattedWattage(wattsGeneratedByCircuit), format: UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED), UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED_TOOLTIP);
				targetPanel.SetLabel("wattageConsumed", string.Format(UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED, GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetWattsUsedByCircuit(selectedTargetCircuitID))), UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED_TOOLTIP);
				targetPanel.SetLabel("potentialWattageConsumed", string.Format(UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED, GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetWattsNeededWhenActive(selectedTargetCircuitID))), UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED_TOOLTIP);
				targetPanel.SetLabel("maxSafeWattage", string.Format(UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE, GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetMaxSafeWattageForCircuit(selectedTargetCircuitID))), UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE_TOOLTIP);
			}
		}
		targetPanel.Commit();
	}

	private static void RefreshEnergyGeneratorPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		ushort selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
		if (selectedTargetCircuitID == ushort.MaxValue)
		{
			targetPanel.SetActive(active: false);
			return;
		}
		targetPanel.SetActive(active: true);
		ReadOnlyCollection<Generator> generatorsOnCircuit = Game.Instance.circuitManager.GetGeneratorsOnCircuit(selectedTargetCircuitID);
		if (generatorsOnCircuit.Count > 0)
		{
			foreach (Generator item in generatorsOnCircuit)
			{
				if (item != null && item.GetComponent<Battery>() == null)
				{
					string text = "";
					text = ((!item.IsProducingPower()) ? $"{item.GetComponent<KSelectable>().entityName}: {GameUtil.GetFormattedWattage(0f)} / {GameUtil.GetFormattedWattage(item.WattageRating)}" : $"{item.GetComponent<KSelectable>().entityName}: {GameUtil.GetFormattedWattage(item.WattageRating)}");
					text = ((item.gameObject == targetEntity) ? ("<b>" + text + "</b>") : text);
					targetPanel.SetLabel(item.gameObject.GetInstanceID().ToString(), text, "");
				}
			}
		}
		else
		{
			targetPanel.SetLabel("nogenerators", UI.DETAILTABS.ENERGYGENERATOR.NOGENERATORS, "");
		}
		targetPanel.Commit();
	}

	private static void RefreshEnergyConsumerPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		ushort selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
		if (selectedTargetCircuitID == ushort.MaxValue)
		{
			targetPanel.SetActive(active: false);
			return;
		}
		targetPanel.SetActive(active: true);
		ReadOnlyCollection<IEnergyConsumer> consumersOnCircuit = Game.Instance.circuitManager.GetConsumersOnCircuit(selectedTargetCircuitID);
		ReadOnlyCollection<Battery> transformersOnCircuit = Game.Instance.circuitManager.GetTransformersOnCircuit(selectedTargetCircuitID);
		if (consumersOnCircuit.Count > 0 || transformersOnCircuit.Count > 0)
		{
			foreach (IEnergyConsumer item in consumersOnCircuit)
			{
				AddConsumerInfo(item);
			}
			foreach (Battery item2 in transformersOnCircuit)
			{
				AddConsumerInfo(item2);
			}
		}
		else
		{
			targetPanel.SetLabel("noconsumers", UI.DETAILTABS.ENERGYGENERATOR.NOCONSUMERS, "");
		}
		targetPanel.Commit();
		void AddConsumerInfo(IEnergyConsumer consumer)
		{
			KMonoBehaviour kMonoBehaviour = consumer as KMonoBehaviour;
			if (kMonoBehaviour != null)
			{
				float wattsUsed = consumer.WattsUsed;
				float wattsNeededWhenActive = consumer.WattsNeededWhenActive;
				string text = null;
				string text2 = string.Format(arg1: (wattsUsed != wattsNeededWhenActive) ? $"{GameUtil.GetFormattedWattage(wattsUsed)} / {GameUtil.GetFormattedWattage(wattsNeededWhenActive)}" : GameUtil.GetFormattedWattage(wattsUsed), format: "{0}: {1}", arg0: consumer.Name);
				text2 = ((kMonoBehaviour.gameObject == targetEntity) ? ("<b>" + text2 + "</b>") : text2);
				targetPanel.SetLabel(kMonoBehaviour.gameObject.GetInstanceID().ToString(), text2, "");
			}
		}
	}

	private static void RefreshEnergyBatteriesPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		ushort selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
		if (selectedTargetCircuitID == ushort.MaxValue)
		{
			targetPanel.SetActive(active: false);
			return;
		}
		targetPanel.SetActive(active: true);
		List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(selectedTargetCircuitID);
		if (batteriesOnCircuit.Count > 0)
		{
			foreach (Battery item in batteriesOnCircuit)
			{
				if (item != null)
				{
					string text = $"{item.GetComponent<KSelectable>().entityName}: {GameUtil.GetFormattedJoules(item.JoulesAvailable)}";
					text = ((item.gameObject == targetEntity) ? ("<b>" + text + "</b>") : text);
					targetPanel.SetLabel(item.gameObject.GetInstanceID().ToString(), text, "");
				}
			}
		}
		else
		{
			targetPanel.SetLabel("nobatteries", UI.DETAILTABS.ENERGYGENERATOR.NOBATTERIES, "");
		}
		targetPanel.Commit();
	}

	private static ushort GetSelectedTargetCircuitID(GameObject targetEntity)
	{
		_ = Game.Instance.circuitManager;
		ICircuitConnected component = targetEntity.GetComponent<ICircuitConnected>();
		ushort result = ushort.MaxValue;
		if (component != null)
		{
			result = Game.Instance.circuitManager.GetCircuitID(component);
		}
		else if (targetEntity.GetComponent<Wire>() != null)
		{
			int cell = Grid.PosToCell(targetEntity.transform.GetPosition());
			result = Game.Instance.circuitManager.GetCircuitID(cell);
		}
		return result;
	}
}
