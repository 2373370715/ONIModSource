﻿using System;
using System.Collections.Generic;
using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ReportErrorDialog : MonoBehaviour
{
		private void Start()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(true);
		}
		this.StackTrace.SetActive(false);
		this.CrashLabel.text = ((this.mode == ReportErrorDialog.Mode.SubmitError) ? UI.CRASHSCREEN.TITLE : UI.CRASHSCREEN.TITLE_MODS);
		this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
		this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
		if (this.mode == ReportErrorDialog.Mode.DisableMods)
		{
			this.BuildModsList();
		}
		this.submitButton.gameObject.SetActive(this.submitAction != null);
		this.submitButton.onClick += this.OnSelect_SUBMIT;
		this.moreInfoButton.onClick += this.OnSelect_MOREINFO;
		this.continueGameButton.gameObject.SetActive(this.continueAction != null);
		this.continueGameButton.onClick += this.OnSelect_CONTINUE;
		this.quitButton.onClick += this.OnSelect_QUIT;
		this.messageInputField.text = UI.CRASHSCREEN.BODY;
		KCrashReporter.onCrashReported += this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress += this.UpdateProgressBar;
	}

		private void BuildModsList()
	{
		DebugUtil.Assert(Global.Instance != null && Global.Instance.modManager != null);
		Manager mod_mgr = Global.Instance.modManager;
		List<Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
		allCrashableMods.Sort((Mod x, Mod y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace));
		foreach (Mod mod in allCrashableMods)
		{
			if (mod.foundInStackTrace && mod.label.distribution_platform != Label.DistributionPlatform.Dev)
			{
				mod_mgr.EnableMod(mod.label, false, this);
			}
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject, false);
			LocText reference = hierarchyReferences.GetReference<LocText>("Title");
			reference.text = mod.title;
			reference.color = (mod.foundInStackTrace ? Color.red : Color.white);
			MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
			toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
			Label mod_label = mod.label;
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				bool flag = !mod_mgr.IsModEnabled(mod_label);
				toggle.ChangeState(flag ? 1 : 0);
				mod_mgr.EnableMod(mod_label, flag, this);
			}));
			toggle.GetComponent<ToolTip>().OnToolTip = (() => mod_mgr.IsModEnabled(mod_label) ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
			hierarchyReferences.gameObject.SetActive(true);
		}
	}

		private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

		private void OnDestroy()
	{
		if (KCrashReporter.terminateOnError)
		{
			App.Quit();
		}
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(false);
		}
		KCrashReporter.onCrashReported -= this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress -= this.UpdateProgressBar;
	}

		public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_QUIT();
		}
	}

		public void PopupSubmitErrorDialog(string stackTrace, System.Action onSubmit, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.SubmitError;
		this.m_stackTrace = stackTrace;
		this.submitAction = onSubmit;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

		public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.DisableMods;
		this.m_stackTrace = stackTrace;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

		public void OnSelect_MOREINFO()
	{
		this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
		this.StackTrace.SetActive(true);
		this.moreInfoButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
		this.moreInfoButton.ClearOnClick();
		this.moreInfoButton.onClick += this.OnSelect_COPYTOCLIPBOARD;
	}

		public void OnSelect_COPYTOCLIPBOARD()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.m_stackTrace + "\nBuild: " + BuildWatermark.GetBuildText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

		public void OnSelect_SUBMIT()
	{
		this.submitButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.REPORTING;
		this.submitButton.GetComponent<KButton>().isInteractable = false;
		this.Submit();
	}

		public void OnSelect_QUIT()
	{
		if (this.quitAction != null)
		{
			this.quitAction();
		}
	}

		public void OnSelect_CONTINUE()
	{
		if (this.continueAction != null)
		{
			this.continueAction();
		}
	}

		public void OpenRefMessage(bool success)
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(false);
		this.referenceMessage.SetActive(true);
		this.messageText.text = (success ? UI.CRASHSCREEN.THANKYOU : UI.CRASHSCREEN.UPLOAD_FAILED);
		this.m_crashSubmitted = success;
	}

		public void OpenUploadingMessagee()
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(true);
		this.referenceMessage.SetActive(false);
		this.progressBar.fillAmount = 0f;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0f, GameUtil.TimeSlice.None));
	}

		public void OnSelect_MESSAGE()
	{
		if (!this.m_crashSubmitted)
		{
			Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}
	}

		public string UserMessage()
	{
		return this.messageInputField.text;
	}

		private void Submit()
	{
		this.submitAction();
		this.OpenUploadingMessagee();
	}

		public void UpdateProgressBar(float progress)
	{
		this.progressBar.fillAmount = progress;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f, GameUtil.TimeSlice.None));
	}

		private System.Action submitAction;

		private System.Action quitAction;

		private System.Action continueAction;

		public KInputTextField messageInputField;

		[Header("Message")]
	public GameObject referenceMessage;

		public LocText messageText;

		[Header("Upload Progress")]
	public GameObject uploadInProgress;

		public Image progressBar;

		public LocText progressText;

		private string m_stackTrace;

		private bool m_crashSubmitted;

		[SerializeField]
	private KButton submitButton;

		[SerializeField]
	private KButton moreInfoButton;

		[SerializeField]
	private KButton quitButton;

		[SerializeField]
	private KButton continueGameButton;

		[SerializeField]
	private LocText CrashLabel;

		[SerializeField]
	private GameObject CrashDescription;

		[SerializeField]
	private GameObject ModsInfo;

		[SerializeField]
	private GameObject StackTrace;

		[SerializeField]
	private GameObject modEntryPrefab;

		[SerializeField]
	private Transform modEntryParent;

		private ReportErrorDialog.Mode mode;

		private enum Mode
	{
				SubmitError,
				DisableMods
	}
}
