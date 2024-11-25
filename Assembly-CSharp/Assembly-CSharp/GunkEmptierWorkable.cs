﻿using System;
using Klei.AI;
using UnityEngine;

public class GunkEmptierWorkable : Workable
{
		private GunkEmptierWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gunkdump_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		this.storage = base.GetComponent<Storage>();
		base.SetWorkTime(8.5f);
	}

		protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float mass = Mathf.Min(new float[]
		{
			dt / this.workTime * GunkMonitor.GUNK_CAPACITY,
			this.gunkMonitor.CurrentGunkMass,
			this.storage.RemainingCapacity()
		});
		this.gunkMonitor.ExpellGunk(mass, this.storage);
		return base.OnWorkTick(worker, dt);
	}

		protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		this.TriggerRoomEffects();
	}

		private void TriggerRoomEffects()
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), base.worker.GetComponent<Effects>());
		}
	}

		protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.gunkMonitor != null)
		{
			this.gunkMonitor.ExpellAllGunk(this.storage);
		}
		this.gunkMonitor = null;
		base.OnCompleteWork(worker);
	}

		protected override void OnStopWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnStopWork(worker);
	}

		protected override void OnAbortWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnAbortWork(worker);
		this.gunkMonitor = null;
	}

		private void RemoveExpellingRadStatusItem()
	{
		if (Sim.IsRadiationEnabled())
		{
			base.worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
	}

		private Storage storage;

		private GunkMonitor.Instance gunkMonitor;
}
