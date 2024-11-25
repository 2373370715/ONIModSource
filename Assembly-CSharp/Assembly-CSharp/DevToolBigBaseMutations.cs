using ImGuiNET;

public class DevToolBigBaseMutations : DevTool {
    protected override void RenderTo(DevPanel panel) {
        if (Game.Instance != null) {
            ShowButtons();
            return;
        }

        ImGui.Text("Game not available");
    }

    private void ShowButtons() {
        if (ImGui.Button("Destroy Ladders")) DestroyGameObjects(Components.Ladders,         Tag.Invalid);
        if (ImGui.Button("Destroy Tiles")) DestroyGameObjects(Components.BuildingCompletes, GameTags.FloorTiles);
        if (ImGui.Button("Destroy Wires")) DestroyGameObjects(Components.BuildingCompletes, GameTags.Wires);
        if (ImGui.Button("Destroy Pipes")) DestroyGameObjects(Components.BuildingCompletes, GameTags.Pipes);
    }

    private void DestroyGameObjects<T>(Components.Cmps<T> componentsList, Tag filterForTag) {
        for (var i = componentsList.Count - 1; i >= 0; i--)
            if (!componentsList[i].IsNullOrDestroyed() &&
                (!(filterForTag != Tag.Invalid) ||
                 (componentsList[i] as KMonoBehaviour).gameObject.HasTag(filterForTag)))
                Util.KDestroyGameObject(componentsList[i] as KMonoBehaviour);
    }
}