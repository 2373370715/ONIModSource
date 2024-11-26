using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;
using Random = UnityEngine.Random;

[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl {
    private const float STUDY_WORK_TIME = 3600f;
    private       Guid  additionalStatusItemGuid;
    private       Chore chore;

    [Serialize]
    private bool markedForStudy;

    public  string          meterAnim;
    public  string          meterTrackerSymbol;
    private Guid            statusItemGuid;
    public  MeterController studiedIndicator;

    [field: Serialize]
    public bool Studied { get; private set; }

    public bool   Studying           => chore != null && chore.InProgress();
    public string SidescreenTitleKey => "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";

    public string SidescreenStatusMessage {
        get {
            if (Studied) return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;

            if (markedForStudy) return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;

            return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
        }
    }

    public string SidescreenButtonText {
        get {
            if (Studied) return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;

            if (markedForStudy) return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;

            return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
        }
    }

    public string SidescreenButtonTooltip {
        get {
            if (Studied) return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;

            if (markedForStudy) return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;

            return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
        }
    }

    public int  HorizontalGroupID()                                { return -1; }
    public void SetButtonTextOverride(ButtonMenuTextOverride text) { throw new NotImplementedException(); }
    public bool SidescreenEnabled()                                { return true; }
    public bool SidescreenButtonInteractable()                     { return !Studied; }

    // 按下研究时触发
    public void OnSidescreenButtonPressed() {
        ToggleStudyChore();
    }
    public int  ButtonSideScreenSortOrder()                        { return 20; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        overrideAnims                 = new[] { Assets.GetAnim("anim_use_machine_kanim") };
        faceTargetWhenWorking         = true;
        synchronizeAnims              = false;
        workerStatusItem              = Db.Get().DuplicantStatusItems.Studying;
        resetProgressOnStop           = false;
        requiredSkillPerk             = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
        attributeConverter            = Db.Get().AttributeConverters.ResearchSpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Research.Id;
        skillExperienceMultiplier     = SKILLS.MOST_DAY_EXPERIENCE;
        SetWorkTime(3600f);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        studiedIndicator = new MeterController(GetComponent<KBatchedAnimController>(),
                                               meterTrackerSymbol,
                                               meterAnim,
                                               Meter.Offset.Infront,
                                               Grid.SceneLayer.NoLayer,
                                               meterTrackerSymbol);

        studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
        Refresh();
    }

    /**
     * 取消研究任务
     */
    public void CancelChore() {
        if (chore != null) {
            chore.Cancel("Studyable.CancelChore");
            chore = null;
            Trigger(1488501379);
        }
    }

    public void Refresh() {
        if (isLoadingScene) return;

        var component = GetComponent<KSelectable>();
        if (Studied) {
            statusItemGuid = component.ReplaceStatusItem(statusItemGuid, Db.Get().MiscStatusItems.Studied);
            studiedIndicator.gameObject.SetActive(true);
            studiedIndicator.meterController.Play(meterAnim, KAnim.PlayMode.Loop);
            requiredSkillPerk = null;
            UpdateStatusItem();
            return;
        }

        if (markedForStudy) {
            if (chore == null)
                chore = new WorkChore<Studyable>(Db.Get().ChoreTypes.Research,
                                                 this,
                                                 null,
                                                 true,
                                                 null,
                                                 null,
                                                 null,
                                                 true,
                                                 null,
                                                 false,
                                                 false);

            statusItemGuid = component.ReplaceStatusItem(statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy);
        } else {
            CancelChore();
            statusItemGuid = component.RemoveStatusItem(statusItemGuid);
        }

        studiedIndicator.gameObject.SetActive(false);
    }

    private void ToggleStudyChore() {
        if (DebugHandler.InstantBuildMode) {
            Studied = true;
            if (chore != null) {
                chore.Cancel("debug");
                chore = null;
            }

            Trigger(-1436775550);
        } else
            markedForStudy = !markedForStudy;

        Refresh();
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        base.OnCompleteWork(worker);
        Studied = true;
        chore   = null;
        Refresh();
        Trigger(-1436775550);
        if (DlcManager.IsExpansion1Active()) DropDatabanks();
    }

    private void DropDatabanks() {
        var num = Random.Range(7, 13);
        for (var i = 0; i <= num; i++) {
            var gameObject = GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"),
                                                   transform.position + new Vector3(0f, 1f, 0f),
                                                   Grid.SceneLayer.Ore);

            gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
            gameObject.SetActive(true);
        }
    }
}