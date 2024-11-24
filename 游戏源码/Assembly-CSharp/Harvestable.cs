using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020013E0 RID: 5088
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Harvestable")]
public class Harvestable : Workable
{
	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06006867 RID: 26727 RVA: 0x000E4627 File Offset: 0x000E2827
	// (set) Token: 0x06006868 RID: 26728 RVA: 0x000E462F File Offset: 0x000E282F
	public WorkerBase completed_by { get; protected set; }

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06006869 RID: 26729 RVA: 0x000E4638 File Offset: 0x000E2838
	public bool CanBeHarvested
	{
		get
		{
			return this.canBeHarvested;
		}
	}

	// Token: 0x0600686A RID: 26730 RVA: 0x000E4640 File Offset: 0x000E2840
	protected Harvestable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x0600686B RID: 26731 RVA: 0x000E4668 File Offset: 0x000E2868
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x0600686C RID: 26732 RVA: 0x002D6F58 File Offset: 0x002D5158
	protected override void OnSpawn()
	{
		this.harvestDesignatable = base.GetComponent<HarvestDesignatable>();
		base.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
		base.SetWorkTime(10f);
		base.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
		this.faceTargetWhenWorking = true;
		Components.Harvestables.Add(this);
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x0600686D RID: 26733 RVA: 0x000E46A5 File Offset: 0x000E28A5
	public void OnUprooted(object data)
	{
		if (this.canBeHarvested)
		{
			this.Harvest();
		}
	}

	// Token: 0x0600686E RID: 26734 RVA: 0x002D6FF8 File Offset: 0x002D51F8
	public void Harvest()
	{
		this.harvestDesignatable.MarkedForHarvest = false;
		this.chore = null;
		base.Trigger(1272413801, this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600686F RID: 26735 RVA: 0x002D706C File Offset: 0x002D526C
	public void OnMarkedForHarvest()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.chore == null)
		{
			this.chore = new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, this);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
	}

	// Token: 0x06006870 RID: 26736 RVA: 0x002D70E4 File Offset: 0x002D52E4
	public void SetCanBeHarvested(bool state)
	{
		this.canBeHarvested = state;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.canBeHarvested)
		{
			component.AddStatusItem(this.readyForHarvestStatusItem, null);
			if (this.harvestDesignatable.HarvestWhenReady)
			{
				this.harvestDesignatable.MarkForHarvest();
			}
			else if (this.harvestDesignatable.InPlanterBox)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		else
		{
			component.RemoveStatusItem(this.readyForHarvestStatusItem, false);
			component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06006871 RID: 26737 RVA: 0x000E46B5 File Offset: 0x000E28B5
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.completed_by = worker;
		this.Harvest();
	}

	// Token: 0x06006872 RID: 26738 RVA: 0x002D7190 File Offset: 0x002D5390
	protected virtual void OnCancel(object data)
	{
		bool flag = data == null || (data is bool && !(bool)data);
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel harvest");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
			if (flag)
			{
				this.harvestDesignatable.SetHarvestWhenReady(false);
			}
		}
		if (flag)
		{
			this.harvestDesignatable.MarkedForHarvest = false;
		}
	}

	// Token: 0x06006873 RID: 26739 RVA: 0x000E46C4 File Offset: 0x000E28C4
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x000E46D1 File Offset: 0x000E28D1
	public virtual void ForceCancelHarvest(object data = null)
	{
		this.OnCancel(data);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06006875 RID: 26741 RVA: 0x000E470B File Offset: 0x000E290B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Harvestables.Remove(this);
	}

	// Token: 0x06006876 RID: 26742 RVA: 0x000E471E File Offset: 0x000E291E
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
	}

	// Token: 0x04004EB6 RID: 20150
	public StatusItem readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest;

	// Token: 0x04004EB7 RID: 20151
	public HarvestDesignatable harvestDesignatable;

	// Token: 0x04004EB8 RID: 20152
	[Serialize]
	protected bool canBeHarvested;

	// Token: 0x04004EBA RID: 20154
	protected Chore chore;

	// Token: 0x04004EBB RID: 20155
	private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.ForceCancelHarvest(data);
	});

	// Token: 0x04004EBC RID: 20156
	private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.OnCancel(data);
	});
}
