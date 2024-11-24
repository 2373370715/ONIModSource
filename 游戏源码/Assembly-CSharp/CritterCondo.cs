using System;
using System.Collections.Generic;
using UnityEngine;

public class CritterCondo : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public Func<Instance, bool> IsCritterCondoOperationalCb;

		public StatusItem moveToStatusItem;

		public StatusItem interactStatusItem;

		public Tag condoTag = "CritterCondo";

		public string effectId;

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}
	}

	public new class Instance : GameInstance
	{
		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			Components.CritterCondos.Add(base.smi.GetMyWorldId(), this);
		}

		protected override void OnCleanUp()
		{
			Components.CritterCondos.Remove(base.smi.GetMyWorldId(), this);
		}

		public bool IsReserved()
		{
			return HasTag(GameTags.Creatures.ReservedByCreature);
		}

		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, set: true);
			}
			else if (HasTag(GameTags.Creatures.ReservedByCreature))
			{
				GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
			}
			else
			{
				Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a condo that wasn't reserved");
			}
		}

		public int GetInteractStartCell()
		{
			return Grid.PosToCell(this);
		}

		public bool CanBeReserved()
		{
			if (!IsReserved())
			{
				return IsOperational(this);
			}
			return false;
		}
	}

	public State inoperational;

	public State operational;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = inoperational;
		inoperational.PlayAnim("off").EventTransition(GameHashes.UpdateRoom, operational, IsOperational).EventTransition(GameHashes.OperationalChanged, operational, IsOperational);
		operational.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.UpdateRoom, inoperational, GameStateMachine<CritterCondo, Instance, IStateMachineTarget, Def>.Not(IsOperational)).EventTransition(GameHashes.OperationalChanged, inoperational, GameStateMachine<CritterCondo, Instance, IStateMachineTarget, Def>.Not(IsOperational));
	}

	private static bool IsOperational(Instance smi)
	{
		return smi.def.IsCritterCondoOperationalCb(smi);
	}
}
