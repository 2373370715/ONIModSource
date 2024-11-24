using System;
using ImGuiNET;

// Token: 0x02000B99 RID: 2969
public class DevToolBigBaseMutations : DevTool
{
	// Token: 0x060038E3 RID: 14563 RVA: 0x000C4D40 File Offset: 0x000C2F40
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance != null)
		{
			this.ShowButtons();
			return;
		}
		ImGui.Text("Game not available");
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x0021C580 File Offset: 0x0021A780
	private void ShowButtons()
	{
		if (ImGui.Button("Destroy Ladders"))
		{
			this.DestroyGameObjects<Ladder>(Components.Ladders, Tag.Invalid);
		}
		if (ImGui.Button("Destroy Tiles"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.FloorTiles);
		}
		if (ImGui.Button("Destroy Wires"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Wires);
		}
		if (ImGui.Button("Destroy Pipes"))
		{
			this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Pipes);
		}
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0021C600 File Offset: 0x0021A800
	private void DestroyGameObjects<T>(Components.Cmps<T> componentsList, Tag filterForTag)
	{
		for (int i = componentsList.Count - 1; i >= 0; i--)
		{
			if (!componentsList[i].IsNullOrDestroyed() && (!(filterForTag != Tag.Invalid) || (componentsList[i] as KMonoBehaviour).gameObject.HasTag(filterForTag)))
			{
				Util.KDestroyGameObject(componentsList[i] as KMonoBehaviour);
			}
		}
	}
}
