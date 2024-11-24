using System;
using System.Collections.Generic;

// Token: 0x020011B5 RID: 4533
public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06005C77 RID: 23671 RVA: 0x000DC580 File Offset: 0x000DA780
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateNearbyCreatures", delegate(NearbyCreatureMonitor.Instance smi, float dt)
		{
			smi.UpdateNearbyCreatures(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x020011B6 RID: 4534
	public new class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06005C79 RID: 23673 RVA: 0x0029B1EC File Offset: 0x002993EC
		// (remove) Token: 0x06005C7A RID: 23674 RVA: 0x0029B224 File Offset: 0x00299424
		public event Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyCreatures;

		// Token: 0x06005C7B RID: 23675 RVA: 0x000DC5C4 File Offset: 0x000DA7C4
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x0029B25C File Offset: 0x0029945C
		public void UpdateNearbyCreatures(float dt)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
			if (cavityForCell != null)
			{
				this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures, cavityForCell.eggs);
			}
		}
	}
}
