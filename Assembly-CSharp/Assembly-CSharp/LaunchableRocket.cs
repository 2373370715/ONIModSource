using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocket : StateMachineComponent<LaunchableRocket.StatesInstance>, ILaunchableRocket {
    [Serialize]
    private float flightAnimOffset;

    public  List<GameObject> parts = new List<GameObject>();
    private GameObject       soundSpeakerObject;

    [Serialize]
    private int takeOffLocation;

    public LaunchableRocketRegisterType registerType         => LaunchableRocketRegisterType.Spacecraft;
    public GameObject                   LaunchableGameObject => gameObject;
    public float                        rocketSpeed          { get; private set; }
    public bool                         isLanding            { get; private set; }

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.master.parts = AttachableBuilding.GetAttachedNetwork(smi.master.GetComponent<AttachableBuilding>());
        if (SpacecraftManager.instance.GetSpacecraftID(this) == -1) {
            var spacecraft = new Spacecraft(GetComponent<LaunchConditionManager>());
            spacecraft.GenerateName();
            SpacecraftManager.instance.RegisterSpacecraft(spacecraft);
            gameObject.AddOrGet<RocketLaunchConditionVisualizerEffect>();
        }

        smi.StartSM();
    }

    public List<GameObject> GetEngines() {
        var list = new List<GameObject>();
        foreach (var gameObject in parts)
            if (gameObject.GetComponent<RocketEngine>())
                list.Add(gameObject);

        return list;
    }

    protected override void OnCleanUp() {
        SpacecraftManager.instance.UnregisterSpacecraft(GetComponent<LaunchConditionManager>());
        base.OnCleanUp();
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, LaunchableRocket, object>.GameInstance {
        public StatesInstance(LaunchableRocket master) : base(master) { }

        public bool IsMissionState(Spacecraft.MissionState state) {
            return SpacecraftManager.instance
                                    .GetSpacecraftFromLaunchConditionManager(master
                                                                                 .GetComponent<
                                                                                     LaunchConditionManager>())
                                    .state ==
                   state;
        }

        public void SetMissionState(Spacecraft.MissionState state) {
            SpacecraftManager.instance
                             .GetSpacecraftFromLaunchConditionManager(master.GetComponent<LaunchConditionManager>())
                             .SetState(state);
        }
    }

    public class States : GameStateMachine<States, StatesInstance, LaunchableRocket> {
        public State             grounded;
        public NotGroundedStates not_grounded;

        public override void InitializeStates(out BaseState default_state) {
            default_state = grounded;
            serializable  = SerializeType.Both_DEPRECATED;
            grounded.ToggleTag(GameTags.RocketOnGround)
                    .Enter(delegate(StatesInstance smi) {
                               foreach (var gameObject in smi.master.parts)
                                   if (!(gameObject == null))
                                       gameObject.AddTag(GameTags.RocketOnGround);
                           })
                    .Exit(delegate(StatesInstance smi) {
                              foreach (var gameObject in smi.master.parts)
                                  if (!(gameObject == null))
                                      gameObject.RemoveTag(GameTags.RocketOnGround);
                          })
                    .EventTransition(GameHashes.DoLaunchRocket, not_grounded.launch_pre)
                    .Enter(delegate(StatesInstance smi) {
                               smi.master.rocketSpeed = 0f;
                               foreach (var gameObject in smi.master.parts)
                                   if (!(gameObject == null))
                                       gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;

                               smi.SetMissionState(Spacecraft.MissionState.Grounded);
                           });

            not_grounded.ToggleTag(GameTags.RocketNotOnGround)
                        .Enter(delegate(StatesInstance smi) {
                                   foreach (var gameObject in smi.master.parts)
                                       if (!(gameObject == null))
                                           gameObject.AddTag(GameTags.RocketNotOnGround);
                               })
                        .Exit(delegate(StatesInstance smi) {
                                  foreach (var gameObject in smi.master.parts)
                                      if (!(gameObject == null))
                                          gameObject.RemoveTag(GameTags.RocketNotOnGround);
                              });

            not_grounded.launch_pre.Enter(delegate(StatesInstance smi) {
                                              smi.master.isLanding   = false;
                                              smi.master.rocketSpeed = 0f;
                                              smi.master.parts
                                                  = AttachableBuilding.GetAttachedNetwork(smi.master
                                                      .GetComponent<AttachableBuilding>());

                                              if (smi.master.soundSpeakerObject == null) {
                                                  smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
                                                  smi.master.soundSpeakerObject.transform
                                                     .SetParent(smi.master.gameObject.transform);
                                              }

                                              foreach (var go in smi.master.GetEngines()) go.Trigger(-1358394196);
                                              Game.Instance.Trigger(-1277991738, smi.gameObject);
                                              foreach (var gameObject in smi.master.parts)
                                                  if (!(gameObject == null)) {
                                                      smi.master.takeOffLocation
                                                          = Grid.PosToCell(smi.master.gameObject);

                                                      gameObject.Trigger(-1277991738);
                                                  }

                                              smi.SetMissionState(Spacecraft.MissionState.Launching);
                                          })
                        .ScheduleGoTo(5f, not_grounded.launch_loop);

            not_grounded.launch_loop.EventTransition(GameHashes.DoReturnRocket, not_grounded.returning)
                        .Update(delegate(StatesInstance smi, float dt) {
                                    smi.master.isLanding = false;
                                    var flag = true;
                                    var num  = Mathf.Clamp(Mathf.Pow(smi.timeinstate / 5f, 4f), 0f, 10f);
                                    smi.master.rocketSpeed      =  num;
                                    smi.master.flightAnimOffset += dt * num;
                                    foreach (var gameObject in smi.master.parts)
                                        if (!(gameObject == null)) {
                                            var component = gameObject.GetComponent<KBatchedAnimController>();
                                            component.Offset = Vector3.up * smi.master.flightAnimOffset;
                                            var positionIncludingOffset = component.PositionIncludingOffset;
                                            if (smi.master.soundSpeakerObject == null) {
                                                smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
                                                smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject
                                                    .transform);
                                            }

                                            smi.master.soundSpeakerObject.transform
                                               .SetLocalPosition(smi.master.flightAnimOffset *
                                                                 Vector3.up);

                                            if (Grid.PosToXY(positionIncludingOffset).y >
                                                Singleton<KBatchedAnimUpdater>.Instance.GetVisibleSize().y + 20)
                                                gameObject.GetComponent<KBatchedAnimController>().enabled = false;
                                            else {
                                                flag = false;
                                                DoWorldDamage(gameObject, positionIncludingOffset);
                                            }
                                        }

                                    if (flag) smi.GoTo(not_grounded.space);
                                },
                                UpdateRate.SIM_33ms)
                        .Exit(delegate(StatesInstance smi) { smi.gameObject.GetMyWorld().RevealSurface(); });

            not_grounded.space.Enter(delegate(StatesInstance smi) {
                                         smi.master.rocketSpeed = 0f;
                                         foreach (var gameObject in smi.master.parts)
                                             if (!(gameObject == null)) {
                                                 gameObject.GetComponent<KBatchedAnimController>().Offset
                                                     = Vector3.up * smi.master.flightAnimOffset;

                                                 gameObject.GetComponent<KBatchedAnimController>().enabled = false;
                                             }

                                         smi.SetMissionState(Spacecraft.MissionState.Underway);
                                     })
                        .EventTransition(GameHashes.DoReturnRocket,
                                         not_grounded.returning,
                                         smi => smi.IsMissionState(Spacecraft.MissionState.WaitingToLand));

            not_grounded.returning.Enter(delegate(StatesInstance smi) {
                                             smi.master.isLanding   = true;
                                             smi.master.rocketSpeed = 0f;
                                             smi.SetMissionState(Spacecraft.MissionState.Landing);
                                         })
                        .Update(delegate(StatesInstance smi, float dt) {
                                    smi.master.isLanding = true;
                                    var component = smi.master.gameObject.GetComponent<KBatchedAnimController>();
                                    component.Offset = Vector3.up * smi.master.flightAnimOffset;
                                    var num = Mathf.Abs(smi.master.gameObject.transform.position.y +
                                                        component.Offset.y -
                                                        (Grid.CellToPos(smi.master.takeOffLocation) +
                                                         Vector3.down * (Grid.CellSizeInMeters / 2f)).y);

                                    var num2 = Mathf.Clamp(0.5f * num, 0f, 10f) * dt;
                                    smi.master.rocketSpeed      =  num2;
                                    smi.master.flightAnimOffset -= num2;
                                    var flag = true;
                                    if (smi.master.soundSpeakerObject == null) {
                                        smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
                                        smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject
                                            .transform);
                                    }

                                    smi.master.soundSpeakerObject.transform
                                       .SetLocalPosition(smi.master.flightAnimOffset *
                                                         Vector3.up);

                                    foreach (var gameObject in smi.master.parts)
                                        if (!(gameObject == null)) {
                                            var component2 = gameObject.GetComponent<KBatchedAnimController>();
                                            component2.Offset = Vector3.up * smi.master.flightAnimOffset;
                                            var positionIncludingOffset = component2.PositionIncludingOffset;
                                            if (Grid.IsValidCell(Grid.PosToCell(gameObject)))
                                                gameObject.GetComponent<KBatchedAnimController>().enabled = true;
                                            else
                                                flag = false;

                                            DoWorldDamage(gameObject, positionIncludingOffset);
                                        }

                                    if (flag) smi.GoTo(not_grounded.landing_loop);
                                },
                                UpdateRate.SIM_33ms);

            not_grounded.landing_loop.Enter(delegate(StatesInstance smi) {
                                                smi.master.isLanding = true;
                                                var num = -1;
                                                for (var i = 0; i < smi.master.parts.Count; i++) {
                                                    var gameObject = smi.master.parts[i];
                                                    if (!(gameObject == null) &&
                                                        gameObject != smi.master.gameObject &&
                                                        gameObject.GetComponent<RocketEngine>() != null)
                                                        num = i;
                                                }

                                                if (num != -1) smi.master.parts[num].Trigger(-1358394196);
                                            })
                        .Update(delegate(StatesInstance smi, float dt) {
                                    smi.master.gameObject.GetComponent<KBatchedAnimController>().Offset
                                        = Vector3.up * smi.master.flightAnimOffset;

                                    var flightAnimOffset = smi.master.flightAnimOffset;
                                    var num              = Mathf.Clamp(0.5f * flightAnimOffset, 0f, 10f);
                                    smi.master.rocketSpeed      =  num;
                                    smi.master.flightAnimOffset -= num * dt;
                                    if (smi.master.soundSpeakerObject == null) {
                                        smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
                                        smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject
                                            .transform);
                                    }

                                    smi.master.soundSpeakerObject.transform
                                       .SetLocalPosition(smi.master.flightAnimOffset *
                                                         Vector3.up);

                                    if (num <= 0.0025f && dt != 0f) {
                                        smi.master.GetComponent<KSelectable>().IsSelectable = true;
                                        Game.Instance.Trigger(-887025858, smi.gameObject);
                                        foreach (var gameObject in smi.master.parts)
                                            if (!(gameObject == null))
                                                gameObject.Trigger(-887025858);

                                        smi.GoTo(grounded);
                                        return;
                                    }

                                    foreach (var gameObject2 in smi.master.parts)
                                        if (!(gameObject2 == null)) {
                                            var component = gameObject2.GetComponent<KBatchedAnimController>();
                                            component.Offset = Vector3.up * smi.master.flightAnimOffset;
                                            var positionIncludingOffset = component.PositionIncludingOffset;
                                            DoWorldDamage(gameObject2, positionIncludingOffset);
                                        }
                                },
                                UpdateRate.SIM_33ms);
        }

        private static void DoWorldDamage(GameObject part, Vector3 apparentPosition) {
            var component = part.GetComponent<OccupyArea>();
            component.UpdateOccupiedArea();
            foreach (var offset in component.OccupiedCellsOffsets) {
                var num = Grid.OffsetCell(Grid.PosToCell(apparentPosition), offset);
                if (Grid.IsValidCell(num)) {
                    if (Grid.Solid[num])
                        WorldDamage.Instance.ApplyDamage(num,
                                                         10000f,
                                                         num,
                                                         BUILDINGS.DAMAGESOURCES.ROCKET,
                                                         UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET);
                    else if (Grid.FakeFloor[num]) {
                        var gameObject = Grid.Objects[num, 39];
                        if (gameObject != null) {
                            var component2 = gameObject.GetComponent<BuildingHP>();
                            if (component2 != null)
                                gameObject.Trigger(-794517298,
                                                   new BuildingHP.DamageSourceInfo {
                                                       damage    = component2.MaxHitPoints,
                                                       source    = BUILDINGS.DAMAGESOURCES.ROCKET,
                                                       popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
                                                   });
                        }
                    }
                }
            }
        }

        public class NotGroundedStates : State {
            public State landing_loop;
            public State launch_loop;
            public State launch_pre;
            public State returning;
            public State space;
        }
    }
}