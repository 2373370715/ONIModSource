using System.Collections.Generic;
using UnityEngine;

public class DoorTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	private List<INavDoor> doors = new List<INavDoor>();

	public DoorTransitionLayer(Navigator navigator)
		: base(navigator)
	{
	}

	private bool AreAllDoorsOpen()
	{
		foreach (INavDoor door in doors)
		{
			if (door != null && !door.IsOpen())
			{
				return false;
			}
		}
		return true;
	}

	protected override bool IsOverrideComplete()
	{
		if (base.IsOverrideComplete())
		{
			return AreAllDoorsOpen();
		}
		return false;
	}

	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (doors.Count > 0)
		{
			return;
		}
		int cell = Grid.PosToCell(navigator);
		int cell2 = Grid.OffsetCell(cell, transition.x, transition.y);
		AddDoor(cell2);
		if (navigator.CurrentNavType != NavType.Tube)
		{
			AddDoor(Grid.CellAbove(cell2));
		}
		for (int i = 0; i < transition.navGridTransition.voidOffsets.Length; i++)
		{
			int cell3 = Grid.OffsetCell(cell, transition.navGridTransition.voidOffsets[i]);
			AddDoor(cell3);
		}
		if (doors.Count == 0)
		{
			return;
		}
		if (!AreAllDoorsOpen())
		{
			base.BeginTransition(navigator, transition);
			transition.anim = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
			transition.start = originalTransition.start;
			transition.end = originalTransition.start;
		}
		foreach (INavDoor door in doors)
		{
			door.Open();
		}
	}

	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (doors.Count == 0)
		{
			return;
		}
		foreach (INavDoor door in doors)
		{
			if (!door.IsNullOrDestroyed())
			{
				door.Close();
			}
		}
		doors.Clear();
	}

	private void AddDoor(int cell)
	{
		INavDoor door = GetDoor(cell);
		if (!door.IsNullOrDestroyed() && !doors.Contains(door))
		{
			doors.Add(door);
		}
	}

	private INavDoor GetDoor(int cell)
	{
		if (!Grid.HasDoor[cell])
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			INavDoor navDoor = gameObject.GetComponent<INavDoor>();
			if (navDoor == null)
			{
				navDoor = gameObject.GetSMI<INavDoor>();
			}
			if (navDoor != null && navDoor.isSpawned)
			{
				return navDoor;
			}
		}
		return null;
	}
}
