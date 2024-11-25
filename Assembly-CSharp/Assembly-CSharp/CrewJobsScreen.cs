using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class CrewJobsScreen : CrewListScreen<CrewJobsEntry> {
    public enum everyoneToggleState {
        off,
        mixed,
        on
    }

    public static    CrewJobsScreen                            Instance;
    private readonly List<ChoreGroup>                          choreGroups = new List<ChoreGroup>();
    private          bool                                      dirty;
    private          KeyValuePair<Button, everyoneToggleState> EveryoneAllTaskToggle;

    private readonly Dictionary<Button, everyoneToggleState> EveryoneToggles
        = new Dictionary<Button, everyoneToggleState>();

    private float            screenWidth;
    public  Toggle           SortEveryoneToggle;
    public  TextStyleSetting TextStyle_JobTooltip_Description;
    public  TextStyleSetting TextStyle_JobTooltip_RelevantAttributes;
    public  TextStyleSetting TextStyle_JobTooltip_Title;

    protected override void OnActivate() {
        Instance = this;
        foreach (var item in Db.Get().ChoreGroups.resources) choreGroups.Add(item);
        base.OnActivate();
    }

    protected override void OnCmpEnable() {
        base.OnCmpEnable();
        RefreshCrewPortraitContent();
        SortByPreviousSelected();
    }

    protected override void OnForcedCleanUp() {
        Instance = null;
        base.OnForcedCleanUp();
    }

    protected override void SpawnEntries() {
        base.SpawnEntries();
        foreach (var identity in Components.LiveMinionIdentities.Items) {
            var component = Util.KInstantiateUI(Prefab_CrewEntry, EntriesPanelTransform.gameObject)
                                .GetComponent<CrewJobsEntry>();

            component.Populate(identity);
            EntryObjects.Add(component);
        }

        SortEveryoneToggle.group = sortToggleGroup;
        var toggleImage = SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
        SortEveryoneToggle.onValueChanged.AddListener(delegate {
                                                          SortByName(!SortEveryoneToggle.isOn);
                                                          lastSortToggle   = SortEveryoneToggle;
                                                          lastSortReversed = !SortEveryoneToggle.isOn;
                                                          ResetSortToggles(SortEveryoneToggle);
                                                          if (SortEveryoneToggle.isOn) {
                                                              toggleImage.SetActive();
                                                              return;
                                                          }

                                                          toggleImage.SetInactive();
                                                      });

        SortByPreviousSelected();
        dirty = true;
    }

    private void SortByPreviousSelected() {
        if (sortToggleGroup == null || lastSortToggle == null) return;

        var childCount = ColumnTitlesContainer.childCount;
        for (var i = 0; i < childCount; i++)
            if (i                                                                                < choreGroups.Count &&
                ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>() == lastSortToggle) {
                SortByEffectiveness(choreGroups[i], lastSortReversed, false);
                return;
            }

        if (SortEveryoneToggle == lastSortToggle) SortByName(lastSortReversed);
    }

    protected override void PositionColumnTitles() {
        base.PositionColumnTitles();
        var childCount = ColumnTitlesContainer.childCount;
        for (var i = 0; i < childCount; i++) {
            if (i < choreGroups.Count) {
                var sortToggle = ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>();
                ColumnTitlesContainer.GetChild(i).rectTransform().localScale = Vector3.one;
                var chore_group = choreGroups[i];
                var toggleImage = sortToggle.GetComponentInChildren<ImageToggleState>(true);
                sortToggle.group = sortToggleGroup;
                sortToggle.onValueChanged.AddListener(delegate {
                                                          var playSound                               = false;
                                                          if (lastSortToggle == sortToggle) playSound = true;
                                                          SortByEffectiveness(chore_group, !sortToggle.isOn, playSound);
                                                          lastSortToggle   = sortToggle;
                                                          lastSortReversed = !sortToggle.isOn;
                                                          ResetSortToggles(sortToggle);
                                                          if (sortToggle.isOn) {
                                                              toggleImage.SetActive();
                                                              return;
                                                          }

                                                          toggleImage.SetInactive();
                                                      });
            }

            var JobTooltip = ColumnTitlesContainer.GetChild(i).GetComponent<ToolTip>();
            var jobTooltip = JobTooltip;
            jobTooltip.OnToolTip
                = (Func<string>)Delegate.Combine(jobTooltip.OnToolTip,
                                                 new Func<string>(() => GetJobTooltip(JobTooltip.gameObject)));

            var componentInChildren = ColumnTitlesContainer.GetChild(i).GetComponentInChildren<Button>();
            EveryoneToggles.Add(componentInChildren, everyoneToggleState.on);
        }

        for (var j = 0; j < choreGroups.Count; j++) {
            var chore_group = choreGroups[j];
            var b           = EveryoneToggles.Keys.ElementAt(j);
            EveryoneToggles.Keys.ElementAt(j).onClick.AddListener(delegate { ToggleJobEveryone(b, chore_group); });
        }

        var key = EveryoneToggles.ElementAt(EveryoneToggles.Count - 1).Key;
        key.transform.parent.Find("Title").gameObject.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
        key.onClick.AddListener(delegate { ToggleAllTasksEveryone(); });
        EveryoneAllTaskToggle = new KeyValuePair<Button, everyoneToggleState>(key, EveryoneAllTaskToggle.Value);
    }

    private string GetJobTooltip(GameObject go) {
        var component = go.GetComponent<ToolTip>();
        component.ClearMultiStringTooltip();
        var component2 = go.GetComponent<OverviewColumnIdentity>();
        if (component2.columnID != "AllTasks") {
            var choreGroup = Db.Get().ChoreGroups.Get(component2.columnID);
            component.AddMultiStringTooltip(component2.Column_DisplayName,     TextStyle_JobTooltip_Title);
            component.AddMultiStringTooltip(choreGroup.description,            TextStyle_JobTooltip_Description);
            component.AddMultiStringTooltip("\n",                              TextStyle_JobTooltip_Description);
            component.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_ATTRIBUTES, TextStyle_JobTooltip_Description);
            component.AddMultiStringTooltip("•  " + choreGroup.attribute.Name, TextStyle_JobTooltip_RelevantAttributes);
        }

        return "";
    }

    private void ToggleAllTasksEveryone() {
        var name                                                        = "HUD_Click_Deselect";
        if (EveryoneAllTaskToggle.Value != everyoneToggleState.on) name = "HUD_Click";
        PlaySound(GlobalAssets.GetSound(name));
        for (var i = 0; i < choreGroups.Count; i++)
            SetJobEveryone(EveryoneAllTaskToggle.Value != everyoneToggleState.on, choreGroups[i]);
    }

    private void SetJobEveryone(Button button, ChoreGroup chore_group) {
        SetJobEveryone(EveryoneToggles[button] != everyoneToggleState.on, chore_group);
    }

    private void SetJobEveryone(bool state, ChoreGroup chore_group) {
        foreach (var crewJobsEntry in EntryObjects) crewJobsEntry.consumer.SetPermittedByUser(chore_group, state);
    }

    private void ToggleJobEveryone(Button button, ChoreGroup chore_group) {
        var name                                                    = "HUD_Click_Deselect";
        if (EveryoneToggles[button] != everyoneToggleState.on) name = "HUD_Click";
        PlaySound(GlobalAssets.GetSound(name));
        foreach (var crewJobsEntry in EntryObjects)
            crewJobsEntry.consumer.SetPermittedByUser(chore_group, EveryoneToggles[button] != everyoneToggleState.on);
    }

    private void SortByEffectiveness(ChoreGroup chore_group, bool reverse, bool playSound) {
        if (playSound) PlaySound(GlobalAssets.GetSound("HUD_Click"));
        var list = new List<CrewJobsEntry>(EntryObjects);
        list.Sort(delegate(CrewJobsEntry a, CrewJobsEntry b) {
                      var value  = a.Identity.GetAttributes().GetValue(chore_group.attribute.Id);
                      var value2 = b.Identity.GetAttributes().GetValue(chore_group.attribute.Id);
                      return value.CompareTo(value2);
                  });

        ReorderEntries(list, reverse);
    }

    private void ResetSortToggles(Toggle exceptToggle) {
        for (var i = 0; i < ColumnTitlesContainer.childCount; i++) {
            var componentInChildren = ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>();
            if (!(componentInChildren == null)) {
                var componentInChildren2 = componentInChildren.GetComponentInChildren<ImageToggleState>(true);
                if (componentInChildren != exceptToggle) componentInChildren2.SetDisabled();
            }
        }

        var componentInChildren3 = SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
        if (SortEveryoneToggle != exceptToggle) componentInChildren3.SetDisabled();
    }

    private void Refresh() {
        if (dirty) {
            var childCount = ColumnTitlesContainer.childCount;
            var flag       = false;
            var flag2      = false;
            for (var i = 0; i < childCount; i++) {
                var flag3 = false;
                var flag4 = false;
                if (choreGroups.Count - 1 >= i) {
                    var chore_group = choreGroups[i];
                    for (var j = 0; j < EntryObjects.Count; j++) {
                        var consumer = EntryObjects[j].GetComponent<CrewJobsEntry>().consumer;
                        if (consumer.IsPermittedByTraits(chore_group)) {
                            if (consumer.IsPermittedByUser(chore_group)) {
                                flag3 = true;
                                flag  = true;
                            } else {
                                flag4 = true;
                                flag2 = true;
                            }
                        }
                    }

                    if (flag3 && flag4)
                        EveryoneToggles[EveryoneToggles.ElementAt(i).Key] = everyoneToggleState.mixed;
                    else if (flag3)
                        EveryoneToggles[EveryoneToggles.ElementAt(i).Key] = everyoneToggleState.on;
                    else
                        EveryoneToggles[EveryoneToggles.ElementAt(i).Key] = everyoneToggleState.off;

                    var componentInChildren = ColumnTitlesContainer.GetChild(i).GetComponentInChildren<Button>();
                    var component = componentInChildren.GetComponentsInChildren<Image>(true)[1]
                                                       .GetComponent<ImageToggleState>();

                    switch (EveryoneToggles[componentInChildren]) {
                        case everyoneToggleState.off:
                            component.SetDisabled();
                            break;
                        case everyoneToggleState.mixed:
                            component.SetInactive();
                            break;
                        case everyoneToggleState.on:
                            component.SetActive();
                            break;
                    }
                }
            }

            if (flag && flag2)
                EveryoneAllTaskToggle
                    = new KeyValuePair<Button, everyoneToggleState>(EveryoneAllTaskToggle.Key,
                                                                    everyoneToggleState.mixed);
            else if (flag)
                EveryoneAllTaskToggle
                    = new KeyValuePair<Button, everyoneToggleState>(EveryoneAllTaskToggle.Key, everyoneToggleState.on);
            else if (flag2)
                EveryoneAllTaskToggle
                    = new KeyValuePair<Button, everyoneToggleState>(EveryoneAllTaskToggle.Key, everyoneToggleState.off);

            var component2 = EveryoneAllTaskToggle.Key.GetComponentsInChildren<Image>(true)[1]
                                                  .GetComponent<ImageToggleState>();

            switch (EveryoneAllTaskToggle.Value) {
                case everyoneToggleState.off:
                    component2.SetDisabled();
                    break;
                case everyoneToggleState.mixed:
                    component2.SetInactive();
                    break;
                case everyoneToggleState.on:
                    component2.SetActive();
                    break;
            }

            screenWidth = EntriesPanelTransform.rectTransform().sizeDelta.x;
            ScrollRectTransform.GetComponent<LayoutElement>().minWidth = screenWidth;
            var num = 31f;
            GetComponent<LayoutElement>().minWidth = screenWidth + num;
            dirty                                  = false;
        }
    }

    private void Update()                  { Refresh(); }
    public  void Dirty(object data = null) { dirty = true; }
}