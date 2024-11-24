using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F04 RID: 7940
public class ScenariosMenu : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x0600A762 RID: 42850 RVA: 0x003F8928 File Offset: 0x003F6B28
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.RebuildScreen();
	}

	// Token: 0x0600A763 RID: 42851 RVA: 0x003F89AC File Offset: 0x003F6BAC
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.RebuildUGCButtons();
	}

	// Token: 0x0600A764 RID: 42852 RVA: 0x003F8A10 File Offset: 0x003F6C10
	private void RebuildUGCButtons()
	{
		ListPool<SteamUGCService.Mod, ScenariosMenu>.PooledList pooledList = ListPool<SteamUGCService.Mod, ScenariosMenu>.Allocate();
		bool flag = pooledList.Count > 0;
		this.noScenariosText.gameObject.SetActive(!flag);
		this.contentRoot.gameObject.SetActive(flag);
		bool flag2 = true;
		if (pooledList.Count != 0)
		{
			for (int i = 0; i < pooledList.Count; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.ugcButtonPrefab, this.ugcContainer, false);
				gameObject.name = pooledList[i].title + "_button";
				gameObject.gameObject.SetActive(true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Title").SetText(pooledList[i].title);
				Texture2D previewImage = pooledList[i].previewImage;
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				KButton component2 = gameObject.GetComponent<KButton>();
				int index = i;
				PublishedFileId_t item = pooledList[index].fileId;
				component2.onClick += delegate()
				{
					this.ShowDetails(item);
				};
				component2.onDoubleClick += delegate()
				{
					this.LoadScenario(item);
				};
				this.buttons.Add(gameObject);
				if (item == this.activeItem)
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.HideDetails();
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A765 RID: 42853 RVA: 0x003F8BBC File Offset: 0x003F6DBC
	private void LoadScenario(PublishedFileId_t item)
	{
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		DebugUtil.LogArgs(new object[]
		{
			"LoadScenario",
			text,
			num,
			num2
		});
		System.DateTime dateTime;
		byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, new string[]
		{
			".sav"
		}, out dateTime, false);
		string text2 = Path.Combine(SaveLoader.GetSavePrefix(), "scenario.sav");
		File.WriteAllBytes(text2, bytesFromZip);
		SaveLoader.SetActiveSaveFilePath(text2);
		Time.timeScale = 0f;
		App.LoadScene("backend");
	}

	// Token: 0x0600A766 RID: 42854 RVA: 0x00106500 File Offset: 0x00104700
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x0600A767 RID: 42855 RVA: 0x003F8C50 File Offset: 0x003F6E50
	private void ShowDetails(PublishedFileId_t item)
	{
		this.activeItem = item;
		SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(item);
		if (mod != null)
		{
			this.scenarioTitle.text = mod.title;
			this.scenarioDetails.text = mod.description;
		}
		this.loadScenarioButton.onClick += delegate()
		{
			this.LoadScenario(item);
		};
		this.detailsRoot.gameObject.SetActive(true);
	}

	// Token: 0x0600A768 RID: 42856 RVA: 0x0010CA29 File Offset: 0x0010AC29
	private void HideDetails()
	{
		this.detailsRoot.gameObject.SetActive(false);
	}

	// Token: 0x0600A769 RID: 42857 RVA: 0x0010CA3C File Offset: 0x0010AC3C
	protected override void OnActivate()
	{
		base.OnActivate();
		SteamUGCService.Instance.AddClient(this);
		this.HideDetails();
	}

	// Token: 0x0600A76A RID: 42858 RVA: 0x0010CA55 File Offset: 0x0010AC55
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		SteamUGCService.Instance.RemoveClient(this);
	}

	// Token: 0x0600A76B RID: 42859 RVA: 0x0010CA68 File Offset: 0x0010AC68
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=scenario");
	}

	// Token: 0x0600A76C RID: 42860 RVA: 0x0010CA74 File Offset: 0x0010AC74
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		this.RebuildScreen();
	}

	// Token: 0x0400839A RID: 33690
	public const string TAG_SCENARIO = "scenario";

	// Token: 0x0400839B RID: 33691
	public KButton textButton;

	// Token: 0x0400839C RID: 33692
	public KButton dismissButton;

	// Token: 0x0400839D RID: 33693
	public KButton closeButton;

	// Token: 0x0400839E RID: 33694
	public KButton workshopButton;

	// Token: 0x0400839F RID: 33695
	public KButton loadScenarioButton;

	// Token: 0x040083A0 RID: 33696
	[Space]
	public GameObject ugcContainer;

	// Token: 0x040083A1 RID: 33697
	public GameObject ugcButtonPrefab;

	// Token: 0x040083A2 RID: 33698
	public LocText noScenariosText;

	// Token: 0x040083A3 RID: 33699
	public RectTransform contentRoot;

	// Token: 0x040083A4 RID: 33700
	public RectTransform detailsRoot;

	// Token: 0x040083A5 RID: 33701
	public LocText scenarioTitle;

	// Token: 0x040083A6 RID: 33702
	public LocText scenarioDetails;

	// Token: 0x040083A7 RID: 33703
	private PublishedFileId_t activeItem;

	// Token: 0x040083A8 RID: 33704
	private List<GameObject> buttons = new List<GameObject>();
}
