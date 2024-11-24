using UnityEngine;

internal class InputInit : MonoBehaviour
{
	private void Awake()
	{
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.GetControllerCount(); i++)
		{
			KInputController controller = inputManager.GetController(i);
			if (!controller.IsGamepad)
			{
				continue;
			}
			Component[] components = base.gameObject.GetComponents<Component>();
			for (int j = 0; j < components.Length; j++)
			{
				if (components[j] is IInputHandler inputHandler)
				{
					KInputHandler.Add(controller, inputHandler);
					inputManager.usedMenus.Add(inputHandler);
				}
			}
		}
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, KScreenManager.Instance, 10);
		}
		else
		{
			KInputHandler.Add(inputManager.GetDefaultController(), KScreenManager.Instance, 10);
		}
		inputManager.usedMenus.Add(KScreenManager.Instance);
		DebugHandler debugHandler = new DebugHandler();
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, debugHandler, -1);
		}
		else
		{
			KInputHandler.Add(inputManager.GetDefaultController(), debugHandler, -1);
		}
		inputManager.usedMenus.Add(debugHandler);
	}
}
