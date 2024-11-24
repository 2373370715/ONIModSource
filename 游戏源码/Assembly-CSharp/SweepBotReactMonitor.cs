using System;
using UnityEngine;

// Token: 0x020017D7 RID: 6103
public class SweepBotReactMonitor : GameStateMachine<SweepBotReactMonitor, SweepBotReactMonitor.Instance, IStateMachineTarget, SweepBotReactMonitor.Def>
{
	// Token: 0x06007DA0 RID: 32160 RVA: 0x00327284 File Offset: 0x00325484
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.OccupantChanged, delegate(SweepBotReactMonitor.Instance smi)
		{
			if (smi.master.gameObject.GetComponent<OrnamentReceptacle>().Occupant != null)
			{
				smi.GoTo(this.reactNewOrnament);
			}
		}).Update(delegate(SweepBotReactMonitor.Instance smi, float dt)
		{
			SweepStates.Instance smi2 = smi.master.gameObject.GetSMI<SweepStates.Instance>();
			int num = Grid.InvalidCell;
			if (smi2 != null)
			{
				if (smi2.sm.headingRight.Get(smi2))
				{
					num = Grid.CellRight(Grid.PosToCell(smi.master.gameObject));
				}
				else
				{
					num = Grid.CellLeft(Grid.PosToCell(smi.master.gameObject));
				}
				bool flag = false;
				bool flag2 = false;
				int num2;
				int num3;
				Grid.CellToXY(Grid.PosToCell(smi), out num2, out num3);
				ListPool<ScenePartitionerEntry, SweepBotReactMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, SweepBotReactMonitor>.Allocate();
				GameScenePartitioner.Instance.GatherEntries(num2 - 1, num3 - 1, 3, 3, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
				foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
				{
					Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
					if (!(pickupable == null) && !(pickupable.gameObject == smi.gameObject))
					{
						int num4 = Grid.PosToCell(pickupable);
						if (Vector3.Distance(smi.gameObject.transform.position, pickupable.gameObject.transform.position) < Grid.CellSizeInMeters)
						{
							if (pickupable.KPrefabID.IsPrefabID("SweepBot") && num4 == num)
							{
								smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("bump");
								smi2.sm.headingRight.Set(!smi2.sm.headingRight.Get(smi2), smi2, false);
								flag = true;
							}
							else if (pickupable.KPrefabID.HasTag(GameTags.Creature))
							{
								flag2 = true;
							}
						}
					}
				}
				pooledList.Recycle();
				if (!flag && smi.timeinstate > 10f && Grid.IsValidCell(num))
				{
					if (Grid.Objects[num, 0] != null && !Grid.Objects[num, 0].HasTag(GameTags.Dead))
					{
						smi.GoTo(this.reactFriendlyThing);
						return;
					}
					if (smi2.sm.bored.Get(smi2) && Grid.Objects[num, 3] != null)
					{
						smi.GoTo(this.reactFriendlyThing);
						return;
					}
					if (flag2)
					{
						smi.GoTo(this.reactScaryThing);
					}
				}
			}
		}, UpdateRate.SIM_33ms, false);
		this.reactScaryThing.Enter(delegate(SweepBotReactMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_neg");
		}).ToggleStatusItem(Db.Get().RobotStatusItems.ReactNegative, null, Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.idle);
		this.reactFriendlyThing.Enter(delegate(SweepBotReactMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_pos");
		}).ToggleStatusItem(Db.Get().RobotStatusItems.ReactPositive, null, Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.idle);
		this.reactNewOrnament.Enter(delegate(SweepBotReactMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_ornament");
		}).OnAnimQueueComplete(this.idle).ToggleStatusItem(Db.Get().RobotStatusItems.ReactPositive, null);
	}

	// Token: 0x04005F28 RID: 24360
	private GameStateMachine<SweepBotReactMonitor, SweepBotReactMonitor.Instance, IStateMachineTarget, SweepBotReactMonitor.Def>.State idle;

	// Token: 0x04005F29 RID: 24361
	private GameStateMachine<SweepBotReactMonitor, SweepBotReactMonitor.Instance, IStateMachineTarget, SweepBotReactMonitor.Def>.State reactScaryThing;

	// Token: 0x04005F2A RID: 24362
	private GameStateMachine<SweepBotReactMonitor, SweepBotReactMonitor.Instance, IStateMachineTarget, SweepBotReactMonitor.Def>.State reactFriendlyThing;

	// Token: 0x04005F2B RID: 24363
	private GameStateMachine<SweepBotReactMonitor, SweepBotReactMonitor.Instance, IStateMachineTarget, SweepBotReactMonitor.Def>.State reactNewOrnament;

	// Token: 0x020017D8 RID: 6104
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017D9 RID: 6105
	public new class Instance : GameStateMachine<SweepBotReactMonitor, SweepBotReactMonitor.Instance, IStateMachineTarget, SweepBotReactMonitor.Def>.GameInstance
	{
		// Token: 0x06007DA5 RID: 32165 RVA: 0x000F2C62 File Offset: 0x000F0E62
		public Instance(IStateMachineTarget master, SweepBotReactMonitor.Def def) : base(master, def)
		{
		}
	}
}
