using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001057 RID: 4183
public class CargoDropperMinion : GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>
{
	// Token: 0x0600555E RID: 21854 RVA: 0x0027E058 File Offset: 0x0027C258
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.notLanded;
		this.root.ParamTransition<bool>(this.hasLanded, this.complete, GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.IsTrue);
		this.notLanded.EventHandlerTransition(GameHashes.JettisonCargo, this.landed, (CargoDropperMinion.StatesInstance smi, object obj) => true);
		this.landed.Enter(delegate(CargoDropperMinion.StatesInstance smi)
		{
			smi.JettisonCargo(null);
			smi.GoTo(this.exiting);
		});
		this.exiting.Update(delegate(CargoDropperMinion.StatesInstance smi, float dt)
		{
			if (!smi.SyncMinionExitAnimation())
			{
				smi.GoTo(this.complete);
			}
		}, UpdateRate.SIM_200ms, false);
		this.complete.Enter(delegate(CargoDropperMinion.StatesInstance smi)
		{
			this.hasLanded.Set(true, smi, false);
		});
	}

	// Token: 0x04003BE0 RID: 15328
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State notLanded;

	// Token: 0x04003BE1 RID: 15329
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State landed;

	// Token: 0x04003BE2 RID: 15330
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State exiting;

	// Token: 0x04003BE3 RID: 15331
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State complete;

	// Token: 0x04003BE4 RID: 15332
	public StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter hasLanded = new StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter(false);

	// Token: 0x02001058 RID: 4184
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003BE5 RID: 15333
		public Vector3 dropOffset;

		// Token: 0x04003BE6 RID: 15334
		public string kAnimName;

		// Token: 0x04003BE7 RID: 15335
		public string animName;

		// Token: 0x04003BE8 RID: 15336
		public Grid.SceneLayer animLayer = Grid.SceneLayer.Move;

		// Token: 0x04003BE9 RID: 15337
		public bool notifyOnJettison;
	}

	// Token: 0x02001059 RID: 4185
	public class StatesInstance : GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.GameInstance
	{
		// Token: 0x06005564 RID: 21860 RVA: 0x000D7BE4 File Offset: 0x000D5DE4
		public StatesInstance(IStateMachineTarget master, CargoDropperMinion.Def def) : base(master, def)
		{
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x0027E114 File Offset: 0x0027C314
		public void JettisonCargo(object data = null)
		{
			Vector3 pos = base.master.transform.GetPosition() + base.def.dropOffset;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
				for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
				{
					MinionStorage.Info info = storedMinionInfo[i];
					GameObject gameObject = component.DeserializeMinion(info.id, pos);
					this.escapingMinion = gameObject.GetComponent<MinionIdentity>();
					gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						this.exitAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, base.def.kAnimName, new HashedString[]
						{
							base.def.animName
						}, KAnim.PlayMode.Once, false);
						Vector3 position = gameObject.transform.GetPosition();
						position.z = Grid.GetLayerZ(base.def.animLayer);
						gameObject.transform.SetPosition(position);
						gameObject.GetMyWorld().SetDupeVisited();
					}
					if (base.def.notifyOnJettison)
					{
						gameObject.GetComponent<Notifier>().Add(this.CreateCrashLandedNotification(), "");
					}
				}
			}
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x0027E270 File Offset: 0x0027C470
		public bool SyncMinionExitAnimation()
		{
			if (this.escapingMinion != null && this.exitAnimChore != null && !this.exitAnimChore.isComplete)
			{
				KBatchedAnimController component = this.escapingMinion.GetComponent<KBatchedAnimController>();
				KBatchedAnimController component2 = base.master.GetComponent<KBatchedAnimController>();
				if (component2.CurrentAnim.name == base.def.animName)
				{
					component.SetElapsedTime(component2.GetElapsedTime());
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x0027E2E4 File Offset: 0x0027C4E4
		public Notification CreateCrashLandedNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x04003BEA RID: 15338
		public MinionIdentity escapingMinion;

		// Token: 0x04003BEB RID: 15339
		public Chore exitAnimChore;
	}
}
