using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000EE2 RID: 3810
public class ObjectDispenser : Switch, IUserControlledCapacity
{
	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x000D1D51 File Offset: 0x000CFF51
	// (set) Token: 0x06004CD1 RID: 19665 RVA: 0x000D1D69 File Offset: 0x000CFF69
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06004CD2 RID: 19666 RVA: 0x000D1D7D File Offset: 0x000CFF7D
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06004CD3 RID: 19667 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06004CD4 RID: 19668 RVA: 0x000D1D8A File Offset: 0x000CFF8A
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06004CD5 RID: 19669 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06004CD6 RID: 19670 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x000D1D97 File Offset: 0x000CFF97
	protected override void OnPrefabInit()
	{
		this.Initialize();
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x002639D0 File Offset: 0x00261BD0
	protected void Initialize()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("ObjectDispenser", 35);
		this.filteredStorage = new FilteredStorage(this, null, this, false, Db.Get().ChoreTypes.StorageFetch);
		base.Subscribe<ObjectDispenser>(-905833192, ObjectDispenser.OnCopySettingsDelegate);
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x00263A24 File Offset: 0x00261C24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ObjectDispenser.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		if (ObjectDispenser.infoStatusItem == null)
		{
			ObjectDispenser.infoStatusItem = new StatusItem("ObjectDispenserAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ObjectDispenser.infoStatusItem.resolveStringCallback = new Func<string, object, string>(ObjectDispenser.ResolveInfoStatusItemString);
		}
		this.filteredStorage.FilterChanged();
		base.GetComponent<KSelectable>().ToggleStatusItem(ObjectDispenser.infoStatusItem, true, this.smi);
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x000D1D9F File Offset: 0x000CFF9F
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x00263ABC File Offset: 0x00261CBC
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ObjectDispenser component = gameObject.GetComponent<ObjectDispenser>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06004CDC RID: 19676 RVA: 0x00263AF8 File Offset: 0x00261CF8
	public void DropHeldItems()
	{
		while (this.storage.Count > 0)
		{
			GameObject gameObject = this.storage.Drop(this.storage.items[0], true);
			if (this.rotatable != null)
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.rotatable.GetRotatedCellOffset(this.dropOffset).ToVector3());
			}
			else
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.dropOffset.ToVector3());
			}
		}
		this.smi.GetMaster().GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x000D1DB2 File Offset: 0x000CFFB2
	protected override void Toggle()
	{
		base.Toggle();
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x000D1DBA File Offset: 0x000CFFBA
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x06004CDF RID: 19679 RVA: 0x00263BC8 File Offset: 0x00261DC8
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		ObjectDispenser.Instance instance = (ObjectDispenser.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.OBJECTDISPENSER.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.OBJECTDISPENSER.MANUAL_CONTROL;
		string arg = instance.IsOpened ? BUILDING.STATUSITEMS.OBJECTDISPENSER.OPENED : BUILDING.STATUSITEMS.OBJECTDISPENSER.CLOSED;
		return string.Format(format, arg);
	}

	// Token: 0x04003562 RID: 13666
	public static readonly HashedString PORT_ID = "ObjectDispenser";

	// Token: 0x04003563 RID: 13667
	private LoggerFS log;

	// Token: 0x04003564 RID: 13668
	public CellOffset dropOffset;

	// Token: 0x04003565 RID: 13669
	[MyCmpReq]
	private Building building;

	// Token: 0x04003566 RID: 13670
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003567 RID: 13671
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04003568 RID: 13672
	private ObjectDispenser.Instance smi;

	// Token: 0x04003569 RID: 13673
	private static StatusItem infoStatusItem;

	// Token: 0x0400356A RID: 13674
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x0400356B RID: 13675
	protected FilteredStorage filteredStorage;

	// Token: 0x0400356C RID: 13676
	private static readonly EventSystem.IntraObjectHandler<ObjectDispenser> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ObjectDispenser>(delegate(ObjectDispenser component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000EE3 RID: 3811
	public class States : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser>
	{
		// Token: 0x06004CE2 RID: 19682 RVA: 0x00263C18 File Offset: 0x00261E18
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.idle.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, delegate(ObjectDispenser.Instance smi)
			{
				smi.UpdateState();
			}).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p && !smi.master.GetComponent<Storage>().IsEmpty());
			this.load_item.PlayAnim("working_load").OnAnimQueueComplete(this.load_item_pst);
			this.load_item_pst.ParamTransition<bool>(this.should_open, this.idle, (ObjectDispenser.Instance smi, bool p) => !p).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p);
			this.drop_item.PlayAnim("working_dispense").OnAnimQueueComplete(this.idle).Exit(delegate(ObjectDispenser.Instance smi)
			{
				smi.master.DropHeldItems();
			});
		}

		// Token: 0x0400356D RID: 13677
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item;

		// Token: 0x0400356E RID: 13678
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item_pst;

		// Token: 0x0400356F RID: 13679
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State drop_item;

		// Token: 0x04003570 RID: 13680
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State idle;

		// Token: 0x04003571 RID: 13681
		public StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.BoolParameter should_open;
	}

	// Token: 0x02000EE5 RID: 3813
	public class Instance : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.GameInstance
	{
		// Token: 0x06004CEB RID: 19691 RVA: 0x00263D64 File Offset: 0x00261F64
		public Instance(ObjectDispenser master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_open.Set(true, base.smi, false);
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x000D1E51 File Offset: 0x000D0051
		public void UpdateState()
		{
			base.smi.GoTo(base.sm.load_item);
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x000D1E69 File Offset: 0x000D0069
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(ObjectDispenser.PORT_ID);
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x000D1E7B File Offset: 0x000D007B
		public bool IsOpened
		{
			get
			{
				if (!this.IsAutomated())
				{
					return this.manual_on;
				}
				return this.logic_on;
			}
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x000D1E92 File Offset: 0x000D0092
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldOpen();
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x000D1EA1 File Offset: 0x000D00A1
		public void SetActive(bool active)
		{
			this.operational.SetActive(active, false);
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x000D1EB0 File Offset: 0x000D00B0
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldOpen();
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x00263DEC File Offset: 0x00261FEC
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != ObjectDispenser.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldOpen();
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x00263E2C File Offset: 0x0026202C
		private void UpdateShouldOpen()
		{
			this.SetActive(this.operational.IsOperational);
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_open.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_open.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04003578 RID: 13688
		private Operational operational;

		// Token: 0x04003579 RID: 13689
		public LogicPorts logic;

		// Token: 0x0400357A RID: 13690
		public bool logic_on = true;

		// Token: 0x0400357B RID: 13691
		private bool manual_on;
	}
}
