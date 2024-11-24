using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ImGuiNET;
using KSerialization;
using UnityEngine;

// Token: 0x020008D0 RID: 2256
public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine where StateMachineInstanceType : StateMachine.Instance where MasterType : IStateMachineTarget
{
	// Token: 0x060027FE RID: 10238 RVA: 0x001D2028 File Offset: 0x001D0228
	public override string[] GetStateNames()
	{
		List<string> list = new List<string>();
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			list.Add(state.name);
		}
		return list.ToArray();
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000B9FDE File Offset: 0x000B81DE
	public void Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
	{
		this.stateTarget = target;
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x001D208C File Offset: 0x001D028C
	public void BindState(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, string state_name)
	{
		if (parent_state != null)
		{
			state_name = parent_state.name + "." + state_name;
		}
		state.name = state_name;
		state.longName = this.name + "." + state_name;
		state.debugPushName = "PuS: " + state.longName;
		state.debugPopName = "PoS: " + state.longName;
		state.debugExecuteName = "EA: " + state.longName;
		List<StateMachine.BaseState> list;
		if (parent_state != null)
		{
			list = new List<StateMachine.BaseState>(parent_state.branch);
		}
		else
		{
			list = new List<StateMachine.BaseState>();
		}
		list.Add(state);
		state.parent = parent_state;
		state.branch = list.ToArray();
		this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
		this.states.Add(state);
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x001D2168 File Offset: 0x001D0368
	public void BindStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)fieldInfo.GetValue(state_machine);
				if (state != parent_state)
				{
					string name = fieldInfo.Name;
					this.BindState(parent_state, state, name);
					this.BindStates(state, state);
				}
			}
		}
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000B9FE7 File Offset: 0x000B81E7
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000B9FF0 File Offset: 0x000B81F0
	public override void BindStates()
	{
		this.BindStates(null, this);
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000B9FFA File Offset: 0x000B81FA
	public override Type GetStateMachineInstanceType()
	{
		return typeof(StateMachineInstanceType);
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x001D21D8 File Offset: 0x001D03D8
	public override StateMachine.BaseState GetState(string state_name)
	{
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			if (state.name == state_name)
			{
				return state;
			}
		}
		return null;
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x001D223C File Offset: 0x001D043C
	public override void FreeResources()
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			this.states[i].FreeResources();
		}
		this.states.Clear();
		base.FreeResources();
	}

	// Token: 0x04001B11 RID: 6929
	private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();

	// Token: 0x04001B12 RID: 6930
	public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;

	// Token: 0x04001B13 RID: 6931
	[StateMachine.DoNotAutoCreate]
	protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

	// Token: 0x020008D1 RID: 2257
	public class GenericInstance : StateMachine.Instance
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x000BA019 File Offset: 0x000B8219
		// (set) Token: 0x06002809 RID: 10249 RVA: 0x000BA021 File Offset: 0x000B8221
		public StateMachineType sm { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x000BA02A File Offset: 0x000B822A
		protected StateMachineInstanceType smi
		{
			get
			{
				return (StateMachineInstanceType)((object)this);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x000BA032 File Offset: 0x000B8232
		// (set) Token: 0x0600280C RID: 10252 RVA: 0x000BA03A File Offset: 0x000B823A
		public MasterType master { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600280D RID: 10253 RVA: 0x000BA043 File Offset: 0x000B8243
		// (set) Token: 0x0600280E RID: 10254 RVA: 0x000BA04B File Offset: 0x000B824B
		public DefType def { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600280F RID: 10255 RVA: 0x000BA054 File Offset: 0x000B8254
		public bool isMasterNull
		{
			get
			{
				return this.internalSm.masterTarget.IsNull((StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x000BA06C File Offset: 0x000B826C
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
		{
			get
			{
				return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>)((object)this.sm);
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected virtual void OnCleanUp()
		{
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000BA07E File Offset: 0x000B827E
		public override float timeinstate
		{
			get
			{
				return Time.time - this.stateEnterTime;
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x001D2284 File Offset: 0x001D0484
		public override void FreeResources()
		{
			this.updateHandle.FreeResources();
			this.updateHandle = default(SchedulerHandle);
			this.controller = null;
			if (this.gotoStack != null)
			{
				this.gotoStack.Clear();
			}
			this.gotoStack = null;
			if (this.transitionStack != null)
			{
				this.transitionStack.Clear();
			}
			this.transitionStack = null;
			if (this.currentSchedulerGroup != null)
			{
				this.currentSchedulerGroup.FreeResources();
			}
			this.currentSchedulerGroup = null;
			if (this.stateStack != null)
			{
				for (int i = 0; i < this.stateStack.Length; i++)
				{
					if (this.stateStack[i].schedulerGroup != null)
					{
						this.stateStack[i].schedulerGroup.FreeResources();
					}
				}
			}
			this.stateStack = null;
			base.FreeResources();
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x001D2350 File Offset: 0x001D0550
		public GenericInstance(MasterType master) : base((StateMachine)((object)Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>()), master)
		{
			this.master = master;
			this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
			for (int i = 0; i < this.stateStack.Length; i++)
			{
				this.stateStack[i].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
			}
			this.sm = (StateMachineType)((object)this.stateMachine);
			this.dataTable = new object[base.GetStateMachine().dataTableSize];
			this.updateTable = new StateMachine.Instance.UpdateTableEntry[base.GetStateMachine().updateTableSize];
			this.controller = master.GetComponent<StateMachineController>();
			if (this.controller == null)
			{
				this.controller = master.gameObject.AddComponent<StateMachineController>();
			}
			this.internalSm.masterTarget.Set(master.gameObject, this.smi, false);
			this.controller.AddStateMachineInstance(this);
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000BA08C File Offset: 0x000B828C
		public override IStateMachineTarget GetMaster()
		{
			return this.master;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x001D248C File Offset: 0x001D068C
		private void PushEvent(StateEvent evt)
		{
			StateEvent.Context item = evt.Subscribe(this);
			this.subscribedEvents.Push(item);
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x001D24B0 File Offset: 0x001D06B0
		private void PopEvent()
		{
			StateEvent.Context context = this.subscribedEvents.Pop();
			context.stateEvent.Unsubscribe(this, context);
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x001D24D8 File Offset: 0x001D06D8
		private bool TryEvaluateTransitions(StateMachine.BaseState state, int goto_id)
		{
			if (state.transitions == null)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition baseTransition = state.transitions[i];
				if (goto_id != this.gotoId)
				{
					result = false;
					break;
				}
				baseTransition.Evaluate(this.smi);
			}
			return result;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x001D2534 File Offset: 0x001D0734
		private void PushTransitions(StateMachine.BaseState state)
		{
			if (state.transitions == null)
			{
				return;
			}
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition transition = state.transitions[i];
				this.PushTransition(transition);
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x001D2574 File Offset: 0x001D0774
		private void PushTransition(StateMachine.BaseTransition transition)
		{
			StateMachine.BaseTransition.Context item = transition.Register(this.smi);
			this.transitionStack.Push(item);
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x001D25A0 File Offset: 0x001D07A0
		private void PopTransition(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine.BaseTransition.Context context = this.transitionStack.Pop();
			state.transitions[context.idx].Unregister(this.smi, context);
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x001D25DC File Offset: 0x001D07DC
		private void PushState(StateMachine.BaseState state)
		{
			int num = this.gotoId;
			this.currentActionIdx = -1;
			if (state.events != null)
			{
				foreach (StateEvent evt in state.events)
				{
					this.PushEvent(evt);
				}
			}
			this.PushTransitions(state);
			if (state.updateActions != null)
			{
				for (int i = 0; i < state.updateActions.Count; i++)
				{
					StateMachine.UpdateAction updateAction = state.updateActions[i];
					int updateTableIdx = updateAction.updateTableIdx;
					int nextBucketIdx = updateAction.nextBucketIdx;
					updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
					UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = (UpdateBucketWithUpdater<StateMachineInstanceType>)updateAction.buckets[nextBucketIdx];
					this.smi.updateTable[updateTableIdx].bucket = updateBucketWithUpdater;
					this.smi.updateTable[updateTableIdx].handle = updateBucketWithUpdater.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, updateBucketWithUpdater.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater)updateAction.updater);
					state.updateActions[i] = updateAction;
				}
			}
			this.stateEnterTime = Time.time;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int stackSize = this.stackSize;
			this.stackSize = stackSize + 1;
			array[stackSize].state = state;
			this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
			if (!this.TryEvaluateTransitions(state, num))
			{
				return;
			}
			if (num != this.gotoId)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
			int num2 = this.gotoId;
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x001D27B8 File Offset: 0x001D09B8
		private void ExecuteActions(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, List<StateMachine.Action> actions)
		{
			if (actions == null)
			{
				return;
			}
			int num = this.gotoId;
			this.currentActionIdx++;
			while (this.currentActionIdx < actions.Count && num == this.gotoId)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback)actions[this.currentActionIdx].callback;
				try
				{
					callback(this.smi);
				}
				catch (Exception e)
				{
					if (!StateMachine.Instance.error)
					{
						base.Error();
						string text = "(NULL).";
						IStateMachineTarget master = this.GetMaster();
						if (!master.isNull)
						{
							KPrefabID component = master.GetComponent<KPrefabID>();
							if (component != null)
							{
								text = "(" + component.PrefabTag.ToString() + ").";
							}
							else
							{
								text = "(" + base.gameObject.name + ").";
							}
						}
						string text2 = string.Concat(new string[]
						{
							"Exception in: ",
							text,
							this.stateMachine.ToString(),
							".",
							state.name,
							"."
						});
						if (this.currentActionIdx > 0 && this.currentActionIdx < actions.Count)
						{
							text2 += actions[this.currentActionIdx].name;
						}
						DebugUtil.LogException(this.controller, text2, e);
					}
				}
				this.currentActionIdx++;
			}
			this.currentActionIdx = 2147483646;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x001D294C File Offset: 0x001D0B4C
		private void PopState()
		{
			this.currentActionIdx = -1;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int num = this.stackSize - 1;
			this.stackSize = num;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry stackEntry = array[num];
			StateMachine.BaseState state = stackEntry.state;
			int num2 = 0;
			while (state.transitions != null && num2 < state.transitions.Count)
			{
				this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state);
				num2++;
			}
			if (state.events != null)
			{
				for (int i = 0; i < state.events.Count; i++)
				{
					this.PopEvent();
				}
			}
			if (state.updateActions != null)
			{
				foreach (StateMachine.UpdateAction updateAction in state.updateActions)
				{
					int updateTableIdx = updateAction.updateTableIdx;
					StateMachineUpdater.BaseUpdateBucket baseUpdateBucket = (UpdateBucketWithUpdater<StateMachineInstanceType>)this.smi.updateTable[updateTableIdx].bucket;
					this.smi.updateTable[updateTableIdx].bucket = null;
					baseUpdateBucket.Remove(this.smi.updateTable[updateTableIdx].handle);
				}
			}
			stackEntry.schedulerGroup.Reset();
			this.currentSchedulerGroup = stackEntry.schedulerGroup;
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.exitActions);
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x001D2AB0 File Offset: 0x001D0CB0
		public override SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.Schedule(name, time, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x001D2AD4 File Offset: 0x001D0CD4
		public override SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.ScheduleNextFrame(name, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000BA099 File Offset: 0x000B8299
		public override void StartSM()
		{
			if (this.controller != null && !this.controller.HasStateMachineInstance(this))
			{
				this.controller.AddStateMachineInstance(this);
			}
			base.StartSM();
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x001D2AF8 File Offset: 0x001D0CF8
		public override void StopSM(string reason)
		{
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (!base.IsRunning())
			{
				return;
			}
			this.gotoId++;
			while (this.stackSize > 0)
			{
				this.PopState();
			}
			if (this.master != null && this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (this.status == StateMachine.Status.Running)
			{
				base.SetStatus(StateMachine.Status.Failed);
			}
			if (this.OnStop != null)
			{
				this.OnStop(reason, this.status);
			}
			for (int i = 0; i < this.parameterContexts.Length; i++)
			{
				this.parameterContexts[i].Cleanup();
			}
			this.OnCleanUp();
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000BA0C9 File Offset: 0x000B82C9
		private void FinishStateInProgress(StateMachine.BaseState state)
		{
			if (state.enterActions == null)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x001D2BC8 File Offset: 0x001D0DC8
		public override void GoTo(StateMachine.BaseState base_state)
		{
			if (App.IsExiting)
			{
				return;
			}
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.isMasterNull)
			{
				return;
			}
			if (this.smi.IsNullOrDestroyed())
			{
				return;
			}
			try
			{
				if (base.IsBreakOnGoToEnabled())
				{
					Debugger.Break();
				}
				if (base_state != null)
				{
					while (base_state.defaultState != null)
					{
						base_state = base_state.defaultState;
					}
				}
				if (this.GetCurrentState() == null)
				{
					base.SetStatus(StateMachine.Status.Running);
				}
				if (this.gotoStack.Count > 100)
				{
					string text = "Potential infinite transition loop detected in state machine: " + this.ToString() + "\nGoto stack:\n";
					foreach (StateMachine.BaseState baseState in this.gotoStack)
					{
						text = text + "\n" + baseState.name;
					}
					global::Debug.LogError(text);
					base.Error();
				}
				else
				{
					this.gotoStack.Push(base_state);
					if (base_state == null)
					{
						this.StopSM("StateMachine.GoTo(null)");
						this.gotoStack.Pop();
					}
					else
					{
						int num = this.gotoId + 1;
						this.gotoId = num;
						int num2 = num;
						StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
						int num3 = 0;
						while (num3 < this.stackSize && num3 < branch.Length && this.stateStack[num3].state == branch[num3])
						{
							num3++;
						}
						int num4 = this.stackSize - 1;
						if (num4 >= 0 && num4 == num3 - 1)
						{
							this.FinishStateInProgress(this.stateStack[num4].state);
						}
						while (this.stackSize > num3 && num2 == this.gotoId)
						{
							this.PopState();
						}
						int num5 = num3;
						while (num5 < branch.Length && num2 == this.gotoId)
						{
							this.PushState(branch[num5]);
							num5++;
						}
						this.gotoStack.Pop();
					}
				}
			}
			catch (Exception ex)
			{
				if (!StateMachine.Instance.error)
				{
					base.Error();
					string text2 = "(Stop)";
					if (base_state != null)
					{
						text2 = base_state.name;
					}
					string text3 = "(NULL).";
					if (!this.GetMaster().isNull)
					{
						text3 = "(" + base.gameObject.name + ").";
					}
					string str = string.Concat(new string[]
					{
						"Exception in: ",
						text3,
						this.stateMachine.ToString(),
						".GoTo(",
						text2,
						")"
					});
					DebugUtil.LogErrorArgs(this.controller, new object[]
					{
						str + "\n" + ex.ToString()
					});
				}
			}
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000BA0E6 File Offset: 0x000B82E6
		public override StateMachine.BaseState GetCurrentState()
		{
			if (this.stackSize > 0)
			{
				return this.stateStack[this.stackSize - 1].state;
			}
			return null;
		}

		// Token: 0x04001B14 RID: 6932
		private float stateEnterTime;

		// Token: 0x04001B15 RID: 6933
		private int gotoId;

		// Token: 0x04001B16 RID: 6934
		private int currentActionIdx = -1;

		// Token: 0x04001B17 RID: 6935
		private SchedulerHandle updateHandle;

		// Token: 0x04001B18 RID: 6936
		private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();

		// Token: 0x04001B19 RID: 6937
		protected Stack<StateMachine.BaseTransition.Context> transitionStack = new Stack<StateMachine.BaseTransition.Context>();

		// Token: 0x04001B1D RID: 6941
		protected StateMachineController controller;

		// Token: 0x04001B1E RID: 6942
		private SchedulerGroup currentSchedulerGroup;

		// Token: 0x04001B1F RID: 6943
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

		// Token: 0x020008D2 RID: 2258
		public struct StackEntry
		{
			// Token: 0x04001B20 RID: 6944
			public StateMachine.BaseState state;

			// Token: 0x04001B21 RID: 6945
			public SchedulerGroup schedulerGroup;
		}
	}

	// Token: 0x020008D3 RID: 2259
	public class State : StateMachine.BaseState
	{
		// Token: 0x04001B22 RID: 6946
		protected StateMachineType sm;

		// Token: 0x020008D4 RID: 2260
		// (Invoke) Token: 0x06002828 RID: 10280
		public delegate void Callback(StateMachineInstanceType smi);
	}

	// Token: 0x020008D5 RID: 2261
	public new abstract class ParameterTransition : StateMachine.ParameterTransition
	{
		// Token: 0x0600282B RID: 10283 RVA: 0x000BA113 File Offset: 0x000B8313
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020008D6 RID: 2262
	public class Transition : StateMachine.BaseTransition
	{
		// Token: 0x0600282C RID: 10284 RVA: 0x000BA120 File Offset: 0x000B8320
		public Transition(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition) : base(idx, name, source_state, target_state)
		{
			this.condition = condition;
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000BA135 File Offset: 0x000B8335
		public override string ToString()
		{
			if (this.targetState != null)
			{
				return this.name + "->" + this.targetState.name;
			}
			return this.name + "->(Stop)";
		}

		// Token: 0x04001B23 RID: 6947
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

		// Token: 0x020008D7 RID: 2263
		// (Invoke) Token: 0x0600282F RID: 10287
		public delegate bool ConditionCallback(StateMachineInstanceType smi);
	}

	// Token: 0x020008D8 RID: 2264
	public abstract class Parameter<ParameterType> : StateMachine.Parameter
	{
		// Token: 0x06002832 RID: 10290 RVA: 0x000BA16B File Offset: 0x000B836B
		public Parameter()
		{
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000BA173 File Offset: 0x000B8373
		public Parameter(ParameterType default_value)
		{
			this.defaultValue = default_value;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000BA182 File Offset: 0x000B8382
		public ParameterType Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).Set(value, smi, silenceEvents);
			return value;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000BA19E File Offset: 0x000B839E
		public ParameterType Get(StateMachineInstanceType smi)
		{
			return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).value;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000BA1B6 File Offset: 0x000B83B6
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context GetContext(StateMachineInstanceType smi)
		{
			return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this);
		}

		// Token: 0x04001B24 RID: 6948
		public ParameterType defaultValue;

		// Token: 0x04001B25 RID: 6949
		public bool isSignal;

		// Token: 0x020008D9 RID: 2265
		// (Invoke) Token: 0x06002838 RID: 10296
		public delegate bool Callback(StateMachineInstanceType smi, ParameterType p);

		// Token: 0x020008DA RID: 2266
		public class Transition : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
		{
			// Token: 0x0600283B RID: 10299 RVA: 0x000BA1C9 File Offset: 0x000B83C9
			public Transition(int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback) : base(idx, parameter.name, null, state)
			{
				this.parameter = parameter;
				this.callback = callback;
			}

			// Token: 0x0600283C RID: 10300 RVA: 0x001D2E94 File Offset: 0x001D1094
			public override void Evaluate(StateMachine.Instance smi)
			{
				StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
				global::Debug.Assert(stateMachineInstanceType != null);
				if (this.parameter.isSignal && this.callback == null)
				{
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)stateMachineInstanceType.GetParameterContext(this.parameter);
				if (this.callback(stateMachineInstanceType, context.value))
				{
					stateMachineInstanceType.GoTo(this.targetState);
				}
			}

			// Token: 0x0600283D RID: 10301 RVA: 0x000B8B66 File Offset: 0x000B6D66
			private void Trigger(StateMachineInstanceType smi)
			{
				smi.GoTo(this.targetState);
			}

			// Token: 0x0600283E RID: 10302 RVA: 0x001D2F10 File Offset: 0x001D1110
			public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
				}
				else
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
					context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
				}
				return new StateMachine.BaseTransition.Context(this);
			}

			// Token: 0x0600283F RID: 10303 RVA: 0x001D2F94 File Offset: 0x001D1194
			public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context transitionContext)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
				context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
			}

			// Token: 0x06002840 RID: 10304 RVA: 0x000BA1E9 File Offset: 0x000B83E9
			public override string ToString()
			{
				if (this.targetState != null)
				{
					return this.parameter.name + "->" + this.targetState.name;
				}
				return this.parameter.name + "->(Stop)";
			}

			// Token: 0x04001B26 RID: 6950
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;

			// Token: 0x04001B27 RID: 6951
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;
		}

		// Token: 0x020008DB RID: 2267
		public new abstract class Context : StateMachine.Parameter.Context
		{
			// Token: 0x06002841 RID: 10305 RVA: 0x000BA229 File Offset: 0x000B8429
			public Context(StateMachine.Parameter parameter, ParameterType default_value) : base(parameter)
			{
				this.value = default_value;
			}

			// Token: 0x06002842 RID: 10306 RVA: 0x000BA239 File Offset: 0x000B8439
			public virtual void Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!EqualityComparer<ParameterType>.Default.Equals(value, this.value))
				{
					this.value = value;
					if (!silenceEvents && this.onDirty != null)
					{
						this.onDirty(smi);
					}
				}
			}

			// Token: 0x04001B28 RID: 6952
			public ParameterType value;

			// Token: 0x04001B29 RID: 6953
			public Action<StateMachineInstanceType> onDirty;
		}
	}

	// Token: 0x020008DC RID: 2268
	public class BoolParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
	{
		// Token: 0x06002843 RID: 10307 RVA: 0x000BA26C File Offset: 0x000B846C
		public BoolParameter()
		{
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000BA274 File Offset: 0x000B8474
		public BoolParameter(bool default_value) : base(default_value)
		{
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000BA27D File Offset: 0x000B847D
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008DD RID: 2269
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context
		{
			// Token: 0x06002846 RID: 10310 RVA: 0x000BA28B File Offset: 0x000B848B
			public Context(StateMachine.Parameter parameter, bool default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002847 RID: 10311 RVA: 0x000BA295 File Offset: 0x000B8495
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value ? 1 : 0);
			}

			// Token: 0x06002848 RID: 10312 RVA: 0x000BA2AA File Offset: 0x000B84AA
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (reader.ReadByte() > 0);
			}

			// Token: 0x06002849 RID: 10313 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600284A RID: 10314 RVA: 0x001D3010 File Offset: 0x001D1210
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				bool value = this.value;
				if (ImGui.Checkbox(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020008DE RID: 2270
	public class Vector3Parameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
	{
		// Token: 0x0600284B RID: 10315 RVA: 0x000BA2BB File Offset: 0x000B84BB
		public Vector3Parameter()
		{
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000BA2C3 File Offset: 0x000B84C3
		public Vector3Parameter(Vector3 default_value) : base(default_value)
		{
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000BA2CC File Offset: 0x000B84CC
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008DF RID: 2271
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context
		{
			// Token: 0x0600284E RID: 10318 RVA: 0x000BA2DA File Offset: 0x000B84DA
			public Context(StateMachine.Parameter parameter, Vector3 default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600284F RID: 10319 RVA: 0x000BA2E4 File Offset: 0x000B84E4
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.x);
				writer.Write(this.value.y);
				writer.Write(this.value.z);
			}

			// Token: 0x06002850 RID: 10320 RVA: 0x000BA319 File Offset: 0x000B8519
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value.x = reader.ReadSingle();
				this.value.y = reader.ReadSingle();
				this.value.z = reader.ReadSingle();
			}

			// Token: 0x06002851 RID: 10321 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002852 RID: 10322 RVA: 0x001D3048 File Offset: 0x001D1248
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				Vector3 value = this.value;
				if (ImGui.InputFloat3(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020008E0 RID: 2272
	public class EnumParameter<EnumType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>
	{
		// Token: 0x06002853 RID: 10323 RVA: 0x000BA34E File Offset: 0x000B854E
		public EnumParameter(EnumType default_value) : base(default_value)
		{
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000BA357 File Offset: 0x000B8557
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context(this, this.defaultValue);
		}

		// Token: 0x020008E1 RID: 2273
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context
		{
			// Token: 0x06002855 RID: 10325 RVA: 0x000BA365 File Offset: 0x000B8565
			public Context(StateMachine.Parameter parameter, EnumType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002856 RID: 10326 RVA: 0x000BA36F File Offset: 0x000B856F
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write((int)((object)this.value));
			}

			// Token: 0x06002857 RID: 10327 RVA: 0x000BA387 File Offset: 0x000B8587
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (EnumType)((object)reader.ReadInt32());
			}

			// Token: 0x06002858 RID: 10328 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002859 RID: 10329 RVA: 0x001D3080 File Offset: 0x001D1280
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string[] names = Enum.GetNames(typeof(EnumType));
				Array values = Enum.GetValues(typeof(EnumType));
				int index = Array.IndexOf(values, this.value);
				if (ImGui.Combo(this.parameter.name, ref index, names, names.Length))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set((EnumType)((object)values.GetValue(index)), smi, false);
				}
			}
		}
	}

	// Token: 0x020008E2 RID: 2274
	public class FloatParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x000BA39F File Offset: 0x000B859F
		public FloatParameter()
		{
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x000BA3A7 File Offset: 0x000B85A7
		public FloatParameter(float default_value) : base(default_value)
		{
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x001D30F4 File Offset: 0x001D12F4
		public float Delta(float delta_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x001D3118 File Offset: 0x001D1318
		public float DeltaClamp(float delta_value, float min_value, float max_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			num = Mathf.Clamp(num, min_value, max_value);
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000BA3B0 File Offset: 0x000B85B0
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008E3 RID: 2275
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context
		{
			// Token: 0x0600285F RID: 10335 RVA: 0x000BA3BE File Offset: 0x000B85BE
			public Context(StateMachine.Parameter parameter, float default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002860 RID: 10336 RVA: 0x000BA3C8 File Offset: 0x000B85C8
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x06002861 RID: 10337 RVA: 0x000BA3D6 File Offset: 0x000B85D6
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadSingle();
			}

			// Token: 0x06002862 RID: 10338 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002863 RID: 10339 RVA: 0x001D3148 File Offset: 0x001D1348
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				float value = this.value;
				if (ImGui.InputFloat(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020008E4 RID: 2276
	public class IntParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
	{
		// Token: 0x06002864 RID: 10340 RVA: 0x000BA3E4 File Offset: 0x000B85E4
		public IntParameter()
		{
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000BA3EC File Offset: 0x000B85EC
		public IntParameter(int default_value) : base(default_value)
		{
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x001D3180 File Offset: 0x001D1380
		public int Delta(int delta_value, StateMachineInstanceType smi)
		{
			int num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000BA3F5 File Offset: 0x000B85F5
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008E5 RID: 2277
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context
		{
			// Token: 0x06002868 RID: 10344 RVA: 0x000BA403 File Offset: 0x000B8603
			public Context(StateMachine.Parameter parameter, int default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002869 RID: 10345 RVA: 0x000BA40D File Offset: 0x000B860D
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600286A RID: 10346 RVA: 0x000BA41B File Offset: 0x000B861B
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt32();
			}

			// Token: 0x0600286B RID: 10347 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600286C RID: 10348 RVA: 0x001D31A4 File Offset: 0x001D13A4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				int value = this.value;
				if (ImGui.InputInt(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020008E6 RID: 2278
	public class LongParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>
	{
		// Token: 0x0600286D RID: 10349 RVA: 0x000BA429 File Offset: 0x000B8629
		public LongParameter()
		{
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000BA431 File Offset: 0x000B8631
		public LongParameter(long default_value) : base(default_value)
		{
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x001D31DC File Offset: 0x001D13DC
		public long Delta(long delta_value, StateMachineInstanceType smi)
		{
			long num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000BA43A File Offset: 0x000B863A
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.LongParameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008E7 RID: 2279
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>.Context
		{
			// Token: 0x06002871 RID: 10353 RVA: 0x000BA448 File Offset: 0x000B8648
			public Context(StateMachine.Parameter parameter, long default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002872 RID: 10354 RVA: 0x000BA452 File Offset: 0x000B8652
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x06002873 RID: 10355 RVA: 0x000BA460 File Offset: 0x000B8660
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt64();
			}

			// Token: 0x06002874 RID: 10356 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002875 RID: 10357 RVA: 0x000BA46E File Offset: 0x000B866E
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				long value = this.value;
			}
		}
	}

	// Token: 0x020008E8 RID: 2280
	public class ResourceParameter<ResourceType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType> where ResourceType : Resource
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x001D3200 File Offset: 0x001D1400
		public ResourceParameter() : base(default(ResourceType))
		{
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000BA477 File Offset: 0x000B8677
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context(this, this.defaultValue);
		}

		// Token: 0x020008E9 RID: 2281
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context
		{
			// Token: 0x06002878 RID: 10360 RVA: 0x000BA365 File Offset: 0x000B8565
			public Context(StateMachine.Parameter parameter, ResourceType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002879 RID: 10361 RVA: 0x001D321C File Offset: 0x001D141C
			public override void Serialize(BinaryWriter writer)
			{
				string str = "";
				if (this.value != null)
				{
					if (this.value.Guid == null)
					{
						global::Debug.LogError("Cannot serialize resource with invalid guid: " + this.value.Id);
					}
					else
					{
						str = this.value.Guid.Guid;
					}
				}
				writer.WriteKleiString(str);
			}

			// Token: 0x0600287A RID: 10362 RVA: 0x001D3294 File Offset: 0x001D1494
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				string text = reader.ReadKleiString();
				if (text != "")
				{
					ResourceGuid guid = new ResourceGuid(text, null);
					this.value = Db.Get().GetResource<ResourceType>(guid);
				}
			}

			// Token: 0x0600287B RID: 10363 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600287C RID: 10364 RVA: 0x001D32D0 File Offset: 0x001D14D0
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020008EA RID: 2282
	public class TagParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>
	{
		// Token: 0x0600287D RID: 10365 RVA: 0x000BA485 File Offset: 0x000B8685
		public TagParameter()
		{
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000BA48D File Offset: 0x000B868D
		public TagParameter(Tag default_value) : base(default_value)
		{
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000BA496 File Offset: 0x000B8696
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagParameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008EB RID: 2283
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>.Context
		{
			// Token: 0x06002880 RID: 10368 RVA: 0x000BA4A4 File Offset: 0x000B86A4
			public Context(StateMachine.Parameter parameter, Tag default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002881 RID: 10369 RVA: 0x000BA4AE File Offset: 0x000B86AE
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.GetHash());
			}

			// Token: 0x06002882 RID: 10370 RVA: 0x000BA4C1 File Offset: 0x000B86C1
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = new Tag(reader.ReadInt32());
			}

			// Token: 0x06002883 RID: 10371 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002884 RID: 10372 RVA: 0x000BA4D4 File Offset: 0x000B86D4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				ImGui.LabelText(this.parameter.name, this.value.ToString());
			}
		}
	}

	// Token: 0x020008EC RID: 2284
	public class ObjectParameter<ObjectType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType> where ObjectType : class
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x001D3200 File Offset: 0x001D1400
		public ObjectParameter() : base(default(ObjectType))
		{
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000BA4F7 File Offset: 0x000B86F7
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context(this, this.defaultValue);
		}

		// Token: 0x020008ED RID: 2285
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context
		{
			// Token: 0x06002887 RID: 10375 RVA: 0x000BA365 File Offset: 0x000B8565
			public Context(StateMachine.Parameter parameter, ObjectType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002888 RID: 10376 RVA: 0x000BA505 File Offset: 0x000B8705
			public override void Serialize(BinaryWriter writer)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x06002889 RID: 10377 RVA: 0x000BA505 File Offset: 0x000B8705
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600288A RID: 10378 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600288B RID: 10379 RVA: 0x001D32D0 File Offset: 0x001D14D0
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020008EE RID: 2286
	public class TargetParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
	{
		// Token: 0x0600288C RID: 10380 RVA: 0x000BA511 File Offset: 0x000B8711
		public TargetParameter() : base(null)
		{
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x001D3314 File Offset: 0x001D1514
		public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				SMT smi2 = gameObject.GetSMI<SMT>();
				if (smi2 != null)
				{
					return smi2;
				}
				global::Debug.LogError(gameObject.name + " does not have state machine " + typeof(StateMachineType).Name);
			}
			return default(SMT);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000BA51A File Offset: 0x000B871A
		public bool IsNull(StateMachineInstanceType smi)
		{
			return base.Get(smi) == null;
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x001D3370 File Offset: 0x001D1570
		public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType component = gameObject.GetComponent<ComponentType>();
				if (component != null)
				{
					return component;
				}
				global::Debug.LogError(gameObject.name + " does not have component " + typeof(ComponentType).Name);
			}
			return default(ComponentType);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x001D33CC File Offset: 0x001D15CC
		public ComponentType AddOrGet<ComponentType>(StateMachineInstanceType smi) where ComponentType : Component
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType componentType = gameObject.GetComponent<ComponentType>();
				if (componentType == null)
				{
					componentType = gameObject.AddComponent<ComponentType>();
				}
				return componentType;
			}
			return default(ComponentType);
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x001D3414 File Offset: 0x001D1614
		public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
		{
			GameObject value2 = null;
			if (value != null)
			{
				value2 = value.gameObject;
			}
			base.Set(value2, smi, false);
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000BA529 File Offset: 0x000B8729
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context(this, this.defaultValue);
		}

		// Token: 0x020008EF RID: 2287
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context
		{
			// Token: 0x06002893 RID: 10387 RVA: 0x000BA537 File Offset: 0x000B8737
			public Context(StateMachine.Parameter parameter, GameObject default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002894 RID: 10388 RVA: 0x001D3440 File Offset: 0x001D1640
			public override void Serialize(BinaryWriter writer)
			{
				if (this.value != null)
				{
					int instanceID = this.value.GetComponent<KPrefabID>().InstanceID;
					writer.Write(instanceID);
					return;
				}
				writer.Write(0);
			}

			// Token: 0x06002895 RID: 10389 RVA: 0x001D347C File Offset: 0x001D167C
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				try
				{
					int num = reader.ReadInt32();
					if (num != 0)
					{
						KPrefabID instance = KPrefabIDTracker.Get().GetInstance(num);
						if (instance != null)
						{
							this.value = instance.gameObject;
							this.objectDestroyedHandler = instance.Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
						}
						this.m_smi = (StateMachineInstanceType)((object)smi);
					}
				}
				catch (Exception ex)
				{
					if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
					{
						global::Debug.LogWarning("Missing statemachine target params. " + ex.Message);
					}
				}
			}

			// Token: 0x06002896 RID: 10390 RVA: 0x000BA541 File Offset: 0x000B8741
			public override void Cleanup()
			{
				base.Cleanup();
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
			}

			// Token: 0x06002897 RID: 10391 RVA: 0x001D3520 File Offset: 0x001D1720
			public override void Set(GameObject value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				this.m_smi = smi;
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
				if (value != null)
				{
					this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
				}
				base.Set(value, smi, silenceEvents);
			}

			// Token: 0x06002898 RID: 10392 RVA: 0x000BA574 File Offset: 0x000B8774
			private void OnObjectDestroyed(object data)
			{
				this.Set(null, this.m_smi, false);
			}

			// Token: 0x06002899 RID: 10393 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600289A RID: 10394 RVA: 0x001D3594 File Offset: 0x001D1794
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (this.value != null)
				{
					ImGui.LabelText(this.parameter.name, this.value.name);
					return;
				}
				ImGui.LabelText(this.parameter.name, "null");
			}

			// Token: 0x04001B2A RID: 6954
			private StateMachineInstanceType m_smi;

			// Token: 0x04001B2B RID: 6955
			private int objectDestroyedHandler;
		}
	}

	// Token: 0x020008F0 RID: 2288
	public class SignalParameter
	{
	}

	// Token: 0x020008F1 RID: 2289
	public class Signal : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
	{
		// Token: 0x0600289C RID: 10396 RVA: 0x000BA584 File Offset: 0x000B8784
		public Signal() : base(null)
		{
			this.isSignal = true;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000BA594 File Offset: 0x000B8794
		public void Trigger(StateMachineInstanceType smi)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context)smi.GetParameterContext(this)).Set(null, smi, false);
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000BA5AF File Offset: 0x000B87AF
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context(this, this.defaultValue);
		}

		// Token: 0x020008F2 RID: 2290
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context
		{
			// Token: 0x0600289F RID: 10399 RVA: 0x000BA5BD File Offset: 0x000B87BD
			public Context(StateMachine.Parameter parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028A0 RID: 10400 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void Serialize(BinaryWriter writer)
			{
			}

			// Token: 0x060028A1 RID: 10401 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
			}

			// Token: 0x060028A2 RID: 10402 RVA: 0x000BA5C7 File Offset: 0x000B87C7
			public override void Set(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!silenceEvents && this.onDirty != null)
				{
					this.onDirty(smi);
				}
			}

			// Token: 0x060028A3 RID: 10403 RVA: 0x000A5E40 File Offset: 0x000A4040
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028A4 RID: 10404 RVA: 0x001D35E0 File Offset: 0x001D17E0
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (ImGui.Button(this.parameter.name))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(null, smi, false);
				}
			}
		}
	}
}
