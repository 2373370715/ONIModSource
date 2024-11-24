using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

public class GameOptionsScreen : KModalButtonMenu
{
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

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		unitConfiguration.Init();
		if (SaveGame.Instance != null)
		{
			saveConfiguration.ToggleDisabledContent(enable: true);
			saveConfiguration.Init();
			SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			saveConfiguration.ToggleDisabledContent(enable: false);
		}
		resetTutorialButton.onClick += OnTutorialReset;
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			controlsButton.gameObject.SetActive(value: false);
		}
		else
		{
			controlsButton.onClick += OnKeyBindings;
		}
		sandboxButton.onClick += OnUnlockSandboxMode;
		doneButton.onClick += Deactivate;
		closeButton.onClick += Deactivate;
		if (defaultToCloudSaveToggle != null)
		{
			RefreshCloudSaveToggle();
			defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += OnDefaultToCloudSaveToggle;
		}
		if (cloudSavesPanel != null)
		{
			cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
		}
		cameraSpeedSlider.minValue = 1f;
		cameraSpeedSlider.maxValue = 20f;
		cameraSpeedSlider.onValueChanged.AddListener(delegate(float val)
		{
			OnCameraSpeedValueChanged(Mathf.FloorToInt(val));
		});
		cameraSpeedSlider.value = CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
		RefreshCameraSliderLabel();
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SaveGame.Instance != null)
		{
			savePanel.SetActive(value: true);
			saveConfiguration.Show(show);
			SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			savePanel.SetActive(value: false);
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
		RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175);
		}
	}

	private void RefreshCameraSliderLabel()
	{
		cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		RefreshCloudSaveToggle();
	}

	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			Deactivate();
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	private void OnTutorialReset()
	{
		ConfirmDialogScreen component = ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, delegate
		{
			Tutorial.ResetHiddenTutorialMessages();
		}, delegate
		{
		});
		component.Activate();
	}

	private void OnUnlockSandboxMode()
	{
		ConfirmDialogScreen component = ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING, delegate
		{
			SaveGame.Instance.sandboxEnabled = true;
			SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			Deactivate();
		}, delegate
		{
			string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
			string path = string.Concat(SaveGame.Instance.BaseName, UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND, ".sav");
			SaveLoader.Instance.Save(Path.Combine(savePrefixAndCreateFolder, path), isAutoSave: false, updateSavePointer: false);
			SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			Deactivate();
		}, confirm_text: UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM, cancel_text: UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP, configurable_text: UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL, on_configurable_clicked: delegate
		{
		});
		component.Activate();
	}

	private void OnKeyBindings()
	{
		ActivateChildScreen(inputBindingsScreenPrefab.gameObject);
	}

	private void SetSandboxModeActive(bool active)
	{
		sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		sandboxButton.isInteractable = !active;
		sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}
}
