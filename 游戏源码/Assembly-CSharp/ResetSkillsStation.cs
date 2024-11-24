using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000F5A RID: 3930
[AddComponentMenu("KMonoBehaviour/Workable/ResetSkillsStation")]
public class ResetSkillsStation : Workable
{
	// Token: 0x06004F96 RID: 20374 RVA: 0x000D3DB5 File Offset: 0x000D1FB5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x06004F97 RID: 20375 RVA: 0x000D3DC4 File Offset: 0x000D1FC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnAssign(this.assignable.assignee);
		this.assignable.OnAssign += this.OnAssign;
	}

	// Token: 0x06004F98 RID: 20376 RVA: 0x000D3DF4 File Offset: 0x000D1FF4
	private void OnAssign(IAssignableIdentity obj)
	{
		if (obj != null)
		{
			this.CreateChore();
			return;
		}
		if (this.chore != null)
		{
			this.chore.Cancel("Unassigned");
			this.chore = null;
		}
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x0026C64C File Offset: 0x0026A84C
	private void CreateChore()
	{
		this.chore = new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.UnlearnSkill, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x000D3E1F File Offset: 0x000D201F
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x0026C688 File Offset: 0x0026A888
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.assignable.Unassign();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null)
		{
			component.ResetSkillLevels(true);
			component.SetHats(component.CurrentHat, null);
			component.ApplyTargetHat();
			this.notification = new Notification(MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
			worker.GetComponent<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x000D3E51 File Offset: 0x000D2051
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
		this.chore = null;
	}

	// Token: 0x04003783 RID: 14211
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04003784 RID: 14212
	private Notification notification;

	// Token: 0x04003785 RID: 14213
	private Chore chore;
}
