using System;
using System.Collections.Generic;

// Token: 0x020007DA RID: 2010
public class NavGridUpdater
{
	// Token: 0x060023F6 RID: 9206 RVA: 0x000B75BD File Offset: 0x000B57BD
	public static void InitializeNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type)
	{
		NavGridUpdater.MarkValidCells(nav_table, validators, bounding_offsets);
		NavGridUpdater.CreateLinks(nav_table, max_links_per_cell, links, transitions_by_nav_type, new Dictionary<int, int>());
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000B75D7 File Offset: 0x000B57D7
	public static void UpdateNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions, IEnumerable<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateValidCells(dirty_nav_cells, nav_table, validators, bounding_offsets);
		NavGridUpdater.UpdateLinks(dirty_nav_cells, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x001C850C File Offset: 0x001C670C
	private static void UpdateValidCells(IEnumerable<int> dirty_solid_cells, NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		foreach (int cell in dirty_solid_cells)
		{
			for (int i = 0; i < validators.Length; i++)
			{
				validators[i].UpdateCell(cell, nav_table, bounding_offsets);
			}
		}
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000B75F2 File Offset: 0x000B57F2
	private static void CreateLinksForCell(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		NavGridUpdater.CreateLinks(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x001C8568 File Offset: 0x001C6768
	private static void UpdateLinks(IEnumerable<int> dirty_nav_cells, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		foreach (int cell in dirty_nav_cells)
		{
			NavGridUpdater.CreateLinksForCell(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
		}
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x001C85B4 File Offset: 0x001C67B4
	private static void CreateLinks(NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.CreateLinkWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x001C8604 File Offset: 0x001C6804
	private static void CreateLinks(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		int num = cell * max_links_per_cell;
		int num2 = 0;
		for (int i = 0; i < 11; i++)
		{
			NavType nav_type = (NavType)i;
			NavGrid.Transition[] array = transitions_by_nav_type[i];
			if (array != null && nav_table.IsValid(cell, nav_type))
			{
				NavGrid.Transition[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					NavGrid.Transition transition;
					if ((transition = array2[j]).start == NavType.Teleport && teleport_transitions.ContainsKey(cell))
					{
						int num3;
						int num4;
						Grid.CellToXY(cell, out num3, out num4);
						int num5 = teleport_transitions[cell];
						int num6;
						int num7;
						Grid.CellToXY(teleport_transitions[cell], out num6, out num7);
						transition.x = num6 - num3;
						transition.y = num7 - num4;
					}
					int num8 = transition.IsValid(cell, nav_table);
					if (num8 != Grid.InvalidCell)
					{
						links[num] = new NavGrid.Link(num8, transition.start, transition.end, transition.id, transition.cost);
						num++;
						num2++;
					}
				}
			}
		}
		if (num2 >= max_links_per_cell)
		{
			Debug.LogError("Out of nav links. Need to increase maxLinksPerCell:" + max_links_per_cell.ToString());
		}
		links[num].link = Grid.InvalidCell;
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x001C8730 File Offset: 0x001C6930
	private static void MarkValidCells(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.MarkValidCellWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, bounding_offsets, validators));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000B7550 File Offset: 0x000B5750
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x001C877C File Offset: 0x001C697C
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGridUpdater.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x04001816 RID: 6166
	public static int InvalidHandle = -1;

	// Token: 0x04001817 RID: 6167
	public static int InvalidIdx = -1;

	// Token: 0x04001818 RID: 6168
	public static int InvalidCell = -1;

	// Token: 0x020007DB RID: 2011
	private struct CreateLinkWorkItem : IWorkItem<object>
	{
		// Token: 0x06002402 RID: 9218 RVA: 0x000B7615 File Offset: 0x000B5815
		public CreateLinkWorkItem(int start_cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.maxLinksPerCell = max_links_per_cell;
			this.links = links;
			this.transitionsByNavType = transitions_by_nav_type;
			this.teleportTransitions = teleport_transitions;
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x001C87D4 File Offset: 0x001C69D4
		public void Run(object shared_data)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				NavGridUpdater.CreateLinksForCell(this.startCell + i, this.navTable, this.maxLinksPerCell, this.links, this.transitionsByNavType, this.teleportTransitions);
			}
		}

		// Token: 0x04001819 RID: 6169
		private int startCell;

		// Token: 0x0400181A RID: 6170
		private NavTable navTable;

		// Token: 0x0400181B RID: 6171
		private int maxLinksPerCell;

		// Token: 0x0400181C RID: 6172
		private NavGrid.Link[] links;

		// Token: 0x0400181D RID: 6173
		private NavGrid.Transition[][] transitionsByNavType;

		// Token: 0x0400181E RID: 6174
		private Dictionary<int, int> teleportTransitions;
	}

	// Token: 0x020007DC RID: 2012
	private struct MarkValidCellWorkItem : IWorkItem<object>
	{
		// Token: 0x06002404 RID: 9220 RVA: 0x000B7644 File Offset: 0x000B5844
		public MarkValidCellWorkItem(int start_cell, NavTable nav_table, CellOffset[] bounding_offsets, NavTableValidator[] validators)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.boundingOffsets = bounding_offsets;
			this.validators = validators;
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x001C881C File Offset: 0x001C6A1C
		public void Run(object shared_data)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				int cell = this.startCell + i;
				NavTableValidator[] array = this.validators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].UpdateCell(cell, this.navTable, this.boundingOffsets);
				}
			}
		}

		// Token: 0x0400181F RID: 6175
		private NavTable navTable;

		// Token: 0x04001820 RID: 6176
		private CellOffset[] boundingOffsets;

		// Token: 0x04001821 RID: 6177
		private NavTableValidator[] validators;

		// Token: 0x04001822 RID: 6178
		private int startCell;
	}
}
