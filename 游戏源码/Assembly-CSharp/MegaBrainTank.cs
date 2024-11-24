using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class MegaBrainTank : StateMachineComponent<MegaBrainTank.StatesInstance>
{
	public class States : GameStateMachine<States, StatesInstance, MegaBrainTank>
	{
		public class CommonState : State
		{
			public State dormant;

			public State idle;

			public State active;
		}

		public CommonState common;

		public Signal storyTraitCompleted;

		public Effect StatBonus;

		public override void InitializeStates(out BaseState default_state)
		{
			base.serializable = SerializeType.ParamsOnly;
			default_state = root;
			root.Enter(delegate(StatesInstance smi)
			{
				if (StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.MegaBrainTank))
				{
					if (smi.IsHungry)
					{
						smi.GoTo(common.idle);
					}
					else
					{
						smi.GoTo(common.active);
					}
				}
				else
				{
					smi.GoTo(common.dormant);
				}
			});
			common.Update(delegate(StatesInstance smi, float dt)
			{
				smi.IncrementMeter(dt);
				if (smi.UnitsFromLastStore != 0)
				{
					smi.ShelveJournals(dt);
				}
				bool flag = smi.ElementConverter.HasEnoughMass(GameTags.Oxygen, includeInactive: true);
				smi.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainNotEnoughOxygen, !flag);
			}, UpdateRate.SIM_33ms);
			common.dormant.Enter(delegate(StatesInstance smi)
			{
				smi.SetBonusActive(active: false);
				smi.ElementConverter.SetAllConsumedActive(active: false);
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, active: false);
				smi.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis);
				smi.master.GetComponent<Light2D>().enabled = false;
			}).Exit(delegate(StatesInstance smi)
			{
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, active: true);
				smi.ElementConverter.SetConsumedElementActive(GameTags.Oxygen, active: true);
				RequireInputs component = smi.GetComponent<RequireInputs>();
				component.requireConduitHasMass = true;
				component.visualizeRequirements = RequireInputs.Requirements.All;
			}).Update(delegate(StatesInstance smi, float dt)
			{
				smi.ActivateBrains(dt);
			}, UpdateRate.SIM_33ms)
				.OnSignal(storyTraitCompleted, common.active);
			common.idle.Enter(delegate(StatesInstance smi)
			{
				smi.CleanTank(active: false);
			}).UpdateTransition(common.active, (StatesInstance smi, float _) => !smi.IsHungry && smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms);
			common.active.Enter(delegate(StatesInstance smi)
			{
				smi.CleanTank(active: true);
			}).Update(delegate(StatesInstance smi, float dt)
			{
				smi.Digest(dt);
			}, UpdateRate.SIM_33ms).UpdateTransition(common.idle, (StatesInstance smi, float _) => smi.IsHungry || !smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms);
			StatBonus = new Effect("MegaBrainTankBonus", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
			object[,] sTAT_BONUSES = MegaBrainTankConfig.STAT_BONUSES;
			int length = sTAT_BONUSES.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				string attribute_id = sTAT_BONUSES[i, 0] as string;
				float? num = (float?)sTAT_BONUSES[i, 1];
				Units? units = (Units?)sTAT_BONUSES[i, 2];
				StatBonus.Add(new AttributeModifier(attribute_id, ModifierSet.ConvertValue(num.Value, units.Value), DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME));
			}
		}
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, MegaBrainTank, object>.GameInstance
	{
		private static List<Effects> minionEffects;

		public short UnitsFromLastStore;

		private float meterFill = 0.04f;

		private float targetProgress;

		private float timeTilDigested;

		private float journalActivationTimer;

		private float lastRemainingTime;

		private byte activatedJournals;

		private bool currentlyActivating;

		private short nextActiveBrain = 1;

		private string brainHum;

		private KBatchedAnimController[] controllers;

		private KAnimLink fxLink;

		private MeterController meter;

		private EventInfoData eventInfo;

		private Notification eventComplete;

		private Notifier notifier;

		public KBatchedAnimController BrainController => controllers[0];

		public KBatchedAnimController ShelfController => controllers[1];

		public Storage BrainStorage { get; private set; }

		public KSelectable Selectable { get; private set; }

		public Operational Operational { get; private set; }

		public ElementConverter ElementConverter { get; private set; }

		public ManualDeliveryKG JournalDelivery { get; private set; }

		public LoopingSounds BrainSounds { get; private set; }

		public bool IsHungry => !ElementConverter.HasEnoughMassToStartConverting(includeInactive: true);

		public int TimeTilDigested => (int)timeTilDigested;

		public int ActivationProgress => (int)(25f * meterFill);

		public HashedString CurrentActivationAnim => MegaBrainTankConfig.ACTIVATION_ANIMS[nextActiveBrain - 1];

		private HashedString currentActivationLoop
		{
			get
			{
				int num = nextActiveBrain - 1 + 5;
				return MegaBrainTankConfig.ACTIVATION_ANIMS[num];
			}
		}

		public StatesInstance(MegaBrainTank master)
			: base(master)
		{
			BrainSounds = GetComponent<LoopingSounds>();
			BrainStorage = GetComponent<Storage>();
			ElementConverter = GetComponent<ElementConverter>();
			JournalDelivery = GetComponent<ManualDeliveryKG>();
			Operational = GetComponent<Operational>();
			Selectable = GetComponent<KSelectable>();
			notifier = GetComponent<Notifier>();
			controllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>();
			meter = new MeterController(BrainController, "meter_oxygen_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, MegaBrainTankConfig.METER_SYMBOLS);
			fxLink = new KAnimLink(BrainController, ShelfController);
		}

		public override void StartSM()
		{
			InitializeEffectsList();
			base.StartSM();
			BrainController.onAnimComplete += OnAnimComplete;
			ShelfController.onAnimComplete += OnAnimComplete;
			Storage brainStorage = BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnJournalDeliveryStateChanged));
			brainHum = GlobalAssets.GetSound("MegaBrainTank_brain_wave_LP");
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.MegaBrainTank);
			float unitsAvailable = BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			if (GetCurrentState() == base.sm.common.dormant)
			{
				meterFill = (targetProgress = unitsAvailable / 25f);
				meter.SetPositionPercent(meterFill);
				short num = (short)(5f * meterFill);
				if (num > 0)
				{
					nextActiveBrain = num;
					BrainSounds.StartSound(brainHum);
					BrainSounds.SetParameter(brainHum, "BrainTankProgress", num);
					CompleteBrainActivation();
				}
			}
			else
			{
				timeTilDigested = unitsAvailable * 60f;
				meterFill = timeTilDigested - timeTilDigested % 0.04f;
				meterFill /= 1500f;
				meter.SetPositionPercent(meterFill);
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.MegaBrainTank);
				nextActiveBrain = 5;
				CompleteBrainActivation();
			}
		}

		public override void StopSM(string reason)
		{
			BrainController.onAnimComplete -= OnAnimComplete;
			ShelfController.onAnimComplete -= OnAnimComplete;
			Storage brainStorage = BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnJournalDeliveryStateChanged));
			base.StopSM(reason);
		}

		private void InitializeEffectsList()
		{
			Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
			liveMinionIdentities.OnAdd += OnLiveMinionIdAdded;
			liveMinionIdentities.OnRemove += OnLiveMinionIdRemoved;
			minionEffects = new List<Effects>((liveMinionIdentities.Count > 32) ? liveMinionIdentities.Count : 32);
			for (int i = 0; i < liveMinionIdentities.Count; i++)
			{
				OnLiveMinionIdAdded(liveMinionIdentities[i]);
			}
		}

		private void OnLiveMinionIdAdded(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			minionEffects.Add(component);
			if (GetCurrentState() == base.sm.common.active)
			{
				component.Add(base.sm.StatBonus, should_save: false);
			}
		}

		private void OnLiveMinionIdRemoved(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			minionEffects.Remove(component);
		}

		public void SetBonusActive(bool active)
		{
			for (int i = 0; i < minionEffects.Count; i++)
			{
				if (active)
				{
					minionEffects[i].Add(base.sm.StatBonus, should_save: false);
				}
				else
				{
					minionEffects[i].Remove(base.sm.StatBonus);
				}
			}
		}

		private void OnAnimComplete(HashedString anim)
		{
			if (anim == MegaBrainTankConfig.KACHUNK)
			{
				StoreJournals();
			}
			else if ((anim == base.smi.CurrentActivationAnim || anim == MegaBrainTankConfig.ACTIVATE_ALL) && GetCurrentState() != base.sm.common.idle)
			{
				CompleteBrainActivation();
			}
		}

		private void OnJournalDeliveryStateChanged(Workable w, Workable.WorkableEvent state)
		{
			switch (state)
			{
			case Workable.WorkableEvent.WorkStopped:
				break;
			case Workable.WorkableEvent.WorkStarted:
			{
				FetchAreaChore.StatesInstance sMI = w.worker.GetSMI<FetchAreaChore.StatesInstance>();
				if (!sMI.IsNullOrStopped())
				{
					GameObject gameObject = sMI.sm.deliveryObject.Get(sMI);
					if (!(gameObject == null))
					{
						Pickupable component = gameObject.GetComponent<Pickupable>();
						UnitsFromLastStore = (short)component.PrimaryElement.Units;
						float num = Mathf.Clamp01(component.PrimaryElement.Units / 5f);
						BrainStorage.SetWorkTime(num * BrainStorage.storageWorkTime);
					}
				}
				break;
			}
			default:
				ShelfController.Play(MegaBrainTankConfig.KACHUNK);
				break;
			}
		}

		public void ShelveJournals(float dt)
		{
			float num = lastRemainingTime - BrainStorage.WorkTimeRemaining;
			if (num <= 0f)
			{
				num = BrainStorage.storageWorkTime - BrainStorage.WorkTimeRemaining;
			}
			lastRemainingTime = BrainStorage.WorkTimeRemaining;
			if (BrainStorage.storageWorkTime / 5f - journalActivationTimer > 0.001f)
			{
				journalActivationTimer += num;
				return;
			}
			int num2 = -1;
			journalActivationTimer = 0f;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				byte b = (byte)(1 << i);
				bool num3 = (activatedJournals & b) == 0;
				if (num3 && num2 == -1)
				{
					num2 = i;
				}
				if (num3 & (UnityEngine.Random.Range(0f, 1f) >= 0.5f))
				{
					num2 = -1;
					activatedJournals |= b;
					ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], is_visible: true);
					break;
				}
			}
			if (num2 != -1)
			{
				ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[num2], is_visible: true);
			}
			UnitsFromLastStore--;
		}

		public void StoreJournals()
		{
			lastRemainingTime = 0f;
			activatedJournals = 0;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], is_visible: false);
			}
			ShelfController.PlayMode = KAnim.PlayMode.Paused;
			ShelfController.SetPositionPercent(0f);
			float unitsAvailable = BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			targetProgress = Mathf.Clamp01(unitsAvailable / 25f);
		}

		public void ActivateBrains(float dt)
		{
			if (currentlyActivating)
			{
				return;
			}
			currentlyActivating = (float)nextActiveBrain / 5f - meterFill <= 0.001f;
			if (currentlyActivating)
			{
				BrainController.QueueAndSyncTransition(CurrentActivationAnim);
				if (nextActiveBrain > 0)
				{
					BrainSounds.StartSound(brainHum);
					BrainSounds.SetParameter(brainHum, "BrainTankProgress", nextActiveBrain);
				}
			}
		}

		public void CompleteBrainActivation()
		{
			BrainController.Play(currentActivationLoop, KAnim.PlayMode.Loop);
			nextActiveBrain++;
			currentlyActivating = false;
			if (nextActiveBrain > 5)
			{
				float unitsAvailable = BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
				timeTilDigested = unitsAvailable * 60f;
				CompleteEvent();
			}
		}

		public void Digest(float dt)
		{
			float unitsAvailable = BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			timeTilDigested = unitsAvailable * 60f;
			if (!(targetProgress - meterFill > Mathf.Epsilon))
			{
				targetProgress = 0f;
				float num = meterFill - timeTilDigested / 1500f;
				if (num >= 0.04f)
				{
					meterFill -= num - num % 0.04f;
					meter.SetPositionPercent(meterFill);
				}
			}
		}

		public void CleanTank(bool active)
		{
			SetBonusActive(active);
			GetComponent<Light2D>().enabled = active;
			Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, active, this);
			ElementConverter.SetAllConsumedActive(active);
			BrainController.ClearQueue();
			float unitsAvailable = BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			timeTilDigested = unitsAvailable * 60f;
			if (active)
			{
				nextActiveBrain = 5;
				BrainController.QueueAndSyncTransition(MegaBrainTankConfig.ACTIVATE_ALL);
				BrainSounds.StartSound(brainHum);
				BrainSounds.SetParameter(brainHum, "BrainTankProgress", nextActiveBrain);
				return;
			}
			if (timeTilDigested < 1f / 60f)
			{
				BrainStorage.ConsumeAllIgnoringDisease(DreamJournalConfig.ID);
				timeTilDigested = 0f;
				meterFill = 0f;
				meter.SetPositionPercent(meterFill);
			}
			BrainController.QueueAndSyncTransition(MegaBrainTankConfig.DEACTIVATE_ALL);
			BrainSounds.StopSound(brainHum);
		}

		public bool IncrementMeter(float dt)
		{
			if (targetProgress - meterFill <= Mathf.Epsilon)
			{
				return false;
			}
			meterFill += Mathf.Lerp(0f, 1f, 0.04f * dt);
			if (1f - meterFill <= 0.001f)
			{
				meterFill = 1f;
			}
			meter.SetPositionPercent(meterFill);
			return targetProgress - meterFill > 0.001f;
		}

		public void CompleteEvent()
		{
			Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankActivationProgress);
			Selectable.AddStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankComplete, base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance != null && storyInstance.CurrentState != StoryInstance.State.COMPLETE)
			{
				eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.BUTTON, "braintankcomplete_kanim", EventInfoDataHelper.PopupType.COMPLETE);
				base.smi.Selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
				eventComplete = EventInfoScreen.CreateNotification(eventInfo, ShowEventCompleteUI);
				notifier.Add(eventComplete);
			}
		}

		public void ShowEventCompleteUI(object _ = null)
		{
			if (eventComplete != null)
			{
				base.smi.Selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
				notifier.Remove(eventComplete);
				eventComplete = null;
				Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_competed");
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 3)), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, base.master, new FocusTargetSequence.Data
				{
					WorldId = base.master.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = eventInfo,
					CompleteCB = OnCompleteStorySequence,
					CanCompleteCB = null
				});
			}
		}

		private void OnCompleteStorySequence()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, keepsakeSpawnPosition);
			eventInfo = null;
			base.sm.storyTraitCompleted.Trigger(this);
		}
	}

	[Serialize]
	private bool introDisplayed;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryManager.Instance.ForceCreateStory(Db.Get().Stories.MegaBrainTank, base.gameObject.GetMyWorldId());
		base.smi.StartSM();
		Subscribe(-1503271301, OnBuildingSelect);
		GetComponent<Activatable>().SetWorkTime(5f);
		base.smi.JournalDelivery.refillMass = 25f;
		base.smi.JournalDelivery.FillToCapacity = true;
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Unsubscribe(-1503271301);
	}

	private void OnBuildingSelect(object obj)
	{
		if ((bool)obj)
		{
			if (!introDisplayed)
			{
				introDisplayed = true;
				EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "braintankdiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, DoInitialUnlock));
			}
			base.smi.ShowEventCompleteUI();
		}
	}

	private void DoInitialUnlock()
	{
		Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_initial");
	}
}
