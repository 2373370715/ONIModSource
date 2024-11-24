using System.Collections.Generic;
using UnityEngine;

public class MilkSeparator : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
	public class Def : BaseDef
	{
		public float MILK_FAT_CAPACITY = 100f;

		public Tag MILK_TAG;

		public Tag MILK_FAT_TAG;

		public Def()
		{
			MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
			MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		}
	}

	public class WorkingStates : State
	{
		public State pre;

		public State work;

		public State post;
	}

	public class OperationalStates : State
	{
		public State idle;

		public WorkingStates working;

		public State full;

		public State emptyComplete;
	}

	public new class Instance : GameInstance
	{
		[MyCmpGet]
		public EmptyMilkSeparatorWorkable workable;

		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		public ElementConverter elementConverter;

		[MyCmpGet]
		private Storage storage;

		private MeterController fatMeter;

		public float MilkFatStored => storage.GetAmountAvailable(base.def.MILK_FAT_TAG);

		public float MilkFatStoragePercentage => Mathf.Clamp(MilkFatStored / base.def.MILK_FAT_CAPACITY, 0f, 1f);

		public bool MilkFatLimitReached => MilkFatStored >= base.def.MILK_FAT_CAPACITY;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			KAnimControllerBase component = GetComponent<KBatchedAnimController>();
			fatMeter = new MeterController(component, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_target_1");
		}

		public override void StartSM()
		{
			base.StartSM();
			workable.OnWork_PST_Begins = Play_Empty_MeterAnimation;
			RefreshMeters();
		}

		private void Play_Empty_MeterAnimation()
		{
			fatMeter.SetPositionPercent(0f);
			fatMeter.meterController.Play("meter_fat_empty");
		}

		public void DropMilkFat()
		{
			List<GameObject> list = new List<GameObject>();
			storage.Drop(base.def.MILK_FAT_TAG, list);
			Vector3 dropSpawnLocation = GetDropSpawnLocation();
			foreach (GameObject item in list)
			{
				item.transform.position = dropSpawnLocation;
			}
		}

		private Vector3 GetDropSpawnLocation()
		{
			bool symbolVisible;
			Vector3 vector = GetComponent<KBatchedAnimController>().GetSymbolTransform(new HashedString("milkfat"), out symbolVisible).GetColumn(3);
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				return vector;
			}
			return base.transform.GetPosition();
		}

		public void RefreshMeters()
		{
			if (fatMeter.meterController.currentAnim != "meter_fat")
			{
				fatMeter.meterController.Play("meter_fat", KAnim.PlayMode.Paused);
			}
			fatMeter.SetPositionPercent(MilkFatStoragePercentage);
		}
	}

	public const string WORK_PRE_ANIM_NAME = "separating_pre";

	public const string WORK_ANIM_NAME = "separating_loop";

	public const string WORK_POST_ANIM_NAME = "separating_pst";

	public State noOperational;

	public OperationalStates operational;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = noOperational;
		base.serializable = SerializeType.ParamsOnly;
		root.EventHandler(GameHashes.OnStorageChange, RefreshMeters);
		noOperational.TagTransition(GameTags.Operational, operational).PlayAnim("off");
		operational.TagTransition(GameTags.Operational, noOperational, on_remove: true).PlayAnim("on").DefaultState(operational.idle);
		operational.idle.EventTransition(GameHashes.OnStorageChange, operational.working.pre, CanBeginSeparate).EnterTransition(operational.full, RequiresEmptying);
		operational.working.pre.QueueAnim("separating_pre").OnAnimQueueComplete(operational.working.work);
		operational.working.work.Enter(BeginSeparation).PlayAnim("separating_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, operational.working.post, CanNOTKeepSeparating)
			.Exit(EndSeparation);
		operational.working.post.QueueAnim("separating_pst").OnAnimQueueComplete(operational.idle);
		operational.full.PlayAnim("ready").ToggleRecurringChore(CreateEmptyChore).WorkableCompleteTransition((Instance smi) => smi.workable, operational.emptyComplete)
			.ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying);
		operational.emptyComplete.Enter(DropMilkFat).ScheduleActionNextFrame("AfterMilkFatDrop", delegate(Instance smi)
		{
			smi.GoTo(operational.idle);
		});
	}

	public static void BeginSeparation(Instance smi)
	{
		smi.operational.SetActive(value: true);
	}

	public static void EndSeparation(Instance smi)
	{
		smi.operational.SetActive(value: false);
	}

	public static bool CanBeginSeparate(Instance smi)
	{
		if (!smi.MilkFatLimitReached)
		{
			return smi.elementConverter.HasEnoughMassToStartConverting();
		}
		return false;
	}

	public static bool CanKeepSeparating(Instance smi)
	{
		if (!smi.MilkFatLimitReached)
		{
			return smi.elementConverter.CanConvertAtAll();
		}
		return false;
	}

	public static bool CanNOTKeepSeparating(Instance smi)
	{
		return !CanKeepSeparating(smi);
	}

	public static bool RequiresEmptying(Instance smi)
	{
		return smi.MilkFatLimitReached;
	}

	public static bool ThereIsCapacityForMilkFat(Instance smi)
	{
		return !smi.MilkFatLimitReached;
	}

	public static void DropMilkFat(Instance smi)
	{
		smi.DropMilkFat();
	}

	public static void RefreshMeters(Instance smi)
	{
		smi.RefreshMeters();
	}

	private static Chore CreateEmptyChore(Instance smi)
	{
		return new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, smi.workable);
	}
}
