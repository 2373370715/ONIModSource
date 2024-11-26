using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, IGameObjectEffectDescriptor, ISim1000ms {
    private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate
        = new EventSystem.IntraObjectHandler<TinkerStation>(delegate(TinkerStation component, object data) {
                                                                component.OnOperationalChanged(data);
                                                            });

    public    bool            alwaysTinker;
    private   Chore           chore;
    public    HashedString    choreType;
    public    string          EffectItemString  = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM;
    public    string          EffectItemTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM;
    public    string          EffectTitle       = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS;
    public    string          EffectTooltip     = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS;
    public    HashedString    fetchChoreType;
    protected FilteredStorage filteredStorage;
    public    Tag             inputMaterial;
    public    float           massPerTinker;

    [MyCmpAdd]
    private Operational operational;

    public Tag   outputPrefab;
    public float outputTemperature;

    [MyCmpAdd]
    private Storage storage;

    public bool useFilteredStorage;

    public AttributeConverter AttributeConverter {
        set => attributeConverter = value;
    }

    public float AttributeExperienceMultiplier {
        set => attributeExperienceMultiplier = value;
    }

    public string SkillExperienceSkillGroup {
        set => skillExperienceSkillGroup = value;
    }

    public float SkillExperienceMultiplier {
        set => skillExperienceMultiplier = value;
    }

    public override List<Descriptor> GetDescriptors(GameObject go) {
        var arg         = inputMaterial.ProperName();
        var descriptors = base.GetDescriptors(go);
        descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE,
                                                     arg,
                                                     GameUtil.GetFormattedMass(massPerTinker)),
                                       string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE,
                                                     arg,
                                                     GameUtil.GetFormattedMass(massPerTinker)),
                                       Descriptor.DescriptorType.Requirement));

        descriptors.AddRange(GameUtil.GetAllDescriptors(Assets.GetPrefab(outputPrefab)));
        var list = new List<Tinkerable>();
        foreach (var gameObject in Assets.GetPrefabsWithComponent<Tinkerable>()) {
            var component = gameObject.GetComponent<Tinkerable>();
            if (component.tinkerMaterialTag == outputPrefab) list.Add(component);
        }

        if (list.Count > 0) {
            var effect = Db.Get().effects.Get(list[0].addedEffect);
            descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ADDED_EFFECT, effect.Name),
                                           string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT,
                                                         effect.Name,
                                                         Effect.CreateTooltip(effect, true))));

            descriptors.Add(new Descriptor(EffectTitle, EffectTooltip));
            foreach (var cmp in list) {
                var item = new Descriptor(string.Format(EffectItemString,  cmp.GetProperName()),
                                          string.Format(EffectItemTooltip, cmp.GetProperName()));

                item.IncreaseIndent();
                descriptors.Add(item);
            }
        }

        return descriptors;
    }

    public void Sim1000ms(float dt) { UpdateChore(); }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        attributeConverter            = Db.Get().AttributeConverters.MachinerySpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Technicals.Id;
        skillExperienceMultiplier     = SKILLS.MOST_DAY_EXPERIENCE;
        if (useFilteredStorage) {
            var byHash = Db.Get().ChoreTypes.GetByHash(fetchChoreType);
            filteredStorage = new FilteredStorage(this, null, null, false, byHash);
        }

        Subscribe(-592767678, OnOperationalChangedDelegate);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        if (useFilteredStorage && filteredStorage != null) filteredStorage.FilterChanged();
    }

    protected override void OnCleanUp() {
        if (filteredStorage != null) filteredStorage.CleanUp();
        base.OnCleanUp();
    }

    private bool CorrectRolePrecondition(MinionIdentity worker) {
        var component = worker.GetComponent<MinionResume>();
        return component != null && component.HasPerk(requiredSkillPerk);
    }

    private void OnOperationalChanged(object data) {
        var component = GetComponent<RoomTracker>();
        if (component != null && component.room != null) component.room.RetriggerBuildings();
    }

    protected override void OnStartWork(WorkerBase worker) {
        base.OnStartWork(worker);
        if (!operational.IsOperational) return;

        GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
        operational.SetActive(true);
    }

    protected override void OnStopWork(WorkerBase worker) {
        base.OnStopWork(worker);
        ShowProgressBar(false);
        GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
        operational.SetActive(false);
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        base.OnCompleteWork(worker);
        var primaryElement = storage.FindFirstWithMass(inputMaterial, massPerTinker);
        if (primaryElement != null) {
            var elementID = primaryElement.ElementID;
            storage.ConsumeIgnoringDisease(elementID.CreateTag(), massPerTinker);
            var gameObject = GameUtil.KInstantiate(Assets.GetPrefab(outputPrefab),
                                                   transform.GetPosition() + Vector3.up,
                                                   Grid.SceneLayer.Ore);

            var component = gameObject.GetComponent<PrimaryElement>();
            component.SetElement(elementID);
            component.Temperature = outputTemperature;
            gameObject.SetActive(true);
        }

        chore = null;
    }

    private void UpdateChore() {
        if (operational.IsOperational && (ToolsRequested() || alwaysTinker) && HasMaterial()) {
            if (chore == null) {
                chore = new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(choreType), this);
                chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, requiredSkillPerk);
                SetWorkTime(workTime);
            }
        } else if (chore != null) {
            chore.Cancel("Can't tinker");
            chore = null;
        }
    }

    private bool HasMaterial() { return storage.MassStored() > 0f; }

    private bool ToolsRequested() {
        return MaterialNeeds.GetAmount(outputPrefab, gameObject.GetMyWorldId(), false) > 0f &&
               this.GetMyWorld().worldInventory.GetAmount(outputPrefab, true)          <= 0f;
    }

    public static TinkerStation AddTinkerStation(GameObject go, string required_room_type) {
        var result = go.AddOrGet<TinkerStation>();
        go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
        return result;
    }
}