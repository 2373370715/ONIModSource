using Klei;
using Klei.AI;
using UnityEngine;

public class OilChangerWorkableUse : Workable, IGameObjectEffectDescriptor {
    private OilChangerWorkableUse() { SetReportType(ReportManager.ReportType.PersonalTime); }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        showProgressBar     = true;
        resetProgressOnStop = true;
        attributeConverter  = Db.Get().AttributeConverters.ToiletSpeed;
        SetWorkTime(8.5f);
    }

    protected override void OnStartWork(WorkerBase worker) {
        base.OnStartWork(worker);
        if (worker != null) {
            var position = worker.transform.GetPosition();
            position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
            worker.transform.SetPosition(position);
        }

        var roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameObject);
        if (roomOfGameObject != null)
            roomOfGameObject.roomType.TriggerRoomEffects(GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
    }

    protected override void OnStopWork(WorkerBase worker) {
        if (worker != null) {
            var position = worker.transform.GetPosition();
            position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
            worker.transform.SetPosition(position);
        }

        base.OnStopWork(worker);
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        var component = GetComponent<Storage>();
        var smi       = worker.GetSMI<BionicOilMonitor.Instance>();
        if (smi != null) {
            var b          = 200f - smi.CurrentOilMass;
            var num        = Mathf.Min(component.GetMassAvailable(GameTags.LubricatingOil), b);
            var num2       = num;
            var num3       = 0f;
            var component2 = GetComponent<Storage>();
            var simHashes  = SimHashes.CrudeOil;
            foreach (var simHashes2 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys) {
                float               num4;
                SimUtil.DiseaseInfo diseaseInfo;
                float               num5;
                component2.ConsumeAndGetDisease(simHashes2.CreateTag(), num2, out num4, out diseaseInfo, out num5);
                if (num4 > num3) {
                    simHashes = simHashes2;
                    num3      = num4;
                }

                num2 -= num4;
            }

            GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, num2);
            smi.RefillOil(num);
            var component3 = worker.GetComponent<Effects>();
            foreach (var simHashes3 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys) {
                var effect = BionicOilMonitor.LUBRICANT_TYPE_EFFECT[simHashes3];
                if (simHashes == simHashes3)
                    component3.Add(effect, true);
                else
                    component3.Remove(effect);
            }
        }

        base.OnCompleteWork(worker);
    }
}