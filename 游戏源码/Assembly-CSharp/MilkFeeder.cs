using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class MilkFeeder : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			go.GetSMI<Instance>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(CREATURES.MODIFIERS.GOTMILK.NAME, "");
			list.Add(item);
			Effect.AddModifierDescriptions(list, "HadMilk", increase_indent: true, "STRINGS.CREATURES.STATS.");
			return list;
		}
	}

	public class OnState : State
	{
		public class WorkingState : State
		{
			public State empty;

			public State refilling;

			public State full;

			public State emptying;
		}

		public State pre;

		public WorkingState working;

		public State pst;
	}

	public new class Instance : GameInstance
	{
		public Storage milkStorage;

		public MeterController storageMeter;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			milkStorage = GetComponent<Storage>();
			storageMeter = new MeterController(base.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
		}

		public override void StartSM()
		{
			base.StartSM();
			Components.MilkFeeders.Add(base.smi.GetMyWorldId(), this);
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.MilkFeeders.Remove(base.smi.GetMyWorldId(), this);
		}

		public void UpdateStorageMeter()
		{
			storageMeter.SetPositionPercent(1f - Mathf.Clamp01(milkStorage.RemainingCapacity() / milkStorage.capacityKg));
		}

		public bool IsOperational()
		{
			return GetComponent<Operational>().IsOperational;
		}

		public bool IsReserved()
		{
			return HasTag(GameTags.Creatures.ReservedByCreature);
		}

		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				Debug.Assert(!HasTag(GameTags.Creatures.ReservedByCreature));
				GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, set: true);
			}
			else if (HasTag(GameTags.Creatures.ReservedByCreature))
			{
				GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
			}
			else
			{
				Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved");
			}
		}

		public bool IsReadyToStartFeeding()
		{
			if (!IsOperational())
			{
				return false;
			}
			return base.sm.isReadyToStartFeeding.Get(base.smi);
		}

		public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
		{
			base.sm.currentFeedingCritter.Set(feedingCritter, base.smi);
		}

		public void StopFeeding()
		{
			base.sm.currentFeedingCritter.Get(base.smi)?.RequestToStopFeeding();
			base.sm.currentFeedingCritter.Set(null, base.smi);
		}

		public bool HasEnoughMilkForOneFeeding()
		{
			return milkStorage.GetAmountAvailable(MilkFeederConfig.MILK_TAG) >= 5f;
		}

		public void ConsumeMilkForOneFeeding()
		{
			milkStorage.ConsumeIgnoringDisease(MilkFeederConfig.MILK_TAG, 5f);
		}

		public bool IsInCreaturePenRoom()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject == null)
			{
				return false;
			}
			return roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
		}
	}

	private State off;

	private OnState on;

	public BoolParameter isReadyToStartFeeding;

	public ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = off;
		root.Enter(delegate(Instance smi)
		{
			smi.UpdateStorageMeter();
		}).EventHandler(GameHashes.OnStorageChange, delegate(Instance smi)
		{
			smi.UpdateStorageMeter();
		});
		off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, on, (Instance smi) => smi.GetComponent<Operational>().IsOperational);
		on.DefaultState(on.pre).EventTransition(GameHashes.OperationalChanged, on.pst, (Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() != on.pre).EventTransition(GameHashes.OperationalChanged, off, (Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() == on.pre);
		on.pre.PlayAnim("working_pre").OnAnimQueueComplete(on.working);
		on.working.PlayAnim("on").DefaultState(on.working.empty);
		on.working.empty.PlayAnim("empty").EnterTransition(on.working.refilling, (Instance smi) => smi.HasEnoughMilkForOneFeeding()).EventHandler(GameHashes.OnStorageChange, delegate(Instance smi)
		{
			if (smi.HasEnoughMilkForOneFeeding())
			{
				smi.GoTo(on.working.refilling);
			}
		});
		on.working.refilling.PlayAnim("fill").OnAnimQueueComplete(on.working.full);
		on.working.full.PlayAnim("full").Enter(delegate(Instance smi)
		{
			isReadyToStartFeeding.Set(value: true, smi);
		}).Exit(delegate(Instance smi)
		{
			isReadyToStartFeeding.Set(value: false, smi);
		})
			.ParamTransition(currentFeedingCritter, on.working.emptying, (Instance smi, DrinkMilkStates.Instance val) => val != null);
		on.working.emptying.EnterTransition(on.working.full, delegate(Instance smi)
		{
			DrinkMilkMonitor.Instance sMI = currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
			return sMI != null && !sMI.def.consumesMilk;
		}).PlayAnim("emptying").OnAnimQueueComplete(on.working.empty)
			.Exit(delegate(Instance smi)
			{
				smi.StopFeeding();
			});
		on.pst.PlayAnim("working_pst").OnAnimQueueComplete(off);
	}
}
