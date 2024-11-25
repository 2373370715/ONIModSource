using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor {
    public enum State {
        Invalid,
        Ready,
        Blocked,
        OverPressure,
        Closed
    }

    private int cell = -1;

    [SerializeField]
    public ConduitType conduitType = ConduitType.Gas;

    [SerializeField]
    public Endpoint endpointType;

    [Serialize]
    public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();

    [MyCmpGet, NonSerialized]
    public Operational operational;

    [SerializeField]
    public float overpressureMass = 1f;

    [NonSerialized]
    public bool showConnectivityIcons = true;

    private StatesInstance smi;

    [MyCmpGet, NonSerialized]
    public Structure structure;

    public int  SortKey   { get; set; }
    public bool IsBlocked => GetEndPointState() != State.Ready;

    public List<Descriptor> GetDescriptors(GameObject go) {
        var formattedMass = GameUtil.GetFormattedMass(overpressureMass);
        return new List<Descriptor> {
            new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS,          formattedMass),
                           string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass))
        };
    }

    public void UpdateVentedMass(SimHashes element, float mass) {
        if (!lifeTimeVentMass.ContainsKey(element)) {
            lifeTimeVentMass.Add(element, mass);
            return;
        }

        var dictionary = lifeTimeVentMass;
        dictionary[element] += mass;
    }

    public float GetVentedMass(SimHashes element) {
        if (lifeTimeVentMass.ContainsKey(element)) return lifeTimeVentMass[element];

        return 0f;
    }

    public bool Closed() {
        var flag = false;
        return (operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag) ||
               (operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag,               out flag) && !flag);
    }

    protected override void OnSpawn() {
        var component = GetComponent<Building>();
        cell = component.GetUtilityOutputCell();
        smi  = new StatesInstance(this);
        smi.StartSM();
    }

    public State GetEndPointState() {
        var result   = State.Invalid;
        var endpoint = endpointType;
        if (endpoint != Endpoint.Source) {
            if (endpoint == Endpoint.Sink) {
                result = State.Ready;
                var num                             = cell;
                if (!IsValidOutputCell(num)) result = Grid.Solid[num] ? State.Blocked : State.OverPressure;
            }
        } else
            result = IsConnected() ? State.Ready : State.Blocked;

        return result;
    }

    public bool IsConnected() {
        var networkForCell = Conduit.GetNetworkManager(conduitType).GetNetworkForCell(cell);
        return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
    }

    private bool IsValidOutputCell(int output_cell) {
        var result = false;
        if ((structure == null || !structure.IsEntombed() || !Closed()) && !Grid.Solid[output_cell])
            result = Grid.Mass[output_cell] < overpressureMass;

        return result;
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, Vent, object>.GameInstance {
        private readonly Exhaust exhaust;
        public StatesInstance(Vent master) : base(master) { exhaust = master.GetComponent<Exhaust>(); }

        public bool NeedsExhaust() {
            return exhaust                   != null        &&
                   master.GetEndPointState() != State.Ready &&
                   master.endpointType       == Endpoint.Source;
        }

        public bool Blocked() {
            return master.GetEndPointState() == State.Blocked && master.endpointType > Endpoint.Source;
        }

        public bool OverPressure() {
            return exhaust                   != null               &&
                   master.GetEndPointState() == State.OverPressure &&
                   master.endpointType       > Endpoint.Source;
        }

        public void CheckTransitions() {
            if (NeedsExhaust()) {
                smi.GoTo(sm.needExhaust);
                return;
            }

            if (master.Closed()) {
                smi.GoTo(sm.closed);
                return;
            }

            if (Blocked()) {
                smi.GoTo(sm.open.blocked);
                return;
            }

            if (OverPressure()) {
                smi.GoTo(sm.open.overPressure);
                return;
            }

            smi.GoTo(sm.open.idle);
        }

        public StatusItem SelectStatusItem(StatusItem gas_status_item, StatusItem liquid_status_item) {
            if (master.conduitType != ConduitType.Gas) return liquid_status_item;

            return gas_status_item;
        }
    }

    public class States : GameStateMachine<States, StatesInstance, Vent> {
        public State     closed;
        public State     needExhaust;
        public OpenState open;

        public override void InitializeStates(out BaseState default_state) {
            default_state = open.idle;
            root.Update("CheckTransitions", delegate(StatesInstance smi, float dt) { smi.CheckTransitions(); });
            open.TriggerOnEnter(GameHashes.VentOpen);
            closed.TriggerOnEnter(GameHashes.VentClosed);
            open.blocked.ToggleStatusItem(smi => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed,
                                                                      Db.Get()
                                                                        .BuildingStatusItems.LiquidVentObstructed));

            open.overPressure.ToggleStatusItem(smi =>
                                                   smi.SelectStatusItem(Db.Get()
                                                                          .BuildingStatusItems.GasVentOverPressure,
                                                                        Db.Get()
                                                                          .BuildingStatusItems.LiquidVentOverPressure));
        }

        public class OpenState : State {
            public State blocked;
            public State idle;
            public State overPressure;
        }
    }
}