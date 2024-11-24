﻿using System;
using Klei.AI;

namespace Database
{
	public class Amounts : ResourceSet<Amount>
	{
		public void Load()
		{
			this.Stamina = this.CreateAmount("Stamina", 0f, 100f, false, Units.Flat, 0.35f, true, "STRINGS.DUPLICANTS", "ui_icon_stamina", "attribute_stamina", "mod_stamina");
			this.Stamina.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Calories = this.CreateAmount("Calories", 0f, 0f, false, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS", "ui_icon_calories", "attribute_calories", "mod_calories");
			this.Calories.SetDisplayer(new CaloriesDisplayer());
			this.Breath = this.CreateAmount("Breath", 0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_breath", null, "mod_breath");
			this.Breath.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Stress = this.CreateAmount("Stress", 0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_stress", "attribute_stress", "mod_stress");
			this.Stress.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Toxicity = this.CreateAmount("Toxicity", 0f, 100f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", null, null, null);
			this.Toxicity.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Bladder = this.CreateAmount("Bladder", 0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_bladder", null, "mod_bladder");
			this.Bladder.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Decor = this.CreateAmount("Decor", -1000f, 1000f, false, Units.Flat, 0.016666668f, true, "STRINGS.DUPLICANTS", "ui_icon_decor", null, "mod_decor");
			this.Decor.SetDisplayer(new DecorDisplayer());
			this.RadiationBalance = this.CreateAmount("RadiationBalance", 0f, 10000f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_radiation", null, "mod_health");
			this.RadiationBalance.SetDisplayer(new RadiationBalanceDisplayer());
			this.Temperature = this.CreateAmount("Temperature", 0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_temperature", null, null);
			this.Temperature.SetDisplayer(new DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
			this.CritterTemperature = this.CreateAmount("CritterTemperature", 0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.CREATURES", "ui_icon_temperature", null, null);
			this.CritterTemperature.SetDisplayer(new CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
			this.HitPoints = this.CreateAmount("HitPoints", 0f, 0f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_hitpoints", "attribute_hitpoints", "mod_health");
			this.HitPoints.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Possessive));
			this.AirPressure = this.CreateAmount("AirPressure", 0f, 1E+09f, false, Units.Flat, 0f, true, "STRINGS.CREATURES", null, null, null);
			this.AirPressure.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Maturity = this.CreateAmount("Maturity", 0f, 0f, true, Units.Flat, 0.0009166667f, true, "STRINGS.CREATURES", "ui_icon_maturity", null, null);
			this.Maturity.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Cycles, GameUtil.TimeSlice.None, null, GameUtil.IdentityDescriptorTense.Normal));
			this.OldAge = this.CreateAmount("OldAge", 0f, 0f, false, Units.Flat, 0f, false, "STRINGS.CREATURES", null, null, null);
			this.Fertilization = this.CreateAmount("Fertilization", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", null, null, null);
			this.Fertilization.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Fertility = this.CreateAmount("Fertility", 0f, 100f, true, Units.Flat, 0.008375f, true, "STRINGS.CREATURES", "ui_icon_fertility", null, null);
			this.Fertility.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Wildness = this.CreateAmount("Wildness", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_wildness", null, null);
			this.Wildness.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Incubation = this.CreateAmount("Incubation", 0f, 100f, true, Units.Flat, 0.01675f, true, "STRINGS.CREATURES", "ui_icon_incubation", null, null);
			this.Incubation.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Viability = this.CreateAmount("Viability", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_viability", null, null);
			this.Viability.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.PowerCharge = this.CreateAmount("PowerCharge", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", null, null, null);
			this.PowerCharge.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Age = this.CreateAmount("Age", 0f, 0f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_age", null, null);
			this.Age.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Irrigation = this.CreateAmount("Irrigation", 0f, 1f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", null, null, null);
			this.Irrigation.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.ImmuneLevel = this.CreateAmount("ImmuneLevel", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_immunelevel", "attribute_immunelevel", null);
			this.ImmuneLevel.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Rot = this.CreateAmount("Rot", 0f, 0f, false, Units.Flat, 0f, true, "STRINGS.CREATURES", null, null, null);
			this.Rot.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Illumination = this.CreateAmount("Illumination", 0f, 1f, false, Units.Flat, 0f, true, "STRINGS.CREATURES", null, null, null);
			this.Illumination.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None, null, GameUtil.IdentityDescriptorTense.Normal));
			this.ScaleGrowth = this.CreateAmount("ScaleGrowth", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_scale_growth", null, null);
			this.ScaleGrowth.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.MilkProduction = this.CreateAmount("MilkProduction", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_milk_production", null, null);
			this.MilkProduction.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.ElementGrowth = this.CreateAmount("ElementGrowth", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_scale_growth", null, null);
			this.ElementGrowth.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Beckoning = this.CreateAmount("Beckoning", 0f, 100f, true, Units.Flat, 100.5f, true, "STRINGS.CREATURES", "ui_icon_moo", null, null);
			this.Beckoning.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.InternalBattery = this.CreateAmount("InternalBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalChemicalBattery = this.CreateAmount("InternalChemicalBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalChemicalBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalBioBattery = this.CreateAmount("InternalBioBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalBioBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
		}

		public Amount CreateAmount(string id, float min, float max, bool show_max, Units units, float delta_threshold, bool show_in_ui, string string_root, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null)
		{
			string text = Strings.Get(string.Format("{1}.STATS.{0}.NAME", id.ToUpper(), string_root.ToUpper()));
			string description = Strings.Get(string.Format("{1}.STATS.{0}.TOOLTIP", id.ToUpper(), string_root.ToUpper()));
			Klei.AI.Attribute.Display show_in_ui2 = show_in_ui ? Klei.AI.Attribute.Display.Normal : Klei.AI.Attribute.Display.Never;
			string text2 = id + "Min";
			StringEntry stringEntry;
			string name = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.NAME", text2.ToUpper(), string_root)), out stringEntry) ? stringEntry.String : ("Minimum" + text);
			StringEntry stringEntry2;
			string attribute_description = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.DESC", text2.ToUpper(), string_root)), out stringEntry2) ? stringEntry2.String : ("Minimum" + text);
			Klei.AI.Attribute attribute = new Klei.AI.Attribute(id + "Min", name, "", attribute_description, min, show_in_ui2, false, null, null, uiFullColourSprite);
			string text3 = id + "Max";
			StringEntry stringEntry3;
			string name2 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.NAME", text3.ToUpper(), string_root)), out stringEntry3) ? stringEntry3.String : ("Maximum" + text);
			StringEntry stringEntry4;
			string attribute_description2 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.DESC", text3.ToUpper(), string_root)), out stringEntry4) ? stringEntry4.String : ("Maximum" + text);
			Klei.AI.Attribute attribute2 = new Klei.AI.Attribute(id + "Max", name2, "", attribute_description2, max, show_in_ui2, false, null, null, uiFullColourSprite);
			string text4 = id + "Delta";
			string name3 = Strings.Get(string.Format("{1}.ATTRIBUTES.{0}.NAME", text4.ToUpper(), string_root));
			string attribute_description3 = Strings.Get(string.Format("{1}.ATTRIBUTES.{0}.DESC", text4.ToUpper(), string_root));
			Klei.AI.Attribute attribute3 = new Klei.AI.Attribute(text4, name3, "", attribute_description3, 0f, Klei.AI.Attribute.Display.Normal, false, null, null, uiFullColourSprite);
			Amount amount = new Amount(id, text, description, attribute, attribute2, attribute3, show_max, units, delta_threshold, show_in_ui, uiSprite, thoughtSprite);
			Db.Get().Attributes.Add(attribute);
			Db.Get().Attributes.Add(attribute2);
			Db.Get().Attributes.Add(attribute3);
			base.Add(amount);
			return amount;
		}

		public Amount Stamina;

		public Amount Calories;

		public Amount ImmuneLevel;

		public Amount Breath;

		public Amount Stress;

		public Amount Toxicity;

		public Amount Bladder;

		public Amount Decor;

		public Amount RadiationBalance;

		public Amount Temperature;

		public Amount CritterTemperature;

		public Amount HitPoints;

		public Amount AirPressure;

		public Amount Maturity;

		public Amount OldAge;

		public Amount Age;

		public Amount Fertilization;

		public Amount Illumination;

		public Amount Irrigation;

		public Amount Fertility;

		public Amount Viability;

		public Amount PowerCharge;

		public Amount Wildness;

		public Amount Incubation;

		public Amount ScaleGrowth;

		public Amount ElementGrowth;

		public Amount Beckoning;

		public Amount MilkProduction;

		public Amount InternalBattery;

		public Amount InternalChemicalBattery;

		public Amount InternalBioBattery;

		public Amount Rot;
	}
}
