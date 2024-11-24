using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200099F RID: 2463
[AddComponentMenu("KMonoBehaviour/Workable/Breakable")]
public class Breakable : Workable
{
	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06002CCA RID: 11466 RVA: 0x000BD016 File Offset: 0x000BB216
	public bool IsInvincible
	{
		get
		{
			return this.hp == null || this.hp.invincible;
		}
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000BD033 File Offset: 0x000BB233
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_break_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x000BD06B File Offset: 0x000BB26B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Breakables.Add(this);
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x000BD07E File Offset: 0x000BB27E
	public bool isBroken()
	{
		return this.hp == null || this.hp.HitPoints <= 0;
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x001ECC70 File Offset: 0x001EAE70
	public Notification CreateDamageNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION, NotificationType.BadMinor, (List<Notification> notificationList, object data) => BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x001ECCC8 File Offset: 0x001EAEC8
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x001ECD30 File Offset: 0x001EAF30
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.secondsPerTenPercentDamage = 2f;
		this.tenPercentDamage = Mathf.CeilToInt((float)this.hp.MaxHitPoints * 0.1f);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
		this.notification = this.CreateDamageNotification();
		base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		this.elapsedDamageTime = 0f;
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x001ECDBC File Offset: 0x001EAFBC
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedDamageTime >= this.secondsPerTenPercentDamage)
		{
			this.elapsedDamageTime -= this.elapsedDamageTime;
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = this.tenPercentDamage,
				source = BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION
			});
		}
		this.elapsedDamageTime += dt;
		return this.hp.HitPoints <= 0;
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x001ECE54 File Offset: 0x001EB054
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AngerDamage, false);
		base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		if (worker != null)
		{
			worker.Trigger(-1734580852, null);
		}
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000BD0A1 File Offset: 0x000BB2A1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Breakables.Remove(this);
	}

	// Token: 0x04001E16 RID: 7702
	private const float TIME_TO_BREAK_AT_FULL_HEALTH = 20f;

	// Token: 0x04001E17 RID: 7703
	private Notification notification;

	// Token: 0x04001E18 RID: 7704
	private float secondsPerTenPercentDamage = float.PositiveInfinity;

	// Token: 0x04001E19 RID: 7705
	private float elapsedDamageTime;

	// Token: 0x04001E1A RID: 7706
	private int tenPercentDamage = int.MaxValue;

	// Token: 0x04001E1B RID: 7707
	[MyCmpGet]
	private BuildingHP hp;
}
