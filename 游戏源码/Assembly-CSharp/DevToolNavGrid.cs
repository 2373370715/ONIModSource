using System.Linq;
using ImGuiNET;
using UnityEngine;

public class DevToolNavGrid : DevTool
{
	private const string INVALID_OVERLAY_MODE_STR = "None";

	private string[] navGridNames;

	private int selectedNavGrid;

	private bool drawLinks;

	public static DevToolNavGrid Instance;

	private int[] linkStats;

	private int highestLinkCell;

	private int highestLinkCount;

	private int selectedCell;

	private bool follow;

	private GameObject lockObject;

	public DevToolNavGrid()
	{
		Instance = this;
	}

	private bool Init()
	{
		if (Pathfinding.Instance == null)
		{
			return false;
		}
		if (navGridNames != null)
		{
			return true;
		}
		navGridNames = (from x in Pathfinding.Instance.GetNavGrids()
			select x.id).ToArray();
		return true;
	}

	protected override void RenderTo(DevPanel panel)
	{
		if (Init())
		{
			Contents();
		}
		else
		{
			ImGui.Text("Game not initialized");
		}
	}

	public void SetCell(int cell)
	{
		selectedCell = cell;
	}

	private void Contents()
	{
		ImGui.Combo("Nav Grid ID", ref selectedNavGrid, navGridNames, navGridNames.Length);
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid(navGridNames[selectedNavGrid]);
		ImGui.Text("Max Links per cell: " + navGrid.maxLinksPerCell);
		ImGui.Spacing();
		if (ImGui.Button("Calculate Stats"))
		{
			linkStats = new int[navGrid.maxLinksPerCell];
			highestLinkCell = 0;
			highestLinkCount = 0;
			for (int i = 0; i < Grid.CellCount; i++)
			{
				int num = 0;
				for (int j = 0; j < navGrid.maxLinksPerCell; j++)
				{
					int num2 = i * navGrid.maxLinksPerCell + j;
					if (navGrid.Links[num2].link == Grid.InvalidCell)
					{
						break;
					}
					num++;
				}
				if (num > highestLinkCount)
				{
					highestLinkCell = i;
					highestLinkCount = num;
				}
				linkStats[num]++;
			}
		}
		ImGui.SameLine();
		if (ImGui.Button("Clear"))
		{
			linkStats = null;
		}
		ImGui.SameLine();
		if (ImGui.Button("Rescan"))
		{
			navGrid.InitializeGraph();
		}
		if (linkStats != null)
		{
			ImGui.Text("Highest link count: " + highestLinkCount);
			ImGui.Text($"Utilized percentage: {(float)highestLinkCount / (float)navGrid.maxLinksPerCell * 100f} %");
			ImGui.SameLine();
			if (ImGui.Button($"Select {highestLinkCell}"))
			{
				selectedCell = highestLinkCell;
			}
			for (int k = 0; k < linkStats.Length; k++)
			{
				if (linkStats[k] > 0)
				{
					ImGui.Text($"\t{k}: {linkStats[k]}");
				}
			}
		}
		ImGui.Checkbox("DrawDebugPath", ref DebugHandler.DebugPathFinding);
		if (Camera.main != null && SelectTool.Instance != null)
		{
			GameObject gameObject = null;
			ImGui.Checkbox("Lock", ref follow);
			if (follow)
			{
				if (lockObject == null && SelectTool.Instance.selected != null)
				{
					lockObject = SelectTool.Instance.selected.gameObject;
				}
				gameObject = lockObject;
			}
			else if (SelectTool.Instance.selected != null)
			{
				gameObject = SelectTool.Instance.selected.gameObject;
				lockObject = null;
			}
			if (gameObject != null)
			{
				Navigator component = gameObject.GetComponent<Navigator>();
				if (component != null)
				{
					Vector2 positionFor = DevToolEntity.GetPositionFor(component.gameObject);
					ImGui.GetBackgroundDrawList().AddCircleFilled(positionFor, 10f, ImGui.GetColorU32(Color.green));
					Vector2 screenPosition = DevToolEntity.GetScreenPosition(component.GetComponent<KBatchedAnimController>().GetPivotSymbolPosition());
					ImGui.GetBackgroundDrawList().AddCircleFilled(screenPosition, 10f, ImGui.GetColorU32(Color.blue));
				}
			}
		}
		ImGui.Spacing();
		ImGui.Checkbox("Draw Links", ref drawLinks);
		if (drawLinks)
		{
			DebugDrawLinks(navGrid);
		}
		ImGui.Spacing();
		Grid.CellToXY(selectedCell, out var x, out var y);
		ImGui.Text($"Selected Cell: {selectedCell} ({x},{y})");
		if (!Grid.IsValidCell(selectedCell) || navGrid.Links == null || navGrid.Links.Length <= navGrid.maxLinksPerCell * selectedCell)
		{
			return;
		}
		for (int l = 0; l < navGrid.maxLinksPerCell; l++)
		{
			int num3 = selectedCell * navGrid.maxLinksPerCell + l;
			NavGrid.Link l2 = navGrid.Links[num3];
			if (l2.link != Grid.InvalidCell)
			{
				DrawLink(l, l2, navGrid);
				continue;
			}
			break;
		}
	}

