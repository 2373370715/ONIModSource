using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x020021C0 RID: 8640
	public class SkillPerks : ResourceSet<SkillPerk>
	{
		// Token: 0x0600B759 RID: 46937 RVA: 0x0045DFA8 File Offset: 0x0045C1A8
		public SkillPerks(ResourceSet parent) : base("SkillPerks", parent)
		{
			this.IncreaseDigSpeedSmall = base.Add(new SkillAttributePerk("IncreaseDigSpeedSmall", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MINER.NAME));
			this.IncreaseDigSpeedMedium = base.Add(new SkillAttributePerk("IncreaseDigSpeedMedium", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MINER.NAME));
			this.IncreaseDigSpeedLarge = base.Add(new SkillAttributePerk("IncreaseDigSpeedLarge", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MINER.NAME));
			this.CanDigVeryFirm = base.Add(new SimpleSkillPerk("CanDigVeryFirm", UI.ROLES_SCREEN.PERKS.CAN_DIG_VERY_FIRM.DESCRIPTION));
			this.CanDigNearlyImpenetrable = base.Add(new SimpleSkillPerk("CanDigAbyssalite", UI.ROLES_SCREEN.PERKS.CAN_DIG_NEARLY_IMPENETRABLE.DESCRIPTION));
			this.CanDigSuperDuperHard = base.Add(new SimpleSkillPerk("CanDigDiamondAndObsidan", UI.ROLES_SCREEN.PERKS.CAN_DIG_SUPER_SUPER_HARD.DESCRIPTION));
			this.CanDigRadioactiveMaterials = base.Add(new SimpleSkillPerk("CanDigCorium", UI.ROLES_SCREEN.PERKS.CAN_DIG_RADIOACTIVE_MATERIALS.DESCRIPTION));
			this.CanDigUnobtanium = base.Add(new SimpleSkillPerk("CanDigUnobtanium", UI.ROLES_SCREEN.PERKS.CAN_DIG_UNOBTANIUM.DESCRIPTION));
			this.IncreaseConstructionSmall = base.Add(new SkillAttributePerk("IncreaseConstructionSmall", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME));
			this.IncreaseConstructionMedium = base.Add(new SkillAttributePerk("IncreaseConstructionMedium", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.BUILDER.NAME));
			this.IncreaseConstructionLarge = base.Add(new SkillAttributePerk("IncreaseConstructionLarge", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_BUILDER.NAME));
			this.IncreaseConstructionMechatronics = base.Add(new SkillAttributePerk("IncreaseConstructionMechatronics", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
			this.CanDemolish = base.Add(new SimpleSkillPerk("CanDemonlish", UI.ROLES_SCREEN.PERKS.CAN_DEMOLISH.DESCRIPTION));
			this.IncreaseLearningSmall = base.Add(new SkillAttributePerk("IncreaseLearningSmall", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME));
			this.IncreaseLearningMedium = base.Add(new SkillAttributePerk("IncreaseLearningMedium", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.RESEARCHER.NAME));
			this.IncreaseLearningLarge = base.Add(new SkillAttributePerk("IncreaseLearningLarge", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME));
			this.IncreaseLearningLargeSpace = base.Add(new SkillAttributePerk("IncreaseLearningLargeSpace", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME));
			this.IncreaseBotanySmall = base.Add(new SkillAttributePerk("IncreaseBotanySmall", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_FARMER.NAME));
			this.IncreaseBotanyMedium = base.Add(new SkillAttributePerk("IncreaseBotanyMedium", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.FARMER.NAME));
			this.IncreaseBotanyLarge = base.Add(new SkillAttributePerk("IncreaseBotanyLarge", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_FARMER.NAME));
			this.CanFarmTinker = base.Add(new SimpleSkillPerk("CanFarmTinker", UI.ROLES_SCREEN.PERKS.CAN_FARM_TINKER.DESCRIPTION));
			this.CanIdentifyMutantSeeds = base.Add(new SimpleSkillPerk("CanIdentifyMutantSeeds", UI.ROLES_SCREEN.PERKS.CAN_IDENTIFY_MUTANT_SEEDS.DESCRIPTION));
			this.IncreaseRanchingSmall = base.Add(new SkillAttributePerk("IncreaseRanchingSmall", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.RANCHER.NAME));
			this.IncreaseRanchingMedium = base.Add(new SkillAttributePerk("IncreaseRanchingMedium", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SENIOR_RANCHER.NAME));
			this.CanWrangleCreatures = base.Add(new SimpleSkillPerk("CanWrangleCreatures", UI.ROLES_SCREEN.PERKS.CAN_WRANGLE_CREATURES.DESCRIPTION));
			this.CanUseRanchStation = base.Add(new SimpleSkillPerk("CanUseRanchStation", UI.ROLES_SCREEN.PERKS.CAN_USE_RANCH_STATION.DESCRIPTION));
			this.CanUseMilkingStation = base.Add(new SimpleSkillPerk("CanUseMilkingStation", UI.ROLES_SCREEN.PERKS.CAN_USE_MILKING_STATION.DESCRIPTION));
			this.IncreaseAthleticsSmall = base.Add(new SkillAttributePerk("IncreaseAthleticsSmall", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseAthleticsMedium = base.Add(new SkillAttributePerk("IncreaseAthletics", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_EXPERT.NAME));
			this.IncreaseAthleticsLarge = base.Add(new SkillAttributePerk("IncreaseAthleticsLarge", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_DURABILITY.NAME));
			this.IncreaseStrengthGofer = base.Add(new SkillAttributePerk("IncreaseStrengthGofer", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseStrengthCourier = base.Add(new SkillAttributePerk("IncreaseStrengthCourier", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
			this.IncreaseStrengthGroundskeeper = base.Add(new SkillAttributePerk("IncreaseStrengthGroundskeeper", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HANDYMAN.NAME));
			this.IncreaseStrengthPlumber = base.Add(new SkillAttributePerk("IncreaseStrengthPlumber", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.PLUMBER.NAME));
			this.IncreaseCarryAmountSmall = base.Add(new SkillAttributePerk("IncreaseCarryAmountSmall", Db.Get().Attributes.CarryAmount.Id, 400f, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseCarryAmountMedium = base.Add(new SkillAttributePerk("IncreaseCarryAmountMedium", Db.Get().Attributes.CarryAmount.Id, 800f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
			this.IncreaseArtSmall = base.Add(new SkillAttributePerk("IncreaseArtSmall", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME));
			this.IncreaseArtMedium = base.Add(new SkillAttributePerk("IncreaseArt", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.ARTIST.NAME));
			this.IncreaseArtLarge = base.Add(new SkillAttributePerk("IncreaseArtLarge", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MASTER_ARTIST.NAME));
			this.CanArt = base.Add(new SimpleSkillPerk("CanArt", UI.ROLES_SCREEN.PERKS.CAN_ART.DESCRIPTION));
			this.CanArtUgly = base.Add(new SimpleSkillPerk("CanArtUgly", UI.ROLES_SCREEN.PERKS.CAN_ART_UGLY.DESCRIPTION));
			this.CanArtOkay = base.Add(new SimpleSkillPerk("CanArtOkay", UI.ROLES_SCREEN.PERKS.CAN_ART_OKAY.DESCRIPTION));
			this.CanArtGreat = base.Add(new SimpleSkillPerk("CanArtGreat", UI.ROLES_SCREEN.PERKS.CAN_ART_GREAT.DESCRIPTION));
			this.CanStudyArtifact = base.Add(new SimpleSkillPerk("CanStudyArtifact", UI.ROLES_SCREEN.PERKS.CAN_STUDY_ARTIFACTS.DESCRIPTION));
			this.CanClothingAlteration = base.Add(new SimpleSkillPerk("CanClothingAlteration", UI.ROLES_SCREEN.PERKS.CAN_CLOTHING_ALTERATION.DESCRIPTION));
			this.IncreaseMachinerySmall = base.Add(new SkillAttributePerk("IncreaseMachinerySmall", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME));
			this.IncreaseMachineryMedium = base.Add(new SkillAttributePerk("IncreaseMachineryMedium", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME));
			this.IncreaseMachineryLarge = base.Add(new SkillAttributePerk("IncreaseMachineryLarge", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
			this.ConveyorBuild = base.Add(new SimpleSkillPerk("ConveyorBuild", UI.ROLES_SCREEN.PERKS.CONVEYOR_BUILD.DESCRIPTION));
			this.CanPowerTinker = base.Add(new SimpleSkillPerk("CanPowerTinker", UI.ROLES_SCREEN.PERKS.CAN_POWER_TINKER.DESCRIPTION));
			this.CanMakeMissiles = base.Add(new SimpleSkillPerk("CanMakeMissiles", UI.ROLES_SCREEN.PERKS.CAN_MAKE_MISSILES.DESCRIPTION));
			this.CanCraftElectronics = base.Add(new SimpleSkillPerk("CanCraftElectronics", UI.ROLES_SCREEN.PERKS.CAN_CRAFT_ELECTRONICS.DESCRIPTION, DlcManager.DLC3));
			this.CanElectricGrill = base.Add(new SimpleSkillPerk("CanElectricGrill", UI.ROLES_SCREEN.PERKS.CAN_ELECTRIC_GRILL.DESCRIPTION));
			this.IncreaseCookingSmall = base.Add(new SkillAttributePerk("IncreaseCookingSmall", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_COOK.NAME));
			this.IncreaseCookingMedium = base.Add(new SkillAttributePerk("IncreaseCookingMedium", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.COOK.NAME));
			this.CanSpiceGrinder = base.Add(new SimpleSkillPerk("CanSpiceGrinder ", UI.ROLES_SCREEN.PERKS.CAN_SPICE_GRINDER.DESCRIPTION));
			this.IncreaseCaringSmall = base.Add(new SkillAttributePerk("IncreaseCaringSmall", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME));
			this.IncreaseCaringMedium = base.Add(new SkillAttributePerk("IncreaseCaringMedium", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MEDIC.NAME));
			this.IncreaseCaringLarge = base.Add(new SkillAttributePerk("IncreaseCaringLarge", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MEDIC.NAME));
			this.CanCompound = base.Add(new SimpleSkillPerk("CanCompound", UI.ROLES_SCREEN.PERKS.CAN_COMPOUND.DESCRIPTION));
			this.CanDoctor = base.Add(new SimpleSkillPerk("CanDoctor", UI.ROLES_SCREEN.PERKS.CAN_DOCTOR.DESCRIPTION));
			this.CanAdvancedMedicine = base.Add(new SimpleSkillPerk("CanAdvancedMedicine", UI.ROLES_SCREEN.PERKS.CAN_ADVANCED_MEDICINE.DESCRIPTION));
			this.ExosuitExpertise = base.Add(new SimpleSkillPerk("ExosuitExpertise", UI.ROLES_SCREEN.PERKS.EXOSUIT_EXPERTISE.DESCRIPTION));
			this.ExosuitDurability = base.Add(new SimpleSkillPerk("ExosuitDurability", UI.ROLES_SCREEN.PERKS.EXOSUIT_DURABILITY.DESCRIPTION));
			this.AllowAdvancedResearch = base.Add(new SimpleSkillPerk("AllowAdvancedResearch", UI.ROLES_SCREEN.PERKS.ADVANCED_RESEARCH.DESCRIPTION));
			this.AllowInterstellarResearch = base.Add(new SimpleSkillPerk("AllowInterStellarResearch", UI.ROLES_SCREEN.PERKS.INTERSTELLAR_RESEARCH.DESCRIPTION));
			this.AllowNuclearResearch = base.Add(new SimpleSkillPerk("AllowNuclearResearch", UI.ROLES_SCREEN.PERKS.NUCLEAR_RESEARCH.DESCRIPTION));
			this.AllowOrbitalResearch = base.Add(new SimpleSkillPerk("AllowOrbitalResearch", UI.ROLES_SCREEN.PERKS.ORBITAL_RESEARCH.DESCRIPTION));
			this.AllowGeyserTuning = base.Add(new SimpleSkillPerk("AllowGeyserTuning", UI.ROLES_SCREEN.PERKS.GEYSER_TUNING.DESCRIPTION));
			this.CanStudyWorldObjects = base.Add(new SimpleSkillPerk("CanStudyWorldObjects", UI.ROLES_SCREEN.PERKS.CAN_STUDY_WORLD_OBJECTS.DESCRIPTION));
			this.CanUseClusterTelescope = base.Add(new SimpleSkillPerk("CanUseClusterTelescope", UI.ROLES_SCREEN.PERKS.CAN_USE_CLUSTER_TELESCOPE.DESCRIPTION));
			this.CanDoPlumbing = base.Add(new SimpleSkillPerk("CanDoPlumbing", UI.ROLES_SCREEN.PERKS.CAN_DO_PLUMBING.DESCRIPTION));
			this.CanUseRockets = base.Add(new SimpleSkillPerk("CanUseRockets", UI.ROLES_SCREEN.PERKS.CAN_USE_ROCKETS.DESCRIPTION));
			this.FasterSpaceFlight = base.Add(new SkillAttributePerk("FasterSpaceFlight", Db.Get().Attributes.SpaceNavigation.Id, 0.1f, DUPLICANTS.ROLES.ASTRONAUT.NAME));
			this.CanTrainToBeAstronaut = base.Add(new SimpleSkillPerk("CanTrainToBeAstronaut", UI.ROLES_SCREEN.PERKS.CAN_DO_ASTRONAUT_TRAINING.DESCRIPTION));
			this.CanMissionControl = base.Add(new SimpleSkillPerk("CanMissionControl", UI.ROLES_SCREEN.PERKS.CAN_MISSION_CONTROL.DESCRIPTION));
			this.CanUseRocketControlStation = base.Add(new SimpleSkillPerk("CanUseRocketControlStation", UI.ROLES_SCREEN.PERKS.CAN_PILOT_ROCKET.DESCRIPTION));
			this.IncreaseRocketSpeedSmall = base.Add(new SkillAttributePerk("IncreaseRocketSpeedSmall", Db.Get().Attributes.SpaceNavigation.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.ROCKETPILOT.NAME));
		}

		// Token: 0x0400956F RID: 38255
		public SkillPerk IncreaseDigSpeedSmall;

		// Token: 0x04009570 RID: 38256
		public SkillPerk IncreaseDigSpeedMedium;

		// Token: 0x04009571 RID: 38257
		public SkillPerk IncreaseDigSpeedLarge;

		// Token: 0x04009572 RID: 38258
		public SkillPerk CanDigVeryFirm;

		// Token: 0x04009573 RID: 38259
		public SkillPerk CanDigNearlyImpenetrable;

		// Token: 0x04009574 RID: 38260
		public SkillPerk CanDigSuperDuperHard;

		// Token: 0x04009575 RID: 38261
		public SkillPerk CanDigRadioactiveMaterials;

		// Token: 0x04009576 RID: 38262
		public SkillPerk CanDigUnobtanium;

		// Token: 0x04009577 RID: 38263
		public SkillPerk IncreaseConstructionSmall;

		// Token: 0x04009578 RID: 38264
		public SkillPerk IncreaseConstructionMedium;

		// Token: 0x04009579 RID: 38265
		public SkillPerk IncreaseConstructionLarge;

		// Token: 0x0400957A RID: 38266
		public SkillPerk IncreaseConstructionMechatronics;

		// Token: 0x0400957B RID: 38267
		public SkillPerk CanDemolish;

		// Token: 0x0400957C RID: 38268
		public SkillPerk IncreaseLearningSmall;

		// Token: 0x0400957D RID: 38269
		public SkillPerk IncreaseLearningMedium;

		// Token: 0x0400957E RID: 38270
		public SkillPerk IncreaseLearningLarge;

		// Token: 0x0400957F RID: 38271
		public SkillPerk IncreaseLearningLargeSpace;

		// Token: 0x04009580 RID: 38272
		public SkillPerk IncreaseBotanySmall;

		// Token: 0x04009581 RID: 38273
		public SkillPerk IncreaseBotanyMedium;

		// Token: 0x04009582 RID: 38274
		public SkillPerk IncreaseBotanyLarge;

		// Token: 0x04009583 RID: 38275
		public SkillPerk CanFarmTinker;

		// Token: 0x04009584 RID: 38276
		public SkillPerk CanIdentifyMutantSeeds;

		// Token: 0x04009585 RID: 38277
		public SkillPerk CanWrangleCreatures;

		// Token: 0x04009586 RID: 38278
		public SkillPerk CanUseRanchStation;

		// Token: 0x04009587 RID: 38279
		public SkillPerk CanUseMilkingStation;

		// Token: 0x04009588 RID: 38280
		public SkillPerk IncreaseRanchingSmall;

		// Token: 0x04009589 RID: 38281
		public SkillPerk IncreaseRanchingMedium;

		// Token: 0x0400958A RID: 38282
		public SkillPerk IncreaseAthleticsSmall;

		// Token: 0x0400958B RID: 38283
		public SkillPerk IncreaseAthleticsMedium;

		// Token: 0x0400958C RID: 38284
		public SkillPerk IncreaseAthleticsLarge;

		// Token: 0x0400958D RID: 38285
		public SkillPerk IncreaseStrengthSmall;

		// Token: 0x0400958E RID: 38286
		public SkillPerk IncreaseStrengthMedium;

		// Token: 0x0400958F RID: 38287
		public SkillPerk IncreaseStrengthGofer;

		// Token: 0x04009590 RID: 38288
		public SkillPerk IncreaseStrengthCourier;

		// Token: 0x04009591 RID: 38289
		public SkillPerk IncreaseStrengthGroundskeeper;

		// Token: 0x04009592 RID: 38290
		public SkillPerk IncreaseStrengthPlumber;

		// Token: 0x04009593 RID: 38291
		public SkillPerk IncreaseCarryAmountSmall;

		// Token: 0x04009594 RID: 38292
		public SkillPerk IncreaseCarryAmountMedium;

		// Token: 0x04009595 RID: 38293
		public SkillPerk IncreaseArtSmall;

		// Token: 0x04009596 RID: 38294
		public SkillPerk IncreaseArtMedium;

		// Token: 0x04009597 RID: 38295
		public SkillPerk IncreaseArtLarge;

		// Token: 0x04009598 RID: 38296
		public SkillPerk CanArt;

		// Token: 0x04009599 RID: 38297
		public SkillPerk CanArtUgly;

		// Token: 0x0400959A RID: 38298
		public SkillPerk CanArtOkay;

		// Token: 0x0400959B RID: 38299
		public SkillPerk CanArtGreat;

		// Token: 0x0400959C RID: 38300
		public SkillPerk CanStudyArtifact;

		// Token: 0x0400959D RID: 38301
		public SkillPerk CanClothingAlteration;

		// Token: 0x0400959E RID: 38302
		public SkillPerk IncreaseMachinerySmall;

		// Token: 0x0400959F RID: 38303
		public SkillPerk IncreaseMachineryMedium;

		// Token: 0x040095A0 RID: 38304
		public SkillPerk IncreaseMachineryLarge;

		// Token: 0x040095A1 RID: 38305
		public SkillPerk ConveyorBuild;

		// Token: 0x040095A2 RID: 38306
		public SkillPerk CanMakeMissiles;

		// Token: 0x040095A3 RID: 38307
		public SkillPerk CanPowerTinker;

		// Token: 0x040095A4 RID: 38308
		public SkillPerk CanCraftElectronics;

		// Token: 0x040095A5 RID: 38309
		public SkillPerk CanElectricGrill;

		// Token: 0x040095A6 RID: 38310
		public SkillPerk IncreaseCookingSmall;

		// Token: 0x040095A7 RID: 38311
		public SkillPerk IncreaseCookingMedium;

		// Token: 0x040095A8 RID: 38312
		public SkillPerk CanSpiceGrinder;

		// Token: 0x040095A9 RID: 38313
		public SkillPerk IncreaseCaringSmall;

		// Token: 0x040095AA RID: 38314
		public SkillPerk IncreaseCaringMedium;

		// Token: 0x040095AB RID: 38315
		public SkillPerk IncreaseCaringLarge;

		// Token: 0x040095AC RID: 38316
		public SkillPerk CanCompound;

		// Token: 0x040095AD RID: 38317
		public SkillPerk CanDoctor;

		// Token: 0x040095AE RID: 38318
		public SkillPerk CanAdvancedMedicine;

		// Token: 0x040095AF RID: 38319
		public SkillPerk ExosuitExpertise;

		// Token: 0x040095B0 RID: 38320
		public SkillPerk ExosuitDurability;

		// Token: 0x040095B1 RID: 38321
		public SkillPerk AllowAdvancedResearch;

		// Token: 0x040095B2 RID: 38322
		public SkillPerk AllowInterstellarResearch;

		// Token: 0x040095B3 RID: 38323
		public SkillPerk AllowNuclearResearch;

		// Token: 0x040095B4 RID: 38324
		public SkillPerk AllowOrbitalResearch;

		// Token: 0x040095B5 RID: 38325
		public SkillPerk AllowGeyserTuning;

		// Token: 0x040095B6 RID: 38326
		public SkillPerk CanStudyWorldObjects;

		// Token: 0x040095B7 RID: 38327
		public SkillPerk CanUseClusterTelescope;

		// Token: 0x040095B8 RID: 38328
		public SkillPerk IncreaseRocketSpeedSmall;

		// Token: 0x040095B9 RID: 38329
		public SkillPerk CanMissionControl;

		// Token: 0x040095BA RID: 38330
		public SkillPerk CanDoPlumbing;

		// Token: 0x040095BB RID: 38331
		public SkillPerk CanUseRockets;

		// Token: 0x040095BC RID: 38332
		public SkillPerk FasterSpaceFlight;

		// Token: 0x040095BD RID: 38333
		public SkillPerk CanTrainToBeAstronaut;

		// Token: 0x040095BE RID: 38334
		public SkillPerk CanUseRocketControlStation;
	}
}
