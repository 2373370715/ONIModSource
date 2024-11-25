using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

public class CrewRationsScreen : CrewListScreen<CrewRationsEntry> {
    [SerializeField]
    private KButton closebutton;

    protected override void OnSpawn() {
        base.OnSpawn();
        closebutton.onClick += delegate { ManagementMenu.Instance.CloseAll(); };
    }

    protected override void OnCmpEnable() {
        base.OnCmpEnable();
        RefreshCrewPortraitContent();
        SortByPreviousSelected();
    }

    private void SortByPreviousSelected() {
        if (sortToggleGroup == null) return;

        if (lastSortToggle == null) return;

        for (var i = 0; i < ColumnTitlesContainer.childCount; i++) {
            var component = ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
            if (ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>() == lastSortToggle) {
                if (component.columnID == "name") SortByName(lastSortReversed);
                if (component.columnID == "health") SortByAmount("HitPoints",  lastSortReversed);
                if (component.columnID == "stress") SortByAmount("Stress",     lastSortReversed);
                if (component.columnID == "calories") SortByAmount("Calories", lastSortReversed);
            }
        }
    }

    protected override void PositionColumnTitles() {
        base.PositionColumnTitles();
        for (var i = 0; i < ColumnTitlesContainer.childCount; i++) {
            var component = ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
            if (component.Sortable) {
                var toggle = ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
                toggle.group = sortToggleGroup;
                var toggleImage = toggle.GetComponentInChildren<ImageToggleState>(true);
                if (component.columnID == "name")
                    toggle.onValueChanged.AddListener(delegate {
                                                          SortByName(!toggle.isOn);
                                                          lastSortToggle   = toggle;
                                                          lastSortReversed = !toggle.isOn;
                                                          ResetSortToggles(toggle);
                                                          if (toggle.isOn) {
                                                              toggleImage.SetActive();
                                                              return;
                                                          }

                                                          toggleImage.SetInactive();
                                                      });

                if (component.columnID == "health")
                    toggle.onValueChanged.AddListener(delegate {
                                                          SortByAmount("HitPoints", !toggle.isOn);
                                                          lastSortToggle   = toggle;
                                                          lastSortReversed = !toggle.isOn;
                                                          ResetSortToggles(toggle);
                                                          if (toggle.isOn) {
                                                              toggleImage.SetActive();
                                                              return;
                                                          }

                                                          toggleImage.SetInactive();
                                                      });

                if (component.columnID == "stress")
                    toggle.onValueChanged.AddListener(delegate {
                                                          SortByAmount("Stress", !toggle.isOn);
                                                          lastSortToggle   = toggle;
                                                          lastSortReversed = !toggle.isOn;
                                                          ResetSortToggles(toggle);
                                                          if (toggle.isOn) {
                                                              toggleImage.SetActive();
                                                              return;
                                                          }

                                                          toggleImage.SetInactive();
                                                      });

                if (component.columnID == "calories")
                    toggle.onValueChanged.AddListener(delegate {
                                                          SortByAmount("Calories", !toggle.isOn);
                                                          lastSortToggle   = toggle;
                                                          lastSortReversed = !toggle.isOn;
                                                          ResetSortToggles(toggle);
                                                          if (toggle.isOn) {
                                                              toggleImage.SetActive();
                                                              return;
                                                          }

                                                          toggleImage.SetInactive();
                                                      });
            }
        }
    }

    protected override void SpawnEntries() {
        base.SpawnEntries();
        foreach (var identity in Components.LiveMinionIdentities.Items) {
            var component = Util.KInstantiateUI(Prefab_CrewEntry, EntriesPanelTransform.gameObject)
                                .GetComponent<CrewRationsEntry>();

            component.Populate(identity);
            EntryObjects.Add(component);
        }

        SortByPreviousSelected();
    }

    public override void ScreenUpdate(bool topLevel) {
        base.ScreenUpdate(topLevel);
        foreach (var crewRationsEntry in EntryObjects) crewRationsEntry.Refresh();
    }

    private void SortByAmount(string amount_id, bool reverse) {
        var list = new List<CrewRationsEntry>(EntryObjects);
        list.Sort(delegate(CrewRationsEntry a, CrewRationsEntry b) {
                      var value  = a.Identity.GetAmounts().GetValue(amount_id);
                      var value2 = b.Identity.GetAmounts().GetValue(amount_id);
                      return value.CompareTo(value2);
                  });

        ReorderEntries(list, reverse);
    }

    private void ResetSortToggles(Toggle exceptToggle) {
        for (var i = 0; i < ColumnTitlesContainer.childCount; i++) {
            var component           = ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
            var componentInChildren = component.GetComponentInChildren<ImageToggleState>(true);
            if (component != exceptToggle) componentInChildren.SetDisabled();
        }
    }
}