	private void DrawLink(int idx, NavGrid.Link l, NavGrid navGrid)
	{
		NavGrid.Transition transition = navGrid.transitions[l.transitionId];
		ImGui.Text($"   {transition.start} -> {transition.end} x:{transition.x} y:{transition.y} anim:{transition.anim} cost:{transition.cost}");
	}

	private void DebugDrawLinks(NavGrid navGrid)
	{
		if (Camera.main == null)
		{
			return;
		}
		Camera main = Camera.main;
		int pixelHeight = main.pixelHeight;
		Color color = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * navGrid.maxLinksPerCell;
			for (int link = navGrid.Links[num].link; link != NavGrid.InvalidCell; link = navGrid.Links[num].link)
			{
				if (DrawNavTypeLink(navGrid, num, ref color))
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(i, navGrid.Links[num].startNavType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(link, navGrid.Links[num].endNavType);
					if (IsInCameraView(main, navPos) && IsInCameraView(main, navPos2))
					{
						Vector2 start = main.WorldToScreenPoint(navPos);
						Vector2 end = main.WorldToScreenPoint(navPos2);
						start.y = (float)pixelHeight - start.y;
						end.y = (float)pixelHeight - end.y;
						uint colorU = ImGui.GetColorU32(color);
						DrawArrowLink(start, end, colorU);
					}
				}
				num++;
			}
		}
	}

	private bool IsInCameraView(Camera camera, Vector3 pos)
	{
		Vector3 vector = camera.WorldToViewportPoint(pos);
		if (vector.x >= 0f && vector.y >= 0f && vector.x <= 1f)
		{
			return vector.y <= 1f;
		}
		return false;
	}

	private bool DrawNavTypeLink(NavGrid navGrid, int end_cell_idx, ref Color color)
	{
		for (int i = 0; i < navGrid.ValidNavTypes.Length; i++)
		{
			if (navGrid.ValidNavTypes[i] == navGrid.Links[end_cell_idx].startNavType)
			{
				color = navGrid.NavTypeColor(navGrid.Links[end_cell_idx].startNavType);
				return true;
			}
			if (navGrid.ValidNavTypes[i] == navGrid.Links[end_cell_idx].endNavType)
			{
				color = navGrid.NavTypeColor(navGrid.Links[end_cell_idx].endNavType);
				return true;
			}
		}
		return false;
	}

	private void DrawArrowLink(Vector2 start, Vector2 end, uint color)
	{
		ImDrawListPtr backgroundDrawList = ImGui.GetBackgroundDrawList();
		Vector2 vector = end - start;
		float magnitude = vector.magnitude;
		if (magnitude > 0f)
		{
			vector *= 1f / Mathf.Sqrt(magnitude);
		}
		Vector2 p = end - vector * 1f + new Vector2(0f - vector.y, vector.x) * 1f;
		Vector2 p2 = end - vector * 1f - new Vector2(0f - vector.y, vector.x) * 1f;
		backgroundDrawList.AddLine(start, end, color);
		backgroundDrawList.AddTriangleFilled(end, p, p2, color);
	}
}
