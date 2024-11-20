using System;
using System.Collections;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/AchievementWidget")]
public class AchievementWidget : KMonoBehaviour {
    private readonly Color          color_dark_grey = new Color(0.21568628f, 0.21568628f, 0.21568628f);
    private readonly Color          color_dark_red  = new Color(0.28235295f, 0.16078432f, 0.14901961f);
    private readonly Color          color_gold      = new Color(1f,          0.63529414f, 0.28627452f);
    private readonly Color          color_grey      = new Color(0.6901961f,  0.6901961f,  0.6901961f);
    public           string         dlcIdFrom;
    public           AnimationCurve flourish_iconScaleCurve;
    public           AnimationCurve flourish_sheenPositionCurve;
    private          int            numRequirementsDisplayed;

    [SerializeField]
    private RectTransform progressParent;

    [SerializeField]
    private GameObject requirementPrefab;

    [SerializeField]
    private RectTransform sheenTransform;

    public KBatchedAnimController[] sparks;

    [SerializeField]
    private Sprite statusFailureIcon;

    [SerializeField]
    private Sprite statusSuccessIcon;

    protected override void OnSpawn() {
        base.OnSpawn();
        var component = GetComponent<MultiToggle>();
        component.onClick
            = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate { ExpandAchievement(); }));
    }

    private void Update() { }

    private void ExpandAchievement() {
        if (SaveGame.Instance != null) progressParent.gameObject.SetActive(!progressParent.gameObject.activeSelf);
    }

    public void ActivateNewlyAchievedFlourish(float delay = 1f) { StartCoroutine(Flourish(delay)); }

    private IEnumerator Flourish(float startDelay) {
        SetNeverAchieved();
        var canvas                 = GetComponent<Canvas>();
        if (canvas == null) canvas = gameObject.AddComponent<Canvas>();
        yield return SequenceUtil.WaitForSecondsRealtime(startDelay);

        var component              = transform.parent.parent.GetComponent<KScrollRect>();
        var num                    = 1.1f;
        var smoothAutoScrollTarget = 1f + transform.localPosition.y * num / component.content.rect.height;
        component.SetSmoothAutoScrollTarget(smoothAutoScrollTarget);
        yield return SequenceUtil.WaitForSecondsRealtime(0.5f);

        canvas.overrideSorting = true;
        canvas.sortingOrder    = 30;
        var icon = GetComponent<HierarchyReferences>().GetReference<Image>("icon").transform.parent.gameObject;
        foreach (var kbatchedAnimController in sparks)
            if (kbatchedAnimController.transform.parent != icon.transform.parent) {
                kbatchedAnimController.GetComponent<KBatchedAnimController>().TintColour
                    = new Color(1f, 0.86f, 0.56f, 1f);

                kbatchedAnimController.transform.SetParent(icon.transform.parent);
                kbatchedAnimController.transform.SetSiblingIndex(icon.transform.GetSiblingIndex());
                kbatchedAnimController.GetComponent<KBatchedAnimCanvasRenderer>().compare = CompareFunction.Always;
            }

        var component2 = GetComponent<HierarchyReferences>();
        component2.GetReference<Image>("iconBG").color     = color_dark_red;
        component2.GetReference<Image>("iconBorder").color = color_gold;
        component2.GetReference<Image>("icon").color       = color_gold;
        var colorChanged = false;
        var instance     = KFMOD.BeginOneShot(GlobalAssets.GetSound("AchievementUnlocked"), Vector3.zero);
        var num2         = Mathf.RoundToInt(MathUtil.Clamp(1f, 7f, startDelay - startDelay % 1f / 1f)) - 1;
        instance.setParameterByName("num_achievements", num2);
        KFMOD.EndOneShot(instance);
        for (var i = 0f; i < 1.2f; i += Time.unscaledDeltaTime) {
            icon.transform.localScale = Vector3.one * flourish_iconScaleCurve.Evaluate(i);
            sheenTransform.anchoredPosition
                = new Vector2(flourish_sheenPositionCurve.Evaluate(i), sheenTransform.anchoredPosition.y);

            if (i > 1f && !colorChanged) {
                colorChanged = true;
                var array = sparks;
                for (var j = 0; j < array.Length; j++) array[j].Play("spark");
                SetAchievedNow();
            }

            yield return SequenceUtil.WaitForNextFrame;
        }

        icon.transform.localScale = Vector3.one;
        CompleteFlourish();
        for (var i = 0f; i < 0.6f; i += Time.unscaledDeltaTime) yield return SequenceUtil.WaitForNextFrame;

        transform.localScale = Vector3.one;
    }

    public void CompleteFlourish() {
        var canvas                 = GetComponent<Canvas>();
        if (canvas == null) canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = false;
    }

    public void SetAchievedNow() {
        GetComponent<MultiToggle>().ChangeState(1);
        var component = GetComponent<HierarchyReferences>();
        component.GetReference<Image>("iconBG").color = color_dark_red;
        component.GetReference<Image>("iconBorder").color = color_gold;
        component.GetReference<Image>("icon").color = color_gold;
        var componentsInChildren = GetComponentsInChildren<LocText>();
        for (var i = 0; i < componentsInChildren.Length; i++) componentsInChildren[i].color = Color.white;
        ConfigureToolTip(GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_THIS_COLONY_TOOLTIP);
    }

    public void SetAchievedBefore() {
        GetComponent<MultiToggle>().ChangeState(1);
        var component = GetComponent<HierarchyReferences>();
        component.GetReference<Image>("iconBG").color = color_dark_red;
        component.GetReference<Image>("iconBorder").color = color_gold;
        component.GetReference<Image>("icon").color = color_gold;
        var componentsInChildren = GetComponentsInChildren<LocText>();
        for (var i = 0; i < componentsInChildren.Length; i++) componentsInChildren[i].color = Color.white;
        ConfigureToolTip(GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_OTHER_COLONY_TOOLTIP);
    }

    public void SetNeverAchieved() {
        GetComponent<MultiToggle>().ChangeState(2);
        var component = GetComponent<HierarchyReferences>();
        component.GetReference<Image>("iconBG").color     = color_dark_grey;
        component.GetReference<Image>("iconBorder").color = color_grey;
        component.GetReference<Image>("icon").color       = color_grey;
        foreach (var locText in GetComponentsInChildren<LocText>())
            locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);

        ConfigureToolTip(GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_EVER);
    }

    public void SetNotAchieved() {
        GetComponent<MultiToggle>().ChangeState(2);
        var component = GetComponent<HierarchyReferences>();
        component.GetReference<Image>("iconBG").color     = color_dark_grey;
        component.GetReference<Image>("iconBorder").color = color_grey;
        component.GetReference<Image>("icon").color       = color_grey;
        foreach (var locText in GetComponentsInChildren<LocText>())
            locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);

        ConfigureToolTip(GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_THIS_COLONY);
    }

    public void SetFailed() {
        GetComponent<MultiToggle>().ChangeState(2);
        var component = GetComponent<HierarchyReferences>();
        component.GetReference<Image>("iconBG").color = color_dark_grey;
        component.GetReference<Image>("iconBG").SetAlpha(0.5f);
        component.GetReference<Image>("iconBorder").color = color_grey;
        component.GetReference<Image>("iconBorder").SetAlpha(0.5f);
        component.GetReference<Image>("icon").color = color_grey;
        component.GetReference<Image>("icon").SetAlpha(0.5f);
        foreach (var locText in GetComponentsInChildren<LocText>())
            locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.25f);

        ConfigureToolTip(GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.FAILED_THIS_COLONY);
    }

    private void ConfigureToolTip(ToolTip tooltip, string status) {
        tooltip.ClearMultiStringTooltip();
        tooltip.AddMultiStringTooltip(status, null);
        if (SaveGame.Instance != null && !progressParent.gameObject.activeSelf)
            tooltip.AddMultiStringTooltip(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXPAND_TOOLTIP, null);

        if (DlcManager.IsDlcId(dlcIdFrom))
            tooltip.AddMultiStringTooltip(string.Format(COLONY_ACHIEVEMENTS.DLC_ACHIEVEMENT,
                                                        DlcManager.GetDlcTitle(dlcIdFrom)),
                                          null);
    }

    public void ShowProgress(ColonyAchievementStatus achievement) {
        if (progressParent == null) return;

        numRequirementsDisplayed = 0;
        for (var i = 0; i < achievement.Requirements.Count; i++) {
            var colonyAchievementRequirement = achievement.Requirements[i];
            if (colonyAchievementRequirement is CritterTypesWithTraits)
                ShowCritterChecklist(colonyAchievementRequirement);
            else if (colonyAchievementRequirement is DupesCompleteChoreInExoSuitForCycles)
                ShowDupesInExoSuitsRequirement(achievement.success, colonyAchievementRequirement);
            else if (colonyAchievementRequirement is DupesVsSolidTransferArmFetch)
                ShowArmsOutPeformingDupesRequirement(achievement.success, colonyAchievementRequirement);
            else if (colonyAchievementRequirement is ProduceXEngeryWithoutUsingYList)
                ShowEngeryWithoutUsing(achievement.success, colonyAchievementRequirement);
            else if (colonyAchievementRequirement is MinimumMorale)
                ShowMinimumMoraleRequirement(achievement.success, colonyAchievementRequirement);
            else if (colonyAchievementRequirement is SurviveARocketWithMinimumMorale)
                ShowRocketMoraleRequirement(achievement.success, colonyAchievementRequirement);
            else
                ShowRequirement(achievement.success, colonyAchievementRequirement);
        }
    }

    private HierarchyReferences GetNextRequirementWidget() {
        GameObject gameObject;
        if (progressParent.childCount <= numRequirementsDisplayed)
            gameObject = Util.KInstantiateUI(requirementPrefab, progressParent.gameObject, true);
        else {
            gameObject = progressParent.GetChild(numRequirementsDisplayed).gameObject;
            gameObject.SetActive(true);
        }

        numRequirementsDisplayed++;
        return gameObject.GetComponent<HierarchyReferences>();
    }

    private void SetDescription(string str, HierarchyReferences refs) {
        refs.GetReference<LocText>("Desc").SetText(str);
    }

    private void SetIcon(Sprite sprite, Color color, HierarchyReferences refs) {
        var reference = refs.GetReference<Image>("Icon");
        reference.sprite = sprite;
        reference.color  = color;
        reference.gameObject.SetActive(true);
    }

    private void ShowIcon(bool show, HierarchyReferences refs) {
        refs.GetReference<Image>("Icon").gameObject.SetActive(show);
    }

    private void ShowRequirement(bool succeed, ColonyAchievementRequirement req) {
        var nextRequirementWidget = GetNextRequirementWidget();
        var flag                  = req.Success() || succeed;
        var flag2                 = req.Fail();
        if (flag && !flag2)
            SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
        else if (flag2)
            SetIcon(statusFailureIcon, Color.red, nextRequirementWidget);
        else
            ShowIcon(false, nextRequirementWidget);

        SetDescription(req.GetProgress(flag), nextRequirementWidget);
    }

    private void ShowCritterChecklist(ColonyAchievementRequirement req) {
        var critterTypesWithTraits = req as CritterTypesWithTraits;
        if (req == null) return;

        foreach (var keyValuePair in critterTypesWithTraits.critterTypesToCheck) {
            var nextRequirementWidget = GetNextRequirementWidget();
            if (keyValuePair.Value)
                SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
            else
                ShowIcon(false, nextRequirementWidget);

            SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TAME_A_CRITTER,
                                         keyValuePair.Key.Name),
                           nextRequirementWidget);
        }
    }

    private void ShowArmsOutPeformingDupesRequirement(bool succeed, ColonyAchievementRequirement req) {
        var dupesVsSolidTransferArmFetch = req as DupesVsSolidTransferArmFetch;
        if (dupesVsSolidTransferArmFetch == null) return;

        var nextRequirementWidget = GetNextRequirementWidget();
        if (succeed) SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
        SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_PERFORMANCE,
                                     succeed
                                         ? dupesVsSolidTransferArmFetch.numCycles
                                         : dupesVsSolidTransferArmFetch.currentCycleCount,
                                     dupesVsSolidTransferArmFetch.numCycles),
                       nextRequirementWidget);

        if (!succeed) {
            var fetchDupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
            var fetchAutomatedChoreDeliveries
                = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;

            var num = 0;
            fetchDupeChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num);
            var num2 = 0;
            fetchAutomatedChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num2);
            nextRequirementWidget = GetNextRequirementWidget();
            if (num < num2 * dupesVsSolidTransferArmFetch.percentage)
                SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
            else
                ShowIcon(false, nextRequirementWidget);

            SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_VS_DUPE_FETCHES,
                                         "SolidTransferArm",
                                         num2,
                                         num),
                           nextRequirementWidget);
        }
    }

    private void ShowDupesInExoSuitsRequirement(bool succeed, ColonyAchievementRequirement req) {
        var dupesCompleteChoreInExoSuitForCycles = req as DupesCompleteChoreInExoSuitForCycles;
        if (dupesCompleteChoreInExoSuitForCycles == null) return;

        var nextRequirementWidget = GetNextRequirementWidget();
        if (succeed)
            SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
        else
            ShowIcon(false, nextRequirementWidget);

        SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_CYCLES,
                                     succeed
                                         ? dupesCompleteChoreInExoSuitForCycles.numCycles
                                         : dupesCompleteChoreInExoSuitForCycles.currentCycleStreak,
                                     dupesCompleteChoreInExoSuitForCycles.numCycles),
                       nextRequirementWidget);

        if (!succeed) {
            nextRequirementWidget = GetNextRequirementWidget();
            var num = dupesCompleteChoreInExoSuitForCycles.GetNumberOfDupesForCycle(GameClock.Instance.GetCycle());
            if (num >= Components.LiveMinionIdentities.Count) {
                num = Components.LiveMinionIdentities.Count;
                SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
            }

            SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_THIS_CYCLE,
                                         num,
                                         Components.LiveMinionIdentities.Count),
                           nextRequirementWidget);
        }
    }

    private void ShowEngeryWithoutUsing(bool succeed, ColonyAchievementRequirement req) {
        var produceXEngeryWithoutUsingYList = req as ProduceXEngeryWithoutUsingYList;
        if (req == null) return;

        var nextRequirementWidget = GetNextRequirementWidget();
        var productionAmount      = produceXEngeryWithoutUsingYList.GetProductionAmount(succeed);
        SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.GENERATE_POWER,
                                     GameUtil.GetFormattedRoundedJoules(productionAmount),
                                     GameUtil.GetFormattedRoundedJoules(produceXEngeryWithoutUsingYList
                                                                            .amountToProduce *
                                                                        1000f)),
                       nextRequirementWidget);

        if (succeed)
            SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
        else
            ShowIcon(false, nextRequirementWidget);

        foreach (var key in produceXEngeryWithoutUsingYList.disallowedBuildings) {
            nextRequirementWidget = GetNextRequirementWidget();
            if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
                SetIcon(statusFailureIcon, Color.red, nextRequirementWidget);
            else
                SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);

            var buildingDef = Assets.GetBuildingDef(key.Name);
            SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_BUILDING, buildingDef.Name),
                           nextRequirementWidget);
        }
    }

    private void ShowMinimumMoraleRequirement(bool success, ColonyAchievementRequirement req) {
        var minimumMorale = req as MinimumMorale;
        if (minimumMorale == null) return;

        if (success) {
            ShowRequirement(success, req);
            return;
        }

        foreach (var obj in Components.MinionAssignablesProxy) {
            var targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
            if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead)) {
                var attributeInstance
                    = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());

                if (attributeInstance != null) {
                    var nextRequirementWidget = GetNextRequirementWidget();
                    if (attributeInstance.GetTotalValue() >= minimumMorale.minimumMorale)
                        SetIcon(statusSuccessIcon, Color.green, nextRequirementWidget);
                    else
                        ShowIcon(false, nextRequirementWidget);

                    SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MORALE,
                                                 targetGameObject.GetProperName(),
                                                 attributeInstance.GetTotalDisplayValue()),
                                   nextRequirementWidget);
                }
            }
        }
    }

    private void ShowRocketMoraleRequirement(bool success, ColonyAchievementRequirement req) {
        var surviveARocketWithMinimumMorale = req as SurviveARocketWithMinimumMorale;
        if (surviveARocketWithMinimumMorale == null) return;

        if (success) {
            ShowRequirement(success, req);
            return;
        }

        foreach (var keyValuePair in
                 SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement) {
            var world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
            if (world != null) {
                var nextRequirementWidget = GetNextRequirementWidget();
                SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE,
                                             surviveARocketWithMinimumMorale.minimumMorale,
                                             keyValuePair.Value,
                                             surviveARocketWithMinimumMorale.numberOfCycles,
                                             world.GetProperName()),
                               nextRequirementWidget);
            }
        }
    }
}