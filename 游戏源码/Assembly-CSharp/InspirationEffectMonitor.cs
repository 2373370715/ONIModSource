using System;
using UnityEngine;

// Token: 0x02001598 RID: 5528
public class InspirationEffectMonitor : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>
{
	// Token: 0x060072CE RID: 29390 RVA: 0x002FEB10 File Offset: 0x002FCD10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.CatchyTune, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.OnCatchyTune)).ParamTransition<bool>(this.shouldCatchyTune, this.catchyTune, (InspirationEffectMonitor.Instance smi, bool shouldCatchyTune) => shouldCatchyTune);
		this.catchyTune.Exit(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.shouldCatchyTune.Set(false, smi, false);
		}).ToggleEffect("HeardJoySinger").ToggleThought(Db.Get().Thoughts.CatchyTune, null).EventHandler(GameHashes.StartWork, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.TryThinkCatchyTune)).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HeardJoySinger, null).Enter(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.SingCatchyTune(smi);
		}).Update(delegate(InspirationEffectMonitor.Instance smi, float dt)
		{
			this.TryThinkCatchyTune(smi, null);
			this.inspirationTimeRemaining.Delta(-dt, smi);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<float>(this.inspirationTimeRemaining, this.idle, (InspirationEffectMonitor.Instance smi, float p) => p <= 0f);
	}

	// Token: 0x060072CF RID: 29391 RVA: 0x000EB339 File Offset: 0x000E9539
	private void OnCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		this.inspirationTimeRemaining.Set(600f, smi, false);
		this.shouldCatchyTune.Set(true, smi, false);
	}

	// Token: 0x060072D0 RID: 29392 RVA: 0x000EB35D File Offset: 0x000E955D
	private void TryThinkCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		if (UnityEngine.Random.Range(1, 101) > 66)
		{
			this.SingCatchyTune(smi);
		}
	}

	// Token: 0x060072D1 RID: 29393 RVA: 0x002FEC30 File Offset: 0x002FCE30
	private void SingCatchyTune(InspirationEffectMonitor.Instance smi)
	{
		smi.master.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.CatchyTune);
		if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
		{
			smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
		}
	}

	// Token: 0x040055DB RID: 21979
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.BoolParameter shouldCatchyTune;

	// Token: 0x040055DC RID: 21980
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.FloatParameter inspirationTimeRemaining;

	// Token: 0x040055DD RID: 21981
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State idle;

	// Token: 0x040055DE RID: 21982
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State catchyTune;

	// Token: 0x02001599 RID: 5529
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200159A RID: 5530
	public new class Instance : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameInstance
	{
		// Token: 0x060072D7 RID: 29399 RVA: 0x000EB3AD File Offset: 0x000E95AD
		public Instance(IStateMachineTarget master, InspirationEffectMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060072D8 RID: 29400 RVA: 0x000EB3B7 File Offset: 0x000E95B7
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x040055DF RID: 21983
		public SpeechMonitor.Instance speechMonitor;
	}
}
