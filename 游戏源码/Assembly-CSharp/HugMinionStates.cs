using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class HugMinionStates : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>
{
	// Token: 0x0600061E RID: 1566 RVA: 0x0015AF98 File Offset: 0x00159198
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		this.moving.MoveTo(new Func<HugMinionStates.Instance, int>(HugMinionStates.FindFlopLocation), this.waiting, this.behaviourcomplete, false);
		GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State state = this.waiting.Enter(delegate(HugMinionStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		}).ParamTransition<float>(this.timeout, this.behaviourcomplete, (HugMinionStates.Instance smi, float p) => p > 60f && !smi.GetSMI<HugMonitor.Instance>().IsHugging()).Update(delegate(HugMinionStates.Instance smi, float dt)
		{
			smi.sm.timeout.Delta(dt, smi);
		}, UpdateRate.SIM_200ms, false).PlayAnim("waiting_pre").QueueAnim("waiting_loop", true, null);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsAHug, false);
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0015B0C0 File Offset: 0x001592C0
	private static int FindFlopLocation(HugMinionStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 1);
		component.RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetFlopCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
		else
		{
			smi.targetFlopCell = Grid.InvalidCell;
		}
		return smi.targetFlopCell;
	}

	// Token: 0x04000478 RID: 1144
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x04000479 RID: 1145
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State waiting;

	// Token: 0x0400047A RID: 1146
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State behaviourcomplete;

	// Token: 0x0400047B RID: 1147
	public StateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.FloatParameter timeout;

	// Token: 0x020001C0 RID: 448
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001C1 RID: 449
	public new class Instance : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.GameInstance
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x000A8C6F File Offset: 0x000A6E6F
		public Instance(Chore<HugMinionStates.Instance> chore, HugMinionStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsAHug);
		}

		// Token: 0x0400047C RID: 1148
		public int targetFlopCell;
	}
}
