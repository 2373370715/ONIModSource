using System.Collections;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr {
    private System.Action firstFrameCallback;

    [MyCmpReq]
    private KAnimGraphTileVisualizer graphTileDependency;

    public Vector3 Position => transform.GetPosition();

    public void SetFirstFrameCallback(System.Action ffCb) {
        firstFrameCallback = ffCb;
        StartCoroutine(RunCallback());
    }

    public IUtilityNetworkMgr GetNetworkManager() { return Game.Instance.solidConduitSystem; }

    private IEnumerator RunCallback() {
        yield return null;

        if (firstFrameCallback != null) {
            firstFrameCallback();
            firstFrameCallback = null;
        }

        yield return null;
    }

    public        UtilityNetwork   GetNetwork() { return GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this)); }
    public static SolidConduitFlow GetFlowManager() { return Game.Instance.solidConduitFlow; }

    protected override void OnSpawn() {
        base.OnSpawn();
        GetComponent<KSelectable>()
            .SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, this);
    }

    protected override void OnCleanUp() {
        var cell      = Grid.PosToCell(this);
        var component = GetComponent<BuildingComplete>();
        if (component.Def.ReplacementLayer                          == ObjectLayer.NumLayers ||
            Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null) {
            GetNetworkManager().RemoveFromNetworks(cell, this, false);
            GetFlowManager().EmptyConduit(cell);
        }

        base.OnCleanUp();
    }
}