using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class MinorFossilDigSite : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>
{
	public class Def : BaseDef
	{
		public HashedString fossilQuestCriteriaID;
	}

	public class ReadyToBeWorked : State
	{
		public State Operational;

		public State NonOperational;
	}

	public new class Instance : GameInstance, ICheckboxListGroupControl
	{
		[MyCmpGet]
		private MinorDigSiteWorkable workable;

		[MyCmpGet]
		private EntombVulnerable entombComponent;

		private ExcavateButton excavateButton;

		private Chore chore;

		private AttributeModifier negativeDecorModifier;

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
			Components.MinorFossilDigSites.Add(this);
			negativeDecorModifier = new AttributeModifier(Db.Get().Attributes.Decor.Id, -1f, CODEX.STORY_TRAITS.FOSSILHUNT.MISC.DECREASE_DECOR_ATTRIBUTE, is_multiplier: true);
		}

		public void SetDecorState(bool isDusty)
		{
			if (isDusty)
			{
				base.gameObject.GetComponent<DecorProvider>().decor.Add(negativeDecorModifier);
			}
			else
			{
				base.gameObject.GetComponent<DecorProvider>().decor.Remove(negativeDecorModifier);
			}
		}

		public override void StartSM()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStorytraitProgressChanged));
			}
			if (!base.sm.IsRevealed.Get(this))
			{
				CreateExcavateButton();
			}
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			RefreshUI();
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
			base.sm.IsQuestCompleted.Set(value: true, this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
		}

		protected override void OnCleanUp()
		{
			ProgressQuest(base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStorytraitProgressChanged));
			}
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			}
			Components.MinorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		public void OnStorytraitProgressChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.IN_PROGRESS)
			{
				_ = 3;
			}
			else
			{
				RefreshUI();
			}
		}

		public void RefreshUI()
		{
			Trigger(1980521255);
		}

		public Workable GetWorkable()
		{
			return workable;
		}

		public void CreateWorkableChore()
		{
			if (chore == null)
			{
				chore = new WorkChore<MinorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, workable, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
			}
		}

		public void CancelWorkChore()
		{
			if (chore != null)
			{
				chore.Cancel("MinorFossilDigsite.CancelChore");
				chore = null;
			}
		}

		public void SetEntombStatusItemVisibility(bool visible)
		{
			entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		public void ShowCompletionNotification()
		{
			base.gameObject.GetSMI<FossilHuntInitializer.Instance>()?.ShowObjectiveCompletedNotification();
		}

		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this);
			workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
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
			workable.SetShouldShowSkillPerkStatusItem(shouldItBeShown: false);
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

		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}
	}

	public State Idle;

	public State Completed;

	public State WaitingForQuestCompletion;

	public ReadyToBeWorked Workable;

	public BoolParameter MarkedForDig;

	public BoolParameter IsRevealed;

	public BoolParameter IsQuestCompleted;

	private const string UNEXCAVATED_ANIM_NAME = "object_dirty";

	private const string EXCAVATED_ANIM_NAME = "object";

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Idle;
		base.serializable = SerializeType.ParamsOnly;
		Idle.Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: false);
		}).Enter(delegate(Instance smi)
		{
			smi.SetDecorState(isDusty: true);
		}).PlayAnim("object_dirty")
			.ParamTransition(IsQuestCompleted, Completed, GameStateMachine<MinorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue)
			.ParamTransition(IsRevealed, WaitingForQuestCompletion, GameStateMachine<MinorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue)
			.ParamTransition(MarkedForDig, Workable, GameStateMachine<MinorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue);
		Workable.PlayAnim("object_dirty").Toggle("Activate Entombed Status Item If Required", delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: true);
		}, delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: false);
		}).DefaultState(Workable.NonOperational)
			.ParamTransition(MarkedForDig, Idle, GameStateMachine<MinorFossilDigSite, Instance, IStateMachineTarget, Def>.IsFalse);
		Workable.NonOperational.TagTransition(GameTags.Operational, Workable.Operational);
		Workable.Operational.Enter(StartWorkChore).Exit(CancelWorkChore).TagTransition(GameTags.Operational, Workable.NonOperational, on_remove: true)
			.WorkableCompleteTransition((Instance smi) => smi.GetWorkable(), WaitingForQuestCompletion);
		WaitingForQuestCompletion.Enter(delegate(Instance smi)
		{
			smi.SetDecorState(isDusty: false);
		}).Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: true);
		}).Enter(Reveal)
			.Enter(ProgressStoryTrait)
			.Enter(DestroyUIExcavateButton)
			.Enter(MakeItDemolishable)
			.PlayAnim("object")
			.ParamTransition(IsQuestCompleted, Completed, GameStateMachine<MinorFossilDigSite, Instance, IStateMachineTarget, Def>.IsTrue);
		Completed.Enter(delegate(Instance smi)
		{
			smi.SetDecorState(isDusty: false);
		}).Enter(delegate(Instance smi)
		{
			SetEntombedStatusItemVisibility(smi, val: true);
		}).PlayAnim("object")
			.Enter(DestroyUIExcavateButton)
			.Enter(MakeItDemolishable);
	}

	public static void MakeItDemolishable(Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	public static void DestroyUIExcavateButton(Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	public static void SetEntombedStatusItemVisibility(Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	public static void UnregisterFromComponents(Instance smi)
	{
		Components.MinorFossilDigSites.Remove(smi);
	}

	public static void SelfDestroy(Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	public static void StartWorkChore(Instance smi)
	{
		smi.CreateWorkableChore();
	}

	public static void CancelWorkChore(Instance smi)
	{
		smi.CancelWorkChore();
	}

	public static void Reveal(Instance smi)
	{
		bool num = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(value: true, smi);
		if (num)
		{
			smi.ShowCompletionNotification();
			DropLoot(smi);
		}
	}

	public static void DropLoot(Instance smi)
	{
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element == null)
		{
			return;
		}
		float num = component.Mass;
		for (int i = 0; (float)i < component.Mass / 400f; i++)
		{
			float num2 = num;
			if (num > 400f)
			{
				num2 = 400f;
				num -= 400f;
			}
			int disease_count = (int)((float)component.DiseaseCount * (num2 / component.Mass));
			element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num2, component.Temperature, component.DiseaseIdx, disease_count);
		}
	}

	public static void ProgressStoryTrait(Instance smi)
	{
		ProgressQuest(smi);
	}

	public static QuestInstance ProgressQuest(Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData itemData = default(Quest.ItemData);
			itemData.CriteriaId = smi.def.fossilQuestCriteriaID;
			itemData.CurrentValue = 1f;
			Quest.ItemData data = itemData;
			instance.TrackProgress(data, out var _, out var _);
			return instance;
		}
		return null;
	}
}
