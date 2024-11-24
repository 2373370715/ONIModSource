using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001AE7 RID: 6887
public class LoadingOverlay : KModalScreen
{
	// Token: 0x0600905F RID: 36959 RVA: 0x000FE5A6 File Offset: 0x000FC7A6
	protected override void OnPrefabInit()
	{
		this.pause = false;
		this.fadeIn = false;
		base.OnPrefabInit();
	}

	// Token: 0x06009060 RID: 36960 RVA: 0x000FE5BC File Offset: 0x000FC7BC
	private void Update()
	{
		if (!this.loadNextFrame && this.showLoad)
		{
			this.loadNextFrame = true;
			this.showLoad = false;
			return;
		}
		if (this.loadNextFrame)
		{
			this.loadNextFrame = false;
			this.loadCb();
		}
	}

	// Token: 0x06009061 RID: 36961 RVA: 0x000FE5F7 File Offset: 0x000FC7F7
	public static void DestroyInstance()
	{
		LoadingOverlay.instance = null;
	}

	// Token: 0x06009062 RID: 36962 RVA: 0x0037B7C0 File Offset: 0x003799C0
	public static void Load(System.Action cb)
	{
		GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
		if (LoadingOverlay.instance == null)
		{
			LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, (GameScreenManager.Instance == null) ? gameObject : GameScreenManager.Instance.ssOverlayCanvas, false);
			LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.LOADING);
		}
		if (GameScreenManager.Instance != null)
		{
			LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
		}
		else
		{
			LoadingOverlay.instance.transform.SetParent(gameObject.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
			if (MainMenu.Instance != null)
			{
				MainMenu.Instance.StopAmbience();
			}
		}
		LoadingOverlay.instance.loadCb = cb;
		LoadingOverlay.instance.showLoad = true;
		LoadingOverlay.instance.Activate();
	}

	// Token: 0x06009063 RID: 36963 RVA: 0x000FE5FF File Offset: 0x000FC7FF
	public static void Clear()
	{
		if (LoadingOverlay.instance != null)
		{
			LoadingOverlay.instance.Deactivate();
		}
	}

	// Token: 0x04006D12 RID: 27922
	private bool loadNextFrame;

	// Token: 0x04006D13 RID: 27923
	private bool showLoad;

	// Token: 0x04006D14 RID: 27924
	private System.Action loadCb;

	// Token: 0x04006D15 RID: 27925
	private static LoadingOverlay instance;
}
