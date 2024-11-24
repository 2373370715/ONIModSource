using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001AD6 RID: 6870
public class InspectSaveScreen : KModalScreen
{
	// Token: 0x06008FE4 RID: 36836 RVA: 0x000FE0BC File Offset: 0x000FC2BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += this.CloseScreen;
		this.deleteSaveBtn.onClick += this.DeleteSave;
	}

	// Token: 0x06008FE5 RID: 36837 RVA: 0x000FE0F2 File Offset: 0x000FC2F2
	private void CloseScreen()
	{
		LoadScreen.Instance.Show(true);
		this.Show(false);
	}

	// Token: 0x06008FE6 RID: 36838 RVA: 0x000FE106 File Offset: 0x000FC306
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			this.buttonPool.ClearAll();
			this.buttonFileMap.Clear();
		}
	}

	// Token: 0x06008FE7 RID: 36839 RVA: 0x00378850 File Offset: 0x00376A50
	public void SetTarget(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogError("The directory path provided is empty.");
			this.Show(false);
			return;
		}
		if (!Directory.Exists(path))
		{
			global::Debug.LogError("The directory provided does not exist.");
			this.Show(false);
			return;
		}
		if (this.buttonPool == null)
		{
			this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
		}
		this.currentPath = path;
		List<string> list = (from filename in Directory.GetFiles(path)
		where Path.GetExtension(filename).ToLower() == ".sav"
		orderby File.GetLastWriteTime(filename) descending
		select filename).ToList<string>();
		string text = list[0];
		if (File.Exists(text))
		{
			this.mainSaveBtn.gameObject.SetActive(true);
			this.AddNewSave(this.mainSaveBtn, text);
		}
		else
		{
			this.mainSaveBtn.gameObject.SetActive(false);
		}
		if (list.Count > 1)
		{
			for (int i = 1; i < list.Count; i++)
			{
				this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[i]);
			}
		}
		this.Show(true);
	}

	// Token: 0x06008FE8 RID: 36840 RVA: 0x00378988 File Offset: 0x00376B88
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x06008FE9 RID: 36841 RVA: 0x000FE128 File Offset: 0x000FC328
	private void DeleteSave()
	{
		if (string.IsNullOrEmpty(this.currentPath))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, delegate
		{
			string[] files = Directory.GetFiles(this.currentPath);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			Directory.Delete(this.currentPath);
			this.CloseScreen();
		});
	}

	// Token: 0x06008FEA RID: 36842 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void AddNewSave(KButton btn, string file)
	{
	}

	// Token: 0x06008FEB RID: 36843 RVA: 0x000FE15E File Offset: 0x000FC35E
	private void ButtonClicked(KButton btn)
	{
		LoadingOverlay.Load(delegate
		{
			this.Load(this.buttonFileMap[btn]);
		});
	}

	// Token: 0x06008FEC RID: 36844 RVA: 0x000FE183 File Offset: 0x000FC383
	private void Load(string filename)
	{
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		App.LoadScene("backend");
		this.Deactivate();
	}

	// Token: 0x06008FED RID: 36845 RVA: 0x000FE1AD File Offset: 0x000FC3AD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.CloseScreen();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04006CAA RID: 27818
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006CAB RID: 27819
	[SerializeField]
	private KButton mainSaveBtn;

	// Token: 0x04006CAC RID: 27820
	[SerializeField]
	private KButton backupBtnPrefab;

	// Token: 0x04006CAD RID: 27821
	[SerializeField]
	private KButton deleteSaveBtn;

	// Token: 0x04006CAE RID: 27822
	[SerializeField]
	private GameObject buttonGroup;

	// Token: 0x04006CAF RID: 27823
	private UIPool<KButton> buttonPool;

	// Token: 0x04006CB0 RID: 27824
	private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();

	// Token: 0x04006CB1 RID: 27825
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x04006CB2 RID: 27826
	private string currentPath = "";
}
