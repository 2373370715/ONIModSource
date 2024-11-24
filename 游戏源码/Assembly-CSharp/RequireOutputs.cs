using System;
using UnityEngine;

// Token: 0x0200179A RID: 6042
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireOutputs")]
public class RequireOutputs : KMonoBehaviour
{
	// Token: 0x06007C65 RID: 31845 RVA: 0x00320F20 File Offset: 0x0031F120
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ScenePartitionerLayer scenePartitionerLayer = null;
		Building component = base.GetComponent<Building>();
		this.utilityCell = component.GetUtilityOutputCell();
		this.conduitType = component.Def.OutputConduitType;
		switch (component.Def.OutputConduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		this.UpdateConnectionState(true);
		this.UpdatePipeRoomState(true);
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("RequireOutputs", base.gameObject, this.utilityCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConnectionState(false);
			});
		}
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.UpdatePipeState), ConduitFlowPriority.First);
	}

	// Token: 0x06007C66 RID: 31846 RVA: 0x00320FF8 File Offset: 0x0031F1F8
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		IConduitFlow conduitFlow = this.GetConduitFlow();
		if (conduitFlow != null)
		{
			conduitFlow.RemoveConduitUpdater(new Action<float>(this.UpdatePipeState));
		}
		base.OnCleanUp();
	}

	// Token: 0x06007C67 RID: 31847 RVA: 0x00321038 File Offset: 0x0031F238
	private void UpdateConnectionState(bool force_update = false)
	{
		this.connected = this.IsConnected(this.utilityCell);
		if (this.connected != this.previouslyConnected || force_update)
		{
			this.operational.SetFlag(RequireOutputs.outputConnectedFlag, this.connected);
			this.previouslyConnected = this.connected;
			StatusItem status_item = null;
			switch (this.conduitType)
			{
			case ConduitType.Gas:
				status_item = Db.Get().BuildingStatusItems.NeedGasOut;
				break;
			case ConduitType.Liquid:
				status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
				break;
			case ConduitType.Solid:
				status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
				break;
			}
			this.hasPipeGuid = this.selectable.ToggleStatusItem(status_item, this.hasPipeGuid, !this.connected, this);
		}
	}

	// Token: 0x06007C68 RID: 31848 RVA: 0x00321108 File Offset: 0x0031F308
	private bool OutputPipeIsEmpty()
	{
		if (this.ignoreFullPipe)
		{
			return true;
		}
		bool result = true;
		if (this.connected)
		{
			result = this.GetConduitFlow().IsConduitEmpty(this.utilityCell);
		}
		return result;
	}

	// Token: 0x06007C69 RID: 31849 RVA: 0x000F1F08 File Offset: 0x000F0108
	private void UpdatePipeState(float dt)
	{
		this.UpdatePipeRoomState(false);
	}

	// Token: 0x06007C6A RID: 31850 RVA: 0x0032113C File Offset: 0x0031F33C
	private void UpdatePipeRoomState(bool force_update = false)
	{
		bool flag = this.OutputPipeIsEmpty();
		if (flag != this.previouslyHadRoom || force_update)
		{
			this.operational.SetFlag(RequireOutputs.pipesHaveRoomFlag, flag);
			this.previouslyHadRoom = flag;
			StatusItem status_item = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
			if (this.conduitType == ConduitType.Solid)
			{
				status_item = Db.Get().BuildingStatusItems.SolidConduitBlockedMultiples;
			}
			this.pipeBlockedGuid = this.selectable.ToggleStatusItem(status_item, this.pipeBlockedGuid, !flag, null);
		}
	}

	// Token: 0x06007C6B RID: 31851 RVA: 0x003211C0 File Offset: 0x0031F3C0
	private IConduitFlow GetConduitFlow()
	{
		switch (this.conduitType)
		{
		case ConduitType.Gas:
			return Game.Instance.gasConduitFlow;
		case ConduitType.Liquid:
			return Game.Instance.liquidConduitFlow;
		case ConduitType.Solid:
			return Game.Instance.solidConduitFlow;
		default:
			global::Debug.LogWarning("GetConduitFlow() called with unexpected conduitType: " + this.conduitType.ToString());
			return null;
		}
	}

	// Token: 0x06007C6C RID: 31852 RVA: 0x000F1F11 File Offset: 0x000F0111
	private bool IsConnected(int cell)
	{
		return RequireOutputs.IsConnected(cell, this.conduitType);
	}

	// Token: 0x06007C6D RID: 31853 RVA: 0x0032122C File Offset: 0x0031F42C
	public static bool IsConnected(int cell, ConduitType conduitType)
	{
		ObjectLayer layer = ObjectLayer.NumLayers;
		switch (conduitType)
		{
		case ConduitType.Gas:
			layer = ObjectLayer.GasConduit;
			break;
		case ConduitType.Liquid:
			layer = ObjectLayer.LiquidConduit;
			break;
		case ConduitType.Solid:
			layer = ObjectLayer.SolidConduit;
			break;
		}
		GameObject gameObject = Grid.Objects[cell, (int)layer];
		return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
	}

	// Token: 0x04005E23 RID: 24099
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005E24 RID: 24100
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04005E25 RID: 24101
	public bool ignoreFullPipe;

	// Token: 0x04005E26 RID: 24102
	private int utilityCell;

	// Token: 0x04005E27 RID: 24103
	private ConduitType conduitType;

	// Token: 0x04005E28 RID: 24104
	private static readonly Operational.Flag outputConnectedFlag = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);

	// Token: 0x04005E29 RID: 24105
	private static readonly Operational.Flag pipesHaveRoomFlag = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);

	// Token: 0x04005E2A RID: 24106
	private bool previouslyConnected = true;

	// Token: 0x04005E2B RID: 24107
	private bool previouslyHadRoom = true;

	// Token: 0x04005E2C RID: 24108
	private bool connected;

	// Token: 0x04005E2D RID: 24109
	private Guid hasPipeGuid;

	// Token: 0x04005E2E RID: 24110
	private Guid pipeBlockedGuid;

	// Token: 0x04005E2F RID: 24111
	private HandleVector<int>.Handle partitionerEntry;
}
