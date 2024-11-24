using System;
using System.Collections.Generic;
using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EBC RID: 7868
public class ReportErrorDialog : MonoBehaviour
{
	// Token: 0x0600A550 RID: 42320 RVA: 0x003EC110 File Offset: 0x003EA310
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

	// Token: 0x0600A551 RID: 42321 RVA: 0x003EC26C File Offset: 0x003EA46C
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

	// Token: 0x0600A552 RID: 42322 RVA: 0x001047BE File Offset: 0x001029BE
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x0600A553 RID: 42323 RVA: 0x003EC440 File Offset: 0x003EA640
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

	// Token: 0x0600A554 RID: 42324 RVA: 0x0010B3F7 File Offset: 0x001095F7
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_QUIT();
		}
	}

	// Token: 0x0600A555 RID: 42325 RVA: 0x0010B408 File Offset: 0x00109608
	public void PopupSubmitErrorDialog(string stackTrace, System.Action onSubmit, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.SubmitError;
		this.m_stackTrace = stackTrace;
		this.submitAction = onSubmit;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x0600A556 RID: 42326 RVA: 0x0010B42E File Offset: 0x0010962E
	public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.DisableMods;
		this.m_stackTrace = stackTrace;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x0600A557 RID: 42327 RVA: 0x003EC494 File Offset: 0x003EA694
	public void OnSelect_MOREINFO()
	{
		this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
		this.StackTrace.SetActive(true);
		this.moreInfoButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
		this.moreInfoButton.ClearOnClick();
		this.moreInfoButton.onClick += this.OnSelect_COPYTOCLIPBOARD;
	}

	// Token: 0x0600A558 RID: 42328 RVA: 0x0010B44C File Offset: 0x0010964C
	public void OnSelect_COPYTOCLIPBOARD()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.m_stackTrace + "\nBuild: " + BuildWatermark.GetBuildText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x0600A559 RID: 42329 RVA: 0x0010B479 File Offset: 0x00109679
	public void OnSelect_SUBMIT()
	{
		this.submitButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.REPORTING;
		this.submitButton.GetComponent<KButton>().isInteractable = false;
		this.Submit();
	}

	// Token: 0x0600A55A RID: 42330 RVA: 0x0010B4AC File Offset: 0x001096AC
	public void OnSelect_QUIT()
	{
		if (this.quitAction != null)
		{
			this.quitAction();
		}
	}

	// Token: 0x0600A55B RID: 42331 RVA: 0x0010B4C1 File Offset: 0x001096C1
	public void OnSelect_CONTINUE()
	{
		if (this.continueAction != null)
		{
			this.continueAction();
		}
	}

	// Token: 0x0600A55C RID: 42332 RVA: 0x003EC500 File Offset: 0x003EA700
	public void OpenRefMessage(bool success)
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(false);
		this.referenceMessage.SetActive(true);
		this.messageText.text = (success ? UI.CRASHSCREEN.THANKYOU : UI.CRASHSCREEN.UPLOAD_FAILED);
		this.m_crashSubmitted = success;
	}

	// Token: 0x0600A55D RID: 42333 RVA: 0x003EC55C File Offset: 0x003EA75C
	public void OpenUploadingMessagee()
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(true);
		this.referenceMessage.SetActive(false);
		this.progressBar.fillAmount = 0f;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0f, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600A55E RID: 42334 RVA: 0x0010B4D6 File Offset: 0x001096D6
	public void OnSelect_MESSAGE()
	{
		if (!this.m_crashSubmitted)
		{
			Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}
	}

	// Token: 0x0600A55F RID: 42335 RVA: 0x0010B4EA File Offset: 0x001096EA
	public string UserMessage()
	{
		return this.messageInputField.text;
	}

	// Token: 0x0600A560 RID: 42336 RVA: 0x0010B4F7 File Offset: 0x001096F7
	private void Submit()
	{
		this.submitAction();
		this.OpenUploadingMessagee();
	}

	// Token: 0x0600A561 RID: 42337 RVA: 0x0010B50A File Offset: 0x0010970A
	public void UpdateProgressBar(float progress)
	{
		this.progressBar.fillAmount = progress;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x04008177 RID: 33143
	private System.Action submitAction;

	// Token: 0x04008178 RID: 33144
	private System.Action quitAction;

	// Token: 0x04008179 RID: 33145
	private System.Action continueAction;

	// Token: 0x0400817A RID: 33146
	public KInputTextField messageInputField;

	// Token: 0x0400817B RID: 33147
	[Header("Message")]
	public GameObject referenceMessage;

	// Token: 0x0400817C RID: 33148
	public LocText messageText;

	// Token: 0x0400817D RID: 33149
	[Header("Upload Progress")]
	public GameObject uploadInProgress;

	// Token: 0x0400817E RID: 33150
	public Image progressBar;

	// Token: 0x0400817F RID: 33151
	public LocText progressText;

	// Token: 0x04008180 RID: 33152
	private string m_stackTrace;

	// Token: 0x04008181 RID: 33153
	private bool m_crashSubmitted;

	// Token: 0x04008182 RID: 33154
	[SerializeField]
	private KButton submitButton;

	// Token: 0x04008183 RID: 33155
	[SerializeField]
	private KButton moreInfoButton;

	// Token: 0x04008184 RID: 33156
	[SerializeField]
	private KButton quitButton;

	// Token: 0x04008185 RID: 33157
	[SerializeField]
	private KButton continueGameButton;

	// Token: 0x04008186 RID: 33158
	[SerializeField]
	private LocText CrashLabel;

	// Token: 0x04008187 RID: 33159
	[SerializeField]
	private GameObject CrashDescription;

	// Token: 0x04008188 RID: 33160
	[SerializeField]
	private GameObject ModsInfo;

	// Token: 0x04008189 RID: 33161
	[SerializeField]
	private GameObject StackTrace;

	// Token: 0x0400818A RID: 33162
	[SerializeField]
	private GameObject modEntryPrefab;

	// Token: 0x0400818B RID: 33163
	[SerializeField]
	private Transform modEntryParent;

	// Token: 0x0400818C RID: 33164
	private ReportErrorDialog.Mode mode;

	// Token: 0x02001EBD RID: 7869
	private enum Mode
	{
		// Token: 0x0400818E RID: 33166
		SubmitError,
		// Token: 0x0400818F RID: 33167
		DisableMods
	}
}
