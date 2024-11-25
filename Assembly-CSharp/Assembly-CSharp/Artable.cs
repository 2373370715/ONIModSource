using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Artable")]
public class Artable : Workable {
    private          AttributeModifier  artQualityDecorModifier;
    private          WorkChore<Artable> chore;
    public           string             defaultAnimName;
    private readonly string             defaultArtworkId = "Default";

    [Serialize]
    private string userChosenTargetStage;

    protected Artable() { faceTargetWhenWorking = true; }

    [field: Serialize]
    public string CurrentStage { get; private set; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        workerStatusItem              = Db.Get().DuplicantStatusItems.Arting;
        attributeConverter            = Db.Get().AttributeConverters.ArtSpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Art.Id;
        skillExperienceMultiplier     = SKILLS.MOST_DAY_EXPERIENCE;
        requiredSkillPerk             = Db.Get().SkillPerks.CanArt.Id;
        SetWorkTime(80f);
    }

    protected override void OnSpawn() {
        GetComponent<KPrefabID>().PrefabID();
        if (string.IsNullOrEmpty(CurrentStage) || CurrentStage == defaultArtworkId)
            SetDefault();
        else
            SetStage(CurrentStage, true);

        shouldShowSkillPerkStatusItem = false;
        base.OnSpawn();
    }

    [OnDeserialized]
    public void OnDeserialized() {
        if (Db.GetArtableStages().TryGet(CurrentStage) == null && CurrentStage != defaultArtworkId) {
            var id = string.Format("{0}_{1}", GetComponent<KPrefabID>().PrefabID().ToString(), CurrentStage);
            if (Db.GetArtableStages().TryGet(id) == null) {
                Debug.LogWarning("Failed up to update " + CurrentStage + " to ArtableStages");
                CurrentStage = defaultArtworkId;
                return;
            }

            CurrentStage = id;
        }
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        if (string.IsNullOrEmpty(userChosenTargetStage)) {
            var db           = Db.Get();
            var prefab_id    = GetComponent<KPrefabID>().PrefabID();
            var prefabStages = Db.GetArtableStages().GetPrefabStages(prefab_id);
            var artist_skill = db.ArtableStatuses.LookingUgly;
            var component    = worker.GetComponent<MinionResume>();
            if (component != null) {
                if (component.HasPerk(db.SkillPerks.CanArtGreat.Id))
                    artist_skill                                                      = db.ArtableStatuses.LookingGreat;
                else if (component.HasPerk(db.SkillPerks.CanArtOkay.Id)) artist_skill = db.ArtableStatuses.LookingOkay;
            }

            prefabStages.RemoveAll(stage => stage.statusItem.StatusType > artist_skill.StatusType ||
                                            stage.statusItem.StatusType ==
                                            ArtableStatuses.ArtableStatusType.AwaitingArting);

            prefabStages.Sort((x, y) => y.statusItem.StatusType.CompareTo(x.statusItem.StatusType));
            var highest_type = prefabStages[0].statusItem.StatusType;
            prefabStages.RemoveAll(stage => stage.statusItem.StatusType < highest_type);
            prefabStages.RemoveAll(stage => !stage.IsUnlocked());
            prefabStages.Shuffle();
            SetStage(prefabStages[0].id, false);
            if (prefabStages[0].cheerOnComplete)
                new EmoteChore(worker.GetComponent<ChoreProvider>(),
                               db.ChoreTypes.EmoteHighPriority,
                               db.Emotes.Minion.Cheer);
            else
                new EmoteChore(worker.GetComponent<ChoreProvider>(),
                               db.ChoreTypes.EmoteHighPriority,
                               db.Emotes.Minion.Disappointed);
        } else {
            SetStage(userChosenTargetStage, false);
            userChosenTargetStage = null;
        }

        shouldShowSkillPerkStatusItem = false;
        UpdateStatusItem();
        Prioritizable.RemoveRef(gameObject);
    }

    public void SetDefault() {
        CurrentStage = defaultArtworkId;
        GetComponent<KBatchedAnimController>().SwapAnims(GetComponent<Building>().Def.AnimFiles);
        GetComponent<KAnimControllerBase>().Play(defaultAnimName);
        var component = GetComponent<KSelectable>();
        var def       = GetComponent<Building>().Def;
        component.SetName(def.Name);
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().ArtableStatuses.AwaitingArting, this);
        this.GetAttributes().Remove(artQualityDecorModifier);
        shouldShowSkillPerkStatusItem = false;
        UpdateStatusItem();
        if (CurrentStage == defaultArtworkId) {
            shouldShowSkillPerkStatusItem = true;
            Prioritizable.AddRef(gameObject);
            chore = new WorkChore<Artable>(Db.Get().ChoreTypes.Art, this);
            chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, requiredSkillPerk);
        }
    }

    public virtual void SetStage(string stage_id, bool skip_effect) {
        var artableStage = Db.GetArtableStages().Get(stage_id);
        if (artableStage == null) {
            Debug.LogError("Missing stage: " + stage_id);
            return;
        }

        CurrentStage = artableStage.id;
        GetComponent<KBatchedAnimController>().SwapAnims(new[] { Assets.GetAnim(artableStage.animFile) });
        GetComponent<KAnimControllerBase>().Play(artableStage.anim);
        this.GetAttributes().Remove(artQualityDecorModifier);
        if (artableStage.decor != 0) {
            artQualityDecorModifier
                = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, artableStage.decor, "Art Quality");

            this.GetAttributes().Add(artQualityDecorModifier);
        }

        var component = GetComponent<KSelectable>();
        component.SetName(artableStage.Name);
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, artableStage.statusItem, this);
        gameObject.GetComponent<BuildingComplete>().SetDescriptionFlavour(artableStage.Description);
        shouldShowSkillPerkStatusItem = false;
        UpdateStatusItem();
    }

    public void SetUserChosenTargetState(string stageID) {
        SetDefault();
        userChosenTargetStage = stageID;
    }
}