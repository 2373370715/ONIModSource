using System;

// Token: 0x020015D6 RID: 5590
public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
	// Token: 0x060073D7 RID: 29655 RVA: 0x000EC008 File Offset: 0x000EA208
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType));
	}

	// Token: 0x060073D8 RID: 29656 RVA: 0x00301830 File Offset: 0x002FFA30
	private static void UpdateRoomType(RoomMonitor.Instance smi)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
		if (roomOfGameObject != smi.currentRoom)
		{
			smi.currentRoom = roomOfGameObject;
			if (roomOfGameObject != null)
			{
				roomOfGameObject.cavity.OnEnter(smi.master.gameObject);
			}
		}
	}

	// Token: 0x020015D7 RID: 5591
	public new class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060073DA RID: 29658 RVA: 0x000EC037 File Offset: 0x000EA237
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x040056A6 RID: 22182
		public Room currentRoom;
	}
}
