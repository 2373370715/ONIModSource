using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class LonelyMinionHouse : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>
{
	public class Def : TraitDef
	{
	}

	public new class Instance : TraitInstance, ICheckboxListGroupControl
	{
		private KAnimLink lightsLink;

		private HashedString questOwnerId;

		private LonelyMinion.Instance lonelyMinion;

		private KBatchedAnimController[] animControllers;

		private Light2D light;

		private FilteredStorage storageFilter;

		private MeterController meter;

		private MeterController blinds;

		private Workable.WorkableEvent currentWorkState = Workable.WorkableEvent.WorkStopped;

		private Notification knockNotification;

		private KBatchedAnimController knocker;

		public HashedString QuestOwnerId => questOwnerId;

		public KBatchedAnimController AnimController => animControllers[0];

		public KBatchedAnimController LightsController => animControllers[1];

		public KBatchedAnimController BlindsController => blinds.meterController;

		public Light2D Light => light;

		public string Title => CODEX.STORY_TRAITS.LONELYMINION.NAME;

		public string Description => CODEX.STORY_TRAITS.LONELYMINION.DESCRIPTION_BUILDINGMENU;

		public Instance(StateMachineController master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			animControllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>(includeInactive: true);
			light = LightsController.GetComponent<Light2D>();
			light.transform.position += Vector3.forward * Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
			light.gameObject.SetActive(value: true);
			lightsLink = new KAnimLink(AnimController, LightsController);
			Activatable component = GetComponent<Activatable>();
			component.SetOffsets(new CellOffset[1]
			{
				new CellOffset(-3, 0)
			});
			if (!component.IsActivated)
			{
				component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkStateChanged));
				component.onActivate = (System.Action)Delegate.Combine(component.onActivate, new System.Action(StartStoryTrait));
			}
			meter = new MeterController(AnimController, "meter_storage_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.METER_SYMBOLS);
			blinds = new MeterController(AnimController, "blinds_target", string.Format("{0}_{1}", "meter_blinds", 0), Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.BLINDS_SYMBOLS);
			KPrefabID component2 = GetComponent<KPrefabID>();
			questOwnerId = new HashedString(component2.PrefabTag.GetHash());
			SpawnMinion();
			if (lonelyMinion != null && !TryFindMailbox())
			{
				GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], OnBuildingLayerChanged);
			}
			QuestManager.InitializeQuest(questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			QuestInstance questInstance = QuestManager.InitializeQuest(questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			QuestInstance questInstance2 = QuestManager.InitializeQuest(questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			QuestInstance questInstance3 = QuestManager.InitializeQuest(questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			NonEssentialEnergyConsumer component3 = GetComponent<NonEssentialEnergyConsumer>();
			component3.PoweredStateChanged = (Action<bool>)Delegate.Combine(component3.PoweredStateChanged, new Action<bool>(OnPoweredStateChanged));
			OnPoweredStateChanged(component3.IsPowered);
			if (lonelyMinion == null)
			{
				base.StartSM();
				return;
			}
			Subscribe(-592767678, OnBuildingActivated);
			base.StartSM();
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			questInstance2.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance2.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			questInstance3.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance3.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			float num = base.sm.QuestProgress.Get(this) * 3f;
			int num2 = (Mathf.Approximately(num, Mathf.Ceil(num)) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num));
			if (num2 != 0)
			{
				HashedString[] array = new HashedString[num2];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = string.Format("{0}_{1}", "meter_blinds", i);
				}
				blinds.meterController.Play(array);
			}
		}

		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Activatable component = GetComponent<Activatable>();
			component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkStateChanged));
			component.onActivate = (System.Action)Delegate.Remove(component.onActivate, new System.Action(StartStoryTrait));
			Unsubscribe(-592767678, OnBuildingActivated);
		}

		private void OnQuestProgressChanged(QuestInstance quest, Quest.State prevState, float delta)
		{
			float num = base.sm.QuestProgress.Get(this);
			num += delta / 3f;
			if (1f - num <= 0.001f)
			{
				num = 1f;
			}
			base.sm.QuestProgress.Set(Mathf.Clamp01(num), this, silenceEvents: true);
			lonelyMinion.UnlockQuestIdle(quest, prevState, delta);
			lonelyMinion.ShowQuestCompleteNotification(quest, prevState);
			base.gameObject.Trigger(1980521255);
			if (quest.IsComplete)
			{
				if (num == 1f)
				{
					base.sm.CompleteStory.Trigger(this);
				}
				float num2 = num * 3f;
				int num3 = (Mathf.Approximately(num2, Mathf.Ceil(num2)) ? Mathf.CeilToInt(num2) : Mathf.FloorToInt(num2));
				blinds.meterController.Queue(string.Format("{0}_{1}", "meter_blinds", num3 - 1));
			}
		}

		public void MailboxContentChanged(GameObject item)
		{
			lonelyMinion.sm.Mail.Set(item, lonelyMinion);
		}

		public override void CompleteEvent()
		{
			if (lonelyMinion == null)
			{
				base.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop);
				base.gameObject.AddOrGet<TreeFilterable>();
				base.gameObject.AddOrGet<BuildingEnabledButton>();
				base.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
				base.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
				base.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
				Storage component = GetComponent<Storage>();
				component.allowItemRemoval = true;
				component.showInUI = true;
				component.showDescriptor = true;
				component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkStateChanged));
				storageFilter = new FilteredStorage(base.smi.GetComponent<KPrefabID>(), null, null, use_logic_meter: false, Db.Get().ChoreTypes.StorageFetch);
				storageFilter.SetMeter(meter);
				meter = null;
				RootMenu.Instance.Refresh();
				component.RenotifyAll();
				return;
			}
			List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
			list.Shuffle();
			int num = 3;
			base.def.EventCompleteInfo.Minions = new GameObject[1 + Mathf.Min(num, list.Count)];
			base.def.EventCompleteInfo.Minions[0] = lonelyMinion.gameObject;
			for (int i = 0; i < list.Count; i++)
			{
				if (num <= 0)
				{
					break;
				}
				base.def.EventCompleteInfo.Minions[i + 1] = list[i].gameObject;
				num--;
			}
			base.CompleteEvent();
		}

		public override void OnCompleteStorySequence()
		{
			SpawnMinion();
			Unsubscribe(-592767678, OnBuildingActivated);
			base.OnCompleteStorySequence();
			QuestInstance instance = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			QuestInstance instance2 = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			instance2.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance2.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			QuestInstance instance3 = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			instance3.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance3.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(OnQuestProgressChanged));
			blinds.meterController.Play(blinds.meterController.initialAnim, blinds.meterController.initialMode);
			base.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop);
			base.gameObject.AddOrGet<TreeFilterable>();
			base.gameObject.AddOrGet<BuildingEnabledButton>();
			base.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
			base.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
			base.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
			Storage component = GetComponent<Storage>();
			component.allowItemRemoval = true;
			component.showInUI = true;
			component.showDescriptor = true;
			component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkStateChanged));
			storageFilter = new FilteredStorage(base.smi.GetComponent<KPrefabID>(), null, null, use_logic_meter: false, Db.Get().ChoreTypes.StorageFetch);
			storageFilter.SetMeter(meter);
			meter = null;
			RootMenu.Instance.Refresh();
		}

		private void SpawnMinion()
		{
			if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.LonelyMinion))
			{
				return;
			}
			if (lonelyMinion == null)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(LonelyMinionConfig.ID), base.gameObject);
				Debug.Assert(gameObject != null);
				gameObject.transform.localPosition = new Vector3(0.54f, 0f, -0.01f);
				gameObject.SetActive(value: true);
				Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.gameObject));
				BuildingDef buildingDef = GetComponent<Building>().Def;
				lonelyMinion = gameObject.GetSMI<LonelyMinion.Instance>();
				lonelyMinion.def.QuestOwnerId = questOwnerId;
				lonelyMinion.def.DecorInspectionArea = new Extents(vector2I.x - Mathf.CeilToInt((float)buildingDef.WidthInCells / 2f) + 1, vector2I.y, buildingDef.WidthInCells, buildingDef.HeightInCells);
				return;
			}
			MinionStartingStats minionStartingStats = new MinionStartingStats(lonelyMinion.def.Personality, null, "AncientKnowledge");
			minionStartingStats.Traits.Add(Db.Get().traits.TryGet("Chatty"));
			minionStartingStats.voiceIdx = -2;
			string[] aLL_ATTRIBUTES = DUPLICANTSTATS.ALL_ATTRIBUTES;
			for (int i = 0; i < aLL_ATTRIBUTES.Length; i++)
			{
				minionStartingStats.StartingLevels[aLL_ATTRIBUTES[i]] += 7;
			}
			UnityEngine.Object.Destroy(lonelyMinion.gameObject);
			lonelyMinion = null;
			MinionIdentity minionIdentity = Util.KInstantiate<MinionIdentity>(Assets.GetPrefab(MinionConfig.ID));
			Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
			minionIdentity.gameObject.SetActive(value: true);
			minionStartingStats.Apply(minionIdentity.gameObject);
			minionIdentity.arrivalTime += UnityEngine.Random.Range(2190, 3102);
			minionIdentity.arrivalTime *= -1f;
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			for (int j = 0; j < 3; j++)
			{
				component.ForceAddSkillPoint();
			}
			Vector3 position = base.transform.position + Vector3.left * Grid.CellSizeInMeters * 2f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			minionIdentity.transform.SetPosition(position);
		}

		private bool TryFindMailbox()
		{
			if (base.sm.QuestProgress.Get(this) == 1f)
			{
				return true;
			}
			int cell = Grid.PosToCell(base.gameObject);
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(new Extents(cell, 10), GameScenePartitioner.Instance.objectLayers[1], pooledList);
			bool flag = false;
			int num = 0;
			while (!flag && num < pooledList.Count)
			{
				if ((pooledList[num].obj as GameObject).GetComponent<KPrefabID>().PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
				{
					OnBuildingLayerChanged(0, pooledList[num].obj);
					flag = true;
				}
				num++;
			}
			pooledList.Recycle();
			return flag;
		}

		private void OnBuildingLayerChanged(int cell, object data)
		{
			GameObject gameObject = data as GameObject;
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (component.PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
				{
					component.GetComponent<LonelyMinionMailbox>().Initialize(this);
					GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], OnBuildingLayerChanged);
				}
			}
		}

		public void OnPoweredStateChanged(bool isPowered)
		{
			light.enabled = isPowered && GetComponent<Operational>().IsOperational;
			LightsController.Play(light.enabled ? LonelyMinionHouseConfig.LIGHTS_ON : LonelyMinionHouseConfig.LIGHTS_OFF, KAnim.PlayMode.Loop);
		}

		private void StartStoryTrait()
		{
			TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		protected override void OnBuildingActivated(object data)
		{
			if (IsIntroSequenceComplete())
			{
				bool isActivated = GetComponent<Activatable>().IsActivated;
				if (lonelyMinion != null)
				{
					lonelyMinion.sm.Active.Set(isActivated && GetComponent<Operational>().IsOperational, lonelyMinion);
				}
				if (isActivated && base.sm.QuestProgress.Get(this) < 1f)
				{
					GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.AllPower;
				}
			}
		}

		protected override void OnObjectSelect(object clicked)
		{
			if (!(bool)clicked)
			{
				return;
			}
			if (knockNotification != null)
			{
				knocker.gameObject.Unsubscribe(-1503271301, OnObjectSelect);
				knockNotification.Clear();
				knockNotification = null;
				PlayIntroSequence();
				return;
			}
			if (!StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN))
			{
				int count = Components.LiveMinionIdentities.Count;
				int idx = UnityEngine.Random.Range(0, count);
				base.def.EventIntroInfo.Minions = new GameObject[2]
				{
					lonelyMinion.gameObject,
					(count == 0) ? null : Components.LiveMinionIdentities[idx].gameObject
				};
			}
			base.OnObjectSelect(clicked);
		}

		private void OnWorkStateChanged(Workable w, Workable.WorkableEvent state)
		{
			Activatable activatable = w as Activatable;
			if (activatable != null)
			{
				if (state == Workable.WorkableEvent.WorkStarted)
				{
					knocker = w.worker.GetComponent<KBatchedAnimController>();
					knocker.gameObject.Subscribe(-1503271301, OnObjectSelect);
					knockNotification = new Notification(CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TEXT, NotificationType.Event, null, null, expires: false, 0f, PlayIntroSequence, null, null, volume_attenuation: true, clear_on_click: true);
					base.gameObject.AddOrGet<Notifier>().Add(knockNotification);
					GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
				}
				if (state == Workable.WorkableEvent.WorkStopped)
				{
					if (currentWorkState == Workable.WorkableEvent.WorkStarted)
					{
						if (knockNotification != null)
						{
							GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
							knockNotification.Clear();
							knockNotification = null;
						}
						FocusTargetSequence.Cancel(base.master);
						knocker.gameObject.Unsubscribe(-1503271301, OnObjectSelect);
						knocker = null;
					}
					if (currentWorkState == Workable.WorkableEvent.WorkCompleted)
					{
						activatable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(activatable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkStateChanged));
						activatable.onActivate = (System.Action)Delegate.Remove(activatable.onActivate, new System.Action(StartStoryTrait));
					}
				}
				currentWorkState = state;
			}
			else if (state == Workable.WorkableEvent.WorkStopped)
			{
				AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORK_PST);
				AnimController.Queue(LonelyMinionHouseConfig.STORAGE);
			}
			else
			{
				bool flag = AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[0] || AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[1];
				if (state == Workable.WorkableEvent.WorkStarted && !flag)
				{
					AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORKING, KAnim.PlayMode.Loop);
				}
			}
		}

		private void ReleaseKnocker(object _)
		{
			Navigator component = knocker.GetComponent<Navigator>();
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			knocker.RemoveAnimOverrides(GetComponent<Activatable>().overrideAnims[0]);
			knocker.Play(navTypeData.idleAnim);
			blinds.meterController.Play(blinds.meterController.initialAnim, blinds.meterController.initialMode);
			lonelyMinion.AnimController.Play(lonelyMinion.AnimController.defaultAnim, lonelyMinion.AnimController.initialMode);
			knocker.gameObject.Unsubscribe(-1061186183, ReleaseKnocker);
			knocker.GetComponent<Brain>().Reset("knock sequence");
			knocker = null;
		}

		private void PlayIntroSequence(object _ = null)
		{
			GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.gameObject), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
			FocusTargetSequence.Start(base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 2f,
				TargetSize = 6f,
				Target = target,
				PopupData = null,
				CompleteCB = OnIntroSequenceComplete,
				CanCompleteCB = IsIntroSequenceComplete
			});
			GetComponent<KnockKnock>().AnswerDoor();
			knockNotification = null;
		}

		private void OnIntroSequenceComplete()
		{
			OnBuildingActivated(null);
			QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest).TrackProgress(new Quest.ItemData
			{
				CriteriaId = LonelyMinionConfig.GreetingCriteraId
			}, out var _, out var _);
		}

		private bool IsIntroSequenceComplete()
		{
			if (currentWorkState == Workable.WorkableEvent.WorkStarted)
			{
				return false;
			}
			if (currentWorkState == Workable.WorkableEvent.WorkStopped && knocker != null && knocker.currentAnim != LonelyMinionHouseConfig.ANSWER)
			{
				knocker.GetComponent<Brain>().Stop("knock sequence");
				knocker.gameObject.Subscribe(-1061186183, ReleaseKnocker);
				knocker.AddAnimOverrides(GetComponent<Activatable>().overrideAnims[0]);
				knocker.Play(LonelyMinionHouseConfig.ANSWER);
				lonelyMinion.AnimController.Play(LonelyMinionHouseConfig.ANSWER);
				blinds.meterController.Play(LonelyMinionHouseConfig.ANSWER);
			}
			if (currentWorkState == Workable.WorkableEvent.WorkStopped)
			{
				return knocker == null;
			}
			return false;
		}

		public Vector3 GetParcelPosition()
		{
			int index = -1;
			KAnimFileData data = Assets.GetAnim("anim_interacts_lonely_dupe_kanim").GetData();
			for (int i = 0; i < data.animCount; i++)
			{
				if (data.GetAnim(i).hash == LonelyMinionConfig.CHECK_MAIL)
				{
					index = data.GetAnim(i).firstFrameIdx;
					break;
				}
			}
			List<KAnim.Anim.FrameElement> frameElements = lonelyMinion.AnimController.GetBatch().group.data.frameElements;
			lonelyMinion.AnimController.GetBatch().group.data.TryGetFrame(index, out var frame);
			bool flag = false;
			Matrix2x3 matrix2x = default(Matrix2x3);
			int num = 0;
			while (!flag && num < frame.numElements)
			{
				if (frameElements[frame.firstElementIdx + num].symbol == LonelyMinionConfig.PARCEL_SNAPTO)
				{
					flag = true;
					matrix2x = frameElements[frame.firstElementIdx + num].transform;
					break;
				}
				num++;
			}
			Vector3 result = Vector3.zero;
			if (flag)
			{
				Matrix4x4 matrix4x = lonelyMinion.AnimController.GetTransformMatrix();
				result = (matrix4x * matrix2x).GetColumn(3);
			}
			return result;
		}

		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			QuestInstance greetingQuest = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			if (greetingQuest.IsComplete)
			{
				QuestInstance foodQuest = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
				QuestInstance decorQuest = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
				QuestInstance powerQuest = QuestManager.GetInstance(questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
				return new ICheckboxListGroupControl.ListGroup[4]
				{
					new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(), (string title) => ResolveQuestTitle(title, greetingQuest)),
					new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionFoodQuest.Title, foodQuest.GetCheckBoxData(ResolveQuestToolTips), (string title) => ResolveQuestTitle(title, foodQuest)),
					new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionDecorQuest.Title, decorQuest.GetCheckBoxData(ResolveQuestToolTips), (string title) => ResolveQuestTitle(title, decorQuest)),
					new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionPowerQuest.Title, powerQuest.GetCheckBoxData(ResolveQuestToolTips), (string title) => ResolveQuestTitle(title, powerQuest))
				};
			}
			return new ICheckboxListGroupControl.ListGroup[1]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(), (string title) => ResolveQuestTitle(title, greetingQuest))
			};
		}

		private string ResolveQuestTitle(string title, QuestInstance quest)
		{
			string text = GameUtil.FloatToString(quest.CurrentProgress * 100f, "##0") + UI.UNITSUFFIXES.PERCENT;
			return title + " - " + text;
		}

		private string ResolveQuestToolTips(int criteriaId, string toolTip, QuestInstance quest)
		{
			if (criteriaId == LonelyMinionConfig.FoodCriteriaId.HashValue)
			{
				int quality = (int)quest.GetTargetValue(LonelyMinionConfig.FoodCriteriaId);
				int targetCount = quest.GetTargetCount(LonelyMinionConfig.FoodCriteriaId);
				string text = string.Empty;
				for (int i = 0; i < targetCount; i++)
				{
					Tag satisfyingItem = quest.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, i);
					if (!satisfyingItem.IsValid)
					{
						break;
					}
					text = text + "    • " + TagManager.GetProperName(satisfyingItem);
					if (targetCount - i != 1)
					{
						text += "\n";
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = string.Format("{0}{1}", "    • ", CODEX.QUESTS.CRITERIA.FOODQUALITY.NONE);
				}
				return string.Format(toolTip, GameUtil.GetFormattedFoodQuality(quality), text);
			}
			if (criteriaId == LonelyMinionConfig.DecorCriteriaId.HashValue)
			{
				int num = (int)quest.GetTargetValue(LonelyMinionConfig.DecorCriteriaId);
				float num2 = CalculateAverageDecor(lonelyMinion.def.DecorInspectionArea);
				return string.Format(toolTip, num, num2);
			}
			float f = quest.GetTargetValue(LonelyMinionConfig.PowerCriteriaId) - quest.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId);
			return string.Format(toolTip, Mathf.CeilToInt(f));
		}

		public bool SidescreenEnabled()
		{
			if (StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN))
			{
				return !StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion);
			}
			return false;
		}

		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}
	}

	public class ActiveStates : State
	{
		public State StoryComplete;

		public static void OnEnterStoryComplete(Instance smi)
		{
			smi.CompleteEvent();
		}
	}

	public State Inactive;

	public ActiveStates Active;

	public Signal MailDelivered;

	public Signal CompleteStory;

	public FloatParameter QuestProgress;

	private bool ValidateOperationalTransition(Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Active);
		if (component != null)
		{
			return flag != component.IsOperational;
		}
		return false;
	}

	private static bool AllQuestsComplete(Instance smi)
	{
		return 1f - smi.sm.QuestProgress.Get(smi) <= Mathf.Epsilon;
	}

	public static void EvaluateLights(Instance smi, float dt)
	{
		bool num = smi.IsInsideState(smi.sm.Active);
		QuestInstance instance = QuestManager.GetInstance(smi.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
		if (num && smi.Light.enabled && !instance.IsComplete)
		{
			instance.TrackProgress(new Quest.ItemData
			{
				CriteriaId = LonelyMinionConfig.PowerCriteriaId,
				CurrentValue = instance.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId) + dt
			}, out var _, out var _);
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Inactive;
		base.serializable = SerializeType.ParamsOnly;
		root.Update(EvaluateLights, UpdateRate.SIM_1000ms);
		Inactive.EventTransition(GameHashes.OperationalChanged, Active, ValidateOperationalTransition);
		Active.Enter(delegate(Instance smi)
		{
			smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered);
		}).Exit(delegate(Instance smi)
		{
			smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered);
		}).OnSignal(CompleteStory, Active.StoryComplete, AllQuestsComplete)
			.EventTransition(GameHashes.OperationalChanged, Inactive, ValidateOperationalTransition);
		Active.StoryComplete.Enter(ActiveStates.OnEnterStoryComplete);
	}

	public static float CalculateAverageDecor(Extents area)
	{
		float num = 0f;
		int cell = Grid.XYToCell(area.x, area.y);
		for (int i = 0; i < area.width * area.height; i++)
		{
			int num2 = Grid.OffsetCell(cell, i % area.width, i / area.width);
			num += Grid.Decor[num2];
		}
		return num / (float)(area.width * area.height);
	}
}
