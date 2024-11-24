using System;
using UnityEngine;

// Token: 0x02000644 RID: 1604
public class RobotAi : GameStateMachine<RobotAi, RobotAi.Instance>
{
	// Token: 0x06001D3D RID: 7485 RVA: 0x001AE7D8 File Offset: 0x001AC9D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RobotAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RobotAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.DefaultState(this.alive.normal).TagTransition(GameTags.Dead, this.dead, false).Toggle("Toggle Component Registration", delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, true);
		}, delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, false);
		});
		this.alive.normal.TagTransition(GameTags.Stored, this.alive.stored, false).Enter(delegate(RobotAi.Instance smi)
		{
			if (!smi.HasTag(GameTags.Robots.Models.FetchDrone))
			{
				smi.fallMonitor = new FallMonitor.Instance(smi.master, false, null);
				smi.fallMonitor.StartSM();
			}
		}).Exit(delegate(RobotAi.Instance smi)
		{
			if (smi.fallMonitor != null)
			{
				smi.fallMonitor.StopSM("StoredRobotAI");
			}
		});
		this.alive.stored.PlayAnim("in_storage").TagTransition(GameTags.Stored, this.alive.normal, true).ToggleBrain("stored").Enter(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Pause("stored");
		}).Exit(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Unpause("unstored");
		});
		this.dead.ToggleBrain("dead").ToggleComponentIfFound<Deconstructable>(false).ToggleStateMachine((RobotAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).Enter("RefreshUserMenu", delegate(RobotAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		}).Enter("Delete", new StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback(RobotAi.DeleteOnDeath));
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x000B344C File Offset: 0x000B164C
	public static void DeleteOnDeath(RobotAi.Instance smi)
	{
		if (((RobotAi.Def)smi.def).DeleteOnDead)
		{
			smi.gameObject.DeleteObject();
		}
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x000B346B File Offset: 0x000B166B
	private static void ToggleRegistration(RobotAi.Instance smi, bool register)
	{
		if (register)
		{
			Components.LiveRobotsIdentities.Add(smi);
			return;
		}
		Components.LiveRobotsIdentities.Remove(smi);
	}

	// Token: 0x04001230 RID: 4656
	public RobotAi.AliveStates alive;

	// Token: 0x04001231 RID: 4657
	public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x02000645 RID: 1605
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001232 RID: 4658
		public bool DeleteOnDead;
	}

	// Token: 0x02000646 RID: 1606
	public class AliveStates : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001233 RID: 4659
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04001234 RID: 4660
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State stored;
	}

	// Token: 0x02000647 RID: 1607
	public new class Instance : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06001D44 RID: 7492 RVA: 0x001AEA24 File Offset: 0x001ACC24
		public Instance(IStateMachineTarget master, RobotAi.Def def) : base(master, def)
		{
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			base.Subscribe(-1988963660, new Action<object>(this.OnBeginChore));
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x001AEA80 File Offset: 0x001ACC80
		private void OnBeginChore(object data)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.DropAll(false, false, default(Vector3), true, null);
			}
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000B34BF File Offset: 0x000B16BF
		protected override void OnCleanUp()
		{
			base.Unsubscribe(-1988963660, new Action<object>(this.OnBeginChore));
			base.OnCleanUp();
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000B34DE File Offset: 0x000B16DE
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x04001235 RID: 4661
		public FallMonitor.Instance fallMonitor;
	}
}
