using System.Collections.Generic;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class ForestTreeBranchConfig : IEntityConfig {
    public const            string                                            ID          = "ForestTreeBranch";
    public const            float                                             WOOD_AMOUNT = 300f;
    private static readonly Dictionary<CellOffset, StandardCropPlant.AnimSet> animationSets;
    private static readonly Dictionary<CellOffset, Vector3>                   animOffset;

    // Note: this type is marked as 'beforefieldinit'.
    static ForestTreeBranchConfig() {
        var dictionary = new Dictionary<CellOffset, StandardCropPlant.AnimSet>();
        var key        = new CellOffset(-1, 0);
        dictionary[key] = new StandardCropPlant.AnimSet {
            grow      = "branch_a_grow",
            grow_pst  = "branch_a_grow_pst",
            idle_full = "branch_a_idle_full",
            wilt_base = "branch_a_wilt",
            harvest   = "branch_a_harvest"
        };

        var key2 = new CellOffset(-1, 1);
        dictionary[key2] = new StandardCropPlant.AnimSet {
            grow      = "branch_b_grow",
            grow_pst  = "branch_b_grow_pst",
            idle_full = "branch_b_idle_full",
            wilt_base = "branch_b_wilt",
            harvest   = "branch_b_harvest"
        };

        var key3 = new CellOffset(-1, 2);
        dictionary[key3] = new StandardCropPlant.AnimSet {
            grow      = "branch_c_grow",
            grow_pst  = "branch_c_grow_pst",
            idle_full = "branch_c_idle_full",
            wilt_base = "branch_c_wilt",
            harvest   = "branch_c_harvest"
        };

        var key4 = new CellOffset(0, 2);
        dictionary[key4] = new StandardCropPlant.AnimSet {
            grow      = "branch_d_grow",
            grow_pst  = "branch_d_grow_pst",
            idle_full = "branch_d_idle_full",
            wilt_base = "branch_d_wilt",
            harvest   = "branch_d_harvest"
        };

        var key5 = new CellOffset(1, 2);
        dictionary[key5] = new StandardCropPlant.AnimSet {
            grow      = "branch_e_grow",
            grow_pst  = "branch_e_grow_pst",
            idle_full = "branch_e_idle_full",
            wilt_base = "branch_e_wilt",
            harvest   = "branch_e_harvest"
        };

        var key6 = new CellOffset(1, 1);
        dictionary[key6] = new StandardCropPlant.AnimSet {
            grow      = "branch_f_grow",
            grow_pst  = "branch_f_grow_pst",
            idle_full = "branch_f_idle_full",
            wilt_base = "branch_f_wilt",
            harvest   = "branch_f_harvest"
        };

        var key7 = new CellOffset(1, 0);
        dictionary[key7] = new StandardCropPlant.AnimSet {
            grow      = "branch_g_grow",
            grow_pst  = "branch_g_grow_pst",
            idle_full = "branch_g_idle_full",
            wilt_base = "branch_g_wilt",
            harvest   = "branch_g_harvest"
        };

        animationSets = dictionary;
        var dictionary2 = new Dictionary<CellOffset, Vector3>();
        key7              = new CellOffset(-1, 0);
        dictionary2[key7] = new Vector3(1f, 0f, 0f);
        key6              = new CellOffset(-1, 1);
        dictionary2[key6] = new Vector3(1f, -1f, 0f);
        key5              = new CellOffset(-1, 2);
        dictionary2[key5] = new Vector3(1f, -2f, 0f);
        key4              = new CellOffset(0, 2);
        dictionary2[key4] = new Vector3(0f, -2f, 0f);
        key3              = new CellOffset(1, 2);
        dictionary2[key3] = new Vector3(-1f, -2f, 0f);
        key2              = new CellOffset(1, 1);
        dictionary2[key2] = new Vector3(-1f, -1f, 0f);
        key               = new CellOffset(1, 0);
        dictionary2[key]  = new Vector3(-1f, 0f, 0f);
        animOffset        = dictionary2;
    }

    public string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var    id             = "ForestTreeBranch";
        string name           = CREATURES.SPECIES.WOOD_TREE.NAME;
        string desc           = CREATURES.SPECIES.WOOD_TREE.DESC;
        var    mass           = 8f;
        var    tier           = DECOR.BONUS.TIER1;
        var    anim           = Assets.GetAnim("tree_kanim");
        var    initialAnim    = "idle_empty";
        var    sceneLayer     = Grid.SceneLayer.BuildingFront;
        var    width          = 1;
        var    height         = 1;
        var    decor          = tier;
        var    additionalTags = new List<Tag> { GameTags.HideFromSpawnTool, GameTags.PlantBranch };
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            anim,
                                                            initialAnim,
                                                            sceneLayer,
                                                            width,
                                                            height,
                                                            decor,
                                                            default(EffectorValues),
                                                            SimHashes.Creature,
                                                            additionalTags,
                                                            298.15f);

        EntityTemplates.ExtendEntityToBasicPlant(gameObject,
                                                 258.15f,
                                                 288.15f,
                                                 313.15f,
                                                 448.15f,
                                                 null,
                                                 true,
                                                 0f,
                                                 0.15f,
                                                 "WoodLog",
                                                 true,
                                                 true,
                                                 false,
                                                 true,
                                                 12000f,
                                                 0f,
                                                 9800f,
                                                 "ForestTreeBranchOriginal",
                                                 CREATURES.SPECIES.WOOD_TREE.NAME);

        gameObject.AddOrGet<TreeBud>();
        gameObject.AddOrGet<StandardCropPlant>();
        gameObject.AddOrGet<BudUprootedMonitor>();
        var def = gameObject.AddOrGetDef<PlantBranch.Def>();
        def.preventStartSMIOnSpawn = true;
        def.onEarlySpawn           = TranslateOldTrunkToNewSystem;
        def.animationSetupCallback = AdjustAnimation;
        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) {
        inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
    }

    public void OnSpawn(GameObject inst) { }

    public void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch) {
        var base_cell   = Grid.PosToCell(trunk);
        var offset_cell = Grid.PosToCell(branch);
        var offset      = Grid.GetOffset(base_cell, offset_cell);
        var component   = branch.GetComponent<StandardCropPlant>();
        var component2  = branch.GetComponent<KBatchedAnimController>();
        component.anims   = animationSets[offset];
        component2.Offset = animOffset[offset];
        component2.Play(component.anims.grow, KAnim.PlayMode.Paused);
        component.RefreshPositionPercent();
    }

    public void TranslateOldTrunkToNewSystem(PlantBranch.Instance smi) {
        var andForgetOldTrunk = smi.GetComponent<TreeBud>().GetAndForgetOldTrunk();
        if (andForgetOldTrunk != null) {
            var smi2 = andForgetOldTrunk.GetSMI<PlantBranchGrower.Instance>();
            smi.SetTrunk(smi2);
        }
    }
}