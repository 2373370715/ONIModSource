using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001039 RID: 4153
public class WarpPortal : Workable
{
	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x060054B7 RID: 21687 RVA: 0x000D7394 File Offset: 0x000D5594
	public bool ReadyToWarp
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied.waiting);
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x060054B8 RID: 21688 RVA: 0x000D73B6 File Offset: 0x000D55B6
	public bool IsWorking
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied);
		}
	}

	// Token: 0x060054B9 RID: 21689 RVA: 0x000D73D3 File Offset: 0x000D55D3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x0027C470 File Offset: 0x0027A670
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpPortalSMI = new WarpPortal.WarpPortalSM.Instance(this);
		this.warpPortalSMI.sm.isCharged.Set(!this.IsConsumed, this.warpPortalSMI, false);
		this.warpPortalSMI.StartSM();
		this.selectEventHandle = Game.Instance.Subscribe(-1503271301, new Action<object>(this.OnObjectSelected));
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x000D73F2 File Offset: 0x000D55F2
	private void OnObjectSelected(object data)
	{
		if (data != null && (GameObject)data == base.gameObject && Components.LiveMinionIdentities.Count > 0)
		{
			this.Discover();
		}
	}

	// Token: 0x060054BC RID: 21692 RVA: 0x000D741D File Offset: 0x000D561D
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(this.selectEventHandle);
		base.OnCleanUp();
	}

	// Token: 0x060054BD RID: 21693 RVA: 0x0027C4E4 File Offset: 0x0027A6E4
	private void Discover()
	{
		if (this.discovered)
		{
			return;
		}
		ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).SetDiscovered(true);
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.WarpWorldReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.minions = new GameObject[]
		{
			Components.LiveMinionIdentities[0].gameObject
		};
		statesInstance.callback = delegate()
		{
			ManagementMenu.Instance.OpenClusterMap();
			ClusterMapScreen.Instance.SetTargetFocusPosition(ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).GetMyWorldLocation(), 0.5f);
		};
		statesInstance.ShowEventPopup();
		this.discovered = true;
	}

	// Token: 0x060054BE RID: 21694 RVA: 0x000D7435 File Offset: 0x000D5635
	public void StartWarpSequence()
	{
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.occupied.warping);
	}

	// Token: 0x060054BF RID: 21695 RVA: 0x000D7457 File Offset: 0x000D5657
	public void CancelAssignment()
	{
		this.CancelChore();
		this.assignable.Unassign();
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.idle);
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x0027C574 File Offset: 0x0027A774
	private int GetTargetWorldID()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
		foreach (WarpReceiver component in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				return component.GetMyWorldId();
			}
		}
		global::Debug.LogError("No receiver world found for warp portal sender");
		return -1;
	}

	// Token: 0x060054C1 RID: 21697 RVA: 0x0027C5D0 File Offset: 0x0027A7D0
	private void Warp()
	{
		if (base.worker == null || base.worker.HasTag(GameTags.Dying) || base.worker.HasTag(GameTags.Dead))
		{
			return;
		}
		WarpReceiver warpReceiver = null;
		foreach (WarpReceiver warpReceiver2 in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (warpReceiver2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpReceiver = warpReceiver2;
				break;
			}
		}
		if (warpReceiver == null)
		{
			SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
			warpReceiver = UnityEngine.Object.FindObjectOfType<WarpReceiver>();
		}
		if (warpReceiver != null)
		{
			this.delayWarpRoutine = base.StartCoroutine(this.DelayedWarp(warpReceiver));
		}
		else
		{
			global::Debug.LogWarning("No warp receiver found - maybe POI stomping or failure to spawn?");
		}
		if (SelectTool.Instance.selected == base.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x060054C2 RID: 21698 RVA: 0x000D7485 File Offset: 0x000D5685
	public IEnumerator DelayedWarp(WarpReceiver receiver)
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		int myWorldId = base.worker.GetMyWorldId();
		int myWorldId2 = receiver.GetMyWorldId();
		CameraController.Instance.ActiveWorldStarWipe(myWorldId2, Grid.CellToPos(Grid.PosToCell(receiver)), 10f, null);
		WorkerBase worker = base.worker;
		worker.StopWork();
		receiver.ReceiveWarpedDuplicant(worker);
		ClusterManager.Instance.MigrateMinion(worker.GetComponent<MinionIdentity>(), myWorldId2, myWorldId);
		this.delayWarpRoutine = null;
		yield break;
	}

	// Token: 0x060054C3 RID: 21699 RVA: 0x000D749B File Offset: 0x000D569B
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x060054C4 RID: 21700 RVA: 0x000D74AF File Offset: 0x000D56AF
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x0027C6AC File Offset: 0x0027A8AC
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Migrate, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_warp_portal_sender_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.workLayer = Grid.SceneLayer.Building;
		this.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"idle_loop"
		};
		this.showProgressBar = false;
	}

	// Token: 0x060054C6 RID: 21702 RVA: 0x000D74C0 File Offset: 0x000D56C0
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
		if (this.delayWarpRoutine != null)
		{
			base.StopCoroutine(this.delayWarpRoutine);
			this.delayWarpRoutine = null;
		}
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x000D74FD File Offset: 0x000D56FD
	private void CompleteChore()
	{
		this.IsConsumed = true;
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x060054C8 RID: 21704 RVA: 0x000CC16A File Offset: 0x000CA36A
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x04003B6B RID: 15211
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04003B6C RID: 15212
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04003B6D RID: 15213
	private Chore chore;

	// Token: 0x04003B6E RID: 15214
	private WarpPortal.WarpPortalSM.Instance warpPortalSMI;

	// Token: 0x04003B6F RID: 15215
	private Notification notification;

	// Token: 0x04003B70 RID: 15216
	public const float RECHARGE_TIME = 3000f;

	// Token: 0x04003B71 RID: 15217
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04003B72 RID: 15218
	[Serialize]
	public float rechargeProgress;

	// Token: 0x04003B73 RID: 15219
	[Serialize]
	private bool discovered;

	// Token: 0x04003B74 RID: 15220
	private int selectEventHandle = -1;

	// Token: 0x04003B75 RID: 15221
	private Coroutine delayWarpRoutine;

	// Token: 0x04003B76 RID: 15222
	private static readonly HashedString[] printing_anim = new HashedString[]
	{
		"printing_pre",
		"printing_loop",
		"printing_pst"
	};

	// Token: 0x0200103A RID: 4154
	public class WarpPortalSM : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal>
	{
		// Token: 0x060054CD RID: 21709 RVA: 0x0027C790 File Offset: 0x0027A990
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				if (smi.master.rechargeProgress != 0f)
				{
					smi.GoTo(this.recharging);
				}
			}).DefaultState(this.idle);
			this.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.IsConsumed = false;
				smi.sm.isCharged.Set(true, smi, false);
				smi.master.SetAssignable(true);
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((WarpPortal.WarpPortalSM.Instance smi) => smi.master, this.become_occupied).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.IsFalse);
			this.become_occupied.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(smi.master.worker, smi);
				smi.GoTo(this.occupied.get_on);
			});
			this.occupied.OnTargetLost(this.worker, this.idle).Target(this.worker).TagTransition(GameTags.Dying, this.idle, false).Target(this.masterTarget).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(null, smi);
			});
			this.occupied.get_on.PlayAnim("sending_pre").OnAnimQueueComplete(this.occupied.waiting);
			this.occupied.waiting.PlayAnim("sending_loop", KAnim.PlayMode.Loop).ToggleNotification((WarpPortal.WarpPortalSM.Instance smi) => smi.CreateDupeWaitingNotification()).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			});
			this.occupied.warping.PlayAnim("sending_pst").OnAnimQueueComplete(this.do_warp);
			this.do_warp.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.Warp();
			}).GoTo(this.recharging);
			this.recharging.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
				smi.master.IsConsumed = true;
				this.isCharged.Set(false, smi, false);
			}).PlayAnim("recharge", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.WarpPortalCharging, (WarpPortal.WarpPortalSM.Instance smi) => smi.master).Update(delegate(WarpPortal.WarpPortalSM.Instance smi, float dt)
			{
				smi.master.rechargeProgress += dt;
				if (smi.master.rechargeProgress > 3000f)
				{
					this.isCharged.Set(true, smi, false);
					smi.master.rechargeProgress = 0f;
					smi.GoTo(this.idle);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x04003B77 RID: 15223
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State idle;

		// Token: 0x04003B78 RID: 15224
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State become_occupied;

		// Token: 0x04003B79 RID: 15225
		public WarpPortal.WarpPortalSM.OccupiedStates occupied;

		// Token: 0x04003B7A RID: 15226
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State do_warp;

		// Token: 0x04003B7B RID: 15227
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State recharging;

		// Token: 0x04003B7C RID: 15228
		public StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.BoolParameter isCharged;

		// Token: 0x04003B7D RID: 15229
		private StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.TargetParameter worker;

		// Token: 0x0200103B RID: 4155
		public class OccupiedStates : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State
		{
			// Token: 0x04003B7E RID: 15230
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State get_on;

			// Token: 0x04003B7F RID: 15231
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State waiting;

			// Token: 0x04003B80 RID: 15232
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State warping;
		}

		// Token: 0x0200103C RID: 4156
		public new class Instance : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.GameInstance
		{
			// Token: 0x060054D5 RID: 21717 RVA: 0x000D7631 File Offset: 0x000D5831
			public Instance(WarpPortal master) : base(master)
			{
			}

			// Token: 0x060054D6 RID: 21718 RVA: 0x0027CA94 File Offset: 0x0027AC94
			public Notification CreateDupeWaitingNotification()
			{
				if (base.master.worker != null)
				{
					return new Notification(MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.NAME.Replace("{dupe}", base.master.worker.name), NotificationType.Neutral, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.TOOLTIP.Replace("{dupe}", base.master.worker.name), null, false, 0f, null, null, base.master.transform, true, false, false);
				}
				return null;
			}
		}
	}
}
