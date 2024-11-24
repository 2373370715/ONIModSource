using System;
using UnityEngine;

// Token: 0x02000A9E RID: 2718
[AddComponentMenu("KMonoBehaviour/scripts/NavPathDrawer")]
public class NavPathDrawer : KMonoBehaviour
{
	// Token: 0x1700020B RID: 523
	// (get) Token: 0x0600326E RID: 12910 RVA: 0x000C0C5F File Offset: 0x000BEE5F
	// (set) Token: 0x0600326F RID: 12911 RVA: 0x000C0C66 File Offset: 0x000BEE66
	public static NavPathDrawer Instance { get; private set; }

	// Token: 0x06003270 RID: 12912 RVA: 0x000C0C6E File Offset: 0x000BEE6E
	public static void DestroyInstance()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x00203898 File Offset: 0x00201A98
	protected override void OnPrefabInit()
	{
		Shader shader = Shader.Find("Lines/Colored Blended");
		this.material = new Material(shader);
		NavPathDrawer.Instance = this;
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x000C0C6E File Offset: 0x000BEE6E
	protected override void OnCleanUp()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x000C0C76 File Offset: 0x000BEE76
	public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
	{
		this.navigatorPos = navigator_pos;
		this.navigatorPos.y = this.navigatorPos.y + 0.5f;
		this.path = path;
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x000C0CA2 File Offset: 0x000BEEA2
	public Navigator GetNavigator()
	{
		return this.navigator;
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000C0CAA File Offset: 0x000BEEAA
	public void SetNavigator(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x000C0CB3 File Offset: 0x000BEEB3
	public void ClearNavigator()
	{
		this.navigator = null;
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x002038C4 File Offset: 0x00201AC4
	private void DrawPath(PathFinder.Path path, Vector3 navigator_pos, Color color)
	{
		if (path.nodes != null && path.nodes.Count > 1)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			GL.Color(color);
			GL.Vertex(navigator_pos);
			GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
			for (int i = 1; i < path.nodes.Count - 1; i++)
			{
				if ((int)Grid.WorldIdx[path.nodes[i].cell] == ClusterManager.Instance.activeWorldId && (int)Grid.WorldIdx[path.nodes[i + 1].cell] == ClusterManager.Instance.activeWorldId)
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(path.nodes[i].cell, path.nodes[i].navType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[i + 1].cell, path.nodes[i + 1].navType);
					GL.Vertex(navPos);
					GL.Vertex(navPos2);
				}
			}
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x00203A10 File Offset: 0x00201C10
	private void OnPostRender()
	{
		this.DrawPath(this.path, this.navigatorPos, Color.white);
		this.path = default(PathFinder.Path);
		this.DebugDrawSelectedNavigator();
		if (this.navigator != null)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			PathFinderQuery query = PathFinderQueries.drawNavGridQuery.Reset(null);
			this.navigator.RunQuery(query);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x00203A90 File Offset: 0x00201C90
	private void DebugDrawSelectedNavigator()
	{
		if (!DebugHandler.DebugPathFinding)
		{
			return;
		}
		if (SelectTool.Instance == null)
		{
			return;
		}
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
		if (component == null)
		{
			return;
		}
		int mouseCell = DebugHandler.GetMouseCell();
		if (Grid.IsValidCell(mouseCell))
		{
			PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell(component), component.CurrentNavType, component.flags);
			PathFinder.Path path = default(PathFinder.Path);
			PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
			string text = "";
			text = text + "Source: " + Grid.PosToCell(component).ToString() + "\n";
			text = text + "Dest: " + mouseCell.ToString() + "\n";
			text = text + "Cost: " + path.cost.ToString();
			this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green);
			DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
		}
	}

	// Token: 0x040021DF RID: 8671
	private PathFinder.Path path;

	// Token: 0x040021E0 RID: 8672
	public Material material;

	// Token: 0x040021E1 RID: 8673
	private Vector3 navigatorPos;

	// Token: 0x040021E2 RID: 8674
	private Navigator navigator;
}
