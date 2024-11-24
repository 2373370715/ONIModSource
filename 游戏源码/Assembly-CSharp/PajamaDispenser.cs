using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

public class PajamaDispenser : Workable, IDispenser
{
	[Serialize]
	private bool hasDispenseChore;

	private static GameObject pajamaPrefab = null;

	private WorkChore<PajamaDispenser> chore;

	private static List<Tag> PajamaList = new List<Tag> { "SleepClinicPajamas" };

	private WorkChore<PajamaDispenser> Chore
	{
		get
		{
			return chore;
		}
		set
		{
			chore = value;
			if (chore != null)
			{
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested);
			}
			else
			{
				base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, immediate: true);
			}
		}
	}

	public event System.Action OnStopWorkEvent;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (!(pajamaPrefab != null))
		{
			pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		Vector3 targetPoint = GetTargetPoint();
		targetPoint.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
		Util.KInstantiate(pajamaPrefab, targetPoint, Quaternion.identity).SetActive(value: true);
		hasDispenseChore = false;
	}

	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		if (Chore != null && Chore.smi.IsRunning())
		{
			Chore.Cancel("work interrupted");
		}
		Chore = null;
		if (hasDispenseChore)
		{
			FetchPajamas();
		}
		if (this.OnStopWorkEvent != null)
		{
			this.OnStopWorkEvent();
		}
	}

	[ContextMenu("fetch")]
	public void FetchPajamas()
	{
		if (Chore == null)
		{
			hasDispenseChore = true;
			Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, this, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false, null, is_preemptable: false, allow_in_context_menu: true, allow_prioritization: true, PriorityScreen.PriorityClass.basic, 5, ignore_building_assignment: false, add_to_daily_report: false);
			Chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot);
		}
	}

	public void CancelFetch()
	{
		if (Chore != null)
		{
			Chore.Cancel("User Cancelled");
			Chore = null;
			hasDispenseChore = false;
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested);
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (hasDispenseChore)
		{
			FetchPajamas();
		}
	}

	public List<Tag> DispensedItems()
	{
		return PajamaList;
	}

	public Tag SelectedItem()
	{
		return PajamaList[0];
	}

	public void SelectItem(Tag tag)
	{
	}

	public void OnOrderDispense()
	{
		FetchPajamas();
	}

	public void OnCancelDispense()
	{
		CancelFetch();
	}

	public bool HasOpenChore()
	{
		return Chore != null;
	}
}
