using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class LockerNavigator : KModalScreen
{
	public readonly struct HistoryEntry
	{
		public readonly GameObject screen;

		public readonly Option<System.Action> onClose;

		public HistoryEntry(GameObject screen, System.Action onClose = null)
		{
			this.screen = screen;
			this.onClose = onClose;
		}
	}

	public static LockerNavigator Instance;

	[SerializeField]
	private RectTransform slot;

	[SerializeField]
	private KButton backButton;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	public GameObject kleiInventoryScreen;

	[SerializeField]
	public GameObject duplicantCatalogueScreen;

	[SerializeField]
	public GameObject outfitDesignerScreen;

	[SerializeField]
	public GameObject outfitBrowserScreen;

	[SerializeField]
	public GameObject joyResponseDesignerScreen;

	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	private const string MUSIC_PARAMETER = "SupplyClosetView";

	private List<HistoryEntry> navigationHistory = new List<HistoryEntry>();

	private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

	private static bool didDisplayDataCollectionWarningPopupOnce;

	public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

	public GameObject ContentSlot => slot.gameObject;

	protected override void OnActivate()
	{
		Instance = this;
		Show(show: false);
		backButton.onClick += OnClickBack;
	}

	public override float GetSortKey()
	{
		return 41f;
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			PopScreen();
		}
		base.OnKeyDown(e);
	}

	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show)
		{
			PopAllScreens();
		}
		StreamedTextures.SetBundlesLoaded(show);
	}

	private void OnClickBack()
	{
		PopScreen();
	}

	public void PushScreen(GameObject screen, System.Action onClose = null)
	{
		if (screen == null)
		{
			return;
		}
		if (navigationHistory.Count == 0)
		{
			Show();
			if (!didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
			{
				MakeDataCollectionWarningPopup(base.gameObject.transform.parent.gameObject);
				didDisplayDataCollectionWarningPopupOnce = true;
			}
		}
		if (navigationHistory.Count <= 0 || !(screen == navigationHistory[navigationHistory.Count - 1].screen))
		{
			if (navigationHistory.Count > 0)
			{
				navigationHistory[navigationHistory.Count - 1].screen.SetActive(value: false);
			}
			navigationHistory.Add(new HistoryEntry(screen, onClose));
			navigationHistory[navigationHistory.Count - 1].screen.SetActive(value: true);
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			RefreshButtons();
		}
	}

	public bool PopScreen()
	{
		while (preventScreenPop.Count > 0)
		{
			int index = preventScreenPop.Count - 1;
			Func<bool> func = preventScreenPop[index];
			preventScreenPop.RemoveAt(index);
			if (func())
			{
				return true;
			}
		}
		int index2 = navigationHistory.Count - 1;
		HistoryEntry historyEntry = navigationHistory[index2];
		historyEntry.screen.SetActive(value: false);
		if (historyEntry.onClose.IsSome())
		{
			historyEntry.onClose.Unwrap()();
		}
		navigationHistory.RemoveAt(index2);
		if (navigationHistory.Count > 0)
		{
			navigationHistory[navigationHistory.Count - 1].screen.SetActive(value: true);
			RefreshButtons();
			return true;
		}
		Show(show: false);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial");
		return false;
	}

	public void PopAllScreens()
	{
		if (navigationHistory.Count == 0 && preventScreenPop.Count == 0)
		{
			return;
		}
		int num = 0;
		while (PopScreen())
		{
			if (num > 100)
			{
				DebugUtil.DevAssert(test: false, $"Can't close all LockerNavigator screens, hit limit of trying to close {100} screens");
				break;
			}
			num++;
		}
	}

	private void RefreshButtons()
	{
		backButton.isInteractable = true;
	}

	public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
	{
		InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, ContentSlot);
		configureDialogFn(dialog);
		dialog.Activate();
		dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
		dialog.gameObject.AddOrGet<RectTransform>().Fill();
		Func<bool> preventScreenPopFn = delegate
		{
			dialog.Deactivate();
			return true;
		};
		preventScreenPop.Add(preventScreenPopFn);
		InfoDialogScreen infoDialogScreen = dialog;
		infoDialogScreen.onDeactivateFn = (System.Action)Delegate.Combine(infoDialogScreen.onDeactivateFn, (System.Action)delegate
		{
			preventScreenPop.Remove(preventScreenPopFn);
		});
	}

	public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
	{
		Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, rightSide: true)
				.AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS, delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					Instance.PopAllScreens();
					LockerMenuScreen.Instance.Show(show: false);
					Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, force_active: true).ShowMetricsScreen();
				});
		});
	}
}
