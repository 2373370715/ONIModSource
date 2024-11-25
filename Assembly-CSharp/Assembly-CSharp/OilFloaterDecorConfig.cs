using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class OilFloaterDecorConfig : IEntityConfig {
    public const            string    ID                     = "OilfloaterDecor";
    public const            string    BASE_TRAIT_ID          = "OilfloaterDecorBaseTrait";
    public const            string    EGG_ID                 = "OilfloaterDecorEgg";
    public const            SimHashes CONSUME_ELEMENT        = SimHashes.Oxygen;
    private static readonly float     KG_ORE_EATEN_PER_CYCLE = 30f;

    private static readonly float CALORIES_PER_KG_OF_ORE
        = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;

    public static int      EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 2;
    public        string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = CreateOilFloater("OilfloaterDecor",
                                          CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.NAME,
                                          CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC,
                                          "oilfloater_kanim",
                                          false);

        var    eggId             = "OilfloaterDecorEgg";
        string eggName           = CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.EGG_NAME;
        string eggDesc           = CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC;
        var    egg_anim          = "egg_oilfloater_kanim";
        var    egg_MASS          = OilFloaterTuning.EGG_MASS;
        var    baby_id           = "OilfloaterDecorBaby";
        var    fertility_cycles  = 90f;
        var    incubation_cycles = 30f;
        var    egg_CHANCES_DECOR = OilFloaterTuning.EGG_CHANCES_DECOR;
        var    egg_SORT_ORDER    = EGG_SORT_ORDER;
        EntityTemplates.ExtendEntityToFertileCreature(gameObject,
                                                      eggId,
                                                      eggName,
                                                      eggDesc,
                                                      egg_anim,
                                                      egg_MASS,
                                                      baby_id,
                                                      fertility_cycles,
                                                      incubation_cycles,
                                                      egg_CHANCES_DECOR,
                                                      GetDlcIds(),
                                                      egg_SORT_ORDER);

        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }

    public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby) {
        var gameObject = BaseOilFloaterConfig.BaseOilFloater(id,
                                                             name,
                                                             desc,
                                                             anim_file,
                                                             "OilfloaterDecorBaseTrait",
                                                             273.15f,
                                                             323.15f,
                                                             223.15f,
                                                             373.15f,
                                                             is_baby,
                                                             "oxy_");

        gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER6);
        EntityTemplates.ExtendEntityToWildCreature(gameObject, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
        var trait = Db.Get()
        .CreateTrait("OilfloaterDecorBaseTrait",
                     name,
                     name,
                     null,
                     false,
                     null,
                     true,
                     true);

        trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id,
                                        OilFloaterTuning.STANDARD_STOMACH_SIZE,
                                        name));

        trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id,
                                        -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f,
                                        UI.TOOLTIPS.BASE_VALUE));

        trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f,  name));
        trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id,       150f, name));
        return BaseOilFloaterConfig.SetupDiet(gameObject,
                                              SimHashes.Oxygen.CreateTag(),
                                              Tag.Invalid,
                                              CALORIES_PER_KG_OF_ORE,
                                              0f,
                                              null,
                                              0f,
                                              0f);
    }
}