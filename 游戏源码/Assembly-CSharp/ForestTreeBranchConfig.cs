using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ForestTreeBranchConfig : IEntityConfig
{
	public const string ID = "ForestTreeBranch";

	public const float WOOD_AMOUNT = 300f;

	private static Dictionary<CellOffset, StandardCropPlant.AnimSet> animationSets = new Dictionary<CellOffset, StandardCropPlant.AnimSet>
	{
		[new CellOffset(-1, 0)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_a_grow",
			grow_pst = "branch_a_grow_pst",
			idle_full = "branch_a_idle_full",
			wilt_base = "branch_a_wilt",
			harvest = "branch_a_harvest"
		},
		[new CellOffset(-1, 1)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_b_grow",
			grow_pst = "branch_b_grow_pst",
			idle_full = "branch_b_idle_full",
			wilt_base = "branch_b_wilt",
			harvest = "branch_b_harvest"
		},
		[new CellOffset(-1, 2)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_c_grow",
			grow_pst = "branch_c_grow_pst",
			idle_full = "branch_c_idle_full",
			wilt_base = "branch_c_wilt",
			harvest = "branch_c_harvest"
		},
		[new CellOffset(0, 2)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_d_grow",
			grow_pst = "branch_d_grow_pst",
			idle_full = "branch_d_idle_full",
			wilt_base = "branch_d_wilt",
			harvest = "branch_d_harvest"
		},
		[new CellOffset(1, 2)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_e_grow",
			grow_pst = "branch_e_grow_pst",
			idle_full = "branch_e_idle_full",
			wilt_base = "branch_e_wilt",
			harvest = "branch_e_harvest"
		},
		[new CellOffset(1, 1)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_f_grow",
			grow_pst = "branch_f_grow_pst",
			idle_full = "branch_f_idle_full",
			wilt_base = "branch_f_wilt",
			harvest = "branch_f_harvest"
		},
		[new CellOffset(1, 0)] = new StandardCropPlant.AnimSet
		{
			grow = "branch_g_grow",
			grow_pst = "branch_g_grow_pst",
			idle_full = "branch_g_idle_full",
			wilt_base = "branch_g_wilt",
			harvest = "branch_g_harvest"
		}
	};

	private static Dictionary<CellOffset, Vector3> animOffset = new Dictionary<CellOffset, Vector3>
	{
		[new CellOffset(-1, 0)] = new Vector3(1f, 0f, 0f),
		[new CellOffset(-1, 1)] = new Vector3(1f, -1f, 0f),
		[new CellOffset(-1, 2)] = new Vector3(1f, -2f, 0f),
		[new CellOffset(0, 2)] = new Vector3(0f, -2f, 0f),
		[new CellOffset(1, 2)] = new Vector3(-1f, -2f, 0f),
		[new CellOffset(1, 1)] = new Vector3(-1f, -1f, 0f),
		[new CellOffset(1, 0)] = new Vector3(-1f, 0f, 0f)
	};

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject obj = EntityTemplates.CreatePlacedEntity("ForestTreeBranch", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME, STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC, 8f, decor: DECOR.BONUS.TIER1, anim: Assets.GetAnim("tree_kanim"), initialAnim: "idle_empty", sceneLayer: Grid.SceneLayer.BuildingFront, width: 1, height: 1, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: new List<Tag>
		{
			GameTags.HideFromSpawnTool,
			GameTags.PlantBranch
		}, defaultTemperature: 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(obj, 258.15f, 288.15f, 313.15f, 448.15f, null, pressure_sensitive: true, 0f, 0.15f, "WoodLog", can_drown: true, can_tinker: true, require_solid_tile: false, should_grow_old: true, 12000f, 0f, 9800f, "ForestTreeBranchOriginal", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
		obj.AddOrGet<TreeBud>();
		obj.AddOrGet<StandardCropPlant>();
		obj.AddOrGet<BudUprootedMonitor>();
		PlantBranch.Def def = obj.AddOrGetDef<PlantBranch.Def>();
		def.preventStartSMIOnSpawn = true;
		def.onEarlySpawn = TranslateOldTrunkToNewSystem;
		def.animationSetupCallback = AdjustAnimation;
		return obj;
	}

	public void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch)
	{
		int base_cell = Grid.PosToCell(trunk);
		int offset_cell = Grid.PosToCell(branch);
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		StandardCropPlant component = branch.GetComponent<StandardCropPlant>();
		KBatchedAnimController component2 = branch.GetComponent<KBatchedAnimController>();
		component.anims = animationSets[offset];
		component2.Offset = animOffset[offset];
		component2.Play(component.anims.grow, KAnim.PlayMode.Paused);
		component.RefreshPositionPercent();
	}

	public void TranslateOldTrunkToNewSystem(PlantBranch.Instance smi)
	{
		BuddingTrunk andForgetOldTrunk = smi.GetComponent<TreeBud>().GetAndForgetOldTrunk();
		if (andForgetOldTrunk != null)
		{
			PlantBranchGrower.Instance sMI = andForgetOldTrunk.GetSMI<PlantBranchGrower.Instance>();
			smi.SetTrunk(sMI);
		}
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
