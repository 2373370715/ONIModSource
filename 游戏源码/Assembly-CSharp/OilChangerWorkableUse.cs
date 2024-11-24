using System;
using Klei;
using Klei.AI;
using UnityEngine;

// Token: 0x02000EEB RID: 3819
public class OilChangerWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06004D05 RID: 19717 RVA: 0x000AC786 File Offset: 0x000AA986
	private OilChangerWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004D06 RID: 19718 RVA: 0x000D1F83 File Offset: 0x000D0183
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06004D07 RID: 19719 RVA: 0x00263FF4 File Offset: 0x002621F4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			worker.transform.SetPosition(position);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
	}

	// Token: 0x06004D08 RID: 19720 RVA: 0x00264068 File Offset: 0x00262268
	protected override void OnStopWork(WorkerBase worker)
	{
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			worker.transform.SetPosition(position);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x06004D09 RID: 19721 RVA: 0x002640AC File Offset: 0x002622AC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		BionicOilMonitor.Instance smi = worker.GetSMI<BionicOilMonitor.Instance>();
		if (smi != null)
		{
			float b = 200f - smi.CurrentOilMass;
			float num = Mathf.Min(component.GetMassAvailable(GameTags.LubricatingOil), b);
			float num2 = num;
			float num3 = 0f;
			Storage component2 = base.GetComponent<Storage>();
			SimHashes simHashes = SimHashes.CrudeOil;
			foreach (SimHashes simHashes2 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo;
				float num5;
				component2.ConsumeAndGetDisease(simHashes2.CreateTag(), num2, out num4, out diseaseInfo, out num5);
				if (num4 > num3)
				{
					simHashes = simHashes2;
					num3 = num4;
				}
				num2 -= num4;
			}
			base.GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, num2);
			smi.RefillOil(num);
			Effects component3 = worker.GetComponent<Effects>();
			foreach (SimHashes simHashes3 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				Effect effect = BionicOilMonitor.LUBRICANT_TYPE_EFFECT[simHashes3];
				if (simHashes == simHashes3)
				{
					component3.Add(effect, true);
				}
				else
				{
					component3.Remove(effect);
				}
			}
		}
		base.OnCompleteWork(worker);
	}
}
