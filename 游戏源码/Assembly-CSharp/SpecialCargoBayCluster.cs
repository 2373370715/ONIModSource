using System.Collections.Generic;
using UnityEngine;

public class SpecialCargoBayCluster : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
	public class Def : BaseDef
	{
		public Vector2 trappedOffset = new Vector2(0f, -0.3f);
	}

	public class OpenStates : State
	{
		public State opening;

		public State idle;
	}

	public class CloseStates : State
	{
		public State closing;

		public State idle;

		public State cloud;
	}

	public new class Instance : GameInstance
	{
		public MeterController doorMeter;

		private Storage critterStorage;

		private Storage sideProductStorage;

		private KBatchedAnimController buildingAnimController;

		private KBatchedAnimController doorAnimController;

		[MyCmpGet]
		private RocketModuleCluster rocketModuleCluster;

		public void PlayDeathCloud()
		{
			if (IsInsideState(base.sm.close.idle))
			{
				GoTo(base.sm.close.cloud);
			}
		}

		public void CloseDoor()
		{
			base.sm.IsDoorOpen.Set(value: false, base.smi);
		}

		public void OpenDoor()
		{
			base.sm.IsDoorOpen.Set(value: true, base.smi);
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			buildingAnimController = GetComponent<KBatchedAnimController>();
			doorMeter = new MeterController(buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront);
			doorAnimController = doorMeter.meterController;
			KBatchedAnimTracker componentInChildren = doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			base.sm.Door.Set(doorAnimController.gameObject, base.smi);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			critterStorage = components[0];
			sideProductStorage = components[1];
			Subscribe(1655598572, OnLaunchConditionChanged);
		}

		public void CloseDoorAutomatically()
		{
			CloseDoor();
		}

		public override void StartSM()
		{
			base.StartSM();
		}

		private void OnLaunchConditionChanged(object obj)
		{
			if (rocketModuleCluster.CraftInterface != null)
			{
				Clustercraft component = rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
				if (component != null && component.Status == Clustercraft.CraftStatus.Launching)
				{
					CloseDoor();
				}
			}
		}

		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (GameObject item in critterStorage.items)
			{
				if (item != null)
				{
					Baggable component = item.GetComponent<Baggable>();
					if (component != null)
					{
						component.keepWrangledNextTimeRemovedFromStorage = true;
					}
				}
			}
			critterStorage.DropAll(vent_gas: false, dump_liquid: false, default(Vector3), do_disease_transfer: true, list);
			sideProductStorage.DropAll(vent_gas: false, dump_liquid: false, default(Vector3), do_disease_transfer: true, list2);
			foreach (GameObject item2 in list)
			{
				KBatchedAnimController component2 = item2.GetComponent<KBatchedAnimController>();
				TransformExtensions.SetPosition(position: GetStorePositionForCritter(item2), transform: item2.transform);
				component2.SetSceneLayer(Grid.SceneLayer.Creatures);
				component2.Play("trussed", KAnim.PlayMode.Loop);
			}
			foreach (GameObject item3 in list2)
			{
				KBatchedAnimController component3 = item3.GetComponent<KBatchedAnimController>();
				TransformExtensions.SetPosition(position: GetStorePositionForDrops(), transform: item3.transform);
				component3.SetSceneLayer(Grid.SceneLayer.Ore);
			}
		}

		public Vector3 GetCritterPositionOffet(GameObject critter)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			Vector3 zero = Vector3.zero;
			zero.x = base.def.trappedOffset.x - component.Offset.x;
			zero.y = base.def.trappedOffset.y - component.Offset.y;
			return zero;
		}

		public Vector3 GetStorePositionForCritter(GameObject critter)
		{
			Vector3 critterPositionOffet = GetCritterPositionOffet(critter);
			bool symbolVisible;
			return (Vector3)buildingAnimController.GetSymbolTransform("critter", out symbolVisible).GetColumn(3) + critterPositionOffet;
		}

		public Vector3 GetStorePositionForDrops()
		{
			bool symbolVisible;
			return buildingAnimController.GetSymbolTransform("loot", out symbolVisible).GetColumn(3);
		}
	}

	public const string DOOR_METER_TARGET_NAME = "fg_meter_target";

	public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";

	public const string LOOT_SYMBOL_NAME = "loot";

	public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";

	private const string OPEN_DOOR_ANIM_NAME = "open";

	private const string CLOSE_DOOR_ANIM_NAME = "close";

	private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";

	private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";

	public OpenStates open;

	public CloseStates close;

	public BoolParameter IsDoorOpen;

	public TargetParameter Door;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = close;
		close.DefaultState(close.idle);
		close.closing.Target(Door).PlayAnim("close").OnAnimQueueComplete(close.idle)
			.Target(masterTarget);
		close.idle.Target(Door).PlayAnim("close_idle").ParamTransition(IsDoorOpen, open.opening, GameStateMachine<SpecialCargoBayCluster, Instance, IStateMachineTarget, Def>.IsTrue)
			.Target(masterTarget);
		close.cloud.Target(Door).PlayAnim("play_cloud").OnAnimQueueComplete(close.idle)
			.Target(masterTarget);
		open.DefaultState(close.idle);
		open.opening.Target(Door).PlayAnim("open").OnAnimQueueComplete(open.idle)
			.Target(masterTarget);
		open.idle.Target(Door).PlayAnim("open_idle").Enter(DropInventory)
			.Enter(CloseDoorAutomatically)
			.ParamTransition(IsDoorOpen, close.closing, GameStateMachine<SpecialCargoBayCluster, Instance, IStateMachineTarget, Def>.IsFalse)
			.Target(masterTarget);
	}

	public static void CloseDoorAutomatically(Instance smi)
	{
		smi.CloseDoorAutomatically();
	}

	public static void DropInventory(Instance smi)
	{
		smi.DropInventory();
	}
}
