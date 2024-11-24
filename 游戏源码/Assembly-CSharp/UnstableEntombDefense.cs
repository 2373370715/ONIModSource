using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class UnstableEntombDefense : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public float Cooldown = 5f;

		public string defaultAnimName = "";

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			Instance sMI = go.GetSMI<Instance>();
			if (sMI != null)
			{
				Descriptor stateDescriptor = sMI.GetStateDescriptor();
				if (stateDescriptor.type == Descriptor.DescriptorType.Effect)
				{
					list.Add(stateDescriptor);
				}
			}
			return list;
		}
	}

	public class SafeStates : State
	{
		public State idle;

		public State newThreat;
	}

	public class ThreatenedStates : State
	{
		public State inCooldown;

		public State react;

		public State complete;
	}

	public class ActiveState : State
	{
		public SafeStates safe;

		public ThreatenedStates threatened;
	}

	public new class Instance : GameInstance
	{
		public string UnentombAnimName;

		[MyCmpGet]
		private EntombVulnerable entombVulnerable;

		[MyCmpGet]
		private OccupyArea occupyArea;

		public float RemainingCooldown => base.sm.TimeBeforeNextReaction.Get(this);

		public bool IsEntombed => entombVulnerable.GetEntombed;

		public bool IsActive => base.sm.Active.Get(this);

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			UnentombAnimName = ((UnentombAnimName == null) ? def.defaultAnimName : UnentombAnimName);
		}

		public bool IsInPressenceOfUnstableSolids()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					return true;
				}
			}
			return false;
		}

		public void AttackUnstableCells()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					SimMessages.Dig(num);
				}
			}
		}

		public void SetActive(bool active)
		{
			base.sm.Active.Set(active, this);
		}

		public Descriptor GetStateDescriptor()
		{
			if (IsInsideState(base.sm.disabled))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEOFF, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEOFF);
			}
			if (IsInsideState(base.sm.active.safe))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREADY, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREADY);
			}
			if (IsInsideState(base.sm.active.threatened.inCooldown))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSETHREATENED, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSETHREATENED);
			}
			if (IsInsideState(base.sm.active.threatened.react))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREACTING, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREACTING);
			}
			Descriptor result = default(Descriptor);
			result.type = Descriptor.DescriptorType.Detail;
			return result;
		}
	}

	public ActiveState active;

	public State disabled;

	public State dead;

	public FloatParameter TimeBeforeNextReaction;

	public BoolParameter Active = new BoolParameter(default_value: true);

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = disabled;
		disabled.EventTransition(GameHashes.Died, dead).ParamTransition(Active, active, GameStateMachine<UnstableEntombDefense, Instance, IStateMachineTarget, Def>.IsTrue);
		active.EventTransition(GameHashes.Died, dead).ParamTransition(Active, disabled, GameStateMachine<UnstableEntombDefense, Instance, IStateMachineTarget, Def>.IsFalse).DefaultState(active.safe);
		active.safe.DefaultState(active.safe.idle);
		active.safe.idle.ParamTransition(TimeBeforeNextReaction, active.threatened, (Instance smi, float p) => GameStateMachine<UnstableEntombDefense, Instance, IStateMachineTarget, Def>.IsGTZero(smi, p) && IsEntombedByUnstable(smi)).EventTransition(GameHashes.EntombedChanged, active.safe.newThreat, IsEntombedByUnstable);
		active.safe.newThreat.Enter(ResetCooldown).GoTo(active.threatened);
		active.threatened.EventTransition(GameHashes.Died, dead).Exit(ResetCooldown).EventTransition(GameHashes.EntombedChanged, active.safe, GameStateMachine<UnstableEntombDefense, Instance, IStateMachineTarget, Def>.Not(IsEntombedByUnstable))
			.DefaultState(active.threatened.inCooldown);
		active.threatened.inCooldown.ParamTransition(TimeBeforeNextReaction, active.threatened.react, GameStateMachine<UnstableEntombDefense, Instance, IStateMachineTarget, Def>.IsLTEZero).Update(CooldownTick);
		active.threatened.react.TriggerOnEnter(GameHashes.EntombDefenseReactionBegins).PlayAnim((Instance smi) => smi.UnentombAnimName).OnAnimQueueComplete(active.threatened.complete)
			.ScheduleGoTo(2f, active.threatened.complete);
		active.threatened.complete.TriggerOnEnter(GameHashes.EntombDefenseReact).Enter(AttemptToBreakFree).Enter(ResetCooldown)
			.GoTo(active.threatened.inCooldown);
		dead.DoNothing();
	}

	public static void ResetCooldown(Instance smi)
	{
		smi.sm.TimeBeforeNextReaction.Set(smi.def.Cooldown, smi);
	}

	public static bool IsEntombedByUnstable(Instance smi)
	{
		if (smi.IsEntombed)
		{
			return smi.IsInPressenceOfUnstableSolids();
		}
		return false;
	}

	public static void AttemptToBreakFree(Instance smi)
	{
		smi.AttackUnstableCells();
	}

	public static void CooldownTick(Instance smi, float dt)
	{
		float value = smi.RemainingCooldown - dt;
		smi.sm.TimeBeforeNextReaction.Set(value, smi);
	}
}
