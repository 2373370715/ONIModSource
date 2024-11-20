using System.Collections.Generic;
using HarmonyLib;
using Klei;
#if 泉水
[HarmonyPatch]
public class 泉水 {
    [HarmonyPatch(typeof(GeyserGenericConfig), "GenerateConfigs"), HarmonyPostfix]
    public static void Postfix(ref List<GeyserGenericConfig.GeyserPrefabParams> __result) {
        var list = new List<GeyserGenericConfig.GeyserPrefabParams>();
        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim",
                                                            4,
                                                            2,
                                                            new GeyserConfigurator.GeyserType("slush_water",
                                                             SimHashes.DirtyWater,
                                                             GeyserConfigurator.GeyserShape.Liquid,
                                                             263.15f,
                                                             1000f,
                                                             2000f,
                                                             500f,
                                                             60f,
                                                             1140f,
                                                             0.1f,
                                                             0.9f,
                                                             15000f,
                                                             135000f,
                                                             0.4f,
                                                             0.8f,
                                                             263f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_cool_slush_kanim",
                                                            4,
                                                            2,
                                                            new GeyserConfigurator.GeyserType("slush_salt_water",
                                                             SimHashes.Brine,
                                                             GeyserConfigurator.GeyserShape.Liquid,
                                                             263.15f,
                                                             1000f,
                                                             2000f,
                                                             500f,
                                                             60f,
                                                             1140f,
                                                             0.1f,
                                                             0.9f,
                                                             15000f,
                                                             135000f,
                                                             0.4f,
                                                             0.8f,
                                                             263f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_kanim",
                                                            2,
                                                            4,
                                                            new GeyserConfigurator.GeyserType("steam",
                                                             SimHashes.Steam,
                                                             GeyserConfigurator.GeyserShape.Gas,
                                                             383.15f,
                                                             1000f,
                                                             2000f,
                                                             5f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_big_kanim",
                                                            3,
                                                            3,
                                                            new GeyserConfigurator.GeyserType("big_volcano",
                                                             SimHashes.Magma,
                                                             GeyserConfigurator.GeyserShape.Molten,
                                                             2000f,
                                                             800f,
                                                             1600f,
                                                             150f,
                                                             6000f,
                                                             12000f,
                                                             0.005f,
                                                             0.01f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim",
                                                            3,
                                                            3,
                                                            new GeyserConfigurator.GeyserType("molten_gold",
                                                             SimHashes.MoltenGold,
                                                             GeyserConfigurator.GeyserShape.Molten,
                                                             2900f,
                                                             200f,
                                                             400f,
                                                             150f,
                                                             480f,
                                                             1080f,
                                                             0.016666668f,
                                                             0.1f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_hydrogen_hot_kanim",
                                                            2,
                                                            4,
                                                            new GeyserConfigurator.GeyserType("hot_hydrogen",
                                                             SimHashes.Hydrogen,
                                                             GeyserConfigurator.GeyserShape.Gas,
                                                             773.15f,
                                                             70f,
                                                             140f,
                                                             5f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_hot_kanim",
                                                            4,
                                                            2,
                                                            new GeyserConfigurator.GeyserType("hot_water",
                                                             SimHashes.Water,
                                                             GeyserConfigurator.GeyserShape.Liquid,
                                                             368.15f,
                                                             2000f,
                                                             4000f,
                                                             500f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_methane_kanim",
                                                            2,
                                                            4,
                                                            new GeyserConfigurator.GeyserType("methane",
                                                             SimHashes.Methane,
                                                             GeyserConfigurator.GeyserShape.Gas,
                                                             423.15f,
                                                             70f,
                                                             140f,
                                                             5f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim",
                                                            3,
                                                            3,
                                                            new GeyserConfigurator.GeyserType("molten_iron",
                                                             SimHashes.MoltenIron,
                                                             GeyserConfigurator.GeyserShape.Molten,
                                                             2800f,
                                                             200f,
                                                             400f,
                                                             150f,
                                                             480f,
                                                             1080f,
                                                             0.016666668f,
                                                             0.1f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_copper_kanim",
                                                            3,
                                                            3,
                                                            new GeyserConfigurator.GeyserType("molten_copper",
                                                             SimHashes.MoltenCopper,
                                                             GeyserConfigurator.GeyserShape.Molten,
                                                             2500f,
                                                             200f,
                                                             400f,
                                                             150f,
                                                             480f,
                                                             1080f,
                                                             0.016666668f,
                                                             0.1f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_filthy_kanim",
                                                            4,
                                                            2,
                                                            new GeyserConfigurator.GeyserType("filthy_water",
                                                             SimHashes.DirtyWater,
                                                             GeyserConfigurator.GeyserShape.Liquid,
                                                             303.15f,
                                                             2000f,
                                                             4000f,
                                                             500f).AddDisease(new SimUtil.DiseaseInfo {
                                                                idx   = Db.Get().Diseases.GetIndex("FoodPoisoning"),
                                                                count = 20000
                                                            }),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_small_kanim",
                                                            3,
                                                            3,
                                                            new GeyserConfigurator.GeyserType("small_volcano",
                                                             SimHashes.Magma,
                                                             GeyserConfigurator.GeyserShape.Molten,
                                                             2000f,
                                                             400f,
                                                             800f,
                                                             150f,
                                                             6000f,
                                                             12000f,
                                                             0.005f,
                                                             0.01f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim",
                                                            4,
                                                            2,
                                                            new GeyserConfigurator.GeyserType("salt_water",
                                                             SimHashes.SaltWater,
                                                             GeyserConfigurator.GeyserShape.Liquid,
                                                             368.15f,
                                                             2000f,
                                                             4000f,
                                                             500f),
                                                            true));

        list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_hot_kanim",
                                                            2,
                                                            4,
                                                            new GeyserConfigurator.GeyserType("hot_steam",
                                                             SimHashes.Steam,
                                                             GeyserConfigurator.GeyserShape.Gas,
                                                             773.15f,
                                                             500f,
                                                             1000f,
                                                             5f),
                                                            true));

        list.RemoveAll(geyser => !geyser.geyserType.DlcID.IsNullOrWhiteSpace() &&
                                 !DlcManager.IsContentActive(geyser.geyserType.DlcID));

        __result = list;
    }
}
#endif