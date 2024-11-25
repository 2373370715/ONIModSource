using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

public abstract class OverlayModes {
    public enum BringToFrontLayerSetting {
        None,
        Constant,
        Conditional
    }

    public class GasConduits : ConduitMode {
        public static readonly HashedString ID = "GasConduit";
        public GasConduits() : base(OverlayScreen.GasVentIDs) { }
        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "GasVent"; }
    }

    public class LiquidConduits : ConduitMode {
        public static readonly HashedString ID = "LiquidConduit";
        public LiquidConduits() : base(OverlayScreen.LiquidVentIDs) { }
        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "LiquidVent"; }
    }

    public abstract class ConduitMode : Mode {
        private readonly int                       cameraLayerMask;
        private readonly int                       conduitTargetLayer;
        private readonly HashSet<UtilityNetwork>   connectedNetworks = new HashSet<UtilityNetwork>();
        private readonly HashSet<SaveLoadRoot>     layerTargets      = new HashSet<SaveLoadRoot>();
        private readonly int                       objectTargetLayer;
        private          UniformGrid<SaveLoadRoot> partition;
        private readonly int                       selectionMask;
        private readonly ICollection<Tag>          targetIDs;
        private readonly List<int>                 visited = new List<int>();

        public ConduitMode(ICollection<Tag> ids) {
            objectTargetLayer  = LayerMask.NameToLayer("MaskedOverlayBG");
            conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask    = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask      = cameraLayerMask;
            targetIDs          = ids;
        }

        public override void Enable() {
            RegisterSaveLoadListeners();
            partition               =  PopulatePartition<SaveLoadRoot>(targetIDs);
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
            GridCompositor.Instance.ToggleMinor(false);
            base.Enable();
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (targetIDs.Contains(saveLoadTag)) partition.Add(item);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            if (layerTargets.Contains(item)) layerTargets.Remove(item);
            partition.Remove(item);
        }

        public override void Disable() {
            foreach (var saveLoadRoot in layerTargets) {
                var defaultDepth = GetDefaultDepth(saveLoadRoot);
                var position     = saveLoadRoot.transform.GetPosition();
                position.z = defaultDepth;
                saveLoadRoot.transform.SetPosition(position);
                var componentsInChildren = saveLoadRoot.GetComponentsInChildren<KBatchedAnimController>();
                for (var i = 0; i < componentsInChildren.Length; i++) TriggerResorting(componentsInChildren[i]);
            }

            ResetDisplayValues(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            UnregisterSaveLoadListeners();
            partition.Clear();
            layerTargets.Clear();
            GridCompositor.Instance.ToggleMinor(false);
            base.Disable();
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets,
                                   vector2I,
                                   vector2I2,
                                   delegate(SaveLoadRoot root) {
                                       if (root == null) return;

                                       var defaultDepth = GetDefaultDepth(root);
                                       var position     = root.transform.GetPosition();
                                       position.z = defaultDepth;
                                       root.transform.SetPosition(position);
                                       var componentsInChildren
                                           = root.GetComponentsInChildren<KBatchedAnimController>();

                                       for (var i = 0; i < componentsInChildren.Length; i++)
                                           TriggerResorting(componentsInChildren[i]);
                                   });

            foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                             new Vector2(vector2I2.x, vector2I2.y))) {
                var saveLoadRoot = (SaveLoadRoot)obj;
                if (saveLoadRoot.GetComponent<Conduit>() != null)
                    AddTargetIfVisible(saveLoadRoot, vector2I, vector2I2, layerTargets, conduitTargetLayer);
                else
                    AddTargetIfVisible(saveLoadRoot,
                                       vector2I,
                                       vector2I2,
                                       layerTargets,
                                       objectTargetLayer,
                                       delegate(SaveLoadRoot root) {
                                           var position   = root.transform.GetPosition();
                                           var z          = position.z;
                                           var component3 = root.GetComponent<KPrefabID>();
                                           if (component3 != null) {
                                               if (component3.HasTag(GameTags.OverlayInFrontOfConduits))
                                                   z = Grid.GetLayerZ(ViewMode() == LiquidConduits.ID
                                                                          ? Grid.SceneLayer.LiquidConduits
                                                                          : Grid.SceneLayer.GasConduits) -
                                                       0.2f;
                                               else if (component3.HasTag(GameTags.OverlayBehindConduits))
                                                   z = Grid.GetLayerZ(ViewMode() == LiquidConduits.ID
                                                                          ? Grid.SceneLayer.LiquidConduits
                                                                          : Grid.SceneLayer.GasConduits) +
                                                       0.2f;
                                           }

                                           position.z = z;
                                           root.transform.SetPosition(position);
                                           var componentsInChildren
                                               = root.GetComponentsInChildren<KBatchedAnimController>();

                                           for (var i = 0; i < componentsInChildren.Length; i++)
                                               TriggerResorting(componentsInChildren[i]);
                                       });
            }

            GameObject gameObject = null;
            if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
                gameObject = SelectTool.Instance.hover.gameObject;

            connectedNetworks.Clear();
            var num = 1f;
            if (gameObject != null) {
                var component = gameObject.GetComponent<IBridgedNetworkItem>();
                if (component != null) {
                    var networkCell = component.GetNetworkCell();
                    var mgr = ViewMode() == LiquidConduits.ID
                                  ? Game.Instance.liquidConduitSystem
                                  : Game.Instance.gasConduitSystem;

                    visited.Clear();
                    FindConnectedNetworks(networkCell, mgr, connectedNetworks, visited);
                    visited.Clear();
                    num = ModeUtil.GetHighlightScale();
                }
            }

            var conduitVisInfo = ViewMode() == LiquidConduits.ID
                                     ? Game.Instance.liquidConduitVisInfo
                                     : Game.Instance.gasConduitVisInfo;

            foreach (var saveLoadRoot2 in layerTargets)
                if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<IBridgedNetworkItem>() != null) {
                    var     def = saveLoadRoot2.GetComponent<Building>().Def;
                    Color32 colorByName;
                    if (def.ThermalConductivity == 1f)
                        colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayTintName);
                    else if (def.ThermalConductivity < 1f)
                        colorByName
                            = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayInsulatedTintName);
                    else
                        colorByName
                            = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayRadiantTintName);

                    if (connectedNetworks.Count > 0) {
                        var component2 = saveLoadRoot2.GetComponent<IBridgedNetworkItem>();
                        if (component2 != null && component2.IsConnectedToNetworks(connectedNetworks)) {
                            colorByName.r = (byte)(colorByName.r * num);
                            colorByName.g = (byte)(colorByName.g * num);
                            colorByName.b = (byte)(colorByName.b * num);
                        }
                    }

                    saveLoadRoot2.GetComponent<KBatchedAnimController>().TintColour = colorByName;
                }
        }

        private void TriggerResorting(KBatchedAnimController kbac) {
            if (kbac.enabled) {
                kbac.enabled = false;
                kbac.enabled = true;
            }
        }

        private void FindConnectedNetworks(int                         cell,
                                           IUtilityNetworkMgr          mgr,
                                           ICollection<UtilityNetwork> networks,
                                           List<int>                   visited) {
            if (visited.Contains(cell)) return;

            visited.Add(cell);
            var networkForCell = mgr.GetNetworkForCell(cell);
            if (networkForCell != null) {
                networks.Add(networkForCell);
                var connections = mgr.GetConnections(cell, false);
                if ((connections & UtilityConnections.Right) != 0)
                    FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Left) != 0)
                    FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Up) != 0)
                    FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Down) != 0)
                    FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);

                var endpoint = mgr.GetEndpoint(cell);
                if (endpoint != null) {
                    var networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
                    if (networkItem != null) {
                        var component = networkItem.GameObject.GetComponent<IBridgedNetworkItem>();
                        if (component != null) component.AddNetworks(networks);
                    }
                }
            }
        }
    }

    public class Crop : BasePlantMode {
        public static readonly HashedString              ID = "Crop";
        private                int                       freeHarvestableNotificationIdx;
        private readonly       List<GameObject>          harvestableNotificationList = new List<GameObject>();
        private readonly       GameObject                harvestableNotificationPrefab;
        private readonly       ColorHighlightCondition[] highlightConditions;
        private readonly       Canvas                    uiRoot;
        private readonly       List<UpdateCropInfo>      updateCropInfo = new List<UpdateCropInfo>();

        public Crop(Canvas ui_root, GameObject harvestable_notification_prefab) {
            var array = new ColorHighlightCondition[3];
            array[0] = new ColorHighlightCondition(h => GlobalAssets.Instance.colorSet.cropHalted,
                                                   delegate(KMonoBehaviour h) {
                                                       var component = h.GetComponent<WiltCondition>();
                                                       return component != null && component.IsWilting();
                                                   });

            array[1] = new ColorHighlightCondition(h => GlobalAssets.Instance.colorSet.cropGrowing,
                                                   h => !(h as HarvestDesignatable).CanBeHarvested());

            array[2] = new ColorHighlightCondition(h => GlobalAssets.Instance.colorSet.cropGrown,
                                                   h => (h as HarvestDesignatable).CanBeHarvested());

            highlightConditions = array;
            base..ctor(OverlayScreen.HarvestableIDs);
            uiRoot                        = ui_root;
            harvestableNotificationPrefab = harvestable_notification_prefab;
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Harvest"; }

        public override List<LegendEntry> GetCustomLegendData() {
            return new List<LegendEntry> {
                new LegendEntry(UI.OVERLAYS.CROP.FULLY_GROWN,
                                UI.OVERLAYS.CROP.TOOLTIPS.FULLY_GROWN,
                                GlobalAssets.Instance.colorSet.cropGrown),
                new LegendEntry(UI.OVERLAYS.CROP.GROWING,
                                UI.OVERLAYS.CROP.TOOLTIPS.GROWING,
                                GlobalAssets.Instance.colorSet.cropGrowing),
                new LegendEntry(UI.OVERLAYS.CROP.GROWTH_HALTED,
                                UI.OVERLAYS.CROP.TOOLTIPS.GROWTH_HALTED,
                                GlobalAssets.Instance.colorSet.cropHalted)
            };
        }

        public override void Update() {
            this.updateCropInfo.Clear();
            freeHarvestableNotificationIdx = 0;
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                             new Vector2(vector2I2.x, vector2I2.y))) {
                var instance = (HarvestDesignatable)obj;
                AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
            }

            foreach (var harvestDesignatable in layerTargets) {
                var vector2I3 = Grid.PosToXY(harvestDesignatable.transform.GetPosition());
                if (vector2I <= vector2I3 && vector2I3 <= vector2I2) AddCropUI(harvestDesignatable);
            }

            foreach (var updateCropInfo in this.updateCropInfo)
                updateCropInfo.harvestableUI.GetComponent<HarvestableOverlayWidget>()
                              .Refresh(updateCropInfo.harvestable);

            for (var i = freeHarvestableNotificationIdx; i < harvestableNotificationList.Count; i++)
                if (harvestableNotificationList[i].activeSelf)
                    harvestableNotificationList[i].SetActive(false);

            UpdateHighlightTypeOverlay(vector2I,
                                       vector2I2,
                                       layerTargets,
                                       targetIDs,
                                       highlightConditions,
                                       BringToFrontLayerSetting.Constant,
                                       targetLayer);

            base.Update();
        }

        public override void Disable() {
            DisableHarvestableUINotifications();
            base.Disable();
        }

        private void DisableHarvestableUINotifications() {
            freeHarvestableNotificationIdx = 0;
            foreach (var gameObject in harvestableNotificationList) gameObject.SetActive(false);
            updateCropInfo.Clear();
        }

        public GameObject GetFreeCropUI() {
            GameObject gameObject;
            if (freeHarvestableNotificationIdx < harvestableNotificationList.Count) {
                gameObject = harvestableNotificationList[freeHarvestableNotificationIdx];
                if (!gameObject.gameObject.activeSelf) gameObject.gameObject.SetActive(true);
                freeHarvestableNotificationIdx++;
            } else {
                gameObject = Util.KInstantiateUI(harvestableNotificationPrefab.gameObject, uiRoot.transform.gameObject);
                harvestableNotificationList.Add(gameObject);
                freeHarvestableNotificationIdx++;
            }

            return gameObject;
        }

        private void AddCropUI(HarvestDesignatable harvestable) {
            var     freeCropUI = GetFreeCropUI();
            var     item       = new UpdateCropInfo(harvestable, freeCropUI);
            Vector3 b          = Grid.CellToPos(Grid.PosToCell(harvestable), 0.5f, -1.25f, 0f) + harvestable.iconOffset;
            freeCropUI.GetComponent<RectTransform>().SetPosition(Vector3.up                    + b);
            updateCropInfo.Add(item);
        }

        private struct UpdateCropInfo {
            public UpdateCropInfo(HarvestDesignatable harvestable, GameObject harvestableUI) {
                this.harvestable   = harvestable;
                this.harvestableUI = harvestableUI;
            }

            public readonly HarvestDesignatable harvestable;
            public readonly GameObject          harvestableUI;
        }
    }

    public class Harvest : BasePlantMode {
        public static readonly HashedString              ID = "HarvestWhenReady";
        private readonly       ColorHighlightCondition[] highlightConditions;

        public Harvest() {
            var array = new ColorHighlightCondition[1];
            array[0] = new ColorHighlightCondition(harvestable => new Color(0.65f, 0.65f, 0.65f, 0.65f),
                                                   harvestable => true);

            highlightConditions = array;
            base..ctor(OverlayScreen.HarvestableIDs);
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Harvest"; }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                             new Vector2(vector2I2.x, vector2I2.y))) {
                var instance = (HarvestDesignatable)obj;
                AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
            }

            UpdateHighlightTypeOverlay(vector2I,
                                       vector2I2,
                                       layerTargets,
                                       targetIDs,
                                       highlightConditions,
                                       BringToFrontLayerSetting.Constant,
                                       targetLayer);

            base.Update();
        }
    }

    public abstract class BasePlantMode : Mode {
        private readonly int                              cameraLayerMask;
        protected        HashSet<HarvestDesignatable>     layerTargets = new HashSet<HarvestDesignatable>();
        protected        UniformGrid<HarvestDesignatable> partition;
        private readonly int                              selectionMask;
        protected        ICollection<Tag>                 targetIDs;
        protected        int                              targetLayer;

        public BasePlantMode(ICollection<Tag> ids) {
            targetLayer     = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask   = LayerMask.GetMask("MaskedOverlay");
            targetIDs       = ids;
        }

        public override void Enable() {
            RegisterSaveLoadListeners();
            partition               =  PopulatePartition<HarvestDesignatable>(targetIDs);
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (!targetIDs.Contains(saveLoadTag)) return;

            var component = item.GetComponent<HarvestDesignatable>();
            if (component == null) return;

            partition.Add(component);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            var component = item.GetComponent<HarvestDesignatable>();
            if (component == null) return;

            if (layerTargets.Contains(component)) layerTargets.Remove(component);
            partition.Remove(component);
        }

        public override void Disable() {
            UnregisterSaveLoadListeners();
            DisableHighlightTypeOverlay(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            partition.Clear();
            layerTargets.Clear();
            SelectTool.Instance.ClearLayerMask();
        }
    }

    public class Decor : Mode {
        public static readonly HashedString               ID = "Decor";
        private readonly       int                        cameraLayerMask;
        private readonly       ColorHighlightCondition[]  highlightConditions;
        private readonly       HashSet<DecorProvider>     layerTargets = new HashSet<DecorProvider>();
        private                UniformGrid<DecorProvider> partition;
        private readonly       HashSet<Tag>               targetIDs = new HashSet<Tag>();
        private readonly       int                        targetLayer;
        private readonly       List<DecorProvider>        workingTargets = new List<DecorProvider>();

        public Decor() {
            var array = new ColorHighlightCondition[1];
            array[0] = new ColorHighlightCondition(delegate(KMonoBehaviour dp) {
                                                       var black = Color.black;
                                                       var b     = Color.black;
                                                       if (dp != null) {
                                                           var cell = Grid.PosToCell(CameraController.Instance
                                                               .baseCamera.ScreenToWorldPoint(KInputManager
                                                                   .GetMousePos()));

                                                           var decorForCell
                                                               = (dp as DecorProvider).GetDecorForCell(cell);

                                                           if (decorForCell > 0f)
                                                               b = GlobalAssets.Instance.colorSet
                                                                               .decorHighlightPositive;
                                                           else if (decorForCell < 0f)
                                                               b = GlobalAssets.Instance.colorSet
                                                                               .decorHighlightNegative;
                                                           else if (dp.GetComponent<MonumentPart>() != null &&
                                                                    dp.GetComponent<MonumentPart>()
                                                                      .IsMonumentCompleted())
                                                               foreach (var gameObject in AttachableBuilding
                                                                            .GetAttachedNetwork(dp.GetComponent<
                                                                                AttachableBuilding>())) {
                                                                   decorForCell = gameObject
                                                                       .GetComponent<DecorProvider>()
                                                                       .GetDecorForCell(cell);

                                                                   if (decorForCell > 0f) {
                                                                       b = GlobalAssets.Instance.colorSet
                                                                           .decorHighlightPositive;

                                                                       break;
                                                                   }

                                                                   if (decorForCell < 0f) {
                                                                       b = GlobalAssets.Instance.colorSet
                                                                           .decorHighlightNegative;

                                                                       break;
                                                                   }
                                                               }
                                                       }

                                                       return Color.Lerp(black, b, 0.85f);
                                                   },
                                                   dp =>
                                                       SelectToolHoverTextCard.highlightedObjects
                                                                              .Contains(dp.gameObject));

            highlightConditions = array;
            base..ctor();
            targetLayer     = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Decor"; }

        public override List<LegendEntry> GetCustomLegendData() {
            return new List<LegendEntry> {
                new LegendEntry(UI.OVERLAYS.DECOR.HIGHDECOR,
                                UI.OVERLAYS.DECOR.TOOLTIPS.HIGHDECOR,
                                GlobalAssets.Instance.colorSet.decorPositive),
                new LegendEntry(UI.OVERLAYS.DECOR.LOWDECOR,
                                UI.OVERLAYS.DECOR.TOOLTIPS.LOWDECOR,
                                GlobalAssets.Instance.colorSet.decorNegative)
            };
        }

        public override void Enable() {
            RegisterSaveLoadListeners();
            var prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<DecorProvider>();
            targetIDs.UnionWith(prefabTagsWithComponent);
            foreach (var item in new[] {
                         new Tag("Tile"),
                         new Tag("SnowTile"),
                         new Tag("WoodTile"),
                         new Tag("MeshTile"),
                         new Tag("InsulationTile"),
                         new Tag("GasPermeableMembrane"),
                         new Tag("CarpetTile")
                     })
                targetIDs.Remove(item);

            foreach (var item2 in OverlayScreen.GasVentIDs) targetIDs.Remove(item2);
            foreach (var item3 in OverlayScreen.LiquidVentIDs) targetIDs.Remove(item3);
            partition               =  PopulatePartition<DecorProvider>(targetIDs);
            Camera.main.cullingMask |= cameraLayerMask;
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                         new Vector2(vector2I2.x, vector2I2.y),
                                         workingTargets);

            for (var i = 0; i < workingTargets.Count; i++) {
                var instance = workingTargets[i];
                AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
            }

            UpdateHighlightTypeOverlay(vector2I,
                                       vector2I2,
                                       layerTargets,
                                       targetIDs,
                                       highlightConditions,
                                       BringToFrontLayerSetting.Conditional,
                                       targetLayer);

            workingTargets.Clear();
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (targetIDs.Contains(saveLoadTag)) {
                var component = item.GetComponent<DecorProvider>();
                if (component != null) partition.Add(component);
            }
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            var component = item.GetComponent<DecorProvider>();
            if (component != null) {
                if (layerTargets.Contains(component)) layerTargets.Remove(component);
                partition.Remove(component);
            }
        }

        public override void Disable() {
            DisableHighlightTypeOverlay(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            UnregisterSaveLoadListeners();
            partition.Clear();
            layerTargets.Clear();
        }
    }

    public class Disease : Mode {
        public static readonly HashedString            ID = "Disease";
        private readonly       int                     cameraLayerMask;
        private readonly       GameObject              diseaseOverlayPrefab;
        private readonly       List<GameObject>        diseaseUIList = new List<GameObject>();
        private readonly       Canvas                  diseaseUIParent;
        private                int                     freeDiseaseUI;
        private readonly       HashSet<KMonoBehaviour> layerTargets      = new HashSet<KMonoBehaviour>();
        private readonly       HashSet<KMonoBehaviour> privateTargets    = new HashSet<KMonoBehaviour>();
        private readonly       List<KMonoBehaviour>    queuedAdds        = new List<KMonoBehaviour>();
        private readonly       List<UpdateDiseaseInfo> updateDiseaseInfo = new List<UpdateDiseaseInfo>();

        public Disease(Canvas diseaseUIParent, GameObject diseaseOverlayPrefab) {
            this.diseaseUIParent      = diseaseUIParent;
            this.diseaseOverlayPrefab = diseaseOverlayPrefab;
            legendFilters             = CreateDefaultFilters();
            cameraLayerMask           = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
        }

        private static float CalculateHUE(Color32 colour) {
            var b      = Math.Max(colour.r, Math.Max(colour.g, colour.b));
            var b2     = Math.Min(colour.r, Math.Min(colour.g, colour.b));
            var result = 0f;
            var num    = b - b2;
            if (num == 0)
                result = 0f;
            else if (b == colour.r)
                result = (colour.g - colour.b) / (float)num % 6f;
            else if (b == colour.g)
                result                     = (colour.b - colour.r) / (float)num + 2f;
            else if (b == colour.b) result = (colour.r - colour.g) / (float)num + 4f;

            return result;
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Disease"; }

        public override void Enable() {
            Infrared.Instance.SetMode(Infrared.Mode.Disease);
            CameraController.Instance.ToggleColouredOverlayView(true);
            Camera.main.cullingMask |= cameraLayerMask;
            RegisterSaveLoadListeners();
            foreach (var diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
                if (!(diseaseSourceVisualizer == null))
                    diseaseSourceVisualizer.Show(ViewMode());
        }

        public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters() {
            return new Dictionary<string, ToolParameterMenu.ToggleState> {
                { ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off }
            };
        }

        public override void OnFiltersChanged() {
            Game.Instance.showGasConduitDisease = InFilter(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, legendFilters);
            Game.Instance.showLiquidConduitDisease
                = InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, legendFilters);
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            if (item == null) return;

            var component = item.GetComponent<KBatchedAnimController>();
            if (component == null) return;

            InfraredVisualizerComponents.ClearOverlayColour(component);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) { }

        public override void Disable() {
            foreach (var diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
                if (!(diseaseSourceVisualizer == null))
                    diseaseSourceVisualizer.Show(None.ID);

            UnregisterSaveLoadListeners();
            Camera.main.cullingMask &= ~cameraLayerMask;
            foreach (var kmonoBehaviour in layerTargets)
                if (!(kmonoBehaviour == null)) {
                    var defaultDepth = GetDefaultDepth(kmonoBehaviour);
                    var position     = kmonoBehaviour.transform.GetPosition();
                    position.z = defaultDepth;
                    kmonoBehaviour.transform.SetPosition(position);
                    var component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
                    component.enabled = false;
                    component.enabled = true;
                }

            CameraController.Instance.ToggleColouredOverlayView(false);
            Infrared.Instance.SetMode(Infrared.Mode.Disabled);
            Game.Instance.showGasConduitDisease    = false;
            Game.Instance.showLiquidConduitDisease = false;
            freeDiseaseUI                          = 0;
            foreach (var updateDiseaseInfo in this.updateDiseaseInfo) updateDiseaseInfo.ui.gameObject.SetActive(false);
            this.updateDiseaseInfo.Clear();
            privateTargets.Clear();
            layerTargets.Clear();
        }

        public override List<LegendEntry> GetCustomLegendData() {
            var list  = new List<LegendEntry>();
            var list2 = new List<DiseaseSortInfo>();
            foreach (var d in Db.Get().Diseases.resources) list2.Add(new DiseaseSortInfo(d));
            list2.Sort((a, b) => a.sortkey.CompareTo(b.sortkey));
            foreach (var diseaseSortInfo in list2)
                list.Add(new LegendEntry(diseaseSortInfo.disease.Name,
                                         diseaseSortInfo.disease.overlayLegendHovertext,
                                         GlobalAssets.Instance.colorSet.GetColorByName(diseaseSortInfo.disease
                                             .overlayColourName)));

            return list;
        }

        public GameObject GetFreeDiseaseUI() {
            GameObject gameObject;
            if (freeDiseaseUI < diseaseUIList.Count) {
                gameObject = diseaseUIList[freeDiseaseUI];
                gameObject.gameObject.SetActive(true);
                freeDiseaseUI++;
            } else {
                gameObject = Util.KInstantiateUI(diseaseOverlayPrefab, diseaseUIParent.transform.gameObject);
                diseaseUIList.Add(gameObject);
                freeDiseaseUI++;
            }

            return gameObject;
        }

        private void AddDiseaseUI(MinionIdentity target) {
            var gameObject  = GetFreeDiseaseUI();
            var component   = gameObject.GetComponent<DiseaseOverlayWidget>();
            var amount_inst = target.GetComponent<Modifiers>().amounts.Get(Db.Get().Amounts.ImmuneLevel);
            var item        = new UpdateDiseaseInfo(amount_inst, component);
            var component2  = target.GetComponent<KAnimControllerBase>();
            var position = component2 != null
                               ? component2.GetWorldPivot()
                               : target.transform.GetPosition() + Vector3.down;

            gameObject.GetComponent<RectTransform>().SetPosition(position);
            updateDiseaseInfo.Add(item);
        }

        public override void Update() {
            Vector2I u;
            Vector2I v;
            Grid.GetVisibleExtents(out u, out v);
            using (new KProfiler.Region("UpdateDiseaseCarriers")) {
                queuedAdds.Clear();
                foreach (var minionIdentity in Components.LiveMinionIdentities.Items)
                    if (!(minionIdentity == null)) {
                        var vector2I = Grid.PosToXY(minionIdentity.transform.GetPosition());
                        if (u <= vector2I && vector2I <= v && !privateTargets.Contains(minionIdentity)) {
                            AddDiseaseUI(minionIdentity);
                            queuedAdds.Add(minionIdentity);
                        }
                    }

                foreach (var item in queuedAdds) privateTargets.Add(item);
                queuedAdds.Clear();
            }

            foreach (var updateDiseaseInfo in this.updateDiseaseInfo)
                updateDiseaseInfo.ui.Refresh(updateDiseaseInfo.valueSrc);

            var flag = false;
            if (Game.Instance.showLiquidConduitDisease)
                using (var enumerator4 = OverlayScreen.LiquidVentIDs.GetEnumerator()) {
                    while (enumerator4.MoveNext()) {
                        var item2 = enumerator4.Current;
                        if (!OverlayScreen.DiseaseIDs.Contains(item2)) {
                            OverlayScreen.DiseaseIDs.Add(item2);
                            flag = true;
                        }
                    }

                    goto IL_1F1;
                }

            foreach (var item3 in OverlayScreen.LiquidVentIDs)
                if (OverlayScreen.DiseaseIDs.Contains(item3)) {
                    OverlayScreen.DiseaseIDs.Remove(item3);
                    flag = true;
                }

            IL_1F1:
            if (Game.Instance.showGasConduitDisease)
                using (var enumerator4 = OverlayScreen.GasVentIDs.GetEnumerator()) {
                    while (enumerator4.MoveNext()) {
                        var item4 = enumerator4.Current;
                        if (!OverlayScreen.DiseaseIDs.Contains(item4)) {
                            OverlayScreen.DiseaseIDs.Add(item4);
                            flag = true;
                        }
                    }

                    goto IL_297;
                }

            foreach (var item5 in OverlayScreen.GasVentIDs)
                if (OverlayScreen.DiseaseIDs.Contains(item5)) {
                    OverlayScreen.DiseaseIDs.Remove(item5);
                    flag = true;
                }

            IL_297:
            if (flag) SetLayerZ(-50f);
        }

        private void SetLayerZ(float offset_z) {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            ClearOutsideViewObjects(layerTargets,
                                    vector2I,
                                    vector2I2,
                                    OverlayScreen.DiseaseIDs,
                                    delegate(KMonoBehaviour go) {
                                        if (go != null) {
                                            var defaultDepth2 = GetDefaultDepth(go);
                                            var position2     = go.transform.GetPosition();
                                            position2.z = defaultDepth2;
                                            go.transform.SetPosition(position2);
                                            var component2 = go.GetComponent<KBatchedAnimController>();
                                            component2.enabled = false;
                                            component2.enabled = true;
                                        }
                                    });

            var lists = SaveLoader.Instance.saveManager.GetLists();
            foreach (var key in OverlayScreen.DiseaseIDs) {
                List<SaveLoadRoot> list;
                if (lists.TryGetValue(key, out list))
                    foreach (KMonoBehaviour kmonoBehaviour in list)
                        if (!(kmonoBehaviour == null) && !layerTargets.Contains(kmonoBehaviour)) {
                            var position = kmonoBehaviour.transform.GetPosition();
                            if (Grid.IsVisible(Grid.PosToCell(position)) &&
                                vector2I <= position                     &&
                                position <= vector2I2) {
                                var defaultDepth = GetDefaultDepth(kmonoBehaviour);
                                position.z = defaultDepth + offset_z;
                                kmonoBehaviour.transform.SetPosition(position);
                                var component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
                                component.enabled = false;
                                component.enabled = true;
                                layerTargets.Add(kmonoBehaviour);
                            }
                        }
            }
        }

        private struct DiseaseSortInfo {
            public DiseaseSortInfo(Klei.AI.Disease d) {
                disease = d;
                sortkey = CalculateHUE(GlobalAssets.Instance.colorSet.GetColorByName(d.overlayColourName));
            }

            public readonly float           sortkey;
            public readonly Klei.AI.Disease disease;
        }

        private struct UpdateDiseaseInfo {
            public UpdateDiseaseInfo(AmountInstance amount_inst, DiseaseOverlayWidget ui) {
                this.ui  = ui;
                valueSrc = amount_inst;
            }

            public readonly DiseaseOverlayWidget ui;
            public readonly AmountInstance       valueSrc;
        }
    }

    public class Logic : Mode {
        public static readonly HashedString ID = "Logic";
        public static HashSet<Tag> HighlightItemIDs = new HashSet<Tag>();
        public static KAnimHashedString RIBBON_WIRE_1_SYMBOL_NAME = "wire1";
        public static KAnimHashedString RIBBON_WIRE_2_SYMBOL_NAME = "wire2";
        public static KAnimHashedString RIBBON_WIRE_3_SYMBOL_NAME = "wire3";
        public static KAnimHashedString RIBBON_WIRE_4_SYMBOL_NAME = "wire4";
        private readonly HashSet<BridgeInfo> bridgeControllers = new HashSet<BridgeInfo>();
        private readonly int cameraLayerMask;
        private readonly int conduitTargetLayer;
        private readonly HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();
        private UniformGrid<SaveLoadRoot> gameObjPartition;
        private readonly HashSet<SaveLoadRoot> gameObjTargets = new HashSet<SaveLoadRoot>();
        private UniformGrid<ILogicUIElement> ioPartition;
        private readonly HashSet<ILogicUIElement> ioTargets = new HashSet<ILogicUIElement>();
        private readonly int objectTargetLayer;
        private readonly HashSet<BridgeInfo> ribbonBridgeControllers = new HashSet<BridgeInfo>();
        private readonly HashSet<KBatchedAnimController> ribbonControllers = new HashSet<KBatchedAnimController>();
        private readonly int selectionMask;
        private readonly LogicModeUI uiAsset;
        private readonly KCompactedVector<UIInfo> uiInfo = new KCompactedVector<UIInfo>();
        private readonly Dictionary<ILogicUIElement, EventInfo> uiNodes = new Dictionary<ILogicUIElement, EventInfo>();
        private readonly List<int> visited = new List<int>();
        private readonly HashSet<KBatchedAnimController> wireControllers = new HashSet<KBatchedAnimController>();
        private readonly HashSet<ILogicUIElement> workingIOTargets = new HashSet<ILogicUIElement>();

        public Logic(LogicModeUI ui_asset) {
            conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
            objectTargetLayer  = LayerMask.NameToLayer("MaskedOverlayBG");
            cameraLayerMask    = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask      = cameraLayerMask;
            uiAsset            = ui_asset;
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Logic"; }

        public override List<LegendEntry> GetCustomLegendData() {
            return new List<LegendEntry> {
                new LegendEntry(UI.OVERLAYS.LOGIC.INPUT,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.INPUT,
                                Color.white,
                                null,
                                Assets.GetSprite("logicInput")),
                new LegendEntry(UI.OVERLAYS.LOGIC.OUTPUT,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.OUTPUT,
                                Color.white,
                                null,
                                Assets.GetSprite("logicOutput")),
                new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_INPUT,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_INPUT,
                                Color.white,
                                null,
                                Assets.GetSprite("logic_ribbon_all_in")),
                new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_OUTPUT,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_OUTPUT,
                                Color.white,
                                null,
                                Assets.GetSprite("logic_ribbon_all_out")),
                new LegendEntry(UI.OVERLAYS.LOGIC.RESET_UPDATE,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.RESET_UPDATE,
                                Color.white,
                                null,
                                Assets.GetSprite("logicResetUpdate")),
                new LegendEntry(UI.OVERLAYS.LOGIC.CONTROL_INPUT,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.CONTROL_INPUT,
                                Color.white,
                                null,
                                Assets.GetSprite("control_input_frame_legend")),
                new LegendEntry(UI.OVERLAYS.LOGIC.CIRCUIT_STATUS_HEADER, null, Color.white, null, null, false),
                new LegendEntry(UI.OVERLAYS.LOGIC.ONE, null, GlobalAssets.Instance.colorSet.logicOnText),
                new LegendEntry(UI.OVERLAYS.LOGIC.ZERO, null, GlobalAssets.Instance.colorSet.logicOffText),
                new LegendEntry(UI.OVERLAYS.LOGIC.DISCONNECTED,
                                UI.OVERLAYS.LOGIC.TOOLTIPS.DISCONNECTED,
                                GlobalAssets.Instance.colorSet.logicDisconnected)
            };
        }

        public override void Enable() {
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
            RegisterSaveLoadListeners();
            gameObjPartition = PopulatePartition<SaveLoadRoot>(HighlightItemIDs);
            ioPartition      = CreateLogicUIPartition();
            GridCompositor.Instance.ToggleMinor(true);
            var logicCircuitManager = Game.Instance.logicCircuitManager;
            logicCircuitManager.onElemAdded
                = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager.onElemAdded,
                                                            new Action<ILogicUIElement>(OnUIElemAdded));

            var logicCircuitManager2 = Game.Instance.logicCircuitManager;
            logicCircuitManager2.onElemRemoved
                = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager2.onElemRemoved,
                                                            new Action<ILogicUIElement>(OnUIElemRemoved));

            AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterLogicOn);
        }

        public override void Disable() {
            var logicCircuitManager = Game.Instance.logicCircuitManager;
            logicCircuitManager.onElemAdded
                = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager.onElemAdded,
                                                           new Action<ILogicUIElement>(OnUIElemAdded));

            var logicCircuitManager2 = Game.Instance.logicCircuitManager;
            logicCircuitManager2.onElemRemoved
                = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager2.onElemRemoved,
                                                           new Action<ILogicUIElement>(OnUIElemRemoved));

            AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterLogicOn);
            foreach (var saveLoadRoot in gameObjTargets) {
                var defaultDepth = GetDefaultDepth(saveLoadRoot);
                var position     = saveLoadRoot.transform.GetPosition();
                position.z = defaultDepth;
                saveLoadRoot.transform.SetPosition(position);
                saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = false;
                saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = true;
            }

            ResetDisplayValues(gameObjTargets);
            ResetDisplayValues(wireControllers);
            ResetDisplayValues(ribbonControllers);
            ResetRibbonSymbolTints(ribbonControllers);
            foreach (var bridgeInfo in bridgeControllers)
                if (bridgeInfo.controller != null)
                    ResetDisplayValues(bridgeInfo.controller);

            foreach (var bridgeInfo2 in ribbonBridgeControllers)
                if (bridgeInfo2.controller != null)
                    ResetRibbonTint(bridgeInfo2.controller);

            Camera.main.cullingMask &= ~cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            UnregisterSaveLoadListeners();
            foreach (var uiinfo in uiInfo.GetDataList()) uiinfo.Release();
            uiInfo.Clear();
            uiNodes.Clear();
            ioPartition.Clear();
            ioTargets.Clear();
            gameObjPartition.Clear();
            gameObjTargets.Clear();
            wireControllers.Clear();
            ribbonControllers.Clear();
            bridgeControllers.Clear();
            ribbonBridgeControllers.Clear();
            GridCompositor.Instance.ToggleMinor(false);
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (HighlightItemIDs.Contains(saveLoadTag)) gameObjPartition.Add(item);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            if (gameObjTargets.Contains(item)) gameObjTargets.Remove(item);
            gameObjPartition.Remove(item);
        }

        private void OnUIElemAdded(ILogicUIElement elem) { ioPartition.Add(elem); }

        private void OnUIElemRemoved(ILogicUIElement elem) {
            ioPartition.Remove(elem);
            if (ioTargets.Contains(elem)) {
                ioTargets.Remove(elem);
                FreeUI(elem);
            }
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            var wire_id          = TagManager.Create("LogicWire");
            var ribbon_id        = TagManager.Create("LogicRibbon");
            var bridge_id        = TagManager.Create("LogicWireBridge");
            var ribbon_bridge_id = TagManager.Create("LogicRibbonBridge");
            RemoveOffscreenTargets(gameObjTargets,
                                   vector2I,
                                   vector2I2,
                                   delegate(SaveLoadRoot root) {
                                       if (root == null) return;

                                       var component7 = root.GetComponent<KPrefabID>();
                                       if (component7 != null) {
                                           var prefabTag = component7.PrefabTag;
                                           if (prefabTag == wire_id) {
                                               wireControllers.Remove(root.GetComponent<KBatchedAnimController>());
                                               return;
                                           }

                                           if (prefabTag == ribbon_id) {
                                               ResetRibbonTint(root.GetComponent<KBatchedAnimController>());
                                               ribbonControllers.Remove(root.GetComponent<KBatchedAnimController>());
                                               return;
                                           }

                                           if (prefabTag == bridge_id) {
                                               var controller = root.GetComponent<KBatchedAnimController>();
                                               bridgeControllers.RemoveWhere(x => x.controller == controller);
                                               return;
                                           }

                                           if (prefabTag == ribbon_bridge_id) {
                                               var controller = root.GetComponent<KBatchedAnimController>();
                                               ResetRibbonTint(controller);
                                               ribbonBridgeControllers.RemoveWhere(x => x.controller == controller);
                                               return;
                                           }

                                           var defaultDepth = GetDefaultDepth(root);
                                           var position     = root.transform.GetPosition();
                                           position.z = defaultDepth;
                                           root.transform.SetPosition(position);
                                           root.GetComponent<KBatchedAnimController>().enabled = false;
                                           root.GetComponent<KBatchedAnimController>().enabled = true;
                                       }
                                   });

            RemoveOffscreenTargets(ioTargets, workingIOTargets, vector2I, vector2I2, FreeUI);
            using (new KProfiler.Region("UpdateLogicOverlay")) {
                Action<SaveLoadRoot> <>9__3;
                foreach (var obj in gameObjPartition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                                        new Vector2(vector2I2.x, vector2I2.y))) {
                    var saveLoadRoot = (SaveLoadRoot)obj;
                    if (saveLoadRoot != null) {
                        var component = saveLoadRoot.GetComponent<KPrefabID>();
                        if (component.PrefabTag == wire_id   ||
                            component.PrefabTag == bridge_id ||
                            component.PrefabTag == ribbon_id ||
                            component.PrefabTag == ribbon_bridge_id) {
                            var                       instance = saveLoadRoot;
                            var                       vis_min  = vector2I;
                            var                       vis_max  = vector2I2;
                            ICollection<SaveLoadRoot> targets  = gameObjTargets;
                            var                       layer    = conduitTargetLayer;
                            Action<SaveLoadRoot>      on_added;
                            if ((on_added =  <>9__3) == null)
                            {
                                on_added = (<>9__3 = delegate(SaveLoadRoot root) {
                                                         if (root == null) return;

                                                         var component7 = root.GetComponent<KPrefabID>();
                                                         if (HighlightItemIDs.Contains(component7.PrefabTag)) {
                                                             if (component7.PrefabTag == wire_id) {
                                                                 wireControllers.Add(root.GetComponent<
                                                                     KBatchedAnimController>());

                                                                 return;
                                                             }

                                                             if (component7.PrefabTag == ribbon_id) {
                                                                 ribbonControllers.Add(root.GetComponent<
                                                                     KBatchedAnimController>());

                                                                 return;
                                                             }

                                                             if (component7.PrefabTag == bridge_id) {
                                                                 var component8
                                                                     = root.GetComponent<KBatchedAnimController>();

                                                                 var networkCell2 = root
                                                                     .GetComponent<LogicUtilityNetworkLink>()
                                                                     .GetNetworkCell();

                                                                 bridgeControllers.Add(new BridgeInfo {
                                                                     cell = networkCell2, controller = component8
                                                                 });

                                                                 return;
                                                             }

                                                             if (component7.PrefabTag == ribbon_bridge_id) {
                                                                 var component9
                                                                     = root.GetComponent<KBatchedAnimController>();

                                                                 var networkCell3 = root
                                                                     .GetComponent<LogicUtilityNetworkLink>()
                                                                     .GetNetworkCell();

                                                                 ribbonBridgeControllers.Add(new BridgeInfo {
                                                                     cell = networkCell3, controller = component9
                                                                 });
                                                             }
                                                         }
                                                     });
                            }

                            AddTargetIfVisible(instance, vis_min, vis_max, targets, layer, on_added);
                        } else
                            AddTargetIfVisible(saveLoadRoot,
                                               vector2I,
                                               vector2I2,
                                               gameObjTargets,
                                               objectTargetLayer,
                                               delegate(SaveLoadRoot root) {
                                                   var position   = root.transform.GetPosition();
                                                   var z          = position.z;
                                                   var component7 = root.GetComponent<KPrefabID>();
                                                   if (component7 != null) {
                                                       if (component7.HasTag(GameTags.OverlayInFrontOfConduits))
                                                           z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) - 0.2f;
                                                       else if (component7.HasTag(GameTags.OverlayBehindConduits))
                                                           z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) + 0.2f;
                                                   }

                                                   position.z = z;
                                                   root.transform.SetPosition(position);
                                                   var component8 = root.GetComponent<KBatchedAnimController>();
                                                   component8.enabled = false;
                                                   component8.enabled = true;
                                               });
                    }
                }

                foreach (var obj2 in ioPartition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                                    new Vector2(vector2I2.x, vector2I2.y))) {
                    var logicUIElement = (ILogicUIElement)obj2;
                    if (logicUIElement != null)
                        AddTargetIfVisible(logicUIElement,
                                           vector2I,
                                           vector2I2,
                                           ioTargets,
                                           objectTargetLayer,
                                           AddUI,
                                           kcmp => kcmp != null &&
                                                   HighlightItemIDs.Contains(kcmp.GetComponent<KPrefabID>().PrefabTag));
                }

                connectedNetworks.Clear();
                var        num        = 1f;
                GameObject gameObject = null;
                if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
                    gameObject = SelectTool.Instance.hover.gameObject;

                if (gameObject != null) {
                    var component2 = gameObject.GetComponent<IBridgedNetworkItem>();
                    if (component2 != null) {
                        var networkCell = component2.GetNetworkCell();
                        visited.Clear();
                        FindConnectedNetworks(networkCell,
                                              Game.Instance.logicCircuitSystem,
                                              connectedNetworks,
                                              visited);

                        visited.Clear();
                        num = ModeUtil.GetHighlightScale();
                    }
                }

                var logicCircuitManager = Game.Instance.logicCircuitManager;
                var logicOn             = GlobalAssets.Instance.colorSet.logicOn;
                var logicOff            = GlobalAssets.Instance.colorSet.logicOff;
                logicOff.a = logicOn.a  = 0;
                foreach (var kbatchedAnimController in wireControllers)
                    if (!(kbatchedAnimController == null)) {
                        var color = logicOff;
                        var networkForCell
                            = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController.transform
                                                                        .GetPosition()));

                        if (networkForCell != null) color = networkForCell.IsBitActive(0) ? logicOn : logicOff;
                        if (connectedNetworks.Count > 0) {
                            var component3 = kbatchedAnimController.GetComponent<IBridgedNetworkItem>();
                            if (component3 != null && component3.IsConnectedToNetworks(connectedNetworks)) {
                                color.r = (byte)(color.r * num);
                                color.g = (byte)(color.g * num);
                                color.b = (byte)(color.b * num);
                            }
                        }

                        kbatchedAnimController.TintColour = color;
                    }

                foreach (var kbatchedAnimController2 in ribbonControllers)
                    if (!(kbatchedAnimController2 == null)) {
                        var color2 = logicOff;
                        var color3 = logicOff;
                        var color4 = logicOff;
                        var color5 = logicOff;
                        var networkForCell2
                            = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController2.transform
                                                                        .GetPosition()));

                        if (networkForCell2 != null) {
                            color2 = networkForCell2.IsBitActive(0) ? logicOn : logicOff;
                            color3 = networkForCell2.IsBitActive(1) ? logicOn : logicOff;
                            color4 = networkForCell2.IsBitActive(2) ? logicOn : logicOff;
                            color5 = networkForCell2.IsBitActive(3) ? logicOn : logicOff;
                        }

                        if (connectedNetworks.Count > 0) {
                            var component4 = kbatchedAnimController2.GetComponent<IBridgedNetworkItem>();
                            if (component4 != null && component4.IsConnectedToNetworks(connectedNetworks)) {
                                color2.r = (byte)(color2.r * num);
                                color2.g = (byte)(color2.g * num);
                                color2.b = (byte)(color2.b * num);
                                color3.r = (byte)(color3.r * num);
                                color3.g = (byte)(color3.g * num);
                                color3.b = (byte)(color3.b * num);
                                color4.r = (byte)(color4.r * num);
                                color4.g = (byte)(color4.g * num);
                                color4.b = (byte)(color4.b * num);
                                color5.r = (byte)(color5.r * num);
                                color5.g = (byte)(color5.g * num);
                                color5.b = (byte)(color5.b * num);
                            }
                        }

                        kbatchedAnimController2.SetSymbolTint(RIBBON_WIRE_1_SYMBOL_NAME, color2);
                        kbatchedAnimController2.SetSymbolTint(RIBBON_WIRE_2_SYMBOL_NAME, color3);
                        kbatchedAnimController2.SetSymbolTint(RIBBON_WIRE_3_SYMBOL_NAME, color4);
                        kbatchedAnimController2.SetSymbolTint(RIBBON_WIRE_4_SYMBOL_NAME, color5);
                    }

                foreach (var bridgeInfo in bridgeControllers)
                    if (!(bridgeInfo.controller == null)) {
                        var color6                          = logicOff;
                        var networkForCell3                 = logicCircuitManager.GetNetworkForCell(bridgeInfo.cell);
                        if (networkForCell3 != null) color6 = networkForCell3.IsBitActive(0) ? logicOn : logicOff;
                        if (connectedNetworks.Count > 0) {
                            var component5 = bridgeInfo.controller.GetComponent<IBridgedNetworkItem>();
                            if (component5 != null && component5.IsConnectedToNetworks(connectedNetworks)) {
                                color6.r = (byte)(color6.r * num);
                                color6.g = (byte)(color6.g * num);
                                color6.b = (byte)(color6.b * num);
                            }
                        }

                        bridgeInfo.controller.TintColour = color6;
                    }

                foreach (var bridgeInfo2 in ribbonBridgeControllers)
                    if (!(bridgeInfo2.controller == null)) {
                        var color7          = logicOff;
                        var color8          = logicOff;
                        var color9          = logicOff;
                        var color10         = logicOff;
                        var networkForCell4 = logicCircuitManager.GetNetworkForCell(bridgeInfo2.cell);
                        if (networkForCell4 != null) {
                            color7  = networkForCell4.IsBitActive(0) ? logicOn : logicOff;
                            color8  = networkForCell4.IsBitActive(1) ? logicOn : logicOff;
                            color9  = networkForCell4.IsBitActive(2) ? logicOn : logicOff;
                            color10 = networkForCell4.IsBitActive(3) ? logicOn : logicOff;
                        }

                        if (connectedNetworks.Count > 0) {
                            var component6 = bridgeInfo2.controller.GetComponent<IBridgedNetworkItem>();
                            if (component6 != null && component6.IsConnectedToNetworks(connectedNetworks)) {
                                color7.r  = (byte)(color7.r  * num);
                                color7.g  = (byte)(color7.g  * num);
                                color7.b  = (byte)(color7.b  * num);
                                color8.r  = (byte)(color8.r  * num);
                                color8.g  = (byte)(color8.g  * num);
                                color8.b  = (byte)(color8.b  * num);
                                color9.r  = (byte)(color9.r  * num);
                                color9.g  = (byte)(color9.g  * num);
                                color9.b  = (byte)(color9.b  * num);
                                color10.r = (byte)(color10.r * num);
                                color10.g = (byte)(color10.g * num);
                                color10.b = (byte)(color10.b * num);
                            }
                        }

                        bridgeInfo2.controller.SetSymbolTint(RIBBON_WIRE_1_SYMBOL_NAME, color7);
                        bridgeInfo2.controller.SetSymbolTint(RIBBON_WIRE_2_SYMBOL_NAME, color8);
                        bridgeInfo2.controller.SetSymbolTint(RIBBON_WIRE_3_SYMBOL_NAME, color9);
                        bridgeInfo2.controller.SetSymbolTint(RIBBON_WIRE_4_SYMBOL_NAME, color10);
                    }
            }

            UpdateUI();
        }

        private void UpdateUI() {
            var logicOn            = GlobalAssets.Instance.colorSet.logicOn;
            var logicOff           = GlobalAssets.Instance.colorSet.logicOff;
            var logicDisconnected  = GlobalAssets.Instance.colorSet.logicDisconnected;
            logicOff.a = logicOn.a = byte.MaxValue;
            foreach (var uiinfo in uiInfo.GetDataList()) {
                var networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(uiinfo.cell);
                var c              = logicDisconnected;
                var component      = uiinfo.instance.GetComponent<LogicControlInputUI>();
                if (component != null)
                    component.SetContent(networkForCell);
                else if (uiinfo.bitDepth == 4) {
                    var component2 = uiinfo.instance.GetComponent<LogicRibbonDisplayUI>();
                    if (component2 != null) component2.SetContent(networkForCell);
                } else if (uiinfo.bitDepth == 1) {
                    if (networkForCell     != null) c = networkForCell.IsBitActive(0) ? logicOn : logicOff;
                    if (uiinfo.image.color != c) uiinfo.image.color = c;
                }
            }
        }

        private void AddUI(ILogicUIElement ui_elem) {
            if (uiNodes.ContainsKey(ui_elem)) return;

            var uiHandle = uiInfo.Allocate(new UIInfo(ui_elem, uiAsset));
            uiNodes.Add(ui_elem, new EventInfo { uiHandle = uiHandle });
        }

        private void FreeUI(ILogicUIElement item) {
            if (item == null) return;

            EventInfo eventInfo;
            if (uiNodes.TryGetValue(item, out eventInfo)) {
                uiInfo.GetData(eventInfo.uiHandle).Release();
                uiInfo.Free(eventInfo.uiHandle);
                uiNodes.Remove(item);
            }
        }

        protected UniformGrid<ILogicUIElement> CreateLogicUIPartition() {
            var uniformGrid = new UniformGrid<ILogicUIElement>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
            foreach (var logicUIElement in Game.Instance.logicCircuitManager.GetVisElements())
                if (logicUIElement != null)
                    uniformGrid.Add(logicUIElement);

            return uniformGrid;
        }

        private bool IsBitActive(int value, int bit) { return (value & (1 << bit)) > 0; }

        private void FindConnectedNetworks(int                         cell,
                                           IUtilityNetworkMgr          mgr,
                                           ICollection<UtilityNetwork> networks,
                                           List<int>                   visited) {
            if (visited.Contains(cell)) return;

            visited.Add(cell);
            var networkForCell = mgr.GetNetworkForCell(cell);
            if (networkForCell != null) {
                networks.Add(networkForCell);
                var connections = mgr.GetConnections(cell, false);
                if ((connections & UtilityConnections.Right) != 0)
                    FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Left) != 0)
                    FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Up) != 0)
                    FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Down) != 0)
                    FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
            }
        }

        private void ResetRibbonSymbolTints<T>(ICollection<T> targets) where T : MonoBehaviour {
            foreach (var t in targets)
                if (!(t == null)) {
                    var component = t.GetComponent<KBatchedAnimController>();
                    ResetRibbonTint(component);
                }
        }

        private void ResetRibbonTint(KBatchedAnimController kbac) {
            if (kbac != null) {
                kbac.SetSymbolTint(RIBBON_WIRE_1_SYMBOL_NAME, Color.white);
                kbac.SetSymbolTint(RIBBON_WIRE_2_SYMBOL_NAME, Color.white);
                kbac.SetSymbolTint(RIBBON_WIRE_3_SYMBOL_NAME, Color.white);
                kbac.SetSymbolTint(RIBBON_WIRE_4_SYMBOL_NAME, Color.white);
            }
        }

        private struct BridgeInfo {
            public int                    cell;
            public KBatchedAnimController controller;
        }

        private struct EventInfo {
            public HandleVector<int>.Handle uiHandle;
        }

        private struct UIInfo {
            public UIInfo(ILogicUIElement ui_elem, LogicModeUI ui_data) {
                cell = ui_elem.GetLogicUICell();
                GameObject original = null;
                Sprite     sprite   = null;
                bitDepth = 1;
                switch (ui_elem.GetLogicPortSpriteType()) {
                    case LogicPortSpriteType.Input:
                        original = ui_data.prefab;
                        sprite   = ui_data.inputSprite;
                        break;
                    case LogicPortSpriteType.Output:
                        original = ui_data.prefab;
                        sprite   = ui_data.outputSprite;
                        break;
                    case LogicPortSpriteType.ResetUpdate:
                        original = ui_data.prefab;
                        sprite   = ui_data.resetSprite;
                        break;
                    case LogicPortSpriteType.ControlInput:
                        original = ui_data.controlInputPrefab;
                        break;
                    case LogicPortSpriteType.RibbonInput:
                        original = ui_data.ribbonInputPrefab;
                        bitDepth = 4;
                        break;
                    case LogicPortSpriteType.RibbonOutput:
                        original = ui_data.ribbonOutputPrefab;
                        bitDepth = 4;
                        break;
                }

                instance = Util.KInstantiate(original,
                                             Grid.CellToPosCCC(cell, Grid.SceneLayer.Front),
                                             Quaternion.identity,
                                             GameScreenManager.Instance.worldSpaceCanvas);

                instance.SetActive(true);
                image = instance.GetComponent<Image>();
                if (image != null) {
                    image.raycastTarget = false;
                    image.sprite        = sprite;
                }
            }

            public          void       Release() { Util.KDestroyGameObject(instance); }
            public readonly GameObject instance;
            public readonly Image      image;
            public readonly int        cell;
            public readonly int        bitDepth;
        }
    }

    public class ColorHighlightCondition {
        public Func<KMonoBehaviour, Color> highlight_color;
        public Func<KMonoBehaviour, bool>  highlight_condition;

        public ColorHighlightCondition(Func<KMonoBehaviour, Color> highlight_color,
                                       Func<KMonoBehaviour, bool>  highlight_condition) {
            this.highlight_color     = highlight_color;
            this.highlight_condition = highlight_condition;
        }
    }

    public class None : Mode {
        public static readonly HashedString ID = HashedString.Invalid;
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "Off"; }
    }

    public class PathProber : Mode {
        public static readonly HashedString ID = "PathProber";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "Off"; }
    }

    public class Oxygen : Mode {
        public static readonly HashedString ID = "Oxygen";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "Oxygen"; }

        public override void Enable() {
            base.Enable();
            var defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
            var mask             = LayerMask.GetMask("MaskedOverlay");
            SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
        }

        public override void Disable() {
            base.Disable();
            SelectTool.Instance.ClearLayerMask();
        }
    }

    public class Light : Mode {
        public static readonly HashedString ID = "Light";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "Lights"; }
    }

    public class Priorities : Mode {
        public static readonly HashedString ID = "Priorities";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "Priorities"; }
    }

    public class ThermalConductivity : Mode {
        public static readonly HashedString ID = "ThermalConductivity";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "HeatFlow"; }
    }

    public class HeatFlow : Mode {
        public static readonly HashedString ID = "HeatFlow";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "HeatFlow"; }
    }

    public class Rooms : Mode {
        public static readonly HashedString ID = "Rooms";
        public override        HashedString ViewMode()     { return ID; }
        public override        string       GetSoundName() { return "Rooms"; }

        public override List<LegendEntry> GetCustomLegendData() {
            var list  = new List<LegendEntry>();
            var list2 = new List<RoomType>(Db.Get().RoomTypes.resources);
            list2.Sort((a, b) => a.sortKey.CompareTo(b.sortKey));
            foreach (var roomType in list2) {
                var text = roomType.GetCriteriaString();
                if (roomType.effects != null && roomType.effects.Length != 0)
                    text = text + "\n\n" + roomType.GetRoomEffectsString();

                list.Add(new LegendEntry(roomType.Name + "\n" + roomType.effect,
                                         text,
                                         GlobalAssets.Instance.colorSet.GetColorByName(roomType.category.colorName)));
            }

            return list;
        }
    }

    public abstract class Mode {
        private static readonly List<KMonoBehaviour> workingTargets = new List<KMonoBehaviour>();
        public Dictionary<string, ToolParameterMenu.ToggleState> legendFilters;
        public static void Clear() { workingTargets.Clear(); }
        public abstract HashedString ViewMode();
        public virtual void Enable() { }
        public virtual void Update() { }
        public virtual void Disable() { }
        public virtual List<LegendEntry> GetCustomLegendData() { return null; }
        public virtual Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters() { return null; }
        public virtual void OnFiltersChanged() { }
        public virtual void DisableOverlay() { }
        public virtual void OnRenderImage(RenderTexture src, RenderTexture dest) { }
        public abstract string GetSoundName();

        protected bool InFilter(string layer, Dictionary<string, ToolParameterMenu.ToggleState> filter) {
            return (filter.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) &&
                    filter[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On) ||
                   (filter.ContainsKey(layer) && filter[layer] == ToolParameterMenu.ToggleState.On);
        }

        public void RegisterSaveLoadListeners() {
            var saveManager = SaveLoader.Instance.saveManager;
            saveManager.onRegister   += OnSaveLoadRootRegistered;
            saveManager.onUnregister += OnSaveLoadRootUnregistered;
        }

        public void UnregisterSaveLoadListeners() {
            var saveManager = SaveLoader.Instance.saveManager;
            saveManager.onRegister   -= OnSaveLoadRootRegistered;
            saveManager.onUnregister -= OnSaveLoadRootUnregistered;
        }

        protected virtual void OnSaveLoadRootRegistered(SaveLoadRoot   root) { }
        protected virtual void OnSaveLoadRootUnregistered(SaveLoadRoot root) { }

        protected void ProcessExistingSaveLoadRoots() {
            foreach (var keyValuePair in SaveLoader.Instance.saveManager.GetLists()) {
                foreach (var root in keyValuePair.Value) OnSaveLoadRootRegistered(root);
            }
        }

        protected static UniformGrid<T> PopulatePartition<T>(ICollection<Tag> tags) where T : IUniformGridObject {
            var lists       = SaveLoader.Instance.saveManager.GetLists();
            var uniformGrid = new UniformGrid<T>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
            foreach (var key in tags) {
                List<SaveLoadRoot> list = null;
                if (lists.TryGetValue(key, out list))
                    foreach (var saveLoadRoot in list) {
                        var component = saveLoadRoot.GetComponent<T>();
                        if (component != null) uniformGrid.Add(component);
                    }
            }

            return uniformGrid;
        }

        protected static void ResetDisplayValues<T>(ICollection<T> targets) where T : MonoBehaviour {
            foreach (var t in targets)
                if (!(t == null)) {
                    var component = t.GetComponent<KBatchedAnimController>();
                    if (component != null) ResetDisplayValues(component);
                }
        }

        protected static void ResetDisplayValues(KBatchedAnimController controller) {
            controller.SetLayer(0);
            controller.HighlightColour = Color.clear;
            controller.TintColour      = Color.white;
            controller.SetLayer(controller.GetComponent<KPrefabID>().defaultLayer);
        }

        protected static void RemoveOffscreenTargets<T>(ICollection<T> targets,
                                                        Vector2I       min,
                                                        Vector2I       max,
                                                        Action<T>      on_removed = null) where T : KMonoBehaviour {
            ClearOutsideViewObjects(targets,
                                    min,
                                    max,
                                    null,
                                    delegate(T cmp) {
                                        if (cmp != null) {
                                            var component = cmp.GetComponent<KBatchedAnimController>();
                                            if (component  != null) ResetDisplayValues(component);
                                            if (on_removed != null) on_removed(cmp);
                                        }
                                    });

            workingTargets.Clear();
        }

        protected static void ClearOutsideViewObjects<T>(ICollection<T>   targets,
                                                         Vector2I         vis_min,
                                                         Vector2I         vis_max,
                                                         ICollection<Tag> item_ids,
                                                         Action<T>        on_remove) where T : KMonoBehaviour {
            workingTargets.Clear();
            foreach (var t in targets)
                if (!(t == null)) {
                    var vector2I = Grid.PosToXY(t.transform.GetPosition());
                    if (!(vis_min  <= vector2I) ||
                        !(vector2I <= vis_max)  ||
                        t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
                        workingTargets.Add(t);
                    else {
                        var component = t.GetComponent<KPrefabID>();
                        if (item_ids != null                        &&
                            !item_ids.Contains(component.PrefabTag) &&
                            t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
                            workingTargets.Add(t);
                    }
                }

            foreach (var kmonoBehaviour in workingTargets) {
                var t2 = (T)kmonoBehaviour;
                if (!(t2 == null)) {
                    if (on_remove != null) on_remove(t2);
                    targets.Remove(t2);
                }
            }

            workingTargets.Clear();
        }

        protected static void RemoveOffscreenTargets<T>(ICollection<T> targets,
                                                        ICollection<T> working_targets,
                                                        Vector2I       vis_min,
                                                        Vector2I       vis_max,
                                                        Action<T>      on_removed              = null,
                                                        Func<T, bool>  special_clear_condition = null)
            where T : IUniformGridObject {
            ClearOutsideViewObjects(targets,
                                    working_targets,
                                    vis_min,
                                    vis_max,
                                    delegate(T cmp) {
                                        if (cmp != null && on_removed != null) on_removed(cmp);
                                    });

            if (special_clear_condition != null) {
                working_targets.Clear();
                foreach (var t in targets)
                    if (special_clear_condition(t))
                        working_targets.Add(t);

                foreach (var t2 in working_targets)
                    if (t2 != null) {
                        if (on_removed != null) on_removed(t2);
                        targets.Remove(t2);
                    }

                working_targets.Clear();
            }
        }

        protected static void ClearOutsideViewObjects<T>(ICollection<T> targets,
                                                         ICollection<T> working_targets,
                                                         Vector2I vis_min,
                                                         Vector2I vis_max,
                                                         Action<T> on_removed = null) where T : IUniformGridObject {
            working_targets.Clear();
            foreach (var t in targets)
                if (t != null) {
                    var vector  = t.PosMin();
                    var vector2 = t.PosMin();
                    if (vector2.x < vis_min.x || vector2.y < vis_min.y || vis_max.x < vector.x || vis_max.y < vector.y)
                        working_targets.Add(t);
                }

            foreach (var t2 in working_targets)
                if (t2 != null) {
                    if (on_removed != null) on_removed(t2);
                    targets.Remove(t2);
                }

            working_targets.Clear();
        }

        protected static float GetDefaultDepth(KMonoBehaviour cmp) {
            var   component = cmp.GetComponent<BuildingComplete>();
            float layerZ;
            if (component != null)
                layerZ = Grid.GetLayerZ(component.Def.SceneLayer);
            else
                layerZ = Grid.GetLayerZ(Grid.SceneLayer.Creatures);

            return layerZ;
        }

        protected void UpdateHighlightTypeOverlay<T>(Vector2I                  min,
                                                     Vector2I                  max,
                                                     ICollection<T>            targets,
                                                     ICollection<Tag>          item_ids,
                                                     ColorHighlightCondition[] highlights,
                                                     BringToFrontLayerSetting  bringToFrontSetting,
                                                     int                       layer) where T : KMonoBehaviour {
            foreach (var t in targets)
                if (!(t == null)) {
                    var position = t.transform.GetPosition();
                    var cell     = Grid.PosToCell(position);
                    if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && min <= position && position <= max) {
                        var component = t.GetComponent<KBatchedAnimController>();
                        if (!(component == null)) {
                            var     layer2          = 0;
                            Color32 highlightColour = Color.clear;
                            if (highlights != null)
                                foreach (var colorHighlightCondition in highlights)
                                    if (colorHighlightCondition.highlight_condition(t)) {
                                        highlightColour = colorHighlightCondition.highlight_color(t);
                                        layer2          = layer;
                                        break;
                                    }

                            if (bringToFrontSetting != BringToFrontLayerSetting.Constant) {
                                if (bringToFrontSetting == BringToFrontLayerSetting.Conditional)
                                    component.SetLayer(layer2);
                            } else
                                component.SetLayer(layer);

                            component.HighlightColour = highlightColour;
                        }
                    }
                }
        }

        protected void DisableHighlightTypeOverlay<T>(ICollection<T> targets) where T : KMonoBehaviour {
            Color32 highlightColour = Color.clear;
            foreach (var t in targets)
                if (!(t == null)) {
                    var component = t.GetComponent<KBatchedAnimController>();
                    if (component != null) {
                        component.HighlightColour = highlightColour;
                        component.SetLayer(0);
                    }
                }

            targets.Clear();
        }

        protected void AddTargetIfVisible<T>(T                          instance,
                                             Vector2I                   vis_min,
                                             Vector2I                   vis_max,
                                             ICollection<T>             targets,
                                             int                        layer,
                                             Action<T>                  on_added   = null,
                                             Func<KMonoBehaviour, bool> should_add = null)
            where T : IUniformGridObject {
            if (instance.Equals(null)) return;

            var vector  = instance.PosMin();
            var vector2 = instance.PosMax();
            if (vector2.x < vis_min.x || vector2.y < vis_min.y || vector.x > vis_max.x || vector.y > vis_max.y) return;

            if (targets.Contains(instance)) return;

            var flag = false;
            var num  = (int)vector.y;
            while (num <= vector2.y) {
                var num2 = (int)vector.x;
                while (num2 <= vector2.x) {
                    var num3 = Grid.XYToCell(num2, num);
                    if ((Grid.IsValidCell(num3)   &&
                         Grid.Visible[num3]  > 20 &&
                         Grid.WorldIdx[num3] == ClusterManager.Instance.activeWorldId) ||
                        !PropertyTextures.IsFogOfWarEnabled) {
                        flag = true;
                        break;
                    }

                    num2++;
                }

                num++;
            }

            if (flag) {
                var flag2                                               = true;
                var kmonoBehaviour                                      = instance as KMonoBehaviour;
                if (kmonoBehaviour != null && should_add != null) flag2 = should_add(kmonoBehaviour);
                if (flag2) {
                    if (kmonoBehaviour != null) {
                        var component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
                        if (component != null) component.SetLayer(layer);
                    }

                    targets.Add(instance);
                    if (on_added != null) on_added(instance);
                }
            }
        }
    }

    public class ModeUtil {
        public static float GetHighlightScale() {
            return Mathf.SmoothStep(0.5f, 1f, Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f)));
        }
    }

    public class Power : Mode {
        public static readonly HashedString              ID            = "Power";
        private readonly       List<BatteryUI>           batteryUIList = new List<BatteryUI>();
        private readonly       Vector3                   batteryUIOffset;
        private readonly       BatteryUI                 batteryUIPrefab;
        private readonly       Vector3                   batteryUISmallTransformerOffset;
        private readonly       Vector3                   batteryUITransformerOffset;
        private readonly       int                       cameraLayerMask;
        private readonly       HashSet<UtilityNetwork>   connectedNetworks = new HashSet<UtilityNetwork>();
        private                int                       freeBatteryUIIdx;
        private                int                       freePowerLabelIdx;
        private readonly       HashSet<SaveLoadRoot>     layerTargets = new HashSet<SaveLoadRoot>();
        private                UniformGrid<SaveLoadRoot> partition;
        private readonly       Vector3                   powerLabelOffset;
        private readonly       Canvas                    powerLabelParent;
        private readonly       LocText                   powerLabelPrefab;
        private readonly       List<LocText>             powerLabels    = new List<LocText>();
        private readonly       HashSet<SaveLoadRoot>     privateTargets = new HashSet<SaveLoadRoot>();
        private readonly       List<SaveLoadRoot>        queuedAdds     = new List<SaveLoadRoot>();
        private readonly       int                       selectionMask;
        private readonly       int                       targetLayer;
        private readonly       List<UpdateBatteryInfo>   updateBatteryInfo = new List<UpdateBatteryInfo>();
        private readonly       List<UpdatePowerInfo>     updatePowerInfo   = new List<UpdatePowerInfo>();
        private readonly       List<int>                 visited           = new List<int>();

        public Power(Canvas    powerLabelParent,
                     LocText   powerLabelPrefab,
                     BatteryUI batteryUIPrefab,
                     Vector3   powerLabelOffset,
                     Vector3   batteryUIOffset,
                     Vector3   batteryUITransformerOffset,
                     Vector3   batteryUISmallTransformerOffset) {
            this.powerLabelParent                = powerLabelParent;
            this.powerLabelPrefab                = powerLabelPrefab;
            this.batteryUIPrefab                 = batteryUIPrefab;
            this.powerLabelOffset                = powerLabelOffset;
            this.batteryUIOffset                 = batteryUIOffset;
            this.batteryUITransformerOffset      = batteryUITransformerOffset;
            this.batteryUISmallTransformerOffset = batteryUISmallTransformerOffset;
            targetLayer                          = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask                      = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask                        = cameraLayerMask;
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Power"; }

        public override void Enable() {
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
            RegisterSaveLoadListeners();
            partition = PopulatePartition<SaveLoadRoot>(OverlayScreen.WireIDs);
            GridCompositor.Instance.ToggleMinor(true);
        }

        public override void Disable() {
            ResetDisplayValues(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            UnregisterSaveLoadListeners();
            partition.Clear();
            layerTargets.Clear();
            privateTargets.Clear();
            queuedAdds.Clear();
            DisablePowerLabels();
            DisableBatteryUIs();
            GridCompositor.Instance.ToggleMinor(false);
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (OverlayScreen.WireIDs.Contains(saveLoadTag)) partition.Add(item);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            if (layerTargets.Contains(item)) layerTargets.Remove(item);
            partition.Remove(item);
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            using (new KProfiler.Region("UpdatePowerOverlay")) {
                foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                                 new Vector2(vector2I2.x, vector2I2.y))) {
                    var instance = (SaveLoadRoot)obj;
                    AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
                }

                connectedNetworks.Clear();
                var        num        = 1f;
                GameObject gameObject = null;
                if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
                    gameObject = SelectTool.Instance.hover.gameObject;

                if (gameObject != null) {
                    var component = gameObject.GetComponent<IBridgedNetworkItem>();
                    if (component != null) {
                        var networkCell = component.GetNetworkCell();
                        visited.Clear();
                        FindConnectedNetworks(networkCell,
                                              Game.Instance.electricalConduitSystem,
                                              connectedNetworks,
                                              visited);

                        visited.Clear();
                        num = ModeUtil.GetHighlightScale();
                    }
                }

                var circuitManager = Game.Instance.circuitManager;
                foreach (var saveLoadRoot in layerTargets)
                    if (!(saveLoadRoot == null)) {
                        var component2 = saveLoadRoot.GetComponent<IBridgedNetworkItem>();
                        if (component2 != null) {
                            KAnimControllerBase component3
                                = (component2 as KMonoBehaviour).GetComponent<KBatchedAnimController>();

                            var networkCell2 = component2.GetNetworkCell();
                            var networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(networkCell2);
                            var num2 = networkForCell != null ? (ushort)networkForCell.id : ushort.MaxValue;
                            var wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(num2);
                            var num3 = circuitManager.GetMaxSafeWattageForCircuit(num2);
                            num3 += POWER.FLOAT_FUDGE_FACTOR;
                            var     wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(num2);
                            Color32 color;
                            if (wattsUsedByCircuit <= 0f)
                                color = GlobalAssets.Instance.colorSet.powerCircuitUnpowered;
                            else if (wattsUsedByCircuit > num3)
                                color = GlobalAssets.Instance.colorSet.powerCircuitOverloading;
                            else if (wattsNeededWhenActive > num3 && num3 > 0f && wattsUsedByCircuit / num3 >= 0.75f)
                                color = GlobalAssets.Instance.colorSet.powerCircuitStraining;
                            else
                                color = GlobalAssets.Instance.colorSet.powerCircuitSafe;

                            if (connectedNetworks.Count > 0 && component2.IsConnectedToNetworks(connectedNetworks)) {
                                color.r = (byte)(color.r * num);
                                color.g = (byte)(color.g * num);
                                color.b = (byte)(color.b * num);
                            }

                            component3.TintColour = color;
                        }
                    }
            }

            queuedAdds.Clear();
            using (new KProfiler.Region("BatteryUI")) {
                foreach (var battery in Components.Batteries.Items) {
                    var vector2I3 = Grid.PosToXY(battery.transform.GetPosition());
                    if (vector2I               <= vector2I3 &&
                        vector2I3              <= vector2I2 &&
                        battery.GetMyWorldId() == ClusterManager.Instance.activeWorldId) {
                        var component4 = battery.GetComponent<SaveLoadRoot>();
                        if (!privateTargets.Contains(component4)) {
                            AddBatteryUI(battery);
                            queuedAdds.Add(component4);
                        }
                    }
                }

                foreach (var generator in Components.Generators.Items) {
                    var vector2I4 = Grid.PosToXY(generator.transform.GetPosition());
                    if (vector2I                 <= vector2I4 &&
                        vector2I4                <= vector2I2 &&
                        generator.GetMyWorldId() == ClusterManager.Instance.activeWorldId) {
                        var component5 = generator.GetComponent<SaveLoadRoot>();
                        if (!privateTargets.Contains(component5)) {
                            privateTargets.Add(component5);
                            if (generator.GetComponent<PowerTransformer>() == null) AddPowerLabels(generator);
                        }
                    }
                }

                foreach (var energyConsumer in Components.EnergyConsumers.Items) {
                    var vector2I5 = Grid.PosToXY(energyConsumer.transform.GetPosition());
                    if (vector2I                      <= vector2I5 &&
                        vector2I5                     <= vector2I2 &&
                        energyConsumer.GetMyWorldId() == ClusterManager.Instance.activeWorldId) {
                        var component6 = energyConsumer.GetComponent<SaveLoadRoot>();
                        if (!privateTargets.Contains(component6)) {
                            privateTargets.Add(component6);
                            AddPowerLabels(energyConsumer);
                        }
                    }
                }
            }

            foreach (var item in queuedAdds) privateTargets.Add(item);
            queuedAdds.Clear();
            UpdatePowerLabels();
        }

        private LocText GetFreePowerLabel() {
            LocText locText;
            if (freePowerLabelIdx < powerLabels.Count) {
                locText = powerLabels[freePowerLabelIdx];
                freePowerLabelIdx++;
            } else {
                locText = Util.KInstantiateUI<LocText>(powerLabelPrefab.gameObject,
                                                       powerLabelParent.transform.gameObject);

                powerLabels.Add(locText);
                freePowerLabelIdx++;
            }

            return locText;
        }

        private void UpdatePowerLabels() {
            foreach (var updatePowerInfo in this.updatePowerInfo) {
                var item       = updatePowerInfo.item;
                var powerLabel = updatePowerInfo.powerLabel;
                var unitLabel  = updatePowerInfo.unitLabel;
                var generator  = updatePowerInfo.generator;
                var consumer   = updatePowerInfo.consumer;
                if (updatePowerInfo.item                           == null ||
                    updatePowerInfo.item.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
                    powerLabel.gameObject.SetActive(false);
                else {
                    powerLabel.gameObject.SetActive(true);
                    if (generator != null && consumer == null) {
                        int num;
                        if (generator.GetComponent<ManualGenerator>() == null) {
                            generator.GetComponent<Operational>();
                            num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
                        } else
                            num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));

                        powerLabel.text = num != 0 ? "+" + num : num.ToString();
                        var component = item.GetComponent<BuildingEnabledButton>();
                        Color color = component != null && !component.IsEnabled
                                          ? GlobalAssets.Instance.colorSet.powerBuildingDisabled
                                          : GlobalAssets.Instance.colorSet.powerGenerator;

                        powerLabel.color = color;
                        unitLabel.color  = color;
                        var component2 = generator.GetComponent<BuildingCellVisualizer>();
                        if (component2 != null) {
                            var powerOutputIcon                                = component2.GetPowerOutputIcon();
                            if (powerOutputIcon != null) powerOutputIcon.color = color;
                        }
                    }

                    if (consumer != null) {
                        var component3 = item.GetComponent<BuildingEnabledButton>();
                        Color color2 = component3 != null && !component3.IsEnabled
                                           ? GlobalAssets.Instance.colorSet.powerBuildingDisabled
                                           : GlobalAssets.Instance.colorSet.powerConsumer;

                        var num2 = Mathf.Max(0, Mathf.RoundToInt(consumer.WattsNeededWhenActive));
                        var text = num2.ToString();
                        powerLabel.text = num2 != 0 ? "-" + text : text;
                        powerLabel.color = color2;
                        unitLabel.color = color2;
                        var powerInputIcon = item.GetComponentInChildren<BuildingCellVisualizer>().GetPowerInputIcon();
                        if (powerInputIcon != null) powerInputIcon.color = color2;
                    }
                }
            }

            foreach (var updateBatteryInfo in this.updateBatteryInfo)
                updateBatteryInfo.ui.SetContent(updateBatteryInfo.battery);
        }

        private void AddPowerLabels(KMonoBehaviour item) {
            if (item.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId) {
                var componentInChildren  = item.gameObject.GetComponentInChildren<IEnergyConsumer>();
                var componentInChildren2 = item.gameObject.GetComponentInChildren<Generator>();
                if (componentInChildren != null || componentInChildren2 != null) {
                    var num = -10f;
                    if (componentInChildren2 != null) {
                        var freePowerLabel = GetFreePowerLabel();
                        freePowerLabel.gameObject.SetActive(true);
                        freePowerLabel.gameObject.name = item.gameObject.name + "power label";
                        var component = freePowerLabel.transform.GetChild(0).GetComponent<LocText>();
                        component.gameObject.SetActive(true);
                        freePowerLabel.enabled = true;
                        component.enabled      = true;
                        var a = Grid.CellToPos(componentInChildren2.PowerCell, 0.5f, 0f, 0f);
                        freePowerLabel.rectTransform.SetPosition(a + powerLabelOffset + Vector3.up * (num * 0.02f));
                        if (componentInChildren           != null &&
                            componentInChildren.PowerCell == componentInChildren2.PowerCell)
                            num -= 15f;

                        SetToolTip(freePowerLabel, UI.OVERLAYS.POWER.WATTS_GENERATED);
                        updatePowerInfo.Add(new UpdatePowerInfo(item,
                                                                freePowerLabel,
                                                                component,
                                                                componentInChildren2,
                                                                null));
                    }

                    if (componentInChildren != null && componentInChildren.GetType() != typeof(Battery)) {
                        var freePowerLabel2 = GetFreePowerLabel();
                        var component2      = freePowerLabel2.transform.GetChild(0).GetComponent<LocText>();
                        freePowerLabel2.gameObject.SetActive(true);
                        component2.gameObject.SetActive(true);
                        freePowerLabel2.gameObject.name = item.gameObject.name + "power label";
                        freePowerLabel2.enabled         = true;
                        component2.enabled              = true;
                        var a2 = Grid.CellToPos(componentInChildren.PowerCell, 0.5f, 0f, 0f);
                        freePowerLabel2.rectTransform.SetPosition(a2 + powerLabelOffset + Vector3.up * (num * 0.02f));
                        SetToolTip(freePowerLabel2, UI.OVERLAYS.POWER.WATTS_CONSUMED);
                        updatePowerInfo.Add(new UpdatePowerInfo(item,
                                                                freePowerLabel2,
                                                                component2,
                                                                null,
                                                                componentInChildren));
                    }
                }
            }
        }

        private void DisablePowerLabels() {
            freePowerLabelIdx = 0;
            foreach (var locText in powerLabels) locText.gameObject.SetActive(false);
            updatePowerInfo.Clear();
        }

        private void AddBatteryUI(Battery bat) {
            var freeBatteryUI = GetFreeBatteryUI();
            freeBatteryUI.SetContent(bat);
            var b                                                        = Grid.CellToPos(bat.PowerCell, 0.5f, 0f, 0f);
            var flag                                                     = bat.powerTransformer != null;
            var num                                                      = 1f;
            var component                                                = bat.GetComponent<Rotatable>();
            if (component != null && component.GetVisualizerFlipX()) num = -1f;
            var b2                                                       = batteryUIOffset;
            if (flag)
                b2 = bat.GetComponent<Building>().Def.WidthInCells == 2
                         ? batteryUISmallTransformerOffset
                         : batteryUITransformerOffset;

            b2.x *= num;
            freeBatteryUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b + b2);
            updateBatteryInfo.Add(new UpdateBatteryInfo(bat, freeBatteryUI));
        }

        private void SetToolTip(LocText label, string text) {
            var component                            = label.GetComponent<ToolTip>();
            if (component != null) component.toolTip = text;
        }

        private void DisableBatteryUIs() {
            freeBatteryUIIdx = 0;
            foreach (var batteryUI in batteryUIList) batteryUI.gameObject.SetActive(false);
            updateBatteryInfo.Clear();
        }

        private BatteryUI GetFreeBatteryUI() {
            BatteryUI batteryUI;
            if (freeBatteryUIIdx < batteryUIList.Count) {
                batteryUI = batteryUIList[freeBatteryUIIdx];
                batteryUI.gameObject.SetActive(true);
                freeBatteryUIIdx++;
            } else {
                batteryUI = Util.KInstantiateUI<BatteryUI>(batteryUIPrefab.gameObject,
                                                           powerLabelParent.transform.gameObject);

                batteryUIList.Add(batteryUI);
                freeBatteryUIIdx++;
            }

            return batteryUI;
        }

        private void FindConnectedNetworks(int                         cell,
                                           IUtilityNetworkMgr          mgr,
                                           ICollection<UtilityNetwork> networks,
                                           List<int>                   visited) {
            if (visited.Contains(cell)) return;

            visited.Add(cell);
            var networkForCell = mgr.GetNetworkForCell(cell);
            if (networkForCell != null) {
                networks.Add(networkForCell);
                var connections = mgr.GetConnections(cell, false);
                if ((connections & UtilityConnections.Right) != 0)
                    FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Left) != 0)
                    FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Up) != 0)
                    FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Down) != 0)
                    FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
            }
        }

        private struct UpdatePowerInfo {
            public UpdatePowerInfo(KMonoBehaviour  item,
                                   LocText         power_label,
                                   LocText         unit_label,
                                   Generator       g,
                                   IEnergyConsumer c) {
                this.item  = item;
                powerLabel = power_label;
                unitLabel  = unit_label;
                generator  = g;
                consumer   = c;
            }

            public readonly KMonoBehaviour  item;
            public readonly LocText         powerLabel;
            public readonly LocText         unitLabel;
            public readonly Generator       generator;
            public readonly IEnergyConsumer consumer;
        }

        private struct UpdateBatteryInfo {
            public UpdateBatteryInfo(Battery battery, BatteryUI ui) {
                this.battery = battery;
                this.ui      = ui;
            }

            public readonly Battery   battery;
            public readonly BatteryUI ui;
        }
    }

    public class Radiation : Mode {
        public static readonly HashedString ID = "Radiation";
        public override HashedString ViewMode() { return ID; }
        public override string GetSoundName() { return "Radiation"; }
        public override void Enable() { AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterRadiationOn); }
        public override void Disable() { AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterRadiationOn); }
    }

    public class SolidConveyor : Mode {
        public static readonly HashedString              ID = "SolidConveyor";
        private readonly       int                       cameraLayerMask;
        private readonly       HashSet<UtilityNetwork>   connectedNetworks = new HashSet<UtilityNetwork>();
        private readonly       HashSet<SaveLoadRoot>     layerTargets      = new HashSet<SaveLoadRoot>();
        private                UniformGrid<SaveLoadRoot> partition;
        private readonly       int                       selectionMask;
        private readonly       ICollection<Tag>          targetIDs = OverlayScreen.SolidConveyorIDs;
        private readonly       int                       targetLayer;
        private readonly       Color32                   tint_color = new Color32(201, 201, 201, 0);
        private readonly       List<int>                 visited    = new List<int>();

        public SolidConveyor() {
            targetLayer     = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask   = cameraLayerMask;
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "LiquidVent"; }

        public override void Enable() {
            RegisterSaveLoadListeners();
            partition               =  PopulatePartition<SaveLoadRoot>(targetIDs);
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
            GridCompositor.Instance.ToggleMinor(false);
            base.Enable();
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (targetIDs.Contains(saveLoadTag)) partition.Add(item);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            if (layerTargets.Contains(item)) layerTargets.Remove(item);
            partition.Remove(item);
        }

        public override void Disable() {
            ResetDisplayValues(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            UnregisterSaveLoadListeners();
            partition.Clear();
            layerTargets.Clear();
            GridCompositor.Instance.ToggleMinor(false);
            base.Disable();
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                             new Vector2(vector2I2.x, vector2I2.y))) {
                var instance = (SaveLoadRoot)obj;
                AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
            }

            GameObject gameObject = null;
            if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
                gameObject = SelectTool.Instance.hover.gameObject;

            connectedNetworks.Clear();
            var num = 1f;
            if (gameObject != null) {
                var component = gameObject.GetComponent<SolidConduit>();
                if (component != null) {
                    var cell               = Grid.PosToCell(component);
                    var solidConduitSystem = Game.Instance.solidConduitSystem;
                    visited.Clear();
                    FindConnectedNetworks(cell, solidConduitSystem, connectedNetworks, visited);
                    visited.Clear();
                    num = ModeUtil.GetHighlightScale();
                }
            }

            foreach (var saveLoadRoot in layerTargets)
                if (!(saveLoadRoot == null)) {
                    var color      = tint_color;
                    var component2 = saveLoadRoot.GetComponent<SolidConduit>();
                    if (component2 != null) {
                        if (connectedNetworks.Count > 0 && IsConnectedToNetworks(component2, connectedNetworks)) {
                            color.r = (byte)(color.r * num);
                            color.g = (byte)(color.g * num);
                            color.b = (byte)(color.b * num);
                        }

                        saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = color;
                    }
                }
        }

        public bool IsConnectedToNetworks(SolidConduit conduit, ICollection<UtilityNetwork> networks) {
            var network = conduit.GetNetwork();
            return networks.Contains(network);
        }

        private void FindConnectedNetworks(int                         cell,
                                           IUtilityNetworkMgr          mgr,
                                           ICollection<UtilityNetwork> networks,
                                           List<int>                   visited) {
            if (visited.Contains(cell)) return;

            visited.Add(cell);
            var networkForCell = mgr.GetNetworkForCell(cell);
            if (networkForCell != null) {
                networks.Add(networkForCell);
                var connections = mgr.GetConnections(cell, false);
                if ((connections & UtilityConnections.Right) != 0)
                    FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Left) != 0)
                    FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Up) != 0)
                    FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);

                if ((connections & UtilityConnections.Down) != 0)
                    FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);

                var endpoint = mgr.GetEndpoint(cell);
                if (endpoint != null) {
                    var networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
                    if (networkItem != null) {
                        var gameObject = networkItem.GameObject;
                        if (gameObject != null) {
                            var component = gameObject.GetComponent<IBridgedNetworkItem>();
                            if (component != null) component.AddNetworks(networks);
                        }
                    }
                }
            }
        }
    }

    public class Sound : Mode {
        public static readonly HashedString               ID = "Sound";
        private readonly       int                        cameraLayerMask;
        private readonly       ColorHighlightCondition[]  highlightConditions;
        private readonly       HashSet<NoisePolluter>     layerTargets = new HashSet<NoisePolluter>();
        private                UniformGrid<NoisePolluter> partition;
        private readonly       HashSet<Tag>               targetIDs = new HashSet<Tag>();
        private readonly       int                        targetLayer;

        public Sound() {
            var array = new ColorHighlightCondition[1];
            array[0] = new ColorHighlightCondition(delegate(KMonoBehaviour np) {
                                                       var black  = Color.black;
                                                       var black2 = Color.black;
                                                       var t      = 0.8f;
                                                       if (np != null) {
                                                           var cell = Grid.PosToCell(CameraController.Instance
                                                               .baseCamera.ScreenToWorldPoint(KInputManager
                                                                   .GetMousePos()));

                                                           if ((np as NoisePolluter).GetNoiseForCell(cell) < 36f) {
                                                               t      = 1f;
                                                               black2 = new Color(0.4f, 0.4f, 0.4f);
                                                           }
                                                       }

                                                       return Color.Lerp(black, black2, t);
                                                   },
                                                   delegate(KMonoBehaviour np) {
                                                       var highlightedObjects
                                                           = SelectToolHoverTextCard.highlightedObjects;

                                                       var result = false;
                                                       for (var i = 0; i < highlightedObjects.Count; i++)
                                                           if (highlightedObjects[i] != null &&
                                                               highlightedObjects[i] == np.gameObject) {
                                                               result = true;
                                                               break;
                                                           }

                                                       return result;
                                                   });

            highlightConditions = array;
            base..ctor();
            targetLayer     = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            var prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
            targetIDs.UnionWith(prefabTagsWithComponent);
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Sound"; }

        public override void Enable() {
            RegisterSaveLoadListeners();
            var prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
            targetIDs.UnionWith(prefabTagsWithComponent);
            partition               =  PopulatePartition<NoisePolluter>(targetIDs);
            Camera.main.cullingMask |= cameraLayerMask;
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                             new Vector2(vector2I2.x, vector2I2.y))) {
                var instance = (NoisePolluter)obj;
                AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
            }

            UpdateHighlightTypeOverlay(vector2I,
                                       vector2I2,
                                       layerTargets,
                                       targetIDs,
                                       highlightConditions,
                                       BringToFrontLayerSetting.Conditional,
                                       targetLayer);
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (targetIDs.Contains(saveLoadTag)) {
                var component = item.GetComponent<NoisePolluter>();
                partition.Add(component);
            }
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            var component = item.GetComponent<NoisePolluter>();
            if (layerTargets.Contains(component)) layerTargets.Remove(component);
            partition.Remove(component);
        }

        public override void Disable() {
            DisableHighlightTypeOverlay(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            UnregisterSaveLoadListeners();
            partition.Clear();
            layerTargets.Clear();
        }
    }

    public class Suit : Mode {
        public static readonly HashedString              ID = "Suit";
        private readonly       int                       cameraLayerMask;
        private                int                       freeUiIdx;
        private readonly       HashSet<SaveLoadRoot>     layerTargets = new HashSet<SaveLoadRoot>();
        private readonly       GameObject                overlayPrefab;
        private                UniformGrid<SaveLoadRoot> partition;
        private readonly       int                       selectionMask;
        private readonly       ICollection<Tag>          targetIDs;
        private readonly       int                       targetLayer;
        private readonly       List<GameObject>          uiList = new List<GameObject>();
        private readonly       Canvas                    uiParent;

        public Suit(Canvas ui_parent, GameObject overlay_prefab) {
            targetLayer     = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask   = cameraLayerMask;
            targetIDs       = OverlayScreen.SuitIDs;
            uiParent        = ui_parent;
            overlayPrefab   = overlay_prefab;
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "SuitRequired"; }

        public override void Enable() {
            partition = new UniformGrid<SaveLoadRoot>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
            ProcessExistingSaveLoadRoots();
            RegisterSaveLoadListeners();
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
            GridCompositor.Instance.ToggleMinor(false);
            base.Enable();
        }

        public override void Disable() {
            UnregisterSaveLoadListeners();
            ResetDisplayValues(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            partition.Clear();
            partition = null;
            layerTargets.Clear();
            for (var i = 0; i < uiList.Count; i++) uiList[i].SetActive(false);
            GridCompositor.Instance.ToggleMinor(false);
            base.Disable();
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot item) {
            var saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
            if (targetIDs.Contains(saveLoadTag)) partition.Add(item);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item) {
            if (item == null || item.gameObject == null) return;

            if (layerTargets.Contains(item)) layerTargets.Remove(item);
            partition.Remove(item);
        }

        private GameObject GetFreeUI() {
            GameObject gameObject;
            if (freeUiIdx >= uiList.Count) {
                gameObject = Util.KInstantiateUI(overlayPrefab, uiParent.transform.gameObject);
                uiList.Add(gameObject);
            } else {
                var list = uiList;
                var num  = freeUiIdx;
                freeUiIdx  = num + 1;
                gameObject = list[num];
            }

            if (!gameObject.activeSelf) gameObject.SetActive(true);
            return gameObject;
        }

        public override void Update() {
            freeUiIdx = 0;
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            foreach (var obj in partition.GetAllIntersecting(new Vector2(vector2I.x,  vector2I.y),
                                                             new Vector2(vector2I2.x, vector2I2.y))) {
                var instance = (SaveLoadRoot)obj;
                AddTargetIfVisible(instance, vector2I, vector2I2, layerTargets, targetLayer);
            }

            foreach (var saveLoadRoot in layerTargets)
                if (!(saveLoadRoot == null)) {
                    saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = Color.white;
                    var flag = false;
                    if (saveLoadRoot.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
                        flag = true;
                    else {
                        var component               = saveLoadRoot.GetComponent<SuitLocker>();
                        if (component != null) flag = component.GetStoredOutfit() != null;
                    }

                    if (flag)
                        GetFreeUI().GetComponent<RectTransform>().SetPosition(saveLoadRoot.transform.GetPosition());
                }

            for (var i = freeUiIdx; i < uiList.Count; i++)
                if (uiList[i].activeSelf)
                    uiList[i].SetActive(false);
        }
    }

    public class Temperature : Mode {
        public static readonly HashedString ID = "Temperature";

        public List<LegendEntry> expandedTemperatureLegend = new List<LegendEntry> {
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.8901961f, 0.13725491f, 0.12941177f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.9843137f, 0.3254902f, 0.3137255f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(1f, 0.6627451f, 0.14117648f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.9372549f, 1f, 0f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.23137255f, 0.99607843f, 0.2901961f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.12156863f, 0.6313726f, 1f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.16862746f, 0.79607844f, 1f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.5019608f, 0.99607843f, 0.9411765f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_source")),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_sink"))
        };

        public List<LegendEntry> heatFlowLegend = new List<LegendEntry> {
            new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING,
                            UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING,
                            new Color(0.9098039f, 0.25882354f, 0.14901961f)),
            new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL,
                            UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL,
                            new Color(0.30980393f, 0.30980393f, 0.30980393f)),
            new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING,
                            UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING,
                            new Color(0.2509804f, 0.6313726f, 0.90588236f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_source")),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_sink"))
        };

        private Vector2 previousUserSetting;

        public List<LegendEntry> stateChangeLegend = new List<LegendEntry> {
            new LegendEntry(UI.OVERLAYS.STATECHANGE.HIGHPOINT,
                            UI.OVERLAYS.STATECHANGE.TOOLTIPS.HIGHPOINT,
                            new Color(0.8901961f, 0.13725491f, 0.12941177f)),
            new LegendEntry(UI.OVERLAYS.STATECHANGE.STABLE,
                            UI.OVERLAYS.STATECHANGE.TOOLTIPS.STABLE,
                            new Color(0.23137255f, 0.99607843f, 0.2901961f)),
            new LegendEntry(UI.OVERLAYS.STATECHANGE.LOWPOINT,
                            UI.OVERLAYS.STATECHANGE.TOOLTIPS.LOWPOINT,
                            new Color(0.5019608f, 0.99607843f, 0.9411765f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_source")),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_sink"))
        };

        public List<LegendEntry> temperatureLegend = new List<LegendEntry> {
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.8901961f, 0.13725491f, 0.12941177f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.9843137f, 0.3254902f, 0.3137255f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(1f, 0.6627451f, 0.14117648f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.9372549f, 1f, 0f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.23137255f, 0.99607843f, 0.2901961f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.12156863f, 0.6313726f, 1f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.16862746f, 0.79607844f, 1f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE,
                            new Color(0.5019608f, 0.99607843f, 0.9411765f)),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_source")),
            new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK,
                            UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK,
                            Color.white,
                            null,
                            Assets.GetSprite("heat_sink"))
        };

        public Temperature() { legendFilters = CreateDefaultFilters(); }
        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Temperature"; }

        public override void Update() {
            base.Update();
            if (previousUserSetting != SimDebugView.Instance.user_temperatureThresholds) {
                RefreshLegendValues();
                previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
            }
        }

        public override void Enable() {
            base.Enable();
            previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
            RefreshLegendValues();
        }

        public void RefreshLegendValues() {
            var num = SimDebugView.Instance.temperatureThresholds.Length - 1;
            for (var i = 0; i < num; i++) {
                temperatureLegend[i].colour
                    = GlobalAssets.Instance.colorSet.GetColorByName(SimDebugView.Instance.temperatureThresholds[num - i]
                                                                        .colorName);

                temperatureLegend[i].desc_arg
                    = GameUtil.GetFormattedTemperature(SimDebugView.Instance.temperatureThresholds[num - i].value);
            }
        }

        public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters() {
            return new Dictionary<string, ToolParameterMenu.ToggleState> {
                { ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.HEATFLOW, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.STATECHANGE, ToolParameterMenu.ToggleState.Off }
            };
        }

        public override void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (Game.IsQuitting()) return;

            KAnimBatchManager.Instance().RenderKAnimTemperaturePostProcessingEffects();
        }

        public override List<LegendEntry> GetCustomLegendData() {
            switch (Game.Instance.temperatureOverlayMode) {
                case Game.TemperatureOverlayModes.AbsoluteTemperature:
                    return temperatureLegend;
                case Game.TemperatureOverlayModes.AdaptiveTemperature:
                    return expandedTemperatureLegend;
                case Game.TemperatureOverlayModes.HeatFlow:
                    return heatFlowLegend;
                case Game.TemperatureOverlayModes.StateChange:
                    return stateChangeLegend;
                case Game.TemperatureOverlayModes.RelativeTemperature:
                    return new List<LegendEntry>();
                default:
                    return temperatureLegend;
            }
        }

        public override void OnFiltersChanged() {
            if (InFilter(ToolParameterMenu.FILTERLAYERS.HEATFLOW, legendFilters))
                Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.HeatFlow;

            if (InFilter(ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, legendFilters))
                Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AbsoluteTemperature;

            if (InFilter(ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE, legendFilters))
                Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.RelativeTemperature;

            if (InFilter(ToolParameterMenu.FILTERLAYERS.ADAPTIVETEMPERATURE, legendFilters))
                Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AdaptiveTemperature;

            if (InFilter(ToolParameterMenu.FILTERLAYERS.STATECHANGE, legendFilters))
                Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.StateChange;

            switch (Game.Instance.temperatureOverlayMode) {
                case Game.TemperatureOverlayModes.AbsoluteTemperature:
                    Infrared.Instance.SetMode(Infrared.Mode.Infrared);
                    CameraController.Instance.ToggleColouredOverlayView(true);
                    return;
                case Game.TemperatureOverlayModes.AdaptiveTemperature:
                    Infrared.Instance.SetMode(Infrared.Mode.Infrared);
                    CameraController.Instance.ToggleColouredOverlayView(true);
                    return;
                case Game.TemperatureOverlayModes.HeatFlow:
                    Infrared.Instance.SetMode(Infrared.Mode.Disabled);
                    CameraController.Instance.ToggleColouredOverlayView(false);
                    return;
                case Game.TemperatureOverlayModes.StateChange:
                    Infrared.Instance.SetMode(Infrared.Mode.Disabled);
                    CameraController.Instance.ToggleColouredOverlayView(false);
                    return;
                case Game.TemperatureOverlayModes.RelativeTemperature:
                    Infrared.Instance.SetMode(Infrared.Mode.Infrared);
                    CameraController.Instance.ToggleColouredOverlayView(true);
                    return;
                default:
                    return;
            }
        }

        public override void Disable() {
            Infrared.Instance.SetMode(Infrared.Mode.Disabled);
            CameraController.Instance.ToggleColouredOverlayView(false);
            base.Disable();
        }
    }

    public class TileMode : Mode {
        public static readonly HashedString              ID = "TileMode";
        private readonly       int                       cameraLayerMask;
        private readonly       ColorHighlightCondition[] highlightConditions;
        private readonly       HashSet<PrimaryElement>   layerTargets = new HashSet<PrimaryElement>();
        private readonly       HashSet<Tag>              targetIDs    = new HashSet<Tag>();
        private readonly       int                       targetLayer;

        public TileMode() {
            var array = new ColorHighlightCondition[1];
            array[0] = new ColorHighlightCondition(delegate(KMonoBehaviour primary_element) {
                                                       var result = Color.black;
                                                       if (primary_element != null)
                                                           result = (primary_element as PrimaryElement).Element
                                                               .substance.uiColour;

                                                       return result;
                                                   },
                                                   primary_element =>
                                                       primary_element.gameObject.GetComponent<KBatchedAnimController>()
                                                                      .IsVisible());

            highlightConditions = array;
            base..ctor();
            targetLayer     = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            legendFilters   = CreateDefaultFilters();
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "SuitRequired"; }

        public override void Enable() {
            base.Enable();
            var prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<PrimaryElement>();
            targetIDs.UnionWith(prefabTagsWithComponent);
            Camera.main.cullingMask |= cameraLayerMask;
            var defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
            var mask             = LayerMask.GetMask("MaskedOverlay");
            SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
        }

        public override void Update() {
            Vector2I vector2I;
            Vector2I vector2I2;
            Grid.GetVisibleExtents(out vector2I, out vector2I2);
            RemoveOffscreenTargets(layerTargets, vector2I, vector2I2);
            var height  = vector2I2.y - vector2I.y;
            var width   = vector2I2.x - vector2I.x;
            var extents = new Extents(vector2I.x, vector2I.y, width, height);
            var list    = new List<ScenePartitionerEntry>();
            GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, list);
            foreach (var scenePartitionerEntry in list) {
                var component = ((Pickupable)scenePartitionerEntry.obj).gameObject.GetComponent<PrimaryElement>();
                if (component != null) TryAddObject(component, vector2I, vector2I2);
            }

            list.Clear();
            GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, list);
            foreach (var scenePartitionerEntry2 in list) {
                var buildingComplete = (BuildingComplete)scenePartitionerEntry2.obj;
                var component2       = buildingComplete.gameObject.GetComponent<PrimaryElement>();
                if (component2 != null && buildingComplete.gameObject.layer == 0)
                    TryAddObject(component2, vector2I, vector2I2);
            }

            UpdateHighlightTypeOverlay(vector2I,
                                       vector2I2,
                                       layerTargets,
                                       targetIDs,
                                       highlightConditions,
                                       BringToFrontLayerSetting.Conditional,
                                       targetLayer);
        }

        private void TryAddObject(PrimaryElement pe, Vector2I min, Vector2I max) {
            var element = pe.Element;
            foreach (var search_tag in Game.Instance.tileOverlayFilters)
                if (element.HasTag(search_tag)) {
                    AddTargetIfVisible(pe, min, max, layerTargets, targetLayer);
                    break;
                }
        }

        public override void Disable() {
            base.Disable();
            DisableHighlightTypeOverlay(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            layerTargets.Clear();
            SelectTool.Instance.ClearLayerMask();
        }

        public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters() {
            return new Dictionary<string, ToolParameterMenu.ToggleState> {
                { ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.METAL, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.BUILDABLE, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.FILTER, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.ORGANICS, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.FARMABLE, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.GAS, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.LIQUID, ToolParameterMenu.ToggleState.Off },
                { ToolParameterMenu.FILTERLAYERS.MISC, ToolParameterMenu.ToggleState.Off }
            };
        }

        public override void OnFiltersChanged() {
            Game.Instance.tileOverlayFilters.Clear();
            if (InFilter(ToolParameterMenu.FILTERLAYERS.METAL, legendFilters)) {
                Game.Instance.tileOverlayFilters.Add(GameTags.Metal);
                Game.Instance.tileOverlayFilters.Add(GameTags.RefinedMetal);
            }

            if (InFilter(ToolParameterMenu.FILTERLAYERS.BUILDABLE, legendFilters)) {
                Game.Instance.tileOverlayFilters.Add(GameTags.BuildableRaw);
                Game.Instance.tileOverlayFilters.Add(GameTags.BuildableProcessed);
            }

            if (InFilter(ToolParameterMenu.FILTERLAYERS.FILTER, legendFilters))
                Game.Instance.tileOverlayFilters.Add(GameTags.Filter);

            if (InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, legendFilters))
                Game.Instance.tileOverlayFilters.Add(GameTags.Liquifiable);

            if (InFilter(ToolParameterMenu.FILTERLAYERS.LIQUID, legendFilters))
                Game.Instance.tileOverlayFilters.Add(GameTags.Liquid);

            if (InFilter(ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, legendFilters)) {
                Game.Instance.tileOverlayFilters.Add(GameTags.ConsumableOre);
                Game.Instance.tileOverlayFilters.Add(GameTags.Sublimating);
            }

            if (InFilter(ToolParameterMenu.FILTERLAYERS.ORGANICS, legendFilters))
                Game.Instance.tileOverlayFilters.Add(GameTags.Organics);

            if (InFilter(ToolParameterMenu.FILTERLAYERS.FARMABLE, legendFilters)) {
                Game.Instance.tileOverlayFilters.Add(GameTags.Farmable);
                Game.Instance.tileOverlayFilters.Add(GameTags.Agriculture);
            }

            if (InFilter(ToolParameterMenu.FILTERLAYERS.GAS, legendFilters)) {
                Game.Instance.tileOverlayFilters.Add(GameTags.Breathable);
                Game.Instance.tileOverlayFilters.Add(GameTags.Unbreathable);
            }

            if (InFilter(ToolParameterMenu.FILTERLAYERS.MISC, legendFilters))
                Game.Instance.tileOverlayFilters.Add(GameTags.Other);

            DisableHighlightTypeOverlay(layerTargets);
            layerTargets.Clear();
            Game.Instance.ForceOverlayUpdate();
        }
    }
}