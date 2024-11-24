using System;
using System.Collections.Generic;
using System.Threading;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001344 RID: 4932
public static class GameUtil
{
	// Token: 0x06006508 RID: 25864 RVA: 0x002C86B8 File Offset: 0x002C68B8
	public static string GetTemperatureUnitSuffix()
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		string result;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
			{
				result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			}
			else
			{
				result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			}
		}
		else
		{
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
		}
		return result;
	}

	// Token: 0x06006509 RID: 25865 RVA: 0x000E1F0A File Offset: 0x000E010A
	private static string AddTemperatureUnitSuffix(string text)
	{
		return text + GameUtil.GetTemperatureUnitSuffix();
	}

	// Token: 0x0600650A RID: 25866 RVA: 0x000E1F17 File Offset: 0x000E0117
	public static float GetTemperatureConvertedFromKelvin(float temperature, GameUtil.TemperatureUnit targetUnit)
	{
		if (targetUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature - 273.15f;
		}
		if (targetUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return temperature * 1.8f - 459.67f;
	}

	// Token: 0x0600650B RID: 25867 RVA: 0x002C86FC File Offset: 0x002C68FC
	public static float GetConvertedTemperature(float temperature, bool roundOutput = false)
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
			{
				if (!roundOutput)
				{
					return temperature;
				}
				return Mathf.Round(temperature);
			}
			else
			{
				float num = temperature * 1.8f - 459.67f;
				if (!roundOutput)
				{
					return num;
				}
				return Mathf.Round(num);
			}
		}
		else
		{
			float num = temperature - 273.15f;
			if (!roundOutput)
			{
				return num;
			}
			return Mathf.Round(num);
		}
	}

	// Token: 0x0600650C RID: 25868 RVA: 0x000E1F39 File Offset: 0x000E0139
	public static float GetTemperatureConvertedToKelvin(float temperature, GameUtil.TemperatureUnit fromUnit)
	{
		if (fromUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature + 273.15f;
		}
		if (fromUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return (temperature + 459.67f) * 5f / 9f;
	}

	// Token: 0x0600650D RID: 25869 RVA: 0x002C8758 File Offset: 0x002C6958
	public static float GetTemperatureConvertedToKelvin(float temperature)
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature + 273.15f;
		}
		if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return (temperature + 459.67f) * 5f / 9f;
	}

	// Token: 0x0600650E RID: 25870 RVA: 0x002C8794 File Offset: 0x002C6994
	private static float GetConvertedTemperatureDelta(float kelvin_delta)
	{
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			return kelvin_delta;
		case GameUtil.TemperatureUnit.Fahrenheit:
			return kelvin_delta * 1.8f;
		case GameUtil.TemperatureUnit.Kelvin:
			return kelvin_delta;
		default:
			return kelvin_delta;
		}
	}

	// Token: 0x0600650F RID: 25871 RVA: 0x000E1F61 File Offset: 0x000E0161
	public static float ApplyTimeSlice(float val, GameUtil.TimeSlice timeSlice)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return val * 600f;
		}
		return val;
	}

	// Token: 0x06006510 RID: 25872 RVA: 0x000E1F70 File Offset: 0x000E0170
	public static float ApplyTimeSlice(int val, GameUtil.TimeSlice timeSlice)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return (float)val * 600f;
		}
		return (float)val;
	}

	// Token: 0x06006511 RID: 25873 RVA: 0x000E1F81 File Offset: 0x000E0181
	public static string AddTimeSliceText(string text, GameUtil.TimeSlice timeSlice)
	{
		switch (timeSlice)
		{
		case GameUtil.TimeSlice.PerSecond:
			return text + UI.UNITSUFFIXES.PERSECOND;
		case GameUtil.TimeSlice.PerCycle:
			return text + UI.UNITSUFFIXES.PERCYCLE;
		}
		return text;
	}

	// Token: 0x06006512 RID: 25874 RVA: 0x000E1FBE File Offset: 0x000E01BE
	public static string AddPositiveSign(string text, bool positive)
	{
		if (positive)
		{
			return string.Format(UI.POSITIVE_FORMAT, text);
		}
		return text;
	}

	// Token: 0x06006513 RID: 25875 RVA: 0x000E1FD5 File Offset: 0x000E01D5
	public static float AttributeSkillToAlpha(AttributeInstance attributeInstance)
	{
		return Mathf.Min(attributeInstance.GetTotalValue() / 10f, 1f);
	}

	// Token: 0x06006514 RID: 25876 RVA: 0x000E1FED File Offset: 0x000E01ED
	public static float AttributeSkillToAlpha(float attributeSkill)
	{
		return Mathf.Min(attributeSkill / 10f, 1f);
	}

	// Token: 0x06006515 RID: 25877 RVA: 0x000E1FED File Offset: 0x000E01ED
	public static float AptitudeToAlpha(float aptitude)
	{
		return Mathf.Min(aptitude / 10f, 1f);
	}

	// Token: 0x06006516 RID: 25878 RVA: 0x000E2000 File Offset: 0x000E0200
	public static float GetThermalEnergy(PrimaryElement pe)
	{
		return pe.Temperature * pe.Mass * pe.Element.specificHeatCapacity;
	}

	// Token: 0x06006517 RID: 25879 RVA: 0x000E201B File Offset: 0x000E021B
	public static float CalculateTemperatureChange(float shc, float mass, float kilowatts)
	{
		return kilowatts / (shc * mass);
	}

	// Token: 0x06006518 RID: 25880 RVA: 0x002C87C8 File Offset: 0x002C69C8
	public static void DeltaThermalEnergy(PrimaryElement pe, float kilowatts, float targetTemperature)
	{
		float num = GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, kilowatts);
		float num2 = pe.Temperature + num;
		if (targetTemperature > pe.Temperature)
		{
			num2 = Mathf.Clamp(num2, pe.Temperature, targetTemperature);
		}
		else
		{
			num2 = Mathf.Clamp(num2, targetTemperature, pe.Temperature);
		}
		pe.Temperature = num2;
	}

	// Token: 0x06006519 RID: 25881 RVA: 0x002C8824 File Offset: 0x002C6A24
	public static BindingEntry ActionToBinding(global::Action action)
	{
		foreach (BindingEntry bindingEntry in GameInputMapping.KeyBindings)
		{
			if (bindingEntry.mAction == action)
			{
				return bindingEntry;
			}
		}
		throw new ArgumentException(action.ToString() + " is not bound in GameInputBindings");
	}

	// Token: 0x0600651A RID: 25882 RVA: 0x002C8874 File Offset: 0x002C6A74
	public static string GetIdentityDescriptor(GameObject go, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		if (go.GetComponent<MinionIdentity>())
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT_POSSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT_PLURAL;
			}
		}
		else if (go.GetComponent<CreatureBrain>())
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE_POSSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE_PLURAL;
			}
		}
		else
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.PLANT;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.PLANT_POSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.PLANT_PLURAL;
			}
		}
		return "";
	}

	// Token: 0x0600651B RID: 25883 RVA: 0x000E2022 File Offset: 0x000E0222
	public static float GetEnergyInPrimaryElement(PrimaryElement element)
	{
		return 0.001f * (element.Temperature * (element.Mass * 1000f * element.Element.specificHeatCapacity));
	}

	// Token: 0x0600651C RID: 25884 RVA: 0x002C8944 File Offset: 0x002C6B44
	public static float EnergyToTemperatureDelta(float kilojoules, PrimaryElement element)
	{
		global::Debug.Assert(element.Mass > 0f);
		float num = Mathf.Max(GameUtil.GetEnergyInPrimaryElement(element) - kilojoules, 1f);
		float temperature = element.Temperature;
		return num / (0.001f * (element.Mass * (element.Element.specificHeatCapacity * 1000f))) - temperature;
	}

	// Token: 0x0600651D RID: 25885 RVA: 0x000E2049 File Offset: 0x000E0249
	public static float CalculateEnergyDeltaForElement(PrimaryElement element, float startTemp, float endTemp)
	{
		return GameUtil.CalculateEnergyDeltaForElementChange(element.Mass, element.Element.specificHeatCapacity, startTemp, endTemp);
	}

	// Token: 0x0600651E RID: 25886 RVA: 0x000E2063 File Offset: 0x000E0263
	public static float CalculateEnergyDeltaForElementChange(float mass, float shc, float startTemp, float endTemp)
	{
		return (endTemp - startTemp) * mass * shc;
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x002C89A0 File Offset: 0x002C6BA0
	public static float GetFinalTemperature(float t1, float m1, float t2, float m2)
	{
		float num = m1 + m2;
		float num2 = (t1 * m1 + t2 * m2) / num;
		float num3 = Mathf.Min(t1, t2);
		float num4 = Mathf.Max(t1, t2);
		num2 = Mathf.Clamp(num2, num3, num4);
		if (float.IsNaN(num2) || float.IsInfinity(num2))
		{
			global::Debug.LogError(string.Format("Calculated an invalid temperature: t1={0}, m1={1}, t2={2}, m2={3}, min_temp={4}, max_temp={5}", new object[]
			{
				t1,
				m1,
				t2,
				m2,
				num3,
				num4
			}));
		}
		return num2;
	}

	// Token: 0x06006520 RID: 25888 RVA: 0x002C8A30 File Offset: 0x002C6C30
	public static void ForceConduction(PrimaryElement a, PrimaryElement b, float dt)
	{
		float num = a.Temperature * a.Element.specificHeatCapacity * a.Mass;
		float num2 = b.Temperature * b.Element.specificHeatCapacity * b.Mass;
		float num3 = Math.Min(a.Element.thermalConductivity, b.Element.thermalConductivity);
		float num4 = Math.Min(a.Mass, b.Mass);
		float num5 = (b.Temperature - a.Temperature) * (num3 * num4) * dt;
		float num6 = (num + num2) / (a.Element.specificHeatCapacity * a.Mass + b.Element.specificHeatCapacity * b.Mass);
		float val = Math.Abs((num6 - a.Temperature) * a.Element.specificHeatCapacity * a.Mass);
		float val2 = Math.Abs((num6 - b.Temperature) * b.Element.specificHeatCapacity * b.Mass);
		float num7 = Math.Min(val, val2);
		num5 = Math.Min(num5, num7);
		num5 = Math.Max(num5, -num7);
		a.Temperature = (num + num5) / a.Element.specificHeatCapacity / a.Mass;
		b.Temperature = (num2 - num5) / b.Element.specificHeatCapacity / b.Mass;
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x000E206C File Offset: 0x000E026C
	public static string FloatToString(float f, string format = null)
	{
		if (float.IsPositiveInfinity(f))
		{
			return UI.POS_INFINITY;
		}
		if (float.IsNegativeInfinity(f))
		{
			return UI.NEG_INFINITY;
		}
		return f.ToString(format);
	}

	// Token: 0x06006522 RID: 25890 RVA: 0x002C8B7C File Offset: 0x002C6D7C
	public static string GetFloatWithDecimalPoint(float f)
	{
		string format;
		if (f == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "#,##0.#";
		}
		else
		{
			format = "#,###.#";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x06006523 RID: 25891 RVA: 0x002C8BC4 File Offset: 0x002C6DC4
	public static string GetStandardFloat(float f)
	{
		string format;
		if (f == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "#,##0.#";
		}
		else if (Mathf.Abs(f) < 10f)
		{
			format = "#,###.#";
		}
		else
		{
			format = "#,###";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x06006524 RID: 25892 RVA: 0x002C8C20 File Offset: 0x002C6E20
	public static string GetStandardPercentageFloat(float f, bool allowHundredths = false)
	{
		string format;
		if (Mathf.Abs(f) == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 0.1f && allowHundredths)
		{
			format = "##0.##";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "##0.#";
		}
		else
		{
			format = "##0";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x06006525 RID: 25893 RVA: 0x002C8C84 File Offset: 0x002C6E84
	public static string GetUnitFormattedName(GameObject go, bool upperName = false)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null && Assets.IsTagCountable(component.PrefabTag))
		{
			PrimaryElement component2 = go.GetComponent<PrimaryElement>();
			return GameUtil.GetUnitFormattedName(go.GetProperName(), component2.Units, upperName);
		}
		if (!upperName)
		{
			return go.GetProperName();
		}
		return StringFormatter.ToUpper(go.GetProperName());
	}

	// Token: 0x06006526 RID: 25894 RVA: 0x000E209C File Offset: 0x000E029C
	public static string GetUnitFormattedName(string name, float count, bool upperName = false)
	{
		if (upperName)
		{
			name = name.ToUpper();
		}
		return StringFormatter.Replace(UI.NAME_WITH_UNITS, "{0}", name).Replace("{1}", string.Format("{0:0.##}", count));
	}

	// Token: 0x06006527 RID: 25895 RVA: 0x002C8CE0 File Offset: 0x002C6EE0
	public static string GetFormattedUnits(float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displaySuffix = true, string floatFormatOverride = "")
	{
		string str = (units == 1f) ? UI.UNITSUFFIXES.UNIT : UI.UNITSUFFIXES.UNITS;
		units = GameUtil.ApplyTimeSlice(units, timeSlice);
		string text = GameUtil.GetStandardFloat(units);
		if (!floatFormatOverride.IsNullOrWhiteSpace())
		{
			text = string.Format(floatFormatOverride, units);
		}
		if (displaySuffix)
		{
			text += str;
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x06006528 RID: 25896 RVA: 0x000E20D8 File Offset: 0x000E02D8
	public static string GetFormattedRocketRangePerCycle(float range, bool displaySuffix = true)
	{
		return range.ToString("N1") + (displaySuffix ? (" " + UI.CLUSTERMAP.TILES_PER_CYCLE) : "");
	}

	// Token: 0x06006529 RID: 25897 RVA: 0x000E2109 File Offset: 0x000E0309
	public static string GetFormattedRocketRange(int rangeInTiles, bool displaySuffix = true)
	{
		return rangeInTiles.ToString() + (displaySuffix ? (" " + UI.CLUSTERMAP.TILES) : "");
	}

	// Token: 0x0600652A RID: 25898 RVA: 0x000E2135 File Offset: 0x000E0335
	public static string ApplyBoldString(string source)
	{
		return "<b>" + source + "</b>";
	}

	// Token: 0x0600652B RID: 25899 RVA: 0x002C8D40 File Offset: 0x002C6F40
	public static float GetRoundedTemperatureInKelvin(float kelvin)
	{
		float result = 0f;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = GameUtil.GetTemperatureConvertedToKelvin(Mathf.Round(GameUtil.GetConvertedTemperature(Mathf.Round(kelvin), true)));
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = GameUtil.GetTemperatureConvertedToKelvin((float)Mathf.RoundToInt(GameUtil.GetTemperatureConvertedFromKelvin(kelvin, GameUtil.TemperatureUnit.Fahrenheit)), GameUtil.TemperatureUnit.Fahrenheit);
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = (float)Mathf.RoundToInt(kelvin);
			break;
		}
		return result;
	}

	// Token: 0x0600652C RID: 25900 RVA: 0x002C8DA8 File Offset: 0x002C6FA8
	public static string GetFormattedTemperature(float temp, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute, bool displayUnits = true, bool roundInDestinationFormat = false)
	{
		if (interpretation != GameUtil.TemperatureInterpretation.Absolute)
		{
			if (interpretation != GameUtil.TemperatureInterpretation.Relative)
			{
			}
			temp = GameUtil.GetConvertedTemperatureDelta(temp);
		}
		else
		{
			temp = GameUtil.GetConvertedTemperature(temp, roundInDestinationFormat);
		}
		temp = GameUtil.ApplyTimeSlice(temp, timeSlice);
		string text;
		if (Mathf.Abs(temp) < 0.1f)
		{
			text = GameUtil.FloatToString(temp, "##0.####");
		}
		else
		{
			text = GameUtil.FloatToString(temp, "##0.#");
		}
		if (displayUnits)
		{
			text = GameUtil.AddTemperatureUnitSuffix(text);
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x0600652D RID: 25901 RVA: 0x002C8E1C File Offset: 0x002C701C
	public static string GetFormattedCaloriesForItem(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
		return GameUtil.GetFormattedCalories((foodInfo != null) ? (foodInfo.CaloriesPerUnit * amount) : -1f, timeSlice, forceKcal);
	}

	// Token: 0x0600652E RID: 25902 RVA: 0x002C8E50 File Offset: 0x002C7050
	public static string GetFormattedCalories(float calories, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		string str = UI.UNITSUFFIXES.CALORIES.CALORIE;
		if (Mathf.Abs(calories) >= 1000f || forceKcal)
		{
			calories /= 1000f;
			str = UI.UNITSUFFIXES.CALORIES.KILOCALORIE;
		}
		calories = GameUtil.ApplyTimeSlice(calories, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardFloat(calories) + str, timeSlice);
	}

	// Token: 0x0600652F RID: 25903 RVA: 0x002C8EAC File Offset: 0x002C70AC
	public static string GetFormattedDirectPlantConsumptionValuePerCycle(Tag plantTag, float consumer_caloriesLossPerCaloriesPerKG, bool perCycle = true)
	{
		IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(Assets.GetPrefab(plantTag));
		if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
		{
			return "Error";
		}
		foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
		{
			if (plantConsumptionInstructions2.GetDietFoodType() == Diet.Info.FoodType.EatPlantDirectly)
			{
				return plantConsumptionInstructions2.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG);
			}
		}
		return "Error";
	}

	// Token: 0x06006530 RID: 25904 RVA: 0x002C8EFC File Offset: 0x002C70FC
	public static string GetFormattedPlantStorageConsumptionValuePerCycle(Tag plantTag, float consumer_caloriesLossPerCaloriesPerKG, bool perCycle = true)
	{
		IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(Assets.GetPrefab(plantTag));
		if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
		{
			return "Error";
		}
		foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
		{
			if (plantConsumptionInstructions2.GetDietFoodType() == Diet.Info.FoodType.EatPlantStorage)
			{
				return plantConsumptionInstructions2.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG);
			}
		}
		return "Error";
	}

	// Token: 0x06006531 RID: 25905 RVA: 0x002C8F4C File Offset: 0x002C714C
	public static IPlantConsumptionInstructions[] GetPlantConsumptionInstructions(GameObject prefab)
	{
		IPlantConsumptionInstructions[] components = prefab.GetComponents<IPlantConsumptionInstructions>();
		List<IPlantConsumptionInstructions> allSMI = prefab.GetAllSMI<IPlantConsumptionInstructions>();
		List<IPlantConsumptionInstructions> list = new List<IPlantConsumptionInstructions>();
		if (components != null)
		{
			list.AddRange(components);
		}
		if (allSMI != null)
		{
			list.AddRange(allSMI);
		}
		return list.ToArray();
	}

	// Token: 0x06006532 RID: 25906 RVA: 0x000E2147 File Offset: 0x000E0347
	public static string GetFormattedPlantGrowth(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardPercentageFloat(percent, true) + UI.UNITSUFFIXES.PERCENT + " " + UI.UNITSUFFIXES.GROWTH, timeSlice);
	}

	// Token: 0x06006533 RID: 25907 RVA: 0x000E217D File Offset: 0x000E037D
	public static string GetFormattedPercent(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardPercentageFloat(percent, true) + UI.UNITSUFFIXES.PERCENT, timeSlice);
	}

	// Token: 0x06006534 RID: 25908 RVA: 0x002C8F88 File Offset: 0x002C7188
	public static string GetFormattedRoundedJoules(float joules)
	{
		if (Mathf.Abs(joules) > 1000f)
		{
			return GameUtil.FloatToString(joules / 1000f, "F1") + UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE;
		}
		return GameUtil.FloatToString(joules, "F1") + UI.UNITSUFFIXES.ELECTRICAL.JOULE;
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x002C8FE0 File Offset: 0x002C71E0
	public static string GetFormattedJoules(float joules, string floatFormat = "F1", GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (timeSlice == GameUtil.TimeSlice.PerSecond)
		{
			return GameUtil.GetFormattedWattage(joules, GameUtil.WattageFormatterUnit.Automatic, true);
		}
		joules = GameUtil.ApplyTimeSlice(joules, timeSlice);
		string text;
		if (Math.Abs(joules) > 1000000f)
		{
			text = GameUtil.FloatToString(joules / 1000000f, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.MEGAJOULE;
		}
		else if (Mathf.Abs(joules) > 1000f)
		{
			text = GameUtil.FloatToString(joules / 1000f, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE;
		}
		else
		{
			text = GameUtil.FloatToString(joules, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.JOULE;
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x000E21A4 File Offset: 0x000E03A4
	public static string GetFormattedRads(float rads, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		rads = GameUtil.ApplyTimeSlice(rads, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.GetStandardFloat(rads) + UI.UNITSUFFIXES.RADIATION.RADS, timeSlice);
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x002C907C File Offset: 0x002C727C
	public static string GetFormattedHighEnergyParticles(float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displayUnits = true)
	{
		string str = (units == 1f) ? UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLE : UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		units = GameUtil.ApplyTimeSlice(units, timeSlice);
		return GameUtil.AddTimeSliceText(displayUnits ? (GameUtil.GetFloatWithDecimalPoint(units) + str) : GameUtil.GetFloatWithDecimalPoint(units), timeSlice);
	}

	// Token: 0x06006538 RID: 25912 RVA: 0x002C90CC File Offset: 0x002C72CC
	public static string GetFormattedWattage(float watts, GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic, bool displayUnits = true)
	{
		LocString loc_string = "";
		switch (unit)
		{
		case GameUtil.WattageFormatterUnit.Watts:
			loc_string = UI.UNITSUFFIXES.ELECTRICAL.WATT;
			break;
		case GameUtil.WattageFormatterUnit.Kilowatts:
			watts /= 1000f;
			loc_string = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
			break;
		case GameUtil.WattageFormatterUnit.Automatic:
			if (Mathf.Abs(watts) > 1000f)
			{
				watts /= 1000f;
				loc_string = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
			}
			else
			{
				loc_string = UI.UNITSUFFIXES.ELECTRICAL.WATT;
			}
			break;
		}
		if (displayUnits)
		{
			return GameUtil.FloatToString(watts, "###0.##") + loc_string;
		}
		return GameUtil.FloatToString(watts, "###0.##");
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x002C915C File Offset: 0x002C735C
	public static string GetFormattedHeatEnergy(float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		LocString loc_string = "";
		string format;
		switch (unit)
		{
		case GameUtil.HeatEnergyFormatterUnit.DTU_S:
			loc_string = UI.UNITSUFFIXES.HEAT.DTU;
			format = "###0.";
			break;
		case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
			dtu /= 1000f;
			loc_string = UI.UNITSUFFIXES.HEAT.KDTU;
			format = "###0.##";
			break;
		default:
			if (Mathf.Abs(dtu) > 1000f)
			{
				dtu /= 1000f;
				loc_string = UI.UNITSUFFIXES.HEAT.KDTU;
				format = "###0.##";
			}
			else
			{
				loc_string = UI.UNITSUFFIXES.HEAT.DTU;
				format = "###0.";
			}
			break;
		}
		return GameUtil.FloatToString(dtu, format) + loc_string;
	}

	// Token: 0x0600653A RID: 25914 RVA: 0x002C91F0 File Offset: 0x002C73F0
	public static string GetFormattedHeatEnergyRate(float dtu_s, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		LocString loc_string = "";
		switch (unit)
		{
		case GameUtil.HeatEnergyFormatterUnit.DTU_S:
			loc_string = UI.UNITSUFFIXES.HEAT.DTU_S;
			break;
		case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
			dtu_s /= 1000f;
			loc_string = UI.UNITSUFFIXES.HEAT.KDTU_S;
			break;
		case GameUtil.HeatEnergyFormatterUnit.Automatic:
			if (Mathf.Abs(dtu_s) > 1000f)
			{
				dtu_s /= 1000f;
				loc_string = UI.UNITSUFFIXES.HEAT.KDTU_S;
			}
			else
			{
				loc_string = UI.UNITSUFFIXES.HEAT.DTU_S;
			}
			break;
		}
		return GameUtil.FloatToString(dtu_s, "###0.##") + loc_string;
	}

	// Token: 0x0600653B RID: 25915 RVA: 0x000E21CA File Offset: 0x000E03CA
	public static string GetFormattedInt(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		num = GameUtil.ApplyTimeSlice(num, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.FloatToString(num, "F0"), timeSlice);
	}

	// Token: 0x0600653C RID: 25916 RVA: 0x002C9270 File Offset: 0x002C7470
	public static string GetFormattedSimple(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, string formatString = null)
	{
		num = GameUtil.ApplyTimeSlice(num, timeSlice);
		string text;
		if (formatString != null)
		{
			text = GameUtil.FloatToString(num, formatString);
		}
		else if (num == 0f)
		{
			text = "0";
		}
		else if (Mathf.Abs(num) < 1f)
		{
			text = GameUtil.FloatToString(num, "#,##0.##");
		}
		else if (Mathf.Abs(num) < 10f)
		{
			text = GameUtil.FloatToString(num, "#,###.##");
		}
		else
		{
			text = GameUtil.FloatToString(num, "#,###.##");
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x0600653D RID: 25917 RVA: 0x000E21E6 File Offset: 0x000E03E6
	public static string GetFormattedLux(int lux)
	{
		return lux.ToString() + UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x0600653E RID: 25918 RVA: 0x002C92F4 File Offset: 0x002C74F4
	public static string GetLightDescription(int lux)
	{
		if (lux == 0)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.NO_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.LOW_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.VERY_LOW_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.MEDIUM_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.LOW_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.MEDIUM_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.VERY_HIGH_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.HIGH_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.MAX_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.VERY_HIGH_LIGHT;
		}
		return UI.OVERLAYS.LIGHTING.RANGES.MAX_LIGHT;
	}

	// Token: 0x0600653F RID: 25919 RVA: 0x002C93AC File Offset: 0x002C75AC
	public static string GetRadiationDescription(float radsPerCycle)
	{
		if (radsPerCycle == 0f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.NONE;
		}
		if (radsPerCycle < 100f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.VERY_LOW;
		}
		if (radsPerCycle < 200f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.LOW;
		}
		if (radsPerCycle < 400f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.MEDIUM;
		}
		if (radsPerCycle < 2000f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.HIGH;
		}
		if (radsPerCycle < 4000f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.VERY_HIGH;
		}
		return UI.OVERLAYS.RADIATION.RANGES.MAX;
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x002C9438 File Offset: 0x002C7638
	public static string GetFormattedByTag(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (GameTags.DisplayAsCalories.Contains(tag))
		{
			return GameUtil.GetFormattedCaloriesForItem(tag, amount, timeSlice, true);
		}
		if (GameTags.DisplayAsUnits.Contains(tag))
		{
			return GameUtil.GetFormattedUnits(amount, timeSlice, true, "");
		}
		return GameUtil.GetFormattedMass(amount, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}

	// Token: 0x06006541 RID: 25921 RVA: 0x002C9488 File Offset: 0x002C7688
	public static string GetFormattedFoodQuality(int quality)
	{
		if (GameUtil.adjectives == null)
		{
			GameUtil.adjectives = LocString.GetStrings(typeof(DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
		}
		LocString loc_string = (quality >= 0) ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE;
		int num = quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET;
		num = Mathf.Clamp(num, 0, GameUtil.adjectives.Length);
		return string.Format(loc_string, GameUtil.adjectives[num], GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x002C94F8 File Offset: 0x002C76F8
	public static string GetFormattedBytes(ulong amount)
	{
		string[] array = new string[]
		{
			UI.UNITSUFFIXES.INFORMATION.BYTE,
			UI.UNITSUFFIXES.INFORMATION.KILOBYTE,
			UI.UNITSUFFIXES.INFORMATION.MEGABYTE,
			UI.UNITSUFFIXES.INFORMATION.GIGABYTE,
			UI.UNITSUFFIXES.INFORMATION.TERABYTE
		};
		int num = (amount == 0UL) ? 0 : ((int)Math.Floor(Math.Floor(Math.Log(amount)) / Math.Log(1024.0)));
		double num2 = amount / Math.Pow(1024.0, (double)num);
		global::Debug.Assert(num >= 0 && num < array.Length);
		return string.Format("{0:F} {1}", num2, array[num]);
	}

	// Token: 0x06006543 RID: 25923 RVA: 0x002C95B0 File Offset: 0x002C77B0
	public static string GetFormattedInfomation(float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		amount = GameUtil.ApplyTimeSlice(amount, timeSlice);
		string str = "";
		if (amount < 1024f)
		{
			str = UI.UNITSUFFIXES.INFORMATION.KILOBYTE;
		}
		else if (amount < 1048576f)
		{
			amount /= 1000f;
			str = UI.UNITSUFFIXES.INFORMATION.MEGABYTE;
		}
		else if (amount < 1.0737418E+09f)
		{
			amount /= 1048576f;
			str = UI.UNITSUFFIXES.INFORMATION.GIGABYTE;
		}
		return GameUtil.AddTimeSliceText(amount.ToString() + str, timeSlice);
	}

	// Token: 0x06006544 RID: 25924 RVA: 0x002C9630 File Offset: 0x002C7830
	public static LocString GetCurrentMassUnit(bool useSmallUnit = false)
	{
		LocString result = null;
		GameUtil.MassUnit massUnit = GameUtil.massUnit;
		if (massUnit != GameUtil.MassUnit.Kilograms)
		{
			if (massUnit == GameUtil.MassUnit.Pounds)
			{
				result = UI.UNITSUFFIXES.MASS.POUND;
			}
		}
		else if (useSmallUnit)
		{
			result = UI.UNITSUFFIXES.MASS.GRAM;
		}
		else
		{
			result = UI.UNITSUFFIXES.MASS.KILOGRAM;
		}
		return result;
	}

	// Token: 0x06006545 RID: 25925 RVA: 0x002C9668 File Offset: 0x002C7868
	public static string GetFormattedMass(float mass, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold, bool includeSuffix = true, string floatFormat = "{0:0.#}")
	{
		if (mass == -3.4028235E+38f)
		{
			return UI.CALCULATING;
		}
		if (float.IsPositiveInfinity(mass))
		{
			return UI.POS_INFINITY + UI.UNITSUFFIXES.MASS.TONNE;
		}
		if (float.IsNegativeInfinity(mass))
		{
			return UI.NEG_INFINITY + UI.UNITSUFFIXES.MASS.TONNE;
		}
		mass = GameUtil.ApplyTimeSlice(mass, timeSlice);
		string str;
		if (GameUtil.massUnit == GameUtil.MassUnit.Kilograms)
		{
			str = UI.UNITSUFFIXES.MASS.TONNE;
			if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
			{
				float num = Mathf.Abs(mass);
				if (0f < num)
				{
					if (num < 5E-06f)
					{
						str = UI.UNITSUFFIXES.MASS.MICROGRAM;
						mass = Mathf.Floor(mass * 1E+09f);
					}
					else if (num < 0.005f)
					{
						mass *= 1000000f;
						str = UI.UNITSUFFIXES.MASS.MILLIGRAM;
					}
					else if (Mathf.Abs(mass) < 5f)
					{
						mass *= 1000f;
						str = UI.UNITSUFFIXES.MASS.GRAM;
					}
					else if (Mathf.Abs(mass) < 5000f)
					{
						str = UI.UNITSUFFIXES.MASS.KILOGRAM;
					}
					else
					{
						mass /= 1000f;
						str = UI.UNITSUFFIXES.MASS.TONNE;
					}
				}
				else
				{
					str = UI.UNITSUFFIXES.MASS.KILOGRAM;
				}
			}
			else if (massFormat == GameUtil.MetricMassFormat.Kilogram)
			{
				str = UI.UNITSUFFIXES.MASS.KILOGRAM;
			}
			else if (massFormat == GameUtil.MetricMassFormat.Gram)
			{
				mass *= 1000f;
				str = UI.UNITSUFFIXES.MASS.GRAM;
			}
			else if (massFormat == GameUtil.MetricMassFormat.Tonne)
			{
				mass /= 1000f;
				str = UI.UNITSUFFIXES.MASS.TONNE;
			}
		}
		else
		{
			mass /= 2.2f;
			str = UI.UNITSUFFIXES.MASS.POUND;
			if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
			{
				float num2 = Mathf.Abs(mass);
				if (num2 < 5f && num2 > 0.001f)
				{
					mass *= 256f;
					str = UI.UNITSUFFIXES.MASS.DRACHMA;
				}
				else
				{
					mass *= 7000f;
					str = UI.UNITSUFFIXES.MASS.GRAIN;
				}
			}
		}
		if (!includeSuffix)
		{
			str = "";
			timeSlice = GameUtil.TimeSlice.None;
		}
		return GameUtil.AddTimeSliceText(string.Format(floatFormat, mass) + str, timeSlice);
	}

	// Token: 0x06006546 RID: 25926 RVA: 0x000E21FE File Offset: 0x000E03FE
	public static string GetFormattedTime(float seconds, string floatFormat = "F0")
	{
		return string.Format(UI.FORMATSECONDS, seconds.ToString(floatFormat));
	}

	// Token: 0x06006547 RID: 25927 RVA: 0x000E2217 File Offset: 0x000E0417
	public static string GetFormattedEngineEfficiency(float amount)
	{
		return amount.ToString() + " km /" + UI.UNITSUFFIXES.MASS.KILOGRAM;
	}

	// Token: 0x06006548 RID: 25928 RVA: 0x002C987C File Offset: 0x002C7A7C
	public static string GetFormattedDistance(float meters)
	{
		if (Mathf.Abs(meters) < 1f)
		{
			string text = (meters * 100f).ToString();
			string text2 = text.Substring(0, text.LastIndexOf('.') + Mathf.Min(3, text.Length - text.LastIndexOf('.')));
			if (text2 == "-0.0")
			{
				text2 = "0";
			}
			return text2 + " cm";
		}
		if (meters < 1000f)
		{
			return meters.ToString() + " m";
		}
		return Util.FormatOneDecimalPlace(meters / 1000f) + " km";
	}

	// Token: 0x06006549 RID: 25929 RVA: 0x000E2234 File Offset: 0x000E0434
	public static string GetFormattedCycles(float seconds, string formatString = "F1", bool forceCycles = false)
	{
		if (forceCycles || Mathf.Abs(seconds) > 100f)
		{
			return string.Format(UI.FORMATDAY, GameUtil.FloatToString(seconds / 600f, formatString));
		}
		return GameUtil.GetFormattedTime(seconds, "F0");
	}

	// Token: 0x0600654A RID: 25930 RVA: 0x000E226E File Offset: 0x000E046E
	public static float GetDisplaySHC(float shc)
	{
		if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
		{
			shc /= 1.8f;
		}
		return shc;
	}

	// Token: 0x0600654B RID: 25931 RVA: 0x000E2282 File Offset: 0x000E0482
	public static string GetSHCSuffix()
	{
		return string.Format("(DTU/g)/{0}", GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x0600654C RID: 25932 RVA: 0x000E2293 File Offset: 0x000E0493
	public static string GetFormattedSHC(float shc)
	{
		shc = GameUtil.GetDisplaySHC(shc);
		return string.Format("{0} (DTU/g)/{1}", shc.ToString("0.000"), GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x0600654D RID: 25933 RVA: 0x000E226E File Offset: 0x000E046E
	public static float GetDisplayThermalConductivity(float tc)
	{
		if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
		{
			tc /= 1.8f;
		}
		return tc;
	}

	// Token: 0x0600654E RID: 25934 RVA: 0x000E22B8 File Offset: 0x000E04B8
	public static string GetThermalConductivitySuffix()
	{
		return string.Format("(DTU/(m*s))/{0}", GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x0600654F RID: 25935 RVA: 0x000E22C9 File Offset: 0x000E04C9
	public static string GetFormattedThermalConductivity(float tc)
	{
		tc = GameUtil.GetDisplayThermalConductivity(tc);
		return string.Format("{0} (DTU/(m*s))/{1}", tc.ToString("0.000"), GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x06006550 RID: 25936 RVA: 0x000E22EE File Offset: 0x000E04EE
	public static string GetElementNameByElementHash(SimHashes elementHash)
	{
		return ElementLoader.FindElementByHash(elementHash).tag.ProperName();
	}

	// Token: 0x06006551 RID: 25937 RVA: 0x002C991C File Offset: 0x002C7B1C
	public static bool HasTrait(GameObject go, string traitName)
	{
		Traits component = go.GetComponent<Traits>();
		return !(component == null) && component.HasTrait(traitName);
	}

	// Token: 0x06006552 RID: 25938 RVA: 0x002C9944 File Offset: 0x002C7B44
	public static HashSet<int> GetFloodFillCavity(int startCell, bool allowLiquid)
	{
		HashSet<int> result = new HashSet<int>();
		if (allowLiquid)
		{
			result = GameUtil.FloodCollectCells(startCell, (int cell) => !Grid.Solid[cell], 300, null, true);
		}
		else
		{
			result = GameUtil.FloodCollectCells(startCell, (int cell) => Grid.Element[cell].IsVacuum || Grid.Element[cell].IsGas, 300, null, true);
		}
		return result;
	}

	// Token: 0x06006553 RID: 25939 RVA: 0x002C99B8 File Offset: 0x002C7BB8
	public static float GetRadiationAbsorptionPercentage(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return GameUtil.GetRadiationAbsorptionPercentage(Grid.Element[cell], Grid.Mass[cell], Grid.IsSolidCell(cell) && (Grid.Properties[cell] & 128) == 128);
		}
		return 0f;
	}

	// Token: 0x06006554 RID: 25940 RVA: 0x002C9A10 File Offset: 0x002C7C10
	public static float GetRadiationAbsorptionPercentage(Element elem, float mass, bool isConstructed)
	{
		float num = 2000f;
		float num2 = 0.3f;
		float num3 = 0.7f;
		float num4 = 0.8f;
		float value;
		if (isConstructed)
		{
			value = elem.radiationAbsorptionFactor * num4;
		}
		else
		{
			value = elem.radiationAbsorptionFactor * num2 + mass / num * elem.radiationAbsorptionFactor * num3;
		}
		return Mathf.Clamp(value, 0f, 1f);
	}

	// Token: 0x06006555 RID: 25941 RVA: 0x002C9A74 File Offset: 0x002C7C74
	public static HashSet<int> CollectCellsBreadthFirst(int start_cell, Func<int, bool> test_func, int max_depth = 10)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		HashSet<int> hashSet3 = new HashSet<int>();
		hashSet3.Add(start_cell);
		Vector2Int[] array = new Vector2Int[]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};
		for (int i = 0; i < max_depth; i++)
		{
			List<int> list = new List<int>();
			foreach (int cell in hashSet3)
			{
				foreach (Vector2Int vector2Int in array)
				{
					int num = Grid.OffsetCell(cell, vector2Int.x, vector2Int.y);
					if (!hashSet2.Contains(num) && !hashSet.Contains(num))
					{
						if (Grid.IsValidCell(num) && test_func(num))
						{
							hashSet.Add(num);
							list.Add(num);
						}
						else
						{
							hashSet2.Add(num);
						}
					}
				}
			}
			hashSet3.Clear();
			foreach (int item in list)
			{
				hashSet3.Add(item);
			}
			list.Clear();
			if (hashSet3.Count == 0)
			{
				break;
			}
		}
		return hashSet;
	}

	// Token: 0x06006556 RID: 25942 RVA: 0x002C9C10 File Offset: 0x002C7E10
	public static HashSet<int> FloodCollectCells(int start_cell, Func<int, bool> is_valid, int maxSize = 300, HashSet<int> AddInvalidCellsToSet = null, bool clearOversizedResults = true)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		GameUtil.probeFromCell(start_cell, is_valid, hashSet, hashSet2, maxSize);
		if (AddInvalidCellsToSet != null)
		{
			AddInvalidCellsToSet.UnionWith(hashSet2);
			if (hashSet.Count > maxSize)
			{
				AddInvalidCellsToSet.UnionWith(hashSet);
			}
		}
		if (hashSet.Count > maxSize && clearOversizedResults)
		{
			hashSet.Clear();
		}
		return hashSet;
	}

	// Token: 0x06006557 RID: 25943 RVA: 0x002C9C64 File Offset: 0x002C7E64
	public static HashSet<int> FloodCollectCells(HashSet<int> results, int start_cell, Func<int, bool> is_valid, int maxSize = 300, HashSet<int> AddInvalidCellsToSet = null, bool clearOversizedResults = true)
	{
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.probeFromCell(start_cell, is_valid, results, hashSet, maxSize);
		if (AddInvalidCellsToSet != null)
		{
			AddInvalidCellsToSet.UnionWith(hashSet);
			if (results.Count > maxSize)
			{
				AddInvalidCellsToSet.UnionWith(results);
			}
		}
		if (results.Count > maxSize && clearOversizedResults)
		{
			results.Clear();
		}
		return results;
	}

	// Token: 0x06006558 RID: 25944 RVA: 0x002C9CB4 File Offset: 0x002C7EB4
	private static void probeFromCell(int start_cell, Func<int, bool> is_valid, HashSet<int> cells, HashSet<int> invalidCells, int maxSize = 300)
	{
		if (cells.Count > maxSize || !Grid.IsValidCell(start_cell) || invalidCells.Contains(start_cell) || cells.Contains(start_cell) || !is_valid(start_cell))
		{
			invalidCells.Add(start_cell);
			return;
		}
		cells.Add(start_cell);
		GameUtil.probeFromCell(Grid.CellLeft(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellRight(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellAbove(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellBelow(start_cell), is_valid, cells, invalidCells, maxSize);
	}

	// Token: 0x06006559 RID: 25945 RVA: 0x000E2300 File Offset: 0x000E0500
	public static bool FloodFillCheck<ArgType>(Func<int, ArgType, bool> fn, ArgType arg, int start_cell, int max_depth, bool stop_at_solid, bool stop_at_liquid)
	{
		return GameUtil.FloodFillFind<ArgType>(fn, arg, start_cell, max_depth, stop_at_solid, stop_at_liquid) != -1;
	}

	// Token: 0x0600655A RID: 25946 RVA: 0x002C9D40 File Offset: 0x002C7F40
	private static bool CellCheck(int cell, bool stop_at_solid, bool stop_at_liquid)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		Element element = Grid.Element[cell];
		return (!stop_at_solid || !element.IsSolid) && (!stop_at_liquid || !element.IsLiquid) && !GameUtil.FloodFillVisited.Value.Contains(cell);
	}

	// Token: 0x0600655B RID: 25947 RVA: 0x002C9D90 File Offset: 0x002C7F90
	public static int FloodFillFind<ArgType>(Func<int, ArgType, bool> fn, ArgType arg, int start_cell, int max_depth, bool stop_at_solid, bool stop_at_liquid)
	{
		if (GameUtil.CellCheck(start_cell, stop_at_solid, stop_at_liquid))
		{
			GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
			{
				cell = start_cell,
				depth = 0
			});
		}
		int result = -1;
		while (GameUtil.FloodFillNext.Value.Count > 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = GameUtil.FloodFillNext.Value.Dequeue();
			if (!GameUtil.FloodFillVisited.Value.Contains(floodFillInfo.cell))
			{
				GameUtil.FloodFillVisited.Value.Add(floodFillInfo.cell);
				if (fn(floodFillInfo.cell, arg))
				{
					result = floodFillInfo.cell;
					break;
				}
				if (floodFillInfo.depth < max_depth)
				{
					GameUtil.FloodFillNeighbors.Value[0] = Grid.CellLeft(floodFillInfo.cell);
					GameUtil.FloodFillNeighbors.Value[1] = Grid.CellAbove(floodFillInfo.cell);
					GameUtil.FloodFillNeighbors.Value[2] = Grid.CellRight(floodFillInfo.cell);
					GameUtil.FloodFillNeighbors.Value[3] = Grid.CellBelow(floodFillInfo.cell);
					foreach (int cell in GameUtil.FloodFillNeighbors.Value)
					{
						if (GameUtil.CellCheck(cell, stop_at_solid, stop_at_liquid))
						{
							GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
							{
								cell = cell,
								depth = floodFillInfo.depth + 1
							});
						}
					}
				}
			}
		}
		GameUtil.FloodFillVisited.Value.Clear();
		GameUtil.FloodFillNext.Value.Clear();
		return result;
	}

	// Token: 0x0600655C RID: 25948 RVA: 0x002C9F60 File Offset: 0x002C8160
	public static void FloodFillConditional(int start_cell, Func<int, bool> condition, ICollection<int> visited_cells, ICollection<int> valid_cells = null)
	{
		GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = start_cell,
			depth = 0
		});
		GameUtil.FloodFillConditional(GameUtil.FloodFillNext.Value, condition, visited_cells, valid_cells, 10000);
	}

	// Token: 0x0600655D RID: 25949 RVA: 0x002C9FAC File Offset: 0x002C81AC
	public static void FloodFillConditional(Queue<GameUtil.FloodFillInfo> queue, Func<int, bool> condition, ICollection<int> visited_cells, ICollection<int> valid_cells = null, int max_depth = 10000)
	{
		while (queue.Count > 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = queue.Dequeue();
			if (floodFillInfo.depth < max_depth && Grid.IsValidCell(floodFillInfo.cell) && !visited_cells.Contains(floodFillInfo.cell))
			{
				visited_cells.Add(floodFillInfo.cell);
				if (condition(floodFillInfo.cell))
				{
					if (valid_cells != null)
					{
						valid_cells.Add(floodFillInfo.cell);
					}
					int depth = floodFillInfo.depth + 1;
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellLeft(floodFillInfo.cell),
						depth = depth
					});
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellRight(floodFillInfo.cell),
						depth = depth
					});
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellAbove(floodFillInfo.cell),
						depth = depth
					});
					queue.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = Grid.CellBelow(floodFillInfo.cell),
						depth = depth
					});
				}
			}
		}
		queue.Clear();
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x002CA0E0 File Offset: 0x002C82E0
	public static string GetHardnessString(Element element, bool addColor = true)
	{
		if (!element.IsSolid)
		{
			return ELEMENTS.HARDNESS.NA;
		}
		Color c = GameUtil.Hardness.firmColor;
		string text;
		if (element.hardness >= 255)
		{
			c = GameUtil.Hardness.ImpenetrableColor;
			text = string.Format(ELEMENTS.HARDNESS.IMPENETRABLE, element.hardness);
		}
		else if (element.hardness >= 150)
		{
			c = GameUtil.Hardness.nearlyImpenetrableColor;
			text = string.Format(ELEMENTS.HARDNESS.NEARLYIMPENETRABLE, element.hardness);
		}
		else if (element.hardness >= 50)
		{
			c = GameUtil.Hardness.veryFirmColor;
			text = string.Format(ELEMENTS.HARDNESS.VERYFIRM, element.hardness);
		}
		else if (element.hardness >= 25)
		{
			c = GameUtil.Hardness.firmColor;
			text = string.Format(ELEMENTS.HARDNESS.FIRM, element.hardness);
		}
		else if (element.hardness >= 10)
		{
			c = GameUtil.Hardness.softColor;
			text = string.Format(ELEMENTS.HARDNESS.SOFT, element.hardness);
		}
		else
		{
			c = GameUtil.Hardness.verySoftColor;
			text = string.Format(ELEMENTS.HARDNESS.VERYSOFT, element.hardness);
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		return text;
	}

	// Token: 0x0600655F RID: 25951 RVA: 0x002CA230 File Offset: 0x002C8430
	public static string GetGermResistanceModifierString(float modifier, bool addColor = true)
	{
		Color c = Color.black;
		string text = "";
		if (modifier > 0f)
		{
			if (modifier >= 5f)
			{
				c = GameUtil.GermResistanceValues.PositiveLargeColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_LARGE, modifier);
			}
			else if (modifier >= 2f)
			{
				c = GameUtil.GermResistanceValues.PositiveMediumColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_MEDIUM, modifier);
			}
			else if (modifier > 0f)
			{
				c = GameUtil.GermResistanceValues.PositiveSmallColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_SMALL, modifier);
			}
		}
		else if (modifier < 0f)
		{
			if (modifier <= -5f)
			{
				c = GameUtil.GermResistanceValues.NegativeLargeColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_LARGE, modifier);
			}
			else if (modifier <= -2f)
			{
				c = GameUtil.GermResistanceValues.NegativeMediumColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_MEDIUM, modifier);
			}
			else if (modifier < 0f)
			{
				c = GameUtil.GermResistanceValues.NegativeSmallColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_SMALL, modifier);
			}
		}
		else
		{
			addColor = false;
			text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NONE, modifier);
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		return text;
	}

	// Token: 0x06006560 RID: 25952 RVA: 0x002CA378 File Offset: 0x002C8578
	public static string GetThermalConductivityString(Element element, bool addColor = true, bool addValue = true)
	{
		Color c = GameUtil.ThermalConductivityValues.mediumConductivityColor;
		string text;
		if (element.thermalConductivity >= 50f)
		{
			c = GameUtil.ThermalConductivityValues.veryHighConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_HIGH_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 10f)
		{
			c = GameUtil.ThermalConductivityValues.highConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.HIGH_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 2f)
		{
			c = GameUtil.ThermalConductivityValues.mediumConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.MEDIUM_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 1f)
		{
			c = GameUtil.ThermalConductivityValues.lowConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.LOW_CONDUCTIVITY;
		}
		else
		{
			c = GameUtil.ThermalConductivityValues.veryLowConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_LOW_CONDUCTIVITY;
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		if (addValue)
		{
			text = string.Format(UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VALUE_WITH_ADJECTIVE, element.thermalConductivity.ToString(), text);
		}
		return text;
	}

	// Token: 0x06006561 RID: 25953 RVA: 0x002CA458 File Offset: 0x002C8658
	public static string GetBreathableString(Element element, float Mass)
	{
		if (!element.IsGas && !element.IsVacuum)
		{
			return "";
		}
		Color c = GameUtil.BreathableValues.positiveColor;
		SimHashes id = element.id;
		LocString arg;
		if (id != SimHashes.Oxygen)
		{
			if (id != SimHashes.ContaminatedOxygen)
			{
				c = GameUtil.BreathableValues.negativeColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND4;
			}
			else if (Mass >= SimDebugView.optimallyBreathable)
			{
				c = GameUtil.BreathableValues.positiveColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND1;
			}
			else if (Mass >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f)
			{
				c = GameUtil.BreathableValues.positiveColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND2;
			}
			else if (Mass >= SimDebugView.minimumBreathable)
			{
				c = GameUtil.BreathableValues.warningColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND3;
			}
			else
			{
				c = GameUtil.BreathableValues.negativeColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND4;
			}
		}
		else if (Mass >= SimDebugView.optimallyBreathable)
		{
			c = GameUtil.BreathableValues.positiveColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND1;
		}
		else if (Mass >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f)
		{
			c = GameUtil.BreathableValues.positiveColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND2;
		}
		else if (Mass >= SimDebugView.minimumBreathable)
		{
			c = GameUtil.BreathableValues.warningColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND3;
		}
		else
		{
			c = GameUtil.BreathableValues.negativeColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND4;
		}
		return string.Format(ELEMENTS.BREATHABLEDESC, c.ToHexString(), arg);
	}

	// Token: 0x06006562 RID: 25954 RVA: 0x002CA58C File Offset: 0x002C878C
	public static string GetWireLoadColor(float load, float maxLoad, float potentialLoad)
	{
		Color c;
		if (load > maxLoad + POWER.FLOAT_FUDGE_FACTOR)
		{
			c = GameUtil.WireLoadValues.negativeColor;
		}
		else if (potentialLoad > maxLoad && load / maxLoad >= 0.75f)
		{
			c = GameUtil.WireLoadValues.warningColor;
		}
		else
		{
			c = Color.white;
		}
		return c.ToHexString();
	}

	// Token: 0x06006563 RID: 25955 RVA: 0x000E2315 File Offset: 0x000E0515
	public static string GetHotkeyString(global::Action action)
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			return UI.FormatAsHotkey(GameUtil.GetActionString(action));
		}
		return UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]");
	}

	// Token: 0x06006564 RID: 25956 RVA: 0x000E2344 File Offset: 0x000E0544
	public static string ReplaceHotkeyString(string template, global::Action action)
	{
		return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action));
	}

	// Token: 0x06006565 RID: 25957 RVA: 0x000E2357 File Offset: 0x000E0557
	public static string ReplaceHotkeyString(string template, global::Action action1, global::Action action2)
	{
		return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action1) + GameUtil.GetHotkeyString(action2));
	}

	// Token: 0x06006566 RID: 25958 RVA: 0x002CA5D0 File Offset: 0x002C87D0
	public static string GetKeycodeLocalized(KKeyCode key_code)
	{
		string result = key_code.ToString();
		if (key_code <= KKeyCode.Slash)
		{
			if (key_code <= KKeyCode.Tab)
			{
				if (key_code == KKeyCode.None)
				{
					return result;
				}
				if (key_code == KKeyCode.Backspace)
				{
					return INPUT.BACKSPACE;
				}
				if (key_code == KKeyCode.Tab)
				{
					return INPUT.TAB;
				}
			}
			else if (key_code <= KKeyCode.Escape)
			{
				if (key_code == KKeyCode.Return)
				{
					return INPUT.ENTER;
				}
				if (key_code == KKeyCode.Escape)
				{
					return INPUT.ESCAPE;
				}
			}
			else
			{
				if (key_code == KKeyCode.Space)
				{
					return INPUT.SPACE;
				}
				switch (key_code)
				{
				case KKeyCode.Plus:
					return "+";
				case KKeyCode.Comma:
					return ",";
				case KKeyCode.Minus:
					return "-";
				case KKeyCode.Period:
					return INPUT.PERIOD;
				case KKeyCode.Slash:
					return "/";
				}
			}
		}
		else if (key_code <= KKeyCode.Insert)
		{
			switch (key_code)
			{
			case KKeyCode.Colon:
				return ":";
			case KKeyCode.Semicolon:
				return ";";
			case KKeyCode.Less:
				break;
			case KKeyCode.Equals:
				return "=";
			default:
				switch (key_code)
				{
				case KKeyCode.LeftBracket:
					return "[";
				case KKeyCode.Backslash:
					return "\\";
				case KKeyCode.RightBracket:
					return "]";
				case KKeyCode.Caret:
				case KKeyCode.Underscore:
					break;
				case KKeyCode.BackQuote:
					return INPUT.BACKQUOTE;
				default:
					switch (key_code)
					{
					case KKeyCode.Keypad0:
						return INPUT.NUM + " 0";
					case KKeyCode.Keypad1:
						return INPUT.NUM + " 1";
					case KKeyCode.Keypad2:
						return INPUT.NUM + " 2";
					case KKeyCode.Keypad3:
						return INPUT.NUM + " 3";
					case KKeyCode.Keypad4:
						return INPUT.NUM + " 4";
					case KKeyCode.Keypad5:
						return INPUT.NUM + " 5";
					case KKeyCode.Keypad6:
						return INPUT.NUM + " 6";
					case KKeyCode.Keypad7:
						return INPUT.NUM + " 7";
					case KKeyCode.Keypad8:
						return INPUT.NUM + " 8";
					case KKeyCode.Keypad9:
						return INPUT.NUM + " 9";
					case KKeyCode.KeypadPeriod:
						return INPUT.NUM + " " + INPUT.PERIOD;
					case KKeyCode.KeypadDivide:
						return INPUT.NUM + " /";
					case KKeyCode.KeypadMultiply:
						return INPUT.NUM + " *";
					case KKeyCode.KeypadMinus:
						return INPUT.NUM + " -";
					case KKeyCode.KeypadPlus:
						return INPUT.NUM + " +";
					case KKeyCode.KeypadEnter:
						return INPUT.NUM + " " + INPUT.ENTER;
					case KKeyCode.Insert:
						return INPUT.INSERT;
					}
					break;
				}
				break;
			}
		}
		else if (key_code <= KKeyCode.Mouse6)
		{
			switch (key_code)
			{
			case KKeyCode.RightShift:
				return INPUT.RIGHT_SHIFT;
			case KKeyCode.LeftShift:
				return INPUT.LEFT_SHIFT;
			case KKeyCode.RightControl:
				return INPUT.RIGHT_CTRL;
			case KKeyCode.LeftControl:
				return INPUT.LEFT_CTRL;
			case KKeyCode.RightAlt:
				return INPUT.RIGHT_ALT;
			case KKeyCode.LeftAlt:
				return INPUT.LEFT_ALT;
			default:
				switch (key_code)
				{
				case KKeyCode.Mouse0:
					return INPUT.MOUSE + " 0";
				case KKeyCode.Mouse1:
					return INPUT.MOUSE + " 1";
				case KKeyCode.Mouse2:
					return INPUT.MOUSE + " 2";
				case KKeyCode.Mouse3:
					return INPUT.MOUSE + " 3";
				case KKeyCode.Mouse4:
					return INPUT.MOUSE + " 4";
				case KKeyCode.Mouse5:
					return INPUT.MOUSE + " 5";
				case KKeyCode.Mouse6:
					return INPUT.MOUSE + " 6";
				}
				break;
			}
		}
		else
		{
			if (key_code == KKeyCode.MouseScrollDown)
			{
				return INPUT.MOUSE_SCROLL_DOWN;
			}
			if (key_code == KKeyCode.MouseScrollUp)
			{
				return INPUT.MOUSE_SCROLL_UP;
			}
		}
		if (KKeyCode.A <= key_code && key_code <= KKeyCode.Z)
		{
			result = ((char)(65 + (key_code - KKeyCode.A))).ToString();
		}
		else if (KKeyCode.Alpha0 <= key_code && key_code <= KKeyCode.Alpha9)
		{
			result = ((char)(48 + (key_code - KKeyCode.Alpha0))).ToString();
		}
		else if (KKeyCode.F1 <= key_code && key_code <= KKeyCode.F12)
		{
			result = "F" + (key_code - KKeyCode.F1 + 1).ToString();
		}
		else
		{
			global::Debug.LogWarning("Unable to find proper string for KKeyCode: " + key_code.ToString() + " using key_code.ToString()");
		}
		return result;
	}

	// Token: 0x06006567 RID: 25959 RVA: 0x002CABD8 File Offset: 0x002C8DD8
	public static string GetActionString(global::Action action)
	{
		string result = "";
		if (action == global::Action.NumActions)
		{
			return result;
		}
		BindingEntry bindingEntry = GameUtil.ActionToBinding(action);
		KKeyCode mKeyCode = bindingEntry.mKeyCode;
		if (KInputManager.currentControllerIsGamepad)
		{
			return KInputManager.steamInputInterpreter.GetActionGlyph(action);
		}
		if (bindingEntry.mModifier == global::Modifier.None)
		{
			return GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
		}
		string str = "";
		global::Modifier mModifier = bindingEntry.mModifier;
		switch (mModifier)
		{
		case global::Modifier.Alt:
			str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftAlt).ToUpper();
			break;
		case global::Modifier.Ctrl:
			str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftControl).ToUpper();
			break;
		case (global::Modifier)3:
			break;
		case global::Modifier.Shift:
			str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftShift).ToUpper();
			break;
		default:
			if (mModifier != global::Modifier.CapsLock)
			{
				if (mModifier == global::Modifier.Backtick)
				{
					str = GameUtil.GetKeycodeLocalized(KKeyCode.BackQuote).ToUpper();
				}
			}
			else
			{
				str = GameUtil.GetKeycodeLocalized(KKeyCode.CapsLock).ToUpper();
			}
			break;
		}
		return str + " + " + GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
	}

	// Token: 0x06006568 RID: 25960 RVA: 0x002CACCC File Offset: 0x002C8ECC
	public static void CreateExplosion(Vector3 explosion_pos)
	{
		Vector2 b = new Vector2(explosion_pos.x, explosion_pos.y);
		float num = 5f;
		float num2 = num * num;
		foreach (Health health in Components.Health.Items)
		{
			Vector3 position = health.transform.GetPosition();
			float sqrMagnitude = (new Vector2(position.x, position.y) - b).sqrMagnitude;
			if (num2 >= sqrMagnitude && health != null)
			{
				health.Damage(health.maxHitPoints);
			}
		}
	}

	// Token: 0x06006569 RID: 25961 RVA: 0x002CAD84 File Offset: 0x002C8F84
	private static void GetNonSolidCells(int x, int y, List<int> cells, int min_x, int min_y, int max_x, int max_y)
	{
		int num = Grid.XYToCell(x, y);
		if (Grid.IsValidCell(num) && !Grid.Solid[num] && !Grid.DupePassable[num] && x >= min_x && x <= max_x && y >= min_y && y <= max_y && !cells.Contains(num))
		{
			cells.Add(num);
			GameUtil.GetNonSolidCells(x + 1, y, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x - 1, y, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x, y + 1, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x, y - 1, cells, min_x, min_y, max_x, max_y);
		}
	}

	// Token: 0x0600656A RID: 25962 RVA: 0x002CAE28 File Offset: 0x002C9028
	public static void GetNonSolidCells(int cell, int radius, List<int> cells)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		GameUtil.GetNonSolidCells(num, num2, cells, num - radius, num2 - radius, num + radius, num2 + radius);
	}

	// Token: 0x0600656B RID: 25963 RVA: 0x002CAE58 File Offset: 0x002C9058
	public static float GetMaxStressInActiveWorld()
	{
		if (Components.LiveMinionIdentities.Count <= 0)
		{
			return 0f;
		}
		float num = 0f;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(minionIdentity);
				if (amountInstance != null)
				{
					num = Mathf.Max(num, amountInstance.value);
				}
			}
		}
		return num;
	}

	// Token: 0x0600656C RID: 25964 RVA: 0x002CAF04 File Offset: 0x002C9104
	public static float GetAverageStressInActiveWorld()
	{
		if (Components.LiveMinionIdentities.Count <= 0)
		{
			return 0f;
		}
		float num = 0f;
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				num += Db.Get().Amounts.Stress.Lookup(minionIdentity).value;
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x0600656D RID: 25965 RVA: 0x000E2375 File Offset: 0x000E0575
	public static string MigrateFMOD(FMODAsset asset)
	{
		if (asset == null)
		{
			return null;
		}
		if (asset.path == null)
		{
			return asset.name;
		}
		return asset.path;
	}

	// Token: 0x0600656E RID: 25966 RVA: 0x000E2397 File Offset: 0x000E0597
	private static void SortGameObjectDescriptors(List<IGameObjectEffectDescriptor> descriptorList)
	{
		descriptorList.Sort(delegate(IGameObjectEffectDescriptor e1, IGameObjectEffectDescriptor e2)
		{
			int num = TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType());
			int value = TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType());
			return num.CompareTo(value);
		});
	}

	// Token: 0x0600656F RID: 25967 RVA: 0x002CAFB0 File Offset: 0x002C91B0
	public static void IndentListOfDescriptors(List<Descriptor> list, int indentCount = 1)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Descriptor value = list[i];
			for (int j = 0; j < indentCount; j++)
			{
				value.IncreaseIndent();
			}
			list[i] = value;
		}
	}

	// Token: 0x06006570 RID: 25968 RVA: 0x002CAFF4 File Offset: 0x002C91F4
	public static List<Descriptor> GetAllDescriptors(GameObject go, bool simpleInfoScreen = false)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if (!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen)
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2 != null && component2.AdditionalRequirements != null)
		{
			foreach (Descriptor descriptor2 in component2.AdditionalRequirements)
			{
				if (!descriptor2.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor2);
				}
			}
		}
		if (component2 != null && component2.AdditionalEffects != null)
		{
			foreach (Descriptor descriptor3 in component2.AdditionalEffects)
			{
				if (!descriptor3.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor3);
				}
			}
		}
		return list;
	}

	// Token: 0x06006571 RID: 25969 RVA: 0x002CB194 File Offset: 0x002C9394
	public static List<Descriptor> GetDetailDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Detail)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06006572 RID: 25970 RVA: 0x000E23BE File Offset: 0x000E05BE
	public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors)
	{
		return GameUtil.GetRequirementDescriptors(descriptors, true);
	}

	// Token: 0x06006573 RID: 25971 RVA: 0x002CB1FC File Offset: 0x002C93FC
	public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors, bool indent)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Requirement)
			{
				list.Add(descriptor);
			}
		}
		if (indent)
		{
			GameUtil.IndentListOfDescriptors(list, 1);
		}
		return list;
	}

	// Token: 0x06006574 RID: 25972 RVA: 0x002CB264 File Offset: 0x002C9464
	public static List<Descriptor> GetEffectDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06006575 RID: 25973 RVA: 0x002CB2D4 File Offset: 0x002C94D4
	public static List<Descriptor> GetInformationDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06006576 RID: 25974 RVA: 0x002CB33C File Offset: 0x002C953C
	public static List<Descriptor> GetCropOptimumConditionDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
			{
				Descriptor descriptor2 = descriptor;
				descriptor2.text = "• " + descriptor2.text;
				list.Add(descriptor2);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06006577 RID: 25975 RVA: 0x002CB3BC File Offset: 0x002C95BC
	public static List<Descriptor> GetGameObjectRequirements(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if (descriptor.type == Descriptor.DescriptorType.Requirement)
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2.AdditionalRequirements != null)
		{
			list.AddRange(component2.AdditionalRequirements);
		}
		return list;
	}

	// Token: 0x06006578 RID: 25976 RVA: 0x002CB4AC File Offset: 0x002C96AC
	public static List<Descriptor> GetGameObjectEffects(GameObject go, bool simpleInfoScreen = false)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if ((!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen) && (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource))
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2 != null && component2.AdditionalEffects != null)
		{
			foreach (Descriptor descriptor2 in component2.AdditionalEffects)
			{
				if (!descriptor2.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor2);
				}
			}
		}
		return list;
	}

	// Token: 0x06006579 RID: 25977 RVA: 0x002CB600 File Offset: 0x002C9800
	public static List<Descriptor> GetPlantRequirementDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(go, false));
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTREQUIREMENTS, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTREQUIREMENTS, Descriptor.DescriptorType.Requirement);
			list.Add(item);
			list.AddRange(requirementDescriptors);
		}
		return list;
	}

	// Token: 0x0600657A RID: 25978 RVA: 0x002CB65C File Offset: 0x002C985C
	public static List<Descriptor> GetPlantLifeCycleDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<Descriptor> informationDescriptors = GameUtil.GetInformationDescriptors(GameUtil.GetAllDescriptors(go, false));
		if (informationDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.LIFECYCLE, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTLIFECYCLE, Descriptor.DescriptorType.Lifecycle);
			list.Add(item);
			list.AddRange(informationDescriptors);
		}
		return list;
	}

	// Token: 0x0600657B RID: 25979 RVA: 0x002CB6B8 File Offset: 0x002C98B8
	public static List<Descriptor> GetPlantEffectDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Growing>() == null)
		{
			return list;
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(go, false);
		List<Descriptor> list2 = new List<Descriptor>();
		list2.AddRange(GameUtil.GetEffectDescriptors(allDescriptors));
		if (list2.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTEFFECTS, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTEFFECTS, Descriptor.DescriptorType.Effect);
			list.Add(item);
			list.AddRange(list2);
		}
		return list;
	}

	// Token: 0x0600657C RID: 25980 RVA: 0x002CB734 File Offset: 0x002C9934
	public static string GetGameObjectEffectsTooltipString(GameObject go)
	{
		string text = "";
		List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(go, false);
		if (gameObjectEffects.Count > 0)
		{
			text = text + UI.BUILDINGEFFECTS.OPERATIONEFFECTS + "\n";
		}
		foreach (Descriptor descriptor in gameObjectEffects)
		{
			text = text + descriptor.IndentedText() + "\n";
		}
		return text;
	}

	// Token: 0x0600657D RID: 25981 RVA: 0x002CB7BC File Offset: 0x002C99BC
	public static List<Descriptor> GetEquipmentEffects(EquipmentDef def)
	{
		global::Debug.Assert(def != null);
		List<Descriptor> list = new List<Descriptor>();
		List<AttributeModifier> attributeModifiers = def.AttributeModifiers;
		if (attributeModifiers != null)
		{
			foreach (AttributeModifier attributeModifier in attributeModifiers)
			{
				string name = Db.Get().Attributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				string newValue = (attributeModifier.Value >= 0f) ? "produced" : "consumed";
				string text = UI.GAMEOBJECTEFFECTS.EQUIPMENT_MODS.text.Replace("{Attribute}", name).Replace("{Style}", newValue).Replace("{Value}", formattedString);
				list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
			}
		}
		return list;
	}

	// Token: 0x0600657E RID: 25982 RVA: 0x002CB8AC File Offset: 0x002C9AAC
	public static string GetRecipeDescription(Recipe recipe)
	{
		string text = null;
		if (recipe != null)
		{
			text = recipe.recipeDescription;
		}
		if (text == null)
		{
			text = RESEARCH.TYPES.MISSINGRECIPEDESC;
			global::Debug.LogWarning("Missing recipeDescription");
		}
		return text;
	}

	// Token: 0x0600657F RID: 25983 RVA: 0x000E23C7 File Offset: 0x000E05C7
	public static int GetCurrentCycle()
	{
		return GameClock.Instance.GetCycle() + 1;
	}

	// Token: 0x06006580 RID: 25984 RVA: 0x000E23D5 File Offset: 0x000E05D5
	public static float GetCurrentTimeInCycles()
	{
		return GameClock.Instance.GetTimeInCycles() + 1f;
	}

	// Token: 0x06006581 RID: 25985 RVA: 0x002CB8E0 File Offset: 0x002C9AE0
	public static GameObject GetActiveTelepad()
	{
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.activeWorldId);
		if (telepad == null)
		{
			telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		}
		return telepad;
	}

	// Token: 0x06006582 RID: 25986 RVA: 0x002CB91C File Offset: 0x002C9B1C
	public static GameObject GetTelepad(int worldId)
	{
		if (Components.Telepads.Count > 0)
		{
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				if (Components.Telepads[i].GetMyWorldId() == worldId)
				{
					return Components.Telepads[i].gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x06006583 RID: 25987 RVA: 0x000E23E7 File Offset: 0x000E05E7
	public static GameObject KInstantiate(GameObject original, Vector3 position, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original, position, sceneLayer, null, name, gameLayer);
	}

	// Token: 0x06006584 RID: 25988 RVA: 0x000E23F5 File Offset: 0x000E05F5
	public static GameObject KInstantiate(GameObject original, Vector3 position, Grid.SceneLayer sceneLayer, GameObject parent, string name = null, int gameLayer = 0)
	{
		position.z = Grid.GetLayerZ(sceneLayer);
		return Util.KInstantiate(original, position, Quaternion.identity, parent, name, true, gameLayer);
	}

	// Token: 0x06006585 RID: 25989 RVA: 0x000E2416 File Offset: 0x000E0616
	public static GameObject KInstantiate(GameObject original, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original, Vector3.zero, sceneLayer, name, gameLayer);
	}

	// Token: 0x06006586 RID: 25990 RVA: 0x000E2426 File Offset: 0x000E0626
	public static GameObject KInstantiate(Component original, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original.gameObject, Vector3.zero, sceneLayer, name, gameLayer);
	}

	// Token: 0x06006587 RID: 25991 RVA: 0x002CB970 File Offset: 0x002C9B70
	public unsafe static void IsEmissionBlocked(int cell, out bool all_not_gaseous, out bool all_over_pressure)
	{
		int* ptr = stackalloc int[(UIntPtr)16];
		*ptr = Grid.CellBelow(cell);
		ptr[1] = Grid.CellLeft(cell);
		ptr[2] = Grid.CellRight(cell);
		ptr[3] = Grid.CellAbove(cell);
		all_not_gaseous = true;
		all_over_pressure = true;
		for (int i = 0; i < 4; i++)
		{
			int num = ptr[i];
			if (Grid.IsValidCell(num))
			{
				Element element = Grid.Element[num];
				all_not_gaseous = (all_not_gaseous && !element.IsGas && !element.IsVacuum);
				all_over_pressure = (all_over_pressure && ((!element.IsGas && !element.IsVacuum) || Grid.Mass[num] >= 1.8f));
			}
		}
	}

	// Token: 0x06006588 RID: 25992 RVA: 0x002CBA28 File Offset: 0x002C9C28
	public static float GetDecorAtCell(int cell)
	{
		float num = 0f;
		if (!Grid.Solid[cell])
		{
			num = Grid.Decor[cell];
			num += (float)DecorProvider.GetLightDecorBonus(cell);
		}
		return num;
	}

	// Token: 0x06006589 RID: 25993 RVA: 0x002CBA5C File Offset: 0x002C9C5C
	public static string GetUnitTypeMassOrUnit(GameObject go)
	{
		string result = UI.UNITSUFFIXES.UNITS;
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null)
		{
			result = (component.Tags.Contains(GameTags.Seed) ? UI.UNITSUFFIXES.UNITS : UI.UNITSUFFIXES.MASS.KILOGRAM);
		}
		return result;
	}

	// Token: 0x0600658A RID: 25994 RVA: 0x002CBAAC File Offset: 0x002C9CAC
	public static string GetKeywordStyle(Tag tag)
	{
		Element element = ElementLoader.GetElement(tag);
		string result;
		if (element != null)
		{
			result = GameUtil.GetKeywordStyle(element);
		}
		else if (GameUtil.foodTags.Contains(tag))
		{
			result = "food";
		}
		else if (GameUtil.solidTags.Contains(tag))
		{
			result = "solid";
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x0600658B RID: 25995 RVA: 0x002CBAFC File Offset: 0x002C9CFC
	public static string GetKeywordStyle(SimHashes hash)
	{
		Element element = ElementLoader.FindElementByHash(hash);
		if (element != null)
		{
			return GameUtil.GetKeywordStyle(element);
		}
		return null;
	}

	// Token: 0x0600658C RID: 25996 RVA: 0x002CBB1C File Offset: 0x002C9D1C
	public static string GetKeywordStyle(Element element)
	{
		if (element.id == SimHashes.Oxygen)
		{
			return "oxygen";
		}
		if (element.IsSolid)
		{
			return "solid";
		}
		if (element.IsLiquid)
		{
			return "liquid";
		}
		if (element.IsGas)
		{
			return "gas";
		}
		if (element.IsVacuum)
		{
			return "vacuum";
		}
		return null;
	}

	// Token: 0x0600658D RID: 25997 RVA: 0x002CBB78 File Offset: 0x002C9D78
	public static string GetKeywordStyle(GameObject go)
	{
		string result = "";
		UnityEngine.Object component = go.GetComponent<Edible>();
		Equippable component2 = go.GetComponent<Equippable>();
		MedicinalPill component3 = go.GetComponent<MedicinalPill>();
		ResearchPointObject component4 = go.GetComponent<ResearchPointObject>();
		if (component != null)
		{
			result = "food";
		}
		else if (component2 != null)
		{
			result = "equipment";
		}
		else if (component3 != null)
		{
			result = "medicine";
		}
		else if (component4 != null)
		{
			result = "research";
		}
		return result;
	}

	// Token: 0x0600658E RID: 25998 RVA: 0x002CBBE8 File Offset: 0x002C9DE8
	public static Sprite GetBiomeSprite(string id)
	{
		string text = "biomeIcon" + char.ToUpper(id[0]).ToString() + id.Substring(1).ToLower();
		Sprite sprite = Assets.GetSprite(text);
		if (sprite != null)
		{
			return new global::Tuple<Sprite, Color>(sprite, Color.white).first;
		}
		global::Debug.LogWarning("Missing codex biome icon: " + text);
		return null;
	}

	// Token: 0x0600658F RID: 25999 RVA: 0x002CBC58 File Offset: 0x002C9E58
	public static string GenerateRandomDuplicantName()
	{
		string text = "";
		string text2 = "";
		bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
		List<string> list = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.NB)));
		list.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.FEMALE)));
		string random = list.GetRandom<string>();
		if (UnityEngine.Random.Range(0f, 1f) > 0.7f)
		{
			List<string> list2 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.NB)));
			list2.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.FEMALE)));
			text = list2.GetRandom<string>();
		}
		if (!string.IsNullOrEmpty(text))
		{
			text += " ";
		}
		if (UnityEngine.Random.Range(0f, 1f) >= 0.9f)
		{
			List<string> list3 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.NB)));
			list3.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.FEMALE)));
			text2 = list3.GetRandom<string>();
		}
		if (!string.IsNullOrEmpty(text2))
		{
			text2 = " " + text2;
		}
		return text + random + text2;
	}

	// Token: 0x06006590 RID: 26000 RVA: 0x002CBDC0 File Offset: 0x002C9FC0
	public static string GenerateRandomLaunchPadName()
	{
		return NAMEGEN.LAUNCHPAD.FORMAT.Replace("{Name}", UnityEngine.Random.Range(1, 1000).ToString());
	}

	// Token: 0x06006591 RID: 26001 RVA: 0x002CBDF0 File Offset: 0x002C9FF0
	public static string GenerateRandomRocketName()
	{
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		int num = 1;
		int num2 = 2;
		int num3 = 4;
		string random = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.NOUN))).GetRandom<string>();
		int num4 = 0;
		if (UnityEngine.Random.value > 0.7f)
		{
			newValue = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.PREFIX))).GetRandom<string>();
			num4 |= num;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			newValue2 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.ADJECTIVE))).GetRandom<string>();
			num4 |= num2;
		}
		if (UnityEngine.Random.value > 0.1f)
		{
			newValue3 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.SUFFIX))).GetRandom<string>();
			num4 |= num3;
		}
		string text;
		if (num4 == (num | num2 | num3))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN_SUFFIX;
		}
		else if (num4 == (num2 | num3))
		{
			text = NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN_SUFFIX;
		}
		else if (num4 == (num | num3))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_NOUN_SUFFIX;
		}
		else if (num4 == num3)
		{
			text = NAMEGEN.ROCKET.FMT_NOUN_SUFFIX;
		}
		else if (num4 == (num | num2))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN;
		}
		else if (num4 == num)
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_NOUN;
		}
		else if (num4 == num2)
		{
			text = NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN;
		}
		else
		{
			text = NAMEGEN.ROCKET.FMT_NOUN;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Rocket name bits:",
			Convert.ToString(num4, 2)
		});
		return text.Replace("{Prefix}", newValue).Replace("{Adjective}", newValue2).Replace("{Noun}", random).Replace("{Suffix}", newValue3);
	}

	// Token: 0x06006592 RID: 26002 RVA: 0x002CBFB8 File Offset: 0x002CA1B8
	public static string GenerateRandomWorldName(string[] nameTables)
	{
		if (nameTables == null)
		{
			global::Debug.LogWarning("No name tables provided to generate world name. Using GENERIC");
			nameTables = new string[]
			{
				"GENERIC"
			};
		}
		string text = "";
		foreach (string text2 in nameTables)
		{
			text += Strings.Get("STRINGS.NAMEGEN.WORLD.ROOTS." + text2.ToUpper());
		}
		string text3 = GameUtil.RandomValueFromSeparatedString(text, "\n");
		if (string.IsNullOrEmpty(text3))
		{
			text3 = GameUtil.RandomValueFromSeparatedString(Strings.Get(NAMEGEN.WORLD.ROOTS.GENERIC), "\n");
		}
		string str = GameUtil.RandomValueFromSeparatedString(NAMEGEN.WORLD.SUFFIXES.GENERICLIST, "\n");
		return text3 + str;
	}

	// Token: 0x06006593 RID: 26003 RVA: 0x002CC074 File Offset: 0x002CA274
	public static float GetThermalComfort(Tag duplicantType, int cell, float tolerance)
	{
		DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(duplicantType);
		float num = 0f;
		Element element = ElementLoader.FindElementByHash(SimHashes.Creature);
		if (Grid.Element[cell].thermalConductivity != 0f)
		{
			num = SimUtil.CalculateEnergyFlowCreatures(cell, statsFor.Temperature.Internal.IDEAL, element.specificHeatCapacity, element.thermalConductivity, statsFor.Temperature.SURFACE_AREA, statsFor.Temperature.SKIN_THICKNESS + 0.0025f);
		}
		num -= tolerance;
		return num * 1000f;
	}

	// Token: 0x06006594 RID: 26004 RVA: 0x002CC0FC File Offset: 0x002CA2FC
	public static void FocusCamera(Transform target, bool select = true)
	{
		GameUtil.FocusCamera(target.GetPosition());
		if (select)
		{
			KSelectable component = target.GetComponent<KSelectable>();
			SelectTool.Instance.Select(component, false);
		}
	}

	// Token: 0x06006595 RID: 26005 RVA: 0x000E243B File Offset: 0x000E063B
	public static void FocusCamera(Vector3 position)
	{
		CameraController.Instance.CameraGoTo(position, 2f, true);
	}

	// Token: 0x06006596 RID: 26006 RVA: 0x000E244E File Offset: 0x000E064E
	public static void FocusCamera(int cell)
	{
		GameUtil.FocusCamera(Grid.CellToPos(cell));
	}

	// Token: 0x06006597 RID: 26007 RVA: 0x002CC12C File Offset: 0x002CA32C
	public static string RandomValueFromSeparatedString(string source, string separator = "\n")
	{
		int num = 0;
		int num2 = 0;
		for (;;)
		{
			num = source.IndexOf(separator, num);
			if (num == -1)
			{
				break;
			}
			num += separator.Length;
			num2++;
		}
		if (num2 == 0)
		{
			return "";
		}
		int num3 = UnityEngine.Random.Range(0, num2);
		num = 0;
		for (int i = 0; i < num3; i++)
		{
			num = source.IndexOf(separator, num) + separator.Length;
		}
		int num4 = source.IndexOf(separator, num);
		return source.Substring(num, (num4 == -1) ? (source.Length - num) : (num4 - num));
	}

	// Token: 0x06006598 RID: 26008 RVA: 0x002CC1B0 File Offset: 0x002CA3B0
	public static string GetFormattedDiseaseName(byte idx, bool color = false)
	{
		Disease disease = Db.Get().Diseases[(int)idx];
		if (color)
		{
			return string.Format(UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT, disease.Name, GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName)));
		}
		return string.Format(UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT_NO_COLOR, disease.Name);
	}

	// Token: 0x06006599 RID: 26009 RVA: 0x002CC218 File Offset: 0x002CA418
	public static string GetFormattedDisease(byte idx, int units, bool color = false)
	{
		if (idx == 255 || units <= 0)
		{
			return UI.OVERLAYS.DISEASE.NO_DISEASE;
		}
		Disease disease = Db.Get().Diseases[(int)idx];
		if (color)
		{
			return string.Format(UI.OVERLAYS.DISEASE.DISEASE_FORMAT, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None), GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName)));
		}
		return string.Format(UI.OVERLAYS.DISEASE.DISEASE_FORMAT_NO_COLOR, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600659A RID: 26010 RVA: 0x000E245B File Offset: 0x000E065B
	public static string GetFormattedDiseaseAmount(int units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		GameUtil.ApplyTimeSlice(units, timeSlice);
		return GameUtil.AddTimeSliceText(units.ToString("#,##0") + UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
	}

	// Token: 0x0600659B RID: 26011 RVA: 0x000E2486 File Offset: 0x000E0686
	public static string GetFormattedDiseaseAmount(long units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		GameUtil.ApplyTimeSlice((float)units, timeSlice);
		return GameUtil.AddTimeSliceText(units.ToString("#,##0") + UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
	}

	// Token: 0x0600659C RID: 26012 RVA: 0x000E24B2 File Offset: 0x000E06B2
	public static string ColourizeString(Color32 colour, string str)
	{
		return string.Format("<color=#{0}>{1}</color>", GameUtil.ColourToHex(colour), str);
	}

	// Token: 0x0600659D RID: 26013 RVA: 0x002CC2A4 File Offset: 0x002CA4A4
	public static string ColourToHex(Color32 colour)
	{
		return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
		{
			colour.r,
			colour.g,
			colour.b,
			colour.a
		});
	}

	// Token: 0x0600659E RID: 26014 RVA: 0x002CC2FC File Offset: 0x002CA4FC
	public static string GetFormattedDecor(float value, bool enforce_max = false)
	{
		string arg = "";
		LocString loc_string = (value > DecorMonitor.MAXIMUM_DECOR_VALUE && enforce_max) ? UI.OVERLAYS.DECOR.MAXIMUM_DECOR : UI.OVERLAYS.DECOR.VALUE;
		if (enforce_max)
		{
			value = Math.Min(value, DecorMonitor.MAXIMUM_DECOR_VALUE);
		}
		if (value > 0f)
		{
			arg = "+";
		}
		else if (value >= 0f)
		{
			loc_string = UI.OVERLAYS.DECOR.VALUE_ZERO;
		}
		return string.Format(loc_string, arg, value);
	}

	// Token: 0x0600659F RID: 26015 RVA: 0x002CC368 File Offset: 0x002CA568
	public static Color GetDecorColourFromValue(int decor)
	{
		Color result = Color.black;
		float num = (float)decor / 100f;
		if (num > 0f)
		{
			result = Color.Lerp(new Color(0.15f, 0f, 0f), new Color(0f, 1f, 0f), Mathf.Abs(num));
		}
		else
		{
			result = Color.Lerp(new Color(0.15f, 0f, 0f), new Color(1f, 0f, 0f), Mathf.Abs(num));
		}
		return result;
	}

	// Token: 0x060065A0 RID: 26016 RVA: 0x002CC3F8 File Offset: 0x002CA5F8
	public static List<Descriptor> GetMaterialDescriptors(Element element)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (element.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in element.attributeModifiers)
			{
				string txt = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
				string tooltip = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
				Descriptor item = default(Descriptor);
				item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
				item.IncreaseIndent();
				list.Add(item);
			}
		}
		list.AddRange(GameUtil.GetSignificantMaterialPropertyDescriptors(element));
		return list;
	}

	// Token: 0x060065A1 RID: 26017 RVA: 0x002CC4F4 File Offset: 0x002CA6F4
	public static string GetMaterialTooltips(Element element)
	{
		string text = element.tag.ProperName();
		foreach (AttributeModifier attributeModifier in element.attributeModifiers)
		{
			string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
			string formattedString = attributeModifier.GetFormattedString();
			text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
		}
		text += GameUtil.GetSignificantMaterialPropertyTooltips(element);
		return text;
	}

	// Token: 0x060065A2 RID: 26018 RVA: 0x002CC59C File Offset: 0x002CA79C
	public static string GetSignificantMaterialPropertyTooltips(Element element)
	{
		string text = "";
		List<Descriptor> significantMaterialPropertyDescriptors = GameUtil.GetSignificantMaterialPropertyDescriptors(element);
		if (significantMaterialPropertyDescriptors.Count > 0)
		{
			text += "\n";
			for (int i = 0; i < significantMaterialPropertyDescriptors.Count; i++)
			{
				text = text + "    • " + Util.StripTextFormatting(significantMaterialPropertyDescriptors[i].text) + "\n";
			}
		}
		return text;
	}

	// Token: 0x060065A3 RID: 26019 RVA: 0x002CC600 File Offset: 0x002CA800
	public static List<Descriptor> GetSignificantMaterialPropertyDescriptors(Element element)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (element.thermalConductivity > 10f)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(ELEMENTS.MATERIAL_MODIFIERS.HIGH_THERMAL_CONDUCTIVITY, GameUtil.GetThermalConductivityString(element, false, false)), string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_THERMAL_CONDUCTIVITY, element.name, element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
			item.IncreaseIndent();
			list.Add(item);
		}
		if (element.thermalConductivity < 1f)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(string.Format(ELEMENTS.MATERIAL_MODIFIERS.LOW_THERMAL_CONDUCTIVITY, GameUtil.GetThermalConductivityString(element, false, false)), string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_THERMAL_CONDUCTIVITY, element.name, element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		if (element.specificHeatCapacity <= 0.2f)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.LOW_SPECIFIC_HEAT_CAPACITY, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_SPECIFIC_HEAT_CAPACITY, element.name, element.specificHeatCapacity * 1f), Descriptor.DescriptorType.Effect);
			item3.IncreaseIndent();
			list.Add(item3);
		}
		if (element.specificHeatCapacity >= 1f)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.HIGH_SPECIFIC_HEAT_CAPACITY, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_SPECIFIC_HEAT_CAPACITY, element.name, element.specificHeatCapacity * 1f), Descriptor.DescriptorType.Effect);
			item4.IncreaseIndent();
			list.Add(item4);
		}
		if (Sim.IsRadiationEnabled() && element.radiationAbsorptionFactor >= 0.8f)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EXCELLENT_RADIATION_SHIELD, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EXCELLENT_RADIATION_SHIELD, element.name, element.radiationAbsorptionFactor), Descriptor.DescriptorType.Effect);
			item5.IncreaseIndent();
			list.Add(item5);
		}
		return list;
	}

	// Token: 0x060065A4 RID: 26020 RVA: 0x000E24C5 File Offset: 0x000E06C5
	public static int NaturalBuildingCell(this KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x060065A5 RID: 26021 RVA: 0x002CC7FC File Offset: 0x002CA9FC
	public static List<Descriptor> GetMaterialDescriptors(Tag tag)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.GetElement(tag);
		if (element != null)
		{
			if (element.attributeModifiers.Count > 0)
			{
				foreach (AttributeModifier attributeModifier in element.attributeModifiers)
				{
					string txt = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
					string tooltip = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			list.AddRange(GameUtil.GetSignificantMaterialPropertyDescriptors(element));
		}
		else
		{
			GameObject gameObject = Assets.TryGetPrefab(tag);
			if (gameObject != null)
			{
				PrefabAttributeModifiers component = gameObject.GetComponent<PrefabAttributeModifiers>();
				if (component != null)
				{
					foreach (AttributeModifier attributeModifier2 in component.descriptors)
					{
						string txt2 = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier2.AttributeId.ToUpper())), attributeModifier2.GetFormattedString());
						string tooltip2 = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier2.AttributeId.ToUpper())), attributeModifier2.GetFormattedString());
						Descriptor item2 = default(Descriptor);
						item2.SetupDescriptor(txt2, tooltip2, Descriptor.DescriptorType.Effect);
						item2.IncreaseIndent();
						list.Add(item2);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x060065A6 RID: 26022 RVA: 0x002CCA04 File Offset: 0x002CAC04
	public static string GetMaterialTooltips(Tag tag)
	{
		string text = tag.ProperName();
		Element element = ElementLoader.GetElement(tag);
		if (element != null)
		{
			foreach (AttributeModifier attributeModifier in element.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
			text += GameUtil.GetSignificantMaterialPropertyTooltips(element);
		}
		else
		{
			GameObject gameObject = Assets.TryGetPrefab(tag);
			if (gameObject != null)
			{
				PrefabAttributeModifiers component = gameObject.GetComponent<PrefabAttributeModifiers>();
				if (component != null)
				{
					foreach (AttributeModifier attributeModifier2 in component.descriptors)
					{
						string name2 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId).Name;
						string formattedString2 = attributeModifier2.GetFormattedString();
						text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name2, formattedString2);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x060065A7 RID: 26023 RVA: 0x002CCB64 File Offset: 0x002CAD64
	public static bool AreChoresUIMergeable(Chore.Precondition.Context choreA, Chore.Precondition.Context choreB)
	{
		if (choreA.chore.target.isNull || choreB.chore.target.isNull)
		{
			return false;
		}
		ChoreType choreType = choreB.chore.choreType;
		ChoreType choreType2 = choreA.chore.choreType;
		return (choreA.chore.choreType == choreB.chore.choreType && choreA.chore.target.GetComponent<KPrefabID>().PrefabTag == choreB.chore.target.GetComponent<KPrefabID>().PrefabTag) || (choreA.chore.choreType == Db.Get().ChoreTypes.Dig && choreB.chore.choreType == Db.Get().ChoreTypes.Dig) || (choreA.chore.choreType == Db.Get().ChoreTypes.Relax && choreB.chore.choreType == Db.Get().ChoreTypes.Relax) || ((choreType2 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType2 == Db.Get().ChoreTypes.ReturnSuitUrgent) && (choreType == Db.Get().ChoreTypes.ReturnSuitIdle || choreType == Db.Get().ChoreTypes.ReturnSuitUrgent)) || (choreA.chore.target.gameObject == choreB.chore.target.gameObject && choreA.chore.choreType == choreB.chore.choreType);
	}

	// Token: 0x060065A8 RID: 26024 RVA: 0x002CCCFC File Offset: 0x002CAEFC
	public static string GetChoreName(Chore chore, object choreData)
	{
		string result = "";
		if (chore.choreType == Db.Get().ChoreTypes.Fetch || chore.choreType == Db.Get().ChoreTypes.MachineFetch || chore.choreType == Db.Get().ChoreTypes.FabricateFetch || chore.choreType == Db.Get().ChoreTypes.FetchCritical || chore.choreType == Db.Get().ChoreTypes.PowerFetch)
		{
			result = chore.GetReportName(chore.gameObject.GetProperName());
		}
		else if (chore.choreType == Db.Get().ChoreTypes.StorageFetch || chore.choreType == Db.Get().ChoreTypes.FoodFetch)
		{
			FetchChore fetchChore = chore as FetchChore;
			FetchAreaChore fetchAreaChore = chore as FetchAreaChore;
			if (fetchAreaChore != null)
			{
				GameObject getFetchTarget = fetchAreaChore.GetFetchTarget;
				KMonoBehaviour kmonoBehaviour = choreData as KMonoBehaviour;
				if (getFetchTarget != null)
				{
					result = chore.GetReportName(getFetchTarget.GetProperName());
				}
				else if (kmonoBehaviour != null)
				{
					result = chore.GetReportName(kmonoBehaviour.GetProperName());
				}
				else
				{
					result = chore.GetReportName(null);
				}
			}
			else if (fetchChore != null)
			{
				Pickupable fetchTarget = fetchChore.fetchTarget;
				KMonoBehaviour kmonoBehaviour2 = choreData as KMonoBehaviour;
				if (fetchTarget != null)
				{
					result = chore.GetReportName(fetchTarget.GetProperName());
				}
				else if (kmonoBehaviour2 != null)
				{
					result = chore.GetReportName(kmonoBehaviour2.GetProperName());
				}
				else
				{
					result = chore.GetReportName(null);
				}
			}
		}
		else
		{
			result = chore.GetReportName(null);
		}
		return result;
	}

	// Token: 0x060065A9 RID: 26025 RVA: 0x002CCE80 File Offset: 0x002CB080
	public static string ChoreGroupsForChoreType(ChoreType choreType)
	{
		if (choreType.groups == null || choreType.groups.Length == 0)
		{
			return null;
		}
		string text = "";
		for (int i = 0; i < choreType.groups.Length; i++)
		{
			if (i != 0)
			{
				text += UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_GROUP_SEPARATOR;
			}
			text += choreType.groups[i].Name;
		}
		return text;
	}

	// Token: 0x060065AA RID: 26026 RVA: 0x000E24D7 File Offset: 0x000E06D7
	public static bool IsCapturingTimeLapse()
	{
		return Game.Instance != null && Game.Instance.timelapser != null && Game.Instance.timelapser.CapturingTimelapseScreenshot;
	}

	// Token: 0x060065AB RID: 26027 RVA: 0x002CCEE4 File Offset: 0x002CB0E4
	public static ExposureType GetExposureTypeForDisease(Disease disease)
	{
		for (int i = 0; i < GERM_EXPOSURE.TYPES.Length; i++)
		{
			if (disease.id == GERM_EXPOSURE.TYPES[i].germ_id)
			{
				return GERM_EXPOSURE.TYPES[i];
			}
		}
		return null;
	}

	// Token: 0x060065AC RID: 26028 RVA: 0x002CCF2C File Offset: 0x002CB12C
	public static Sickness GetSicknessForDisease(Disease disease)
	{
		int i = 0;
		while (i < GERM_EXPOSURE.TYPES.Length)
		{
			if (disease.id == GERM_EXPOSURE.TYPES[i].germ_id)
			{
				if (GERM_EXPOSURE.TYPES[i].sickness_id == null)
				{
					return null;
				}
				return Db.Get().Sicknesses.Get(GERM_EXPOSURE.TYPES[i].sickness_id);
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	// Token: 0x060065AD RID: 26029 RVA: 0x000E2509 File Offset: 0x000E0709
	public static void SubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler, bool triggerImmediately) where T : KMonoBehaviour
	{
		if (triggerImmediately)
		{
			handler.Trigger(target.gameObject, new TagChangedEventData(Tag.Invalid, false));
		}
		target.Subscribe<T>(-1582839653, handler);
	}

	// Token: 0x060065AE RID: 26030 RVA: 0x000E2541 File Offset: 0x000E0741
	public static void UnsubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler) where T : KMonoBehaviour
	{
		target.Unsubscribe<T>(-1582839653, handler, false);
	}

	// Token: 0x060065AF RID: 26031 RVA: 0x000E2555 File Offset: 0x000E0755
	public static EventSystem.IntraObjectHandler<T> CreateHasTagHandler<T>(Tag tag, Action<T, object> callback) where T : KMonoBehaviour
	{
		return new EventSystem.IntraObjectHandler<T>(delegate(T component, object data)
		{
			TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
			if (tagChangedEventData.tag == Tag.Invalid)
			{
				KPrefabID component2 = component.GetComponent<KPrefabID>();
				tagChangedEventData = new TagChangedEventData(tag, component2.HasTag(tag));
			}
			if (tagChangedEventData.tag == tag && tagChangedEventData.added)
			{
				callback(component, data);
			}
		});
	}

	// Token: 0x04004C00 RID: 19456
	public static GameUtil.TemperatureUnit temperatureUnit;

	// Token: 0x04004C01 RID: 19457
	public static GameUtil.MassUnit massUnit;

	// Token: 0x04004C02 RID: 19458
	private static string[] adjectives;

	// Token: 0x04004C03 RID: 19459
	public static ThreadLocal<Queue<GameUtil.FloodFillInfo>> FloodFillNext = new ThreadLocal<Queue<GameUtil.FloodFillInfo>>(() => new Queue<GameUtil.FloodFillInfo>());

	// Token: 0x04004C04 RID: 19460
	public static ThreadLocal<HashSet<int>> FloodFillVisited = new ThreadLocal<HashSet<int>>(() => new HashSet<int>());

	// Token: 0x04004C05 RID: 19461
	public static ThreadLocal<List<int>> FloodFillNeighbors = new ThreadLocal<List<int>>(() => new List<int>(4)
	{
		-1,
		-1,
		-1,
		-1
	});

	// Token: 0x04004C06 RID: 19462
	public static TagSet foodTags = new TagSet(new string[]
	{
		"BasicPlantFood",
		"MushBar",
		"ColdWheatSeed",
		"ColdWheatSeed",
		"SpiceNut",
		"PrickleFruit",
		"Meat",
		"Mushroom",
		"ColdWheat",
		GameTags.Compostable.Name
	});

	// Token: 0x04004C07 RID: 19463
	public static TagSet solidTags = new TagSet(new string[]
	{
		"Filter",
		"Coal",
		"BasicFabric",
		"SwampLilyFlower",
		"RefinedMetal"
	});

	// Token: 0x02001345 RID: 4933
	public enum UnitClass
	{
		// Token: 0x04004C09 RID: 19465
		SimpleFloat,
		// Token: 0x04004C0A RID: 19466
		SimpleInteger,
		// Token: 0x04004C0B RID: 19467
		Temperature,
		// Token: 0x04004C0C RID: 19468
		Mass,
		// Token: 0x04004C0D RID: 19469
		Calories,
		// Token: 0x04004C0E RID: 19470
		Percent,
		// Token: 0x04004C0F RID: 19471
		Distance,
		// Token: 0x04004C10 RID: 19472
		Disease,
		// Token: 0x04004C11 RID: 19473
		Radiation,
		// Token: 0x04004C12 RID: 19474
		Energy,
		// Token: 0x04004C13 RID: 19475
		Power,
		// Token: 0x04004C14 RID: 19476
		Lux,
		// Token: 0x04004C15 RID: 19477
		Time,
		// Token: 0x04004C16 RID: 19478
		Seconds,
		// Token: 0x04004C17 RID: 19479
		Cycles
	}

	// Token: 0x02001346 RID: 4934
	public enum TemperatureUnit
	{
		// Token: 0x04004C19 RID: 19481
		Celsius,
		// Token: 0x04004C1A RID: 19482
		Fahrenheit,
		// Token: 0x04004C1B RID: 19483
		Kelvin
	}

	// Token: 0x02001347 RID: 4935
	public enum MassUnit
	{
		// Token: 0x04004C1D RID: 19485
		Kilograms,
		// Token: 0x04004C1E RID: 19486
		Pounds
	}

	// Token: 0x02001348 RID: 4936
	public enum MetricMassFormat
	{
		// Token: 0x04004C20 RID: 19488
		UseThreshold,
		// Token: 0x04004C21 RID: 19489
		Kilogram,
		// Token: 0x04004C22 RID: 19490
		Gram,
		// Token: 0x04004C23 RID: 19491
		Tonne
	}

	// Token: 0x02001349 RID: 4937
	public enum TemperatureInterpretation
	{
		// Token: 0x04004C25 RID: 19493
		Absolute,
		// Token: 0x04004C26 RID: 19494
		Relative
	}

	// Token: 0x0200134A RID: 4938
	public enum TimeSlice
	{
		// Token: 0x04004C28 RID: 19496
		None,
		// Token: 0x04004C29 RID: 19497
		ModifyOnly,
		// Token: 0x04004C2A RID: 19498
		PerSecond,
		// Token: 0x04004C2B RID: 19499
		PerCycle
	}

	// Token: 0x0200134B RID: 4939
	public enum MeasureUnit
	{
		// Token: 0x04004C2D RID: 19501
		mass,
		// Token: 0x04004C2E RID: 19502
		kcal,
		// Token: 0x04004C2F RID: 19503
		quantity
	}

	// Token: 0x0200134C RID: 4940
	public enum IdentityDescriptorTense
	{
		// Token: 0x04004C31 RID: 19505
		Normal,
		// Token: 0x04004C32 RID: 19506
		Possessive,
		// Token: 0x04004C33 RID: 19507
		Plural
	}

	// Token: 0x0200134D RID: 4941
	public enum WattageFormatterUnit
	{
		// Token: 0x04004C35 RID: 19509
		Watts,
		// Token: 0x04004C36 RID: 19510
		Kilowatts,
		// Token: 0x04004C37 RID: 19511
		Automatic
	}

	// Token: 0x0200134E RID: 4942
	public enum HeatEnergyFormatterUnit
	{
		// Token: 0x04004C39 RID: 19513
		DTU_S,
		// Token: 0x04004C3A RID: 19514
		KDTU_S,
		// Token: 0x04004C3B RID: 19515
		Automatic
	}

	// Token: 0x0200134F RID: 4943
	public struct FloodFillInfo
	{
		// Token: 0x04004C3C RID: 19516
		public int cell;

		// Token: 0x04004C3D RID: 19517
		public int depth;
	}

	// Token: 0x02001350 RID: 4944
	public static class Hardness
	{
		// Token: 0x04004C3E RID: 19518
		public const int VERY_SOFT = 0;

		// Token: 0x04004C3F RID: 19519
		public const int SOFT = 10;

		// Token: 0x04004C40 RID: 19520
		public const int FIRM = 25;

		// Token: 0x04004C41 RID: 19521
		public const int VERY_FIRM = 50;

		// Token: 0x04004C42 RID: 19522
		public const int NEARLY_IMPENETRABLE = 150;

		// Token: 0x04004C43 RID: 19523
		public const int SUPER_DUPER_HARD = 200;

		// Token: 0x04004C44 RID: 19524
		public const int RADIOACTIVE_MATERIALS = 251;

		// Token: 0x04004C45 RID: 19525
		public const int IMPENETRABLE = 255;

		// Token: 0x04004C46 RID: 19526
		public static Color ImpenetrableColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x04004C47 RID: 19527
		public static Color nearlyImpenetrableColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x04004C48 RID: 19528
		public static Color veryFirmColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04004C49 RID: 19529
		public static Color firmColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x04004C4A RID: 19530
		public static Color softColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);

		// Token: 0x04004C4B RID: 19531
		public static Color verySoftColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);
	}

	// Token: 0x02001351 RID: 4945
	public static class GermResistanceValues
	{
		// Token: 0x04004C4C RID: 19532
		public const float MEDIUM = 2f;

		// Token: 0x04004C4D RID: 19533
		public const float LARGE = 5f;

		// Token: 0x04004C4E RID: 19534
		public static Color NegativeLargeColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x04004C4F RID: 19535
		public static Color NegativeMediumColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x04004C50 RID: 19536
		public static Color NegativeSmallColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04004C51 RID: 19537
		public static Color PositiveSmallColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x04004C52 RID: 19538
		public static Color PositiveMediumColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);

		// Token: 0x04004C53 RID: 19539
		public static Color PositiveLargeColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);
	}

	// Token: 0x02001352 RID: 4946
	public static class ThermalConductivityValues
	{
		// Token: 0x04004C54 RID: 19540
		public const float VERY_HIGH = 50f;

		// Token: 0x04004C55 RID: 19541
		public const float HIGH = 10f;

		// Token: 0x04004C56 RID: 19542
		public const float MEDIUM = 2f;

		// Token: 0x04004C57 RID: 19543
		public const float LOW = 1f;

		// Token: 0x04004C58 RID: 19544
		public static Color veryLowConductivityColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x04004C59 RID: 19545
		public static Color lowConductivityColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x04004C5A RID: 19546
		public static Color mediumConductivityColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04004C5B RID: 19547
		public static Color highConductivityColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x04004C5C RID: 19548
		public static Color veryHighConductivityColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);
	}

	// Token: 0x02001353 RID: 4947
	public static class BreathableValues
	{
		// Token: 0x04004C5D RID: 19549
		public static Color positiveColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);

		// Token: 0x04004C5E RID: 19550
		public static Color warningColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04004C5F RID: 19551
		public static Color negativeColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);
	}

	// Token: 0x02001354 RID: 4948
	public static class WireLoadValues
	{
		// Token: 0x04004C60 RID: 19552
		public static Color warningColor = new Color(0.9843137f, 0.6901961f, 0.23137255f);

		// Token: 0x04004C61 RID: 19553
		public static Color negativeColor = new Color(1f, 0.19215687f, 0.19215687f);
	}
}
