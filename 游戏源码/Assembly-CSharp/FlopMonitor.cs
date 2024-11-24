using System;
using UnityEngine;

// Token: 0x02000A0C RID: 2572
public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
	// Token: 0x06002F17 RID: 12055 RVA: 0x000BE916 File Offset: 0x000BCB16
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (FlopMonitor.Instance smi) => smi.ShouldBeginFlopping(), null);
	}

	// Token: 0x02000A0D RID: 2573
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A0E RID: 2574
	public new class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
	{
		// Token: 0x06002F1A RID: 12058 RVA: 0x000BE959 File Offset: 0x000BCB59
		public Instance(IStateMachineTarget master, FlopMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x001F70FC File Offset: 0x001F52FC
		public bool ShouldBeginFlopping()
		{
			Vector3 position = base.transform.GetPosition();
			position.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			int num = Grid.PosToCell(position);
			return Grid.IsValidCell(num) && Grid.Solid[num] && !Grid.IsSubstantialLiquid(cell, 0.35f) && !Grid.IsLiquid(Grid.CellAbove(cell));
		}
	}
}
