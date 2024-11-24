using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
public class HappySinger : GameStateMachine<HappySinger, HappySinger.Instance>
{
	// Token: 0x0600236C RID: 9068 RVA: 0x001C5438 File Offset: 0x001C3638
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsJoySinger").ToggleLoopingSound(this.soundPath, null, true, true, true).ToggleAnims("anim_loco_singer_kanim", 0f).ToggleAnims("anim_idle_singer_kanim", 0f).EventHandler(GameHashes.TagsChanged, delegate(HappySinger.Instance smi, object obj)
		{
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Enter(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX = Util.KInstantiate(EffectPrefabs.Instance.HappySingerFX, smi.master.transform.GetPosition() + this.offset);
			smi.musicParticleFX.transform.SetParent(smi.master.transform);
			smi.CreatePasserbyReactable();
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Update(delegate(HappySinger.Instance smi, float dt)
		{
			if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
			{
				smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
			}
		}, UpdateRate.SIM_1000ms, false).Exit(delegate(HappySinger.Instance smi)
		{
			Util.KDestroyGameObject(smi.musicParticleFX);
			smi.ClearPasserbyReactable();
			smi.musicParticleFX.SetActive(false);
		});
	}

	// Token: 0x04001764 RID: 5988
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04001765 RID: 5989
	public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04001766 RID: 5990
	public HappySinger.OverjoyedStates overjoyed;

	// Token: 0x04001767 RID: 5991
	public string soundPath = GlobalAssets.GetSound("DupeSinging_NotesFX_LP", false);

	// Token: 0x020007B8 RID: 1976
	public class OverjoyedStates : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001768 RID: 5992
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04001769 RID: 5993
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x020007B9 RID: 1977
	public new class Instance : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06002370 RID: 9072 RVA: 0x000B70E7 File Offset: 0x000B52E7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x001C55D4 File Offset: 0x001C37D4
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote sing = Db.Get().Emotes.Minion.Sing;
				emoteReactable.SetEmote(sing).SetThought(Db.Get().Thoughts.CatchyTune).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000B70F0 File Offset: 0x000B52F0
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000B7116 File Offset: 0x000B5316
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.Trigger(-1278274506, null);
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000B7124 File Offset: 0x000B5324
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000B712F File Offset: 0x000B532F
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x0400176A RID: 5994
		private Reactable passerbyReactable;

		// Token: 0x0400176B RID: 5995
		public GameObject musicParticleFX;

		// Token: 0x0400176C RID: 5996
		public SpeechMonitor.Instance speechMonitor;
	}
}
