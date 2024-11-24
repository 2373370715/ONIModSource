using UnityEngine;

public class MoveToLocationTool : InterfaceTool
{
	public static MoveToLocationTool Instance;

	private Navigator targetNavigator;

	private Movable targetMovable;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		visualizer = Util.KInstantiate(visualizer);
	}

	public void Activate(Navigator navigator)
	{
		targetNavigator = navigator;
		targetMovable = null;
		PlayerController.Instance.ActivateTool(this);
	}

	public void Activate(Movable movable)
	{
		targetNavigator = null;
		targetMovable = movable;
		PlayerController.Instance.ActivateTool(this);
	}

	public bool CanMoveTo(int target_cell)
	{
		if (targetNavigator != null)
		{
			if (targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null)
			{
				return targetNavigator.CanReach(target_cell);
			}
			return false;
		}
		if (targetMovable != null)
		{
			return targetMovable.CanMoveTo(target_cell);
		}
		return false;
	}

	private void SetMoveToLocation(int target_cell)
	{
		if (targetNavigator != null)
		{
			targetNavigator.GetSMI<MoveToLocationMonitor.Instance>()?.MoveToLocation(target_cell);
		}
		else if (targetMovable != null)
		{
			targetMovable.MoveToLocation(target_cell);
		}
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		visualizer.gameObject.SetActive(value: true);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (targetNavigator != null && new_tool == SelectTool.Instance)
		{
			SelectTool.Instance.SelectNextFrame(targetNavigator.GetComponent<KSelectable>(), skipSound: true);
		}
		visualizer.gameObject.SetActive(value: false);
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (targetNavigator != null || targetMovable != null)
		{
			int mouseCell = DebugHandler.GetMouseCell();
			if (CanMoveTo(mouseCell))
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
				SetMoveToLocation(mouseCell);
				SelectTool.Instance.Activate();
			}
			else
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
			}
		}
	}

	private void RefreshColor()
	{
		Color c = new Color(0.91f, 0.21f, 0.2f);
		if (CanMoveTo(DebugHandler.GetMouseCell()))
		{
			c = Color.white;
		}
		SetColor(visualizer, c);
	}

	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		RefreshColor();
	}

	private void SetColor(GameObject root, Color c)
	{
		root.GetComponentInChildren<MeshRenderer>().material.color = c;
	}
}
