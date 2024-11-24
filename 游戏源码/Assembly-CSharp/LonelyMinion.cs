using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class LonelyMinion : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>
{
	public class Def : BaseDef
	{
		public Personality Personality;

		public HashedString QuestOwnerId;

		public Extents DecorInspectionArea;
	}

	public new class Instance : GameInstance
	{
		public SchedulerHandle ResetHandle;

		public float StartingAverageDecor = float.NegativeInfinity;

		public float IdleDelayTimer;

		private KBatchedAnimController[] animControllers;

		private Storage storage;

		private const int maxIdles = 8;

		private List<HashedString> availableIdles = new List<HashedString>(8);

		public KBatchedAnimController AnimController => animControllers[0];

		public KBatchedAnimController PackageSnapPoint => animControllers[1];

		public Instance(StateMachineController master, Def def)
			: base(master, def)
		{
			animControllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>(includeInactive: true);
			storage = GetComponent<Storage>();
			Debug.Assert(def.Personality != null);
			GetComponent<Accessorizer>().ApplyMinionPersonality(def.Personality);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStoryStateChanged));
		}

		public override void StartSM()
		{
			LonelyMinionHouse.Instance sMI = base.smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			base.smi.AnimController.GetSynchronizer().Add(sMI.AnimController);
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(ShowQuestCompleteNotification));
			base.smi.IdleDelayTimer = UnityEngine.Random.Range(20f, 40f);
			InitializeIdles();
			base.StartSM();
		}

		public override void StopSM(string reason)
		{
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(ShowQuestCompleteNotification));
			StoryCleanUp();
			base.StopSM(reason);
			ResetHandle.ClearScheduler();
			ResetHandle.FreeResources();
		}

		public HashedString ChooseIdle()
		{
			if (availableIdles.Count > 1)
			{
				availableIdles.Shuffle();
			}
			return availableIdles[0];
		}

		public void Pickup(Pickupable pickupable, bool store)
		{
			base.sm.Mail.Set(null, this, silenceEvents: true);
			pickupable.storage.GetComponent<SingleEntityReceptacle>().OrderRemoveOccupant();
			PackageSnapPoint.Play("object", KAnim.PlayMode.Loop);
			if (store)
			{
				storage.Store(pickupable.gameObject, hide_popups: true, block_events: true, do_disease_transfer: false);
			}
			else
			{
				UnityEngine.Object.Destroy(pickupable.gameObject);
			}
		}

		public void Drop()
		{
			storage.DropAll(PackageSnapPoint.transform.position);
		}

		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state == StoryInstance.State.COMPLETE)
			{
				StoryCleanUp();
			}
		}

		private void StoryCleanUp()
		{
			AnimController.GetSynchronizer().Clear();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStoryStateChanged));
		}

		private void InitializeIdles()
		{
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			if (instance.IsStarted)
			{
				availableIdles.Add(LonelyMinionConfig.FOOD_IDLE);
				if (!instance.IsComplete)
				{
					availableIdles.Add(LonelyMinionConfig.CHECK_MAIL);
				}
			}
			instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			if (instance.IsStarted)
			{
				availableIdles.Add(LonelyMinionConfig.DECOR_IDLE);
			}
			instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			if (instance.IsStarted)
			{
				availableIdles.Add(LonelyMinionConfig.POWER_IDLE);
			}
			LonelyMinionHouse.Instance sMI = base.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			LonelyMinionHouse lonelyMinionHouse = sMI.GetStateMachine() as LonelyMinionHouse;
			float num = 3f * lonelyMinionHouse.QuestProgress.Get(sMI);
			int num2 = (Mathf.Approximately(Mathf.CeilToInt(num), num) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num));
			if (num2 == 0)
			{
				availableIdles.Add(LonelyMinionConfig.BLINDS_IDLE_0);
				return;
			}
			for (int i = 1; i <= num2 && i != 3; i++)
			{
				availableIdles.Add(string.Format("{0}_{1}", "idle_blinds", i));
			}
		}

		public void UnlockQuestIdle(QuestInstance quest, Quest.State prevState, float delta)
		{
			if (prevState == Quest.State.NotStarted && quest.IsStarted)
			{
				if (quest.Id == Db.Get().Quests.LonelyMinionFoodQuest.IdHash)
				{
					availableIdles.Add(LonelyMinionConfig.FOOD_IDLE);
				}
				else if (quest.Id == Db.Get().Quests.LonelyMinionDecorQuest.IdHash)
				{
					availableIdles.Add(LonelyMinionConfig.DECOR_IDLE);
				}
				else
				{
					availableIdles.Add(LonelyMinionConfig.POWER_IDLE);
				}
			}
			if (quest.IsComplete)
			{
				if (quest.Id == Db.Get().Quests.LonelyMinionFoodQuest.IdHash)
				{
					availableIdles.Remove(LonelyMinionConfig.CHECK_MAIL);
				}
				LonelyMinionHouse.Instance sMI = base.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
				LonelyMinionHouse lonelyMinionHouse = sMI.GetStateMachine() as LonelyMinionHouse;
				float num = 3f * lonelyMinionHouse.QuestProgress.Get(sMI);
				int num2 = (Mathf.Approximately(Mathf.CeilToInt(num), num) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num));
				if (num2 > 0 && num2 < 3)
				{
					availableIdles.Add(string.Format("{0}_{1}", "idle_blinds", num2));
				}
				availableIdles.Remove(LonelyMinionConfig.BLINDS_IDLE_0);
			}
		}

		public void ShowQuestCompleteNotification(QuestInstance quest, Quest.State prevState, float delta = 0f)
		{
			if (quest.IsComplete)
			{
				string text = string.Empty;
				if (quest.Id != Db.Get().Quests.LonelyMinionGreetingQuest.IdHash)
				{
					text = "story_trait_lonelyminion_" + quest.Name.ToLower();
					Game.Instance.unlocks.Unlock(text, shouldTryShowCodexNotification: false);
				}
				Notification notification = new Notification(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.NAME, NotificationType.Event, null, null, expires: false, 0f, ShowQuestCompletePopup, new Tuple<string, string>(text, quest.CompletionText), null, volume_attenuation: true, clear_on_click: true, show_dismiss_button: true);
				base.transform.parent.gameObject.AddOrGet<Notifier>().Add(notification);
			}
		}

		private void ShowQuestCompletePopup(object data)
		{
			Tuple<string, string> tuple = data as Tuple<string, string>;
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.NAME).AddPlainText(tuple.second)
				.AddDefaultOK();
			if (!string.IsNullOrEmpty(tuple.first))
			{
				infoDialogScreen.AddOption(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByLockKeyID(tuple.first, focusContent: true));
			}
		}
	}

	public class MailStates : State
	{
		public State Success;

		public State Failure;

		public State Duplicate;

		public static void OnEnter(Instance smi)
		{
			KBatchedAnimController component = smi.sm.Mail.Get(smi).GetComponent<KBatchedAnimController>();
			smi.PackageSnapPoint.gameObject.SetActive(component.gameObject != smi.gameObject);
			if (smi.PackageSnapPoint.gameObject.activeSelf)
			{
				smi.PackageSnapPoint.SwapAnims(component.AnimFiles);
			}
			smi.AnimController.Play(LonelyMinionConfig.CHECK_MAIL);
		}

		public static void OnExit(Instance smi)
		{
			smi.ResetHandle = smi.ScheduleNextFrame(ResetState, smi);
		}

		private static void ResetState(object data)
		{
			Instance instance = data as Instance;
			instance.AnimController.Play(instance.AnimController.initialAnim, instance.AnimController.initialMode);
			instance.Drop();
		}

		public static void PlayAnims(Instance smi, HashedString anim)
		{
			if (anim.IsValid)
			{
				smi.AnimController.Queue(anim);
			}
			else
			{
				smi.GoTo(smi.sm.Idle);
			}
		}
	}

	public TargetParameter Mail;

	public BoolParameter Active;

	public State Idle;

	public State Inactive;

	public MailStates CheckMail;

	private bool HahCheckedMail(Instance smi)
	{
		if (smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL)
		{
			if (Mail.Get(smi) == smi.gameObject)
			{
				Mail.Set(null, smi, silenceEvents: true);
				smi.AnimController.Play(LonelyMinionConfig.CHECK_MAIL_FAILURE);
				return false;
			}
			CheckForMail(smi);
			return false;
		}
		if (smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_FAILURE || smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_DUPLICATE)
		{
			smi.Drop();
			return false;
		}
		if (!(smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_FAILURE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_SUCCESS))
		{
			return smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_DUPLICATE;
		}
		return true;
	}

	private void CheckForMail(Instance smi)
	{
		Tag prefabTag = Mail.Get(smi).GetComponent<KPrefabID>().PrefabTag;
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		Debug.Assert(instance != null);
		Quest.ItemData data2 = new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.FoodCriteriaId,
			SatisfyingItem = prefabTag,
			QualifyingTag = GameTags.Edible,
			CurrentValue = EdiblesManager.GetFoodInfo(prefabTag.Name).Quality
		};
		MailStates mailStates = smi.GetCurrentState() as MailStates;
		instance.TrackProgress(data2, out var dataSatisfies, out var itemIsRedundant);
		BaseState baseState = mailStates.Success;
		string title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.TASTYFOOD.NAME;
		string tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.TASTYFOOD.TOOLTIP;
		if (itemIsRedundant)
		{
			baseState = mailStates.Duplicate;
			title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.REPEATEDFOOD.NAME;
			tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.REPEATEDFOOD.TOOLTIP;
		}
		else if (!dataSatisfies)
		{
			baseState = mailStates.Failure;
			title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.CRAPPYFOOD.NAME;
			tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.CRAPPYFOOD.TOOLTIP;
		}
		Pickupable component = Mail.Get(smi).GetComponent<Pickupable>();
		smi.Pickup(component, baseState != mailStates.Success);
		smi.ScheduleGoTo(0.016f, baseState);
		Notification notification = new Notification(title, NotificationType.Event, (List<Notification> notificationList, object data) => data as string, tooltip_data, expires: false, 0f, null, null, smi.transform.parent, volume_attenuation: true, clear_on_click: true, show_dismiss_button: true);
		smi.transform.parent.gameObject.AddOrGet<Notifier>().Add(notification);
	}

	private void EvaluateCurrentDecor(Instance smi, float dt)
	{
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
		if (smi.GetCurrentState() != Inactive && !instance.IsComplete)
		{
			float num = LonelyMinionHouse.CalculateAverageDecor(smi.def.DecorInspectionArea);
			bool flag = num >= 0f || (num > smi.StartingAverageDecor && 1f - num / smi.StartingAverageDecor > 0.01f);
			if (instance.IsStarted || flag)
			{
				instance.TrackProgress(new Quest.ItemData
				{
					CriteriaId = LonelyMinionConfig.DecorCriteriaId,
					CurrentValue = num
				}, out var _, out var _);
			}
		}
	}

	private void DelayIdle(Instance smi, float dt)
	{
		if (!(smi.AnimController.currentAnim != smi.AnimController.defaultAnim))
		{
			if (smi.IdleDelayTimer > 0f)
			{
				smi.IdleDelayTimer -= dt;
			}
			if (smi.IdleDelayTimer <= 0f)
			{
				PlayIdle(smi, smi.ChooseIdle());
				smi.IdleDelayTimer = UnityEngine.Random.Range(20f, 40f);
			}
		}
	}

	private void PlayIdle(Instance smi, HashedString idleAnim)
	{
		if (!idleAnim.IsValid)
		{
			return;
		}
		if (idleAnim == LonelyMinionConfig.CHECK_MAIL)
		{
			Mail.Set(smi.gameObject, smi);
			return;
		}
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		int num = instance.GetCurrentCount(LonelyMinionConfig.FoodCriteriaId) - 1;
		if (idleAnim == LonelyMinionConfig.FOOD_IDLE && num >= 0)
		{
			KBatchedAnimController component = Assets.GetPrefab(instance.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, UnityEngine.Random.Range(0, num))).GetComponent<KBatchedAnimController>();
			smi.PackageSnapPoint.SwapAnims(component.AnimFiles);
			smi.PackageSnapPoint.Play("object", KAnim.PlayMode.Loop);
		}
		if (!(idleAnim == LonelyMinionConfig.FOOD_IDLE) && !(idleAnim == LonelyMinionConfig.DECOR_IDLE) && !(idleAnim == LonelyMinionConfig.POWER_IDLE))
		{
			LonelyMinionHouse.Instance sMI = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			smi.AnimController.GetSynchronizer().Remove(sMI.AnimController);
			if (idleAnim == LonelyMinionConfig.BLINDS_IDLE_0)
			{
				sMI.BlindsController.Play(LonelyMinionConfig.BLINDS_IDLE_0);
			}
		}
		smi.AnimController.Play(idleAnim);
	}

	private void OnIdleAnimComplete(Instance smi)
	{
		if (smi.AnimController.currentAnim == smi.AnimController.defaultAnim)
		{
			return;
		}
		if (!(smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_IDLE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.DECOR_IDLE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.POWER_IDLE))
		{
			LonelyMinionHouse.Instance sMI = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			smi.AnimController.GetSynchronizer().Add(sMI.AnimController);
			if (smi.AnimController.currentAnim == LonelyMinionConfig.BLINDS_IDLE_0)
			{
				sMI.BlindsController.Play(string.Format("{0}_{1}", "meter_blinds", 0), KAnim.PlayMode.Paused);
			}
		}
		smi.AnimController.Play(smi.AnimController.defaultAnim, smi.AnimController.initialMode);
		if (Active.Get(smi) && Mail.Get(smi) != null)
		{
			smi.ScheduleGoTo(0f, CheckMail);
		}
	}

	private void OnBecomeInactive(Instance smi)
	{
		smi.AnimController.GetSynchronizer().Clear();
		smi.AnimController.Play(smi.AnimController.initialAnim, smi.AnimController.initialMode);
	}

	private void OnBecomeActive(Instance smi)
	{
		LonelyMinionHouse.Instance sMI = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
		if (sMI != null)
		{
			smi.AnimController.GetSynchronizer().Add(sMI.AnimController);
			if (smi.StartingAverageDecor == float.NegativeInfinity)
			{
				smi.StartingAverageDecor = LonelyMinionHouse.CalculateAverageDecor(smi.def.DecorInspectionArea);
			}
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Inactive;
		root.ParamTransition(Active, Inactive, (Instance smi, bool p) => !Active.Get(smi)).ParamTransition(Active, Idle, (Instance smi, bool p) => Active.Get(smi)).Update(EvaluateCurrentDecor, UpdateRate.SIM_1000ms);
		Inactive.Enter(OnBecomeInactive).Exit(OnBecomeActive);
		Idle.ParamTransition(Mail, CheckMail, (Instance smi, GameObject p) => smi.AnimController.currentAnim == smi.AnimController.defaultAnim && Mail.Get(smi) != null).Update(DelayIdle, UpdateRate.SIM_EVERY_TICK).EventHandler(GameHashes.AnimQueueComplete, OnIdleAnimComplete)
			.Exit(OnIdleAnimComplete);
		CheckMail.Enter(MailStates.OnEnter).ParamTransition(Mail, Idle, (Instance smi, GameObject p) => Mail.Get(smi) == null).EventTransition(GameHashes.AnimQueueComplete, Idle, HahCheckedMail)
			.Exit(MailStates.OnExit);
		CheckMail.Success.Enter(delegate(Instance smi)
		{
			MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_SUCCESS);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(Instance smi)
		{
			MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_SUCCESS);
		});
		CheckMail.Failure.Enter(delegate(Instance smi)
		{
			MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_FAILURE);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(Instance smi)
		{
			MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_FAILURE);
		});
		CheckMail.Duplicate.Enter(delegate(Instance smi)
		{
			MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_DUPLICATE);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(Instance smi)
		{
			MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_DUPLICATE);
		});
	}
}
