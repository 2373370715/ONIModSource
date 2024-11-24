﻿using System;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

public class GameOptionsScreen : KModalButtonMenu
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitConfiguration.Init();
		if (SaveGame.Instance != null)
		{
			this.saveConfiguration.ToggleDisabledContent(true);
			this.saveConfiguration.Init();
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.saveConfiguration.ToggleDisabledContent(false);
		}
		this.resetTutorialButton.onClick += this.OnTutorialReset;
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.controlsButton.gameObject.SetActive(false);
		}
		else
		{
			this.controlsButton.onClick += this.OnKeyBindings;
		}
		this.sandboxButton.onClick += this.OnUnlockSandboxMode;
		this.doneButton.onClick += this.Deactivate;
		this.closeButton.onClick += this.Deactivate;
		if (this.defaultToCloudSaveToggle != null)
		{
			this.RefreshCloudSaveToggle();
			this.defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += this.OnDefaultToCloudSaveToggle;
		}
		if (this.cloudSavesPanel != null)
		{
			this.cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
		}
		this.cameraSpeedSlider.minValue = 1f;
		this.cameraSpeedSlider.maxValue = 20f;
		this.cameraSpeedSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnCameraSpeedValueChanged(Mathf.FloorToInt(val));
		});
		this.cameraSpeedSlider.value = this.CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
		this.RefreshCameraSliderLabel();
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SaveGame.Instance != null)
		{
			this.savePanel.SetActive(true);
			this.saveConfiguration.Show(show);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.savePanel.SetActive(false);
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
	}

	private float CameraSpeedToSlider(float prefsValue)
	{
		return prefsValue * 10f;
	}

	private void OnCameraSpeedValueChanged(int sliderValue)
	{
		KPlayerPrefs.SetFloat("CameraSpeed", (float)sliderValue / 10f);
		this.RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175, null);
		}
	}

	private void RefreshCameraSliderLabel()
	{
		this.cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		this.RefreshCloudSaveToggle();
	}

	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	private void OnTutorialReset()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, delegate
		{
			Tutorial.ResetHiddenTutorialMessages();
		}, delegate
		{
		}, null, null, null, null, null, null);
		component.Activate();
	}

	private void OnUnlockSandboxMode()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		string text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
		System.Action on_confirm = delegate()
		{
			SaveGame.Instance.sandboxEnabled = true;
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		System.Action on_cancel = delegate()
		{
			string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
			string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
			SaveLoader.Instance.Save(Path.Combine(savePrefixAndCreateFolder, path), false, false);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		string confirm_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
		string cancel_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
		component.PopupConfirmDialog(text, on_confirm, on_cancel, UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL, delegate
		{
		}, null, confirm_text, cancel_text, null);
		component.Activate();
	}

	private void OnKeyBindings()
	{
		base.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
	}

	private void SetSandboxModeActive(bool active)
	{
		this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		this.sandboxButton.isInteractable = !active;
		this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}

	[SerializeField]
	private SaveConfigurationScreen saveConfiguration;

	[SerializeField]
	private UnitConfigurationScreen unitConfiguration;

	[SerializeField]
	private KButton resetTutorialButton;

	[SerializeField]
	private KButton controlsButton;

	[SerializeField]
	private KButton sandboxButton;

	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	[SerializeField]
	private KButton doneButton;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private GameObject cloudSavesPanel;

	[SerializeField]
	private GameObject defaultToCloudSaveToggle;

	[SerializeField]
	private GameObject savePanel;

	[SerializeField]
	private InputBindingsScreen inputBindingsScreenPrefab;

	[SerializeField]
	private KSlider cameraSpeedSlider;

	[SerializeField]
	private LocText cameraSpeedSliderLabel;

	private const int cameraSliderNotchScale = 10;

	public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";
}
