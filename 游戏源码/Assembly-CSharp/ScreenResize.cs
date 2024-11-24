using System;
using UnityEngine;

// Token: 0x0200178F RID: 6031
public class ScreenResize : MonoBehaviour
{
	// Token: 0x06007C11 RID: 31761 RVA: 0x000F1BD0 File Offset: 0x000EFDD0
	private void Awake()
	{
		ScreenResize.Instance = this;
		this.isFullscreen = Screen.fullScreen;
		this.OnResize = (System.Action)Delegate.Combine(this.OnResize, new System.Action(this.SaveResolutionToPrefs));
	}

	// Token: 0x06007C12 RID: 31762 RVA: 0x0031EF0C File Offset: 0x0031D10C
	private void LateUpdate()
	{
		if (Screen.width != this.Width || Screen.height != this.Height || this.isFullscreen != Screen.fullScreen)
		{
			this.Width = Screen.width;
			this.Height = Screen.height;
			this.isFullscreen = Screen.fullScreen;
			this.TriggerResize();
		}
	}

	// Token: 0x06007C13 RID: 31763 RVA: 0x000F1C05 File Offset: 0x000EFE05
	public void TriggerResize()
	{
		if (this.OnResize != null)
		{
			this.OnResize();
		}
	}

	// Token: 0x06007C14 RID: 31764 RVA: 0x000F1C1A File Offset: 0x000EFE1A
	private void SaveResolutionToPrefs()
	{
		GraphicsOptionsScreen.OnResize();
	}

	// Token: 0x04005DCC RID: 24012
	public System.Action OnResize;

	// Token: 0x04005DCD RID: 24013
	public static ScreenResize Instance;

	// Token: 0x04005DCE RID: 24014
	private int Width;

	// Token: 0x04005DCF RID: 24015
	private int Height;

	// Token: 0x04005DD0 RID: 24016
	private bool isFullscreen;
}
