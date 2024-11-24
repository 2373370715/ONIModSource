using Klei.AI;
using UnityEngine;

public class CritterCondoStates : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>
{
	public class Def : BaseDef
	{
		public bool entersBuilding = true;

		public string working_anim = "cc_working";
	}

	public new class Instance : GameInstance
	{
		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_InteractWithCritterCondo);
		}
	}

	public class InteractState : State
	{
		public State pre;

		public State loop;

		public State pst;
	}

	public State goingToCondo;

	public InteractState interact;

	public State behaviourComplete;

	public TargetParameter targetCondo;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = goingToCondo;
		root.Enter(ReserveCondo).Exit(UnreserveCondo);
		goingToCondo.MoveTo(GetCondoInteractCell, interact).ToggleMainStatusItem((Instance smi) => GetTargetCondo(smi).def.moveToStatusItem).OnTargetLost(targetCondo, null);
		interact.DefaultState(interact.pre).OnTargetLost(targetCondo, null).Enter(delegate(Instance smi)
		{
			SetFacing(smi);
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		})
			.Exit(delegate(Instance smi)
			{
				smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
			})
			.ToggleMainStatusItem((Instance smi) => GetTargetCondo(smi).def.interactStatusItem);
		interact.pre.PlayAnim("cc_working_pre").Enter(delegate(Instance smi)
		{
			PlayCondoBuildingAnim(smi, "cc_working_pre");
		}).OnAnimQueueComplete(interact.loop);
		interact.loop.PlayAnim("cc_working").Enter(delegate(Instance smi)
		{
			PlayCondoBuildingAnim(smi, smi.def.working_anim);
		}).OnAnimQueueComplete(interact.pst);
		interact.pst.PlayAnim("cc_working_pst").Enter(delegate(Instance smi)
		{
			PlayCondoBuildingAnim(smi, "cc_working_pst");
		}).OnAnimQueueComplete(behaviourComplete);
		behaviourComplete.BehaviourComplete(GameTags.Creatures.Behaviour_InteractWithCritterCondo).Exit(ApplyEffects);
	}

	private void SetFacing(Instance smi)
	{
		bool isRotated = GetTargetCondo(smi).Get<Rotatable>().IsRotated;
		smi.Get<Facing>().SetFacing(isRotated);
	}

	private static CritterCondo.Instance GetTargetCondo(Instance smi)
	{
		GameObject gameObject = smi.sm.targetCondo.Get(smi);
		CritterCondo.Instance instance = ((gameObject != null) ? gameObject.GetSMI<CritterCondo.Instance>() : null);
		if (instance.IsNullOrStopped())
		{
			return null;
		}
		return instance;
	}

	private static void ReserveCondo(Instance smi)
	{
		CritterCondo.Instance instance = smi.GetSMI<CritterCondoInteractMontior.Instance>().targetCondo;
		if (instance != null)
		{
			smi.sm.targetCondo.Set(instance.gameObject, smi);
			instance.SetReserved(isReserved: true);
		}
	}

	private static void UnreserveCondo(Instance smi)
	{
		CritterCondo.Instance instance = GetTargetCondo(smi);
		if (instance != null)
		{
			instance.GetComponent<KBatchedAnimController>().Play("on", KAnim.PlayMode.Loop);
			smi.sm.targetCondo.Set(null, smi);
			instance.SetReserved(isReserved: false);
		}
	}

	private static int GetCondoInteractCell(Instance smi)
	{
		return GetTargetCondo(smi)?.GetInteractStartCell() ?? Grid.InvalidCell;
	}

	private static void ApplyEffects(Instance smi)
	{
		smi.Get<Effects>().Add(GetTargetCondo(smi).def.effectId, should_save: true);
	}

	private static void PlayCondoBuildingAnim(Instance smi, string anim_name)
	{
		if (smi.def.entersBuilding)
		{
			smi.sm.targetCondo.Get<KBatchedAnimController>(smi).Play(anim_name);
		}
	}
}
