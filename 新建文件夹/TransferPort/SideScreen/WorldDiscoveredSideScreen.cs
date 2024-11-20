using System.Collections.Generic;
using RsLib;
using UnityEngine;
using UnityEngine.UI;

namespace RsTransferPort;

/// <summary>
///     <see cref="LogicBroadcastChannelSideScreen" />
/// </summary>
public class WorldDiscoveredSideScreen : RsSideScreenContent {
    [RsSideScreen.CopyField, SerializeField]
    private LocText headerLabel;

    [RsSideScreen.CopyField, SerializeField]
    private GameObject listContainer;

    // private LogicClusterLocationSensor sensor;
    [RsSideScreen.CopyField, SerializeField]
    private GameObject rowPrefab;

    private readonly Dictionary<AxialI, GameObject> worldRows = new();

    public override bool IsValidForTarget(GameObject target) {
        return target.GetComponent<TransferPortCenter>() != null;
    }

    public override void SetTarget(GameObject target) {
        base.SetTarget(target);
        Build();
    }

    private void ClearRows() {
        foreach (var worldRow in worldRows) Util.KDestroyGameObject(worldRow.Value);
        worldRows.Clear();
    }

    private void Build() {
        headerLabel.SetText(MYSTRINGS.UI.SIDESCREEN.WORLDDISCOVEREDSIDESCREEN.HEADE);
        ClearRows();
        foreach (var worldContainer in ClusterManager.Instance.WorldContainers)
            if (!worldContainer.IsModuleInterior && !worldContainer.IsStartWorld) {
                var gameObject = Util.KInstantiateUI(rowPrefab, listContainer);
                gameObject.gameObject.name = worldContainer.GetProperName();
                var myWorldLocation = worldContainer.GetMyWorldLocation();
                Debug.Assert(!worldRows.ContainsKey(myWorldLocation),
                             "Adding two worlds/POI with the same cluster location to ClusterLocationFilterSideScreen UI: " +
                             worldContainer.GetProperName());

                worldRows.Add(myWorldLocation, gameObject);
            }

        Refresh();
    }

    private void Refresh() {
        foreach (var worldRow in worldRows) {
            var kvp            = worldRow;
            var cmp            = ClusterGrid.Instance.cellContents[kvp.Key][0];
            var worldContainer = cmp.GetComponent<WorldContainer>();
            kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(cmp.GetProperName());
            kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite
                = Def.GetUISprite(cmp).first;

            kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color
                = Def.GetUISprite(cmp).second;

            kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick
                = (System.Action)(() => {
                                      ToggleWorldDiscovered(worldContainer);
                                      Refresh();
                                  });

            kvp.Value.GetComponent<HierarchyReferences>()
               .GetReference<MultiToggle>("Toggle")
               .ChangeState(worldContainer.IsDiscovered ? 1 : 0);

            kvp.Value.SetActive(ClusterGrid.Instance.GetCellRevealLevel(kvp.Key) == ClusterRevealLevel.Visible);
        }
    }

    private void ToggleWorldDiscovered(WorldContainer worldContainer) {
        if (!worldContainer.IsDiscovered)
            worldContainer.SetDiscovered();
        else {
            RsField.SetValue(worldContainer, "isDiscovered", false);
            Game.Instance.Trigger(-521212405, worldContainer);
        }
    }

    public override string GetTitle() { return MYSTRINGS.UI.SIDESCREEN.WORLDDISCOVEREDSIDESCREEN.TITLE; }
}