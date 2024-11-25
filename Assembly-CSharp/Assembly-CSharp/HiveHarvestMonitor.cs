using STRINGS;

public class HiveHarvestMonitor
    : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def> {
    public State         do_not_harvest;
    public HarvestStates harvest;
    public BoolParameter shouldHarvest;

    public override void InitializeStates(out BaseState default_state) {
        default_state = do_not_harvest;
        serializable  = SerializeType.ParamsOnly;
        root.EventHandler(GameHashes.RefreshUserMenu, delegate(Instance smi) { smi.OnRefreshUserMenu(); });
        do_not_harvest.ParamTransition(shouldHarvest, harvest, (smi, bShouldHarvest) => bShouldHarvest);
        harvest.ParamTransition(shouldHarvest, do_not_harvest, (smi, bShouldHarvest) => !bShouldHarvest)
               .DefaultState(harvest.not_ready);

        harvest.not_ready.EventTransition(GameHashes.OnStorageChange,
                                          harvest.ready,
                                          smi => smi.storage.GetMassAvailable(smi.def.producedOre) >=
                                                 smi.def.harvestThreshold);

        harvest.ready.ToggleChore(smi => smi.CreateHarvestChore(), harvest.not_ready)
               .EventTransition(GameHashes.OnStorageChange,
                                harvest.not_ready,
                                smi => smi.storage.GetMassAvailable(smi.def.producedOre) < smi.def.harvestThreshold);
    }

    public class Def : BaseDef {
        public float harvestThreshold;
        public Tag   producedOre;
    }

    public class HarvestStates : State {
        public State not_ready;
        public State ready;
    }

    public new class Instance : GameInstance {
        [MyCmpReq]
        public Storage storage;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) { }

        public void OnRefreshUserMenu() {
            if (sm.shouldHarvest.Get(this)) {
                Game.Instance.userMenu.AddButton(gameObject,
                                                 new KIconButtonMenu.ButtonInfo("action_building_disabled",
                                                                                UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE
                                                                                    .NAME,
                                                                                delegate {
                                                                                    sm.shouldHarvest.Set(false, this);
                                                                                },
                                                                                global::Action.NumActions,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE
                                                                                    .TOOLTIP));

                return;
            }

            Game.Instance.userMenu.AddButton(gameObject,
                                             new KIconButtonMenu.ButtonInfo("action_empty_contents",
                                                                            UI.USERMENUACTIONS.EMPTYBEEHIVE.NAME,
                                                                            delegate {
                                                                                sm.shouldHarvest.Set(true, this);
                                                                            },
                                                                            global::Action.NumActions,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            UI.USERMENUACTIONS.EMPTYBEEHIVE.TOOLTIP));
        }

        public Chore CreateHarvestChore() {
            return new WorkChore<HiveWorkableEmpty>(Db.Get().ChoreTypes.Ranch,
                                                    master.GetComponent<HiveWorkableEmpty>(),
                                                    null,
                                                    true,
                                                    smi.OnEmptyComplete);
        }

        public void OnEmptyComplete(Chore chore) { smi.storage.Drop(smi.def.producedOre); }
    }
}