using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

public class FetchChore : Chore<FetchChore.StatesInstance>
{
	public enum MatchCriteria
	{
		MatchID,
		MatchTags
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, FetchChore, object>.GameInstance
	{
		public StatesInstance(FetchChore master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, FetchChore>
	{
		public TargetParameter fetcher;

		public TargetParameter source;

		public TargetParameter chunk;

		public FloatParameter requestedamount;

		public FloatParameter actualamount;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
		}
	}

	public HashSet<Tag> tags;

	public Tag tagsFirst;

	public MatchCriteria criteria;

	public int tagsHash;

	public bool validateRequiredTagOnTagChange;

	public Tag requiredTag;

	public Tag[] forbiddenTags;

	public int forbidHash;

	public Automatable automatable;

	public bool allowMultifetch = true;

	private HandleVector<int>.Handle partitionerEntry;

	public static readonly Precondition IsFetchTargetAvailable = new Precondition
	{
		id = "IsFetchTargetAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
		fn = delegate(ref Precondition.Context context, object data)
		{
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag = false;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = pickupable != null;
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			if (flag)
			{
				if (pickupable == null)
				{
					Debug.Log($"Failed to find fetch target for {fetchChore.destination}");
					return false;
				}
				context.data = pickupable;
				if (context.consumerState.consumer.GetNavigationCost(pickupable, out var cost))
				{
					context.cost += cost;
					return true;
				}
			}
			return false;
		}
	};

	public float originalAmount => base.smi.sm.requestedamount.Get(base.smi);

	public float amount
	{
		get
		{
			return base.smi.sm.actualamount.Get(base.smi);
		}
		set
		{
			base.smi.sm.actualamount.Set(value, base.smi);
		}
	}

	public Pickupable fetchTarget
	{
		get
		{
			return base.smi.sm.chunk.Get<Pickupable>(base.smi);
		}
		set
		{
			base.smi.sm.chunk.Set(value, base.smi);
		}
	}

	public GameObject fetcher
	{
		get
		{
			return base.smi.sm.fetcher.Get(base.smi);
		}
		set
		{
			base.smi.sm.fetcher.Set(value, base.smi);
		}
	}

	public Storage destination { get; private set; }

	public void FetchAreaBegin(Precondition.Context context, float amount_to_be_fetched)
	{
		amount = amount_to_be_fetched;
		base.smi.sm.fetcher.Set(context.consumerState.gameObject, base.smi);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName(this, context.data));
		base.Begin(context);
	}

