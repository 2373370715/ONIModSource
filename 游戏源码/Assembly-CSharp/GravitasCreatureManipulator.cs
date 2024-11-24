using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class GravitasCreatureManipulator : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>
{
	public class Def : BaseDef
	{
		public CellOffset pickupOffset;

		public CellOffset dropOffset;

		public int numSpeciesToUnlockMorphMode;

		public float workingDuration;

		public float cooldownDuration;
	}

	public class WorkingStates : State
	{
		public State pre;

		public State loop;

		public State pst;
	}

	public class ActiveStates : State
	{
		public State idle;

		public State capture;

		public WorkingStates working;

		public State cooldown;
	}

	public new class Instance : GameInstance
	{
		public int pickupCell;

		[MyCmpGet]
		private Storage m_storage;

		[Serialize]
		public HashSet<Tag> ScannedSpecies = new HashSet<Tag>();

		[Serialize]
		private bool m_introPopupSeen;

		[Serialize]
		private bool m_morphModeUnlocked;

		private EventInfoData eventInfo;

		private Notification m_endNotification;

		private MeterController m_progressMeter;

		private HandleVector<int>.Handle m_partitionEntry;

		private HandleVector<int>.Handle m_largeCreaturePartitionEntry;

		public bool IsMorphMode => m_morphModeUnlocked;

		public bool IsCritterStored => m_storage.Count > 0;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			pickupCell = Grid.OffsetCell(Grid.PosToCell(master.gameObject), base.smi.def.pickupOffset);
			m_partitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator", base.gameObject, pickupCell, GameScenePartitioner.Instance.pickupablesChangedLayer, DetectCreature);
			m_largeCreaturePartitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator.large", base.gameObject, Grid.CellLeft(pickupCell), GameScenePartitioner.Instance.pickupablesChangedLayer, DetectLargeCreature);
			m_progressMeter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TileFront);
		}

		public override void StartSM()
		{
			base.StartSM();
			UpdateStatusItems();
			UpdateMeter();
			StoryManager.Instance.ForceCreateStory(Db.Get().Stories.CreatureManipulator, base.gameObject.GetMyWorldId());
			if (ScannedSpecies.Count >= base.smi.def.numSpeciesToUnlockMorphMode)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
			TryShowCompletedNotification();
			Subscribe(-1503271301, OnBuildingSelect);
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.CreatureManipulator);
		}

		public override void StopSM(string reason)
		{
			Unsubscribe(-1503271301, OnBuildingSelect);
			base.StopSM(reason);
		}

		private void OnBuildingSelect(object obj)
		{
			if ((bool)obj)
			{
				if (!m_introPopupSeen)
				{
					ShowIntroNotification();
				}
				if (m_endNotification != null)
				{
					m_endNotification.customClickCallback(m_endNotification.customClickData);
				}
			}
		}

		private void UpdateStatusItems()
		{
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorProgress, !IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphMode, IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphModeLocked, !IsMorphMode, this);
		}

		public void UpdateMeter()
		{
			m_progressMeter.SetPositionPercent(Mathf.Clamp01((float)ScannedSpecies.Count / (float)base.smi.def.numSpeciesToUnlockMorphMode));
		}

		public bool IsAccepted(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			if (component.HasTag(GameTags.Creature) && !component.HasTag(GameTags.Robot))
			{
				return component.PrefabTag != GameTags.Creature;
			}
			return false;
		}

		private void DetectLargeCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (!(pickupable == null) && pickupable.GetComponent<KCollider2D>().bounds.size.x > 1.5f)
			{
				DetectCreature(obj);
			}
		}

		private void DetectCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && IsAccepted(pickupable.gameObject) && base.smi.sm.creatureTarget.IsNull(base.smi) && base.smi.IsInsideState(base.smi.sm.operational.idle))
			{
				SetCritterTarget(pickupable.gameObject);
			}
		}

		public void SetCritterTarget(GameObject go)
		{
			base.smi.sm.creatureTarget.Set(go.gameObject, base.smi);
		}

		public void StoreCreature()
		{
			GameObject go = base.smi.sm.creatureTarget.Get(base.smi);
			m_storage.Store(go);
		}

		public void DropCritter()
		{
			List<GameObject> list = new List<GameObject>();
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.smi), Grid.SceneLayer.Creatures);
			m_storage.DropAll(position, vent_gas: false, dump_liquid: false, base.smi.def.dropOffset.ToVector3(), do_disease_transfer: true, list);
			foreach (GameObject item in list)
			{
				CreatureBrain component = item.GetComponent<CreatureBrain>();
				if (!(component == null))
				{
					Scan(component.species);
					if (component.HasTag(GameTags.OriginalCreature) && IsMorphMode)
					{
						SpawnMorph(component);
					}
					else
					{
						item.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("idle_loop");
					}
				}
			}
			base.smi.sm.creatureTarget.Set(null, base.smi);
		}

		private void Scan(Tag species)
		{
			if (ScannedSpecies.Add(species))
			{
				base.gameObject.Trigger(1980521255);
				UpdateStatusItems();
				UpdateMeter();
				ShowCritterScannedNotification(species);
			}
			TryShowCompletedNotification();
		}

		public void SpawnMorph(Brain brain)
		{
			Tag tag = Tag.Invalid;
			BabyMonitor.Instance sMI = brain.GetSMI<BabyMonitor.Instance>();
			FertilityMonitor.Instance sMI2 = brain.GetSMI<FertilityMonitor.Instance>();
			bool flag = sMI != null;
			bool flag2 = sMI2 != null;
			if (flag2)
			{
				tag = FertilityMonitor.EggBreedingRoll(sMI2.breedingChances, excludeOriginalCreature: true);
			}
			else if (flag)
			{
				FertilityMonitor.Def def = Assets.GetPrefab(sMI.def.adultPrefab).GetDef<FertilityMonitor.Def>();
				if (def == null)
				{
					return;
				}
				tag = FertilityMonitor.EggBreedingRoll(def.initialBreedingWeights, excludeOriginalCreature: true);
			}
			if (!tag.IsValid)
			{
				return;
			}
			Tag tag2 = Assets.GetPrefab(tag).GetDef<IncubationMonitor.Def>().spawnedCreature;
			if (flag2)
			{
				tag2 = Assets.GetPrefab(tag2).GetDef<BabyMonitor.Def>().adultPrefab;
			}
			Vector3 position = brain.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(tag2), position);
			gameObject.SetActive(value: true);
			gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			foreach (AmountInstance amount in brain.gameObject.GetAmounts())
			{
				AmountInstance amountInstance = amount.amount.Lookup(gameObject);
				if (amountInstance != null)
				{
					float num = amount.value / amount.GetMax();
					amountInstance.value = num * amountInstance.GetMax();
				}
			}
			gameObject.Trigger(-2027483228, brain.gameObject);
			KSelectable component = brain.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
			}
			base.smi.sm.cooldownTimer.Set(base.smi.def.cooldownDuration, base.smi);
			brain.gameObject.DeleteObject();
		}

		public void ShowIntroNotification()
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_initial");
			m_introPopupSeen = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "crittermanipulatoractivate_kanim", EventInfoDataHelper.PopupType.BEGIN));
		}

		public void ShowCritterScannedNotification(Tag species)
		{
			string unlockID = GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species);
			Game.Instance.unlocks.Unlock(unlockID, shouldTryShowCodexNotification: false);
			ShowCritterScannedNotificationAndWaitForClick().Then(delegate
			{
				ShowLoreUnlockedPopup(species);
			});
			Promise ShowCritterScannedNotificationAndWaitForClick()
			{
				return new Promise(delegate(System.Action resolve)
				{
					Notification notification = new Notification(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_NOTIFICATION.NAME, NotificationType.Event, delegate(List<Notification> notifications, object obj)
					{
						string text = CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_NOTIFICATION.TOOLTIP;
						foreach (Notification notification2 in notifications)
						{
							string text2 = notification2.tooltipData as string;
							text = text + "\n • " + Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + text2);
						}
						return text;
					}, species.ToString().ToUpper(), expires: false, 0f, delegate
					{
						resolve();
					}, null, null, volume_attenuation: true, clear_on_click: true);
					base.gameObject.AddOrGet<Notifier>().Add(notification);
				});
			}
		}

		public static void ShowLoreUnlockedPopup(Tag species)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.NAME).AddDefaultOK();
			bool num = CodexCache.GetEntryForLock(GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species)) != null;
			Option<string> bodyContentForSpeciesTag = GravitasCreatureManipulatorConfig.GetBodyContentForSpeciesTag(species);
			if (num && bodyContentForSpeciesTag.HasValue)
			{
				infoDialogScreen.AddPlainText(bodyContentForSpeciesTag.Value).AddOption(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITCRITTERMANIPULATOR"));
			}
			else
			{
				infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
			}
		}

		public void TryShowCompletedNotification()
		{
			if (ScannedSpecies.Count >= base.smi.def.numSpeciesToUnlockMorphMode && !IsMorphMode)
			{
				eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.BUTTON, "crittermanipulatormorphmode_kanim", EventInfoDataHelper.PopupType.COMPLETE);
				m_endNotification = EventInfoScreen.CreateNotification(eventInfo, UnlockMorphMode);
				base.gameObject.AddOrGet<Notifier>().Add(m_endNotification);
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
			}
		}

		public void ClearEndNotification()
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
			if (m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(m_endNotification);
			}
			m_endNotification = null;
		}

		public void UnlockMorphMode(object _)
		{
			if (!m_morphModeUnlocked)
			{
				Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_complete");
				if (m_endNotification != null)
				{
					base.gameObject.AddOrGet<Notifier>().Remove(m_endNotification);
				}
				m_morphModeUnlocked = true;
				UpdateStatusItems();
				ClearEndNotification();
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, base.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data
				{
					WorldId = base.smi.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = eventInfo,
					CompleteCB = OnStorySequenceComplete,
					CanCompleteCB = null
				});
			}
		}

		private void OnStorySequenceComplete()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, keepsakeSpawnPosition);
			eventInfo = null;
		}

		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref m_partitionEntry);
			GameScenePartitioner.Instance.Free(ref m_largeCreaturePartitionEntry);
			if (m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(m_endNotification);
			}
		}
	}

	public State inoperational;

	public ActiveStates operational;

	public TargetParameter creatureTarget;

	public FloatParameter cooldownTimer;

	public FloatParameter workingTimer;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = inoperational;
		base.serializable = SerializeType.ParamsOnly;
		root.Enter(delegate(Instance smi)
		{
			smi.DropCritter();
		}).Enter(delegate(Instance smi)
		{
			smi.UpdateMeter();
		}).EventHandler(GameHashes.BuildingActivated, delegate(Instance smi, object activated)
		{
			if ((bool)activated)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
		});
		inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, operational.idle, (Instance smi) => smi.GetComponent<Operational>().IsOperational);
		operational.DefaultState(operational.idle).EventTransition(GameHashes.OperationalChanged, inoperational, (Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		operational.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(CheckForCritter).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWaiting)
			.ParamTransition(creatureTarget, operational.capture, (Instance smi, GameObject p) => p != null && !smi.IsCritterStored)
			.ParamTransition(creatureTarget, operational.working.pre, (Instance smi, GameObject p) => p != null && smi.IsCritterStored)
			.ParamTransition(cooldownTimer, operational.cooldown, GameStateMachine<GravitasCreatureManipulator, Instance, IStateMachineTarget, Def>.IsGTZero);
		operational.capture.PlayAnim("working_capture").OnAnimQueueComplete(operational.working.pre);
		operational.working.DefaultState(operational.working.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWorking);
		operational.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(operational.working.loop).Enter(delegate(Instance smi)
		{
			smi.StoreCreature();
		})
			.Exit(delegate(Instance smi)
			{
				smi.sm.workingTimer.Set(smi.def.workingDuration, smi);
			})
			.OnTargetLost(creatureTarget, operational.idle)
			.Target(creatureTarget)
			.ToggleStationaryIdling();
		operational.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(Instance smi, float dt)
		{
			smi.sm.workingTimer.DeltaClamp(0f - dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms).ParamTransition(workingTimer, operational.working.pst, GameStateMachine<GravitasCreatureManipulator, Instance, IStateMachineTarget, Def>.IsLTEZero)
			.OnTargetLost(creatureTarget, operational.idle);
		operational.working.pst.PlayAnim("working_pst").Enter(delegate(Instance smi)
		{
			smi.DropCritter();
		}).OnAnimQueueComplete(operational.cooldown);
		operational.cooldown.PlayAnim("working_cooldown", KAnim.PlayMode.Loop).Update(delegate(Instance smi, float dt)
		{
			smi.sm.cooldownTimer.DeltaClamp(0f - dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms).ParamTransition(cooldownTimer, operational.idle, GameStateMachine<GravitasCreatureManipulator, Instance, IStateMachineTarget, Def>.IsLTEZero)
			.ToggleStatusItem(CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.NAME, CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, category: Db.Get().StatusItemCategories.Main, resolve_string_callback: Processing, resolve_tooltip_callback: ProcessingTooltip);
	}

	private static string Processing(string str, Instance smi)
	{
		return str.Replace("{percent}", GameUtil.GetFormattedPercent((1f - smi.sm.cooldownTimer.Get(smi) / smi.def.cooldownDuration) * 100f));
	}

	private static string ProcessingTooltip(string str, Instance smi)
	{
		return str.Replace("{timeleft}", GameUtil.GetFormattedTime(smi.sm.cooldownTimer.Get(smi)));
	}

	private static void CheckForCritter(Instance smi)
	{
		if (!smi.sm.creatureTarget.IsNull(smi))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[smi.pickupCell, 3];
		if (!(gameObject != null))
		{
			return;
		}
		ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
		while (objectLayerListItem != null)
		{
			GameObject gameObject2 = objectLayerListItem.gameObject;
			objectLayerListItem = objectLayerListItem.nextItem;
			if (!(gameObject2 == null) && smi.IsAccepted(gameObject2))
			{
				smi.SetCritterTarget(gameObject2);
				break;
			}
		}
	}
}
