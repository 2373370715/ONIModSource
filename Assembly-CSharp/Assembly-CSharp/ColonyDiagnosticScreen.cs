using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ColonyDiagnosticScreen : KScreen, ISim1000ms {
    public static ColonyDiagnosticScreen Instance;

    public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsActive
        = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> {
            {
                ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, "Diagnostic_Active_DuplicantThreatening"
            },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Bad, "Diagnostic_Active_Bad" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Warning, "Diagnostic_Active_Warning" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Concern, "Diagnostic_Active_Concern" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, "Diagnostic_Active_Suggestion" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, "Diagnostic_Active_Tutorial" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Good, "" }
        };

    public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsInactive
        = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> {
            {
                ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
                "Diagnostic_Inactive_DuplicantThreatening"
            },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Bad, "Diagnostic_Inactive_Bad" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Warning, "Diagnostic_Inactive_Warning" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Concern, "Diagnostic_Inactive_Concern" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, "Diagnostic_Inactive_Suggestion" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, "Diagnostic_Inactive_Tutorial" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "" },
            { ColonyDiagnostic.DiagnosticResult.Opinion.Good, "" }
        };

    public           GameObject          contentContainer;
    private readonly List<DiagnosticRow> diagnosticRows = new List<DiagnosticRow>();
    public           GameObject          header;
    public           GameObject          linePrefab;
    public           GameObject          rootIndicator;
    public           MultiToggle         seeAllButton;
    public           void                Sim1000ms(float dt) { RefreshAll(); }

    protected override void OnSpawn() {
        base.OnSpawn();
        Instance = this;
        RefreshSingleWorld();
        Game.Instance.Subscribe(1983128072, RefreshSingleWorld);
        var multiToggle = seeAllButton;
        multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick,
                                                              new System.Action(delegate {
                                                                                    var flag = !AllDiagnosticsScreen
                                                                                        .Instance
                                                                                        .isHiddenButActive;

                                                                                    AllDiagnosticsScreen.Instance
                                                                                        .Show(!flag);
                                                                                }));
    }

    protected override void OnForcedCleanUp() {
        Instance = null;
        base.OnForcedCleanUp();
    }

    private void RefreshSingleWorld(object data = null) {
        foreach (var diagnosticRow in diagnosticRows) {
            diagnosticRow.OnCleanUp();
            Util.KDestroyGameObject(diagnosticRow.gameObject);
        }

        diagnosticRows.Clear();
        SpawnTrackerLines(ClusterManager.Instance.activeWorldId);
    }

    private void SpawnTrackerLines(int world) {
        AddDiagnostic<BreathabilityDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<FoodDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<StressDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<RadiationDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<ReactorDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<FloatingRocketDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<RocketFuelDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<RocketOxidizerDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<FarmDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<ToiletDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<BedDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<IdleDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<TrappedDuplicantDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<EntombedDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<PowerUseDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<BatteryDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<RocketsInOrbitDiagnostic>(world, contentContainer, diagnosticRows);
        AddDiagnostic<MeteorDiagnostic>(world, contentContainer, diagnosticRows);
        var list = new List<DiagnosticRow>();
        foreach (var item in diagnosticRows) list.Add(item);
        list.Sort((a, b) => a.diagnostic.name.CompareTo(b.diagnostic.name));
        foreach (var diagnosticRow in list) diagnosticRow.gameObject.transform.SetAsLastSibling();
        list.Clear();
        seeAllButton.transform.SetAsLastSibling();
        RefreshAll();
    }

    private GameObject AddDiagnostic<T>(int worldID, GameObject parent, List<DiagnosticRow> parentCollection)
        where T : ColonyDiagnostic {
        var diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic<T>(worldID);
        if (diagnostic == null) return null;

        var gameObject = Util.KInstantiateUI(linePrefab, parent, true);
        parentCollection.Add(new DiagnosticRow(worldID, gameObject, diagnostic));
        return gameObject;
    }

    public static void
        SetIndication(ColonyDiagnostic.DiagnosticResult.Opinion opinion, GameObject indicatorGameObject) {
        indicatorGameObject.GetComponentInChildren<Image>().color = GetDiagnosticIndicationColor(opinion);
    }

    public static Color GetDiagnosticIndicationColor(ColonyDiagnostic.DiagnosticResult.Opinion opinion) {
        switch (opinion) {
            case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
            case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
            case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
                return Constants.NEGATIVE_COLOR;
            case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
                return Constants.WARNING_COLOR;
        }

        return Color.white;
    }

    public void RefreshAll() {
        foreach (var diagnosticRow in diagnosticRows)
            if (diagnosticRow.worldID == ClusterManager.Instance.activeWorldId)
                UpdateDiagnosticRow(diagnosticRow);

        SetIndication(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(ClusterManager.Instance.activeWorldId),
                      rootIndicator);

        seeAllButton.GetComponentInChildren<LocText>()
                    .SetText(string.Format(UI.DIAGNOSTICS_SCREEN.SEE_ALL, AllDiagnosticsScreen.Instance.GetRowCount()));
    }

    private ColonyDiagnostic.DiagnosticResult.Opinion UpdateDiagnosticRow(DiagnosticRow row) {
        var currentDisplayedResult = row.currentDisplayedResult;
        var activeInHierarchy      = row.gameObject.activeInHierarchy;
        if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(row.diagnostic.id))
            SetRowActive(row, false);
        else {
            switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[row.worldID][row.diagnostic.id]) {
                case ColonyDiagnosticUtility.DisplaySetting.Always:
                    SetRowActive(row, true);
                    break;
                case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
                    SetRowActive(row,
                                 row.diagnostic.LatestResult.opinion <
                                 ColonyDiagnostic.DiagnosticResult.Opinion.Normal);

                    break;
                case ColonyDiagnosticUtility.DisplaySetting.Never:
                    SetRowActive(row, false);
                    break;
            }

            if (row.gameObject.activeInHierarchy &&
                (row.currentDisplayedResult < currentDisplayedResult ||
                 (row.currentDisplayedResult < ColonyDiagnostic.DiagnosticResult.Opinion.Normal &&
                  !activeInHierarchy)) &&
                row.CheckAllowVisualNotification())
                row.TriggerVisualNotification();
        }

        return row.diagnostic.LatestResult.opinion;
    }

    private void SetRowActive(DiagnosticRow row, bool active) {
        if (row.gameObject.activeSelf != active) {
            row.gameObject.SetActive(active);
            row.ResolveNotificationRoutine();
        }
    }

    private class DiagnosticRow : ISim4000ms {
        private const    float                                     displayHistoryPeriod           = 600f;
        private const    float                                     MIN_TIME_BETWEEN_NOTIFICATIONS = 300f;
        private          Coroutine                                 activeRoutine;
        private readonly MultiToggle                               button;
        public           ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult;
        private          Vector2                                   defaultIndicatorSizeDelta;
        public readonly  ColonyDiagnostic                          diagnostic;
        private readonly Image                                     image;
        private readonly Image                                     indicator;
        public readonly  SparkLayer                                sparkLayer;
        private          float                                     timeOfLastNotification;
        private readonly LocText                                   titleLabel;
        private readonly ToolTip                                   tooltip;
        private readonly LocText                                   valueLabel;
        public readonly  int                                       worldID;

        public DiagnosticRow(int worldID, GameObject gameObject, ColonyDiagnostic diagnostic) {
            Debug.Assert(diagnostic != null);
            var component = gameObject.GetComponent<HierarchyReferences>();
            this.worldID    = worldID;
            sparkLayer      = component.GetReference<SparkLayer>("SparkLayer");
            this.diagnostic = diagnostic;
            titleLabel      = component.GetReference<LocText>("TitleLabel");
            valueLabel      = component.GetReference<LocText>("ValueLabel");
            indicator       = component.GetReference<Image>("Indicator");
            image           = component.GetReference<Image>("Image");
            tooltip         = gameObject.GetComponent<ToolTip>();
            this.gameObject = gameObject;
            titleLabel.SetText(diagnostic.name);
            sparkLayer.colorRules.setOwnColor = false;
            if (diagnostic.tracker == null)
                sparkLayer.transform.parent.gameObject.SetActive(false);
            else {
                sparkLayer.ClearLines();
                var points = diagnostic.tracker.ChartableData(600f);
                sparkLayer.NewLine(points, diagnostic.name);
            }

            button = gameObject.GetComponent<MultiToggle>();
            var multiToggle = button;
            multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick,
                                                                  new System.Action(delegate {
                                                                      KSelectable kselectable = null;
                                                                      var         pos         = Vector3.zero;
                                                                      if (diagnostic.LatestResult
                                                                              .clickThroughTarget !=
                                                                          null) {
                                                                          pos = diagnostic.LatestResult
                                                                              .clickThroughTarget.first;

                                                                          kselectable
                                                                              = diagnostic.LatestResult
                                                                                          .clickThroughTarget
                                                                                          .second ==
                                                                                      null
                                                                                      ? null
                                                                                      : diagnostic.LatestResult
                                                                                          .clickThroughTarget
                                                                                          .second
                                                                                          .GetComponent<
                                                                                              KSelectable>();
                                                                      } else {
                                                                          var nextClickThroughObject
                                                                              = diagnostic
                                                                                  .GetNextClickThroughObject();

                                                                          if (nextClickThroughObject != null) {
                                                                              kselectable
                                                                                  = nextClickThroughObject
                                                                                      .GetComponent<
                                                                                          KSelectable>();

                                                                              pos = nextClickThroughObject
                                                                                  .transform.GetPosition();
                                                                          }
                                                                      }

                                                                      if (kselectable == null) {
                                                                          CameraController.Instance
                                                                              .ActiveWorldStarWipe(diagnostic
                                                                                  .worldID);

                                                                          return;
                                                                      }

                                                                      SelectTool.Instance.SelectAndFocus(pos,
                                                                       kselectable);
                                                                  }));

            defaultIndicatorSizeDelta = Vector2.zero;
            Update(true);
            SimAndRenderScheduler.instance.Add(this, true);
        }

        public GameObject gameObject          { get; }
        public void       Sim4000ms(float dt) { Update(); }
        public void       OnCleanUp()         { SimAndRenderScheduler.instance.Remove(this); }

        public void Update(bool force = false) {
            if (!force && ClusterManager.Instance.activeWorldId != worldID) return;

            var color = Color.white;
            Debug.Assert(diagnostic.LatestResult.opinion > ColonyDiagnostic.DiagnosticResult.Opinion.Unset,
                         string.Format("{0} criteria returned no opinion. Make sure the DiagnosticResult parameters are used or an opinion result is otherwise set in all of its criteria",
                                       diagnostic));

            currentDisplayedResult = diagnostic.LatestResult.opinion;
            color                  = diagnostic.colors[diagnostic.LatestResult.opinion];
            if (diagnostic.tracker != null) {
                var data = diagnostic.tracker.ChartableData(600f);
                sparkLayer.RefreshLine(data, diagnostic.name);
                sparkLayer.SetColor(color);
            }

            indicator.color = diagnostic.colors[diagnostic.LatestResult.opinion];
            tooltip.SetSimpleTooltip((diagnostic.LatestResult.Message.IsNullOrWhiteSpace()
                                          ? UI.COLONY_DIAGNOSTICS.GENERIC_STATUS_NORMAL.text
                                          : diagnostic.LatestResult.Message) +
                                     "\n\n"                                  +
                                     UI.COLONY_DIAGNOSTICS.MUTE_TUTORIAL.text);

            var presentationSetting = diagnostic.presentationSetting;
            if (presentationSetting == ColonyDiagnostic.PresentationSetting.AverageValue ||
                presentationSetting != ColonyDiagnostic.PresentationSetting.CurrentValue)
                valueLabel.SetText(diagnostic.GetAverageValueString());
            else
                valueLabel.SetText(diagnostic.GetCurrentValueString());

            if (!string.IsNullOrEmpty(diagnostic.icon)) image.sprite = Assets.GetSprite(diagnostic.icon);
            if (color == Constants.NEUTRAL_COLOR) color              = Color.white;
            titleLabel.color = color;
        }

        public bool CheckAllowVisualNotification() {
            return timeOfLastNotification == 0f || GameClock.Instance.GetTime() >= timeOfLastNotification + 300f;
        }

        public void TriggerVisualNotification() {
            if (DebugHandler.NotificationsDisabled) return;

            if (activeRoutine == null) {
                timeOfLastNotification = GameClock.Instance.GetTime();
                KFMOD.PlayUISound(GlobalAssets.GetSound(notificationSoundsActive[currentDisplayedResult]));
                activeRoutine = gameObject.GetComponent<KMonoBehaviour>().StartCoroutine(VisualNotificationRoutine());
            }
        }

        private IEnumerator VisualNotificationRoutine() {
            gameObject.GetComponentInChildren<NotificationAnimator>().Begin(false);
            var indicator = gameObject.GetComponent<HierarchyReferences>()
                                      .GetReference<Image>("Indicator")
                                      .rectTransform;

            defaultIndicatorSizeDelta = Vector2.zero;
            indicator.sizeDelta       = defaultIndicatorSizeDelta;
            var bounceDuration = 3f;
            for (var i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime) {
                indicator.sizeDelta = defaultIndicatorSizeDelta +
                                      Vector2.one *
                                      Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));

                yield return 0;
            }

            for (var i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime) {
                indicator.sizeDelta = defaultIndicatorSizeDelta +
                                      Vector2.one *
                                      Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));

                yield return 0;
            }

            for (var i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime) {
                indicator.sizeDelta = defaultIndicatorSizeDelta +
                                      Vector2.one *
                                      Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));

                yield return 0;
            }

            ResolveNotificationRoutine();
        }

        public void ResolveNotificationRoutine() {
            gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform.sizeDelta
                = Vector2.zero;

            gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").localPosition
                = Vector2.zero;

            activeRoutine = null;
        }
    }
}