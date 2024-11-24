using ImGuiNET;

public class DevToolBigBaseMutations : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance != null)
		{
			ShowButtons();
		}
		else
		{
			ImGui.Text("Game not available");
		}
	}

	private void ShowButtons()
	{
		if (ImGui.Button("Destroy Ladders"))
		{
			DestroyGameObjects(Components.Ladders, Tag.Invalid);
		}
		if (ImGui.Button("Destroy Tiles"))
		{
			DestroyGameObjects(Components.BuildingCompletes, GameTags.FloorTiles);
		}
		if (ImGui.Button("Destroy Wires"))
		{
			DestroyGameObjects(Components.BuildingCompletes, GameTags.Wires);
		}
		if (ImGui.Button("Destroy Pipes"))
		{
			DestroyGameObjects(Components.BuildingCompletes, GameTags.Pipes);
		}
	}

	private void DestroyGameObjects<T>(Components.Cmps<T> componentsList, Tag filterForTag)
	{
		for (int num = componentsList.Count - 1; num >= 0; num--)
		{
			if (!componentsList[num].IsNullOrDestroyed() && (!(filterForTag != Tag.Invalid) || (componentsList[num] as KMonoBehaviour).gameObject.HasTag(filterForTag)))
			{
				Util.KDestroyGameObject(componentsList[num] as KMonoBehaviour);
			}
		}
	}
}
