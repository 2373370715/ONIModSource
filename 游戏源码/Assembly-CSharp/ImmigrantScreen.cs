using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02001AD4 RID: 6868
public class ImmigrantScreen : CharacterSelectionController
{
	// Token: 0x06008FD0 RID: 36816 RVA: 0x000FE003 File Offset: 0x000FC203
	public static void DestroyInstance()
	{
		ImmigrantScreen.instance = null;
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06008FD1 RID: 36817 RVA: 0x000FE00B File Offset: 0x000FC20B
	public Telepad Telepad
	{
		get
		{
			return this.telepad;
		}
	}

	// Token: 0x06008FD2 RID: 36818 RVA: 0x000FE013 File Offset: 0x000FC213
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06008FD3 RID: 36819 RVA: 0x00378540 File Offset: 0x00376740
	protected override void OnSpawn()
	{
		this.activateOnSpawn = false;
		base.ConsumeMouseScroll = false;
		base.OnSpawn();
		base.IsStarterMinion = false;
		this.rejectButton.onClick += this.OnRejectAll;
		this.confirmRejectionBtn.onClick += this.OnRejectionConfirmed;
		this.cancelRejectionBtn.onClick += this.OnRejectionCancelled;
		ImmigrantScreen.instance = this;
		this.title.text = UI.IMMIGRANTSCREEN.IMMIGRANTSCREENTITLE;
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.PROCEEDBUTTON;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.Show(false);
	}

	// Token: 0x06008FD4 RID: 36820 RVA: 0x00378600 File Offset: 0x00376800
	protected override void OnShow(bool show)
	{
		if (show)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("Dialog_Popup", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
			MusicManager.instance.PlaySong("Music_SelectDuplicant", false);
			this.hasShown = true;
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.StopSong("Music_SelectDuplicant", true, STOP_MODE.ALLOWFADEOUT);
			}
			if (Immigration.Instance.ImmigrantsAvailable && this.hasShown)
			{
				AudioMixer.instance.Start(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
			}
		}
		base.OnShow(show);
	}

	// Token: 0x06008FD5 RID: 36821 RVA: 0x000FE01B File Offset: 0x000FC21B
	public void DebugShuffleOptions()
	{
		this.OnRejectionConfirmed();
		Immigration.Instance.timeBeforeSpawn = 0f;
	}

	// Token: 0x06008FD6 RID: 36822 RVA: 0x000FE032 File Offset: 0x000FC232
	public override void OnPressBack()
	{
		if (this.rejectConfirmationScreen.activeSelf)
		{
			this.OnRejectionCancelled();
			return;
		}
		base.OnPressBack();
	}

	// Token: 0x06008FD7 RID: 36823 RVA: 0x000FE04E File Offset: 0x000FC24E
	public override void Deactivate()
	{
		this.Show(false);
	}

	// Token: 0x06008FD8 RID: 36824 RVA: 0x000FE057 File Offset: 0x000FC257
	public static void InitializeImmigrantScreen(Telepad telepad)
	{
		ImmigrantScreen.instance.Initialize(telepad);
		ImmigrantScreen.instance.Show(true);
	}

	// Token: 0x06008FD9 RID: 36825 RVA: 0x003786B8 File Offset: 0x003768B8
	private void Initialize(Telepad telepad)
	{
		this.InitializeContainers();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.SetReshufflingState(false);
			}
		}
		this.telepad = telepad;
	}

	// Token: 0x06008FDA RID: 36826 RVA: 0x00378728 File Offset: 0x00376928
	protected override void OnProceed()
	{
		this.telepad.OnAcceptDelivery(this.selectedDeliverables[0]);
		this.Show(false);
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.PlaySong("Stinger_NewDuplicant", false);
	}

	// Token: 0x06008FDB RID: 36827 RVA: 0x000FE06F File Offset: 0x000FC26F
	private void OnRejectAll()
	{
		this.rejectConfirmationScreen.transform.SetAsLastSibling();
		this.rejectConfirmationScreen.SetActive(true);
	}

	// Token: 0x06008FDC RID: 36828 RVA: 0x000FE08D File Offset: 0x000FC28D
	private void OnRejectionCancelled()
	{
		this.rejectConfirmationScreen.SetActive(false);
	}

	// Token: 0x06008FDD RID: 36829 RVA: 0x003787C4 File Offset: 0x003769C4
	private void OnRejectionConfirmed()
	{
		this.telepad.RejectAll();
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		this.rejectConfirmationScreen.SetActive(false);
		this.Show(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x04006C9E RID: 27806
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006C9F RID: 27807
	[SerializeField]
	private KButton rejectButton;

	// Token: 0x04006CA0 RID: 27808
	[SerializeField]
	private LocText title;

	// Token: 0x04006CA1 RID: 27809
	[SerializeField]
	private GameObject rejectConfirmationScreen;

	// Token: 0x04006CA2 RID: 27810
	[SerializeField]
	private KButton confirmRejectionBtn;

	// Token: 0x04006CA3 RID: 27811
	[SerializeField]
	private KButton cancelRejectionBtn;

	// Token: 0x04006CA4 RID: 27812
	public static ImmigrantScreen instance;

	// Token: 0x04006CA5 RID: 27813
	private Telepad telepad;

	// Token: 0x04006CA6 RID: 27814
	private bool hasShown;
}
