using System;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

// Token: 0x02001CDE RID: 7390
public class GameOptionsScreen : KModalButtonMenu
{
	// Token: 0x06009A43 RID: 39491 RVA: 0x001045DF File Offset: 0x001027DF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06009A44 RID: 39492 RVA: 0x003B8A50 File Offset: 0x003B6C50
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

	// Token: 0x06009A45 RID: 39493 RVA: 0x003B8BF4 File Offset: 0x003B6DF4
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

	// Token: 0x06009A46 RID: 39494 RVA: 0x001045E7 File Offset: 0x001027E7
	private float CameraSpeedToSlider(float prefsValue)
	{
		return prefsValue * 10f;
	}

	// Token: 0x06009A47 RID: 39495 RVA: 0x001045F0 File Offset: 0x001027F0
	private void OnCameraSpeedValueChanged(int sliderValue)
	{
		KPlayerPrefs.SetFloat("CameraSpeed", (float)sliderValue / 10f);
		this.RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175, null);
		}
	}

	// Token: 0x06009A48 RID: 39496 RVA: 0x003B8C5C File Offset: 0x003B6E5C
	private void RefreshCameraSliderLabel()
	{
		this.cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	// Token: 0x06009A49 RID: 39497 RVA: 0x00104627 File Offset: 0x00102827
	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		this.RefreshCloudSaveToggle();
	}

	// Token: 0x06009A4A RID: 39498 RVA: 0x003B8CA4 File Offset: 0x003B6EA4
	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	// Token: 0x06009A4B RID: 39499 RVA: 0x0010463C File Offset: 0x0010283C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009A4C RID: 39500 RVA: 0x003B8CD8 File Offset: 0x003B6ED8
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

	// Token: 0x06009A4D RID: 39501 RVA: 0x003B8D58 File Offset: 0x003B6F58
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

	// Token: 0x06009A4E RID: 39502 RVA: 0x0010465E File Offset: 0x0010285E
	private void OnKeyBindings()
	{
		base.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
	}

	// Token: 0x06009A4F RID: 39503 RVA: 0x003B8DF0 File Offset: 0x003B6FF0
	private void SetSandboxModeActive(bool active)
	{
		this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		this.sandboxButton.isInteractable = !active;
		this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}

	// Token: 0x04007865 RID: 30821
	[SerializeField]
	private SaveConfigurationScreen saveConfiguration;

	// Token: 0x04007866 RID: 30822
	[SerializeField]
	private UnitConfigurationScreen unitConfiguration;

	// Token: 0x04007867 RID: 30823
	[SerializeField]
	private KButton resetTutorialButton;

	// Token: 0x04007868 RID: 30824
	[SerializeField]
	private KButton controlsButton;

	// Token: 0x04007869 RID: 30825
	[SerializeField]
	private KButton sandboxButton;

	// Token: 0x0400786A RID: 30826
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x0400786B RID: 30827
	[SerializeField]
	private KButton doneButton;

	// Token: 0x0400786C RID: 30828
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400786D RID: 30829
	[SerializeField]
	private GameObject cloudSavesPanel;

	// Token: 0x0400786E RID: 30830
	[SerializeField]
	private GameObject defaultToCloudSaveToggle;

	// Token: 0x0400786F RID: 30831
	[SerializeField]
	private GameObject savePanel;

	// Token: 0x04007870 RID: 30832
	[SerializeField]
	private InputBindingsScreen inputBindingsScreenPrefab;

	// Token: 0x04007871 RID: 30833
	[SerializeField]
	private KSlider cameraSpeedSlider;

	// Token: 0x04007872 RID: 30834
	[SerializeField]
	private LocText cameraSpeedSliderLabel;

	// Token: 0x04007873 RID: 30835
	private const int cameraSliderNotchScale = 10;

	// Token: 0x04007874 RID: 30836
	public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";
}
