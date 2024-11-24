using System;
using Klei.AI;

// Token: 0x02001590 RID: 5520
public class HygieneMonitor : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance>
{
	// Token: 0x060072B1 RID: 29361 RVA: 0x002FE738 File Offset: 0x002FC938
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.needsshower;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.clean.EventTransition(GameHashes.EffectRemoved, this.needsshower, (HygieneMonitor.Instance smi) => smi.NeedsShower());
		this.needsshower.EventTransition(GameHashes.EffectAdded, this.clean, (HygieneMonitor.Instance smi) => !smi.NeedsShower()).ToggleUrge(Db.Get().Urges.Shower).Enter(delegate(HygieneMonitor.Instance smi)
		{
			smi.SetDirtiness(1f);
		});
	}

	// Token: 0x040055C3 RID: 21955
	public StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.FloatParameter dirtiness;

	// Token: 0x040055C4 RID: 21956
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State clean;

	// Token: 0x040055C5 RID: 21957
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State needsshower;

	// Token: 0x02001591 RID: 5521
	public new class Instance : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060072B3 RID: 29363 RVA: 0x000EB1AD File Offset: 0x000E93AD
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x000EB1C2 File Offset: 0x000E93C2
		public float GetDirtiness()
		{
			return base.sm.dirtiness.Get(this);
		}

		// Token: 0x060072B5 RID: 29365 RVA: 0x000EB1D5 File Offset: 0x000E93D5
		public void SetDirtiness(float dirtiness)
		{
			base.sm.dirtiness.Set(dirtiness, this, false);
		}

		// Token: 0x060072B6 RID: 29366 RVA: 0x000EB1EB File Offset: 0x000E93EB
		public bool NeedsShower()
		{
			return !this.effects.HasEffect(Shower.SHOWER_EFFECT);
		}

		// Token: 0x040055C6 RID: 21958
		private Effects effects;
	}
}
