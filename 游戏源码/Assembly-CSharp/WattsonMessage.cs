using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B59 RID: 7001
public class WattsonMessage : KScreen
{
	// Token: 0x06009325 RID: 37669 RVA: 0x000EAD7D File Offset: 0x000E8F7D
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06009326 RID: 37670 RVA: 0x0038CB74 File Offset: 0x0038AD74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(-122303817, new Action<object>(this.OnNewBaseCreated));
		string welcomeMessage = CustomGameSettings.Instance.GetCurrentClusterLayout().welcomeMessage;
		if (welcomeMessage != null)
		{
			StringEntry stringEntry;
			this.message.SetText(Strings.TryGet(welcomeMessage, out stringEntry) ? stringEntry.String : welcomeMessage);
			return;
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.message.SetText(UI.WELCOMEMESSAGEBODY_SPACEDOUT);
			return;
		}
		this.message.SetText(UI.WELCOMEMESSAGEBODY);
	}

	// Token: 0x06009327 RID: 37671 RVA: 0x0010020B File Offset: 0x000FE40B
	private IEnumerator ExpandPanel()
	{
		this.button.isInteractable = false;
		if (CustomGameSettings.Instance.GetSettingsCoordinate().StartsWith("KF23"))
		{
			this.dialog.rectTransform().rotation = Quaternion.Euler(0f, 0f, -90f);
		}
		yield return SequenceUtil.WaitForSecondsRealtime(0.2f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		if (CustomGameSettings.Instance.GetSettingsCoordinate().StartsWith("KF23"))
		{
			Quaternion initialOrientation = Quaternion.Euler(0f, 0f, -90f);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			float t = 0f;
			float duration = 0.5f;
			while (t < duration)
			{
				t += Time.unscaledDeltaTime;
				this.dialog.rectTransform().rotation = Quaternion.Slerp(initialOrientation, Quaternion.identity, t / duration);
				yield return 0;
			}
			initialOrientation = default(Quaternion);
		}
		this.button.isInteractable = true;
		yield return null;
		yield break;
	}

	// Token: 0x06009328 RID: 37672 RVA: 0x0010021A File Offset: 0x000FE41A
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 1f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 0f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x06009329 RID: 37673 RVA: 0x0038CC08 File Offset: 0x0038AE08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hideScreensWhileActive.Add(NotificationScreen.Instance);
		this.hideScreensWhileActive.Add(OverlayMenu.Instance);
		if (PlanScreen.Instance != null)
		{
			this.hideScreensWhileActive.Add(PlanScreen.Instance);
		}
		if (BuildMenu.Instance != null)
		{
			this.hideScreensWhileActive.Add(BuildMenu.Instance);
		}
		this.hideScreensWhileActive.Add(ManagementMenu.Instance);
		this.hideScreensWhileActive.Add(ToolMenu.Instance);
		this.hideScreensWhileActive.Add(ToolMenu.Instance.PriorityScreen);
		this.hideScreensWhileActive.Add(PinnedResourcesPanel.Instance);
		this.hideScreensWhileActive.Add(TopLeftControlScreen.Instance);
		this.hideScreensWhileActive.Add(global::DateTime.Instance);
		this.hideScreensWhileActive.Add(BuildWatermark.Instance);
		this.hideScreensWhileActive.Add(BuildWatermark.Instance);
		this.hideScreensWhileActive.Add(ColonyDiagnosticScreen.Instance);
		if (WorldSelector.Instance != null)
		{
			this.hideScreensWhileActive.Add(WorldSelector.Instance);
		}
		foreach (KScreen kscreen in this.hideScreensWhileActive)
		{
			kscreen.Show(false);
		}
	}

	// Token: 0x0600932A RID: 37674 RVA: 0x0038CD6C File Offset: 0x0038AF6C
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x0600932B RID: 37675 RVA: 0x0038CDC4 File Offset: 0x0038AFC4
	protected override void OnActivate()
	{
		global::Debug.Log("WattsonMessage OnActivate");
		base.OnActivate();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NewBaseSetupSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().IntroNIS);
		AudioMixer.instance.activeNIS = true;
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad != null)
		{
			KAnimControllerBase kac = telepad.GetComponent<KAnimControllerBase>();
			kac.Play(WattsonMessage.WorkLoopAnims, KAnim.PlayMode.Loop);
			NameDisplayScreen.Instance.gameObject.SetActive(false);
			for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
			{
				int idx = i + 1;
				MinionIdentity minionIdentity = Components.LiveMinionIdentities[i];
				minionIdentity.gameObject.transform.SetPosition(new Vector3(telepad.transform.GetPosition().x + (float)idx - 1.5f, telepad.transform.GetPosition().y, minionIdentity.gameObject.transform.GetPosition().z));
				GameObject gameObject = minionIdentity.gameObject;
				ChoreProvider chore_provider = gameObject.GetComponent<ChoreProvider>();
				EmoteChore chorePre = new EmoteChore(chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", new HashedString[]
				{
					"portalbirth_pre_" + idx.ToString()
				}, KAnim.PlayMode.Loop, false);
				UIScheduler.Instance.Schedule("DupeBirth", (float)idx * 0.5f, delegate(object data)
				{
					chorePre.Cancel("Done looping");
					EmoteChore emoteChore = new EmoteChore(chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", new HashedString[]
					{
						"portalbirth_" + idx.ToString()
					}, null);
					emoteChore.onComplete = (Action<Chore>)Delegate.Combine(emoteChore.onComplete, new Action<Chore>(delegate(Chore param)
					{
						this.birthsComplete++;
						if (this.birthsComplete == Components.LiveMinionIdentities.Count - 1 && base.IsActive())
						{
							NameDisplayScreen.Instance.gameObject.SetActive(true);
							this.PauseAndShowMessage();
						}
					}));
				}, null, null);
			}
			UIScheduler.Instance.Schedule("Welcome", 6.6f, delegate(object data)
			{
				kac.Play(new HashedString[]
				{
					"working_pst",
					"idle"
				}, KAnim.PlayMode.Once);
			}, null, null);
			CameraController.Instance.DisableUserCameraControl = true;
		}
		else
		{
			global::Debug.LogWarning("Failed to spawn telepad - does the starting base template lack a 'Headquarters' ?");
			this.PauseAndShowMessage();
		}
		this.scheduleHandles.Add(UIScheduler.Instance.Schedule("GoHome", 0.1f, delegate(object data)
		{
			CameraController.Instance.OrthographicSize = TuningData<WattsonMessage.Tuning>.Get().initialOrthographicSize;
			CameraController.Instance.CameraGoHome(0.5f);
			this.startFade = true;
			MusicManager.instance.PlaySong(this.WelcomeMusic, false);
		}, null, null));
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x0600932C RID: 37676 RVA: 0x0038D03C File Offset: 0x0038B23C
	private string WelcomeMusic
	{
		get
		{
			string musicWelcome = CustomGameSettings.Instance.GetCurrentClusterLayout().clusterAudio.musicWelcome;
			if (!musicWelcome.IsNullOrWhiteSpace())
			{
				return musicWelcome;
			}
			return "Music_WattsonMessage";
		}
	}

	// Token: 0x0600932D RID: 37677 RVA: 0x0038D070 File Offset: 0x0038B270
	protected void PauseAndShowMessage()
	{
		SpeedControlScreen.Instance.Pause(false, false);
		base.StartCoroutine(this.ExpandPanel());
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
	}

	// Token: 0x0600932E RID: 37678 RVA: 0x0038D0D4 File Offset: 0x0038B2D4
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().IntroNIS, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StartPersistentSnapshots();
		MusicManager.instance.StopSong(this.WelcomeMusic, true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.WattsonStartDynamicMusic();
		AudioMixer.instance.activeNIS = false;
		DemoTimer.Instance.CountdownActive = true;
		SpeedControlScreen.Instance.Unpause(false);
		CameraController.Instance.DisableUserCameraControl = false;
		foreach (SchedulerHandle schedulerHandle in this.scheduleHandles)
		{
			schedulerHandle.ClearScheduler();
		}
		UIScheduler.Instance.Schedule("fadeInUI", 0.5f, delegate(object d)
		{
			foreach (KScreen kscreen in this.hideScreensWhileActive)
			{
				if (!(kscreen == null))
				{
					kscreen.SetShouldFadeIn(true);
					kscreen.Show(true);
				}
			}
			CameraController.Instance.SetMaxOrthographicSize(20f);
			Game.Instance.StartDelayedInitialSave();
			UIScheduler.Instance.Schedule("InitialScreenshot", 1f, delegate(object data)
			{
				Game.Instance.timelapser.InitialScreenshot();
			}, null, null);
			GameScheduler.Instance.Schedule("BasicTutorial", 1.5f, delegate(object data)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Basics, true);
			}, null, null);
			GameScheduler.Instance.Schedule("WelcomeTutorial", 2f, delegate(object data)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Welcome, true);
			}, null, null);
			GameScheduler.Instance.Schedule("DiggingTutorial", 420f, delegate(object data)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
			}, null, null);
		}, null, null);
		Game.Instance.SetGameStarted();
		if (TopLeftControlScreen.Instance != null)
		{
			TopLeftControlScreen.Instance.RefreshName();
		}
	}

	// Token: 0x0600932F RID: 37679 RVA: 0x00100229 File Offset: 0x000FE429
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			CameraController.Instance.CameraGoHome(2f);
			this.Deactivate();
		}
		e.Consumed = true;
	}

	// Token: 0x06009330 RID: 37680 RVA: 0x000FDE18 File Offset: 0x000FC018
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x06009331 RID: 37681 RVA: 0x00100250 File Offset: 0x000FE450
	private void OnNewBaseCreated(object data)
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x04006F6F RID: 28527
	private const float STARTTIME = 0.1f;

	// Token: 0x04006F70 RID: 28528
	private const float ENDTIME = 6.6f;

	// Token: 0x04006F71 RID: 28529
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x04006F72 RID: 28530
	private const float expandedHeight = 300f;

	// Token: 0x04006F73 RID: 28531
	[SerializeField]
	private GameObject dialog;

	// Token: 0x04006F74 RID: 28532
	[SerializeField]
	private RectTransform content;

	// Token: 0x04006F75 RID: 28533
	[SerializeField]
	private LocText message;

	// Token: 0x04006F76 RID: 28534
	[SerializeField]
	private Image bg;

	// Token: 0x04006F77 RID: 28535
	[SerializeField]
	private KButton button;

	// Token: 0x04006F78 RID: 28536
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x04006F79 RID: 28537
	private List<KScreen> hideScreensWhileActive = new List<KScreen>();

	// Token: 0x04006F7A RID: 28538
	private bool startFade;

	// Token: 0x04006F7B RID: 28539
	private List<SchedulerHandle> scheduleHandles = new List<SchedulerHandle>();

	// Token: 0x04006F7C RID: 28540
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04006F7D RID: 28541
	private int birthsComplete;

	// Token: 0x02001B5A RID: 7002
	public class Tuning : TuningData<WattsonMessage.Tuning>
	{
		// Token: 0x04006F7E RID: 28542
		public float initialOrthographicSize;
	}
}
