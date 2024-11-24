using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KSerialization;
using UnityEngine;

// Token: 0x020008C0 RID: 2240
public abstract class StateMachine
{
	// Token: 0x060027B0 RID: 10160 RVA: 0x000B9CA1 File Offset: 0x000B7EA1
	public StateMachine()
	{
		this.name = base.GetType().FullName;
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000B9CC6 File Offset: 0x000B7EC6
	public virtual void FreeResources()
	{
		this.name = null;
		if (this.defaultState != null)
		{
			this.defaultState.FreeResources();
		}
		this.defaultState = null;
		this.parameters = null;
	}

	// Token: 0x060027B2 RID: 10162
	public abstract string[] GetStateNames();

	// Token: 0x060027B3 RID: 10163
	public abstract StateMachine.BaseState GetState(string name);

	// Token: 0x060027B4 RID: 10164
	public abstract void BindStates();

	// Token: 0x060027B5 RID: 10165
	public abstract Type GetStateMachineInstanceType();

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060027B6 RID: 10166 RVA: 0x000B9CF0 File Offset: 0x000B7EF0
	// (set) Token: 0x060027B7 RID: 10167 RVA: 0x000B9CF8 File Offset: 0x000B7EF8
	public int version { get; protected set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060027B8 RID: 10168 RVA: 0x000B9D01 File Offset: 0x000B7F01
	// (set) Token: 0x060027B9 RID: 10169 RVA: 0x000B9D09 File Offset: 0x000B7F09
	public StateMachine.SerializeType serializable { get; protected set; }

	// Token: 0x060027BA RID: 10170 RVA: 0x000B9D12 File Offset: 0x000B7F12
	public virtual void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = null;
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x001D1C68 File Offset: 0x001CFE68
	public void InitializeStateMachine()
	{
		this.debugSettings = StateMachineDebuggerSettings.Get().CreateEntry(base.GetType());
		StateMachine.BaseState baseState = null;
		this.InitializeStates(out baseState);
		DebugUtil.Assert(baseState != null);
		this.defaultState = baseState;
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x001D1CA8 File Offset: 0x001CFEA8
	public void CreateStates(object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			object[] customAttributes = fieldInfo.GetCustomAttributes(false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j].GetType() == typeof(StateMachine.DoNotAutoCreate))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
				{
					StateMachine.BaseState baseState = (StateMachine.BaseState)Activator.CreateInstance(fieldInfo.FieldType);
					this.CreateStates(baseState);
					fieldInfo.SetValue(state_machine, baseState);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.Parameter)))
				{
					StateMachine.Parameter parameter = (StateMachine.Parameter)fieldInfo.GetValue(state_machine);
					if (parameter == null)
					{
						parameter = (StateMachine.Parameter)Activator.CreateInstance(fieldInfo.FieldType);
						fieldInfo.SetValue(state_machine, parameter);
					}
					parameter.name = fieldInfo.Name;
					parameter.idx = this.parameters.Length;
					this.parameters = this.parameters.Append(parameter);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine)))
				{
					fieldInfo.SetValue(state_machine, this);
				}
			}
		}
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x000B9D17 File Offset: 0x000B7F17
	public StateMachine.BaseState GetDefaultState()
	{
		return this.defaultState;
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000B9D1F File Offset: 0x000B7F1F
	public int GetMaxDepth()
	{
		return this.maxDepth;
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000B9D27 File Offset: 0x000B7F27
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x04001ACE RID: 6862
	protected string name;

	// Token: 0x04001ACF RID: 6863
	protected int maxDepth;

	// Token: 0x04001AD0 RID: 6864
	protected StateMachine.BaseState defaultState;

	// Token: 0x04001AD1 RID: 6865
	protected StateMachine.Parameter[] parameters = new StateMachine.Parameter[0];

	// Token: 0x04001AD2 RID: 6866
	public int dataTableSize;

	// Token: 0x04001AD3 RID: 6867
	public int updateTableSize;

	// Token: 0x04001AD6 RID: 6870
	public StateMachineDebuggerSettings.Entry debugSettings;

	// Token: 0x04001AD7 RID: 6871
	public bool saveHistory;

	// Token: 0x020008C1 RID: 2241
	public sealed class DoNotAutoCreate : Attribute
	{
	}

	// Token: 0x020008C2 RID: 2242
	public enum Status
	{
		// Token: 0x04001AD9 RID: 6873
		Initialized,
		// Token: 0x04001ADA RID: 6874
		Running,
		// Token: 0x04001ADB RID: 6875
		Failed,
		// Token: 0x04001ADC RID: 6876
		Success
	}

	// Token: 0x020008C3 RID: 2243
	public class BaseDef
	{
		// Token: 0x060027C1 RID: 10177 RVA: 0x000B9D2F File Offset: 0x000B7F2F
		public StateMachine.Instance CreateSMI(IStateMachineTarget master)
		{
			return Singleton<StateMachineManager>.Instance.CreateSMIFromDef(master, this);
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000B9D3D File Offset: 0x000B7F3D
		public Type GetStateMachineType()
		{
			return base.GetType().DeclaringType;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void Configure(GameObject prefab)
		{
		}

		// Token: 0x04001ADD RID: 6877
		public bool preventStartSMIOnSpawn;
	}

	// Token: 0x020008C4 RID: 2244
	public class Category : Resource
	{
		// Token: 0x060027C5 RID: 10181 RVA: 0x000B68D8 File Offset: 0x000B4AD8
		public Category(string id) : base(id, null, null)
		{
		}
	}

	// Token: 0x020008C5 RID: 2245
	[SerializationConfig(MemberSerialization.OptIn)]
	public abstract class Instance
	{
		// Token: 0x060027C6 RID: 10182
		public abstract StateMachine.BaseState GetCurrentState();

		// Token: 0x060027C7 RID: 10183
		public abstract void GoTo(StateMachine.BaseState state);

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060027C8 RID: 10184
		public abstract float timeinstate { get; }

		// Token: 0x060027C9 RID: 10185
		public abstract IStateMachineTarget GetMaster();

		// Token: 0x060027CA RID: 10186
		public abstract void StopSM(string reason);

		// Token: 0x060027CB RID: 10187
		public abstract SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null);

		// Token: 0x060027CC RID: 10188
		public abstract SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null);

		// Token: 0x060027CD RID: 10189 RVA: 0x000B9D4A File Offset: 0x000B7F4A
		public virtual void FreeResources()
		{
			this.stateMachine = null;
			if (this.subscribedEvents != null)
			{
				this.subscribedEvents.Clear();
			}
			this.subscribedEvents = null;
			this.parameterContexts = null;
			this.dataTable = null;
			this.updateTable = null;
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000B9D82 File Offset: 0x000B7F82
		public Instance(StateMachine state_machine, IStateMachineTarget master)
		{
			this.stateMachine = state_machine;
			this.CreateParameterContexts();
			this.log = new LoggerFSSSS(this.stateMachine.name, 35);
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000B9DBA File Offset: 0x000B7FBA
		public bool IsRunning()
		{
			return this.GetCurrentState() != null;
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x001D1DF4 File Offset: 0x001CFFF4
		public void GoTo(string state_name)
		{
			DebugUtil.DevAssert(!KMonoBehaviour.isLoadingScene, "Using Goto while scene was loaded", null);
			StateMachine.BaseState state = this.stateMachine.GetState(state_name);
			this.GoTo(state);
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000B9DC5 File Offset: 0x000B7FC5
		public int GetStackSize()
		{
			return this.stackSize;
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000B9DCD File Offset: 0x000B7FCD
		public StateMachine GetStateMachine()
		{
			return this.stateMachine;
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000A5E40 File Offset: 0x000A4040
		[Conditional("UNITY_EDITOR")]
		public void Log(string a, string b = "", string c = "", string d = "")
		{
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000B9DD5 File Offset: 0x000B7FD5
		public bool IsConsoleLoggingEnabled()
		{
			return this.enableConsoleLogging || this.stateMachine.debugSettings.enableConsoleLogging;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000B9DF1 File Offset: 0x000B7FF1
		public bool IsBreakOnGoToEnabled()
		{
			return this.breakOnGoTo || this.stateMachine.debugSettings.breakOnGoTo;
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000B9E0D File Offset: 0x000B800D
		public LoggerFSSSS GetLog()
		{
			return this.log;
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000B9E15 File Offset: 0x000B8015
		public StateMachine.Parameter.Context[] GetParameterContexts()
		{
			return this.parameterContexts;
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000B9E1D File Offset: 0x000B801D
		public StateMachine.Parameter.Context GetParameterContext(StateMachine.Parameter parameter)
		{
			return this.parameterContexts[parameter.idx];
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x000B9E2C File Offset: 0x000B802C
		public StateMachine.Status GetStatus()
		{
			return this.status;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000B9E34 File Offset: 0x000B8034
		public void SetStatus(StateMachine.Status status)
		{
			this.status = status;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000B9E3D File Offset: 0x000B803D
		public void Error()
		{
			if (!StateMachine.Instance.error)
			{
				this.isCrashed = true;
				StateMachine.Instance.error = true;
				RestartWarning.ShouldWarn = true;
			}
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x001D1E28 File Offset: 0x001D0028
		public override string ToString()
		{
			string str = "";
			if (this.GetCurrentState() != null)
			{
				str = this.GetCurrentState().name;
			}
			else if (this.GetStatus() != StateMachine.Status.Initialized)
			{
				str = this.GetStatus().ToString();
			}
			return this.stateMachine.ToString() + "(" + str + ")";
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x001D1E8C File Offset: 0x001D008C
		public virtual void StartSM()
		{
			if (!this.IsRunning())
			{
				StateMachineController component = this.GetComponent<StateMachineController>();
				MyAttributes.OnStart(this, component);
				StateMachine.BaseState defaultState = this.stateMachine.GetDefaultState();
				DebugUtil.Assert(defaultState != null);
				if (!component.Restore(this))
				{
					this.GoTo(defaultState);
				}
			}
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000B9E59 File Offset: 0x000B8059
		public bool HasTag(Tag tag)
		{
			return this.GetComponent<KPrefabID>().HasTag(tag);
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x001D1ED4 File Offset: 0x001D00D4
		public bool IsInsideState(StateMachine.BaseState state)
		{
			StateMachine.BaseState currentState = this.GetCurrentState();
			if (currentState == null)
			{
				return false;
			}
			bool flag = state == currentState;
			int num = 0;
			while (!flag && num < currentState.branch.Length && !(flag = (state == currentState.branch[num])))
			{
				num++;
			}
			return flag;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000B9E67 File Offset: 0x000B8067
		public void ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			if (this.scheduleGoToCallback == null)
			{
				this.scheduleGoToCallback = delegate(object d)
				{
					this.GoTo((StateMachine.BaseState)d);
				};
			}
			this.Schedule(time, this.scheduleGoToCallback, state);
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000B9E92 File Offset: 0x000B8092
		public void Subscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Subscribe(hash, handler);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000B9EA2 File Offset: 0x000B80A2
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Unsubscribe(hash, handler);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000B9EB1 File Offset: 0x000B80B1
		public void Trigger(int hash, object data = null)
		{
			this.GetMaster().GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000B9EC5 File Offset: 0x000B80C5
		public ComponentType Get<ComponentType>()
		{
			return this.GetComponent<ComponentType>();
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000B9ECD File Offset: 0x000B80CD
		public ComponentType GetComponent<ComponentType>()
		{
			return this.GetMaster().GetComponent<ComponentType>();
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x001D1F18 File Offset: 0x001D0118
		private void CreateParameterContexts()
		{
			this.parameterContexts = new StateMachine.Parameter.Context[this.stateMachine.parameters.Length];
			for (int i = 0; i < this.stateMachine.parameters.Length; i++)
			{
				this.parameterContexts[i] = this.stateMachine.parameters[i].CreateContext();
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x000B9EDA File Offset: 0x000B80DA
		public GameObject gameObject
		{
			get
			{
				return this.GetMaster().gameObject;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x000B9EE7 File Offset: 0x000B80E7
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x04001ADE RID: 6878
		public string serializationSuffix;

		// Token: 0x04001ADF RID: 6879
		protected LoggerFSSSS log;

		// Token: 0x04001AE0 RID: 6880
		protected StateMachine.Status status;

		// Token: 0x04001AE1 RID: 6881
		protected StateMachine stateMachine;

		// Token: 0x04001AE2 RID: 6882
		protected Stack<StateEvent.Context> subscribedEvents = new Stack<StateEvent.Context>();

		// Token: 0x04001AE3 RID: 6883
		protected int stackSize;

		// Token: 0x04001AE4 RID: 6884
		protected StateMachine.Parameter.Context[] parameterContexts;

		// Token: 0x04001AE5 RID: 6885
		public object[] dataTable;

		// Token: 0x04001AE6 RID: 6886
		public StateMachine.Instance.UpdateTableEntry[] updateTable;

		// Token: 0x04001AE7 RID: 6887
		private Action<object> scheduleGoToCallback;

		// Token: 0x04001AE8 RID: 6888
		public Action<string, StateMachine.Status> OnStop;

		// Token: 0x04001AE9 RID: 6889
		public bool breakOnGoTo;

		// Token: 0x04001AEA RID: 6890
		public bool enableConsoleLogging;

		// Token: 0x04001AEB RID: 6891
		public bool isCrashed;

		// Token: 0x04001AEC RID: 6892
		public static bool error;

		// Token: 0x020008C6 RID: 2246
		public struct UpdateTableEntry
		{
			// Token: 0x04001AED RID: 6893
			public HandleVector<int>.Handle handle;

			// Token: 0x04001AEE RID: 6894
			public StateMachineUpdater.BaseUpdateBucket bucket;
		}
	}

	// Token: 0x020008C7 RID: 2247
	[DebuggerDisplay("{longName}")]
	public class BaseState
	{
		// Token: 0x060027EA RID: 10218 RVA: 0x000B9F02 File Offset: 0x000B8102
		public BaseState()
		{
			this.branch = new StateMachine.BaseState[1];
			this.branch[0] = this;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x001D1F70 File Offset: 0x001D0170
		public void FreeResources()
		{
			if (this.name == null)
			{
				return;
			}
			this.name = null;
			if (this.defaultState != null)
			{
				this.defaultState.FreeResources();
			}
			this.defaultState = null;
			this.events = null;
			int num = 0;
			while (this.transitions != null && num < this.transitions.Count)
			{
				this.transitions[num].Clear();
				num++;
			}
			this.transitions = null;
			this.enterActions = null;
			this.exitActions = null;
			if (this.branch != null)
			{
				for (int i = 0; i < this.branch.Length; i++)
				{
					this.branch[i].FreeResources();
				}
			}
			this.branch = null;
			this.parent = null;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000B9F1F File Offset: 0x000B811F
		public int GetStateCount()
		{
			return this.branch.Length;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000B9F29 File Offset: 0x000B8129
		public StateMachine.BaseState GetState(int idx)
		{
			return this.branch[idx];
		}

		// Token: 0x04001AEF RID: 6895
		public string name;

		// Token: 0x04001AF0 RID: 6896
		public string longName;

		// Token: 0x04001AF1 RID: 6897
		public string debugPushName;

		// Token: 0x04001AF2 RID: 6898
		public string debugPopName;

		// Token: 0x04001AF3 RID: 6899
		public string debugExecuteName;

		// Token: 0x04001AF4 RID: 6900
		public StateMachine.BaseState defaultState;

		// Token: 0x04001AF5 RID: 6901
		public List<StateEvent> events;

		// Token: 0x04001AF6 RID: 6902
		public List<StateMachine.BaseTransition> transitions;

		// Token: 0x04001AF7 RID: 6903
		public List<StateMachine.UpdateAction> updateActions;

		// Token: 0x04001AF8 RID: 6904
		public List<StateMachine.Action> enterActions;

		// Token: 0x04001AF9 RID: 6905
		public List<StateMachine.Action> exitActions;

		// Token: 0x04001AFA RID: 6906
		public StateMachine.BaseState[] branch;

		// Token: 0x04001AFB RID: 6907
		public StateMachine.BaseState parent;
	}

	// Token: 0x020008C8 RID: 2248
	public class BaseTransition
	{
		// Token: 0x060027EE RID: 10222 RVA: 0x000B9F33 File Offset: 0x000B8133
		public BaseTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state)
		{
			this.idx = idx;
			this.name = name;
			this.sourceState = source_state;
			this.targetState = target_state;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void Evaluate(StateMachine.Instance smi)
		{
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000B9F58 File Offset: 0x000B8158
		public virtual StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			return new StateMachine.BaseTransition.Context(this);
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000B9F60 File Offset: 0x000B8160
		public void Clear()
		{
			this.name = null;
			if (this.sourceState != null)
			{
				this.sourceState.FreeResources();
			}
			this.sourceState = null;
			if (this.targetState != null)
			{
				this.targetState.FreeResources();
			}
			this.targetState = null;
		}

		// Token: 0x04001AFC RID: 6908
		public int idx;

		// Token: 0x04001AFD RID: 6909
		public string name;

		// Token: 0x04001AFE RID: 6910
		public StateMachine.BaseState sourceState;

		// Token: 0x04001AFF RID: 6911
		public StateMachine.BaseState targetState;

		// Token: 0x020008C9 RID: 2249
		public struct Context
		{
			// Token: 0x060027F3 RID: 10227 RVA: 0x000B9F9D File Offset: 0x000B819D
			public Context(StateMachine.BaseTransition transition)
			{
				this.idx = transition.idx;
				this.handlerId = 0;
			}

			// Token: 0x04001B00 RID: 6912
			public int idx;

			// Token: 0x04001B01 RID: 6913
			public int handlerId;
		}
	}

	// Token: 0x020008CA RID: 2250
	public struct UpdateAction
	{
		// Token: 0x04001B02 RID: 6914
		public int updateTableIdx;

		// Token: 0x04001B03 RID: 6915
		public UpdateRate updateRate;

		// Token: 0x04001B04 RID: 6916
		public int nextBucketIdx;

		// Token: 0x04001B05 RID: 6917
		public StateMachineUpdater.BaseUpdateBucket[] buckets;

		// Token: 0x04001B06 RID: 6918
		public object updater;
	}

	// Token: 0x020008CB RID: 2251
	public struct Action
	{
		// Token: 0x060027F4 RID: 10228 RVA: 0x000B9FB2 File Offset: 0x000B81B2
		public Action(string name, object callback)
		{
			this.name = name;
			this.callback = callback;
		}

		// Token: 0x04001B07 RID: 6919
		public string name;

		// Token: 0x04001B08 RID: 6920
		public object callback;
	}

	// Token: 0x020008CC RID: 2252
	public class ParameterTransition : StateMachine.BaseTransition
	{
		// Token: 0x060027F5 RID: 10229 RVA: 0x000B9FC2 File Offset: 0x000B81C2
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020008CD RID: 2253
	public abstract class Parameter
	{
		// Token: 0x060027F6 RID: 10230
		public abstract StateMachine.Parameter.Context CreateContext();

		// Token: 0x04001B09 RID: 6921
		public string name;

		// Token: 0x04001B0A RID: 6922
		public int idx;

		// Token: 0x020008CE RID: 2254
		public abstract class Context
		{
			// Token: 0x060027F8 RID: 10232 RVA: 0x000B9FCF File Offset: 0x000B81CF
			public Context(StateMachine.Parameter parameter)
			{
				this.parameter = parameter;
			}

			// Token: 0x060027F9 RID: 10233
			public abstract void Serialize(BinaryWriter writer);

			// Token: 0x060027FA RID: 10234
			public abstract void Deserialize(IReader reader, StateMachine.Instance smi);

			// Token: 0x060027FB RID: 10235 RVA: 0x000A5E40 File Offset: 0x000A4040
			public virtual void Cleanup()
			{
			}

			// Token: 0x060027FC RID: 10236
			public abstract void ShowEditor(StateMachine.Instance base_smi);

			// Token: 0x060027FD RID: 10237
			public abstract void ShowDevTool(StateMachine.Instance base_smi);

			// Token: 0x04001B0B RID: 6923
			public StateMachine.Parameter parameter;
		}
	}

	// Token: 0x020008CF RID: 2255
	public enum SerializeType
	{
		// Token: 0x04001B0D RID: 6925
		Never,
		// Token: 0x04001B0E RID: 6926
		ParamsOnly,
		// Token: 0x04001B0F RID: 6927
		CurrentStateOnly_DEPRECATED,
		// Token: 0x04001B10 RID: 6928
		Both_DEPRECATED
	}
}
