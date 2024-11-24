using System;
using STRINGS;

// Token: 0x020003DC RID: 988
public class LogicGateAndConfig : LogicGateBaseConfig
{
	// Token: 0x06001067 RID: 4199 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.And;
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x000AD2FA File Offset: 0x000AB4FA
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

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x000AD31C File Offset: 0x000AB51C
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

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600106A RID: 4202 RVA: 0x000AD332 File Offset: 0x000AB532
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00180FC8 File Offset: 0x0017F1C8
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x000AD335 File Offset: 0x000AB535
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateAND", "logic_and_kanim", 2, 2);
	}

	// Token: 0x04000B7D RID: 2941
	public const string ID = "LogicGateAND";
}
