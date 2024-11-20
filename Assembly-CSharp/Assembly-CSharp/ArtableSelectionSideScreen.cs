using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtableSelectionSideScreen : SideScreenContent {
    private const int     INVALID_SUBSCRIPTION = -1;
    public        KButton applyButton;

    [SerializeField]
    private RectTransform buttonContainer;

    private readonly Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();
    public           KButton                         clearButton;

    [SerializeField]
    private RectTransform scrollTransoform;

    private string     selectedStage = "";
    public  GameObject stateButtonPrefab;
    private Artable    target;
    private int        workCompleteSub = -1;

    public override bool IsValidForTarget(GameObject target) {
        var component = target.GetComponent<Artable>();
        return !(component == null) && !(component.CurrentStage == "Default");
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        applyButton.onClick += delegate {
                                   target.SetUserChosenTargetState(selectedStage);
                                   SelectTool.Instance.Select(null, true);
                               };

        clearButton.onClick += delegate {
                                   selectedStage = "";
                                   target.SetDefault();
                                   SelectTool.Instance.Select(null, true);
                               };
    }

    public override void SetTarget(GameObject target) {
        if (workCompleteSub != -1) {
            target.Unsubscribe(workCompleteSub);
            workCompleteSub = -1;
        }

        base.SetTarget(target);
        this.target     = target.GetComponent<Artable>();
        workCompleteSub = target.Subscribe(-2011693419, OnRefreshTarget);
        OnRefreshTarget();
    }

    public override void ClearTarget() {
        target.Unsubscribe(-2011693419);
        workCompleteSub = -1;
        base.ClearTarget();
    }

    private void OnRefreshTarget(object data = null) {
        if (target == null) return;

        GenerateStateButtons();
        selectedStage = target.CurrentStage;
        RefreshButtons();
    }

    public void GenerateStateButtons() {
        foreach (var keyValuePair in buttons) Util.KDestroyGameObject(keyValuePair.Value.gameObject);
        buttons.Clear();
        foreach (var artableStage in Db.GetArtableStages().GetPrefabStages(target.GetComponent<KPrefabID>().PrefabID()))
            if (!(artableStage.id == "Default")) {
                var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);
                var sprite     = artableStage.GetPermitPresentationInfo().sprite;
                var component  = gameObject.GetComponent<MultiToggle>();
                component.GetComponent<ToolTip>().SetSimpleTooltip(artableStage.Name);
                component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
                buttons.Add(artableStage.id, component);
            }
    }

    private void RefreshButtons() {
        var prefabStages = Db.GetArtableStages().GetPrefabStages(target.GetComponent<KPrefabID>().PrefabID());
        var artableStage = prefabStages.Find(match => match.id == target.CurrentStage);
        var num          = 0;

        // using (Dictionary<string, MultiToggle>.Enumerator enumerator = this.buttons.GetEnumerator())
        // {
        // while (enumerator.MoveNext())
        // {
        // ArtableSelectionSideScreen.<>c__DisplayClass16_0 CS$<>8__locals1 = new ArtableSelectionSideScreen.<>c__DisplayClass16_0();
        // CS$<>8__locals1.<>4__this = this;
        // CS$<>8__locals1.kvp = enumerator.Current;
        // ArtableStage stage = prefabStages.Find((ArtableStage match) => match.id == CS$<>8__locals1.kvp.Key);
        // if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
        // {
        // CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
        // }
        // else if (!stage.IsUnlocked())
        // {
        // CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
        // }
        // else
        // {
        // num++;
        // CS$<>8__locals1.kvp.Value.gameObject.SetActive(true);
        // CS$<>8__locals1.kvp.Value.ChangeState((this.selectedStage == CS$<>8__locals1.kvp.Key) ? 1 : 0);
        // MultiToggle value = CS$<>8__locals1.kvp.Value;
        // value.onClick = (System.Action)Delegate.Combine(value.onClick, new System.Action(delegate()
        // {
        // CS$<>8__locals1.<>4__this.selectedStage = stage.id;
        // CS$<>8__locals1.<>4__this.RefreshButtons();
        // }));
        // }
        // }
        // }
        scrollTransoform.GetComponent<LayoutElement>().preferredHeight = num > 3 ? 200 : 100;
    }
}