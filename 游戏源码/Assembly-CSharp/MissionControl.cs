using System.Collections.Generic;

public class MissionControl : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>
{
	public class Def : BaseDef
	{
	}

	public new class Instance : GameInstance
	{
		private List<Spacecraft> boostableSpacecraft = new List<Spacecraft>();

		[MyCmpReq]
		public RoomTracker roomTracker;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public void UpdateWorkableRockets(object data)
		{
			boostableSpacecraft.Clear();
			for (int i = 0; i < SpacecraftManager.instance.GetSpacecraft().Count; i++)
			{
				if (!CanBeBoosted(SpacecraftManager.instance.GetSpacecraft()[i]))
				{
					continue;
				}
				bool flag = false;
				foreach (MissionControlWorkable missionControlWorkable in Components.MissionControlWorkables)
				{
					if (!(missionControlWorkable.gameObject == base.gameObject) && missionControlWorkable.TargetSpacecraft == SpacecraftManager.instance.GetSpacecraft()[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					boostableSpacecraft.Add(SpacecraftManager.instance.GetSpacecraft()[i]);
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(boostableSpacecraft.Count > 0, base.smi);
		}

		public Spacecraft GetRandomBoostableSpacecraft()
		{
			return boostableSpacecraft.GetRandom();
		}

		private bool CanBeBoosted(Spacecraft spacecraft)
		{
			if (spacecraft.controlStationBuffTimeRemaining != 0f)
			{
				return false;
			}
			if (spacecraft.state != Spacecraft.MissionState.Underway)
			{
				return false;
			}
			return true;
		}

		public void ApplyEffect(Spacecraft spacecraft)
		{
			spacecraft.controlStationBuffTimeRemaining = 600f;
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
		Operational.EventTransition(GameHashes.OperationalChanged, Inoperational, ValidateOperationalTransition).EventTransition(GameHashes.UpdateRoom, Operational.WrongRoom, GameStateMachine<MissionControl, Instance, IStateMachineTarget, Def>.Not(IsInLabRoom)).Enter(OnEnterOperational)
			.DefaultState(Operational.NoRockets)
			.Update(delegate(Instance smi, float dt)
			{
				smi.UpdateWorkableRockets(null);
			}, UpdateRate.SIM_1000ms);
		Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, Operational.NoRockets, IsInLabRoom);
		Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlBoost).ParamTransition(WorkableRocketsAreInRange, Operational.HasRockets, (Instance smi, bool inRange) => WorkableRocketsAreInRange.Get(smi));
		Operational.HasRockets.ParamTransition(WorkableRocketsAreInRange, Operational.NoRockets, (Instance smi, bool inRange) => !WorkableRocketsAreInRange.Get(smi)).ToggleChore(CreateChore, Operational);
	}

	private Chore CreateChore(Instance smi)
	{
		MissionControlWorkable component = smi.master.gameObject.GetComponent<MissionControlWorkable>();
		WorkChore<MissionControlWorkable> result = new WorkChore<MissionControlWorkable>(Db.Get().ChoreTypes.Research, component);
		Spacecraft randomBoostableSpacecraft = smi.GetRandomBoostableSpacecraft();
		component.TargetSpacecraft = randomBoostableSpacecraft;
		return result;
	}

	private void OnEnterOperational(Instance smi)
	{
		smi.UpdateWorkableRockets(null);
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
