﻿using System;
using System.Collections.Generic;
using HUSL;
using UnityEngine;

public class NavGrid
{
			public NavTable NavTable { get; private set; }

			public NavGrid.Transition[] transitions { get; set; }

			public NavGrid.Transition[][] transitionsByNavType { get; private set; }

			public int updateRangeX { get; private set; }

			public int updateRangeY { get; private set; }

			public int maxLinksPerCell { get; private set; }

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

	public NavGrid(string id, NavGrid.Transition[] transitions, NavGrid.NavTypeData[] nav_type_data, CellOffset[] bounding_offsets, NavTableValidator[] validators, int update_range_x, int update_range_y, int max_links_per_cell)
	{
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

	public HashedString GetIdleAnim(NavType nav_type)
	{
		return this.GetNavTypeData(nav_type).idleAnim;
	}

	public void InitializeGraph()
	{
		NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
	}

	public void UpdateGraph()
	{
		foreach (int cell in this.DirtyCells)
		{
			for (int i = -this.updateRangeY; i <= this.updateRangeY; i++)
			{
				for (int j = -this.updateRangeX; j <= this.updateRangeX; j++)
				{
					int num = Grid.OffsetCell(cell, j, i);
					if (Grid.IsValidCell(num))
					{
						this.ExpandedDirtyCells.Add(num);
					}
				}
			}
		}
		this.UpdateGraph(this.ExpandedDirtyCells);
		this.DirtyCells = new HashSet<int>();
		this.ExpandedDirtyCells = new HashSet<int>();
	}

	public void UpdateGraph(HashSet<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
		if (this.OnNavGridUpdateComplete != null)
		{
			this.OnNavGridUpdateComplete(dirty_nav_cells);
		}
	}

	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

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

	public void AddDirtyCell(int cell)
	{
		this.DirtyCells.Add(cell);
	}

	public void Clear()
	{
		NavTableValidator[] validators = this.Validators;
		for (int i = 0; i < validators.Length; i++)
		{
			validators[i].Clear();
		}
	}

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

	public bool DebugViewAllPaths;

	public bool DebugViewValidCells;

	public bool[] DebugViewValidCellsType;

	public bool DebugViewValidCellsAll;

	public bool DebugViewLinks;

	public bool[] DebugViewLinkType;

	public bool DebugViewLinksAll;

	public static int InvalidHandle = -1;

	public static int InvalidIdx = -1;

	public static int InvalidCell = -1;

	public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();

	public NavGrid.Link[] Links;

	private HashSet<int> DirtyCells = new HashSet<int>();

	private HashSet<int> ExpandedDirtyCells = new HashSet<int>();

	private NavTableValidator[] Validators = new NavTableValidator[0];

	private CellOffset[] boundingOffsets;

	public string id;

	public bool updateEveryFrame;

	public PathFinder.PotentialScratchPad potentialScratchPad;

	public Action<HashSet<int>> OnNavGridUpdateComplete;

	public NavType[] ValidNavTypes;

	public NavGrid.NavTypeData[] navTypeData;

	private Color[] debugColorLookup;

	public struct Link
	{
		public Link(int link, NavType start_nav_type, NavType end_nav_type, byte transition_id, byte cost)
		{
			this.link = link;
			this.startNavType = start_nav_type;
			this.endNavType = end_nav_type;
			this.transitionId = transition_id;
			this.cost = cost;
		}

		public int link;

		public NavType startNavType;

		public NavType endNavType;

		public byte transitionId;

		public byte cost;
	}

	public struct NavTypeData
	{
		public NavType navType;

		public Vector2 animControllerOffset;

		public bool flipX;

		public bool flipY;

		public float rotation;

		public HashedString idleAnim;
	}

	public struct Transition
	{
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

		public NavType start;

		public NavType end;

		public NavAxis startAxis;

		public int x;

		public int y;

		public byte id;

		public byte cost;

		public bool isLooping;

		public bool isEscape;

		public string preAnim;

		public string anim;

		public float animSpeed;

		public CellOffset[] voidOffsets;

		public CellOffset[] solidOffsets;

		public NavOffset[] validNavOffsets;

		public NavOffset[] invalidNavOffsets;

		public bool isCritter;
	}
}
