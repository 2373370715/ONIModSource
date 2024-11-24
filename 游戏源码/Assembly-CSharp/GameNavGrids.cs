using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001327 RID: 4903
public class GameNavGrids
{
	// Token: 0x060064A9 RID: 25769 RVA: 0x002C0F50 File Offset: 0x002BF150
	public GameNavGrids(Pathfinding pathfinding)
	{
		this.CreateDuplicantNavigation(pathfinding);
		this.WalkerGrid1x1 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerBabyGrid1x1 = this.CreateWalkerBabyNavigation(pathfinding, "WalkerBabyNavGrid", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerGrid1x2 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.WalkerGrid2x2 = this.CreateWalkerLargeNavigation(pathfinding, "WalkerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.CreateDreckoNavigation(pathfinding);
		this.CreateDreckoBabyNavigation(pathfinding);
		this.CreateFloaterNavigation(pathfinding);
		this.FlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.FlyerGrid1x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.FlyerGrid2x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(1, 1)
		});
		this.CreateSwimmerNavigation(pathfinding);
		this.CreateDiggerNavigation(pathfinding);
		this.CreateSquirrelNavigation(pathfinding);
	}

	// Token: 0x060064AA RID: 25770 RVA: 0x002C10F4 File Offset: 0x002BF2F4
	private void CreateDuplicantNavigation(Pathfinding pathfinding)
	{
		NavOffset[] invalid_nav_offsets = new NavOffset[]
		{
			new NavOffset(NavType.Floor, 1, 0),
			new NavOffset(NavType.Ladder, 1, 0),
			new NavOffset(NavType.Pole, 1, 0)
		};
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		};
		NavGrid.Transition[] setA = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Teleport, 0, 0, NavAxis.NA, false, false, false, 14, "fall_pre", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Teleport, NavType.Floor, 0, 0, NavAxis.NA, false, false, false, 1, "fall_pst", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Ladder, 0, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, 1, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, -1, NavAxis.NA, true, true, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 25, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Pole, 0, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, 1, NavAxis.NA, true, true, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, -1, NavAxis.NA, true, true, false, 6, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Tube, 0, 2, NavAxis.NA, false, false, false, 40, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 0, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, 1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, -1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, 1)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, -1)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, false, 25, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, 1)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, false, 25, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, -1)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 0, 1, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] setB = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f)
		};
		NavGrid.Transition[] transitions = this.MirrorTransitions(this.CombineTransitions(setA, setB));
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_default"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ladder,
				idleAnim = "ladder_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Pole,
				idleAnim = "pole_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Tube,
				idleAnim = "tube_idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "hover_hover_1_0_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Teleport,
				idleAnim = "idle_default"
			}
		};
		this.DuplicantGrid = new NavGrid("MinionNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator(),
			new GameNavGrids.PoleValidator(),
			new GameNavGrids.TubeValidator(),
			new GameNavGrids.TeleporterValidator(),
			new GameNavGrids.FlyingValidator(true, true, true)
		}, 2, 3, 32);
		this.DuplicantGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.DuplicantGrid);
		NavGrid.Transition[] setB2 = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "climb_up_2_1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(this.CombineTransitions(setA, setB2));
		this.RobotGrid = new NavGrid("RobotNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator()
		}, 2, 3, 22);
		this.RobotGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.RobotGrid);
	}

	// Token: 0x060064AB RID: 25771 RVA: 0x002C3678 File Offset: 0x002C1878
	private NavGrid CreateWalkerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060064AC RID: 25772 RVA: 0x002C38AC File Offset: 0x002C1AAC
	private NavGrid CreateWalkerBabyNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060064AD RID: 25773 RVA: 0x002C3958 File Offset: 0x002C1B58
	private NavGrid CreateWalkerLargeNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(2, 1),
				new CellOffset(0, 2)
			}, new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0)
			}, new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(2, -1),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(2, -2)
			}, new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060064AE RID: 25774 RVA: 0x002C3AF4 File Offset: 0x002C1CF4
	private void CreateDreckoNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 2, NavAxis.NA, false, false, true, 5, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 2)
			}, new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0),
				new NavOffset(NavType.Floor, 2, 2)
			}, true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, -1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, -1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -2, -1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, -1),
				new CellOffset(-1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, -1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -2, -1)
			}, true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(-1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 2)
			}, true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 2, 1)
			}, true, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoGrid = new NavGrid("DreckoNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoGrid);
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x002C4444 File Offset: 0x002C2644
	private void CreateDreckoBabyNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoBabyGrid = new NavGrid("DreckoBabyNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoBabyGrid);
	}

	// Token: 0x060064B0 RID: 25776 RVA: 0x002C48C8 File Offset: 0x002C2AC8
	private void CreateFloaterNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, 0)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -1)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, -1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 1)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -3)
			}, true, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "swim_idle_loop"
			}
		};
		this.FloaterGrid = new NavGrid("FloaterNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.HoverValidator(),
			new GameNavGrids.SwimValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.FloaterGrid);
	}

	// Token: 0x060064B1 RID: 25777 RVA: 0x002C4EF8 File Offset: 0x002C30F8
	private NavGrid CreateFlyerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FlyingValidator(false, false, false),
			new GameNavGrids.SwimValidator()
		}, 2, 2, 16);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060064B2 RID: 25778 RVA: 0x002C5298 File Offset: 0x002C3498
	private void CreateSwimmerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		this.SwimmerGrid = new NavGrid("SwimmerNavGrid", array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SwimValidator()
		}, 1, 2, array.Length);
		pathfinding.AddNavGrid(this.SwimmerGrid);
	}

	// Token: 0x060064B3 RID: 25779 RVA: 0x002C547C File Offset: 0x002C367C
	private void CreateDiggerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 0, NavAxis.NA, false, false, true, 1, "idle1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 1, NavAxis.NA, false, false, true, 1, "idle2", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 0, 1, NavAxis.NA, false, false, true, 1, "idle3", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, -1, NavAxis.NA, false, false, true, 1, "idle4", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Solid, 0, -1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 1, "drill_out", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Solid, 0, 1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Ceiling, 0, -1, NavAxis.NA, false, false, true, 1, "drill_out_ceiling", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.LeftWall, 1, 0, NavAxis.NA, false, false, true, 1, "drill_out_left_wall", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Solid, -1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.RightWall, -1, 0, NavAxis.NA, false, false, true, 1, "drill_out_right_wall", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Solid, 1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Solid,
				idleAnim = "idle1"
			}
		};
		this.DiggerGrid = new NavGrid("DiggerNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SolidValidator(),
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.DiggerGrid);
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x002C5BFC File Offset: 0x002C3DFC
	private void CreateSquirrelNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.SquirrelGrid = new NavGrid("SquirrelNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 20);
		pathfinding.AddNavGrid(this.SquirrelGrid);
	}

	// Token: 0x060064B5 RID: 25781 RVA: 0x002C620C File Offset: 0x002C440C
	private CellOffset[] MirrorOffsets(CellOffset[] offsets)
	{
		List<CellOffset> list = new List<CellOffset>();
		foreach (CellOffset cellOffset in offsets)
		{
			cellOffset.x = -cellOffset.x;
			list.Add(cellOffset);
		}
		return list.ToArray();
	}

	// Token: 0x060064B6 RID: 25782 RVA: 0x002C6254 File Offset: 0x002C4454
	private NavOffset[] MirrorNavOffsets(NavOffset[] offsets)
	{
		List<NavOffset> list = new List<NavOffset>();
		foreach (NavOffset navOffset in offsets)
		{
			navOffset.navType = NavGrid.MirrorNavType(navOffset.navType);
			navOffset.offset.x = -navOffset.offset.x;
			list.Add(navOffset);
		}
		return list.ToArray();
	}

	// Token: 0x060064B7 RID: 25783 RVA: 0x002C62B8 File Offset: 0x002C44B8
	private NavGrid.Transition[] MirrorTransitions(NavGrid.Transition[] transitions)
	{
		List<NavGrid.Transition> list = new List<NavGrid.Transition>();
		foreach (NavGrid.Transition transition in transitions)
		{
			list.Add(transition);
			if (transition.x != 0 || transition.start == NavType.RightWall || transition.end == NavType.RightWall || transition.start == NavType.LeftWall || transition.end == NavType.LeftWall)
			{
				NavGrid.Transition transition2 = transition;
				transition2.x = -transition2.x;
				transition2.voidOffsets = this.MirrorOffsets(transition.voidOffsets);
				transition2.solidOffsets = this.MirrorOffsets(transition.solidOffsets);
				transition2.validNavOffsets = this.MirrorNavOffsets(transition.validNavOffsets);
				transition2.invalidNavOffsets = this.MirrorNavOffsets(transition.invalidNavOffsets);
				transition2.start = NavGrid.MirrorNavType(transition2.start);
				transition2.end = NavGrid.MirrorNavType(transition2.end);
				list.Add(transition2);
			}
		}
		list.Sort((NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return list.ToArray();
	}

	// Token: 0x060064B8 RID: 25784 RVA: 0x002C63D8 File Offset: 0x002C45D8
	private NavGrid.Transition[] CombineTransitions(NavGrid.Transition[] setA, NavGrid.Transition[] setB)
	{
		NavGrid.Transition[] array = new NavGrid.Transition[setA.Length + setB.Length];
		Array.Copy(setA, array, setA.Length);
		Array.Copy(setB, 0, array, setA.Length, setB.Length);
		Array.Sort<NavGrid.Transition>(array, (NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return array;
	}

	// Token: 0x04004A3E RID: 19006
	public NavGrid DuplicantGrid;

	// Token: 0x04004A3F RID: 19007
	public NavGrid WalkerGrid1x1;

	// Token: 0x04004A40 RID: 19008
	public NavGrid WalkerBabyGrid1x1;

	// Token: 0x04004A41 RID: 19009
	public NavGrid WalkerGrid1x2;

	// Token: 0x04004A42 RID: 19010
	public NavGrid WalkerGrid2x2;

	// Token: 0x04004A43 RID: 19011
	public NavGrid DreckoGrid;

	// Token: 0x04004A44 RID: 19012
	public NavGrid DreckoBabyGrid;

	// Token: 0x04004A45 RID: 19013
	public NavGrid FloaterGrid;

	// Token: 0x04004A46 RID: 19014
	public NavGrid FlyerGrid1x2;

	// Token: 0x04004A47 RID: 19015
	public NavGrid FlyerGrid1x1;

	// Token: 0x04004A48 RID: 19016
	public NavGrid FlyerGrid2x2;

	// Token: 0x04004A49 RID: 19017
	public NavGrid SwimmerGrid;

	// Token: 0x04004A4A RID: 19018
	public NavGrid DiggerGrid;

	// Token: 0x04004A4B RID: 19019
	public NavGrid SquirrelGrid;

	// Token: 0x04004A4C RID: 19020
	public NavGrid RobotGrid;

	// Token: 0x02001328 RID: 4904
	public class SwimValidator : NavTableValidator
	{
		// Token: 0x060064B9 RID: 25785 RVA: 0x002C6430 File Offset: 0x002C4630
		public SwimValidator()
		{
			World instance = World.Instance;
			instance.OnLiquidChanged = (Action<int>)Delegate.Combine(instance.OnLiquidChanged, new Action<int>(this.OnLiquidChanged));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new Action<int, object>(this.OnFoundationTileChanged));
		}

		// Token: 0x060064BA RID: 25786 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void OnFoundationTileChanged(int cell, object unused)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064BB RID: 25787 RVA: 0x002C648C File Offset: 0x002C468C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (!flag)
			{
				flag = Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f);
			}
			bool is_valid = Grid.IsWorldValidCell(cell) && flag && base.IsClear(cell, bounding_offsets, false);
			nav_table.SetValid(cell, NavType.Swim, is_valid);
		}

		// Token: 0x060064BC RID: 25788 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void OnLiquidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}
	}

	// Token: 0x02001329 RID: 4905
	public class FloorValidator : NavTableValidator
	{
		// Token: 0x060064BD RID: 25789 RVA: 0x002C64DC File Offset: 0x002C46DC
		public FloorValidator(bool is_dupe)
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
			this.isDupe = is_dupe;
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x002C6540 File Offset: 0x002C4740
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), this.isDupe);
			nav_table.SetValid(cell, NavType.Floor, flag && base.IsClear(cell, bounding_offsets, this.isDupe));
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x002C657C File Offset: 0x002C477C
		public static bool IsWalkableCell(int cell, int anchor_cell, bool is_dupe)
		{
			if (!Grid.IsWorldValidCell(cell))
			{
				return false;
			}
			if (!Grid.IsWorldValidCell(anchor_cell))
			{
				return false;
			}
			if (!NavTableValidator.IsCellPassable(cell, is_dupe))
			{
				return false;
			}
			if (Grid.FakeFloor[anchor_cell])
			{
				return true;
			}
			if (Grid.Solid[anchor_cell])
			{
				return !Grid.DupePassable[anchor_cell];
			}
			return is_dupe && (Grid.NavValidatorMasks[cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) == (Grid.NavValidatorFlags)0 && (Grid.NavValidatorMasks[anchor_cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) > (Grid.NavValidatorFlags)0;
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x002C65F4 File Offset: 0x002C47F4
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x002C65F4 File Offset: 0x002C47F4
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064C2 RID: 25794 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064C3 RID: 25795 RVA: 0x002C661C File Offset: 0x002C481C
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x04004A4D RID: 19021
		private bool isDupe;
	}

	// Token: 0x0200132A RID: 4906
	public class WallValidator : NavTableValidator
	{
		// Token: 0x060064C4 RID: 25796 RVA: 0x000E1B2A File Offset: 0x000DFD2A
		public WallValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x002C6674 File Offset: 0x002C4874
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellRight(cell));
			bool flag2 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellLeft(cell));
			nav_table.SetValid(cell, NavType.RightWall, flag && base.IsClear(cell, bounding_offsets, false));
			nav_table.SetValid(cell, NavType.LeftWall, flag2 && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x060064C6 RID: 25798 RVA: 0x000E1B58 File Offset: 0x000DFD58
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.CritterImpassable[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x000E1B94 File Offset: 0x000DFD94
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200132B RID: 4907
	public class CeilingValidator : NavTableValidator
	{
		// Token: 0x060064C9 RID: 25801 RVA: 0x000E1BBC File Offset: 0x000DFDBC
		public CeilingValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x002C66CC File Offset: 0x002C48CC
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.CeilingValidator.IsWalkableCell(cell, Grid.CellAbove(cell));
			nav_table.SetValid(cell, NavType.Ceiling, flag && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x002C6700 File Offset: 0x002C4900
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[cell] && !Grid.FakeFloor[cell])
				{
					return false;
				}
				if (Grid.FakeFloor[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x000E1BEA File Offset: 0x000DFDEA
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200132C RID: 4908
	public class LadderValidator : NavTableValidator
	{
		// Token: 0x060064CE RID: 25806 RVA: 0x000E1C12 File Offset: 0x000DFE12
		public LadderValidator()
		{
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x002C65F4 File Offset: 0x002C47F4
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x002C65F4 File Offset: 0x002C47F4
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x000E1C3C File Offset: 0x000DFE3C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Ladder, base.IsClear(cell, bounding_offsets, true) && Grid.HasLadder[cell]);
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x000E1C5F File Offset: 0x000DFE5F
		public override void Clear()
		{
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}
	}

	// Token: 0x0200132D RID: 4909
	public class PoleValidator : GameNavGrids.LadderValidator
	{
		// Token: 0x060064D3 RID: 25811 RVA: 0x000E1C83 File Offset: 0x000DFE83
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Pole, base.IsClear(cell, bounding_offsets, true) && Grid.HasPole[cell]);
		}
	}

	// Token: 0x0200132E RID: 4910
	public class TubeValidator : NavTableValidator
	{
		// Token: 0x060064D5 RID: 25813 RVA: 0x000E1CAE File Offset: 0x000DFEAE
		public TubeValidator()
		{
			Components.ITravelTubePieces.Register(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x002C6774 File Offset: 0x002C4974
		private void OnAddLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x002C6774 File Offset: 0x002C4974
		private void OnRemoveLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x000E1CD8 File Offset: 0x000DFED8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Tube, Grid.HasTube[cell]);
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x000E1CED File Offset: 0x000DFEED
		public override void Clear()
		{
			Components.ITravelTubePieces.Unregister(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}
	}

	// Token: 0x0200132F RID: 4911
	public class TeleporterValidator : NavTableValidator
	{
		// Token: 0x060064DA RID: 25818 RVA: 0x000E1D11 File Offset: 0x000DFF11
		public TeleporterValidator()
		{
			Components.NavTeleporters.Register(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x002C65F4 File Offset: 0x002C47F4
		private void OnAddTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x002C65F4 File Offset: 0x002C47F4
		private void OnRemoveTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x002C67A4 File Offset: 0x002C49A4
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = Grid.IsWorldValidCell(cell) && Grid.HasNavTeleporter[cell];
			nav_table.SetValid(cell, NavType.Teleport, is_valid);
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x000E1D3B File Offset: 0x000DFF3B
		public override void Clear()
		{
			Components.NavTeleporters.Unregister(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}
	}

	// Token: 0x02001330 RID: 4912
	public class FlyingValidator : NavTableValidator
	{
		// Token: 0x060064DF RID: 25823 RVA: 0x002C67D4 File Offset: 0x002C49D4
		public FlyingValidator(bool exclude_floor = false, bool exclude_jet_suit_blockers = false, bool allow_door_traversal = false)
		{
			this.exclude_floor = exclude_floor;
			this.exclude_jet_suit_blockers = exclude_jet_suit_blockers;
			this.allow_door_traversal = allow_door_traversal;
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x002C686C File Offset: 0x002C4A6C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = false;
			if (Grid.IsWorldValidCell(Grid.CellAbove(cell)))
			{
				flag = (!Grid.IsSubstantialLiquid(cell, 0.35f) && base.IsClear(cell, bounding_offsets, this.allow_door_traversal));
				if (flag && this.exclude_floor)
				{
					int cell2 = Grid.CellBelow(cell);
					if (Grid.IsWorldValidCell(cell2))
					{
						flag = base.IsClear(cell2, bounding_offsets, this.allow_door_traversal);
					}
				}
				if (flag && this.exclude_jet_suit_blockers)
				{
					GameObject gameObject = Grid.Objects[cell, 1];
					flag = (gameObject == null || !gameObject.HasTag(GameTags.JetSuitBlocker));
				}
			}
			nav_table.SetValid(cell, NavType.Hover, flag);
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x000E1D5F File Offset: 0x000DFF5F
		private void OnBuildingChange(int cell, object data)
		{
			this.MarkCellDirty(cell);
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x002C690C File Offset: 0x002C4B0C
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x04004A4E RID: 19022
		private bool exclude_floor;

		// Token: 0x04004A4F RID: 19023
		private bool exclude_jet_suit_blockers;

		// Token: 0x04004A50 RID: 19024
		private bool allow_door_traversal;

		// Token: 0x04004A51 RID: 19025
		private HandleVector<int>.Handle buildingParititonerEntry;
	}

	// Token: 0x02001331 RID: 4913
	public class HoverValidator : NavTableValidator
	{
		// Token: 0x060064E4 RID: 25828 RVA: 0x002C6988 File Offset: 0x002C4B88
		public HoverValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x002C69E8 File Offset: 0x002C4BE8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			int num = Grid.CellBelow(cell);
			if (Grid.IsWorldValidCell(num))
			{
				bool flag = Grid.Solid[num] || Grid.FakeFloor[num] || Grid.IsSubstantialLiquid(num, 0.35f);
				nav_table.SetValid(cell, NavType.Hover, !Grid.IsSubstantialLiquid(cell, 0.35f) && flag && base.IsClear(cell, bounding_offsets, false));
			}
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x002C6A54 File Offset: 0x002C4C54
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}
	}

	// Token: 0x02001332 RID: 4914
	public class SolidValidator : NavTableValidator
	{
		// Token: 0x060064E8 RID: 25832 RVA: 0x000E1D68 File Offset: 0x000DFF68
		public SolidValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x002C6AB0 File Offset: 0x002C4CB0
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = GameNavGrids.SolidValidator.IsDiggable(cell, Grid.CellBelow(cell));
			nav_table.SetValid(cell, NavType.Solid, is_valid);
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x002C6AD4 File Offset: 0x002C4CD4
		public static bool IsDiggable(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x000E1B14 File Offset: 0x000DFD14
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x000E1D96 File Offset: 0x000DFF96
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}
}
