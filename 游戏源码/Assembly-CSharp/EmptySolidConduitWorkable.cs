using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
	[MyCmpReq]
	private SolidConduit conduit;

	private static StatusItem emptySolidConduitStatusItem;

	private Chore chore;

	private const float RECHECK_PIPE_INTERVAL = 2f;

	private const float TIME_TO_EMPTY_PIPE = 4f;

	private const float NO_EMPTY_SCHEDULED = -1f;

	[Serialize]
	private float elapsedTime = -1f;

	private bool emptiedPipe = true;

	private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>(delegate(EmptySolidConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SetOffsetTable(OffsetGroups.InvertedStandardTable);
		SetWorkTime(float.PositiveInfinity);
		faceTargetWhenWorking = true;
		multitoolContext = "build";
		multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		Subscribe(2127324410, OnEmptyConduitCancelledDelegate);
		if (emptySolidConduitStatusItem == null)
		{
			emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.SolidConveyor.ID, 32770);
		}
		requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		shouldShowSkillPerkStatusItem = false;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (elapsedTime != -1f)
		{
			MarkForEmptying();
		}
	}

	public void MarkForEmptying()
	{
		if (chore == null && HasContents())
		{
			StatusItem statusItem = GetStatusItem();
			GetComponent<KSelectable>().ToggleStatusItem(statusItem, on: true);
			CreateWorkChore();
		}
	}

	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
	}

	private void CancelEmptying()
	{
		CleanUpVisualization();
		if (chore != null)
		{
			chore.Cancel("Cancel");
			chore = null;
			shouldShowSkillPerkStatusItem = false;
			UpdateStatusItem();
		}
	}

	private void CleanUpVisualization()
	{
		StatusItem statusItem = GetStatusItem();
		KSelectable component = GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, on: false);
		}
		elapsedTime = -1f;
		if (chore != null)
		{
			GetComponent<Prioritizable>().RemoveRef();
		}
	}

	protected override void OnCleanUp()
	{
		CancelEmptying();
		base.OnCleanUp();
	}

	private SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	private void OnEmptyConduitCancelled(object data)
	{
		CancelEmptying();
	}

	private StatusItem GetStatusItem()
	{
		return emptySolidConduitStatusItem;
	}

	private void CreateWorkChore()
	{
		GetComponent<Prioritizable>().AddRef();
		chore = new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
		chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		elapsedTime = 0f;
		emptiedPipe = false;
		shouldShowSkillPerkStatusItem = true;
		UpdateStatusItem();
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		elapsedTime += dt;
		if (!emptiedPipe)
		{
			if (elapsedTime > 4f)
			{
				EmptyContents();
				emptiedPipe = true;
				elapsedTime = 0f;
			}
		}
		else if (elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
			{
				elapsedTime = 0f;
				emptiedPipe = false;
			}
			else
			{
				CleanUpVisualization();
				chore = null;
				result = true;
				shouldShowSkillPerkStatusItem = false;
				UpdateStatusItem();
			}
		}
		return result;
	}

	public override bool InstantlyFinish(Worker worker)
	{
		worker.Work(4f);
		return true;
	}

	public void EmptyContents()
	{
		int cell_idx = Grid.PosToCell(base.transform.GetPosition());
		GetFlowManager().RemovePickupable(cell_idx);
		elapsedTime = 0f;
	}

	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(elapsedTime / 4f);
	}
}
