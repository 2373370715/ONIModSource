using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class SpaceTreeBranchConfig : IEntityConfig
{
	public const string ID = "SpaceTreeBranch";

	public static string[] BRANCH_NAMES = new string[5] { "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_l\">", "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_tl\">", "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_t\">", "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_tr\">", "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_r\">" };

	public const float GROWTH_DURATION = 2700f;

	public const int WOOD_AMOUNT = 75;

	private static Dictionary<CellOffset, string> entombDefenseAnimNames = new Dictionary<CellOffset, string>
	{
		[new CellOffset(-1, 1)] = "shake_branch_b",
		[new CellOffset(-1, 2)] = "shake_branch_c",
		[new CellOffset(0, 2)] = "shake_branch_d",
		[new CellOffset(1, 2)] = "shake_branch_e",
		[new CellOffset(1, 1)] = "shake_branch_f"
	};

	private static Dictionary<CellOffset, SpaceTreeBranch.AnimSet> animationSets = new Dictionary<CellOffset, SpaceTreeBranch.AnimSet>
	{
		[new CellOffset(-1, 1)] = new SpaceTreeBranch.AnimSet
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
			meterAnim_flowerWilted = new string[1] { "leaves_b_wilt" },
			die = "branch_b_harvest",
			meterTargets = new string[1] { "leaves_b_target" },
			meterAnimNames = new string[1] { "leaves_b_meter" }
		},
		[new CellOffset(-1, 2)] = new SpaceTreeBranch.AnimSet
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
			meterAnim_flowerWilted = new string[1] { "leaves_c_wilt" },
			die = "branch_c_harvest",
			meterTargets = new string[1] { "leaves_c_target" },
			meterAnimNames = new string[1] { "leaves_c_meter" }
		},
		[new CellOffset(0, 2)] = new SpaceTreeBranch.AnimSet
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
			meterAnim_flowerWilted = new string[1] { "leaves_d_wilt" },
			die = "branch_d_harvest",
			meterTargets = new string[1] { "leaves_d_target" },
			meterAnimNames = new string[1] { "leaves_d_meter" }
		},
		[new CellOffset(1, 2)] = new SpaceTreeBranch.AnimSet
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
			meterAnim_flowerWilted = new string[1] { "leaves_e_wilt" },
			die = "branch_e_harvest",
			meterTargets = new string[1] { "leaves_e_target" },
			meterAnimNames = new string[1] { "leaves_e_meter" }
		},
		[new CellOffset(1, 1)] = new SpaceTreeBranch.AnimSet
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
			meterAnim_flowerWilted = new string[2] { "leaves_f1_wilt", "leaves_f2_wilt" },
			die = "branch_f_harvest",
			meterTargets = new string[2] { "leaves_f1_target", "leaves_f2_target" },
			meterAnimNames = new string[2] { "leaves_f1_meter", "leaves_f2_meter" }
		}
	};

	private static Dictionary<CellOffset, Vector3> animOffset = new Dictionary<CellOffset, Vector3>
	{
		[new CellOffset(-1, 1)] = new Vector3(1f, -1f, 0f),
		[new CellOffset(-1, 2)] = new Vector3(1f, -2f, 0f),
		[new CellOffset(0, 2)] = new Vector3(0f, -2f, 0f),
		[new CellOffset(1, 2)] = new Vector3(-1f, -2f, 0f),
		[new CellOffset(1, 1)] = new Vector3(-1f, -1f, 0f)
	};

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("SpaceTreeBranch", STRINGS.CREATURES.SPECIES.SPACETREE.NAME, STRINGS.CREATURES.SPECIES.SPACETREE.DESC, 8f, decor: DECOR.BONUS.TIER1, anim: Assets.GetAnim("syrup_tree_kanim"), initialAnim: "idle_empty", sceneLayer: Grid.SceneLayer.BuildingFront, width: 1, height: 1, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: new List<Tag>
		{
			GameTags.HideFromSpawnTool,
			GameTags.PlantBranch
		}, defaultTemperature: 255f);
		string text = "SpaceTreeBranchOriginal";
		string text2 = STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 198.15f, 258.15f, 293.15f, null, pressure_sensitive: false, 0f, 0.15f, null, can_drown: true, can_tinker: true, require_solid_tile: false, should_grow_old: true, 12000f, 0f, 12200f, text, text2);
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
		Crop.CropVal cropval = new Crop.CropVal("WoodLog", 2700f, 75);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component3.PrefabID().ToString());
		component2.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
		component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
		Trait trait = Db.Get().traits.Get(component2.initialTraits[0]);
		trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, cropval.numProduced, text2));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, cropval.cropDuration / 600f, text2));
		trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 300f, STRINGS.CREATURES.SPECIES.SPACETREE.NAME));
		component2.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness();
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			gameObject.AddOrGet<MutantPlant>().SpeciesID = component3.PrefabTag;
			SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		}
		gameObject.AddOrGet<Crop>().Configure(cropval);
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.UpdateComponentRequirement<Uprootable>(required: false);
		gameObject.AddOrGetDef<PlantBranch.Def>().animationSetupCallback = AdjustAnimation;
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
		SpaceTreeBranch.Instance sMI = branch.GetSMI<SpaceTreeBranch.Instance>();
		KBatchedAnimController component = branch.GetComponent<KBatchedAnimController>();
		if (sMI != null && component != null && animationSets.ContainsKey(offset))
		{
			SpaceTreeBranch.AnimSet animations = animationSets[offset];
			sMI.Animations = animations;
			component.Offset = animOffset[offset];
			sMI.RefreshAnimation();
			branch.GetSMI<UnstableEntombDefense.Instance>().UnentombAnimName = entombDefenseAnimNames[offset];
		}
		else
		{
			Debug.LogWarning("Error on AdjustAnimation().SpaceTreeBranchConfig.cs, spaceBranchFound: " + (sMI != null) + ", animControllerFound: " + (component != null) + ", animationSetFound: " + animationSets.ContainsKey(offset));
		}
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
		inst.AddOrGet<HarvestDesignatable>().iconOffset = new Vector2(0f, Grid.CellSizeInMeters * 0.5f);
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
