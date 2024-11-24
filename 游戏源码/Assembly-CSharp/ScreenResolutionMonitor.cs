using System;
using UnityEngine;

// Token: 0x02002068 RID: 8296
public class ScreenResolutionMonitor : MonoBehaviour
{
	// Token: 0x0600B086 RID: 45190 RVA: 0x00112CAB File Offset: 0x00110EAB
	private void Awake()
	{
		this.previousSize = new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x0600B087 RID: 45191 RVA: 0x004253E0 File Offset: 0x004235E0
	private void Update()
	{
		if ((this.previousSize.x != (float)Screen.width || this.previousSize.y != (float)Screen.height) && Game.Instance != null)
		{
			Game.Instance.Trigger(445618876, null);
			this.previousSize.x = (float)Screen.width;
			this.previousSize.y = (float)Screen.height;
		}
		this.UpdateShouldUseGamepadUIMode();
	}

	// Token: 0x0600B088 RID: 45192 RVA: 0x00112CC4 File Offset: 0x00110EC4
	public static bool UsingGamepadUIMode()
	{
		return ScreenResolutionMonitor.previousGamepadUIMode;
	}

	// Token: 0x0600B089 RID: 45193 RVA: 0x00425458 File Offset: 0x00423658
	private void UpdateShouldUseGamepadUIMode()
	{
		bool flag = (Screen.dpi > 130f && Screen.height < 900) || KInputManager.currentControllerIsGamepad;
		if (flag != ScreenResolutionMonitor.previousGamepadUIMode)
		{
			ScreenResolutionMonitor.previousGamepadUIMode = flag;
			if (Game.Instance == null)
			{
				return;
			}
			Game.Instance.Trigger(-442024484, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(flag ? "ControllerType_ToggleOn" : "ControllerType_ToggleOff", false));
		}
	}

	// Token: 0x04008B7F RID: 35711
	[SerializeField]
	private Vector2 previousSize;

	// Token: 0x04008B80 RID: 35712
	private static bool previousGamepadUIMode;

	// Token: 0x04008B81 RID: 35713
	private const float HIGH_DPI = 130f;
}
