using System;
using STRINGS;

// Token: 0x020003E3 RID: 995
public class LogicGateDemultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x0600109C RID: 4252 RVA: 0x000AD486 File Offset: 0x000AB686
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600109D RID: 4253 RVA: 0x000AD489 File Offset: 0x000AB689
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3)
			};
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600109E RID: 4254 RVA: 0x000AD49F File Offset: 0x000AB69F
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600109F RID: 4255 RVA: 0x000AD4DF File Offset: 0x000AB6DF
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(0, 0)
			};
		}
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x00181068 File Offset: 0x0017F268
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x000AD503 File Offset: 0x000AB703
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateDemultiplexer", "logic_demultiplexer_kanim", 3, 4);
	}

	// Token: 0x04000B84 RID: 2948
	public const string ID = "LogicGateDemultiplexer";
}
