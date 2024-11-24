using System;

// Token: 0x020009D4 RID: 2516
public class CleaningMonitor : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>
{
	// Token: 0x06002E48 RID: 11848 RVA: 0x001F4584 File Offset: 0x001F2784
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.clean;
		this.clean.ToggleBehaviour(GameTags.Creatures.Cleaning, (CleaningMonitor.Instance smi) => smi.CanCleanElementState(), delegate(CleaningMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
		this.cooldown.ScheduleGoTo((CleaningMonitor.Instance smi) => smi.def.coolDown, this.clean);
	}

	// Token: 0x04001F1B RID: 7963
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State cooldown;

	// Token: 0x04001F1C RID: 7964
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State clean;

	// Token: 0x020009D5 RID: 2517
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001F1D RID: 7965
		public Element.State elementState = Element.State.Liquid;

		// Token: 0x04001F1E RID: 7966
		public CellOffset[] cellOffsets;

		// Token: 0x04001F1F RID: 7967
		public float coolDown = 30f;
	}

	// Token: 0x020009D6 RID: 2518
	public new class Instance : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.GameInstance
	{
		// Token: 0x06002E4C RID: 11852 RVA: 0x000BE01A File Offset: 0x000BC21A
		public Instance(IStateMachineTarget master, CleaningMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x001F4608 File Offset: 0x001F2808
		public bool CanCleanElementState()
		{
			int num = Grid.PosToCell(base.smi.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (!Grid.IsLiquid(num) && base.smi.def.elementState == Element.State.Liquid)
			{
				return false;
			}
			if (Grid.DiseaseCount[num] > 0)
			{
				return true;
			}
			if (base.smi.def.cellOffsets != null)
			{
				foreach (CellOffset offset in base.smi.def.cellOffsets)
				{
					int num2 = Grid.OffsetCell(num, offset);
					if (Grid.IsValidCell(num2) && Grid.DiseaseCount[num2] > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
