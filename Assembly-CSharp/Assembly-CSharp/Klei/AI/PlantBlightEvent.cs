using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI {
    public class PlantBlightEvent : GameplayEvent<PlantBlightEvent.StatesInstance> {
        private const float  BLIGHT_DISTANCE = 6f;
        public        float  incubationDuration;
        public        float  infectionDuration;
        public        string targetPlantPrefab;

        public PlantBlightEvent(string id,
                                string targetPlantPrefab,
                                float  infectionDuration,
                                float  incubationDuration) : base(id) {
            this.targetPlantPrefab  = targetPlantPrefab;
            this.infectionDuration  = infectionDuration;
            this.incubationDuration = incubationDuration;
            title                   = GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.NAME;
            description             = GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.DESCRIPTION;
        }

        public override StateMachine.Instance
            GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance) {
            return new StatesInstance(manager, eventInstance, this);
        }

        public class States
            : GameplayEventStateMachine<States, StatesInstance, GameplayEventManager, PlantBlightEvent> {
            public Signal         doFinish;
            public State          finished;
            public FloatParameter nextInfection;
            public State          planning;
            public RunningStates  running;

            public override void InitializeStates(out BaseState default_state) {
                base.InitializeStates(out default_state);
                default_state = planning;
                serializable  = SerializeType.Both_DEPRECATED;
                planning.Enter(delegate(StatesInstance smi) { smi.InfectAPlant(true); }).GoTo(running);
                running.ToggleNotification(smi => EventInfoScreen.CreateNotification(GenerateEventPopupData(smi)))
                       .EventHandlerTransition(GameHashes.Uprooted, finished, NoBlightedPlants)
                       .DefaultState(running.waiting)
                       .OnSignal(doFinish, finished);

                running.waiting.ParamTransition(nextInfection, running.infect, (smi, p) => p <= 0f)
                       .Update(delegate(StatesInstance smi, float dt) { nextInfection.Delta(-dt, smi); },
                               UpdateRate.SIM_4000ms);

                running.infect.Enter(delegate(StatesInstance smi) { smi.InfectAPlant(false); }).GoTo(running.waiting);
                finished.DoNotification(smi => CreateSuccessNotification(smi, GenerateEventPopupData(smi)))
                        .ReturnSuccess();
            }

            public override EventInfoData GenerateEventPopupData(StatesInstance smi) {
                var eventInfoData = new EventInfoData(smi.gameplayEvent.title,
                                                      smi.gameplayEvent.description,
                                                      smi.gameplayEvent.animFileName);

                var value = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
                eventInfoData.location        = GAMEPLAY_EVENTS.LOCATIONS.COLONY_WIDE;
                eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
                eventInfoData.SetTextParameter("plant", value);
                return eventInfoData;
            }

            private Notification CreateSuccessNotification(StatesInstance smi, EventInfoData eventInfoData) {
                var plantName = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
                return new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS.Replace("{plant}", plantName),
                                        NotificationType.Neutral,
                                        (list, data) =>
                                            GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS_TOOLTIP.Replace("{plant}",
                                             plantName));
            }

            private bool NoBlightedPlants(StatesInstance smi, object obj) {
                var gameObject = (GameObject)obj;
                if (!gameObject.HasTag(GameTags.Blighted)) return true;

                foreach (var crop in Components.Crops.Items.FindAll(p => p.name == smi.gameplayEvent.targetPlantPrefab))
                    if (!(gameObject.gameObject == crop.gameObject) && crop.HasTag(GameTags.Blighted))
                        return false;

                return true;
            }

            public class RunningStates : State {
                public State infect;
                public State waiting;
            }
        }

        public class StatesInstance
            : GameplayEventStateMachine<States, StatesInstance, GameplayEventManager, PlantBlightEvent>.
                GameplayEventStateMachineInstance {
            [Serialize]
            private readonly float startTime;

            public StatesInstance(GameplayEventManager  master,
                                  GameplayEventInstance eventInstance,
                                  PlantBlightEvent      blightEvent) : base(master, eventInstance, blightEvent) {
                startTime = Time.time;
            }

            public void InfectAPlant(bool initialInfection) {
                if (Time.time - startTime > smi.gameplayEvent.infectionDuration) {
                    sm.doFinish.Trigger(smi);
                    return;
                }

                var list = Components.Crops.Items.FindAll(p => p.name == smi.gameplayEvent.targetPlantPrefab);
                if (list.Count == 0) {
                    sm.doFinish.Trigger(smi);
                    return;
                }

                if (list.Count > 0) {
                    var list2 = new List<Crop>();
                    var list3 = new List<Crop>();
                    foreach (var crop in list)
                        if (crop.HasTag(GameTags.Blighted))
                            list2.Add(crop);
                        else
                            list3.Add(crop);

                    if (list2.Count == 0) {
                        if (initialInfection) {
                            var crop2 = list3[Random.Range(0, list3.Count)];
                            Debug.Log("Blighting a random plant", crop2);
                            crop2.GetComponent<BlightVulnerable>().MakeBlighted();
                        } else
                            sm.doFinish.Trigger(smi);
                    } else if (list3.Count > 0) {
                        var crop3 = list2[Random.Range(0, list2.Count)];
                        Debug.Log("Spreading blight from a plant", crop3);
                        list3.Shuffle();
                        foreach (var crop4 in list3)
                            if ((crop4.transform.GetPosition() - crop3.transform.GetPosition()).magnitude < 6f) {
                                crop4.GetComponent<BlightVulnerable>().MakeBlighted();
                                break;
                            }
                    }
                }

                sm.nextInfection.Set(smi.gameplayEvent.incubationDuration, this);
            }
        }
    }
}