using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E2D RID: 7725
[AddComponentMenu("KMonoBehaviour/scripts/MinionVitalsPanel")]
public class MinionVitalsPanel : CollapsibleDetailContentPanel
{
	// Token: 0x0600A1BC RID: 41404 RVA: 0x003D9250 File Offset: 0x003D7450
	public void Init()
	{
		this.AddAmountLine(Db.Get().Amounts.HitPoints, null);
		this.AddAmountLine(Db.Get().Amounts.BionicInternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.BionicOil, null);
		this.AddAmountLine(Db.Get().Amounts.BionicGunk, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Happiness, null);
		this.AddAmountLine(Db.Get().Amounts.Wildness, null);
		this.AddAmountLine(Db.Get().Amounts.Incubation, null);
		this.AddAmountLine(Db.Get().Amounts.Viability, null);
		this.AddAmountLine(Db.Get().Amounts.PowerCharge, null);
		this.AddAmountLine(Db.Get().Amounts.Fertility, null);
		this.AddAmountLine(Db.Get().Amounts.Beckoning, null);
		this.AddAmountLine(Db.Get().Amounts.Age, null);
		this.AddAmountLine(Db.Get().Amounts.Stress, null);
		this.AddAttributeLine(Db.Get().Attributes.QualityOfLife, null);
		this.AddAmountLine(Db.Get().Amounts.Bladder, null);
		this.AddAmountLine(Db.Get().Amounts.Breath, null);
		this.AddAmountLine(Db.Get().Amounts.BionicOxygenTank, null);
		this.AddAmountLine(Db.Get().Amounts.Stamina, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Metabolism, null);
		this.AddAmountLine(Db.Get().Amounts.Calories, null);
		this.AddAmountLine(Db.Get().Amounts.ScaleGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.MilkProduction, null);
		this.AddAmountLine(Db.Get().Amounts.ElementGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.Temperature, null);
		this.AddAmountLine(Db.Get().Amounts.CritterTemperature, null);
		this.AddAmountLine(Db.Get().Amounts.Decor, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalChemicalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBioBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalElectroBank, null);
		if (DlcManager.FeatureRadiationEnabled())
		{
			this.AddAmountLine(Db.Get().Amounts.RadiationBalance, null);
		}
		this.AddCheckboxLine(Db.Get().Amounts.AirPressure, this.conditionsContainerNormal, (GameObject go) => this.GetAirPressureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().pressure_sensitive)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_pressure(go), (GameObject go) => this.GetAirPressureTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetAtmosphereLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().safe_atmospheres.Count > 0)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_atmosphere(go), (GameObject go) => this.GetAtmosphereTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Temperature, this.conditionsContainerNormal, (GameObject go) => this.GetInternalTemperatureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<TemperatureVulnerable>() != null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_temperature(go), (GameObject go) => this.GetInternalTemperatureTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Fertilization, this.conditionsContainerAdditional, (GameObject go) => this.GetFertilizationLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<ReceptacleMonitor>() == null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			}
			if (go.GetComponent<ReceptacleMonitor>().Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
		}, (GameObject go) => this.check_fertilizer(go), (GameObject go) => this.GetFertilizationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Irrigation, this.conditionsContainerAdditional, (GameObject go) => this.GetIrrigationLabel(go), delegate(GameObject go)
		{
			ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
			if (!(component != null) || !component.Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
		}, (GameObject go) => this.check_irrigation(go), (GameObject go) => this.GetIrrigationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Illumination, this.conditionsContainerNormal, (GameObject go) => this.GetIlluminationLabel(go), (GameObject go) => MinionVitalsPanel.CheckboxLineDisplayType.Normal, (GameObject go) => this.check_illumination(go), (GameObject go) => this.GetIlluminationTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetRadiationLabel(go), delegate(GameObject go)
		{
			AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
			if (attributeInstance != null && attributeInstance.GetTotalValue() > 0f)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_radiation(go), (GameObject go) => this.GetRadiationTooltip(go));
	}

	// Token: 0x0600A1BD RID: 41405 RVA: 0x001090D3 File Offset: 0x001072D3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Init();
	}

	// Token: 0x0600A1BE RID: 41406 RVA: 0x001090E1 File Offset: 0x001072E1
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x0600A1BF RID: 41407 RVA: 0x001090F5 File Offset: 0x001072F5
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600A1C0 RID: 41408 RVA: 0x003D9784 File Offset: 0x003D7984
	private void AddAmountLine(Amount amount, Func<AmountInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(amount.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AmountLine item = default(MinionVitalsPanel.AmountLine);
		item.amount = amount;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		item.imageToggle = gameObject.GetComponentInChildren<ValueTrendImageToggle>();
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AmountInstance, string>(amount.GetTooltip));
		this.amountsLines.Add(item);
	}

	// Token: 0x0600A1C1 RID: 41409 RVA: 0x003D983C File Offset: 0x003D7A3C
	private void AddAttributeLine(Klei.AI.Attribute attribute, Func<AttributeInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(attribute.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AttributeLine item = default(MinionVitalsPanel.AttributeLine);
		item.attribute = attribute;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		gameObject.GetComponentInChildren<ValueTrendImageToggle>().gameObject.SetActive(false);
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AttributeInstance, string>(attribute.GetTooltip));
		this.attributesLines.Add(item);
	}

	// Token: 0x0600A1C2 RID: 41410 RVA: 0x003D98F8 File Offset: 0x003D7AF8
	private void AddCheckboxLine(Amount amount, Transform parentContainer, Func<GameObject, string> label_text_func, Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition, Func<GameObject, bool> checkbox_value_func, Func<GameObject, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.CheckboxLinePrefab, this.Content.gameObject, false);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.CheckboxLine checkboxLine = default(MinionVitalsPanel.CheckboxLine);
		checkboxLine.go = gameObject;
		checkboxLine.parentContainer = parentContainer;
		checkboxLine.amount = amount;
		checkboxLine.locText = (component.GetReference("Label") as LocText);
		checkboxLine.get_value = checkbox_value_func;
		checkboxLine.display_condition = display_condition;
		checkboxLine.label_text_func = label_text_func;
		checkboxLine.go.name = "Checkbox_";
		if (amount != null)
		{
			GameObject go = checkboxLine.go;
			go.name += amount.Name;
		}
		else
		{
			GameObject go2 = checkboxLine.go;
			go2.name += "Unnamed";
		}
		if (tooltip_func != null)
		{
			checkboxLine.tooltip = tooltip_func;
			ToolTip tt = checkboxLine.go.GetComponent<ToolTip>();
			tt.refreshWhileHovering = true;
			tt.OnToolTip = delegate()
			{
				tt.ClearMultiStringTooltip();
				tt.AddMultiStringTooltip(tooltip_func(this.lastSelectedEntity), null);
				return "";
			};
		}
		this.checkboxLines.Add(checkboxLine);
	}

	// Token: 0x0600A1C3 RID: 41411 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void ShouldShowVitalsPanel(GameObject selectedEntity)
	{
	}

	// Token: 0x0600A1C4 RID: 41412 RVA: 0x003D9A40 File Offset: 0x003D7C40
	public void Refresh(GameObject selectedEntity)
	{
		if (selectedEntity == null)
		{
			return;
		}
		if (selectedEntity.gameObject == null)
		{
			return;
		}
		this.lastSelectedEntity = selectedEntity;
		WiltCondition component = selectedEntity.GetComponent<WiltCondition>();
		MinionIdentity component2 = selectedEntity.GetComponent<MinionIdentity>();
		CreatureBrain component3 = selectedEntity.GetComponent<CreatureBrain>();
		IncubationMonitor.Instance smi = selectedEntity.GetSMI<IncubationMonitor.Instance>();
		object[] array = new object[]
		{
			component,
			component2,
			component3,
			smi
		};
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			base.SetActive(false);
			return;
		}
		base.SetActive(true);
		base.SetTitle((component == null) ? UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION : UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		Amounts amounts = selectedEntity.GetAmounts();
		Attributes attributes = selectedEntity.GetAttributes();
		if (amounts == null || attributes == null)
		{
			return;
		}
		if (component == null)
		{
			this.conditionsContainerNormal.gameObject.SetActive(false);
			this.conditionsContainerAdditional.gameObject.SetActive(false);
			foreach (MinionVitalsPanel.AmountLine amountLine in this.amountsLines)
			{
				bool flag2 = amountLine.TryUpdate(amounts);
				if (amountLine.go.activeSelf != flag2)
				{
					amountLine.go.SetActive(flag2);
				}
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine in this.attributesLines)
			{
				bool flag3 = attributeLine.TryUpdate(attributes);
				if (attributeLine.go.activeSelf != flag3)
				{
					attributeLine.go.SetActive(flag3);
				}
			}
		}
		bool flag4 = false;
		for (int j = 0; j < this.checkboxLines.Count; j++)
		{
			MinionVitalsPanel.CheckboxLine checkboxLine = this.checkboxLines[j];
			MinionVitalsPanel.CheckboxLineDisplayType checkboxLineDisplayType = MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			if (this.checkboxLines[j].amount != null)
			{
				for (int k = 0; k < amounts.Count; k++)
				{
					AmountInstance amountInstance = amounts[k];
					if (checkboxLine.amount == amountInstance.amount)
					{
						checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
						break;
					}
				}
			}
			else
			{
				checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
			}
			if (checkboxLineDisplayType != MinionVitalsPanel.CheckboxLineDisplayType.Hidden)
			{
				checkboxLine.locText.SetText(checkboxLine.label_text_func(selectedEntity.gameObject));
				if (!checkboxLine.go.activeSelf)
				{
					checkboxLine.go.SetActive(true);
				}
				GameObject gameObject = checkboxLine.go.GetComponent<HierarchyReferences>().GetReference("Check").gameObject;
				gameObject.SetActive(checkboxLine.get_value(selectedEntity.gameObject));
				if (checkboxLine.go.transform.parent != checkboxLine.parentContainer)
				{
					checkboxLine.go.transform.SetParent(checkboxLine.parentContainer);
					checkboxLine.go.transform.localScale = Vector3.one;
				}
				if (checkboxLine.parentContainer == this.conditionsContainerAdditional)
				{
					flag4 = true;
				}
				if (checkboxLineDisplayType == MinionVitalsPanel.CheckboxLineDisplayType.Normal)
				{
					if (checkboxLine.get_value(selectedEntity.gameObject))
					{
						checkboxLine.locText.color = Color.black;
						gameObject.transform.parent.GetComponent<Image>().color = Color.black;
					}
					else
					{
						Color color = new Color(0.99215686f, 0f, 0.101960786f);
						checkboxLine.locText.color = color;
						gameObject.transform.parent.GetComponent<Image>().color = color;
					}
				}
				else
				{
					checkboxLine.locText.color = Color.grey;
					gameObject.transform.parent.GetComponent<Image>().color = Color.grey;
				}
			}
			else if (checkboxLine.go.activeSelf)
			{
				checkboxLine.go.SetActive(false);
			}
		}
		if (component != null)
		{
			IManageGrowingStates manageGrowingStates = component.GetComponent<IManageGrowingStates>();
			manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : component.GetSMI<IManageGrowingStates>());
			bool flag5 = component.HasTag(GameTags.Decoration);
			this.conditionsContainerNormal.gameObject.SetActive(true);
			this.conditionsContainerAdditional.gameObject.SetActive(!flag5);
			if (manageGrowingStates == null)
			{
				float num = 1f;
				LocText reference = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference.text = "";
				reference.text = (flag5 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_DECOR.BASE, Array.Empty<object>()) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 0.25f * 100f)));
				reference.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.TOOLTIP, Array.Empty<object>()));
				LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				ReceptacleMonitor component4 = selectedEntity.GetComponent<ReceptacleMonitor>();
				reference2.color = ((component4 == null || component4.Replanted) ? Color.black : Color.grey);
				reference2.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 100f));
				reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.TOOLTIP, Array.Empty<object>()));
			}
			else
			{
				LocText reference3 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference3.text = "";
				reference3.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime(), "F1", false));
				reference3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.TOOLTIP, GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime(), "F1", false)));
				LocText reference4 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference4.color = (selectedEntity.GetComponent<ReceptacleMonitor>().Replanted ? Color.black : Color.grey);
				reference4.text = "";
				reference4.text = (flag4 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.DOMESTIC.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)));
				reference4.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.TOOLTIP, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)));
			}
			foreach (MinionVitalsPanel.AmountLine amountLine2 in this.amountsLines)
			{
				amountLine2.go.SetActive(false);
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine2 in this.attributesLines)
			{
				attributeLine2.go.SetActive(false);
			}
		}
	}

	// Token: 0x0600A1C5 RID: 41413 RVA: 0x003DA184 File Offset: 0x003D8384
	private string GetAirPressureTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_PRESSURE.text.Replace("{pressure}", GameUtil.GetFormattedMass(component.GetExternalPressure(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x0600A1C6 RID: 41414 RVA: 0x003DA1D0 File Offset: 0x003D83D0
	private string GetInternalTemperatureTooltip(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_TEMPERATURE.text.Replace("{temperature}", GameUtil.GetFormattedTemperature(component.InternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x0600A1C7 RID: 41415 RVA: 0x003DA218 File Offset: 0x003D8418
	private string GetFertilizationTooltip(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_FERTILIZER.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x0600A1C8 RID: 41416 RVA: 0x003DA25C File Offset: 0x003D845C
	private string GetIrrigationTooltip(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_IRRIGATION.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x0600A1C9 RID: 41417 RVA: 0x003DA2A0 File Offset: 0x003D84A0
	private string GetIlluminationTooltip(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		if (illuminationTracker == null)
		{
			return "";
		}
		return illuminationTracker.GetIlluminationUITooltip();
	}

	// Token: 0x0600A1CA RID: 41418 RVA: 0x003DA2D0 File Offset: 0x003D84D0
	private string GetRadiationTooltip(GameObject go)
	{
		int num = Grid.PosToCell(go);
		float rads = Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f;
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		MutantPlant component = go.GetComponent<MutantPlant>();
		bool flag = component != null && component.IsOriginal;
		string text;
		if (attributeInstance.GetTotalValue() == 0f)
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION_NO_MIN.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		else
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		if (flag)
		{
			text += UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP;
		}
		return text;
	}

	// Token: 0x0600A1CB RID: 41419 RVA: 0x003DA3D8 File Offset: 0x003D85D8
	private string GetReceptacleTooltip(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return "";
		}
		if (component.HasOperationalReceptacle())
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL;
	}

	// Token: 0x0600A1CC RID: 41420 RVA: 0x003DA418 File Offset: 0x003D8618
	private string GetAtmosphereTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component != null && component.currentAtmoElement != null)
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE.text.Replace("{element}", component.currentAtmoElement.name);
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE;
	}

	// Token: 0x0600A1CD RID: 41421 RVA: 0x003DA468 File Offset: 0x003D8668
	private string GetAirPressureLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.AirPressure.Name,
			"\n    • ",
			GameUtil.GetFormattedMass(component.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, false, "{0:0.#}"),
			" - ",
			GameUtil.GetFormattedMass(component.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}")
		});
	}

	// Token: 0x0600A1CE RID: 41422 RVA: 0x003DA4DC File Offset: 0x003D86DC
	private string GetInternalTemperatureLabel(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.Temperature.Name,
			"\n    • ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false),
			" - ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)
		});
	}

	// Token: 0x0600A1CF RID: 41423 RVA: 0x003DA548 File Offset: 0x003D8748
	private string GetFertilizationLabel(GameObject go)
	{
		StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GenericInstance smi = go.GetSMI<FertilizationMonitor.Instance>();
		string text = Db.Get().Amounts.Fertilization.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				" ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x0600A1D0 RID: 41424 RVA: 0x003DA600 File Offset: 0x003D8800
	private string GetIrrigationLabel(GameObject go)
	{
		StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GenericInstance smi = go.GetSMI<IrrigationMonitor.Instance>();
		string text = Db.Get().Amounts.Irrigation.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				": ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x0600A1D1 RID: 41425 RVA: 0x003DA6B8 File Offset: 0x003D88B8
	private string GetIlluminationLabel(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		return illuminationTracker.GetIlluminationUILabel();
	}

	// Token: 0x0600A1D2 RID: 41426 RVA: 0x003DA6DC File Offset: 0x003D88DC
	private string GetAtmosphereLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		string text = UI.VITALSSCREEN.ATMOSPHERE_CONDITION;
		foreach (Element element in component.safe_atmospheres)
		{
			text = text + "\n    • " + element.name;
		}
		return text;
	}

	// Token: 0x0600A1D3 RID: 41427 RVA: 0x003DA74C File Offset: 0x003D894C
	private string GetRadiationLabel(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		if (attributeInstance.GetTotalValue() == 0f)
		{
			return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_NO_MIN_RADIATION_FMT.Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION_FMT.Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
	}

	// Token: 0x0600A1D4 RID: 41428 RVA: 0x003DA800 File Offset: 0x003D8A00
	private bool check_pressure(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.ExternalPressureState == PressureVulnerable.PressureState.Normal;
	}

	// Token: 0x0600A1D5 RID: 41429 RVA: 0x003DA828 File Offset: 0x003D8A28
	private bool check_temperature(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return !(component != null) || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
	}

	// Token: 0x0600A1D6 RID: 41430 RVA: 0x003DA850 File Offset: 0x003D8A50
	private bool check_irrigation(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		return smi == null || (!smi.IsInsideState(smi.sm.replanted.starved) && !smi.IsInsideState(smi.sm.wild));
	}

	// Token: 0x0600A1D7 RID: 41431 RVA: 0x003DA898 File Offset: 0x003D8A98
	private bool check_illumination(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		return illuminationTracker == null || illuminationTracker.ShouldIlluminationUICheckboxBeChecked();
	}

	// Token: 0x0600A1D8 RID: 41432 RVA: 0x003DA8C4 File Offset: 0x003D8AC4
	private bool check_radiation(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		if (attributeInstance != null && attributeInstance.GetTotalValue() != 0f)
		{
			int num = Grid.PosToCell(go);
			return (Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f) >= attributeInstance.GetTotalValue();
		}
		return true;
	}

	// Token: 0x0600A1D9 RID: 41433 RVA: 0x003DA92C File Offset: 0x003D8B2C
	private bool check_receptacle(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		return !(component == null) && component.HasOperationalReceptacle();
	}

	// Token: 0x0600A1DA RID: 41434 RVA: 0x003DA954 File Offset: 0x003D8B54
	private bool check_fertilizer(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		return smi == null || smi.sm.hasCorrectFertilizer.Get(smi);
	}

	// Token: 0x0600A1DB RID: 41435 RVA: 0x003DA980 File Offset: 0x003D8B80
	private bool check_atmosphere(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.testAreaElementSafe;
	}

	// Token: 0x04007E26 RID: 32294
	public GameObject LineItemPrefab;

	// Token: 0x04007E27 RID: 32295
	public GameObject CheckboxLinePrefab;

	// Token: 0x04007E28 RID: 32296
	private GameObject lastSelectedEntity;

	// Token: 0x04007E29 RID: 32297
	public List<MinionVitalsPanel.AmountLine> amountsLines = new List<MinionVitalsPanel.AmountLine>();

	// Token: 0x04007E2A RID: 32298
	public List<MinionVitalsPanel.AttributeLine> attributesLines = new List<MinionVitalsPanel.AttributeLine>();

	// Token: 0x04007E2B RID: 32299
	public List<MinionVitalsPanel.CheckboxLine> checkboxLines = new List<MinionVitalsPanel.CheckboxLine>();

	// Token: 0x04007E2C RID: 32300
	public Transform conditionsContainerNormal;

	// Token: 0x04007E2D RID: 32301
	public Transform conditionsContainerAdditional;

	// Token: 0x02001E2E RID: 7726
	[DebuggerDisplay("{amount.Name}")]
	public struct AmountLine
	{
		// Token: 0x0600A1F2 RID: 41458 RVA: 0x003DA9A8 File Offset: 0x003D8BA8
		public bool TryUpdate(Amounts amounts)
		{
			foreach (AmountInstance amountInstance in amounts)
			{
				if (this.amount == amountInstance.amount && !amountInstance.hide)
				{
					this.locText.SetText(this.amount.GetDescription(amountInstance));
					this.toolTip.toolTip = this.toolTipFunc(amountInstance);
					this.imageToggle.SetValue(amountInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04007E2E RID: 32302
		public Amount amount;

		// Token: 0x04007E2F RID: 32303
		public GameObject go;

		// Token: 0x04007E30 RID: 32304
		public ValueTrendImageToggle imageToggle;

		// Token: 0x04007E31 RID: 32305
		public LocText locText;

		// Token: 0x04007E32 RID: 32306
		public ToolTip toolTip;

		// Token: 0x04007E33 RID: 32307
		public Func<AmountInstance, string> toolTipFunc;
	}

	// Token: 0x02001E2F RID: 7727
	[DebuggerDisplay("{attribute.Name}")]
	public struct AttributeLine
	{
		// Token: 0x0600A1F3 RID: 41459 RVA: 0x003DAA40 File Offset: 0x003D8C40
		public bool TryUpdate(Attributes attributes)
		{
			foreach (AttributeInstance attributeInstance in attributes)
			{
				if (this.attribute == attributeInstance.modifier && !attributeInstance.hide)
				{
					this.locText.SetText(this.attribute.GetDescription(attributeInstance));
					this.toolTip.toolTip = this.toolTipFunc(attributeInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04007E34 RID: 32308
		public Klei.AI.Attribute attribute;

		// Token: 0x04007E35 RID: 32309
		public GameObject go;

		// Token: 0x04007E36 RID: 32310
		public LocText locText;

		// Token: 0x04007E37 RID: 32311
		public ToolTip toolTip;

		// Token: 0x04007E38 RID: 32312
		public Func<AttributeInstance, string> toolTipFunc;
	}

	// Token: 0x02001E30 RID: 7728
	public struct CheckboxLine
	{
		// Token: 0x04007E39 RID: 32313
		public Amount amount;

		// Token: 0x04007E3A RID: 32314
		public GameObject go;

		// Token: 0x04007E3B RID: 32315
		public LocText locText;

		// Token: 0x04007E3C RID: 32316
		public Func<GameObject, string> tooltip;

		// Token: 0x04007E3D RID: 32317
		public Func<GameObject, bool> get_value;

		// Token: 0x04007E3E RID: 32318
		public Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition;

		// Token: 0x04007E3F RID: 32319
		public Func<GameObject, string> label_text_func;

		// Token: 0x04007E40 RID: 32320
		public Transform parentContainer;
	}

	// Token: 0x02001E31 RID: 7729
	public enum CheckboxLineDisplayType
	{
		// Token: 0x04007E42 RID: 32322
		Normal,
		// Token: 0x04007E43 RID: 32323
		Diminished,
		// Token: 0x04007E44 RID: 32324
		Hidden
	}
}
