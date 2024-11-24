using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020015E4 RID: 5604
public class SlipperyMonitor : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>
{
	// Token: 0x06007423 RID: 29731 RVA: 0x003026B4 File Offset: 0x003008B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.EventTransition(GameHashes.NavigationCellChanged, this.unsafeCell, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell));
		this.unsafeCell.EventTransition(GameHashes.NavigationCellChanged, this.safe, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell))).DefaultState(this.unsafeCell.atRisk);
		this.unsafeCell.atRisk.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).EventTransition(GameHashes.EffectAdded, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).DefaultState(this.unsafeCell.atRisk.idle);
		this.unsafeCell.atRisk.idle.EventHandlerTransition(GameHashes.NavigationCellChanged, this.unsafeCell.atRisk.slip, new Func<SlipperyMonitor.Instance, object, bool>(SlipperyMonitor.RollDTwenty));
		this.unsafeCell.atRisk.slip.ToggleReactable(new Func<SlipperyMonitor.Instance, Reactable>(this.GetReactable)).ScheduleGoTo(8f, this.unsafeCell.atRisk.idle);
		this.unsafeCell.immune.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces))).EventTransition(GameHashes.EffectRemoved, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)));
	}

	// Token: 0x06007424 RID: 29732 RVA: 0x000EC364 File Offset: 0x000EA564
	public bool IsImmuneToSlipperySurfaces(SlipperyMonitor.Instance smi)
	{
		return smi.IsImmune;
	}

	// Token: 0x06007425 RID: 29733 RVA: 0x000EC36C File Offset: 0x000EA56C
	public Reactable GetReactable(SlipperyMonitor.Instance smi)
	{
		return smi.CreateReactable();
	}

	// Token: 0x06007426 RID: 29734 RVA: 0x0030285C File Offset: 0x00300A5C
	private static bool IsStandingOnASlipperyCell(SlipperyMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		int num2 = Grid.OffsetCell(num, 0, -1);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsSlippery) || (Grid.IsValidCell(num2) && Grid.Element[num2].IsSlippery);
	}

	// Token: 0x06007427 RID: 29735 RVA: 0x000EC374 File Offset: 0x000EA574
	private static bool RollDTwenty(SlipperyMonitor.Instance smi, object o)
	{
		return UnityEngine.Random.value <= 0.05f;
	}

	// Token: 0x040056E2 RID: 22242
	public const string EFFECT_NAME = "Slipped";

	// Token: 0x040056E3 RID: 22243
	public const float SLIP_FAIL_TIMEOUT = 8f;

	// Token: 0x040056E4 RID: 22244
	public const float PROBABILITY_OF_SLIP = 0.05f;

	// Token: 0x040056E5 RID: 22245
	public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State safe;

	// Token: 0x040056E6 RID: 22246
	public SlipperyMonitor.UnsafeCellState unsafeCell;

	// Token: 0x020015E5 RID: 5605
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015E6 RID: 5606
	public class UnsafeCellState : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x040056E7 RID: 22247
		public SlipperyMonitor.RiskStates atRisk;

		// Token: 0x040056E8 RID: 22248
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State immune;
	}

	// Token: 0x020015E7 RID: 5607
	public class RiskStates : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x040056E9 RID: 22249
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State idle;

		// Token: 0x040056EA RID: 22250
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State slip;
	}

	// Token: 0x020015E8 RID: 5608
	public new class Instance : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.GameInstance
	{
		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x0600742C RID: 29740 RVA: 0x000EC395 File Offset: 0x000EA595
		public bool IsImmune
		{
			get
			{
				return this.effects.HasEffect("Slipped") || this.effects.HasImmunityTo(this.effect);
			}
		}

		// Token: 0x0600742D RID: 29741 RVA: 0x000EC3BC File Offset: 0x000EA5BC
		public Instance(IStateMachineTarget master, SlipperyMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
			this.effect = Db.Get().effects.Get("Slipped");
		}

		// Token: 0x0600742E RID: 29742 RVA: 0x000EC3EC File Offset: 0x000EA5EC
		public SlipperyMonitor.SlipReactable CreateReactable()
		{
			return new SlipperyMonitor.SlipReactable(this);
		}

		// Token: 0x040056EB RID: 22251
		private Effect effect;

		// Token: 0x040056EC RID: 22252
		public Effects effects;
	}

	// Token: 0x020015E9 RID: 5609
	public class SlipReactable : Reactable
	{
		// Token: 0x0600742F RID: 29743 RVA: 0x003028A8 File Offset: 0x00300AA8
		public SlipReactable(SlipperyMonitor.Instance _smi) : base(_smi.gameObject, "Slip", Db.Get().ChoreTypes.Slip, 1, 1, false, 0f, 0f, 8f, 0f, ObjectLayer.NumLayers)
		{
			this.smi = _smi;
		}

		// Token: 0x06007430 RID: 29744 RVA: 0x003028FC File Offset: 0x00300AFC
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (new_reactor == null)
			{
				return false;
			}
			if (this.gameObject != new_reactor)
			{
				return false;
			}
			if (this.smi == null)
			{
				return false;
			}
			Navigator component = new_reactor.GetComponent<Navigator>();
			return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
		}

		// Token: 0x06007431 RID: 29745 RVA: 0x00302970 File Offset: 0x00300B70
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_slip_kanim"), 1f);
			component.Play("slip_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.reactor.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.Slippering, null);
		}

		// Token: 0x06007432 RID: 29746 RVA: 0x000EC3F4 File Offset: 0x000EA5F4
		public override void Update(float dt)
		{
			if (Time.time - this.startTime > 4.3f)
			{
				base.Cleanup();
				this.ApplyEffect();
			}
		}

		// Token: 0x06007433 RID: 29747 RVA: 0x000EC415 File Offset: 0x000EA615
		public void ApplyEffect()
		{
			this.smi.effects.Add("Slipped", true);
		}

		// Token: 0x06007434 RID: 29748 RVA: 0x00302A20 File Offset: 0x00300C20
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					this.reactor.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.Slippering, false);
					component.RemoveAnimOverrides(Assets.GetAnim("anim_slip_kanim"));
				}
			}
		}

		// Token: 0x06007435 RID: 29749 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040056ED RID: 22253
		private SlipperyMonitor.Instance smi;

		// Token: 0x040056EE RID: 22254
		private float startTime;

		// Token: 0x040056EF RID: 22255
		private const string ANIM_FILE_NAME = "anim_slip_kanim";

		// Token: 0x040056F0 RID: 22256
		private const float DURATION = 4.3f;
	}
}
