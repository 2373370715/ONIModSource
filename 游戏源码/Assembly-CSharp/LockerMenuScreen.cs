using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D74 RID: 7540
public class LockerMenuScreen : KModalScreen
{
	// Token: 0x06009D99 RID: 40345 RVA: 0x00106877 File Offset: 0x00104A77
	protected override void OnActivate()
	{
		LockerMenuScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06009D9A RID: 40346 RVA: 0x00106886 File Offset: 0x00104A86
	public override float GetSortKey()
	{
		return 40f;
	}

	// Token: 0x06009D9B RID: 40347 RVA: 0x0010688D File Offset: 0x00104A8D
	public void ShowInventoryScreen()
	{
		if (!base.isActiveAndEnabled)
		{
			this.Show(true);
		}
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "inventory", true);
	}

	// Token: 0x06009D9C RID: 40348 RVA: 0x003C7DD8 File Offset: 0x003C5FD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.buttonInventory;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ShowInventoryScreen();
		}));
		MultiToggle multiToggle2 = this.buttonDuplicants;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			MinionBrowserScreenConfig.Personalities(default(Option<Personality>)).ApplyAndOpenScreen(null);
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "dupe", true);
		}));
		MultiToggle multiToggle3 = this.buttonOutfitBroswer;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			OutfitBrowserScreenConfig.Mannequin().ApplyAndOpenScreen();
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "wardrobe", true);
		}));
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.ConfigureHoverForButton(this.buttonInventory, UI.LOCKER_MENU.BUTTON_INVENTORY_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonDuplicants, UI.LOCKER_MENU.BUTTON_DUPLICANTS_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonOutfitBroswer, UI.LOCKER_MENU.BUTTON_OUTFITS_DESCRIPTION, true);
		this.descriptionArea.text = UI.LOCKER_MENU.DEFAULT_DESCRIPTION;
	}

	// Token: 0x06009D9D RID: 40349 RVA: 0x003C7EF8 File Offset: 0x003C60F8
	private void ConfigureHoverForButton(MultiToggle toggle, string desc, bool useHoverColor = true)
	{
		LockerMenuScreen.<>c__DisplayClass17_0 CS$<>8__locals1 = new LockerMenuScreen.<>c__DisplayClass17_0();
		CS$<>8__locals1.useHoverColor = useHoverColor;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.defaultColor = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		CS$<>8__locals1.hoverColor = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		toggle.onEnter = null;
		toggle.onExit = null;
		toggle.onEnter = (System.Action)Delegate.Combine(toggle.onEnter, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverEnterFn|0(toggle, desc));
		toggle.onExit = (System.Action)Delegate.Combine(toggle.onExit, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverExitFn|1(toggle));
	}

	// Token: 0x06009D9E RID: 40350 RVA: 0x003C7FA0 File Offset: 0x003C61A0
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(true, 0.5f);
			MusicManager.instance.PlaySong("Music_SupplyCloset", false);
			ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(new KleiAccount.GetUserIDdelegate(this.TriggerShouldRefreshClaimItems), false);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		this.RefreshClaimItemsButton();
	}

	// Token: 0x06009D9F RID: 40351 RVA: 0x001068CD File Offset: 0x00104ACD
	private void TriggerShouldRefreshClaimItems()
	{
		this.refreshRequested = true;
	}

	// Token: 0x06009DA0 RID: 40352 RVA: 0x0010197B File Offset: 0x000FFB7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06009DA1 RID: 40353 RVA: 0x001068D6 File Offset: 0x00104AD6
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x06009DA2 RID: 40354 RVA: 0x003C8054 File Offset: 0x003C6254
	private void RefreshClaimItemsButton()
	{
		this.noConnectionIcon.SetActive(!ThreadedHttps<KleiAccount>.Instance.HasValidTicket());
		this.refreshRequested = false;
		bool hasClaimable = PermitItems.HasUnopenedItem();
		this.dropsAvailableNotification.SetActive(hasClaimable);
		this.buttonClaimItems.ChangeState(hasClaimable ? 0 : 1);
		this.buttonClaimItems.GetComponent<HierarchyReferences>().GetReference<Image>("FGIcon").material = (hasClaimable ? null : this.desatUIMaterial);
		this.buttonClaimItems.onClick = null;
		MultiToggle multiToggle = this.buttonClaimItems;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (!hasClaimable)
			{
				return;
			}
			UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
			this.Show(false);
		}));
		this.ConfigureHoverForButton(this.buttonClaimItems, hasClaimable ? UI.LOCKER_MENU.BUTTON_CLAIM_DESCRIPTION : UI.LOCKER_MENU.BUTTON_CLAIM_NONE_DESCRIPTION, hasClaimable);
	}

	// Token: 0x06009DA3 RID: 40355 RVA: 0x003C814C File Offset: 0x003C634C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009DA4 RID: 40356 RVA: 0x001068DE File Offset: 0x00104ADE
	private void Update()
	{
		if (this.refreshRequested)
		{
			this.RefreshClaimItemsButton();
		}
	}

	// Token: 0x04007B6A RID: 31594
	public static LockerMenuScreen Instance;

	// Token: 0x04007B6B RID: 31595
	[SerializeField]
	private MultiToggle buttonInventory;

	// Token: 0x04007B6C RID: 31596
	[SerializeField]
	private MultiToggle buttonDuplicants;

	// Token: 0x04007B6D RID: 31597
	[SerializeField]
	private MultiToggle buttonOutfitBroswer;

	// Token: 0x04007B6E RID: 31598
	[SerializeField]
	private MultiToggle buttonClaimItems;

	// Token: 0x04007B6F RID: 31599
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04007B70 RID: 31600
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007B71 RID: 31601
	[SerializeField]
	private GameObject dropsAvailableNotification;

	// Token: 0x04007B72 RID: 31602
	[SerializeField]
	private GameObject noConnectionIcon;

	// Token: 0x04007B73 RID: 31603
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04007B74 RID: 31604
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04007B75 RID: 31605
	[SerializeField]
	private Material desatUIMaterial;

	// Token: 0x04007B76 RID: 31606
	private bool refreshRequested;
}
