using System;
using STRINGS;

// Token: 0x020003DE RID: 990
public class LogicGateXorConfig : LogicGateBaseConfig
{
	// Token: 0x06001075 RID: 4213 RVA: 0x000AD365 File Offset: 0x000AB565
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Xor;
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06001076 RID: 4214 RVA: 0x000AD2FA File Offset: 0x000AB4FA
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

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06001077 RID: 4215 RVA: 0x000AD31C File Offset: 0x000AB51C
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

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06001078 RID: 4216 RVA: 0x000AD332 File Offset: 0x000AB532
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x00181068 File Offset: 0x0017F268
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

	// Token: 0x0600107A RID: 4218 RVA: 0x000AD368 File Offset: 0x000AB568
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateXOR", "logic_xor_kanim", 2, 2);
	}

	// Token: 0x04000B7F RID: 2943
	public const string ID = "LogicGateXOR";
}
