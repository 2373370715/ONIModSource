using System;
using STRINGS;

// Token: 0x020003DF RID: 991
public class LogicGateNotConfig : LogicGateBaseConfig
{
	// Token: 0x0600107C RID: 4220 RVA: 0x000A6603 File Offset: 0x000A4803
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Not;
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600107D RID: 4221 RVA: 0x000AD37C File Offset: 0x000AB57C
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none
			};
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600107E RID: 4222 RVA: 0x000AD31C File Offset: 0x000AB51C
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600107F RID: 4223 RVA: 0x000AD332 File Offset: 0x000AB532
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x001810B8 File Offset: 0x0017F2B8
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x000AD390 File Offset: 0x000AB590
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateNOT", "logic_not_kanim", 2, 1);
	}

	// Token: 0x04000B80 RID: 2944
	public const string ID = "LogicGateNOT";
}
