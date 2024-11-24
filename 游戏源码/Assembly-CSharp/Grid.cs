using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProcGen;
using UnityEngine;

// Token: 0x020013A0 RID: 5024
public class Grid
{
	// Token: 0x0600673D RID: 26429 RVA: 0x000E383E File Offset: 0x000E1A3E
	private static void UpdateBuildMask(int i, Grid.BuildFlags flag, bool state)
	{
		if (state)
		{
			Grid.BuildMasks[i] |= flag;
			return;
		}
		Grid.BuildMasks[i] &= ~flag;
	}

	// Token: 0x0600673E RID: 26430 RVA: 0x000E3866 File Offset: 0x000E1A66
	public static void SetSolid(int cell, bool solid, CellSolidEvent ev)
	{
		Grid.UpdateBuildMask(cell, Grid.BuildFlags.Solid, solid);
	}

	// Token: 0x0600673F RID: 26431 RVA: 0x000E3872 File Offset: 0x000E1A72
	private static void UpdateVisMask(int i, Grid.VisFlags flag, bool state)
	{
		if (state)
		{
			Grid.VisMasks[i] |= flag;
			return;
		}
		Grid.VisMasks[i] &= ~flag;
	}

	// Token: 0x06006740 RID: 26432 RVA: 0x000E389A File Offset: 0x000E1A9A
	private static void UpdateNavValidatorMask(int i, Grid.NavValidatorFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavValidatorMasks[i] |= flag;
			return;
		}
		Grid.NavValidatorMasks[i] &= ~flag;
	}

	// Token: 0x06006741 RID: 26433 RVA: 0x000E38C2 File Offset: 0x000E1AC2
	private static void UpdateNavMask(int i, Grid.NavFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavMasks[i] |= flag;
			return;
		}
		Grid.NavMasks[i] &= ~flag;
	}

	// Token: 0x06006742 RID: 26434 RVA: 0x000E38EA File Offset: 0x000E1AEA
	public static void ResetNavMasksAndDetails()
	{
		Grid.NavMasks = null;
		Grid.tubeEntrances.Clear();
		Grid.restrictions.Clear();
		Grid.suitMarkers.Clear();
	}

	// Token: 0x06006743 RID: 26435 RVA: 0x000E3910 File Offset: 0x000E1B10
	public static bool DEBUG_GetRestrictions(int cell, out Grid.Restriction restriction)
	{
		return Grid.restrictions.TryGetValue(cell, out restriction);
	}

	// Token: 0x06006744 RID: 26436 RVA: 0x002D4180 File Offset: 0x002D2380
	public static void RegisterRestriction(int cell, Grid.Restriction.Orientation orientation)
	{
		Grid.HasAccessDoor[cell] = true;
		Grid.restrictions[cell] = new Grid.Restriction
		{
			DirectionMasksForMinionInstanceID = new Dictionary<int, Grid.Restriction.Directions>(),
			orientation = orientation
		};
	}

	// Token: 0x06006745 RID: 26437 RVA: 0x000E391E File Offset: 0x000E1B1E
	public static void UnregisterRestriction(int cell)
	{
		Grid.restrictions.Remove(cell);
		Grid.HasAccessDoor[cell] = false;
	}

	// Token: 0x06006746 RID: 26438 RVA: 0x000E3938 File Offset: 0x000E1B38
	public static void SetRestriction(int cell, int minionInstanceID, Grid.Restriction.Directions directions)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID[minionInstanceID] = directions;
	}

	// Token: 0x06006747 RID: 26439 RVA: 0x000E3951 File Offset: 0x000E1B51
	public static void ClearRestriction(int cell, int minionInstanceID)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID.Remove(minionInstanceID);
	}

	// Token: 0x06006748 RID: 26440 RVA: 0x002D41C4 File Offset: 0x002D23C4
	public static bool HasPermission(int cell, int minionInstanceID, int fromCell, NavType fromNavType)
	{
		if (!Grid.HasAccessDoor[cell])
		{
			return true;
		}
		Grid.Restriction restriction = Grid.restrictions[cell];
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = Grid.CellToXY(fromCell);
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		int num = vector2I.x - vector2I2.x;
		int num2 = vector2I.y - vector2I2.y;
		switch (restriction.orientation)
		{
		case Grid.Restriction.Orientation.Vertical:
			if (num < 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num > 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.Horizontal:
			if (num2 > 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num2 < 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.SingleCell:
			if (Math.Abs(num) != 1 && Math.Abs(num2) != 1 && fromNavType != NavType.Teleport)
			{
				directions |= Grid.Restriction.Directions.Teleport;
			}
			break;
		}
		Grid.Restriction.Directions directions2 = (Grid.Restriction.Directions)0;
		return (!restriction.DirectionMasksForMinionInstanceID.TryGetValue(minionInstanceID, out directions2) && !restriction.DirectionMasksForMinionInstanceID.TryGetValue(-1, out directions2)) || (directions2 & directions) == (Grid.Restriction.Directions)0;
	}

	// Token: 0x06006749 RID: 26441 RVA: 0x002D42A4 File Offset: 0x002D24A4
	public static void RegisterTubeEntrance(int cell, int reservationCapacity)
	{
		DebugUtil.Assert(!Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = true;
		Grid.tubeEntrances[cell] = new Grid.TubeEntrance
		{
			reservationCapacity = reservationCapacity,
			reservedInstanceIDs = new HashSet<int>()
		};
	}

	// Token: 0x0600674A RID: 26442 RVA: 0x000E396A File Offset: 0x000E1B6A
	public static void UnregisterTubeEntrance(int cell)
	{
		DebugUtil.Assert(Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = false;
		Grid.tubeEntrances.Remove(cell);
	}

	// Token: 0x0600674B RID: 26443 RVA: 0x002D42F8 File Offset: 0x002D24F8
	public static bool ReserveTubeEntrance(int cell, int minionInstanceID, bool reserve)
	{
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		if (!reserve)
		{
			return reservedInstanceIDs.Remove(minionInstanceID);
		}
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		if (reservedInstanceIDs.Count == tubeEntrance.reservationCapacity)
		{
			return false;
		}
		DebugUtil.Assert(reservedInstanceIDs.Add(minionInstanceID));
		return true;
	}

	// Token: 0x0600674C RID: 26444 RVA: 0x002D4350 File Offset: 0x002D2550
	public static void SetTubeEntranceReservationCapacity(int cell, int newReservationCapacity)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.reservationCapacity = newReservationCapacity;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x0600674D RID: 26445 RVA: 0x002D4390 File Offset: 0x002D2590
	public static bool HasUsableTubeEntrance(int cell, int minionInstanceID)
	{
		if (!Grid.HasTubeEntrance[cell])
		{
			return false;
		}
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		if (!tubeEntrance.operational)
		{
			return false;
		}
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		return reservedInstanceIDs.Count < tubeEntrance.reservationCapacity || reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x0600674E RID: 26446 RVA: 0x000E3994 File Offset: 0x000E1B94
	public static bool HasReservedTubeEntrance(int cell, int minionInstanceID)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		return Grid.tubeEntrances[cell].reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x0600674F RID: 26447 RVA: 0x002D43E0 File Offset: 0x002D25E0
	public static void SetTubeEntranceOperational(int cell, bool operational)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.operational = operational;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x06006750 RID: 26448 RVA: 0x002D4420 File Offset: 0x002D2620
	public static void RegisterSuitMarker(int cell)
	{
		DebugUtil.Assert(!Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = true;
		Grid.suitMarkers[cell] = new Grid.SuitMarker
		{
			suitCount = 0,
			lockerCount = 0,
			flags = Grid.SuitMarker.Flags.Operational,
			minionIDsWithSuitReservations = new HashSet<int>(),
			minionIDsWithEmptyLockerReservations = new HashSet<int>()
		};
	}

	// Token: 0x06006751 RID: 26449 RVA: 0x000E39BC File Offset: 0x000E1BBC
	public static void UnregisterSuitMarker(int cell)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = false;
		Grid.suitMarkers.Remove(cell);
	}

	// Token: 0x06006752 RID: 26450 RVA: 0x002D4490 File Offset: 0x002D2690
	public static bool ReserveSuit(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithSuitReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag);
			return flag;
		}
		if (minionIDsWithSuitReservations.Count >= suitMarker.suitCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithSuitReservations.Add(minionInstanceID));
		return true;
	}

	// Token: 0x06006753 RID: 26451 RVA: 0x002D44F0 File Offset: 0x002D26F0
	public static bool ReserveEmptyLocker(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell], "No suit marker");
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithEmptyLockerReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag, "Reservation not removed");
			return flag;
		}
		if (minionIDsWithEmptyLockerReservations.Count >= suitMarker.emptyLockerCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithEmptyLockerReservations.Add(minionInstanceID), "Reservation not made");
		return true;
	}

	// Token: 0x06006754 RID: 26452 RVA: 0x002D4560 File Offset: 0x002D2760
	public static void UpdateSuitMarker(int cell, int fullLockerCount, int emptyLockerCount, Grid.SuitMarker.Flags flags, PathFinder.PotentialPath.Flags pathFlags)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker value = Grid.suitMarkers[cell];
		value.suitCount = fullLockerCount;
		value.lockerCount = fullLockerCount + emptyLockerCount;
		value.flags = flags;
		value.pathFlags = pathFlags;
		Grid.suitMarkers[cell] = value;
	}

	// Token: 0x06006755 RID: 26453 RVA: 0x000E39E6 File Offset: 0x000E1BE6
	public static bool TryGetSuitMarkerFlags(int cell, out Grid.SuitMarker.Flags flags, out PathFinder.PotentialPath.Flags pathFlags)
	{
		if (Grid.HasSuitMarker[cell])
		{
			flags = Grid.suitMarkers[cell].flags;
			pathFlags = Grid.suitMarkers[cell].pathFlags;
			return true;
		}
		flags = (Grid.SuitMarker.Flags)0;
		pathFlags = PathFinder.PotentialPath.Flags.None;
		return false;
	}

	// Token: 0x06006756 RID: 26454 RVA: 0x002D45B8 File Offset: 0x002D27B8
	public static bool HasSuit(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		return minionIDsWithSuitReservations.Count < suitMarker.suitCount || minionIDsWithSuitReservations.Contains(minionInstanceID);
	}

	// Token: 0x06006757 RID: 26455 RVA: 0x002D4600 File Offset: 0x002D2800
	public static bool HasEmptyLocker(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		return minionIDsWithEmptyLockerReservations.Count < suitMarker.emptyLockerCount || minionIDsWithEmptyLockerReservations.Contains(minionInstanceID);
	}

	// Token: 0x06006758 RID: 26456 RVA: 0x002D4648 File Offset: 0x002D2848
	public unsafe static void InitializeCells()
	{
		for (int num = 0; num != Grid.WidthInCells * Grid.HeightInCells; num++)
		{
			ushort index = Grid.elementIdx[num];
			Element element = ElementLoader.elements[(int)index];
			Grid.Element[num] = element;
			if (element.IsSolid)
			{
				Grid.BuildMasks[num] |= Grid.BuildFlags.Solid;
			}
			else
			{
				Grid.BuildMasks[num] &= ~Grid.BuildFlags.Solid;
			}
			Grid.RenderedByWorld[num] = (element.substance != null && element.substance.renderedByWorld && Grid.Objects[num, 9] == null);
		}
	}

	// Token: 0x06006759 RID: 26457 RVA: 0x000E3A22 File Offset: 0x000E1C22
	public static bool IsInitialized()
	{
		return Grid.mass != null;
	}

	// Token: 0x0600675A RID: 26458 RVA: 0x002D46F8 File Offset: 0x002D28F8
	public static int GetCellInDirection(int cell, Direction d)
	{
		switch (d)
		{
		case Direction.Up:
			return Grid.CellAbove(cell);
		case Direction.Right:
			return Grid.CellRight(cell);
		case Direction.Down:
			return Grid.CellBelow(cell);
		case Direction.Left:
			return Grid.CellLeft(cell);
		case Direction.None:
			return cell;
		}
		return -1;
	}

	// Token: 0x0600675B RID: 26459 RVA: 0x002D4744 File Offset: 0x002D2944
	public static bool Raycast(int cell, Vector2I direction, out int hitDistance, int maxDistance = 100, Grid.BuildFlags layerMask = Grid.BuildFlags.Any)
	{
		bool flag = false;
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = vector2I + direction * maxDistance;
		int num = cell;
		int num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
		int num3 = 0;
		int num4 = 0;
		float num5 = (float)maxDistance * 0.5f;
		while ((float)num3 < num5)
		{
			if (!Grid.IsValidCell(num) || (Grid.BuildMasks[num] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				flag = true;
				break;
			}
			if (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				num4 = maxDistance - num3;
			}
			vector2I += direction;
			vector2I2 -= direction;
			num = Grid.XYToCell(vector2I.x, vector2I.y);
			num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
			num3++;
		}
		if (!flag && maxDistance % 2 == 0)
		{
			flag = (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		hitDistance = (flag ? num3 : ((num4 > 0) ? num4 : maxDistance));
		return flag | hitDistance == num4;
	}

	// Token: 0x0600675C RID: 26460 RVA: 0x000E3A30 File Offset: 0x000E1C30
	public static int CellAbove(int cell)
	{
		return cell + Grid.WidthInCells;
	}

	// Token: 0x0600675D RID: 26461 RVA: 0x000E3A39 File Offset: 0x000E1C39
	public static int CellBelow(int cell)
	{
		return cell - Grid.WidthInCells;
	}

	// Token: 0x0600675E RID: 26462 RVA: 0x000E3A42 File Offset: 0x000E1C42
	public static int CellLeft(int cell)
	{
		if (cell % Grid.WidthInCells <= 0)
		{
			return Grid.InvalidCell;
		}
		return cell - 1;
	}

	// Token: 0x0600675F RID: 26463 RVA: 0x000E3A57 File Offset: 0x000E1C57
	public static int CellRight(int cell)
	{
		if (cell % Grid.WidthInCells >= Grid.WidthInCells - 1)
		{
			return Grid.InvalidCell;
		}
		return cell + 1;
	}

	// Token: 0x06006760 RID: 26464 RVA: 0x002D4848 File Offset: 0x002D2A48
	public static CellOffset GetOffset(int cell)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return new CellOffset(x, y);
	}

	// Token: 0x06006761 RID: 26465 RVA: 0x002D486C File Offset: 0x002D2A6C
	public static int CellUpLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06006762 RID: 26466 RVA: 0x002D48A4 File Offset: 0x002D2AA4
	public static int CellUpRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06006763 RID: 26467 RVA: 0x002D48E4 File Offset: 0x002D2AE4
	public static int CellDownLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell > Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06006764 RID: 26468 RVA: 0x002D4914 File Offset: 0x002D2B14
	public static int CellDownRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell >= Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06006765 RID: 26469 RVA: 0x000E3A72 File Offset: 0x000E1C72
	public static bool IsCellLeftOf(int cell, int other_cell)
	{
		return Grid.CellColumn(cell) < Grid.CellColumn(other_cell);
	}

	// Token: 0x06006766 RID: 26470 RVA: 0x002D494C File Offset: 0x002D2B4C
	public static bool IsCellOffsetOf(int cell, int target_cell, CellOffset[] target_offsets)
	{
		int num = target_offsets.Length;
		for (int i = 0; i < num; i++)
		{
			if (cell == Grid.OffsetCell(target_cell, target_offsets[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006767 RID: 26471 RVA: 0x002D497C File Offset: 0x002D2B7C
	public static int GetCellDistance(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Abs(num - num3) + Math.Abs(num2 - num4);
	}

	// Token: 0x06006768 RID: 26472 RVA: 0x002D49B0 File Offset: 0x002D2BB0
	public static int GetCellRange(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Max(Math.Abs(num - num3), Math.Abs(num2 - num4));
	}

	// Token: 0x06006769 RID: 26473 RVA: 0x002D49E8 File Offset: 0x002D2BE8
	public static CellOffset GetOffset(int base_cell, int offset_cell)
	{
		int num;
		int num2;
		Grid.CellToXY(base_cell, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(offset_cell, out num3, out num4);
		return new CellOffset(num3 - num, num4 - num2);
	}

	// Token: 0x0600676A RID: 26474 RVA: 0x002D4A14 File Offset: 0x002D2C14
	public static CellOffset GetCellOffsetDirection(int base_cell, int offset_cell)
	{
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		offset.x = Mathf.Clamp(offset.x, -1, 1);
		offset.y = Mathf.Clamp(offset.y, -1, 1);
		return offset;
	}

	// Token: 0x0600676B RID: 26475 RVA: 0x000E3A82 File Offset: 0x000E1C82
	public static int OffsetCell(int cell, CellOffset offset)
	{
		return cell + offset.x + offset.y * Grid.WidthInCells;
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x000E3A99 File Offset: 0x000E1C99
	public static int OffsetCell(int cell, int x, int y)
	{
		return cell + x + y * Grid.WidthInCells;
	}

	// Token: 0x0600676D RID: 26477 RVA: 0x002D4A54 File Offset: 0x002D2C54
	public static bool IsCellOffsetValid(int cell, int x, int y)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return num + x >= 0 && num + x < Grid.WidthInCells && num2 + y >= 0 && num2 + y < Grid.HeightInCells;
	}

	// Token: 0x0600676E RID: 26478 RVA: 0x000E3AA6 File Offset: 0x000E1CA6
	public static bool IsCellOffsetValid(int cell, CellOffset offset)
	{
		return Grid.IsCellOffsetValid(cell, offset.x, offset.y);
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x000E3ABA File Offset: 0x000E1CBA
	public static int PosToCell(StateMachine.Instance smi)
	{
		return Grid.PosToCell(smi.transform.GetPosition());
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x000E3ACC File Offset: 0x000E1CCC
	public static int PosToCell(GameObject go)
	{
		return Grid.PosToCell(go.transform.GetPosition());
	}

	// Token: 0x06006771 RID: 26481 RVA: 0x000E24C5 File Offset: 0x000E06C5
	public static int PosToCell(KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x06006772 RID: 26482 RVA: 0x002D4A90 File Offset: 0x002D2C90
	public static bool IsValidBuildingCell(int cell)
	{
		if (!Grid.IsWorldValidCell(cell))
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
		if (world == null)
		{
			return false;
		}
		Vector2I vector2I = Grid.CellToXY(cell);
		return (float)vector2I.x >= world.minimumBounds.x && (float)vector2I.x <= world.maximumBounds.x && (float)vector2I.y >= world.minimumBounds.y && (float)vector2I.y <= world.maximumBounds.y - (float)Grid.TopBorderHeight;
	}

	// Token: 0x06006773 RID: 26483 RVA: 0x000E3ADE File Offset: 0x000E1CDE
	public static bool IsWorldValidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue;
	}

	// Token: 0x06006774 RID: 26484 RVA: 0x000E3AFB File Offset: 0x000E1CFB
	public static bool IsValidCell(int cell)
	{
		return cell >= 0 && cell < Grid.CellCount;
	}

	// Token: 0x06006775 RID: 26485 RVA: 0x000E3B0B File Offset: 0x000E1D0B
	public static bool IsValidCellInWorld(int cell, int world)
	{
		return cell >= 0 && cell < Grid.CellCount && (int)Grid.WorldIdx[cell] == world;
	}

	// Token: 0x06006776 RID: 26486 RVA: 0x000E3B25 File Offset: 0x000E1D25
	public static bool IsActiveWorld(int cell)
	{
		return ClusterManager.Instance != null && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[cell];
	}

	// Token: 0x06006777 RID: 26487 RVA: 0x000E3B49 File Offset: 0x000E1D49
	public static bool AreCellsInSameWorld(int cell, int world_cell)
	{
		return Grid.IsValidCell(cell) && Grid.IsValidCell(world_cell) && Grid.WorldIdx[cell] == Grid.WorldIdx[world_cell];
	}

	// Token: 0x06006778 RID: 26488 RVA: 0x000E3B6D File Offset: 0x000E1D6D
	public static bool IsCellOpenToSpace(int cell)
	{
		return !Grid.IsSolidCell(cell) && !(Grid.Objects[cell, 2] != null) && global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
	}

	// Token: 0x06006779 RID: 26489 RVA: 0x002D4B28 File Offset: 0x002D2D28
	public static int PosToCell(Vector2 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x0600677A RID: 26490 RVA: 0x002D4B54 File Offset: 0x002D2D54
	public static int PosToCell(Vector3 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x0600677B RID: 26491 RVA: 0x000E3BA2 File Offset: 0x000E1DA2
	public static void PosToXY(Vector3 pos, out int x, out int y)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out x, out y);
	}

	// Token: 0x0600677C RID: 26492 RVA: 0x000E3BB1 File Offset: 0x000E1DB1
	public static void PosToXY(Vector3 pos, out Vector2I xy)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out xy.x, out xy.y);
	}

	// Token: 0x0600677D RID: 26493 RVA: 0x002D4B80 File Offset: 0x002D2D80
	public static Vector2I PosToXY(Vector3 pos)
	{
		Vector2I result;
		Grid.CellToXY(Grid.PosToCell(pos), out result.x, out result.y);
		return result;
	}

	// Token: 0x0600677E RID: 26494 RVA: 0x000E3BCA File Offset: 0x000E1DCA
	public static int XYToCell(int x, int y)
	{
		return x + y * Grid.WidthInCells;
	}

	// Token: 0x0600677F RID: 26495 RVA: 0x000E3BD5 File Offset: 0x000E1DD5
	public static void CellToXY(int cell, out int x, out int y)
	{
		x = Grid.CellColumn(cell);
		y = Grid.CellRow(cell);
	}

	// Token: 0x06006780 RID: 26496 RVA: 0x000E3BE7 File Offset: 0x000E1DE7
	public static Vector2I CellToXY(int cell)
	{
		return new Vector2I(Grid.CellColumn(cell), Grid.CellRow(cell));
	}

	// Token: 0x06006781 RID: 26497 RVA: 0x002D4BA8 File Offset: 0x002D2DA8
	public static Vector3 CellToPos(int cell, float x_offset, float y_offset, float z_offset)
	{
		int widthInCells = Grid.WidthInCells;
		float num = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float num2 = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(num + x_offset, num2 + y_offset, z_offset);
	}

	// Token: 0x06006782 RID: 26498 RVA: 0x002D4BDC File Offset: 0x002D2DDC
	public static Vector3 CellToPos(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(x, y, 0f);
	}

	// Token: 0x06006783 RID: 26499 RVA: 0x002D4C10 File Offset: 0x002D2E10
	public static Vector3 CellToPos2D(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector2(x, y);
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x000E3BFA File Offset: 0x000E1DFA
	public static int CellRow(int cell)
	{
		return cell / Grid.WidthInCells;
	}

	// Token: 0x06006785 RID: 26501 RVA: 0x000E3C03 File Offset: 0x000E1E03
	public static int CellColumn(int cell)
	{
		return cell % Grid.WidthInCells;
	}

	// Token: 0x06006786 RID: 26502 RVA: 0x000E3C0C File Offset: 0x000E1E0C
	public static int ClampX(int x)
	{
		return Math.Min(Math.Max(x, 0), Grid.WidthInCells - 1);
	}

	// Token: 0x06006787 RID: 26503 RVA: 0x000E3C21 File Offset: 0x000E1E21
	public static int ClampY(int y)
	{
		return Math.Min(Math.Max(y, 0), Grid.HeightInCells - 1);
	}

	// Token: 0x06006788 RID: 26504 RVA: 0x002D4C44 File Offset: 0x002D2E44
	public static Vector2I Constrain(Vector2I val)
	{
		val.x = Mathf.Max(0, Mathf.Min(val.x, Grid.WidthInCells - 1));
		val.y = Mathf.Max(0, Mathf.Min(val.y, Grid.HeightInCells - 1));
		return val;
	}

	// Token: 0x06006789 RID: 26505 RVA: 0x002D4C90 File Offset: 0x002D2E90
	public static void Reveal(int cell, byte visibility = 255, bool forceReveal = false)
	{
		bool flag = Grid.Spawnable[cell] == 0 && visibility > 0;
		Grid.Spawnable[cell] = Math.Max(visibility, Grid.Visible[cell]);
		if (forceReveal || !Grid.PreventFogOfWarReveal[cell])
		{
			Grid.Visible[cell] = Math.Max(visibility, Grid.Visible[cell]);
		}
		if (flag && Grid.OnReveal != null)
		{
			Grid.OnReveal(cell);
		}
	}

	// Token: 0x0600678A RID: 26506 RVA: 0x000E3C36 File Offset: 0x000E1E36
	public static ObjectLayer GetObjectLayerForConduitType(ConduitType conduit_type)
	{
		switch (conduit_type)
		{
		case ConduitType.Gas:
			return ObjectLayer.GasConduitConnection;
		case ConduitType.Liquid:
			return ObjectLayer.LiquidConduitConnection;
		case ConduitType.Solid:
			return ObjectLayer.SolidConduitConnection;
		default:
			throw new ArgumentException("Invalid value.", "conduit_type");
		}
	}

	// Token: 0x0600678B RID: 26507 RVA: 0x002D4CFC File Offset: 0x002D2EFC
	public static Vector3 CellToPos(int cell, CellAlignment alignment, Grid.SceneLayer layer)
	{
		switch (alignment)
		{
		case CellAlignment.Bottom:
			return Grid.CellToPosCBC(cell, layer);
		case CellAlignment.Top:
			return Grid.CellToPosCTC(cell, layer);
		case CellAlignment.Left:
			return Grid.CellToPosLCC(cell, layer);
		case CellAlignment.Right:
			return Grid.CellToPosRCC(cell, layer);
		case CellAlignment.RandomInternal:
		{
			Vector3 b = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0f, 0f);
			return Grid.CellToPosCCC(cell, layer) + b;
		}
		}
		return Grid.CellToPosCCC(cell, layer);
	}

	// Token: 0x0600678C RID: 26508 RVA: 0x000E3C66 File Offset: 0x000E1E66
	public static float GetLayerZ(Grid.SceneLayer layer)
	{
		return -Grid.HalfCellSizeInMeters - Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier;
	}

	// Token: 0x0600678D RID: 26509 RVA: 0x000E3C7D File Offset: 0x000E1E7D
	public static Vector3 CellToPosCCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600678E RID: 26510 RVA: 0x000E3C95 File Offset: 0x000E1E95
	public static Vector3 CellToPosCBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600678F RID: 26511 RVA: 0x000E3CAD File Offset: 0x000E1EAD
	public static Vector3 CellToPosCCF(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, -Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier);
	}

	// Token: 0x06006790 RID: 26512 RVA: 0x000E3CCE File Offset: 0x000E1ECE
	public static Vector3 CellToPosLCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x06006791 RID: 26513 RVA: 0x000E3CE6 File Offset: 0x000E1EE6
	public static Vector3 CellToPosRCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x06006792 RID: 26514 RVA: 0x000E3D04 File Offset: 0x000E1F04
	public static Vector3 CellToPosCTC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.CellSizeInMeters - 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x06006793 RID: 26515 RVA: 0x000E3D22 File Offset: 0x000E1F22
	public static bool IsSolidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x06006794 RID: 26516 RVA: 0x002D4D80 File Offset: 0x002D2F80
	public unsafe static bool IsSubstantialLiquid(int cell, float threshold = 0.35f)
	{
		if (Grid.IsValidCell(cell))
		{
			ushort num = Grid.elementIdx[cell];
			if ((int)num < ElementLoader.elements.Count)
			{
				Element element = ElementLoader.elements[(int)num];
				if (element.IsLiquid && Grid.mass[cell] >= element.defaultValues.mass * threshold)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006795 RID: 26517 RVA: 0x002D4DE0 File Offset: 0x002D2FE0
	public static bool IsVisiblyInLiquid(Vector2 pos)
	{
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && Grid.IsLiquid(num))
		{
			int cell = Grid.CellAbove(num);
			if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
			{
				return true;
			}
			float num2 = Grid.Mass[num];
			float num3 = (float)((int)pos.y) - pos.y;
			if (num2 / 1000f <= num3)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006796 RID: 26518 RVA: 0x002D4E44 File Offset: 0x002D3044
	public static bool IsNavigatableLiquid(int cell)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			return true;
		}
		if (Grid.IsLiquid(cell))
		{
			if (Grid.Element[num].IsLiquid)
			{
				return true;
			}
			if (Grid.Element[num].IsSolid)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006797 RID: 26519 RVA: 0x000E3D39 File Offset: 0x000E1F39
	public static bool IsLiquid(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsLiquid;
	}

	// Token: 0x06006798 RID: 26520 RVA: 0x000E3D5A File Offset: 0x000E1F5A
	public static bool IsGas(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsGas;
	}

	// Token: 0x06006799 RID: 26521 RVA: 0x002D4EA4 File Offset: 0x002D30A4
	public static void GetVisibleExtents(out int min_x, out int min_y, out int max_x, out int max_y)
	{
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		min_y = (int)vector2.y;
		max_y = (int)(vector.y + 0.5f);
		min_x = (int)vector2.x;
		max_x = (int)(vector.x + 0.5f);
	}

	// Token: 0x0600679A RID: 26522 RVA: 0x000E3D7B File Offset: 0x000E1F7B
	public static void GetVisibleExtents(out Vector2I min, out Vector2I max)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
	}

	// Token: 0x0600679B RID: 26523 RVA: 0x002D4F40 File Offset: 0x002D3140
	public static void GetVisibleCellRangeInActiveWorld(out Vector2I min, out Vector2I max, int padding = 4, float rangeScale = 1.5f)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
		min.x -= padding;
		min.y -= padding;
		if (CameraController.Instance != null && DlcManager.IsExpansion1Active())
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
			min.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, min.x));
			min.y = Math.Min(vector2I.y + vector2I2.y - 1, Math.Max(vector2I.y, min.y));
			max.x += padding;
			max.y += padding;
			max.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, max.x));
			max.y = Math.Min(vector2I.y + vector2I2.y - 1 + 20, Math.Max(vector2I.y, max.y));
			return;
		}
		min.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, min.x));
		min.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, min.y));
		max.x += padding;
		max.y += padding;
		max.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, max.x));
		max.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, max.y));
	}

	// Token: 0x0600679C RID: 26524 RVA: 0x002D510C File Offset: 0x002D330C
	public static Extents GetVisibleExtentsInActiveWorld(int padding = 4, float rangeScale = 1.5f)
	{
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		return new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
	}

	// Token: 0x0600679D RID: 26525 RVA: 0x000E3D9A File Offset: 0x000E1F9A
	public static bool IsVisible(int cell)
	{
		return Grid.Visible[cell] > 0 || !PropertyTextures.IsFogOfWarEnabled;
	}

	// Token: 0x0600679E RID: 26526 RVA: 0x000E3DB0 File Offset: 0x000E1FB0
	public static bool VisibleBlockingCB(int cell)
	{
		return !Grid.Transparent[cell] && Grid.IsSolidCell(cell);
	}

	// Token: 0x0600679F RID: 26527 RVA: 0x000E3DC7 File Offset: 0x000E1FC7
	public static bool VisibilityTest(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.TestLineOfSight(x, y, x2, y2, Grid.VisibleBlockingDelegate, blocking_tile_visible, false);
	}

	// Token: 0x060067A0 RID: 26528 RVA: 0x002D5154 File Offset: 0x002D3354
	public static bool VisibilityTest(int cell, int target_cell, bool blocking_tile_visible = false)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		int x2 = 0;
		int y2 = 0;
		Grid.CellToXY(target_cell, out x2, out y2);
		return Grid.VisibilityTest(x, y, x2, y2, blocking_tile_visible);
	}

	// Token: 0x060067A1 RID: 26529 RVA: 0x000E3DDA File Offset: 0x000E1FDA
	public static bool PhysicalBlockingCB(int cell)
	{
		return Grid.Solid[cell];
	}

	// Token: 0x060067A2 RID: 26530 RVA: 0x000E3DE7 File Offset: 0x000E1FE7
	public static bool IsPhysicallyAccessible(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.FastTestLineOfSightSolid(x, y, x2, y2);
	}

	// Token: 0x060067A3 RID: 26531 RVA: 0x002D5188 File Offset: 0x002D3388
	public static void CollectCellsInLine(int startCell, int endCell, HashSet<int> outputCells)
	{
		int num = 2;
		int cellDistance = Grid.GetCellDistance(startCell, endCell);
		Vector2 a = (Grid.CellToPos(endCell) - Grid.CellToPos(startCell)).normalized;
		for (float num2 = 0f; num2 < (float)cellDistance; num2 = Mathf.Min(num2 + 1f / (float)num, (float)cellDistance))
		{
			int num3 = Grid.PosToCell(Grid.CellToPos(startCell) + a * num2);
			if (Grid.GetCellDistance(startCell, num3) <= cellDistance)
			{
				outputCells.Add(num3);
			}
		}
	}

	// Token: 0x060067A4 RID: 26532 RVA: 0x002D5214 File Offset: 0x002D3414
	public static bool IsRangeExposedToSunlight(int cell, int scanRadius, CellOffset scanShape, out int cellsClear, int clearThreshold = 1)
	{
		cellsClear = 0;
		if (Grid.IsValidCell(cell) && (int)Grid.ExposedToSunlight[cell] >= clearThreshold)
		{
			cellsClear++;
		}
		bool flag = true;
		bool flag2 = true;
		int num = 1;
		while (num <= scanRadius && (flag || flag2))
		{
			int num2 = Grid.OffsetCell(cell, scanShape.x * num, scanShape.y * num);
			int num3 = Grid.OffsetCell(cell, -scanShape.x * num, scanShape.y * num);
			if (Grid.IsValidCell(num2) && (int)Grid.ExposedToSunlight[num2] >= clearThreshold)
			{
				cellsClear++;
			}
			if (Grid.IsValidCell(num3) && (int)Grid.ExposedToSunlight[num3] >= clearThreshold)
			{
				cellsClear++;
			}
			num++;
		}
		return cellsClear > 0;
	}

	// Token: 0x060067A5 RID: 26533 RVA: 0x002D52C8 File Offset: 0x002D34C8
	public static bool FastTestLineOfSightSolid(int x, int y, int x2, int y2)
	{
		int value = x2 - x;
		int num = y2 - y;
		int num2 = 0;
		int num4;
		int num3 = num4 = Math.Sign(value);
		int num5 = Math.Sign(num);
		int num6 = Math.Abs(value);
		int num7 = Math.Abs(num);
		if (num6 <= num7)
		{
			num6 = Math.Abs(num);
			num7 = Math.Abs(value);
			if (num < 0)
			{
				num2 = -1;
			}
			else if (num > 0)
			{
				num2 = 1;
			}
			num4 = 0;
		}
		int num8 = num6 >> 1;
		int num9 = num3 + num5 * Grid.WidthInCells;
		int num10 = num4 + num2 * Grid.WidthInCells;
		int num11 = Grid.XYToCell(x, y);
		for (int i = 1; i < num6; i++)
		{
			num8 += num7;
			if (num8 < num6)
			{
				num11 += num10;
			}
			else
			{
				num8 -= num6;
				num11 += num9;
			}
			if (Grid.Solid[num11])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060067A6 RID: 26534 RVA: 0x002D5398 File Offset: 0x002D3598
	public static bool TestLineOfSightFixedBlockingVisible(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x060067A7 RID: 26535 RVA: 0x002D54B4 File Offset: 0x002D36B4
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, Func<int, bool> blocking_tile_visible_cb, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible_cb(num12) && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x060067A8 RID: 26536 RVA: 0x000E3DF2 File Offset: 0x000E1FF2
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible = false, bool allow_invalid_cells = false)
	{
		return Grid.TestLineOfSightFixedBlockingVisible(x, y, x2, y2, blocking_cb, blocking_tile_visible, allow_invalid_cells);
	}

	// Token: 0x060067A9 RID: 26537 RVA: 0x002D55DC File Offset: 0x002D37DC
	public static bool GetFreeGridSpace(Vector2I size, out Vector2I offset)
	{
		Vector2I gridOffset = BestFit.GetGridOffset(ClusterManager.Instance.WorldContainers, size, out offset);
		if (gridOffset.X <= Grid.WidthInCells && gridOffset.Y <= Grid.HeightInCells)
		{
			SimMessages.SimDataResizeGridAndInitializeVacuumCells(gridOffset, size.x, size.y, offset.x, offset.y);
			Game.Instance.roomProber.Refresh();
			return true;
		}
		return false;
	}

	// Token: 0x060067AA RID: 26538 RVA: 0x002D5648 File Offset: 0x002D3848
	public static void FreeGridSpace(Vector2I size, Vector2I offset)
	{
		SimMessages.SimDataFreeCells(size.x, size.y, offset.x, offset.y);
		for (int i = offset.y; i < size.y + offset.y + 1; i++)
		{
			for (int j = offset.x - 1; j < size.x + offset.x + 1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num))
				{
					Grid.Element[num] = ElementLoader.FindElementByHash(SimHashes.Vacuum);
				}
			}
		}
		Game.Instance.roomProber.Refresh();
	}

	// Token: 0x060067AB RID: 26539 RVA: 0x000E3E03 File Offset: 0x000E2003
	[Conditional("UNITY_EDITOR")]
	public static void DrawBoxOnCell(int cell, Color color, float offset = 0f)
	{
		Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0f);
	}

	// Token: 0x04004DCD RID: 19917
	public static readonly CellOffset[] DefaultOffset = new CellOffset[1];

	// Token: 0x04004DCE RID: 19918
	public static float WidthInMeters;

	// Token: 0x04004DCF RID: 19919
	public static float HeightInMeters;

	// Token: 0x04004DD0 RID: 19920
	public static int WidthInCells;

	// Token: 0x04004DD1 RID: 19921
	public static int HeightInCells;

	// Token: 0x04004DD2 RID: 19922
	public static float CellSizeInMeters;

	// Token: 0x04004DD3 RID: 19923
	public static float InverseCellSizeInMeters;

	// Token: 0x04004DD4 RID: 19924
	public static float HalfCellSizeInMeters;

	// Token: 0x04004DD5 RID: 19925
	public static int CellCount;

	// Token: 0x04004DD6 RID: 19926
	public static int InvalidCell = -1;

	// Token: 0x04004DD7 RID: 19927
	public static int TopBorderHeight = 2;

	// Token: 0x04004DD8 RID: 19928
	public static Dictionary<int, GameObject>[] ObjectLayers;

	// Token: 0x04004DD9 RID: 19929
	public static Action<int> OnReveal;

	// Token: 0x04004DDA RID: 19930
	public static Grid.BuildFlags[] BuildMasks;

	// Token: 0x04004DDB RID: 19931
	public static Grid.BuildFlagsFoundationIndexer Foundation;

	// Token: 0x04004DDC RID: 19932
	public static Grid.BuildFlagsSolidIndexer Solid;

	// Token: 0x04004DDD RID: 19933
	public static Grid.BuildFlagsDupeImpassableIndexer DupeImpassable;

	// Token: 0x04004DDE RID: 19934
	public static Grid.BuildFlagsFakeFloorIndexer FakeFloor;

	// Token: 0x04004DDF RID: 19935
	public static Grid.BuildFlagsDupePassableIndexer DupePassable;

	// Token: 0x04004DE0 RID: 19936
	public static Grid.BuildFlagsImpassableIndexer CritterImpassable;

	// Token: 0x04004DE1 RID: 19937
	public static Grid.BuildFlagsDoorIndexer HasDoor;

	// Token: 0x04004DE2 RID: 19938
	public static Grid.VisFlags[] VisMasks;

	// Token: 0x04004DE3 RID: 19939
	public static Grid.VisFlagsRevealedIndexer Revealed;

	// Token: 0x04004DE4 RID: 19940
	public static Grid.VisFlagsPreventFogOfWarRevealIndexer PreventFogOfWarReveal;

	// Token: 0x04004DE5 RID: 19941
	public static Grid.VisFlagsRenderedByWorldIndexer RenderedByWorld;

	// Token: 0x04004DE6 RID: 19942
	public static Grid.VisFlagsAllowPathfindingIndexer AllowPathfinding;

	// Token: 0x04004DE7 RID: 19943
	public static Grid.NavValidatorFlags[] NavValidatorMasks;

	// Token: 0x04004DE8 RID: 19944
	public static Grid.NavValidatorFlagsLadderIndexer HasLadder;

	// Token: 0x04004DE9 RID: 19945
	public static Grid.NavValidatorFlagsPoleIndexer HasPole;

	// Token: 0x04004DEA RID: 19946
	public static Grid.NavValidatorFlagsTubeIndexer HasTube;

	// Token: 0x04004DEB RID: 19947
	public static Grid.NavValidatorFlagsNavTeleporterIndexer HasNavTeleporter;

	// Token: 0x04004DEC RID: 19948
	public static Grid.NavValidatorFlagsUnderConstructionIndexer IsTileUnderConstruction;

	// Token: 0x04004DED RID: 19949
	public static Grid.NavFlags[] NavMasks;

	// Token: 0x04004DEE RID: 19950
	private static Grid.NavFlagsAccessDoorIndexer HasAccessDoor;

	// Token: 0x04004DEF RID: 19951
	public static Grid.NavFlagsTubeEntranceIndexer HasTubeEntrance;

	// Token: 0x04004DF0 RID: 19952
	public static Grid.NavFlagsPreventIdleTraversalIndexer PreventIdleTraversal;

	// Token: 0x04004DF1 RID: 19953
	public static Grid.NavFlagsReservedIndexer Reserved;

	// Token: 0x04004DF2 RID: 19954
	public static Grid.NavFlagsSuitMarkerIndexer HasSuitMarker;

	// Token: 0x04004DF3 RID: 19955
	private static Dictionary<int, Grid.Restriction> restrictions = new Dictionary<int, Grid.Restriction>();

	// Token: 0x04004DF4 RID: 19956
	private static Dictionary<int, Grid.TubeEntrance> tubeEntrances = new Dictionary<int, Grid.TubeEntrance>();

	// Token: 0x04004DF5 RID: 19957
	private static Dictionary<int, Grid.SuitMarker> suitMarkers = new Dictionary<int, Grid.SuitMarker>();

	// Token: 0x04004DF6 RID: 19958
	public unsafe static ushort* elementIdx;

	// Token: 0x04004DF7 RID: 19959
	public unsafe static float* temperature;

	// Token: 0x04004DF8 RID: 19960
	public unsafe static float* radiation;

	// Token: 0x04004DF9 RID: 19961
	public unsafe static float* mass;

	// Token: 0x04004DFA RID: 19962
	public unsafe static byte* properties;

	// Token: 0x04004DFB RID: 19963
	public unsafe static byte* strengthInfo;

	// Token: 0x04004DFC RID: 19964
	public unsafe static byte* insulation;

	// Token: 0x04004DFD RID: 19965
	public unsafe static byte* diseaseIdx;

	// Token: 0x04004DFE RID: 19966
	public unsafe static int* diseaseCount;

	// Token: 0x04004DFF RID: 19967
	public unsafe static byte* exposedToSunlight;

	// Token: 0x04004E00 RID: 19968
	public unsafe static float* AccumulatedFlowValues = null;

	// Token: 0x04004E01 RID: 19969
	public static byte[] Visible;

	// Token: 0x04004E02 RID: 19970
	public static byte[] Spawnable;

	// Token: 0x04004E03 RID: 19971
	public static float[] Damage;

	// Token: 0x04004E04 RID: 19972
	public static float[] Decor;

	// Token: 0x04004E05 RID: 19973
	public static bool[] GravitasFacility;

	// Token: 0x04004E06 RID: 19974
	public static byte[] WorldIdx;

	// Token: 0x04004E07 RID: 19975
	public static float[] Loudness;

	// Token: 0x04004E08 RID: 19976
	public static Element[] Element;

	// Token: 0x04004E09 RID: 19977
	public static int[] LightCount;

	// Token: 0x04004E0A RID: 19978
	public static Grid.PressureIndexer Pressure;

	// Token: 0x04004E0B RID: 19979
	public static Grid.TransparentIndexer Transparent;

	// Token: 0x04004E0C RID: 19980
	public static Grid.ElementIdxIndexer ElementIdx;

	// Token: 0x04004E0D RID: 19981
	public static Grid.TemperatureIndexer Temperature;

	// Token: 0x04004E0E RID: 19982
	public static Grid.RadiationIndexer Radiation;

	// Token: 0x04004E0F RID: 19983
	public static Grid.MassIndexer Mass;

	// Token: 0x04004E10 RID: 19984
	public static Grid.PropertiesIndexer Properties;

	// Token: 0x04004E11 RID: 19985
	public static Grid.ExposedToSunlightIndexer ExposedToSunlight;

	// Token: 0x04004E12 RID: 19986
	public static Grid.StrengthInfoIndexer StrengthInfo;

	// Token: 0x04004E13 RID: 19987
	public static Grid.Insulationndexer Insulation;

	// Token: 0x04004E14 RID: 19988
	public static Grid.DiseaseIdxIndexer DiseaseIdx;

	// Token: 0x04004E15 RID: 19989
	public static Grid.DiseaseCountIndexer DiseaseCount;

	// Token: 0x04004E16 RID: 19990
	public static Grid.LightIntensityIndexer LightIntensity;

	// Token: 0x04004E17 RID: 19991
	public static Grid.AccumulatedFlowIndexer AccumulatedFlow;

	// Token: 0x04004E18 RID: 19992
	public static Grid.ObjectLayerIndexer Objects;

	// Token: 0x04004E19 RID: 19993
	public static float LayerMultiplier = 1f;

	// Token: 0x04004E1A RID: 19994
	private static readonly Func<int, bool> VisibleBlockingDelegate = (int cell) => Grid.VisibleBlockingCB(cell);

	// Token: 0x04004E1B RID: 19995
	private static readonly Func<int, bool> PhysicalBlockingDelegate = (int cell) => Grid.PhysicalBlockingCB(cell);

	// Token: 0x020013A1 RID: 5025
	[Flags]
	public enum BuildFlags : byte
	{
		// Token: 0x04004E1D RID: 19997
		Solid = 1,
		// Token: 0x04004E1E RID: 19998
		Foundation = 2,
		// Token: 0x04004E1F RID: 19999
		Door = 4,
		// Token: 0x04004E20 RID: 20000
		DupePassable = 8,
		// Token: 0x04004E21 RID: 20001
		DupeImpassable = 16,
		// Token: 0x04004E22 RID: 20002
		CritterImpassable = 32,
		// Token: 0x04004E23 RID: 20003
		FakeFloor = 192,
		// Token: 0x04004E24 RID: 20004
		Any = 255
	}

	// Token: 0x020013A2 RID: 5026
	public struct BuildFlagsFoundationIndexer
	{
		// Token: 0x17000670 RID: 1648
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Foundation) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Foundation, value);
			}
		}
	}

	// Token: 0x020013A3 RID: 5027
	public struct BuildFlagsSolidIndexer
	{
		// Token: 0x17000671 RID: 1649
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Solid) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}
	}

	// Token: 0x020013A4 RID: 5028
	public struct BuildFlagsDupeImpassableIndexer
	{
		// Token: 0x17000672 RID: 1650
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupeImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupeImpassable, value);
			}
		}
	}

	// Token: 0x020013A5 RID: 5029
	public struct BuildFlagsFakeFloorIndexer
	{
		// Token: 0x17000673 RID: 1651
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.FakeFloor) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x002D5764 File Offset: 0x002D3964
		public void Add(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) + 1);
			num = Math.Min(num, 3);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x002D57A4 File Offset: 0x002D39A4
		public void Remove(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) - Grid.BuildFlags.Solid);
			num = Math.Max(num, 0);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}
	}

	// Token: 0x020013A6 RID: 5030
	public struct BuildFlagsDupePassableIndexer
	{
		// Token: 0x17000674 RID: 1652
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupePassable, value);
			}
		}
	}

	// Token: 0x020013A7 RID: 5031
	public struct BuildFlagsImpassableIndexer
	{
		// Token: 0x17000675 RID: 1653
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.CritterImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.CritterImpassable, value);
			}
		}
	}

	// Token: 0x020013A8 RID: 5032
	public struct BuildFlagsDoorIndexer
	{
		// Token: 0x17000676 RID: 1654
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Door) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Door, value);
			}
		}
	}

	// Token: 0x020013A9 RID: 5033
	[Flags]
	public enum VisFlags : byte
	{
		// Token: 0x04004E26 RID: 20006
		Revealed = 1,
		// Token: 0x04004E27 RID: 20007
		PreventFogOfWarReveal = 2,
		// Token: 0x04004E28 RID: 20008
		RenderedByWorld = 4,
		// Token: 0x04004E29 RID: 20009
		AllowPathfinding = 8
	}

	// Token: 0x020013AA RID: 5034
	public struct VisFlagsRevealedIndexer
	{
		// Token: 0x17000677 RID: 1655
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.Revealed) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.Revealed, value);
			}
		}
	}

	// Token: 0x020013AB RID: 5035
	public struct VisFlagsPreventFogOfWarRevealIndexer
	{
		// Token: 0x17000678 RID: 1656
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.PreventFogOfWarReveal) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.PreventFogOfWarReveal, value);
			}
		}
	}

	// Token: 0x020013AC RID: 5036
	public struct VisFlagsRenderedByWorldIndexer
	{
		// Token: 0x17000679 RID: 1657
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.RenderedByWorld) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.RenderedByWorld, value);
			}
		}
	}

	// Token: 0x020013AD RID: 5037
	public struct VisFlagsAllowPathfindingIndexer
	{
		// Token: 0x1700067A RID: 1658
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.AllowPathfinding) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.AllowPathfinding, value);
			}
		}
	}

	// Token: 0x020013AE RID: 5038
	[Flags]
	public enum NavValidatorFlags : byte
	{
		// Token: 0x04004E2B RID: 20011
		Ladder = 1,
		// Token: 0x04004E2C RID: 20012
		Pole = 2,
		// Token: 0x04004E2D RID: 20013
		Tube = 4,
		// Token: 0x04004E2E RID: 20014
		NavTeleporter = 8,
		// Token: 0x04004E2F RID: 20015
		UnderConstruction = 16
	}

	// Token: 0x020013AF RID: 5039
	public struct NavValidatorFlagsLadderIndexer
	{
		// Token: 0x1700067B RID: 1659
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Ladder) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Ladder, value);
			}
		}
	}

	// Token: 0x020013B0 RID: 5040
	public struct NavValidatorFlagsPoleIndexer
	{
		// Token: 0x1700067C RID: 1660
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Pole) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Pole, value);
			}
		}
	}

	// Token: 0x020013B1 RID: 5041
	public struct NavValidatorFlagsTubeIndexer
	{
		// Token: 0x1700067D RID: 1661
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Tube) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Tube, value);
			}
		}
	}

	// Token: 0x020013B2 RID: 5042
	public struct NavValidatorFlagsNavTeleporterIndexer
	{
		// Token: 0x1700067E RID: 1662
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.NavTeleporter) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.NavTeleporter, value);
			}
		}
	}

	// Token: 0x020013B3 RID: 5043
	public struct NavValidatorFlagsUnderConstructionIndexer
	{
		// Token: 0x1700067F RID: 1663
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.UnderConstruction) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.UnderConstruction, value);
			}
		}
	}

	// Token: 0x020013B4 RID: 5044
	[Flags]
	public enum NavFlags : byte
	{
		// Token: 0x04004E31 RID: 20017
		AccessDoor = 1,
		// Token: 0x04004E32 RID: 20018
		TubeEntrance = 2,
		// Token: 0x04004E33 RID: 20019
		PreventIdleTraversal = 4,
		// Token: 0x04004E34 RID: 20020
		Reserved = 8,
		// Token: 0x04004E35 RID: 20021
		SuitMarker = 16
	}

	// Token: 0x020013B5 RID: 5045
	public struct NavFlagsAccessDoorIndexer
	{
		// Token: 0x17000680 RID: 1664
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.AccessDoor) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.AccessDoor, value);
			}
		}
	}

	// Token: 0x020013B6 RID: 5046
	public struct NavFlagsTubeEntranceIndexer
	{
		// Token: 0x17000681 RID: 1665
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.TubeEntrance) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.TubeEntrance, value);
			}
		}
	}

	// Token: 0x020013B7 RID: 5047
	public struct NavFlagsPreventIdleTraversalIndexer
	{
		// Token: 0x17000682 RID: 1666
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.PreventIdleTraversal) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.PreventIdleTraversal, value);
			}
		}
	}

	// Token: 0x020013B8 RID: 5048
	public struct NavFlagsReservedIndexer
	{
		// Token: 0x17000683 RID: 1667
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.Reserved) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.Reserved, value);
			}
		}
	}

	// Token: 0x020013B9 RID: 5049
	public struct NavFlagsSuitMarkerIndexer
	{
		// Token: 0x17000684 RID: 1668
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.SuitMarker) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.SuitMarker, value);
			}
		}
	}

	// Token: 0x020013BA RID: 5050
	public struct Restriction
	{
		// Token: 0x04004E36 RID: 20022
		public const int DefaultID = -1;

		// Token: 0x04004E37 RID: 20023
		public Dictionary<int, Grid.Restriction.Directions> DirectionMasksForMinionInstanceID;

		// Token: 0x04004E38 RID: 20024
		public Grid.Restriction.Orientation orientation;

		// Token: 0x020013BB RID: 5051
		[Flags]
		public enum Directions : byte
		{
			// Token: 0x04004E3A RID: 20026
			Left = 1,
			// Token: 0x04004E3B RID: 20027
			Right = 2,
			// Token: 0x04004E3C RID: 20028
			Teleport = 4
		}

		// Token: 0x020013BC RID: 5052
		public enum Orientation : byte
		{
			// Token: 0x04004E3E RID: 20030
			Vertical,
			// Token: 0x04004E3F RID: 20031
			Horizontal,
			// Token: 0x04004E40 RID: 20032
			SingleCell
		}
	}

	// Token: 0x020013BD RID: 5053
	private struct TubeEntrance
	{
		// Token: 0x04004E41 RID: 20033
		public bool operational;

		// Token: 0x04004E42 RID: 20034
		public int reservationCapacity;

		// Token: 0x04004E43 RID: 20035
		public HashSet<int> reservedInstanceIDs;
	}

	// Token: 0x020013BE RID: 5054
	public struct SuitMarker
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060067D8 RID: 26584 RVA: 0x000E4015 File Offset: 0x000E2215
		public int emptyLockerCount
		{
			get
			{
				return this.lockerCount - this.suitCount;
			}
		}

		// Token: 0x04004E44 RID: 20036
		public int suitCount;

		// Token: 0x04004E45 RID: 20037
		public int lockerCount;

		// Token: 0x04004E46 RID: 20038
		public Grid.SuitMarker.Flags flags;

		// Token: 0x04004E47 RID: 20039
		public PathFinder.PotentialPath.Flags pathFlags;

		// Token: 0x04004E48 RID: 20040
		public HashSet<int> minionIDsWithSuitReservations;

		// Token: 0x04004E49 RID: 20041
		public HashSet<int> minionIDsWithEmptyLockerReservations;

		// Token: 0x020013BF RID: 5055
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x04004E4B RID: 20043
			OnlyTraverseIfUnequipAvailable = 1,
			// Token: 0x04004E4C RID: 20044
			Operational = 2,
			// Token: 0x04004E4D RID: 20045
			Rotated = 4
		}
	}

	// Token: 0x020013C0 RID: 5056
	public struct ObjectLayerIndexer
	{
		// Token: 0x17000686 RID: 1670
		public GameObject this[int cell, int layer]
		{
			get
			{
				GameObject result = null;
				Grid.ObjectLayers[layer].TryGetValue(cell, out result);
				return result;
			}
			set
			{
				if (value == null)
				{
					Grid.ObjectLayers[layer].Remove(cell);
				}
				else
				{
					Grid.ObjectLayers[layer][cell] = value;
				}
				GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.objectLayers[layer], value);
			}
		}
	}

	// Token: 0x020013C1 RID: 5057
	public struct PressureIndexer
	{
		// Token: 0x17000687 RID: 1671
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i] * 101.3f;
			}
		}
	}

	// Token: 0x020013C2 RID: 5058
	public struct TransparentIndexer
	{
		// Token: 0x17000688 RID: 1672
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 16) > 0;
			}
		}
	}

	// Token: 0x020013C3 RID: 5059
	public struct ElementIdxIndexer
	{
		// Token: 0x17000689 RID: 1673
		public unsafe ushort this[int i]
		{
			get
			{
				return Grid.elementIdx[i];
			}
		}
	}

	// Token: 0x020013C4 RID: 5060
	public struct TemperatureIndexer
	{
		// Token: 0x1700068A RID: 1674
		public unsafe float this[int i]
		{
			get
			{
				return Grid.temperature[i];
			}
		}
	}

	// Token: 0x020013C5 RID: 5061
	public struct RadiationIndexer
	{
		// Token: 0x1700068B RID: 1675
		public unsafe float this[int i]
		{
			get
			{
				return Grid.radiation[i];
			}
		}
	}

	// Token: 0x020013C6 RID: 5062
	public struct MassIndexer
	{
		// Token: 0x1700068C RID: 1676
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i];
			}
		}
	}

	// Token: 0x020013C7 RID: 5063
	public struct PropertiesIndexer
	{
		// Token: 0x1700068D RID: 1677
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.properties[i];
			}
		}
	}

	// Token: 0x020013C8 RID: 5064
	public struct ExposedToSunlightIndexer
	{
		// Token: 0x1700068E RID: 1678
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.exposedToSunlight[i];
			}
		}
	}

	// Token: 0x020013C9 RID: 5065
	public struct StrengthInfoIndexer
	{
		// Token: 0x1700068F RID: 1679
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.strengthInfo[i];
			}
		}
	}

	// Token: 0x020013CA RID: 5066
	public struct Insulationndexer
	{
		// Token: 0x17000690 RID: 1680
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.insulation[i];
			}
		}
	}

	// Token: 0x020013CB RID: 5067
	public struct DiseaseIdxIndexer
	{
		// Token: 0x17000691 RID: 1681
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.diseaseIdx[i];
			}
		}
	}

	// Token: 0x020013CC RID: 5068
	public struct DiseaseCountIndexer
	{
		// Token: 0x17000692 RID: 1682
		public unsafe int this[int i]
		{
			get
			{
				return Grid.diseaseCount[i];
			}
		}
	}

	// Token: 0x020013CD RID: 5069
	public struct AccumulatedFlowIndexer
	{
		// Token: 0x17000693 RID: 1683
		public unsafe float this[int i]
		{
			get
			{
				return Grid.AccumulatedFlowValues[i];
			}
		}
	}

	// Token: 0x020013CE RID: 5070
	public struct LightIntensityIndexer
	{
		// Token: 0x17000694 RID: 1684
		public unsafe int this[int i]
		{
			get
			{
				float num = Game.Instance.currentFallbackSunlightIntensity;
				WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[i]);
				if (world != null)
				{
					num = world.currentSunlightIntensity;
				}
				int num2 = (int)((float)Grid.exposedToSunlight[i] / 255f * num);
				int num3 = Grid.LightCount[i];
				return num2 + num3;
			}
		}
	}

	// Token: 0x020013CF RID: 5071
	public enum SceneLayer
	{
		// Token: 0x04004E4F RID: 20047
		WorldSelection = -3,
		// Token: 0x04004E50 RID: 20048
		NoLayer,
		// Token: 0x04004E51 RID: 20049
		Background,
		// Token: 0x04004E52 RID: 20050
		Backwall = 1,
		// Token: 0x04004E53 RID: 20051
		Gas,
		// Token: 0x04004E54 RID: 20052
		GasConduits,
		// Token: 0x04004E55 RID: 20053
		GasConduitBridges,
		// Token: 0x04004E56 RID: 20054
		LiquidConduits,
		// Token: 0x04004E57 RID: 20055
		LiquidConduitBridges,
		// Token: 0x04004E58 RID: 20056
		SolidConduits,
		// Token: 0x04004E59 RID: 20057
		SolidConduitContents,
		// Token: 0x04004E5A RID: 20058
		SolidConduitBridges,
		// Token: 0x04004E5B RID: 20059
		Wires,
		// Token: 0x04004E5C RID: 20060
		WireBridges,
		// Token: 0x04004E5D RID: 20061
		WireBridgesFront,
		// Token: 0x04004E5E RID: 20062
		LogicWires,
		// Token: 0x04004E5F RID: 20063
		LogicGates,
		// Token: 0x04004E60 RID: 20064
		LogicGatesFront,
		// Token: 0x04004E61 RID: 20065
		InteriorWall,
		// Token: 0x04004E62 RID: 20066
		GasFront,
		// Token: 0x04004E63 RID: 20067
		BuildingBack,
		// Token: 0x04004E64 RID: 20068
		Building,
		// Token: 0x04004E65 RID: 20069
		BuildingUse,
		// Token: 0x04004E66 RID: 20070
		BuildingFront,
		// Token: 0x04004E67 RID: 20071
		TransferArm,
		// Token: 0x04004E68 RID: 20072
		Ore,
		// Token: 0x04004E69 RID: 20073
		Creatures,
		// Token: 0x04004E6A RID: 20074
		Move,
		// Token: 0x04004E6B RID: 20075
		Front,
		// Token: 0x04004E6C RID: 20076
		GlassTile,
		// Token: 0x04004E6D RID: 20077
		Liquid,
		// Token: 0x04004E6E RID: 20078
		Ground,
		// Token: 0x04004E6F RID: 20079
		TileMain,
		// Token: 0x04004E70 RID: 20080
		TileFront,
		// Token: 0x04004E71 RID: 20081
		FXFront,
		// Token: 0x04004E72 RID: 20082
		FXFront2,
		// Token: 0x04004E73 RID: 20083
		SceneMAX
	}
}
