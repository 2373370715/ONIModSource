using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001692 RID: 5778
public class POITechItemUnlocks : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>
{
	// Token: 0x0600775C RID: 30556 RVA: 0x0030DB5C File Offset: 0x0030BD5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.locked;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.locked.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isUnlocked, this.unlocked, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.ParamTransition<bool>(this.seenNotification, this.unlocked.notify, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsFalse).ParamTransition<bool>(this.seenNotification, this.unlocked.done, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.notify.PlayAnim("notify", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null).ToggleNotification(delegate(POITechItemUnlocks.Instance smi)
		{
			smi.notificationReference = EventInfoScreen.CreateNotification(POITechItemUnlocks.GenerateEventPopupData(smi), null);
			smi.notificationReference.Type = NotificationType.MessageImportant;
			return smi.notificationReference;
		});
		this.unlocked.done.PlayAnim("off");
	}

	// Token: 0x0600775D RID: 30557 RVA: 0x0030DC44 File Offset: 0x0030BE44
	private static string GetMessageBody(POITechItemUnlocks.Instance smi)
	{
		string text = "";
		foreach (TechItem techItem in smi.unlockTechItems)
		{
			text = text + "\n    • " + techItem.Name;
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	// Token: 0x0600775E RID: 30558 RVA: 0x0030DCB8 File Offset: 0x0030BEB8
	private static EventInfoData GenerateEventPopupData(POITechItemUnlocks.Instance smi)
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, POITechItemUnlocks.GetMessageBody(smi), smi.def.animName);
		int num = Mathf.Max(2, Components.LiveMinionIdentities.Count);
		GameObject[] array = new GameObject[num];
		using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle<MinionIdentity>().GetEnumerator())
		{
			for (int i = 0; i < num; i++)
			{
				if (!enumerator.MoveNext())
				{
					num = 0;
					array = new GameObject[num];
					break;
				}
				array[i] = enumerator.Current.gameObject;
			}
		}
		eventInfoData.minions = array;
		if (smi.def.loreUnlockId != null)
		{
			eventInfoData.AddOption(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE, null).callback = delegate()
			{
				smi.sm.seenNotification.Set(true, smi, false);
				smi.notificationReference = null;
				Game.Instance.unlocks.Unlock(smi.def.loreUnlockId, true);
				ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId, false);
			};
		}
		eventInfoData.AddDefaultOption(delegate
		{
			smi.sm.seenNotification.Set(true, smi, false);
			smi.notificationReference = null;
		});
		eventInfoData.clickFocus = smi.gameObject.transform;
		return eventInfoData;
	}

	// Token: 0x04005936 RID: 22838
	public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State locked;

	// Token: 0x04005937 RID: 22839
	public POITechItemUnlocks.UnlockedStates unlocked;

	// Token: 0x04005938 RID: 22840
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter isUnlocked;

	// Token: 0x04005939 RID: 22841
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter pendingChore;

	// Token: 0x0400593A RID: 22842
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter seenNotification;

	// Token: 0x02001693 RID: 5779
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400593B RID: 22843
		public List<string> POITechUnlockIDs;

		// Token: 0x0400593C RID: 22844
		public LocString PopUpName;

		// Token: 0x0400593D RID: 22845
		public string animName;

		// Token: 0x0400593E RID: 22846
		public string loreUnlockId;
	}

	// Token: 0x02001694 RID: 5780
	public new class Instance : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06007761 RID: 30561 RVA: 0x0030DDE4 File Offset: 0x0030BFE4
		public Instance(IStateMachineTarget master, POITechItemUnlocks.Def def) : base(master, def)
		{
			this.unlockTechItems = new List<TechItem>(def.POITechUnlockIDs.Count);
			foreach (string text in def.POITechUnlockIDs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(text);
				if (techItem != null)
				{
					this.unlockTechItems.Add(techItem);
				}
				else
				{
					DebugUtil.DevAssert(false, "Invalid tech item " + text + " for POI Tech Unlock", null);
				}
			}
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x0030DE88 File Offset: 0x0030C088
		public override void StartSM()
		{
			base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			this.UpdateUnlocked();
			base.StartSM();
			if (base.sm.pendingChore.Get(this) && this.unlockChore == null)
			{
				this.CreateChore();
			}
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x000EE647 File Offset: 0x000EC847
		public override void StopSM(string reason)
		{
			base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			base.StopSM(reason);
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x0030DEDC File Offset: 0x0030C0DC
		public void OnBuildingSelect(object obj)
		{
			if (!(bool)obj)
			{
				return;
			}
			if (!base.sm.seenNotification.Get(this) && this.notificationReference != null)
			{
				this.notificationReference.customClickCallback(this.notificationReference.customClickData);
			}
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x000A5E40 File Offset: 0x000A4040
		private void ShowPopup()
		{
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x0030DF28 File Offset: 0x0030C128
		public void UnlockTechItems()
		{
			foreach (TechItem techItem in this.unlockTechItems)
			{
				if (techItem != null)
				{
					techItem.POIUnlocked();
				}
			}
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			this.UpdateUnlocked();
		}

		// Token: 0x06007767 RID: 30567 RVA: 0x0030DF94 File Offset: 0x0030C194
		private void UpdateUnlocked()
		{
			bool value = true;
			using (List<TechItem>.Enumerator enumerator = this.unlockTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						value = false;
						break;
					}
				}
			}
			base.sm.isUnlocked.Set(value, base.smi, false);
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06007768 RID: 30568 RVA: 0x0030E008 File Offset: 0x0030C208
		public string SidescreenButtonText
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06007769 RID: 30569 RVA: 0x0030E058 File Offset: 0x0030C258
		public string SidescreenButtonTooltip
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
			}
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x000EE667 File Offset: 0x000EC867
		public bool SidescreenEnabled()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x000EE667 File Offset: 0x000EC867
		public bool SidescreenButtonInteractable()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x0600776D RID: 30573 RVA: 0x0030E0A8 File Offset: 0x0030C2A8
		public void OnSidescreenButtonPressed()
		{
			if (this.unlockChore == null)
			{
				base.smi.sm.pendingChore.Set(true, base.smi, false);
				base.smi.CreateChore();
				return;
			}
			base.smi.sm.pendingChore.Set(false, base.smi, false);
			base.smi.CancelChore();
		}

		// Token: 0x0600776E RID: 30574 RVA: 0x0030E110 File Offset: 0x0030C310
		private void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<POITechItemUnlockWorkable>();
			Prioritizable.AddRef(base.gameObject);
			base.Trigger(1980521255, null);
			this.unlockChore = new WorkChore<POITechItemUnlockWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x0600776F RID: 30575 RVA: 0x000EE67F File Offset: 0x000EC87F
		private void CancelChore()
		{
			this.unlockChore.Cancel("UserCancel");
			this.unlockChore = null;
			Prioritizable.RemoveRef(base.gameObject);
			base.Trigger(1980521255, null);
		}

		// Token: 0x06007770 RID: 30576 RVA: 0x000ABC75 File Offset: 0x000A9E75
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06007771 RID: 30577 RVA: 0x000ABCBD File Offset: 0x000A9EBD
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x0400593F RID: 22847
		public List<TechItem> unlockTechItems;

		// Token: 0x04005940 RID: 22848
		public Notification notificationReference;

		// Token: 0x04005941 RID: 22849
		private Chore unlockChore;
	}

	// Token: 0x02001695 RID: 5781
	public class UnlockedStates : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State
	{
		// Token: 0x04005942 RID: 22850
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State notify;

		// Token: 0x04005943 RID: 22851
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State done;
	}
}
