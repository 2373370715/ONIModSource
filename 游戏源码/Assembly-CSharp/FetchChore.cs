using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020006AA RID: 1706
public class FetchChore : Chore<FetchChore.StatesInstance>
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06001EEA RID: 7914 RVA: 0x000B4574 File Offset: 0x000B2774
	public float originalAmount
	{
		get
		{
			return base.smi.sm.requestedamount.Get(base.smi);
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06001EEB RID: 7915 RVA: 0x000B4591 File Offset: 0x000B2791
	// (set) Token: 0x06001EEC RID: 7916 RVA: 0x000B45AE File Offset: 0x000B27AE
	public float amount
	{
		get
		{
			return base.smi.sm.actualamount.Get(base.smi);
		}
		set
		{
			base.smi.sm.actualamount.Set(value, base.smi, false);
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06001EED RID: 7917 RVA: 0x000B45CE File Offset: 0x000B27CE
	// (set) Token: 0x06001EEE RID: 7918 RVA: 0x000B45EB File Offset: 0x000B27EB
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

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06001EEF RID: 7919 RVA: 0x000B4609 File Offset: 0x000B2809
	// (set) Token: 0x06001EF0 RID: 7920 RVA: 0x000B4626 File Offset: 0x000B2826
	public GameObject fetcher
	{
		get
		{
			return base.smi.sm.fetcher.Get(base.smi);
		}
		set
		{
			base.smi.sm.fetcher.Set(value, base.smi, false);
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x000B4646 File Offset: 0x000B2846
	// (set) Token: 0x06001EF2 RID: 7922 RVA: 0x000B464E File Offset: 0x000B284E
	public Storage destination { get; private set; }

	// Token: 0x06001EF3 RID: 7923 RVA: 0x001B659C File Offset: 0x001B479C
	public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
	{
		this.amount = amount_to_be_fetched;
		base.smi.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName(this, context.data));
		base.Begin(context);
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x001B660C File Offset: 0x001B480C
	public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
	{
		if (is_success)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, pickupable));
			this.fetchTarget = pickupable;
			this.driver = driver;
			this.fetcher = driver.gameObject;
			base.Succeed("FetchAreaEnd");
			SaveGame.Instance.ColonyAchievementTracker.LogFetchChore(this.fetcher, this.choreType);
			return;
		}
		base.SetOverrideTarget(null);
		this.Fail("FetchAreaFail");
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x001B6694 File Offset: 0x001B4894
	public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
	{
		if (!(this.destination != null))
		{
			return null;
		}
		if (consumer_state.hasSolidTransferArm)
		{
			return consumer_state.solidTransferArm.FindFetchTarget(this.destination, this);
		}
		return Game.Instance.fetchManager.FindFetchTarget(this.destination, this);
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x001B66E4 File Offset: 0x001B48E4
	public override void Begin(Chore.Precondition.Context context)
	{
		Pickupable pickupable = (Pickupable)context.data;
		if (pickupable == null)
		{
			pickupable = this.FindFetchTarget(context.consumerState);
		}
		base.smi.sm.source.Set(pickupable.gameObject, base.smi, false);
		pickupable.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		base.Begin(context);
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x001B6758 File Offset: 0x001B4958
	protected override void End(string reason)
	{
		Pickupable pickupable = base.smi.sm.source.Get<Pickupable>(base.smi);
		if (pickupable != null)
		{
			pickupable.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
		base.End(reason);
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000B4657 File Offset: 0x000B2857
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.chunk.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000B4687 File Offset: 0x000B2887
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
		context.chore = new FetchAreaChore(context);
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000B469A File Offset: 0x000B289A
	public float AmountWaitingToFetch()
	{
		if (this.fetcher == null)
		{
			return this.originalAmount;
		}
		return this.amount;
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x001B67A8 File Offset: 0x001B49A8
	public FetchChore(ChoreType choreType, Storage destination, float amount, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags = null, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, Operational.State operational_requirement = Operational.State.Operational, int priority_mod = 0) : base(choreType, destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, false, true, priority_mod, false, ReportManager.ReportType.WorkTime)
	{
		if (choreType == null)
		{
			global::Debug.LogError("You must specify a chore type for fetching!");
		}
		this.tagsFirst = ((tags.Count > 0) ? tags.First<Tag>() : Tag.Invalid);
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Chore {0} is requesting {1} {2} to {3}", new object[]
				{
					choreType.Id,
					this.tagsFirst,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		base.SetPrioritizable((destination.prioritizable != null) ? destination.prioritizable : destination.GetComponent<Prioritizable>());
		base.smi = new FetchChore.StatesInstance(this);
		base.smi.sm.requestedamount.Set(amount, base.smi, false);
		this.destination = destination;
		DebugUtil.DevAssert(criteria != FetchChore.MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!", null);
		this.tags = tags;
		this.criteria = criteria;
		this.tagsHash = FetchChore.ComputeHashCodeForTags(tags);
		this.requiredTag = required_tag;
		this.forbiddenTags = ((forbidden_tags != null) ? forbidden_tags : new Tag[0]);
		this.forbidHash = FetchChore.ComputeHashCodeForTags(this.forbiddenTags);
		DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.", null);
		if (destination.GetOnlyFetchMarkedItems())
		{
			DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
			this.requiredTag = GameTags.Garbage;
		}
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, destination);
		this.AddPrecondition(FetchChore.IsFetchTargetAvailable, null);
		Deconstructable component = this.target.GetComponent<Deconstructable>();
		if (component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
		}
		BuildingEnabledButton component2 = this.target.GetComponent<BuildingEnabledButton>();
		if (component2 != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component2);
		}
		if (operational_requirement != Operational.State.None)
		{
			Operational component3 = destination.GetComponent<Operational>();
			if (component3 != null)
			{
				Chore.Precondition precondition = ChorePreconditions.instance.IsOperational;
				if (operational_requirement == Operational.State.Functional)
				{
					precondition = ChorePreconditions.instance.IsFunctional;
				}
				this.AddPrecondition(precondition, component3);
			}
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, this, Grid.PosToCell(destination), GameScenePartitioner.Instance.fetchChoreLayer, null);
		destination.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.automatable = destination.GetComponent<Automatable>();
		if (this.automatable)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, this.automatable);
		}
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x001B6A94 File Offset: 0x001B4C94
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		if (this.destination != null)
		{
			if (this.destination.GetOnlyFetchMarkedItems())
			{
				DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
				this.requiredTag = GameTags.Garbage;
				return;
			}
			this.requiredTag = Tag.Invalid;
		}
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x000B46B7 File Offset: 0x000B28B7
	private void OnMasterPriorityChanged(PriorityScreen.PriorityClass priorityClass, int priority_value)
	{
		this.masterPriority.priority_class = priorityClass;
		this.masterPriority.priority_value = priority_value;
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x000B46D1 File Offset: 0x000B28D1
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000B46DF File Offset: 0x000B28DF
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		base.CollectChores(consumer_state, succeeded_contexts, incomplete_contexts, failed_contexts, is_attempting_override);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x001B6AEC File Offset: 0x001B4CEC
	public override void Cleanup()
	{
		base.Cleanup();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.destination != null)
		{
			this.destination.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x001B6B3C File Offset: 0x001B4D3C
	public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
	{
		int num = 123137;
		foreach (Tag tag in new SortedSet<Tag>(tags))
		{
			num = ((num << 5) + num ^ tag.GetHash());
		}
		return num;
	}

	// Token: 0x040013E2 RID: 5090
	public HashSet<Tag> tags;

	// Token: 0x040013E3 RID: 5091
	public Tag tagsFirst;

	// Token: 0x040013E4 RID: 5092
	public FetchChore.MatchCriteria criteria;

	// Token: 0x040013E5 RID: 5093
	public int tagsHash;

	// Token: 0x040013E6 RID: 5094
	public bool validateRequiredTagOnTagChange;

	// Token: 0x040013E7 RID: 5095
	public Tag requiredTag;

	// Token: 0x040013E8 RID: 5096
	public Tag[] forbiddenTags;

	// Token: 0x040013E9 RID: 5097
	public int forbidHash;

	// Token: 0x040013EA RID: 5098
	public Automatable automatable;

	// Token: 0x040013EB RID: 5099
	public bool allowMultifetch = true;

	// Token: 0x040013EC RID: 5100
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040013ED RID: 5101
	public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition
	{
		id = "IsFetchTargetAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			if (flag)
			{
				if (pickupable == null)
				{
					global::Debug.Log(string.Format("Failed to find fetch target for {0}", fetchChore.destination));
					return false;
				}
				context.data = pickupable;
				int num;
				if (context.consumerState.consumer.GetNavigationCost(pickupable, out num))
				{
					context.cost += num;
					return true;
				}
			}
			return false;
		}
	};

	// Token: 0x020006AB RID: 1707
	public enum MatchCriteria
	{
		// Token: 0x040013EF RID: 5103
		MatchID,
		// Token: 0x040013F0 RID: 5104
		MatchTags
	}

	// Token: 0x020006AC RID: 1708
	public class StatesInstance : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance
	{
		// Token: 0x06001F04 RID: 7940 RVA: 0x000B46EE File Offset: 0x000B28EE
		public StatesInstance(FetchChore master) : base(master)
		{
		}
	}

	// Token: 0x020006AD RID: 1709
	public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
	{
		// Token: 0x06001F05 RID: 7941 RVA: 0x000B46F7 File Offset: 0x000B28F7
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
		}

		// Token: 0x040013F1 RID: 5105
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;

		// Token: 0x040013F2 RID: 5106
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;

		// Token: 0x040013F3 RID: 5107
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;

		// Token: 0x040013F4 RID: 5108
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;

		// Token: 0x040013F5 RID: 5109
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;
	}
}
