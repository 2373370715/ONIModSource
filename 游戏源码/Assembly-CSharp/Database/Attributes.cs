using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02002116 RID: 8470
	public class Attributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x0600B3F8 RID: 46072 RVA: 0x0043CDF0 File Offset: 0x0043AFF0
		public Attributes(ResourceSet parent) : base("Attributes", parent)
		{
			this.Construction = base.Add(new Klei.AI.Attribute("Construction", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_construction", null));
			this.Construction.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Digging = base.Add(new Klei.AI.Attribute("Digging", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_excavation", null));
			this.Digging.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Machinery = base.Add(new Klei.AI.Attribute("Machinery", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_machinery", null));
			this.Machinery.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Athletics = base.Add(new Klei.AI.Attribute("Athletics", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_athletics", null));
			this.Athletics.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Learning = base.Add(new Klei.AI.Attribute("Learning", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_science", null));
			this.Learning.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Cooking = base.Add(new Klei.AI.Attribute("Cooking", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_cusine", null));
			this.Cooking.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Caring = base.Add(new Klei.AI.Attribute("Caring", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_medicine", null));
			this.Caring.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Strength = base.Add(new Klei.AI.Attribute("Strength", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_strength", null));
			this.Strength.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Art = base.Add(new Klei.AI.Attribute("Art", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_creativity", null));
			this.Art.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Botanist = base.Add(new Klei.AI.Attribute("Botanist", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_agriculture", null));
			this.Botanist.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Ranching = base.Add(new Klei.AI.Attribute("Ranching", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, "mod_husbandry", null));
			this.Ranching.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.PowerTinker = base.Add(new Klei.AI.Attribute("PowerTinker", true, Klei.AI.Attribute.Display.Normal, true, 0f, null, null, null, null));
			this.PowerTinker.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.FarmTinker = base.Add(new Klei.AI.Attribute("FarmTinker", true, Klei.AI.Attribute.Display.Normal, true, 0f, null, null, null, null));
			this.FarmTinker.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			if (DlcManager.IsExpansion1Active())
			{
				this.SpaceNavigation = base.Add(new Klei.AI.Attribute("SpaceNavigation", true, Klei.AI.Attribute.Display.Skill, true, 0f, null, null, null, null));
				this.SpaceNavigation.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			}
			else
			{
				this.SpaceNavigation = base.Add(new Klei.AI.Attribute("SpaceNavigation", true, Klei.AI.Attribute.Display.Normal, true, 0f, null, null, null, null));
				this.SpaceNavigation.SetFormatter(new PercentAttributeFormatter());
			}
			this.Immunity = base.Add(new Klei.AI.Attribute("Immunity", true, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, null));
			this.Immunity.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.ThermalConductivityBarrier = base.Add(new Klei.AI.Attribute("ThermalConductivityBarrier", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, null));
			this.ThermalConductivityBarrier.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Distance, GameUtil.TimeSlice.None));
			this.Insulation = base.Add(new Klei.AI.Attribute("Insulation", false, Klei.AI.Attribute.Display.General, true, 0f, null, null, null, null));
			this.Luminescence = base.Add(new Klei.AI.Attribute("Luminescence", false, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Decor = base.Add(new Klei.AI.Attribute("Decor", false, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Decor.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.FoodQuality = base.Add(new Klei.AI.Attribute("FoodQuality", false, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.FoodQuality.SetFormatter(new FoodQualityAttributeFormatter());
			this.ScaldingThreshold = base.Add(new Klei.AI.Attribute("ScaldingThreshold", false, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.ScaldingThreshold.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.None));
			this.ScoldingThreshold = base.Add(new Klei.AI.Attribute("ScoldingThreshold", false, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.ScoldingThreshold.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.None));
			this.GeneratorOutput = base.Add(new Klei.AI.Attribute("GeneratorOutput", false, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.GeneratorOutput.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None));
			this.MachinerySpeed = base.Add(new Klei.AI.Attribute("MachinerySpeed", false, Klei.AI.Attribute.Display.General, false, 1f, null, null, null, null));
			this.MachinerySpeed.SetFormatter(new PercentAttributeFormatter());
			this.RadiationResistance = base.Add(new Klei.AI.Attribute("RadiationResistance", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, DlcManager.AVAILABLE_EXPANSION1_ONLY));
			this.RadiationResistance.SetFormatter(new PercentAttributeFormatter());
			this.RadiationRecovery = base.Add(new Klei.AI.Attribute("RadiationRecovery", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, DlcManager.AVAILABLE_EXPANSION1_ONLY));
			this.RadiationRecovery.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Radiation, GameUtil.TimeSlice.PerCycle));
			this.DecorExpectation = base.Add(new Klei.AI.Attribute("DecorExpectation", false, Klei.AI.Attribute.Display.Expectation, false, 0f, null, null, null, null));
			this.DecorExpectation.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.FoodExpectation = base.Add(new Klei.AI.Attribute("FoodExpectation", false, Klei.AI.Attribute.Display.Expectation, false, 0f, null, null, null, null));
			this.FoodExpectation.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.RoomTemperaturePreference = base.Add(new Klei.AI.Attribute("RoomTemperaturePreference", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.RoomTemperaturePreference.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.None));
			this.QualityOfLifeExpectation = base.Add(new Klei.AI.Attribute("QualityOfLifeExpectation", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.QualityOfLifeExpectation.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.AirConsumptionRate = base.Add(new Klei.AI.Attribute("AirConsumptionRate", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.AirConsumptionRate.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));
			this.MaxUnderwaterTravelCost = base.Add(new Klei.AI.Attribute("MaxUnderwaterTravelCost", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.ToiletEfficiency = base.Add(new Klei.AI.Attribute("ToiletEfficiency", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, null));
			this.ToiletEfficiency.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			this.Sneezyness = base.Add(new Klei.AI.Attribute("Sneezyness", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, null));
			this.TransitTubeTravelSpeed = base.Add(new Klei.AI.Attribute("TransitTubeTravelSpeed", false, Klei.AI.Attribute.Display.Never, false, 0f, null, null, null, null));
			this.TransitTubeTravelSpeed.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None));
			this.DiseaseCureSpeed = base.Add(new Klei.AI.Attribute("DiseaseCureSpeed", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.DiseaseCureSpeed.BaseValue = 1f;
			this.DiseaseCureSpeed.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			this.DoctoredLevel = base.Add(new Klei.AI.Attribute("DoctoredLevel", false, Klei.AI.Attribute.Display.Never, false, 0f, null, null, null, null));
			this.CarryAmount = base.Add(new Klei.AI.Attribute("CarryAmount", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, null));
			this.CarryAmount.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.None));
			this.QualityOfLife = base.Add(new Klei.AI.Attribute("QualityOfLife", false, Klei.AI.Attribute.Display.Details, false, 0f, "ui_icon_qualityoflife", "attribute_qualityoflife", "mod_morale", null));
			this.QualityOfLife.SetFormatter(new QualityOfLifeAttributeFormatter());
			this.GermResistance = base.Add(new Klei.AI.Attribute("GermResistance", false, Klei.AI.Attribute.Display.Details, false, 0f, "ui_icon_immunelevel", "attribute_immunelevel", "mod_germresistance", null));
			this.GermResistance.SetFormatter(new GermResistanceAttributeFormatter());
			this.LifeSupport = base.Add(new Klei.AI.Attribute("LifeSupport", true, Klei.AI.Attribute.Display.Never, false, 0f, null, null, null, null));
			this.LifeSupport.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Toggle = base.Add(new Klei.AI.Attribute("Toggle", true, Klei.AI.Attribute.Display.Never, false, 0f, null, null, null, null));
			this.Toggle.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
		}

		// Token: 0x04008E4B RID: 36427
		public Klei.AI.Attribute Construction;

		// Token: 0x04008E4C RID: 36428
		public Klei.AI.Attribute Digging;

		// Token: 0x04008E4D RID: 36429
		public Klei.AI.Attribute Machinery;

		// Token: 0x04008E4E RID: 36430
		public Klei.AI.Attribute Athletics;

		// Token: 0x04008E4F RID: 36431
		public Klei.AI.Attribute Learning;

		// Token: 0x04008E50 RID: 36432
		public Klei.AI.Attribute Cooking;

		// Token: 0x04008E51 RID: 36433
		public Klei.AI.Attribute Caring;

		// Token: 0x04008E52 RID: 36434
		public Klei.AI.Attribute Strength;

		// Token: 0x04008E53 RID: 36435
		public Klei.AI.Attribute Art;

		// Token: 0x04008E54 RID: 36436
		public Klei.AI.Attribute Botanist;

		// Token: 0x04008E55 RID: 36437
		public Klei.AI.Attribute Ranching;

		// Token: 0x04008E56 RID: 36438
		public Klei.AI.Attribute LifeSupport;

		// Token: 0x04008E57 RID: 36439
		public Klei.AI.Attribute Toggle;

		// Token: 0x04008E58 RID: 36440
		public Klei.AI.Attribute PowerTinker;

		// Token: 0x04008E59 RID: 36441
		public Klei.AI.Attribute FarmTinker;

		// Token: 0x04008E5A RID: 36442
		public Klei.AI.Attribute SpaceNavigation;

		// Token: 0x04008E5B RID: 36443
		public Klei.AI.Attribute Immunity;

		// Token: 0x04008E5C RID: 36444
		public Klei.AI.Attribute GermResistance;

		// Token: 0x04008E5D RID: 36445
		public Klei.AI.Attribute Insulation;

		// Token: 0x04008E5E RID: 36446
		public Klei.AI.Attribute Luminescence;

		// Token: 0x04008E5F RID: 36447
		public Klei.AI.Attribute ThermalConductivityBarrier;

		// Token: 0x04008E60 RID: 36448
		public Klei.AI.Attribute Decor;

		// Token: 0x04008E61 RID: 36449
		public Klei.AI.Attribute FoodQuality;

		// Token: 0x04008E62 RID: 36450
		public Klei.AI.Attribute ScaldingThreshold;

		// Token: 0x04008E63 RID: 36451
		public Klei.AI.Attribute ScoldingThreshold;

		// Token: 0x04008E64 RID: 36452
		public Klei.AI.Attribute GeneratorOutput;

		// Token: 0x04008E65 RID: 36453
		public Klei.AI.Attribute MachinerySpeed;

		// Token: 0x04008E66 RID: 36454
		public Klei.AI.Attribute RadiationResistance;

		// Token: 0x04008E67 RID: 36455
		public Klei.AI.Attribute RadiationRecovery;

		// Token: 0x04008E68 RID: 36456
		public Klei.AI.Attribute DecorExpectation;

		// Token: 0x04008E69 RID: 36457
		public Klei.AI.Attribute FoodExpectation;

		// Token: 0x04008E6A RID: 36458
		public Klei.AI.Attribute RoomTemperaturePreference;

		// Token: 0x04008E6B RID: 36459
		public Klei.AI.Attribute QualityOfLifeExpectation;

		// Token: 0x04008E6C RID: 36460
		public Klei.AI.Attribute AirConsumptionRate;

		// Token: 0x04008E6D RID: 36461
		public Klei.AI.Attribute MaxUnderwaterTravelCost;

		// Token: 0x04008E6E RID: 36462
		public Klei.AI.Attribute ToiletEfficiency;

		// Token: 0x04008E6F RID: 36463
		public Klei.AI.Attribute Sneezyness;

		// Token: 0x04008E70 RID: 36464
		public Klei.AI.Attribute TransitTubeTravelSpeed;

		// Token: 0x04008E71 RID: 36465
		public Klei.AI.Attribute DiseaseCureSpeed;

		// Token: 0x04008E72 RID: 36466
		public Klei.AI.Attribute DoctoredLevel;

		// Token: 0x04008E73 RID: 36467
		public Klei.AI.Attribute CarryAmount;

		// Token: 0x04008E74 RID: 36468
		public Klei.AI.Attribute QualityOfLife;
	}
}
