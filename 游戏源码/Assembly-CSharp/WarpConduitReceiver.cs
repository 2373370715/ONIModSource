using UnityEngine;

public class WarpConduitReceiver : StateMachineComponent<WarpConduitReceiver.StatesInstance>, ISecondaryOutput
{
	public struct ConduitPort
	{
		public ConduitPortInfo portInfo;

		public int outputCell;

		public FlowUtilityNetwork.NetworkItem networkItem;

		public ConduitDispenser dispenser;

		public SolidConduitDispenser solidDispenser;

		public MeterController airlock;

		private bool open;

		private string pre;

		private string loop;

		private string pst;

		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage, int number)
		{
			portInfo = info;
			outputCell = Grid.OffsetCell(Grid.PosToCell(parent), portInfo.offset);
			if (portInfo.conduitType != ConduitType.Solid)
			{
				ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
				conduitDispenser.conduitType = portInfo.conduitType;
				conduitDispenser.useSecondaryOutput = true;
				conduitDispenser.alwaysDispense = true;
				conduitDispenser.storage = senderStorage;
				dispenser = conduitDispenser;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(portInfo.conduitType);
				networkItem = new FlowUtilityNetwork.NetworkItem(portInfo.conduitType, Endpoint.Source, outputCell, parent);
				networkManager.AddToNetworks(outputCell, networkItem, is_endpoint: true);
			}
			else
			{
				SolidConduitDispenser solidConduitDispenser = parent.AddComponent<SolidConduitDispenser>();
				solidConduitDispenser.storage = senderStorage;
				solidConduitDispenser.alwaysDispense = true;
				solidConduitDispenser.useSecondaryOutput = true;
				solidConduitDispenser.solidOnly = true;
				solidDispenser = solidConduitDispenser;
				networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, outputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(outputCell, networkItem, is_endpoint: true);
			}
			string meter_animation = "airlock_" + number;
			string text = "airlock_target_" + number;
			pre = "airlock_" + number + "_pre";
			loop = "airlock_" + number + "_loop";
			pst = "airlock_" + number + "_pst";
			airlock = new MeterController(parent.GetComponent<KBatchedAnimController>(), text, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, text);
		}

		public bool IsOn()
		{
			if (solidDispenser != null)
			{
				return solidDispenser.IsDispensing;
			}
			if (dispenser != null)
			{
				if (!dispenser.blocked)
				{
					return !dispenser.empty;
				}
				return false;
			}
			return false;
		}

		public void UpdatePortAnim()
		{
			bool flag = IsOn();
			if (flag != open)
			{
				open = flag;
				if (open)
				{
					airlock.meterController.Play(pre);
					airlock.meterController.Queue(loop, KAnim.PlayMode.Loop);
				}
				else
				{
					airlock.meterController.Play(pst);
				}
			}
		}
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, WarpConduitReceiver, object>.GameInstance
	{
		public StatesInstance(WarpConduitReceiver master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, WarpConduitReceiver>
	{
		public class onStates : State
		{
			public State working;

			public State idle;
		}

		public State off;

		public onStates on;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;
			base.serializable = SerializeType.Both_DEPRECATED;
			root.EventHandler(GameHashes.BuildingActivated, delegate(StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			off.PlayAnim("off").Enter(delegate(StatesInstance smi)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}).EventTransition(GameHashes.OperationalFlagChanged, on, (StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			on.DefaultState(on.idle).Update(delegate(StatesInstance smi, float dt)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}, UpdateRate.SIM_1000ms);
			on.idle.QueueAnim("idle").ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal).Update(delegate(StatesInstance smi, float dt)
			{
				if (smi.master.IsReceiving())
				{
					smi.GoTo(on.working);
				}
			}, UpdateRate.SIM_1000ms);
			on.working.PlayAnim("working_pre").QueueAnim("working_loop", loop: true).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working)
				.Update(delegate(StatesInstance smi, float dt)
				{
					if (!smi.master.IsReceiving())
					{
						smi.GoTo(on.idle);
					}
				}, UpdateRate.SIM_1000ms)
				.Exit(delegate(StatesInstance smi)
				{
					smi.Play("working_pst");
				});
		}
	}

	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	private ConduitPort liquidPort;

	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	private ConduitPort solidPort;

	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	private ConduitPort gasPort;

	public Storage senderGasStorage;

	public Storage senderLiquidStorage;

	public Storage senderSolidStorage;

	private bool IsReceiving()
	{
		if (!base.smi.master.gasPort.IsOn() && !base.smi.master.liquidPort.IsOn())
		{
			return base.smi.master.solidPort.IsOn();
		}
		return true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		FindPartner();
		base.smi.StartSM();
	}

	private void FindPartner()
	{
		if (senderGasStorage != null)
		{
			return;
		}
		WarpConduitSender warpConduitSender = null;
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitSender");
		WarpConduitSender[] array = Object.FindObjectsOfType<WarpConduitSender>();
		foreach (WarpConduitSender warpConduitSender2 in array)
		{
			if (warpConduitSender2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpConduitSender = warpConduitSender2;
				break;
			}
		}
		if (warpConduitSender == null)
		{
			Debug.LogWarning("No warp conduit sender found - maybe POI stomping or failure to spawn?");
			return;
		}
		SetStorage(warpConduitSender.gasStorage, warpConduitSender.liquidStorage, warpConduitSender.solidStorage);
		WarpConduitStatus.UpdateWarpConduitsOperational(warpConduitSender.gameObject, base.gameObject);
	}

	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(liquidPortInfo.conduitType).RemoveFromNetworks(liquidPort.outputCell, liquidPort.networkItem, is_endpoint: true);
		if (gasPort.portInfo != null)
		{
			Conduit.GetNetworkManager(gasPort.portInfo.conduitType).RemoveFromNetworks(gasPort.outputCell, gasPort.networkItem, is_endpoint: true);
		}
		else
		{
			Debug.LogWarning("Conduit Receiver gasPort portInfo is null in OnCleanUp");
		}
		Game.Instance.solidConduitSystem.RemoveFromNetworks(solidPort.outputCell, solidPort.networkItem, is_endpoint: true);
		base.OnCleanUp();
	}

	public void OnActivatedChanged(object data)
	{
		if (senderGasStorage == null)
		{
			FindPartner();
		}
		WarpConduitStatus.UpdateWarpConduitsOperational((senderGasStorage != null) ? senderGasStorage.gameObject : null, base.gameObject);
	}

	public void SetStorage(Storage gasStorage, Storage liquidStorage, Storage solidStorage)
	{
		senderGasStorage = gasStorage;
		senderLiquidStorage = liquidStorage;
		senderSolidStorage = solidStorage;
		gasPort.SetPortInfo(base.gameObject, gasPortInfo, gasStorage, 1);
		liquidPort.SetPortInfo(base.gameObject, liquidPortInfo, liquidStorage, 2);
		solidPort.SetPortInfo(base.gameObject, solidPortInfo, solidStorage, 3);
		Vector3 position = liquidPort.airlock.gameObject.transform.position;
		liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
	}

	public bool HasSecondaryConduitType(ConduitType type)
	{
		if (type != gasPortInfo.conduitType && type != liquidPortInfo.conduitType)
		{
			return type == solidPortInfo.conduitType;
		}
		return true;
	}

	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == gasPortInfo.conduitType)
		{
			return gasPortInfo.offset;
		}
		if (type == liquidPortInfo.conduitType)
		{
			return liquidPortInfo.offset;
		}
		if (type == solidPortInfo.conduitType)
		{
			return solidPortInfo.offset;
		}
		return CellOffset.none;
	}
}
