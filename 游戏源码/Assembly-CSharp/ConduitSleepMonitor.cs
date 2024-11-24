using UnityEngine;

public class ConduitSleepMonitor : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>
{
	public class Def : BaseDef
	{
		public ObjectLayer conduitLayer;
	}

	private class SleepSearchStates : State
	{
		public State looking;

		public State found;
	}

	public new class Instance : GameInstance
	{
		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}
	}

	private State idle;

	private SleepSearchStates searching;

	public IntParameter targetSleepCell = new IntParameter(Grid.InvalidCell);

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = idle;
		base.serializable = SerializeType.ParamsOnly;
		idle.Enter(delegate(Instance smi)
		{
			targetSleepCell.Set(Grid.InvalidCell, smi);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		}).EventTransition(GameHashes.NewBlock, (Instance smi) => GameClock.Instance, searching.looking, IsSleepyTime);
		searching.Enter(TryRecoverSave).EventTransition(GameHashes.NewBlock, (Instance smi) => GameClock.Instance, idle, GameStateMachine<ConduitSleepMonitor, Instance, IStateMachineTarget, Def>.Not(IsSleepyTime)).Exit(delegate(Instance smi)
		{
			targetSleepCell.Set(Grid.InvalidCell, smi);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		});
		searching.looking.Update(delegate(Instance smi, float dt)
		{
			FindSleepLocation(smi);
		}, UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.NoSleepSpot).ParamTransition(targetSleepCell, searching.found, (Instance smi, int sleepCell) => sleepCell != Grid.InvalidCell);
		searching.found.Enter(delegate(Instance smi)
		{
			smi.GetComponent<Staterpillar>().SpawnConnectorBuilding(targetSleepCell.Get(smi));
		}).ParamTransition(targetSleepCell, searching.looking, (Instance smi, int sleepCell) => sleepCell == Grid.InvalidCell).ToggleBehaviour(GameTags.Creatures.WantsConduitConnection, (Instance smi) => targetSleepCell.Get(smi) != Grid.InvalidCell && IsSleepyTime(smi));
	}

	public static bool IsSleepyTime(Instance smi)
	{
		return GameClock.Instance.GetTimeSinceStartOfCycle() >= 500f;
	}

	private void TryRecoverSave(Instance smi)
	{
		Staterpillar component = smi.GetComponent<Staterpillar>();
		if (targetSleepCell.Get(smi) == Grid.InvalidCell && component.IsConnectorBuildingSpawned())
		{
			int value = Grid.PosToCell(component.GetConnectorBuilding());
			targetSleepCell.Set(value, smi);
		}
	}

	private void FindSleepLocation(Instance smi)
	{
		StaterpillarCellQuery staterpillarCellQuery = PathFinderQueries.staterpillarCellQuery.Reset(10, smi.gameObject, smi.def.conduitLayer);
		smi.GetComponent<Navigator>().RunQuery(staterpillarCellQuery);
		if (staterpillarCellQuery.result_cells.Count <= 0)
		{
			return;
		}
		foreach (int result_cell in staterpillarCellQuery.result_cells)
		{
			int cellInDirection = Grid.GetCellInDirection(result_cell, Direction.Down);
			if (Grid.Objects[cellInDirection, (int)smi.def.conduitLayer] != null)
			{
				targetSleepCell.Set(result_cell, smi);
				break;
			}
		}
		if (targetSleepCell.Get(smi) == Grid.InvalidCell)
		{
			targetSleepCell.Set(staterpillarCellQuery.result_cells[Random.Range(0, staterpillarCellQuery.result_cells.Count)], smi);
		}
	}
}
