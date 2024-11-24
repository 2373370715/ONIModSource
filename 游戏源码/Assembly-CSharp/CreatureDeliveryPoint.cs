using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

public class CreatureDeliveryPoint : StateMachineComponent<CreatureDeliveryPoint.SMInstance>
{
	public class SMInstance : GameStateMachine<States, SMInstance, CreatureDeliveryPoint, object>.GameInstance
	{
		public SMInstance(CreatureDeliveryPoint master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, SMInstance, CreatureDeliveryPoint>
	{
		public class OperationalState : State
		{
			public State waiting;

			public State interact_waiting;

			public State interact_delivery;
		}

		public OperationalState operational;

		public State unoperational;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = operational.waiting;
			root.Update("RefreshCreatureCount", delegate(SMInstance smi, float dt)
			{
				smi.master.critterCapacity.RefreshCreatureCount();
			}, UpdateRate.SIM_1000ms).EventHandler(GameHashes.OnStorageChange, DropAllCreatures);
			unoperational.EventTransition(GameHashes.LogicEvent, operational, (SMInstance smi) => smi.master.LogicEnabled());
			operational.EventTransition(GameHashes.LogicEvent, unoperational, (SMInstance smi) => !smi.master.LogicEnabled());
			operational.waiting.EnterTransition(operational.interact_waiting, (SMInstance smi) => smi.master.playAnimsOnFetch);
			operational.interact_waiting.WorkableStartTransition((SMInstance smi) => smi.master.GetComponent<Storage>(), operational.interact_delivery);
			operational.interact_delivery.PlayAnim("working_pre").QueueAnim("working_pst").OnAnimQueueComplete(operational.interact_waiting);
		}

		public static void DropAllCreatures(SMInstance smi)
		{
			Storage component = smi.master.GetComponent<Storage>();
			if (!component.IsEmpty())
			{
				List<GameObject> items = component.items;
				int count = items.Count;
				Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), smi.master.spawnOffset), Grid.SceneLayer.Creatures);
				for (int num = count - 1; num >= 0; num--)
				{
					GameObject gameObject = items[num];
					component.Drop(gameObject);
					gameObject.transform.SetPosition(position);
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				}
				smi.master.critterCapacity.RefreshCreatureCount();
			}
		}
	}

	[MyCmpAdd]
	private Prioritizable prioritizable;

	[MyCmpReq]
	public BaggableCritterCapacityTracker critterCapacity;

	[Obsolete]
	[Serialize]
	private int creatureLimit = 20;

	public CellOffset[] deliveryOffsets = new CellOffset[1];

	public CellOffset spawnOffset = new CellOffset(0, 0);

	private List<FetchOrder2> fetches;

	public bool playAnimsOnFetch;

	private LogicPorts logicPorts;

	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.OnCopySettings(data);
	});

	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> RefreshCreatureCountDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.critterCapacity.RefreshCreatureCount(data);
	});

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		fetches = new List<FetchOrder2>();
		TreeFilterable component = GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(OnFilterChanged));
		GetComponent<Storage>().SetOffsets(deliveryOffsets);
		Prioritizable.AddRef(base.gameObject);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Subscribe(-905833192, OnCopySettingsDelegate);
		Subscribe(643180843, RefreshCreatureCountDelegate);
		critterCapacity = GetComponent<BaggableCritterCapacityTracker>();
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = critterCapacity;
		baggableCritterCapacityTracker.onCountChanged = (System.Action)Delegate.Combine(baggableCritterCapacityTracker.onCountChanged, new System.Action(RebalanceFetches));
		critterCapacity.RefreshCreatureCount();
		logicPorts = GetComponent<LogicPorts>();
		if (logicPorts != null)
		{
			logicPorts.Subscribe(-801688580, OnLogicChanged);
		}
	}

	private void OnLogicChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == "CritterDropOffInput")
		{
			if (logicValueChanged.newValue > 0)
			{
				RebalanceFetches();
			}
			else
			{
				ClearFetches();
			}
		}
	}

	[Obsolete]
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (critterCapacity != null && creatureLimit > 0)
		{
			critterCapacity.creatureLimit = creatureLimit;
			creatureLimit = -1;
		}
	}

	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (!(gameObject == null) && !(gameObject.GetComponent<CreatureDeliveryPoint>() == null))
		{
			RebalanceFetches();
		}
	}

	private void OnFilterChanged(HashSet<Tag> tags)
	{
		ClearFetches();
		RebalanceFetches();
	}

	private void ClearFetches()
	{
		for (int num = fetches.Count - 1; num >= 0; num--)
		{
			fetches[num].Cancel("clearing all fetches");
		}
		fetches.Clear();
	}

	private void RebalanceFetches()
	{
		if (!LogicEnabled())
		{
			return;
		}
		HashSet<Tag> tags = GetComponent<TreeFilterable>().GetTags();
		ChoreType creatureFetch = Db.Get().ChoreTypes.CreatureFetch;
		Storage component = GetComponent<Storage>();
		int num = critterCapacity.creatureLimit - critterCapacity.storedCreatureCount;
		_ = fetches.Count;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int num6 = fetches.Count - 1; num6 >= 0; num6--)
		{
			if (fetches[num6].IsComplete())
			{
				fetches.RemoveAt(num6);
				num2++;
			}
		}
		int num7 = 0;
		for (int i = 0; i < fetches.Count; i++)
		{
			if (!fetches[i].InProgress)
			{
				num7++;
			}
		}
		if (num7 == 0 && fetches.Count < num)
		{
			FetchOrder2 fetchOrder = new FetchOrder2(creatureFetch, tags, FetchChore.MatchCriteria.MatchID, GameTags.Creatures.Deliverable, null, component, 1f, Operational.State.Operational);
			fetchOrder.validateRequiredTagOnTagChange = true;
			fetchOrder.Submit(OnFetchComplete, check_storage_contents: false, OnFetchBegun);
			fetches.Add(fetchOrder);
			num3++;
		}
		int num8 = fetches.Count - num;
		int num9 = fetches.Count - 1;
		while (num9 >= 0 && num8 > 0)
		{
			if (!fetches[num9].InProgress)
			{
				fetches[num9].Cancel("fewer creatures in room");
				fetches.RemoveAt(num9);
				num8--;
				num4++;
			}
			num9--;
		}
		while (num8 > 0 && fetches.Count > 0)
		{
			fetches[fetches.Count - 1].Cancel("fewer creatures in room");
			fetches.RemoveAt(fetches.Count - 1);
			num8--;
			num5++;
		}
	}

	private void OnFetchComplete(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		RebalanceFetches();
	}

	private void OnFetchBegun(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		RebalanceFetches();
	}

	protected override void OnCleanUp()
	{
		base.smi.StopSM("OnCleanUp");
		TreeFilterable component = GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(component.OnFilterChanged, new Action<HashSet<Tag>>(OnFilterChanged));
		base.OnCleanUp();
	}

	public bool LogicEnabled()
	{
		if (!(logicPorts == null) && logicPorts.IsPortConnected("CritterDropOffInput"))
		{
			return logicPorts.GetInputValue("CritterDropOffInput") == 1;
		}
		return true;
	}
}
