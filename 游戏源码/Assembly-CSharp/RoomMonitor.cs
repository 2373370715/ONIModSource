public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
	public new class Instance : GameInstance
	{
		public Room currentRoom;

		public Instance(IStateMachineTarget master)
			: base(master)
		{
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		root.EventHandler(GameHashes.PathAdvanced, UpdateRoomType);
	}

	private static void UpdateRoomType(Instance smi)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
		if (roomOfGameObject != smi.currentRoom)
		{
			smi.currentRoom = roomOfGameObject;
			roomOfGameObject?.cavity.OnEnter(smi.master.gameObject);
		}
	}
}
