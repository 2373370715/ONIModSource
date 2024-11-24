using System;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class BeIncapacitatedChore : Chore<BeIncapacitatedChore.StatesInstance>
{
	// Token: 0x06001DB8 RID: 7608 RVA: 0x001B0518 File Offset: 0x001AE718
	public void FindAvailableMedicalBed(Navigator navigator)
	{
		Clinic clinic = null;
		AssignableSlot clinic2 = Db.Get().AssignableSlots.Clinic;
		Ownables soleOwner = this.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
		AssignableSlotInstance slot = soleOwner.GetSlot(clinic2);
		if (slot.assignable == null)
		{
			Assignable assignable = soleOwner.AutoAssignSlot(clinic2);
			if (assignable != null)
			{
				clinic = assignable.GetComponent<Clinic>();
			}
		}
		else
		{
			clinic = slot.assignable.GetComponent<Clinic>();
		}
		if (clinic != null && navigator.CanReach(clinic))
		{
			base.smi.sm.clinic.Set(clinic.gameObject, base.smi, false);
			base.smi.GoTo(base.smi.sm.incapacitation_root.rescue.waitingForPickup);
		}
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x000B39EF File Offset: 0x000B1BEF
	public GameObject GetChosenClinic()
	{
		return base.smi.sm.clinic.Get(base.smi);
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x001B05E4 File Offset: 0x001AE7E4
	public BeIncapacitatedChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeIncapacitated, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeIncapacitatedChore.StatesInstance(this);
	}

	// Token: 0x04001298 RID: 4760
	private static string IncapacitatedDuplicantAnim_pre = "incapacitate_pre";

	// Token: 0x04001299 RID: 4761
	private static string IncapacitatedDuplicantAnim_loop = "incapacitate_loop";

	// Token: 0x0400129A RID: 4762
	private static string IncapacitatedDuplicantAnim_death = "incapacitate_death";

	// Token: 0x0400129B RID: 4763
	private static string IncapacitatedDuplicantAnim_carry = "carry_loop";

	// Token: 0x0400129C RID: 4764
	private static string IncapacitatedDuplicantAnim_place = "place";

	// Token: 0x02000663 RID: 1635
	public class StatesInstance : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.GameInstance
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x000B3A40 File Offset: 0x000B1C40
		public StatesInstance(BeIncapacitatedChore master) : base(master)
		{
		}
	}

	// Token: 0x02000664 RID: 1636
	public class States : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore>
	{
		// Token: 0x06001DBD RID: 7613 RVA: 0x001B0628 File Offset: 0x001AE828
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_incapacitated_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.Incapacitated, (BeIncapacitatedChore.StatesInstance smi) => smi.master.gameObject.GetSMI<IncapacitationMonitor.Instance>()).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.GoTo(this.incapacitation_root.lookingForBed);
			});
			this.incapacitation_root.EventHandler(GameHashes.Died, delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("died");
			});
			this.incapacitation_root.lookingForBed.Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				smi.master.FindAvailableMedicalBed(smi.master.GetComponent<Navigator>());
			}, UpdateRate.SIM_1000ms, false).Enter("PlayAnim", delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.sm.clinic.Set(null, smi);
				smi.Play(BeIncapacitatedChore.IncapacitatedDuplicantAnim_pre, KAnim.PlayMode.Once);
				smi.Queue(BeIncapacitatedChore.IncapacitatedDuplicantAnim_loop, KAnim.PlayMode.Loop);
			});
			this.incapacitation_root.rescue.ToggleChore((BeIncapacitatedChore.StatesInstance smi) => new RescueIncapacitatedChore(smi.master, this.masterTarget.Get(smi)), this.incapacitation_root.recovering, this.incapacitation_root.lookingForBed);
			this.incapacitation_root.rescue.waitingForPickup.EventTransition(GameHashes.OnStore, this.incapacitation_root.rescue.carried, null).Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				bool flag = false;
				if (smi.sm.clinic.Get(smi) == null)
				{
					flag = true;
				}
				else if (!smi.master.gameObject.GetComponent<Navigator>().CanReach(this.clinic.Get(smi).GetComponent<Clinic>()))
				{
					flag = true;
				}
				else if (!this.clinic.Get(smi).GetComponent<Assignable>().IsAssignedTo(smi.master.GetComponent<IAssignableIdentity>()))
				{
					flag = true;
				}
				if (flag)
				{
					smi.GoTo(this.incapacitation_root.lookingForBed);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.incapacitation_root.rescue.carried.Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				bool flag = false;
				if (smi.sm.clinic.Get(smi) == null)
				{
					flag = true;
				}
				else if (!this.clinic.Get(smi).GetComponent<Assignable>().IsAssignedTo(smi.master.GetComponent<IAssignableIdentity>()))
				{
					flag = true;
				}
				if (flag)
				{
					smi.GoTo(this.incapacitation_root.lookingForBed);
				}
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Queue(BeIncapacitatedChore.IncapacitatedDuplicantAnim_carry, KAnim.PlayMode.Loop);
			}).Exit(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Play(BeIncapacitatedChore.IncapacitatedDuplicantAnim_place, KAnim.PlayMode.Once);
			});
			this.incapacitation_root.death.PlayAnim(BeIncapacitatedChore.IncapacitatedDuplicantAnim_death).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("died");
			});
			this.incapacitation_root.recovering.ToggleUrge(Db.Get().Urges.HealCritical).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Trigger(-1256572400, null);
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("recovering");
			});
		}

		// Token: 0x0400129D RID: 4765
		public BeIncapacitatedChore.States.IncapacitatedStates incapacitation_root;

		// Token: 0x0400129E RID: 4766
		public StateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.TargetParameter clinic;

		// Token: 0x02000665 RID: 1637
		public class IncapacitatedStates : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State
		{
			// Token: 0x0400129F RID: 4767
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State lookingForBed;

			// Token: 0x040012A0 RID: 4768
			public BeIncapacitatedChore.States.BeingRescued rescue;

			// Token: 0x040012A1 RID: 4769
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State death;

			// Token: 0x040012A2 RID: 4770
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State recovering;
		}

		// Token: 0x02000666 RID: 1638
		public class BeingRescued : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State
		{
			// Token: 0x040012A3 RID: 4771
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State waitingForPickup;

			// Token: 0x040012A4 RID: 4772
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State carried;
		}
	}
}
