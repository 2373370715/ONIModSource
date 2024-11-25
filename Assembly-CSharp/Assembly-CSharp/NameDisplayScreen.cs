using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class NameDisplayScreen : KScreen {
    public static NameDisplayScreen Instance;

    [SerializeField]
    private Canvas areaTextDisplayCanvas;

    public  GameObject barsPrefab;
    public  float      cameraDistance_fontsize_max = 4f;
    public  float      cameraDistance_fontsize_min = 6f;
    private int        currentUpdateIndex;

    [SerializeField]
    private Color defaultColor;

    public List<Entry> entries      = new List<Entry>();
    public int         fontsize_max = 32;
    public int         fontsize_min = 14;

    [SerializeField]
    private float HideDistance;

    private bool         isOverlayChangeBound;
    private HashedString lastKnownOverlayID = OverlayModes.None.ID;
    public  GameObject   nameAndBarsPrefab;

    [SerializeField]
    private Canvas nameDisplayCanvas;

    [SerializeField]
    private Color selectedColor;

    public        List<TextEntry>  textEntries = new List<TextEntry>();
    public        TextStyleSetting ToolTipStyle_Property;
    public        bool             worldSpace = true;
    public static void             DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Instance = this;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        Components.Health.Register(OnHealthAdded, null);
        Components.Equipment.Register(OnEquipmentAdded, null);
        BindOnOverlayChange();
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        if (isOverlayChangeBound && OverlayScreen.Instance != null) {
            var instance = OverlayScreen.Instance;
            instance.OnOverlayChanged
                = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged,
                                                        new Action<HashedString>(OnOverlayChanged));

            isOverlayChangeBound = false;
        }
    }

    private void BindOnOverlayChange() {
        if (isOverlayChangeBound) return;

        if (OverlayScreen.Instance != null) {
            var instance = OverlayScreen.Instance;
            instance.OnOverlayChanged
                = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged,
                                                         new Action<HashedString>(OnOverlayChanged));

            isOverlayChangeBound = true;
        }
    }

    public void RemoveWorldEntries(int worldId) {
        entries.RemoveAll(entry => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId);
    }

    private void OnOverlayChanged(HashedString new_mode) {
        var hashedString = lastKnownOverlayID;
        lastKnownOverlayID        = new_mode;
        nameDisplayCanvas.enabled = lastKnownOverlayID == OverlayModes.None.ID;
    }

    private void OnHealthAdded(Health health) { RegisterComponent(health.gameObject, health); }

    private void OnEquipmentAdded(Equipment equipment) {
        var component        = equipment.GetComponent<MinionAssignablesProxy>();
        var targetGameObject = component.GetTargetGameObject();
        if (targetGameObject) {
            RegisterComponent(targetGameObject, equipment);
            return;
        }

        Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", component.TargetInstanceID);
    }

    private bool ShouldShowName(GameObject representedObject) {
        var component = representedObject.GetComponent<CharacterOverlay>();
        return component != null && component.shouldShowName;
    }

    public Guid AddAreaText(string initialText, GameObject prefab) {
        var textEntry = new TextEntry();
        textEntry.guid = Guid.NewGuid();
        textEntry.display_go = Util.KInstantiateUI(prefab, areaTextDisplayCanvas.gameObject, true);
        textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
        textEntries.Add(textEntry);
        return textEntry.guid;
    }

    public GameObject GetWorldText(Guid guid) {
        GameObject result = null;
        foreach (var textEntry in textEntries)
            if (textEntry.guid == guid) {
                result = textEntry.display_go;
                break;
            }

        return result;
    }

    public void RemoveWorldText(Guid guid) {
        var num = -1;
        for (var i = 0; i < textEntries.Count; i++)
            if (textEntries[i].guid == guid) {
                num = i;
                break;
            }

        if (num >= 0) {
            Destroy(textEntries[num].display_go);
            textEntries.RemoveAt(num);
        }
    }

    public void AddNewEntry(GameObject representedObject) {
        var entry = new Entry();
        entry.world_go                 = representedObject;
        entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
        var original = ShouldShowName(representedObject) ? nameAndBarsPrefab : barsPrefab;
        entry.kprfabID = representedObject.GetComponent<KPrefabID>();
        entry.collider = representedObject.GetComponent<KBoxCollider2D>();
        var gameObject = Util.KInstantiateUI(original, nameDisplayCanvas.gameObject, true);
        entry.display_go      = gameObject;
        entry.display_go_rect = gameObject.GetComponent<RectTransform>();
        entry.nameLabel       = entry.display_go.GetComponentInChildren<LocText>();
        entry.display_go.SetActive(false);
        if (worldSpace) entry.display_go.transform.localScale = Vector3.one * 0.01f;
        gameObject.name = representedObject.name + " character overlay";
        entry.Name      = representedObject.name;
        entry.refs      = gameObject.GetComponent<HierarchyReferences>();
        entries.Add(entry);
        Object component  = representedObject.GetComponent<KSelectable>();
        var    component2 = representedObject.GetComponent<FactionAlignment>();
        if (component != null) {
            if (component2 != null) {
                if (component2.Alignment == FactionManager.FactionID.Friendly ||
                    component2.Alignment == FactionManager.FactionID.Duplicant)
                    UpdateName(representedObject);
            } else
                UpdateName(representedObject);
        }
    }

    public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false) {
        var entry = force_new_entry ? null : GetEntry(representedObject);
        if (entry == null) {
            var component2 = representedObject.GetComponent<CharacterOverlay>();
            if (component2 != null) {
                component2.Register();
                entry = GetEntry(representedObject);
            }
        }

        if (entry == null) return;

        var reference = entry.refs.GetReference<Transform>("Bars");
        entry.bars_go = reference.gameObject;
        if (component is Health) {
            if (!entry.healthBar) {
                var health     = (Health)component;
                var gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject);
                gameObject.name                                         = "Health Bar";
                health.healthBar                                        = gameObject.GetComponent<HealthBar>();
                health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
                health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable
                    = representedObject.GetComponent<MinionBrain>() != null;

                entry.healthBar          = health.healthBar;
                entry.healthBar.autoHide = false;
                gameObject.transform.Find("Bar").GetComponent<Image>().color
                    = ProgressBarsConfig.Instance.GetBarColor("HealthBar");

                return;
            }

            Debug.LogWarningFormat("Health added twice {0}", component);
            return;
        }

        if (component is OxygenBreather) {
            if (!entry.breathBar) {
                var gameObject2
                    = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);

                entry.breathBar          = gameObject2.GetComponent<ProgressBar>();
                entry.breathBar.autoHide = false;
                gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", ToolTipStyle_Property);
                gameObject2.name = "Breath Bar";
                gameObject2.transform.Find("Bar").GetComponent<Image>().color
                    = ProgressBarsConfig.Instance.GetBarColor("BreathBar");

                gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
                return;
            }

            Debug.LogWarningFormat("OxygenBreather added twice {0}", component);
            return;
        }

        if (component is Equipment) {
            if (!entry.suitBar) {
                var gameObject3
                    = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);

                entry.suitBar          = gameObject3.GetComponent<ProgressBar>();
                entry.suitBar.autoHide = false;
                gameObject3.name       = "Suit Tank Bar";
                gameObject3.transform.Find("Bar").GetComponent<Image>().color
                    = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");

                gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
            } else
                Debug.LogWarningFormat("SuitBar added twice {0}", component);

            if (!entry.suitFuelBar) {
                var gameObject4
                    = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);

                entry.suitFuelBar          = gameObject4.GetComponent<ProgressBar>();
                entry.suitFuelBar.autoHide = false;
                gameObject4.name           = "Suit Fuel Bar";
                gameObject4.transform.Find("Bar").GetComponent<Image>().color
                    = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");

                gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
            } else
                Debug.LogWarningFormat("FuelBar added twice {0}", component);

            if (!entry.suitBatteryBar) {
                var gameObject5
                    = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject);

                entry.suitBatteryBar          = gameObject5.GetComponent<ProgressBar>();
                entry.suitBatteryBar.autoHide = false;
                gameObject5.name              = "Suit Battery Bar";
                gameObject5.transform.Find("Bar").GetComponent<Image>().color
                    = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");

                gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
                return;
            }

            Debug.LogWarningFormat("CoolantBar added twice {0}", component);
            return;
        }

        if (component is ThoughtGraph.Instance || component is CreatureThoughtGraph.Instance) {
            if (!entry.thoughtBubble) {
                var gameObject6 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go);
                entry.thoughtBubble = gameObject6.GetComponent<HierarchyReferences>();
                gameObject6.name = (component is CreatureThoughtGraph.Instance ? "Creature " : "") + "Thought Bubble";
                var gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go);
                entry.thoughtBubbleConvo = gameObject7.GetComponent<HierarchyReferences>();
                gameObject7.name = (component is CreatureThoughtGraph.Instance ? "Creature " : "") +
                                   "Thought Bubble Convo";

                return;
            }

            Debug.LogWarningFormat("ThoughtGraph added twice {0}", component);
            return;
        }

        if (!(component is GameplayEventMonitor.Instance)) {
            if (component is Dreamer.Instance && !entry.dreamBubble) {
                var gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go);
                gameObject8.name  = "Dream Bubble";
                entry.dreamBubble = gameObject8.GetComponent<DreamBubble>();
            }

            return;
        }

        if (!entry.gameplayEventDisplay) {
            var gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go);
            entry.gameplayEventDisplay = gameObject9.GetComponent<HierarchyReferences>();
            gameObject9.name           = "Gameplay Event Display";
            return;
        }

        Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", component);
    }

    public bool IsVisibleToZoom() {
        return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < HideDistance;
    }

    private void LateUpdate() {
        if (App.isLoading || App.IsExiting) return;

        BindOnOverlayChange();
        if (Game.MainCamera == null) return;

        if (lastKnownOverlayID != OverlayModes.None.ID) return;

        var count = entries.Count;
        var flag = IsVisibleToZoom();
        var flag2 = flag && lastKnownOverlayID == OverlayModes.None.ID;
        if (nameDisplayCanvas.enabled != flag2) nameDisplayCanvas.enabled = flag2;
        if (flag) {
            RemoveDestroyedEntries();
            Culling();
            UpdatePos();
            HideDeadProgressBars();
        }
    }

    private void Culling() {
        if (entries.Count == 0) return;

        Vector2I vector2I;
        Vector2I vector2I2;
        Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2);
        var num = Mathf.Min(500, entries.Count);
        for (var i = 0; i < num; i++) {
            var index    = (currentUpdateIndex + i) % entries.Count;
            var entry    = entries[index];
            var position = entry.world_go.transform.GetPosition();
            var flag = position.x >= vector2I.x &&
                       position.y >= vector2I.y &&
                       position.x < vector2I2.x &&
                       position.y < vector2I2.y;

            if (entry.visible != flag) entry.display_go.SetActive(flag);
            entry.visible = flag;
        }

        currentUpdateIndex = (currentUpdateIndex + num) % entries.Count;
    }

    private void UpdatePos() {
        var instance     = CameraController.Instance;
        var followTarget = instance.followTarget;
        var count        = entries.Count;
        for (var i = 0; i < count; i++) {
            var entry = entries[i];
            if (entry.visible) {
                var world_go = entry.world_go;
                if (!(world_go == null)) {
                    var vector = world_go.transform.GetPosition();
                    if (followTarget == world_go.transform)
                        vector = instance.followTargetPos;
                    else if (entry.world_go_anim_controller != null && entry.collider != null) {
                        vector.x += entry.collider.offset.x;
                        vector.y += entry.collider.offset.y - entry.collider.size.y / 2f;
                    }

                    entry.display_go_rect.anchoredPosition = worldSpace ? vector : WorldToScreen(vector);
                }
            }
        }
    }

    private void RemoveDestroyedEntries() {
        var num = entries.Count;
        var i   = 0;
        while (i < num)
            if (entries[i].world_go == null) {
                Destroy(entries[i].display_go);
                num--;
                entries[i] = entries[num];
            } else
                i++;

        entries.RemoveRange(num, entries.Count - num);
    }

    private void HideDeadProgressBars() {
        var count = entries.Count;
        for (var i = 0; i < count; i++)
            if (entries[i].visible                        &&
                !(entries[i].world_go == null)            &&
                entries[i].kprfabID.HasTag(GameTags.Dead) &&
                entries[i].bars_go.activeSelf)
                entries[i].bars_go.SetActive(false);
    }

    public void UpdateName(GameObject representedObject) {
        var entry = GetEntry(representedObject);
        if (entry == null) return;

        var component = representedObject.GetComponent<KSelectable>();
        entry.display_go.name = component.GetProperName() + " character overlay";
        if (entry.nameLabel != null) {
            entry.nameLabel.text = component.GetProperName();
            if (representedObject.GetComponent<RocketModule>() != null)
                entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
        }
    }

    public void SetDream(GameObject minion_go, Dream dream) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.dreamBubble == null) return;

        entry.dreamBubble.SetDream(dream);
        entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
        entry.dreamBubble.gameObject.SetActive(true);
        entry.dreamBubble.SetVisibility(true);
    }

    public void StopDreaming(GameObject minion_go) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.dreamBubble == null) return;

        entry.dreamBubble.StopDreaming();
        entry.dreamBubble.gameObject.SetActive(false);
    }

    public void DreamTick(GameObject minion_go, float dt) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.dreamBubble == null) return;

        entry.dreamBubble.Tick(dt);
    }

    public void SetThoughtBubbleDisplay(GameObject minion_go,
                                        bool       bVisible,
                                        string     hover_text,
                                        Sprite     bubble_sprite,
                                        Sprite     topic_sprite) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.thoughtBubble == null) return;

        ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
        ApplyThoughtSprite(entry.thoughtBubble, topic_sprite,  "icon_sprite");
        entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
        entry.thoughtBubble.gameObject.SetActive(bVisible);
    }

    public void SetThoughtBubbleConvoDisplay(GameObject minion_go,
                                             bool       bVisible,
                                             string     hover_text,
                                             Sprite     bubble_sprite,
                                             Sprite     topic_sprite,
                                             Sprite     mode_sprite) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.thoughtBubble == null) return;

        ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
        ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite,  "icon_sprite");
        ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite,   "icon_sprite_mode");
        entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
        entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
    }

    private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target) {
        active_bubble.GetReference<Image>(target).sprite = sprite;
    }

    public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.gameplayEventDisplay == null) return;

        entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
        entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName    = hover_text;
        entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
    }

    public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.breathBar == null) return;

        entry.breathBar.SetUpdateFunc(updatePercentFull);
        entry.breathBar.SetVisibility(bVisible);
    }

    public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.healthBar == null) return;

        entry.healthBar.OnChange();
        entry.healthBar.SetUpdateFunc(updatePercentFull);
        if (entry.healthBar.gameObject.activeSelf != bVisible) entry.healthBar.SetVisibility(bVisible);
    }

    public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.suitBar == null) return;

        entry.suitBar.SetUpdateFunc(updatePercentFull);
        entry.suitBar.SetVisibility(bVisible);
    }

    public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.suitFuelBar == null) return;

        entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
        entry.suitFuelBar.SetVisibility(bVisible);
    }

    public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible) {
        var entry = GetEntry(minion_go);
        if (entry == null || entry.suitBatteryBar == null) return;

        entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
        entry.suitBatteryBar.SetVisibility(bVisible);
    }

    private Entry GetEntry(GameObject worldObject) { return entries.Find(entry => entry.world_go == worldObject); }

    [Serializable]
    public class Entry {
        public GameObject          bars_go;
        public ProgressBar         breathBar;
        public KBoxCollider2D      collider;
        public GameObject          display_go;
        public RectTransform       display_go_rect;
        public DreamBubble         dreamBubble;
        public HierarchyReferences gameplayEventDisplay;
        public HealthBar           healthBar;
        public KPrefabID           kprfabID;
        public string              Name;
        public LocText             nameLabel;
        public HierarchyReferences refs;
        public ProgressBar         suitBar;
        public ProgressBar         suitBatteryBar;
        public ProgressBar         suitFuelBar;
        public HierarchyReferences thoughtBubble;
        public HierarchyReferences thoughtBubbleConvo;
        public bool                visible;
        public GameObject          world_go;
        public KAnimControllerBase world_go_anim_controller;
    }

    public class TextEntry {
        public GameObject display_go;
        public Guid       guid;
    }
}