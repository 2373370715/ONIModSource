using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000EAE RID: 3758
public class MilkFeeder : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
	// Token: 0x06004BBC RID: 19388 RVA: 0x0025F978 File Offset: 0x0025DB78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Enter(delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		}).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		});
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (MilkFeeder.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).EventTransition(GameHashes.OperationalChanged, this.on.pst, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() != this.on.pre).EventTransition(GameHashes.OperationalChanged, this.off, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() == this.on.pre);
		this.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
		this.on.working.PlayAnim("on").DefaultState(this.on.working.empty);
		this.on.working.empty.PlayAnim("empty").EnterTransition(this.on.working.refilling, (MilkFeeder.Instance smi) => smi.HasEnoughMilkForOneFeeding()).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			if (smi.HasEnoughMilkForOneFeeding())
			{
				smi.GoTo(this.on.working.refilling);
			}
		});
		this.on.working.refilling.PlayAnim("fill").OnAnimQueueComplete(this.on.working.full);
		this.on.working.full.PlayAnim("full").Enter(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(true, smi, false);
		}).Exit(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(false, smi, false);
		}).ParamTransition<DrinkMilkStates.Instance>(this.currentFeedingCritter, this.on.working.emptying, (MilkFeeder.Instance smi, DrinkMilkStates.Instance val) => val != null);
		this.on.working.emptying.EnterTransition(this.on.working.full, delegate(MilkFeeder.Instance smi)
		{
			DrinkMilkMonitor.Instance smi2 = this.currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
			return smi2 != null && !smi2.def.consumesMilk;
		}).PlayAnim("emptying").OnAnimQueueComplete(this.on.working.empty).Exit(delegate(MilkFeeder.Instance smi)
		{
			smi.StopFeeding();
		});
		this.on.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x0400347D RID: 13437
	private GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State off;

	// Token: 0x0400347E RID: 13438
	private MilkFeeder.OnState on;

	// Token: 0x0400347F RID: 13439
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.BoolParameter isReadyToStartFeeding;

	// Token: 0x04003480 RID: 13440
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

	// Token: 0x02000EAF RID: 3759
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004BC4 RID: 19396 RVA: 0x0025FC9C File Offset: 0x0025DE9C
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			go.GetSMI<MilkFeeder.Instance>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(CREATURES.MODIFIERS.GOTMILK.NAME, "", Descriptor.DescriptorType.Effect);
			list.Add(item);
			Effect.AddModifierDescriptions(list, "HadMilk", true, "STRINGS.CREATURES.STATS.");
			return list;
		}
	}

	// Token: 0x02000EB0 RID: 3760
	public class OnState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
	{
		// Token: 0x04003481 RID: 13441
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pre;

		// Token: 0x04003482 RID: 13442
		public MilkFeeder.OnState.WorkingState working;

		// Token: 0x04003483 RID: 13443
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pst;

		// Token: 0x02000EB1 RID: 3761
		public class WorkingState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
		{
			// Token: 0x04003484 RID: 13444
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State empty;

			// Token: 0x04003485 RID: 13445
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State refilling;

			// Token: 0x04003486 RID: 13446
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State full;

			// Token: 0x04003487 RID: 13447
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State emptying;
		}
	}

	// Token: 0x02000EB2 RID: 3762
	public new class Instance : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.GameInstance
	{
		// Token: 0x06004BC8 RID: 19400 RVA: 0x000D0F85 File Offset: 0x000CF185
		public Instance(IStateMachineTarget master, MilkFeeder.Def def) : base(master, def)
		{
			this.milkStorage = base.GetComponent<Storage>();
			this.storageMeter = new MeterController(base.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x000D0FC3 File Offset: 0x000CF1C3
		public override void StartSM()
		{
			base.StartSM();
			Components.MilkFeeders.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x000D0FE1 File Offset: 0x000CF1E1
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.MilkFeeders.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x000D0FFF File Offset: 0x000CF1FF
		public void UpdateStorageMeter()
		{
			this.storageMeter.SetPositionPercent(1f - Mathf.Clamp01(this.milkStorage.RemainingCapacity() / this.milkStorage.capacityKg));
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x000D102E File Offset: 0x000CF22E
		public bool IsOperational()
		{
			return base.GetComponent<Operational>().IsOperational;
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x000CA5B5 File Offset: 0x000C87B5
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x0025FCEC File Offset: 0x0025DEEC
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				global::Debug.Assert(!base.HasTag(GameTags.Creatures.ReservedByCreature));
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x000D103B File Offset: 0x000CF23B
		public bool IsReadyToStartFeeding()
		{
			return this.IsOperational() && base.sm.isReadyToStartFeeding.Get(base.smi);
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x000D105D File Offset: 0x000CF25D
		public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
		{
			base.sm.currentFeedingCritter.Set(feedingCritter, base.smi, false);
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0025FD5C File Offset: 0x0025DF5C
		public void StopFeeding()
		{
			DrinkMilkStates.Instance instance = base.sm.currentFeedingCritter.Get(base.smi);
			if (instance != null)
			{
				instance.RequestToStopFeeding();
			}
			base.sm.currentFeedingCritter.Set(null, base.smi, false);
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x000D1078 File Offset: 0x000CF278
		public bool HasEnoughMilkForOneFeeding()
		{
			return this.milkStorage.GetAmountAvailable(MilkFeederConfig.MILK_TAG) >= 5f;
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x000D1094 File Offset: 0x000CF294
		public void ConsumeMilkForOneFeeding()
		{
			this.milkStorage.ConsumeIgnoringDisease(MilkFeederConfig.MILK_TAG, 5f);
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0025FDA4 File Offset: 0x0025DFA4
		public bool IsInCreaturePenRoom()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			return roomOfGameObject != null && roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
		}

		// Token: 0x04003488 RID: 13448
		public Storage milkStorage;

		// Token: 0x04003489 RID: 13449
		public MeterController storageMeter;
	}
}
