using System;
using UnityEngine;

// Token: 0x02000E41 RID: 3649
[AddComponentMenu("KMonoBehaviour/scripts/LogicGateBase")]
public class LogicGateBase : KMonoBehaviour
{
	// Token: 0x06004848 RID: 18504 RVA: 0x00254CCC File Offset: 0x00252ECC
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06004849 RID: 18505 RVA: 0x000CEE45 File Offset: 0x000CD045
	public int InputCellOne
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[0]);
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x0600484A RID: 18506 RVA: 0x000CEE59 File Offset: 0x000CD059
	public int InputCellTwo
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[1]);
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x0600484B RID: 18507 RVA: 0x000CEE6D File Offset: 0x000CD06D
	public int InputCellThree
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[2]);
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x0600484C RID: 18508 RVA: 0x000CEE81 File Offset: 0x000CD081
	public int InputCellFour
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[3]);
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x0600484D RID: 18509 RVA: 0x000CEE95 File Offset: 0x000CD095
	public int OutputCellOne
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[0]);
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x0600484E RID: 18510 RVA: 0x000CEEA9 File Offset: 0x000CD0A9
	public int OutputCellTwo
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[1]);
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x0600484F RID: 18511 RVA: 0x000CEEBD File Offset: 0x000CD0BD
	public int OutputCellThree
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[2]);
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06004850 RID: 18512 RVA: 0x000CEED1 File Offset: 0x000CD0D1
	public int OutputCellFour
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[3]);
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06004851 RID: 18513 RVA: 0x000CEEE5 File Offset: 0x000CD0E5
	public int ControlCellOne
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[0]);
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06004852 RID: 18514 RVA: 0x000CEEF9 File Offset: 0x000CD0F9
	public int ControlCellTwo
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[1]);
		}
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x00254D08 File Offset: 0x00252F08
	public int PortCell(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.InputCellOne;
		case LogicGateBase.PortId.InputTwo:
			return this.InputCellTwo;
		case LogicGateBase.PortId.InputThree:
			return this.InputCellThree;
		case LogicGateBase.PortId.InputFour:
			return this.InputCellFour;
		case LogicGateBase.PortId.OutputOne:
			return this.OutputCellOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.OutputCellTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.OutputCellThree;
		case LogicGateBase.PortId.OutputFour:
			return this.OutputCellFour;
		case LogicGateBase.PortId.ControlOne:
			return this.ControlCellOne;
		case LogicGateBase.PortId.ControlTwo:
			return this.ControlCellTwo;
		default:
			return this.OutputCellOne;
		}
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x00254D94 File Offset: 0x00252F94
	public bool TryGetPortAtCell(int cell, out LogicGateBase.PortId port)
	{
		if (cell == this.InputCellOne)
		{
			port = LogicGateBase.PortId.InputOne;
			return true;
		}
		if ((this.RequiresTwoInputs || this.RequiresFourInputs) && cell == this.InputCellTwo)
		{
			port = LogicGateBase.PortId.InputTwo;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellThree)
		{
			port = LogicGateBase.PortId.InputThree;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellFour)
		{
			port = LogicGateBase.PortId.InputFour;
			return true;
		}
		if (cell == this.OutputCellOne)
		{
			port = LogicGateBase.PortId.OutputOne;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellTwo)
		{
			port = LogicGateBase.PortId.OutputTwo;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellThree)
		{
			port = LogicGateBase.PortId.OutputThree;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellFour)
		{
			port = LogicGateBase.PortId.OutputFour;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellOne)
		{
			port = LogicGateBase.PortId.ControlOne;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellTwo)
		{
			port = LogicGateBase.PortId.ControlTwo;
			return true;
		}
		port = LogicGateBase.PortId.InputOne;
		return false;
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06004855 RID: 18517 RVA: 0x000CEF0D File Offset: 0x000CD10D
	public bool RequiresTwoInputs
	{
		get
		{
			return LogicGateBase.OpRequiresTwoInputs(this.op);
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06004856 RID: 18518 RVA: 0x000CEF1A File Offset: 0x000CD11A
	public bool RequiresFourInputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourInputs(this.op);
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06004857 RID: 18519 RVA: 0x000CEF27 File Offset: 0x000CD127
	public bool RequiresFourOutputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourOutputs(this.op);
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06004858 RID: 18520 RVA: 0x000CEF34 File Offset: 0x000CD134
	public bool RequiresControlInputs
	{
		get
		{
			return LogicGateBase.OpRequiresControlInputs(this.op);
		}
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x000CEF41 File Offset: 0x000CD141
	public static bool OpRequiresTwoInputs(LogicGateBase.Op op)
	{
		return op != LogicGateBase.Op.Not && op - LogicGateBase.Op.CustomSingle > 2;
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x000CEF50 File Offset: 0x000CD150
	public static bool OpRequiresFourInputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x0600485B RID: 18523 RVA: 0x000CEF59 File Offset: 0x000CD159
	public static bool OpRequiresFourOutputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x000CEF62 File Offset: 0x000CD162
	public static bool OpRequiresControlInputs(LogicGateBase.Op op)
	{
		return op - LogicGateBase.Op.Multiplexer <= 1;
	}

	// Token: 0x04003224 RID: 12836
	public static LogicModeUI uiSrcData;

	// Token: 0x04003225 RID: 12837
	public static readonly HashedString OUTPUT_TWO_PORT_ID = new HashedString("LogicGateOutputTwo");

	// Token: 0x04003226 RID: 12838
	public static readonly HashedString OUTPUT_THREE_PORT_ID = new HashedString("LogicGateOutputThree");

	// Token: 0x04003227 RID: 12839
	public static readonly HashedString OUTPUT_FOUR_PORT_ID = new HashedString("LogicGateOutputFour");

	// Token: 0x04003228 RID: 12840
	[SerializeField]
	public LogicGateBase.Op op;

	// Token: 0x04003229 RID: 12841
	public static CellOffset[] portOffsets = new CellOffset[]
	{
		CellOffset.none,
		new CellOffset(0, 1),
		new CellOffset(1, 0)
	};

	// Token: 0x0400322A RID: 12842
	public CellOffset[] inputPortOffsets;

	// Token: 0x0400322B RID: 12843
	public CellOffset[] outputPortOffsets;

	// Token: 0x0400322C RID: 12844
	public CellOffset[] controlPortOffsets;

	// Token: 0x02000E42 RID: 3650
	public enum PortId
	{
		// Token: 0x0400322E RID: 12846
		InputOne,
		// Token: 0x0400322F RID: 12847
		InputTwo,
		// Token: 0x04003230 RID: 12848
		InputThree,
		// Token: 0x04003231 RID: 12849
		InputFour,
		// Token: 0x04003232 RID: 12850
		OutputOne,
		// Token: 0x04003233 RID: 12851
		OutputTwo,
		// Token: 0x04003234 RID: 12852
		OutputThree,
		// Token: 0x04003235 RID: 12853
		OutputFour,
		// Token: 0x04003236 RID: 12854
		ControlOne,
		// Token: 0x04003237 RID: 12855
		ControlTwo
	}

	// Token: 0x02000E43 RID: 3651
	public enum Op
	{
		// Token: 0x04003239 RID: 12857
		And,
		// Token: 0x0400323A RID: 12858
		Or,
		// Token: 0x0400323B RID: 12859
		Not,
		// Token: 0x0400323C RID: 12860
		Xor,
		// Token: 0x0400323D RID: 12861
		CustomSingle,
		// Token: 0x0400323E RID: 12862
		Multiplexer,
		// Token: 0x0400323F RID: 12863
		Demultiplexer
	}
}
