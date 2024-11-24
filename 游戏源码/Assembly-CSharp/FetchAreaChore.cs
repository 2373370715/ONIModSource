using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020006A1 RID: 1697
public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x000B430C File Offset: 0x000B250C
	public bool IsFetching
	{
		get
		{
			return base.smi.pickingup;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x000B4319 File Offset: 0x000B2519
	public bool IsDelivering
	{
		get
		{
			return base.smi.delivering;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x000B4326 File Offset: 0x000B2526
	public GameObject GetFetchTarget
	{
		get
		{
			return base.smi.sm.fetchTarget.Get(base.smi);
		}
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x001B4C50 File Offset: 0x001B2E50
	public FetchAreaChore(Chore.Precondition.Context context) : base(context.chore.choreType, context.consumerState.consumer, context.consumerState.choreProvider, false, null, null, null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new FetchAreaChore.StatesInstance(this, context);
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000B4343 File Offset: 0x000B2543
	public override void Cleanup()
	{
		base.Cleanup();
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000B434B File Offset: 0x000B254B
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.Begin(context);
		base.Begin(context);
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x000B4360 File Offset: 0x000B2560
	protected override void End(string reason)
	{
		base.smi.End();
		base.End(reason);
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000B4374 File Offset: 0x000B2574
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.fetchTarget.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x001B4CB8 File Offset: 0x001B2EB8
	private static bool IsPickupableStillValidForChore(Pickupable pickupable, FetchChore chore)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		if ((chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst)))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it is not or does not contain one of these tags: {1}", pickupable, string.Join<Tag>(",", chore.tags)));
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it does not have the required tag: {1}", pickupable, chore.requiredTag));
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it has the forbidden tags: {1}", pickupable, string.Join<Tag>(",", chore.forbiddenTags)));
			return false;
		}
		return pickupable.isChoreAllowedToPickup(chore.choreType);
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x001B4D94 File Offset: 0x001B2F94
	public static void GatherNearbyFetchChores(FetchChore root_chore, Chore.Precondition.Context context, int x, int y, int radius, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts)
	{
		ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x - radius, y - radius, radius * 2 + 1, radius * 2 + 1, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			(pooledList[i].obj as FetchChore).CollectChoresFromGlobalChoreProvider(context.consumerState, succeeded_contexts, null, failed_contexts, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x020006A2 RID: 1698
	public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06001EAE RID: 7854 RVA: 0x000B43A4 File Offset: 0x000B25A4
		public Tag RootChore_RequiredTag
		{
			get
			{
				return this.rootChore.requiredTag;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x000B43B1 File Offset: 0x000B25B1
		public bool RootChore_ValidateRequiredTagOnTagChange
		{
			get
			{
				return this.rootChore.validateRequiredTagOnTagChange;
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x001B4E0C File Offset: 0x001B300C
		public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context) : base(master)
		{
			this.rootContext = context;
			this.rootChore = (context.chore as FetchChore);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x001B4E70 File Offset: 0x001B3070
		public void Begin(Chore.Precondition.Context context)
		{
			base.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
			this.chores.Clear();
			this.chores.Add(this.rootChore);
			int x;
			int y;
			Grid.CellToXY(Grid.PosToCell(this.rootChore.destination.transform.GetPosition()), out x, out y);
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList2 = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			if (this.rootChore.allowMultifetch)
			{
				FetchAreaChore.GatherNearbyFetchChores(this.rootChore, context, x, y, 3, pooledList, pooledList2);
			}
			float num = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(context.consumerState.consumer).GetTotalValue());
			Pickupable pickupable = context.data as Pickupable;
			if (pickupable == null)
			{
				global::Debug.Assert(pooledList.Count > 0, "succeeded_contexts was empty");
				FetchChore fetchChore = (FetchChore)pooledList[0].chore;
				global::Debug.Assert(fetchChore != null, "fetch_chore was null");
				DebugUtil.LogWarningArgs(new object[]
				{
					"Missing root_fetchable for FetchAreaChore",
					fetchChore.destination,
					fetchChore.tagsFirst
				});
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
			}
			global::Debug.Assert(pickupable != null, "root_fetchable was null");
			List<Pickupable> list = new List<Pickupable>();
			list.Add(pickupable);
			float num2 = pickupable.UnreservedAmount;
			float minTakeAmount = pickupable.MinTakeAmount;
			int num3 = 0;
			int num4 = 0;
			Grid.CellToXY(Grid.PosToCell(pickupable.transform.GetPosition()), out num3, out num4);
			int num5 = 9;
			num3 -= 3;
			num4 -= 3;
			Tag prefabTag = pickupable.GetComponent<KPrefabID>().PrefabTag;
			IEnumerable<object> first = GameScenePartitioner.Instance.AsyncSafeEnumerate(num3, num4, num5, num5, GameScenePartitioner.Instance.pickupablesLayer);
			IEnumerable<object> second = GameScenePartitioner.Instance.AsyncSafeEnumerate(num3, num4, num5, num5, GameScenePartitioner.Instance.storedPickupablesLayer);
			foreach (object obj in first.Concat(second))
			{
				if (num2 > num)
				{
					break;
				}
				Pickupable pickupable2 = obj as Pickupable;
				KPrefabID kprefabID = pickupable2.KPrefabID;
				if (!kprefabID.HasTag(GameTags.StoredPrivate) && !(kprefabID.PrefabTag != prefabTag) && pickupable2.UnreservedAmount > 0f && (this.rootChore.criteria != FetchChore.MatchCriteria.MatchID || this.rootChore.tags.Contains(kprefabID.PrefabTag)) && (this.rootChore.criteria != FetchChore.MatchCriteria.MatchTags || kprefabID.HasTag(this.rootChore.tagsFirst)) && (!this.rootChore.requiredTag.IsValid || kprefabID.HasTag(this.rootChore.requiredTag)) && !kprefabID.HasAnyTags(this.rootChore.forbiddenTags) && !list.Contains(pickupable2) && this.rootContext.consumerState.consumer.CanReach(pickupable2) && !kprefabID.HasTag(GameTags.MarkedForMove))
				{
					float unreservedAmount = pickupable2.UnreservedAmount;
					list.Add(pickupable2);
					num2 += unreservedAmount;
					if (list.Count >= 10)
					{
						break;
					}
				}
			}
			num2 = Mathf.Min(num, num2);
			if (minTakeAmount > 0f)
			{
				num2 -= num2 % minTakeAmount;
			}
			this.deliveries.Clear();
			float num6 = Mathf.Min(this.rootChore.originalAmount, num2);
			if (minTakeAmount > 0f)
			{
				num6 -= num6 % minTakeAmount;
			}
			this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(this.rootContext, num6, new Action<FetchChore>(this.OnFetchChoreCancelled)));
			float num7 = num6;
			int num8 = 0;
			while (num8 < pooledList.Count && num7 < num2)
			{
				Chore.Precondition.Context context2 = pooledList[num8];
				FetchChore fetchChore2 = context2.chore as FetchChore;
				if (fetchChore2 != this.rootChore && fetchChore2.overrideTarget == null && fetchChore2.driver == null && fetchChore2.tagsHash == this.rootChore.tagsHash && fetchChore2.requiredTag == this.rootChore.requiredTag && fetchChore2.forbidHash == this.rootChore.forbidHash)
				{
					num6 = Mathf.Min(fetchChore2.originalAmount, num2 - num7);
					if (minTakeAmount > 0f)
					{
						num6 -= num6 % minTakeAmount;
					}
					this.chores.Add(fetchChore2);
					this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(context2, num6, new Action<FetchChore>(this.OnFetchChoreCancelled)));
					num7 += num6;
					if (this.deliveries.Count >= 10)
					{
						break;
					}
				}
				num8++;
			}
			num7 = Mathf.Min(num7, num2);
			float num9 = num7;
			this.fetchables.Clear();
			int num10 = 0;
			while (num10 < list.Count && num9 > 0f)
			{
				Pickupable pickupable3 = list[num10];
				num9 -= pickupable3.UnreservedAmount;
				this.fetchables.Add(pickupable3);
				num10++;
			}
			this.fetchAmountRequested = num7;
			this.reservations.Clear();
			pooledList.Recycle();
			pooledList2.Recycle();
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x001B5400 File Offset: 0x001B3600
		public void End()
		{
			foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
			{
				delivery.Cleanup();
			}
			this.deliveries.Clear();
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x001B5460 File Offset: 0x001B3660
		public void SetupDelivery()
		{
			if (this.deliveries.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			FetchAreaChore.StatesInstance.Delivery nextDelivery = this.deliveries[0];
			if (FetchAreaChore.StatesInstance.s_transientDeliveryTags.Contains(nextDelivery.chore.requiredTag))
			{
				nextDelivery.chore.requiredTag = Tag.Invalid;
			}
			this.deliverables.RemoveAll(delegate(Pickupable x)
			{
				if (x == null || x.TotalAmount <= 0f)
				{
					return true;
				}
				if (x.KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					return true;
				}
				if (!FetchAreaChore.IsPickupableStillValidForChore(x, nextDelivery.chore))
				{
					global::Debug.LogWarning(string.Format("Removing deliverable {0} for a delivery to {1} which did not request it", x, nextDelivery.chore.destination));
					return true;
				}
				return false;
			});
			if (this.deliverables.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			base.sm.deliveryDestination.Set(nextDelivery.destination, base.smi);
			base.sm.deliveryObject.Set(this.deliverables[0], base.smi);
			if (!(nextDelivery.destination != null))
			{
				base.smi.GoTo(base.sm.delivering.deliverfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.delivering.movetostorage);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.deliveries[0].destination))
			{
				this.GoTo(base.sm.delivering.storing);
				return;
			}
			this.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x001B55F8 File Offset: 0x001B37F8
		public void SetupFetch()
		{
			if (this.reservations.Count <= 0)
			{
				this.GoTo(base.sm.delivering.next);
				return;
			}
			this.SetFetchTarget(this.reservations[0].pickupable);
			base.sm.fetchResultTarget.Set(null, base.smi);
			base.sm.fetchAmount.Set(this.reservations[0].amount, base.smi, false);
			if (!(this.reservations[0].pickupable != null))
			{
				this.GoTo(base.sm.fetching.fetchfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.fetching.movetopickupable);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.reservations[0].pickupable))
			{
				this.GoTo(base.sm.fetching.pickup);
				return;
			}
			this.GoTo(base.sm.fetching.fetchfail);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000B43BE File Offset: 0x000B25BE
		public void SetFetchTarget(Pickupable fetching)
		{
			base.sm.fetchTarget.Set(fetching, base.smi);
			if (fetching != null)
			{
				fetching.Subscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x001B5744 File Offset: 0x001B3944
		public void DeliverFail()
		{
			if (this.deliveries.Count > 0)
			{
				this.deliveries[0].Cleanup();
				this.deliveries.RemoveAt(0);
			}
			this.GoTo(base.sm.delivering.next);
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x001B5798 File Offset: 0x001B3998
		public void DeliverComplete()
		{
			Pickupable pickupable = base.sm.deliveryObject.Get<Pickupable>(base.smi);
			if (!(pickupable == null) && pickupable.TotalAmount > 0f)
			{
				if (this.deliveries.Count > 0)
				{
					FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
					Chore chore = delivery.chore;
					delivery.Complete(this.deliverables);
					delivery.Cleanup();
					if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
					{
						this.deliveries.RemoveAt(0);
					}
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			if (this.deliveries.Count > 0 && this.deliveries[0].chore.amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				FetchAreaChore.StatesInstance.Delivery delivery2 = this.deliveries[0];
				Chore chore2 = delivery2.chore;
				delivery2.Complete(this.deliverables);
				delivery2.Cleanup();
				if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore2)
				{
					this.deliveries.RemoveAt(0);
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			base.smi.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x001B5914 File Offset: 0x001B3B14
		public void FetchFail()
		{
			if (base.smi.sm.fetchTarget.Get(base.smi) != null)
			{
				base.smi.sm.fetchTarget.Get(base.smi).Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x001B59AC File Offset: 0x001B3BAC
		public void FetchComplete()
		{
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x001B59F0 File Offset: 0x001B3BF0
		public void SetupDeliverables()
		{
			foreach (GameObject gameObject in base.sm.fetcher.Get<Storage>(base.smi).items)
			{
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!(component == null) && !component.HasTag(GameTags.MarkedForMove))
					{
						Pickupable component2 = component.GetComponent<Pickupable>();
						if (component2 != null)
						{
							this.deliverables.Add(component2);
						}
					}
				}
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x001B5A94 File Offset: 0x001B3C94
		public void ReservePickupables()
		{
			ChoreConsumer consumer = base.sm.fetcher.Get<ChoreConsumer>(base.smi);
			float num = this.fetchAmountRequested;
			foreach (Pickupable pickupable in this.fetchables)
			{
				if (num <= 0f)
				{
					break;
				}
				if (!pickupable.KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					float num2 = Math.Min(num, pickupable.UnreservedAmount);
					num -= num2;
					FetchAreaChore.StatesInstance.Reservation item = new FetchAreaChore.StatesInstance.Reservation(consumer, pickupable, num2);
					this.reservations.Add(item);
				}
			}
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x001B5B48 File Offset: 0x001B3D48
		private void OnFetchChoreCancelled(FetchChore chore)
		{
			int i = 0;
			while (i < this.deliveries.Count)
			{
				if (this.deliveries[i].chore == chore)
				{
					if (this.deliveries.Count == 1)
					{
						this.StopSM("AllDelivericesCancelled");
						return;
					}
					if (i == 0)
					{
						base.sm.currentdeliverycancelled.Trigger(this);
						return;
					}
					this.deliveries[i].Cleanup();
					this.deliveries.RemoveAt(i);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x001B5BD4 File Offset: 0x001B3DD4
		public void UnreservePickupables()
		{
			foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
			{
				reservation.Cleanup();
			}
			this.reservations.Clear();
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x001B5C34 File Offset: 0x001B3E34
		public bool SameDestination(FetchChore fetch)
		{
			using (List<FetchChore>.Enumerator enumerator = this.chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.destination == fetch.destination)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x001B5C98 File Offset: 0x001B3E98
		public void OnMarkForMove(object data)
		{
			GameObject x = base.smi.sm.fetchTarget.Get(base.smi);
			GameObject gameObject = data as GameObject;
			if (x != null)
			{
				if (x == gameObject)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
					base.smi.sm.fetchTarget.Set(null, base.smi);
					return;
				}
				global::Debug.LogError("Listening for MarkForMove on the incorrect fetch target. Subscriptions did not update correctly.");
			}
		}

		// Token: 0x040013AA RID: 5034
		private List<FetchChore> chores = new List<FetchChore>();

		// Token: 0x040013AB RID: 5035
		private List<Pickupable> fetchables = new List<Pickupable>();

		// Token: 0x040013AC RID: 5036
		private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();

		// Token: 0x040013AD RID: 5037
		private List<Pickupable> deliverables = new List<Pickupable>();

		// Token: 0x040013AE RID: 5038
		public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();

		// Token: 0x040013AF RID: 5039
		private FetchChore rootChore;

		// Token: 0x040013B0 RID: 5040
		private Chore.Precondition.Context rootContext;

		// Token: 0x040013B1 RID: 5041
		private float fetchAmountRequested;

		// Token: 0x040013B2 RID: 5042
		public bool delivering;

		// Token: 0x040013B3 RID: 5043
		public bool pickingup;

		// Token: 0x040013B4 RID: 5044
		private static Tag[] s_transientDeliveryTags = new Tag[]
		{
			GameTags.Garbage,
			GameTags.Creatures.Deliverable
		};

		// Token: 0x020006A3 RID: 1699
		public struct Delivery
		{
			// Token: 0x170000BB RID: 187
			// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x000B441D File Offset: 0x000B261D
			// (set) Token: 0x06001EC2 RID: 7874 RVA: 0x000B4425 File Offset: 0x000B2625
			public Storage destination { readonly get; private set; }

			// Token: 0x170000BC RID: 188
			// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000B442E File Offset: 0x000B262E
			// (set) Token: 0x06001EC4 RID: 7876 RVA: 0x000B4436 File Offset: 0x000B2636
			public float amount { readonly get; private set; }

			// Token: 0x170000BD RID: 189
			// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000B443F File Offset: 0x000B263F
			// (set) Token: 0x06001EC6 RID: 7878 RVA: 0x000B4447 File Offset: 0x000B2647
			public FetchChore chore { readonly get; private set; }

			// Token: 0x06001EC7 RID: 7879 RVA: 0x001B5D18 File Offset: 0x001B3F18
			public Delivery(Chore.Precondition.Context context, float amount_to_be_fetched, Action<FetchChore> on_cancelled)
			{
				this = default(FetchAreaChore.StatesInstance.Delivery);
				this.chore = (context.chore as FetchChore);
				this.amount = this.chore.originalAmount;
				this.destination = this.chore.destination;
				this.chore.SetOverrideTarget(context.consumerState.consumer);
				this.onCancelled = on_cancelled;
				this.onFetchChoreCleanup = new Action<Chore>(this.OnFetchChoreCleanup);
				this.chore.FetchAreaBegin(context, amount_to_be_fetched);
				FetchChore chore = this.chore;
				chore.onCleanup = (Action<Chore>)Delegate.Combine(chore.onCleanup, this.onFetchChoreCleanup);
			}

			// Token: 0x06001EC8 RID: 7880 RVA: 0x001B5DC8 File Offset: 0x001B3FC8
			public void Complete(List<Pickupable> deliverables)
			{
				using (new KProfiler.Region("FAC.Delivery.Complete", null))
				{
					if (!(this.destination == null) && !this.destination.IsEndOfLife())
					{
						FetchChore chore = this.chore;
						chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
						float num = this.amount;
						Pickupable pickupable = null;
						int num2 = 0;
						while (num2 < deliverables.Count && num > 0f)
						{
							if (deliverables[num2] == null)
							{
								if (num < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
								{
									this.destination.ForceStore(this.chore.tagsFirst, num);
								}
							}
							else if (!FetchAreaChore.IsPickupableStillValidForChore(deliverables[num2], this.chore))
							{
								global::Debug.LogError(string.Format("Attempting to store {0} in a {1} which did not request it", deliverables[num2], this.destination));
							}
							else
							{
								Pickupable pickupable2 = deliverables[num2].Take(num);
								if (pickupable2 != null && pickupable2.TotalAmount > 0f)
								{
									num -= pickupable2.TotalAmount;
									this.destination.Store(pickupable2.gameObject, false, false, true, false);
									pickupable = pickupable2;
									if (pickupable2 == deliverables[num2])
									{
										deliverables[num2] = null;
									}
								}
							}
							num2++;
						}
						if (this.chore.overrideTarget != null)
						{
							this.chore.FetchAreaEnd(this.chore.overrideTarget.GetComponent<ChoreDriver>(), pickupable, true);
						}
						this.chore = null;
					}
				}
			}

			// Token: 0x06001EC9 RID: 7881 RVA: 0x000B4450 File Offset: 0x000B2650
			private void OnFetchChoreCleanup(Chore chore)
			{
				if (this.onCancelled != null)
				{
					this.onCancelled(chore as FetchChore);
				}
			}

			// Token: 0x06001ECA RID: 7882 RVA: 0x000B446B File Offset: 0x000B266B
			public void Cleanup()
			{
				if (this.chore != null)
				{
					FetchChore chore = this.chore;
					chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
					this.chore.FetchAreaEnd(null, null, false);
				}
			}

			// Token: 0x040013B8 RID: 5048
			private Action<FetchChore> onCancelled;

			// Token: 0x040013B9 RID: 5049
			private Action<Chore> onFetchChoreCleanup;
		}

		// Token: 0x020006A4 RID: 1700
		public struct Reservation
		{
			// Token: 0x170000BE RID: 190
			// (get) Token: 0x06001ECB RID: 7883 RVA: 0x000B44A4 File Offset: 0x000B26A4
			// (set) Token: 0x06001ECC RID: 7884 RVA: 0x000B44AC File Offset: 0x000B26AC
			public float amount { readonly get; private set; }

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x06001ECD RID: 7885 RVA: 0x000B44B5 File Offset: 0x000B26B5
			// (set) Token: 0x06001ECE RID: 7886 RVA: 0x000B44BD File Offset: 0x000B26BD
			public Pickupable pickupable { readonly get; private set; }

			// Token: 0x06001ECF RID: 7887 RVA: 0x001B5F80 File Offset: 0x001B4180
			public Reservation(ChoreConsumer consumer, Pickupable pickupable, float reservation_amount)
			{
				this = default(FetchAreaChore.StatesInstance.Reservation);
				if (reservation_amount <= 0f)
				{
					global::Debug.LogError("Invalid amount: " + reservation_amount.ToString());
				}
				this.amount = reservation_amount;
				this.pickupable = pickupable;
				this.handle = pickupable.Reserve("FetchAreaChore", consumer.gameObject, reservation_amount);
			}

			// Token: 0x06001ED0 RID: 7888 RVA: 0x000B44C6 File Offset: 0x000B26C6
			public void Cleanup()
			{
				if (this.pickupable != null)
				{
					this.pickupable.Unreserve("FetchAreaChore", this.handle);
				}
			}

			// Token: 0x040013BC RID: 5052
			private int handle;
		}
	}

	// Token: 0x020006A6 RID: 1702
	public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
	{
		// Token: 0x06001ED3 RID: 7891 RVA: 0x001B6048 File Offset: 0x001B4248
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetching;
			base.Target(this.fetcher);
			this.fetching.DefaultState(this.fetching.next).Enter("ReservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.ReservePickupables();
			}).Exit("UnreservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.UnreservePickupables();
			}).Enter("pickingup-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = true;
			}).Exit("pickingup-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = false;
			});
			this.fetching.next.Enter("SetupFetch", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupFetch();
			});
			this.fetching.movetopickupable.InitializeStates(this.fetcher, this.fetchTarget, new Func<FetchAreaChore.StatesInstance, CellOffset[]>(this.GetFetchOffset), this.fetching.pickup, this.fetching.fetchfail, NavigationTactics.ReduceTravelDistance).Target(this.fetchTarget).EventHandlerTransition(GameHashes.TagsChanged, this.fetching.fetchfail, (FetchAreaChore.StatesInstance smi, object obj) => smi.RootChore_ValidateRequiredTagOnTagChange && smi.RootChore_RequiredTag.IsValid && !this.fetchTarget.Get(smi).HasTag(smi.RootChore_RequiredTag)).Target(this.fetcher);
			this.fetching.pickup.DoPickup(this.fetchTarget, this.fetchResultTarget, this.fetchAmount, this.fetching.fetchcomplete, this.fetching.fetchfail).Exit(delegate(FetchAreaChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.fetchTarget.Get(smi);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(smi.OnMarkForMove));
				}
			});
			this.fetching.fetchcomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchComplete();
			});
			this.fetching.fetchfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchFail();
			});
			this.delivering.DefaultState(this.delivering.next).OnSignal(this.currentdeliverycancelled, this.delivering.deliverfail).Enter("SetupDeliverables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDeliverables();
			}).Enter("delivering-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = true;
			}).Exit("delivering-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = false;
			});
			this.delivering.next.Enter("SetupDelivery", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDelivery();
			});
			this.delivering.movetostorage.InitializeStates(this.fetcher, this.deliveryDestination, new Func<FetchAreaChore.StatesInstance, CellOffset[]>(this.GetFetchOffset), this.delivering.storing, this.delivering.deliverfail, NavigationTactics.ReduceTravelDistance).Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				if (this.deliveryObject.Get(smi) != null && this.deliveryObject.Get(smi).GetComponent<MinionIdentity>() != null)
				{
					this.deliveryObject.Get(smi).transform.SetLocalPosition(Vector3.zero);
					KBatchedAnimTracker component = this.deliveryObject.Get(smi).GetComponent<KBatchedAnimTracker>();
					component.symbol = new HashedString("snapTo_chest");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			});
			this.delivering.storing.DoDelivery(this.fetcher, this.deliveryDestination, this.delivering.delivercomplete, this.delivering.deliverfail);
			this.delivering.deliverfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverFail();
			});
			this.delivering.delivercomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverComplete();
			});
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x001B6458 File Offset: 0x001B4658
		private CellOffset[] GetFetchOffset(FetchAreaChore.StatesInstance smi)
		{
			WorkerBase component = this.fetcher.Get(smi).GetComponent<WorkerBase>();
			if (!(component != null))
			{
				return null;
			}
			return component.GetFetchCellOffsets();
		}

		// Token: 0x040013BE RID: 5054
		public FetchAreaChore.States.FetchStates fetching;

		// Token: 0x040013BF RID: 5055
		public FetchAreaChore.States.DeliverStates delivering;

		// Token: 0x040013C0 RID: 5056
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;

		// Token: 0x040013C1 RID: 5057
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;

		// Token: 0x040013C2 RID: 5058
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;

		// Token: 0x040013C3 RID: 5059
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;

		// Token: 0x040013C4 RID: 5060
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;

		// Token: 0x040013C5 RID: 5061
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;

		// Token: 0x040013C6 RID: 5062
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;

		// Token: 0x040013C7 RID: 5063
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

		// Token: 0x020006A7 RID: 1703
		public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x040013C8 RID: 5064
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x040013C9 RID: 5065
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;

			// Token: 0x040013CA RID: 5066
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;

			// Token: 0x040013CB RID: 5067
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;

			// Token: 0x040013CC RID: 5068
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
		}

		// Token: 0x020006A8 RID: 1704
		public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x040013CD RID: 5069
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x040013CE RID: 5070
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;

			// Token: 0x040013CF RID: 5071
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;

			// Token: 0x040013D0 RID: 5072
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;

			// Token: 0x040013D1 RID: 5073
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
		}
	}
}
