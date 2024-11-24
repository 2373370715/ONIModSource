using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A08 RID: 6664
[AddComponentMenu("KMonoBehaviour/Workable/Uprootable")]
public class Uprootable : Workable, IDigActionEntity
{
	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06008AC6 RID: 35526 RVA: 0x000FAD57 File Offset: 0x000F8F57
	public bool IsMarkedForUproot
	{
		get
		{
			return this.isMarkedForUproot;
		}
	}

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x000FAD5F File Offset: 0x000F8F5F
	public Storage GetPlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
	}

	// Token: 0x06008AC8 RID: 35528 RVA: 0x0035D53C File Offset: 0x0035B73C
	protected Uprootable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.buttonLabel = UI.USERMENUACTIONS.UPROOT.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.UPROOT.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELUPROOT.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
	}

	// Token: 0x06008AC9 RID: 35529 RVA: 0x0035D5DC File Offset: 0x0035B7DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		base.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);
	}

	// Token: 0x06008ACA RID: 35530 RVA: 0x0035D690 File Offset: 0x0035B890
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
		base.SetWorkTime(12.5f);
		base.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
		base.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Components.Uprootables.Add(this);
		this.area = base.GetComponent<OccupyArea>();
		Prioritizable.AddRef(base.gameObject);
		base.gameObject.AddTag(GameTags.Plant);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForUproot)
		{
			this.MarkForUproot(true);
		}
	}

	// Token: 0x06008ACB RID: 35531 RVA: 0x0035D780 File Offset: 0x0035B980
	private void OnPlanterStorage(object data)
	{
		this.planterStorage = (Storage)data;
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.showIcon = (this.planterStorage == null);
		}
	}

	// Token: 0x06008ACC RID: 35532 RVA: 0x000FAD67 File Offset: 0x000F8F67
	public bool IsInPlanterBox()
	{
		return this.planterStorage != null;
	}

	// Token: 0x06008ACD RID: 35533 RVA: 0x0035D7BC File Offset: 0x0035B9BC
	public void Uproot()
	{
		this.isMarkedForUproot = false;
		this.chore = null;
		this.uprootComplete = true;
		base.Trigger(-216549700, this);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06008ACE RID: 35534 RVA: 0x000FAD75 File Offset: 0x000F8F75
	public void SetCanBeUprooted(bool state)
	{
		this.canBeUprooted = state;
		if (this.canBeUprooted)
		{
			this.SetUprootedComplete(false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06008ACF RID: 35535 RVA: 0x000FADA2 File Offset: 0x000F8FA2
	public void SetUprootedComplete(bool state)
	{
		this.uprootComplete = state;
	}

	// Token: 0x06008AD0 RID: 35536 RVA: 0x0035D838 File Offset: 0x0035BA38
	public void MarkForUproot(bool instantOnDebug = true)
	{
		if (!this.canBeUprooted)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Uproot();
		}
		else if (this.chore == null)
		{
			this.chore = new WorkChore<Uprootable>(Db.Get().ChoreTypes.Uproot, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
		this.isMarkedForUproot = true;
	}

	// Token: 0x06008AD1 RID: 35537 RVA: 0x000FADAB File Offset: 0x000F8FAB
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Uproot();
	}

	// Token: 0x06008AD2 RID: 35538 RVA: 0x0035D8B0 File Offset: 0x0035BAB0
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		}
		this.isMarkedForUproot = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06008AD3 RID: 35539 RVA: 0x000FADB3 File Offset: 0x000F8FB3
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06008AD4 RID: 35540 RVA: 0x000FADC0 File Offset: 0x000F8FC0
	private void OnClickUproot()
	{
		this.MarkForUproot(true);
	}

	// Token: 0x06008AD5 RID: 35541 RVA: 0x000FADC9 File Offset: 0x000F8FC9
	protected void OnClickCancelUproot()
	{
		this.OnCancel(null);
	}

	// Token: 0x06008AD6 RID: 35542 RVA: 0x000FADC9 File Offset: 0x000F8FC9
	public virtual void ForceCancelUproot(object data = null)
	{
		this.OnCancel(null);
	}

	// Token: 0x06008AD7 RID: 35543 RVA: 0x0035D914 File Offset: 0x0035BB14
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		if (this.uprootComplete)
		{
			if (this.deselectOnUproot)
			{
				KSelectable component = base.GetComponent<KSelectable>();
				if (component != null && SelectTool.Instance.selected == component)
				{
					SelectTool.Instance.Select(null, false);
				}
			}
			return;
		}
		if (!this.canBeUprooted)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_uproot", this.cancelButtonLabel, new System.Action(this.OnClickCancelUproot), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_uproot", this.buttonLabel, new System.Action(this.OnClickUproot), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06008AD8 RID: 35544 RVA: 0x000FADD2 File Offset: 0x000F8FD2
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Components.Uprootables.Remove(this);
	}

	// Token: 0x06008AD9 RID: 35545 RVA: 0x000FADF5 File Offset: 0x000F8FF5
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
	}

	// Token: 0x06008ADA RID: 35546 RVA: 0x000FADAB File Offset: 0x000F8FAB
	public void Dig()
	{
		this.Uproot();
	}

	// Token: 0x06008ADB RID: 35547 RVA: 0x000FAE1A File Offset: 0x000F901A
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForUproot(instantOnDebug);
	}

	// Token: 0x04006880 RID: 26752
	[Serialize]
	protected bool isMarkedForUproot;

	// Token: 0x04006881 RID: 26753
	protected bool uprootComplete;

	// Token: 0x04006882 RID: 26754
	[MyCmpReq]
	private Prioritizable prioritizable;

	// Token: 0x04006883 RID: 26755
	[Serialize]
	protected bool canBeUprooted = true;

	// Token: 0x04006884 RID: 26756
	public bool deselectOnUproot = true;

	// Token: 0x04006885 RID: 26757
	protected Chore chore;

	// Token: 0x04006886 RID: 26758
	private string buttonLabel;

	// Token: 0x04006887 RID: 26759
	private string buttonTooltip;

	// Token: 0x04006888 RID: 26760
	private string cancelButtonLabel;

	// Token: 0x04006889 RID: 26761
	private string cancelButtonTooltip;

	// Token: 0x0400688A RID: 26762
	private StatusItem pendingStatusItem;

	// Token: 0x0400688B RID: 26763
	public OccupyArea area;

	// Token: 0x0400688C RID: 26764
	private Storage planterStorage;

	// Token: 0x0400688D RID: 26765
	public bool showUserMenuButtons = true;

	// Token: 0x0400688E RID: 26766
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400688F RID: 26767
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnPlanterStorage(data);
	});

	// Token: 0x04006890 RID: 26768
	private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.ForceCancelUproot(data);
	});

	// Token: 0x04006891 RID: 26769
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04006892 RID: 26770
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
