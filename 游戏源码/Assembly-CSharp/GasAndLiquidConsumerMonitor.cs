using UnityEngine;

public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
	public class Def : BaseDef
	{
		public Tag[] transitionTag = new Tag[1] { GameTags.Creatures.Hungry };

		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		public float minCooldown = 5f;

		public float maxCooldown = 5f;

		public Diet diet;

		public float consumptionRate = 0.5f;

		public Tag consumableElementTag = Tag.Invalid;
	}

	public new class Instance : GameInstance
	{
		public int targetCell = -1;

		private Element targetElement;

		private Navigator navigator;

		private int massUnavailableFrameCount;

		[MyCmpGet]
		private Storage storage;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			navigator = base.smi.GetComponent<Navigator>();
			DebugUtil.Assert(base.smi.def.diet != null || storage != null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
		}

		public void ClearTargetCell()
		{
			targetCell = -1;
			massUnavailableFrameCount = 0;
		}

		public void FindElement()
		{
			targetCell = -1;
			FindTargetCell();
		}

		public Element GetTargetElement()
		{
			return targetElement;
		}

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

		public void FindTargetCell()
		{
			ConsumableCellQuery consumableCellQuery = new ConsumableCellQuery(base.smi, 25);
			navigator.RunQuery(consumableCellQuery);
			if (consumableCellQuery.success)
			{
				targetCell = consumableCellQuery.GetResultCell();
				targetElement = consumableCellQuery.targetElement;
			}
		}

		public void Consume(float dt)
		{
			int index = Game.Instance.massConsumedCallbackManager.Add(OnMassConsumedCallback, this, "GasAndLiquidConsumerMonitor").index;
			SimMessages.ConsumeMass(Grid.PosToCell(this), targetElement.id, base.def.consumptionRate * dt, 3, index);
		}

		private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
		{
			((Instance)data).OnMassConsumed(mcd);
		}

		private void OnMassConsumed(Sim.MassConsumedCallback mcd)
		{
			if (!IsRunning())
			{
				return;
			}
			if (mcd.mass > 0f)
			{
				if (base.def.diet != null)
				{
					massUnavailableFrameCount = 0;
					Diet.Info dietInfo = base.def.diet.GetDietInfo(targetElement.tag);
					if (dietInfo != null)
					{
						float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
						CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = default(CreatureCalorieMonitor.CaloriesConsumedEvent);
						caloriesConsumedEvent.tag = targetElement.tag;
						caloriesConsumedEvent.calories = calories;
						CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent2 = caloriesConsumedEvent;
						Trigger(-2038961714, caloriesConsumedEvent2);
					}
				}
				else if (storage != null)
				{
					storage.AddElement(targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount);
				}
			}
			else
			{
				massUnavailableFrameCount++;
				if (massUnavailableFrameCount >= 2)
				{
					Trigger(801383139);
				}
			}
		}
	}

	public class ConsumableCellQuery : PathFinderQuery
	{
		public bool success;

		public Element targetElement;

		private Instance smi;

		private int maxIterations;

		public ConsumableCellQuery(Instance smi, int maxIterations)
		{
			this.smi = smi;
			this.maxIterations = maxIterations;
		}

		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			int cell2 = Grid.CellAbove(cell);
			success = smi.IsConsumableCell(cell, out targetElement) || (Grid.IsValidCell(cell2) && smi.IsConsumableCell(cell2, out targetElement));
			if (!success)
			{
				return --maxIterations <= 0;
			}
			return true;
		}
	}

	private State cooldown;

	private State satisfied;

	private State looking;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = cooldown;
		cooldown.Enter("ClearTargetCell", delegate(Instance smi)
		{
			smi.ClearTargetCell();
		}).ScheduleGoTo((Instance smi) => Random.Range(smi.def.minCooldown, smi.def.maxCooldown), satisfied);
		satisfied.Enter("ClearTargetCell", delegate(Instance smi)
		{
			smi.ClearTargetCell();
		}).TagTransition((Instance smi) => smi.def.transitionTag, looking);
		looking.ToggleBehaviour((Instance smi) => smi.def.behaviourTag, (Instance smi) => smi.targetCell != -1, delegate(Instance smi)
		{
			smi.GoTo(cooldown);
		}).TagTransition((Instance smi) => smi.def.transitionTag, satisfied, on_remove: true).PreBrainUpdate(delegate(Instance smi)
		{
			smi.FindElement();
		});
	}
}
