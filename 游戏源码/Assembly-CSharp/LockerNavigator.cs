using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D7A RID: 7546
public class LockerNavigator : KModalScreen
{
	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x06009DB5 RID: 40373 RVA: 0x00106994 File Offset: 0x00104B94
	public GameObject ContentSlot
	{
		get
		{
			return this.slot.gameObject;
		}
	}

	// Token: 0x06009DB6 RID: 40374 RVA: 0x001069A1 File Offset: 0x00104BA1
	protected override void OnActivate()
	{
		LockerNavigator.Instance = this;
		this.Show(false);
		this.backButton.onClick += this.OnClickBack;
	}

	// Token: 0x06009DB7 RID: 40375 RVA: 0x001069C7 File Offset: 0x00104BC7
	public override float GetSortKey()
	{
		return 41f;
	}

	// Token: 0x06009DB8 RID: 40376 RVA: 0x001069CE File Offset: 0x00104BCE
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.PopScreen();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009DB9 RID: 40377 RVA: 0x001069F0 File Offset: 0x00104BF0
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show)
		{
			this.PopAllScreens();
		}
		StreamedTextures.SetBundlesLoaded(show);
	}

	// Token: 0x06009DBA RID: 40378 RVA: 0x00106A08 File Offset: 0x00104C08
	private void OnClickBack()
	{
		this.PopScreen();
	}

	// Token: 0x06009DBB RID: 40379 RVA: 0x003C82F4 File Offset: 0x003C64F4
	public void PushScreen(GameObject screen, System.Action onClose = null)
	{
		if (screen == null)
		{
			return;
		}
		if (this.navigationHistory.Count == 0)
		{
			this.Show(true);
			if (!LockerNavigator.didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
			{
				LockerNavigator.MakeDataCollectionWarningPopup(base.gameObject.transform.parent.gameObject);
				LockerNavigator.didDisplayDataCollectionWarningPopupOnce = true;
			}
		}
		if (this.navigationHistory.Count > 0 && screen == this.navigationHistory[this.navigationHistory.Count - 1].screen)
		{
			return;
		}
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(false);
		}
		this.navigationHistory.Add(new LockerNavigator.HistoryEntry(screen, onClose));
		this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.RefreshButtons();
	}

	// Token: 0x06009DBC RID: 40380 RVA: 0x003C840C File Offset: 0x003C660C
	public bool PopScreen()
	{
		while (this.preventScreenPop.Count > 0)
		{
			int index = this.preventScreenPop.Count - 1;
			Func<bool> func = this.preventScreenPop[index];
			this.preventScreenPop.RemoveAt(index);
			if (func())
			{
				return true;
			}
		}
		int index2 = this.navigationHistory.Count - 1;
		LockerNavigator.HistoryEntry historyEntry = this.navigationHistory[index2];
		historyEntry.screen.SetActive(false);
		if (historyEntry.onClose.IsSome())
		{
			historyEntry.onClose.Unwrap()();
		}
		this.navigationHistory.RemoveAt(index2);
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
			this.RefreshButtons();
			return true;
		}
		this.Show(false);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial", true);
		return false;
	}

	// Token: 0x06009DBD RID: 40381 RVA: 0x003C8508 File Offset: 0x003C6708
	public void PopAllScreens()
	{
		if (this.navigationHistory.Count == 0 && this.preventScreenPop.Count == 0)
		{
			return;
		}
		int num = 0;
		while (this.PopScreen())
		{
			if (num > 100)
			{
				DebugUtil.DevAssert(false, string.Format("Can't close all LockerNavigator screens, hit limit of trying to close {0} screens", 100), null);
				return;
			}
			num++;
		}
	}

	// Token: 0x06009DBE RID: 40382 RVA: 0x00106A11 File Offset: 0x00104C11
	private void RefreshButtons()
	{
		this.backButton.isInteractable = true;
	}

	// Token: 0x06009DBF RID: 40383 RVA: 0x003C8560 File Offset: 0x003C6760
	public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
	{
		InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.ContentSlot, false);
		configureDialogFn(dialog);
		dialog.Activate();
		dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
		dialog.gameObject.AddOrGet<RectTransform>().Fill();
		Func<bool> preventScreenPopFn = delegate()
		{
			dialog.Deactivate();
			return true;
		};
		this.preventScreenPop.Add(preventScreenPopFn);
		InfoDialogScreen dialog2 = dialog;
		dialog2.onDeactivateFn = (System.Action)Delegate.Combine(dialog2.onDeactivateFn, new System.Action(delegate()
		{
			this.preventScreenPop.Remove(preventScreenPopFn);
		}));
	}

	// Token: 0x06009DC0 RID: 40384 RVA: 0x003C8628 File Offset: 0x003C6828
	public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
	{
		Action<InfoDialogScreen> <>9__2;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, true);
			string text = UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS;
			Action<InfoDialogScreen> action;
			if ((action = <>9__2) == null)
			{
				action = (<>9__2 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					LockerNavigator.Instance.PopAllScreens();
					LockerMenuScreen.Instance.Show(false);
					Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, true).ShowMetricsScreen();
				});
			}
			infoDialogScreen.AddOption(text, action, false);
		});
	}

	// Token: 0x04007B85 RID: 31621
	public static LockerNavigator Instance;

	// Token: 0x04007B86 RID: 31622
	[SerializeField]
	private RectTransform slot;

	// Token: 0x04007B87 RID: 31623
	[SerializeField]
	private KButton backButton;

	// Token: 0x04007B88 RID: 31624
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007B89 RID: 31625
	[SerializeField]
	public GameObject kleiInventoryScreen;

	// Token: 0x04007B8A RID: 31626
	[SerializeField]
	public GameObject duplicantCatalogueScreen;

	// Token: 0x04007B8B RID: 31627
	[SerializeField]
	public GameObject outfitDesignerScreen;

	// Token: 0x04007B8C RID: 31628
	[SerializeField]
	public GameObject outfitBrowserScreen;

	// Token: 0x04007B8D RID: 31629
	[SerializeField]
	public GameObject joyResponseDesignerScreen;

	// Token: 0x04007B8E RID: 31630
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04007B8F RID: 31631
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04007B90 RID: 31632
	private List<LockerNavigator.HistoryEntry> navigationHistory = new List<LockerNavigator.HistoryEntry>();

	// Token: 0x04007B91 RID: 31633
	private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

	// Token: 0x04007B92 RID: 31634
	private static bool didDisplayDataCollectionWarningPopupOnce;

	// Token: 0x04007B93 RID: 31635
	public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

	// Token: 0x02001D7B RID: 7547
	public readonly struct HistoryEntry
	{
		// Token: 0x06009DC2 RID: 40386 RVA: 0x00106A48 File Offset: 0x00104C48
		public HistoryEntry(GameObject screen, System.Action onClose = null)
		{
			this.screen = screen;
			this.onClose = onClose;
		}

		// Token: 0x04007B94 RID: 31636
		public readonly GameObject screen;

		// Token: 0x04007B95 RID: 31637
		public readonly Option<System.Action> onClose;
	}
}
