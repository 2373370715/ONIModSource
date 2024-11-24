using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021C4 RID: 8644
	public class Skills : ResourceSet<Skill>
	{
		// Token: 0x0600B761 RID: 46945 RVA: 0x0045F58C File Offset: 0x0045D78C
		public Skills(ResourceSet parent) : base("Skills", parent)
		{
			this.Mining1 = this.AddSkill(new Skill("Mining1", DUPLICANTS.ROLES.JUNIOR_MINER.NAME, DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION, "", 0, "hat_role_mining1", "skillbadge_role_mining1", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedSmall,
				Db.Get().SkillPerks.CanDigVeryFirm
			}, null));
			this.Mining2 = this.AddSkill(new Skill("Mining2", DUPLICANTS.ROLES.MINER.NAME, DUPLICANTS.ROLES.MINER.DESCRIPTION, "", 1, "hat_role_mining2", "skillbadge_role_mining2", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedMedium,
				Db.Get().SkillPerks.CanDigNearlyImpenetrable
			}, new List<string>
			{
				this.Mining1.Id
			}));
			this.Mining3 = this.AddSkill(new Skill("Mining3", DUPLICANTS.ROLES.SENIOR_MINER.NAME, DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION, "", 2, "hat_role_mining3", "skillbadge_role_mining3", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedLarge,
				Db.Get().SkillPerks.CanDigSuperDuperHard
			}, new List<string>
			{
				this.Mining2.Id
			}));
			this.Mining4 = this.AddSkill(new Skill("Mining4", DUPLICANTS.ROLES.MASTER_MINER.NAME, DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION, "EXPANSION1_ID", 3, "hat_role_mining4", "skillbadge_role_mining4", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDigRadioactiveMaterials
			}, new List<string>
			{
				this.Mining3.Id
			}));
			this.Building1 = this.AddSkill(new Skill("Building1", DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME, DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION, "", 0, "hat_role_building1", "skillbadge_role_building1", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionSmall
			}, null));
			this.Building2 = this.AddSkill(new Skill("Building2", DUPLICANTS.ROLES.BUILDER.NAME, DUPLICANTS.ROLES.BUILDER.DESCRIPTION, "", 1, "hat_role_building2", "skillbadge_role_building2", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionMedium
			}, new List<string>
			{
				this.Building1.Id
			}));
			this.Building3 = this.AddSkill(new Skill("Building3", DUPLICANTS.ROLES.SENIOR_BUILDER.NAME, DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION, "", 2, "hat_role_building3", "skillbadge_role_building3", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionLarge,
				Db.Get().SkillPerks.CanDemolish
			}, new List<string>
			{
				this.Building2.Id
			}));
			this.Farming1 = this.AddSkill(new Skill("Farming1", DUPLICANTS.ROLES.JUNIOR_FARMER.NAME, DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION, "", 0, "hat_role_farming1", "skillbadge_role_farming1", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanySmall
			}, null));
			this.Farming2 = this.AddSkill(new Skill("Farming2", DUPLICANTS.ROLES.FARMER.NAME, DUPLICANTS.ROLES.FARMER.DESCRIPTION, "", 1, "hat_role_farming2", "skillbadge_role_farming2", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyMedium,
				Db.Get().SkillPerks.CanFarmTinker
			}, new List<string>
			{
				this.Farming1.Id
			}));
			this.Farming3 = this.AddSkill(new Skill("Farming3", DUPLICANTS.ROLES.SENIOR_FARMER.NAME, DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION, "", 2, "hat_role_farming3", "skillbadge_role_farming3", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyLarge
			}, new List<string>
			{
				this.Farming2.Id
			}));
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				this.Farming3.perks.Add(Db.Get().SkillPerks.CanIdentifyMutantSeeds);
			}
			this.Ranching1 = this.AddSkill(new Skill("Ranching1", DUPLICANTS.ROLES.RANCHER.NAME, DUPLICANTS.ROLES.RANCHER.DESCRIPTION, "", 1, "hat_role_rancher1", "skillbadge_role_rancher1", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanWrangleCreatures,
				Db.Get().SkillPerks.CanUseRanchStation,
				Db.Get().SkillPerks.IncreaseRanchingSmall
			}, new List<string>
			{
				this.Farming1.Id
			}));
			this.Ranching2 = this.AddSkill(new Skill("Ranching2", DUPLICANTS.ROLES.SENIOR_RANCHER.NAME, DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION, "", 2, "hat_role_rancher2", "skillbadge_role_rancher2", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseMilkingStation,
				Db.Get().SkillPerks.IncreaseRanchingMedium
			}, new List<string>
			{
				this.Ranching1.Id
			}));
			this.Researching1 = this.AddSkill(new Skill("Researching1", DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION, "", 0, "hat_role_research1", "skillbadge_role_research1", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningSmall,
				Db.Get().SkillPerks.AllowAdvancedResearch
			}, null));
			this.Researching2 = this.AddSkill(new Skill("Researching2", DUPLICANTS.ROLES.RESEARCHER.NAME, DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION, "", 1, "hat_role_research2", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningMedium,
				Db.Get().SkillPerks.CanStudyWorldObjects,
				Db.Get().SkillPerks.AllowGeyserTuning
			}, new List<string>
			{
				this.Researching1.Id
			}));
			this.AtomicResearch = this.AddSkill(new Skill("AtomicResearch", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research5", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching4 = this.AddSkill(new Skill("Researching4", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching4.deprecated = true;
			this.Researching3 = this.AddSkill(new Skill("Researching3", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, "", 2, "hat_role_research3", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowInterstellarResearch,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching3.deprecated = DlcManager.IsExpansion1Active();
			this.Astronomy = this.AddSkill(new Skill("Astronomy", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 1, "hat_role_research3", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseClusterTelescope,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching1.Id
			}));
			this.SpaceResearch = this.AddSkill(new Skill("SpaceResearch", DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME, DUPLICANTS.ROLES.SPACE_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLargeSpace,
				Db.Get().SkillPerks.AllowOrbitalResearch
			}, new List<string>
			{
				this.Astronomy.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.RocketPiloting1 = this.AddSkill(new Skill("RocketPiloting1", DUPLICANTS.ROLES.ROCKETPILOT.NAME, DUPLICANTS.ROLES.ROCKETPILOT.DESCRIPTION, "EXPANSION1_ID", 0, "hat_role_astronaut1", "skillbadge_role_rocketry1", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRocketControlStation
				}, new List<string>()));
				this.RocketPiloting2 = this.AddSkill(new Skill("RocketPiloting2", DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.NAME, DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_astronaut2", "skillbadge_role_rocketry3", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseRocketSpeedSmall
				}, new List<string>
				{
					this.RocketPiloting1.Id,
					this.Astronomy.Id
				}));
			}
			this.Cooking1 = this.AddSkill(new Skill("Cooking1", DUPLICANTS.ROLES.JUNIOR_COOK.NAME, DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION, "", 0, "hat_role_cooking1", "skillbadge_role_cooking1", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingSmall,
				Db.Get().SkillPerks.CanElectricGrill
			}, null));
			this.Cooking2 = this.AddSkill(new Skill("Cooking2", DUPLICANTS.ROLES.COOK.NAME, DUPLICANTS.ROLES.COOK.DESCRIPTION, "", 1, "hat_role_cooking2", "skillbadge_role_cooking2", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingMedium,
				Db.Get().SkillPerks.CanSpiceGrinder
			}, new List<string>
			{
				this.Cooking1.Id
			}));
			this.Arting1 = this.AddSkill(new Skill("Arting1", DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME, DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION, "", 0, "hat_role_art1", "skillbadge_role_art1", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArt,
				Db.Get().SkillPerks.CanArtUgly,
				Db.Get().SkillPerks.IncreaseArtSmall
			}, null));
			this.Arting2 = this.AddSkill(new Skill("Arting2", DUPLICANTS.ROLES.ARTIST.NAME, DUPLICANTS.ROLES.ARTIST.DESCRIPTION, "", 1, "hat_role_art2", "skillbadge_role_art2", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArtOkay,
				Db.Get().SkillPerks.IncreaseArtMedium,
				Db.Get().SkillPerks.CanClothingAlteration
			}, new List<string>
			{
				this.Arting1.Id
			}));
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Arting2.perks.Add(Db.Get().SkillPerks.CanStudyArtifact);
			}
			this.Arting3 = this.AddSkill(new Skill("Arting3", DUPLICANTS.ROLES.MASTER_ARTIST.NAME, DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION, "", 2, "hat_role_art3", "skillbadge_role_art3", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArtGreat,
				Db.Get().SkillPerks.IncreaseArtLarge
			}, new List<string>
			{
				this.Arting2.Id
			}));
			this.Hauling1 = this.AddSkill(new Skill("Hauling1", DUPLICANTS.ROLES.HAULER.NAME, DUPLICANTS.ROLES.HAULER.DESCRIPTION, "", 0, "hat_role_hauling1", "skillbadge_role_hauling1", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGofer,
				Db.Get().SkillPerks.IncreaseCarryAmountSmall
			}, null));
			this.Hauling2 = this.AddSkill(new Skill("Hauling2", DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION, "", 1, "hat_role_hauling2", "skillbadge_role_hauling2", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthCourier,
				Db.Get().SkillPerks.IncreaseCarryAmountMedium
			}, new List<string>
			{
				this.Hauling1.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, "EXPANSION1_ID", 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id,
					this.RocketPiloting1.Id
				}));
			}
			else
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, "", 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id
				}));
			}
			this.Suits1 = this.AddSkill(new Skill("Suits1", DUPLICANTS.ROLES.SUIT_EXPERT.NAME, DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION, "", 2, "hat_role_suits2", "skillbadge_role_suits3", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.ExosuitExpertise,
				Db.Get().SkillPerks.IncreaseAthleticsMedium
			}, new List<string>
			{
				this.ThermalSuits.Id
			}));
			this.Technicals1 = this.AddSkill(new Skill("Technicals1", DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION, "", 0, "hat_role_technicals1", "skillbadge_role_technicals1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachinerySmall
			}, null));
			this.Technicals2 = this.AddSkill(new Skill("Technicals2", DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME, DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION, "", 1, "hat_role_technicals2", "skillbadge_role_technicals2", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryMedium,
				Db.Get().SkillPerks.CanPowerTinker,
				Db.Get().SkillPerks.CanCraftElectronics
			}, new List<string>
			{
				this.Technicals1.Id
			}));
			this.Engineering1 = this.AddSkill(new Skill("Engineering1", DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION, "", 2, "hat_role_engineering1", "skillbadge_role_engineering1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryLarge,
				Db.Get().SkillPerks.IncreaseConstructionMechatronics,
				Db.Get().SkillPerks.ConveyorBuild
			}, new List<string>
			{
				this.Hauling2.Id,
				this.Technicals2.Id
			}));
			this.Basekeeping1 = this.AddSkill(new Skill("Basekeeping1", DUPLICANTS.ROLES.HANDYMAN.NAME, DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION, "", 0, "hat_role_basekeeping1", "skillbadge_role_basekeeping1", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGroundskeeper
			}, null));
			this.Basekeeping2 = this.AddSkill(new Skill("Basekeeping2", DUPLICANTS.ROLES.PLUMBER.NAME, DUPLICANTS.ROLES.PLUMBER.DESCRIPTION, "", 1, "hat_role_basekeeping2", "skillbadge_role_basekeeping2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthPlumber,
				Db.Get().SkillPerks.CanDoPlumbing
			}, new List<string>
			{
				this.Basekeeping1.Id
			}));
			this.Pyrotechnics = this.AddSkill(new Skill("Pyrotechnics", DUPLICANTS.ROLES.PYROTECHNIC.NAME, DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION, "", 2, "hat_role_pyrotechnics", "skillbadge_role_technicals2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanMakeMissiles
			}, new List<string>
			{
				this.Basekeeping2.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, "EXPANSION1_ID", 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}));
				this.Astronauting1.deprecated = true;
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, "EXPANSION1_ID", 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Astronauting1.Id
				}));
				this.Astronauting2.deprecated = true;
			}
			else
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME, DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION, "", 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRockets
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}));
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.ASTRONAUT.NAME, DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION, "", 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.FasterSpaceFlight
				}, new List<string>
				{
					this.Astronauting1.Id
				}));
			}
			this.Medicine1 = this.AddSkill(new Skill("Medicine1", DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME, DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION, "", 0, "hat_role_medicalaid1", "skillbadge_role_medicalaid1", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanCompound,
				Db.Get().SkillPerks.IncreaseCaringSmall
			}, null));
			this.Medicine2 = this.AddSkill(new Skill("Medicine2", DUPLICANTS.ROLES.MEDIC.NAME, DUPLICANTS.ROLES.MEDIC.DESCRIPTION, "", 1, "hat_role_medicalaid2", "skillbadge_role_medicalaid2", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDoctor,
				Db.Get().SkillPerks.IncreaseCaringMedium
			}, new List<string>
			{
				this.Medicine1.Id
			}));
			this.Medicine3 = this.AddSkill(new Skill("Medicine3", DUPLICANTS.ROLES.SENIOR_MEDIC.NAME, DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION, "", 2, "hat_role_medicalaid3", "skillbadge_role_medicalaid3", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanAdvancedMedicine,
				Db.Get().SkillPerks.IncreaseCaringLarge
			}, new List<string>
			{
				this.Medicine2.Id
			}));
		}

		// Token: 0x0600B762 RID: 46946 RVA: 0x0011624E File Offset: 0x0011444E
		private Skill AddSkill(Skill skill)
		{
			if (DlcManager.IsContentSubscribed(skill.dlcId))
			{
				return base.Add(skill);
			}
			return skill;
		}

		// Token: 0x0600B763 RID: 46947 RVA: 0x00460E70 File Offset: 0x0045F070
		public List<Skill> GetSkillsWithPerk(string perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x0600B764 RID: 46948 RVA: 0x00460ED8 File Offset: 0x0045F0D8
		public List<Skill> GetSkillsWithPerk(SkillPerk perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x0600B765 RID: 46949 RVA: 0x00460F3C File Offset: 0x0045F13C
		public List<Skill> GetAllPriorSkills(Skill skill)
		{
			List<Skill> list = new List<Skill>();
			foreach (string id in skill.priorSkills)
			{
				Skill skill2 = base.Get(id);
				list.Add(skill2);
				list.AddRange(this.GetAllPriorSkills(skill2));
			}
			return list;
		}

		// Token: 0x0600B766 RID: 46950 RVA: 0x00460FAC File Offset: 0x0045F1AC
		public List<Skill> GetTerminalSkills()
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				bool flag = true;
				using (List<Skill>.Enumerator enumerator2 = this.resources.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.priorSkills.Contains(skill.Id))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x040095DA RID: 38362
		public Skill Mining1;

		// Token: 0x040095DB RID: 38363
		public Skill Mining2;

		// Token: 0x040095DC RID: 38364
		public Skill Mining3;

		// Token: 0x040095DD RID: 38365
		public Skill Mining4;

		// Token: 0x040095DE RID: 38366
		public Skill Building1;

		// Token: 0x040095DF RID: 38367
		public Skill Building2;

		// Token: 0x040095E0 RID: 38368
		public Skill Building3;

		// Token: 0x040095E1 RID: 38369
		public Skill Farming1;

		// Token: 0x040095E2 RID: 38370
		public Skill Farming2;

		// Token: 0x040095E3 RID: 38371
		public Skill Farming3;

		// Token: 0x040095E4 RID: 38372
		public Skill Ranching1;

		// Token: 0x040095E5 RID: 38373
		public Skill Ranching2;

		// Token: 0x040095E6 RID: 38374
		public Skill Researching1;

		// Token: 0x040095E7 RID: 38375
		public Skill Researching2;

		// Token: 0x040095E8 RID: 38376
		public Skill Researching3;

		// Token: 0x040095E9 RID: 38377
		public Skill Researching4;

		// Token: 0x040095EA RID: 38378
		public Skill AtomicResearch;

		// Token: 0x040095EB RID: 38379
		public Skill SpaceResearch;

		// Token: 0x040095EC RID: 38380
		public Skill Astronomy;

		// Token: 0x040095ED RID: 38381
		public Skill RocketPiloting1;

		// Token: 0x040095EE RID: 38382
		public Skill RocketPiloting2;

		// Token: 0x040095EF RID: 38383
		public Skill Cooking1;

		// Token: 0x040095F0 RID: 38384
		public Skill Cooking2;

		// Token: 0x040095F1 RID: 38385
		public Skill Arting1;

		// Token: 0x040095F2 RID: 38386
		public Skill Arting2;

		// Token: 0x040095F3 RID: 38387
		public Skill Arting3;

		// Token: 0x040095F4 RID: 38388
		public Skill Hauling1;

		// Token: 0x040095F5 RID: 38389
		public Skill Hauling2;

		// Token: 0x040095F6 RID: 38390
		public Skill ThermalSuits;

		// Token: 0x040095F7 RID: 38391
		public Skill Suits1;

		// Token: 0x040095F8 RID: 38392
		public Skill Technicals1;

		// Token: 0x040095F9 RID: 38393
		public Skill Technicals2;

		// Token: 0x040095FA RID: 38394
		public Skill Engineering1;

		// Token: 0x040095FB RID: 38395
		public Skill Basekeeping1;

		// Token: 0x040095FC RID: 38396
		public Skill Basekeeping2;

		// Token: 0x040095FD RID: 38397
		public Skill Pyrotechnics;

		// Token: 0x040095FE RID: 38398
		public Skill Astronauting1;

		// Token: 0x040095FF RID: 38399
		public Skill Astronauting2;

		// Token: 0x04009600 RID: 38400
		public Skill Medicine1;

		// Token: 0x04009601 RID: 38401
		public Skill Medicine2;

		// Token: 0x04009602 RID: 38402
		public Skill Medicine3;
	}
}
