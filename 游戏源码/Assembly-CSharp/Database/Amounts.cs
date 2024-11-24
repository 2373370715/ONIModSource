using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace Database
{
	// Token: 0x0200210A RID: 8458
	public class Amounts : ResourceSet<Amount>
	{
		// Token: 0x0600B3DB RID: 46043 RVA: 0x0043B42C File Offset: 0x0043962C
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
			this.ImmuneLevel = this.CreateAmount("ImmuneLevel", 0f, DUPLICANTSTATS.STANDARD.BaseStats.IMMUNE_LEVEL_MAX, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_immunelevel", "attribute_immunelevel", null);
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
			this.BionicOxygenTank = this.CreateAmount("BionicOxygenTank", 0f, BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG, true, Units.Flat, 60f, true, "STRINGS.DUPLICANTS", "ui_icon_breath", null, null);
			this.BionicOxygenTank.SetDisplayer(new BionicOxygenTankDisplayer(GameUtil.TimeSlice.PerCycle));
			this.BionicOxygenTank.debugSetValue = delegate(AmountInstance instance, float val)
			{
				BionicOxygenTankMonitor.Instance smi = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
				if (smi == null)
				{
					instance.SetValue(val);
					return;
				}
				float availableOxygen = smi.AvailableOxygen;
				if (val >= availableOxygen)
				{
					float mass = val - availableOxygen;
					smi.AddGas(SimHashes.Oxygen, mass, 6282.4497f, byte.MaxValue, 0);
					return;
				}
				float amount = Mathf.Min(availableOxygen - val, availableOxygen);
				float num;
				SimUtil.DiseaseInfo diseaseInfo;
				float num2;
				smi.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out num2);
			};
			this.BionicInternalBattery = this.CreateAmount("BionicInternalBattery", 0f, 360000f, false, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS", "ui_icon_battery", null, null);
			this.BionicInternalBattery.SetDisplayer(new BionicBatteryDisplayer());
			this.BionicInternalBattery.debugSetValue = delegate(AmountInstance instance, float val)
			{
				BionicBatteryMonitor.Instance smi = instance.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
				if (smi == null)
				{
					instance.SetValue(val);
					return;
				}
				float currentCharge = smi.CurrentCharge;
				if (val >= currentCharge)
				{
					float joules = val - currentCharge;
					smi.DebugAddCharge(joules);
					return;
				}
				float joules2 = currentCharge - val;
				smi.ConsumePower(joules2);
			};
			this.BionicOil = this.CreateAmount("BionicOil", 0f, 200f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_liquid", null, null);
			this.BionicOil.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.BionicGunk = this.CreateAmount("BionicGunk", 0f, GunkMonitor.GUNK_CAPACITY, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_gunk", null, null);
			this.BionicGunk.SetDisplayer(new BionicGunkDisplayer(GameUtil.TimeSlice.PerCycle));
			this.InternalBattery = this.CreateAmount("InternalBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalChemicalBattery = this.CreateAmount("InternalChemicalBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalChemicalBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalBioBattery = this.CreateAmount("InternalBioBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalBioBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalElectroBank = this.CreateAmount("InternalElectroBank", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalElectroBank.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
		}

		// Token: 0x0600B3DC RID: 46044 RVA: 0x0043BE10 File Offset: 0x0043A010
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

		// Token: 0x04008DEC RID: 36332
		public Amount Stamina;

		// Token: 0x04008DED RID: 36333
		public Amount Calories;

		// Token: 0x04008DEE RID: 36334
		public Amount ImmuneLevel;

		// Token: 0x04008DEF RID: 36335
		public Amount Breath;

		// Token: 0x04008DF0 RID: 36336
		public Amount Stress;

		// Token: 0x04008DF1 RID: 36337
		public Amount Toxicity;

		// Token: 0x04008DF2 RID: 36338
		public Amount Bladder;

		// Token: 0x04008DF3 RID: 36339
		public Amount Decor;

		// Token: 0x04008DF4 RID: 36340
		public Amount RadiationBalance;

		// Token: 0x04008DF5 RID: 36341
		public Amount BionicOxygenTank;

		// Token: 0x04008DF6 RID: 36342
		public Amount BionicOil;

		// Token: 0x04008DF7 RID: 36343
		public Amount BionicGunk;

		// Token: 0x04008DF8 RID: 36344
		public Amount BionicInternalBattery;

		// Token: 0x04008DF9 RID: 36345
		public Amount Temperature;

		// Token: 0x04008DFA RID: 36346
		public Amount CritterTemperature;

		// Token: 0x04008DFB RID: 36347
		public Amount HitPoints;

		// Token: 0x04008DFC RID: 36348
		public Amount AirPressure;

		// Token: 0x04008DFD RID: 36349
		public Amount Maturity;

		// Token: 0x04008DFE RID: 36350
		public Amount OldAge;

		// Token: 0x04008DFF RID: 36351
		public Amount Age;

		// Token: 0x04008E00 RID: 36352
		public Amount Fertilization;

		// Token: 0x04008E01 RID: 36353
		public Amount Illumination;

		// Token: 0x04008E02 RID: 36354
		public Amount Irrigation;

		// Token: 0x04008E03 RID: 36355
		public Amount Fertility;

		// Token: 0x04008E04 RID: 36356
		public Amount Viability;

		// Token: 0x04008E05 RID: 36357
		public Amount PowerCharge;

		// Token: 0x04008E06 RID: 36358
		public Amount Wildness;

		// Token: 0x04008E07 RID: 36359
		public Amount Incubation;

		// Token: 0x04008E08 RID: 36360
		public Amount ScaleGrowth;

		// Token: 0x04008E09 RID: 36361
		public Amount ElementGrowth;

		// Token: 0x04008E0A RID: 36362
		public Amount Beckoning;

		// Token: 0x04008E0B RID: 36363
		public Amount MilkProduction;

		// Token: 0x04008E0C RID: 36364
		public Amount InternalBattery;

		// Token: 0x04008E0D RID: 36365
		public Amount InternalChemicalBattery;

		// Token: 0x04008E0E RID: 36366
		public Amount InternalBioBattery;

		// Token: 0x04008E0F RID: 36367
		public Amount InternalElectroBank;

		// Token: 0x04008E10 RID: 36368
		public Amount Rot;
	}
}
