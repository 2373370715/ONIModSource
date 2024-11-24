using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DD2 RID: 3538
public class GravitasCreatureManipulator : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>
{
	// Token: 0x0600458E RID: 17806 RVA: 0x0024BF98 File Offset: 0x0024A198
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.DropCritter();
		}).Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.UpdateMeter();
		}).EventHandler(GameHashes.BuildingActivated, delegate(GravitasCreatureManipulator.Instance smi, object activated)
		{
			if ((bool)activated)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational.idle, (GravitasCreatureManipulator.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.inoperational, (GravitasCreatureManipulator.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.operational.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(new StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State.Callback(GravitasCreatureManipulator.CheckForCritter)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWaiting, null).ParamTransition<GameObject>(this.creatureTarget, this.operational.capture, (GravitasCreatureManipulator.Instance smi, GameObject p) => p != null && !smi.IsCritterStored).ParamTransition<GameObject>(this.creatureTarget, this.operational.working.pre, (GravitasCreatureManipulator.Instance smi, GameObject p) => p != null && smi.IsCritterStored).ParamTransition<float>(this.cooldownTimer, this.operational.cooldown, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsGTZero);
		this.operational.capture.PlayAnim("working_capture").OnAnimQueueComplete(this.operational.working.pre);
		this.operational.working.DefaultState(this.operational.working.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWorking, null);
		this.operational.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.working.loop).Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.StoreCreature();
		}).Exit(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.sm.workingTimer.Set(smi.def.workingDuration, smi, false);
		}).OnTargetLost(this.creatureTarget, this.operational.idle).Target(this.creatureTarget).ToggleStationaryIdling();
		this.operational.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(GravitasCreatureManipulator.Instance smi, float dt)
		{
			smi.sm.workingTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<float>(this.workingTimer, this.operational.working.pst, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsLTEZero).OnTargetLost(this.creatureTarget, this.operational.idle);
		this.operational.working.pst.PlayAnim("working_pst").Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.DropCritter();
		}).OnAnimQueueComplete(this.operational.cooldown);
		GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State state = this.operational.cooldown.PlayAnim("working_cooldown", KAnim.PlayMode.Loop).Update(delegate(GravitasCreatureManipulator.Instance smi, float dt)
		{
			smi.sm.cooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<float>(this.cooldownTimer, this.operational.idle, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsLTEZero);
		string name = CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.NAME;
		string tooltip = CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		Func<string, GravitasCreatureManipulator.Instance, string> resolve_string_callback = new Func<string, GravitasCreatureManipulator.Instance, string>(GravitasCreatureManipulator.Processing);
		Func<string, GravitasCreatureManipulator.Instance, string> resolve_tooltip_callback = new Func<string, GravitasCreatureManipulator.Instance, string>(GravitasCreatureManipulator.ProcessingTooltip);
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, resolve_tooltip_callback, main);
	}

	// Token: 0x0600458F RID: 17807 RVA: 0x000CD093 File Offset: 0x000CB293
	private static string Processing(string str, GravitasCreatureManipulator.Instance smi)
	{
		return str.Replace("{percent}", GameUtil.GetFormattedPercent((1f - smi.sm.cooldownTimer.Get(smi) / smi.def.cooldownDuration) * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x06004590 RID: 17808 RVA: 0x000CD0CF File Offset: 0x000CB2CF
	private static string ProcessingTooltip(string str, GravitasCreatureManipulator.Instance smi)
	{
		return str.Replace("{timeleft}", GameUtil.GetFormattedTime(smi.sm.cooldownTimer.Get(smi), "F0"));
	}

	// Token: 0x06004591 RID: 17809 RVA: 0x0024C3F8 File Offset: 0x0024A5F8
	private static void CheckForCritter(GravitasCreatureManipulator.Instance smi)
	{
		if (smi.sm.creatureTarget.IsNull(smi))
		{
			GameObject gameObject = Grid.Objects[smi.pickupCell, 3];
			if (gameObject != null)
			{
				ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
				while (objectLayerListItem != null)
				{
					GameObject gameObject2 = objectLayerListItem.gameObject;
					objectLayerListItem = objectLayerListItem.nextItem;
					if (!(gameObject2 == null) && smi.IsAccepted(gameObject2))
					{
						smi.SetCritterTarget(gameObject2);
						return;
					}
				}
			}
		}
	}

	// Token: 0x04002FFE RID: 12286
	public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State inoperational;

	// Token: 0x04002FFF RID: 12287
	public GravitasCreatureManipulator.ActiveStates operational;

	// Token: 0x04003000 RID: 12288
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.TargetParameter creatureTarget;

	// Token: 0x04003001 RID: 12289
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.FloatParameter cooldownTimer;

	// Token: 0x04003002 RID: 12290
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.FloatParameter workingTimer;

	// Token: 0x02000DD3 RID: 3539
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003003 RID: 12291
		public CellOffset pickupOffset;

		// Token: 0x04003004 RID: 12292
		public CellOffset dropOffset;

		// Token: 0x04003005 RID: 12293
		public int numSpeciesToUnlockMorphMode;

		// Token: 0x04003006 RID: 12294
		public float workingDuration;

		// Token: 0x04003007 RID: 12295
		public float cooldownDuration;
	}

	// Token: 0x02000DD4 RID: 3540
	public class WorkingStates : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State
	{
		// Token: 0x04003008 RID: 12296
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State pre;

		// Token: 0x04003009 RID: 12297
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State loop;

		// Token: 0x0400300A RID: 12298
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State pst;
	}

	// Token: 0x02000DD5 RID: 3541
	public class ActiveStates : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State
	{
		// Token: 0x0400300B RID: 12299
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State idle;

		// Token: 0x0400300C RID: 12300
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State capture;

		// Token: 0x0400300D RID: 12301
		public GravitasCreatureManipulator.WorkingStates working;

		// Token: 0x0400300E RID: 12302
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State cooldown;
	}

	// Token: 0x02000DD6 RID: 3542
	public new class Instance : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.GameInstance
	{
		// Token: 0x06004596 RID: 17814 RVA: 0x0024C46C File Offset: 0x0024A66C
		public Instance(IStateMachineTarget master, GravitasCreatureManipulator.Def def) : base(master, def)
		{
			this.pickupCell = Grid.OffsetCell(Grid.PosToCell(master.gameObject), base.smi.def.pickupOffset);
			this.m_partitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator", base.gameObject, this.pickupCell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.DetectCreature));
			this.m_largeCreaturePartitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator.large", base.gameObject, Grid.CellLeft(this.pickupCell), GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.DetectLargeCreature));
			this.m_progressMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TileFront, Array.Empty<string>());
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x0024C548 File Offset: 0x0024A748
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateStatusItems();
			this.UpdateMeter();
			StoryManager.Instance.ForceCreateStory(Db.Get().Stories.CreatureManipulator, base.gameObject.GetMyWorldId());
			if (this.ScannedSpecies.Count >= base.smi.def.numSpeciesToUnlockMorphMode)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
			this.TryShowCompletedNotification();
			base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.CreatureManipulator);
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x000CD107 File Offset: 0x000CB307
		public override void StopSM(string reason)
		{
			base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			base.StopSM(reason);
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x000CD127 File Offset: 0x000CB327
		private void OnBuildingSelect(object obj)
		{
			if (!(bool)obj)
			{
				return;
			}
			if (!this.m_introPopupSeen)
			{
				this.ShowIntroNotification();
			}
			if (this.m_endNotification != null)
			{
				this.m_endNotification.customClickCallback(this.m_endNotification.customClickData);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x000CD163 File Offset: 0x000CB363
		public bool IsMorphMode
		{
			get
			{
				return this.m_morphModeUnlocked;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600459B RID: 17819 RVA: 0x000CD16B File Offset: 0x000CB36B
		public bool IsCritterStored
		{
			get
			{
				return this.m_storage.Count > 0;
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x0024C5F8 File Offset: 0x0024A7F8
		private void UpdateStatusItems()
		{
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorProgress, !this.IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphMode, this.IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphModeLocked, !this.IsMorphMode, this);
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x000CD17B File Offset: 0x000CB37B
		public void UpdateMeter()
		{
			this.m_progressMeter.SetPositionPercent(Mathf.Clamp01((float)this.ScannedSpecies.Count / (float)base.smi.def.numSpeciesToUnlockMorphMode));
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x0024C66C File Offset: 0x0024A86C
		public bool IsAccepted(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			return component.HasTag(GameTags.Creature) && !component.HasTag(GameTags.Robot) && component.PrefabTag != GameTags.Creature;
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x0024C6AC File Offset: 0x0024A8AC
		private void DetectLargeCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable == null)
			{
				return;
			}
			if (pickupable.GetComponent<KCollider2D>().bounds.size.x > 1.5f)
			{
				this.DetectCreature(obj);
			}
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0024C6F0 File Offset: 0x0024A8F0
		private void DetectCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && this.IsAccepted(pickupable.gameObject) && base.smi.sm.creatureTarget.IsNull(base.smi) && base.smi.IsInsideState(base.smi.sm.operational.idle))
			{
				this.SetCritterTarget(pickupable.gameObject);
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x000CD1AB File Offset: 0x000CB3AB
		public void SetCritterTarget(GameObject go)
		{
			base.smi.sm.creatureTarget.Set(go.gameObject, base.smi, false);
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x0024C768 File Offset: 0x0024A968
		public void StoreCreature()
		{
			GameObject go = base.smi.sm.creatureTarget.Get(base.smi);
			this.m_storage.Store(go, false, false, true, false);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x0024C7A4 File Offset: 0x0024A9A4
		public void DropCritter()
		{
			List<GameObject> list = new List<GameObject>();
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.smi), Grid.SceneLayer.Creatures);
			this.m_storage.DropAll(position, false, false, base.smi.def.dropOffset.ToVector3(), true, list);
			foreach (GameObject gameObject in list)
			{
				CreatureBrain component = gameObject.GetComponent<CreatureBrain>();
				if (!(component == null))
				{
					this.Scan(component.species);
					if (component.HasTag(GameTags.OriginalCreature) && this.IsMorphMode)
					{
						this.SpawnMorph(component);
					}
					else
					{
						gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("idle_loop");
					}
				}
			}
			base.smi.sm.creatureTarget.Set(null, base.smi);
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x000CD1D0 File Offset: 0x000CB3D0
		private void Scan(Tag species)
		{
			if (this.ScannedSpecies.Add(species))
			{
				base.gameObject.Trigger(1980521255, null);
				this.UpdateStatusItems();
				this.UpdateMeter();
				this.ShowCritterScannedNotification(species);
			}
			this.TryShowCompletedNotification();
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x0024C89C File Offset: 0x0024AA9C
		public void SpawnMorph(Brain brain)
		{
			Tag tag = Tag.Invalid;
			BabyMonitor.Instance smi = brain.GetSMI<BabyMonitor.Instance>();
			FertilityMonitor.Instance smi2 = brain.GetSMI<FertilityMonitor.Instance>();
			bool flag = smi != null;
			bool flag2 = smi2 != null;
			if (flag2)
			{
				tag = FertilityMonitor.EggBreedingRoll(smi2.breedingChances, true);
			}
			else if (flag)
			{
				FertilityMonitor.Def def = Assets.GetPrefab(smi.def.adultPrefab).GetDef<FertilityMonitor.Def>();
				if (def == null)
				{
					return;
				}
				tag = FertilityMonitor.EggBreedingRoll(def.initialBreedingWeights, true);
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
			gameObject.SetActive(true);
			gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			foreach (AmountInstance amountInstance in brain.gameObject.GetAmounts())
			{
				AmountInstance amountInstance2 = amountInstance.amount.Lookup(gameObject);
				if (amountInstance2 != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					amountInstance2.value = num * amountInstance2.GetMax();
				}
			}
			gameObject.Trigger(-2027483228, brain.gameObject);
			KSelectable component = brain.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			}
			base.smi.sm.cooldownTimer.Set(base.smi.def.cooldownDuration, base.smi, false);
			brain.gameObject.DeleteObject();
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x0024CA9C File Offset: 0x0024AC9C
		public void ShowIntroNotification()
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_initial", true);
			this.m_introPopupSeen = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "crittermanipulatoractivate_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, null));
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x0024CAF8 File Offset: 0x0024ACF8
		public void ShowCritterScannedNotification(Tag species)
		{
			GravitasCreatureManipulator.Instance.<>c__DisplayClass29_0 CS$<>8__locals1 = new GravitasCreatureManipulator.Instance.<>c__DisplayClass29_0();
			CS$<>8__locals1.species = species;
			CS$<>8__locals1.<>4__this = this;
			string unlockID = GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(CS$<>8__locals1.species);
			Game.Instance.unlocks.Unlock(unlockID, false);
			CS$<>8__locals1.<ShowCritterScannedNotification>g__ShowCritterScannedNotificationAndWaitForClick|1().Then(delegate
			{
				GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(CS$<>8__locals1.species);
			});
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x0024CB50 File Offset: 0x0024AD50
		public static void ShowLoreUnlockedPopup(Tag species)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.NAME).AddDefaultOK(false);
			bool flag = CodexCache.GetEntryForLock(GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species)) != null;
			Option<string> bodyContentForSpeciesTag = GravitasCreatureManipulatorConfig.GetBodyContentForSpeciesTag(species);
			if (flag && bodyContentForSpeciesTag.HasValue)
			{
				infoDialogScreen.AddPlainText(bodyContentForSpeciesTag.Value).AddOption(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITCRITTERMANIPULATOR"), false);
				return;
			}
			infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x0024CBD0 File Offset: 0x0024ADD0
		public void TryShowCompletedNotification()
		{
			if (this.ScannedSpecies.Count < base.smi.def.numSpeciesToUnlockMorphMode)
			{
				return;
			}
			if (this.IsMorphMode)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.BUTTON, "crittermanipulatormorphmode_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			this.m_endNotification = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.UnlockMorphMode));
			base.gameObject.AddOrGet<Notifier>().Add(this.m_endNotification, "");
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0024CC94 File Offset: 0x0024AE94
		public void ClearEndNotification()
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_endNotification = null;
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x0024CCE8 File Offset: 0x0024AEE8
		public void UnlockMorphMode(object _)
		{
			if (this.m_morphModeUnlocked)
			{
				return;
			}
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_complete", true);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_morphModeUnlocked = true;
			this.UpdateStatusItems();
			this.ClearEndNotification();
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, base.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data
			{
				WorldId = base.smi.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnStorySequenceComplete),
				CanCompleteCB = null
			});
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x0024CDEC File Offset: 0x0024AFEC
		private void OnStorySequenceComplete()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, keepsakeSpawnPosition);
			this.eventInfo = null;
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x000CD20A File Offset: 0x000CB40A
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.m_partitionEntry);
			GameScenePartitioner.Instance.Free(ref this.m_largeCreaturePartitionEntry);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
		}

		// Token: 0x0400300F RID: 12303
		public int pickupCell;

		// Token: 0x04003010 RID: 12304
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04003011 RID: 12305
		[Serialize]
		public HashSet<Tag> ScannedSpecies = new HashSet<Tag>();

		// Token: 0x04003012 RID: 12306
		[Serialize]
		private bool m_introPopupSeen;

		// Token: 0x04003013 RID: 12307
		[Serialize]
		private bool m_morphModeUnlocked;

		// Token: 0x04003014 RID: 12308
		private EventInfoData eventInfo;

		// Token: 0x04003015 RID: 12309
		private Notification m_endNotification;

		// Token: 0x04003016 RID: 12310
		private MeterController m_progressMeter;

		// Token: 0x04003017 RID: 12311
		private HandleVector<int>.Handle m_partitionEntry;

		// Token: 0x04003018 RID: 12312
		private HandleVector<int>.Handle m_largeCreaturePartitionEntry;
	}
}
