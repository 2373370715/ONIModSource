using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000EE7 RID: 3815
public class OilChanger : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>
{
	// Token: 0x06004CF7 RID: 19703 RVA: 0x00263EA8 File Offset: 0x002620A8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inoperational;
		this.inoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.operational.oilNeeded);
		this.operational.oilNeeded.ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.Transition.ConditionCallback(OilChanger.HasEnoughLubricant));
		this.operational.ready.ToggleChore(new Func<OilChanger.Instance, Chore>(OilChanger.CreateChore), this.operational.oilNeeded);
	}

	// Token: 0x06004CF8 RID: 19704 RVA: 0x000D1ECD File Offset: 0x000D00CD
	public static bool HasEnoughLubricant(OilChanger.Instance smi)
	{
		return smi.OilAmount >= smi.def.MIN_LUBRICANT_MASS_TO_WORK;
	}

	// Token: 0x06004CF9 RID: 19705 RVA: 0x000D1EE5 File Offset: 0x000D00E5
	private static bool IsOperational(OilChanger.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06004CFA RID: 19706 RVA: 0x00263F70 File Offset: 0x00262170
	private static WorkChore<OilChangerWorkableUse> CreateChore(OilChanger.Instance smi)
	{
		return new WorkChore<OilChangerWorkableUse>(Db.Get().ChoreTypes.OilChange, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
	}

	// Token: 0x0400357D RID: 13693
	public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State inoperational;

	// Token: 0x0400357E RID: 13694
	public OilChanger.OperationalStates operational;

	// Token: 0x02000EE8 RID: 3816
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400357F RID: 13695
		public float MIN_LUBRICANT_MASS_TO_WORK = 200f;
	}

	// Token: 0x02000EE9 RID: 3817
	public class OperationalStates : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State
	{
		// Token: 0x04003580 RID: 13696
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State oilNeeded;

		// Token: 0x04003581 RID: 13697
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State ready;
	}

	// Token: 0x02000EEA RID: 3818
	public new class Instance : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.GameInstance, IFetchList
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06004CFE RID: 19710 RVA: 0x000D1F10 File Offset: 0x000D0110
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x000D1F1D File Offset: 0x000D011D
		public float OilAmount
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.LubricatingOil);
			}
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x00263FA8 File Offset: 0x002621A8
		public Instance(IStateMachineTarget master, OilChanger.Def def)
		{
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			Tag lubricatingOil = GameTags.LubricatingOil;
			dictionary[lubricatingOil] = 0f;
			this.remainingLubricationMass = dictionary;
			base..ctor(master, def);
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x000D1F2F File Offset: 0x000D012F
		public Storage Destination
		{
			get
			{
				return this.storage;
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x000D1F37 File Offset: 0x000D0137
		public float GetMinimumAmount(Tag tag)
		{
			return base.def.MIN_LUBRICANT_MASS_TO_WORK;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x000D1F44 File Offset: 0x000D0144
		public Dictionary<Tag, float> GetRemaining()
		{
			this.remainingLubricationMass[GameTags.LubricatingOil] = Mathf.Clamp(base.def.MIN_LUBRICANT_MASS_TO_WORK - this.OilAmount, 0f, base.def.MIN_LUBRICANT_MASS_TO_WORK);
			return this.remainingLubricationMass;
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public Dictionary<Tag, float> GetRemainingMinimum()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04003582 RID: 13698
		private Storage storage;

		// Token: 0x04003583 RID: 13699
		private Operational operational;

		// Token: 0x04003584 RID: 13700
		private Dictionary<Tag, float> remainingLubricationMass;
	}
}
