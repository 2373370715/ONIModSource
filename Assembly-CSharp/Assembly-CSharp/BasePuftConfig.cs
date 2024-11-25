using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

public static class BasePuftConfig {
    public static GameObject BasePuft(string id,
                                      string name,
                                      string desc,
                                      string traitId,
                                      string anim_file,
                                      bool   is_baby,
                                      string symbol_override_prefix,
                                      float  warningLowTemperature,
                                      float  warningHighTemperature,
                                      float  lethalLowTemperature,
                                      float  lethalHighTemperature) {
        var mass = 50f;
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
                                                            tier);

        EntityTemplates.ExtendEntityToBasicCreature(gameObject,
                                                    FactionManager.FactionID.Prey,
                                                    traitId,
                                                    "FlyerNavGrid1x1",
                                                    NavType.Hover,
                                                    32,
                                                    2f,
                                                    "Meat",
                                                    1,
                                                    true,
                                                    true,
                                                    warningLowTemperature,
                                                    warningHighTemperature,
                                                    lethalLowTemperature,
                                                    lethalHighTemperature);

        if (!string.IsNullOrEmpty(symbol_override_prefix))
            gameObject.AddOrGet<SymbolOverrideController>()
                      .ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbol_override_prefix);

        var pickupable = gameObject.AddOrGet<Pickupable>();
        var sortOrder  = CREATURES.SORTING.CRITTER_ORDER["Puft"];
        pickupable.sortOrder = sortOrder;
        var component = gameObject.GetComponent<KPrefabID>();
        component.AddTag(GameTags.Creatures.Flyer);
        component.prefabInitFn += delegate(GameObject inst) {
                                      inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
                                  };

        gameObject.AddOrGet<LoopingSounds>();
        gameObject.AddOrGet<Trappable>();
        gameObject.AddOrGetDef<LureableMonitor.Def>().lures
            = new[] { GameTags.SlimeMold, GameTags.Creatures.FlyersLure };

        gameObject.AddOrGetDef<ThreatMonitor.Def>();
        gameObject.AddOrGetDef<SubmergedMonitor.Def>();
        SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_idle",   NOISE_POLLUTION.CREATURES.TIER2);
        SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_air_intake",   NOISE_POLLUTION.CREATURES.TIER4);
        SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_toot",         NOISE_POLLUTION.CREATURES.TIER5);
        SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_air_inflated", NOISE_POLLUTION.CREATURES.TIER5);
        SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_die",    NOISE_POLLUTION.CREATURES.TIER5);
        SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_hurt",   NOISE_POLLUTION.CREATURES.TIER5);
        EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true);
        var inhaleSound          = "Puft_air_intake";
        if (is_baby) inhaleSound = "PuftBaby_air_intake";
        var chore_table = new ChoreTable.Builder().Add(new DeathStates.Def())
                                                  .Add(new AnimInterruptStates.Def())
                                                  .Add(new GrowUpStates.Def(),     is_baby)
                                                  .Add(new IncubatingStates.Def(), is_baby)
                                                  .Add(new TrappedStates.Def())
                                                  .Add(new BaggedStates.Def())
                                                  .Add(new StunnedStates.Def())
                                                  .Add(new DebugGoToStates.Def())
                                                  .Add(new DrowningStates.Def())
                                                  .PushInterruptGroup()
                                                  .Add(new CreatureSleepStates.Def())
                                                  .Add(new FixedCaptureStates.Def())
                                                  .Add(new RanchedStates.Def(), !is_baby)
                                                  .Add(new UpTopPoopStates.Def())
                                                  .Add(new LayEggStates.Def(), !is_baby)
                                                  .Add(new InhaleStates.Def { inhaleSound               = inhaleSound })
                                                  .Add(new DrinkMilkStates.Def { shouldBeBehindMilkTank = !is_baby })
                                                  .Add(new MoveToLureStates.Def())
                                                  .Add(new CallAdultStates.Def(), is_baby)
                                                  .Add(new CritterCondoStates.Def { working_anim = "cc_working_puft" },
                                                       !is_baby)
                                                  .PopInterruptGroup()
                                                  .Add(new IdleStates.Def { customIdleAnim = CustomIdleAnim });

        EntityTemplates.AddCreatureBrain(gameObject,
                                         chore_table,
                                         GameTags.Creatures.Species.PuftSpecies,
                                         symbol_override_prefix);

        gameObject.AddOrGetDef<CritterCondoInteractMontior.Def>().condoPrefabTag = "AirBorneCritterCondo";
        return gameObject;
    }

    public static GameObject SetupDiet(GameObject prefab,
                                       Tag        consumed_tag,
                                       Tag        producedTag,
                                       float      caloriesPerKg,
                                       float      producedConversionRate,
                                       string     diseaseId,
                                       float      diseasePerKgProduced,
                                       float      minPoopSizeInKg) {
        Diet.Info[] diet_infos = {
            new Diet.Info(new HashSet<Tag> { consumed_tag },
                          producedTag,
                          caloriesPerKg,
                          producedConversionRate,
                          diseaseId,
                          diseasePerKgProduced)
        };

        return SetupDiet(prefab, diet_infos, caloriesPerKg, minPoopSizeInKg);
    }

    public static GameObject SetupDiet(GameObject  prefab,
                                       Diet.Info[] diet_infos,
                                       float       caloriesPerKg,
                                       float       minPoopSizeInKg) {
        var diet = new Diet(diet_infos);
        var def  = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
        def.diet                                                   = diet;
        def.minConsumedCaloriesBeforePooping                       = minPoopSizeInKg * caloriesPerKg;
        prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>().diet = diet;
        return prefab;
    }

    private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim) {
        var smi2 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
        return smi2 != null && smi2.stomach.IsReadyToPoop() ? "idle_loop_full" : "idle_loop";
    }

    public static void OnSpawn(GameObject inst) {
        var component = inst.GetComponent<Navigator>();
        component.transitionDriver.overrideLayers.Add(new FullPuftTransitionLayer(component));
    }
}