using System;
using UnityEngine;

// Token: 0x02001D2F RID: 7471
public class KModalButtonMenu : KButtonMenu
{
	// Token: 0x06009BE7 RID: 39911 RVA: 0x00105682 File Offset: 0x00103882
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.modalBackground = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x06009BE8 RID: 39912 RVA: 0x00105696 File Offset: 0x00103896
	protected override void OnCmpEnable()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06009BE9 RID: 39913 RVA: 0x003C1998 File Offset: 0x003BFB98
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.childDialog == null)
		{
			base.Trigger(476357528, null);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06009BEA RID: 39914 RVA: 0x001056C9 File Offset: 0x001038C9
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
	}

	// Token: 0x06009BEB RID: 39915 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009BEC RID: 39916 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06009BED RID: 39917 RVA: 0x003C19EC File Offset: 0x003BFBEC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SpeedControlScreen.Instance != null)
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
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = show;
		}
	}

	// Token: 0x06009BEE RID: 39918 RVA: 0x001056D6 File Offset: 0x001038D6
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x06009BEF RID: 39919 RVA: 0x001056E6 File Offset: 0x001038E6
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x06009BF0 RID: 39920 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void SetBackgroundActive(bool active)
	{
	}

	// Token: 0x06009BF1 RID: 39921 RVA: 0x003C1A5C File Offset: 0x003BFC5C
	protected GameObject ActivateChildScreen(GameObject screenPrefab)
	{
		GameObject gameObject = Util.KInstantiateUI(screenPrefab, base.transform.parent.gameObject, false);
		this.childDialog = gameObject;
		gameObject.Subscribe(476357528, new Action<object>(this.Unhide));
		this.Hide();
		return gameObject;
	}

	// Token: 0x06009BF2 RID: 39922 RVA: 0x001056F6 File Offset: 0x001038F6
	private void Hide()
	{
		this.panelRoot.rectTransform().localScale = Vector3.zero;
	}

	// Token: 0x06009BF3 RID: 39923 RVA: 0x0010570D File Offset: 0x0010390D
	private void Unhide(object data = null)
	{
		this.panelRoot.rectTransform().localScale = Vector3.one;
		this.childDialog.Unsubscribe(476357528, new Action<object>(this.Unhide));
		this.childDialog = null;
	}

	// Token: 0x04007A39 RID: 31289
	private bool shown;

	// Token: 0x04007A3A RID: 31290
	[SerializeField]
	private GameObject panelRoot;

	// Token: 0x04007A3B RID: 31291
	private GameObject childDialog;

	// Token: 0x04007A3C RID: 31292
	private RectTransform modalBackground;
}
