using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class AssignableSideScreen : SideScreenContent {
    private Comparison<IAssignableIdentity> activeSortFunction;
    private MultiToggle                     activeSortToggle;

    [SerializeField]
    private LocText currentOwnerText;

    [SerializeField]
    private MultiToggle dupeSortingToggle;

    [SerializeField]
    private MultiToggle generalSortingToggle;

    private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();

    private readonly Dictionary<IAssignableIdentity, AssignableSideScreenRow> identityRowMap
        = new Dictionary<IAssignableIdentity, AssignableSideScreenRow>();

    [SerializeField]
    private GameObject rowGroup;

    private UIPool<AssignableSideScreenRow> rowPool;

    [SerializeField]
    private AssignableSideScreenRow rowPrefab;

    private bool       sortReversed;
    private int        targetAssignableSubscriptionHandle = -1;
    public  Assignable targetAssignable { get; private set; }

    public override string GetTitle() {
        if (targetAssignable != null) return string.Format(base.GetTitle(), targetAssignable.GetProperName());

        return base.GetTitle();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        var multiToggle = dupeSortingToggle;
        multiToggle.onClick
            = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate { SortByName(true); }));

        var multiToggle2 = generalSortingToggle;
        multiToggle2.onClick
            = (System.Action)Delegate.Combine(multiToggle2.onClick,
                                              new System.Action(delegate { SortByAssignment(true); }));

        Subscribe(Game.Instance.gameObject, 875045922, OnRefreshData);
    }

    private void OnRefreshData(object obj) { SetTarget(targetAssignable.gameObject); }

    public override void ClearTarget() {
        if (targetAssignableSubscriptionHandle != -1 && targetAssignable != null) {
            targetAssignable.Unsubscribe(targetAssignableSubscriptionHandle);
            targetAssignableSubscriptionHandle = -1;
        }

        targetAssignable                         =  null;
        Components.LiveMinionIdentities.OnAdd    -= OnMinionIdentitiesChanged;
        Components.LiveMinionIdentities.OnRemove -= OnMinionIdentitiesChanged;
        base.ClearTarget();
    }

    public override bool IsValidForTarget(GameObject target) {
        return target.GetComponent<Assignable>() != null       &&
               target.GetComponent<Assignable>().CanBeAssigned &&
               target.GetComponent<AssignmentGroupController>() == null;
    }

    public override void SetTarget(GameObject target) {
        Components.LiveMinionIdentities.OnAdd    += OnMinionIdentitiesChanged;
        Components.LiveMinionIdentities.OnRemove += OnMinionIdentitiesChanged;
        if (targetAssignableSubscriptionHandle != -1 && targetAssignable != null)
            targetAssignable.Unsubscribe(targetAssignableSubscriptionHandle);

        targetAssignable = target.GetComponent<Assignable>();
        if (targetAssignable == null) {
            Debug.LogError(string.Format("{0} selected has no Assignable component.", target.GetProperName()));
            return;
        }

        if (rowPool == null) rowPool = new UIPool<AssignableSideScreenRow>(rowPrefab);
        gameObject.SetActive(true);
        identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
        dupeSortingToggle.ChangeState(0);
        generalSortingToggle.ChangeState(0);
        activeSortToggle   = null;
        activeSortFunction = null;
        if (!targetAssignable.CanBeAssigned)
            HideScreen(true);
        else
            HideScreen(false);

        targetAssignableSubscriptionHandle = targetAssignable.Subscribe(684616645, OnAssigneeChanged);
        Refresh(identityList);
        SortByAssignment(false);
    }

    private void OnMinionIdentitiesChanged(MinionIdentity change) {
        identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
        Refresh(identityList);
    }

    private void OnAssigneeChanged(object data = null) {
        foreach (var keyValuePair in identityRowMap) keyValuePair.Value.Refresh();
    }

    private void Refresh(List<MinionAssignablesProxy> identities) {
        ClearContent();
        currentOwnerText.text = string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED, Array.Empty<object>());
        if (targetAssignable == null) return;

        if (targetAssignable.GetComponent<Equippable>() == null &&
            !targetAssignable.HasTag(GameTags.NotRoomAssignable)) {
            var roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(targetAssignable.gameObject);
            if (roomOfGameObject != null) {
                var roomType = roomOfGameObject.roomType;
                if (roomType.primary_constraint != null &&
                    !roomType.primary_constraint.building_criteria(targetAssignable.GetComponent<KPrefabID>())) {
                    var freeElement = rowPool.GetFreeElement(rowGroup, true);
                    freeElement.sideScreen = this;
                    identityRowMap.Add(roomOfGameObject, freeElement);
                    freeElement.SetContent(roomOfGameObject, OnRowClicked, this);
                    return;
                }
            }
        }

        if (targetAssignable.canBePublic) {
            var freeElement2 = rowPool.GetFreeElement(rowGroup, true);
            freeElement2.sideScreen = this;
            freeElement2.transform.SetAsFirstSibling();
            identityRowMap.Add(Game.Instance.assignmentManager.assignment_groups["public"], freeElement2);
            freeElement2.SetContent(Game.Instance.assignmentManager.assignment_groups["public"], OnRowClicked, this);
        }

        foreach (var minionAssignablesProxy in identities) {
            var freeElement3 = rowPool.GetFreeElement(rowGroup, true);
            freeElement3.sideScreen = this;
            identityRowMap.Add(minionAssignablesProxy, freeElement3);
            freeElement3.SetContent(minionAssignablesProxy, OnRowClicked, this);
        }

        ExecuteSort(activeSortFunction);
    }

    private void SortByName(bool reselect) {
        SelectSortToggle(dupeSortingToggle, reselect);
        ExecuteSort((i1, i2) => i1.GetProperName().CompareTo(i2.GetProperName()) * (sortReversed ? -1 : 1));
    }

    private void SortByAssignment(bool reselect) {
        SelectSortToggle(generalSortingToggle, reselect);
        Comparison<IAssignableIdentity> sortFunction = delegate(IAssignableIdentity i1, IAssignableIdentity i2) {
                                                           var num = targetAssignable.CanAssignTo(i1)
                                                               .CompareTo(targetAssignable.CanAssignTo(i2));

                                                           if (num != 0) return num * -1;

                                                           num = identityRowMap[i1]
                                                                 .currentState
                                                                 .CompareTo(identityRowMap[i2].currentState);

                                                           if (num != 0) return num * (sortReversed ? -1 : 1);

                                                           return i1.GetProperName().CompareTo(i2.GetProperName());
                                                       };

        ExecuteSort(sortFunction);
    }

    private void SelectSortToggle(MultiToggle toggle, bool reselect) {
        dupeSortingToggle.ChangeState(0);
        generalSortingToggle.ChangeState(0);
        if (toggle != null) {
            if (reselect && activeSortToggle == toggle) sortReversed = !sortReversed;
            activeSortToggle = toggle;
        }

        activeSortToggle.ChangeState(sortReversed ? 2 : 1);
    }

    private void ExecuteSort(Comparison<IAssignableIdentity> sortFunction) {
        if (sortFunction != null) {
            var list = new List<IAssignableIdentity>(identityRowMap.Keys);
            list.Sort(sortFunction);
            for (var i = 0; i < list.Count; i++) identityRowMap[list[i]].transform.SetSiblingIndex(i);
            activeSortFunction = sortFunction;
        }
    }

    private void ClearContent() {
        if (rowPool != null) rowPool.DestroyAll();
        foreach (var keyValuePair in identityRowMap) keyValuePair.Value.targetIdentity = null;
        identityRowMap.Clear();
    }

    private void HideScreen(bool hide) {
        if (hide) {
            transform.localScale = Vector3.zero;
            return;
        }

        if (transform.localScale != Vector3.one) transform.localScale = Vector3.one;
    }

    private void OnRowClicked(IAssignableIdentity identity) {
        if (targetAssignable.assignee != identity) {
            ChangeAssignment(identity);
            return;
        }

        if (CanDeselect(identity)) ChangeAssignment(null);
    }

    private bool CanDeselect(IAssignableIdentity identity) { return identity is MinionAssignablesProxy; }

    private void ChangeAssignment(IAssignableIdentity new_identity) {
        targetAssignable.Unassign();
        if (!new_identity.IsNullOrDestroyed()) targetAssignable.Assign(new_identity);
    }

    private void OnValidStateChanged(bool state) {
        if (gameObject.activeInHierarchy) Refresh(identityList);
    }
}