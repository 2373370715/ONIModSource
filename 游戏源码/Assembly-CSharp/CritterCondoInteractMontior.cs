using UnityEngine;

public class CritterCondoInteractMontior : GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>
{
	public class Def : BaseDef
	{
		public bool requireCavity = true;

		public Tag condoPrefabTag = "CritterCondo";
	}

	public new class Instance : GameInstance
	{
		public CritterCondo.Instance targetCondo;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}
	}

	public State lookingForCondo;

	public State satisfied;

	private FloatParameter remainingSecondsForEffect;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = lookingForCondo;
		base.serializable = SerializeType.ParamsOnly;
		root.ParamTransition(remainingSecondsForEffect, satisfied, (Instance smi, float val) => val > 0f);
		lookingForCondo.PreBrainUpdate(FindCondoTarget).ToggleBehaviour(GameTags.Creatures.Behaviour_InteractWithCritterCondo, (Instance smi) => !smi.targetCondo.IsNullOrStopped() && !smi.targetCondo.IsReserved(), delegate(Instance smi)
		{
			smi.GoTo(satisfied);
		});
		satisfied.Enter(delegate(Instance smi)
		{
			remainingSecondsForEffect.Set(600f, smi);
		}).ScheduleGoTo((Instance smi) => remainingSecondsForEffect.Get(smi), lookingForCondo);
	}

	private static void FindCondoTarget(Instance smi)
	{
		using ListPool<CritterCondo.Instance, CritterCondoInteractMontior>.PooledList pooledList = PoolsFor<CritterCondoInteractMontior>.AllocateList<CritterCondo.Instance>();
		if (!smi.def.requireCavity)
		{
			Vector3 position = smi.gameObject.transform.GetPosition();
			foreach (CritterCondo.Instance item in Components.CritterCondos.GetItems(smi.GetMyWorldId()))
			{
				if (!item.IsNullOrDestroyed() && !(item.def.condoTag != smi.def.condoPrefabTag) && !((item.transform.GetPosition() - position).sqrMagnitude > 256f) && item.CanBeReserved())
				{
					pooledList.Add(item);
				}
			}
		}
		else
		{
			int cell = Grid.PosToCell(smi.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				foreach (KPrefabID building in cavityForCell.buildings)
				{
					if (!building.IsNullOrDestroyed())
					{
						CritterCondo.Instance sMI = building.GetSMI<CritterCondo.Instance>();
						if (sMI != null && building.HasTag(smi.def.condoPrefabTag) && sMI.CanBeReserved())
						{
							pooledList.Add(sMI);
						}
					}
				}
			}
		}
		Navigator component = smi.GetComponent<Navigator>();
		int num = -1;
		foreach (CritterCondo.Instance item2 in pooledList)
		{
			int interactStartCell = item2.GetInteractStartCell();
			int navigationCost = component.GetNavigationCost(interactStartCell);
			if (navigationCost != -1 && (navigationCost < num || num == -1))
			{
				num = navigationCost;
				smi.targetCondo = item2;
			}
		}
	}
}
