using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandModuleSideScreen : SideScreenContent {
    public GameObject conditionListContainer;

    private readonly Dictionary<ProcessCondition, GameObject> conditionTable
        = new Dictionary<ProcessCondition, GameObject>();

    public MultiToggle debugVictoryButton;
    public MultiToggle destinationButton;
    public GameObject  prefabConditionLineItem;

    [Tooltip("This list is indexed by the ProcessCondition.Status enum")]
    public List<Color> statusColors;

    private LaunchConditionManager target;
    private SchedulerHandle        updateHandle;

    protected override void OnSpawn() {
        base.OnSpawn();
        ScheduleUpdate();
        var multiToggle = debugVictoryButton;
        multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick,
                                                              new System.Action(delegate {
                                                                                    var destination
                                                                                        = SpacecraftManager.instance
                                                                                            .destinations
                                                                                            .Find(match =>
                                                                                                match
                                                                                                    .GetDestinationType() ==
                                                                                                Db.Get()
                                                                                                    .SpaceDestinationTypes
                                                                                                    .Wormhole);

                                                                                    SaveGame.Instance
                                                                                        .GetComponent<
                                                                                            ColonyAchievementTracker>()
                                                                                        .DebugTriggerAchievement(Db
                                                                                            .Get()
                                                                                            .ColonyAchievements
                                                                                            .Clothe8Dupes.Id);

                                                                                    SaveGame.Instance
                                                                                        .GetComponent<
                                                                                            ColonyAchievementTracker>()
                                                                                        .DebugTriggerAchievement(Db
                                                                                            .Get()
                                                                                            .ColonyAchievements
                                                                                            .Build4NatureReserves
                                                                                            .Id);

                                                                                    SaveGame.Instance
                                                                                        .GetComponent<
                                                                                            ColonyAchievementTracker>()
                                                                                        .DebugTriggerAchievement(Db
                                                                                            .Get()
                                                                                            .ColonyAchievements
                                                                                            .ReachedSpace.Id);

                                                                                    target.Launch(destination);
                                                                                }));

        debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && CheckHydrogenRocket());
    }

    private bool CheckHydrogenRocket() {
        var rocketModule = target.rocketModules.Find(match => match.GetComponent<RocketEngine>());
        return rocketModule != null &&
               rocketModule.GetComponent<RocketEngine>().fuelTag ==
               ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
    }

    private void ScheduleUpdate() {
        updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen",
                                                     1f,
                                                     delegate {
                                                         RefreshConditions();
                                                         ScheduleUpdate();
                                                     });
    }

    public override bool IsValidForTarget(GameObject target) {
        return target.GetComponent<LaunchConditionManager>() != null;
    }

    public override void SetTarget(GameObject new_target) {
        if (new_target == null) {
            Debug.LogError("Invalid gameObject received");
            return;
        }

        target = new_target.GetComponent<LaunchConditionManager>();
        if (target == null) {
            Debug.LogError("The gameObject received does not contain a LaunchConditionManager component");
            return;
        }

        ClearConditions();
        ConfigureConditions();
        debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && CheckHydrogenRocket());
    }

    private void ClearConditions() {
        foreach (var keyValuePair in conditionTable) Util.KDestroyGameObject(keyValuePair.Value);
        conditionTable.Clear();
    }

    private void ConfigureConditions() {
        foreach (var key in target.GetLaunchConditionList()) {
            var value = Util.KInstantiateUI(prefabConditionLineItem, conditionListContainer, true);
            conditionTable.Add(key, value);
        }

        RefreshConditions();
    }

    public void RefreshConditions() {
        var flag                = false;
        var launchConditionList = target.GetLaunchConditionList();
        foreach (var processCondition in launchConditionList) {
            if (!conditionTable.ContainsKey(processCondition)) {
                flag = true;
                break;
            }

            var gameObject = conditionTable[processCondition];
            var component  = gameObject.GetComponent<HierarchyReferences>();
            if (processCondition.GetParentCondition()                     != null &&
                processCondition.GetParentCondition().EvaluateCondition() == ProcessCondition.Status.Failure)
                gameObject.SetActive(false);
            else if (!gameObject.activeSelf) gameObject.SetActive(true);

            var status = processCondition.EvaluateCondition();
            var flag2  = status == ProcessCondition.Status.Ready;
            component.GetReference<LocText>("Label").text  = processCondition.GetStatusMessage(status);
            component.GetReference<LocText>("Label").color = flag2 ? Color.black : Color.red;
            component.GetReference<Image>("Box").color     = flag2 ? Color.black : Color.red;
            component.GetReference<Image>("Check").gameObject.SetActive(flag2);
            gameObject.GetComponent<ToolTip>().SetSimpleTooltip(processCondition.GetStatusTooltip(status));
        }

        foreach (var keyValuePair in conditionTable)
            if (!launchConditionList.Contains(keyValuePair.Key)) {
                flag = true;
                break;
            }

        if (flag) {
            ClearConditions();
            ConfigureConditions();
        }

        destinationButton.gameObject.SetActive(ManagementMenu.StarmapAvailable());
        destinationButton.onClick = delegate { ManagementMenu.Instance.ToggleStarmap(); };
    }

    protected override void OnCleanUp() {
        updateHandle.ClearScheduler();
        base.OnCleanUp();
    }
}