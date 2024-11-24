using UnityEngine;
using UnityEngine.EventSystems;

public class InputModuleSwitch : MonoBehaviour
{
	public VirtualInputModule virtualInput;

	public StandaloneInputModule standaloneInput;

	private Vector3 lastMousePosition;

	private void Update()
	{
		if (lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
		{
			KInputManager.currentControllerIsGamepad = false;
			KInputManager.InputChange.Invoke();
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			virtualInput.enabled = KInputManager.currentControllerIsGamepad;
			if (standaloneInput.enabled)
			{
				standaloneInput.enabled = false;
				virtualInput.forceModuleActive = true;
				ChangeInputHandler();
			}
			return;
		}
		lastMousePosition = Input.mousePosition;
		standaloneInput.enabled = true;
		if (virtualInput.enabled)
		{
			virtualInput.enabled = false;
			standaloneInput.forceModuleActive = true;
			ChangeInputHandler();
		}
	}

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
			}
			else
			{
				Cursor.visible = true;
				inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
			}
		}
	}
}
