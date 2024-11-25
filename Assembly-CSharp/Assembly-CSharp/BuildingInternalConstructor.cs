using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

public class BuildingInternalConstructor
    : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget,
        BuildingInternalConstructor.Def> {
    public BoolParameter     constructionRequested = new BoolParameter(true);
    public State             inoperational;
    public OperationalStates operational;

    public override void InitializeStates(out BaseState default_state) {
        default_state = inoperational;
        serializable  = SerializeType.ParamsOnly;
        inoperational
            .EventTransition(GameHashes.OperationalChanged,
                             operational,
                             smi => smi.GetComponent<Operational>().IsOperational)
            .Enter(delegate(Instance smi) { smi.ShowConstructionSymbol(false); });

        operational.DefaultState(operational.constructionRequired)
                   .EventTransition(GameHashes.OperationalChanged,
                                    inoperational,
                                    smi => !smi.GetComponent<Operational>().IsOperational);

        operational.constructionRequired
                   .EventTransition(GameHashes.OnStorageChange,
                                    operational.constructionHappening,
                                    smi => smi.GetMassForConstruction() != null)
                   .EventTransition(GameHashes.OnStorageChange,
                                    operational.constructionSatisfied,
                                    smi => smi.HasOutputInStorage())
                   .ToggleFetch(smi => smi.CreateFetchList(), operational.constructionHappening)
                   .ParamTransition(constructionRequested, operational.constructionSatisfied, IsFalse)
                   .Enter(delegate(Instance smi) { smi.ShowConstructionSymbol(true); })
                   .Exit(delegate(Instance  smi) { smi.ShowConstructionSymbol(false); });

        operational.constructionHappening
                   .EventTransition(GameHashes.OnStorageChange,
                                    operational.constructionSatisfied,
                                    smi => smi.HasOutputInStorage())
                   .EventTransition(GameHashes.OnStorageChange,
                                    operational.constructionRequired,
                                    smi => smi.GetMassForConstruction() == null)
                   .ToggleChore(smi => smi.CreateWorkChore(),
                                operational.constructionHappening,
                                operational.constructionHappening)
                   .ParamTransition(constructionRequested, operational.constructionSatisfied, IsFalse)
                   .Enter(delegate(Instance smi) { smi.ShowConstructionSymbol(true); })
                   .Exit(delegate(Instance  smi) { smi.ShowConstructionSymbol(false); });

        operational.constructionSatisfied
                   .EventTransition(GameHashes.OnStorageChange,
                                    operational.constructionRequired,
                                    smi => !smi.HasOutputInStorage() && constructionRequested.Get(smi))
                   .ParamTransition(constructionRequested,
                                    operational.constructionRequired,
                                    (smi, p) => p && !smi.HasOutputInStorage());
    }

    public class Def : BaseDef {
        public float                 constructionMass;
        public string                constructionSymbol;
        public List<string>          outputIDs;
        public bool                  spawnIntoStorage;
        public DefComponent<Storage> storage;
    }

    public class OperationalStates : State {
        public State constructionHappening;
        public State constructionRequired;
        public State constructionSatisfied;
    }

    public new class Instance : GameInstance, ISidescreenButtonControl {
        [Serialize]
        private float constructionElapsed;

        private          ProgressBar progressBar;
        private readonly Storage     storage;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            storage = def.storage.Get(this);
            GetComponent<RocketModule>()
                .AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep,
                                    new InternalConstructionCompleteCondition(this));
        }

        public string SidescreenButtonText {
            get {
                if (!smi.sm.constructionRequested.Get(smi))
                    return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR.text,
                                         Assets.GetPrefab(def.outputIDs[0]).GetProperName());

                return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR.text,
                                     Assets.GetPrefab(def.outputIDs[0]).GetProperName());
            }
        }

        public string SidescreenButtonTooltip {
            get {
                if (!smi.sm.constructionRequested.Get(smi))
                    return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text,
                                         Assets.GetPrefab(def.outputIDs[0]).GetProperName());

                return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text,
                                     Assets.GetPrefab(def.outputIDs[0]).GetProperName());
            }
        }

        public void OnSidescreenButtonPressed() {
            smi.sm.constructionRequested.Set(!smi.sm.constructionRequested.Get(smi), smi);
            if (DebugHandler.InstantBuildMode && smi.sm.constructionRequested.Get(smi) && !HasOutputInStorage())
                ConstructionComplete(true);
        }

        public void SetButtonTextOverride(ButtonMenuTextOverride text) { throw new NotImplementedException(); }
        public bool SidescreenEnabled()                                { return true; }
        public bool SidescreenButtonInteractable()                     { return true; }
        public int  ButtonSideScreenSortOrder()                        { return 20; }
        public int  HorizontalGroupID()                                { return -1; }

        protected override void OnCleanUp() {
            Element element       = null;
            var     num           = 0f;
            var     num2          = 0f;
            var     maxValue      = byte.MaxValue;
            var     disease_count = 0;
            foreach (var s in def.outputIDs) {
                var gameObject = storage.FindFirst(s);
                if (gameObject != null) {
                    var component = gameObject.GetComponent<PrimaryElement>();
                    Debug.Assert(element == null || element == component.Element);
                    element =  component.Element;
                    num2    =  GameUtil.GetFinalTemperature(num, num2, component.Mass, component.Temperature);
                    num     += component.Mass;
                    gameObject.DeleteObject();
                }
            }

            if (element != null)
                element.substance.SpawnResource(transform.GetPosition(), num, num2, maxValue, disease_count);

            base.OnCleanUp();
        }

        public FetchList2 CreateFetchList() {
            var fetchList = new FetchList2(storage, Db.Get().ChoreTypes.Fetch);
            var component = GetComponent<PrimaryElement>();
            fetchList.Add(component.Element.tag, null, def.constructionMass);
            return fetchList;
        }

        public PrimaryElement GetMassForConstruction() {
            var component = GetComponent<PrimaryElement>();
            return storage.FindFirstWithMass(component.Element.tag, def.constructionMass);
        }

        public bool HasOutputInStorage() { return storage.FindFirst(def.outputIDs[0].ToTag()); }

        public bool IsRequestingConstruction() {
            sm.constructionRequested.Get(this);
            return smi.sm.constructionRequested.Get(smi);
        }

        public void ConstructionComplete(bool force = false) {
            SimHashes element_id;
            if (!force) {
                var massForConstruction = GetMassForConstruction();
                element_id = massForConstruction.ElementID;
                var mass = massForConstruction.Mass;
                var num  = massForConstruction.Temperature * massForConstruction.Mass;
                massForConstruction.Mass -= def.constructionMass;
                Mathf.Clamp(num / mass, 0f, 318.15f);
            } else {
                element_id = SimHashes.Cuprite;
                var temperature = GetComponent<PrimaryElement>().Temperature;
            }

            foreach (var s in def.outputIDs) {
                var gameObject
                    = GameUtil.KInstantiate(Assets.GetPrefab(s), transform.GetPosition(), Grid.SceneLayer.Ore);

                gameObject.GetComponent<PrimaryElement>().SetElement(element_id, false);
                gameObject.SetActive(true);
                if (def.spawnIntoStorage) storage.Store(gameObject);
            }
        }

        public WorkChore<BuildingInternalConstructorWorkable> CreateWorkChore() {
            return new WorkChore<BuildingInternalConstructorWorkable>(Db.Get().ChoreTypes.Build, master);
        }

        public void ShowConstructionSymbol(bool show) {
            var component = master.GetComponent<KBatchedAnimController>();
            if (component != null) component.SetSymbolVisiblity(def.constructionSymbol, show);
        }
    }
}