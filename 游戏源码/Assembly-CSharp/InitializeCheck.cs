using System;
using System.IO;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02001410 RID: 5136
public class InitializeCheck : MonoBehaviour
{
	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x060069D7 RID: 27095 RVA: 0x000E572B File Offset: 0x000E392B
	// (set) Token: 0x060069D8 RID: 27096 RVA: 0x000E5732 File Offset: 0x000E3932
	public static InitializeCheck.SavePathIssue savePathState { get; private set; }

	// Token: 0x060069D9 RID: 27097 RVA: 0x002DBECC File Offset: 0x002DA0CC
	private void Awake()
	{
		this.CheckForSavePathIssue();
		if (InitializeCheck.savePathState == InitializeCheck.SavePathIssue.Ok && !KCrashReporter.hasCrash)
		{
			AudioMixer.Create();
			App.LoadScene("frontend");
			return;
		}
		Canvas cmp = base.gameObject.AddComponent<Canvas>();
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);
		Camera camera = base.gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = 200f;
		camera.backgroundColor = Color.black;
		camera.clearFlags = CameraClearFlags.Color;
		camera.nearClipPlane = 0f;
		global::Debug.Log("Cannot initialize filesystem. [" + InitializeCheck.savePathState.ToString() + "]");
		Localization.Initialize();
		GameObject.Find("BootCanvas").SetActive(false);
		this.ShowFileErrorDialogs();
	}

	// Token: 0x060069DA RID: 27098 RVA: 0x000E573A File Offset: 0x000E393A
	private GameObject CreateUIRoot()
	{
		return Util.KInstantiate(this.rootCanvasPrefab, null, "CanvasRoot");
	}

	// Token: 0x060069DB RID: 27099 RVA: 0x002DBFA8 File Offset: 0x002DA1A8
	private void ShowErrorDialog(string msg)
	{
		GameObject parent = this.CreateUIRoot();
		Util.KInstantiateUI<ConfirmDialogScreen>(this.confirmDialogScreen.gameObject, parent, true).PopupConfirmDialog(msg, new System.Action(this.Quit), null, null, null, null, null, null, this.sadDupe);
	}

	// Token: 0x060069DC RID: 27100 RVA: 0x002DBFEC File Offset: 0x002DA1EC
	private void ShowFileErrorDialogs()
	{
		string text = null;
		switch (InitializeCheck.savePathState)
		{
		case InitializeCheck.SavePathIssue.WriteTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.SpaceTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.WorldGenFilesFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FILES, WorldGen.WORLDGEN_SAVE_FILENAME);
			break;
		}
		if (text != null)
		{
			this.ShowErrorDialog(text);
		}
	}

	// Token: 0x060069DD RID: 27101 RVA: 0x002DC064 File Offset: 0x002DA264
	private void CheckForSavePathIssue()
	{
		if (this.test_issue != InitializeCheck.SavePathIssue.Ok)
		{
			InitializeCheck.savePathState = this.test_issue;
			return;
		}
		string savePrefix = SaveLoader.GetSavePrefix();
		InitializeCheck.savePathState = InitializeCheck.SavePathIssue.Ok;
		try
		{
			SaveLoader.GetSavePrefixAndCreateFolder();
			using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testFile, FileMode.Create, FileAccess.Write))
			{
				new BinaryWriter(fileStream);
				fileStream.Close();
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WriteTestFail;
			goto IL_C8;
		}
		using (FileStream fileStream2 = File.Open(savePrefix + InitializeCheck.testSave, FileMode.Create, FileAccess.Write))
		{
			try
			{
				fileStream2.SetLength(15000000L);
				new BinaryWriter(fileStream2);
				fileStream2.Close();
			}
			catch
			{
				fileStream2.Close();
				InitializeCheck.savePathState = InitializeCheck.SavePathIssue.SpaceTestFail;
				goto IL_C8;
			}
		}
		try
		{
			using (File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Append))
			{
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WorldGenFilesFail;
		}
		IL_C8:
		try
		{
			if (File.Exists(savePrefix + InitializeCheck.testFile))
			{
				File.Delete(savePrefix + InitializeCheck.testFile);
			}
			if (File.Exists(savePrefix + InitializeCheck.testSave))
			{
				File.Delete(savePrefix + InitializeCheck.testSave);
			}
		}
		catch
		{
		}
	}

	// Token: 0x060069DE RID: 27102 RVA: 0x000E574D File Offset: 0x000E394D
	private void Quit()
	{
		global::Debug.Log("Quitting...");
		App.Quit();
	}

	// Token: 0x04004FF2 RID: 20466
	private static readonly string testFile = "testfile";

	// Token: 0x04004FF3 RID: 20467
	private static readonly string testSave = "testsavefile";

	// Token: 0x04004FF4 RID: 20468
	public Canvas rootCanvasPrefab;

	// Token: 0x04004FF5 RID: 20469
	public ConfirmDialogScreen confirmDialogScreen;

	// Token: 0x04004FF6 RID: 20470
	public Sprite sadDupe;

	// Token: 0x04004FF7 RID: 20471
	private InitializeCheck.SavePathIssue test_issue;

	// Token: 0x02001411 RID: 5137
	public enum SavePathIssue
	{
		// Token: 0x04004FF9 RID: 20473
		Ok,
		// Token: 0x04004FFA RID: 20474
		WriteTestFail,
		// Token: 0x04004FFB RID: 20475
		SpaceTestFail,
		// Token: 0x04004FFC RID: 20476
		WorldGenFilesFail
	}
}
