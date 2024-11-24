using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class TopLeftControlScreen : KScreen
{
	private enum MultiToggleState
	{
		Disabled,
		Off,
		On
	}

	public static TopLeftControlScreen Instance;

	[SerializeField]
	private MultiToggle sandboxToggle;

	[SerializeField]
	private MultiToggle kleiItemDropButton;

	[SerializeField]
	private LocText locText;

	[SerializeField]
	private RectTransform secondaryRow;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		Instance = this;
		RefreshName();
		KInputManager.InputChange.AddListener(ResetToolTip);
		UpdateSandboxToggleState();
		MultiToggle multiToggle = sandboxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(OnClickSandboxToggle));
		MultiToggle multiToggle2 = kleiItemDropButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(OnClickKleiItemDropButton));
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(RefreshKleiItemDropButton);
		RefreshKleiItemDropButton();
		Game.Instance.Subscribe(-1948169901, delegate
		{
			UpdateSandboxToggleState();
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(secondaryRow);
	}

	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(ResetToolTip);
		base.OnForcedCleanUp();
	}

	public void RefreshName()
	{
		if (SaveGame.Instance != null)
		{
			locText.text = SaveGame.Instance.BaseName;
		}
	}

	public void ResetToolTip()
	{
		if (CheckSandboxModeLocked())
		{
			sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, Action.ToggleSandboxTools));
		}
		else
		{
			sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, Action.ToggleSandboxTools));
		}
	}

	public void UpdateSandboxToggleState()
	{
		if (CheckSandboxModeLocked())
		{
			sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, Action.ToggleSandboxTools));
			sandboxToggle.ChangeState(0);
		}
		else
		{
			sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, Action.ToggleSandboxTools));
			sandboxToggle.ChangeState((!Game.Instance.SandboxModeActive) ? 1 : 2);
		}
		sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
	}

	private void OnClickSandboxToggle()
	{
		if (CheckSandboxModeLocked())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
		}
		else
		{
			Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
			KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On") : GlobalAssets.GetSound("SandboxTool_Toggle_Off"));
		}
		UpdateSandboxToggleState();
	}

	private void RefreshKleiItemDropButton()
	{
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
			kleiItemDropButton.ChangeState(1);
		}
		else
		{
			kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
			kleiItemDropButton.ChangeState(2);
		}
	}

	private void OnClickKleiItemDropButton()
	{
		RefreshKleiItemDropButton();
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
		UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(includeInactive: true).Show();
	}

	private bool CheckSandboxModeLocked()
	{
		return !SaveGame.Instance.sandboxEnabled;
	}
}
