using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class MorbRoverMakerStorytrait : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>
{
	public class Def : TraitDef
	{
		public const string LORE_UNLOCK_PREFIX = "story_trait_morbrover_";

		public string MachineRevealedLoreId = "story_trait_morbrover_reveal";

		public string MachineRevealedLoreId2 = "story_trait_morbrover_reveal_lore";

		public string CompleteLoreId2 = "story_trait_morbrover_complete_lore";

		public string CompleteLoreId3 = "story_trait_morbrover_biobot";

		public System.Action NormalPopupOpenCodexButtonPressed;

		public StoryManager.PopupInfo EventMachineRevealedInfo;

		public override void Configure(GameObject prefab)
		{
			Story = Db.Get().Stories.MorbRoverMaker;
			CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(0, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			InitalLoreId = "story_trait_morbrover_initial";
			EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.BUTTON,
				TextureName = "biobotdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			EventMachineRevealedInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_CLOSE,
				extraButtons = new StoryManager.ExtraButtonInfo[1]
				{
					new StoryManager.ExtraButtonInfo
					{
						ButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_READLORE,
						OnButtonClick = delegate
						{
							NormalPopupOpenCodexButtonPressed?.Invoke();
							UnlockRevealEntries();
							string entryForLock = CodexCache.GetEntryForLock(MachineRevealedLoreId);
							if (entryForLock == null)
							{
								DebugUtil.DevLogError("Missing codex entry for lock: " + MachineRevealedLoreId);
							}
							else
							{
								ManagementMenu.Instance.OpenCodexToEntry(entryForLock);
							}
						}
					}
				},
				TextureName = "BioBotCleanedUp_kanim",
				PopupType = EventInfoDataHelper.PopupType.NORMAL
			};
			CompleteLoreId = "story_trait_morbrover_complete";
			EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.BUTTON,
				TextureName = "BioBotComplete_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		public void UnlockRevealEntries()
		{
			Game.Instance.unlocks.Unlock(MachineRevealedLoreId);
			Game.Instance.unlocks.Unlock(MachineRevealedLoreId2);
		}
	}

	public new class Instance : TraitInstance
	{
		private MorbRoverMaker.Instance machine;

		private StoryInstance storyInstance;

		public Instance(StateMachineController master, Def def)
			: base(master, def)
		{
			def.NormalPopupOpenCodexButtonPressed = (System.Action)Delegate.Combine(def.NormalPopupOpenCodexButtonPressed, new System.Action(OnNormalPopupOpenCodexButtonPressed));
		}

		public override void StartSM()
		{
			base.StartSM();
			machine = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MorbRoverMaker.HashId);
			if (storyInstance != null && machine != null)
			{
				MorbRoverMaker.Instance instance = machine;
				instance.OnUncovered = (System.Action)Delegate.Combine(instance.OnUncovered, new System.Action(OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Combine(instance2.OnRoverSpawned, new Action<GameObject>(OnRoverSpawned));
				if (machine.HasBeenRevealed && storyInstance.CurrentState != StoryInstance.State.COMPLETE && storyInstance.CurrentState != StoryInstance.State.IN_PROGRESS)
				{
					DisplayPopup(base.def.EventMachineRevealedInfo);
				}
				if (machine.HasBeenRevealed && base.sm.HasAnyBioBotBeenReleased.Get(this) && storyInstance.CurrentState != StoryInstance.State.COMPLETE)
				{
					CompleteEvent();
				}
			}
		}

		private void OnMachineUncovered()
		{
			if (storyInstance != null && !storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				DisplayPopup(base.def.EventMachineRevealedInfo);
			}
		}

		protected override void ShowEventNormalUI()
		{
			base.ShowEventNormalUI();
			if (storyInstance != null && storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
			{
				EventInfoScreen.ShowPopup(storyInstance.EventInfo);
			}
		}

		public override void OnPopupClosed()
		{
			base.OnPopupClosed();
			if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId2);
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId3);
			}
			else if (storyInstance != null && storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
				base.def.UnlockRevealEntries();
			}
		}

		private void OnNormalPopupOpenCodexButtonPressed()
		{
			TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		private void OnRoverSpawned(GameObject rover)
		{
			base.smi.sm.HasAnyBioBotBeenReleased.Set(value: true, base.smi);
			if (!storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				CompleteEvent();
			}
		}

		protected override void OnCleanUp()
		{
			if (machine != null)
			{
				MorbRoverMaker.Instance instance = machine;
				instance.OnUncovered = (System.Action)Delegate.Remove(instance.OnUncovered, new System.Action(OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Remove(instance2.OnRoverSpawned, new Action<GameObject>(OnRoverSpawned));
			}
			base.OnCleanUp();
		}
	}

	public BoolParameter HasAnyBioBotBeenReleased;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = root;
	}
}