	public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
	{
		if (is_success)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, base.choreType.Name, GameUtil.GetChoreName(this, pickupable));
			fetchTarget = pickupable;
			base.driver = driver;
			fetcher = driver.gameObject;
			Succeed("FetchAreaEnd");
			SaveGame.Instance.ColonyAchievementTracker.LogFetchChore(fetcher, base.choreType);
		}
		else
		{
			SetOverrideTarget(null);
			Fail("FetchAreaFail");
		}
	}

	public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
	{
		if (destination != null)
		{
			if (consumer_state.hasSolidTransferArm)
			{
				return consumer_state.solidTransferArm.FindFetchTarget(destination, this);
			}
			return Game.Instance.fetchManager.FindFetchTarget(destination, this);
		}
		return null;
	}

	public override void Begin(Precondition.Context context)
	{
		Pickupable pickupable = (Pickupable)context.data;
		if (pickupable == null)
		{
			pickupable = FindFetchTarget(context.consumerState);
		}
		base.smi.sm.source.Set(pickupable.gameObject, base.smi);
		pickupable.Subscribe(-1582839653, OnTagsChanged);
		base.Begin(context);
	}

	protected override void End(string reason)
	{
		Pickupable pickupable = base.smi.sm.source.Get<Pickupable>(base.smi);
		if (pickupable != null)
		{
			pickupable.Unsubscribe(-1582839653, OnTagsChanged);
		}
		base.End(reason);
	}

	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.chunk.Get(base.smi) != null)
		{
			Fail("Tags changed");
		}
	}

	public override void PrepareChore(ref Precondition.Context context)
	{
		context.chore = new FetchAreaChore(context);
	}

	public float AmountWaitingToFetch()
	{
		if (fetcher == null)
		{
			return originalAmount;
		}
		return amount;
	}

	public FetchChore(ChoreType choreType, Storage destination, float amount, HashSet<Tag> tags, MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags = null, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, Operational.State operational_requirement = Operational.State.Operational, int priority_mod = 0)
		: base(choreType, (IStateMachineTarget)destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, is_preemptable: false, allow_in_context_menu: true, priority_mod, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		if (choreType == null)
		{
			Debug.LogError("You must specify a chore type for fetching!");
		}
		tagsFirst = ((tags.Count > 0) ? tags.First() : Tag.Invalid);
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(string.Format("Chore {0} is requesting {1} {2} to {3}", choreType.Id, tagsFirst, amount, (destination != null) ? destination.name : "to nowhere"));
		}
		SetPrioritizable((destination.prioritizable != null) ? destination.prioritizable : destination.GetComponent<Prioritizable>());
		base.smi = new StatesInstance(this);
		base.smi.sm.requestedamount.Set(amount, base.smi);
		this.destination = destination;
		DebugUtil.DevAssert(criteria != MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!");
		this.tags = tags;
		this.criteria = criteria;
		tagsHash = ComputeHashCodeForTags(tags);
		requiredTag = required_tag;
		forbiddenTags = ((forbidden_tags != null) ? forbidden_tags : new Tag[0]);
		forbidHash = ComputeHashCodeForTags(forbiddenTags);
		DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.");
		if (destination.GetOnlyFetchMarkedItems())
		{
			DebugUtil.DevAssert(!requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!");
			requiredTag = GameTags.Garbage;
		}
		AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		AddPrecondition(ChorePreconditions.instance.CanMoveTo, destination);
		AddPrecondition(IsFetchTargetAvailable);
		Deconstructable component = base.target.GetComponent<Deconstructable>();
		if (component != null)
		{
			AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
		}
		BuildingEnabledButton component2 = base.target.GetComponent<BuildingEnabledButton>();
		if (component2 != null)
		{
			AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component2);
		}
		if (operational_requirement != Operational.State.None)
		{
			Operational component3 = destination.GetComponent<Operational>();
			if (component3 != null)
			{
				Precondition precondition = ChorePreconditions.instance.IsOperational;
				if (operational_requirement == Operational.State.Functional)
				{
					precondition = ChorePreconditions.instance.IsFunctional;
				}
				AddPrecondition(precondition, component3);
			}
		}
		partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, this, Grid.PosToCell(destination), GameScenePartitioner.Instance.fetchChoreLayer, null);
		destination.Subscribe(644822890, OnOnlyFetchMarkedItemsSettingChanged);
		automatable = destination.GetComponent<Automatable>();
		if ((bool)automatable)
		{
			AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, automatable);
		}
	}

	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		if (destination != null)
		{
			if (destination.GetOnlyFetchMarkedItems())
			{
				DebugUtil.DevAssert(!requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!");
				requiredTag = GameTags.Garbage;
			}
			else
			{
				requiredTag = Tag.Invalid;
			}
		}
	}

	private void OnMasterPriorityChanged(PriorityScreen.PriorityClass priorityClass, int priority_value)
	{
		masterPriority.priority_class = priorityClass;
		masterPriority.priority_value = priority_value;
	}

	public override void CollectChores(ChoreConsumerState consumer_state, List<Precondition.Context> succeeded_contexts, List<Precondition.Context> failed_contexts, bool is_attempting_override)
	{
	}

	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Precondition.Context> succeeded_contexts, List<Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		base.CollectChores(consumer_state, succeeded_contexts, failed_contexts, is_attempting_override);
	}

	public override void Cleanup()
	{
		base.Cleanup();
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
		if (destination != null)
		{
			destination.Unsubscribe(644822890, OnOnlyFetchMarkedItemsSettingChanged);
		}
	}

	public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
	{
		int num = 123137;
		foreach (Tag item in new SortedSet<Tag>(tags))
		{
			num = ((num << 5) + num) ^ item.GetHash();
		}
		return num;
	}
}
