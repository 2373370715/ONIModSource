using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class CrewListScreen<EntryType> : KScreen where EntryType : CrewListEntry {
    public    bool            autoColumn;
    public    float           columnTitleHorizontalOffset;
    public    Transform       ColumnTitlesContainer;
    public    Transform       EntriesPanelTransform;
    public    List<EntryType> EntryObjects  = new List<EntryType>();
    protected Vector2         EntryRectSize = new Vector2(750f, 64f);
    protected bool            lastSortReversed;
    protected Toggle          lastSortToggle;
    public    int             maxEntriesBeforeScroll = 5;
    public    Scrollbar       PanelScrollbar;
    public    GameObject      Prefab_ColumnTitle;
    public    GameObject      Prefab_CrewEntry;
    public    Transform       ScrollRectTransform;
    protected ToggleGroup     sortToggleGroup;

    protected override void OnActivate() {
        base.OnActivate();
        ClearEntries();
        SpawnEntries();
        PositionColumnTitles();
        if (autoColumn) UpdateColumnTitles();
        ConsumeMouseScroll = true;
    }

    protected override void OnCmpEnable() {
        if (autoColumn) UpdateColumnTitles();
        Reconstruct();
    }

    private void ClearEntries() {
        for (var i = EntryObjects.Count - 1; i > -1; i--) Util.KDestroyGameObject(EntryObjects[i]);
        EntryObjects.Clear();
    }

    protected void RefreshCrewPortraitContent() {
        EntryObjects.ForEach(delegate(EntryType eo) { eo.RefreshCrewPortraitContent(); });
    }

    protected virtual void SpawnEntries() {
        if (EntryObjects.Count != 0) ClearEntries();
    }

    public override void ScreenUpdate(bool topLevel) {
        base.ScreenUpdate(topLevel);
        if (autoColumn) UpdateColumnTitles();
        var flag           = false;
        var liveIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
        if (EntryObjects.Count                                                   != liveIdentities.Count ||
            EntryObjects.FindAll(o => liveIdentities.Contains(o.Identity)).Count != EntryObjects.Count)
            flag = true;

        if (flag) Reconstruct();
        UpdateScroll();
    }

    public void Reconstruct() {
        ClearEntries();
        SpawnEntries();
    }

    private void UpdateScroll() {
        if (PanelScrollbar) {
            if (EntryObjects.Count <= maxEntriesBeforeScroll) {
                PanelScrollbar.value = Mathf.Lerp(PanelScrollbar.value, 1f, 10f);
                PanelScrollbar.gameObject.SetActive(false);
                return;
            }

            PanelScrollbar.gameObject.SetActive(true);
        }
    }

    private void SetHeadersActive(bool state) {
        for (var i = 0; i < ColumnTitlesContainer.childCount; i++)
            ColumnTitlesContainer.GetChild(i).gameObject.SetActive(state);
    }

    protected virtual void PositionColumnTitles() {
        if (ColumnTitlesContainer == null) return;

        if (EntryObjects.Count <= 0) {
            SetHeadersActive(false);
            return;
        }

        SetHeadersActive(true);
        var childCount = EntryObjects[0].transform.childCount;
        for (var i = 0; i < childCount; i++) {
            var component = EntryObjects[0].transform.GetChild(i).GetComponent<OverviewColumnIdentity>();
            if (component != null) {
                var gameObject = Util.KInstantiate(Prefab_ColumnTitle);
                gameObject.name = component.Column_DisplayName;
                var componentInChildren = gameObject.GetComponentInChildren<LocText>();
                gameObject.transform.SetParent(ColumnTitlesContainer);
                componentInChildren.text = component.StringLookup
                                               ? Strings.Get(component.Column_DisplayName)
                                               : component.Column_DisplayName;

                gameObject.GetComponent<ToolTip>().toolTip
                    = string.Format(UI.TOOLTIPS.SORTCOLUMN, componentInChildren.text);

                gameObject.rectTransform().anchoredPosition
                    = new Vector2(component.rectTransform().anchoredPosition.x, 0f);

                var overviewColumnIdentity = gameObject.GetComponent<OverviewColumnIdentity>();
                if (overviewColumnIdentity == null)
                    overviewColumnIdentity = gameObject.AddComponent<OverviewColumnIdentity>();

                overviewColumnIdentity.Column_DisplayName = component.Column_DisplayName;
                overviewColumnIdentity.columnID           = component.columnID;
                overviewColumnIdentity.xPivot             = component.xPivot;
                overviewColumnIdentity.Sortable           = component.Sortable;
                if (overviewColumnIdentity.Sortable)
                    overviewColumnIdentity.GetComponentInChildren<ImageToggleState>(true).gameObject.SetActive(true);
            }
        }

        UpdateColumnTitles();
        sortToggleGroup                = this.gameObject.AddComponent<ToggleGroup>();
        sortToggleGroup.allowSwitchOff = true;
    }

    protected void SortByName(bool reverse) {
        var list = new List<EntryType>(EntryObjects);
        list.Sort(delegate(EntryType a, EntryType b) {
                      var text = a.Identity.GetProperName() + a.gameObject.GetInstanceID();
                      var strB = b.Identity.GetProperName() + b.gameObject.GetInstanceID();
                      return text.CompareTo(strB);
                  });

        ReorderEntries(list, reverse);
    }

    protected void UpdateColumnTitles() {
        if (EntryObjects.Count <= 0 || !EntryObjects[0].gameObject.activeSelf) {
            SetHeadersActive(false);
            return;
        }

        SetHeadersActive(true);
        for (var i = 0; i < ColumnTitlesContainer.childCount; i++) {
            var rectTransform = ColumnTitlesContainer.GetChild(i).rectTransform();
            for (var j = 0; j < EntryObjects[0].transform.childCount; j++) {
                var component = EntryObjects[0].transform.GetChild(j).GetComponent<OverviewColumnIdentity>();
                if (component != null && component.Column_DisplayName == rectTransform.name) {
                    rectTransform.pivot = new Vector2(component.xPivot, rectTransform.pivot.y);
                    rectTransform.anchoredPosition
                        = new Vector2(component.rectTransform().anchoredPosition.x + columnTitleHorizontalOffset, 0f);

                    rectTransform.sizeDelta
                        = new Vector2(component.rectTransform().sizeDelta.x, rectTransform.sizeDelta.y);

                    if (rectTransform.anchoredPosition.x == 0f)
                        rectTransform.gameObject.SetActive(false);
                    else
                        rectTransform.gameObject.SetActive(true);
                }
            }
        }
    }

    protected void ReorderEntries(List<EntryType> sortedEntries, bool reverse) {
        for (var i = 0; i < sortedEntries.Count; i++)
            if (reverse)
                sortedEntries[i].transform.SetSiblingIndex(sortedEntries.Count - 1 - i);
            else
                sortedEntries[i].transform.SetSiblingIndex(i);
    }
}