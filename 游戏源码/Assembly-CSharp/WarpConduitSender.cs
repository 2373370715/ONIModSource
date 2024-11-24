using System;
using UnityEngine;

// Token: 0x02001033 RID: 4147
public class WarpConduitSender : StateMachineComponent<WarpConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x0600549F RID: 21663 RVA: 0x0027BCE8 File Offset: 0x00279EE8
	private bool IsSending()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x0027BD3C File Offset: 0x00279F3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = base.GetComponents<Storage>();
		this.gasStorage = components[0];
		this.liquidStorage = components[1];
		this.solidStorage = components[2];
		this.gasPort = new WarpConduitSender.ConduitPort(base.gameObject, this.gasPortInfo, 1, this.gasStorage);
		this.liquidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.liquidPortInfo, 2, this.liquidStorage);
		this.solidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.solidPortInfo, 3, this.solidStorage);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
		this.FindPartner();
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
		base.smi.StartSM();
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x000D7282 File Offset: 0x000D5482
	public void OnActivatedChanged(object data)
	{
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x0027BE90 File Offset: 0x0027A090
	private void FindPartner()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitReceiver");
		foreach (WarpConduitReceiver component in UnityEngine.Object.FindObjectsOfType<WarpConduitReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				this.receiver = component;
				break;
			}
		}
		if (this.receiver == null)
		{
			global::Debug.LogWarning("No warp conduit receiver found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.receiver.SetStorage(this.gasStorage, this.liquidStorage, this.solidStorage);
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x0027BF18 File Offset: 0x0027A118
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.inputCell, this.liquidPort.networkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasPort.inputCell, this.gasPort.networkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.inputCell, this.solidPort.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x000D72AB File Offset: 0x000D54AB
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x0027BFAC File Offset: 0x0027A1AC
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04003B4C RID: 15180
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003B4D RID: 15181
	public Storage gasStorage;

	// Token: 0x04003B4E RID: 15182
	public Storage liquidStorage;

	// Token: 0x04003B4F RID: 15183
	public Storage solidStorage;

	// Token: 0x04003B50 RID: 15184
	public WarpConduitReceiver receiver;

	// Token: 0x04003B51 RID: 15185
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04003B52 RID: 15186
	private WarpConduitSender.ConduitPort liquidPort;

	// Token: 0x04003B53 RID: 15187
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04003B54 RID: 15188
	private WarpConduitSender.ConduitPort gasPort;

	// Token: 0x04003B55 RID: 15189
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04003B56 RID: 15190
	private WarpConduitSender.ConduitPort solidPort;

	// Token: 0x02001034 RID: 4148
	private class ConduitPort
	{
		// Token: 0x060054A7 RID: 21671 RVA: 0x0027C00C File Offset: 0x0027A20C
		public ConduitPort(GameObject parent, ConduitPortInfo info, int number, Storage targetStorage)
		{
			this.portInfo = info;
			this.inputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
				conduitConsumer.conduitType = this.portInfo.conduitType;
				conduitConsumer.useSecondaryInput = true;
				conduitConsumer.storage = targetStorage;
				conduitConsumer.capacityKG = targetStorage.capacityKg;
				conduitConsumer.alwaysConsume = false;
				this.conduitConsumer = conduitConsumer;
				this.conduitConsumer.keepZeroMassObject = false;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell, parent);
				networkManager.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			else
			{
				this.solidConsumer = parent.AddComponent<SolidConduitConsumer>();
				this.solidConsumer.useSecondaryInput = true;
				this.solidConsumer.storage = targetStorage;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, this.inputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			string meter_animation = "airlock_" + number.ToString();
			string text = "airlock_target_" + number.ToString();
			this.pre = "airlock_" + number.ToString() + "_pre";
			this.loop = "airlock_" + number.ToString() + "_loop";
			this.pst = "airlock_" + number.ToString() + "_pst";
			this.airlock = new MeterController(parent.GetComponent<KBatchedAnimController>(), text, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				text
			});
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x0027C1D0 File Offset: 0x0027A3D0
		public bool IsOn()
		{
			if (this.solidConsumer != null)
			{
				return this.solidConsumer.IsConsuming;
			}
			return this.conduitConsumer != null && (this.conduitConsumer.IsConnected && this.conduitConsumer.IsSatisfied) && this.conduitConsumer.consumedLastTick;
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x0027C230 File Offset: 0x0027A430
		public void Update()
		{
			bool flag = this.IsOn();
			if (flag != this.open)
			{
				this.open = flag;
				if (this.open)
				{
					this.airlock.meterController.Play(this.pre, KAnim.PlayMode.Once, 1f, 0f);
					this.airlock.meterController.Queue(this.loop, KAnim.PlayMode.Loop, 1f, 0f);
					return;
				}
				this.airlock.meterController.Play(this.pst, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x04003B57 RID: 15191
		public ConduitPortInfo portInfo;

		// Token: 0x04003B58 RID: 15192
		public int inputCell;

		// Token: 0x04003B59 RID: 15193
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04003B5A RID: 15194
		private ConduitConsumer conduitConsumer;

		// Token: 0x04003B5B RID: 15195
		public SolidConduitConsumer solidConsumer;

		// Token: 0x04003B5C RID: 15196
		public MeterController airlock;

		// Token: 0x04003B5D RID: 15197
		private bool open;

		// Token: 0x04003B5E RID: 15198
		private string pre;

		// Token: 0x04003B5F RID: 15199
		private string loop;

		// Token: 0x04003B60 RID: 15200
		private string pst;
	}

	// Token: 0x02001035 RID: 4149
	public class StatesInstance : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameInstance
	{
		// Token: 0x060054AA RID: 21674 RVA: 0x000D72E1 File Offset: 0x000D54E1
		public StatesInstance(WarpConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x02001036 RID: 4150
	public class States : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender>
	{
		// Token: 0x060054AB RID: 21675 RVA: 0x0027C2D4 File Offset: 0x0027A4D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitSender.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}).EventTransition(GameHashes.OperationalChanged, this.on, (WarpConduitSender.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.waiting).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (!smi.master.IsSending())
				{
					smi.GoTo(this.on.waiting);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
			this.on.waiting.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (smi.master.IsSending())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x04003B61 RID: 15201
		public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State off;

		// Token: 0x04003B62 RID: 15202
		public WarpConduitSender.States.onStates on;

		// Token: 0x02001037 RID: 4151
		public class onStates : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State
		{
			// Token: 0x04003B63 RID: 15203
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State working;

			// Token: 0x04003B64 RID: 15204
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State waiting;
		}
	}
}
