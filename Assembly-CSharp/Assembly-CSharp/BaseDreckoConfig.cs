﻿using Klei.AI;
using TUNING;
using UnityEngine;

public static class BaseDreckoConfig {
    public static GameObject BaseDrecko(string id,
                                        string name,
                                        string desc,
                                        string anim_file,
                                        string trait_id,
                                        bool   is_baby,
                                        string symbol_override_prefix,
                                        float  warnLowTemp,
                                        float  warnHighTemp,
                                        float  lethalLowTemp,
                                        float  lethalHighTemp) {
        var mass = 200f;
        var tier = DECOR.BONUS.TIER0;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim(anim_file),
                                                            "idle_loop",
                                                            Grid.SceneLayer.Creatures,
                                                            1,
                                                            1,
                                                            tier,
                                                            default(EffectorValues),
                                                            SimHashes.Creature,
                                                            null,
                                                            (warnLowTemp + warnHighTemp) / 2f);

        var component = gameObject.GetComponent<KPrefabID>();
        component.AddTag(GameTags.Creatures.Walker);
        component.prefabInitFn += delegate(GameObject inst) {
                                      inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
                                  };

        var navGridName          = "DreckoNavGrid";
        if (is_baby) navGridName = "DreckoBabyNavGrid";
        EntityTemplates.ExtendEntityToBasicCreature(gameObject,
                                                    FactionManager.FactionID.Pest,
                                                    trait_id,
                                                    navGridName,
                                                    NavType.Floor,
                                                    32,
                                                    1f,
                                                    "Meat",
                                                    2,
                                                    true,
                                                    false,
                                                    warnLowTemp,
                                                    warnHighTemp,
                                                    lethalLowTemp,
                                                    lethalHighTemp);

        if (!string.IsNullOrEmpty(symbol_override_prefix))
            gameObject.AddOrGet<SymbolOverrideController>()
                      .ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbol_override_prefix);

        var pickupable = gameObject.AddOrGet<Pickupable>();
        var sortOrder  = CREATURES.SORTING.CRITTER_ORDER["Drecko"];
        pickupable.sortOrder = sortOrder;
        gameObject.AddOrGet<Trappable>();
        gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
        gameObject.AddOrGet<LoopingSounds>();
        gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
        gameObject.AddWeapon(1f, 1f);
        EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true);
        var chore_table = new ChoreTable.Builder().Add(new DeathStates.Def())
                                                  .Add(new AnimInterruptStates.Def())
                                                  .Add(new GrowUpStates.Def(), is_baby)
                                                  .Add(new TrappedStates.Def())
                                                  .Add(new IncubatingStates.Def(), is_baby)
                                                  .Add(new BaggedStates.Def())
                                                  .Add(new FallStates.Def())
                                                  .Add(new StunnedStates.Def())
                                                  .Add(new DrowningStates.Def())
                                                  .Add(new DebugGoToStates.Def())
                                                  .Add(new FleeStates.Def())
                                                  .Add(new AttackStates.Def(), !is_baby)
                                                  .PushInterruptGroup()
                                                  .Add(new FixedCaptureStates.Def())
                                                  .Add(new RanchedStates.Def(), !is_baby)
                                                  .Add(new LayEggStates.Def(),  !is_baby)
                                                  .Add(new EatStates.Def())
                                                  .Add(new DrinkMilkStates.Def { shouldBeBehindMilkTank = is_baby })
                                                  .Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop,
                                                                               false,
                                                                               "poop",
                                                                               STRINGS.CREATURES.STATUSITEMS
                                                                                   .EXPELLING_SOLID.NAME,
                                                                               STRINGS.CREATURES.STATUSITEMS
                                                                                   .EXPELLING_SOLID.TOOLTIP))
                                                  .Add(new CallAdultStates.Def(),    is_baby)
                                                  .Add(new CritterCondoStates.Def(), !is_baby)
                                                  .PopInterruptGroup()
                                                  .Add(new CreatureSleepStates.Def())
                                                  .Add(new IdleStates.Def { customIdleAnim = CustomIdleAnim });

        EntityTemplates.AddCreatureBrain(gameObject,
                                         chore_table,
                                         GameTags.Creatures.Species.DreckoSpecies,
                                         symbol_override_prefix);

        return gameObject;
    }

    private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim) {
        var offset         = new CellOffset(0, -1);
        var facing         = smi.GetComponent<Facing>().GetFacing();
        var currentNavType = smi.GetComponent<Navigator>().CurrentNavType;
        if (currentNavType != NavType.Floor) {
            if (currentNavType == NavType.Ceiling) offset = facing ? new CellOffset(1, 1) : new CellOffset(-1, 1);
        } else
            offset = facing ? new CellOffset(1, -1) : new CellOffset(-1, -1);

        HashedString result = "idle_loop";
        var          num    = Grid.OffsetCell(Grid.PosToCell(smi), offset);
        if (Grid.IsValidCell(num) && !Grid.Solid[num]) {
            pre_anim = "idle_loop_hang_pre";
            result   = "idle_loop_hang";
        }

        return result;
    }
}