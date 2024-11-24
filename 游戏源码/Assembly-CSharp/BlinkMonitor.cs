using System;
using UnityEngine;

// Token: 0x0200152A RID: 5418
public class BlinkMonitor : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>
{
	// Token: 0x06007108 RID: 28936 RVA: 0x002F97F0 File Offset: 0x002F79F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.CreateEyes)).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.DestroyEyes));
		this.satisfied.ScheduleGoTo(new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime), this.blinking);
		this.blinking.EnterTransition(this.satisfied, GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Not(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Transition.ConditionCallback(BlinkMonitor.CanBlink))).Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.BeginBlinking)).Update(new Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.eyes).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.EndBlinking));
	}

	// Token: 0x06007109 RID: 28937 RVA: 0x000E9F70 File Offset: 0x000E8170
	private static bool CanBlink(BlinkMonitor.Instance smi)
	{
		return SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject) && smi.Get<Navigator>().CurrentNavType != NavType.Ladder;
	}

	// Token: 0x0600710A RID: 28938 RVA: 0x000E9F92 File Offset: 0x000E8192
	private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
	{
		return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
	}

	// Token: 0x0600710B RID: 28939 RVA: 0x002F98BC File Offset: 0x002F7ABC
	private static void CreateEyes(BlinkMonitor.Instance smi)
	{
		smi.eyes = Util.KInstantiate(Assets.GetPrefab(EyeAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.eyes.gameObject.SetActive(true);
		smi.sm.eyes.Set(smi.eyes.gameObject, smi, false);
	}

	// Token: 0x0600710C RID: 28940 RVA: 0x000E9FAD File Offset: 0x000E81AD
	private static void DestroyEyes(BlinkMonitor.Instance smi)
	{
		if (smi.eyes != null)
		{
			Util.KDestroyGameObject(smi.eyes);
			smi.eyes = null;
		}
	}

	// Token: 0x0600710D RID: 28941 RVA: 0x000E9FCF File Offset: 0x000E81CF
	public static void BeginBlinking(BlinkMonitor.Instance smi)
	{
		smi.eyes.Play(smi.eye_anim, KAnim.PlayMode.Once, 1f, 0f);
		BlinkMonitor.UpdateBlinking(smi, 0f);
	}

	// Token: 0x0600710E RID: 28942 RVA: 0x000E9FFD File Offset: 0x000E81FD
	public static void EndBlinking(BlinkMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
	}

	// Token: 0x0600710F RID: 28943 RVA: 0x002F991C File Offset: 0x002F7B1C
	public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
	{
		int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
		KAnimBatch batch = smi.eyes.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return;
		}
		KAnim.Anim.Frame frame;
		if (!smi.eyes.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return;
		}
		HashedString hash = HashedString.Invalid;
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					hash = frameElement.symbol;
					break;
				}
			}
		}
		smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol(hash), 3);
	}

	// Token: 0x0400546D RID: 21613
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State satisfied;

	// Token: 0x0400546E RID: 21614
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State blinking;

	// Token: 0x0400546F RID: 21615
	public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.TargetParameter eyes;

	// Token: 0x04005470 RID: 21616
	private static HashedString HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x0200152B RID: 5419
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200152C RID: 5420
	public class Tuning : TuningData<BlinkMonitor.Tuning>
	{
		// Token: 0x04005471 RID: 21617
		public float randomBlinkIntervalMin;

		// Token: 0x04005472 RID: 21618
		public float randomBlinkIntervalMax;
	}

	// Token: 0x0200152D RID: 5421
	public new class Instance : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.GameInstance
	{
		// Token: 0x06007114 RID: 28948 RVA: 0x000EA032 File Offset: 0x000E8232
		public Instance(IStateMachineTarget master, BlinkMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x000EA03C File Offset: 0x000E823C
		public bool IsBlinking()
		{
			return base.IsInsideState(base.sm.blinking);
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x000EA04F File Offset: 0x000E824F
		public void Blink()
		{
			this.GoTo(base.sm.blinking);
		}

		// Token: 0x04005473 RID: 21619
		public KBatchedAnimController eyes;

		// Token: 0x04005474 RID: 21620
		public string eye_anim;
	}
}
