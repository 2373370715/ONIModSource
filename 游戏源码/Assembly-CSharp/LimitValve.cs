using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000E20 RID: 3616
[SerializationConfig(MemberSerialization.OptIn)]
public class LimitValve : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06004717 RID: 18199 RVA: 0x000CE18D File Offset: 0x000CC38D
	public float RemainingCapacity
	{
		get
		{
			return Mathf.Max(0f, this.m_limit - this.m_amount);
		}
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x000CE1A6 File Offset: 0x000CC3A6
	public NonLinearSlider.Range[] GetRanges()
	{
		if (this.sliderRanges != null && this.sliderRanges.Length != 0)
		{
			return this.sliderRanges;
		}
		return NonLinearSlider.GetDefaultRange(this.maxLimitKg);
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06004719 RID: 18201 RVA: 0x000CE1CB File Offset: 0x000CC3CB
	// (set) Token: 0x0600471A RID: 18202 RVA: 0x000CE1D3 File Offset: 0x000CC3D3
	public float Limit
	{
		get
		{
			return this.m_limit;
		}
		set
		{
			this.m_limit = value;
			this.Refresh();
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x0600471B RID: 18203 RVA: 0x000CE1E2 File Offset: 0x000CC3E2
	// (set) Token: 0x0600471C RID: 18204 RVA: 0x000CE1EA File Offset: 0x000CC3EA
	public float Amount
	{
		get
		{
			return this.m_amount;
		}
		set
		{
			this.m_amount = value;
			base.Trigger(-1722241721, this.Amount);
			this.Refresh();
		}
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x000CE20F File Offset: 0x000CC40F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LimitValve>(-905833192, LimitValve.OnCopySettingsDelegate);
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x00251048 File Offset: 0x0024F248
	protected override void OnSpawn()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.Subscribe<LimitValve>(-801688580, LimitValve.OnLogicValueChangedDelegate);
		if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
		{
			ConduitBridge conduitBridge = this.conduitBridge;
			conduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(conduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			ConduitBridge conduitBridge2 = this.conduitBridge;
			conduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(conduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		else if (this.conduitType == ConduitType.Solid)
		{
			SolidConduitBridge solidConduitBridge = this.solidConduitBridge;
			solidConduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(solidConduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			SolidConduitBridge solidConduitBridge2 = this.solidConduitBridge;
			solidConduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(solidConduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		if (this.limitMeter == null)
		{
			this.limitMeter = new MeterController(this.controller, "meter_target_counter", "meter_counter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_counter"
			});
		}
		this.Refresh();
		base.OnSpawn();
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x000CE228 File Offset: 0x000CC428
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x000CE25B File Offset: 0x000CC45B
	private void LogicTick()
	{
		if (this.m_resetRequested)
		{
			this.ResetAmount();
		}
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x000CE26B File Offset: 0x000CC46B
	public void ResetAmount()
	{
		this.m_resetRequested = false;
		this.Amount = 0f;
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x0025118C File Offset: 0x0024F38C
	private float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!this.operational.IsOperational)
		{
			return 0f;
		}
		if (this.conduitType == ConduitType.Solid && pickupable != null && GameTags.DisplayAsUnits.Contains(pickupable.KPrefabID.PrefabID()))
		{
			float num = pickupable.PrimaryElement.Units;
			if (this.RemainingCapacity < num)
			{
				num = (float)Mathf.FloorToInt(this.RemainingCapacity);
			}
			return num * pickupable.PrimaryElement.MassPerUnit;
		}
		return Mathf.Min(mass, this.RemainingCapacity);
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x00251218 File Offset: 0x0024F418
	private void OnMassTransfer(SimHashes element, float transferredMass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!LogicCircuitNetwork.IsBitActive(0, this.ports.GetInputValue(LimitValve.RESET_PORT_ID)))
		{
			if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
			{
				this.Amount += transferredMass;
			}
			else if (this.conduitType == ConduitType.Solid && pickupable != null)
			{
				this.Amount += transferredMass / pickupable.PrimaryElement.MassPerUnit;
			}
		}
		this.operational.SetActive(this.operational.IsOperational && transferredMass > 0f, false);
		this.Refresh();
	}

	// Token: 0x06004724 RID: 18212 RVA: 0x002512B8 File Offset: 0x0024F4B8
	private void Refresh()
	{
		if (this.operational == null)
		{
			return;
		}
		this.ports.SendSignal(LimitValve.OUTPUT_PORT_ID, (this.RemainingCapacity <= 0f) ? 1 : 0);
		this.operational.SetFlag(LimitValve.limitNotReached, this.RemainingCapacity > 0f);
		if (this.RemainingCapacity > 0f)
		{
			this.limitMeter.meterController.Play("meter_counter", KAnim.PlayMode.Paused, 1f, 0f);
			this.limitMeter.SetPositionPercent(this.Amount / this.Limit);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitNotReached, this);
			return;
		}
		this.limitMeter.meterController.Play("meter_on", KAnim.PlayMode.Paused, 1f, 0f);
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitReached, this);
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x002513D8 File Offset: 0x0024F5D8
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LimitValve.RESET_PORT_ID && LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
		{
			this.ResetAmount();
		}
	}

	// Token: 0x06004726 RID: 18214 RVA: 0x00251414 File Offset: 0x0024F614
	private void OnCopySettings(object data)
	{
		LimitValve component = ((GameObject)data).GetComponent<LimitValve>();
		if (component != null)
		{
			this.Limit = component.Limit;
		}
	}

	// Token: 0x0400314E RID: 12622
	public static readonly HashedString RESET_PORT_ID = new HashedString("LimitValveReset");

	// Token: 0x0400314F RID: 12623
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LimitValveOutput");

	// Token: 0x04003150 RID: 12624
	public static readonly Operational.Flag limitNotReached = new Operational.Flag("limitNotReached", Operational.Flag.Type.Requirement);

	// Token: 0x04003151 RID: 12625
	public ConduitType conduitType;

	// Token: 0x04003152 RID: 12626
	public float maxLimitKg = 100f;

	// Token: 0x04003153 RID: 12627
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003154 RID: 12628
	[MyCmpReq]
	private LogicPorts ports;

	// Token: 0x04003155 RID: 12629
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04003156 RID: 12630
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003157 RID: 12631
	[MyCmpGet]
	private ConduitBridge conduitBridge;

	// Token: 0x04003158 RID: 12632
	[MyCmpGet]
	private SolidConduitBridge solidConduitBridge;

	// Token: 0x04003159 RID: 12633
	[Serialize]
	[SerializeField]
	private float m_limit;

	// Token: 0x0400315A RID: 12634
	[Serialize]
	private float m_amount;

	// Token: 0x0400315B RID: 12635
	[Serialize]
	private bool m_resetRequested;

	// Token: 0x0400315C RID: 12636
	private MeterController limitMeter;

	// Token: 0x0400315D RID: 12637
	public bool displayUnitsInsteadOfMass;

	// Token: 0x0400315E RID: 12638
	public NonLinearSlider.Range[] sliderRanges;

	// Token: 0x0400315F RID: 12639
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003160 RID: 12640
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003161 RID: 12641
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnCopySettings(data);
	});
}
