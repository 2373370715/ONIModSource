using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class LightBugOrangeConfig : IEntityConfig {
    public const            string ID                     = "LightBugOrange";
    public const            string BASE_TRAIT_ID          = "LightBugOrangeBaseTrait";
    public const            string EGG_ID                 = "LightBugOrangeEgg";
    private static readonly float  KG_ORE_EATEN_PER_CYCLE = 0.25f;

    private static readonly float CALORIES_PER_KG_OF_ORE
        = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;

    public static int      EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 1;
    public        string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = CreateLightBug("LightBugOrange",
                                        CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.NAME,
                                        CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC,
                                        "lightbug_kanim",
                                        false);

        var    eggId              = "LightBugOrangeEgg";
        string eggName            = CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.EGG_NAME;
        string eggDesc            = CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC;
        var    egg_anim           = "egg_lightbug_kanim";
        var    egg_MASS           = LightBugTuning.EGG_MASS;
        var    baby_id            = "LightBugOrangeBaby";
        var    fertility_cycles   = 15.000001f;
        var    incubation_cycles  = 5f;
        var    egg_CHANCES_ORANGE = LightBugTuning.EGG_CHANCES_ORANGE;
        var    egg_SORT_ORDER     = EGG_SORT_ORDER;
        EntityTemplates.ExtendEntityToFertileCreature(gameObject,
                                                      eggId,
                                                      eggName,
                                                      eggDesc,
                                                      egg_anim,
                                                      egg_MASS,
                                                      baby_id,
                                                      fertility_cycles,
                                                      incubation_cycles,
                                                      egg_CHANCES_ORANGE,
                                                      GetDlcIds(),
                                                      egg_SORT_ORDER);

        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { BaseLightBugConfig.SetupLoopingSounds(inst); }

    public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby) {
        var prefab = BaseLightBugConfig.BaseLightBug(id,
                                                     name,
                                                     desc,
                                                     anim_file,
                                                     "LightBugOrangeBaseTrait",
                                                     LIGHT2D.LIGHTBUG_COLOR_ORANGE,
                                                     DECOR.BONUS.TIER6,
                                                     is_baby,
                                                     "org_");

        EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
        var trait = Db.Get()
        .CreateTrait("LightBugOrangeBaseTrait",
                     name,
                     name,
                     null,
                     false,
                     null,
                     true,
                     true);

        trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id,
                                        LightBugTuning.STANDARD_STOMACH_SIZE,
                                        name));

        trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id,
                                        -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f,
                                        UI.TOOLTIPS.BASE_VALUE));

        trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f,  name));
        trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id,       25f, name));
        var hashSet = new HashSet<Tag>();
        hashSet.Add(TagManager.Create(MushroomConfig.ID));
        hashSet.Add(TagManager.Create("FriedMushroom"));
        hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
        if (DlcManager.IsContentSubscribed("DLC2_ID")) hashSet.Add(TagManager.Create("CookedPikeapple"));
        hashSet.Add(SimHashes.Phosphorite.CreateTag());
        return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, CALORIES_PER_KG_OF_ORE);
    }
}