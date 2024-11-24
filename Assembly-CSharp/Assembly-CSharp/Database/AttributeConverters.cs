﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

namespace Database
{
	public class AttributeConverters : ResourceSet<AttributeConverter>
	{
		public AttributeConverter Create(string id, string name, string description, Klei.AI.Attribute attribute, float multiplier, float base_value, IAttributeFormatter formatter, string[] available_dlcs)
		{
			AttributeConverter attributeConverter = new AttributeConverter(id, name, description, multiplier, base_value, attribute, formatter);
			if (DlcManager.IsDlcListValidForCurrentContent(available_dlcs))
			{
				base.Add(attributeConverter);
				attribute.converters.Add(attributeConverter);
			}
			return attributeConverter;
		}

		public AttributeConverters()
		{
			ToPercentAttributeFormatter formatter = new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None);
			StandardAttributeFormatter formatter2 = new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.None);
			this.MovementSpeed = this.Create("MovementSpeed", "Movement Speed", DUPLICANTS.ATTRIBUTES.ATHLETICS.SPEEDMODIFIER, Db.Get().Attributes.Athletics, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ConstructionSpeed = this.Create("ConstructionSpeed", "Construction Speed", DUPLICANTS.ATTRIBUTES.CONSTRUCTION.SPEEDMODIFIER, Db.Get().Attributes.Construction, 0.25f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.DiggingSpeed = this.Create("DiggingSpeed", "Digging Speed", DUPLICANTS.ATTRIBUTES.DIGGING.SPEEDMODIFIER, Db.Get().Attributes.Digging, 0.25f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.MachinerySpeed = this.Create("MachinerySpeed", "Machinery Speed", DUPLICANTS.ATTRIBUTES.MACHINERY.SPEEDMODIFIER, Db.Get().Attributes.Machinery, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.HarvestSpeed = this.Create("HarvestSpeed", "Harvest Speed", DUPLICANTS.ATTRIBUTES.BOTANIST.HARVEST_SPEED_MODIFIER, Db.Get().Attributes.Botanist, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.PlantTendSpeed = this.Create("PlantTendSpeed", "Plant Tend Speed", DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_MODIFIER, Db.Get().Attributes.Botanist, 0.025f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CompoundingSpeed = this.Create("CompoundingSpeed", "Compounding Speed", DUPLICANTS.ATTRIBUTES.CARING.FABRICATE_SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ResearchSpeed = this.Create("ResearchSpeed", "Research Speed", DUPLICANTS.ATTRIBUTES.LEARNING.RESEARCHSPEED, Db.Get().Attributes.Learning, 0.4f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.TrainingSpeed = this.Create("TrainingSpeed", "Training Speed", DUPLICANTS.ATTRIBUTES.LEARNING.SPEEDMODIFIER, Db.Get().Attributes.Learning, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CookingSpeed = this.Create("CookingSpeed", "Cooking Speed", DUPLICANTS.ATTRIBUTES.COOKING.SPEEDMODIFIER, Db.Get().Attributes.Cooking, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ArtSpeed = this.Create("ArtSpeed", "Art Speed", DUPLICANTS.ATTRIBUTES.ART.SPEEDMODIFIER, Db.Get().Attributes.Art, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.DoctorSpeed = this.Create("DoctorSpeed", "Doctor Speed", DUPLICANTS.ATTRIBUTES.CARING.SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.2f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.TidyingSpeed = this.Create("TidyingSpeed", "Tidying Speed", DUPLICANTS.ATTRIBUTES.STRENGTH.SPEEDMODIFIER, Db.Get().Attributes.Strength, 0.25f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.AttackDamage = this.Create("AttackDamage", "Attack Damage", DUPLICANTS.ATTRIBUTES.DIGGING.ATTACK_MODIFIER, Db.Get().Attributes.Digging, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.PilotingSpeed = this.Create("PilotingSpeed", "Piloting Speed", DUPLICANTS.ATTRIBUTES.SPACENAVIGATION.SPEED_MODIFIER, Db.Get().Attributes.SpaceNavigation, 0.025f, 0f, formatter, DlcManager.AVAILABLE_EXPANSION1_ONLY);
			this.ImmuneLevelBoost = this.Create("ImmuneLevelBoost", "Immune Level Boost", DUPLICANTS.ATTRIBUTES.IMMUNITY.BOOST_MODIFIER, Db.Get().Attributes.Immunity, 0.0016666667f, 0f, new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.PerCycle), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.ToiletSpeed = this.Create("ToiletSpeed", "Toilet Speed", "", Db.Get().Attributes.ToiletEfficiency, 1f, -1f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CarryAmountFromStrength = this.Create("CarryAmountFromStrength", "Carry Amount", DUPLICANTS.ATTRIBUTES.STRENGTH.CARRYMODIFIER, Db.Get().Attributes.Strength, 40f, 0f, formatter2, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.TemperatureInsulation = this.Create("TemperatureInsulation", "Temperature Insulation", DUPLICANTS.ATTRIBUTES.INSULATION.SPEEDMODIFIER, Db.Get().Attributes.Insulation, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.SeedHarvestChance = this.Create("SeedHarvestChance", "Seed Harvest Chance", DUPLICANTS.ATTRIBUTES.BOTANIST.BONUS_SEEDS, Db.Get().Attributes.Botanist, 0.033f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.CapturableSpeed = this.Create("CapturableSpeed", "Capturable Speed", DUPLICANTS.ATTRIBUTES.RANCHING.CAPTURABLESPEED, Db.Get().Attributes.Ranching, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.GeotuningSpeed = this.Create("GeotuningSpeed", "Geotuning Speed", DUPLICANTS.ATTRIBUTES.LEARNING.GEOTUNER_SPEED_MODIFIER, Db.Get().Attributes.Learning, 0.05f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.RanchingEffectDuration = this.Create("RanchingEffectDuration", "Ranching Effect Duration", DUPLICANTS.ATTRIBUTES.RANCHING.EFFECTMODIFIER, Db.Get().Attributes.Ranching, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.FarmedEffectDuration = this.Create("FarmedEffectDuration", "Farmer's Touch Duration", DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_EFFECT_MODIFIER, Db.Get().Attributes.Botanist, 0.1f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
			this.PowerTinkerEffectDuration = this.Create("PowerTinkerEffectDuration", "Engie's Tune-Up Effect Duration", DUPLICANTS.ATTRIBUTES.MACHINERY.TINKER_EFFECT_MODIFIER, Db.Get().Attributes.Machinery, 0.025f, 0f, formatter, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		public List<AttributeConverter> GetConvertersForAttribute(Klei.AI.Attribute attrib)
		{
			List<AttributeConverter> list = new List<AttributeConverter>();
			foreach (AttributeConverter attributeConverter in this.resources)
			{
				if (attributeConverter.attribute == attrib)
				{
					list.Add(attributeConverter);
				}
			}
			return list;
		}

		public AttributeConverter MovementSpeed;

		public AttributeConverter ConstructionSpeed;

		public AttributeConverter DiggingSpeed;

		public AttributeConverter MachinerySpeed;

		public AttributeConverter HarvestSpeed;

		public AttributeConverter PlantTendSpeed;

		public AttributeConverter CompoundingSpeed;

		public AttributeConverter ResearchSpeed;

		public AttributeConverter TrainingSpeed;

		public AttributeConverter CookingSpeed;

		public AttributeConverter ArtSpeed;

		public AttributeConverter DoctorSpeed;

		public AttributeConverter TidyingSpeed;

		public AttributeConverter AttackDamage;

		public AttributeConverter PilotingSpeed;

		public AttributeConverter ImmuneLevelBoost;

		public AttributeConverter ToiletSpeed;

		public AttributeConverter CarryAmountFromStrength;

		public AttributeConverter TemperatureInsulation;

		public AttributeConverter SeedHarvestChance;

		public AttributeConverter RanchingEffectDuration;

		public AttributeConverter FarmedEffectDuration;

		public AttributeConverter PowerTinkerEffectDuration;

		public AttributeConverter CapturableSpeed;

		public AttributeConverter GeotuningSpeed;
	}
}
