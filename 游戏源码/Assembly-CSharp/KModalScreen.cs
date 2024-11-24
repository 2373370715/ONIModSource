using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D30 RID: 7472
public class KModalScreen : KScreen
{
	// Token: 0x06009BF5 RID: 39925 RVA: 0x00105747 File Offset: 0x00103947
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backgroundRectTransform = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x06009BF6 RID: 39926 RVA: 0x003C1AA8 File Offset: 0x003BFCA8
	public static RectTransform MakeScreenModal(KScreen screen)
	{
		screen.ConsumeMouseScroll = true;
		screen.activateOnSpawn = true;
		GameObject gameObject = new GameObject("background");
		gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
		gameObject.AddComponent<CanvasRenderer>();
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color32(0, 0, 0, 160);
		image.raycastTarget = true;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(screen.transform);
		KModalScreen.ResizeBackground(component);
		return component;
	}

	// Token: 0x06009BF7 RID: 39927 RVA: 0x003C1B1C File Offset: 0x003BFD1C
	public static void ResizeBackground(RectTransform rectTransform)
	{
		rectTransform.SetAsFirstSibling();
		rectTransform.SetLocalPosition(Vector3.zero);
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x06009BF8 RID: 39928 RVA: 0x003C1B88 File Offset: 0x003BFD88
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = true;
		}
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x06009BF9 RID: 39929 RVA: 0x003C1BE8 File Offset: 0x003BFDE8
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = false;
		}
		base.Trigger(476357528, null);
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x06009BFA RID: 39930 RVA: 0x0010575B File Offset: 0x0010395B
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.backgroundRectTransform);
	}

	// Token: 0x06009BFB RID: 39931 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009BFC RID: 39932 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06009BFD RID: 39933 RVA: 0x00105768 File Offset: 0x00103968
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06009BFE RID: 39934 RVA: 0x00105771 File Offset: 0x00103971
	protected override void OnDeactivate()
	{
		this.OnShow(false);
	}

	// Token: 0x06009BFF RID: 39935 RVA: 0x003C1C54 File Offset: 0x003BFE54
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (this.pause && SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
	}

	// Token: 0x06009C00 RID: 39936 RVA: 0x003C1CB4 File Offset: 0x003BFEB4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (Game.Instance != null && (e.TryConsume(global::Action.TogglePause) || e.TryConsume(global::Action.CycleSpeed)))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		if (!e.Consumed && (e.TryConsume(global::Action.Escape) || (e.TryConsume(global::Action.MouseRight) && this.canBackoutWithRightClick)))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x06009C01 RID: 39937 RVA: 0x001056E6 File Offset: 0x001038E6
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x04007A3D RID: 31293
	private bool shown;

	// Token: 0x04007A3E RID: 31294
	public bool pause = true;

	// Token: 0x04007A3F RID: 31295
	[Tooltip("Only used for main menu")]
	public bool canBackoutWithRightClick;

	// Token: 0x04007A40 RID: 31296
	private RectTransform backgroundRectTransform;
}
