﻿using System;
using UnityEngine;

public class ScreenResize : MonoBehaviour
{
	private void Awake()
	{
		ScreenResize.Instance = this;
		this.isFullscreen = Screen.fullScreen;
		this.OnResize = (System.Action)Delegate.Combine(this.OnResize, new System.Action(this.SaveResolutionToPrefs));
	}

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

	public void TriggerResize()
	{
		if (this.OnResize != null)
		{
			this.OnResize();
		}
	}

	private void SaveResolutionToPrefs()
	{
		GraphicsOptionsScreen.OnResize();
	}

	public System.Action OnResize;

	public static ScreenResize Instance;

	private int Width;

	private int Height;

	private bool isFullscreen;
}
