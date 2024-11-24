using System;
using STRINGS;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class LogicGateBufferConfig : LogicGateBaseConfig
{
	// Token: 0x06001083 RID: 4227 RVA: 0x000AD3A4 File Offset: 0x000AB5A4
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06001084 RID: 4228 RVA: 0x000AD37C File Offset: 0x000AB57C
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

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06001085 RID: 4229 RVA: 0x000AD31C File Offset: 0x000AB51C
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

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06001086 RID: 4230 RVA: 0x000AD332 File Offset: 0x000AB532
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x00181108 File Offset: 0x0017F308
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x000AD3A7 File Offset: 0x000AB5A7
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", 2, 1);
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x00181158 File Offset: 0x0017F358
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateBuffer logicGateBuffer = go.AddComponent<LogicGateBuffer>();
		logicGateBuffer.op = this.GetLogicOp();
		logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
		logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
		logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000B81 RID: 2945
	public const string ID = "LogicGateBUFFER";
}
