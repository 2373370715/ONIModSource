﻿using System;
using FMOD.Studio;
using UnityEngine;

public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
		this.satisfied.DoNothing();
		this.talking.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.BeginTalking)).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.mouth).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.EndTalking));
	}

		private static void CreateMouth(SpeechMonitor.Instance smi)
	{
		smi.mouth = global::Util.KInstantiate(Assets.GetPrefab(MouthAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.mouth.gameObject.SetActive(true);
		smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
		smi.SetMouthId();
	}

		private static void DestroyMouth(SpeechMonitor.Instance smi)
	{
		if (smi.mouth != null)
		{
			global::Util.KDestroyGameObject(smi.mouth);
			smi.mouth = null;
		}
	}

		private static string GetRandomSpeechAnim(SpeechMonitor.Instance smi)
	{
		return smi.speechPrefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString() + smi.mouthId;
	}

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

		public static void EndTalking(SpeechMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
	}

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

		public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;

		public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State talking;

		public static string PREFIX_SAD = "sad";

		public static string PREFIX_HAPPY = "happy";

		public static string PREFIX_SINGER = "sing";

		public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;

		private static HashedString HASH_SNAPTO_MOUTH = "snapto_mouth";

		private static HashedString GENERIC_CONVO_ANIM_NAME = new HashedString("anim_generic_convo_kanim");

		public class Def : StateMachine.BaseDef
	{
	}

		public class Tuning : TuningData<SpeechMonitor.Tuning>
	{
				public float randomSpeechIntervalMin;

				public float randomSpeechIntervalMax;

				public int speechCount;
	}

		public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
	{
				public Instance(IStateMachineTarget master, SpeechMonitor.Def def) : base(master, def)
		{
		}

				public bool IsPlayingSpeech()
		{
			return base.IsInsideState(base.sm.talking);
		}

				public void PlaySpeech(string speech_prefix, string voice_event)
		{
			this.speechPrefix = speech_prefix;
			this.voiceEvent = voice_event;
			this.GoTo(base.sm.talking);
		}

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

				public void SetMouthId()
		{
			if (base.smi.Get<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Mouth).Id.Contains("006"))
			{
				base.smi.mouthId = "_006";
			}
		}

				public KBatchedAnimController mouth;

				public string speechPrefix = "happy";

				public string voiceEvent;

				public EventInstance ev;

				public string mouthId;
	}
}
