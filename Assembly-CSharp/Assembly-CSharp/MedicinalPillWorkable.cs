﻿using System;
using Klei.AI;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		this.showProgressBar = false;
		this.synchronizeAnims = false;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		this.CreateChore();
	}

		protected override void OnCompleteWork(WorkerBase worker)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = worker.GetComponent<Effects>();
			EffectInstance effectInstance = component.Get(this.pill.info.effect);
			if (effectInstance != null)
			{
				effectInstance.timeRemaining = effectInstance.effect.duration;
			}
			else
			{
				component.Add(this.pill.info.effect, true);
			}
		}
		Sicknesses sicknesses = worker.GetSicknesses();
		foreach (string id in this.pill.info.curedSicknesses)
		{
			SicknessInstance sicknessInstance = sicknesses.Get(id);
			if (sicknessInstance != null)
			{
				Game.Instance.savedInfo.curedDisease = true;
				sicknessInstance.Cure();
			}
		}
		base.gameObject.DeleteObject();
	}

		private void CreateChore()
	{
		new TakeMedicineChore(this);
	}

		public bool CanBeTakenBy(GameObject consumer)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = consumer.GetComponent<Effects>();
			if (component == null || component.HasEffect(this.pill.info.effect))
			{
				return false;
			}
		}
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
		{
			return true;
		}
		Sicknesses sicknesses = consumer.GetSicknesses();
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
		{
			return true;
		}
		foreach (SicknessInstance sicknessInstance in sicknesses)
		{
			if (this.pill.info.curedSicknesses.Contains(sicknessInstance.modifier.Id))
			{
				return true;
			}
		}
		return false;
	}

			public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

			public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

			public int MajorOrder
	{
		get
		{
			return (int)(this.pill.info.medicineType + 1000);
		}
	}

			public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

			public bool Display
	{
		get
		{
			return true;
		}
	}

		public MedicinalPill pill;
}
