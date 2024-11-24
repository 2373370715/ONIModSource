using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02001504 RID: 5380
public class BionicOilMonitor : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>
{
	// Token: 0x06007034 RID: 28724 RVA: 0x002F777C File Offset: 0x002F597C
	private static Effect CreateFreshOilEffectVariation(string id, float stressBonus, float moralBonus)
	{
		Effect effect = new Effect("FreshOil_" + id, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, DUPLICANTS.MODIFIERS.FRESHOIL.TOOLTIP, 4800f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, moralBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, stressBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		return effect;
	}

	// Token: 0x06007035 RID: 28725 RVA: 0x002F7828 File Offset: 0x002F5A28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.offline;
		this.root.Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.offline.EventTransition(GameHashes.BionicOnline, this.online, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline)).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.online.EventTransition(GameHashes.BionicOffline, this.offline, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline))).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.AddBaseOilDeltaModifier)).DefaultState(this.online.idle);
		this.online.idle.EnterTransition(this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.WantsOilChange)).OnSignal(this.OilValueChanged, this.online.seeking, new Func<BionicOilMonitor.Instance, bool>(BionicOilMonitor.WantsOilChange));
		this.online.seeking.OnSignal(this.OilFilledSignal, this.online.idle).OnSignal(this.OilValueChanged, this.online.idle, new Func<BionicOilMonitor.Instance, bool>(BionicOilMonitor.HasDecentAmountOfOil)).DefaultState(this.online.seeking.hasOil).ToggleUrge(Db.Get().Urges.OilRefill);
		this.online.seeking.hasOil.EnterTransition(this.online.seeking.noOil, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.HasAnyAmountOfOil))).OnSignal(this.OilRanOutSignal, this.online.seeking.noOil).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWantsOilChange, null);
		this.online.seeking.noOil.ToggleEffect("NoLubrication");
	}

	// Token: 0x06007036 RID: 28726 RVA: 0x000E965B File Offset: 0x000E785B
	public static bool IsBionicOnline(BionicOilMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06007037 RID: 28727 RVA: 0x000E9663 File Offset: 0x000E7863
	public static bool HasAnyAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilMass > 0f;
	}

	// Token: 0x06007038 RID: 28728 RVA: 0x000E9672 File Offset: 0x000E7872
	public static bool HasDecentAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilPercentage > 0.2f;
	}

	// Token: 0x06007039 RID: 28729 RVA: 0x000E9681 File Offset: 0x000E7881
	public static bool WantsOilChange(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilPercentage <= 0.2f;
	}

	// Token: 0x0600703A RID: 28730 RVA: 0x000E9693 File Offset: 0x000E7893
	public static void AddBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(true);
	}

	// Token: 0x0600703B RID: 28731 RVA: 0x000E969C File Offset: 0x000E789C
	public static void RemoveBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(false);
	}

	// Token: 0x0600703D RID: 28733 RVA: 0x002F7A14 File Offset: 0x002F5C14
	// Note: this type is marked as 'beforefieldinit'.
	static BionicOilMonitor()
	{
		Dictionary<SimHashes, Effect> dictionary = new Dictionary<SimHashes, Effect>();
		dictionary[SimHashes.CrudeOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.CrudeOil.ToString(), -0.016666668f, 3f);
		dictionary[SimHashes.PhytoOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.PhytoOil.ToString(), -0.008333334f, 2f);
		BionicOilMonitor.LUBRICANT_TYPE_EFFECT = dictionary;
	}

	// Token: 0x040053E1 RID: 21473
	public static Dictionary<SimHashes, Effect> LUBRICANT_TYPE_EFFECT;

	// Token: 0x040053E2 RID: 21474
	public const float OIL_CAPACITY = 200f;

	// Token: 0x040053E3 RID: 21475
	public const float OIL_TANK_DURATION = 6000f;

	// Token: 0x040053E4 RID: 21476
	public const float OIL_REFILL_TRESHOLD = 0.2f;

	// Token: 0x040053E5 RID: 21477
	public const string NO_OIL_EFFECT_NAME = "NoLubrication";

	// Token: 0x040053E6 RID: 21478
	public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State offline;

	// Token: 0x040053E7 RID: 21479
	public BionicOilMonitor.OnlineStates online;

	// Token: 0x040053E8 RID: 21480
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilFilledSignal;

	// Token: 0x040053E9 RID: 21481
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilRanOutSignal;

	// Token: 0x040053EA RID: 21482
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilValueChanged;

	// Token: 0x02001505 RID: 5381
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001506 RID: 5382
	public class WantsOilChangeState : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x040053EB RID: 21483
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State hasOil;

		// Token: 0x040053EC RID: 21484
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State noOil;
	}

	// Token: 0x02001507 RID: 5383
	public class OnlineStates : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x040053ED RID: 21485
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State idle;

		// Token: 0x040053EE RID: 21486
		public BionicOilMonitor.WantsOilChangeState seeking;
	}

	// Token: 0x02001508 RID: 5384
	public new class Instance : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.GameInstance
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06007041 RID: 28737 RVA: 0x000E96B5 File Offset: 0x000E78B5
		public bool IsOnline
		{
			get
			{
				return this.batterySMI != null && this.batterySMI.IsOnline;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06007042 RID: 28738 RVA: 0x000E96CC File Offset: 0x000E78CC
		public bool HasOil
		{
			get
			{
				return this.CurrentOilMass > 0f;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06007043 RID: 28739 RVA: 0x000E96DB File Offset: 0x000E78DB
		public float CurrentOilPercentage
		{
			get
			{
				return this.CurrentOilMass / this.oilAmount.GetMax();
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06007044 RID: 28740 RVA: 0x000E96EF File Offset: 0x000E78EF
		public float CurrentOilMass
		{
			get
			{
				if (this.oilAmount != null)
				{
					return this.oilAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06007046 RID: 28742 RVA: 0x000E9713 File Offset: 0x000E7913
		// (set) Token: 0x06007045 RID: 28741 RVA: 0x000E970A File Offset: 0x000E790A
		public AmountInstance oilAmount { get; private set; }

		// Token: 0x06007047 RID: 28743 RVA: 0x002F7A88 File Offset: 0x002F5C88
		public Instance(IStateMachineTarget master, BionicOilMonitor.Def def) : base(master, def)
		{
			this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(base.gameObject);
			AmountInstance oilAmount = this.oilAmount;
			oilAmount.OnMaxValueReached = (System.Action)Delegate.Combine(oilAmount.OnMaxValueReached, new System.Action(this.OnOilTankFilled));
			AmountInstance oilAmount2 = this.oilAmount;
			oilAmount2.OnMinValueReached = (System.Action)Delegate.Combine(oilAmount2.OnMinValueReached, new System.Action(this.OnOilRanOut));
			AmountInstance oilAmount3 = this.oilAmount;
			oilAmount3.OnValueChanged = (Action<float>)Delegate.Combine(oilAmount3.OnValueChanged, new Action<float>(this.OnOilValueChanged));
			this.batterySMI = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
		}

		// Token: 0x06007048 RID: 28744 RVA: 0x000E971B File Offset: 0x000E791B
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x000E9723 File Offset: 0x000E7923
		private void OnOilTankFilled()
		{
			base.sm.OilFilledSignal.Trigger(this);
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x000E9736 File Offset: 0x000E7936
		private void OnOilRanOut()
		{
			base.sm.OilRanOutSignal.Trigger(this);
		}

		// Token: 0x0600704B RID: 28747 RVA: 0x000E9749 File Offset: 0x000E7949
		private void OnOilValueChanged(float delta)
		{
			base.sm.OilValueChanged.Trigger(this);
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x000E975C File Offset: 0x000E795C
		public void SetOilMassValue(float value)
		{
			this.oilAmount.SetValue(value);
		}

		// Token: 0x0600704D RID: 28749 RVA: 0x002F7B74 File Offset: 0x002F5D74
		public void SetBaseDeltaModifierActiveState(bool isActive)
		{
			MinionModifiers component = base.GetComponent<MinionModifiers>();
			if (isActive)
			{
				bool flag = false;
				int count = component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers.Count;
				for (int i = 0; i < count; i++)
				{
					if (component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers[i] == this.BaseOilDeltaModifier)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					component.attributes.Add(this.BaseOilDeltaModifier);
					return;
				}
			}
			else
			{
				component.attributes.Remove(this.BaseOilDeltaModifier);
			}
		}

		// Token: 0x0600704E RID: 28750 RVA: 0x000E976B File Offset: 0x000E796B
		public void RefillOil(float amount)
		{
			this.oilAmount.SetValue(this.CurrentOilMass + amount);
			this.OnOilTankFilled();
		}

		// Token: 0x040053EF RID: 21487
		private BionicBatteryMonitor.Instance batterySMI;

		// Token: 0x040053F0 RID: 21488
		private AttributeModifier BaseOilDeltaModifier = new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, -0.033333335f, BionicMinionConfig.NAME, false, false, true);
	}
}
