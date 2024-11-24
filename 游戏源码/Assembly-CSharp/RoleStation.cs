using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017E6 RID: 6118
[AddComponentMenu("KMonoBehaviour/Workable/RoleStation")]
public class RoleStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06007DD0 RID: 32208 RVA: 0x000F2E6F File Offset: 0x000F106F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = true;
		this.UpdateStatusItemDelegate = new Action<object>(this.UpdateSkillPointAvailableStatusItem);
	}

	// Token: 0x06007DD1 RID: 32209 RVA: 0x0032817C File Offset: 0x0032637C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RoleStations.Add(this);
		this.smi = new RoleStation.RoleStationSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(7.53f);
		this.resetProgressOnStop = true;
		this.subscriptions.Add(Game.Instance.Subscribe(-1523247426, this.UpdateStatusItemDelegate));
		this.subscriptions.Add(Game.Instance.Subscribe(1505456302, this.UpdateStatusItemDelegate));
		this.UpdateSkillPointAvailableStatusItem(null);
	}

	// Token: 0x06007DD2 RID: 32210 RVA: 0x0032820C File Offset: 0x0032640C
	protected override void OnStopWork(WorkerBase worker)
	{
		Telepad.StatesInstance statesInstance = this.GetSMI<Telepad.StatesInstance>();
		statesInstance.sm.idlePortal.Trigger(statesInstance);
	}

	// Token: 0x06007DD3 RID: 32211 RVA: 0x00328234 File Offset: 0x00326434
	private void UpdateSkillPointAvailableStatusItem(object data = null)
	{
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				if (this.skillPointAvailableStatusItem == Guid.Empty)
				{
					this.skillPointAvailableStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, null);
				}
				return;
			}
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, false);
		this.skillPointAvailableStatusItem = Guid.Empty;
	}

	// Token: 0x06007DD4 RID: 32212 RVA: 0x00328300 File Offset: 0x00326500
	private Chore CreateWorkChore()
	{
		return new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
	}

	// Token: 0x06007DD5 RID: 32213 RVA: 0x000F2E90 File Offset: 0x000F1090
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<MinionResume>().SkillLearned();
	}

	// Token: 0x06007DD6 RID: 32214 RVA: 0x000F2EA4 File Offset: 0x000F10A4
	private void OnSelectRolesClick()
	{
		DetailsScreen.Instance.Show(false);
		ManagementMenu.Instance.ToggleSkills();
	}

	// Token: 0x06007DD7 RID: 32215 RVA: 0x00328344 File Offset: 0x00326544
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.subscriptions)
		{
			Game.Instance.Unsubscribe(id);
		}
		Components.RoleStations.Remove(this);
	}

	// Token: 0x06007DD8 RID: 32216 RVA: 0x000F2EBB File Offset: 0x000F10BB
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		return base.GetDescriptors(go);
	}

	// Token: 0x04005F51 RID: 24401
	private Chore chore;

	// Token: 0x04005F52 RID: 24402
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04005F53 RID: 24403
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x04005F54 RID: 24404
	private RoleStation.RoleStationSM.Instance smi;

	// Token: 0x04005F55 RID: 24405
	private Guid skillPointAvailableStatusItem;

	// Token: 0x04005F56 RID: 24406
	private Action<object> UpdateStatusItemDelegate;

	// Token: 0x04005F57 RID: 24407
	private List<int> subscriptions = new List<int>();

	// Token: 0x020017E7 RID: 6119
	public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
	{
		// Token: 0x06007DDA RID: 32218 RVA: 0x003283AC File Offset: 0x003265AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RoleStation.RoleStationSM.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.operational.ToggleChore((RoleStation.RoleStationSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x04005F58 RID: 24408
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;

		// Token: 0x04005F59 RID: 24409
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

		// Token: 0x020017E8 RID: 6120
		public new class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
		{
			// Token: 0x06007DDC RID: 32220 RVA: 0x000F2EDF File Offset: 0x000F10DF
			public Instance(RoleStation master) : base(master)
			{
			}
		}
	}
}
