using System;
using System.Collections.Generic;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001709 RID: 5897
[AddComponentMenu("KMonoBehaviour/scripts/PlayerController")]
public class PlayerController : KMonoBehaviour, IInputHandler
{
	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x06007956 RID: 31062 RVA: 0x000EFCF1 File Offset: 0x000EDEF1
	public string handlerName
	{
		get
		{
			return "PlayerController";
		}
	}

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x06007957 RID: 31063 RVA: 0x000EFCF8 File Offset: 0x000EDEF8
	// (set) Token: 0x06007958 RID: 31064 RVA: 0x000EFD00 File Offset: 0x000EDF00
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06007959 RID: 31065 RVA: 0x000EFD09 File Offset: 0x000EDF09
	public InterfaceTool ActiveTool
	{
		get
		{
			return this.activeTool;
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x0600795A RID: 31066 RVA: 0x000EFD11 File Offset: 0x000EDF11
	// (set) Token: 0x0600795B RID: 31067 RVA: 0x000EFD18 File Offset: 0x000EDF18
	public static PlayerController Instance { get; private set; }

	// Token: 0x0600795C RID: 31068 RVA: 0x000EFD20 File Offset: 0x000EDF20
	public static void DestroyInstance()
	{
		PlayerController.Instance = null;
	}

	// Token: 0x0600795D RID: 31069 RVA: 0x00313F30 File Offset: 0x00312130
	protected override void OnPrefabInit()
	{
		PlayerController.Instance = this;
		InterfaceTool.InitializeConfigs(this.defaultConfigKey, this.interfaceConfigs);
		this.vim = UnityEngine.Object.FindObjectOfType<VirtualInputModule>(true);
		for (int i = 0; i < this.tools.Length; i++)
		{
			if (DlcManager.IsDlcListValidForCurrentContent(this.tools[i].DlcIDs))
			{
				GameObject gameObject = Util.KInstantiate(this.tools[i].gameObject, base.gameObject, null);
				this.tools[i] = gameObject.GetComponent<InterfaceTool>();
				this.tools[i].gameObject.SetActive(true);
				this.tools[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600795E RID: 31070 RVA: 0x000EFD28 File Offset: 0x000EDF28
	protected override void OnSpawn()
	{
		if (this.tools.Length == 0)
		{
			return;
		}
		this.ActivateTool(this.tools[0]);
	}

	// Token: 0x0600795F RID: 31071 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void InitializeConfigs()
	{
	}

	// Token: 0x06007960 RID: 31072 RVA: 0x000E7027 File Offset: 0x000E5227
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06007961 RID: 31073 RVA: 0x00313FD8 File Offset: 0x003121D8
	public static Vector3 GetCursorPos(Vector3 mouse_pos)
	{
		RaycastHit raycastHit;
		Vector3 vector;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out raycastHit, float.PositiveInfinity, Game.BlockSelectionLayerMask))
		{
			vector = raycastHit.point;
		}
		else
		{
			mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
			vector = Camera.main.ScreenToWorldPoint(mouse_pos);
		}
		float num = vector.x;
		float num2 = vector.y;
		num = Mathf.Max(num, 0f);
		num = Mathf.Min(num, Grid.WidthInMeters);
		num2 = Mathf.Max(num2, 0f);
		num2 = Mathf.Min(num2, Grid.HeightInMeters);
		vector.x = num;
		vector.y = num2;
		return vector;
	}

	// Token: 0x06007962 RID: 31074 RVA: 0x0031408C File Offset: 0x0031228C
	private void UpdateHover()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			this.activeTool.OnFocus(!current.IsPointerOverGameObject());
		}
	}

	// Token: 0x06007963 RID: 31075 RVA: 0x003140BC File Offset: 0x003122BC
	private void Update()
	{
		this.UpdateDrag();
		if (this.activeTool && this.activeTool.enabled)
		{
			this.UpdateHover();
			Vector3 cursorPos = this.GetCursorPos();
			if (cursorPos != this.prevMousePos)
			{
				this.prevMousePos = cursorPos;
				this.activeTool.OnMouseMove(cursorPos);
			}
		}
		if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
		{
			this.DebugHidingCursor = !this.DebugHidingCursor;
			Cursor.visible = !this.DebugHidingCursor;
			HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
		}
	}

	// Token: 0x06007964 RID: 31076 RVA: 0x000EFD42 File Offset: 0x000EDF42
	private void OnCleanup()
	{
		Global.GetInputManager().usedMenus.Remove(this);
	}

	// Token: 0x06007965 RID: 31077 RVA: 0x000EFD55 File Offset: 0x000EDF55
	private void LateUpdate()
	{
		if (this.queueStopDrag)
		{
			this.queueStopDrag = false;
			this.dragging = false;
			this.dragAction = global::Action.Invalid;
			this.dragDelta = Vector3.zero;
			this.worldDragDelta = Vector3.zero;
		}
	}

	// Token: 0x06007966 RID: 31078 RVA: 0x0031416C File Offset: 0x0031236C
	public void ActivateTool(InterfaceTool tool)
	{
		if (this.activeTool == tool)
		{
			return;
		}
		this.DeactivateTool(tool);
		this.activeTool = tool;
		this.activeTool.enabled = true;
		this.activeTool.gameObject.SetActive(true);
		this.activeTool.ActivateTool();
		this.UpdateHover();
	}

	// Token: 0x06007967 RID: 31079 RVA: 0x000EFD8A File Offset: 0x000EDF8A
	public void ToolDeactivated(InterfaceTool tool)
	{
		if (this.activeTool == tool && this.activeTool != null)
		{
			this.DeactivateTool(null);
		}
		if (this.activeTool == null)
		{
			this.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06007968 RID: 31080 RVA: 0x000EFDC8 File Offset: 0x000EDFC8
	private void DeactivateTool(InterfaceTool new_tool = null)
	{
		if (this.activeTool != null)
		{
			this.activeTool.enabled = false;
			this.activeTool.gameObject.SetActive(false);
			InterfaceTool interfaceTool = this.activeTool;
			this.activeTool = null;
			interfaceTool.DeactivateTool(new_tool);
		}
	}

	// Token: 0x06007969 RID: 31081 RVA: 0x000EFE08 File Offset: 0x000EE008
	public bool IsUsingDefaultTool()
	{
		return this.tools.Length != 0 && this.activeTool == this.tools[0];
	}

	// Token: 0x0600796A RID: 31082 RVA: 0x000EFE28 File Offset: 0x000EE028
	private void StartDrag(global::Action action)
	{
		if (this.dragAction == global::Action.Invalid)
		{
			this.dragAction = action;
			this.startDragPos = KInputManager.GetMousePos();
			this.startDragTime = Time.unscaledTime;
		}
	}

	// Token: 0x0600796B RID: 31083 RVA: 0x003141C4 File Offset: 0x003123C4
	private void UpdateDrag()
	{
		this.dragDelta = Vector2.zero;
		Vector3 mousePos = KInputManager.GetMousePos();
		if (!this.dragging && this.CanDrag() && ((mousePos - this.startDragPos).sqrMagnitude > 36f || Time.unscaledTime - this.startDragTime > 0.3f))
		{
			this.dragging = true;
		}
		if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad && this.dragging)
		{
			return;
		}
		if (this.dragging)
		{
			this.dragDelta = mousePos - this.startDragPos;
			this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
			this.startDragPos = mousePos;
		}
	}

	// Token: 0x0600796C RID: 31084 RVA: 0x000EFE4F File Offset: 0x000EE04F
	private void StopDrag(global::Action action)
	{
		if (this.dragAction == action)
		{
			this.queueStopDrag = true;
			if (KInputManager.currentControllerIsGamepad)
			{
				this.dragging = false;
			}
		}
	}

	// Token: 0x0600796D RID: 31085 RVA: 0x0031428C File Offset: 0x0031248C
	public void CancelDragging()
	{
		this.queueStopDrag = true;
		if (this.activeTool != null)
		{
			DragTool dragTool = this.activeTool as DragTool;
			if (dragTool != null)
			{
				dragTool.CancelDragging();
			}
		}
	}

	// Token: 0x0600796E RID: 31086 RVA: 0x000EFE6F File Offset: 0x000EE06F
	public void OnCancelInput()
	{
		this.CancelDragging();
	}

	// Token: 0x0600796F RID: 31087 RVA: 0x003142CC File Offset: 0x003124CC
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.ToggleScreenshotMode))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		if (DebugHandler.HideUI && e.TryConsume(global::Action.Escape))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StartDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StartDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StartDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
		pointerEventData.position = KInputManager.GetMousePos();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				return;
			}
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
		{
			this.activeTool.OnLeftClickDown(this.GetCursorPos());
			return;
		}
		if (e.IsAction(global::Action.MouseRight))
		{
			this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
			return;
		}
		this.activeTool.OnKeyDown(e);
	}

	// Token: 0x06007970 RID: 31088 RVA: 0x00314408 File Offset: 0x00312608
	public void OnKeyUp(KButtonEvent e)
	{
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StopDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StopDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StopDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		if (!this.activeTool.hasFocus)
		{
			return;
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
		else
		{
			if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
	}

	// Token: 0x06007971 RID: 31089 RVA: 0x000EFE77 File Offset: 0x000EE077
	public bool ConsumeIfNotDragging(KButtonEvent e, global::Action action)
	{
		return (this.dragAction != action || !this.dragging) && e.TryConsume(action);
	}

	// Token: 0x06007972 RID: 31090 RVA: 0x000EFE93 File Offset: 0x000EE093
	public bool IsDragging()
	{
		return this.dragging && this.CanDrag();
	}

	// Token: 0x06007973 RID: 31091 RVA: 0x000EFEA5 File Offset: 0x000EE0A5
	public bool CanDrag()
	{
		return this.draggingAllowed && this.dragAction > global::Action.Invalid;
	}

	// Token: 0x06007974 RID: 31092 RVA: 0x000EFEBA File Offset: 0x000EE0BA
	public void AllowDragging(bool allow)
	{
		this.draggingAllowed = allow;
	}

	// Token: 0x06007975 RID: 31093 RVA: 0x000EFEC3 File Offset: 0x000EE0C3
	public Vector3 GetDragDelta()
	{
		return this.dragDelta;
	}

	// Token: 0x06007976 RID: 31094 RVA: 0x000EFECB File Offset: 0x000EE0CB
	public Vector3 GetWorldDragDelta()
	{
		if (!this.draggingAllowed)
		{
			return Vector3.zero;
		}
		return this.worldDragDelta;
	}

	// Token: 0x04005AFC RID: 23292
	[SerializeField]
	private global::Action defaultConfigKey;

	// Token: 0x04005AFD RID: 23293
	[SerializeField]
	private List<InterfaceToolConfig> interfaceConfigs;

	// Token: 0x04005AFF RID: 23295
	public InterfaceTool[] tools;

	// Token: 0x04005B00 RID: 23296
	private InterfaceTool activeTool;

	// Token: 0x04005B01 RID: 23297
	public VirtualInputModule vim;

	// Token: 0x04005B03 RID: 23299
	private bool DebugHidingCursor;

	// Token: 0x04005B04 RID: 23300
	private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0f, 0f);

	// Token: 0x04005B05 RID: 23301
	private const float MIN_DRAG_DIST_SQR = 36f;

	// Token: 0x04005B06 RID: 23302
	private const float MIN_DRAG_TIME = 0.3f;

	// Token: 0x04005B07 RID: 23303
	private global::Action dragAction;

	// Token: 0x04005B08 RID: 23304
	private bool draggingAllowed = true;

	// Token: 0x04005B09 RID: 23305
	private bool dragging;

	// Token: 0x04005B0A RID: 23306
	private bool queueStopDrag;

	// Token: 0x04005B0B RID: 23307
	private Vector3 startDragPos;

	// Token: 0x04005B0C RID: 23308
	private float startDragTime;

	// Token: 0x04005B0D RID: 23309
	private Vector3 dragDelta;

	// Token: 0x04005B0E RID: 23310
	private Vector3 worldDragDelta;
}
