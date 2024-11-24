using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class LockerMenuScreen : KModalScreen
{
	public static LockerMenuScreen Instance;

	[SerializeField]
	private MultiToggle buttonInventory;

	[SerializeField]
	private MultiToggle buttonDuplicants;

	[SerializeField]
	private MultiToggle buttonOutfitBroswer;

	[SerializeField]
	private MultiToggle buttonClaimItems;

	[SerializeField]
	private LocText descriptionArea;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private GameObject dropsAvailableNotification;

	[SerializeField]
	private GameObject noConnectionIcon;

	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	private const string MUSIC_PARAMETER = "SupplyClosetView";

	[SerializeField]
	private Material desatUIMaterial;

	private bool refreshRequested;

	protected override void OnActivate()
	{
		Instance = this;
		Show(show: false);
	}

	public override float GetSortKey()
	{
		return 40f;
	}

	public void ShowInventoryScreen()
	{
		if (!base.isActiveAndEnabled)
		{
			Show();
		}
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "inventory");
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = buttonInventory;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			ShowInventoryScreen();
		});
		MultiToggle multiToggle2 = buttonDuplicants;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, (System.Action)delegate
		{
			MinionBrowserScreenConfig.Personalities().ApplyAndOpenScreen();
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "dupe");
		});
		MultiToggle multiToggle3 = buttonOutfitBroswer;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, (System.Action)delegate
		{
			OutfitBrowserScreenConfig.Mannequin().ApplyAndOpenScreen();
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "wardrobe");
		});
		closeButton.onClick += delegate
		{
			Show(show: false);
		};
		ConfigureHoverForButton(buttonInventory, UI.LOCKER_MENU.BUTTON_INVENTORY_DESCRIPTION);
		ConfigureHoverForButton(buttonDuplicants, UI.LOCKER_MENU.BUTTON_DUPLICANTS_DESCRIPTION);
		ConfigureHoverForButton(buttonOutfitBroswer, UI.LOCKER_MENU.BUTTON_OUTFITS_DESCRIPTION);
		descriptionArea.text = UI.LOCKER_MENU.DEFAULT_DESCRIPTION;
	}

	private void ConfigureHoverForButton(MultiToggle toggle, string desc, bool useHoverColor = true)
	{
		Color defaultColor = new Color(0.30980393f, 29f / 85f, 0.38431373f, 1f);
		Color hoverColor = new Color(0.7019608f, 31f / 85f, 8f / 15f, 1f);
		toggle.onEnter = null;
		toggle.onExit = null;
		toggle.onEnter = (System.Action)Delegate.Combine(toggle.onEnter, OnHoverEnterFn(toggle, desc));
		toggle.onExit = (System.Action)Delegate.Combine(toggle.onExit, OnHoverExitFn(toggle));
		System.Action OnHoverEnterFn(MultiToggle toggle, string desc)
		{
			Image headerBackground2 = toggle.GetComponent<HierarchyReferences>().GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			return delegate
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
				if (useHoverColor)
				{
					headerBackground2.color = hoverColor;
				}
				descriptionArea.text = desc;
			};
		}
		System.Action OnHoverExitFn(MultiToggle toggle)
		{
			Image headerBackground = toggle.GetComponent<HierarchyReferences>().GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			return delegate
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
				if (useHoverColor)
				{
					headerBackground.color = defaultColor;
				}
				descriptionArea.text = UI.LOCKER_MENU.DEFAULT_DESCRIPTION;
			};
		}
	}

	public override void Show(bool show = true)
	{
		base.Show(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(paused: true, 0.5f);
			MusicManager.instance.PlaySong("Music_SupplyCloset");
			ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(TriggerShouldRefreshClaimItems);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(paused: false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset");
			}
		}
		RefreshClaimItemsButton();
	}

	private void TriggerShouldRefreshClaimItems()
	{
		refreshRequested = true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	private void RefreshClaimItemsButton()
	{
		noConnectionIcon.SetActive(!ThreadedHttps<KleiAccount>.Instance.HasValidTicket());
		refreshRequested = false;
		bool hasClaimable = PermitItems.HasUnopenedItem();
		dropsAvailableNotification.SetActive(hasClaimable);
		buttonClaimItems.ChangeState((!hasClaimable) ? 1 : 0);
		buttonClaimItems.GetComponent<HierarchyReferences>().GetReference<Image>("FGIcon").material = (hasClaimable ? null : desatUIMaterial);
		buttonClaimItems.onClick = null;
		MultiToggle multiToggle = buttonClaimItems;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			if (hasClaimable)
			{
				UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(includeInactive: true).Show();
				Show(show: false);
			}
		});
		ConfigureHoverForButton(buttonClaimItems, hasClaimable ? UI.LOCKER_MENU.BUTTON_CLAIM_DESCRIPTION : UI.LOCKER_MENU.BUTTON_CLAIM_NONE_DESCRIPTION, hasClaimable);
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			Show(show: false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(paused: false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset");
			}
		}
		base.OnKeyDown(e);
	}

	private void Update()
	{
		if (refreshRequested)
		{
			RefreshClaimItemsButton();
		}
	}
}
