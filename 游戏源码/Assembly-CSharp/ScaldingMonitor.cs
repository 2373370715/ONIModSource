using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020015D8 RID: 5592
public class ScaldingMonitor : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>
{
	// Token: 0x060073DB RID: 29659 RVA: 0x00301884 File Offset: 0x002FFA84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State.Callback(ScaldingMonitor.SetInitialAverageExternalTemperature)).EventHandler(GameHashes.OnUnequip, new GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameEvent.Callback(ScaldingMonitor.OnSuitUnequipped)).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.AverageExternalTemperatureUpdate), UpdateRate.SIM_200ms, false);
		this.idle.Transition(this.transitionToScalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding), UpdateRate.SIM_200ms).Transition(this.transitionToScolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding), UpdateRate.SIM_200ms);
		this.transitionToScalding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding)), UpdateRate.SIM_200ms).Transition(this.scalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScaldingTimed), UpdateRate.SIM_200ms);
		this.transitionToScolding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding)), UpdateRate.SIM_200ms).Transition(this.scolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScoldingTimed), UpdateRate.SIM_200ms);
		this.scalding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScalding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeScaldDamage), UpdateRate.SIM_1000ms, false);
		this.scolding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScolding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scolding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeColdDamage), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x060073DC RID: 29660 RVA: 0x000EC040 File Offset: 0x000EA240
	public static void OnSuitUnequipped(ScaldingMonitor.Instance smi, object obj)
	{
		if (obj != null && ((Equippable)obj).HasTag(GameTags.AirtightSuit))
		{
			smi.ResetExternalTemperatureAverage();
		}
	}

	// Token: 0x060073DD RID: 29661 RVA: 0x000EC05D File Offset: 0x000EA25D
	public static void SetInitialAverageExternalTemperature(ScaldingMonitor.Instance smi)
	{
		smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature();
	}

	// Token: 0x060073DE RID: 29662 RVA: 0x000EC06B File Offset: 0x000EA26B
	public static bool CanEscapeScalding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x060073DF RID: 29663 RVA: 0x000EC084 File Offset: 0x000EA284
	public static bool CanEscapeScolding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x060073E0 RID: 29664 RVA: 0x000EC09D File Offset: 0x000EA29D
	public static bool IsScaldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x060073E1 RID: 29665 RVA: 0x000EC0B6 File Offset: 0x000EA2B6
	public static bool IsScalding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding();
	}

	// Token: 0x060073E2 RID: 29666 RVA: 0x000EC0BE File Offset: 0x000EA2BE
	public static bool IsScolding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding();
	}

	// Token: 0x060073E3 RID: 29667 RVA: 0x000EC0C6 File Offset: 0x000EA2C6
	public static bool IsScoldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x060073E4 RID: 29668 RVA: 0x000EC0DF File Offset: 0x000EA2DF
	public static void TakeScaldDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x060073E5 RID: 29669 RVA: 0x000EC0DF File Offset: 0x000EA2DF
	public static void TakeColdDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x060073E6 RID: 29670 RVA: 0x00301AB0 File Offset: 0x002FFCB0
	public static void AverageExternalTemperatureUpdate(ScaldingMonitor.Instance smi, float dt)
	{
		smi.AverageExternalTemperature *= Mathf.Max(0f, 1f - dt / 6f);
		smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature() * (dt / 6f);
	}

	// Token: 0x040056A7 RID: 22183
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x040056A8 RID: 22184
	private const float TEMPERATURE_AVERAGING_RANGE = 6f;

	// Token: 0x040056A9 RID: 22185
	private const float MIN_SCALD_INTERVAL = 5f;

	// Token: 0x040056AA RID: 22186
	private const float SCALDING_DAMAGE_AMOUNT = 10f;

	// Token: 0x040056AB RID: 22187
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State idle;

	// Token: 0x040056AC RID: 22188
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScalding;

	// Token: 0x040056AD RID: 22189
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScolding;

	// Token: 0x040056AE RID: 22190
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scalding;

	// Token: 0x040056AF RID: 22191
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scolding;

	// Token: 0x020015D9 RID: 5593
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040056B0 RID: 22192
		public float defaultScaldingTreshold = 345f;

		// Token: 0x040056B1 RID: 22193
		public float defaultScoldingTreshold = 183f;
	}

	// Token: 0x020015DA RID: 5594
	public new class Instance : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameInstance
	{
		// Token: 0x060073E9 RID: 29673 RVA: 0x00301AFC File Offset: 0x002FFCFC
		public Instance(IStateMachineTarget master, ScaldingMonitor.Def def) : base(master, def)
		{
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", def.defaultScaldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.baseScoldingThreshold = new AttributeModifier("ScoldingThreshold", def.defaultScoldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.attributes = base.gameObject.GetAttributes();
		}

		// Token: 0x060073EA RID: 29674 RVA: 0x00301B88 File Offset: 0x002FFD88
		public override void StartSM()
		{
			base.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(this.baseScalindingThreshold);
			base.smi.attributes.Get(Db.Get().Attributes.ScoldingThreshold).Add(this.baseScoldingThreshold);
			base.StartSM();
		}

		// Token: 0x060073EB RID: 29675 RVA: 0x00301BF0 File Offset: 0x002FFDF0
		public bool IsScalding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature > this.GetScaldingThreshold();
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x000EC10E File Offset: 0x000EA30E
		public float GetScaldingThreshold()
		{
			return base.smi.attributes.GetValue("ScaldingThreshold");
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x00301C48 File Offset: 0x002FFE48
		public bool IsScolding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature < this.GetScoldingThreshold();
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x000EC125 File Offset: 0x000EA325
		public float GetScoldingThreshold()
		{
			return base.smi.attributes.GetValue("ScoldingThreshold");
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x000EC13C File Offset: 0x000EA33C
		public void TemperatureDamage(float dt)
		{
			if (this.health != null && Time.time - this.lastScaldTime > 5f)
			{
				this.lastScaldTime = Time.time;
				this.health.Damage(dt * 10f);
			}
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x000EC17C File Offset: 0x000EA37C
		public void ResetExternalTemperatureAverage()
		{
			base.smi.AverageExternalTemperature = this.internalTemperature.value;
		}

		// Token: 0x060073F1 RID: 29681 RVA: 0x00301CA0 File Offset: 0x002FFEA0
		public float GetCurrentExternalTemperature()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (this.occupyArea != null)
			{
				float num2 = 0f;
				int num3 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num4 = Grid.OffsetCell(num, this.occupyArea.OccupiedCellsOffsets[i]);
					if (Grid.IsValidCell(num4))
					{
						bool flag = Grid.Element[num4].id == SimHashes.Vacuum || Grid.Element[num4].id == SimHashes.Void;
						num3++;
						num2 += (flag ? this.internalTemperature.value : Grid.Temperature[num4]);
					}
				}
				return num2 / (float)Mathf.Max(1, num3);
			}
			if (Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void)
			{
				return Grid.Temperature[num];
			}
			return this.internalTemperature.value;
		}

		// Token: 0x040056B2 RID: 22194
		public float AverageExternalTemperature;

		// Token: 0x040056B3 RID: 22195
		private float lastScaldTime;

		// Token: 0x040056B4 RID: 22196
		private Attributes attributes;

		// Token: 0x040056B5 RID: 22197
		[MyCmpGet]
		private Health health;

		// Token: 0x040056B6 RID: 22198
		[MyCmpGet]
		private OccupyArea occupyArea;

		// Token: 0x040056B7 RID: 22199
		private AttributeModifier baseScalindingThreshold;

		// Token: 0x040056B8 RID: 22200
		private AttributeModifier baseScoldingThreshold;

		// Token: 0x040056B9 RID: 22201
		public AmountInstance internalTemperature;
	}
}
