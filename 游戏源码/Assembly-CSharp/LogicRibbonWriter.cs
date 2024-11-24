using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E63 RID: 3683
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonWriter")]
public class LogicRibbonWriter : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x060049AE RID: 18862 RVA: 0x000CFC26 File Offset: 0x000CDE26
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonWriter>(-905833192, LogicRibbonWriter.OnCopySettingsDelegate);
	}

	// Token: 0x060049AF RID: 18863 RVA: 0x002590F0 File Offset: 0x002572F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonWriter>(-801688580, LogicRibbonWriter.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x0025914C File Offset: 0x0025734C
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonWriter.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x0025918C File Offset: 0x0025738C
	private void OnCopySettings(object data)
	{
		LogicRibbonWriter component = ((GameObject)data).GetComponent<LogicRibbonWriter>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x002591BC File Offset: 0x002573BC
	private void UpdateLogicCircuit()
	{
		int new_value = this.currentValue << this.selectedBit;
		base.GetComponent<LogicPorts>().SendSignal(LogicRibbonWriter.OUTPUT_PORT_ID, new_value);
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x000CFC3F File Offset: 0x000CDE3F
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x060049B4 RID: 18868 RVA: 0x002591EC File Offset: 0x002573EC
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x0025922C File Offset: 0x0025742C
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x000CFC47 File Offset: 0x000CDE47
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x000CFC56 File Offset: 0x000CDE56
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x060049B8 RID: 18872 RVA: 0x000CFC5E File Offset: 0x000CDE5E
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060049B9 RID: 18873 RVA: 0x000CFC66 File Offset: 0x000CDE66
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_TITLE";
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060049BA RID: 18874 RVA: 0x000CFC6D File Offset: 0x000CDE6D
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_DESCRIPTION;
		}
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SideScreenDisplayWriterDescription()
	{
		return true;
	}

	// Token: 0x060049BC RID: 18876 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool SideScreenDisplayReaderDescription()
	{
		return false;
	}

	// Token: 0x060049BD RID: 18877 RVA: 0x0025926C File Offset: 0x0025746C
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x060049BE RID: 18878 RVA: 0x002592B8 File Offset: 0x002574B8
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonWriter.INPUT_PORT_ID);
	}

	// Token: 0x060049BF RID: 18879 RVA: 0x002592E4 File Offset: 0x002574E4
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonWriter.OUTPUT_PORT_ID);
	}

	// Token: 0x060049C0 RID: 18880 RVA: 0x00259310 File Offset: 0x00257510
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		int num = 0;
		if (inputNetwork)
		{
			num++;
			this.kbac.SetSymbolTint(LogicRibbonWriter.INPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetInputValue()) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num += 4;
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003341 RID: 13121
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonWriterInput");

	// Token: 0x04003342 RID: 13122
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonWriterOutput");

	// Token: 0x04003343 RID: 13123
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003344 RID: 13124
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003345 RID: 13125
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003346 RID: 13126
	private LogicPorts ports;

	// Token: 0x04003347 RID: 13127
	public int bitDepth = 4;

	// Token: 0x04003348 RID: 13128
	[Serialize]
	public int selectedBit;

	// Token: 0x04003349 RID: 13129
	[Serialize]
	private int currentValue;

	// Token: 0x0400334A RID: 13130
	private KBatchedAnimController kbac;

	// Token: 0x0400334B RID: 13131
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x0400334C RID: 13132
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x0400334D RID: 13133
	private static KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x0400334E RID: 13134
	private static KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x0400334F RID: 13135
	private static KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04003350 RID: 13136
	private static KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04003351 RID: 13137
	private static KAnimHashedString INPUT_SYMBOL = "input_light_bloom";
}
