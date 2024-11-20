using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using ProcGen;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class SimpleInfoScreen : DetailScreenTab, ISim4000ms, ISim1000ms {
    private static readonly EventSystem.IntraObjectHandler<SimpleInfoScreen> OnRefreshDataDelegate
        = new EventSystem.IntraObjectHandler<SimpleInfoScreen>(delegate(SimpleInfoScreen component, object data) {
                                                                   component.OnRefreshData(data);
                                                               });

    [SerializeField]
    private GameObject attributesLabelButtonTemplate;

    [SerializeField]
    private GameObject attributesLabelTemplate;

    [SerializeField]
    private GameObject bigIconLabelRow;

    private readonly Dictionary<Tag, GameObject> biomeRows = new Dictionary<Tag, GameObject>();

    [SerializeField]
    private DescriptorPanel DescriptorContentPrefab;

    private          DescriptorPanel               effectsContent;
    private          CollapsibleDetailContentPanel effectsPanel;
    private          CollapsibleDetailContentPanel fertilityPanel;
    private readonly Dictionary<Tag, GameObject>   geyserRows = new Dictionary<Tag, GameObject>();
    public           GameObject                    iconLabelRow;
    private          CollapsibleDetailContentPanel infoPanel;
    private          GameObject                    lastTarget;
    private          Dictionary<Tag, GameObject>   lifeformRows     = new Dictionary<Tag, GameObject>();
    private readonly Dictionary<Tag, GameObject>   meteorShowerRows = new Dictionary<Tag, GameObject>();
    private          CollapsibleDetailContentPanel movePanel;
    private readonly List<StatusItemEntry>         oldStatusItems = new List<StatusItemEntry>();
    private          CollapsibleDetailContentPanel processConditionContainer;

    [SerializeField]
    private HierarchyReferences processConditionHeader;

    [SerializeField]
    private GameObject processConditionRow;

    private readonly List<GameObject>              processConditionRows = new List<GameObject>();
    private          DescriptorPanel               requirementContent;
    private          CollapsibleDetailContentPanel requirementsPanel;
    private          RocketSimpleInfoPanel         rocketSimpleInfoPanel;
    private          CollapsibleDetailContentPanel rocketStatusContainer;
    private          CollapsibleDetailContentPanel spacePOIPanel;
    public           GameObject                    spacerRow;
    private          SpacePOISimpleInfoPanel       spaceSimpleInfoPOIPanel;
    private          CollapsibleDetailContentPanel statusItemPanel;

    [SerializeField]
    private GameObject StatusItemPrefab;

    private readonly List<StatusItemEntry> statusItems = new List<StatusItemEntry>();
    private          GameObject            statusItemsFolder;

    [SerializeField]
    private TextStyleSetting StatusItemStyle_Main;

    [SerializeField]
    private TextStyleSetting StatusItemStyle_Other;

    [SerializeField]
    private Color statusItemTextColor_old = new Color(0.8235294f, 0.8235294f, 0.8235294f);

    [SerializeField]
    private readonly Color statusItemTextColor_regular = Color.black;

    [SerializeField]
    private Text StatusPanelCurrentActionLabel;

    [SerializeField]
    private Sprite statusWarningIcon;

    private          DetailsPanelDrawer            stressDrawer;
    private          CollapsibleDetailContentPanel stressPanel;
    private readonly List<GameObject>              surfaceConditionRows = new List<GameObject>();
    private          bool                          TargetIsMinion;

    [SerializeField]
    private TextStyleSetting ToolTipStyle_Property;

    private MinionVitalsPanel vitalsPanel;

    [SerializeField]
    private GameObject VitalsPanelTemplate;

    private          CollapsibleDetailContentPanel worldBiomesPanel;
    private          CollapsibleDetailContentPanel worldElementsPanel;
    private          CollapsibleDetailContentPanel worldGeysersPanel;
    private          CollapsibleDetailContentPanel worldLifePanel;
    private          CollapsibleDetailContentPanel worldMeteorShowersPanel;
    private readonly List<GameObject>              worldTraitRows = new List<GameObject>();
    private          CollapsibleDetailContentPanel worldTraitsPanel;
    public           CollapsibleDetailContentPanel StoragePanel { get; private set; }

    public void Sim1000ms(float dt) {
        if (selectedTarget != null && selectedTarget.GetComponent<IProcessConditionSet>() != null)
            RefreshProcessConditionsPanel();
    }

    public void Sim4000ms(float dt) {
        RefreshWorldPanel();
        spaceSimpleInfoPOIPanel.Refresh(spacePOIPanel, selectedTarget);
    }

    public override bool IsValidForTarget(GameObject target) { return true; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        processConditionContainer = CreateCollapsableSection(UI.DETAILTABS.PROCESS_CONDITIONS.NAME);
        statusItemPanel = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_STATUS);
        statusItemPanel.Content.GetComponent<VerticalLayoutGroup>().padding.bottom = 10;
        statusItemPanel.scalerMask.hoverLock = true;
        statusItemsFolder = statusItemPanel.Content.gameObject;
        spaceSimpleInfoPOIPanel = new SpacePOISimpleInfoPanel(this);
        spacePOIPanel = CreateCollapsableSection();
        rocketSimpleInfoPanel = new RocketSimpleInfoPanel(this);
        rocketStatusContainer = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ROCKET);
        vitalsPanel = Util.KInstantiateUI(VitalsPanelTemplate, gameObject).GetComponent<MinionVitalsPanel>();
        fertilityPanel = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_FERTILITY);
        infoPanel = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_DESCRIPTION);
        requirementsPanel = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
        requirementContent
            = Util.KInstantiateUI<DescriptorPanel>(DescriptorContentPrefab.gameObject,
                                                   requirementsPanel.Content.gameObject);

        effectsPanel = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_EFFECTS);
        effectsContent
            = Util.KInstantiateUI<DescriptorPanel>(DescriptorContentPrefab.gameObject, effectsPanel.Content.gameObject);

        worldMeteorShowersPanel = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_METEORSHOWERS);
        worldElementsPanel      = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ELEMENTS);
        worldGeysersPanel       = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_GEYSERS);
        worldTraitsPanel        = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_WORLDTRAITS);
        worldBiomesPanel        = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_BIOMES);
        worldLifePanel          = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_LIFE);
        StoragePanel            = CreateCollapsableSection();
        stressPanel             = CreateCollapsableSection();
        stressDrawer            = new DetailsPanelDrawer(attributesLabelTemplate, stressPanel.Content.gameObject);
        movePanel               = CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_MOVABLE);
        Subscribe(-1514841199, OnRefreshDataDelegate);
    }

    protected override void OnSelectTarget(GameObject target) {
        base.OnSelectTarget(target);
        Subscribe(target, -1697596308, TriggerRefreshStorage);
        Subscribe(target, -1197125120, TriggerRefreshStorage);
        Subscribe(target, 1059811075,  OnBreedingChanceChanged);
        var component = target.GetComponent<KSelectable>();
        if (component != null) {
            var statusItemGroup = component.GetStatusItemGroup();
            if (statusItemGroup != null) {
                var statusItemGroup2 = statusItemGroup;
                statusItemGroup2.OnAddStatusItem
                    = (Action<StatusItemGroup.Entry, StatusItemCategory>)
                    Delegate.Combine(statusItemGroup2.OnAddStatusItem,
                                     new Action<StatusItemGroup.Entry, StatusItemCategory>(OnAddStatusItem));

                var statusItemGroup3 = statusItemGroup;
                statusItemGroup3.OnRemoveStatusItem
                    = (Action<StatusItemGroup.Entry, bool>)Delegate.Combine(statusItemGroup3.OnRemoveStatusItem,
                                                                            new Action<StatusItemGroup.Entry,
                                                                                bool>(OnRemoveStatusItem));

                foreach (var entry in statusItemGroup)
                    if (entry.category != null && entry.category.Id == "Main")
                        DoAddStatusItem(entry, entry.category);

                foreach (var entry2 in statusItemGroup)
                    if (entry2.category == null || entry2.category.Id != "Main")
                        DoAddStatusItem(entry2, entry2.category);
            }
        }

        statusItemPanel.gameObject.SetActive(true);
        statusItemPanel.scalerMask.UpdateSize();
        Refresh(true);
        RefreshWorldPanel();
        RefreshProcessConditionsPanel();
        spaceSimpleInfoPOIPanel.Refresh(spacePOIPanel, selectedTarget);
    }

    public override void OnDeselectTarget(GameObject target) {
        base.OnDeselectTarget(target);
        if (target != null) {
            Unsubscribe(target, -1697596308, TriggerRefreshStorage);
            Unsubscribe(target, -1197125120, TriggerRefreshStorage);
            Unsubscribe(target, 1059811075,  OnBreedingChanceChanged);
        }

        var component = target.GetComponent<KSelectable>();
        if (component != null) {
            var statusItemGroup = component.GetStatusItemGroup();
            if (statusItemGroup != null) {
                var statusItemGroup2 = statusItemGroup;
                statusItemGroup2.OnAddStatusItem
                    = (Action<StatusItemGroup.Entry, StatusItemCategory>)
                    Delegate.Remove(statusItemGroup2.OnAddStatusItem,
                                    new Action<StatusItemGroup.Entry, StatusItemCategory>(OnAddStatusItem));

                var statusItemGroup3 = statusItemGroup;
                statusItemGroup3.OnRemoveStatusItem
                    = (Action<StatusItemGroup.Entry, bool>)Delegate.Remove(statusItemGroup3.OnRemoveStatusItem,
                                                                           new Action<StatusItemGroup.Entry,
                                                                               bool>(OnRemoveStatusItem));

                foreach (var statusItemEntry in statusItems) statusItemEntry.Destroy(true);
                statusItems.Clear();
                foreach (var statusItemEntry2 in oldStatusItems) {
                    statusItemEntry2.onDestroy = null;
                    statusItemEntry2.Destroy(true);
                }

                oldStatusItems.Clear();
            }
        }
    }

    private void OnStorageChange(object         data) { RefreshStoragePanel(StoragePanel, selectedTarget); }
    private void OnBreedingChanceChanged(object data) { RefreshFertilityPanel(fertilityPanel, selectedTarget); }

    private void OnAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category) {
        DoAddStatusItem(status_item, category);
    }

    private void DoAddStatusItem(StatusItemGroup.Entry status_item,
                                 StatusItemCategory    category,
                                 bool                  show_immediate = false) {
        var   gameObject = statusItemsFolder;
        Color color;
        if (status_item.item.notificationType == NotificationType.BadMinor ||
            status_item.item.notificationType == NotificationType.Bad      ||
            status_item.item.notificationType == NotificationType.DuplicantThreatening)
            color = GlobalAssets.Instance.colorSet.statusItemBad;
        else if (status_item.item.notificationType == NotificationType.Event)
            color = GlobalAssets.Instance.colorSet.statusItemEvent;
        else if (status_item.item.notificationType == NotificationType.MessageImportant)
            color = GlobalAssets.Instance.colorSet.statusItemMessageImportant;
        else
            color = statusItemTextColor_regular;

        var style = category == Db.Get().StatusItemCategories.Main ? StatusItemStyle_Main : StatusItemStyle_Other;
        var statusItemEntry = new StatusItemEntry(status_item,
                                                  category,
                                                  StatusItemPrefab,
                                                  gameObject.transform,
                                                  ToolTipStyle_Property,
                                                  color,
                                                  style,
                                                  show_immediate,
                                                  OnStatusItemDestroy);

        statusItemEntry.SetSprite(status_item.item.sprite);
        if (category != null) {
            var num = -1;
            foreach (var statusItemEntry2 in oldStatusItems.FindAll(e => e.category == category)) {
                num = statusItemEntry2.GetIndex();
                statusItemEntry2.Destroy(true);
                oldStatusItems.Remove(statusItemEntry2);
            }

            if (category == Db.Get().StatusItemCategories.Main) num = 0;
            if (num      != -1) statusItemEntry.SetIndex(num);
        }

        statusItems.Add(statusItemEntry);
    }

    private void OnRemoveStatusItem(StatusItemGroup.Entry status_item, bool immediate = false) {
        DoRemoveStatusItem(status_item, immediate);
    }

    private void DoRemoveStatusItem(StatusItemGroup.Entry status_item, bool destroy_immediate = false) {
        for (var i = 0; i < statusItems.Count; i++)
            if (statusItems[i].item.item == status_item.item) {
                var statusItemEntry = statusItems[i];
                statusItems.RemoveAt(i);
                oldStatusItems.Add(statusItemEntry);
                statusItemEntry.Destroy(destroy_immediate);
                return;
            }
    }

    private void OnStatusItemDestroy(StatusItemEntry item) { oldStatusItems.Remove(item); }
    private void OnRefreshData(object                obj)  { Refresh(); }

    protected override void Refresh(bool force = false) {
        if (selectedTarget != lastTarget || force) lastTarget = selectedTarget;
        var count                                             = statusItems.Count;
        statusItemPanel.gameObject.SetActive(count > 0);
        for (var i = 0; i < count; i++) statusItems[i].Refresh();
        RefreshStressPanel(stressPanel, selectedTarget);
        RefreshStoragePanel(StoragePanel, selectedTarget);
        RefreshMovePanel(movePanel, selectedTarget);
        RefreshFertilityPanel(fertilityPanel, selectedTarget);
        RefreshEffectsPanel(effectsPanel, selectedTarget, effectsContent);
        RefreshRequirementsPanel(requirementsPanel, selectedTarget, requirementContent);
        RefreshInfoPanel(infoPanel, selectedTarget);
        vitalsPanel.Refresh(selectedTarget);
        rocketSimpleInfoPanel.Refresh(rocketStatusContainer, selectedTarget);
    }

    private static void RefreshInfoPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var    text       = "";
        var    text2      = "";
        Object component  = targetEntity.GetComponent<MinionIdentity>();
        var    component2 = targetEntity.GetComponent<InfoDescription>();
        var    component3 = targetEntity.GetComponent<BuildingComplete>();
        var    component4 = targetEntity.GetComponent<BuildingUnderConstruction>();
        var    component5 = targetEntity.GetComponent<Edible>();
        var    component6 = targetEntity.GetComponent<PrimaryElement>();
        var    component7 = targetEntity.GetComponent<CellSelectionObject>();
        if (!component) {
            if (component2) {
                text  = component2.description;
                text2 = component2.effect;
            } else if (component3 != null)
                text = component3.DescEffect + "\n\n" + component3.Desc;
            else if (component4 != null)
                text = component4.DescEffect + "\n\n" + component4.Desc;
            else if (component5 != null) {
                var foodInfo = component5.FoodInfo;
                text += string.Format(UI.GAMEOBJECTEFFECTS.CALORIES,
                                      GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit));
            } else if (component7 != null)
                text = component7.element.FullDescription(false);
            else if (component6 != null) {
                var element = ElementLoader.FindElementByHash(component6.ElementID);
                text = element != null ? element.FullDescription(false) : "";
            }

            if (!string.IsNullOrEmpty(text)) targetPanel.SetLabel("Description", text, "");
            var flag  = !string.IsNullOrEmpty(text2) && text2 != "\n";
            var text3 = "\n" + text2;
            if (flag) targetPanel.SetLabel("Flavour", text3, "");
            var roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(targetEntity);
            if (roomClassForObject != null) {
                var text4                                     = "\n" + CODEX.HEADERS.BUILDINGTYPE + ":";
                foreach (var str in roomClassForObject) text4 = text4 + "\n    • " + str;
                targetPanel.SetLabel("RoomClass", text4, "");
            }
        }

        targetPanel.Commit();
    }

    private static void RefreshEffectsPanel(CollapsibleDetailContentPanel targetPanel,
                                            GameObject                    targetEntity,
                                            DescriptorPanel               effectsContent) {
        if (targetEntity.GetComponent<MinionIdentity>() != null) {
            targetPanel.SetActive(false);
            return;
        }

        targetEntity.GetComponent<BuildingComplete>();
        var component = targetEntity.GetComponent<BuildingUnderConstruction>();
        var gameObjectEffects
            = GameUtil.GetGameObjectEffects(component ? component.Def.BuildingComplete : targetEntity, true);

        var flag = gameObjectEffects.Count > 0;
        effectsContent.gameObject.SetActive(flag);
        if (flag) effectsContent.SetDescriptors(gameObjectEffects);
        targetPanel.SetActive(targetEntity != null && flag);
    }

    private static void RefreshRequirementsPanel(CollapsibleDetailContentPanel targetPanel,
                                                 GameObject                    targetEntity,
                                                 DescriptorPanel               requirementContent) {
        var    component  = targetEntity.GetComponent<MinionIdentity>();
        Object component2 = targetEntity.GetComponent<WiltCondition>();
        var    component3 = targetEntity.GetComponent<CreatureBrain>();
        if (component2 != null || component != null || component3 != null) {
            targetPanel.SetActive(false);
            return;
        }

        targetPanel.SetActive(true);
        var component4 = targetEntity.GetComponent<BuildingUnderConstruction>();
        var requirementDescriptors
            = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(component4
                                                                                ? component4.Def.BuildingComplete
                                                                                : targetEntity,
                                                                            true),
                                                 false);

        var flag = requirementDescriptors.Count > 0;
        requirementContent.gameObject.SetActive(flag);
        if (flag) requirementContent.SetDescriptors(requirementDescriptors);
        targetPanel.SetActive(flag);
    }

    private static void RefreshFertilityPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var smi = targetEntity.GetSMI<FertilityMonitor.Instance>();
        if (smi != null) {
            var num = 0;
            foreach (var breedingChance in smi.breedingChances) {
                var forTag = Db.Get().FertilityModifiers.GetForTag(breedingChance.egg);
                if (forTag.Count > 0) {
                    var text = "";
                    foreach (var fertilityModifier in forTag)
                        text += string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_MOD_FORMAT,
                                              fertilityModifier.GetTooltip());

                    targetPanel.SetLabel("breeding_" + num++,
                                         string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT,
                                                       breedingChance.egg.ProperName(),
                                                       GameUtil.GetFormattedPercent(breedingChance.weight * 100f)),
                                         string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP,
                                                       breedingChance.egg.ProperName(),
                                                       GameUtil.GetFormattedPercent(breedingChance.weight * 100f),
                                                       text));
                } else
                    targetPanel.SetLabel("breeding_" + num++,
                                         string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT,
                                                       breedingChance.egg.ProperName(),
                                                       GameUtil.GetFormattedPercent(breedingChance.weight * 100f)),
                                         string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP_NOMOD,
                                                       breedingChance.egg.ProperName(),
                                                       GameUtil.GetFormattedPercent(breedingChance.weight * 100f)));
            }
        }

        targetPanel.Commit();
    }

    private void TriggerRefreshStorage(object data = null) { RefreshStoragePanel(StoragePanel, selectedTarget); }

    private static void RefreshStoragePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        if (targetEntity == null) {
            targetPanel.gameObject.SetActive(false);
            targetPanel.Commit();
            return;
        }

        var array = targetEntity.GetComponentsInChildren<IStorage>();
        if (array == null) {
            targetPanel.gameObject.SetActive(false);
            targetPanel.Commit();
            return;
        }

        array = Array.FindAll(array, n => n.ShouldShowInUI());
        if (array.Length == 0) {
            targetPanel.gameObject.SetActive(false);
            targetPanel.Commit();
            return;
        }

        string title = targetEntity.GetComponent<MinionIdentity>() != null
                           ? UI.DETAILTABS.DETAILS.GROUPNAME_MINION_CONTENTS
                           : UI.DETAILTABS.DETAILS.GROUPNAME_CONTENTS;

        targetPanel.gameObject.SetActive(true);
        targetPanel.SetTitle(title);
        var num    = 0;
        var array2 = array;
        for (var i = 0; i < array2.Length; i++)
            using (var enumerator = array2[i].GetItems().GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var go = enumerator.Current;
                    if (!(go == null)) {
                        var component = go.GetComponent<PrimaryElement>();
                        if (!(component != null) || component.Mass != 0f) {
                            var smi        = go.GetSMI<Rottable.Instance>();
                            var component2 = go.GetComponent<HighEnergyParticleStorage>();
                            var text       = "";
                            var text2      = "";
                            if (component != null && component2 == null) {
                                text = GameUtil.GetUnitFormattedName(go);
                                text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS,
                                                     text,
                                                     GameUtil.GetFormattedMass(component.Mass));

                                text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE,
                                                     text,
                                                     GameUtil.GetFormattedTemperature(component.Temperature));
                            }

                            if (component2 != null) {
                                text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
                                text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS,
                                                     text,
                                                     GameUtil.GetFormattedHighEnergyParticles(component2.Particles));
                            }

                            if (smi != null) {
                                var text3 = smi.StateString();
                                if (!string.IsNullOrEmpty(text3))
                                    text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text3);

                                text2 += smi.GetToolTip();
                            }

                            if (component.DiseaseIdx != 255) {
                                text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED,
                                                      GameUtil.GetFormattedDisease(component.DiseaseIdx,
                                                       component.DiseaseCount));

                                text2 += GameUtil.GetFormattedDisease(component.DiseaseIdx,
                                                                      component.DiseaseCount,
                                                                      true);
                            }

                            targetPanel.SetLabelWithButton("storage_" + num,
                                                           text,
                                                           text2,
                                                           delegate {
                                                               SelectTool.Instance
                                                                         .Select(go.GetComponent<KSelectable>());
                                                           });

                            num++;
                        }
                    }
                }
            }

        if (num == 0) targetPanel.SetLabel("storage_empty", UI.DETAILTABS.DETAILS.STORAGE_EMPTY, "");
        targetPanel.Commit();
    }

    private void CreateWorldTraitRow() {
        var gameObject = Util.KInstantiateUI(iconLabelRow, worldTraitsPanel.Content.gameObject, true);
        worldTraitRows.Add(gameObject);
        var component = gameObject.GetComponent<HierarchyReferences>();
        component.GetReference<Image>("Icon").gameObject.SetActive(false);
        component.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
    }

    private static void RefreshMovePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var component = targetEntity.GetComponent<CancellableMove>();
        var moving    = targetEntity.GetComponent<Movable>();
        if (component != null) {
            var movingObjects = component.movingObjects;
            var num           = 0;
            using (var enumerator = movingObjects.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var @ref    = enumerator.Current;
                    var movable = @ref.Get();
                    var go      = movable != null ? movable.gameObject : null;
                    if (!(go == null)) {
                        var component2 = go.GetComponent<PrimaryElement>();
                        if (!(component2 != null) || component2.Mass != 0f) {
                            var smi        = go.GetSMI<Rottable.Instance>();
                            var component3 = go.GetComponent<HighEnergyParticleStorage>();
                            var text       = "";
                            var text2      = "";
                            if (component2 != null && component3 == null) {
                                text = GameUtil.GetUnitFormattedName(go);
                                text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS,
                                                     text,
                                                     GameUtil.GetFormattedMass(component2.Mass));

                                text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE,
                                                     text,
                                                     GameUtil.GetFormattedTemperature(component2.Temperature));
                            }

                            if (component3 != null) {
                                text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
                                text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS,
                                                     text,
                                                     GameUtil.GetFormattedHighEnergyParticles(component3.Particles));
                            }

                            if (smi != null) {
                                var text3 = smi.StateString();
                                if (!string.IsNullOrEmpty(text3))
                                    text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text3);

                                text2 += smi.GetToolTip();
                            }

                            if (component2.DiseaseIdx != 255) {
                                text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED,
                                                      GameUtil.GetFormattedDisease(component2.DiseaseIdx,
                                                       component2.DiseaseCount));

                                var formattedDisease
                                    = GameUtil.GetFormattedDisease(component2.DiseaseIdx,
                                                                   component2.DiseaseCount,
                                                                   true);

                                text2 += formattedDisease;
                            }

                            targetPanel.SetLabelWithButton("move_" + num,
                                                           text,
                                                           text2,
                                                           delegate {
                                                               SelectTool.Instance.SelectAndFocus(go.transform
                                                                    .GetPosition(),
                                                                go.GetComponent<KSelectable>(),
                                                                new Vector3(5f, 0f, 0f));
                                                           });

                            num++;
                        }
                    }
                }

                goto IL_29A;
            }
        }

        if (moving != null && moving.IsMarkedForMove)
            targetPanel.SetLabelWithButton("moveplacer",
                                           MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS,
                                           MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS_TOOLTIP,
                                           delegate {
                                               SelectTool.Instance.SelectAndFocus(moving.StorageProxy.transform
                                                    .GetPosition(),
                                                moving.StorageProxy.GetComponent<KSelectable>(),
                                                new Vector3(5f, 0f, 0f));
                                           });

        IL_29A:
        targetPanel.Commit();
    }

    private void RefreshWorldPanel() {
        var worldContainer = selectedTarget == null ? null : selectedTarget.GetComponent<WorldContainer>();
        var x              = selectedTarget == null ? null : selectedTarget.GetComponent<AsteroidGridEntity>();
        var flag = ManagementMenu.Instance.IsScreenOpen(ClusterMapScreen.Instance) &&
                   worldContainer != null                                          &&
                   x              != null;

        worldBiomesPanel.gameObject.SetActive(flag);
        worldGeysersPanel.gameObject.SetActive(flag);
        worldMeteorShowersPanel.gameObject.SetActive(flag);
        worldTraitsPanel.gameObject.SetActive(flag);
        if (!flag) return;

        foreach (var keyValuePair in biomeRows) keyValuePair.Value.SetActive(false);
        if (worldContainer.Biomes != null)
            using (var enumerator2 = worldContainer.Biomes.GetEnumerator()) {
                while (enumerator2.MoveNext()) {
                    var text        = enumerator2.Current;
                    var biomeSprite = GameUtil.GetBiomeSprite(text);
                    if (!biomeRows.ContainsKey(text)) {
                        biomeRows.Add(text,
                                      Util.KInstantiateUI(bigIconLabelRow, worldBiomesPanel.Content.gameObject, true));

                        var component = biomeRows[text].GetComponent<HierarchyReferences>();
                        component.GetReference<Image>("Icon").sprite = biomeSprite;
                        component.GetReference<LocText>("NameLabel")
                                 .SetText(UI.FormatAsLink(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".NAME"),
                                                          "BIOME" + text.ToUpper()));

                        component.GetReference<LocText>("DescriptionLabel")
                                 .SetText(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".DESC"));
                    }

                    biomeRows[text].SetActive(true);
                }

                goto IL_23C;
            }

        worldBiomesPanel.gameObject.SetActive(false);
        IL_23C:
        var list = new List<Tag>();
        foreach (var cmp in Components.Geysers.GetItems(worldContainer.id)) list.Add(cmp.PrefabID());
        list.AddRange(SaveGame.Instance.worldGenSpawner.GetUnspawnedWithType<Geyser>(worldContainer.id));
        list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("OilWell", worldContainer.id, true));
        foreach (var keyValuePair2 in geyserRows) keyValuePair2.Value.SetActive(false);
        foreach (var tag in list) {
            var uisprite = Def.GetUISprite(tag);
            if (!geyserRows.ContainsKey(tag)) {
                geyserRows.Add(tag, Util.KInstantiateUI(iconLabelRow, worldGeysersPanel.Content.gameObject, true));
                var component2 = geyserRows[tag].GetComponent<HierarchyReferences>();
                component2.GetReference<Image>("Icon").sprite = uisprite.first;
                component2.GetReference<Image>("Icon").color  = uisprite.second;
                component2.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(tag).GetProperName());
                component2.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
            }

            geyserRows[tag].SetActive(true);
        }

        var count = SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("GeyserGeneric", worldContainer.id).Count;
        if (count > 0) {
            var uisprite2 = Def.GetUISprite("GeyserGeneric");
            Tag key       = "GeyserGeneric";
            if (!geyserRows.ContainsKey(key))
                geyserRows.Add(key, Util.KInstantiateUI(iconLabelRow, worldGeysersPanel.Content.gameObject, true));

            var component3 = geyserRows[key].GetComponent<HierarchyReferences>();
            component3.GetReference<Image>("Icon").sprite = uisprite2.first;
            component3.GetReference<Image>("Icon").color  = uisprite2.second;
            component3.GetReference<LocText>("NameLabel")
                      .SetText(UI.DETAILTABS.SIMPLEINFO.UNKNOWN_GEYSERS.Replace("{num}", count.ToString()));

            component3.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
            geyserRows[key].SetActive(true);
        }

        Tag key2 = "NoGeysers";
        if (!geyserRows.ContainsKey(key2)) {
            geyserRows.Add(key2, Util.KInstantiateUI(iconLabelRow, worldGeysersPanel.Content.gameObject, true));
            var component4 = geyserRows[key2].GetComponent<HierarchyReferences>();
            component4.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
            component4.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_GEYSERS);
            component4.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
        }

        geyserRows[key2].gameObject.SetActive(list.Count == 0 && count == 0);
        foreach (var keyValuePair3 in meteorShowerRows) keyValuePair3.Value.SetActive(false);
        var flag2 = false;
        foreach (var id in worldContainer.GetSeasonIds()) {
            var gameplaySeason = Db.Get().GameplaySeasons.TryGet(id);
            if (gameplaySeason != null)
                foreach (var gameplayEvent in gameplaySeason.events)
                    if (gameplayEvent.tags.Contains(GameTags.SpaceDanger) && gameplayEvent is MeteorShowerEvent) {
                        flag2 = true;
                        var meteorShowerEvent = gameplayEvent as MeteorShowerEvent;
                        var id2               = meteorShowerEvent.Id;
                        var uisprite3         = Def.GetUISprite(meteorShowerEvent.GetClusterMapMeteorShowerID());
                        if (!meteorShowerRows.ContainsKey(id2)) {
                            meteorShowerRows.Add(id2,
                                                 Util.KInstantiateUI(iconLabelRow,
                                                                     worldMeteorShowersPanel.Content.gameObject,
                                                                     true));

                            var component5 = meteorShowerRows[id2].GetComponent<HierarchyReferences>();
                            component5.GetReference<Image>("Icon").sprite = uisprite3.first;
                            component5.GetReference<Image>("Icon").color  = uisprite3.second;
                            component5.GetReference<LocText>("NameLabel")
                                      .SetText(Assets.GetPrefab(meteorShowerEvent.GetClusterMapMeteorShowerID())
                                                     .GetProperName());

                            component5.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
                        }

                        meteorShowerRows[id2].SetActive(true);
                    }
        }

        Tag key3 = "NoMeteorShowers";
        if (!meteorShowerRows.ContainsKey(key3)) {
            meteorShowerRows.Add(key3,
                                 Util.KInstantiateUI(iconLabelRow, worldMeteorShowersPanel.Content.gameObject, true));

            var component6 = meteorShowerRows[key3].GetComponent<HierarchyReferences>();
            component6.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
            component6.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_METEORSHOWERS);
            component6.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
        }

        meteorShowerRows[key3].gameObject.SetActive(!flag2);
        var worldTraitIds = worldContainer.WorldTraitIds;
        if (worldTraitIds != null) {
            for (var i = 0; i < worldTraitIds.Count; i++) {
                if (i > worldTraitRows.Count - 1) CreateWorldTraitRow();
                var cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraitIds[i], false);
                var reference = worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
                if (cachedWorldTrait != null) {
                    var sprite
                        = Assets.GetSprite(cachedWorldTrait.filePath.Substring(cachedWorldTrait.filePath
                                                                                   .LastIndexOf("/") +
                                                                               1));

                    reference.gameObject.SetActive(true);
                    reference.sprite = sprite == null ? Assets.GetSprite("unknown") : sprite;
                    reference.color  = Util.ColorFromHex(cachedWorldTrait.colorHex);
                    worldTraitRows[i]
                        .GetComponent<HierarchyReferences>()
                        .GetReference<LocText>("NameLabel")
                        .SetText(Strings.Get(cachedWorldTrait.name));

                    worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip(Strings.Get(cachedWorldTrait.description));
                } else {
                    var sprite2 = Assets.GetSprite("NoTraits");
                    reference.gameObject.SetActive(true);
                    reference.sprite = sprite2;
                    reference.color  = Color.white;
                    worldTraitRows[i]
                        .GetComponent<HierarchyReferences>()
                        .GetReference<LocText>("NameLabel")
                        .SetText(WORLD_TRAITS.MISSING_TRAIT);

                    worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip("");
                }
            }

            for (var j = 0; j < worldTraitRows.Count; j++) worldTraitRows[j].SetActive(j < worldTraitIds.Count);
            if (worldTraitIds.Count == 0) {
                if (worldTraitRows.Count < 1) CreateWorldTraitRow();
                var reference2 = worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
                var sprite3    = Assets.GetSprite("NoTraits");
                reference2.gameObject.SetActive(true);
                reference2.sprite = sprite3;
                reference2.color  = Color.black;
                worldTraitRows[0]
                    .GetComponent<HierarchyReferences>()
                    .GetReference<LocText>("NameLabel")
                    .SetText(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND);

                worldTraitRows[0].AddOrGet<ToolTip>().SetSimpleTooltip(WORLD_TRAITS.NO_TRAITS.DESCRIPTION);
                worldTraitRows[0].SetActive(true);
            }
        }

        for (var k = surfaceConditionRows.Count - 1; k >= 0; k--) Util.KDestroyGameObject(surfaceConditionRows[k]);
        surfaceConditionRows.Clear();
        var gameObject = Util.KInstantiateUI(iconLabelRow, worldTraitsPanel.Content.gameObject, true);
        var component7 = gameObject.GetComponent<HierarchyReferences>();
        component7.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_lights");
        component7.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.LIGHT);
        component7.GetReference<LocText>("ValueLabel")
                  .SetText(GameUtil.GetFormattedLux(worldContainer.SunlightFixedTraits
                                                        [worldContainer.sunlightFixedTrait]));

        component7.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
        surfaceConditionRows.Add(gameObject);
        var gameObject2 = Util.KInstantiateUI(iconLabelRow, worldTraitsPanel.Content.gameObject, true);
        var component8  = gameObject2.GetComponent<HierarchyReferences>();
        component8.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_radiation");
        component8.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.RADIATION);
        component8.GetReference<LocText>("ValueLabel")
                  .SetText(GameUtil.GetFormattedRads(worldContainer.CosmicRadiationFixedTraits
                                                         [worldContainer.cosmicRadiationFixedTrait]));

        component8.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
        surfaceConditionRows.Add(gameObject2);
    }

    private void RefreshProcessConditionsPanel() {
        foreach (var original in processConditionRows) Util.KDestroyGameObject(original);
        processConditionRows.Clear();
        processConditionContainer.SetActive(selectedTarget.GetComponent<IProcessConditionSet>() != null);
        if (!DlcManager.FeatureClusterSpaceEnabled()) {
            if (selectedTarget.GetComponent<LaunchableRocket>() != null) {
                RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketPrep);
                RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketStorage);
                RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketBoard);
                return;
            }

            RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.All);
            return;
        }

        if (selectedTarget.GetComponent<LaunchPad>()                           != null ||
            selectedTarget.GetComponent<RocketProcessConditionDisplayTarget>() != null) {
            RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketFlight);
            RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketPrep);
            RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketStorage);
            RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.RocketBoard);
            return;
        }

        RefreshProcessConditionsForType(selectedTarget, ProcessCondition.ProcessConditionType.All);
    }

    private static void RefreshStressPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity) {
        var identity = targetEntity != null ? targetEntity.GetComponent<MinionIdentity>() : null;
        if (identity != null) {
            var stressNotes = new List<ReportManager.ReportEntry.Note>();
            targetPanel.gameObject.SetActive(true);
            targetPanel.SetTitle(UI.DETAILTABS.STATS.GROUPNAME_STRESS);
            var reportEntry
                = ReportManager.Instance.TodaysReport.reportEntries.Find(entry => entry.reportType ==
                                                                             ReportManager.ReportType.StressDelta);

            var num = 0f;
            stressNotes.Clear();
            var num2         = reportEntry.contextEntries.FindIndex(entry => entry.context == identity.GetProperName());
            var reportEntry2 = num2 != -1 ? reportEntry.contextEntries[num2] : null;
            if (reportEntry2 != null) {
                reportEntry2.IterateNotes(delegate(ReportManager.ReportEntry.Note note) { stressNotes.Add(note); });
                stressNotes.Sort((a, b) => a.value.CompareTo(b.value));
                for (var i = 0; i < stressNotes.Count; i++) {
                    var text = float.IsNegativeInfinity(stressNotes[i].value)
                                   ? UI.NEG_INFINITY.ToString()
                                   : Util.FormatTwoDecimalPlace(stressNotes[i].value);

                    targetPanel.SetLabel("stressNotes_" + i,
                                         string.Concat(stressNotes[i].value > 0f ? UIConstants.ColorPrefixRed : "",
                                                       stressNotes[i].note,
                                                       ": ",
                                                       text,
                                                       "%",
                                                       stressNotes[i].value > 0f ? UIConstants.ColorSuffix : ""),
                                         "");

                    num += stressNotes[i].value;
                }
            }

            var arg = float.IsNegativeInfinity(num) ? UI.NEG_INFINITY.ToString() : Util.FormatTwoDecimalPlace(num);
            targetPanel.SetLabel("net_stress",
                                 (num > 0f ? UIConstants.ColorPrefixRed : "")         +
                                 string.Format(UI.DETAILTABS.DETAILS.NET_STRESS, arg) +
                                 (num > 0f ? UIConstants.ColorSuffix : ""),
                                 "");
        }

        targetPanel.Commit();
    }

    private void RefreshProcessConditionsForType(GameObject                            target,
                                                 ProcessCondition.ProcessConditionType conditionType) {
        var component = target.GetComponent<IProcessConditionSet>();
        if (component == null) return;

        var conditionSet = component.GetConditionSet(conditionType);
        if (conditionSet.Count == 0) return;

        var hierarchyReferences
            = Util.KInstantiateUI<HierarchyReferences>(processConditionHeader.gameObject,
                                                       processConditionContainer.Content.gameObject,
                                                       true);

        hierarchyReferences.GetReference<LocText>("Label").text
            = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper());

        hierarchyReferences.GetComponent<ToolTip>().toolTip
            = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." +
                          conditionType.ToString().ToUpper()          +
                          "_TOOLTIP");

        processConditionRows.Add(hierarchyReferences.gameObject);
        var list = new List<ProcessCondition>();
        using (var enumerator = conditionSet.GetEnumerator()) {
            while (enumerator.MoveNext()) {
                var condition = enumerator.Current;
                if (condition.ShowInUI() &&
                    (condition.GetType()                                        == typeof(RequireAttachedComponent) ||
                     list.Find(match => match.GetType() == condition.GetType()) == null)) {
                    list.Add(condition);
                    var gameObject
                        = Util.KInstantiateUI(processConditionRow, processConditionContainer.Content.gameObject, true);

                    processConditionRows.Add(gameObject);
                    ConditionListSideScreen.SetRowState(gameObject, condition);
                }
            }
        }
    }

    [DebuggerDisplay("{item.item.Name}")]
    public class StatusItemEntry : IRenderEveryTick {
        private readonly KButton                 button;
        public           StatusItemCategory      category;
        public           Color                   color;
        private          float                   fade;
        private          float                   fadeInTime;
        private readonly float                   fadeOutTime = 1.8f;
        private          FadeStage               fadeStage;
        public           StatusItemGroup.Entry   item;
        public           Action<StatusItemEntry> onDestroy;
        private          LayoutElement           spacerLayout;
        public           TextStyleSetting        style;
        private readonly LocText                 text;
        private readonly ToolTip                 toolTip;
        private readonly TextStyleSetting        tooltipStyle;
        private readonly GameObject              widget;

        public StatusItemEntry(StatusItemGroup.Entry   item,
                               StatusItemCategory      category,
                               GameObject              status_item_prefab,
                               Transform               parent,
                               TextStyleSetting        tooltip_style,
                               Color                   color,
                               TextStyleSetting        style,
                               bool                    skip_fade,
                               Action<StatusItemEntry> onDestroy) {
            this.item      = item;
            this.category  = category;
            tooltipStyle   = tooltip_style;
            this.onDestroy = onDestroy;
            this.color     = color;
            this.style     = style;
            widget         = Util.KInstantiateUI(status_item_prefab, parent.gameObject);
            text           = widget.GetComponentInChildren<LocText>(true);
            SetTextStyleSetting.ApplyStyle(text, style);
            toolTip  = widget.GetComponentInChildren<ToolTip>(true);
            GetImage = widget.GetComponentInChildren<Image>(true);
            item.SetIcon(GetImage);
            widget.SetActive(true);
            toolTip.OnToolTip = OnToolTip;
            button            = widget.GetComponentInChildren<KButton>();
            if (item.item.statusItemClickCallback != null)
                button.onClick += OnClick;
            else
                button.enabled = false;

            fadeStage = skip_fade ? FadeStage.WAIT : FadeStage.IN;
            SimAndRenderScheduler.instance.Add(this);
            Refresh();
            SetColor();
        }

        public Image GetImage { get; }

        public void RenderEveryTick(float dt) {
            switch (fadeStage) {
                case FadeStage.IN: {
                    fade = Mathf.Min(fade + Time.deltaTime / fadeInTime, 1f);
                    var num = fade;
                    SetColor(num);
                    if (fade >= 1f) fadeStage = FadeStage.WAIT;
                    break;
                }
                case FadeStage.WAIT:
                    break;
                case FadeStage.OUT: {
                    var num2 = fade;
                    SetColor(num2);
                    fade = Mathf.Max(fade - Time.deltaTime / fadeOutTime, 0f);
                    if (fade <= 0f) Destroy(true);
                    break;
                }
                default:
                    return;
            }
        }

        internal void SetSprite(TintedSprite sprite) {
            if (sprite != null) GetImage.sprite = sprite.sprite;
        }

        public int  GetIndex()          { return widget.transform.GetSiblingIndex(); }
        public void SetIndex(int index) { widget.transform.SetSiblingIndex(index); }

        private string OnToolTip() {
            item.ShowToolTip(toolTip, tooltipStyle);
            return "";
        }

        private void OnClick() { item.OnClick(); }

        public void Refresh() {
            var name = item.GetName();
            if (name != text.text) {
                text.text = name;
                SetColor();
            }
        }

        private void SetColor(float alpha = 1f) {
            var color = new Color(this.color.r, this.color.g, this.color.b, alpha);
            GetImage.color = color;
            text.color     = color;
        }

        public void Destroy(bool immediate) {
            if (toolTip != null) toolTip.OnToolTip               =  null;
            if (button != null && button.enabled) button.onClick -= OnClick;
            if (immediate) {
                if (onDestroy != null) onDestroy(this);
                SimAndRenderScheduler.instance.Remove(this);
                Object.Destroy(widget);
                return;
            }

            fade      = 0.5f;
            fadeStage = FadeStage.OUT;
        }

        private enum FadeStage {
            IN,
            WAIT,
            OUT
        }
    }
}