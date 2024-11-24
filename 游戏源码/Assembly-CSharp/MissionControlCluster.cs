using System.Collections.Generic;

public class MissionControlCluster : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>
{
	public class Def : BaseDef
	{
	}

	public new class Instance : GameInstance
	{
		private int clusterUpdatedHandle = -1;

		private List<Clustercraft> boostableClustercraft = new List<Clustercraft>();

		[MyCmpReq]
		public RoomTracker roomTracker;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			clusterUpdatedHandle = Game.Instance.Subscribe(-1298331547, UpdateWorkableRocketsInRange);
		}

		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Game.Instance.Unsubscribe(clusterUpdatedHandle);
		}

		public void UpdateWorkableRocketsInRange(object data)
		{
			boostableClustercraft.Clear();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				if (!ClusterGrid.Instance.IsInRange(Components.Clustercrafts[i].Location, myWorldLocation, 2) || IsOwnWorld(Components.Clustercrafts[i]) || !CanBeBoosted(Components.Clustercrafts[i]))
				{
					continue;
				}
				bool flag = false;
				foreach (MissionControlClusterWorkable missionControlClusterWorkable in Components.MissionControlClusterWorkables)
				{
					if (!(missionControlClusterWorkable.gameObject == base.gameObject) && missionControlClusterWorkable.TargetClustercraft == Components.Clustercrafts[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					boostableClustercraft.Add(Components.Clustercrafts[i]);
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(boostableClustercraft.Count > 0, base.smi);
		}

		public Clustercraft GetRandomBoostableClustercraft()
		{
			return boostableClustercraft.GetRandom();
		}

		private bool CanBeBoosted(Clustercraft clustercraft)
		{
			if (clustercraft.controlStationBuffTimeRemaining != 0f)
			{
				return false;
			}
			if (!clustercraft.HasResourcesToMove() || !clustercraft.IsFlightInProgress())
			{
				return false;
			}
			return true;
		}

		private bool IsOwnWorld(Clustercraft candidateClustercraft)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			WorldContainer interiorWorld = candidateClustercraft.ModuleInterface.GetInteriorWorld();
			if (interiorWorld == null)
			{
				return false;
			}
			return myWorldId == interiorWorld.id;
		}

		public void ApplyEffect(Clustercraft clustercraft)
		{
			clustercraft.controlStationBuffTimeRemaining = 600f;
		}
	}

	public class OperationalState : State
	{
		public State WrongRoom;

		public State NoRockets;

		public State HasRockets;
	}

	public State Inoperational;

	public OperationalState Operational;

	public BoolParameter WorkableRocketsAreInRange;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Inoperational;
		Inoperational.EventTransition(GameHashes.OperationalChanged, Operational, ValidateOperationalTransition).EventTransition(GameHashes.UpdateRoom, Operational, ValidateOperationalTransition);
		Operational.EventTransition(GameHashes.OperationalChanged, Inoperational, ValidateOperationalTransition).EventTransition(GameHashes.UpdateRoom, Operational.WrongRoom, GameStateMachine<MissionControlCluster, Instance, IStateMachineTarget, Def>.Not(IsInLabRoom)).Enter(OnEnterOperational)
			.DefaultState(Operational.NoRockets)
			.Update(delegate(Instance smi, float dt)
			{
				smi.UpdateWorkableRocketsInRange(null);
			}, UpdateRate.SIM_1000ms);
		Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, Operational.NoRockets, IsInLabRoom);
		Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlClusterBoost).ParamTransition(WorkableRocketsAreInRange, Operational.HasRockets, (Instance smi, bool inRange) => WorkableRocketsAreInRange.Get(smi));
		Operational.HasRockets.ParamTransition(WorkableRocketsAreInRange, Operational.NoRockets, (Instance smi, bool inRange) => !WorkableRocketsAreInRange.Get(smi)).ToggleChore(CreateChore, Operational);
	}

	private Chore CreateChore(Instance smi)
	{
		MissionControlClusterWorkable component = smi.master.gameObject.GetComponent<MissionControlClusterWorkable>();
		WorkChore<MissionControlClusterWorkable> result = new WorkChore<MissionControlClusterWorkable>(Db.Get().ChoreTypes.Research, component);
		Clustercraft randomBoostableClustercraft = smi.GetRandomBoostableClustercraft();
		component.TargetClustercraft = randomBoostableClustercraft;
		return result;
	}

	private void OnEnterOperational(Instance smi)
	{
		smi.UpdateWorkableRocketsInRange(null);
		if (WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(Operational.HasRockets);
		}
		else
		{
			smi.GoTo(Operational.NoRockets);
		}
	}

	private bool ValidateOperationalTransition(Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		if (component != null)
		{
			return flag != component.IsOperational;
		}
		return false;
	}

	private bool IsInLabRoom(Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}
}
