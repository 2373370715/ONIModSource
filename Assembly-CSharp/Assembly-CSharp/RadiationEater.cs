using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

[SkipSaveFileSerialization]
public class RadiationEater : StateMachineComponent<RadiationEater.StatesInstance> {
    protected override void OnSpawn() { smi.StartSM(); }

    public class StatesInstance : GameStateMachine<States, StatesInstance, RadiationEater, object>.GameInstance {
        public AttributeModifier radiationEating;

        public StatesInstance(RadiationEater master) : base(master) {
            radiationEating = new AttributeModifier(Db.Get().Attributes.RadiationRecovery.Id,
                                                    TRAITS.RADIATION_EATER_RECOVERY,
                                                    DUPLICANTS.TRAITS.RADIATIONEATER.NAME);
        }

        public void OnEatRads(float radsEaten) {
            var delta = Mathf.Abs(radsEaten) * TRAITS.RADS_TO_CALS;
            smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Calories).ApplyDelta(delta);
        }
    }

    public class States : GameStateMachine<States, StatesInstance, RadiationEater> {
        public override void InitializeStates(out BaseState default_state) {
            default_state = root;
            root.ToggleAttributeModifier("Radiation Eating", smi => smi.radiationEating)
                .EventHandler(GameHashes.RadiationRecovery,
                              delegate(StatesInstance smi, object data) {
                                  var radsEaten = (float)data;
                                  smi.OnEatRads(radsEaten);
                              });
        }
    }
}