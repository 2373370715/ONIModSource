using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using UnityEngine;
using Attribute = Klei.AI.Attribute;

public class AdditionalDetailsPanel : DetailScreenTab {
    private static readonly EventSystem.IntraObjectHandler<AdditionalDetailsPanel> OnRefreshDataDelegate
        = new EventSystem.IntraObjectHandler<AdditionalDetailsPanel>(delegate(AdditionalDetailsPanel component,
                                                                              object                 data) {
                                                                         component.OnRefreshData(data);
                                                                     });

    public          GameObject                    attributesLabelTemplate;
    private         CollapsibleDetailContentPanel batteriesPanel;
    private         CollapsibleDetailContentPanel consumersPanel;
    private         CollapsibleDetailContentPanel currentGermsPanel;
    private         CollapsibleDetailContentPanel detailsPanel;
    private         CollapsibleDetailContentPanel diseaseSourcePanel;
    private         DetailsPanelDrawer            drawer;
    private         CollapsibleDetailContentPanel generatorsPanel;
    private         CollapsibleDetailContentPanel immuneSystemPanel;
    private         CollapsibleDetailContentPanel overviewPanel;
    public override bool                          IsValidForTarget(GameObject target) { return true; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        detailsPanel = CreateCollapsableSection(UI.DETAILTABS.DETAILS.GROUPNAME_DETAILS);
        drawer = new DetailsPanelDrawer(attributesLabelTemplate,
                                        detailsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);

        immuneSystemPanel  = CreateCollapsableSection(UI.DETAILTABS.DISEASE.CONTRACTION_RATES);
        diseaseSourcePanel = CreateCollapsableSection(UI.DETAILTABS.DISEASE.DISEASE_SOURCE);
        currentGermsPanel  = CreateCollapsableSection(UI.DETAILTABS.DISEASE.CURRENT_GERMS);
        overviewPanel      = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.CIRCUITOVERVIEW);
        generatorsPanel    = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.GENERATORS);
        consumersPanel     = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.CONSUMERS);
        batteriesPanel     = CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.BATTERIES);
        Subscribe(-1514841199, OnRefreshDataDelegate);
    }

    private void OnRefreshData(object obj) { Refresh(); }
    private void Update()                  { Refresh(); }

    protected override void OnSelectTarget(GameObject target) {
        base.OnSelectTarget(target);
        Refresh();
    }

    private void Refresh() {
        RefreshDetailsPanel(detailsPanel, selectedTarget);
        RefreshImuneSystemPanel(immuneSystemPanel, selectedTarget);
        RefreshCurrentGermsPanel(currentGermsPanel, selectedTarget);
        RefreshDiseaseSourcePanel(diseaseSourcePanel, selectedTarget);
        RefreshEnergyOverviewPanel(overviewPanel, selectedTarget);
        RefreshEnergyGeneratorPanel(generatorsPanel, selectedTarget);
        RefreshEnergyConsumerPanel(consumersPanel, selectedTarget);
        RefreshEnergyBatteriesPanel(batteriesPanel, selectedTarget);
    }

    private static void RefreshDetailsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var     component  = targetEntity.GetComponent<PrimaryElement>();
        var     component2 = targetEntity.GetComponent<CellSelectionObject>();
        float   mass;
        float   temperature;
        Element element;
        byte    diseaseIdx;
        int     diseaseCount;
        if (component != null) {
            mass         = component.Mass;
            temperature  = component.Temperature;
            element      = component.Element;
            diseaseIdx   = component.DiseaseIdx;
            diseaseCount = component.DiseaseCount;
        } else {
            if (!(component2 != null)) return;

            mass         = component2.Mass;
            temperature  = component2.temperature;
            element      = component2.element;
            diseaseIdx   = component2.diseaseIdx;
            diseaseCount = component2.diseaseCount;
        }

        var   flag                 = element.id == SimHashes.Vacuum || element.id == SimHashes.Void;
        var   specificHeatCapacity = element.specificHeatCapacity;
        var   highTemp             = element.highTemp;
        var   lowTemp              = element.lowTemp;
        var   component3           = targetEntity.GetComponent<BuildingComplete>();
        float num;
        if (component3 != null)
            num = component3.creationTime;
        else
            num = -1f;

        var component4 = targetEntity.GetComponent<LogicPorts>();
        var component5 = targetEntity.GetComponent<EnergyConsumer>();
        var component6 = targetEntity.GetComponent<Operational>();
        var component7 = targetEntity.GetComponent<Battery>();
        targetPanel.SetLabel("element_name",
                             string.Format(UI.ELEMENTAL.PRIMARYELEMENT.NAME,    element.name),
                             string.Format(UI.ELEMENTAL.PRIMARYELEMENT.TOOLTIP, element.name));

        targetPanel.SetLabel("element_mass",
                             string.Format(UI.ELEMENTAL.MASS.NAME,    GameUtil.GetFormattedMass(mass)),
                             string.Format(UI.ELEMENTAL.MASS.TOOLTIP, GameUtil.GetFormattedMass(mass)));

        if (num > 0f)
            targetPanel.SetLabel("element_age",
                                 string.Format(UI.ELEMENTAL.AGE.NAME,
                                               Util.FormatTwoDecimalPlace((GameClock.Instance.GetTime() - num) / 600f)),
                                 string.Format(UI.ELEMENTAL.AGE.TOOLTIP,
                                               Util.FormatTwoDecimalPlace((GameClock.Instance.GetTime() - num) /
                                                                          600f)));

        var   num_cycles = 5;
        float num2;
        float num3;
        float num4;
        if (component6 != null && (component4 != null || component5 != null || component7 != null)) {
            num2 = component6.GetCurrentCycleUptime();
            num3 = component6.GetLastCycleUptime();
            num4 = component6.GetUptimeOverCycles(num_cycles);
        } else {
            num2 = -1f;
            num3 = -1f;
            num4 = -1f;
        }

        if (num2 >= 0f) {
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

        if (!flag) {
            var flag2      = false;
            var num5       = element.thermalConductivity;
            var component8 = targetEntity.GetComponent<Building>();
            if (component8 != null) {
                num5  *= component8.Def.ThermalConductivity;
                flag2 =  component8.Def.ThermalConductivity < 1f;
            }

            var    temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
            var    shc = specificHeatCapacity * 1f;
            var    text2 = string.Format(UI.ELEMENTAL.SHC.NAME, GameUtil.GetDisplaySHC(shc).ToString("0.000"));
            string text3 = UI.ELEMENTAL.SHC.TOOLTIP;
            text3 = text3.Replace("{SPECIFIC_HEAT_CAPACITY}", text2 + GameUtil.GetSHCSuffix());
            text3 = text3.Replace("{TEMPERATURE_UNIT}",       temperatureUnitSuffix);
            var text4 = string.Format(UI.ELEMENTAL.THERMALCONDUCTIVITY.NAME,
                                      GameUtil.GetDisplayThermalConductivity(num5).ToString("0.000"));

            string text5 = UI.ELEMENTAL.THERMALCONDUCTIVITY.TOOLTIP;
            text5 = text5.Replace("{THERMAL_CONDUCTIVITY}", text4 + GameUtil.GetThermalConductivitySuffix());
            text5 = text5.Replace("{TEMPERATURE_UNIT}",     temperatureUnitSuffix);
            targetPanel.SetLabel("temperature",
                                 string.Format(UI.ELEMENTAL.TEMPERATURE.NAME,
                                               GameUtil.GetFormattedTemperature(temperature)),
                                 string.Format(UI.ELEMENTAL.TEMPERATURE.TOOLTIP,
                                               GameUtil.GetFormattedTemperature(temperature)));

            targetPanel.SetLabel("disease",
                                 string.Format(UI.ELEMENTAL.DISEASE.NAME,
                                               GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount)),
                                 string.Format(UI.ELEMENTAL.DISEASE.TOOLTIP,
                                               GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount, true)));

            targetPanel.SetLabel("shc", text2, text3);
            targetPanel.SetLabel("tc",  text4, text5);
            if (flag2)
                targetPanel.SetLabel("insulated",
                                     UI.GAMEOBJECTEFFECTS.INSULATED.NAME,
                                     UI.GAMEOBJECTEFFECTS.INSULATED.TOOLTIP);
        }

        if (element.IsSolid) {
            targetPanel.SetLabel("melting_point",
                                 string.Format(UI.ELEMENTAL.MELTINGPOINT.NAME,
                                               GameUtil.GetFormattedTemperature(highTemp)),
                                 string.Format(UI.ELEMENTAL.MELTINGPOINT.TOOLTIP,
                                               GameUtil.GetFormattedTemperature(highTemp)));

            targetPanel.SetLabel("melting_point",
                                 string.Format(UI.ELEMENTAL.MELTINGPOINT.NAME,
                                               GameUtil.GetFormattedTemperature(highTemp)),
                                 string.Format(UI.ELEMENTAL.MELTINGPOINT.TOOLTIP,
                                               GameUtil.GetFormattedTemperature(highTemp)));

            if (targetEntity.GetComponent<ElementChunk>() != null) {
                var attributeModifier
                    = component.Element.attributeModifiers.Find(m => m.AttributeId ==
                                                                     Db.Get()
                                                                       .BuildingAttributes.OverheatTemperature.Id);

                if (attributeModifier != null)
                    targetPanel.SetLabel("overheat",
                                         string.Format(UI.ELEMENTAL.OVERHEATPOINT.NAME,
                                                       attributeModifier.GetFormattedString()),
                                         string.Format(UI.ELEMENTAL.OVERHEATPOINT.TOOLTIP,
                                                       attributeModifier.GetFormattedString()));
            }
        } else if (element.IsLiquid) {
            targetPanel.SetLabel("freezepoint",
                                 string.Format(UI.ELEMENTAL.FREEZEPOINT.NAME,
                                               GameUtil.GetFormattedTemperature(lowTemp)),
                                 string.Format(UI.ELEMENTAL.FREEZEPOINT.TOOLTIP,
                                               GameUtil.GetFormattedTemperature(lowTemp)));

            targetPanel.SetLabel("vapourizationpoint",
                                 string.Format(UI.ELEMENTAL.VAPOURIZATIONPOINT.NAME,
                                               GameUtil.GetFormattedTemperature(highTemp)),
                                 string.Format(UI.ELEMENTAL.VAPOURIZATIONPOINT.TOOLTIP,
                                               GameUtil.GetFormattedTemperature(highTemp)));
        } else if (!flag)
            targetPanel.SetLabel("dewpoint",
                                 string.Format(UI.ELEMENTAL.DEWPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp)),
                                 string.Format(UI.ELEMENTAL.DEWPOINT.TOOLTIP,
                                               GameUtil.GetFormattedTemperature(lowTemp)));

        if (DlcManager.FeatureRadiationEnabled()) {
            var formattedPercent
                = GameUtil.GetFormattedPercent(GameUtil.GetRadiationAbsorptionPercentage(Grid.PosToCell(targetEntity)) *
                                               100f);

            targetPanel.SetLabel("radiationabsorption",
                                 string.Format(UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.NAME, formattedPercent),
                                 string.Format(UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.TOOLTIP,
                                               formattedPercent));
        }

        var attributes = targetEntity.GetAttributes();
        if (attributes != null)
            for (var i = 0; i < attributes.Count; i++) {
                var attributeInstance = attributes.AttributeTable[i];
                if (DlcManager.IsDlcListValidForCurrentContent(attributeInstance.Attribute.DLCIds) &&
                    (attributeInstance.Attribute.ShowInUI == Attribute.Display.Details ||
                     attributeInstance.Attribute.ShowInUI == Attribute.Display.Expectation))
                    targetPanel.SetLabel(attributeInstance.modifier.Id,
                                         attributeInstance.modifier.Name + ": " + attributeInstance.GetFormattedValue(),
                                         attributeInstance.GetAttributeValueTooltip());
            }

        var detailDescriptors = GameUtil.GetDetailDescriptors(GameUtil.GetAllDescriptors(targetEntity));
        for (var j = 0; j < detailDescriptors.Count; j++) {
            var descriptor = detailDescriptors[j];
            targetPanel.SetLabel("descriptor_" + j, descriptor.text, descriptor.tooltipText);
        }

        targetPanel.Commit();
    }

    private static void RefreshDiseaseSourcePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var list       = GameUtil.GetAllDescriptors(targetEntity, true);
        var sicknesses = targetEntity.GetSicknesses();
        if (sicknesses != null)
            for (var i = 0; i < sicknesses.Count; i++)
                list.AddRange(sicknesses[i].GetDescriptors());

        list = list.FindAll(e => e.type == Descriptor.DescriptorType.DiseaseSource);
        if (list.Count > 0)
            for (var j = 0; j < list.Count; j++)
                targetPanel.SetLabel("source_" + j, list[j].text, list[j].tooltipText);

        targetPanel.Commit();
    }

    private static void RefreshCurrentGermsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        if (targetEntity != null) {
            var component = targetEntity.GetComponent<CellSelectionObject>();
            if (component != null) {
                if (component.diseaseIdx != 255 && component.diseaseCount > 0) {
                    var disease = Db.Get().Diseases[component.diseaseIdx];
                    BuildFactorsStrings(targetPanel,
                                        component.diseaseCount,
                                        component.element.idx,
                                        component.SelectedCell,
                                        component.Mass,
                                        component.temperature,
                                        null,
                                        disease,
                                        true);
                } else
                    targetPanel.SetLabel("currentgerms",
                                         UI.DETAILTABS.DISEASE.DETAILS.NODISEASE,
                                         UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
            } else {
                var component2 = targetEntity.GetComponent<PrimaryElement>();
                if (component2 != null) {
                    if (component2.DiseaseIdx != 255 && component2.DiseaseCount > 0) {
                        var disease2        = Db.Get().Diseases[component2.DiseaseIdx];
                        var environmentCell = Grid.PosToCell(component2.transform.GetPosition());
                        var component3      = component2.GetComponent<KPrefabID>();
                        BuildFactorsStrings(targetPanel,
                                            component2.DiseaseCount,
                                            component2.Element.idx,
                                            environmentCell,
                                            component2.Mass,
                                            component2.Temperature,
                                            component3.Tags,
                                            disease2);
                    } else
                        targetPanel.SetLabel("currentgerms",
                                             UI.DETAILTABS.DISEASE.DETAILS.NODISEASE,
                                             UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
                }
            }
        }

        targetPanel.Commit();
    }

    private static void RefreshImuneSystemPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var smi = targetEntity.GetSMI<GermExposureMonitor.Instance>();
        if (smi != null) {
            targetPanel.SetLabel("germ_resistance",
                                 Db.Get().Attributes.GermResistance.Name + ": " + smi.GetGermResistance(),
                                 DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.DESC);

            for (var i = 0; i < Db.Get().Diseases.Count; i++) {
                var disease                = Db.Get().Diseases[i];
                var exposureTypeForDisease = GameUtil.GetExposureTypeForDisease(disease);
                var sicknessForDisease     = GameUtil.GetSicknessForDisease(disease);
                if (sicknessForDisease != null) {
                    var flag = true;
                    var list = new List<string>();
                    if (exposureTypeForDisease.required_traits       != null &&
                        exposureTypeForDisease.required_traits.Count > 0) {
                        for (var j = 0; j < exposureTypeForDisease.required_traits.Count; j++)
                            if (!targetEntity.GetComponent<Traits>()
                                             .HasTrait(exposureTypeForDisease.required_traits[j]))
                                list.Add(exposureTypeForDisease.required_traits[j]);

                        if (list.Count > 0) flag = false;
                    }

                    var flag2 = false;
                    var list2 = new List<string>();
                    if (exposureTypeForDisease.excluded_effects       != null &&
                        exposureTypeForDisease.excluded_effects.Count > 0) {
                        for (var k = 0; k < exposureTypeForDisease.excluded_effects.Count; k++)
                            if (targetEntity.GetComponent<Effects>()
                                            .HasEffect(exposureTypeForDisease.excluded_effects[k]))
                                list2.Add(exposureTypeForDisease.excluded_effects[k]);

                        if (list2.Count > 0) flag2 = true;
                    }

                    var flag3 = false;
                    var list3 = new List<string>();
                    if (exposureTypeForDisease.excluded_traits       != null &&
                        exposureTypeForDisease.excluded_traits.Count > 0) {
                        for (var l = 0; l < exposureTypeForDisease.excluded_traits.Count; l++)
                            if (targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.excluded_traits[l]))
                                list3.Add(exposureTypeForDisease.excluded_traits[l]);

                        if (list3.Count > 0) flag3 = true;
                    }

                    var   text = "";
                    float num;
                    if (!flag) {
                        num = 0f;
                        var text2 = "";
                        for (var m = 0; m < list.Count; m++) {
                            if (text2 != "") text2 += ", ";
                            text2 += Db.Get().traits.Get(list[m]).Name;
                        }

                        text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_MISSING_REQUIRED_TRAIT, text2);
                    } else if (flag3) {
                        num = 0f;
                        var text3 = "";
                        for (var n = 0; n < list3.Count; n++) {
                            if (text3 != "") text3 += ", ";
                            text3 += Db.Get().traits.Get(list3[n]).Name;
                        }

                        if (text != "") text += "\n";
                        text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT, text3);
                    } else if (flag2) {
                        num = 0f;
                        var text4 = "";
                        for (var num2 = 0; num2 < list2.Count; num2++) {
                            if (text4 != "") text4 += ", ";
                            text4 += Db.Get().effects.Get(list2[num2]).Name;
                        }

                        if (text != "") text += "\n";
                        text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXCLUDED_EFFECT, text4);
                    } else if (exposureTypeForDisease.infect_immediately)
                        num = 1f;
                    else
                        num
                            = GermExposureMonitor
                                .GetContractionChance(smi.GetResistanceToExposureType(exposureTypeForDisease, 3f));

                    var arg = text != ""
                                  ? text
                                  : string.Format(DUPLICANTS.DISEASES.CONTRACTION_PROBABILITY,
                                                  GameUtil.GetFormattedPercent(num * 100f),
                                                  targetEntity.GetProperName(),
                                                  sicknessForDisease.Name);

                    targetPanel.SetLabel("disease_" + disease.Id,
                                         "    • "   + disease.Name + ": " + GameUtil.GetFormattedPercent(num * 100f),
                                         string.Format(DUPLICANTS.DISEASES.RESISTANCES_PANEL_TOOLTIP,
                                                       arg,
                                                       sicknessForDisease.Name));
                }
            }
        }

        targetPanel.Commit();
    }

    private static string GetFormattedHalfLife(float hl) {
        return GetFormattedGrowthRate(Disease.HalfLifeToGrowthRate(hl, 600f));
    }

    private static string GetFormattedGrowthRate(float rate) {
        if (rate < 1f)
            return string.Format(UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT,
                                 GameUtil.GetFormattedPercent(100f * (1f - rate)),
                                 UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT_TOOLTIP);

        if (rate > 1f)
            return string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT,
                                 GameUtil.GetFormattedPercent(100f * (rate - 1f)),
                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT_TOOLTIP);

        return string.Format(UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT,
                             UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT_TOOLTIP);
    }

    private static string GetFormattedGrowthEntry(string name,
                                                  float  halfLife,
                                                  string dyingFormat,
                                                  string growingFormat,
                                                  string neutralFormat) {
        string format;
        if (halfLife == float.PositiveInfinity)
            format = neutralFormat;
        else if (halfLife > 0f)
            format = dyingFormat;
        else
            format = growingFormat;

        return string.Format(format, name, GetFormattedHalfLife(halfLife));
    }

    private static void BuildFactorsStrings(CollapsibleDetailContentPanel targetPanel,
                                            int                           diseaseCount,
                                            ushort                        elementIdx,
                                            int                           environmentCell,
                                            float                         environmentMass,
                                            float                         temperature,
                                            HashSet<Tag>                  tags,
                                            Disease                       disease,
                                            bool                          isCell = false) {
        targetPanel.SetTitle(string.Format(UI.DETAILTABS.DISEASE.CURRENT_GERMS, disease.Name.ToUpper()));
        targetPanel.SetLabel("currentgerms",
                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT,
                                           disease.Name,
                                           GameUtil.GetFormattedDiseaseAmount(diseaseCount)),
                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT_TOOLTIP,
                                           GameUtil.GetFormattedDiseaseAmount(diseaseCount)));

        var e                    = ElementLoader.elements[elementIdx];
        var growthRuleForElement = disease.GetGrowthRuleForElement(e);
        var tags_multiplier_base = 1f;
        if (tags != null && tags.Count > 0)
            tags_multiplier_base
                = disease.GetGrowthRateForTags(tags,
                                               diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass);

        var num = DiseaseContainers.CalculateDelta(diseaseCount,
                                                   elementIdx,
                                                   environmentMass,
                                                   environmentCell,
                                                   temperature,
                                                   tags_multiplier_base,
                                                   disease,
                                                   1f,
                                                   Sim.IsRadiationEnabled());

        targetPanel.SetLabel("finaldelta",
                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE,
                                           GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")),
                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE_TOOLTIP,
                                           GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")));

        var num2 = Disease.GrowthRateToHalfLife(1f - num / diseaseCount);
        if (num2 > 0f)
            targetPanel.SetLabel("finalhalflife",
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG,
                                               GameUtil.GetFormattedCycles(num2)),
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG_TOOLTIP,
                                               GameUtil.GetFormattedCycles(num2)));
        else if (num2 < 0f)
            targetPanel.SetLabel("finalhalflife",
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS,
                                               GameUtil.GetFormattedCycles(-num2)),
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS_TOOLTIP,
                                               GameUtil.GetFormattedCycles(num2)));
        else
            targetPanel.SetLabel("finalhalflife",
                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL,
                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL_TOOLTIP);

        targetPanel.SetLabel("factors",
                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TITLE, Array.Empty<object>()),
                             UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TOOLTIP);

        var flag = false;
        if (diseaseCount < growthRuleForElement.minCountPerKG * environmentMass) {
            targetPanel.SetLabel("critical_status",
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TITLE,
                                               GetFormattedGrowthRate(-growthRuleForElement.underPopulationDeathRate)),
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TOOLTIP,
                                               GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement
                                                       .minCountPerKG *
                                                   environmentMass)),
                                               GameUtil.GetFormattedMass(environmentMass),
                                               growthRuleForElement.minCountPerKG));

            flag = true;
        } else if (diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass) {
            targetPanel.SetLabel("critical_status",
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TITLE,
                                               GetFormattedHalfLife(growthRuleForElement.overPopulationHalfLife)),
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TOOLTIP,
                                               GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement
                                                       .maxCountPerKG *
                                                   environmentMass)),
                                               GameUtil.GetFormattedMass(environmentMass),
                                               growthRuleForElement.maxCountPerKG));

            flag = true;
        }

        if (!flag)
            targetPanel.SetLabel("substrate",
                                 GetFormattedGrowthEntry(growthRuleForElement.Name(),
                                                         growthRuleForElement.populationHalfLife,
                                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE,
                                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW,
                                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                           .NEUTRAL),
                                 GetFormattedGrowthEntry(growthRuleForElement.Name(),
                                                         growthRuleForElement.populationHalfLife,
                                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                           .DIE_TOOLTIP,
                                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                           .GROW_TOOLTIP,
                                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                           .NEUTRAL_TOOLTIP));

        var num3 = 0;
        if (tags != null)
            foreach (var t in tags) {
                var growthRuleForTag = disease.GetGrowthRuleForTag(t);
                if (growthRuleForTag != null)
                    targetPanel.SetLabel("tag_" + num3,
                                         GetFormattedGrowthEntry(growthRuleForTag.Name(),
                                                                 growthRuleForTag.populationHalfLife.Value,
                                                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                                   .DIE,
                                                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                                   .GROW,
                                                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                                   .NEUTRAL),
                                         GetFormattedGrowthEntry(growthRuleForTag.Name(),
                                                                 growthRuleForTag.populationHalfLife.Value,
                                                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                                   .DIE_TOOLTIP,
                                                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                                   .GROW_TOOLTIP,
                                                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE
                                                                   .NEUTRAL_TOOLTIP));

                num3++;
            }

        if (Grid.IsValidCell(environmentCell)) {
            if (!isCell) {
                var exposureRuleForElement = disease.GetExposureRuleForElement(Grid.Element[environmentCell]);
                if (exposureRuleForElement                    != null &&
                    exposureRuleForElement.populationHalfLife != float.PositiveInfinity) {
                    if (exposureRuleForElement.GetHalfLifeForCount(diseaseCount) > 0f)
                        targetPanel.SetLabel("environment",
                                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT
                                                             .TITLE,
                                                           exposureRuleForElement.Name(),
                                                           GetFormattedHalfLife(exposureRuleForElement
                                                                                    .GetHalfLifeForCount(diseaseCount))),
                                             UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.DIE_TOOLTIP);
                    else
                        targetPanel.SetLabel("environment",
                                             string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT
                                                             .TITLE,
                                                           exposureRuleForElement.Name(),
                                                           GetFormattedHalfLife(exposureRuleForElement
                                                                                    .GetHalfLifeForCount(diseaseCount))),
                                             UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.GROW_TOOLTIP);
                }
            }

            if (Sim.IsRadiationEnabled()) {
                var num4 = Grid.Radiation[environmentCell];
                if (num4 > 0f) {
                    var num5 = disease.radiationKillRate * num4;
                    var hl   = diseaseCount * 0.5f       / num5;
                    targetPanel.SetLabel("radiation",
                                         string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.TITLE,
                                                       Mathf.RoundToInt(num4),
                                                       GetFormattedHalfLife(hl)),
                                         UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.DIE_TOOLTIP);
                }
            }
        }

        var num6 = disease.CalculateTemperatureHalfLife(temperature);
        if (num6 != float.PositiveInfinity) {
            if (num6 > 0f) {
                targetPanel.SetLabel("temperature",
                                     string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE,
                                                   GameUtil.GetFormattedTemperature(temperature),
                                                   GetFormattedHalfLife(num6)),
                                     UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.DIE_TOOLTIP);

                return;
            }

            targetPanel.SetLabel("temperature",
                                 string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE,
                                               GameUtil.GetFormattedTemperature(temperature),
                                               GetFormattedHalfLife(num6)),
                                 UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.GROW_TOOLTIP);
        }
    }

    private static void RefreshEnergyOverviewPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        if (targetEntity == null) return;

        if (targetEntity.GetComponent<ICircuitConnected>() != null || targetEntity.GetComponent<Wire>() != null) {
            var selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
            if (selectedTargetCircuitID == 65535)
                targetPanel.SetLabel("nocircuit",
                                     UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED,
                                     UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED);
            else {
                var joulesAvailableOnCircuit
                    = Game.Instance.circuitManager.GetJoulesAvailableOnCircuit(selectedTargetCircuitID);

                targetPanel.SetLabel("joulesAvailable",
                                     string.Format(UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES,
                                                   GameUtil.GetFormattedJoules(joulesAvailableOnCircuit)),
                                     UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES_TOOLTIP);

                var wattsGeneratedByCircuit
                    = Game.Instance.circuitManager.GetWattsGeneratedByCircuit(selectedTargetCircuitID);

                var potentialWattsGeneratedByCircuit
                    = Game.Instance.circuitManager.GetPotentialWattsGeneratedByCircuit(selectedTargetCircuitID);

                string arg;
                if (wattsGeneratedByCircuit == potentialWattsGeneratedByCircuit)
                    arg = GameUtil.GetFormattedWattage(wattsGeneratedByCircuit);
                else
                    arg = string.Format("{0} / {1}",
                                        GameUtil.GetFormattedWattage(wattsGeneratedByCircuit),
                                        GameUtil.GetFormattedWattage(potentialWattsGeneratedByCircuit));

                targetPanel.SetLabel("wattageGenerated",
                                     string.Format(UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED, arg),
                                     UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED_TOOLTIP);

                targetPanel.SetLabel("wattageConsumed",
                                     string.Format(UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED,
                                                   GameUtil.GetFormattedWattage(Game.Instance.circuitManager
                                                                                    .GetWattsUsedByCircuit(selectedTargetCircuitID))),
                                     UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED_TOOLTIP);

                targetPanel.SetLabel("potentialWattageConsumed",
                                     string.Format(UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED,
                                                   GameUtil.GetFormattedWattage(Game.Instance.circuitManager
                                                                                    .GetWattsNeededWhenActive(selectedTargetCircuitID))),
                                     UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED_TOOLTIP);

                targetPanel.SetLabel("maxSafeWattage",
                                     string.Format(UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE,
                                                   GameUtil.GetFormattedWattage(Game.Instance.circuitManager
                                                                                    .GetMaxSafeWattageForCircuit(selectedTargetCircuitID))),
                                     UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE_TOOLTIP);
            }
        }

        targetPanel.Commit();
    }

    private static void
        RefreshEnergyGeneratorPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        if (targetEntity == null) return;

        var selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
        if (selectedTargetCircuitID == 65535) {
            targetPanel.SetActive(false);
            return;
        }

        targetPanel.SetActive(true);
        var generatorsOnCircuit = Game.Instance.circuitManager.GetGeneratorsOnCircuit(selectedTargetCircuitID);
        if (generatorsOnCircuit.Count > 0)
            using (var enumerator = generatorsOnCircuit.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var generator = enumerator.Current;
                    if (generator != null && generator.GetComponent<Battery>() == null) {
                        string text;
                        if (generator.IsProducingPower())
                            text = string.Format("{0}: {1}",
                                                 generator.GetComponent<KSelectable>().entityName,
                                                 GameUtil.GetFormattedWattage(generator.WattageRating));
                        else
                            text = string.Format("{0}: {1} / {2}",
                                                 generator.GetComponent<KSelectable>().entityName,
                                                 GameUtil.GetFormattedWattage(0f),
                                                 GameUtil.GetFormattedWattage(generator.WattageRating));

                        text = generator.gameObject == targetEntity ? "<b>" + text + "</b>" : text;
                        targetPanel.SetLabel(generator.gameObject.GetInstanceID().ToString(), text, "");
                    }
                }

                goto IL_151;
            }

        targetPanel.SetLabel("nogenerators", UI.DETAILTABS.ENERGYGENERATOR.NOGENERATORS, "");
        IL_151:
        targetPanel.Commit();
    }

    private static void RefreshEnergyConsumerPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        // AdditionalDetailsPanel.<>c__DisplayClass27_0 CS$<>8__locals1;
        // CS$<>8__locals1.targetEntity = targetEntity;
        // CS$<>8__locals1.targetPanel  = targetPanel;
        // if (CS$<>8__locals1.targetEntity == null)
        // {
        //     return;
        // }
        //
        // ushort selectedTargetCircuitID
        //     = AdditionalDetailsPanel.GetSelectedTargetCircuitID(CS$ <>8__locals1.targetEntity);
        // if (selectedTargetCircuitID == 65535) {
        //     CS$<>8__locals1.targetPanel.SetActive(false);
        //     return;
        // }
        //
        // CS$<>8__locals1.targetPanel.SetActive(true);
        // var consumersOnCircuit    = Game.Instance.circuitManager.GetConsumersOnCircuit(selectedTargetCircuitID);
        // var transformersOnCircuit = Game.Instance.circuitManager.GetTransformersOnCircuit(selectedTargetCircuitID);
        // if (consumersOnCircuit.Count > 0 || transformersOnCircuit.Count > 0) {
        //     foreach (var consumer in consumersOnCircuit) {
        //         AdditionalDetailsPanel.<(RefreshEnergyConsumerPanel > g__AddConsumerInfo) |
        //             27_0(consumer, ref CS$ <>8__locals1);
        //     }
        //
        //     using (var enumerator2 = transformersOnCircuit.GetEnumerator()) {
        //         while (enumerator2.MoveNext()) {
        //             var consumer2 = enumerator2.Current;
        //             AdditionalDetailsPanel.<(RefreshEnergyConsumerPanel > g__AddConsumerInfo) |
        //                 27_0(consumer2, ref CS$ <>8__locals1);
        //         }
        //
        //         goto IL_FD;
        //     }
        // }
        //
        // CS$<>8__locals1.targetPanel.SetLabel("noconsumers", UI.DETAILTABS.ENERGYGENERATOR.NOCONSUMERS, "");
        // IL_FD:
        // CS$<>8__locals1.targetPanel.Commit();
    }

    private static void
        RefreshEnergyBatteriesPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        if (targetEntity == null) return;

        var selectedTargetCircuitID = GetSelectedTargetCircuitID(targetEntity);
        if (selectedTargetCircuitID == 65535) {
            targetPanel.SetActive(false);
            return;
        }

        targetPanel.SetActive(true);
        var batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(selectedTargetCircuitID);
        if (batteriesOnCircuit.Count > 0)
            using (var enumerator = batteriesOnCircuit.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var battery = enumerator.Current;
                    if (battery != null) {
                        var text = string.Format("{0}: {1}",
                                                 battery.GetComponent<KSelectable>().entityName,
                                                 GameUtil.GetFormattedJoules(battery.JoulesAvailable));

                        text = battery.gameObject == targetEntity ? "<b>" + text + "</b>" : text;
                        targetPanel.SetLabel(battery.gameObject.GetInstanceID().ToString(), text, "");
                    }
                }

                goto IL_103;
            }

        targetPanel.SetLabel("nobatteries", UI.DETAILTABS.ENERGYGENERATOR.NOBATTERIES, "");
        IL_103:
        targetPanel.Commit();
    }

    private static ushort GetSelectedTargetCircuitID(GameObject targetEntity) {
        var circuitManager = Game.Instance.circuitManager;
        var component      = targetEntity.GetComponent<ICircuitConnected>();
        var result         = ushort.MaxValue;
        if (component != null)
            result = Game.Instance.circuitManager.GetCircuitID(component);
        else if (targetEntity.GetComponent<Wire>() != null) {
            var cell = Grid.PosToCell(targetEntity.transform.GetPosition());
            result = Game.Instance.circuitManager.GetCircuitID(cell);
        }

        return result;
    }

    // [CompilerGenerated]
    // internal static void<RefreshEnergyConsumerPanel>g__AddConsumerInfo|27_0<>
    // private c__DisplayClass27_0 A_1)
    // {
    //     var kmonoBehaviour = consumer as KMonoBehaviour;
    //     if (kmonoBehaviour != null) {
    //         float  wattsUsed             = consumer.WattsUsed;
    //         float  wattsNeededWhenActive = consumer.WattsNeededWhenActive;
    //         string arg;
    //         if (wattsUsed == wattsNeededWhenActive)
    //             arg = GameUtil.GetFormattedWattage(wattsUsed);
    //         else
    //             arg = string.Format("{0} / {1}",
    //                                 GameUtil.GetFormattedWattage(wattsUsed),
    //                                 GameUtil.GetFormattedWattage(wattsNeededWhenActive));
    //
    //         var text = string.Format("{0}: {1}", consumer.Name, arg);
    //         text = kmonoBehaviour.gameObject == A_1.targetEntity ? "<b>" + text + "</b>" : text;
    //         A_1.targetPanel.SetLabel(kmonoBehaviour.gameObject.GetInstanceID().ToString(), text, "");
    //     }
    // }
}