using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E61 RID: 3681
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonReader")]
public class LogicRibbonReader : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06004995 RID: 18837 RVA: 0x00258C18 File Offset: 0x00256E18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonReader>(-801688580, LogicRibbonReader.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x000CFBB5 File Offset: 0x000CDDB5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonReader>(-905833192, LogicRibbonReader.OnCopySettingsDelegate);
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x00258C74 File Offset: 0x00256E74
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonReader.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x00258CB4 File Offset: 0x00256EB4
	private void OnCopySettings(object data)
	{
		LogicRibbonReader component = ((GameObject)data).GetComponent<LogicRibbonReader>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x00258CE4 File Offset: 0x00256EE4
	private void UpdateLogicCircuit()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
		int portCell = component.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component2 = gameObject.GetComponent<LogicWire>();
			if (component2 != null)
			{
				bitDepth = component2.MaxBitDepth;
			}
		}
		if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
		{
			int num = this.currentValue >> this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, num);
		}
		else
		{
			int num = this.currentValue & 1 << this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, (num > 0) ? 1 : 0);
		}
		this.UpdateVisuals();
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x000CFBCE File Offset: 0x000CDDCE
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x000CFBD6 File Offset: 0x000CDDD6
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x000CFBE5 File Offset: 0x000CDDE5
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x0600499D RID: 18845 RVA: 0x000CFBED File Offset: 0x000CDDED
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x0600499E RID: 18846 RVA: 0x000CFBF5 File Offset: 0x000CDDF5
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_TITLE";
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x0600499F RID: 18847 RVA: 0x000CFBFC File Offset: 0x000CDDFC
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_DESCRIPTION;
		}
	}

	// Token: 0x060049A0 RID: 18848 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool SideScreenDisplayWriterDescription()
	{
		return false;
	}

	// Token: 0x060049A1 RID: 18849 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SideScreenDisplayReaderDescription()
	{
		return true;
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x00258D90 File Offset: 0x00256F90
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x00258DDC File Offset: 0x00256FDC
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonReader.INPUT_PORT_ID);
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x00258E08 File Offset: 0x00257008
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonReader.OUTPUT_PORT_ID);
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x00258E34 File Offset: 0x00257034
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x00258E74 File Offset: 0x00257074
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x00258EB4 File Offset: 0x002570B4
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		this.GetInputValue();
		int num = 0;
		if (inputNetwork)
		{
			num += 4;
			this.kbac.SetSymbolTint(this.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num++;
			this.kbac.SetSymbolTint(this.OUTPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetOutputValue()) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0400332F RID: 13103
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonReaderInput");

	// Token: 0x04003330 RID: 13104
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonReaderOutput");

	// Token: 0x04003331 RID: 13105
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003332 RID: 13106
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003333 RID: 13107
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003334 RID: 13108
	private KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04003335 RID: 13109
	private KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04003336 RID: 13110
	private KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04003337 RID: 13111
	private KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04003338 RID: 13112
	private KAnimHashedString OUTPUT_SYMBOL = "output_light_bloom";

	// Token: 0x04003339 RID: 13113
	private KBatchedAnimController kbac;

	// Token: 0x0400333A RID: 13114
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x0400333B RID: 13115
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x0400333C RID: 13116
	private LogicPorts ports;

	// Token: 0x0400333D RID: 13117
	public int bitDepth = 4;

	// Token: 0x0400333E RID: 13118
	[Serialize]
	public int selectedBit;

	// Token: 0x0400333F RID: 13119
	[Serialize]
	private int currentValue;
}
