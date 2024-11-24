using System;
using UnityEngine;

// Token: 0x02001FA9 RID: 8105
public class PlayerControlledToggleSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x0600AB44 RID: 43844 RVA: 0x004099EC File Offset: 0x00407BEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggleButton.onClick += this.ClickToggle;
		this.togglePendingStatusItem = new StatusItem("PlayerControlledToggleSideScreen", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
	}

	// Token: 0x0600AB45 RID: 43845 RVA: 0x0010F3CA File Offset: 0x0010D5CA
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IPlayerControlledToggle>() != null;
	}

	// Token: 0x0600AB46 RID: 43846 RVA: 0x00409A40 File Offset: 0x00407C40
	public void RenderEveryTick(float dt)
	{
		if (base.isActiveAndEnabled)
		{
			if (!this.keyDown && (Input.GetKeyDown(KeyCode.Return) & Time.unscaledTime - this.lastKeyboardShortcutTime > 0.1f))
			{
				if (SpeedControlScreen.Instance.IsPaused)
				{
					this.RequestToggle();
				}
				else
				{
					this.Toggle();
				}
				this.lastKeyboardShortcutTime = Time.unscaledTime;
				this.keyDown = true;
			}
			if (this.keyDown && Input.GetKeyUp(KeyCode.Return))
			{
				this.keyDown = false;
			}
		}
	}

	// Token: 0x0600AB47 RID: 43847 RVA: 0x0010F3D5 File Offset: 0x0010D5D5
	private void ClickToggle()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			this.RequestToggle();
			return;
		}
		this.Toggle();
	}

	// Token: 0x0600AB48 RID: 43848 RVA: 0x00409AC0 File Offset: 0x00407CC0
	private void RequestToggle()
	{
		this.target.ToggleRequested = !this.target.ToggleRequested;
		if (this.target.ToggleRequested && SpeedControlScreen.Instance.IsPaused)
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, this.togglePendingStatusItem, this);
		}
		else
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), true);
	}

	// Token: 0x0600AB49 RID: 43849 RVA: 0x00409B7C File Offset: 0x00407D7C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IPlayerControlledToggle>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an IPlayerControlledToggle");
			return;
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), false);
		this.titleKey = this.target.SideScreenTitleKey;
	}

	// Token: 0x0600AB4A RID: 43850 RVA: 0x00409BFC File Offset: 0x00407DFC
	private void Toggle()
	{
		this.target.ToggledByPlayer();
		this.UpdateVisuals(this.target.ToggledOn(), true);
		this.target.ToggleRequested = false;
		this.target.GetSelectable().RemoveStatusItem(this.togglePendingStatusItem, false);
	}

	// Token: 0x0600AB4B RID: 43851 RVA: 0x00409C4C File Offset: 0x00407E4C
	private void UpdateVisuals(bool state, bool smooth)
	{
		if (state != this.currentState)
		{
			if (smooth)
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS : PlayerControlledToggleSideScreen.OFF_ANIMS, KAnim.PlayMode.Once);
			}
			else
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS[1] : PlayerControlledToggleSideScreen.OFF_ANIMS[1], KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		this.currentState = state;
	}

	// Token: 0x0400869B RID: 34459
	public IPlayerControlledToggle target;

	// Token: 0x0400869C RID: 34460
	public KButton toggleButton;

	// Token: 0x0400869D RID: 34461
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x0400869E RID: 34462
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x0400869F RID: 34463
	public float animScaleBase = 0.25f;

	// Token: 0x040086A0 RID: 34464
	private StatusItem togglePendingStatusItem;

	// Token: 0x040086A1 RID: 34465
	[SerializeField]
	private KBatchedAnimController kbac;

	// Token: 0x040086A2 RID: 34466
	private float lastKeyboardShortcutTime;

	// Token: 0x040086A3 RID: 34467
	private const float KEYBOARD_COOLDOWN = 0.1f;

	// Token: 0x040086A4 RID: 34468
	private bool keyDown;

	// Token: 0x040086A5 RID: 34469
	private bool currentState;
}
