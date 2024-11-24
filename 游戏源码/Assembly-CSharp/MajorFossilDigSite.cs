using System;
using STRINGS;
using UnityEngine;

public class MajorFossilDigSite : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
	public class Def : BaseDef
	{
		public HashedString questCriteria;
	}

	public class ReadyToBeWorked : State
	{
		public State Operational;

		public State NonOperational;
	}

	public new class Instance : GameInstance, ICheckboxListGroupControl
	{
		[MyCmpGet]
		private Operational operational;

		[MyCmpGet]
		private MajorDigSiteWorkable excavateWorkable;

		[MyCmpGet]
		private FossilMine fossilMine;

		[MyCmpGet]
		private EntombVulnerable entombComponent;

		private Chore chore;

		private FossilHuntInitializer.Instance storyInitializer;

		private ExcavateButton excavateButton;

		public string Title => CODEX.STORY_TRAITS.FOSSILHUNT.NAME;

		public string Description
		{
			get
			{
				if (base.sm.IsRevealed.Get(this))
				{
					return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_REVEALED;
				}
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_BUILDINGMENU_COVERED;
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			Components.MajorFossilDigSites.Add(this);
		}

		public override void StartSM()
		{
			entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
			storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			GetComponent<KPrefabID>();
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			if (!base.sm.IsRevealed.Get(this))
			{
				CreateExcavateButton();
			}
			fossilMine.SetActiveState(base.sm.IsQuestCompleted.Get(this));
			if (base.sm.IsQuestCompleted.Get(this))
			{
				UnlockStandarBuildingButtons();
				ScheduleNextFrame(delegate
				{
					ChangeUIDescriptionToCompleted();
				});
			}
			excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			RefreshUI();
		}

		public void SetLightOnState(bool isOn)
		{
			FossilDigsiteLampLight component = base.gameObject.GetComponent<FossilDigsiteLampLight>();
			component.SetIndependentState(isOn);
			if (!isOn)
			{
				component.enabled = false;
			}
		}

		public Workable GetWorkable()
		{
			return excavateWorkable;
		}

		public void CreateWorkableChore()
		{
			if (chore == null)
			{
				chore = new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, excavateWorkable, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
			}
		}

		public void CancelWorkChore()
		{
			if (chore != null)
			{
				chore.Cancel("MajorFossilDigsite.CancelChore");
				chore = null;
			}
		}

		public void SetEntombStatusItemVisibility(bool visible)
		{
			entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this);
			excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		public ExcavateButton CreateExcavateButton()
		{
			if (excavateButton == null)
			{
				excavateButton = base.gameObject.AddComponent<ExcavateButton>();
				ExcavateButton obj = excavateButton;
				obj.OnButtonPressed = (System.Action)Delegate.Combine(obj.OnButtonPressed, new System.Action(OnExcavateButtonPressed));
				excavateButton.isMarkedForDig = () => base.sm.MarkedForDig.Get(this);
			}
			return excavateButton;
		}

		public void DestroyExcavateButton()
		{
			excavateWorkable.SetShouldShowSkillPerkStatusItem(shouldItBeShown: false);
			if (excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(excavateButton);
				excavateButton = null;
			}
		}

		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		public void RevealMinorDigSites()
		{
			if (storyInitializer == null)
			{
				storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			}
			if (storyInitializer != null)
			{
				storyInitializer.RevealMinorFossilDigSites();
			}
		}

		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				OnQuestCompleted(quest);
			}
			RefreshUI();
		}

		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.CompleteStorySignal.Trigger(this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
		}

		public void CompleteStoryTrait()
		{
			FossilHuntInitializer.Instance sMI = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			sMI.sm.CompleteStory.Trigger(sMI);
		}

		public void UnlockFossilMine()
		{
			fossilMine.SetActiveState(active: true);
			UnlockStandarBuildingButtons();
			ChangeUIDescriptionToCompleted();
		}

		private void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
		}

		private void UnlockStandarBuildingButtons()
		{
			base.gameObject.AddOrGet<BuildingEnabledButton>();
		}

		public void RefreshUI()
		{
			base.gameObject.Trigger(1980521255);
		}

		protected override void OnCleanUp()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			}
			Components.MajorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		public void ShowCompletionNotification()
		{
			base.gameObject.GetSMI<FossilHuntInitializer.Instance>()?.ShowObjectiveCompletedNotification();
		}
	}

	public State Idle;

	public ReadyToBeWorked Workable;

	public State WaitingForQuestCompletion;

	public State Completed;

	public BoolParameter MarkedForDig;

	public BoolParameter IsRevealed;

	public BoolParameter IsQuestCompleted;

	public Signal CompleteStorySignal;

	public const string ANIM_COVERED_NAME = "covered";

	public const string ANIM_REVEALED_NAME = "reveal";

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Idle;
		base.serializable = SerializeType.ParamsOnly;
		Idle.PlayAnim("covered").Enter(TurnOffLight).Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: false);
		})
			.ParamTransition(IsQuestCompleted, Completed, GameStateMachine<MajorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue)
			.ParamTransition(IsRevealed, WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue)
			.ParamTransition(MarkedForDig, Workable, GameStateMachine<MajorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue);
		Workable.PlayAnim("covered").Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: true);
		}).DefaultState(Workable.NonOperational)
			.ParamTransition(MarkedForDig, Idle, GameStateMachine<MajorFossilDigSite, Instance, IStateMachineTarget, Def>.IsFalse);
		Workable.NonOperational.TagTransition(GameTags.Operational, Workable.Operational);
		Workable.Operational.Enter(StartWorkChore).Exit(CancelWorkChore).TagTransition(GameTags.Operational, Workable.NonOperational, on_remove: true)
			.WorkableCompleteTransition((Instance smi) => smi.GetWorkable(), WaitingForQuestCompletion);
		WaitingForQuestCompletion.OnSignal(CompleteStorySignal, Completed).Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: true);
		}).PlayAnim("reveal")
			.Enter(DestroyUIExcavateButton)
			.Enter(Reveal)
			.ScheduleActionNextFrame("Refresh UI", RefreshUI)
			.Enter(CheckForQuestCompletion)
			.Enter(ProgressStoryTrait);
		Completed.Enter(TurnOnLight).Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: true);
		}).Enter(DestroyUIExcavateButton)
			.Enter(CompleteStory)
			.Enter(UnlockFossilMine)
			.Enter(MakeItDemolishable);
	}

	public static void MakeItDemolishable(Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	public static void ProgressStoryTrait(Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData itemData = default(Quest.ItemData);
			itemData.CriteriaId = smi.def.questCriteria;
			itemData.CurrentValue = 1f;
			Quest.ItemData data = itemData;
			instance.TrackProgress(data, out var _, out var _);
		}
	}

	public static void TurnOnLight(Instance smi)
	{
		smi.SetLightOnState(isOn: true);
	}

	public static void TurnOffLight(Instance smi)
	{
		smi.SetLightOnState(isOn: false);
	}

	public static void CheckForQuestCompletion(Instance smi)
	{
		QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (questInstance != null && questInstance.CurrentState == Quest.State.Completed)
		{
			smi.OnQuestCompleted(questInstance);
		}
	}

	public static void SetEntombedStatusItemVisibility(Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	public static void UnlockFossilMine(Instance smi)
	{
		smi.UnlockFossilMine();
	}

	public static void DestroyUIExcavateButton(Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	public static void CompleteStory(Instance smi)
	{
		if (!smi.sm.IsQuestCompleted.Get(smi))
		{
			smi.sm.IsQuestCompleted.Set(value: true, smi);
			smi.CompleteStoryTrait();
		}
	}

	public static void Reveal(Instance smi)
	{
		bool num = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(value: true, smi);
		if (num)
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null && !instance.IsComplete)
			{
				smi.ShowCompletionNotification();
			}
		}
	}

	public static void RevealMinorDigSites(Instance smi)
	{
		smi.RevealMinorDigSites();
	}

	public static void RefreshUI(Instance smi)
	{
		smi.RefreshUI();
	}

	public static void StartWorkChore(Instance smi)
	{
		smi.CreateWorkableChore();
	}

	public static void CancelWorkChore(Instance smi)
	{
		smi.CancelWorkChore();
	}
}
