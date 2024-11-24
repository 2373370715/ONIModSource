using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001005 RID: 4101
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableUse")]
public class ToiletWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06005395 RID: 21397 RVA: 0x000D6753 File Offset: 0x000D4953
	private ToiletWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06005396 RID: 21398 RVA: 0x000D1F83 File Offset: 0x000D0183
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06005397 RID: 21399 RVA: 0x002783D0 File Offset: 0x002765D0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
	}

	// Token: 0x06005398 RID: 21400 RVA: 0x000D676E File Offset: 0x000D496E
	protected override void OnStopWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x06005399 RID: 21401 RVA: 0x000D679F File Offset: 0x000D499F
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnAbortWork(worker);
	}

	// Token: 0x0600539A RID: 21402 RVA: 0x00278464 File Offset: 0x00276664
	protected override void OnCompleteWork(WorkerBase worker)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(worker);
		if (amountInstance != null)
		{
			this.lastAmountOfWasteMassRemovedFromDupe = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
			this.lastElementRemovedFromDupe = SimHashes.DirtyWater;
			amountInstance.SetValue(0f);
		}
		else
		{
			GunkMonitor.Instance smi = worker.GetSMI<GunkMonitor.Instance>();
			if (smi != null)
			{
				this.lastAmountOfWasteMassRemovedFromDupe = smi.CurrentGunkMass;
				this.lastElementRemovedFromDupe = GunkMonitor.GunkElement;
				smi.SetGunkMassValue(0f);
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_GunkedToilet, true);
			}
		}
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
			AmountInstance amountInstance2 = Db.Get().Amounts.RadiationBalance.Lookup(worker);
			RadiationMonitor.Instance smi2 = worker.GetSMI<RadiationMonitor.Instance>();
			float num = Math.Min(amountInstance2.value, 100f * smi2.difficultySettingMod);
			if (num >= 1f)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, worker.transform, Vector3.up * 2f, 1.5f, false, false);
			}
			amountInstance2.ApplyDelta(-num);
		}
		this.timesUsed++;
		if (amountInstance != null)
		{
			base.Trigger(-350347868, worker);
		}
		else
		{
			base.Trigger(261445693, worker);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x04003A67 RID: 14951
	[Serialize]
	public int timesUsed;

	// Token: 0x04003A68 RID: 14952
	public SimHashes lastElementRemovedFromDupe = SimHashes.DirtyWater;

	// Token: 0x04003A69 RID: 14953
	public float lastAmountOfWasteMassRemovedFromDupe;
}
