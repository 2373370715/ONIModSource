using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class BionicUpgradeComponentConfig : IMultiEntityConfig
{
	// Token: 0x060012D0 RID: 4816 RVA: 0x000AE7E1 File Offset: 0x000AC9E1
	public static string CraftBionicPrefabID(string sufix)
	{
		return "bionic_upgrade_" + sufix.ToLower();
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0018B684 File Offset: 0x00189884
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_CONSTRUCTION, STRINGS.ITEMS.BIONIC_BOOSTERS.CONSTRUCTION_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.CONSTRUCTION_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_CONSTRUCTION), Db.Get().Attributes.Construction.Id, "BionicConstructionBoost", new string[]
		{
			"Building1",
			"Building2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "construction", SimHashes.Creature, "PrettyGoodConductors", "DefaultBionicBoostBuilding", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_EXCAVATION, STRINGS.ITEMS.BIONIC_BOOSTERS.EXCAVATION_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.EXCAVATION_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_EXCAVATION), Db.Get().Attributes.Digging.Id, "BionicExcavationBoost", new string[]
		{
			"Mining1",
			"Mining2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "excavation", SimHashes.Creature, "RoboticTools", "DefaultBionicBoostDigging", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_MACHINERY, STRINGS.ITEMS.BIONIC_BOOSTERS.MACHINERY_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.MACHINERY_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_MACHINERY), Db.Get().Attributes.Machinery.Id, "BionicMachineryBoost", new string[]
		{
			"Technicals1",
			"Technicals2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "machinery", SimHashes.Creature, "Smelting", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_COOKING, STRINGS.ITEMS.BIONIC_BOOSTERS.COOKING_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.COOKING_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_COOKING), Db.Get().Attributes.Cooking.Id, "BionicCookingBoost", new string[]
		{
			"Cooking1",
			"Cooking2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "cooking", SimHashes.Creature, "FoodRepurposing", "DefaultBionicBoostCooking", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_MEDICINE, STRINGS.ITEMS.BIONIC_BOOSTERS.MEDICINE_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.MEDICINE_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_MEDICINE), Db.Get().Attributes.Caring.Id, "BionicMedicineBoost", new string[]
		{
			"Medicine1",
			"Medicine2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "medicine", SimHashes.Creature, "MedicineIV", "DefaultBionicBoostMedicine", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_CREATIVITY, STRINGS.ITEMS.BIONIC_BOOSTERS.CREATIVITY_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.CREATIVITY_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_CREATIVITY), Db.Get().Attributes.Art.Id, "BionicCreativityBoost", new string[]
		{
			"Arting1",
			"Arting2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "creativity", SimHashes.Creature, "RefractiveDecor", "DefaultBionicBoostArt", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_AGRICULTURE, STRINGS.ITEMS.BIONIC_BOOSTERS.AGRICULTURE_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.AGRICULTURE_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_AGRICULTURE), Db.Get().Attributes.Botanist.Id, "BionicAgricultureBoost", new string[]
		{
			"Farming1",
			"Farming2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "agriculture", SimHashes.Creature, "AnimalControl", "DefaultBionicBoostFarming", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_HUSBANDRY, STRINGS.ITEMS.BIONIC_BOOSTERS.HUSBANDRY_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.HUSBANDRY_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_1, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_HUSBANDRY), Db.Get().Attributes.Ranching.Id, "BionicHusbandryBoost", new string[]
		{
			"Ranching1"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "ranching", SimHashes.Creature, "AnimalControl", "DefaultBionicBoostRanching", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_SCIENCE, STRINGS.ITEMS.BIONIC_BOOSTERS.SCIENCE_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.SCIENCE_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_1, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_SCIENCE), Db.Get().Attributes.Learning.Id, "BionicScienceBoost", new string[]
		{
			"Researching1"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "science", SimHashes.Creature, "ParallelAutomation", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_PILOTING, STRINGS.ITEMS.BIONIC_BOOSTERS.PILOTING_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.PILOTING_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_1, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_PILOTING), Db.Get().Attributes.SpaceNavigation.Id, "BionicPilotingBoost", new string[]
		{
			"RocketPiloting1"
		})), new string[]
		{
			"EXPANSION1_ID",
			"DLC3_ID"
		}, "upgrade_disc_kanim", "piloting", SimHashes.Creature, "CrashPlan", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_STRENGTH, STRINGS.ITEMS.BIONIC_BOOSTERS.STRENGTH_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.STRENGTH_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_OnGoingEffect.Instance(smi.GetMaster(), new BionicUpgrade_OnGoingEffect.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_STRENGTH), "BionicStrengthBoost", new string[]
		{
			"Hauling1",
			"Hauling2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "strength", SimHashes.Creature, "Suits", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_ATHLETICS, STRINGS.ITEMS.BIONIC_BOOSTERS.ATHLETICS_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.ATHLETICS_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_OnGoingEffect.Instance(smi.GetMaster(), new BionicUpgrade_OnGoingEffect.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_ATHLETICS), "BionicAthleticsBoost", null)), DlcManager.DLC3, "upgrade_disc_kanim", "athletics", SimHashes.Creature, "SolidTransport", null, BionicUpgradeComponentConfig.RarityType.Basic));
		List<GameObject> list2 = list;
		GameObject gameObject = BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_EXPLORER, STRINGS.ITEMS.BIONIC_BOOSTERS.EXPLORER_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.EXPLORER_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_ExplorerBoosterMonitor.Instance(smi.GetMaster(), new BionicUpgrade_ExplorerBoosterMonitor.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_EXPLORER))), DlcManager.DLC3, "upgrade_disc_kanim", "explore", SimHashes.Creature, "HighTempForging", "DefaultBionicBoostExplorer", BionicUpgradeComponentConfig.RarityType.Special);
		if (gameObject != null)
		{
			gameObject.AddOrGetDef<BionicUpgrade_ExplorerBooster.Def>();
			list2.Add(gameObject);
		}
		list2.RemoveAll((GameObject t) => t == null);
		return list2;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0018BC0C File Offset: 0x00189E0C
	public static Tag GetBionicUpgradePrefabIDWithTraitID(string traitID)
	{
		foreach (Tag tag in BionicUpgradeComponentConfig.UpgradesData.Keys)
		{
			BionicUpgradeComponentConfig.BionicUpgradeData bionicUpgradeData = BionicUpgradeComponentConfig.UpgradesData[tag];
			if (bionicUpgradeData.relatedTrait != null && bionicUpgradeData.relatedTrait == traitID)
			{
				return tag;
			}
		}
		return Tag.Invalid;
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0018BC8C File Offset: 0x00189E8C
	public static GameObject CreateNewUpgradeComponent(string id, string name, string desc, float wattageCost, Func<StateMachine.Instance, StateMachine.Instance> stateMachine = null, string[] dlcIDs = null, string animFile = "upgrade_disc_kanim", string animStateName = "object", SimHashes element = SimHashes.Creature, string craftTechUnlockID = null, string relatedTrait = null, BionicUpgradeComponentConfig.RarityType rarity = BionicUpgradeComponentConfig.RarityType.Basic)
	{
		if (!DlcManager.IsAllContentSubscribed(dlcIDs))
		{
			return null;
		}
		string ID = BionicUpgradeComponentConfig.CraftBionicPrefabID(id);
		TechItem techItem = new TechItem(ID, Db.Get().TechItems, Strings.Get("STRINGS.RESEARCH.OTHER_TECH_ITEMS." + id.ToUpper() + ".NAME"), Strings.Get("STRINGS.RESEARCH.OTHER_TECH_ITEMS." + id.ToUpper() + ".DESC"), (string a, bool b) => Def.GetUISprite(Assets.GetPrefab(ID), "ui", false).first, craftTechUnlockID, DlcManager.DLC3, null, false);
		Db.Get().Techs.Get(craftTechUnlockID).AddUnlockedItemIDs(new string[]
		{
			techItem.Id
		});
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ID, name, desc, 25f, true, Assets.GetAnim(animFile), animStateName, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.45f, true, SORTORDER.ARTIFACTS, element, new List<Tag>
		{
			GameTags.MiscPickupable,
			GameTags.BionicUpgrade,
			GameTags.NotRoomAssignable
		});
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.NONE);
		decorProvider.overrideName = gameObject.GetProperName();
		gameObject.AddOrGet<BionicUpgradeComponent>().slotID = Db.Get().AssignableSlots.BionicUpgrade.Id;
		gameObject.AddOrGet<KSelectable>();
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.requiredDlcIds = dlcIDs;
		BionicUpgradeComponentConfig.UpgradesData.Add(component.PrefabTag, new BionicUpgradeComponentConfig.BionicUpgradeData(wattageCost, animStateName, relatedTrait, rarity, stateMachine));
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ID.ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array, array2), array, array2)
		{
			time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_COMPONENT_RECIPE_DESC, DlcManager.IsExpansion1Active() ? STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME : STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME, name),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			},
			requiredTech = craftTechUnlockID
		};
		return gameObject;
	}

	// Token: 0x04000CE5 RID: 3301
	public const string DEFAULT_ANIM_FILE_NAME = "upgrade_disc_kanim";

	// Token: 0x04000CE6 RID: 3302
	public const string ID_PREFIX = "bionic_upgrade_";

	// Token: 0x04000CE7 RID: 3303
	public static string SUFFIX_CONSTRUCTION = "ConstructionBooster".ToLower();

	// Token: 0x04000CE8 RID: 3304
	public static string SUFFIX_EXCAVATION = "ExcavationBooster".ToLower();

	// Token: 0x04000CE9 RID: 3305
	public static string SUFFIX_MACHINERY = "MachineryBooster".ToLower();

	// Token: 0x04000CEA RID: 3306
	public static string SUFFIX_ATHLETICS = "AthleticsBooster".ToLower();

	// Token: 0x04000CEB RID: 3307
	public static string SUFFIX_COOKING = "CookingBooster".ToLower();

	// Token: 0x04000CEC RID: 3308
	public static string SUFFIX_MEDICINE = "MedicineBooster".ToLower();

	// Token: 0x04000CED RID: 3309
	public static string SUFFIX_STRENGTH = "StrengthBooster".ToLower();

	// Token: 0x04000CEE RID: 3310
	public static string SUFFIX_CREATIVITY = "CreativityBooster".ToLower();

	// Token: 0x04000CEF RID: 3311
	public static string SUFFIX_AGRICULTURE = "AgricultureBooster".ToLower();

	// Token: 0x04000CF0 RID: 3312
	public static string SUFFIX_HUSBANDRY = "HusbandryBooster".ToLower();

	// Token: 0x04000CF1 RID: 3313
	public static string SUFFIX_SCIENCE = "ScienceBooster".ToLower();

	// Token: 0x04000CF2 RID: 3314
	public static string SUFFIX_PILOTING = "PilotingBooster".ToLower();

	// Token: 0x04000CF3 RID: 3315
	public static string SUFFIX_EXPLORER = "ExplorerBooster".ToLower();

	// Token: 0x04000CF4 RID: 3316
	public static Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> UpgradesData = new Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>();

	// Token: 0x02000441 RID: 1089
	public enum RarityType
	{
		// Token: 0x04000CF6 RID: 3318
		Basic,
		// Token: 0x04000CF7 RID: 3319
		Special
	}

	// Token: 0x02000442 RID: 1090
	public class BionicUpgradeData
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x000AE7FC File Offset: 0x000AC9FC
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x000AE7F3 File Offset: 0x000AC9F3
		public float WattageCost { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x000AE80D File Offset: 0x000ACA0D
		// (set) Token: 0x060012DA RID: 4826 RVA: 0x000AE804 File Offset: 0x000ACA04
		public Func<StateMachine.Instance, StateMachine.Instance> stateMachine { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x000AE815 File Offset: 0x000ACA15
		public string uiAnimName
		{
			get
			{
				if (!(this.animStateName == "object"))
				{
					return "ui_" + this.animStateName;
				}
				return "ui";
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x000AE848 File Offset: 0x000ACA48
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x000AE83F File Offset: 0x000ACA3F
		public string relatedTrait { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x000AE859 File Offset: 0x000ACA59
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x000AE850 File Offset: 0x000ACA50
		public BionicUpgradeComponentConfig.RarityType rarity { get; private set; }

		// Token: 0x060012E1 RID: 4833 RVA: 0x000AE861 File Offset: 0x000ACA61
		public BionicUpgradeData(float cost, string animStateName, string relatedTrait, BionicUpgradeComponentConfig.RarityType rarity, Func<StateMachine.Instance, StateMachine.Instance> smi)
		{
			this.WattageCost = cost;
			this.stateMachine = smi;
			this.animStateName = animStateName;
			this.relatedTrait = relatedTrait;
			this.rarity = rarity;
		}

		// Token: 0x04000CF8 RID: 3320
		private const string DEFAULT_ANIM_STATE_NAME = "object";

		// Token: 0x04000CFB RID: 3323
		public string animStateName = "object";
	}
}
