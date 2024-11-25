using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor {
    public SimHashes exhaustElement     = SimHashes.CarbonDioxide;
    public float     exhaustEmitRate    = 2f;
    public float     exhaustTemperature = 1000f;

    [Serialize]
    private float flightAnimOffset;

    [Serialize]
    private int landOffLocation;

    private bool             releasingAstronaut;
    private float            rocketSpeed;
    private GameObject       soundSpeakerObject;
    public  List<Descriptor> GetDescriptors(GameObject go) { return null; }

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.StartSM();
    }

    public void ReleaseAstronaut() {
        if (releasingAstronaut) return;

        releasingAstronaut = true;
        var component        = GetComponent<MinionStorage>();
        var storedMinionInfo = component.GetStoredMinionInfo();
        for (var i = storedMinionInfo.Count - 1; i >= 0; i--) {
            var info = storedMinionInfo[i];
            component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(smi.master.transform.GetPosition())));
        }

        releasingAstronaut = false;
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, PodLander, object>.GameInstance {
        public StatesInstance(PodLander master) : base(master) { }
    }

    public class States : GameStateMachine<States, StatesInstance, PodLander> {
        public State crashed;
        public State landing;

        public override void InitializeStates(out BaseState default_state) {
            default_state = landing;
            serializable  = SerializeType.Both_DEPRECATED;
            landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop)
                   .Enter(delegate(StatesInstance smi) { smi.master.flightAnimOffset = 50f; })
                   .Update(delegate(StatesInstance smi, float dt) {
                               var num = 10f;
                               smi.master.rocketSpeed
                                   = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0f, num - 2f);

                               smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
                               var component = smi.master.GetComponent<KBatchedAnimController>();
                               component.Offset = Vector3.up * smi.master.flightAnimOffset;
                               var positionIncludingOffset = component.PositionIncludingOffset;
                               var num2 = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() +
                                                         smi.master.GetComponent<KBatchedAnimController>().Offset);

                               if (Grid.IsValidCell(num2))
                                   SimMessages.EmitMass(num2,
                                                        ElementLoader.GetElementIndex(smi.master.exhaustElement),
                                                        dt * smi.master.exhaustEmitRate,
                                                        smi.master.exhaustTemperature,
                                                        0,
                                                        0);

                               if (component.Offset.y <= 0f) smi.GoTo(crashed);
                           },
                           UpdateRate.SIM_33ms);

            crashed.PlayAnim("grounded")
                   .Enter(delegate(StatesInstance smi) {
                              smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
                              smi.master.rocketSpeed                                   = 0f;
                              smi.master.ReleaseAstronaut();
                          });
        }
    }
}