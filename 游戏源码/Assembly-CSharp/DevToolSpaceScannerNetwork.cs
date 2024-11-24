using System.Collections.Generic;
using ImGuiNET;

public class DevToolSpaceScannerNetwork : DevTool
{
	public readonly struct Entry
	{
		public readonly int worldId;

		public readonly float networkQuality;

		public readonly string targetsString;

		public Entry(int worldId, float networkQuality, string targetsString)
		{
			this.worldId = worldId;
			this.networkQuality = networkQuality;
			this.targetsString = targetsString;
		}
	}

	private ImGuiObjectTableDrawer<Entry> tableDrawer;

	public DevToolSpaceScannerNetwork()
	{
		tableDrawer = ImGuiObjectTableDrawer<Entry>.New().Column("WorldId", (Entry e) => e.worldId).Column("Network Quality (0->1)", (Entry e) => e.networkQuality)
			.Column("Targets Detected", (Entry e) => e.targetsString)
			.FixedHeight(300f)
			.Build();
	}

	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("Game instance is null");
			return;
		}
		if (Game.Instance.spaceScannerNetworkManager == null)
		{
			ImGui.Text("SpaceScannerNetworkQualityManager instance is null");
			return;
		}
		if (ClusterManager.Instance == null)
		{
			ImGui.Text("ClusterManager instance is null");
			return;
		}
		if (ImGui.CollapsingHeader("Worlds Data"))
		{
			tableDrawer.Draw(GetData());
		}
		if (ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
		{
			ImGuiEx.DrawObject(Game.Instance.spaceScannerNetworkManager);
		}
	}

	public static IEnumerable<Entry> GetData()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			yield return new Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), GetTargetsString(worldContainer));
		}
	}

	public static string GetTargetsString(WorldContainer world)
	{
		if (!Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out var value))
		{
			return "<none>";
		}
		if (value.targetIdsDetected.Count == 0)
		{
			return "<none>";
		}
		return string.Join(",", value.targetIdsDetected);
	}
}
