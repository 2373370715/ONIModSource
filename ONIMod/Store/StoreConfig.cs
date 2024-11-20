using System;
using System.Collections.Generic;
using PeterHan.PLib.Buildings;
using TUNING;
using UnityEngine;

namespace Store {
    public sealed class StoreConfig : IBuildingConfig {
        internal static PBuilding CreateBuilding() {
            var pbuilding = new PBuilding("store", "store");
            pbuilding.Animation = "storagelocker_kanim";

            // 在实用里
            pbuilding.Category = "Utilities";

            // 建造时间
            pbuilding.ConstructionTime = 1f;
            pbuilding.AudioCategory    = "Metal";

            // 可以泡水里
            pbuilding.Floods      = true;
            pbuilding.Description = null;
            pbuilding.EffectText  = null;
            pbuilding.Width       = 1;
            pbuilding.Height      = 2;
            pbuilding.HP          = 30;
            pbuilding.Ingredients.Add(new BuildIngredient("BuildableRaw&Metal", 10f));
            pbuilding.OverheatTemperature = 1273.15f;
            pbuilding.HeatGeneration      = 0f;
            Store                         = pbuilding;
            return pbuilding;
        }

        public override BuildingDef CreateBuildingDef() { return Store.CreateDef(); }

        /// <summary>
        /// 配置建筑模板的函数，用于在游戏对象创建时设置特定的属性和组件。
        /// </summary>
        /// <param name="go">待配置的游戏对象。</param>
        /// <param name="prefabTag">游戏对象的预设标签。</param>
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag) {
            var store = Store;
            if (store != null) { store.ConfigureBuildingTemplate(go); }

            base.ConfigureBuildingTemplate(go, prefabTag);
            
            var storage = go.AddComponent<Storage>();
            storage.showInUI         = true;
            storage.allowItemRemoval = true;
            storage.showDescriptor   = true;

            // 存入物体隔热，密封
            storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
                Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Seal, Storage.StoredItemModifier.Insulate
            });

            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
            storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);

            go.AddComponent<StoreTem>();

            Prioritizable.AddRef(go);
        }

        public override void DoPostConfigureComplete(GameObject go) {
            var teleVault = Store;
            if (teleVault != null) { teleVault.DoPostConfigureComplete(go); }

            go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject gameObject) {
                                                             new StoreInstance(gameObject.GetComponent<StoreTem>())
                                                                 .StartSM();
                                                         };
        }

        internal const  string    ID = "Store";
        internal static PBuilding Store;
    }
}