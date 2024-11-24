using System;
using STRINGS;

// Token: 0x020003DD RID: 989
public class LogicGateOrConfig : LogicGateBaseConfig
{
	// Token: 0x0600106E RID: 4206 RVA: 0x000A65EC File Offset: 0x000A47EC
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Or;
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x000AD2FA File Offset: 0x000AB4FA
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none,
				new CellOffset(0, 1)
			};
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06001070 RID: 4208 RVA: 0x000AD31C File Offset: 0x000AB51C
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

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x000AD332 File Offset: 0x000AB532
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x00181018 File Offset: 0x0017F218
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x000AD351 File Offset: 0x000AB551
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateOR", "logic_or_kanim", 2, 2);
	}

	// Token: 0x04000B7E RID: 2942
	public const string ID = "LogicGateOR";
}
