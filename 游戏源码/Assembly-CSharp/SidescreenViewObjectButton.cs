using System;
using UnityEngine;

public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
	public enum Mode
	{
		Target,
		Cell
	}

	public string Text;

	public string Tooltip;

	public Mode TrackMode;

	public GameObject Target;

	public int TargetCell;

	public int horizontalGroupID = -1;

	public string SidescreenButtonText => Text;

	public string SidescreenButtonTooltip => Tooltip;

	public bool IsValid()
	{
		return TrackMode switch
		{
			Mode.Cell => Grid.IsValidCell(TargetCell), 
			Mode.Target => Target != null, 
			_ => false, 
		};
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	public bool SidescreenEnabled()
	{
		return true;
	}

	public bool SidescreenButtonInteractable()
	{
		return IsValid();
	}

	public int HorizontalGroupID()
	{
		return horizontalGroupID;
	}

	public void OnSidescreenButtonPressed()
	{
		if (IsValid())
		{
			switch (TrackMode)
			{
			case Mode.Cell:
				CameraController.Instance.CameraGoTo(Grid.CellToPos(TargetCell));
				break;
			case Mode.Target:
				CameraController.Instance.CameraGoTo(Target.transform.GetPosition());
				break;
			}
		}
		else
		{
			base.gameObject.Trigger(1980521255);
		}
	}

	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}
}
