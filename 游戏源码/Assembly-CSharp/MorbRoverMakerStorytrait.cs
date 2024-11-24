using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class MorbRoverMakerStorytrait : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>
{
	// Token: 0x0600154E RID: 5454 RVA: 0x000AF884 File Offset: 0x000ADA84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04000E62 RID: 3682
	public StateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, StateMachineController, MorbRoverMakerStorytrait.Def>.BoolParameter HasAnyBioBotBeenReleased;

	// Token: 0x020004B9 RID: 1209
	public class Def : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitDef
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x00192F68 File Offset: 0x00191168
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.MorbRoverMaker;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(0, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_morbrover_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.BUTTON,
				TextureName = "biobotdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.EventMachineRevealedInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_CLOSE,
				extraButtons = new StoryManager.ExtraButtonInfo[]
				{
					new StoryManager.ExtraButtonInfo
					{
						ButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_READLORE,
						OnButtonClick = delegate()
						{
							System.Action normalPopupOpenCodexButtonPressed = this.NormalPopupOpenCodexButtonPressed;
							if (normalPopupOpenCodexButtonPressed != null)
							{
								normalPopupOpenCodexButtonPressed();
							}
							this.UnlockRevealEntries();
							string entryForLock = CodexCache.GetEntryForLock(this.MachineRevealedLoreId);
							if (entryForLock == null)
							{
								DebugUtil.DevLogError("Missing codex entry for lock: " + this.MachineRevealedLoreId);
								return;
							}
							ManagementMenu.Instance.OpenCodexToEntry(entryForLock, null);
						}
					}
				},
				TextureName = "BioBotCleanedUp_kanim",
				PopupType = EventInfoDataHelper.PopupType.NORMAL
			};
			this.CompleteLoreId = "story_trait_morbrover_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.BUTTON,
				TextureName = "BioBotComplete_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x000AF89D File Offset: 0x000ADA9D
		public void UnlockRevealEntries()
		{
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId, true);
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId2, true);
		}

		// Token: 0x04000E63 RID: 3683
		public const string LORE_UNLOCK_PREFIX = "story_trait_morbrover_";

		// Token: 0x04000E64 RID: 3684
		public string MachineRevealedLoreId = "story_trait_morbrover_reveal";

		// Token: 0x04000E65 RID: 3685
		public string MachineRevealedLoreId2 = "story_trait_morbrover_reveal_lore";

		// Token: 0x04000E66 RID: 3686
		public string CompleteLoreId2 = "story_trait_morbrover_complete_lore";

		// Token: 0x04000E67 RID: 3687
		public string CompleteLoreId3 = "story_trait_morbrover_biobot";

		// Token: 0x04000E68 RID: 3688
		public System.Action NormalPopupOpenCodexButtonPressed;

		// Token: 0x04000E69 RID: 3689
		public StoryManager.PopupInfo EventMachineRevealedInfo;
	}

	// Token: 0x020004BA RID: 1210
	public new class Instance : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitInstance
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x000AF8FF File Offset: 0x000ADAFF
		public Instance(StateMachineController master, MorbRoverMakerStorytrait.Def def) : base(master, def)
		{
			def.NormalPopupOpenCodexButtonPressed = (System.Action)Delegate.Combine(def.NormalPopupOpenCodexButtonPressed, new System.Action(this.OnNormalPopupOpenCodexButtonPressed));
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00193170 File Offset: 0x00191370
		public override void StartSM()
		{
			base.StartSM();
			this.machine = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MorbRoverMaker.HashId);
			if (this.storyInstance == null)
			{
				return;
			}
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Combine(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Combine(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
				if (this.machine.HasBeenRevealed && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE && this.storyInstance.CurrentState != StoryInstance.State.IN_PROGRESS)
				{
					base.DisplayPopup(base.def.EventMachineRevealedInfo);
				}
				if (this.machine.HasBeenRevealed && base.sm.HasAnyBioBotBeenReleased.Get(this) && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE)
				{
					this.CompleteEvent();
				}
			}
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x000AF92B File Offset: 0x000ADB2B
		private void OnMachineUncovered()
		{
			if (this.storyInstance != null && !this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.DisplayPopup(base.def.EventMachineRevealedInfo);
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x000AF954 File Offset: 0x000ADB54
		protected override void ShowEventNormalUI()
		{
			base.ShowEventNormalUI();
			if (this.storyInstance != null && this.storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
			{
				EventInfoScreen.ShowPopup(this.storyInstance.EventInfo);
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00193288 File Offset: 0x00191488
		public override void OnPopupClosed()
		{
			base.OnPopupClosed();
			if (this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId2, true);
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId3, true);
				return;
			}
			if (this.storyInstance != null && this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
				base.def.UnlockRevealEntries();
				return;
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x000AF983 File Offset: 0x000ADB83
		private void OnNormalPopupOpenCodexButtonPressed()
		{
			base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000AF98C File Offset: 0x000ADB8C
		private void OnRoverSpawned(GameObject rover)
		{
			base.smi.sm.HasAnyBioBotBeenReleased.Set(true, base.smi, false);
			if (!this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				this.CompleteEvent();
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0019330C File Offset: 0x0019150C
		protected override void OnCleanUp()
		{
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Remove(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Remove(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
			}
			base.OnCleanUp();
		}

		// Token: 0x04000E6A RID: 3690
		private MorbRoverMaker.Instance machine;

		// Token: 0x04000E6B RID: 3691
		private StoryInstance storyInstance;
	}
}
