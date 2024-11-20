using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

public abstract class
    GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>
    : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>
    where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>
    where StateMachineInstanceType :
    GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameInstance
    where MasterType : IStateMachineTarget {
    protected static Parameter<bool>.Callback IsFalse = (smi, p) => !p;
    protected static Parameter<bool>.Callback IsTrue = (smi, p) => p;
    protected static Parameter<float>.Callback IsZero = (smi, p) => p == 0f;
    protected static Parameter<float>.Callback IsLTZero = (smi, p) => p < 0f;
    protected static Parameter<float>.Callback IsLTEZero = (smi, p) => p <= 0f;
    protected static Parameter<float>.Callback IsGTZero = (smi, p) => p > 0f;
    protected static Parameter<float>.Callback IsGTEZero = (smi, p) => p >= 0f;
    protected static Parameter<float>.Callback IsOne = (smi, p) => p == 1f;
    protected static Parameter<float>.Callback IsLTOne = (smi, p) => p < 1f;
    protected static Parameter<float>.Callback IsLTEOne = (smi, p) => p <= 1f;
    protected static Parameter<float>.Callback IsGTOne = (smi, p) => p > 1f;
    protected static Parameter<float>.Callback IsGTEOne = (smi, p) => p >= 1f;
    protected static Parameter<GameObject>.Callback IsNotNull = (smi, p) => p != null;
    protected static Parameter<GameObject>.Callback IsNull = (smi, p) => p == null;
    public           State root = new State();
    public override  void InitializeStates(out BaseState default_state) { base.InitializeStates(out default_state); }

    public static Transition.ConditionCallback Not(Transition.ConditionCallback transition_cb) {
        return smi => !transition_cb(smi);
    }

    public override void BindStates() {
        BindState(null, root, "root");
        base.BindStates(root, this);
    }

    public class PreLoopPostState : State {
        public State loop;
        public State pre;
        public State pst;
    }

    public class WorkingState : State {
        public State waiting;
        public State working_loop;
        public State working_pre;
        public State working_pst;
    }

    public class GameInstance : GenericInstance {
        public GameInstance(MasterType master, DefType def) : base(master) { this.def = def; }
        public GameInstance(MasterType master) : base(master) { }

        public void Queue(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once) {
            smi.GetComponent<KBatchedAnimController>().Queue(anim, mode);
        }

        public void Play(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once) {
            smi.GetComponent<KBatchedAnimController>().Play(anim, mode);
        }
    }

    public class TagTransitionData : Transition {
        private          bool                                  is_executing;
        private readonly bool                                  onRemove;
        private readonly Tag[]                                 tags;
        private readonly Func<StateMachineInstanceType, Tag[]> tags_callback;
        private readonly TargetParameter                       target;

        public TagTransitionData(string                                name,
                                 State                                 source_state,
                                 State                                 target_state,
                                 int                                   idx,
                                 Tag[]                                 tags,
                                 bool                                  on_remove,
                                 TargetParameter                       target,
                                 Func<StateMachineInstanceType, Tag[]> tags_callback = null) : base(name,
         source_state,
         target_state,
         idx,
         null) {
            this.tags          = tags;
            onRemove           = on_remove;
            this.target        = target;
            this.tags_callback = tags_callback;
        }

        public override void Evaluate(Instance smi) {
            var stateMachineInstanceType = smi as StateMachineInstanceType;
            Debug.Assert(stateMachineInstanceType != null);
            if (!onRemove) {
                if (!HasAllTags(stateMachineInstanceType)) return;
            } else if (HasAnyTags(stateMachineInstanceType)) return;

            ExecuteTransition(stateMachineInstanceType);
        }

        private bool HasAllTags(StateMachineInstanceType smi) {
            return target.Get(smi)
                         .GetComponent<KPrefabID>()
                         .HasAllTags(tags_callback != null ? tags_callback(smi) : tags);
        }

        private bool HasAnyTags(StateMachineInstanceType smi) {
            return target.Get(smi)
                         .GetComponent<KPrefabID>()
                         .HasAnyTags(tags_callback != null ? tags_callback(smi) : tags);
        }

        private void ExecuteTransition(StateMachineInstanceType smi) {
            if (is_executing) return;

            is_executing = true;
            smi.GoTo(targetState);
            is_executing = false;
        }

        private void OnCallback(StateMachineInstanceType smi) {
            if (target.Get(smi) == null) return;

            if (!onRemove) {
                if (!HasAllTags(smi)) return;
            } else if (HasAnyTags(smi)) return;

            ExecuteTransition(smi);
        }

        public override Context Register(Instance smi) {
            var smi_internal = smi as StateMachineInstanceType;
            Debug.Assert(smi_internal != null);
            var result = base.Register(smi_internal);
            result.handlerId = target.Get(smi_internal).Subscribe(-1582839653, delegate { OnCallback(smi_internal); });
            return result;
        }

        public override void Unregister(Instance smi, Context context) {
            var stateMachineInstanceType = smi as StateMachineInstanceType;
            Debug.Assert(stateMachineInstanceType != null);
            base.Unregister(stateMachineInstanceType, context);
            if (target.Get(stateMachineInstanceType) != null)
                target.Get(stateMachineInstanceType).Unsubscribe(context.handlerId);
        }
    }

    public class EventTransitionData : Transition {
        private readonly GameHashes                                     evtId;
        private readonly Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;
        private readonly TargetParameter                                target;

        public EventTransitionData(State                                          source_state,
                                   State                                          target_state,
                                   int                                            idx,
                                   GameHashes                                     evt,
                                   Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback,
                                   ConditionCallback                              condition,
                                   TargetParameter                                target) : base(evt.ToString(),
         source_state,
         target_state,
         idx,
         condition) {
            evtId                     = evt;
            this.target               = target;
            globalEventSystemCallback = global_event_system_callback;
        }

        public override void Evaluate(Instance smi) {
            var stateMachineInstanceType = smi as StateMachineInstanceType;
            Debug.Assert(stateMachineInstanceType != null);
            if (condition != null && condition(stateMachineInstanceType)) ExecuteTransition(stateMachineInstanceType);
        }

        private void ExecuteTransition(StateMachineInstanceType smi) { smi.GoTo(targetState); }

        private void OnCallback(StateMachineInstanceType smi) {
            if (condition == null || condition(smi)) ExecuteTransition(smi);
        }

        public override Context Register(Instance smi) {
            var smi_internal = smi as StateMachineInstanceType;
            Debug.Assert(smi_internal != null);
            var            result  = base.Register(smi_internal);
            Action<object> handler = delegate { OnCallback(smi_internal); };
            GameObject     gameObject;
            if (globalEventSystemCallback != null)
                gameObject = globalEventSystemCallback(smi_internal).gameObject;
            else {
                gameObject = target.Get(smi_internal);
                if (gameObject == null)
                    throw new InvalidOperationException("TargetParameter: " + target.name + " is null");
            }

            result.handlerId = gameObject.Subscribe((int)evtId, handler);
            return result;
        }

        public override void Unregister(Instance smi, Context context) {
            var stateMachineInstanceType = smi as StateMachineInstanceType;
            Debug.Assert(stateMachineInstanceType != null);
            base.Unregister(stateMachineInstanceType, context);
            GameObject gameObject = null;
            if (globalEventSystemCallback != null) {
                var kmonoBehaviour                     = globalEventSystemCallback(stateMachineInstanceType);
                if (kmonoBehaviour != null) gameObject = kmonoBehaviour.gameObject;
            } else
                gameObject = target.Get(stateMachineInstanceType);

            if (gameObject != null) gameObject.Unsubscribe(context.handlerId);
        }
    }

    public new class State : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State {
        [DoNotAutoCreate]
        private TargetParameter stateTarget;

        public State root => this;

        public State master {
            get {
                stateTarget = sm.masterTarget;
                return this;
            }
        }

        private TargetParameter GetStateTarget() {
            if (stateTarget != null) return stateTarget;

            if (parent != null) return ((State)parent).GetStateTarget();

            var targetParameter = sm.stateTarget;
            if (targetParameter == null) return sm.masterTarget;

            return targetParameter;
        }

        public int CreateDataTableEntry() {
            var stateMachineType = sm;
            var dataTableSize    = stateMachineType.dataTableSize;
            stateMachineType.dataTableSize = dataTableSize + 1;
            return dataTableSize;
        }

        public int CreateUpdateTableEntry() {
            var stateMachineType = sm;
            var updateTableSize  = stateMachineType.updateTableSize;
            stateMachineType.updateTableSize = updateTableSize + 1;
            return updateTableSize;
        }

        public State DoNothing() { return this; }

        private static List<Action> AddAction(string name, Callback callback, List<Action> actions, bool add_to_end) {
            if (actions == null) actions = new List<Action>();
            var item                     = new Action(name, callback);
            if (add_to_end)
                actions.Add(item);
            else
                actions.Insert(0, item);

            return actions;
        }

        public State Target(TargetParameter target) {
            stateTarget = target;
            return this;
        }

        public State Update(Action<StateMachineInstanceType, float> callback,
                            UpdateRate                              update_rate  = UpdateRate.SIM_200ms,
                            bool                                    load_balance = false) {
            return Update(sm.name + "." + name, callback, update_rate, load_balance);
        }

        public State BatchUpdate(UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update,
                                 UpdateRate update_rate = UpdateRate.SIM_200ms) {
            return BatchUpdate(sm.name + "." + name, batch_update, update_rate);
        }

        public State Enter(Callback callback) { return Enter("Enter", callback); }
        public State Exit(Callback  callback) { return Exit("Exit", callback); }

        private State InternalUpdate(string                                                     name,
                                     UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater bucket_updater,
                                     UpdateRate                                                 update_rate,
                                     bool                                                       load_balance,
                                     UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update
                                         = null) {
            var updateTableIdx                       = CreateUpdateTableEntry();
            if (updateActions == null) updateActions = new List<UpdateAction>();
            var updateAction                         = default(UpdateAction);
            updateAction.updateTableIdx = updateTableIdx;
            updateAction.updateRate     = update_rate;
            updateAction.updater        = bucket_updater;
            var num               = 1;
            if (load_balance) num = Singleton<StateMachineUpdater>.Instance.GetFrameCount(update_rate);
            updateAction.buckets = new StateMachineUpdater.BaseUpdateBucket[num];
            for (var i = 0; i < num; i++) {
                var updateBucketWithUpdater = new UpdateBucketWithUpdater<StateMachineInstanceType>(name);
                updateBucketWithUpdater.batch_update_delegate = batch_update;
                Singleton<StateMachineUpdater>.Instance.AddBucket(update_rate, updateBucketWithUpdater);
                updateAction.buckets[i] = updateBucketWithUpdater;
            }

            updateActions.Add(updateAction);
            return this;
        }

        public State UpdateTransition(State                                       destination_state,
                                      Func<StateMachineInstanceType, float, bool> callback,
                                      UpdateRate                                  update_rate  = UpdateRate.SIM_200ms,
                                      bool                                        load_balance = false) {
            Action<StateMachineInstanceType, float> checkCallback = delegate(StateMachineInstanceType smi, float dt) {
                                                                        if (callback(smi, dt))
                                                                            smi.GoTo(destination_state);
                                                                    };

            Enter(delegate(StateMachineInstanceType smi) { checkCallback(smi, 0f); });
            Update(checkCallback, update_rate, load_balance);
            return this;
        }

        public State Update(string                                  name,
                            Action<StateMachineInstanceType, float> callback,
                            UpdateRate                              update_rate  = UpdateRate.SIM_200ms,
                            bool                                    load_balance = false) {
            return InternalUpdate(name,
                                  new BucketUpdater<StateMachineInstanceType>(callback),
                                  update_rate,
                                  load_balance);
        }

        public State BatchUpdate(string name,
                                 UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update,
                                 UpdateRate update_rate = UpdateRate.SIM_200ms) {
            return InternalUpdate(name, null, update_rate, false, batch_update);
        }

        public State FastUpdate(string name,
                                UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater updater,
                                UpdateRate update_rate = UpdateRate.SIM_200ms,
                                bool load_balance = false) {
            return InternalUpdate(name, updater, update_rate, load_balance);
        }

        public State Enter(string name, Callback callback) {
            enterActions = AddAction(name, callback, enterActions, true);
            return this;
        }

        public State Exit(string name, Callback callback) {
            exitActions = AddAction(name, callback, exitActions, false);
            return this;
        }

        public State Toggle(string name, Callback enter_callback, Callback exit_callback) {
            var data_idx = CreateDataTableEntry();
            Enter("ToggleEnter(" + name + ")",
                  delegate(StateMachineInstanceType smi) {
                      smi.dataTable[data_idx] = GameStateMachineHelper.HasToggleEnteredFlag;
                      enter_callback(smi);
                  });

            Exit("ToggleExit(" + name + ")",
                 delegate(StateMachineInstanceType smi) {
                     if (smi.dataTable[data_idx] != null) {
                         smi.dataTable[data_idx] = null;
                         exit_callback(smi);
                     }
                 });

            return this;
        }

        private void  Break(StateMachineInstanceType smi) { }
        public  State BreakOnEnter() { return Enter(delegate(StateMachineInstanceType smi) { Break(smi); }); }
        public  State BreakOnExit() { return Exit(delegate(StateMachineInstanceType smi) { Break(smi); }); }

        public State AddEffect(string effect_name) {
            var state_target = GetStateTarget();
            Enter("AddEffect(" + effect_name + ")",
                  delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Add(effect_name, true); });

            return this;
        }

        public State ToggleAnims(Func<StateMachineInstanceType, KAnimFile> chooser_callback) {
            var state_target = GetStateTarget();
            Enter("EnableAnims()",
                  delegate(StateMachineInstanceType smi) {
                      var kanimFile = chooser_callback(smi);
                      if (kanimFile == null) return;

                      state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(kanimFile);
                  });

            Exit("Disableanims()",
                 delegate(StateMachineInstanceType smi) {
                     var kanimFile = chooser_callback(smi);
                     if (kanimFile == null) return;

                     state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(kanimFile);
                 });

            return this;
        }

        public State ToggleAnims(Func<StateMachineInstanceType, HashedString> chooser_callback) {
            var state_target = GetStateTarget();
            Enter("EnableAnims()",
                  delegate(StateMachineInstanceType smi) {
                      var hashedString = chooser_callback(smi);
                      if (hashedString == null) return;

                      if (hashedString.IsValid) {
                          var anim = Assets.GetAnim(hashedString);
                          if (anim == null) {
                              var str           = "Missing anims: ";
                              var hashedString2 = hashedString;
                              Debug.LogWarning(str + hashedString2);
                              return;
                          }

                          state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim);
                      }
                  });

            Exit("Disableanims()",
                 delegate(StateMachineInstanceType smi) {
                     var hashedString = chooser_callback(smi);
                     if (hashedString == null) return;

                     if (hashedString.IsValid) {
                         var anim = Assets.GetAnim(hashedString);
                         if (anim != null) state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
                     }
                 });

            return this;
        }

        public State ToggleAnims(string anim_file, float priority = 0f) {
            var state_target = GetStateTarget();
            Toggle("ToggleAnims(" + anim_file + ")",
                   delegate(StateMachineInstanceType smi) {
                       var anim = Assets.GetAnim(anim_file);
                       if (anim == null) Debug.LogError("Trying to add missing override anims:" + anim_file);
                       state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, priority);
                   },
                   delegate(StateMachineInstanceType smi) {
                       var anim = Assets.GetAnim(anim_file);
                       state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
                   });

            return this;
        }

        public State ToggleAttributeModifier(string                                            modifier_name,
                                             Func<StateMachineInstanceType, AttributeModifier> callback,
                                             Func<StateMachineInstanceType, bool>              condition = null) {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("AddAttributeModifier( " + modifier_name + " )",
                  delegate(StateMachineInstanceType smi) {
                      if (condition == null || condition(smi)) {
                          var attributeModifier = callback(smi);
                          DebugUtil.Assert(smi.dataTable[data_idx] == null);
                          smi.dataTable[data_idx] = attributeModifier;
                          state_target.Get(smi).GetAttributes().Add(attributeModifier);
                      }
                  });

            Exit("RemoveAttributeModifier( " + modifier_name + " )",
                 delegate(StateMachineInstanceType smi) {
                     if (smi.dataTable[data_idx] != null) {
                         var modifier = (AttributeModifier)smi.dataTable[data_idx];
                         smi.dataTable[data_idx] = null;
                         var gameObject = state_target.Get(smi);
                         if (gameObject != null) gameObject.GetAttributes().Remove(modifier);
                     }
                 });

            return this;
        }

        public State ToggleLoopingSound(string                               event_name,
                                        Func<StateMachineInstanceType, bool> condition                     = null,
                                        bool                                 pause_on_game_pause           = true,
                                        bool                                 enable_culling                = true,
                                        bool                                 enable_camera_scaled_position = true) {
            var state_target = GetStateTarget();
            Enter("StartLoopingSound( " + event_name + " )",
                  delegate(StateMachineInstanceType smi) {
                      if (condition == null || condition(smi))
                          state_target.Get(smi)
                                      .GetComponent<LoopingSounds>()
                                      .StartSound(event_name,
                                                  pause_on_game_pause,
                                                  enable_culling,
                                                  enable_camera_scaled_position);
                  });

            Exit("StopLoopingSound( " + event_name + " )",
                 delegate(StateMachineInstanceType smi) {
                     state_target.Get(smi).GetComponent<LoopingSounds>().StopSound(event_name);
                 });

            return this;
        }

        public State ToggleLoopingSound(string                                 state_label,
                                        Func<StateMachineInstanceType, string> event_name_callback,
                                        Func<StateMachineInstanceType, bool>   condition = null) {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("StartLoopingSound( " + state_label + " )",
                  delegate(StateMachineInstanceType smi) {
                      if (condition == null || condition(smi)) {
                          var text = event_name_callback(smi);
                          smi.dataTable[data_idx] = text;
                          state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(text);
                      }
                  });

            Exit("StopLoopingSound( " + state_label + " )",
                 delegate(StateMachineInstanceType smi) {
                     if (smi.dataTable[data_idx] != null) {
                         state_target.Get(smi).GetComponent<LoopingSounds>().StopSound((string)smi.dataTable[data_idx]);
                         smi.dataTable[data_idx] = null;
                     }
                 });

            return this;
        }

        public State RefreshUserMenuOnEnter() {
            Enter("RefreshUserMenuOnEnter()",
                  delegate(StateMachineInstanceType smi) {
                      var userMenu = Game.Instance.userMenu;
                      var master   = smi.master;
                      userMenu.Refresh(master.gameObject);
                  });

            return this;
        }

        public State WorkableStartTransition(Func<StateMachineInstanceType, Workable> get_workable_callback,
                                             State                                    target_state) {
            var data_idx = CreateDataTableEntry();
            Enter("Enter WorkableStartTransition(" + target_state.longName + ")",
                  delegate(StateMachineInstanceType smi) {
                      var workable3 = get_workable_callback(smi);
                      if (workable3 != null) {
                          Action<Workable, Workable.WorkableEvent> action
                              = delegate(Workable workable, Workable.WorkableEvent evt) {
                                    if (evt == Workable.WorkableEvent.WorkStarted) smi.GoTo(target_state);
                                };

                          smi.dataTable[data_idx] = action;
                          var workable2 = workable3;
                          workable2.OnWorkableEventCB
                              = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB,
                               action);
                      }
                  });

            Exit("Exit WorkableStartTransition(" + target_state.longName + ")",
                 delegate(StateMachineInstanceType smi) {
                     var workable = get_workable_callback(smi);
                     if (workable != null) {
                         var value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
                         smi.dataTable[data_idx] = null;
                         var workable2 = workable;
                         workable2.OnWorkableEventCB
                             = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB,
                              value);
                     }
                 });

            return this;
        }

        public State WorkableStopTransition(Func<StateMachineInstanceType, Workable> get_workable_callback,
                                            State                                    target_state) {
            var data_idx = CreateDataTableEntry();
            Enter("Enter WorkableStopTransition(" + target_state.longName + ")",
                  delegate(StateMachineInstanceType smi) {
                      var workable3 = get_workable_callback(smi);
                      if (workable3 != null) {
                          Action<Workable, Workable.WorkableEvent> action
                              = delegate(Workable workable, Workable.WorkableEvent evt) {
                                    if (evt == Workable.WorkableEvent.WorkStopped) smi.GoTo(target_state);
                                };

                          smi.dataTable[data_idx] = action;
                          var workable2 = workable3;
                          workable2.OnWorkableEventCB
                              = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB,
                               action);
                      }
                  });

            Exit("Exit WorkableStopTransition(" + target_state.longName + ")",
                 delegate(StateMachineInstanceType smi) {
                     var workable = get_workable_callback(smi);
                     if (workable != null) {
                         var value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
                         smi.dataTable[data_idx] = null;
                         var workable2 = workable;
                         workable2.OnWorkableEventCB
                             = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB,
                              value);
                     }
                 });

            return this;
        }

        public State WorkableCompleteTransition(Func<StateMachineInstanceType, Workable> get_workable_callback,
                                                State                                    target_state) {
            var data_idx = CreateDataTableEntry();
            Enter("Enter WorkableCompleteTransition(" + target_state.longName + ")",
                  delegate(StateMachineInstanceType smi) {
                      var workable3 = get_workable_callback(smi);
                      if (workable3 != null) {
                          Action<Workable, Workable.WorkableEvent> action
                              = delegate(Workable workable, Workable.WorkableEvent evt) {
                                    if (evt == Workable.WorkableEvent.WorkCompleted) smi.GoTo(target_state);
                                };

                          smi.dataTable[data_idx] = action;
                          var workable2 = workable3;
                          workable2.OnWorkableEventCB
                              = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB,
                               action);
                      }
                  });

            Exit("Exit WorkableCompleteTransition(" + target_state.longName + ")",
                 delegate(StateMachineInstanceType smi) {
                     var workable = get_workable_callback(smi);
                     if (workable != null) {
                         var value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
                         smi.dataTable[data_idx] = null;
                         var workable2 = workable;
                         workable2.OnWorkableEventCB
                             = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB,
                              value);
                     }
                 });

            return this;
        }

        public State ToggleGravity() {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("AddComponent<Gravity>()",
                  delegate(StateMachineInstanceType smi) {
                      var gameObject = state_target.Get(smi);
                      smi.dataTable[data_idx] = gameObject;
                      GameComps.Gravities.Add(gameObject, Vector2.zero);
                  });

            Exit("RemoveComponent<Gravity>()",
                 delegate(StateMachineInstanceType smi) {
                     var go = (GameObject)smi.dataTable[data_idx];
                     smi.dataTable[data_idx] = null;
                     GameComps.Gravities.Remove(go);
                 });

            return this;
        }

        public State ToggleGravity(State landed_state) {
            var state_target = GetStateTarget();
            EventTransition(GameHashes.Landed, landed_state);
            Toggle("GravityComponent",
                   delegate(StateMachineInstanceType smi) {
                       GameComps.Gravities.Add(state_target.Get(smi), Vector2.zero);
                   },
                   delegate(StateMachineInstanceType smi) { GameComps.Gravities.Remove(state_target.Get(smi)); });

            return this;
        }

        public State ToggleThought(Func<StateMachineInstanceType, Thought> chooser_callback) {
            var state_target = GetStateTarget();
            Enter("EnableThought()",
                  delegate(StateMachineInstanceType smi) {
                      var thought = chooser_callback(smi);
                      state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
                  });

            Exit("DisableThought()",
                 delegate(StateMachineInstanceType smi) {
                     var thought = chooser_callback(smi);
                     state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
                 });

            return this;
        }

        public State ToggleThought(Thought thought, Func<StateMachineInstanceType, bool> condition_callback = null) {
            var state_target = GetStateTarget();
            Enter("AddThought(" + thought.Id + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (condition_callback == null || condition_callback(smi))
                          state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
                  });

            if (condition_callback != null)
                Update("ValidateThought(" + thought.Id + ")",
                       delegate(StateMachineInstanceType smi, float dt) {
                           if (condition_callback(smi)) {
                               state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
                               return;
                           }

                           state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
                       });

            Exit("RemoveThought(" + thought.Id + ")",
                 delegate(StateMachineInstanceType smi) {
                     state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
                 });

            return this;
        }

        public State ToggleCreatureThought(Func<StateMachineInstanceType, Thought> chooser_callback) {
            var state_target = GetStateTarget();
            Enter("EnableCreatureThought()",
                  delegate(StateMachineInstanceType smi) {
                      var thought = chooser_callback(smi);
                      state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
                  });

            Exit("DisableCreatureThought()",
                 delegate(StateMachineInstanceType smi) {
                     var thought = chooser_callback(smi);
                     var smi2    = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
                     if (smi2 != null) smi2.RemoveThought(thought);
                 });

            return this;
        }

        public State ToggleCreatureThought(Thought                              thought,
                                           Func<StateMachineInstanceType, bool> condition_callback = null) {
            var state_target = GetStateTarget();
            Enter("AddCreatureThought(" + thought.Id + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (condition_callback == null || condition_callback(smi))
                          state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
                  });

            if (condition_callback != null)
                Update("ValidateCreatureThought(" + thought.Id + ")",
                       delegate(StateMachineInstanceType smi, float dt) {
                           if (condition_callback(smi)) {
                               state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
                               return;
                           }

                           state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().RemoveThought(thought);
                       });

            Exit("RemoveCreatureThought(" + thought.Id + ")",
                 delegate(StateMachineInstanceType smi) {
                     var smi2 = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
                     if (smi2 != null) smi2.RemoveThought(thought);
                 });

            return this;
        }

        public State ToggleExpression(Func<StateMachineInstanceType, Expression> chooser_callback) {
            var state_target = GetStateTarget();
            Enter("AddExpression",
                  delegate(StateMachineInstanceType smi) {
                      state_target.Get<FaceGraph>(smi).AddExpression(chooser_callback(smi));
                  });

            Exit("RemoveExpression",
                 delegate(StateMachineInstanceType smi) {
                     state_target.Get<FaceGraph>(smi).RemoveExpression(chooser_callback(smi));
                 });

            return this;
        }

        public State ToggleExpression(Expression expression, Func<StateMachineInstanceType, bool> condition = null) {
            var state_target = GetStateTarget();
            Enter("AddExpression(" + expression.Id + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (condition == null || condition(smi))
                          state_target.Get<FaceGraph>(smi).AddExpression(expression);
                  });

            if (condition != null)
                Update("ValidateExpression(" + expression.Id + ")",
                       delegate(StateMachineInstanceType smi, float dt) {
                           if (condition(smi)) {
                               state_target.Get<FaceGraph>(smi).AddExpression(expression);
                               return;
                           }

                           state_target.Get<FaceGraph>(smi).RemoveExpression(expression);
                       });

            Exit("RemoveExpression(" + expression.Id + ")",
                 delegate(StateMachineInstanceType smi) {
                     var faceGraph = state_target.Get<FaceGraph>(smi);
                     if (faceGraph != null) faceGraph.RemoveExpression(expression);
                 });

            return this;
        }

        public State ToggleMainStatusItem(StatusItem                             status_item,
                                          Func<StateMachineInstanceType, object> callback = null) {
            var state_target = GetStateTarget();
            Enter("AddMainStatusItem(" + status_item.Id + ")",
                  delegate(StateMachineInstanceType smi) {
                      var data = callback != null ? callback(smi) : smi;
                      state_target.Get<KSelectable>(smi)
                                  .SetStatusItem(Db.Get().StatusItemCategories.Main, status_item, data);
                  });

            Exit("RemoveMainStatusItem(" + status_item.Id + ")",
                 delegate(StateMachineInstanceType smi) {
                     var kselectable = state_target.Get<KSelectable>(smi);
                     if (kselectable != null) kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null);
                 });

            return this;
        }

        public State ToggleMainStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb,
                                          Func<StateMachineInstanceType, object>     callback = null) {
            var state_target = GetStateTarget();
            Enter("AddMainStatusItem(DynamicGeneration)",
                  delegate(StateMachineInstanceType smi) {
                      var data = callback != null ? callback(smi) : smi;
                      state_target.Get<KSelectable>(smi)
                                  .SetStatusItem(Db.Get().StatusItemCategories.Main, status_item_cb(smi), data);
                  });

            Exit("RemoveMainStatusItem(DynamicGeneration)",
                 delegate(StateMachineInstanceType smi) {
                     var kselectable = state_target.Get<KSelectable>(smi);
                     if (kselectable != null) kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null);
                 });

            return this;
        }

        public State ToggleCategoryStatusItem(StatusItemCategory category, StatusItem status_item, object data = null) {
            var state_target = GetStateTarget();
            Enter(string.Concat("AddCategoryStatusItem(", category.Id, ", ", status_item.Id, ")"),
                  delegate(StateMachineInstanceType smi) {
                      state_target.Get<KSelectable>(smi)
                                  .SetStatusItem(category, status_item, data != null ? data : smi);
                  });

            Exit(string.Concat("RemoveCategoryStatusItem(", category.Id, ", ", status_item.Id, ")"),
                 delegate(StateMachineInstanceType smi) {
                     var kselectable = state_target.Get<KSelectable>(smi);
                     if (kselectable != null) kselectable.SetStatusItem(category, null);
                 });

            return this;
        }

        public State ToggleStatusItem(StatusItem status_item, object data = null) {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("AddStatusItem(" + status_item.Id + ")",
                  delegate(StateMachineInstanceType smi) {
                      var obj              = data;
                      if (obj == null) obj = smi;
                      var guid             = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, obj);
                      smi.dataTable[data_idx] = guid;
                  });

            Exit("RemoveStatusItem(" + status_item.Id + ")",
                 delegate(StateMachineInstanceType smi) {
                     var kselectable = state_target.Get<KSelectable>(smi);
                     if (kselectable != null && smi.dataTable[data_idx] != null) {
                         var guid = (Guid)smi.dataTable[data_idx];
                         kselectable.RemoveStatusItem(guid);
                     }

                     smi.dataTable[data_idx] = null;
                 });

            return this;
        }

        public State ToggleSnapOn(string snap_on) {
            var state_target = GetStateTarget();
            Enter("SnapOn(" + snap_on + ")",
                  delegate(StateMachineInstanceType smi) {
                      state_target.Get<SnapOn>(smi).AttachSnapOnByName(snap_on);
                  });

            Exit("SnapOff(" + snap_on + ")",
                 delegate(StateMachineInstanceType smi) {
                     var snapOn = state_target.Get<SnapOn>(smi);
                     if (snapOn != null) snapOn.DetachSnapOnByName(snap_on);
                 });

            return this;
        }

        public State ToggleTag(Tag tag) {
            var state_target = GetStateTarget();
            Enter("AddTag(" + tag.Name + ")",
                  delegate(StateMachineInstanceType smi) { state_target.Get<KPrefabID>(smi).AddTag(tag); });

            Exit("RemoveTag(" + tag.Name + ")",
                 delegate(StateMachineInstanceType smi) { state_target.Get<KPrefabID>(smi).RemoveTag(tag); });

            return this;
        }

        public State ToggleTag(Func<StateMachineInstanceType, Tag> behaviour_tag_cb) {
            var state_target = GetStateTarget();
            Enter("AddTag(DynamicallyConstructed)",
                  delegate(StateMachineInstanceType smi) {
                      state_target.Get<KPrefabID>(smi).AddTag(behaviour_tag_cb(smi));
                  });

            Exit("RemoveTag(DynamicallyConstructed)",
                 delegate(StateMachineInstanceType smi) {
                     state_target.Get<KPrefabID>(smi).RemoveTag(behaviour_tag_cb(smi));
                 });

            return this;
        }

        public State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback) {
            return ToggleStatusItem(status_item, callback, null);
        }

        public State ToggleStatusItem(StatusItem                             status_item,
                                      Func<StateMachineInstanceType, object> callback,
                                      StatusItemCategory                     category) {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("AddStatusItem(" + status_item.Id + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (category == null) {
                          var data = callback != null ? callback(smi) : null;
                          var guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, data);
                          smi.dataTable[data_idx] = guid;
                          return;
                      }

                      var data2 = callback != null ? callback(smi) : null;
                      var guid2 = state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, data2);
                      smi.dataTable[data_idx] = guid2;
                  });

            Exit("RemoveStatusItem(" + status_item.Id + ")",
                 delegate(StateMachineInstanceType smi) {
                     var kselectable = state_target.Get<KSelectable>(smi);
                     if (kselectable != null && smi.dataTable[data_idx] != null) {
                         if (category == null) {
                             var guid = (Guid)smi.dataTable[data_idx];
                             kselectable.RemoveStatusItem(guid);
                         } else
                             kselectable.SetStatusItem(category, null);
                     }

                     smi.dataTable[data_idx] = null;
                 });

            return this;
        }

        public State ToggleStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb,
                                      Func<StateMachineInstanceType, object>     data_callback = null) {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("AddStatusItem(DynamicallyConstructed)",
                  delegate(StateMachineInstanceType smi) {
                      var statusItem = status_item_cb(smi);
                      if (statusItem != null) {
                          var data = data_callback != null ? data_callback(smi) : null;
                          var guid = state_target.Get<KSelectable>(smi).AddStatusItem(statusItem, data);
                          smi.dataTable[data_idx] = guid;
                      }
                  });

            Exit("RemoveStatusItem(DynamicallyConstructed)",
                 delegate(StateMachineInstanceType smi) {
                     var kselectable = state_target.Get<KSelectable>(smi);
                     if (kselectable != null && smi.dataTable[data_idx] != null) {
                         var guid = (Guid)smi.dataTable[data_idx];
                         kselectable.RemoveStatusItem(guid);
                     }

                     smi.dataTable[data_idx] = null;
                 });

            return this;
        }

        public State ToggleFX(Func<StateMachineInstanceType, Instance> callback) {
            var data_idx = CreateDataTableEntry();
            Enter("EnableFX()",
                  delegate(StateMachineInstanceType smi) {
                      var instance = callback(smi);
                      if (instance != null) {
                          instance.StartSM();
                          smi.dataTable[data_idx] = instance;
                      }
                  });

            Exit("DisableFX()",
                 delegate(StateMachineInstanceType smi) {
                     var instance = (Instance)smi.dataTable[data_idx];
                     smi.dataTable[data_idx] = null;
                     if (instance != null) instance.StopSM("ToggleFX.Exit");
                 });

            return this;
        }

        public State BehaviourComplete(Func<StateMachineInstanceType, Tag> tag_cb, bool on_exit = false) {
            if (on_exit)
                Exit("BehaviourComplete()",
                     delegate(StateMachineInstanceType smi) {
                         smi.Trigger(-739654666, tag_cb(smi));
                         smi.GoTo(null);
                     });
            else
                Enter("BehaviourComplete()",
                      delegate(StateMachineInstanceType smi) {
                          smi.Trigger(-739654666, tag_cb(smi));
                          smi.GoTo(null);
                      });

            return this;
        }

        public State BehaviourComplete(Tag tag, bool on_exit = false) {
            if (on_exit)
                Exit("BehaviourComplete(" + tag + ")",
                     delegate(StateMachineInstanceType smi) {
                         smi.Trigger(-739654666, tag);
                         smi.GoTo(null);
                     });
            else
                Enter("BehaviourComplete(" + tag + ")",
                      delegate(StateMachineInstanceType smi) {
                          smi.Trigger(-739654666, tag);
                          smi.GoTo(null);
                      });

            return this;
        }

        public State ToggleBehaviour(Tag                              behaviour_tag,
                                     Transition.ConditionCallback     precondition,
                                     Action<StateMachineInstanceType> on_complete = null) {
            Func<object, bool> precondition_cb = obj => precondition(obj as StateMachineInstanceType);
            Enter("AddPrecondition",
                  delegate(StateMachineInstanceType smi) {
                      if (smi.GetComponent<ChoreConsumer>() != null)
                          smi.GetComponent<ChoreConsumer>()
                             .AddBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
                  });

            Exit("RemovePrecondition",
                 delegate(StateMachineInstanceType smi) {
                     if (smi.GetComponent<ChoreConsumer>() != null)
                         smi.GetComponent<ChoreConsumer>()
                            .RemoveBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
                 });

            ToggleTag(behaviour_tag);
            if (on_complete != null)
                EventHandler(GameHashes.BehaviourTagComplete,
                             delegate(StateMachineInstanceType smi, object data) {
                                 if ((Tag)data == behaviour_tag) on_complete(smi);
                             });

            return this;
        }

        public State ToggleBehaviour(Func<StateMachineInstanceType, Tag> behaviour_tag_cb,
                                     Transition.ConditionCallback        precondition,
                                     Action<StateMachineInstanceType>    on_complete = null) {
            Func<object, bool> precondition_cb = obj => precondition(obj as StateMachineInstanceType);
            Enter("AddPrecondition",
                  delegate(StateMachineInstanceType smi) {
                      if (smi.GetComponent<ChoreConsumer>() != null)
                          smi.GetComponent<ChoreConsumer>()
                             .AddBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
                  });

            Exit("RemovePrecondition",
                 delegate(StateMachineInstanceType smi) {
                     if (smi.GetComponent<ChoreConsumer>() != null)
                         smi.GetComponent<ChoreConsumer>()
                            .RemoveBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
                 });

            ToggleTag(behaviour_tag_cb);
            if (on_complete != null)
                EventHandler(GameHashes.BehaviourTagComplete,
                             delegate(StateMachineInstanceType smi, object data) {
                                 if ((Tag)data == behaviour_tag_cb(smi)) on_complete(smi);
                             });

            return this;
        }

        public void ClearFetch(StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx) {
            var fetchList = (FetchList2)smi.dataTable[fetch_data_idx];
            if (fetchList != null) {
                smi.dataTable[fetch_data_idx]    = null;
                smi.dataTable[callback_data_idx] = null;
                fetchList.Cancel("ClearFetchListFromSM");
            }
        }

        public void SetupFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback,
                               State                                      target_state,
                               StateMachineInstanceType                   smi,
                               int                                        fetch_data_idx,
                               int                                        callback_data_idx) {
            var fetchList = create_fetchlist_callback(smi);
            System.Action action = delegate {
                                       ClearFetch(smi, fetch_data_idx, callback_data_idx);
                                       smi.GoTo(target_state);
                                   };

            fetchList.Submit(action, true);
            smi.dataTable[fetch_data_idx]    = fetchList;
            smi.dataTable[callback_data_idx] = action;
        }

        public State ToggleFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback,
                                 State                                      target_state) {
            var data_idx          = CreateDataTableEntry();
            var callback_data_idx = CreateDataTableEntry();
            Enter("ToggleFetchEnter()",
                  delegate(StateMachineInstanceType smi) {
                      SetupFetch(create_fetchlist_callback, target_state, smi, data_idx, callback_data_idx);
                  });

            Exit("ToggleFetchExit()",
                 delegate(StateMachineInstanceType smi) { ClearFetch(smi, data_idx, callback_data_idx); });

            return this;
        }

        private void ClearChore(StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx) {
            var chore = (Chore)smi.dataTable[chore_data_idx];
            if (chore != null) {
                var value = (Action<Chore>)smi.dataTable[callback_data_idx];
                smi.dataTable[chore_data_idx]    = null;
                smi.dataTable[callback_data_idx] = null;
                var chore2 = chore;
                chore2.onExit = (Action<Chore>)Delegate.Remove(chore2.onExit, value);
                chore.Cancel("ClearGlobalChore");
            }
        }

        private Chore SetupChore(Func<StateMachineInstanceType, Chore> create_chore_callback,
                                 State                                 success_state,
                                 State                                 failure_state,
                                 StateMachineInstanceType              smi,
                                 int                                   chore_data_idx,
                                 int                                   callback_data_idx,
                                 bool                                  is_success_state_reentrant,
                                 bool                                  is_failure_state_reentrant) {
            var chore = create_chore_callback(smi);
            DebugUtil.DevAssert(!chore.IsPreemptable,
                                "ToggleChore can't be used with preemptable chores! :( (but it should...)");

            chore.runUntilComplete = false;
            Action<Chore> action = delegate {
                                       var isComplete = chore.isComplete;
                                       if (isComplete & is_success_state_reentrant ||
                                           (is_failure_state_reentrant && !isComplete)) {
                                           SetupChore(create_chore_callback,
                                                      success_state,
                                                      failure_state,
                                                      smi,
                                                      chore_data_idx,
                                                      callback_data_idx,
                                                      is_success_state_reentrant,
                                                      is_failure_state_reentrant);

                                           return;
                                       }

                                       var state              = success_state;
                                       if (!isComplete) state = failure_state;
                                       ClearChore(smi, chore_data_idx, callback_data_idx);
                                       smi.GoTo(state);
                                   };

            var chore2 = chore;
            chore2.onExit                    = (Action<Chore>)Delegate.Combine(chore2.onExit, action);
            smi.dataTable[chore_data_idx]    = chore;
            smi.dataTable[callback_data_idx] = action;
            return chore;
        }

        public State ToggleRecurringChore(Func<StateMachineInstanceType, Chore> callback,
                                          Func<StateMachineInstanceType, bool>  condition = null) {
            var data_idx          = CreateDataTableEntry();
            var callback_data_idx = CreateDataTableEntry();
            Enter("ToggleRecurringChoreEnter()",
                  delegate(StateMachineInstanceType smi) {
                      if (condition == null || condition(smi))
                          SetupChore(callback,
                                     this,
                                     this,
                                     smi,
                                     data_idx,
                                     callback_data_idx,
                                     true,
                                     true);
                  });

            Exit("ToggleRecurringChoreExit()",
                 delegate(StateMachineInstanceType smi) { ClearChore(smi, data_idx, callback_data_idx); });

            return this;
        }

        public State ToggleChore(Func<StateMachineInstanceType, Chore> callback, State target_state) {
            var data_idx          = CreateDataTableEntry();
            var callback_data_idx = CreateDataTableEntry();
            Enter("ToggleChoreEnter()",
                  delegate(StateMachineInstanceType smi) {
                      SetupChore(callback,
                                 target_state,
                                 target_state,
                                 smi,
                                 data_idx,
                                 callback_data_idx,
                                 false,
                                 false);
                  });

            Exit("ToggleChoreExit()",
                 delegate(StateMachineInstanceType smi) { ClearChore(smi, data_idx, callback_data_idx); });

            return this;
        }

        public State ToggleChore(Func<StateMachineInstanceType, Chore> callback,
                                 State                                 success_state,
                                 State                                 failure_state) {
            var data_idx                   = CreateDataTableEntry();
            var callback_data_idx          = CreateDataTableEntry();
            var is_success_state_reentrant = success_state == this;
            var is_failure_state_reentrant = failure_state == this;
            Enter("ToggleChoreEnter()",
                  delegate(StateMachineInstanceType smi) {
                      SetupChore(callback,
                                 success_state,
                                 failure_state,
                                 smi,
                                 data_idx,
                                 callback_data_idx,
                                 is_success_state_reentrant,
                                 is_failure_state_reentrant);
                  });

            Exit("ToggleChoreExit()",
                 delegate(StateMachineInstanceType smi) { ClearChore(smi, data_idx, callback_data_idx); });

            return this;
        }

        public State ToggleReactable(Func<StateMachineInstanceType, Reactable> callback) {
            var data_idx = CreateDataTableEntry();
            Enter(delegate(StateMachineInstanceType smi) { smi.dataTable[data_idx] = callback(smi); });
            Exit(delegate(StateMachineInstanceType smi) {
                     var reactable = (Reactable)smi.dataTable[data_idx];
                     smi.dataTable[data_idx] = null;
                     if (reactable != null) reactable.Cleanup();
                 });

            return this;
        }

        public State RemoveEffect(string effect_name) {
            var state_target = GetStateTarget();
            Enter("RemoveEffect(" + effect_name + ")",
                  delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Remove(effect_name); });

            return this;
        }

        public State ToggleEffect(string effect_name) {
            var state_target = GetStateTarget();
            Enter("AddEffect(" + effect_name + ")",
                  delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Add(effect_name, false); });

            Exit("RemoveEffect(" + effect_name + ")",
                 delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Remove(effect_name); });

            return this;
        }

        public State ToggleEffect(Func<StateMachineInstanceType, Effect> callback) {
            var state_target = GetStateTarget();
            Enter("AddEffect()",
                  delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Add(callback(smi), false); });

            Exit("RemoveEffect()",
                 delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Remove(callback(smi)); });

            return this;
        }

        public State ToggleEffect(Func<StateMachineInstanceType, string> callback) {
            var state_target = GetStateTarget();
            Enter("AddEffect()",
                  delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Add(callback(smi), false); });

            Exit("RemoveEffect()",
                 delegate(StateMachineInstanceType smi) { state_target.Get<Effects>(smi).Remove(callback(smi)); });

            return this;
        }

        public State LogOnExit(Func<StateMachineInstanceType, string> callback) {
            Enter("Log()", delegate { });
            return this;
        }

        public State LogOnEnter(Func<StateMachineInstanceType, string> callback) {
            Exit("Log()", delegate { });
            return this;
        }

        public State ToggleUrge(Urge urge) { return ToggleUrge(smi => urge); }

        public State ToggleUrge(Func<StateMachineInstanceType, Urge> urge_callback) {
            var state_target = GetStateTarget();
            Enter("AddUrge()",
                  delegate(StateMachineInstanceType smi) {
                      var urge = urge_callback(smi);
                      state_target.Get<ChoreConsumer>(smi).AddUrge(urge);
                  });

            Exit("RemoveUrge()",
                 delegate(StateMachineInstanceType smi) {
                     var urge          = urge_callback(smi);
                     var choreConsumer = state_target.Get<ChoreConsumer>(smi);
                     if (choreConsumer != null) choreConsumer.RemoveUrge(urge);
                 });

            return this;
        }

        public State OnTargetLost(TargetParameter parameter, State target_state) {
            ParamTransition(parameter, target_state, (smi, p) => p == null);
            return this;
        }

        public State ToggleBrain(string reason) {
            var state_target = GetStateTarget();
            Enter("StopBrain(" + reason + ")",
                  delegate(StateMachineInstanceType smi) { state_target.Get<Brain>(smi).Stop(reason); });

            Exit("ResetBrain(" + reason + ")",
                 delegate(StateMachineInstanceType smi) { state_target.Get<Brain>(smi).Reset(reason); });

            return this;
        }

        public State PreBrainUpdate(Action<StateMachineInstanceType> callback) {
            var state_target = GetStateTarget();
            var data_idx     = CreateDataTableEntry();
            Enter("EnablePreBrainUpdate",
                  delegate(StateMachineInstanceType smi) {
                      System.Action action = delegate { callback(smi); };
                      smi.dataTable[data_idx] = action;
                      var brain = state_target.Get<Brain>(smi);
                      DebugUtil.AssertArgs(brain != null, "PreBrainUpdate cannot find a brain");
                      brain.onPreUpdate += action;
                  });

            Exit("DisablePreBrainUpdate",
                 delegate(StateMachineInstanceType smi) {
                     var value = (System.Action)smi.dataTable[data_idx];
                     state_target.Get<Brain>(smi).onPreUpdate -= value;
                     smi.dataTable[data_idx]                  =  null;
                 });

            return this;
        }

        public State TriggerOnEnter(GameHashes evt, Func<StateMachineInstanceType, object> callback = null) {
            var state_target = GetStateTarget();
            Enter("Trigger(" + evt + ")",
                  delegate(StateMachineInstanceType smi) {
                      var go   = state_target.Get(smi);
                      var data = callback != null ? callback(smi) : null;
                      go.Trigger((int)evt, data);
                  });

            return this;
        }

        public State TriggerOnExit(GameHashes evt, Func<StateMachineInstanceType, object> callback = null) {
            var state_target = GetStateTarget();
            Exit("Trigger(" + evt + ")",
                 delegate(StateMachineInstanceType smi) {
                     var gameObject = state_target.Get(smi);
                     if (gameObject != null) {
                         var data = callback != null ? callback(smi) : null;
                         gameObject.Trigger((int)evt, data);
                     }
                 });

            return this;
        }

        public State ToggleStateMachine(Func<StateMachineInstanceType, Instance> callback) {
            var data_idx = CreateDataTableEntry();
            Enter("EnableStateMachine()",
                  delegate(StateMachineInstanceType smi) {
                      var instance = callback(smi);
                      smi.dataTable[data_idx] = instance;
                      instance.StartSM();
                  });

            Exit("DisableStateMachine()",
                 delegate(StateMachineInstanceType smi) {
                     var instance = (Instance)smi.dataTable[data_idx];
                     smi.dataTable[data_idx] = null;
                     if (instance != null) instance.StopSM("ToggleStateMachine.Exit");
                 });

            return this;
        }

        public State ToggleComponentIfFound<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour {
            var state_target = GetStateTarget();
            Enter("EnableComponent(" + typeof(ComponentType).Name + ")",
                  delegate(StateMachineInstanceType smi) {
                      var gameObject = state_target.Get(smi);
                      if (gameObject != null) {
                          var component                            = gameObject.GetComponent<ComponentType>();
                          if (component != null) component.enabled = !disable;
                      }
                  });

            Exit("DisableComponent(" + typeof(ComponentType).Name + ")",
                 delegate(StateMachineInstanceType smi) {
                     var gameObject = state_target.Get(smi);
                     if (gameObject != null) {
                         var component                            = gameObject.GetComponent<ComponentType>();
                         if (component != null) component.enabled = disable;
                     }
                 });

            return this;
        }

        public State ToggleComponent<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour {
            var state_target = GetStateTarget();
            Enter("EnableComponent(" + typeof(ComponentType).Name + ")",
                  delegate(StateMachineInstanceType smi) { state_target.Get<ComponentType>(smi).enabled = !disable; });

            Exit("DisableComponent(" + typeof(ComponentType).Name + ")",
                 delegate(StateMachineInstanceType smi) { state_target.Get<ComponentType>(smi).enabled = disable; });

            return this;
        }

        public State InitializeOperationalFlag(Operational.Flag flag, bool init_val = false) {
            Enter(string.Concat("InitOperationalFlag (", flag.Name, ", ", init_val.ToString(), ")"),
                  delegate(StateMachineInstanceType smi) { smi.GetComponent<Operational>().SetFlag(flag, init_val); });

            return this;
        }

        public State ToggleOperationalFlag(Operational.Flag flag) {
            Enter("ToggleOperationalFlag True (" + flag.Name + ")",
                  delegate(StateMachineInstanceType smi) { smi.GetComponent<Operational>().SetFlag(flag, true); });

            Exit("ToggleOperationalFlag False (" + flag.Name + ")",
                 delegate(StateMachineInstanceType smi) { smi.GetComponent<Operational>().SetFlag(flag, false); });

            return this;
        }

        public State ToggleReserve(TargetParameter reserver,
                                   TargetParameter pickup_target,
                                   FloatParameter requested_amount,
                                   FloatParameter actual_amount) {
            var data_idx = CreateDataTableEntry();
            Enter(string.Concat("Reserve(", pickup_target.name, ", ", requested_amount.name, ")"),
                  delegate(StateMachineInstanceType smi) {
                      var pickupable = pickup_target.Get<Pickupable>(smi);
                      var gameObject = reserver.Get(smi);
                      var val = requested_amount.Get(smi);
                      var val2 = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(gameObject).GetTotalValue());
                      var num = Math.Min(val, val2);
                      num = Math.Min(num, pickupable.UnreservedAmount);
                      if (num <= 0f) {
                          pickupable.PrintReservations();
                          Debug.LogError(string.Concat(val2.ToString(),
                                                       ", ",
                                                       val.ToString(),
                                                       ", ",
                                                       pickupable.UnreservedAmount.ToString(),
                                                       ", ",
                                                       num.ToString()));
                      }

                      actual_amount.Set(num, smi);
                      var num2 = pickupable.Reserve("ToggleReserve", gameObject, num);
                      smi.dataTable[data_idx] = num2;
                  });

            Exit(string.Concat("Unreserve(", pickup_target.name, ", ", requested_amount.name, ")"),
                 delegate(StateMachineInstanceType smi) {
                     var ticket = (int)smi.dataTable[data_idx];
                     smi.dataTable[data_idx] = null;
                     var pickupable = pickup_target.Get<Pickupable>(smi);
                     if (pickupable != null) pickupable.Unreserve("ToggleReserve", ticket);
                 });

            return this;
        }

        public State ToggleWork(string                               work_type,
                                Action<StateMachineInstanceType>     callback,
                                Func<StateMachineInstanceType, bool> validate_callback,
                                State                                success_state,
                                State                                failure_state) {
            var state_target = GetStateTarget();
            Enter("StartWork(" + work_type + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (validate_callback(smi)) {
                          callback(smi);
                          return;
                      }

                      smi.GoTo(failure_state);
                  });

            Update("Work(" + work_type + ")",
                   delegate(StateMachineInstanceType smi, float dt) {
                       if (validate_callback(smi)) {
                           var workResult = state_target.Get<Worker>(smi).Work(dt);
                           if (workResult == Worker.WorkResult.Success) {
                               smi.GoTo(success_state);
                               return;
                           }

                           if (workResult == Worker.WorkResult.Failed) smi.GoTo(failure_state);
                       } else
                           smi.GoTo(failure_state);
                   },
                   UpdateRate.SIM_33ms);

            Exit("StopWork()", delegate(StateMachineInstanceType smi) { state_target.Get<Worker>(smi).StopWork(); });
            return this;
        }

        public State ToggleWork<WorkableType>(TargetParameter                      source_target,
                                              State                                success_state,
                                              State                                failure_state,
                                              Func<StateMachineInstanceType, bool> is_valid_cb)
            where WorkableType : Workable {
            var state_target = GetStateTarget();
            ToggleWork(typeof(WorkableType).Name,
                       delegate(StateMachineInstanceType smi) {
                           Workable workable = source_target.Get<WorkableType>(smi);
                           state_target.Get<Worker>(smi).StartWork(new Worker.StartWorkInfo(workable));
                       },
                       smi => source_target.Get<WorkableType>(smi) != null && (is_valid_cb == null || is_valid_cb(smi)),
                       success_state,
                       failure_state);

            return this;
        }

        public State DoEat(TargetParameter source_target,
                           FloatParameter  amount,
                           State           success_state,
                           State           failure_state) {
            var state_target = GetStateTarget();
            ToggleWork("Eat",
                       delegate(StateMachineInstanceType smi) {
                           var workable = source_target.Get<Edible>(smi);
                           var worker   = state_target.Get<Worker>(smi);
                           var amount2  = amount.Get(smi);
                           worker.StartWork(new Edible.EdibleStartWorkInfo(workable, amount2));
                       },
                       smi => source_target.Get<Edible>(smi) != null,
                       success_state,
                       failure_state);

            return this;
        }

        public State DoSleep(TargetParameter sleeper, TargetParameter bed, State success_state, State failure_state) {
            var state_target = GetStateTarget();
            ToggleWork("Sleep",
                       delegate(StateMachineInstanceType smi) {
                           var worker   = state_target.Get<Worker>(smi);
                           var workable = bed.Get<Sleepable>(smi);
                           worker.StartWork(new Worker.StartWorkInfo(workable));
                       },
                       smi => bed.Get<Sleepable>(smi) != null,
                       success_state,
                       failure_state);

            return this;
        }

        public State DoDelivery(TargetParameter worker_param,
                                TargetParameter storage_param,
                                State           success_state,
                                State           failure_state) {
            ToggleWork("Pickup",
                       delegate(StateMachineInstanceType smi) {
                           var worker   = worker_param.Get<Worker>(smi);
                           var workable = storage_param.Get<Storage>(smi);
                           worker.StartWork(new Worker.StartWorkInfo(workable));
                       },
                       smi => storage_param.Get<Storage>(smi) != null,
                       success_state,
                       failure_state);

            return this;
        }

        public State DoPickup(TargetParameter source_target,
                              TargetParameter result_target,
                              FloatParameter  amount,
                              State           success_state,
                              State           failure_state) {
            var state_target = GetStateTarget();
            ToggleWork("Pickup",
                       delegate(StateMachineInstanceType smi) {
                           var pickupable = source_target.Get<Pickupable>(smi);
                           var worker     = state_target.Get<Worker>(smi);
                           var amount2    = amount.Get(smi);
                           worker.StartWork(new Pickupable.PickupableStartWorkInfo(pickupable,
                                             amount2,
                                             delegate(GameObject result) { result_target.Set(result, smi); }));
                       },
                       smi => source_target.Get<Pickupable>(smi) != null || result_target.Get<Pickupable>(smi) != null,
                       success_state,
                       failure_state);

            return this;
        }

        public State ToggleNotification(Func<StateMachineInstanceType, Notification> callback) {
            var data_idx     = CreateDataTableEntry();
            var state_target = GetStateTarget();
            Enter("EnableNotification()",
                  delegate(StateMachineInstanceType smi) {
                      var notification = callback(smi);
                      smi.dataTable[data_idx] = notification;
                      state_target.AddOrGet<Notifier>(smi).Add(notification);
                  });

            Exit("DisableNotification()",
                 delegate(StateMachineInstanceType smi) {
                     var notification = (Notification)smi.dataTable[data_idx];
                     if (notification != null) {
                         if (state_target != null) {
                             var notifier = state_target.Get<Notifier>(smi);
                             if (notifier != null) notifier.Remove(notification);
                         }

                         smi.dataTable[data_idx] = null;
                     }
                 });

            return this;
        }

        public State DoReport(ReportManager.ReportType               reportType,
                              Func<StateMachineInstanceType, float>  callback,
                              Func<StateMachineInstanceType, string> context_callback = null) {
            Enter("DoReport()",
                  delegate(StateMachineInstanceType smi) {
                      var value = callback(smi);
                      var note  = context_callback != null ? context_callback(smi) : null;
                      ReportManager.Instance.ReportValue(reportType, value, note);
                  });

            return this;
        }

        public State DoNotification(Func<StateMachineInstanceType, Notification> callback) {
            var state_target = GetStateTarget();
            Enter("DoNotification()",
                  delegate(StateMachineInstanceType smi) {
                      var notification = callback(smi);
                      state_target.AddOrGet<Notifier>(smi).Add(notification);
                  });

            return this;
        }

        public State DoTutorial(Tutorial.TutorialMessages msg) {
            Enter("DoTutorial()", delegate { Tutorial.Instance.TutorialMessage(msg); });
            return this;
        }

        public State ToggleScheduleCallback(string                                name,
                                            Func<StateMachineInstanceType, float> time_cb,
                                            Action<StateMachineInstanceType>      callback) {
            var            data_idx = CreateDataTableEntry();
            Action<object> <>9__2;
            Enter("AddScheduledCallback(" + name + ")",
                  delegate(StateMachineInstanceType smi) {
                      var            instance = GameScheduler.Instance;
                      var            name2    = name;
                      var            time     = time_cb(smi);
                      Action<object> callback2;
                      if ((callback2 =  <>9__2) == null)
                      {
                          callback2 = (<>9__2 = delegate(object smi_data) {
                                                    callback((StateMachineInstanceType)smi_data);
                                                });
                      }

                      var schedulerHandle = instance.Schedule(name2, time, callback2, smi);
                      DebugUtil.Assert(smi.dataTable[data_idx] == null);
                      smi.dataTable[data_idx] = schedulerHandle;
                  });

            Exit("RemoveScheduledCallback(" + name + ")",
                 delegate(StateMachineInstanceType smi) {
                     if (smi.dataTable[data_idx] != null) {
                         var schedulerHandle = (SchedulerHandle)smi.dataTable[data_idx];
                         smi.dataTable[data_idx] = null;
                         schedulerHandle.ClearScheduler();
                     }
                 });

            return this;
        }

        public State ScheduleGoTo(Func<StateMachineInstanceType, float> time_cb, BaseState state) {
            Enter("ScheduleGoTo(" + state.name + ")",
                  delegate(StateMachineInstanceType smi) { smi.ScheduleGoTo(time_cb(smi), state); });

            return this;
        }

        public State ScheduleGoTo(float time, BaseState state) {
            var array = new string[5];
            array[0] = "ScheduleGoTo(";
            array[1] = time.ToString();
            array[2] = ", ";
            var num    = 3;
            var state2 = state;
            array[num] = state2 != null ? state2.name : null;
            array[4]   = ")";
            Enter(string.Concat(array), delegate(StateMachineInstanceType smi) { smi.ScheduleGoTo(time, state); });
            return this;
        }

        public State ScheduleAction(string                                name,
                                    Func<StateMachineInstanceType, float> time_cb,
                                    Action<StateMachineInstanceType>      action) {
            Enter("ScheduleAction(" + name + ")",
                  delegate(StateMachineInstanceType smi) { smi.Schedule(time_cb(smi), delegate { action(smi); }); });

            return this;
        }

        public State ScheduleAction(string name, float time, Action<StateMachineInstanceType> action) {
            Enter(string.Concat("ScheduleAction(", time.ToString(), ", ", name, ")"),
                  delegate(StateMachineInstanceType smi) { smi.Schedule(time, delegate { action(smi); }); });

            return this;
        }

        public State ScheduleActionNextFrame(string name, Action<StateMachineInstanceType> action) {
            Enter("ScheduleActionNextFrame(" + name + ")",
                  delegate(StateMachineInstanceType smi) { smi.ScheduleNextFrame(delegate { action(smi); }); });

            return this;
        }

        public State EventHandler(GameHashes                                     evt,
                                  Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback,
                                  Callback                                       callback) {
            return EventHandler(evt,
                                global_event_system_callback,
                                delegate(StateMachineInstanceType smi, object d) { callback(smi); });
        }

        public State EventHandler(GameHashes                                     evt,
                                  Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback,
                                  GameEvent.Callback                             callback) {
            if (events == null) events = new List<StateEvent>();
            var target                 = GetStateTarget();
            var item                   = new GameEvent(evt, callback, target, global_event_system_callback);
            events.Add(item);
            return this;
        }

        public State EventHandler(GameHashes evt, Callback callback) {
            return EventHandler(evt, delegate(StateMachineInstanceType smi, object d) { callback(smi); });
        }

        public State EventHandler(GameHashes evt, GameEvent.Callback callback) {
            EventHandler(evt, null, callback);
            return this;
        }

        public State EventHandlerTransition(GameHashes                                   evt,
                                            State                                        state,
                                            Func<StateMachineInstanceType, object, bool> callback) {
            return EventHandler(evt,
                                delegate(StateMachineInstanceType smi, object d) {
                                    if (callback(smi, d)) smi.GoTo(state);
                                });
        }

        public State EventHandlerTransition(GameHashes                                     evt,
                                            Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback,
                                            State                                          state,
                                            Func<StateMachineInstanceType, object, bool>   callback) {
            return EventHandler(evt,
                                global_event_system_callback,
                                delegate(StateMachineInstanceType smi, object d) {
                                    if (callback(smi, d)) smi.GoTo(state);
                                });
        }

        public State ParamTransition<ParameterType>(Parameter<ParameterType> parameter,
                                                    State state,
                                                    Parameter<ParameterType>.Callback callback) {
            DebugUtil.DevAssert(state != this, "Can't transition to self!");
            if (transitions == null) transitions = new List<BaseTransition>();
            var item = new Parameter<ParameterType>.Transition(transitions.Count, parameter, state, callback);
            transitions.Add(item);
            return this;
        }

        public State OnSignal(Signal signal, State state, Func<StateMachineInstanceType, bool> callback) {
            ParamTransition(signal, state, (smi, p) => callback(smi));
            return this;
        }

        public State OnSignal(Signal signal, State state) {
            ParamTransition(signal, state, null);
            return this;
        }

        public State EnterTransition(State state, Transition.ConditionCallback condition) {
            var str                = "(Stop)";
            if (state != null) str = state.name;
            Enter("Transition(" + str + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (condition(smi)) smi.GoTo(state);
                  });

            return this;
        }

        public State Transition(State                        state,
                                Transition.ConditionCallback condition,
                                UpdateRate                   update_rate = UpdateRate.SIM_200ms) {
            var str                = "(Stop)";
            if (state != null) str = state.name;
            Enter("Transition(" + str + ")",
                  delegate(StateMachineInstanceType smi) {
                      if (condition(smi)) smi.GoTo(state);
                  });

            FastUpdate("Transition(" + str + ")", new TransitionUpdater(condition, state), update_rate);
            return this;
        }

        public State DefaultState(State default_state) {
            defaultState = default_state;
            return this;
        }

        public State GoTo(State state) {
            DebugUtil.DevAssert(state != this, "Can't transition to self");
            var str                = "(null)";
            if (state != null) str = state.name;
            Update("GoTo(" + str + ")", delegate(StateMachineInstanceType smi, float dt) { smi.GoTo(state); });
            return this;
        }

        public State StopMoving() {
            var target = GetStateTarget();
            Enter("StopMoving()", delegate(StateMachineInstanceType smi) { target.Get<Navigator>(smi).Stop(); });
            return this;
        }

        public State ToggleStationaryIdling() {
            GetStateTarget();
            ToggleTag(GameTags.StationaryIdling);
            return this;
        }

        public State OnBehaviourComplete(Tag behaviour, Action<StateMachineInstanceType> cb) {
            EventHandler(GameHashes.BehaviourTagComplete,
                         delegate(StateMachineInstanceType smi, object d) {
                             if ((Tag)d == behaviour) cb(smi);
                         });

            return this;
        }

        public State MoveTo(Func<StateMachineInstanceType, int> cell_callback,
                            State                               success_state = null,
                            State                               fail_state    = null,
                            bool                                update_cell   = false) {
            return MoveTo(cell_callback, null, success_state, fail_state, update_cell);
        }

        public State MoveTo(Func<StateMachineInstanceType, int>          cell_callback,
                            Func<StateMachineInstanceType, CellOffset[]> cell_offsets_callback,
                            State                                        success_state = null,
                            State                                        fail_state    = null,
                            bool                                         update_cell   = false) {
            EventTransition(GameHashes.DestinationReached, success_state);
            EventTransition(GameHashes.NavigationFailed,   fail_state);
            var default_offset = new CellOffset[1];
            var state_target   = GetStateTarget();
            Enter("MoveTo()",
                  delegate(StateMachineInstanceType smi) {
                      var cell                                   = cell_callback(smi);
                      var navigator                              = state_target.Get<Navigator>(smi);
                      var offsets                                = default_offset;
                      if (cell_offsets_callback != null) offsets = cell_offsets_callback(smi);
                      navigator.GoTo(cell, offsets);
                  });

            if (update_cell)
                Update("MoveTo()",
                       delegate(StateMachineInstanceType smi, float dt) {
                           var cell = cell_callback(smi);
                           state_target.Get<Navigator>(smi).UpdateTarget(cell);
                       });

            Exit("StopMoving()",
                 delegate(StateMachineInstanceType smi) { state_target.Get(smi).GetComponent<Navigator>().Stop(); });

            return this;
        }

        public State MoveTo<ApproachableType>(TargetParameter move_parameter,
                                              State           success_state,
                                              State           fail_state = null,
                                              CellOffset[]    override_offsets = null,
                                              NavTactic       tactic = null) where ApproachableType : IApproachable {
            EventTransition(GameHashes.DestinationReached, success_state);
            EventTransition(GameHashes.NavigationFailed,   fail_state);
            var          state_target = GetStateTarget();
            CellOffset[] offsets;
            Enter("MoveTo(" + move_parameter.name + ")",
                  delegate(StateMachineInstanceType smi) {
                      offsets = override_offsets;
                      IApproachable approachable   = move_parameter.Get<ApproachableType>(smi);
                      var           kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
                      if (kmonoBehaviour == null) {
                          smi.GoTo(fail_state);
                          return;
                      }

                      var component                = state_target.Get(smi).GetComponent<Navigator>();
                      if (offsets == null) offsets = approachable.GetOffsets();
                      component.GoTo(kmonoBehaviour, offsets, tactic);
                  });

            Exit("StopMoving()", delegate(StateMachineInstanceType smi) { state_target.Get<Navigator>(smi).Stop(); });
            return this;
        }

        public State Face(TargetParameter face_target, float x_offset = 0f) {
            var state_target = GetStateTarget();
            Enter("Face",
                  delegate(StateMachineInstanceType smi) {
                      if (face_target != null) {
                          var approachable = face_target.Get<IApproachable>(smi);
                          if (approachable != null) {
                              var target_x = approachable.transform.GetPosition().x + x_offset;
                              state_target.Get<Facing>(smi).Face(target_x);
                          }
                      }
                  });

            return this;
        }

        public State TagTransition(Tag[] tags, State state, bool on_remove = false) {
            DebugUtil.DevAssert(state != this, "Can't transition to self!");
            if (transitions == null) transitions = new List<BaseTransition>();
            var item = new TagTransitionData(tags.ToString(),
                                             this,
                                             state,
                                             transitions.Count,
                                             tags,
                                             on_remove,
                                             GetStateTarget());

            transitions.Add(item);
            return this;
        }

        public State TagTransition(Func<StateMachineInstanceType, Tag[]> tags_cb, State state, bool on_remove = false) {
            DebugUtil.DevAssert(state != this, "Can't transition to self!");
            if (transitions == null) transitions = new List<BaseTransition>();
            var item = new TagTransitionData("DynamicTransition",
                                             this,
                                             state,
                                             transitions.Count,
                                             null,
                                             on_remove,
                                             GetStateTarget(),
                                             tags_cb);

            transitions.Add(item);
            return this;
        }

        public State TagTransition(Tag tag, State state, bool on_remove = false) {
            return TagTransition(new[] { tag }, state, on_remove);
        }

        /// <summary>
        /// 在状态机中添加一个事件转换。
        /// </summary>
        /// <param name="evt">触发转换的事件。</param>
        /// <param name="global_event_system_callback">全局事件系统回调函数。</param>
        /// <param name="state">转换目标状态。</param>
        /// <param name="condition">转换条件回调函数，如果未提供，则转换无条件进行。</param>
        /// <returns>返回当前状态，以支持链式调用。</returns>
        public State EventTransition(GameHashes evt,
                                     Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback,
                                     State state,
                                     Transition.ConditionCallback condition = null) {
            // 确保状态转换的合法性，不允许自我转换
            DebugUtil.DevAssert(state != this, "Can't transition to self!");
            
            // 初始化转换列表，如果尚未初始化的话
            if (transitions == null) transitions = new List<BaseTransition>();
            
            // 获取状态目标，用于后续的转换判断
            var target = GetStateTarget();
            
            // 创建并添加新的事件转换数据到转换列表中
            var item = new EventTransitionData(this,
                                               state,
                                               transitions.Count,
                                               evt,
                                               global_event_system_callback,
                                               condition,
                                               target);
            transitions.Add(item);
            
            // 支持链式调用，返回当前状态
            return this;
        }

        public State EventTransition(GameHashes evt, State state, Transition.ConditionCallback condition = null) {
            return EventTransition(evt, null, state, condition);
        }

        public State ReturnSuccess() {
            Enter("ReturnSuccess()",
                  delegate(StateMachineInstanceType smi) {
                      smi.SetStatus(Status.Success);
                      smi.StopSM("GameStateMachine.ReturnSuccess()");
                  });

            return this;
        }

        public State ReturnFailure() {
            Enter("ReturnFailure()",
                  delegate(StateMachineInstanceType smi) {
                      smi.SetStatus(Status.Failed);
                      smi.StopSM("GameStateMachine.ReturnFailure()");
                  });

            return this;
        }

        public State ToggleStatusItem(string name,
                                      string tooltip,
                                      string icon = "",
                                      StatusItem.IconType icon_type = StatusItem.IconType.Info,
                                      NotificationType notification_type = NotificationType.Neutral,
                                      bool allow_multiples = false,
                                      HashedString render_overlay = default(HashedString),
                                      int status_overlays = 129022,
                                      Func<string, StateMachineInstanceType, string> resolve_string_callback = null,
                                      Func<string, StateMachineInstanceType, string> resolve_tooltip_callback = null,
                                      StatusItemCategory category = null) {
            var statusItem = new StatusItem(longName,
                                            name,
                                            tooltip,
                                            icon,
                                            icon_type,
                                            notification_type,
                                            allow_multiples,
                                            render_overlay,
                                            status_overlays);

            if (resolve_string_callback != null)
                statusItem.resolveStringCallback
                    = (str, obj) => resolve_string_callback(str, (StateMachineInstanceType)obj);

            if (resolve_tooltip_callback != null)
                statusItem.resolveTooltipCallback
                    = (str, obj) => resolve_tooltip_callback(str, (StateMachineInstanceType)obj);

            ToggleStatusItem(statusItem, smi => smi, category);
            return this;
        }

        public State PlayAnim(string anim) {
            var state_target = GetStateTarget();
            var mode         = KAnim.PlayMode.Once;
            Enter(string.Concat("PlayAnim(", anim, ", ", mode.ToString(), ")"),
                  delegate(StateMachineInstanceType smi) {
                      var kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) kanimControllerBase.Play(anim, mode);
                  });

            return this;
        }

        public State PlayAnim(Func<StateMachineInstanceType, string> anim_cb,
                              KAnim.PlayMode                         mode = KAnim.PlayMode.Once) {
            var state_target = GetStateTarget();
            Enter("PlayAnim(" + mode + ")",
                  delegate(StateMachineInstanceType smi) {
                      var kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) kanimControllerBase.Play(anim_cb(smi), mode);
                  });

            return this;
        }

        public State PlayAnim(string anim, KAnim.PlayMode mode) {
            var state_target = GetStateTarget();
            Enter(string.Concat("PlayAnim(", anim, ", ", mode.ToString(), ")"),
                  delegate(StateMachineInstanceType smi) {
                      var kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) kanimControllerBase.Play(anim, mode);
                  });

            return this;
        }

        public State PlayAnim(string                                 anim,
                              KAnim.PlayMode                         mode,
                              Func<StateMachineInstanceType, string> suffix_callback) {
            var state_target = GetStateTarget();
            Enter(string.Concat("PlayAnim(", anim, ", ", mode.ToString(), ")"),
                  delegate(StateMachineInstanceType smi) {
                      var str                          = "";
                      if (suffix_callback != null) str = suffix_callback(smi);
                      var kanimControllerBase          = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) kanimControllerBase.Play(anim + str, mode);
                  });

            return this;
        }

        public State QueueAnim(Func<StateMachineInstanceType, string> anim_cb,
                               bool                                   loop            = false,
                               Func<StateMachineInstanceType, string> suffix_callback = null) {
            var state_target = GetStateTarget();
            var mode         = KAnim.PlayMode.Once;
            if (loop) mode   = KAnim.PlayMode.Loop;
            Enter("QueueAnim(" + mode + ")",
                  delegate(StateMachineInstanceType smi) {
                      var str                          = "";
                      if (suffix_callback != null) str = suffix_callback(smi);
                      var kanimControllerBase          = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) kanimControllerBase.Queue(anim_cb(smi) + str, mode);
                  });

            return this;
        }

        public State QueueAnim(string                                 anim,
                               bool                                   loop            = false,
                               Func<StateMachineInstanceType, string> suffix_callback = null) {
            var state_target = GetStateTarget();
            var mode         = KAnim.PlayMode.Once;
            if (loop) mode   = KAnim.PlayMode.Loop;
            Enter(string.Concat("QueueAnim(", anim, ", ", mode.ToString(), ")"),
                  delegate(StateMachineInstanceType smi) {
                      var str                          = "";
                      if (suffix_callback != null) str = suffix_callback(smi);
                      var kanimControllerBase          = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) kanimControllerBase.Queue(anim + str, mode);
                  });

            return this;
        }

        public State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback,
                               KAnim.PlayMode                                 mode = KAnim.PlayMode.Once) {
            var state_target = GetStateTarget();
            Enter("PlayAnims",
                  delegate(StateMachineInstanceType smi) {
                      var kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) {
                          var anim_names = anims_callback(smi);
                          kanimControllerBase.Play(anim_names, mode);
                      }
                  });

            return this;
        }

        public State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback,
                               Func<StateMachineInstanceType, KAnim.PlayMode> mode_cb) {
            var state_target = GetStateTarget();
            Enter("PlayAnims",
                  delegate(StateMachineInstanceType smi) {
                      var kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
                      if (kanimControllerBase != null) {
                          var anim_names = anims_callback(smi);
                          var mode       = mode_cb(smi);
                          kanimControllerBase.Play(anim_names, mode);
                      }
                  });

            return this;
        }

        public State OnAnimQueueComplete(State state) {
            var state_target = GetStateTarget();
            Enter("CheckIfAnimQueueIsEmpty",
                  delegate(StateMachineInstanceType smi) {
                      if (state_target.Get<KBatchedAnimController>(smi).IsStopped()) smi.GoTo(state);
                  });

            return EventTransition(GameHashes.AnimQueueComplete, state);
        }

        internal void EventHandler() { throw new NotImplementedException(); }

        private class TransitionUpdater : UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater {
            private readonly Transition.ConditionCallback condition;
            private readonly State                        state;

            public TransitionUpdater(Transition.ConditionCallback condition, State state) {
                this.condition = condition;
                this.state     = state;
            }

            public void Update(StateMachineInstanceType smi, float dt) {
                if (condition(smi)) smi.GoTo(state);
            }
        }
    }

    public class GameEvent : StateEvent {
        public delegate void Callback(StateMachineInstanceType smi, object callback_data);

        private readonly Callback                                       callback;
        private readonly Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;
        private readonly GameHashes                                     id;
        private readonly TargetParameter                                target;

        public GameEvent(GameHashes                                     id,
                         Callback                                       callback,
                         TargetParameter                                target,
                         Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback) :
            base(id.ToString()) {
            this.id                   = id;
            this.target               = target;
            this.callback             = callback;
            globalEventSystemCallback = global_event_system_callback;
        }

        public override Context Subscribe(Instance smi) {
            var result   = base.Subscribe(smi);
            var cast_smi = (StateMachineInstanceType)smi;
            Action<object> handler = delegate(object d) {
                                         if (Instance.error) return;

                                         callback(cast_smi, d);
                                     };

            if (globalEventSystemCallback != null) {
                var kmonoBehaviour = globalEventSystemCallback(cast_smi);
                result.data = kmonoBehaviour.Subscribe((int)id, handler);
            } else
                result.data = target.Get(cast_smi).Subscribe((int)id, handler);

            return result;
        }

        public override void Unsubscribe(Instance smi, Context context) {
            var stateMachineInstanceType = (StateMachineInstanceType)smi;
            if (globalEventSystemCallback != null) {
                var kmonoBehaviour = globalEventSystemCallback(stateMachineInstanceType);
                if (kmonoBehaviour != null) kmonoBehaviour.Unsubscribe(context.data);
            } else {
                var gameObject = target.Get(stateMachineInstanceType);
                if (gameObject != null) gameObject.Unsubscribe(context.data);
            }
        }
    }

    public class ApproachSubState<ApproachableType> : State where ApproachableType : IApproachable {
        public State InitializeStates(TargetParameter mover,
                                      TargetParameter move_target,
                                      State           success_state,
                                      State           failure_state    = null,
                                      CellOffset[]    override_offsets = null,
                                      NavTactic       tactic           = null) {
            root.Target(mover)
                .OnTargetLost(move_target, failure_state)
                .MoveTo<ApproachableType>(move_target,
                                          success_state,
                                          failure_state,
                                          override_offsets,
                                          tactic == null ? NavigationTactics.ReduceTravelDistance : tactic);

            return this;
        }
    }

    public class DebugGoToSubState : State {
        public State InitializeStates(State exit_state) {
            root.Enter("GoToCursor", delegate(StateMachineInstanceType smi) { GoToCursor(smi); })
                .EventHandler(GameHashes.DebugGoTo,
                              smi => Game.Instance,
                              delegate(StateMachineInstanceType smi) { GoToCursor(smi); })
                .EventTransition(GameHashes.DestinationReached, exit_state)
                .EventTransition(GameHashes.NavigationFailed,   exit_state);

            return this;
        }

        public void GoToCursor(StateMachineInstanceType smi) {
            smi.GetComponent<Navigator>().GoTo(Grid.PosToCell(DebugHandler.GetMousePos()), new CellOffset[1]);
        }
    }

    public class DropSubState : State {
        public State InitializeStates(TargetParameter carrier,
                                      TargetParameter item,
                                      TargetParameter drop_target,
                                      State           success_state,
                                      State           failure_state = null) {
            root.Target(carrier)
                .Enter("Drop",
                       delegate(StateMachineInstanceType smi) {
                           var storage    = carrier.Get<Storage>(smi);
                           var gameObject = item.Get(smi);
                           storage.Drop(gameObject);
                           var cell = Grid.CellAbove(Grid.PosToCell(drop_target.Get<Transform>(smi).GetPosition()));
                           gameObject.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
                           smi.GoTo(success_state);
                       });

            return this;
        }
    }

    public class FetchSubState : State {
        public ApproachSubState<Pickupable> approach;
        public State                        pickup;
        public State                        success;

        public State InitializeStates(TargetParameter fetcher,
                                      TargetParameter pickup_source,
                                      TargetParameter pickup_chunk,
                                      FloatParameter  requested_amount,
                                      FloatParameter  actual_amount,
                                      State           success_state,
                                      State           failure_state = null) {
            Target(fetcher);
            root.DefaultState(approach).ToggleReserve(fetcher, pickup_source, requested_amount, actual_amount);
            approach.InitializeStates(fetcher,
                                      pickup_source,
                                      pickup,
                                      null,
                                      null,
                                      NavigationTactics.ReduceTravelDistance)
                    .OnTargetLost(pickup_source, failure_state);

            pickup.DoPickup(pickup_source, pickup_chunk, actual_amount, success_state, failure_state)
                  .EventTransition(GameHashes.AbortWork, failure_state);

            return this;
        }
    }

    public class HungrySubState : State {
        public State hungry;
        public State satisfied;

        public State InitializeStates(TargetParameter target, StatusItem status_item) {
            Target(target);
            root.DefaultState(satisfied);
            satisfied.EventTransition(GameHashes.AddUrge, hungry, smi => IsHungry(smi));
            hungry.EventTransition(GameHashes.RemoveUrge, satisfied, smi => !IsHungry(smi))
                  .ToggleStatusItem(status_item, null);

            return this;
        }

        private static bool IsHungry(StateMachineInstanceType smi) {
            return smi.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
        }
    }

    public class PlantAliveSubState : State {
        public State InitializeStates(TargetParameter plant, State death_state = null) {
            root.Target(plant)
                .TagTransition(GameTags.Uprooted, death_state)
                .EventTransition(GameHashes.TooColdFatal, death_state, smi => isLethalTemperature(plant.Get(smi)))
                .EventTransition(GameHashes.TooHotFatal,  death_state, smi => isLethalTemperature(plant.Get(smi)))
                .EventTransition(GameHashes.Drowned,      death_state);

            return this;
        }

        public bool ForceUpdateStatus(GameObject plant) {
            var component  = plant.GetComponent<TemperatureVulnerable>();
            var component2 = plant.GetComponent<EntombVulnerable>();
            var component3 = plant.GetComponent<PressureVulnerable>();
            return (component  == null || !component.IsLethal)     &&
                   (component2 == null || !component2.GetEntombed) &&
                   (component3 == null || !component3.IsLethal);
        }

        private static bool isLethalTemperature(GameObject plant) {
            var component = plant.GetComponent<TemperatureVulnerable>();
            return !(component == null) &&
                   (component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold ||
                    component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot);
        }
    }
}