﻿using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/JuicerWorkable")]
public class JuicerWorkable : Workable, IWorkerPrioritizable
{
	private JuicerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_juicer_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
		this.juicer = base.GetComponent<Juicer>();
	}

	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
	}

	protected override void OnCompleteWork(Worker worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, this.juicer.waterMassPerUse, out num, out diseaseInfo, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		for (int i = 0; i < this.juicer.ingredientTags.Length; i++)
		{
			SimUtil.DiseaseInfo diseaseInfo2;
			component.ConsumeAndGetDisease(this.juicer.ingredientTags[i], this.juicer.ingredientMassesPerUse[i], out num, out diseaseInfo2, out num2);
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, this.juicer.ingredientTags[i], Sickness.InfectionVector.Digestion);
			}
		}
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.juicer.specificEffect))
		{
			component2.Add(this.juicer.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.juicer.trackingEffect))
		{
			component2.Add(this.juicer.trackingEffect, true);
		}
	}

	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.juicer.trackingEffect) && component.HasEffect(this.juicer.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.juicer.specificEffect) && component.HasEffect(this.juicer.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	[MyCmpReq]
	private Operational operational;

	public int basePriority;

	private Juicer juicer;
}
