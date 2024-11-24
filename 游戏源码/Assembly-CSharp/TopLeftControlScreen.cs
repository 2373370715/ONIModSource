using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02002032 RID: 8242
public class TopLeftControlScreen : KScreen
{
	// Token: 0x0600AF71 RID: 44913 RVA: 0x0011201E File Offset: 0x0011021E
	public static void DestroyInstance()
	{
		TopLeftControlScreen.Instance = null;
	}

	// Token: 0x0600AF72 RID: 44914 RVA: 0x00420AF0 File Offset: 0x0041ECF0
	protected override void OnActivate()
	{
		base.OnActivate();
		TopLeftControlScreen.Instance = this;
		this.RefreshName();
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
		this.UpdateSandboxToggleState();
		MultiToggle multiToggle = this.sandboxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickSandboxToggle));
		MultiToggle multiToggle2 = this.kleiItemDropButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnClickKleiItemDropButton));
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(new System.Action(this.RefreshKleiItemDropButton));
		this.RefreshKleiItemDropButton();
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.UpdateSandboxToggleState();
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.secondaryRow);
	}

	// Token: 0x0600AF73 RID: 44915 RVA: 0x00112026 File Offset: 0x00110226
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600AF74 RID: 44916 RVA: 0x00112044 File Offset: 0x00110244
	public void RefreshName()
	{
		if (SaveGame.Instance != null)
		{
			this.locText.text = SaveGame.Instance.BaseName;
		}
	}

	// Token: 0x0600AF75 RID: 44917 RVA: 0x00420BC0 File Offset: 0x0041EDC0
	public void ResetToolTip()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			return;
		}
		this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
	}

	// Token: 0x0600AF76 RID: 44918 RVA: 0x00420C20 File Offset: 0x0041EE20
	public void UpdateSandboxToggleState()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(0);
		}
		else
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(Game.Instance.SandboxModeActive ? 2 : 1);
		}
		this.sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
	}

	// Token: 0x0600AF77 RID: 44919 RVA: 0x00420CC0 File Offset: 0x0041EEC0
	private void OnClickSandboxToggle()
	{
		if (this.CheckSandboxModeLocked())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		else
		{
			Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
			KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
		}
		this.UpdateSandboxToggleState();
	}

	// Token: 0x0600AF78 RID: 44920 RVA: 0x00420D30 File Offset: 0x0041EF30
	private void RefreshKleiItemDropButton()
	{
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
			this.kleiItemDropButton.ChangeState(1);
			return;
		}
		this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
		this.kleiItemDropButton.ChangeState(2);
	}

	// Token: 0x0600AF79 RID: 44921 RVA: 0x00112068 File Offset: 0x00110268
	private void OnClickKleiItemDropButton()
	{
		this.RefreshKleiItemDropButton();
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
	}

	// Token: 0x0600AF7A RID: 44922 RVA: 0x001120A4 File Offset: 0x001102A4
	private bool CheckSandboxModeLocked()
	{
		return !SaveGame.Instance.sandboxEnabled;
	}

	// Token: 0x04008A47 RID: 35399
	public static TopLeftControlScreen Instance;

	// Token: 0x04008A48 RID: 35400
	[SerializeField]
	private MultiToggle sandboxToggle;

	// Token: 0x04008A49 RID: 35401
	[SerializeField]
	private MultiToggle kleiItemDropButton;

	// Token: 0x04008A4A RID: 35402
	[SerializeField]
	private LocText locText;

	// Token: 0x04008A4B RID: 35403
	[SerializeField]
	private RectTransform secondaryRow;

	// Token: 0x02002033 RID: 8243
	private enum MultiToggleState
	{
		// Token: 0x04008A4D RID: 35405
		Disabled,
		// Token: 0x04008A4E RID: 35406
		Off,
		// Token: 0x04008A4F RID: 35407
		On
	}
}
