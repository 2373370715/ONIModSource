using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001150 RID: 4432
public class CritterTemperatureMonitor : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>
{
	// Token: 0x06005A8F RID: 23183 RVA: 0x00294B04 File Offset: 0x00292D04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.uncomfortableEffect = new Effect("EffectCritterTemperatureUncomfortable", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.uncomfortableEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, false, false, true));
		this.deadlyEffect = new Effect("EffectCritterTemperatureDeadly", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.deadlyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -2f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, false, false, true));
		this.root.Enter(new StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback(CritterTemperatureMonitor.RefreshInternalTemperature)).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			StateMachine.BaseState targetState = smi.GetTargetState();
			if (smi.GetCurrentState() != targetState)
			{
				smi.GoTo(targetState);
			}
		}, UpdateRate.SIM_200ms, false).Update(new Action<CritterTemperatureMonitor.Instance, float>(CritterTemperatureMonitor.UpdateInternalTemperature), UpdateRate.SIM_1000ms, false);
		this.hot.TagTransition(GameTags.Dead, this.dead, false).ToggleCreatureThought(Db.Get().Thoughts.Hot, null);
		this.cold.TagTransition(GameTags.Dead, this.dead, false).ToggleCreatureThought(Db.Get().Thoughts.Cold, null);
		this.hot.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.hot.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.cold.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.cold.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.dead.DoNothing();
	}

	// Token: 0x06005A90 RID: 23184 RVA: 0x000DB04C File Offset: 0x000D924C
	public static void UpdateInternalTemperature(CritterTemperatureMonitor.Instance smi, float dt)
	{
		CritterTemperatureMonitor.RefreshInternalTemperature(smi);
		if (smi.OnUpdate_GetTemperatureInternal != null)
		{
			smi.OnUpdate_GetTemperatureInternal(dt, smi.GetTemperatureInternal());
		}
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x000DB06E File Offset: 0x000D926E
	public static void RefreshInternalTemperature(CritterTemperatureMonitor.Instance smi)
	{
		if (smi.temperature != null)
		{
			smi.temperature.SetValue(smi.GetTemperatureInternal());
		}
	}

	// Token: 0x04003FDD RID: 16349
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State comfortable;

	// Token: 0x04003FDE RID: 16350
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State dead;

	// Token: 0x04003FDF RID: 16351
	public CritterTemperatureMonitor.TemperatureStates hot;

	// Token: 0x04003FE0 RID: 16352
	public CritterTemperatureMonitor.TemperatureStates cold;

	// Token: 0x04003FE1 RID: 16353
	public Effect uncomfortableEffect;

	// Token: 0x04003FE2 RID: 16354
	public Effect deadlyEffect;

	// Token: 0x02001151 RID: 4433
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005A97 RID: 23191 RVA: 0x000DB0A2 File Offset: 0x000D92A2
		public float GetIdealTemperature()
		{
			return (this.temperatureHotUncomfortable + this.temperatureColdUncomfortable) / 2f;
		}

		// Token: 0x04003FE3 RID: 16355
		public float temperatureHotDeadly = float.MaxValue;

		// Token: 0x04003FE4 RID: 16356
		public float temperatureHotUncomfortable = float.MaxValue;

		// Token: 0x04003FE5 RID: 16357
		public float temperatureColdDeadly = float.MinValue;

		// Token: 0x04003FE6 RID: 16358
		public float temperatureColdUncomfortable = float.MinValue;

		// Token: 0x04003FE7 RID: 16359
		public float secondsUntilDamageStarts = 1f;

		// Token: 0x04003FE8 RID: 16360
		public float damagePerSecond = 0.25f;

		// Token: 0x04003FE9 RID: 16361
		public bool isBammoth;
	}

	// Token: 0x02001152 RID: 4434
	public class TemperatureStates : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State
	{
		// Token: 0x04003FEA RID: 16362
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State uncomfortable;

		// Token: 0x04003FEB RID: 16363
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State deadly;
	}

	// Token: 0x02001153 RID: 4435
	public new class Instance : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.GameInstance
	{
		// Token: 0x06005A9A RID: 23194 RVA: 0x00294E6C File Offset: 0x0029306C
		public Instance(IStateMachineTarget master, CritterTemperatureMonitor.Def def) : base(master, def)
		{
			this.health = master.GetComponent<Health>();
			this.occupyArea = master.GetComponent<OccupyArea>();
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.CritterTemperature.Lookup(base.gameObject);
			this.pickupable = master.GetComponent<Pickupable>();
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x000DB0BF File Offset: 0x000D92BF
		public void ResetDamageCooldown()
		{
			this.secondsUntilDamage = base.def.secondsUntilDamageStarts;
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x000DB0D2 File Offset: 0x000D92D2
		public void TryDamage(float deltaSeconds)
		{
			if (this.secondsUntilDamage <= 0f)
			{
				this.health.Damage(base.def.damagePerSecond);
				this.secondsUntilDamage = 1f;
				return;
			}
			this.secondsUntilDamage -= deltaSeconds;
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x00294ED4 File Offset: 0x002930D4
		public StateMachine.BaseState GetTargetState()
		{
			bool flag = this.IsEntirelyInVaccum();
			float temperatureExternal = this.GetTemperatureExternal();
			float temperatureInternal = this.GetTemperatureInternal();
			StateMachine.BaseState result;
			if (this.pickupable.KPrefabID.HasTag(GameTags.Dead))
			{
				result = base.sm.dead;
			}
			else if (!flag && temperatureExternal > base.def.temperatureHotDeadly)
			{
				result = base.sm.hot.deadly;
			}
			else if (!flag && temperatureExternal < base.def.temperatureColdDeadly)
			{
				result = base.sm.cold.deadly;
			}
			else if (temperatureInternal > base.def.temperatureHotUncomfortable)
			{
				result = base.sm.hot.uncomfortable;
			}
			else if (temperatureInternal < base.def.temperatureColdUncomfortable)
			{
				result = base.sm.cold.uncomfortable;
			}
			else
			{
				result = base.sm.comfortable;
			}
			return result;
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x00294FB8 File Offset: 0x002931B8
		public bool IsEntirelyInVaccum()
		{
			int cachedCell = this.pickupable.cachedCell;
			bool result;
			if (this.occupyArea != null)
			{
				result = true;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					if (!base.def.isBammoth || this.occupyArea.OccupiedCellsOffsets[i].x == 0)
					{
						int num = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
						if (!Grid.IsValidCell(num) || !Grid.Element[num].IsVacuum)
						{
							result = false;
							break;
						}
					}
				}
			}
			else
			{
				result = (!Grid.IsValidCell(cachedCell) || Grid.Element[cachedCell].IsVacuum);
			}
			return result;
		}

		// Token: 0x06005A9F RID: 23199 RVA: 0x000DB111 File Offset: 0x000D9311
		public float GetTemperatureInternal()
		{
			return this.primaryElement.Temperature;
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x0029506C File Offset: 0x0029326C
		public float GetTemperatureExternal()
		{
			int cachedCell = this.pickupable.cachedCell;
			if (this.occupyArea != null)
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					if (!base.def.isBammoth || this.occupyArea.OccupiedCellsOffsets[i].x == 0)
					{
						int num3 = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
						if (Grid.IsValidCell(num3))
						{
							bool flag = Grid.Element[num3].id == SimHashes.Vacuum || Grid.Element[num3].id == SimHashes.Void;
							num2++;
							num += (flag ? this.GetTemperatureInternal() : Grid.Temperature[num3]);
						}
					}
				}
				return num / (float)Mathf.Max(1, num2);
			}
			if (Grid.Element[cachedCell].id != SimHashes.Vacuum && Grid.Element[cachedCell].id != SimHashes.Void)
			{
				return Grid.Temperature[cachedCell];
			}
			return this.GetTemperatureInternal();
		}

		// Token: 0x04003FEC RID: 16364
		public AmountInstance temperature;

		// Token: 0x04003FED RID: 16365
		public Health health;

		// Token: 0x04003FEE RID: 16366
		public OccupyArea occupyArea;

		// Token: 0x04003FEF RID: 16367
		public PrimaryElement primaryElement;

		// Token: 0x04003FF0 RID: 16368
		public Pickupable pickupable;

		// Token: 0x04003FF1 RID: 16369
		public float secondsUntilDamage;

		// Token: 0x04003FF2 RID: 16370
		public Action<float, float> OnUpdate_GetTemperatureInternal;
	}
}
