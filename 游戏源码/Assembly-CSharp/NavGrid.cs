using System;
using System.Collections.Generic;
using HUSL;
using UnityEngine;

// Token: 0x020007D6 RID: 2006
public class NavGrid
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000B74A2 File Offset: 0x000B56A2
	// (set) Token: 0x060023D4 RID: 9172 RVA: 0x000B74AA File Offset: 0x000B56AA
	public NavTable NavTable { get; private set; }

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060023D5 RID: 9173 RVA: 0x000B74B3 File Offset: 0x000B56B3
	// (set) Token: 0x060023D6 RID: 9174 RVA: 0x000B74BB File Offset: 0x000B56BB
	public NavGrid.Transition[] transitions { get; set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000B74C4 File Offset: 0x000B56C4
	// (set) Token: 0x060023D8 RID: 9176 RVA: 0x000B74CC File Offset: 0x000B56CC
	public NavGrid.Transition[][] transitionsByNavType { get; private set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060023D9 RID: 9177 RVA: 0x000B74D5 File Offset: 0x000B56D5
	// (set) Token: 0x060023DA RID: 9178 RVA: 0x000B74DD File Offset: 0x000B56DD
	public int updateRangeX { get; private set; }

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060023DB RID: 9179 RVA: 0x000B74E6 File Offset: 0x000B56E6
	// (set) Token: 0x060023DC RID: 9180 RVA: 0x000B74EE File Offset: 0x000B56EE
	public int updateRangeY { get; private set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060023DD RID: 9181 RVA: 0x000B74F7 File Offset: 0x000B56F7
	// (set) Token: 0x060023DE RID: 9182 RVA: 0x000B74FF File Offset: 0x000B56FF
	public int maxLinksPerCell { get; private set; }

	// Token: 0x060023DF RID: 9183 RVA: 0x000B7508 File Offset: 0x000B5708
	public static NavType MirrorNavType(NavType nav_type)
	{
		if (nav_type == NavType.LeftWall)
		{
			return NavType.RightWall;
		}
		if (nav_type == NavType.RightWall)
		{
			return NavType.LeftWall;
		}
		return nav_type;
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x001C76E4 File Offset: 0x001C58E4
	public NavGrid(string id, NavGrid.Transition[] transitions, NavGrid.NavTypeData[] nav_type_data, CellOffset[] bounding_offsets, NavTableValidator[] validators, int update_range_x, int update_range_y, int max_links_per_cell)
	{
		this.DirtyBitFlags = new byte[(Grid.CellCount + 7) / 8];
		this.DirtyCells = new List<int>();
		this.id = id;
		this.Validators = validators;
		this.navTypeData = nav_type_data;
		this.transitions = transitions;
		this.boundingOffsets = bounding_offsets;
		List<NavType> list = new List<NavType>();
		this.updateRangeX = update_range_x;
		this.updateRangeY = update_range_y;
		this.maxLinksPerCell = max_links_per_cell + 1;
		for (int i = 0; i < transitions.Length; i++)
		{
			DebugUtil.Assert(i >= 0 && i <= 255);
			transitions[i].id = (byte)i;
			if (!list.Contains(transitions[i].start))
			{
				list.Add(transitions[i].start);
			}
			if (!list.Contains(transitions[i].end))
			{
				list.Add(transitions[i].end);
			}
		}
		this.ValidNavTypes = list.ToArray();
		this.DebugViewLinkType = new bool[this.ValidNavTypes.Length];
		this.DebugViewValidCellsType = new bool[this.ValidNavTypes.Length];
		foreach (NavType nav_type in this.ValidNavTypes)
		{
			this.GetNavTypeData(nav_type);
		}
		this.Links = new NavGrid.Link[this.maxLinksPerCell * Grid.CellCount];
		this.NavTable = new NavTable(Grid.CellCount);
		this.transitions = transitions;
		this.transitionsByNavType = new NavGrid.Transition[11][];
		for (int k = 0; k < 11; k++)
		{
			List<NavGrid.Transition> list2 = new List<NavGrid.Transition>();
			NavType navType = (NavType)k;
			foreach (NavGrid.Transition transition in transitions)
			{
				if (transition.start == navType)
				{
					list2.Add(transition);
				}
			}
			this.transitionsByNavType[k] = list2.ToArray();
		}
		foreach (NavTableValidator navTableValidator in validators)
		{
			navTableValidator.onDirty = (Action<int>)Delegate.Combine(navTableValidator.onDirty, new Action<int>(this.AddDirtyCell));
		}
		this.potentialScratchPad = new PathFinder.PotentialScratchPad(this.maxLinksPerCell);
		this.InitializeGraph();
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x001C7930 File Offset: 0x001C5B30
	public NavGrid.NavTypeData GetNavTypeData(NavType nav_type)
	{
		foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
		{
			if (navTypeData.navType == nav_type)
			{
				return navTypeData;
			}
		}
		throw new Exception("Missing nav type data for nav type:" + nav_type.ToString());
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x001C7984 File Offset: 0x001C5B84
	public bool HasNavTypeData(NavType nav_type)
	{
		NavGrid.NavTypeData[] array = this.navTypeData;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].navType == nav_type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000B7517 File Offset: 0x000B5717
	public HashedString GetIdleAnim(NavType nav_type)
	{
		return this.GetNavTypeData(nav_type).idleAnim;
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x000B7525 File Offset: 0x000B5725
	public void InitializeGraph()
	{
		NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x001C79B8 File Offset: 0x001C5BB8
	public void UpdateGraph()
	{
		int count = this.DirtyCells.Count;
		for (int i = 0; i < count; i++)
		{
			int num;
			int num2;
			Grid.CellToXY(this.DirtyCells[i], out num, out num2);
			int num3 = Grid.ClampX(num - this.updateRangeX);
			int num4 = Grid.ClampY(num2 - this.updateRangeY);
			int num5 = Grid.ClampX(num + this.updateRangeX);
			int num6 = Grid.ClampY(num2 + this.updateRangeY);
			for (int j = num4; j <= num6; j++)
			{
				for (int k = num3; k <= num5; k++)
				{
					this.AddDirtyCell(Grid.XYToCell(k, j));
				}
			}
		}
		this.UpdateGraph(this.DirtyCells);
		foreach (int num7 in this.DirtyCells)
		{
			this.DirtyBitFlags[num7 / 8] = 0;
		}
		this.DirtyCells.Clear();
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x001C7AC8 File Offset: 0x001C5CC8
	public void UpdateGraph(IEnumerable<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
		if (this.OnNavGridUpdateComplete != null)
		{
			this.OnNavGridUpdateComplete(dirty_nav_cells);
		}
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000B7550 File Offset: 0x000B5750
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x001C7B1C File Offset: 0x001C5D1C
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGrid.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x001C7B74 File Offset: 0x001C5D74
	private void DebugDrawValidCells()
	{
		Color white = Color.white;
		int cellCount = Grid.CellCount;
		for (int i = 0; i < cellCount; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				NavType nav_type = (NavType)j;
				if (this.NavTable.IsValid(i, nav_type) && this.DrawNavTypeCell(nav_type, ref white))
				{
					DebugExtension.DebugPoint(NavTypeHelper.GetNavPos(i, nav_type), white, 1f, 0f, false);
				}
			}
		}
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x001C7BE0 File Offset: 0x001C5DE0
	private void DebugDrawLinks()
	{
		Color white = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * this.maxLinksPerCell;
			for (int link = this.Links[num].link; link != NavGrid.InvalidCell; link = this.Links[num].link)
			{
				NavTypeHelper.GetNavPos(i, this.Links[num].startNavType);
				if (this.DrawNavTypeLink(this.Links[num].startNavType, ref white) || this.DrawNavTypeLink(this.Links[num].endNavType, ref white))
				{
					NavTypeHelper.GetNavPos(link, this.Links[num].endNavType);
				}
				num++;
			}
		}
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x001C7CB0 File Offset: 0x001C5EB0
	private bool DrawNavTypeLink(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewLinksAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewLinkType[i];
			}
		}
		return false;
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x001C7CFC File Offset: 0x001C5EFC
	private bool DrawNavTypeCell(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewValidCellsAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewValidCellsType[i];
			}
		}
		return false;
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000B7564 File Offset: 0x000B5764
	public void DebugUpdate()
	{
		if (this.DebugViewValidCells)
		{
			this.DebugDrawValidCells();
		}
		if (this.DebugViewLinks)
		{
			this.DebugDrawLinks();
		}
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x001C7D48 File Offset: 0x001C5F48
	public void AddDirtyCell(int cell)
	{
		if (Grid.IsValidCell(cell) && ((int)this.DirtyBitFlags[cell / 8] & 1 << cell % 8) == 0)
		{
			this.DirtyCells.Add(cell);
			byte[] dirtyBitFlags = this.DirtyBitFlags;
			int num = cell / 8;
			dirtyBitFlags[num] |= (byte)(1 << cell % 8);
		}
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x001C7D9C File Offset: 0x001C5F9C
	public void Clear()
	{
		NavTableValidator[] validators = this.Validators;
		for (int i = 0; i < validators.Length; i++)
		{
			validators[i].Clear();
		}
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x001C7DC8 File Offset: 0x001C5FC8
	public Color NavTypeColor(NavType navType)
	{
		if (this.debugColorLookup == null)
		{
			this.debugColorLookup = new Color[11];
			for (int i = 0; i < 11; i++)
			{
				double num = (double)i / 11.0;
				IList<double> list = ColorConverter.HUSLToRGB(new double[]
				{
					num * 360.0,
					100.0,
					50.0
				});
				this.debugColorLookup[i] = new Color((float)list[0], (float)list[1], (float)list[2]);
			}
		}
		return this.debugColorLookup[(int)navType];
	}

	// Token: 0x040017DD RID: 6109
	public bool DebugViewAllPaths;

	// Token: 0x040017DE RID: 6110
	public bool DebugViewValidCells;

	// Token: 0x040017DF RID: 6111
	public bool[] DebugViewValidCellsType;

	// Token: 0x040017E0 RID: 6112
	public bool DebugViewValidCellsAll;

	// Token: 0x040017E1 RID: 6113
	public bool DebugViewLinks;

	// Token: 0x040017E2 RID: 6114
	public bool[] DebugViewLinkType;

	// Token: 0x040017E3 RID: 6115
	public bool DebugViewLinksAll;

	// Token: 0x040017E4 RID: 6116
	public static int InvalidHandle = -1;

	// Token: 0x040017E5 RID: 6117
	public static int InvalidIdx = -1;

	// Token: 0x040017E6 RID: 6118
	public static int InvalidCell = -1;

	// Token: 0x040017E7 RID: 6119
	public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();

	// Token: 0x040017E8 RID: 6120
	public NavGrid.Link[] Links;

	// Token: 0x040017EA RID: 6122
	private byte[] DirtyBitFlags;

	// Token: 0x040017EB RID: 6123
	private List<int> DirtyCells;

	// Token: 0x040017EC RID: 6124
	private NavTableValidator[] Validators = new NavTableValidator[0];

	// Token: 0x040017ED RID: 6125
	private CellOffset[] boundingOffsets;

	// Token: 0x040017EE RID: 6126
	public string id;

	// Token: 0x040017EF RID: 6127
	public bool updateEveryFrame;

	// Token: 0x040017F0 RID: 6128
	public PathFinder.PotentialScratchPad potentialScratchPad;

	// Token: 0x040017F1 RID: 6129
	public Action<IEnumerable<int>> OnNavGridUpdateComplete;

	// Token: 0x040017F4 RID: 6132
	public NavType[] ValidNavTypes;

	// Token: 0x040017F5 RID: 6133
	public NavGrid.NavTypeData[] navTypeData;

	// Token: 0x040017F9 RID: 6137
	private Color[] debugColorLookup;

	// Token: 0x020007D7 RID: 2007
	public struct Link
	{
		// Token: 0x060023F2 RID: 9202 RVA: 0x000B7596 File Offset: 0x000B5796
		public Link(int link, NavType start_nav_type, NavType end_nav_type, byte transition_id, byte cost)
		{
			this.link = link;
			this.startNavType = start_nav_type;
			this.endNavType = end_nav_type;
			this.transitionId = transition_id;
			this.cost = cost;
		}

		// Token: 0x040017FA RID: 6138
		public int link;

		// Token: 0x040017FB RID: 6139
		public NavType startNavType;

		// Token: 0x040017FC RID: 6140
		public NavType endNavType;

		// Token: 0x040017FD RID: 6141
		public byte transitionId;

		// Token: 0x040017FE RID: 6142
		public byte cost;
	}

	// Token: 0x020007D8 RID: 2008
	public struct NavTypeData
	{
		// Token: 0x040017FF RID: 6143
		public NavType navType;

		// Token: 0x04001800 RID: 6144
		public Vector2 animControllerOffset;

		// Token: 0x04001801 RID: 6145
		public bool flipX;

		// Token: 0x04001802 RID: 6146
		public bool flipY;

		// Token: 0x04001803 RID: 6147
		public float rotation;

		// Token: 0x04001804 RID: 6148
		public HashedString idleAnim;
	}

	// Token: 0x020007D9 RID: 2009
	public struct Transition
	{
		// Token: 0x060023F3 RID: 9203 RVA: 0x001C7E70 File Offset: 0x001C6070
		public override string ToString()
		{
			return string.Format("{0}: {1}->{2} ({3}); offset {4},{5}", new object[]
			{
				this.id,
				this.start,
				this.end,
				this.startAxis,
				this.x,
				this.y
			});
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x001C7EE4 File Offset: 0x001C60E4
		public Transition(NavType start, NavType end, int x, int y, NavAxis start_axis, bool is_looping, bool loop_has_pre, bool is_escape, int cost, string anim, CellOffset[] void_offsets, CellOffset[] solid_offsets, NavOffset[] valid_nav_offsets, NavOffset[] invalid_nav_offsets, bool critter = false, float animSpeed = 1f)
		{
			DebugUtil.Assert(cost <= 255 && cost >= 0);
			this.id = byte.MaxValue;
			this.start = start;
			this.end = end;
			this.x = x;
			this.y = y;
			this.startAxis = start_axis;
			this.isLooping = is_looping;
			this.isEscape = is_escape;
			this.anim = anim;
			this.preAnim = "";
			this.cost = (byte)cost;
			if (string.IsNullOrEmpty(this.anim))
			{
				this.anim = string.Concat(new string[]
				{
					start.ToString().ToLower(),
					"_",
					end.ToString().ToLower(),
					"_",
					x.ToString(),
					"_",
					y.ToString()
				});
			}
			if (this.isLooping)
			{
				if (loop_has_pre)
				{
					this.preAnim = this.anim + "_pre";
				}
				this.anim += "_loop";
			}
			if (this.startAxis != NavAxis.NA)
			{
				this.anim += ((this.startAxis == NavAxis.X) ? "_x" : "_y");
			}
			this.voidOffsets = void_offsets;
			this.solidOffsets = solid_offsets;
			this.validNavOffsets = valid_nav_offsets;
			this.invalidNavOffsets = invalid_nav_offsets;
			this.isCritter = critter;
			this.animSpeed = animSpeed;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x001C8070 File Offset: 0x001C6270
		public int IsValid(int cell, NavTable nav_table)
		{
			if (!Grid.IsCellOffsetValid(cell, this.x, this.y))
			{
				return Grid.InvalidCell;
			}
			int num = Grid.OffsetCell(cell, this.x, this.y);
			if (!nav_table.IsValid(num, this.end))
			{
				return Grid.InvalidCell;
			}
			Grid.BuildFlags buildFlags = Grid.BuildFlags.Solid | Grid.BuildFlags.DupeImpassable;
			if (this.isCritter)
			{
				buildFlags |= Grid.BuildFlags.CritterImpassable;
			}
			foreach (CellOffset cellOffset in this.voidOffsets)
			{
				int num2 = Grid.OffsetCell(cell, cellOffset.x, cellOffset.y);
				if (Grid.IsValidCell(num2) && (Grid.BuildMasks[num2] & buildFlags) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
				{
					if (this.isCritter)
					{
						return Grid.InvalidCell;
					}
					if ((Grid.BuildMasks[num2] & Grid.BuildFlags.DupePassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
					{
						return Grid.InvalidCell;
					}
				}
			}
			foreach (CellOffset cellOffset2 in this.solidOffsets)
			{
				int num3 = Grid.OffsetCell(cell, cellOffset2.x, cellOffset2.y);
				if (Grid.IsValidCell(num3) && !Grid.Solid[num3])
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset in this.validNavOffsets)
			{
				int cell2 = Grid.OffsetCell(cell, navOffset.offset.x, navOffset.offset.y);
				if (!nav_table.IsValid(cell2, navOffset.navType))
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset2 in this.invalidNavOffsets)
			{
				int cell3 = Grid.OffsetCell(cell, navOffset2.offset.x, navOffset2.offset.y);
				if (nav_table.IsValid(cell3, navOffset2.navType))
				{
					return Grid.InvalidCell;
				}
			}
			if (this.start == NavType.Tube)
			{
				if (this.end == NavType.Tube)
				{
					GameObject gameObject = Grid.Objects[cell, 9];
					GameObject gameObject2 = Grid.Objects[num, 9];
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = gameObject ? gameObject.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink2 = gameObject2 ? gameObject2.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					if (travelTubeUtilityNetworkLink)
					{
						int num4;
						int num5;
						travelTubeUtilityNetworkLink.GetCells(out num4, out num5);
						if (num != num4 && num != num5)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, num);
						if (utilityConnections == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(num, false) != utilityConnections)
						{
							return Grid.InvalidCell;
						}
					}
					else if (travelTubeUtilityNetworkLink2)
					{
						int num6;
						int num7;
						travelTubeUtilityNetworkLink2.GetCells(out num6, out num7);
						if (cell != num6 && cell != num7)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections2 = UtilityConnectionsExtensions.DirectionFromToCell(num, cell);
						if (utilityConnections2 == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(cell, false) != utilityConnections2)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						bool flag = this.startAxis == NavAxis.X;
						int cell4 = cell;
						for (int j = 0; j < 2; j++)
						{
							if ((flag && j == 0) || (!flag && j == 1))
							{
								int num8 = (this.x > 0) ? 1 : -1;
								for (int k = 0; k < Mathf.Abs(this.x); k++)
								{
									UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num8 > 0 && (connections & UtilityConnections.Right) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num8 < 0 && (connections & UtilityConnections.Left) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, num8, 0);
								}
							}
							else
							{
								int num9 = (this.y > 0) ? 1 : -1;
								for (int l = 0; l < Mathf.Abs(this.y); l++)
								{
									UtilityConnections connections2 = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num9 > 0 && (connections2 & UtilityConnections.Up) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num9 < 0 && (connections2 & UtilityConnections.Down) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, 0, num9);
								}
							}
						}
					}
				}
				else
				{
					UtilityConnections connections3 = Game.Instance.travelTubeSystem.GetConnections(cell, false);
					if (this.y > 0)
					{
						if (connections3 != UtilityConnections.Down)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x > 0)
					{
						if (connections3 != UtilityConnections.Left)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x < 0)
					{
						if (connections3 != UtilityConnections.Right)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						if (this.y >= 0)
						{
							return Grid.InvalidCell;
						}
						if (connections3 != UtilityConnections.Up)
						{
							return Grid.InvalidCell;
						}
					}
				}
			}
			else if (this.start == NavType.Floor && this.end == NavType.Tube)
			{
				int cell5 = Grid.OffsetCell(cell, this.x, this.y);
				if (Game.Instance.travelTubeSystem.GetConnections(cell5, false) != UtilityConnections.Up)
				{
					return Grid.InvalidCell;
				}
			}
			return num;
		}

		// Token: 0x04001805 RID: 6149
		public NavType start;

		// Token: 0x04001806 RID: 6150
		public NavType end;

		// Token: 0x04001807 RID: 6151
		public NavAxis startAxis;

		// Token: 0x04001808 RID: 6152
		public int x;

		// Token: 0x04001809 RID: 6153
		public int y;

		// Token: 0x0400180A RID: 6154
		public byte id;

		// Token: 0x0400180B RID: 6155
		public byte cost;

		// Token: 0x0400180C RID: 6156
		public bool isLooping;

		// Token: 0x0400180D RID: 6157
		public bool isEscape;

		// Token: 0x0400180E RID: 6158
		public string preAnim;

		// Token: 0x0400180F RID: 6159
		public string anim;

		// Token: 0x04001810 RID: 6160
		public float animSpeed;

		// Token: 0x04001811 RID: 6161
		public CellOffset[] voidOffsets;

		// Token: 0x04001812 RID: 6162
		public CellOffset[] solidOffsets;

		// Token: 0x04001813 RID: 6163
		public NavOffset[] validNavOffsets;

		// Token: 0x04001814 RID: 6164
		public NavOffset[] invalidNavOffsets;

		// Token: 0x04001815 RID: 6165
		public bool isCritter;
	}
}
