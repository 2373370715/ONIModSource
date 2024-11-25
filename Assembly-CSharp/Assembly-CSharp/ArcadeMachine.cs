using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ArcadeMachine : StateMachineComponent<ArcadeMachine.StatesInstance>, IGameObjectEffectDescriptor {
    public  CellOffset[] choreOffsets = { new CellOffset(-1, 0), new CellOffset(1, 0) };
    private Chore[]      chores;

    public KAnimFile[][] overrideAnims = {
        new[] { Assets.GetAnim("anim_interacts_arcade_cabinet_playerone_kanim") },
        new[] { Assets.GetAnim("anim_interacts_arcade_cabinet_playertwo_kanim") }
    };

    public  HashSet<int>            players = new HashSet<int>();
    private ArcadeMachineWorkable[] workables;

    public HashedString[][] workAnims = {
        new HashedString[] { "working_pre", "working_loop_one_p" },
        new HashedString[] { "working_pre", "working_loop_two_p" }
    };

    List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go) {
        var list = new List<Descriptor>();
        var item = default(Descriptor);
        item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION);
        list.Add(item);
        Effect.AddModifierDescriptions(gameObject, list, "PlayedArcade", true);
        return list;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        GameScheduler.Instance.Schedule("Scheduling Tutorial",
                                        2f,
                                        delegate {
                                            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule);
                                        });

        workables = new ArcadeMachineWorkable[choreOffsets.Length];
        chores = new Chore[choreOffsets.Length];
        for (var i = 0; i < workables.Length; i++) {
            var pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), choreOffsets[i]), Grid.SceneLayer.Move);
            var go = ChoreHelpers.CreateLocator("ArcadeMachineWorkable", pos);
            var arcadeMachineWorkable = go.AddOrGet<ArcadeMachineWorkable>();
            var kselectable = go.AddOrGet<KSelectable>();
            kselectable.SetName(this.GetProperName());
            kselectable.IsSelectable = false;
            var player_index           = i;
            var arcadeMachineWorkable2 = arcadeMachineWorkable;
            arcadeMachineWorkable2.OnWorkableEventCB
                = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(arcadeMachineWorkable2.OnWorkableEventCB,
                                                                             new Action<Workable,
                                                                                 Workable.WorkableEvent>(delegate(
                                                                                 Workable               workable,
                                                                                 Workable.WorkableEvent ev) {
                                                                                 OnWorkableEvent(player_index,
                                                                                  ev);
                                                                             }));

            arcadeMachineWorkable.overrideAnims = overrideAnims[i];
            arcadeMachineWorkable.workAnims     = workAnims[i];
            workables[i]                        = arcadeMachineWorkable;
            workables[i].owner                  = this;
        }

        smi.StartSM();
    }

    protected override void OnCleanUp() {
        UpdateChores(false);
        for (var i = 0; i < workables.Length; i++)
            if (workables[i]) {
                Util.KDestroyGameObject(workables[i]);
                workables[i] = null;
            }

        base.OnCleanUp();
    }

    private Chore CreateChore(int i) {
        Workable            workable           = workables[i];
        var                 relax              = Db.Get().ChoreTypes.Relax;
        IStateMachineTarget target             = workable;
        ChoreProvider       chore_provider     = null;
        var                 run_until_complete = true;
        Action<Chore>       on_complete        = null;
        Action<Chore>       on_begin           = null;
        var                 recreation         = Db.Get().ScheduleBlockTypes.Recreation;
        var workChore = new WorkChore<ArcadeMachineWorkable>(relax,
                                                             target,
                                                             chore_provider,
                                                             run_until_complete,
                                                             on_complete,
                                                             on_begin,
                                                             OnSocialChoreEnd,
                                                             false,
                                                             recreation,
                                                             false,
                                                             true,
                                                             null,
                                                             false,
                                                             true,
                                                             false,
                                                             PriorityScreen.PriorityClass.high);

        workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
        return workChore;
    }

    private void OnSocialChoreEnd(Chore chore) {
        if (gameObject.HasTag(GameTags.Operational)) UpdateChores();
    }

    public void UpdateChores(bool update = true) {
        for (var i = 0; i < choreOffsets.Length; i++) {
            var chore = chores[i];
            if (update) {
                if (chore == null || chore.isComplete) chores[i] = CreateChore(i);
            } else if (chore != null) {
                chore.Cancel("locator invalidated");
                chores[i] = null;
            }
        }
    }

    public void OnWorkableEvent(int player, Workable.WorkableEvent ev) {
        if (ev == Workable.WorkableEvent.WorkStarted)
            players.Add(player);
        else
            players.Remove(player);

        smi.sm.playerCount.Set(players.Count, smi);
    }

    public class States : GameStateMachine<States, StatesInstance, ArcadeMachine> {
        public OperationalStates operational;
        public IntParameter      playerCount;
        public State             unoperational;

        public override void InitializeStates(out BaseState default_state) {
            default_state = unoperational;
            unoperational.Enter(delegate(StatesInstance smi) { smi.SetActive(false); })
                         .TagTransition(GameTags.Operational, operational)
                         .PlayAnim("off");

            operational.TagTransition(GameTags.Operational, unoperational, true)
                       .Enter("CreateChore", delegate(StatesInstance smi) { smi.master.UpdateChores(); })
                       .Exit("CancelChore", delegate(StatesInstance  smi) { smi.master.UpdateChores(false); })
                       .DefaultState(operational.stopped);

            operational.stopped.Enter(delegate(StatesInstance smi) { smi.SetActive(false); })
                       .PlayAnim("on")
                       .ParamTransition(playerCount, operational.pre, (smi, p) => p > 0);

            operational.pre.Enter(delegate(StatesInstance smi) { smi.SetActive(true); })
                       .PlayAnim("working_pre")
                       .OnAnimQueueComplete(operational.playing);

            operational.playing.PlayAnim(GetPlayingAnim, KAnim.PlayMode.Loop)
                       .ParamTransition(playerCount, operational.post,         (smi, p) => p == 0)
                       .ParamTransition(playerCount, operational.playing_coop, (smi, p) => p > 1);

            operational.playing_coop.PlayAnim(GetPlayingAnim, KAnim.PlayMode.Loop)
                       .ParamTransition(playerCount, operational.post,    (smi, p) => p == 0)
                       .ParamTransition(playerCount, operational.playing, (smi, p) => p == 1);

            operational.post.PlayAnim("working_pst").OnAnimQueueComplete(operational.stopped);
        }

        private string GetPlayingAnim(StatesInstance smi) {
            var flag  = smi.master.players.Contains(0);
            var flag2 = smi.master.players.Contains(1);
            if (flag && !flag2) return "working_loop_one_p";

            if (flag2 && !flag) return "working_loop_two_p";

            return "working_loop_coop_p";
        }

        public class OperationalStates : State {
            public State playing;
            public State playing_coop;
            public State post;
            public State pre;
            public State stopped;
        }
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, ArcadeMachine, object>.GameInstance {
        private readonly Operational operational;
        public StatesInstance(ArcadeMachine smi) : base(smi) { operational = master.GetComponent<Operational>(); }
        public void SetActive(bool          active) { operational.SetActive(operational.IsOperational && active); }
    }
}