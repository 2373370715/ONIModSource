using System;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CA3 RID: 3235
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class BatterySmart : Battery, IActivationRangeTarget
{
	// Token: 0x06003E6F RID: 15983 RVA: 0x000C898E File Offset: 0x000C6B8E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<BatterySmart>(-905833192, BatterySmart.OnCopySettingsDelegate);
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0023456C File Offset: 0x0023276C
	private void OnCopySettings(object data)
	{
		BatterySmart component = ((GameObject)data).GetComponent<BatterySmart>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x000C89A7 File Offset: 0x000C6BA7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CreateLogicMeter();
		base.Subscribe<BatterySmart>(-801688580, BatterySmart.OnLogicValueChangedDelegate);
		base.Subscribe<BatterySmart>(-592767678, BatterySmart.UpdateLogicCircuitDelegate);
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x000C89D7 File Offset: 0x000C6BD7
	private void CreateLogicMeter()
	{
		this.logicMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x000C89FC File Offset: 0x000C6BFC
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x002345A8 File Offset: 0x002327A8
	private void UpdateLogicCircuit(object data)
	{
		float num = (float)Mathf.RoundToInt(base.PercentFull * 100f);
		if (this.activated)
		{
			if (num >= (float)this.deactivateValue)
			{
				this.activated = false;
			}
		}
		else if (num <= (float)this.activateValue)
		{
			this.activated = true;
		}
		bool isOperational = this.operational.IsOperational;
		bool flag = this.activated && isOperational;
		this.logicPorts.SendSignal(BatterySmart.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x00234620 File Offset: 0x00232820
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == BatterySmart.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x000C8A0C File Offset: 0x000C6C0C
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06003E77 RID: 15991 RVA: 0x000C8A30 File Offset: 0x000C6C30
	// (set) Token: 0x06003E78 RID: 15992 RVA: 0x000C8A39 File Offset: 0x000C6C39
	public float ActivateValue
	{
		get
		{
			return (float)this.deactivateValue;
		}
		set
		{
			this.deactivateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06003E79 RID: 15993 RVA: 0x000C8A4A File Offset: 0x000C6C4A
	// (set) Token: 0x06003E7A RID: 15994 RVA: 0x000C8A53 File Offset: 0x000C6C53
	public float DeactivateValue
	{
		get
		{
			return (float)this.activateValue;
		}
		set
		{
			this.activateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06003E7B RID: 15995 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06003E7C RID: 15996 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06003E7D RID: 15997 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06003E7E RID: 15998 RVA: 0x000C8A6B File Offset: 0x000C6C6B
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06003E7F RID: 15999 RVA: 0x000C8A77 File Offset: 0x000C6C77
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06003E80 RID: 16000 RVA: 0x000C8A83 File Offset: 0x000C6C83
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06003E81 RID: 16001 RVA: 0x000C8A8F File Offset: 0x000C6C8F
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06003E82 RID: 16002 RVA: 0x000C8A9B File Offset: 0x000C6C9B
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04002AA9 RID: 10921
	public static readonly HashedString PORT_ID = "BatterySmartLogicPort";

	// Token: 0x04002AAA RID: 10922
	[Serialize]
	private int activateValue;

	// Token: 0x04002AAB RID: 10923
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x04002AAC RID: 10924
	[Serialize]
	private bool activated;

	// Token: 0x04002AAD RID: 10925
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04002AAE RID: 10926
	private MeterController logicMeter;

	// Token: 0x04002AAF RID: 10927
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002AB0 RID: 10928
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002AB1 RID: 10929
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002AB2 RID: 10930
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
