using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001880 RID: 6272
[AddComponentMenu("KMonoBehaviour/scripts/SmartReservoir")]
public class SmartReservoir : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x060081CB RID: 33227 RVA: 0x000F5622 File Offset: 0x000F3822
	public float PercentFull
	{
		get
		{
			return this.storage.MassStored() / this.storage.Capacity();
		}
	}

	// Token: 0x060081CC RID: 33228 RVA: 0x000F563B File Offset: 0x000F383B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SmartReservoir>(-801688580, SmartReservoir.OnLogicValueChangedDelegate);
		base.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCircuitDelegate);
	}

	// Token: 0x060081CD RID: 33229 RVA: 0x000F5665 File Offset: 0x000F3865
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SmartReservoir>(-905833192, SmartReservoir.OnCopySettingsDelegate);
	}

	// Token: 0x060081CE RID: 33230 RVA: 0x000F567E File Offset: 0x000F387E
	public void Sim200ms(float dt)
	{
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x060081CF RID: 33231 RVA: 0x0033A994 File Offset: 0x00338B94
	private void UpdateLogicCircuit(object data)
	{
		float num = this.PercentFull * 100f;
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
		bool flag = this.activated;
		this.logicPorts.SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x060081D0 RID: 33232 RVA: 0x0033A9F8 File Offset: 0x00338BF8
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == SmartReservoir.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x060081D1 RID: 33233 RVA: 0x0033AA30 File Offset: 0x00338C30
	private void OnCopySettings(object data)
	{
		SmartReservoir component = ((GameObject)data).GetComponent<SmartReservoir>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x060081D2 RID: 33234 RVA: 0x000F5687 File Offset: 0x000F3887
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x060081D3 RID: 33235 RVA: 0x000F56AB File Offset: 0x000F38AB
	// (set) Token: 0x060081D4 RID: 33236 RVA: 0x000F56B4 File Offset: 0x000F38B4
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

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x060081D5 RID: 33237 RVA: 0x000F56C5 File Offset: 0x000F38C5
	// (set) Token: 0x060081D6 RID: 33238 RVA: 0x000F56CE File Offset: 0x000F38CE
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

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x060081D7 RID: 33239 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x060081D8 RID: 33240 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x060081D9 RID: 33241 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x060081DA RID: 33242 RVA: 0x000F56DF File Offset: 0x000F38DF
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x060081DB RID: 33243 RVA: 0x000F56EB File Offset: 0x000F38EB
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x060081DC RID: 33244 RVA: 0x000F56F7 File Offset: 0x000F38F7
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x060081DD RID: 33245 RVA: 0x000F5703 File Offset: 0x000F3903
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x060081DE RID: 33246 RVA: 0x000F570F File Offset: 0x000F390F
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04006285 RID: 25221
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04006286 RID: 25222
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04006287 RID: 25223
	[Serialize]
	private int activateValue;

	// Token: 0x04006288 RID: 25224
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x04006289 RID: 25225
	[Serialize]
	private bool activated;

	// Token: 0x0400628A RID: 25226
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x0400628B RID: 25227
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400628C RID: 25228
	private MeterController logicMeter;

	// Token: 0x0400628D RID: 25229
	public static readonly HashedString PORT_ID = "SmartReservoirLogicPort";

	// Token: 0x0400628E RID: 25230
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400628F RID: 25231
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04006290 RID: 25232
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
