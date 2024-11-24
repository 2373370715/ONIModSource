using System;
using System.Collections.Generic;

// Token: 0x02000775 RID: 1909
public class ChoreTable
{
	// Token: 0x06002270 RID: 8816 RVA: 0x000B671E File Offset: 0x000B491E
	public ChoreTable(ChoreTable.Entry[] entries)
	{
		this.entries = entries;
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x001C2CE0 File Offset: 0x001C0EE0
	public ref ChoreTable.Entry GetEntry<T>()
	{
		ref ChoreTable.Entry result = ref ChoreTable.InvalidEntry;
		for (int i = 0; i < this.entries.Length; i++)
		{
			if (this.entries[i].stateMachineDef is T)
			{
				result = ref this.entries[i];
				break;
			}
		}
		return ref result;
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x001C2D30 File Offset: 0x001C0F30
	public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
	{
		for (int i = 0; i < this.entries.Length; i++)
		{
			ChoreTable.Entry entry = this.entries[i];
			if (entry.stateMachineDef.GetStateMachineType() == typeof(StateMachineType))
			{
				return entry.choreType.priority;
			}
		}
		Debug.LogError(chore_consumer.name + "'s chore table does not have an entry for: " + typeof(StateMachineType).Name);
		return -1;
	}

	// Token: 0x040016AB RID: 5803
	private ChoreTable.Entry[] entries;

	// Token: 0x040016AC RID: 5804
	public static ChoreTable.Entry InvalidEntry;

	// Token: 0x02000776 RID: 1910
	public class Builder
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x000B672D File Offset: 0x000B492D
		public ChoreTable.Builder PushInterruptGroup()
		{
			this.interruptGroupId++;
			return this;
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000B673E File Offset: 0x000B493E
		public ChoreTable.Builder PopInterruptGroup()
		{
			DebugUtil.Assert(this.interruptGroupId > 0);
			this.interruptGroupId--;
			return this;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x001C2DAC File Offset: 0x001C0FAC
		public ChoreTable.Builder Add(StateMachine.BaseDef def, bool condition = true, int forcePriority = -1)
		{
			if (condition)
			{
				ChoreTable.Builder.Info item = new ChoreTable.Builder.Info
				{
					interruptGroupId = this.interruptGroupId,
					forcePriority = forcePriority,
					def = def
				};
				this.infos.Add(item);
			}
			return this;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x001C2DF0 File Offset: 0x001C0FF0
		public bool HasChoreType(Type choreType)
		{
			return this.infos.Exists((ChoreTable.Builder.Info info) => info.def.GetType() == choreType);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x001C2E24 File Offset: 0x001C1024
		public bool TryGetChoreDef<T>(out T def) where T : StateMachine.BaseDef
		{
			for (int i = 0; i < this.infos.Count; i++)
			{
				if (this.infos[i].def != null && typeof(T).IsAssignableFrom(this.infos[i].def.GetType()))
				{
					def = (T)((object)this.infos[i].def);
					return true;
				}
			}
			def = default(T);
			return false;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x001C2EA8 File Offset: 0x001C10A8
		public ChoreTable CreateTable()
		{
			DebugUtil.Assert(this.interruptGroupId == 0);
			ChoreTable.Entry[] array = new ChoreTable.Entry[this.infos.Count];
			Stack<int> stack = new Stack<int>();
			int num = 10000;
			for (int i = 0; i < this.infos.Count; i++)
			{
				int num2 = (this.infos[i].forcePriority != -1) ? this.infos[i].forcePriority : (num - 100);
				num = num2;
				int num3 = 10000 - i * 100;
				int num4 = this.infos[i].interruptGroupId;
				if (num4 != 0)
				{
					if (stack.Count != num4)
					{
						stack.Push(num3);
					}
					else
					{
						num3 = stack.Peek();
					}
				}
				else if (stack.Count > 0)
				{
					stack.Pop();
				}
				array[i] = new ChoreTable.Entry(this.infos[i].def, num2, num3);
			}
			return new ChoreTable(array);
		}

		// Token: 0x040016AD RID: 5805
		private int interruptGroupId;

		// Token: 0x040016AE RID: 5806
		private List<ChoreTable.Builder.Info> infos = new List<ChoreTable.Builder.Info>();

		// Token: 0x040016AF RID: 5807
		private const int INVALID_PRIORITY = -1;

		// Token: 0x02000777 RID: 1911
		private struct Info
		{
			// Token: 0x040016B0 RID: 5808
			public int interruptGroupId;

			// Token: 0x040016B1 RID: 5809
			public int forcePriority;

			// Token: 0x040016B2 RID: 5810
			public StateMachine.BaseDef def;
		}
	}

	// Token: 0x02000779 RID: 1913
	public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType> where StateMachineInstanceType : StateMachine.Instance
	{
		// Token: 0x0600227D RID: 8829 RVA: 0x001C2FA4 File Offset: 0x001C11A4
		public ChoreTableChore(StateMachine.BaseDef state_machine_def, ChoreType chore_type, KPrefabID prefab_id) : base(chore_type, prefab_id, prefab_id.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
		{
			this.showAvailabilityInHoverText = false;
			base.smi = (state_machine_def.CreateSMI(this) as StateMachineInstanceType);
		}
	}

	// Token: 0x0200077A RID: 1914
	public struct Entry
	{
		// Token: 0x0600227E RID: 8830 RVA: 0x001C2FEC File Offset: 0x001C11EC
		public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
		{
			Type stateMachineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
			Type[] typeArguments = new Type[]
			{
				state_machine_def.GetStateMachineType(),
				stateMachineInstanceType
			};
			this.choreClassType = typeof(ChoreTable.ChoreTableChore<, >).MakeGenericType(typeArguments);
			this.choreType = new ChoreType(state_machine_def.ToString(), null, new string[0], "", "", "", "", new Tag[0], priority, priority);
			this.choreType.interruptPriority = interrupt_priority;
			this.stateMachineDef = state_machine_def;
		}

		// Token: 0x040016B4 RID: 5812
		public Type choreClassType;

		// Token: 0x040016B5 RID: 5813
		public ChoreType choreType;

		// Token: 0x040016B6 RID: 5814
		public StateMachine.BaseDef stateMachineDef;
	}

	// Token: 0x0200077B RID: 1915
	public class Instance
	{
		// Token: 0x0600227F RID: 8831 RVA: 0x001C3080 File Offset: 0x001C1280
		public static void ResetParameters()
		{
			for (int i = 0; i < ChoreTable.Instance.parameters.Length; i++)
			{
				ChoreTable.Instance.parameters[i] = null;
			}
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x001C30A8 File Offset: 0x001C12A8
		public Instance(ChoreTable chore_table, KPrefabID prefab_id)
		{
			this.prefabId = prefab_id;
			this.entries = ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.Allocate();
			for (int i = 0; i < chore_table.entries.Length; i++)
			{
				this.entries.Add(new ChoreTable.Instance.Entry(chore_table.entries[i], prefab_id));
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x001C3100 File Offset: 0x001C1300
		~Instance()
		{
			this.OnCleanUp(this.prefabId);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x001C3134 File Offset: 0x001C1334
		public void OnCleanUp(KPrefabID prefab_id)
		{
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].OnCleanUp(prefab_id);
			}
			this.entries.Recycle();
			this.entries = null;
		}

		// Token: 0x040016B7 RID: 5815
		private static object[] parameters = new object[3];

		// Token: 0x040016B8 RID: 5816
		private KPrefabID prefabId;

		// Token: 0x040016B9 RID: 5817
		private ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList entries;

		// Token: 0x0200077C RID: 1916
		private struct Entry
		{
			// Token: 0x06002284 RID: 8836 RVA: 0x001C3188 File Offset: 0x001C1388
			public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
			{
				ChoreTable.Instance.parameters[0] = chore_table_entry.stateMachineDef;
				ChoreTable.Instance.parameters[1] = chore_table_entry.choreType;
				ChoreTable.Instance.parameters[2] = prefab_id;
				this.chore = (Chore)Activator.CreateInstance(chore_table_entry.choreClassType, ChoreTable.Instance.parameters);
				ChoreTable.Instance.parameters[0] = null;
				ChoreTable.Instance.parameters[1] = null;
				ChoreTable.Instance.parameters[2] = null;
			}

			// Token: 0x06002285 RID: 8837 RVA: 0x000B6795 File Offset: 0x000B4995
			public void OnCleanUp(KPrefabID prefab_id)
			{
				if (this.chore != null)
				{
					this.chore.Cancel("ChoreTable.Instance.OnCleanUp");
					this.chore = null;
				}
			}

			// Token: 0x040016BA RID: 5818
			public Chore chore;
		}
	}
}
