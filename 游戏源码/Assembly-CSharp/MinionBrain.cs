using System;
using System.Collections.Generic;
using Klei.AI;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class MinionBrain : Brain
{
	// Token: 0x06001D1B RID: 7451 RVA: 0x001AE114 File Offset: 0x001AC314
	public bool IsCellClear(int cell)
	{
		if (Grid.Reserved[cell])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 0];
		return !(gameObject != null) || !(base.gameObject != gameObject) || gameObject.GetComponent<Navigator>().IsMoving();
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000B32E3 File Offset: 0x000B14E3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Navigator.SetAbilities(new MinionPathFinderAbilities(this.Navigator));
		base.Subscribe<MinionBrain>(-1697596308, MinionBrain.AnimTrackStoredItemDelegate);
		base.Subscribe<MinionBrain>(-975551167, MinionBrain.OnUnstableGroundImpactDelegate);
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x001AE16C File Offset: 0x001AC36C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject go in base.GetComponent<Storage>().items)
		{
			this.AddAnimTracker(go);
		}
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x001AE1E8 File Offset: 0x001AC3E8
	private void AnimTrackStoredItem(object data)
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = (GameObject)data;
		this.RemoveTracker(gameObject);
		if (component.items.Contains(gameObject))
		{
			this.AddAnimTracker(gameObject);
		}
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x001AE220 File Offset: 0x001AC420
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x001AE298 File Offset: 0x001AC498
	private void RemoveTracker(GameObject go)
	{
		KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x001AE2BC File Offset: 0x001AC4BC
	public override void UpdateBrain()
	{
		base.UpdateBrain();
		if (Game.Instance == null)
		{
			return;
		}
		if (!Game.Instance.savedInfo.discoveredSurface)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
			{
				Game.Instance.savedInfo.discoveredSurface = true;
				DiscoveredSpaceMessage message = new DiscoveredSpaceMessage(base.gameObject.transform.GetPosition());
				Messenger.Instance.QueueMessage(message);
				Game.Instance.Trigger(-818188514, base.gameObject);
			}
		}
		if (!Game.Instance.savedInfo.discoveredOilField)
		{
			int cell2 = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.OilField)
			{
				Game.Instance.savedInfo.discoveredOilField = true;
			}
		}
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x001AE394 File Offset: 0x001AC594
	private void RegisterReactEmotePair(string reactable_id, Emote emote, float max_trigger_time)
	{
		if (base.gameObject == null)
		{
			return;
		}
		ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
		if (smi != null)
		{
			EmoteChore emoteChore = new EmoteChore(base.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, emote, 1, null);
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.gameObject, reactable_id, Db.Get().ChoreTypes.Cough, max_trigger_time, 20f, float.PositiveInfinity, 0f);
			emoteChore.PairReactable(selfEmoteReactable);
			selfEmoteReactable.SetEmote(emote);
			selfEmoteReactable.PairEmote(emoteChore);
			smi.AddOneshotReactable(selfEmoteReactable);
		}
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x001AE430 File Offset: 0x001AC630
	private void OnResearchComplete(object data)
	{
		if (Time.time - this.lastResearchCompleteEmoteTime > 1f)
		{
			this.RegisterReactEmotePair("ResearchComplete", Db.Get().Emotes.Minion.ResearchComplete, 3f);
			this.lastResearchCompleteEmoteTime = Time.time;
		}
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x001AE480 File Offset: 0x001AC680
	public Notification CreateCollapseNotification()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		return new Notification(MISC.NOTIFICATIONS.TILECOLLAPSE.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.TILECOLLAPSE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x001AE4E0 File Offset: 0x001AC6E0
	public void RemoveCollapseNotification(Notification notification)
	{
		Vector3 position = notification.clickFocus.GetPosition();
		position.z = -40f;
		WorldContainer myWorld = notification.clickFocus.gameObject.GetMyWorld();
		if (myWorld != null && myWorld.IsDiscovered)
		{
			CameraController.Instance.ActiveWorldStarWipe(myWorld.id, position, 10f, null);
		}
		base.gameObject.AddOrGet<Notifier>().Remove(notification);
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x001AE550 File Offset: 0x001AC750
	private void OnUnstableGroundImpact(object data)
	{
		GameObject telepad = GameUtil.GetTelepad(base.gameObject.GetMyWorld().id);
		Navigator component = base.GetComponent<Navigator>();
		Assignable assignable = base.GetComponent<MinionIdentity>().GetSoleOwner().GetAssignable(Db.Get().AssignableSlots.Bed);
		bool flag = assignable != null && component.CanReach(Grid.PosToCell(assignable.transform.GetPosition()));
		bool flag2 = telepad != null && component.CanReach(Grid.PosToCell(telepad.transform.GetPosition()));
		if (!flag && !flag2)
		{
			this.RegisterReactEmotePair("UnstableGroundShock", Db.Get().Emotes.Minion.Shock, 1f);
			Notification notification = this.CreateCollapseNotification();
			notification.customClickCallback = delegate(object o)
			{
				this.RemoveCollapseNotification(notification);
			};
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x000B3323 File Offset: 0x000B1523
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x0400121E RID: 4638
	[MyCmpReq]
	public Navigator Navigator;

	// Token: 0x0400121F RID: 4639
	[MyCmpGet]
	public OxygenBreather OxygenBreather;

	// Token: 0x04001220 RID: 4640
	private float lastResearchCompleteEmoteTime;

	// Token: 0x04001221 RID: 4641
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> AnimTrackStoredItemDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.AnimTrackStoredItem(data);
	});

	// Token: 0x04001222 RID: 4642
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> OnUnstableGroundImpactDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.OnUnstableGroundImpact(data);
	});
}
