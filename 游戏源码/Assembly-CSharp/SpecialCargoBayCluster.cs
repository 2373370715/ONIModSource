using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059D RID: 1437
public class SpecialCargoBayCluster : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
	// Token: 0x06001987 RID: 6535 RVA: 0x001A3548 File Offset: 0x001A1748
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.close;
		this.close.DefaultState(this.close.idle);
		this.close.closing.Target(this.Door).PlayAnim("close").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.close.idle.Target(this.Door).PlayAnim("close_idle").ParamTransition<bool>(this.IsDoorOpen, this.open.opening, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsTrue).Target(this.masterTarget);
		this.close.cloud.Target(this.Door).PlayAnim("play_cloud").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.open.DefaultState(this.close.idle);
		this.open.opening.Target(this.Door).PlayAnim("open").OnAnimQueueComplete(this.open.idle).Target(this.masterTarget);
		this.open.idle.Target(this.Door).PlayAnim("open_idle").Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.DropInventory)).Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.CloseDoorAutomatically)).ParamTransition<bool>(this.IsDoorOpen, this.close.closing, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsFalse).Target(this.masterTarget);
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x000B0C0A File Offset: 0x000AEE0A
	public static void CloseDoorAutomatically(SpecialCargoBayCluster.Instance smi)
	{
		smi.CloseDoorAutomatically();
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x000B0C12 File Offset: 0x000AEE12
	public static void DropInventory(SpecialCargoBayCluster.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x0400103D RID: 4157
	public const string DOOR_METER_TARGET_NAME = "fg_meter_target";

	// Token: 0x0400103E RID: 4158
	public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";

	// Token: 0x0400103F RID: 4159
	public const string LOOT_SYMBOL_NAME = "loot";

	// Token: 0x04001040 RID: 4160
	public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";

	// Token: 0x04001041 RID: 4161
	private const string OPEN_DOOR_ANIM_NAME = "open";

	// Token: 0x04001042 RID: 4162
	private const string CLOSE_DOOR_ANIM_NAME = "close";

	// Token: 0x04001043 RID: 4163
	private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";

	// Token: 0x04001044 RID: 4164
	private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";

	// Token: 0x04001045 RID: 4165
	public SpecialCargoBayCluster.OpenStates open;

	// Token: 0x04001046 RID: 4166
	public SpecialCargoBayCluster.CloseStates close;

	// Token: 0x04001047 RID: 4167
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.BoolParameter IsDoorOpen;

	// Token: 0x04001048 RID: 4168
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.TargetParameter Door;

	// Token: 0x0200059E RID: 1438
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001049 RID: 4169
		public Vector2 trappedOffset = new Vector2(0f, -0.3f);
	}

	// Token: 0x0200059F RID: 1439
	public class OpenStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x0400104A RID: 4170
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State opening;

		// Token: 0x0400104B RID: 4171
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
	}

	// Token: 0x020005A0 RID: 1440
	public class CloseStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x0400104C RID: 4172
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State closing;

		// Token: 0x0400104D RID: 4173
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;

		// Token: 0x0400104E RID: 4174
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State cloud;
	}

	// Token: 0x020005A1 RID: 1441
	public new class Instance : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.GameInstance
	{
		// Token: 0x0600198E RID: 6542 RVA: 0x000B0C47 File Offset: 0x000AEE47
		public void PlayDeathCloud()
		{
			if (base.IsInsideState(base.sm.close.idle))
			{
				this.GoTo(base.sm.close.cloud);
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x000B0C77 File Offset: 0x000AEE77
		public void CloseDoor()
		{
			base.sm.IsDoorOpen.Set(false, base.smi, false);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x000B0C92 File Offset: 0x000AEE92
		public void OpenDoor()
		{
			base.sm.IsDoorOpen.Set(true, base.smi, false);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x001A36F8 File Offset: 0x001A18F8
		public Instance(IStateMachineTarget master, SpecialCargoBayCluster.Def def) : base(master, def)
		{
			this.buildingAnimController = base.GetComponent<KBatchedAnimController>();
			this.doorMeter = new MeterController(this.buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.doorAnimController = this.doorMeter.meterController;
			KBatchedAnimTracker componentInChildren = this.doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			base.sm.Door.Set(this.doorAnimController.gameObject, base.smi, false);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.critterStorage = components[0];
			this.sideProductStorage = components[1];
			base.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x000B0CAD File Offset: 0x000AEEAD
		public void CloseDoorAutomatically()
		{
			this.CloseDoor();
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000B0CB5 File Offset: 0x000AEEB5
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x001A37C0 File Offset: 0x001A19C0
		private void OnLaunchConditionChanged(object obj)
		{
			if (this.rocketModuleCluster.CraftInterface != null)
			{
				Clustercraft component = this.rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
				if (component != null && component.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CloseDoor();
				}
			}
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x001A380C File Offset: 0x001A1A0C
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (GameObject gameObject in this.critterStorage.items)
			{
				if (gameObject != null)
				{
					Baggable component = gameObject.GetComponent<Baggable>();
					if (component != null)
					{
						component.keepWrangledNextTimeRemovedFromStorage = true;
					}
				}
			}
			Storage storage = this.critterStorage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			Storage storage2 = this.sideProductStorage;
			bool vent_gas2 = false;
			bool dump_liquid2 = false;
			collect_dropped_items = list2;
			storage2.DropAll(vent_gas2, dump_liquid2, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject2 in list)
			{
				KBatchedAnimController component2 = gameObject2.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForCritter = this.GetStorePositionForCritter(gameObject2);
				gameObject2.transform.SetPosition(storePositionForCritter);
				component2.SetSceneLayer(Grid.SceneLayer.Creatures);
				component2.Play("trussed", KAnim.PlayMode.Loop, 1f, 0f);
			}
			foreach (GameObject gameObject3 in list2)
			{
				KBatchedAnimController component3 = gameObject3.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForDrops = this.GetStorePositionForDrops();
				gameObject3.transform.SetPosition(storePositionForDrops);
				component3.SetSceneLayer(Grid.SceneLayer.Ore);
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x001A399C File Offset: 0x001A1B9C
		public Vector3 GetCritterPositionOffet(GameObject critter)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			Vector3 zero = Vector3.zero;
			zero.x = base.def.trappedOffset.x - component.Offset.x;
			zero.y = base.def.trappedOffset.y - component.Offset.y;
			return zero;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x001A3A00 File Offset: 0x001A1C00
		public Vector3 GetStorePositionForCritter(GameObject critter)
		{
			Vector3 critterPositionOffet = this.GetCritterPositionOffet(critter);
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("critter", out flag).GetColumn(3) + critterPositionOffet;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x001A3A40 File Offset: 0x001A1C40
		public Vector3 GetStorePositionForDrops()
		{
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("loot", out flag).GetColumn(3);
		}

		// Token: 0x0400104F RID: 4175
		public MeterController doorMeter;

		// Token: 0x04001050 RID: 4176
		private Storage critterStorage;

		// Token: 0x04001051 RID: 4177
		private Storage sideProductStorage;

		// Token: 0x04001052 RID: 4178
		private KBatchedAnimController buildingAnimController;

		// Token: 0x04001053 RID: 4179
		private KBatchedAnimController doorAnimController;

		// Token: 0x04001054 RID: 4180
		[MyCmpGet]
		private RocketModuleCluster rocketModuleCluster;
	}
}
