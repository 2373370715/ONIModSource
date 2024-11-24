using System;
using Database;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B86 RID: 15238
	public abstract class StoryTraitStateMachine<TStateMachine, TInstance, TDef> : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef> where TStateMachine : StoryTraitStateMachine<TStateMachine, TInstance, TDef> where TInstance : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitInstance where TDef : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitDef
	{
		// Token: 0x02003B87 RID: 15239
		public class TraitDef : StateMachine.BaseDef
		{
			// Token: 0x0400E5DB RID: 58843
			public string InitalLoreId;

			// Token: 0x0400E5DC RID: 58844
			public string CompleteLoreId;

			// Token: 0x0400E5DD RID: 58845
			public Story Story;

			// Token: 0x0400E5DE RID: 58846
			public StoryCompleteData CompletionData;

			// Token: 0x0400E5DF RID: 58847
			public StoryManager.PopupInfo EventIntroInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};

			// Token: 0x0400E5E0 RID: 58848
			public StoryManager.PopupInfo EventCompleteInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};
		}

		// Token: 0x02003B88 RID: 15240
		public class TraitInstance : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>.GameInstance
		{
			// Token: 0x0600EAB7 RID: 60087 RVA: 0x004CAB00 File Offset: 0x004C8D00
			public TraitInstance(StateMachineController master) : base(master)
			{
				StoryManager.Instance.ForceCreateStory(base.def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600EAB8 RID: 60088 RVA: 0x004CAB60 File Offset: 0x004C8D60
			public TraitInstance(StateMachineController master, TDef def) : base(master, def)
			{
				StoryManager.Instance.ForceCreateStory(def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600EAB9 RID: 60089 RVA: 0x004CABBC File Offset: 0x004C8DBC
			public override void StartSM()
			{
				this.selectable = base.GetComponent<KSelectable>();
				this.notifier = base.gameObject.AddOrGet<Notifier>();
				base.StartSM();
				base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				if (this.buildingActivatedHandle == -1)
				{
					this.buildingActivatedHandle = base.master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				}
				this.TriggerStoryEvent(StoryInstance.State.DISCOVERED);
			}

			// Token: 0x0600EABA RID: 60090 RVA: 0x0013CF14 File Offset: 0x0013B114
			public override void StopSM(string reason)
			{
				base.StopSM(reason);
				base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				base.Unsubscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				this.buildingActivatedHandle = -1;
			}

			// Token: 0x0600EABB RID: 60091 RVA: 0x004CAC38 File Offset: 0x004C8E38
			public void TriggerStoryEvent(StoryInstance.State storyEvent)
			{
				switch (storyEvent)
				{
				case StoryInstance.State.RETROFITTED:
				case StoryInstance.State.NOT_STARTED:
					return;
				case StoryInstance.State.DISCOVERED:
					StoryManager.Instance.DiscoverStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.IN_PROGRESS:
					StoryManager.Instance.BeginStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.COMPLETE:
				{
					Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.KeepSakeSpawnOffset), Grid.SceneLayer.Ore);
					StoryManager.Instance.CompleteStoryEvent(base.def.Story, keepsakeSpawnPosition);
					return;
				}
				default:
					throw new NotImplementedException(storyEvent.ToString());
				}
			}

			// Token: 0x0600EABC RID: 60092 RVA: 0x0013CF54 File Offset: 0x0013B154
			protected virtual void OnBuildingActivated(object activated)
			{
				if (!(bool)activated)
				{
					return;
				}
				this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}

			// Token: 0x0600EABD RID: 60093 RVA: 0x004CACF8 File Offset: 0x004C8EF8
			protected virtual void OnObjectSelect(object clicked)
			{
				if (!(bool)clicked)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE)
				{
					this.OnNotificationClicked(null);
					return;
				}
				if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
				{
					this.DisplayPopup(base.def.EventIntroInfo);
				}
			}

			// Token: 0x0600EABE RID: 60094 RVA: 0x004CAD78 File Offset: 0x004C8F78
			public virtual void CompleteEvent()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
				{
					return;
				}
				this.DisplayPopup(base.def.EventCompleteInfo);
			}

			// Token: 0x0600EABF RID: 60095 RVA: 0x0013CF66 File Offset: 0x0013B166
			public virtual void OnCompleteStorySequence()
			{
				this.TriggerStoryEvent(StoryInstance.State.COMPLETE);
			}

			// Token: 0x0600EAC0 RID: 60096 RVA: 0x004CADC8 File Offset: 0x004C8FC8
			protected void DisplayPopup(StoryManager.PopupInfo info)
			{
				if (info.PopupType == EventInfoDataHelper.PopupType.NONE)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.DisplayPopup(base.def.Story, info, new System.Action(this.OnPopupClosed), new Notification.ClickCallback(this.OnNotificationClicked));
				if (storyInstance != null && !info.DisplayImmediate)
				{
					this.selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
					this.notifier.Add(storyInstance.Notification, "");
				}
			}

			// Token: 0x0600EAC1 RID: 60097 RVA: 0x004CAE5C File Offset: 0x004C905C
			public void OnNotificationClicked(object data = null)
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				this.selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
				this.notifier.Remove(storyInstance.Notification);
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.COMPLETE)
				{
					this.ShowEventCompleteUI();
					return;
				}
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
				{
					this.ShowEventNormalUI();
					return;
				}
				this.ShowEventBeginUI();
			}

			// Token: 0x0600EAC2 RID: 60098 RVA: 0x004CAEE0 File Offset: 0x004C90E0
			public virtual void OnPopupClosed()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
				{
					Game.Instance.unlocks.Unlock(base.def.CompleteLoreId, true);
					return;
				}
				Game.Instance.unlocks.Unlock(base.def.InitalLoreId, true);
			}

			// Token: 0x0600EAC3 RID: 60099 RVA: 0x000A5E40 File Offset: 0x000A4040
			protected virtual void ShowEventBeginUI()
			{
			}

			// Token: 0x0600EAC4 RID: 60100 RVA: 0x000A5E40 File Offset: 0x000A4040
			protected virtual void ShowEventNormalUI()
			{
			}

			// Token: 0x0600EAC5 RID: 60101 RVA: 0x004CAF5C File Offset: 0x004C915C
			protected virtual void ShowEventCompleteUI()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(base.def.Story, base.master, new FocusTargetSequence.Data
				{
					WorldId = base.master.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = storyInstance.EventInfo,
					CompleteCB = new System.Action(this.OnCompleteStorySequence),
					CanCompleteCB = null
				});
			}

			// Token: 0x0400E5E1 RID: 58849
			protected int buildingActivatedHandle = -1;

			// Token: 0x0400E5E2 RID: 58850
			protected Notifier notifier;

			// Token: 0x0400E5E3 RID: 58851
			protected KSelectable selectable;
		}
	}
}
