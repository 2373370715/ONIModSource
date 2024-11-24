using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class FossilHuntInitializer : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>
{
	public class Def : TraitDef
	{
		public const string LORE_UNLOCK_PREFIX = "story_trait_fossilhunt_";

		public bool IsMainDigsite;

		public override void Configure(GameObject prefab)
		{
			Story = Db.Get().Stories.FossilHunt;
			CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(1, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			InitalLoreId = "story_trait_fossilhunt_initial";
			EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.BUTTON,
				TextureName = "fossildigdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			CompleteLoreId = "story_trait_fossilhunt_complete";
			EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.BUTTON,
				TextureName = "fossildigmining_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}
	}

	public class ActiveState : State
	{
		public State inProgress;

		public State StoryComplete;
	}

	public new class Instance : TraitInstance
	{
		public string Title => CODEX.STORY_TRAITS.FOSSILHUNT.NAME;

		public string Description => CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION;

		public Instance(StateMachineController master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			base.gameObject.GetSMI<MajorFossilDigSite>();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance == null)
			{
				return;
			}
			if (base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				switch (storyInstance.CurrentState)
				{
				case StoryInstance.State.IN_PROGRESS:
					base.sm.wasStoryStarted.Set(value: true, this);
					break;
				case StoryInstance.State.COMPLETE:
					GoTo(base.sm.Active.StoryComplete);
					break;
				}
			}
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStoryStateChanged));
		}

		protected override void OnCleanUp()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStoryStateChanged));
			}
			base.OnCleanUp();
		}

		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state == StoryInstance.State.IN_PROGRESS)
			{
				base.sm.wasStoryStarted.Set(value: true, this);
			}
		}

		protected override void OnObjectSelect(object clicked)
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				RevealMajorFossilDigSites();
				RevealMinorFossilDigSites();
			}
			if ((bool)clicked)
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE && (storyInstance.PendingType != EventInfoDataHelper.PopupType.COMPLETE || base.def.IsMainDigsite))
				{
					OnNotificationClicked();
				}
				else if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
				{
					DisplayPopup(base.def.EventIntroInfo);
				}
			}
		}

		public override void OnPopupClosed()
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.COMPLETE))
			{
				TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}
			base.OnPopupClosed();
		}

		protected override void OnBuildingActivated(object activated)
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance != null && !base.sm.wasStoryStarted.Get(this) && storyInstance.CurrentState < StoryInstance.State.IN_PROGRESS)
			{
				RevealMinorFossilDigSites();
				RevealMajorFossilDigSites();
				base.OnBuildingActivated(activated);
			}
		}

		public void RevealMajorFossilDigSites()
		{
			RevealAll(8, "FossilDig");
		}

		public void RevealMinorFossilDigSites()
		{
			RevealAll(3, "FossilResin", "FossilIce", "FossilRock");
		}

		private void RevealAll(int radius, params Tag[] tags)
		{
			foreach (WorldGenSpawner.Spawnable item in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(includeSpawned: false, tags))
			{
				Grid.CellToXY(item.cell, out var x, out var y);
				GridVisibility.Reveal(x, y, radius, radius);
			}
		}

		public override void OnCompleteStorySequence()
		{
			if (base.def.IsMainDigsite)
			{
				base.OnCompleteStorySequence();
			}
		}

		public void ShowLoreUnlockedPopup(int popupID)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.NAME).AddDefaultOK();
			bool num = CodexCache.GetEntryForLock(FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(popupID)) != null;
			Option<string> option = FossilDigSiteConfig.GetBodyContentForFossil(popupID);
			if (num && option.HasValue)
			{
				infoDialogScreen.AddPlainText(option.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITFOSSILHUNT"));
			}
			else
			{
				infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
			}
		}

		public void ShowObjectiveCompletedNotification()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance == null)
			{
				return;
			}
			int objectivesCompleted = Mathf.RoundToInt(instance.CurrentProgress * (float)instance.CriteriaCount);
			if (objectivesCompleted == 0)
			{
				ShowFirstFossilExcavatedNotification();
				return;
			}
			string unlockID = FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(objectivesCompleted);
			Game.Instance.unlocks.Unlock(unlockID, shouldTryShowCodexNotification: false);
			ShowNotificationAndWaitForClick().Then(delegate
			{
				ShowLoreUnlockedPopup(objectivesCompleted);
			});
			Promise ShowNotificationAndWaitForClick()
			{
				return new Promise(delegate(System.Action resolve)
				{
					Notification notification = new Notification(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_NOTIFICATION.NAME, NotificationType.Event, (List<Notification> notifications, object obj) => CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_NOTIFICATION.TOOLTIP, null, expires: false, 0f, delegate
					{
						resolve();
					}, null, null, volume_attenuation: true, clear_on_click: true);
					base.gameObject.AddOrGet<Notifier>().Add(notification);
				});
			}
		}

		public void ShowFirstFossilExcavatedNotification()
		{
			ShowNotificationAndWaitForClick().Then(delegate
			{
				ShowQuestUnlockedPopup();
			});
			Promise ShowNotificationAndWaitForClick()
			{
				return new Promise(delegate(System.Action resolve)
				{
					Notification notification = new Notification(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.NAME, NotificationType.Event, (List<Notification> notifications, object obj) => CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.TOOLTIP, null, expires: false, 0f, delegate
					{
						resolve();
					}, null, null, volume_attenuation: true, clear_on_click: true);
					base.gameObject.AddOrGet<Notifier>().Add(notification);
				});
			}
		}

		public void ShowQuestUnlockedPopup()
		{
			LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.NAME).AddDefaultOK()
				.AddPlainText(((Option<string>)CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.DESCRIPTION.text).Value)
				.AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.CHECK_BUTTON, delegate(InfoDialogScreen dialog)
				{
					dialog.Deactivate();
					GameUtil.FocusCamera(base.transform);
				});
		}
	}

	private State Inactive;

	private ActiveState Active;

	public BoolParameter storyCompleted;

	public BoolParameter wasStoryStarted;

	public Signal CompleteStory;

	public const string LINK_OVERRIDE_PREFIX = "MOVECAMERATO";

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Inactive;
		base.serializable = SerializeType.ParamsOnly;
		Inactive.ParamTransition(storyCompleted, Active.StoryComplete, GameStateMachine<FossilHuntInitializer, Instance, StateMachineController, Def>.IsTrue).ParamTransition(wasStoryStarted, Active.inProgress, GameStateMachine<FossilHuntInitializer, Instance, StateMachineController, Def>.IsTrue);
		Active.inProgress.ParamTransition(storyCompleted, Active.StoryComplete, GameStateMachine<FossilHuntInitializer, Instance, StateMachineController, Def>.IsTrue).OnSignal(CompleteStory, Active.StoryComplete);
		Active.StoryComplete.Enter(CompleteStoryTrait);
	}

	public static bool OnUI_Quest_ObjectiveRowClicked(string rowLinkID)
	{
		rowLinkID = rowLinkID.ToUpper();
		if (rowLinkID.Contains("MOVECAMERATO"))
		{
			string text = rowLinkID.Replace("MOVECAMERATO", "");
			if (Components.MajorFossilDigSites.Count > 0 && CodexCache.FormatLinkID(Components.MajorFossilDigSites[0].gameObject.PrefabID().ToString()) == text)
			{
				GameUtil.FocusCamera(Components.MajorFossilDigSites[0].transform);
				return false;
			}
			foreach (MinorFossilDigSite.Instance minorFossilDigSite in Components.MinorFossilDigSites)
			{
				if (CodexCache.FormatLinkID(minorFossilDigSite.PrefabID().ToString()) == text)
				{
					CameraController.Instance.CameraGoTo(minorFossilDigSite.transform.GetPosition());
					SelectTool.Instance.Select(minorFossilDigSite.gameObject.GetComponent<KSelectable>());
					return false;
				}
			}
			return false;
		}
		return true;
	}

	public static void CompleteStoryTrait(Instance smi)
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
		if (storyInstance != null)
		{
			smi.sm.storyCompleted.Set(value: true, smi);
			if (!storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				smi.CompleteEvent();
			}
		}
	}

	public static string ResolveStrings_QuestObjectivesRowTooltips(string originalText, object obj)
	{
		return originalText + CODEX.STORY_TRAITS.FOSSILHUNT.QUEST.LINKED_TOOLTIP;
	}

	public static string ResolveQuestTitle(string title, QuestInstance quest)
	{
		int discoveredDigsitesRequired = FossilDigSiteConfig.DiscoveredDigsitesRequired;
		string text = Mathf.RoundToInt(quest.CurrentProgress * (float)discoveredDigsitesRequired) + "/" + discoveredDigsitesRequired;
		return title + " - " + text;
	}

	public static ICheckboxListGroupControl.ListGroup[] GetFossilHuntQuestData()
	{
		QuestInstance quest = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		ICheckboxListGroupControl.CheckboxItem[] checkBoxData = quest.GetCheckBoxData();
		for (int i = 0; i < checkBoxData.Length; i++)
		{
			checkBoxData[i].overrideLinkActions = OnUI_Quest_ObjectiveRowClicked;
			checkBoxData[i].resolveTooltipCallback = ResolveStrings_QuestObjectivesRowTooltips;
		}
		if (quest != null)
		{
			return new ICheckboxListGroupControl.ListGroup[1]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.FossilHuntQuest.Title, checkBoxData, (string title) => ResolveQuestTitle(title, quest))
			};
		}
		return new ICheckboxListGroupControl.ListGroup[0];
	}
}
