using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class Telepad : StateMachineComponent<Telepad.StatesInstance> {
    private const          float                     MAX_IMMIGRATION_TIME = 120f;
    private const          int                       NUM_METER_NOTCHES    = 8;
    public static readonly HashedString[]            PortalBirthAnim      = { "portalbirth" };
    private                MeterController           meter;
    private                List<MinionStartingStats> minionStats;

    [MyCmpReq]
    private KSelectable selectable;

    public float startingSkillPoints;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        GetComponent<Deconstructable>().allowDeconstruction = false;
        var num  = 0;
        var num2 = 0;
        Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
        if (num == 0)
            Debug.LogError(string.Concat("Headquarters spawned at: (", num.ToString(), ",", num2.ToString(), ")"));
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        Components.Telepads.Add(this);
        meter = new MeterController(GetComponent<KBatchedAnimController>(),
                                    "meter_target",
                                    "meter",
                                    Meter.Offset.Behind,
                                    Grid.SceneLayer.NoLayer,
                                    "meter_target",
                                    "meter_fill",
                                    "meter_frame",
                                    "meter_OL");

        meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
        smi.StartSM();
    }

    protected override void OnCleanUp() {
        Components.Telepads.Remove(this);
        base.OnCleanUp();
    }

    public void Update() {
        if (smi.IsColonyLost()) return;

        if (Immigration.Instance.ImmigrantsAvailable && GetComponent<Operational>().IsOperational) {
            smi.sm.openPortal.Trigger(smi);
            selectable.SetStatusItem(Db.Get().StatusItemCategories.Main,
                                     Db.Get().BuildingStatusItems.NewDuplicantsAvailable,
                                     this);
        } else {
            smi.sm.closePortal.Trigger(smi);
            selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
        }

        if (GetTimeRemaining() < -120f) {
            Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
            Immigration.Instance.EndImmigration();
        }
    }

    public void RejectAll() {
        Immigration.Instance.EndImmigration();
        smi.sm.closePortal.Trigger(smi);
    }

    public void OnAcceptDelivery(ITelepadDeliverable delivery) {
        var cell = Grid.PosToCell(this);
        Immigration.Instance.EndImmigration();
        var gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
        var component  = gameObject.GetComponent<MinionIdentity>();
        if (component != null) {
            ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime,
                                               GameClock.Instance.GetTimeSinceStartOfReport(),
                                               string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME,
                                                             DUPLICANTS.CHORES.NOT_EXISTING_TASK),
                                               gameObject.GetProperName());

            foreach (var minionIdentity in
                     Components.LiveMinionIdentities.GetWorldItems(this.gameObject.GetComponent<KSelectable>()
                                                                       .GetMyWorldId()))
                minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);

            var component2 = component.GetComponent<MinionResume>();
            var num        = 0;
            while (num < startingSkillPoints) {
                component2.ForceAddSkillPoint();
                num++;
            }

            if (component.HasTag(GameTags.Minions.Models.Bionic))
                GameScheduler.Instance.Schedule("BonusBatteryDelivery", 5f, delegate { Trigger(1982288670); });
        }

        smi.sm.closePortal.Trigger(smi);
    }

    public float GetTimeRemaining() { return Immigration.Instance.GetTimeRemaining(); }

    public class StatesInstance : GameStateMachine<States, StatesInstance, Telepad, object>.GameInstance {
        public StatesInstance(Telepad master) : base(master) { }
        public bool IsColonyLost() { return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver(); }

        public void UpdateMeter() {
            var timeRemaining   = Immigration.Instance.GetTimeRemaining();
            var totalWaitTime   = Immigration.Instance.GetTotalWaitTime();
            var positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
            master.meter.SetPositionPercent(positionPercent);
        }

        public IEnumerator SpawnExtraPowerBanks() {
            var cellTarget = Grid.OffsetCell(Grid.PosToCell(this.gameObject), 1, 2);
            var count      = 5;
            int num;
            for (var i = 0; i < count; i = num + 1) {
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus,
                                              MISC.POPFX.EXTRA_POWERBANKS_BIONIC,
                                              this.gameObject.transform,
                                              new Vector3(0f, 0.5f, 0f));

                PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner"));
                var gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"),
                                                   Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) -
                                                   Vector3.right / 2f);

                gameObject.SetActive(true);
                var initial_velocity = new Vector2((-2.5f + 5f * (i / 5f)) / 2f, 2f);
                if (GameComps.Fallers.Has(gameObject)) GameComps.Fallers.Remove(gameObject);
                GameComps.Fallers.Add(gameObject, initial_velocity);
                yield return new WaitForSeconds(0.25f);

                num = i;
            }

            yield return 0;
        }
    }

    public class States : GameStateMachine<States, StatesInstance, Telepad> {
        private static readonly HashedString[]      workingAnims = { "working_loop", "working_pst" };
        public                  BonusDeliveryStates bonusDelivery;
        public                  State               close;
        public                  Signal              closePortal;
        public                  State               idle;
        public                  Signal              idlePortal;
        public                  State               open;
        public                  State               opening;
        public                  Signal              openPortal;
        public                  State               resetToIdle;
        public                  State               unoperational;

        public override void InitializeStates(out BaseState default_state) {
            default_state = idle;
            serializable  = SerializeType.Both_DEPRECATED;
            root.OnSignal(idlePortal, resetToIdle).EventTransition(GameHashes.BonusTelepadDelivery, bonusDelivery.pre);
            resetToIdle.GoTo(idle);
            idle.Enter(delegate(StatesInstance smi) { smi.UpdateMeter(); })
                .Update("TelepadMeter",
                        delegate(StatesInstance smi, float dt) { smi.UpdateMeter(); },
                        UpdateRate.SIM_4000ms)
                .EventTransition(GameHashes.OperationalChanged,
                                 unoperational,
                                 smi => !smi.GetComponent<Operational>().IsOperational)
                .PlayAnim("idle")
                .OnSignal(openPortal, opening);

            unoperational.PlayAnim("idle")
                         .Enter("StopImmigration",
                                delegate(StatesInstance smi) { smi.master.meter.SetPositionPercent(0f); })
                         .EventTransition(GameHashes.OperationalChanged,
                                          idle,
                                          smi => smi.GetComponent<Operational>().IsOperational);

            opening.Enter(delegate(StatesInstance smi) { smi.master.meter.SetPositionPercent(1f); })
                   .PlayAnim("working_pre")
                   .OnAnimQueueComplete(open);

            open.OnSignal(closePortal, close)
                .Enter(delegate(StatesInstance smi) { smi.master.meter.SetPositionPercent(1f); })
                .PlayAnim("working_loop", KAnim.PlayMode.Loop)
                .Transition(close, smi => smi.IsColonyLost())
                .EventTransition(GameHashes.OperationalChanged,
                                 close,
                                 smi => !smi.GetComponent<Operational>().IsOperational);

            close.Enter(delegate(StatesInstance smi) { smi.master.meter.SetPositionPercent(0f); })
                 .PlayAnims(smi => workingAnims)
                 .OnAnimQueueComplete(idle);

            bonusDelivery.pre.PlayAnim("working_pre").OnAnimQueueComplete(bonusDelivery.loop);
            bonusDelivery.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop)
                         .ScheduleAction("SpawnBonusDelivery",
                                         1f,
                                         delegate(StatesInstance smi) {
                                             smi.master.StartCoroutine(smi.SpawnExtraPowerBanks());
                                         })
                         .ScheduleGoTo(3f, bonusDelivery.pst);

            bonusDelivery.pst.PlayAnim("working_pst").OnAnimQueueComplete(idle);
        }

        public class BonusDeliveryStates : State {
            public State loop;
            public State pre;
            public State pst;
        }
    }
}