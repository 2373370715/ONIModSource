using System;
using STRINGS;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class LogicGateFilterConfig : LogicGateBaseConfig
{
	// Token: 0x0600108C RID: 4236 RVA: 0x000AD3A4 File Offset: 0x000AB5A4
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600108D RID: 4237 RVA: 0x000AD37C File Offset: 0x000AB57C
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

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600108E RID: 4238 RVA: 0x000AD31C File Offset: 0x000AB51C
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

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600108F RID: 4239 RVA: 0x000AD332 File Offset: 0x000AB532
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x001811C4 File Offset: 0x0017F3C4
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x000AD3CE File Offset: 0x000AB5CE
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", 2, 1);
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x00181214 File Offset: 0x0017F414
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
		logicGateFilter.op = this.GetLogicOp();
		logicGateFilter.inputPortOffsets = this.InputPortOffsets;
		logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
		logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000B82 RID: 2946
	public const string ID = "LogicGateFILTER";
}
