using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000BCC RID: 3020
public class DevToolSpaceScannerNetwork : DevTool
{
	// Token: 0x060039BF RID: 14783 RVA: 0x00224C50 File Offset: 0x00222E50
	public DevToolSpaceScannerNetwork()
	{
		this.tableDrawer = ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry>.New().Column("WorldId", (DevToolSpaceScannerNetwork.Entry e) => e.worldId).Column("Network Quality (0->1)", (DevToolSpaceScannerNetwork.Entry e) => e.networkQuality).Column("Targets Detected", (DevToolSpaceScannerNetwork.Entry e) => e.targetsString).FixedHeight(300f).Build();
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x00224CF8 File Offset: 0x00222EF8
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
			this.tableDrawer.Draw(DevToolSpaceScannerNetwork.GetData());
		}
		if (ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
		{
			ImGuiEx.DrawObject(Game.Instance.spaceScannerNetworkManager, null);
		}
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x000C54DC File Offset: 0x000C36DC
	public static IEnumerable<DevToolSpaceScannerNetwork.Entry> GetData()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			yield return new DevToolSpaceScannerNetwork.Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), DevToolSpaceScannerNetwork.GetTargetsString(worldContainer));
		}
		IEnumerator<WorldContainer> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x00224D8C File Offset: 0x00222F8C
	public static string GetTargetsString(WorldContainer world)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (!Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out spaceScannerWorldData))
		{
			return "<none>";
		}
		if (spaceScannerWorldData.targetIdsDetected.Count == 0)
		{
			return "<none>";
		}
		return string.Join(",", spaceScannerWorldData.targetIdsDetected);
	}

	// Token: 0x04002771 RID: 10097
	private ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry> tableDrawer;

	// Token: 0x02000BCD RID: 3021
	public readonly struct Entry
	{
		// Token: 0x060039C3 RID: 14787 RVA: 0x000C54E5 File Offset: 0x000C36E5
		public Entry(int worldId, float networkQuality, string targetsString)
		{
			this.worldId = worldId;
			this.networkQuality = networkQuality;
			this.targetsString = targetsString;
		}

		// Token: 0x04002772 RID: 10098
		public readonly int worldId;

		// Token: 0x04002773 RID: 10099
		public readonly float networkQuality;

		// Token: 0x04002774 RID: 10100
		public readonly string targetsString;
	}
}
