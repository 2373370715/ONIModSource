using System;
using STRINGS;

// Token: 0x020003E2 RID: 994
public class LogicGateMultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06001095 RID: 4245 RVA: 0x000AD3F5 File Offset: 0x000AB5F5
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06001096 RID: 4246 RVA: 0x000AD3F8 File Offset: 0x000AB5F8
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3),
				new CellOffset(-1, 2),
				new CellOffset(-1, 1),
				new CellOffset(-1, 0)
			};
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06001097 RID: 4247 RVA: 0x000AD438 File Offset: 0x000AB638
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3)
			};
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06001098 RID: 4248 RVA: 0x000AD44E File Offset: 0x000AB64E
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00181280 File Offset: 0x0017F480
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x000AD472 File Offset: 0x000AB672
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateMultiplexer", "logic_multiplexer_kanim", 3, 4);
	}

	// Token: 0x04000B83 RID: 2947
	public const string ID = "LogicGateMultiplexer";
}
