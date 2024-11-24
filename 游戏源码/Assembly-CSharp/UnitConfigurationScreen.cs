using System;
using STRINGS;
using UnityEngine;

// Token: 0x02002044 RID: 8260
[Serializable]
public class UnitConfigurationScreen
{
	// Token: 0x0600AFD6 RID: 45014 RVA: 0x004220D0 File Offset: 0x004202D0
	public void Init()
	{
		this.celsiusToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.celsiusToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS_TOOLTIP;
		this.celsiusToggle.GetComponentInChildren<KButton>().onClick += this.OnCelsiusClicked;
		this.celsiusToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS;
		this.kelvinToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.kelvinToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN_TOOLTIP;
		this.kelvinToggle.GetComponentInChildren<KButton>().onClick += this.OnKelvinClicked;
		this.kelvinToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN;
		this.fahrenheitToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.fahrenheitToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT_TOOLTIP;
		this.fahrenheitToggle.GetComponentInChildren<KButton>().onClick += this.OnFahrenheitClicked;
		this.fahrenheitToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT;
		this.DisplayCurrentUnit();
	}

	// Token: 0x0600AFD7 RID: 45015 RVA: 0x0042221C File Offset: 0x0042041C
	private void DisplayCurrentUnit()
	{
		GameUtil.TemperatureUnit @int = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey, 0);
		if (@int == GameUtil.TemperatureUnit.Celsius)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			return;
		}
		if (@int != GameUtil.TemperatureUnit.Kelvin)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			return;
		}
		this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
		this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
		this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
	}

	// Token: 0x0600AFD8 RID: 45016 RVA: 0x00422364 File Offset: 0x00420564
	private void OnCelsiusClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Celsius;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Celsius);
		}
	}

	// Token: 0x0600AFD9 RID: 45017 RVA: 0x004223BC File Offset: 0x004205BC
	private void OnKelvinClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Kelvin;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Kelvin);
		}
	}

	// Token: 0x0600AFDA RID: 45018 RVA: 0x00422414 File Offset: 0x00420614
	private void OnFahrenheitClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Fahrenheit;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Fahrenheit);
		}
	}

	// Token: 0x04008A8D RID: 35469
	[SerializeField]
	private GameObject toggleUnitPrefab;

	// Token: 0x04008A8E RID: 35470
	[SerializeField]
	private GameObject toggleGroup;

	// Token: 0x04008A8F RID: 35471
	private GameObject celsiusToggle;

	// Token: 0x04008A90 RID: 35472
	private GameObject kelvinToggle;

	// Token: 0x04008A91 RID: 35473
	private GameObject fahrenheitToggle;

	// Token: 0x04008A92 RID: 35474
	public static readonly string TemperatureUnitKey = "TemperatureUnit";

	// Token: 0x04008A93 RID: 35475
	public static readonly string MassUnitKey = "MassUnit";
}
