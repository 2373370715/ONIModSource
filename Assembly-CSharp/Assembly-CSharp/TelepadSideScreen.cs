using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class TelepadSideScreen : SideScreenContent {
    [SerializeField]
    private GameObject checkboxLinePrefab;

    [SerializeField]
    private GameObject conditionContainerTemplate;

    private readonly Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries
        = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();

    [SerializeField]
    private Image newAchievementsEarned;

    [SerializeField]
    private KButton openRolesScreenButton;

    [SerializeField]
    private Image skillPointsAvailable;

    [SerializeField]
    private Telepad targetTelepad;

    [SerializeField]
    private LocText timeLabel;

    private readonly Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>
        victoryAchievementWidgets
            = new Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>();

    [SerializeField]
    private GameObject victoryConditionsContainer;

    [SerializeField]
    private KButton viewColonySummaryBtn;

    [SerializeField]
    private KButton viewImmigrantsBtn;

    protected override void OnSpawn() {
        base.OnSpawn();
        viewImmigrantsBtn.onClick += delegate {
                                         ImmigrantScreen.InitializeImmigrantScreen(targetTelepad);
                                         Game.Instance.Trigger(288942073);
                                     };

        viewColonySummaryBtn.onClick += delegate {
                                            newAchievementsEarned.gameObject.SetActive(false);
                                            MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance
                                                 .transform.parent.gameObject,
                                             RetireColonyUtility.GetCurrentColonyRetiredColonyData());
                                        };

        openRolesScreenButton.onClick += delegate { ManagementMenu.Instance.ToggleSkills(); };
        BuildVictoryConditions();
    }

    public override bool IsValidForTarget(GameObject target) { return target.GetComponent<Telepad>() != null; }

    public override void SetTarget(GameObject target) {
        var component = target.GetComponent<Telepad>();
        if (component == null) {
            Debug.LogError("Target doesn't have a telepad associated with it.");
            return;
        }

        targetTelepad = component;
        if (targetTelepad != null) {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }

    private void Update() {
        if (targetTelepad != null) {
            if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver()) {
                gameObject.SetActive(false);
                timeLabel.text = UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
                SetContentState(true);
            } else {
                if (targetTelepad.GetComponent<Operational>().IsOperational)
                    timeLabel.text = string.Format(UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION,
                                                   GameUtil.GetFormattedCycles(targetTelepad.GetTimeRemaining()));
                else
                    gameObject.SetActive(false);

                SetContentState(!Immigration.Instance.ImmigrantsAvailable);
            }

            UpdateVictoryConditions();
            UpdateAchievementsUnlocked();
            UpdateSkills();
        }
    }

    private void SetContentState(bool isLabel) {
        if (timeLabel.gameObject.activeInHierarchy         != isLabel) timeLabel.gameObject.SetActive(isLabel);
        if (viewImmigrantsBtn.gameObject.activeInHierarchy == isLabel) viewImmigrantsBtn.gameObject.SetActive(!isLabel);
    }

    private void BuildVictoryConditions() {
        foreach (var colonyAchievement in Db.Get().ColonyAchievements.resources)
            if (colonyAchievement.isVictoryCondition &&
                !colonyAchievement.Disabled          &&
                colonyAchievement.IsValidForSave()) {
                var dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
                victoryAchievementWidgets.Add(colonyAchievement, dictionary);
                var gameObject = Util.KInstantiateUI(conditionContainerTemplate, victoryConditionsContainer, true);
                gameObject.GetComponent<HierarchyReferences>()
                          .GetReference<LocText>("Label")
                          .SetText(colonyAchievement.Name);

                foreach (var colonyAchievementRequirement in colonyAchievement.requirementChecklist) {
                    var victoryColonyAchievementRequirement
                        = colonyAchievementRequirement as VictoryColonyAchievementRequirement;

                    if (victoryColonyAchievementRequirement != null) {
                        var gameObject2 = Util.KInstantiateUI(checkboxLinePrefab, gameObject, true);
                        gameObject2.GetComponent<HierarchyReferences>()
                                   .GetReference<LocText>("Label")
                                   .SetText(victoryColonyAchievementRequirement.Name());

                        gameObject2.GetComponent<ToolTip>()
                                   .SetSimpleTooltip(victoryColonyAchievementRequirement.Description());

                        dictionary.Add(colonyAchievementRequirement, gameObject2);
                    } else
                        Debug.LogWarning(string
                                             .Format("Colony achievement {0} is not a victory requirement but it is attached to a victory achievement {1}.",
                                                     colonyAchievementRequirement.GetType(),
                                                     colonyAchievement.Name));
                }

                entries.Add(colonyAchievement.Id, dictionary);
            }
    }

    private void UpdateVictoryConditions() {
        foreach (var colonyAchievement in Db.Get().ColonyAchievements.resources)
            if (colonyAchievement.isVictoryCondition &&
                !colonyAchievement.Disabled          &&
                colonyAchievement.IsValidForSave())
                foreach (var colonyAchievementRequirement in colonyAchievement.requirementChecklist)
                    entries[colonyAchievement.Id][colonyAchievementRequirement]
                        .GetComponent<HierarchyReferences>()
                        .GetReference<Image>("Check")
                        .enabled = colonyAchievementRequirement.Success();

        foreach (var keyValuePair in victoryAchievementWidgets) {
            foreach (var keyValuePair2 in keyValuePair.Value)
                keyValuePair2.Value.GetComponent<ToolTip>()
                             .SetSimpleTooltip(keyValuePair2.Key.GetProgress(keyValuePair2.Key.Success()));
        }
    }

    private void UpdateAchievementsUnlocked() {
        if (SaveGame.Instance.ColonyAchievementTracker.achievementsToDisplay.Count > 0)
            newAchievementsEarned.gameObject.SetActive(true);
    }

    private void UpdateSkills() {
        var active = false;
        foreach (var obj in Components.MinionResumes) {
            var minionResume = (MinionResume)obj;
            if (!minionResume.HasTag(GameTags.Dead) &&
                minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0) {
                active = true;
                break;
            }
        }

        skillPointsAvailable.gameObject.SetActive(active);
    }
}