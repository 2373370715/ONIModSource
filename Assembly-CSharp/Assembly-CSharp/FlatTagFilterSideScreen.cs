using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlatTagFilterSideScreen : SideScreenContent {
    [SerializeField]
    private LocText headerLabel;

    [SerializeField]
    private GameObject listContainer;

    [SerializeField]
    private GameObject rowPrefab;

    private readonly Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
    private          FlatTagFilterable           tagFilterable;

    public override bool IsValidForTarget(GameObject target) {
        return target.GetComponent<FlatTagFilterable>() != null;
    }

    public override void SetTarget(GameObject target) {
        base.SetTarget(target);
        tagFilterable = target.GetComponent<FlatTagFilterable>();
        Build();
    }

    private void Build() {
        headerLabel.SetText(tagFilterable.GetHeaderText());
        foreach (var keyValuePair in rows) Util.KDestroyGameObject(keyValuePair.Value);
        rows.Clear();
        foreach (var tag in tagFilterable.tagOptions) {
            var gameObject = Util.KInstantiateUI(rowPrefab, listContainer);
            gameObject.gameObject.name = tag.ProperName();
            rows.Add(tag, gameObject);
        }

        Refresh();
    }

    private void Refresh() {
        using (var enumerator = rows.GetEnumerator()) {
            while (enumerator.MoveNext()) {
                var kvp = enumerator.Current;
                kvp.Value.GetComponent<HierarchyReferences>()
                   .GetReference<LocText>("Label")
                   .SetText(kvp.Key.ProperNameStripLink());

                kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite
                    = Def.GetUISprite(kvp.Key).first;

                kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color
                    = Def.GetUISprite(kvp.Key).second;

                kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate {
                    tagFilterable.ToggleTag(kvp.Key);
                    Refresh();
                };

                kvp.Value.GetComponent<HierarchyReferences>()
                   .GetReference<MultiToggle>("Toggle")
                   .ChangeState(tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);

                kvp.Value.SetActive(!tagFilterable.displayOnlyDiscoveredTags ||
                                    DiscoveredResources.Instance.IsDiscovered(kvp.Key));
            }
        }
    }

    public override string GetTitle() { return tagFilterable.gameObject.GetProperName(); }
}