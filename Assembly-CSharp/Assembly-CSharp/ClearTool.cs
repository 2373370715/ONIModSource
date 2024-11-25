public class ClearTool : DragTool {
    public static ClearTool Instance;
    public static void      DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Instance                       = this;
        interceptNumberKeysForPriority = true;
    }

    public void Activate() { PlayerController.Instance.ActivateTool(this); }

    protected override void OnDragTool(int cell, int distFromOrigin) {
        var gameObject = Grid.Objects[cell, 3];
        if (gameObject == null) return;

        var objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
        while (objectLayerListItem != null) {
            var gameObject2 = objectLayerListItem.gameObject;
            objectLayerListItem = objectLayerListItem.nextItem;
            if (!(gameObject2                                == null) &&
                !(gameObject2.GetComponent<MinionIdentity>() != null) &&
                gameObject2.GetComponent<Clearable>().isClearable) {
                gameObject2.GetComponent<Clearable>().MarkForClear();
                var component = gameObject2.GetComponent<Prioritizable>();
                if (component != null)
                    component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
            }
        }
    }

    protected override void OnActivateTool() {
        base.OnActivateTool();
        ToolMenu.Instance.PriorityScreen.Show();
    }

    protected override void OnDeactivateTool(InterfaceTool new_tool) {
        base.OnDeactivateTool(new_tool);
        ToolMenu.Instance.PriorityScreen.Show(false);
    }
}