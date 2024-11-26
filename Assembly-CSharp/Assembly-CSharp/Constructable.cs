using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/Workable/Constructable")]
public class Constructable : Workable, ISaveLoadable {
    private static readonly EventSystem.IntraObjectHandler<Constructable> OnReachableChangedDelegate
        = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data) {
                                                                component.OnReachableChanged(data);
                                                            });

    private static readonly EventSystem.IntraObjectHandler<Constructable> OnCancelDelegate
        = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data) {
                                                                component.OnCancel(data);
                                                            });

    private static readonly EventSystem.IntraObjectHandler<Constructable> OnRefreshUserMenuDelegate
        = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data) {
                                                                component.OnRefreshUserMenu(data);
                                                            });

    private Chore buildChore;

    [MyCmpReq]
    private Building building;

    private HandleVector<int>.Handle digPartitionerEntry;
    private FetchList2               fetchList;
    private bool                     finished;
    private bool                     hasLadderNearby;
    private bool                     hasUnreachableDigs;

    [Serialize]
    private int[] ids;

    private float        initialTemperature = -1f;
    private Notification invalidLocation;
    public  bool         isDiggingRequired = true;

    [Serialize]
    private bool isPrioritized;

    [Serialize]
    public bool IsReplacementTile;

    private Extents                  ladderDetectionExtents;
    private HandleVector<int>.Handle ladderParititonerEntry;
    private LoggerFSS                log = new LoggerFSS("Constructable");
    private bool                     materialNeedsCleared;

    [MyCmpAdd]
    private Notifier notifier;

    [MyCmpAdd]
    private Prioritizable prioritizable;

    [MyCmpGet]
    private Rotatable rotatable;

    private Element[] selectedElements;

    [Serialize]
    private Tag[] selectedElementsTags;

    private HandleVector<int>.Handle solidPartitionerEntry;

    [MyCmpAdd]
    private Storage storage;

    private bool   unmarked;
    private bool   waitForFetchesBeforeDigging;
    public  Recipe Recipe => building.Def.CraftRecipe;

    public IList<Tag> SelectedElementsTags {
        get => selectedElementsTags;
        set {
            if (selectedElementsTags == null || selectedElementsTags.Length != value.Count)
                selectedElementsTags = new Tag[value.Count];

            value.CopyTo(selectedElementsTags, 0);
        }
    }

    public override string GetConversationTopic() { return building.Def.PrefabID; }

    protected override void OnCompleteWork(WorkerBase worker) {
        var num  = 0f;
        var num2 = 0f;
        var flag = true;
        foreach (var gameObject in storage.items)
            if (!(gameObject == null)) {
                var component = gameObject.GetComponent<PrimaryElement>();
                if (!(component == null)) {
                    num  += component.Mass;
                    num2 += component.Temperature * component.Mass;
                    flag =  flag && component.HasTag(GameTags.Liquifiable);
                }
            }

        if (num <= 0f) {
            DebugUtil.LogWarningArgs(gameObject,
                                     "uhhh this constructable is about to generate a nan",
                                     "Item Count: ",
                                     storage.items.Count);

            return;
        }

        if (flag)
            initialTemperature = Mathf.Min(num2 / num, 318.15f);
        else
            initialTemperature = Mathf.Clamp(num2 / num, 0f, 318.15f);

        var component2  = GetComponent<KAnimGraphTileVisualizer>();
        var connections = component2 == null ? 0 : component2.Connections;
        var flag2       = true;
        if (IsReplacementTile) {
            var cell                 = Grid.PosToCell(transform.GetLocalPosition());
            var replacementCandidate = building.Def.GetReplacementCandidate(cell);
            if (replacementCandidate != null) {
                flag2 = false;
                var component3 = replacementCandidate.GetComponent<SimCellOccupier>();
                if (component3 != null)
                    component3.DestroySelf(delegate {
                                               if (this != null && gameObject != null)
                                                   FinishConstruction(connections, worker);
                                           });
                else {
                    var component4 = replacementCandidate.GetComponent<Conduit>();
                    if (component4 != null) component4.GetFlowManager().MarkForReplacement(cell);
                    var component5 = replacementCandidate.GetComponent<BuildingComplete>();
                    if (component5 != null)
                        component5.Subscribe(-21016276, delegate { FinishConstruction(connections, worker); });
                    else {
                        Debug.LogWarning("Why am I trying to replace a: " + replacementCandidate.name);
                        FinishConstruction(connections, worker);
                    }
                }

                var component6 = replacementCandidate.GetComponent<KAnimGraphTileVisualizer>();
                if (component6 != null) component6.skipCleanup = true;
                var component7 = replacementCandidate.GetComponent<Deconstructable>();
                if (component7 != null) component7.SpawnItemsFromConstruction(worker);
                var replaceCallbackParameters
                    = new ReplaceCallbackParameters { TileLayer = building.Def.TileLayer, Worker = worker };

                replacementCandidate.Trigger(1606648047, replaceCallbackParameters);
                replacementCandidate.DeleteObject();
            }
        }

        if (flag2) FinishConstruction(connections, worker);
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building,
                                      GetComponent<KSelectable>().GetName(),
                                      transform);
    }

    private void FinishConstruction(UtilityConnections connections, WorkerBase workerForGameplayEvent) {
        var component   = GetComponent<Rotatable>();
        var orientation = component != null ? component.GetOrientation() : Orientation.Neutral;
        var cell        = Grid.PosToCell(transform.GetLocalPosition());
        UnmarkArea();
        var gameObject = building.Def.Build(cell,
                                            orientation,
                                            storage,
                                            selectedElementsTags,
                                            initialTemperature,
                                            GetComponent<BuildingFacade>().CurrentFacade,
                                            true,
                                            GameClock.Instance.GetTime());

        var gameplayEventData = new BonusEvent.GameplayEventData();
        gameplayEventData.building     = gameObject.GetComponent<BuildingComplete>();
        gameplayEventData.workable     = this;
        gameplayEventData.worker       = workerForGameplayEvent;
        gameplayEventData.eventTrigger = GameHashes.NewBuilding;
        GameplayEventManager.Instance.Trigger(-1661515756, gameplayEventData);
        gameObject.transform.rotation = transform.rotation;
        var component2 = gameObject.GetComponent<Rotatable>();
        if (component2 != null) component2.SetOrientation(orientation);
        var component3 = GetComponent<KAnimGraphTileVisualizer>();
        if (component3 != null) {
            gameObject.GetComponent<KAnimGraphTileVisualizer>().Connections = connections;
            component3.skipCleanup                                          = true;
        }

        var component4 = GetComponent<KSelectable>();
        if (component4 != null && component4.IsSelected && gameObject.GetComponent<KSelectable>() != null) {
            component4.Unselect();
            if (PlayerController.Instance.ActiveTool.name == "SelectTool")
                ((SelectTool)PlayerController.Instance.ActiveTool)
                    .SelectNextFrame(gameObject.GetComponent<KSelectable>());
        }

        gameObject.Trigger(2121280625, this);
        storage.ConsumeAllIgnoringDisease();
        finished = true;
        this.DeleteObject();
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        invalidLocation = new Notification(MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.NAME,
                                           NotificationType.BadMinor,
                                           (notificationList, data) =>
                                               MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.TOOLTIP +
                                               notificationList.ReduceMessages(false));

        faceTargetWhenWorking = true;
        Subscribe(-1432940121, OnReachableChangedDelegate);
        if (rotatable                                                    == null) MarkArea();
        if (Db.Get().TechItems.GetTechTierForItem(building.Def.PrefabID) > 2) requireMinionToWork = true;
        workerStatusItem              = Db.Get().DuplicantStatusItems.Building;
        workingStatusItem             = null;
        attributeConverter            = Db.Get().AttributeConverters.ConstructionSpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
        minimumAttributeMultiplier    = 0.75f;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Building.Id;
        skillExperienceMultiplier     = SKILLS.MOST_DAY_EXPERIENCE;
        Prioritizable.AddRef(gameObject);
        synchronizeAnims      = false;
        multitoolContext      = "build";
        multitoolHitEffectTag = EffectConfigs.BuildSplashId;
        workingPstComplete    = null;
        workingPstFailed      = null;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        var table                           = OffsetGroups.InvertedStandardTable;
        if (building.Def.IsTilePiece) table = OffsetGroups.InvertedStandardTableWithCorners;
        var array                           = building.Def.PlacementOffsets;
        if (rotatable != null) {
            array = new CellOffset[building.Def.PlacementOffsets.Length];
            for (var i = 0; i < array.Length; i++)
                array[i] = rotatable.GetRotatedCellOffset(building.Def.PlacementOffsets[i]);
        }

        var offsetTable = OffsetGroups.BuildReachabilityTable(array, table, building.Def.ConstructionOffsetFilter);
        SetOffsetTable(offsetTable);
        storage.SetOffsetTable(offsetTable);
        Subscribe(2127324410, OnCancelDelegate);
        if (rotatable != null) MarkArea();
        fetchList = new FetchList2(storage, Db.Get().ChoreTypes.BuildFetch);
        var component = GetComponent<PrimaryElement>();
        var element   = ElementLoader.GetElement(SelectedElementsTags[0]);
        Debug.Assert(element != null, "Missing primary element for Constructable");
        component.ElementID = element.id;
        var b                                         = component.Element.highTemp - 10f;
        component.Temperature = component.Temperature = Mathf.Min(building.Def.Temperature, b);
        foreach (var ingredient in Recipe.GetAllIngredients(selectedElementsTags)) {
            fetchList.Add(ingredient.tag, null, ingredient.amount);
            MaterialNeeds.UpdateNeed(ingredient.tag, ingredient.amount, gameObject.GetMyWorldId());
        }

        if (!building.Def.IsTilePiece) gameObject.layer = LayerMask.NameToLayer("Construction");
        building.RunOnArea(delegate(int offset_cell) {
                               if (gameObject.GetComponent<ConduitBridge>() == null) {
                                   var gameObject2 = Grid.Objects[offset_cell, 7];
                                   if (gameObject2 != null) gameObject2.DeleteObject();
                               }
                           });

        if (IsReplacementTile && building.Def.ReplacementLayer != ObjectLayer.NumLayers) {
            var cell = Grid.PosToCell(transform.GetPosition());
            var x    = Grid.Objects[cell, (int)building.Def.ReplacementLayer];
            if (x == null || x == this.gameObject) {
                Grid.Objects[cell, (int)building.Def.ReplacementLayer] = this.gameObject;
                if (this.gameObject.GetComponent<SimCellOccupier>() != null) {
                    var renderLayer = LayerMask.NameToLayer("Overlay");
                    World.Instance.blockTileRenderer.AddBlock(renderLayer,
                                                              building.Def,
                                                              IsReplacementTile,
                                                              SimHashes.Void,
                                                              cell);
                }

                TileVisualizer.RefreshCell(cell, building.Def.TileLayer, building.Def.ReplacementLayer);
            } else {
                Debug.LogError("multiple replacement tiles on the same cell!");
                Util.KDestroyGameObject(this.gameObject);
            }

            var gameObject = Grid.Objects[cell, (int)building.Def.ObjectLayer];
            if (gameObject != null) {
                var component2 = gameObject.GetComponent<Deconstructable>();
                if (component2 != null) component2.CancelDeconstruction();
            }
        }

        bool flag = building.Def.BuildingComplete.GetComponent<Ladder>();
        waitForFetchesBeforeDigging = flag                                                          ||
                                      building.Def.BuildingComplete.GetComponent<SimCellOccupier>() ||
                                      building.Def.BuildingComplete.GetComponent<Door>()            ||
                                      building.Def.BuildingComplete.GetComponent<LiquidPumpingStation>();

        if (flag) {
            var x2  = 0;
            var num = 0;
            Grid.CellToXY(Grid.PosToCell(this), out x2, out num);
            var y = num - 3;
            ladderDetectionExtents = new Extents(x2, y, 1, 5);
            ladderParititonerEntry = GameScenePartitioner.Instance.Add("Constructable.OnNearbyBuildingLayerChanged",
                                                                       gameObject,
                                                                       ladderDetectionExtents,
                                                                       GameScenePartitioner.Instance.objectLayers[1],
                                                                       OnNearbyBuildingLayerChanged);

            OnNearbyBuildingLayerChanged(null);
        }

        fetchList.Submit(OnFetchListComplete, true);
        PlaceDiggables();
        new ReachabilityMonitor.Instance(this).StartSM();
        Subscribe(493375141, OnRefreshUserMenuDelegate);
        var component3    = GetComponent<Prioritizable>();
        var prioritizable = component3;
        prioritizable.onPriorityChanged
            = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged,
                                                        new Action<PrioritySetting>(OnPriorityChanged));

        OnPriorityChanged(component3.GetMasterPriority());
    }

    private void OnPriorityChanged(PrioritySetting priority) {
        building.RunOnArea(delegate(int cell) {
                               var diggable = Diggable.GetDiggable(cell);
                               if (diggable != null) diggable.GetComponent<Prioritizable>().SetMasterPriority(priority);
                           });
    }

    private void MarkArea() {
        var num         = Grid.PosToCell(transform.GetPosition());
        var def         = building.Def;
        var orientation = building.Orientation;
        var layer       = IsReplacementTile ? def.ReplacementLayer : def.ObjectLayer;
        def.MarkArea(num, orientation, layer, gameObject);
        if (def.IsTilePiece) {
            if (Grid.Objects[num, (int)def.TileLayer] == null) {
                def.MarkArea(num, orientation, def.TileLayer, gameObject);
                def.RunOnArea(num,
                              orientation,
                              delegate(int c) { TileVisualizer.RefreshCell(c, def.TileLayer, def.ReplacementLayer); });
            }

            Grid.IsTileUnderConstruction[num] = true;
        }
    }

    private void UnmarkArea() {
        if (unmarked) return;

        unmarked = true;
        var num   = Grid.PosToCell(transform.GetPosition());
        var def   = building.Def;
        var layer = IsReplacementTile ? building.Def.ReplacementLayer : building.Def.ObjectLayer;
        def.UnmarkArea(num, building.Orientation, layer, gameObject);
        if (def.IsTilePiece) Grid.IsTileUnderConstruction[num] = false;
    }

    private void OnNearbyBuildingLayerChanged(object data) {
        hasLadderNearby = false;
        for (var i = ladderDetectionExtents.y; i < ladderDetectionExtents.y + ladderDetectionExtents.height; i++) {
            var num = Grid.OffsetCell(0, ladderDetectionExtents.x, i);
            if (Grid.IsValidCell(num)) {
                GameObject gameObject = null;
                Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
                if (gameObject != null && gameObject.GetComponent<Ladder>() != null) {
                    hasLadderNearby = true;
                    return;
                }
            }
        }
    }

    private bool IsWire() { return building.Def.name.Contains("Wire"); }

    public bool IconConnectionAnimation(float delay, int connectionCount, string defName, string soundName) {
        var num = Grid.PosToCell(transform.GetPosition());
        if (this.building.Def.Name.Contains(defName)) {
            Building building                = null;
            var      gameObject              = Grid.Objects[num, 1];
            if (gameObject != null) building = gameObject.GetComponent<Building>();
            if (building != null) {
                var flag = IsWire();
                var num2 = flag ? building.GetPowerInputCell() : building.GetUtilityInputCell();
                var num3 = flag ? num2 : building.GetUtilityOutputCell();
                if (num == num2 || num == num3) {
                    var component = building.gameObject.GetComponent<BuildingCellVisualizer>();
                    if (component != null &&
                        (flag
                             ? (component.addedPorts &
                                (EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut)) >
                               0
                             : (component.addedPorts &
                                (EntityCellVisualizer.Ports.GasIn     |
                                 EntityCellVisualizer.Ports.GasOut    |
                                 EntityCellVisualizer.Ports.LiquidIn  |
                                 EntityCellVisualizer.Ports.LiquidOut |
                                 EntityCellVisualizer.Ports.SolidIn   |
                                 EntityCellVisualizer.Ports.SolidOut)) >
                               0)) {
                        component.ConnectedEventWithDelay(delay, connectionCount, num, soundName);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    protected override void OnCleanUp() {
        if (IsReplacementTile && building.Def.isKAnimTile) {
            var cell       = Grid.PosToCell(transform.GetPosition());
            var gameObject = Grid.Objects[cell, (int)building.Def.ReplacementLayer];
            if (gameObject == this.gameObject && gameObject.GetComponent<SimCellOccupier>() != null)
                World.Instance.blockTileRenderer.RemoveBlock(building.Def, IsReplacementTile, SimHashes.Void, cell);
        }

        GameScenePartitioner.Instance.Free(ref solidPartitionerEntry);
        GameScenePartitioner.Instance.Free(ref digPartitionerEntry);
        GameScenePartitioner.Instance.Free(ref ladderParititonerEntry);
        var component = GetComponent<SaveLoadRoot>();
        if (component != null) SaveLoader.Instance.saveManager.Unregister(component);
        if (fetchList != null) fetchList.Cancel("Constructable destroyed");
        UnmarkArea();
        var placementCells = building.PlacementCells;
        for (var i = 0; i < placementCells.Length; i++) {
            var diggable = Diggable.GetDiggable(placementCells[i]);
            if (diggable != null) diggable.gameObject.DeleteObject();
        }

        base.OnCleanUp();
    }

    private void OnDiggableReachabilityChanged(object data) {
        if (!IsReplacementTile) {
            var diggable_count    = 0;
            var unreachable_count = 0;
            building.RunOnArea(delegate(int offset_cell) {
                                   var diggable = Diggable.GetDiggable(offset_cell);
                                   if (diggable != null && diggable.isActiveAndEnabled) {
                                       var num = diggable_count + 1;
                                       diggable_count = num;
                                       if (!diggable.GetComponent<KPrefabID>().HasTag(GameTags.Reachable)) {
                                           num               = unreachable_count + 1;
                                           unreachable_count = num;
                                       }
                                   }
                               });

            var flag = unreachable_count > 0 && unreachable_count == diggable_count;
            if (flag != hasUnreachableDigs) {
                if (flag)
                    GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable);
                else
                    GetComponent<KSelectable>()
                        .RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable);

                hasUnreachableDigs = flag;
            }
        }
    }

    private void PlaceDiggables() {
        if (waitForFetchesBeforeDigging && fetchList != null && !hasLadderNearby) {
            OnDiggableReachabilityChanged(null);
            return;
        }

        var digs_complete = true;
        if (!solidPartitionerEntry.IsValid()) {
            var validPlacementExtents = building.GetValidPlacementExtents();
            solidPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnFetchListComplete",
                                                                      gameObject,
                                                                      validPlacementExtents,
                                                                      GameScenePartitioner.Instance.solidChangedLayer,
                                                                      OnSolidChangedOrDigDestroyed);

            digPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnFetchListComplete",
                                                                    gameObject,
                                                                    validPlacementExtents,
                                                                    GameScenePartitioner.Instance.digDestroyedLayer,
                                                                    OnSolidChangedOrDigDestroyed);
        }

        if (!IsReplacementTile) {
            building.RunOnArea(delegate(int offset_cell) {
                                   var masterPriority = GetComponent<Prioritizable>().GetMasterPriority();
                                   if (Diggable.IsDiggable(offset_cell)) {
                                       digs_complete = false;
                                       var diggable = Diggable.GetDiggable(offset_cell);
                                       if (diggable != null && !diggable.isActiveAndEnabled)
                                           diggable.Unsubscribe(-1432940121, OnDiggableReachabilityChanged);

                                       if (diggable == null || !diggable.isActiveAndEnabled) {
                                           diggable = GameUtil
                                                      .KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")),
                                                                    Grid.SceneLayer.Move)
                                                      .GetComponent<Diggable>();

                                           diggable.gameObject.SetActive(true);
                                           diggable.transform.SetPosition(Grid.CellToPosCBC(offset_cell,
                                                                           Grid.SceneLayer.Move));

                                           Grid.Objects[offset_cell, 7] = diggable.gameObject;
                                       }

                                       diggable.Subscribe(-1432940121, OnDiggableReachabilityChanged);
                                       diggable.choreTypeIdHash = Db.Get().ChoreTypes.BuildDig.IdHash;
                                       diggable.GetComponent<Prioritizable>().SetMasterPriority(masterPriority);
                                       RenderUtil.EnableRenderer(diggable.transform, false);
                                       var component = diggable.GetComponent<SaveLoadRoot>();
                                       if (component != null) Destroy(component);
                                   }
                               });

            OnDiggableReachabilityChanged(null);
        }

        var flag = building.Def.IsValidBuildLocation(gameObject,
                                                     transform.GetPosition(),
                                                     building.Orientation,
                                                     IsReplacementTile);

        if (flag)
            notifier.Remove(invalidLocation);
        else
            notifier.Add(invalidLocation);

        GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidBuildingLocation, !flag, this);
        var flag2 = digs_complete && flag && fetchList == null;
        if (flag2 && buildChore == null) {
            buildChore = new WorkChore<Constructable>(Db.Get().ChoreTypes.Build,
                                                      this,
                                                      null,
                                                      true,
                                                      UpdateBuildState,
                                                      UpdateBuildState,
                                                      UpdateBuildState,
                                                      true,
                                                      null,
                                                      false,
                                                      true,
                                                      null,
                                                      true);

            UpdateBuildState(buildChore);
            return;
        }

        if (!flag2 && buildChore != null) {
            buildChore.Cancel("Need to dig");
            buildChore = null;
        }
    }

    private void OnFetchListComplete() {
        fetchList = null;
        PlaceDiggables();
        ClearMaterialNeeds();
    }

    private void ClearMaterialNeeds() {
        if (materialNeedsCleared) return;

        foreach (var ingredient in Recipe.GetAllIngredients(SelectedElementsTags))
            MaterialNeeds.UpdateNeed(ingredient.tag, -ingredient.amount, gameObject.GetMyWorldId());

        materialNeedsCleared = true;
    }

    private void OnSolidChangedOrDigDestroyed(object data) {
        if (this == null || finished) return;

        PlaceDiggables();
    }

    private void UpdateBuildState(Chore chore) {
        var component = GetComponent<KSelectable>();
        if (chore.InProgress()) {
            component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.UnderConstruction);
            return;
        }

        component.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                Db.Get().BuildingStatusItems.UnderConstructionNoWorker);
    }

    [OnDeserialized]
    internal void OnDeserialized() {
        if (ids != null) {
            selectedElements = new Element[ids.Length];
            for (var i = 0; i < ids.Length; i++)
                selectedElements[i] = ElementLoader.FindElementByHash((SimHashes)ids[i]);

            if (selectedElementsTags == null) {
                selectedElementsTags = new Tag[ids.Length];
                for (var j = 0; j < ids.Length; j++)
                    selectedElementsTags[j] = ElementLoader.FindElementByHash((SimHashes)ids[j]).tag;
            }

            Debug.Assert(selectedElements.Length == selectedElementsTags.Length);
            for (var k = 0; k < selectedElements.Length; k++)
                Debug.Assert(selectedElements[k].tag == SelectedElementsTags[k]);
        }
    }

    private void OnReachableChanged(object data) {
        var component = GetComponent<KAnimControllerBase>();
        if ((bool)data) {
            GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable);
            if (component != null) component.TintColour = Game.Instance.uiColours.Build.validLocation;
        } else {
            GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable, this);
            if (component != null) component.TintColour = Game.Instance.uiColours.Build.unreachable;
        }
    }

    private void OnRefreshUserMenu(object data) {
        Game.Instance.userMenu.AddButton(gameObject,
                                         new KIconButtonMenu.ButtonInfo("action_cancel",
                                                                        UI.USERMENUACTIONS.CANCELCONSTRUCTION.NAME,
                                                                        OnPressCancel,
                                                                        Action.NumActions,
                                                                        null,
                                                                        null,
                                                                        null,
                                                                        UI.USERMENUACTIONS.CANCELCONSTRUCTION.TOOLTIP));
    }

    private void OnPressCancel() { gameObject.Trigger(2127324410); }

    private void OnCancel(object data = null) {
        DetailsScreen.Instance.Show(false);
        ClearMaterialNeeds();
    }

    public struct ReplaceCallbackParameters {
        public ObjectLayer TileLayer;
        public WorkerBase  Worker;
    }
}