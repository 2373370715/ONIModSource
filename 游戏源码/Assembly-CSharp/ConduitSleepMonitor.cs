using System;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class ConduitSleepMonitor : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>
{
	// Token: 0x060004D6 RID: 1238 RVA: 0x001579E8 File Offset: 0x00155BE8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.idle.Enter(delegate(ConduitSleepMonitor.Instance smi)
		{
			this.targetSleepCell.Set(Grid.InvalidCell, smi, false);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		}).EventTransition(GameHashes.NewBlock, (ConduitSleepMonitor.Instance smi) => GameClock.Instance, this.searching.looking, new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Transition.ConditionCallback(ConduitSleepMonitor.IsSleepyTime));
		this.searching.Enter(new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State.Callback(this.TryRecoverSave)).EventTransition(GameHashes.NewBlock, (ConduitSleepMonitor.Instance smi) => GameClock.Instance, this.idle, GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Not(new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Transition.ConditionCallback(ConduitSleepMonitor.IsSleepyTime))).Exit(delegate(ConduitSleepMonitor.Instance smi)
		{
			this.targetSleepCell.Set(Grid.InvalidCell, smi, false);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		});
		this.searching.looking.Update(delegate(ConduitSleepMonitor.Instance smi, float dt)
		{
			this.FindSleepLocation(smi);
		}, UpdateRate.SIM_1000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.NoSleepSpot, null).ParamTransition<int>(this.targetSleepCell, this.searching.found, (ConduitSleepMonitor.Instance smi, int sleepCell) => sleepCell != Grid.InvalidCell);
		this.searching.found.Enter(delegate(ConduitSleepMonitor.Instance smi)
		{
			smi.GetComponent<Staterpillar>().SpawnConnectorBuilding(this.targetSleepCell.Get(smi));
		}).ParamTransition<int>(this.targetSleepCell, this.searching.looking, (ConduitSleepMonitor.Instance smi, int sleepCell) => sleepCell == Grid.InvalidCell).ToggleBehaviour(GameTags.Creatures.WantsConduitConnection, (ConduitSleepMonitor.Instance smi) => this.targetSleepCell.Get(smi) != Grid.InvalidCell && ConduitSleepMonitor.IsSleepyTime(smi), null);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000A7CDF File Offset: 0x000A5EDF
	public static bool IsSleepyTime(ConduitSleepMonitor.Instance smi)
	{
		return GameClock.Instance.GetTimeSinceStartOfCycle() >= 500f;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00157B98 File Offset: 0x00155D98
	private void TryRecoverSave(ConduitSleepMonitor.Instance smi)
	{
		Staterpillar component = smi.GetComponent<Staterpillar>();
		if (this.targetSleepCell.Get(smi) == Grid.InvalidCell && component.IsConnectorBuildingSpawned())
		{
			int value = Grid.PosToCell(component.GetConnectorBuilding());
			this.targetSleepCell.Set(value, smi, false);
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00157BE4 File Offset: 0x00155DE4
	private void FindSleepLocation(ConduitSleepMonitor.Instance smi)
	{
		StaterpillarCellQuery staterpillarCellQuery = PathFinderQueries.staterpillarCellQuery.Reset(10, smi.gameObject, smi.def.conduitLayer);
		smi.GetComponent<Navigator>().RunQuery(staterpillarCellQuery);
		if (staterpillarCellQuery.result_cells.Count > 0)
		{
			foreach (int num in staterpillarCellQuery.result_cells)
			{
				int cellInDirection = Grid.GetCellInDirection(num, Direction.Down);
				if (Grid.Objects[cellInDirection, (int)smi.def.conduitLayer] != null)
				{
					this.targetSleepCell.Set(num, smi, false);
					break;
				}
			}
			if (this.targetSleepCell.Get(smi) == Grid.InvalidCell)
			{
				this.targetSleepCell.Set(staterpillarCellQuery.result_cells[UnityEngine.Random.Range(0, staterpillarCellQuery.result_cells.Count)], smi, false);
			}
		}
	}

	// Token: 0x04000388 RID: 904
	private GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State idle;

	// Token: 0x04000389 RID: 905
	private ConduitSleepMonitor.SleepSearchStates searching;

	// Token: 0x0400038A RID: 906
	public StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.IntParameter targetSleepCell = new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.IntParameter(Grid.InvalidCell);

	// Token: 0x0200014C RID: 332
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400038B RID: 907
		public ObjectLayer conduitLayer;
	}

	// Token: 0x0200014D RID: 333
	private class SleepSearchStates : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State
	{
		// Token: 0x0400038C RID: 908
		public GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State looking;

		// Token: 0x0400038D RID: 909
		public GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State found;
	}

	// Token: 0x0200014E RID: 334
	public new class Instance : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.GameInstance
	{
		// Token: 0x060004E2 RID: 1250 RVA: 0x000A7D74 File Offset: 0x000A5F74
		public Instance(IStateMachineTarget master, ConduitSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
