using System;
using System.Collections.Generic;

public class ChoreTable
{
	public class Builder
	{
		private struct Info
		{
			public int interruptGroupId;

			public int forcePriority;

			public StateMachine.BaseDef def;
		}

		private int interruptGroupId;

		private List<Info> infos = new List<Info>();

		private const int INVALID_PRIORITY = -1;

		public Builder PushInterruptGroup()
		{
			interruptGroupId++;
			return this;
		}

		public Builder PopInterruptGroup()
		{
			DebugUtil.Assert(interruptGroupId > 0);
			interruptGroupId--;
			return this;
		}

		public Builder Add(StateMachine.BaseDef def, bool condition = true, int forcePriority = -1)
		{
			if (condition)
			{
				Info info = default(Info);
				info.interruptGroupId = interruptGroupId;
				info.forcePriority = forcePriority;
				info.def = def;
				Info item = info;
				infos.Add(item);
			}
			return this;
		}

		public bool HasChoreType(Type choreType)
		{
			return infos.Exists((Info info) => info.def.GetType() == choreType);
		}

		public bool TryGetChoreDef<T>(out T def) where T : StateMachine.BaseDef
		{
			for (int i = 0; i < infos.Count; i++)
			{
				if (infos[i].def != null && typeof(T).IsAssignableFrom(infos[i].def.GetType()))
				{
					def = (T)infos[i].def;
					return true;
				}
			}
			def = null;
			return false;
		}

		public ChoreTable CreateTable()
		{
			DebugUtil.Assert(interruptGroupId == 0);
			Entry[] array = new Entry[infos.Count];
			Stack<int> stack = new Stack<int>();
			int num = 10000;
			for (int i = 0; i < infos.Count; i++)
			{
				int num2 = ((infos[i].forcePriority != -1) ? infos[i].forcePriority : (num - 100));
				num = num2;
				int num3 = 10000 - i * 100;
				int num4 = infos[i].interruptGroupId;
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
				array[i] = new Entry(infos[i].def, num2, num3);
			}
			return new ChoreTable(array);
		}
	}

	public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType> where StateMachineInstanceType : StateMachine.Instance
	{
		public ChoreTableChore(StateMachine.BaseDef state_machine_def, ChoreType chore_type, KPrefabID prefab_id)
			: base(chore_type, (IStateMachineTarget)prefab_id, prefab_id.GetComponent<ChoreProvider>(), run_until_complete: true, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.basic, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
		{
			showAvailabilityInHoverText = false;
			base.smi = state_machine_def.CreateSMI(this) as StateMachineInstanceType;
		}
	}

	public struct Entry
	{
		public Type choreClassType;

		public ChoreType choreType;

		public StateMachine.BaseDef stateMachineDef;

		public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
		{
			Type stateMachineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
			Type[] typeArguments = new Type[2]
			{
				state_machine_def.GetStateMachineType(),
				stateMachineInstanceType
			};
			choreClassType = typeof(ChoreTableChore<, >).MakeGenericType(typeArguments);
			choreType = new ChoreType(state_machine_def.ToString(), null, new string[0], "", "", "", "", new Tag[0], priority, priority);
			choreType.interruptPriority = interrupt_priority;
			stateMachineDef = state_machine_def;
		}
	}

	public class Instance
	{
		private struct Entry
		{
			public Chore chore;

			public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
			{
				parameters[0] = chore_table_entry.stateMachineDef;
				parameters[1] = chore_table_entry.choreType;
				parameters[2] = prefab_id;
				chore = (Chore)Activator.CreateInstance(chore_table_entry.choreClassType, parameters);
				parameters[0] = null;
				parameters[1] = null;
				parameters[2] = null;
			}

			public void OnCleanUp(KPrefabID prefab_id)
			{
				if (chore != null)
				{
					chore.Cancel("ChoreTable.Instance.OnCleanUp");
					chore = null;
				}
			}
		}

		private static object[] parameters = new object[3];

		private KPrefabID prefabId;

		private ListPool<Entry, Instance>.PooledList entries;

		public static void ResetParameters()
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				parameters[i] = null;
			}
		}

		public Instance(ChoreTable chore_table, KPrefabID prefab_id)
		{
			prefabId = prefab_id;
			entries = ListPool<Entry, Instance>.Allocate();
			for (int i = 0; i < chore_table.entries.Length; i++)
			{
				entries.Add(new Entry(chore_table.entries[i], prefab_id));
			}
		}

		~Instance()
		{
			OnCleanUp(prefabId);
		}

		public void OnCleanUp(KPrefabID prefab_id)
		{
			if (entries != null)
			{
				for (int i = 0; i < entries.Count; i++)
				{
					entries[i].OnCleanUp(prefab_id);
				}
				entries.Recycle();
				entries = null;
			}
		}
	}

	private Entry[] entries;

	public static Entry InvalidEntry;

	public ChoreTable(Entry[] entries)
	{
		this.entries = entries;
	}

	public ref Entry GetEntry<T>()
	{
		ref Entry result = ref InvalidEntry;
		for (int i = 0; i < entries.Length; i++)
		{
			if (entries[i].stateMachineDef is T)
			{
				result = ref entries[i];
				break;
			}
		}
		return ref result;
	}

	public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
	{
		for (int i = 0; i < entries.Length; i++)
		{
			Entry entry = entries[i];
			if (entry.stateMachineDef.GetStateMachineType() == typeof(StateMachineType))
			{
				return entry.choreType.priority;
			}
		}
		Debug.LogError(chore_consumer.name + "'s chore table does not have an entry for: " + typeof(StateMachineType).Name);
		return -1;
	}
}
