using System;
using UnityEngine;

// Token: 0x02000641 RID: 1601
public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
	// Token: 0x06001D31 RID: 7473 RVA: 0x001AE658 File Offset: 0x001AC858
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RationalAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RationalAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.TagTransition(GameTags.Dead, this.dead, false).ToggleStateMachineList(new Func<RationalAi.Instance, Func<RationalAi.Instance, StateMachine.Instance>[]>(RationalAi.GetStateMachinesToRunWhenAlive));
		this.dead.ToggleStateMachine((RationalAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).ToggleBrain("dead").Enter("RefreshUserMenu", delegate(RationalAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RationalAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		});
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x000B33C5 File Offset: 0x000B15C5
	public static Func<RationalAi.Instance, StateMachine.Instance>[] GetStateMachinesToRunWhenAlive(RationalAi.Instance smi)
	{
		return smi.stateMachinesToRunWhenAlive;
	}

	// Token: 0x04001227 RID: 4647
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;

	// Token: 0x04001228 RID: 4648
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x02000642 RID: 1602
	public new class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06001D35 RID: 7477 RVA: 0x001AE75C File Offset: 0x001AC95C
		public Instance(IStateMachineTarget master, Tag minionModel) : base(master)
		{
			this.MinionModel = minionModel;
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			component.prioritizeBrainIfNoChore = true;
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000B33FD File Offset: 0x000B15FD
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x04001229 RID: 4649
		public Tag MinionModel;

		// Token: 0x0400122A RID: 4650
		public Func<RationalAi.Instance, StateMachine.Instance>[] stateMachinesToRunWhenAlive;
	}
}
