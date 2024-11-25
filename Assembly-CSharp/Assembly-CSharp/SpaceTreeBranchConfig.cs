using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class SpaceTreeBranchConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

		public GameObject CreatePrefab()
	{
		string id = "SpaceTreeBranch";
		string name = STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SPACETREE.DESC;
		float mass = 8f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("syrup_tree_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 1;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.HideFromSpawnTool,
			GameTags.PlantBranch
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 255f);
		string text = "SpaceTreeBranchOriginal";
		string text2 = STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 198.15f, 258.15f, 293.15f, null, false, 0f, 0.15f, null, true, true, false, true, 12000f, 0f, 12200f, text, text2);
		WiltCondition component = gameObject.GetComponent<WiltCondition>();
		component.WiltDelay = 0f;
		component.RecoveryDelay = 0f;
		Modifiers component2 = gameObject.GetComponent<Modifiers>();
		if (gameObject.GetComponent<Traits>() == null)
		{
			gameObject.AddOrGet<Traits>();
			component2.initialTraits.Add(text);
		}
		KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
		Crop.CropVal cropVal = new Crop.CropVal("WoodLog", 2700f, 75, true);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component3.PrefabID().ToString());
		component2.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
		component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
		Trait trait = Db.Get().traits.Get(component2.initialTraits[0]);
		trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float)cropVal.numProduced, text2, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, cropVal.cropDuration / 600f, text2, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 300f, STRINGS.CREATURES.SPECIES.SPACETREE.NAME, false, false, true));
		component2.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(false);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			gameObject.AddOrGet<MutantPlant>().SpeciesID = component3.PrefabTag;
			SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		}
		gameObject.AddOrGet<Crop>().Configure(cropVal);
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.UpdateComponentRequirement(false);
		gameObject.AddOrGetDef<PlantBranch.Def>().animationSetupCallback = new Action<PlantBranchGrower.Instance, PlantBranch.Instance>(this.AdjustAnimation);
		gameObject.AddOrGetDef<SpaceTreeBranch.Def>().OPTIMAL_LUX_LEVELS = 10000;
		gameObject.AddOrGetDef<UnstableEntombDefense.Def>().Cooldown = 5f;
		gameObject.AddOrGet<BudUprootedMonitor>().destroyOnParentLost = true;
		return gameObject;
	}

		public void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch)
	{
		int base_cell = Grid.PosToCell(trunk);
		int offset_cell = Grid.PosToCell(branch);
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
		KBatchedAnimController component = branch.GetComponent<KBatchedAnimController>();
		if (smi != null && component != null && SpaceTreeBranchConfig.animationSets.ContainsKey(offset))
		{
			SpaceTreeBranch.AnimSet animations = SpaceTreeBranchConfig.animationSets[offset];
			smi.Animations = animations;
			component.Offset = SpaceTreeBranchConfig.animOffset[offset];
			smi.RefreshAnimation();
			branch.GetSMI<UnstableEntombDefense.Instance>().UnentombAnimName = SpaceTreeBranchConfig.entombDefenseAnimNames[offset];
			return;
		}
		global::Debug.LogWarning(string.Concat(new string[]
		{
			"Error on AdjustAnimation().SpaceTreeBranchConfig.cs, spaceBranchFound: ",
			(smi != null).ToString(),
			", animControllerFound: ",
			(component != null).ToString(),
			", animationSetFound: ",
			SpaceTreeBranchConfig.animationSets.ContainsKey(offset).ToString()
		}));
	}

		public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
		inst.AddOrGet<HarvestDesignatable>().iconOffset = new Vector2(0f, Grid.CellSizeInMeters * 0.5f);
	}

		public void OnSpawn(GameObject inst)
	{
	}

		// Note: this type is marked as 'beforefieldinit'.
	static SpaceTreeBranchConfig()
	{
		Dictionary<CellOffset, string> dictionary = new Dictionary<CellOffset, string>();
		CellOffset key = new CellOffset(-1, 1);
		dictionary[key] = "shake_branch_b";
		CellOffset key2 = new CellOffset(-1, 2);
		dictionary[key2] = "shake_branch_c";
		CellOffset key3 = new CellOffset(0, 2);
		dictionary[key3] = "shake_branch_d";
		CellOffset key4 = new CellOffset(1, 2);
		dictionary[key4] = "shake_branch_e";
		CellOffset key5 = new CellOffset(1, 1);
		dictionary[key5] = "shake_branch_f";
		SpaceTreeBranchConfig.entombDefenseAnimNames = dictionary;
		Dictionary<CellOffset, SpaceTreeBranch.AnimSet> dictionary2 = new Dictionary<CellOffset, SpaceTreeBranch.AnimSet>();
		key5 = new CellOffset(-1, 1);
		dictionary2[key5] = new SpaceTreeBranch.AnimSet
		{
			spawn = "branch_b_grow",
			undeveloped = "grow_b_healthy_short",
			spawn_pst = "branch_b_grow_pst",
			ready_harvest = "harvest_ready_branch_b",
			fill = "grow_fill_branch_b",
			wilted = "branch_b_wilt",
			wilted_short_trunk_healthy = "grow_b_wilt_short",
			wilted_short_trunk_wilted = "branch_b_wilt_short",
			hidden = "branch_b_hidden",
			manual_harvest_pre = "syrup_harvest_branch_b_pre",
			manual_harvest_loop = "syrup_harvest_branch_b_loop",
			manual_harvest_pst = "syrup_harvest_branch_b_pst",
			meterAnim_flowerWilted = new string[]
			{
				"leaves_b_wilt"
			},
			die = "branch_b_harvest",
			meterTargets = new string[]
			{
				"leaves_b_target"
			},
			meterAnimNames = new string[]
			{
				"leaves_b_meter"
			}
		};
		key4 = new CellOffset(-1, 2);
		dictionary2[key4] = new SpaceTreeBranch.AnimSet
		{
			spawn = "branch_c_grow",
			undeveloped = "grow_c_healthy_short",
			spawn_pst = "branch_c_grow_pst",
			ready_harvest = "harvest_ready_branch_c",
			fill = "grow_fill_branch_c",
			wilted = "branch_c_wilt",
			wilted_short_trunk_healthy = "grow_c_wilt_short",
			wilted_short_trunk_wilted = "branch_c_wilt_short",
			hidden = "branch_c_hidden",
			manual_harvest_pre = "syrup_harvest_branch_c_pre",
			manual_harvest_loop = "syrup_harvest_branch_c_loop",
			manual_harvest_pst = "syrup_harvest_branch_c_pst",
			meterAnim_flowerWilted = new string[]
			{
				"leaves_c_wilt"
			},
			die = "branch_c_harvest",
			meterTargets = new string[]
			{
				"leaves_c_target"
			},
			meterAnimNames = new string[]
			{
				"leaves_c_meter"
			}
		};
		key3 = new CellOffset(0, 2);
		dictionary2[key3] = new SpaceTreeBranch.AnimSet
		{
			spawn = "branch_d_grow",
			undeveloped = "grow_d_healthy_short",
			spawn_pst = "branch_d_grow_pst",
			ready_harvest = "harvest_ready_branch_d",
			fill = "grow_fill_branch_d",
			wilted = "branch_d_wilt",
			wilted_short_trunk_healthy = "grow_d_wilt_short",
			wilted_short_trunk_wilted = "branch_d_wilt_short",
			hidden = "branch_d_hidden",
			manual_harvest_pre = "syrup_harvest_branch_d_pre",
			manual_harvest_loop = "syrup_harvest_branch_d_loop",
			manual_harvest_pst = "syrup_harvest_branch_d_pst",
			meterAnim_flowerWilted = new string[]
			{
				"leaves_d_wilt"
			},
			die = "branch_d_harvest",
			meterTargets = new string[]
			{
				"leaves_d_target"
			},
			meterAnimNames = new string[]
			{
				"leaves_d_meter"
			}
		};
		key2 = new CellOffset(1, 2);
		dictionary2[key2] = new SpaceTreeBranch.AnimSet
		{
			spawn = "branch_e_grow",
			undeveloped = "grow_e_healthy_short",
			spawn_pst = "branch_e_grow_pst",
			ready_harvest = "harvest_ready_branch_e",
			fill = "grow_fill_branch_e",
			wilted = "branch_e_wilt",
			wilted_short_trunk_healthy = "grow_e_wilt_short",
			wilted_short_trunk_wilted = "branch_e_wilt_short",
			hidden = "branch_e_hidden",
			manual_harvest_pre = "syrup_harvest_branch_e_pre",
			manual_harvest_loop = "syrup_harvest_branch_e_loop",
			manual_harvest_pst = "syrup_harvest_branch_e_pst",
			meterAnim_flowerWilted = new string[]
			{
				"leaves_e_wilt"
			},
			die = "branch_e_harvest",
			meterTargets = new string[]
			{
				"leaves_e_target"
			},
			meterAnimNames = new string[]
			{
				"leaves_e_meter"
			}
		};
		key = new CellOffset(1, 1);
		dictionary2[key] = new SpaceTreeBranch.AnimSet
		{
			spawn = "branch_f_grow",
			undeveloped = "grow_f_healthy_short",
			spawn_pst = "branch_f_grow_pst",
			ready_harvest = "harvest_ready_branch_f",
			fill = "grow_fill_branch_f",
			wilted = "branch_f_wilt",
			wilted_short_trunk_healthy = "grow_f_wilt_short",
			wilted_short_trunk_wilted = "branch_f_wilt_short",
			hidden = "branch_f_hidden",
			manual_harvest_pre = "syrup_harvest_branch_f_pre",
			manual_harvest_loop = "syrup_harvest_branch_f_loop",
			manual_harvest_pst = "syrup_harvest_branch_f_pst",
			meterAnim_flowerWilted = new string[]
			{
				"leaves_f1_wilt",
				"leaves_f2_wilt"
			},
			die = "branch_f_harvest",
			meterTargets = new string[]
			{
				"leaves_f1_target",
				"leaves_f2_target"
			},
			meterAnimNames = new string[]
			{
				"leaves_f1_meter",
				"leaves_f2_meter"
			}
		};
		SpaceTreeBranchConfig.animationSets = dictionary2;
		Dictionary<CellOffset, Vector3> dictionary3 = new Dictionary<CellOffset, Vector3>();
		key = new CellOffset(-1, 1);
		dictionary3[key] = new Vector3(1f, -1f, 0f);
		key2 = new CellOffset(-1, 2);
		dictionary3[key2] = new Vector3(1f, -2f, 0f);
		key3 = new CellOffset(0, 2);
		dictionary3[key3] = new Vector3(0f, -2f, 0f);
		key4 = new CellOffset(1, 2);
		dictionary3[key4] = new Vector3(-1f, -2f, 0f);
		key5 = new CellOffset(1, 1);
		dictionary3[key5] = new Vector3(-1f, -1f, 0f);
		SpaceTreeBranchConfig.animOffset = dictionary3;
	}

		public const string ID = "SpaceTreeBranch";

		public static string[] BRANCH_NAMES = new string[]
	{
		"<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_l\">",
		"<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_tl\">",
		"<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_t\">",
		"<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_tr\">",
		"<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_r\">"
	};

		public const float GROWTH_DURATION = 2700f;

		public const int WOOD_AMOUNT = 75;

		private static Dictionary<CellOffset, string> entombDefenseAnimNames;

		private static Dictionary<CellOffset, SpaceTreeBranch.AnimSet> animationSets;

		private static Dictionary<CellOffset, Vector3> animOffset;
}
