using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020015ED RID: 5613
public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
	// Token: 0x06007445 RID: 29765 RVA: 0x00302CC0 File Offset: 0x00300EC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
		this.satisfied.DoNothing();
		this.talking.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.BeginTalking)).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.mouth).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.EndTalking));
	}

	// Token: 0x06007446 RID: 29766 RVA: 0x00302D5C File Offset: 0x00300F5C
	private static void CreateMouth(SpeechMonitor.Instance smi)
	{
		smi.mouth = global::Util.KInstantiate(Assets.GetPrefab(MouthAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.mouth.gameObject.SetActive(true);
		smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
		smi.SetMouthId();
	}

	// Token: 0x06007447 RID: 29767 RVA: 0x000EC4FF File Offset: 0x000EA6FF
	private static void DestroyMouth(SpeechMonitor.Instance smi)
	{
		if (smi.mouth != null)
		{
			global::Util.KDestroyGameObject(smi.mouth);
			smi.mouth = null;
		}
	}

	// Token: 0x06007448 RID: 29768 RVA: 0x00302DC0 File Offset: 0x00300FC0
	private static string GetRandomSpeechAnim(SpeechMonitor.Instance smi)
	{
		return smi.speechPrefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString() + smi.mouthId;
	}

	// Token: 0x06007449 RID: 29769 RVA: 0x00302DF8 File Offset: 0x00300FF8
	public static bool IsAllowedToPlaySpeech(GameObject go)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component.HasTag(GameTags.Dead) || component.HasTag(GameTags.Incapacitated))
		{
			return false;
		}
		KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
		KAnim.Anim currentAnim = component2.GetCurrentAnim();
		return currentAnim == null || (GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim) && SpeechMonitor.CanOverrideHead(component2));
	}

	// Token: 0x0600744A RID: 29770 RVA: 0x00302E50 File Offset: 0x00301050
	private static bool CanOverrideHead(KBatchedAnimController kbac)
	{
		bool result = true;
		KAnim.Anim currentAnim = kbac.GetCurrentAnim();
		if (currentAnim == null)
		{
			result = false;
		}
		else if (currentAnim.animFile.name != SpeechMonitor.GENERIC_CONVO_ANIM_NAME)
		{
			int currentFrameIndex = kbac.GetCurrentFrameIndex();
			KAnim.Anim.Frame frame;
			if (currentFrameIndex <= 0)
			{
				result = false;
			}
			else if (KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag).TryGetFrame(currentFrameIndex, out frame) && frame.hasHead)
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x0600744B RID: 29771 RVA: 0x00302EC4 File Offset: 0x003010C4
	public static void BeginTalking(SpeechMonitor.Instance smi)
	{
		smi.ev.clearHandle();
		if (smi.voiceEvent != null)
		{
			smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.GetComponent<KBatchedAnimController>(), 0f, false, false);
		}
		if (smi.ev.isValid())
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}
		SpeechMonitor.UpdateTalking(smi, 0f);
	}

	// Token: 0x0600744C RID: 29772 RVA: 0x000EC521 File Offset: 0x000EA721
	public static void EndTalking(SpeechMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
	}

	// Token: 0x0600744D RID: 29773 RVA: 0x00302FE8 File Offset: 0x003011E8
	public static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
	{
		KAnim.Anim.FrameElement result = default(KAnim.Anim.FrameElement);
		result.symbol = HashedString.Invalid;
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		KAnimBatch batch = controller.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return result;
		}
		KAnim.Anim.Frame frame;
		if (!controller.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return result;
		}
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					result = frameElement;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600744E RID: 29774 RVA: 0x003030AC File Offset: 0x003012AC
	public static void UpdateTalking(SpeechMonitor.Instance smi, float dt)
	{
		if (smi.ev.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			smi.ev.getPlaybackState(out playback_STATE);
			if (playback_STATE == PLAYBACK_STATE.STOPPING || playback_STATE == PLAYBACK_STATE.STOPPED)
			{
				smi.GoTo(smi.sm.satisfied);
				smi.ev.clearHandle();
				return;
			}
		}
		KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(smi.mouth);
		if (firstFrameElement.symbol == HashedString.Invalid)
		{
			return;
		}
		smi.Get<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
	}

	// Token: 0x040056FF RID: 22271
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;

	// Token: 0x04005700 RID: 22272
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State talking;

	// Token: 0x04005701 RID: 22273
	public static string PREFIX_SAD = "sad";

	// Token: 0x04005702 RID: 22274
	public static string PREFIX_HAPPY = "happy";

	// Token: 0x04005703 RID: 22275
	public static string PREFIX_SINGER = "sing";

	// Token: 0x04005704 RID: 22276
	public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;

	// Token: 0x04005705 RID: 22277
	private static HashedString HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x04005706 RID: 22278
	private static HashedString GENERIC_CONVO_ANIM_NAME = new HashedString("anim_generic_convo_kanim");

	// Token: 0x020015EE RID: 5614
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015EF RID: 5615
	public class Tuning : TuningData<SpeechMonitor.Tuning>
	{
		// Token: 0x04005707 RID: 22279
		public float randomSpeechIntervalMin;

		// Token: 0x04005708 RID: 22280
		public float randomSpeechIntervalMax;

		// Token: 0x04005709 RID: 22281
		public int speechCount;
	}

	// Token: 0x020015F0 RID: 5616
	public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
	{
		// Token: 0x06007453 RID: 29779 RVA: 0x000EC583 File Offset: 0x000EA783
		public Instance(IStateMachineTarget master, SpeechMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007454 RID: 29780 RVA: 0x000EC598 File Offset: 0x000EA798
		public bool IsPlayingSpeech()
		{
			return base.IsInsideState(base.sm.talking);
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x000EC5AB File Offset: 0x000EA7AB
		public void PlaySpeech(string speech_prefix, string voice_event)
		{
			this.speechPrefix = speech_prefix;
			this.voiceEvent = voice_event;
			this.GoTo(base.sm.talking);
		}

		// Token: 0x06007456 RID: 29782 RVA: 0x0030314C File Offset: 0x0030134C
		public void DrawMouth()
		{
			KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(base.smi.mouth);
			if (firstFrameElement.symbol == HashedString.Invalid)
			{
				return;
			}
			KAnim.Build.Symbol symbol = base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			base.GetComponent<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
			KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID).GetSymbol(SpeechMonitor.HASH_SNAPTO_MOUTH);
			KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(symbol.build.batchTag).symbolFrameInstances[symbol.firstFrameIdx + firstFrameElement.frame];
			symbolFrameInstance.buildImageIdx = base.GetComponent<SymbolOverrideController>().GetAtlasIdx(symbol.build.GetTexture(0));
			component.SetSymbolOverride(symbol2.firstFrameIdx, ref symbolFrameInstance);
		}

		// Token: 0x06007457 RID: 29783 RVA: 0x00303260 File Offset: 0x00301460
		public void SetMouthId()
		{
			if (base.smi.Get<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Mouth).Id.Contains("006"))
			{
				base.smi.mouthId = "_006";
			}
		}

		// Token: 0x0400570A RID: 22282
		public KBatchedAnimController mouth;

		// Token: 0x0400570B RID: 22283
		public string speechPrefix = "happy";

		// Token: 0x0400570C RID: 22284
		public string voiceEvent;

		// Token: 0x0400570D RID: 22285
		public EventInstance ev;

		// Token: 0x0400570E RID: 22286
		public string mouthId;
	}
}
