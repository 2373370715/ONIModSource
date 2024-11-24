using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTool : InterfaceTool
{
	private enum DragAxis
	{
		Invalid = -1,
		None,
		Horizontal,
		Vertical
	}

	public enum Mode
	{
		Brush,
		Box,
		Line
	}

	[SerializeField]
	private Texture2D boxCursor;

	[SerializeField]
	private GameObject areaVisualizer;

	[SerializeField]
	private GameObject areaVisualizerTextPrefab;

	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	protected SpriteRenderer areaVisualizerSpriteRenderer;

	protected Guid areaVisualizerText;

	protected Vector3 placementPivot;

	protected bool interceptNumberKeysForPriority;

	private bool dragging;

	private Vector3 previousCursorPos;

	private Mode mode = Mode.Box;

	private DragAxis dragAxis = DragAxis.Invalid;

	protected bool canChangeDragAxis = true;

	protected int lineModeMaxLength = -1;

	protected Vector3 downPos;

	private bool cellChangedSinceDown;

	private VirtualInputModule currentVirtualInputInUse;

	public bool Dragging => dragging;

	protected virtual Mode GetMode()
	{
		return mode;
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		dragging = false;
		SetMode(mode);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(state: true);
		if (KInputManager.currentControllerIsGamepad)
		{
			SetCurrentVirtualInputModuleMousMovementMode(mouseMovementOnly: false);
		}
		RemoveCurrentAreaText();
		base.OnDeactivateTool(new_tool);
	}

	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, OnTutorialOpened);
		base.OnPrefabInit();
		if (visualizer != null)
		{
			visualizer = Util.KInstantiate(visualizer);
		}
		if (areaVisualizer != null)
		{
			areaVisualizer = Util.KInstantiate(areaVisualizer);
			areaVisualizer.SetActive(value: false);
			areaVisualizerSpriteRenderer = areaVisualizer.GetComponent<SpriteRenderer>();
			areaVisualizer.transform.SetParent(base.transform);
			areaVisualizer.GetComponent<Renderer>().material.color = areaColour;
		}
	}

	protected override void OnCmpEnable()
	{
		dragging = false;
	}

	protected override void OnCmpDisable()
	{
		if (visualizer != null)
		{
			visualizer.SetActive(value: false);
		}
		if (areaVisualizer != null)
		{
			areaVisualizer.SetActive(value: false);
		}
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos = ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		dragging = true;
		downPos = cursor_pos;
		cellChangedSinceDown = false;
		previousCursorPos = cursor_pos;
		if (currentVirtualInputInUse != null)
		{
			currentVirtualInputInUse.mouseMovementOnly = false;
			currentVirtualInputInUse = null;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(state: false);
		}
		else
		{
			_ = UnityEngine.EventSystems.EventSystem.current;
			SetCurrentVirtualInputModuleMousMovementMode(mouseMovementOnly: true, delegate(VirtualInputModule module)
			{
				currentVirtualInputInUse = module;
			});
		}
		hasFocus = true;
		RemoveCurrentAreaText();
		if (areaVisualizerTextPrefab != null)
		{
			areaVisualizerText = NameDisplayScreen.Instance.AddAreaText("", areaVisualizerTextPrefab);
			NameDisplayScreen.Instance.GetWorldText(areaVisualizerText).GetComponent<LocText>().color = areaColour;
		}
		switch (GetMode())
		{
		case Mode.Brush:
			if (visualizer != null)
			{
				AddDragPoint(cursor_pos);
			}
			break;
		case Mode.Box:
		case Mode.Line:
			if (visualizer != null)
			{
				visualizer.SetActive(value: false);
			}
			if (areaVisualizer != null)
			{
				areaVisualizer.SetActive(value: true);
				areaVisualizer.transform.SetPosition(cursor_pos);
				areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
			}
			break;
		}
	}

	public void RemoveCurrentAreaText()
	{
		if (areaVisualizerText != Guid.Empty)
		{
			NameDisplayScreen.Instance.RemoveWorldText(areaVisualizerText);
			areaVisualizerText = Guid.Empty;
		}
	}

	public void CancelDragging()
	{
		KScreenManager.Instance.SetEventSystemEnabled(state: true);
		if (currentVirtualInputInUse != null)
		{
			currentVirtualInputInUse.mouseMovementOnly = false;
			currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			SetCurrentVirtualInputModuleMousMovementMode(mouseMovementOnly: false);
		}
		dragAxis = DragAxis.Invalid;
		if (dragging)
		{
			dragging = false;
			RemoveCurrentAreaText();
			Mode mode = GetMode();
			if ((mode == Mode.Box || mode == Mode.Line) && areaVisualizer != null)
			{
				areaVisualizer.SetActive(value: false);
			}
		}
	}

	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		KScreenManager.Instance.SetEventSystemEnabled(state: true);
		if (currentVirtualInputInUse != null)
		{
			currentVirtualInputInUse.mouseMovementOnly = false;
			currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			SetCurrentVirtualInputModuleMousMovementMode(mouseMovementOnly: false);
		}
		dragAxis = DragAxis.Invalid;
		if (!dragging)
		{
			return;
		}
		dragging = false;
		cursor_pos = ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		RemoveCurrentAreaText();
		Mode mode = GetMode();
		if (mode == Mode.Line || Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(Action.DragStraight)))
		{
			cursor_pos = SnapToLine(cursor_pos);
		}
		if ((mode != Mode.Box && mode != Mode.Line) || !(areaVisualizer != null))
		{
			return;
		}
		areaVisualizer.SetActive(value: false);
		Grid.PosToXY(downPos, out var x, out var y);
		int num = x;
		int num2 = y;
		Grid.PosToXY(cursor_pos, out var x2, out var y2);
		if (x2 < x)
		{
			Util.Swap(ref x, ref x2);
		}
		if (y2 < y)
		{
			Util.Swap(ref y, ref y2);
		}
		for (int i = y; i <= y2; i++)
		{
			for (int j = x; j <= x2; j++)
			{
				int cell = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
				{
					int value = i - num2;
					int value2 = j - num;
					value = Mathf.Abs(value);
					value2 = Mathf.Abs(value2);
					OnDragTool(cell, value + value2);
				}
			}
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(GetConfirmSound()));
		OnDragComplete(downPos, cursor_pos);
	}

	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
	{
		position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
		position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
		return position;
	}

	protected Vector3 SnapToLine(Vector3 cursorPos)
	{
		Vector3 vector = cursorPos - downPos;
		if (canChangeDragAxis || (!canChangeDragAxis && !cellChangedSinceDown) || dragAxis == DragAxis.Invalid)
		{
			dragAxis = DragAxis.Invalid;
			if (Mathf.Abs(vector.x) < Mathf.Abs(vector.y))
			{
				dragAxis = DragAxis.Vertical;
			}
			else
			{
				dragAxis = DragAxis.Horizontal;
			}
		}
		switch (dragAxis)
		{
		case DragAxis.Horizontal:
			cursorPos.y = downPos.y;
			if (lineModeMaxLength != -1 && Mathf.Abs(vector.x) > (float)(lineModeMaxLength - 1))
			{
				cursorPos.x = downPos.x + Mathf.Sign(vector.x) * (float)(lineModeMaxLength - 1);
			}
			break;
		case DragAxis.Vertical:
			cursorPos.x = downPos.x;
			if (lineModeMaxLength != -1 && Mathf.Abs(vector.y) > (float)(lineModeMaxLength - 1))
			{
				cursorPos.y = downPos.y + Mathf.Sign(vector.y) * (float)(lineModeMaxLength - 1);
			}
			break;
		}
		return cursorPos;
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (dragging && (Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(Action.DragStraight)) || GetMode() == Mode.Line))
		{
			cursorPos = SnapToLine(cursorPos);
		}
		else
		{
			dragAxis = DragAxis.Invalid;
		}
		base.OnMouseMove(cursorPos);
		if (!dragging)
		{
			return;
		}
		if (Grid.PosToCell(cursorPos) != Grid.PosToCell(downPos))
		{
			cellChangedSinceDown = true;
		}
		switch (GetMode())
		{
		case Mode.Brush:
			AddDragPoints(cursorPos, previousCursorPos);
			if (areaVisualizerText != Guid.Empty)
			{
				int dragLength = GetDragLength();
				LocText component2 = NameDisplayScreen.Instance.GetWorldText(areaVisualizerText).GetComponent<LocText>();
				component2.text = string.Format(UI.TOOLS.TOOL_LENGTH_FMT, dragLength);
				Vector3 position3 = Grid.CellToPos(Grid.PosToCell(cursorPos));
				position3 += new Vector3(0f, 1f, 0f);
				component2.transform.SetPosition(position3);
			}
			break;
		case Mode.Box:
		case Mode.Line:
		{
			Vector2 input = Vector3.Max(downPos, cursorPos);
			Vector2 input2 = Vector3.Min(downPos, cursorPos);
			input = GetWorldRestrictedPosition(input);
			input2 = GetWorldRestrictedPosition(input2);
			input = GetRegularizedPos(input, minimize: false);
			input2 = GetRegularizedPos(input2, minimize: true);
			Vector2 vector = input - input2;
			Vector2 vector2 = (input + input2) * 0.5f;
			areaVisualizer.transform.SetPosition(new Vector2(vector2.x, vector2.y));
			int num = (int)(input.x - input2.x + (input.y - input2.y) - 1f);
			if (areaVisualizerSpriteRenderer.size != vector)
			{
				string sound = GlobalAssets.GetSound(GetDragSound());
				if (sound != null)
				{
					Vector3 position = areaVisualizer.transform.GetPosition();
					position.z = 0f;
					EventInstance instance = SoundEvent.BeginOneShot(sound, position);
					instance.setParameterByName("tileCount", num);
					SoundEvent.EndOneShot(instance);
				}
			}
			areaVisualizerSpriteRenderer.size = vector;
			if (areaVisualizerText != Guid.Empty)
			{
				Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
				LocText component = NameDisplayScreen.Instance.GetWorldText(areaVisualizerText).GetComponent<LocText>();
				component.text = string.Format(UI.TOOLS.TOOL_AREA_FMT, vector2I.x, vector2I.y, vector2I.x * vector2I.y);
				TransformExtensions.SetPosition(position: vector2, transform: component.transform);
			}
			break;
		}
		}
		previousCursorPos = cursorPos;
	}

	protected virtual void OnDragTool(int cell, int distFromOrigin)
	{
	}

	protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
	{
	}

	protected virtual int GetDragLength()
	{
		return 0;
	}

	private void AddDragPoint(Vector3 cursorPos)
	{
		cursorPos = ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			OnDragTool(cell, 0);
		}
	}

	private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
	{
		cursorPos = ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		Vector3 vector = cursorPos - previousCursorPos;
		float magnitude = vector.magnitude;
		float num = Grid.CellSizeInMeters * 0.25f;
		int num2 = 1 + (int)(magnitude / num);
		vector.Normalize();
		for (int i = 0; i < num2; i++)
		{
			Vector3 cursorPos2 = previousCursorPos + vector * ((float)i * num);
			AddDragPoint(cursorPos2);
		}
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (interceptNumberKeysForPriority)
		{
			HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	public override void OnKeyUp(KButtonEvent e)
	{
		if (interceptNumberKeysForPriority)
		{
			HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		Action action = e.GetAction();
		if (Action.Plan1 <= action && action <= Action.Plan10 && e.TryConsume(action))
		{
			int num = (int)(action - 36 + 1);
			if (num <= 9)
			{
				ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), play_sound: true);
			}
			else
			{
				ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), play_sound: true);
			}
		}
	}

	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		Action action = e.GetAction();
		if (Action.Plan1 <= action && action <= Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	protected void SetMode(Mode newMode)
	{
		mode = newMode;
		switch (mode)
		{
		case Mode.Brush:
			if (areaVisualizer != null)
			{
				areaVisualizer.SetActive(value: false);
			}
			if (visualizer != null)
			{
				visualizer.SetActive(value: true);
			}
			SetCursor(cursor, cursorOffset, CursorMode.Auto);
			break;
		case Mode.Box:
			if (visualizer != null)
			{
				visualizer.SetActive(value: true);
			}
			mode = Mode.Box;
			SetCursor(boxCursor, cursorOffset, CursorMode.Auto);
			break;
		case Mode.Line:
			if (visualizer != null)
			{
				visualizer.SetActive(value: true);
			}
			mode = Mode.Line;
			SetCursor(boxCursor, cursorOffset, CursorMode.Auto);
			break;
		}
	}

	public override void OnFocus(bool focus)
	{
		switch (GetMode())
		{
		case Mode.Brush:
			if (visualizer != null)
			{
				visualizer.SetActive(focus);
			}
			hasFocus = focus;
			break;
		case Mode.Box:
		case Mode.Line:
			if (visualizer != null && !dragging)
			{
				visualizer.SetActive(focus);
			}
			hasFocus = focus || dragging;
			break;
		}
	}

	private void OnTutorialOpened(object data)
	{
		dragging = false;
	}

	public override bool ShowHoverUI()
	{
		if (!dragging)
		{
			return base.ShowHoverUI();
		}
		return true;
	}
}
