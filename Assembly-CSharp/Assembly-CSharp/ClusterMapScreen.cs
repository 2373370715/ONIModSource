using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClusterMapScreen : KScreen {
    public enum Mode {
        Default,
        SelectDestination
    }

    private const float                ZOOM_SCALE_MIN       = 50f;
    private const float                ZOOM_SCALE_MAX       = 150f;
    private const float                ZOOM_SCALE_INCREMENT = 25f;
    private const float                ZOOM_SCALE_SPEED     = 4f;
    private const float                ZOOM_NAME_THRESHOLD  = 115f;
    public static ClusterMapScreen     Instance;
    private       Coroutine            activeMoveToTargetRoutine;
    public        GameObject           cellVisContainer;
    public        ClusterMapVisualizer cellVisPrefab;
    public        KButton              closeButton;
    public        float                floatCycleOffset = 0.75f;
    public        float                floatCycleScale  = 4f;
    public        float                floatCycleSpeed  = 0.75f;
    public        GameObject           FXVisContainer;

    private readonly Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation
        = new Dictionary<AxialI, ClusterMapVisualizer>();

    private bool                       m_closeOnSelect;
    private float                      m_currentZoomScale = 75f;
    private ClusterDestinationSelector m_destinationSelector;

    private readonly Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims
        = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

    private readonly Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis
        = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

    private ClusterMapHex     m_hoveredHex;
    private Mode              m_mode;
    private Action<object>    m_onDestinationChangedDelegate;
    private Action<object>    m_onSelectObjectDelegate;
    private ClusterMapPath    m_previewMapPath;
    private ClusterGridEntity m_selectedEntity;
    private ClusterMapHex     m_selectedHex;
    private SelectMarker      m_selectMarker;
    private float             m_targetZoomScale = 75f;

    [SerializeField]
    private KScrollRect mapScrollRect;

    public  GameObject           mobileVisContainer;
    public  ClusterMapVisualizer mobileVisPrefab;
    private bool                 movingToTargetNISPosition;
    public  ClusterMapPathDrawer pathDrawer;
    public  GameObject           POIVisContainer;
    public  Color                rocketPathColor;
    public  Color                rocketPreviewPathColor;
    public  Color                rocketSelectedPathColor;

    [SerializeField]
    private readonly float scrollSpeed = 15f;

    public        GameObject           selectMarkerPrefab;
    private       AxialI               selectOnMoveNISComplete;
    public        ClusterMapVisualizer staticVisPrefab;
    private       Vector3              targetNISPosition;
    private       float                targetNISZoom;
    public        GameObject           telescopeVisContainer;
    public        GameObject           terrainVisContainer;
    public        ClusterMapVisualizer terrainVisPrefab;
    public static void                 DestroyInstance() { Instance = null; }

    public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity) {
        if (m_gridEntityAnims.ContainsKey(entity)) return m_gridEntityAnims[entity];

        return null;
    }

    public override float GetSortKey() {
        if (isEditing) return 50f;

        return 20f;
    }

    public float CurrentZoomPercentage() { return (m_currentZoomScale - 50f) / 100f; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        m_selectMarker = Util.KInstantiateUI<SelectMarker>(selectMarkerPrefab, gameObject);
        m_selectMarker.gameObject.SetActive(false);
        Instance = this;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        Debug.Assert(cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f),
                     "The radius of the cellVisPrefab hex must be 1");

        Debug.Assert(terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f),
                     "The radius of the terrainVisPrefab hex must be 1");

        Debug.Assert(mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f),
                     "The radius of the mobileVisPrefab hex must be 1");

        Debug.Assert(staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f),
                     "The radius of the staticVisPrefab hex must be 1");

        int num;
        int num2;
        int num3;
        int num4;
        GenerateGridVis(out num, out num2, out num3, out num4);
        Show(false);
        mapScrollRect.content.sizeDelta  = new Vector2(num2 * 4, num4 * 4);
        mapScrollRect.content.localScale = new Vector3(m_currentZoomScale, m_currentZoomScale, 1f);
        m_onDestinationChangedDelegate   = OnDestinationChanged;
        m_onSelectObjectDelegate         = OnSelectObject;
        Subscribe(1980521255, UpdateVis);
    }

    protected void MoveToNISPosition() {
        if (!movingToTargetNISPosition) return;

        var b = new Vector3(-targetNISPosition.x * mapScrollRect.content.localScale.x,
                            -targetNISPosition.y * mapScrollRect.content.localScale.y,
                            targetNISPosition.z);

        m_targetZoomScale = Mathf.Lerp(m_targetZoomScale, targetNISZoom, Time.unscaledDeltaTime * 2f);
        mapScrollRect.content.SetLocalPosition(Vector3.Lerp(mapScrollRect.content.GetLocalPosition(),
                                                            b,
                                                            Time.unscaledDeltaTime * 2.5f));

        var num = Vector3.Distance(mapScrollRect.content.GetLocalPosition(), b);
        if (num < 100f) {
            var component = m_cellVisByLocation[selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
            if (m_selectedHex != component) SelectHex(component);
            if (num           < 10f) movingToTargetNISPosition = false;
        }
    }

    public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f) {
        if (activeMoveToTargetRoutine != null) StopCoroutine(activeMoveToTargetRoutine);
        activeMoveToTargetRoutine = StartCoroutine(MoveToTargetRoutine(targetPosition, delayBeforeMove));
    }

    private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove) {
        delayBeforeMove = Mathf.Max(delayBeforeMove, 0f);
        yield return SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);

        targetNISPosition         = AxialUtil.AxialToWorld(targetPosition.r, targetPosition.q);
        targetNISZoom             = 150f;
        movingToTargetNISPosition = true;
        selectOnMoveNISComplete   = targetPosition;
    }

    public override void OnKeyDown(KButtonEvent e) {
        if (!e.Consumed                                               &&
            (e.IsAction(Action.ZoomIn) || e.IsAction(Action.ZoomOut)) &&
            CameraController.IsMouseOverGameWindow) {
            var list             = new List<RaycastResult>();
            var pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointerEventData.position = KInputManager.GetMousePos();
            var current = UnityEngine.EventSystems.EventSystem.current;
            if (current != null) {
                current.RaycastAll(pointerEventData, list);
                var flag = false;
                foreach (var raycastResult in list)
                    if (!raycastResult.gameObject.transform.IsChildOf(transform)) {
                        flag = true;
                        break;
                    }

                if (!flag) {
                    float num;
                    if (KInputManager.currentControllerIsGamepad) {
                        num =  25f;
                        num *= e.IsAction(Action.ZoomIn) ? 1 : -1;
                    } else
                        num = Input.mouseScrollDelta.y * 25f;

                    m_targetZoomScale = Mathf.Clamp(m_targetZoomScale + num, 50f, 150f);
                    e.TryConsume(Action.ZoomIn);
                    if (!e.Consumed) e.TryConsume(Action.ZoomOut);
                }
            }
        }

        CameraController.Instance.ChangeWorldInput(e);
        base.OnKeyDown(e);
    }

    public bool TryHandleCancel() {
        if (m_mode == Mode.SelectDestination && !m_closeOnSelect) {
            SetMode(Mode.Default);
            return true;
        }

        return false;
    }

    public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector) {
        m_destinationSelector = destination_selector;
        if (!gameObject.activeSelf) {
            ManagementMenu.Instance.ToggleClusterMap();
            m_closeOnSelect = true;
        }

        var component = destination_selector.GetComponent<ClusterGridEntity>();
        SetSelectedEntity(component);
        if (m_selectedEntity != null)
            m_selectedHex = m_cellVisByLocation[m_selectedEntity.Location].GetComponent<ClusterMapHex>();
        else {
            var myWorldLocation = destination_selector.GetMyWorldLocation();
            var component2      = m_cellVisByLocation[myWorldLocation].GetComponent<ClusterMapHex>();
            m_selectedHex = component2;
        }

        SetMode(Mode.SelectDestination);
    }

    private void SetMode(Mode mode) {
        m_mode = mode;
        if (m_mode == Mode.Default) m_destinationSelector = null;
        UpdateVis();
    }

    public Mode GetMode() { return m_mode; }

    protected override void OnShow(bool show) {
        base.OnShow(show);
        if (show) {
            MoveToNISPosition();
            UpdateVis();
            if (m_mode == Mode.Default) TrySelectDefault();
            Game.Instance.Subscribe(-1991583975, OnFogOfWarRevealed);
            Game.Instance.Subscribe(-1554423969, OnNewTelescopeTarget);
            Game.Instance.Subscribe(-1298331547, OnClusterLocationChanged);
            ClusterMapSelectTool.Instance.Activate();
            SetShowingNonClusterMapHud(false);
            CameraController.Instance.DisableUserCameraControl = true;
            AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
            MusicManager.instance.PlaySong("Music_Starmap");
            UpdateTearStatus();
            return;
        }

        Game.Instance.Unsubscribe(-1554423969, OnNewTelescopeTarget);
        Game.Instance.Unsubscribe(-1991583975, OnFogOfWarRevealed);
        Game.Instance.Unsubscribe(-1298331547, OnClusterLocationChanged);
        m_mode                = Mode.Default;
        m_closeOnSelect       = false;
        m_destinationSelector = null;
        SelectTool.Instance.Activate();
        SetShowingNonClusterMapHud(true);
        CameraController.Instance.DisableUserCameraControl = false;
        AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
        if (MusicManager.instance.SongIsPlaying("Music_Starmap")) MusicManager.instance.StopSong("Music_Starmap");
    }

    private void SetShowingNonClusterMapHud(bool show) {
        PlanScreen.Instance.gameObject.SetActive(show);
        ToolMenu.Instance.gameObject.SetActive(show);
        OverlayScreen.Instance.gameObject.SetActive(show);
    }

    private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false) {
        if (m_selectedEntity != null) {
            m_selectedEntity.Unsubscribe(543433792,   m_onDestinationChangedDelegate);
            m_selectedEntity.Unsubscribe(-1503271301, m_onSelectObjectDelegate);
        }

        m_selectedEntity = entity;
        if (m_selectedEntity != null) {
            m_selectedEntity.Subscribe(543433792,   m_onDestinationChangedDelegate);
            m_selectedEntity.Subscribe(-1503271301, m_onSelectObjectDelegate);
        }

        var new_selected = m_selectedEntity != null ? m_selectedEntity.GetComponent<KSelectable>() : null;
        if (frameDelay) {
            ClusterMapSelectTool.Instance.SelectNextFrame(new_selected);
            return;
        }

        ClusterMapSelectTool.Instance.Select(new_selected);
    }

    private void OnDestinationChanged(object data) { UpdateVis(); }

    private void OnSelectObject(object data) {
        if (m_selectedEntity == null) return;

        var component = m_selectedEntity.GetComponent<KSelectable>();
        if (component == null || component.IsSelected) return;

        SetSelectedEntity(null);
        if (m_mode == Mode.SelectDestination) {
            if (m_closeOnSelect)
                ManagementMenu.Instance.CloseAll();
            else
                SetMode(Mode.Default);
        }

        UpdateVis();
    }

    private void OnFogOfWarRevealed(object   data = null) { UpdateVis(); }
    private void OnNewTelescopeTarget(object data = null) { UpdateVis(); }

    private void Update() {
        if (KInputManager.currentControllerIsGamepad)
            mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * scrollSpeed);
    }

    private void TrySelectDefault() {
        if (m_selectedHex != null && m_selectedEntity != null) {
            UpdateVis();
            return;
        }

        var activeWorld = ClusterManager.Instance.activeWorld;
        if (activeWorld == null) return;

        var component = activeWorld.GetComponent<ClusterGridEntity>();
        if (component == null) return;

        SelectEntity(component);
    }

    private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ) {
        minR = int.MaxValue;
        maxR = int.MinValue;
        minQ = int.MaxValue;
        maxQ = int.MinValue;
        foreach (var keyValuePair in ClusterGrid.Instance.cellContents) {
            var clusterMapVisualizer
                = Instantiate(cellVisPrefab, Vector3.zero, Quaternion.identity, cellVisContainer.transform);

            clusterMapVisualizer.rectTransform().SetLocalPosition(keyValuePair.Key.ToWorld());
            clusterMapVisualizer.gameObject.SetActive(true);
            var component = clusterMapVisualizer.GetComponent<ClusterMapHex>();
            component.SetLocation(keyValuePair.Key);
            m_cellVisByLocation.Add(keyValuePair.Key, clusterMapVisualizer);
            minR = Mathf.Min(minR, component.location.R);
            maxR = Mathf.Max(maxR, component.location.R);
            minQ = Mathf.Min(minQ, component.location.Q);
            maxQ = Mathf.Max(maxQ, component.location.Q);
        }

        SetupVisGameObjects();
        UpdateVis();
    }

    public Transform GetGridEntityNameTarget(ClusterGridEntity entity) {
        ClusterMapVisualizer clusterMapVisualizer;
        if (m_currentZoomScale >= 115f && m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer))
            return clusterMapVisualizer.nameTarget;

        return null;
    }

    public override void ScreenUpdate(bool topLevel) {
        var t = Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f);
        m_currentZoomScale = Mathf.Lerp(m_currentZoomScale, m_targetZoomScale, t);
        Vector2 v = KInputManager.GetMousePos();
        var     b = mapScrollRect.content.InverseTransformPoint(v);
        mapScrollRect.content.localScale = new Vector3(m_currentZoomScale, m_currentZoomScale, 1f);
        var a = mapScrollRect.content.InverseTransformPoint(v);
        mapScrollRect.content.localPosition += (a - b) * m_currentZoomScale;
        MoveToNISPosition();
        FloatyAsteroidAnimation();
    }

    private void FloatyAsteroidAnimation() {
        var num = 0f;
        foreach (var worldContainer in ClusterManager.Instance.WorldContainers) {
            var component = worldContainer.GetComponent<AsteroidGridEntity>();
            if (component != null                      &&
                m_gridEntityVis.ContainsKey(component) &&
                GetRevealLevel(component) == ClusterRevealLevel.Visible) {
                KAnimControllerBase firstAnimController = m_gridEntityVis[component].GetFirstAnimController();
                var y = floatCycleOffset +
                        floatCycleScale * Mathf.Sin(floatCycleSpeed * (num + GameClock.Instance.GetTime()));

                firstAnimController.Offset = new Vector2(0f, y);
            }

            num += 1f;
        }
    }

    private void SetupVisGameObjects() {
        foreach (var keyValuePair in ClusterGrid.Instance.cellContents) {
            foreach (var clusterGridEntity in keyValuePair.Value) {
                ClusterGrid.Instance.GetCellRevealLevel(keyValuePair.Key);
                var isVisibleInFOW = clusterGridEntity.IsVisibleInFOW;
                var revealLevel    = GetRevealLevel(clusterGridEntity);
                if (clusterGridEntity.IsVisible              &&
                    revealLevel != ClusterRevealLevel.Hidden &&
                    !m_gridEntityVis.ContainsKey(clusterGridEntity)) {
                    ClusterMapVisualizer original   = null;
                    GameObject           gameObject = null;
                    switch (clusterGridEntity.Layer) {
                        case EntityLayer.Asteroid:
                            original   = terrainVisPrefab;
                            gameObject = terrainVisContainer;
                            break;
                        case EntityLayer.Craft:
                            original   = mobileVisPrefab;
                            gameObject = mobileVisContainer;
                            break;
                        case EntityLayer.POI:
                            original   = staticVisPrefab;
                            gameObject = POIVisContainer;
                            break;
                        case EntityLayer.Telescope:
                            original   = staticVisPrefab;
                            gameObject = telescopeVisContainer;
                            break;
                        case EntityLayer.Payload:
                            original   = mobileVisPrefab;
                            gameObject = mobileVisContainer;
                            break;
                        case EntityLayer.FX:
                            original   = staticVisPrefab;
                            gameObject = FXVisContainer;
                            break;
                    }

                    ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
                    var clusterMapVisualizer = Instantiate(original, gameObject.transform);
                    clusterMapVisualizer.Init(clusterGridEntity, pathDrawer);
                    clusterMapVisualizer.gameObject.SetActive(true);
                    m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
                    m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
                    clusterGridEntity.positionDirty = false;
                    clusterGridEntity.Subscribe(1502190696, RemoveDeletedEntities);
                }
            }
        }

        RemoveDeletedEntities();
        foreach (var keyValuePair2 in m_gridEntityVis) {
            var key = keyValuePair2.Key;
            if (key.Layer == EntityLayer.Asteroid) {
                var id = key.GetComponent<WorldContainer>().id;
                keyValuePair2.Value.alertVignette.worldID = id;
            }
        }
    }

    private void RemoveDeletedEntities(object obj = null) {
        foreach (var key in (from x in m_gridEntityVis.Keys where x == null || x.gameObject == (GameObject)obj select x)
                 .ToList()) {
            Util.KDestroyGameObject(m_gridEntityVis[key]);
            m_gridEntityVis.Remove(key);
            m_gridEntityAnims.Remove(key);
        }
    }

    private void OnClusterLocationChanged(object data) { UpdateVis(); }

    public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity) {
        var cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
        var isVisibleInFOW  = entity.IsVisibleInFOW;
        if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFOW == ClusterRevealLevel.Visible)
            return ClusterRevealLevel.Visible;

        if (cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFOW == ClusterRevealLevel.Peeked)
            return ClusterRevealLevel.Peeked;

        return ClusterRevealLevel.Hidden;
    }

    private void UpdateVis(object data = null) {
        SetupVisGameObjects();
        UpdatePaths();
        foreach (var keyValuePair in m_gridEntityAnims) {
            var revealLevel = GetRevealLevel(keyValuePair.Key);
            keyValuePair.Value.Show(revealLevel);
            var selected = m_selectedEntity == keyValuePair.Key;
            keyValuePair.Value.Select(selected);
            if (keyValuePair.Key.positionDirty) {
                var position = ClusterGrid.Instance.GetPosition(keyValuePair.Key);
                keyValuePair.Value.rectTransform().SetLocalPosition(position);
                keyValuePair.Key.positionDirty = false;
            }
        }

        if (m_selectedEntity != null && m_gridEntityVis.ContainsKey(m_selectedEntity)) {
            var clusterMapVisualizer = m_gridEntityVis[m_selectedEntity];
            m_selectMarker.SetTargetTransform(clusterMapVisualizer.transform);
            m_selectMarker.gameObject.SetActive(true);
            clusterMapVisualizer.transform.SetAsLastSibling();
        } else
            m_selectMarker.gameObject.SetActive(false);

        foreach (var keyValuePair2 in m_cellVisByLocation) {
            var component = keyValuePair2.Value.GetComponent<ClusterMapHex>();
            var key       = keyValuePair2.Key;
            component.SetRevealed(ClusterGrid.Instance.GetCellRevealLevel(key));
        }

        UpdateHexToggleStates();
        FloatyAsteroidAnimation();
    }

    private void OnEntityDestroyed(object obj) { RemoveDeletedEntities(); }

    private void UpdateHexToggleStates() {
        var flag = m_hoveredHex != null &&
                   ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(m_hoveredHex.location, EntityLayer.Asteroid);

        foreach (var keyValuePair in m_cellVisByLocation) {
            var                       component = keyValuePair.Value.GetComponent<ClusterMapHex>();
            var                       key       = keyValuePair.Key;
            ClusterMapHex.ToggleState state;
            if (m_selectedHex != null && m_selectedHex.location == key)
                state = ClusterMapHex.ToggleState.Selected;
            else if (flag && m_hoveredHex.location.IsAdjacent(key))
                state = ClusterMapHex.ToggleState.OrbitHighlight;
            else
                state = ClusterMapHex.ToggleState.Unselected;

            component.UpdateToggleState(state);
        }
    }

    public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false) {
        if (entity != null) {
            SetSelectedEntity(entity, frameDelay);
            var component = m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
            m_selectedHex = component;
        }

        UpdateVis();
    }

    public void SelectHex(ClusterMapHex newSelectionHex) {
        if (m_mode == Mode.Default) {
            var visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
            for (var i = visibleEntitiesAtCell.Count - 1; i >= 0; i--) {
                var component = visibleEntitiesAtCell[i].GetComponent<KSelectable>();
                if (component == null || !component.IsSelectable) visibleEntitiesAtCell.RemoveAt(i);
            }

            if (visibleEntitiesAtCell.Count == 0)
                SetSelectedEntity(null);
            else {
                var num             = visibleEntitiesAtCell.IndexOf(m_selectedEntity);
                var index           = 0;
                if (num >= 0) index = (num + 1) % visibleEntitiesAtCell.Count;
                SetSelectedEntity(visibleEntitiesAtCell[index]);
            }

            m_selectedHex = newSelectionHex;
        } else if (m_mode == Mode.SelectDestination) {
            Debug.Assert(m_destinationSelector != null,
                         "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");

            if (ClusterGrid.Instance.GetPath(m_selectedHex.location, newSelectionHex.location, m_destinationSelector) !=
                null) {
                m_destinationSelector.SetDestination(newSelectionHex.location);
                if (m_closeOnSelect)
                    ManagementMenu.Instance.CloseAll();
                else
                    SetMode(Mode.Default);
            }
        }

        UpdateVis();
    }

    public bool   HasCurrentHover()         { return m_hoveredHex != null; }
    public AxialI GetCurrentHoverLocation() { return m_hoveredHex.location; }

    public void OnHoverHex(ClusterMapHex newHoverHex) {
        m_hoveredHex = newHoverHex;
        if (m_mode == Mode.SelectDestination) UpdateVis();
        UpdateHexToggleStates();
    }

    public void OnUnhoverHex(ClusterMapHex unhoveredHex) {
        if (m_hoveredHex == unhoveredHex) {
            m_hoveredHex = null;
            UpdateHexToggleStates();
        }
    }

    public void SetLocationHighlight(AxialI location, bool highlight) {
        m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
    }

    private void UpdatePaths() {
        var clusterDestinationSelector = m_selectedEntity != null
                                             ? m_selectedEntity.GetComponent<ClusterDestinationSelector>()
                                             : null;

        if (m_mode != Mode.SelectDestination || !(m_hoveredHex != null)) {
            if (m_previewMapPath != null) {
                Util.KDestroyGameObject(m_previewMapPath);
                m_previewMapPath = null;
            }

            return;
        }

        Debug.Assert(m_destinationSelector != null, "In SelectDestination mode without a destination selector");
        var    myWorldLocation = m_destinationSelector.GetMyWorldLocation();
        string text;
        var path = ClusterGrid.Instance.GetPath(myWorldLocation,
                                                m_hoveredHex.location,
                                                m_destinationSelector,
                                                out text);

        if (path != null) {
            if (m_previewMapPath == null) m_previewMapPath = pathDrawer.AddPath();
            var clusterMapVisualizer = m_gridEntityVis[GetSelectorGridEntity(m_destinationSelector)];
            m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(clusterMapVisualizer.transform
                                                                                .localPosition,
                                                                            path));

            m_previewMapPath.SetColor(rocketPreviewPathColor);
        } else if (m_previewMapPath != null) {
            Util.KDestroyGameObject(m_previewMapPath);
            m_previewMapPath = null;
        }

        var num = path != null ? path.Count : -1;
        if (m_selectedEntity != null) {
            var rangeInTiles = m_selectedEntity.GetComponent<IClusterRange>().GetRangeInTiles();
            if (num > rangeInTiles && string.IsNullOrEmpty(text))
                text = string.Format(UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, rangeInTiles);

            var repeat = clusterDestinationSelector.GetComponent<RocketClusterDestinationSelector>().Repeat;
            m_hoveredHex.SetDestinationStatus(text, num, rangeInTiles, repeat);
            return;
        }

        m_hoveredHex.SetDestinationStatus(text);
    }

    private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector) {
        var component = selector.GetComponent<ClusterGridEntity>();
        if (component != null && ClusterGrid.Instance.IsVisible(component)) return component;

        var visibleEntityOfLayerAtCell
            = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);

        Debug.Assert(component != null || visibleEntityOfLayerAtCell != null,
                     string.Format("{0} has no grid entity and isn't located at a visible asteroid at {1}",
                                   selector,
                                   selector.GetMyWorldLocation()));

        if (visibleEntityOfLayerAtCell) return visibleEntityOfLayerAtCell;

        return component;
    }

    private void UpdateTearStatus() {
        ClusterPOIManager clusterPOIManager = null;
        if (ClusterManager.Instance != null)
            clusterPOIManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();

        if (clusterPOIManager != null) {
            var temporalTear = clusterPOIManager.GetTemporalTear();
            if (temporalTear != null) temporalTear.UpdateStatus();
        }
    }
}