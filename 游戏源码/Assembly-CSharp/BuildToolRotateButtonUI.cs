using STRINGS;
using UnityEngine;

public class BuildToolRotateButtonUI : MonoBehaviour
{
	[SerializeField]
	protected KButton button;

	[SerializeField]
	protected ToolTip tooltip;

	private void Awake()
	{
		tooltip.refreshWhileHovering = true;
		tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
		button.onClick += delegate
		{
			BuildTool.Instance.TryRotate();
		};
		UpdateTooltip(can_rotate: false);
	}

	private void Update()
	{
		bool flag = BuildTool.Instance.CanRotate();
		UpdateTooltip(flag);
		if (button.isInteractable != flag)
		{
			button.isInteractable = flag;
		}
	}

	private void UpdateTooltip(bool can_rotate)
	{
		PermittedRotations? permittedRotations = BuildTool.Instance.GetPermittedRotations();
		if (!permittedRotations.HasValue)
		{
			can_rotate = false;
		}
		if (can_rotate)
		{
			LocString locString = UI.BUILDTOOL_ROTATE;
			string feedbackString = GetFeedbackString(permittedRotations.Value, BuildTool.Instance.GetBuildingOrientation);
			if (feedbackString != null)
			{
				locString = string.Concat(locString, "\n\n ", feedbackString);
			}
			tooltip.SetSimpleTooltip(locString);
		}
		else
		{
			tooltip.SetSimpleTooltip(UI.BUILDTOOL_CANT_ROTATE);
		}
	}

	private string GetFeedbackString(PermittedRotations permitted_rotations, Orientation current_rotation)
	{
		switch (permitted_rotations)
		{
		case PermittedRotations.R360:
			switch (current_rotation)
			{
			case Orientation.Neutral:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "0");
			case Orientation.R90:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "90");
			case Orientation.R180:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "180");
			case Orientation.R270:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "270");
			}
			break;
		case PermittedRotations.R90:
			switch (current_rotation)
			{
			case Orientation.Neutral:
				return UI.BUILDTOOL_ROTATE_CURRENT_UPRIGHT;
			case Orientation.R90:
				return UI.BUILDTOOL_ROTATE_CURRENT_ON_SIDE;
			}
			break;
		case PermittedRotations.FlipH:
			switch (current_rotation)
			{
			case Orientation.Neutral:
				return UI.BUILDTOOL_ROTATE_CURRENT_RIGHT;
			case Orientation.FlipH:
				return UI.BUILDTOOL_ROTATE_CURRENT_LEFT;
			}
			break;
		case PermittedRotations.FlipV:
			switch (current_rotation)
			{
			case Orientation.Neutral:
				return UI.BUILDTOOL_ROTATE_CURRENT_UP;
			case Orientation.FlipV:
				return UI.BUILDTOOL_ROTATE_CURRENT_DOWN;
			}
			break;
		}
		DebugUtil.DevLogError($"Unexpected rotation value for tooltip (permitted_rotations: {permitted_rotations}, current_rotation: {current_rotation})");
		return null;
	}
}
