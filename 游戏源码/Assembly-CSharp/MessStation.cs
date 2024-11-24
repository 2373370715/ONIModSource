using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000EA6 RID: 3750
[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06004B94 RID: 19348 RVA: 0x000D0C65 File Offset: 0x000CEE65
	protected override void OnPrefabInit()
	{
		this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.HasCaloriesOwnablePrecondition));
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x0025F268 File Offset: 0x0025D468
	private bool HasCaloriesOwnablePrecondition(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (Db.Get().Amounts.Calories.Lookup(minionIdentity) != null);
		}
		return result;
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x000D0CA2 File Offset: 0x000CEEA2
	protected override void OnCompleteWork(WorkerBase worker)
	{
		worker.GetWorkable().GetComponent<Edible>().CompleteWork(worker);
	}

	// Token: 0x06004B97 RID: 19351 RVA: 0x000D0CB5 File Offset: 0x000CEEB5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new MessStation.MessStationSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06004B98 RID: 19352 RVA: 0x0025F2A8 File Offset: 0x0025D4A8
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06004B99 RID: 19353 RVA: 0x000D0CD4 File Offset: 0x000CEED4
	public bool HasSalt
	{
		get
		{
			return this.smi.HasSalt;
		}
	}

	// Token: 0x04003467 RID: 13415
	[MyCmpGet]
	private Ownable ownable;

	// Token: 0x04003468 RID: 13416
	private MessStation.MessStationSM.Instance smi;

	// Token: 0x02000EA7 RID: 3751
	public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
	{
		// Token: 0x06004B9B RID: 19355 RVA: 0x0025F314 File Offset: 0x0025D514
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.salt.none;
			this.salt.none.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("off");
			this.salt.salty.Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating, null);
			this.eating.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).PlayAnim("off");
		}

		// Token: 0x04003469 RID: 13417
		public MessStation.MessStationSM.SaltState salt;

		// Token: 0x0400346A RID: 13418
		public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

		// Token: 0x02000EA8 RID: 3752
		public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
		{
			// Token: 0x0400346B RID: 13419
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;

			// Token: 0x0400346C RID: 13420
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
		}

		// Token: 0x02000EA9 RID: 3753
		public new class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
		{
			// Token: 0x06004B9E RID: 19358 RVA: 0x000D0CF1 File Offset: 0x000CEEF1
			public Instance(MessStation master) : base(master)
			{
				this.saltStorage = master.GetComponent<Storage>();
				this.assigned = master.GetComponent<Assignable>();
			}

			// Token: 0x1700042F RID: 1071
			// (get) Token: 0x06004B9F RID: 19359 RVA: 0x000D0D12 File Offset: 0x000CEF12
			public bool HasSalt
			{
				get
				{
					return this.saltStorage.Has(TableSaltConfig.ID.ToTag());
				}
			}

			// Token: 0x06004BA0 RID: 19360 RVA: 0x0025F43C File Offset: 0x0025D63C
			public bool IsEating()
			{
				if (this.assigned == null || this.assigned.assignee == null)
				{
					return false;
				}
				Ownables soleOwner = this.assigned.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return false;
				}
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject == null)
				{
					return false;
				}
				ChoreDriver component = targetGameObject.GetComponent<ChoreDriver>();
				return component != null && component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
			}

			// Token: 0x0400346D RID: 13421
			private Storage saltStorage;

			// Token: 0x0400346E RID: 13422
			private Assignable assigned;
		}
	}
}
