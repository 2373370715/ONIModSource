using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001585 RID: 5509
public class GunkMonitor : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>
{
	// Token: 0x0600727B RID: 29307 RVA: 0x002FDCF8 File Offset: 0x002FBEF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EnterTransition(this.mildUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold)).OnSignal(this.GunkChangedSignal, this.mildUrge, new Func<GunkMonitor.Instance, bool>(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold));
		this.mildUrge.EnterTransition(this.criticalUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).ToggleThought(Db.Get().Thoughts.ExpellGunkDesire, null).OnSignal(this.GunkChangedSignal, this.criticalUrge, new Func<GunkMonitor.Instance, bool>(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).OnSignal(this.GunkMaxedOutSignal, this.criticalUrge).OnSignal(this.GunkEmptiedSignal, this.idle).DefaultState(this.mildUrge.prevented);
		this.mildUrge.prevented.EventTransition(GameHashes.ScheduleBlocksChanged, this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling)).EventTransition(GameHashes.ScheduleChanged, this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling));
		this.mildUrge.allowed.EventTransition(GameHashes.ScheduleBlocksChanged, this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).EventTransition(GameHashes.ScheduleChanged, this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).ToggleUrge(Db.Get().Urges.Pee).ToggleUrge(Db.Get().Urges.GunkPee);
		this.criticalUrge.EnterTransition(this.cantHold, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.CanNotHoldGunkAnymore)).OnSignal(this.GunkMaxedOutSignal, this.cantHold).OnSignal(this.GunkEmptiedSignal, this.idle).ToggleUrge(Db.Get().Urges.GunkPee).ToggleUrge(Db.Get().Urges.Pee).ToggleEffect("GunkSick").ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
		this.cantHold.ToggleUrge(Db.Get().Urges.GunkPee).ToggleThought(Db.Get().Thoughts.ExpellingGunk, null).ToggleChore((GunkMonitor.Instance smi) => new BionicGunkSpillChore(smi.master), this.emptyRemaining);
		this.emptyRemaining.Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ExpellAllGunk)).Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ApplyGunkHungoverEffect)).GoTo(this.idle);
	}

	// Token: 0x0600727C RID: 29308 RVA: 0x000EAF27 File Offset: 0x000E9127
	public static bool IsGunkLevelsOverCriticalUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.DesperetlySeekForGunkToiletTreshold;
	}

	// Token: 0x0600727D RID: 29309 RVA: 0x000EAF3F File Offset: 0x000E913F
	public static bool IsGunkLevelsOverMildUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.SeekForGunkToiletTreshold_InSchedule;
	}

	// Token: 0x0600727E RID: 29310 RVA: 0x000EAF57 File Offset: 0x000E9157
	public static bool ScheduleAllowsExpelling(GunkMonitor.Instance smi)
	{
		return smi.DoesCurrentScheduleAllowsGunkToilet;
	}

	// Token: 0x0600727F RID: 29311 RVA: 0x000EAF5F File Offset: 0x000E915F
	public static bool DoesNotWantToExpellGunk(GunkMonitor.Instance smi)
	{
		return !GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
	}

	// Token: 0x06007280 RID: 29312 RVA: 0x000EAF6A File Offset: 0x000E916A
	public static bool CanNotHoldGunkAnymore(GunkMonitor.Instance smi)
	{
		return smi.IsGunkBuildupAtMax;
	}

	// Token: 0x06007281 RID: 29313 RVA: 0x000EAF72 File Offset: 0x000E9172
	public static void ExpellAllGunk(GunkMonitor.Instance smi)
	{
		smi.ExpellAllGunk(null);
	}

	// Token: 0x06007282 RID: 29314 RVA: 0x000EAF7B File Offset: 0x000E917B
	public static void ApplyGunkHungoverEffect(GunkMonitor.Instance smi)
	{
		smi.GetComponent<Effects>().Add("GunkHungover", true);
	}

	// Token: 0x0400559D RID: 21917
	public static readonly float GUNK_CAPACITY = 50f;

	// Token: 0x0400559E RID: 21918
	public const string GUNK_FULL_EFFECT_NAME = "GunkSick";

	// Token: 0x0400559F RID: 21919
	public const string GUNK_HUNGOVER_EFFECT_NAME = "GunkHungover";

	// Token: 0x040055A0 RID: 21920
	public static SimHashes GunkElement = SimHashes.LiquidGunk;

	// Token: 0x040055A1 RID: 21921
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State idle;

	// Token: 0x040055A2 RID: 21922
	public GunkMonitor.MildUrgeStates mildUrge;

	// Token: 0x040055A3 RID: 21923
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State criticalUrge;

	// Token: 0x040055A4 RID: 21924
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State cantHold;

	// Token: 0x040055A5 RID: 21925
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State emptyRemaining;

	// Token: 0x040055A6 RID: 21926
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal GunkChangedSignal;

	// Token: 0x040055A7 RID: 21927
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal GunkMaxedOutSignal;

	// Token: 0x040055A8 RID: 21928
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal GunkEmptiedSignal;

	// Token: 0x02001586 RID: 5510
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040055A9 RID: 21929
		public float SeekForGunkToiletTreshold_InSchedule = 0.6f;

		// Token: 0x040055AA RID: 21930
		public float DesperetlySeekForGunkToiletTreshold = 0.9f;
	}

	// Token: 0x02001587 RID: 5511
	public class MildUrgeStates : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State
	{
		// Token: 0x040055AB RID: 21931
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State allowed;

		// Token: 0x040055AC RID: 21932
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State prevented;
	}

	// Token: 0x02001588 RID: 5512
	public new class Instance : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.GameInstance
	{
		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06007287 RID: 29319 RVA: 0x000EAFD3 File Offset: 0x000E91D3
		public bool HasGunk
		{
			get
			{
				return this.CurrentGunkMass > 0f;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06007288 RID: 29320 RVA: 0x000EAFE2 File Offset: 0x000E91E2
		public bool IsGunkBuildupAtMax
		{
			get
			{
				return this.CurrentGunkPercentage >= 1f;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06007289 RID: 29321 RVA: 0x000EAFF4 File Offset: 0x000E91F4
		public float CurrentGunkMass
		{
			get
			{
				if (this.gunkAmount != null)
				{
					return this.gunkAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600728A RID: 29322 RVA: 0x000EB00F File Offset: 0x000E920F
		public float CurrentGunkPercentage
		{
			get
			{
				return this.CurrentGunkMass / this.gunkAmount.GetMax();
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600728B RID: 29323 RVA: 0x000EB023 File Offset: 0x000E9223
		public bool DoesCurrentScheduleAllowsGunkToilet
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat) || this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
			}
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x002FDFD4 File Offset: 0x002FC1D4
		public Instance(IStateMachineTarget master, GunkMonitor.Def def) : base(master, def)
		{
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.gunkAmount = Db.Get().Amounts.BionicGunk.Lookup(base.gameObject);
			this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(base.gameObject);
			AmountInstance amountInstance = this.oilAmount;
			amountInstance.OnValueChanged = (Action<float>)Delegate.Combine(amountInstance.OnValueChanged, new Action<float>(this.OnOilValueChanged));
			AmountInstance amountInstance2 = this.gunkAmount;
			amountInstance2.OnValueChanged = (Action<float>)Delegate.Combine(amountInstance2.OnValueChanged, new Action<float>(this.OnGunkValueChanged));
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x000EB05D File Offset: 0x000E925D
		private void OnMaxGunkBuildupReached()
		{
			base.sm.GunkMaxedOutSignal.Trigger(base.smi);
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x000EB075 File Offset: 0x000E9275
		private void OnGunkEmptied()
		{
			base.sm.GunkEmptiedSignal.Trigger(base.smi);
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x000EB08D File Offset: 0x000E928D
		private void OnGunkValueChanged(float delta)
		{
			base.sm.GunkChangedSignal.Trigger(base.smi);
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x002FE0A4 File Offset: 0x002FC2A4
		private void OnOilValueChanged(float delta)
		{
			float num = (delta < 0f) ? Mathf.Abs(delta) : 0f;
			float gunkMassValue = Mathf.Clamp(this.CurrentGunkMass + num, 0f, this.gunkAmount.GetMax());
			this.SetGunkMassValue(gunkMassValue);
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x002FE0EC File Offset: 0x002FC2EC
		public void SetGunkMassValue(float value)
		{
			bool flag = this.CurrentGunkMass != value;
			this.gunkAmount.SetValue(value);
			if (flag)
			{
				if (this.CurrentGunkMass <= 0f)
				{
					this.OnGunkEmptied();
					return;
				}
				if (this.IsGunkBuildupAtMax)
				{
					this.OnMaxGunkBuildupReached();
					return;
				}
				base.sm.GunkChangedSignal.Trigger(this);
			}
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x002FE148 File Offset: 0x002FC348
		public void ExpellGunk(float mass, Storage targetStorage = null)
		{
			if (this.HasGunk)
			{
				float currentGunkMass = this.CurrentGunkMass;
				float num = Mathf.Min(mass, this.CurrentGunkMass);
				num = Mathf.Max(num, Mathf.Epsilon);
				int gameCell = Grid.PosToCell(base.transform.position);
				byte index = Db.Get().Diseases.GetIndex(DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE);
				float num2 = num / GunkMonitor.GUNK_CAPACITY;
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
				}
				else if (targetStorage != null)
				{
					targetStorage.AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
				}
				else
				{
					SimMessages.AddRemoveSubstance(gameCell, GunkMonitor.GunkElement, CellEventLogger.Instance.Vomit, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), true, -1);
				}
				if (Sim.IsRadiationEnabled())
				{
					MinionIdentity component = base.transform.GetComponent<MinionIdentity>();
					AmountInstance amountInstance = Db.Get().Amounts.RadiationBalance.Lookup(component);
					RadiationMonitor.Instance smi = component.GetSMI<RadiationMonitor.Instance>();
					float num3 = DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND / DUPLICANTSTATS.BIONICS.BaseStats.BLADDER_INCREASE_PER_SECOND;
					float num4 = Math.Min(amountInstance.value, 100f * num3 * smi.difficultySettingMod * num2);
					if (num4 >= 1f)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num4).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, component.transform, Vector3.up * 2f, 1.5f, false, false);
					}
					amountInstance.ApplyDelta(-num4);
				}
				this.SetGunkMassValue(Mathf.Clamp(this.CurrentGunkMass - num, 0f, this.gunkAmount.GetMax()));
			}
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x000EB0A5 File Offset: 0x000E92A5
		public void ExpellAllGunk(Storage targetStorage = null)
		{
			this.ExpellGunk(this.CurrentGunkMass, targetStorage);
		}

		// Token: 0x040055AD RID: 21933
		private AmountInstance oilAmount;

		// Token: 0x040055AE RID: 21934
		private AmountInstance gunkAmount;

		// Token: 0x040055AF RID: 21935
		private AmountInstance bodyTemperature;

		// Token: 0x040055B0 RID: 21936
		private Schedulable schedulable;
	}
}
