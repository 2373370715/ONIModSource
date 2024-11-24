using System;
using STRINGS;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class MajorFossilDigSite : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
	// Token: 0x06001127 RID: 4391 RVA: 0x001831AC File Offset: 0x001813AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.PlayAnim("covered").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOffLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("covered").Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MajorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.OnSignal(this.CompleteStorySignal, this.Completed).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("reveal").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.Reveal)).ScheduleActionNextFrame("Refresh UI", new Action<MajorFossilDigSite.Instance>(MajorFossilDigSite.RefreshUI)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CheckForQuestCompletion)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.ProgressStoryTrait));
		this.Completed.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOnLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CompleteStory)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.UnlockFossilMine)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x000ADAAE File Offset: 0x000ABCAE
	public static void MakeItDemolishable(MajorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x0018345C File Offset: 0x0018165C
	public static void ProgressStoryTrait(MajorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.questCriteria,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
		}
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x000ADAC1 File Offset: 0x000ABCC1
	public static void TurnOnLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(true);
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x000ADACA File Offset: 0x000ABCCA
	public static void TurnOffLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(false);
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x001834BC File Offset: 0x001816BC
	public static void CheckForQuestCompletion(MajorFossilDigSite.Instance smi)
	{
		QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (questInstance != null && questInstance.CurrentState == Quest.State.Completed)
		{
			smi.OnQuestCompleted(questInstance);
		}
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x000ADAD3 File Offset: 0x000ABCD3
	public static void SetEntombedStatusItemVisibility(MajorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x000ADADC File Offset: 0x000ABCDC
	public static void UnlockFossilMine(MajorFossilDigSite.Instance smi)
	{
		smi.UnlockFossilMine();
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x000ADAE4 File Offset: 0x000ABCE4
	public static void DestroyUIExcavateButton(MajorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x000ADAEC File Offset: 0x000ABCEC
	public static void CompleteStory(MajorFossilDigSite.Instance smi)
	{
		if (smi.sm.IsQuestCompleted.Get(smi))
		{
			return;
		}
		smi.sm.IsQuestCompleted.Set(true, smi, false);
		smi.CompleteStoryTrait();
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x001834F8 File Offset: 0x001816F8
	public static void Reveal(MajorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null && !instance.IsComplete)
			{
				smi.ShowCompletionNotification();
			}
		}
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000ADB1C File Offset: 0x000ABD1C
	public static void RevealMinorDigSites(MajorFossilDigSite.Instance smi)
	{
		smi.RevealMinorDigSites();
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x000ADB24 File Offset: 0x000ABD24
	public static void RefreshUI(MajorFossilDigSite.Instance smi)
	{
		smi.RefreshUI();
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x000ADB2C File Offset: 0x000ABD2C
	public static void StartWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x000ADB34 File Offset: 0x000ABD34
	public static void CancelWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x04000BBA RID: 3002
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Idle;

	// Token: 0x04000BBB RID: 3003
	public MajorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x04000BBC RID: 3004
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x04000BBD RID: 3005
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Completed;

	// Token: 0x04000BBE RID: 3006
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000BBF RID: 3007
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000BC0 RID: 3008
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000BC1 RID: 3009
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Signal CompleteStorySignal;

	// Token: 0x04000BC2 RID: 3010
	public const string ANIM_COVERED_NAME = "covered";

	// Token: 0x04000BC3 RID: 3011
	public const string ANIM_REVEALED_NAME = "reveal";

	// Token: 0x020003FF RID: 1023
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000BC4 RID: 3012
		public HashedString questCriteria;
	}

	// Token: 0x02000400 RID: 1024
	public class ReadyToBeWorked : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State
	{
		// Token: 0x04000BC5 RID: 3013
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Operational;

		// Token: 0x04000BC6 RID: 3014
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x02000401 RID: 1025
	public new class Instance : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x000ADB4C File Offset: 0x000ABD4C
		public Instance(IStateMachineTarget master, MajorFossilDigSite.Def def) : base(master, def)
		{
			Components.MajorFossilDigSites.Add(this);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0018355C File Offset: 0x0018175C
		public override void StartSM()
		{
			this.entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
			this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			base.GetComponent<KPrefabID>();
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			this.fossilMine.SetActiveState(base.sm.IsQuestCompleted.Get(this));
			if (base.sm.IsQuestCompleted.Get(this))
			{
				this.UnlockStandarBuildingButtons();
				this.ScheduleNextFrame(delegate(object obj)
				{
					this.ChangeUIDescriptionToCompleted();
				}, null);
			}
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00183660 File Offset: 0x00181860
		public void SetLightOnState(bool isOn)
		{
			FossilDigsiteLampLight component = base.gameObject.GetComponent<FossilDigsiteLampLight>();
			component.SetIndependentState(isOn, true);
			if (!isOn)
			{
				component.enabled = false;
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000ADB61 File Offset: 0x000ABD61
		public Workable GetWorkable()
		{
			return this.excavateWorkable;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0018368C File Offset: 0x0018188C
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.excavateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000ADB69 File Offset: 0x000ABD69
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MajorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000ADB8A File Offset: 0x000ABD8A
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x001836D4 File Offset: 0x001818D4
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00183724 File Offset: 0x00181924
		public ExcavateButton CreateExcavateButton()
		{
			if (this.excavateButton == null)
			{
				this.excavateButton = base.gameObject.AddComponent<ExcavateButton>();
				ExcavateButton excavateButton = this.excavateButton;
				excavateButton.OnButtonPressed = (System.Action)Delegate.Combine(excavateButton.OnButtonPressed, new System.Action(this.OnExcavateButtonPressed));
				this.excavateButton.isMarkedForDig = (() => base.sm.MarkedForDig.Get(this));
			}
			return this.excavateButton;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000ADB98 File Offset: 0x000ABD98
		public void DestroyExcavateButton()
		{
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x000ABDD1 File Offset: 0x000A9FD1
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x000ADBC6 File Offset: 0x000ABDC6
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

		// Token: 0x06001145 RID: 4421 RVA: 0x000ADBF0 File Offset: 0x000ABDF0
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000ADC06 File Offset: 0x000ABE06
		public void RevealMinorDigSites()
		{
			if (this.storyInitializer == null)
			{
				this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			}
			if (this.storyInitializer != null)
			{
				this.storyInitializer.RevealMinorFossilDigSites();
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x000ADC34 File Offset: 0x000ABE34
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000ADC5F File Offset: 0x000ABE5F
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.CompleteStorySignal.Trigger(this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00183794 File Offset: 0x00181994
		public void CompleteStoryTrait()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			smi.sm.CompleteStory.Trigger(smi);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x000ADC94 File Offset: 0x000ABE94
		public void UnlockFossilMine()
		{
			this.fossilMine.SetActiveState(true);
			this.UnlockStandarBuildingButtons();
			this.ChangeUIDescriptionToCompleted();
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x001837C0 File Offset: 0x001819C0
		private void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000ADCAE File Offset: 0x000ABEAE
		private void UnlockStandarBuildingButtons()
		{
			base.gameObject.AddOrGet<BuildingEnabledButton>();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000ADCBC File Offset: 0x000ABEBC
		public void RefreshUI()
		{
			base.gameObject.Trigger(1980521255, null);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00183814 File Offset: 0x00181A14
		protected override void OnCleanUp()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MajorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000ABCBD File Offset: 0x000A9EBD
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x000ADCCF File Offset: 0x000ABECF
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00183874 File Offset: 0x00181A74
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x04000BC7 RID: 3015
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04000BC8 RID: 3016
		[MyCmpGet]
		private MajorDigSiteWorkable excavateWorkable;

		// Token: 0x04000BC9 RID: 3017
		[MyCmpGet]
		private FossilMine fossilMine;

		// Token: 0x04000BCA RID: 3018
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x04000BCB RID: 3019
		private Chore chore;

		// Token: 0x04000BCC RID: 3020
		private FossilHuntInitializer.Instance storyInitializer;

		// Token: 0x04000BCD RID: 3021
		private ExcavateButton excavateButton;
	}
}
