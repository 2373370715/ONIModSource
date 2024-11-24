using System;
using UnityEngine;

// Token: 0x02000A10 RID: 2576
public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
	// Token: 0x06002F1F RID: 12063 RVA: 0x001F7174 File Offset: 0x001F5374
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).ScheduleGoTo((GasAndLiquidConsumerMonitor.Instance smi) => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown), this.satisfied);
		this.satisfied.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.looking, false);
		this.looking.ToggleBehaviour((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.behaviourTag, (GasAndLiquidConsumerMonitor.Instance smi) => smi.targetCell != -1, delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.satisfied, true).PreBrainUpdate(delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.FindElement();
		});
	}

	// Token: 0x04001FBE RID: 8126
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;

	// Token: 0x04001FBF RID: 8127
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;

	// Token: 0x04001FC0 RID: 8128
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State looking;

	// Token: 0x02000A11 RID: 2577
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001FC1 RID: 8129
		public Tag[] transitionTag = new Tag[]
		{
			GameTags.Creatures.Hungry
		};

		// Token: 0x04001FC2 RID: 8130
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x04001FC3 RID: 8131
		public float minCooldown = 5f;

		// Token: 0x04001FC4 RID: 8132
		public float maxCooldown = 5f;

		// Token: 0x04001FC5 RID: 8133
		public Diet diet;

		// Token: 0x04001FC6 RID: 8134
		public float consumptionRate = 0.5f;

		// Token: 0x04001FC7 RID: 8135
		public Tag consumableElementTag = Tag.Invalid;
	}

	// Token: 0x02000A12 RID: 2578
	public new class Instance : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x06002F23 RID: 12067 RVA: 0x001F7348 File Offset: 0x001F5548
		public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def) : base(master, def)
		{
			this.navigator = base.smi.GetComponent<Navigator>();
			DebugUtil.Assert(base.smi.def.diet != null || this.storage != null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000BE98D File Offset: 0x000BCB8D
		public void ClearTargetCell()
		{
			this.targetCell = -1;
			this.massUnavailableFrameCount = 0;
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000BE99D File Offset: 0x000BCB9D
		public void FindElement()
		{
			this.targetCell = -1;
			this.FindTargetCell();
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000BE9AC File Offset: 0x000BCBAC
		public Element GetTargetElement()
		{
			return this.targetElement;
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x001F73A0 File Offset: 0x001F55A0
		public bool IsConsumableCell(int cell, out Element element)
		{
			element = Grid.Element[cell];
			bool flag = true;
			bool flag2 = true;
			if (base.smi.def.consumableElementTag != Tag.Invalid)
			{
				flag = element.HasTag(base.smi.def.consumableElementTag);
			}
			if (base.smi.def.diet != null)
			{
				flag2 = false;
				Diet.Info[] infos = base.smi.def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].IsMatch(element.tag))
					{
						flag2 = true;
						break;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x001F7440 File Offset: 0x001F5640
		public void FindTargetCell()
		{
			GasAndLiquidConsumerMonitor.ConsumableCellQuery consumableCellQuery = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(base.smi, 25);
			this.navigator.RunQuery(consumableCellQuery);
			if (consumableCellQuery.success)
			{
				this.targetCell = consumableCellQuery.GetResultCell();
				this.targetElement = consumableCellQuery.targetElement;
			}
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x001F7488 File Offset: 0x001F5688
		public void Consume(float dt)
		{
			int index = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback), this, "GasAndLiquidConsumerMonitor").index;
			SimMessages.ConsumeMass(Grid.PosToCell(this), this.targetElement.id, base.def.consumptionRate * dt, 3, index);
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000BE9B4 File Offset: 0x000BCBB4
		private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
		{
			((GasAndLiquidConsumerMonitor.Instance)data).OnMassConsumed(mcd);
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x001F74E4 File Offset: 0x001F56E4
		private void OnMassConsumed(Sim.MassConsumedCallback mcd)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (mcd.mass > 0f)
			{
				if (base.def.diet != null)
				{
					this.massUnavailableFrameCount = 0;
					Diet.Info dietInfo = base.def.diet.GetDietInfo(this.targetElement.tag);
					if (dietInfo == null)
					{
						return;
					}
					float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
					CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
					{
						tag = this.targetElement.tag,
						calories = calories
					};
					base.Trigger(-2038961714, caloriesConsumedEvent);
					return;
				}
				else if (this.storage != null)
				{
					this.storage.AddElement(this.targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount, false, true);
					return;
				}
			}
			else
			{
				this.massUnavailableFrameCount++;
				if (this.massUnavailableFrameCount >= 2)
				{
					base.Trigger(801383139, null);
				}
			}
		}

		// Token: 0x04001FC8 RID: 8136
		public int targetCell = -1;

		// Token: 0x04001FC9 RID: 8137
		private Element targetElement;

		// Token: 0x04001FCA RID: 8138
		private Navigator navigator;

		// Token: 0x04001FCB RID: 8139
		private int massUnavailableFrameCount;

		// Token: 0x04001FCC RID: 8140
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02000A13 RID: 2579
	public class ConsumableCellQuery : PathFinderQuery
	{
		// Token: 0x06002F2C RID: 12076 RVA: 0x000BE9C2 File Offset: 0x000BCBC2
		public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
		{
			this.smi = smi;
			this.maxIterations = maxIterations;
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x001F75E4 File Offset: 0x001F57E4
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			int cell2 = Grid.CellAbove(cell);
			this.success = (this.smi.IsConsumableCell(cell, out this.targetElement) || (Grid.IsValidCell(cell2) && this.smi.IsConsumableCell(cell2, out this.targetElement)));
			if (!this.success)
			{
				int num = this.maxIterations - 1;
				this.maxIterations = num;
				return num <= 0;
			}
			return true;
		}

		// Token: 0x04001FCD RID: 8141
		public bool success;

		// Token: 0x04001FCE RID: 8142
		public Element targetElement;

		// Token: 0x04001FCF RID: 8143
		private GasAndLiquidConsumerMonitor.Instance smi;

		// Token: 0x04001FD0 RID: 8144
		private int maxIterations;
	}
}
