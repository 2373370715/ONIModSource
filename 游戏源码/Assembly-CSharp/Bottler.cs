using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000CAD RID: 3245
[AddComponentMenu("KMonoBehaviour/Workable/Bottler")]
public class Bottler : Workable, IUserControlledCapacity
{
	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x000C8CAC File Offset: 0x000C6EAC
	// (set) Token: 0x06003EB7 RID: 16055 RVA: 0x000C8CD8 File Offset: 0x000C6ED8
	public float UserMaxCapacity
	{
		get
		{
			if (this.consumer != null)
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			return 0f;
		}
		set
		{
			this.userMaxCapacity = value;
			this.SetConsumerCapacity(value);
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x000C8CE8 File Offset: 0x000C6EE8
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06003EBA RID: 16058 RVA: 0x000C8CF5 File Offset: 0x000C6EF5
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06003EBB RID: 16059 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06003EBC RID: 16060 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06003EBD RID: 16061 RVA: 0x000C8D0A File Offset: 0x000C6F0A
	private Tag SourceTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.LiquidSource;
			}
			return GameTags.GasSource;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06003EBE RID: 16062 RVA: 0x000C8D2F File Offset: 0x000C6F2F
	private Tag ElementTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.Liquid;
			}
			return GameTags.Gas;
		}
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x00234EA8 File Offset: 0x002330A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bottler_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"pick_up"
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(this.overrideAnims[0].GetData().GetAnim("pick_up").totalTime);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x00234F54 File Offset: 0x00233154
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Bottler.Controller.Instance(this);
		this.smi.StartSM();
		base.Subscribe<Bottler>(-905833192, Bottler.OnCopySettingsDelegate);
		this.UpdateStoredItemState();
		this.SetConsumerCapacity(this.userMaxCapacity);
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x00234FA4 File Offset: 0x002331A4
	protected override void OnForcedCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			if (component != null)
			{
				component.StopChore();
			}
			else
			{
				base.worker.StopWork();
			}
		}
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
		}
		base.OnForcedCleanUp();
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x000C8D54 File Offset: 0x000C6F54
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.CreateBottleProxyObject(worker);
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x00234FFC File Offset: 0x002331FC
	private void CreateBottleProxyObject(WorkerBase worker)
	{
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
			KCrashReporter.ReportDevNotification("CreateBottleProxyObject called before cleanup", Environment.StackTrace, "", false, null);
		}
		PrimaryElement firstPrimaryElement = this.smi.master.GetFirstPrimaryElement();
		if (firstPrimaryElement == null)
		{
			KCrashReporter.ReportDevNotification("CreateBottleProxyObject on a null element", Environment.StackTrace, "", false, null);
			return;
		}
		this.workerMeter = new MeterController(worker.GetComponent<KBatchedAnimController>(), "snapto_chest", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"snapto_chest"
		});
		this.workerMeter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
		this.workerMeter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
		Color32 colour = firstPrimaryElement.Element.substance.colour;
		colour.a = byte.MaxValue;
		this.workerMeter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("water1"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x00235130 File Offset: 0x00233330
	private void CleanupBottleProxyObject()
	{
		if (this.workerMeter != null && !this.workerMeter.gameObject.IsNullOrDestroyed())
		{
			this.workerMeter.Unlink();
			this.workerMeter.gameObject.DeleteObject();
		}
		else
		{
			string str = "Bottler finished work but could not clean up the proxy bottle object. workerMeter=";
			MeterController meterController = this.workerMeter;
			DebugUtil.DevLogError(str + ((meterController != null) ? meterController.ToString() : null));
			KCrashReporter.ReportDevNotification("Bottle emptier could not clean up proxy object", Environment.StackTrace, "", false, null);
		}
		this.workerMeter = null;
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x000C8D64 File Offset: 0x000C6F64
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.CleanupBottleProxyObject();
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x000C8D73 File Offset: 0x000C6F73
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		this.GetAnimController().Play("ready", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x002351B4 File Offset: 0x002333B4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			Pickupable component2 = gameObject.GetComponent<Pickupable>();
			component2.targetWorkable = component2;
			component2.RemoveTag(this.SourceTag);
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x00235260 File Offset: 0x00233460
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Pickupable>().ReservedAmount > 0f)
				{
					forceUnfetchable = true;
					break;
				}
			}
		}
		foreach (GameObject go in this.storage.items)
		{
			FetchableMonitor.Instance instance = go.GetSMI<FetchableMonitor.Instance>();
			if (instance != null)
			{
				instance.SetForceUnfetchable(forceUnfetchable);
			}
		}
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x00235318 File Offset: 0x00233518
	private void SetConsumerCapacity(float value)
	{
		if (this.consumer != null)
		{
			this.consumer.capacityKG = value;
			float num = this.storage.MassStored() - this.userMaxCapacity;
			if (num > 0f)
			{
				this.storage.DropSome(this.storage.FindFirstWithMass(this.smi.master.ElementTag, 0f).ElementID.CreateTag(), num, false, false, new Vector3(0.8f, 0f, 0f), true, false);
			}
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x000C8D9C File Offset: 0x000C6F9C
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
		}
		base.OnCleanUp();
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x002353AC File Offset: 0x002335AC
	private PrimaryElement GetFirstPrimaryElement()
	{
		for (int i = 0; i < this.storage.Count; i++)
		{
			GameObject gameObject = this.storage[i];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x002353F8 File Offset: 0x002335F8
	private void UpdateStoredItemState()
	{
		this.storage.allowItemRemoval = (this.smi != null && this.smi.GetCurrentState() == this.smi.sm.ready);
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				gameObject.Trigger(-778359855, this.storage);
			}
		}
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x00235498 File Offset: 0x00233698
	private void OnCopySettings(object data)
	{
		Bottler component = ((GameObject)data).GetComponent<Bottler>();
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x04002AD4 RID: 10964
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002AD5 RID: 10965
	public Storage storage;

	// Token: 0x04002AD6 RID: 10966
	public ConduitConsumer consumer;

	// Token: 0x04002AD7 RID: 10967
	public CellOffset workCellOffset = new CellOffset(0, 0);

	// Token: 0x04002AD8 RID: 10968
	[Serialize]
	public float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04002AD9 RID: 10969
	private Bottler.Controller.Instance smi;

	// Token: 0x04002ADA RID: 10970
	private int storageHandle;

	// Token: 0x04002ADB RID: 10971
	private MeterController workerMeter;

	// Token: 0x04002ADC RID: 10972
	private static readonly EventSystem.IntraObjectHandler<Bottler> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Bottler>(delegate(Bottler component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000CAE RID: 3246
	private class Controller : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler>
	{
		// Token: 0x06003ED0 RID: 16080 RVA: 0x002354C0 File Offset: 0x002336C0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.PlayAnim("off").EventHandlerTransition(GameHashes.OnStorageChange, this.filling, (Bottler.Controller.Instance smi, object o) => Bottler.Controller.IsFull(smi)).EnterTransition(this.ready, new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull));
			this.filling.PlayAnim("working").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.UpdateMeter();
			}).OnAnimQueueComplete(this.ready);
			this.ready.EventTransition(GameHashes.OnStorageChange, this.empty, GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Not(new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull))).PlayAnim("ready").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
				smi.UpdateMeter();
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = smi.master;
					component.SetOffsets(new CellOffset[]
					{
						smi.master.workCellOffset
					});
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.AddTag(smi.master.SourceTag, false);
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			}).Exit(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = component;
					component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					component.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Remove(component.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.RemoveTag(smi.master.SourceTag);
					FetchableMonitor.Instance smi2 = component.GetSMI<FetchableMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.SetForceUnfetchable(false);
					}
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			});
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x000C8DF8 File Offset: 0x000C6FF8
		public static bool IsFull(Bottler.Controller.Instance smi)
		{
			return smi.master.storage.MassStored() >= smi.master.userMaxCapacity;
		}

		// Token: 0x04002ADD RID: 10973
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State empty;

		// Token: 0x04002ADE RID: 10974
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State filling;

		// Token: 0x04002ADF RID: 10975
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State ready;

		// Token: 0x02000CAF RID: 3247
		public new class Instance : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.GameInstance
		{
			// Token: 0x170002FD RID: 765
			// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x000C8E22 File Offset: 0x000C7022
			// (set) Token: 0x06003ED4 RID: 16084 RVA: 0x000C8E2A File Offset: 0x000C702A
			public MeterController meter { get; private set; }

			// Token: 0x06003ED5 RID: 16085 RVA: 0x002355E8 File Offset: 0x002337E8
			public Instance(Bottler master) : base(master)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "bottle", "off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, new string[]
				{
					"bottle",
					"substance_tinter"
				});
			}

			// Token: 0x06003ED6 RID: 16086 RVA: 0x00235630 File Offset: 0x00233830
			public void UpdateMeter()
			{
				PrimaryElement firstPrimaryElement = base.smi.master.GetFirstPrimaryElement();
				if (firstPrimaryElement == null)
				{
					return;
				}
				this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
				this.meter.meterController.Play(OreSizeVisualizerComponents.GetAnimForMass(firstPrimaryElement.Mass), KAnim.PlayMode.Paused, 1f, 0f);
				Color32 colour = firstPrimaryElement.Element.substance.colour;
				colour.a = byte.MaxValue;
				this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			}
		}
	}
}
