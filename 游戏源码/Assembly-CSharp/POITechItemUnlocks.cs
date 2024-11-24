using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class POITechItemUnlocks : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>
{
	public class Def : BaseDef
	{
		public List<string> POITechUnlockIDs;

		public LocString PopUpName;

		public string animName;

		public string loreUnlockId;
	}

	public new class Instance : GameInstance, ISidescreenButtonControl
	{
		public List<TechItem> unlockTechItems;

		public Notification notificationReference;

		private Chore unlockChore;

		public string SidescreenButtonText
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;
				}
				if (unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
			}
		}

		public string SidescreenButtonTooltip
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;
				}
				if (unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			unlockTechItems = new List<TechItem>(def.POITechUnlockIDs.Count);
			foreach (string pOITechUnlockID in def.POITechUnlockIDs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(pOITechUnlockID);
				if (techItem != null)
				{
					unlockTechItems.Add(techItem);
				}
				else
				{
					DebugUtil.DevAssert(test: false, "Invalid tech item " + pOITechUnlockID + " for POI Tech Unlock");
				}
			}
		}

		public override void StartSM()
		{
			Subscribe(-1503271301, OnBuildingSelect);
			UpdateUnlocked();
			base.StartSM();
			if (base.sm.pendingChore.Get(this) && unlockChore == null)
			{
				CreateChore();
			}
		}

		public override void StopSM(string reason)
		{
			Unsubscribe(-1503271301, OnBuildingSelect);
			base.StopSM(reason);
		}

		public void OnBuildingSelect(object obj)
		{
			if ((bool)obj && !base.sm.seenNotification.Get(this) && notificationReference != null)
			{
				notificationReference.customClickCallback(notificationReference.customClickData);
			}
		}

		private void ShowPopup()
		{
		}

		public void UnlockTechItems()
		{
			foreach (TechItem unlockTechItem in unlockTechItems)
			{
				unlockTechItem?.POIUnlocked();
			}
			MusicManager.instance.PlaySong("Stinger_ResearchComplete");
			UpdateUnlocked();
		}

		private void UpdateUnlocked()
		{
			bool value = true;
			foreach (TechItem unlockTechItem in unlockTechItems)
			{
				if (!unlockTechItem.IsComplete())
				{
					value = false;
					break;
				}
			}
			base.sm.isUnlocked.Set(value, base.smi);
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		public bool SidescreenEnabled()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		public bool SidescreenButtonInteractable()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		public void OnSidescreenButtonPressed()
		{
			if (unlockChore == null)
			{
				base.smi.sm.pendingChore.Set(value: true, base.smi);
				base.smi.CreateChore();
			}
			else
			{
				base.smi.sm.pendingChore.Set(value: false, base.smi);
				base.smi.CancelChore();
			}
		}

		private void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<POITechItemUnlockWorkable>();
			Prioritizable.AddRef(base.gameObject);
			Trigger(1980521255);
			unlockChore = new WorkChore<POITechItemUnlockWorkable>(Db.Get().ChoreTypes.Research, component);
		}

		private void CancelChore()
		{
			unlockChore.Cancel("UserCancel");
			unlockChore = null;
			Prioritizable.RemoveRef(base.gameObject);
			Trigger(1980521255);
		}

		public int HorizontalGroupID()
		{
			return -1;
		}

		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}
	}

	public class UnlockedStates : State
	{
		public State notify;

		public State done;
	}

	public State locked;

	public UnlockedStates unlocked;

	public BoolParameter isUnlocked;

	public BoolParameter pendingChore;

	public BoolParameter seenNotification;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = locked;
		base.serializable = SerializeType.ParamsOnly;
		locked.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition(isUnlocked, unlocked, GameStateMachine<POITechItemUnlocks, Instance, IStateMachineTarget, Def>.IsTrue);
		unlocked.ParamTransition(seenNotification, unlocked.notify, GameStateMachine<POITechItemUnlocks, Instance, IStateMachineTarget, Def>.IsFalse).ParamTransition(seenNotification, unlocked.done, GameStateMachine<POITechItemUnlocks, Instance, IStateMachineTarget, Def>.IsTrue);
		unlocked.notify.PlayAnim("notify", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired).ToggleNotification(delegate(Instance smi)
		{
			smi.notificationReference = EventInfoScreen.CreateNotification(GenerateEventPopupData(smi));
			smi.notificationReference.Type = NotificationType.MessageImportant;
			return smi.notificationReference;
		});
		unlocked.done.PlayAnim("off");
	}

	private static string GetMessageBody(Instance smi)
	{
		string text = "";
		foreach (TechItem unlockTechItem in smi.unlockTechItems)
		{
			text = text + "\n    • " + unlockTechItem.Name;
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	private static EventInfoData GenerateEventPopupData(Instance smi)
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, GetMessageBody(smi), smi.def.animName);
		int num = Mathf.Max(2, Components.LiveMinionIdentities.Count);
		GameObject[] array = new GameObject[num];
		using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle().GetEnumerator())
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
			eventInfoData.AddOption(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE).callback = delegate
			{
				smi.sm.seenNotification.Set(value: true, smi);
				smi.notificationReference = null;
				Game.Instance.unlocks.Unlock(smi.def.loreUnlockId);
				ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId);
			};
		}
		eventInfoData.AddDefaultOption(delegate
		{
			smi.sm.seenNotification.Set(value: true, smi);
			smi.notificationReference = null;
		});
		eventInfoData.clickFocus = smi.gameObject.transform;
		return eventInfoData;
	}
}
