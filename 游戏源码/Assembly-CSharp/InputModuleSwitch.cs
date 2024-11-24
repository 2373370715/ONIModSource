using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001A68 RID: 6760
public class InputModuleSwitch : MonoBehaviour
{
	// Token: 0x06008D67 RID: 36199 RVA: 0x0036B328 File Offset: 0x00369528
	private void Update()
	{
		if (this.lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
		{
			KInputManager.currentControllerIsGamepad = false;
			KInputManager.InputChange.Invoke();
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.virtualInput.enabled = KInputManager.currentControllerIsGamepad;
			if (this.standaloneInput.enabled)
			{
				this.standaloneInput.enabled = false;
				this.ChangeInputHandler();
				return;
			}
		}
		else
		{
			this.lastMousePosition = Input.mousePosition;
			this.standaloneInput.enabled = true;
			if (this.virtualInput.enabled)
			{
				this.virtualInput.enabled = false;
				this.ChangeInputHandler();
			}
		}
	}

	// Token: 0x06008D68 RID: 36200 RVA: 0x0036B3CC File Offset: 0x003695CC
	private void ChangeInputHandler()
	{
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.usedMenus.Count; i++)
		{
			if (inputManager.usedMenus[i].Equals(null))
			{
				inputManager.usedMenus.RemoveAt(i);
			}
		}
		if (inputManager.GetControllerCount() > 1)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				Cursor.visible = false;
				inputManager.GetController(1).inputHandler.TransferHandles(inputManager.GetController(0).inputHandler);
				return;
			}
			Cursor.visible = true;
			inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
		}
	}

	// Token: 0x04006A3D RID: 27197
	public VirtualInputModule virtualInput;

	// Token: 0x04006A3E RID: 27198
	public StandaloneInputModule standaloneInput;

	// Token: 0x04006A3F RID: 27199
	private Vector3 lastMousePosition;
}
