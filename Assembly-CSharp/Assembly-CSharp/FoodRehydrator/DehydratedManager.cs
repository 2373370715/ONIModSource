using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace FoodRehydrator {
    public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen {
        private static readonly string HASH_FOOD = "food";

        private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate
            = new EventSystem.IntraObjectHandler<DehydratedManager>(delegate(DehydratedManager component, object data) {
                                                                        component.OnCopySettings(data);
                                                                    });

        [Serialize]
        private Tag chosenContent = GameTags.Dehydrated;

        [MyCmpAdd]
        private CopyBuildingSettings copyBuildingSettings;

        private KBatchedAnimController foodKBAC;
        private Storage                packages;
        private MeterController        packagesMeter;
        private Storage                water;

        public Tag ChosenContent {
            get => chosenContent;
            set {
                if (chosenContent != value) {
                    GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
                    chosenContent                                     = value;
                    packages.DropUnlessHasTag(chosenContent);
                    if (chosenContent != GameTags.Dehydrated) {
                        var component = GetComponent<AccessabilityManager>();
                        if (component != null) component.CancelActiveWorkable();
                    }
                }
            }
        }

        public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions() {
            var discoveredResourcesFromTag
                = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);

            var array = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + discoveredResourcesFromTag.Count];
            array[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated,
                                                                           UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD,
                                                                           Def.GetUISprite("icon_category_food"));

            var num = 1;
            foreach (var tag in discoveredResourcesFromTag) {
                array[num]
                    = new FewOptionSideScreen.IFewOptionSideScreen.Option(tag, tag.ProperName(), Def.GetUISprite(tag));

                num++;
            }

            return array;
        }

        public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option) {
            ChosenContent = option.tag;
        }

        public Tag GetSelectedOption() { return chosenContent; }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();
            Subscribe(-905833192, OnCopySettingsDelegate);
        }

        public void SetFabricatedFoodSymbol(Tag material) {
            foodKBAC.gameObject.SetActive(true);
            var prefab = Assets.GetPrefab(material);
            foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
            foodKBAC.Play("object", KAnim.PlayMode.Loop);
        }

        protected override void OnSpawn() {
            base.OnSpawn();
            var components = GetComponents<Storage>();
            Debug.Assert(components.Length == 2);
            packages = components[0];
            water    = components[1];
            packagesMeter = new MeterController(GetComponent<KBatchedAnimController>(),
                                                "meter_target",
                                                "meter",
                                                Meter.Offset.Infront,
                                                Grid.SceneLayer.NoLayer,
                                                Vector3.zero,
                                                "meter_target");

            Subscribe(-1697596308, StorageChangeHandler);
            SetupFoodSymbol();
            packagesMeter.SetPositionPercent(packages.items.Count / 5f);
        }

        public void ConsumeResourcesForRehydration(GameObject package, GameObject food) {
            Debug.Assert(packages.items.Contains(package));
            packages.ConsumeIgnoringDisease(package);
            float               num;
            SimUtil.DiseaseInfo diseaseInfo;
            float               num2;
            water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out num, out diseaseInfo, out num2);
            var component = food.GetComponent<PrimaryElement>();
            if (component != null) {
                component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "rehydrating");
                component.SetMassTemperature(component.Mass, component.Temperature * 0.125f + num2 * 0.875f);
            }
        }

        private void StorageChangeHandler(object obj) {
            if (((GameObject)obj).GetComponent<DehydratedFoodPackage>() != null)
                packagesMeter.SetPositionPercent(packages.items.Count / 5f);
        }

        private void SetupFoodSymbol() {
            var gameObject = Util.NewGameObject(this.gameObject, "food_symbol");
            gameObject.SetActive(false);
            var     component = GetComponent<KBatchedAnimController>();
            bool    flag;
            Vector3 position = component.GetSymbolTransform(HASH_FOOD, out flag).GetColumn(3);
            position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
            gameObject.transform.SetPosition(position);
            foodKBAC             = gameObject.AddComponent<KBatchedAnimController>();
            foodKBAC.AnimFiles   = new[] { Assets.GetAnim("mushbar_kanim") };
            foodKBAC.initialAnim = "object";
            component.SetSymbolVisiblity(HASH_FOOD, false);
            foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
            var kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
            kbatchedAnimTracker.symbol = new HashedString("food");
            kbatchedAnimTracker.offset = Vector3.zero;
        }

        protected void OnCopySettings(object data) {
            var gameObject = data as GameObject;
            if (gameObject != null) {
                var component                        = gameObject.GetComponent<DehydratedManager>();
                if (component != null) ChosenContent = component.ChosenContent;
            }
        }
    }
}