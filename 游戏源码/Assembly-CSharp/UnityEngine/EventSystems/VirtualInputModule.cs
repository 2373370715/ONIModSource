using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	// Token: 0x020020D9 RID: 8409
	[AddComponentMenu("Event/Virtual Input Module")]
	public class VirtualInputModule : PointerInputModule, IInputHandler
	{
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600B2D5 RID: 45781 RVA: 0x00114180 File Offset: 0x00112380
		public string handlerName
		{
			get
			{
				return "VirtualCursorInput";
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600B2D6 RID: 45782 RVA: 0x00114187 File Offset: 0x00112387
		// (set) Token: 0x0600B2D7 RID: 45783 RVA: 0x0011418F File Offset: 0x0011238F
		public KInputHandler inputHandler { get; set; }

		// Token: 0x0600B2D8 RID: 45784 RVA: 0x00438DB4 File Offset: 0x00436FB4
		protected VirtualInputModule()
		{
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600B2D9 RID: 45785 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public VirtualInputModule.InputMode inputMode
		{
			get
			{
				return VirtualInputModule.InputMode.Mouse;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600B2DA RID: 45786 RVA: 0x00114198 File Offset: 0x00112398
		// (set) Token: 0x0600B2DB RID: 45787 RVA: 0x001141A0 File Offset: 0x001123A0
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600B2DC RID: 45788 RVA: 0x00114198 File Offset: 0x00112398
		// (set) Token: 0x0600B2DD RID: 45789 RVA: 0x001141A0 File Offset: 0x001123A0
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600B2DE RID: 45790 RVA: 0x001141A9 File Offset: 0x001123A9
		// (set) Token: 0x0600B2DF RID: 45791 RVA: 0x001141B1 File Offset: 0x001123B1
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x0600B2E0 RID: 45792 RVA: 0x001141BA File Offset: 0x001123BA
		// (set) Token: 0x0600B2E1 RID: 45793 RVA: 0x001141C2 File Offset: 0x001123C2
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x0600B2E2 RID: 45794 RVA: 0x001141CB File Offset: 0x001123CB
		// (set) Token: 0x0600B2E3 RID: 45795 RVA: 0x001141D3 File Offset: 0x001123D3
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x0600B2E4 RID: 45796 RVA: 0x001141DC File Offset: 0x001123DC
		// (set) Token: 0x0600B2E5 RID: 45797 RVA: 0x001141E4 File Offset: 0x001123E4
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x0600B2E6 RID: 45798 RVA: 0x001141ED File Offset: 0x001123ED
		// (set) Token: 0x0600B2E7 RID: 45799 RVA: 0x001141F5 File Offset: 0x001123F5
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x0600B2E8 RID: 45800 RVA: 0x001141FE File Offset: 0x001123FE
		// (set) Token: 0x0600B2E9 RID: 45801 RVA: 0x00114206 File Offset: 0x00112406
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x0600B2EA RID: 45802 RVA: 0x0011420F File Offset: 0x0011240F
		public void SetCursor(Texture2D tex)
		{
			this.UpdateModule();
			if (this.m_VirtualCursor)
			{
				this.m_VirtualCursor.GetComponent<RawImage>().texture = tex;
			}
		}

		// Token: 0x0600B2EB RID: 45803 RVA: 0x00438E2C File Offset: 0x0043702C
		public override void UpdateModule()
		{
			GameInputManager inputManager = Global.GetInputManager();
			if (inputManager.GetControllerCount() <= 1)
			{
				return;
			}
			if (this.inputHandler == null || !this.inputHandler.UsesController(this, inputManager.GetController(1)))
			{
				KInputHandler.Add(inputManager.GetController(1), this, int.MaxValue);
				if (!inputManager.usedMenus.Contains(this))
				{
					inputManager.usedMenus.Add(this);
				}
				this.debugName = SceneManager.GetActiveScene().name + "-VirtualInputModule";
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (CameraController.Instance != null)
			{
				this.m_canvasCamera.CopyFrom(CameraController.Instance.overlayCamera);
			}
			else if (this.CursorCanvasShouldBeOverlay)
			{
				this.m_canvasCamera.CopyFrom(GameObject.Find("FrontEndCamera").GetComponent<Camera>());
			}
			if (this.m_canvasCamera != null && this.VCcam == null)
			{
				this.VCcam = GameObject.Find("VirtualCursorCamera").GetComponent<Camera>();
				if (this.VCcam != null)
				{
					if (this.m_virtualCursorCanvas == null)
					{
						this.m_virtualCursorCanvas = GameObject.Find("VirtualCursorCanvas").GetComponent<Canvas>();
						this.m_virtualCursorScaler = this.m_virtualCursorCanvas.GetComponent<CanvasScaler>();
					}
					if (this.CursorCanvasShouldBeOverlay)
					{
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
						this.VCcam.orthographic = false;
					}
					else
					{
						this.VCcam.orthographic = this.m_canvasCamera.orthographic;
						this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
						this.VCcam.transform.position = this.m_canvasCamera.transform.position;
						this.VCcam.enabled = true;
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceCamera;
						this.m_virtualCursorCanvas.worldCamera = this.VCcam;
					}
				}
			}
			if (this.m_canvasCamera != null && this.VCcam != null)
			{
				this.VCcam.orthographic = this.m_canvasCamera.orthographic;
				this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
				this.VCcam.transform.position = this.m_canvasCamera.transform.position;
				this.VCcam.aspect = this.m_canvasCamera.aspect;
				this.VCcam.enabled = true;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			if (this.m_virtualCursorScaler != null && this.m_virtualCursorScaler.referenceResolution != vector)
			{
				this.m_virtualCursorScaler.referenceResolution = vector;
			}
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_VirtualCursor.localScale = Vector2.one;
			Vector2 steamCursorMovement = KInputManager.steamInputInterpreter.GetSteamCursorMovement();
			float num = 1f / (4500f / vector.x);
			steamCursorMovement.x *= num;
			steamCursorMovement.y *= num;
			this.m_VirtualCursor.anchoredPosition += steamCursorMovement * this.m_VirtualCursorSpeed;
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, vector.x), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, vector.y));
			KInputManager.virtualCursorPos = new Vector3F(this.m_VirtualCursor.anchoredPosition.x, this.m_VirtualCursor.anchoredPosition.y, 0f);
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
		}

		// Token: 0x0600B2EC RID: 45804 RVA: 0x00114235 File Offset: 0x00112435
		public override bool IsModuleSupported()
		{
			return this.m_ForceModuleActive || Input.mousePresent;
		}

		// Token: 0x0600B2ED RID: 45805 RVA: 0x0043922C File Offset: 0x0043742C
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return true;
			}
			bool forceModuleActive = this.m_ForceModuleActive;
			Input.GetButtonDown(this.m_SubmitButton);
			return forceModuleActive | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f) | (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f | Input.GetMouseButtonDown(0);
		}

		// Token: 0x0600B2EE RID: 45806 RVA: 0x004392C4 File Offset: 0x004374C4
		public override void ActivateModule()
		{
			base.ActivateModule();
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (Input.mousePosition.x > 0f && Input.mousePosition.x < (float)Screen.width && Input.mousePosition.y > 0f && Input.mousePosition.y < (float)Screen.height)
			{
				this.m_VirtualCursor.anchoredPosition = Input.mousePosition;
			}
			else
			{
				this.m_VirtualCursor.anchoredPosition = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			}
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, (float)Screen.width), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, (float)Screen.height));
			this.m_VirtualCursor.localScale = Vector2.zero;
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
			this.m_LastMousePosition = this.m_VirtualCursor.anchoredPosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = GameObject.Find("FrontEndCamera").GetComponent<Camera>();
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x0600B2EF RID: 45807 RVA: 0x00439480 File Offset: 0x00437680
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
			this.conButtonStates.affirmativeDown = false;
			this.conButtonStates.affirmativeHoldTime = 0f;
			this.conButtonStates.negativeDown = false;
			this.conButtonStates.negativeHoldTime = 0f;
		}

		// Token: 0x0600B2F0 RID: 45808 RVA: 0x004394D4 File Offset: 0x004376D4
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			this.ProcessMouseEvent();
		}

		// Token: 0x0600B2F1 RID: 45809 RVA: 0x00439514 File Offset: 0x00437714
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x0600B2F2 RID: 45810 RVA: 0x0043958C File Offset: 0x0043778C
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x0600B2F3 RID: 45811 RVA: 0x00439644 File Offset: 0x00437844
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x0600B2F4 RID: 45812 RVA: 0x00114246 File Offset: 0x00112446
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x0600B2F5 RID: 45813 RVA: 0x00439758 File Offset: 0x00437958
		protected void ProcessMouseEvent(int id)
		{
			if (this.mouseMovementOnly)
			{
				return;
			}
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			this.ProcessControllerPress(eventData, true);
			this.ProcessControllerPress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData, false);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x0600B2F6 RID: 45814 RVA: 0x00439848 File Offset: 0x00437A48
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x0600B2F7 RID: 45815 RVA: 0x00439890 File Offset: 0x00437A90
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						PointerEventData pointerEventData = buttonData;
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x0600B2F8 RID: 45816 RVA: 0x00439A9C File Offset: 0x00437C9C
		public void OnKeyDown(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					if (this.conButtonStates.affirmativeDown)
					{
						this.conButtonStates.affirmativeHoldTime = this.conButtonStates.affirmativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.affirmativeDown)
					{
						this.leftFirstClick = true;
						this.leftReleased = false;
					}
					this.conButtonStates.affirmativeDown = true;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					if (this.conButtonStates.negativeDown)
					{
						this.conButtonStates.negativeHoldTime = this.conButtonStates.negativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.negativeDown)
					{
						this.rightFirstClick = true;
						this.rightReleased = false;
					}
					this.conButtonStates.negativeDown = true;
				}
			}
		}

		// Token: 0x0600B2F9 RID: 45817 RVA: 0x00439B60 File Offset: 0x00437D60
		public void OnKeyUp(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					this.conButtonStates.affirmativeHoldTime = 0f;
					this.leftReleased = true;
					this.leftFirstClick = false;
					this.conButtonStates.affirmativeDown = false;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					this.conButtonStates.negativeHoldTime = 0f;
					this.rightReleased = true;
					this.rightFirstClick = false;
					this.conButtonStates.negativeDown = false;
				}
			}
		}

		// Token: 0x0600B2FA RID: 45818 RVA: 0x00439BE4 File Offset: 0x00437DE4
		protected void ProcessControllerPress(PointerInputModule.MouseButtonEventData data, bool leftClick)
		{
			if (this.leftClickData == null)
			{
				this.leftClickData = data.buttonData;
			}
			if (this.rightClickData == null)
			{
				this.rightClickData = data.buttonData;
			}
			if (leftClick)
			{
				PointerEventData buttonData = data.buttonData;
				GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				if (this.leftFirstClick)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					buttonData.eligibleForClick = true;
					buttonData.delta = Vector2.zero;
					buttonData.dragging = false;
					buttonData.useDragThreshold = true;
					buttonData.pressPosition = buttonData.position;
					buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
					buttonData.position = new Vector2(KInputManager.virtualCursorPos.x, KInputManager.virtualCursorPos.y);
					base.DeselectIfSelectionChanged(gameObject, buttonData);
					GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
					float unscaledTime = Time.unscaledTime;
					if (gameObject2 == buttonData.lastPress)
					{
						if (unscaledTime - buttonData.clickTime < 0.3f)
						{
							PointerEventData pointerEventData = buttonData;
							int clickCount = pointerEventData.clickCount + 1;
							pointerEventData.clickCount = clickCount;
						}
						else
						{
							buttonData.clickCount = 1;
						}
						buttonData.clickTime = unscaledTime;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.pointerPress = gameObject2;
					buttonData.rawPointerPress = gameObject;
					buttonData.clickTime = unscaledTime;
					buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
					if (buttonData.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
					}
					this.leftFirstClick = false;
					return;
				}
				if (this.leftReleased)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData.pointerDrag != null && buttonData.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
					}
					buttonData.eligibleForClick = false;
					buttonData.pointerPress = null;
					buttonData.rawPointerPress = null;
					if (buttonData.pointerDrag != null && buttonData.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
					}
					buttonData.dragging = false;
					buttonData.pointerDrag = null;
					if (gameObject != buttonData.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData, null);
						base.HandlePointerExitAndEnter(buttonData, gameObject);
					}
					this.leftReleased = false;
					return;
				}
			}
			else
			{
				PointerEventData buttonData2 = data.buttonData;
				GameObject gameObject3 = buttonData2.pointerCurrentRaycast.gameObject;
				buttonData2.position = this.m_VirtualCursor.anchoredPosition;
				if (this.rightFirstClick)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					buttonData2.eligibleForClick = true;
					buttonData2.delta = Vector2.zero;
					buttonData2.dragging = false;
					buttonData2.useDragThreshold = true;
					buttonData2.pressPosition = buttonData2.position;
					buttonData2.pointerPressRaycast = buttonData2.pointerCurrentRaycast;
					buttonData2.position = this.m_VirtualCursor.anchoredPosition;
					base.DeselectIfSelectionChanged(gameObject3, buttonData2);
					GameObject gameObject4 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject3, buttonData2, ExecuteEvents.pointerDownHandler);
					if (gameObject4 == null)
					{
						gameObject4 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					}
					float unscaledTime2 = Time.unscaledTime;
					if (gameObject4 == buttonData2.lastPress)
					{
						if (unscaledTime2 - buttonData2.clickTime < 0.3f)
						{
							PointerEventData pointerEventData2 = buttonData2;
							int clickCount = pointerEventData2.clickCount + 1;
							pointerEventData2.clickCount = clickCount;
						}
						else
						{
							buttonData2.clickCount = 1;
						}
						buttonData2.clickTime = unscaledTime2;
					}
					else
					{
						buttonData2.clickCount = 1;
					}
					buttonData2.pointerPress = gameObject4;
					buttonData2.rawPointerPress = gameObject3;
					buttonData2.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject3);
					if (buttonData2.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.initializePotentialDrag);
					}
					this.rightFirstClick = false;
					return;
				}
				if (this.rightReleased)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					if (buttonData2.pointerPress == eventHandler2 && buttonData2.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject3, buttonData2, ExecuteEvents.dropHandler);
					}
					buttonData2.eligibleForClick = false;
					buttonData2.pointerPress = null;
					buttonData2.rawPointerPress = null;
					if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.endDragHandler);
					}
					buttonData2.dragging = false;
					buttonData2.pointerDrag = null;
					if (gameObject3 != buttonData2.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData2, null);
						base.HandlePointerExitAndEnter(buttonData2, gameObject3);
					}
					this.rightReleased = false;
					return;
				}
			}
		}

		// Token: 0x0600B2FB RID: 45819 RVA: 0x0043A0C0 File Offset: 0x004382C0
		protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData pointerEventData;
			bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			Vector2 position = RectTransformUtility.WorldToScreenPoint(this.m_canvasCamera, this.m_VirtualCursor.position);
			if (pointerData)
			{
				pointerEventData.position = position;
			}
			Vector2 anchoredPosition = this.m_VirtualCursor.anchoredPosition;
			pointerEventData.delta = anchoredPosition - pointerEventData.position;
			pointerEventData.position = anchoredPosition;
			pointerEventData.scrollDelta = Input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PointerEventData pointerEventData2;
			base.GetPointerData(-2, out pointerEventData2, true);
			base.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			base.GetPointerData(-3, out pointerEventData3, true);
			base.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, base.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, base.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, base.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x04008D68 RID: 36200
		private float m_PrevActionTime;

		// Token: 0x04008D69 RID: 36201
		private Vector2 m_LastMoveVector;

		// Token: 0x04008D6A RID: 36202
		private int m_ConsecutiveMoveCount;

		// Token: 0x04008D6B RID: 36203
		private string debugName;

		// Token: 0x04008D6C RID: 36204
		private Vector2 m_LastMousePosition;

		// Token: 0x04008D6D RID: 36205
		private Vector2 m_MousePosition;

		// Token: 0x04008D6E RID: 36206
		public bool mouseMovementOnly;

		// Token: 0x04008D6F RID: 36207
		[SerializeField]
		private RectTransform m_VirtualCursor;

		// Token: 0x04008D70 RID: 36208
		[SerializeField]
		private float m_VirtualCursorSpeed = 1f;

		// Token: 0x04008D71 RID: 36209
		[SerializeField]
		private Vector2 m_VirtualCursorOffset = Vector2.zero;

		// Token: 0x04008D72 RID: 36210
		[SerializeField]
		private Camera m_canvasCamera;

		// Token: 0x04008D73 RID: 36211
		private Camera VCcam;

		// Token: 0x04008D74 RID: 36212
		public bool CursorCanvasShouldBeOverlay;

		// Token: 0x04008D75 RID: 36213
		private Canvas m_virtualCursorCanvas;

		// Token: 0x04008D76 RID: 36214
		private CanvasScaler m_virtualCursorScaler;

		// Token: 0x04008D77 RID: 36215
		private PointerEventData leftClickData;

		// Token: 0x04008D78 RID: 36216
		private PointerEventData rightClickData;

		// Token: 0x04008D79 RID: 36217
		private VirtualInputModule.ControllerButtonStates conButtonStates;

		// Token: 0x04008D7A RID: 36218
		private GameObject m_CurrentFocusedGameObject;

		// Token: 0x04008D7B RID: 36219
		private bool leftReleased;

		// Token: 0x04008D7C RID: 36220
		private bool rightReleased;

		// Token: 0x04008D7D RID: 36221
		private bool leftFirstClick;

		// Token: 0x04008D7E RID: 36222
		private bool rightFirstClick;

		// Token: 0x04008D7F RID: 36223
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04008D80 RID: 36224
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04008D81 RID: 36225
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04008D82 RID: 36226
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04008D83 RID: 36227
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04008D84 RID: 36228
		[SerializeField]
		private float m_RepeatDelay = 0.5f;

		// Token: 0x04008D85 RID: 36229
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		private bool m_ForceModuleActive;

		// Token: 0x04008D86 RID: 36230
		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		// Token: 0x020020DA RID: 8410
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			// Token: 0x04008D88 RID: 36232
			Mouse,
			// Token: 0x04008D89 RID: 36233
			Buttons
		}

		// Token: 0x020020DB RID: 8411
		private struct ControllerButtonStates
		{
			// Token: 0x04008D8A RID: 36234
			public bool affirmativeDown;

			// Token: 0x04008D8B RID: 36235
			public float affirmativeHoldTime;

			// Token: 0x04008D8C RID: 36236
			public bool negativeDown;

			// Token: 0x04008D8D RID: 36237
			public float negativeHoldTime;
		}
	}
}
