using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000761 RID: 1889
public abstract class Chore
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x0600217A RID: 8570
	// (set) Token: 0x0600217B RID: 8571
	public abstract int id { get; protected set; }

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600217C RID: 8572
	// (set) Token: 0x0600217D RID: 8573
	public abstract int priorityMod { get; protected set; }

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x0600217E RID: 8574
	// (set) Token: 0x0600217F RID: 8575
	public abstract ChoreType choreType { get; protected set; }

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06002180 RID: 8576
	// (set) Token: 0x06002181 RID: 8577
	public abstract ChoreDriver driver { get; protected set; }

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06002182 RID: 8578
	// (set) Token: 0x06002183 RID: 8579
	public abstract ChoreDriver lastDriver { get; protected set; }

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06002184 RID: 8580
	public abstract bool isNull { get; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06002185 RID: 8581
	public abstract GameObject gameObject { get; }

	// Token: 0x06002186 RID: 8582
	public abstract bool SatisfiesUrge(Urge urge);

	// Token: 0x06002187 RID: 8583
	public abstract bool IsValid();

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06002188 RID: 8584
	// (set) Token: 0x06002189 RID: 8585
	public abstract IStateMachineTarget target { get; protected set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x0600218A RID: 8586
	// (set) Token: 0x0600218B RID: 8587
	public abstract bool isComplete { get; protected set; }

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600218C RID: 8588
	// (set) Token: 0x0600218D RID: 8589
	public abstract bool IsPreemptable { get; protected set; }

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600218E RID: 8590
	// (set) Token: 0x0600218F RID: 8591
	public abstract ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06002190 RID: 8592
	// (set) Token: 0x06002191 RID: 8593
	public abstract Prioritizable prioritizable { get; protected set; }

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06002192 RID: 8594
	// (set) Token: 0x06002193 RID: 8595
	public abstract ChoreProvider provider { get; set; }

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06002194 RID: 8596
	// (set) Token: 0x06002195 RID: 8597
	public abstract bool runUntilComplete { get; set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06002196 RID: 8598
	// (set) Token: 0x06002197 RID: 8599
	public abstract bool isExpanded { get; protected set; }

	// Token: 0x06002198 RID: 8600
	public abstract List<Chore.PreconditionInstance> GetPreconditions();

	// Token: 0x06002199 RID: 8601
	public abstract bool CanPreempt(Chore.Precondition.Context context);

	// Token: 0x0600219A RID: 8602
	public abstract void PrepareChore(ref Chore.Precondition.Context context);

	// Token: 0x0600219B RID: 8603
	public abstract void Cancel(string reason);

	// Token: 0x0600219C RID: 8604
	public abstract ReportManager.ReportType GetReportType();

	// Token: 0x0600219D RID: 8605
	public abstract string GetReportName(string context = null);

	// Token: 0x0600219E RID: 8606
	public abstract void AddPrecondition(Chore.Precondition precondition, object data = null);

	// Token: 0x0600219F RID: 8607
	public abstract void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override);

	// Token: 0x060021A0 RID: 8608 RVA: 0x000B5E63 File Offset: 0x000B4063
	public void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChores(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x060021A1 RID: 8609
	public abstract void Cleanup();

	// Token: 0x060021A2 RID: 8610
	public abstract void Fail(string reason);

	// Token: 0x060021A3 RID: 8611
	public abstract void Begin(Chore.Precondition.Context context);

	// Token: 0x060021A4 RID: 8612
	public abstract bool InProgress();

	// Token: 0x060021A5 RID: 8613 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public virtual string ResolveString(string str)
	{
		return str;
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000B5E71 File Offset: 0x000B4071
	public static int GetNextChoreID()
	{
		return ++Chore.nextId;
	}

	// Token: 0x0400161C RID: 5660
	public PrioritySetting masterPriority;

	// Token: 0x0400161D RID: 5661
	public bool showAvailabilityInHoverText = true;

	// Token: 0x0400161E RID: 5662
	public Action<Chore> onExit;

	// Token: 0x0400161F RID: 5663
	public Action<Chore> onComplete;

	// Token: 0x04001620 RID: 5664
	private static int nextId;

	// Token: 0x04001621 RID: 5665
	public const int MAX_PLAYER_BASIC_PRIORITY = 9;

	// Token: 0x04001622 RID: 5666
	public const int MIN_PLAYER_BASIC_PRIORITY = 1;

	// Token: 0x04001623 RID: 5667
	public const int MAX_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04001624 RID: 5668
	public const int MIN_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04001625 RID: 5669
	public const int MAX_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04001626 RID: 5670
	public const int MIN_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04001627 RID: 5671
	public const int DEFAULT_BASIC_PRIORITY = 5;

	// Token: 0x04001628 RID: 5672
	public const int MAX_BASIC_PRIORITY = 10;

	// Token: 0x04001629 RID: 5673
	public const int MIN_BASIC_PRIORITY = 0;

	// Token: 0x0400162A RID: 5674
	public static bool ENABLE_PERSONAL_PRIORITIES = true;

	// Token: 0x0400162B RID: 5675
	public static PrioritySetting DefaultPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x02000762 RID: 1890
	// (Invoke) Token: 0x060021AA RID: 8618
	public delegate bool PreconditionFn(ref Chore.Precondition.Context context, object data);

	// Token: 0x02000763 RID: 1891
	public struct PreconditionInstance
	{
		// Token: 0x0400162C RID: 5676
		public Chore.Precondition condition;

		// Token: 0x0400162D RID: 5677
		public object data;
	}

	// Token: 0x02000764 RID: 1892
	public struct Precondition
	{
		// Token: 0x0400162E RID: 5678
		public string id;

		// Token: 0x0400162F RID: 5679
		public string description;

		// Token: 0x04001630 RID: 5680
		public int sortOrder;

		// Token: 0x04001631 RID: 5681
		public Chore.PreconditionFn fn;

		// Token: 0x04001632 RID: 5682
		public bool canExecuteOnAnyThread;

		// Token: 0x02000765 RID: 1893
		[DebuggerDisplay("{chore.GetType()}, {chore.gameObject.name}")]
		public struct Context : IComparable<Chore.Precondition.Context>, IEquatable<Chore.Precondition.Context>
		{
			// Token: 0x060021AD RID: 8621 RVA: 0x001C0738 File Offset: 0x001BE938
			public Context(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.personalPriority = consumer_state.consumer.GetPersonalPriority(chore.choreType);
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.skipMoreSatisfyingEarlyPrecondition = (RootMenu.Instance != null && RootMenu.Instance.IsBuildingChorePanelActive());
				this.SetPriority(chore);
			}

			// Token: 0x060021AE RID: 8622 RVA: 0x001C07F0 File Offset: 0x001BE9F0
			public void Set(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.SetPriority(chore);
			}

			// Token: 0x060021AF RID: 8623 RVA: 0x001C0870 File Offset: 0x001BEA70
			public void SetPriority(Chore chore)
			{
				this.priority = (Game.Instance.advancedPersonalPriorities ? chore.choreType.explicitPriority : chore.choreType.priority);
				this.priorityMod = chore.priorityMod;
				this.interruptPriority = chore.choreType.interruptPriority;
			}

			// Token: 0x060021B0 RID: 8624 RVA: 0x000B5EA3 File Offset: 0x000B40A3
			public bool IsSuccess()
			{
				return this.failedPreconditionId == -1 && !this.skippedPreconditions;
			}

			// Token: 0x060021B1 RID: 8625 RVA: 0x000B5EB9 File Offset: 0x000B40B9
			public bool IsComplete()
			{
				return !this.skippedPreconditions;
			}

			// Token: 0x060021B2 RID: 8626 RVA: 0x001C08C4 File Offset: 0x001BEAC4
			public bool IsPotentialSuccess()
			{
				if (this.IsSuccess())
				{
					return true;
				}
				if (this.chore.driver == this.consumerState.choreDriver)
				{
					return true;
				}
				if (this.failedPreconditionId != -1)
				{
					if (this.failedPreconditionId >= 0 && this.failedPreconditionId < this.chore.GetPreconditions().Count)
					{
						return this.chore.GetPreconditions()[this.failedPreconditionId].condition.id == ChorePreconditions.instance.IsMoreSatisfyingLate.id;
					}
					DebugUtil.DevLogErrorFormat("failedPreconditionId out of range {0}/{1}", new object[]
					{
						this.failedPreconditionId,
						this.chore.GetPreconditions().Count
					});
				}
				return false;
			}

			// Token: 0x060021B3 RID: 8627 RVA: 0x001C0994 File Offset: 0x001BEB94
			private void DoPreconditions(bool mainThreadOnly)
			{
				bool flag = Game.IsOnMainThread();
				List<Chore.PreconditionInstance> preconditions = this.chore.GetPreconditions();
				this.skippedPreconditions = false;
				int i = 0;
				while (i < preconditions.Count)
				{
					Chore.PreconditionInstance preconditionInstance = preconditions[i];
					if (preconditionInstance.condition.canExecuteOnAnyThread)
					{
						if (!mainThreadOnly)
						{
							goto IL_43;
						}
					}
					else
					{
						if (flag)
						{
							goto IL_43;
						}
						this.skippedPreconditions = true;
					}
					IL_6B:
					i++;
					continue;
					IL_43:
					if (!preconditionInstance.condition.fn(ref this, preconditionInstance.data))
					{
						this.failedPreconditionId = i;
						this.skippedPreconditions = false;
						return;
					}
					goto IL_6B;
				}
			}

			// Token: 0x060021B4 RID: 8628 RVA: 0x000B5EC4 File Offset: 0x000B40C4
			public void RunPreconditions()
			{
				this.DoPreconditions(false);
			}

			// Token: 0x060021B5 RID: 8629 RVA: 0x000B5ECD File Offset: 0x000B40CD
			public void FinishPreconditions()
			{
				this.DoPreconditions(true);
			}

			// Token: 0x060021B6 RID: 8630 RVA: 0x001C0A1C File Offset: 0x001BEC1C
			public int CompareTo(Chore.Precondition.Context obj)
			{
				bool flag = this.failedPreconditionId != -1;
				bool flag2 = obj.failedPreconditionId != -1;
				if (flag == flag2)
				{
					int num = this.masterPriority.priority_class - obj.masterPriority.priority_class;
					if (num != 0)
					{
						return num;
					}
					int num2 = this.personalPriority - obj.personalPriority;
					if (num2 != 0)
					{
						return num2;
					}
					int num3 = this.masterPriority.priority_value - obj.masterPriority.priority_value;
					if (num3 != 0)
					{
						return num3;
					}
					int num4 = this.priority - obj.priority;
					if (num4 != 0)
					{
						return num4;
					}
					int num5 = this.priorityMod - obj.priorityMod;
					if (num5 != 0)
					{
						return num5;
					}
					int num6 = this.consumerPriority - obj.consumerPriority;
					if (num6 != 0)
					{
						return num6;
					}
					int num7 = obj.cost - this.cost;
					if (num7 != 0)
					{
						return num7;
					}
					if (this.chore == null && obj.chore == null)
					{
						return 0;
					}
					if (this.chore == null)
					{
						return -1;
					}
					if (obj.chore == null)
					{
						return 1;
					}
					return this.chore.id - obj.chore.id;
				}
				else
				{
					if (!flag)
					{
						return 1;
					}
					return -1;
				}
			}

			// Token: 0x060021B7 RID: 8631 RVA: 0x001C0B38 File Offset: 0x001BED38
			public override bool Equals(object obj)
			{
				Chore.Precondition.Context obj2 = (Chore.Precondition.Context)obj;
				return this.CompareTo(obj2) == 0;
			}

			// Token: 0x060021B8 RID: 8632 RVA: 0x000B5ED6 File Offset: 0x000B40D6
			public bool Equals(Chore.Precondition.Context other)
			{
				return this.CompareTo(other) == 0;
			}

			// Token: 0x060021B9 RID: 8633 RVA: 0x000B5EE2 File Offset: 0x000B40E2
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x060021BA RID: 8634 RVA: 0x000B5EF4 File Offset: 0x000B40F4
			public static bool operator ==(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) == 0;
			}

			// Token: 0x060021BB RID: 8635 RVA: 0x000B5F01 File Offset: 0x000B4101
			public static bool operator !=(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) != 0;
			}

			// Token: 0x060021BC RID: 8636 RVA: 0x000B5F0E File Offset: 0x000B410E
			public static bool ShouldFilter(string filter, string text)
			{
				return !string.IsNullOrEmpty(filter) && (string.IsNullOrEmpty(text) || text.ToLower().IndexOf(filter) < 0);
			}

			// Token: 0x04001633 RID: 5683
			public PrioritySetting masterPriority;

			// Token: 0x04001634 RID: 5684
			public int personalPriority;

			// Token: 0x04001635 RID: 5685
			public int priority;

			// Token: 0x04001636 RID: 5686
			public int priorityMod;

			// Token: 0x04001637 RID: 5687
			public int interruptPriority;

			// Token: 0x04001638 RID: 5688
			public int cost;

			// Token: 0x04001639 RID: 5689
			public int consumerPriority;

			// Token: 0x0400163A RID: 5690
			public Chore chore;

			// Token: 0x0400163B RID: 5691
			public ChoreConsumerState consumerState;

			// Token: 0x0400163C RID: 5692
			public int failedPreconditionId;

			// Token: 0x0400163D RID: 5693
			public bool skippedPreconditions;

			// Token: 0x0400163E RID: 5694
			public object data;

			// Token: 0x0400163F RID: 5695
			public bool isAttemptingOverride;

			// Token: 0x04001640 RID: 5696
			public ChoreType choreTypeForPermission;

			// Token: 0x04001641 RID: 5697
			public bool skipMoreSatisfyingEarlyPrecondition;
		}
	}
}
