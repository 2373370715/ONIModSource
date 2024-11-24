using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class StandardWorker : WorkerBase
{
	// Token: 0x0600351A RID: 13594 RVA: 0x000C2A0A File Offset: 0x000C0C0A
	public override WorkerBase.State GetState()
	{
		return this.state;
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x000C2A12 File Offset: 0x000C0C12
	public override WorkerBase.StartWorkInfo GetStartWorkInfo()
	{
		return this.startWorkInfo;
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x000C2A1A File Offset: 0x000C0C1A
	public override Workable GetWorkable()
	{
		if (this.startWorkInfo != null)
		{
			return this.startWorkInfo.workable;
		}
		return null;
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x000C2A31 File Offset: 0x000C0C31
	public override KBatchedAnimController GetAnimController()
	{
		return base.GetComponent<KBatchedAnimController>();
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x000C2A39 File Offset: 0x000C0C39
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		return base.GetComponent<AttributeConverters>().GetConverter(id);
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x000C2A47 File Offset: 0x000C0C47
	public override Guid OfferStatusItem(StatusItem item, object data = null)
	{
		return base.GetComponent<KSelectable>().AddStatusItem(item, data);
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x000C2A56 File Offset: 0x000C0C56
	public override void RevokeStatusItem(Guid id)
	{
		base.GetComponent<KSelectable>().RemoveStatusItem(id, false);
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x000C2A66 File Offset: 0x000C0C66
	public override void SetWorkCompleteData(object data)
	{
		this.workCompleteData = data;
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x000C2A6F File Offset: 0x000C0C6F
	public override bool UsesMultiTool()
	{
		return this.usesMultiTool;
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x000C2A77 File Offset: 0x000C0C77
	public override bool IsFetchDrone()
	{
		return this.isFetchDrone;
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x000C2A7F File Offset: 0x000C0C7F
	public override CellOffset[] GetFetchCellOffsets()
	{
		if (this.fetchOffsets.Length == 0)
		{
			return null;
		}
		return this.fetchOffsets;
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x000C2A92 File Offset: 0x000C0C92
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.state = WorkerBase.State.Idle;
		base.Subscribe<StandardWorker>(1485595942, StandardWorker.OnChoreInterruptDelegate);
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x000C2AB2 File Offset: 0x000C0CB2
	private string GetWorkableDebugString()
	{
		if (this.GetWorkable() == null)
		{
			return "Null";
		}
		return this.GetWorkable().name;
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x0020D2DC File Offset: 0x0020B4DC
	public void CompleteWork()
	{
		this.successFullyCompleted = false;
		this.state = WorkerBase.State.Idle;
		Workable workable = this.GetWorkable();
		if (workable != null)
		{
			if (workable.triggerWorkReactions && workable.GetWorkTime() > 30f)
			{
				string conversationTopic = workable.GetConversationTopic();
				if (!conversationTopic.IsNullOrWhiteSpace())
				{
					this.CreateCompletionReactable(conversationTopic);
				}
			}
			this.DetachAnimOverrides();
			workable.CompleteWork(this);
			if (workable.worker != null && !(workable is Constructable) && !(workable is Deconstructable) && !(workable is Repairable) && !(workable is Disinfectable))
			{
				BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
				gameplayEventData.workable = workable;
				gameplayEventData.worker = workable.worker;
				gameplayEventData.building = workable.GetComponent<BuildingComplete>();
				gameplayEventData.eventTrigger = GameHashes.UseBuilding;
				GameplayEventManager.Instance.Trigger(1175726587, gameplayEventData);
			}
		}
		this.InternalStopWork(workable, false);
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x0020D3B8 File Offset: 0x0020B5B8
	protected virtual void TryPlayingIdle()
	{
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			if (navTypeData.idleAnim.IsValid)
			{
				base.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x0020D414 File Offset: 0x0020B614
	public override WorkerBase.WorkResult Work(float dt)
	{
		if (this.state == WorkerBase.State.PendingCompletion)
		{
			bool flag = Time.time - this.workPendingCompletionTime > 10f;
			if (!base.GetComponent<KAnimControllerBase>().IsStopped() && !flag)
			{
				return WorkerBase.WorkResult.InProgress;
			}
			this.TryPlayingIdle();
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
		else
		{
			if (this.state != WorkerBase.State.Completing)
			{
				Workable workable = this.GetWorkable();
				if (workable != null)
				{
					if (this.facing)
					{
						if (workable.ShouldFaceTargetWhenWorking())
						{
							this.facing.Face(workable.GetFacingTarget());
						}
						else
						{
							Rotatable component = workable.GetComponent<Rotatable>();
							bool flag2 = component != null && component.GetOrientation() == Orientation.FlipH;
							Vector3 vector = this.facing.transform.GetPosition();
							vector += (flag2 ? Vector3.left : Vector3.right);
							this.facing.Face(vector);
						}
					}
					if (dt > 0f && Game.Instance.FastWorkersModeActive)
					{
						dt = Mathf.Min(workable.WorkTimeRemaining + 0.01f, 5f);
					}
					Klei.AI.Attribute workAttribute = workable.GetWorkAttribute();
					AttributeLevels component2 = base.GetComponent<AttributeLevels>();
					if (workAttribute != null && workAttribute.IsTrainable && component2 != null)
					{
						float attributeExperienceMultiplier = workable.GetAttributeExperienceMultiplier();
						component2.AddExperience(workAttribute.Id, dt, attributeExperienceMultiplier);
					}
					string skillExperienceSkillGroup = workable.GetSkillExperienceSkillGroup();
					if (this.experienceRecipient != null && skillExperienceSkillGroup != null)
					{
						float skillExperienceMultiplier = workable.GetSkillExperienceMultiplier();
						this.experienceRecipient.AddExperienceWithAptitude(skillExperienceSkillGroup, dt, skillExperienceMultiplier);
					}
					float efficiencyMultiplier = workable.GetEfficiencyMultiplier(this);
					float dt2 = dt * efficiencyMultiplier * 1f;
					if (workable.WorkTick(this, dt2) && this.state == WorkerBase.State.Working)
					{
						this.successFullyCompleted = true;
						this.StartPlayingPostAnim();
						workable.OnPendingCompleteWork(this);
					}
				}
				return WorkerBase.WorkResult.InProgress;
			}
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x0020D5F8 File Offset: 0x0020B7F8
	private void StartPlayingPostAnim()
	{
		Workable workable = this.GetWorkable();
		if (workable != null && !workable.alwaysShowProgressBar)
		{
			workable.ShowProgressBar(false);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
		this.state = WorkerBase.State.PendingCompletion;
		this.workPendingCompletionTime = Time.time;
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		HashedString[] workPstAnims = workable.GetWorkPstAnims(this, this.successFullyCompleted);
		if (this.smi == null)
		{
			if (workPstAnims != null && workPstAnims.Length != 0)
			{
				if (workable != null && workable.synchronizeAnims)
				{
					KAnimControllerBase animController = workable.GetAnimController();
					if (animController != null)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
					}
				}
				else
				{
					component.Play(workPstAnims, KAnim.PlayMode.Once);
				}
			}
			else
			{
				this.state = WorkerBase.State.Completing;
			}
		}
		base.Trigger(-1142962013, this);
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x0020D6B4 File Offset: 0x0020B8B4
	protected virtual void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		this.state = WorkerBase.State.Idle;
		base.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
		base.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
		this.workAnimOffset = Vector3.zero;
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
		this.DetachAnimOverrides();
		this.ClearPasserbyReactable();
		AnimEventHandler component = base.GetComponent<AnimEventHandler>();
		if (component)
		{
			component.ClearContext();
		}
		if (this.previousStatusItem.item != null)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
		}
		if (target_workable != null)
		{
			target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
			target_workable.StopWork(this, is_aborted);
		}
		if (this.smi != null)
		{
			this.smi.StopSM("stopping work");
			this.smi = null;
		}
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
		this.startWorkInfo = null;
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x000C2AD3 File Offset: 0x000C0CD3
	private void OnChoreInterrupt(object data)
	{
		if (this.state == WorkerBase.State.Working)
		{
			this.successFullyCompleted = false;
			this.StartPlayingPostAnim();
		}
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x0020D7D8 File Offset: 0x0020B9D8
	private void OnWorkChoreDisabled(object data)
	{
		string text = data as string;
		ChoreConsumer component = base.GetComponent<ChoreConsumer>();
		if (component != null && component.choreDriver != null)
		{
			component.choreDriver.GetCurrentChore().Fail((text != null) ? text : "WorkChoreDisabled");
		}
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x0020D828 File Offset: 0x0020BA28
	public override void StopWork()
	{
		Workable workable = this.GetWorkable();
		if (this.state == WorkerBase.State.PendingCompletion || this.state == WorkerBase.State.Completing)
		{
			this.state = WorkerBase.State.Idle;
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				base.Trigger(1705586602, this);
			}
			else
			{
				base.Trigger(-993481695, this);
				this.InternalStopWork(workable, true);
			}
		}
		else if (this.state == WorkerBase.State.Working)
		{
			if (workable != null && workable.synchronizeAnims)
			{
				KAnimControllerBase animController = workable.GetAnimController();
				if (animController != null)
				{
					HashedString[] workPstAnims = workable.GetWorkPstAnims(this, false);
					if (workPstAnims != null && workPstAnims.Length != 0)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
						animController.SetPositionPercent(1f);
					}
				}
			}
			base.Trigger(-993481695, this);
			this.InternalStopWork(workable, true);
		}
		base.Trigger(2027193395, this);
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x0020D8F4 File Offset: 0x0020BAF4
	public override void StartWork(WorkerBase.StartWorkInfo start_work_info)
	{
		this.startWorkInfo = start_work_info;
		Game.Instance.StartedWork();
		Workable workable = this.GetWorkable();
		if (this.state != WorkerBase.State.Idle)
		{
			string text = "";
			if (workable != null)
			{
				text = workable.name;
			}
			global::Debug.LogError(string.Concat(new string[]
			{
				base.name,
				".",
				text,
				".state should be idle but instead it's:",
				this.state.ToString()
			}));
		}
		string name = workable.GetType().Name;
		try
		{
			base.gameObject.AddTag(GameTags.PerformingWorkRequest);
			this.state = WorkerBase.State.Working;
			base.gameObject.Trigger(1568504979, this);
			if (workable != null)
			{
				this.animInfo = workable.GetAnim(this);
				if (this.animInfo.smi != null)
				{
					this.smi = this.animInfo.smi;
					this.smi.StartSM();
				}
				Vector3 position = base.transform.GetPosition();
				position.z = Grid.GetLayerZ(workable.workLayer);
				base.transform.SetPosition(position);
				KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
				if (this.animInfo.smi == null)
				{
					this.AttachOverrideAnims(component);
				}
				HashedString[] workAnims = workable.GetWorkAnims(this);
				KAnim.PlayMode workAnimPlayMode = workable.GetWorkAnimPlayMode();
				Vector3 workOffset = workable.GetWorkOffset();
				this.workAnimOffset = workOffset;
				component.Offset += workOffset;
				if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null && workAnims.Length != 0 && this.experienceRecipient != null)
				{
					if (workable.synchronizeAnims)
					{
						KAnimControllerBase animController = workable.GetAnimController();
						if (animController != null)
						{
							this.kanimSynchronizer = animController.GetSynchronizer();
							if (this.kanimSynchronizer != null)
							{
								this.kanimSynchronizer.Add(component);
							}
						}
						animController.Play(workAnims, workAnimPlayMode);
					}
					else
					{
						component.Play(workAnims, workAnimPlayMode);
					}
				}
			}
			workable.StartWork(this);
			if (workable == null)
			{
				global::Debug.LogWarning("Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
			}
			else
			{
				this.onWorkChoreDisabledHandle = workable.Subscribe(2108245096, new Action<object>(this.OnWorkChoreDisabled));
				if (workable.triggerWorkReactions && workable.WorkTimeRemaining > 10f)
				{
					this.CreatePasserbyReactable();
				}
				KSelectable component2 = base.GetComponent<KSelectable>();
				this.previousStatusItem = component2.GetStatusItem(Db.Get().StatusItemCategories.Main);
				component2.SetStatusItem(Db.Get().StatusItemCategories.Main, workable.GetWorkerStatusItem(), workable);
			}
		}
		catch (Exception ex)
		{
			string str = "Exception in: Worker.StartWork(" + name + ")";
			DebugUtil.LogErrorArgs(this, new object[]
			{
				str + "\n" + ex.ToString()
			});
			throw;
		}
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x000C2AEB File Offset: 0x000C0CEB
	private void Update()
	{
		if (this.state == WorkerBase.State.Working)
		{
			this.ForceSyncAnims();
		}
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x000C2AFC File Offset: 0x000C0CFC
	private void ForceSyncAnims()
	{
		if (Time.deltaTime > 0f && this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.SyncTime();
		}
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x0020DBDC File Offset: 0x0020BDDC
	public override bool InstantlyFinish()
	{
		Workable workable = this.GetWorkable();
		return workable != null && workable.InstantlyFinish(this);
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x0020DC04 File Offset: 0x0020BE04
	private void AttachOverrideAnims(KAnimControllerBase worker_controller)
	{
		if (this.animInfo.overrideAnims != null && this.animInfo.overrideAnims.Length != 0)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[i], 0f);
			}
		}
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x0020DC5C File Offset: 0x0020BE5C
	private void DetachAnimOverrides()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.RemoveWithoutIdleAnim(component);
			this.kanimSynchronizer = null;
		}
		if (this.animInfo.overrideAnims != null)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				component.RemoveAnimOverrides(this.animInfo.overrideAnims[i]);
			}
			this.animInfo.overrideAnims = null;
		}
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x0020DCD0 File Offset: 0x0020BED0
	private void CreateCompletionReactable(string topic)
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		EmoteReactable emoteReactable = OneshotReactableLocator.CreateOneshotReactable(base.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, 9, 5, 100f);
		Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
		emoteReactable.SetEmote(clapCheer);
		emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.GetReactionEffect), null).RegisterEmoteStepCallbacks("clapcheer_pst", null, delegate(GameObject r)
		{
			r.Trigger(937885943, topic);
		});
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			Thought thought = new Thought("Completion_" + topic, null, uisprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, "", true, 4f);
			emoteReactable.SetThought(thought);
		}
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x0020DDE8 File Offset: 0x0020BFE8
	private void CreatePasserbyReactable()
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		if (this.passerbyReactable == null)
		{
			EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier, float.PositiveInfinity, 0f);
			Emote thumbsUp = Db.Get().Emotes.Minion.ThumbsUp;
			emoteReactable.SetEmote(thumbsUp).SetThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsntPartying));
			emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.GetReactionEffect), null);
			this.passerbyReactable = emoteReactable;
		}
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x0020DEE8 File Offset: 0x0020C0E8
	private void GetReactionEffect(GameObject reactor)
	{
		Effects component = base.GetComponent<Effects>();
		if (component != null)
		{
			component.Add("WorkEncouraged", true);
		}
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x000B7124 File Offset: 0x000B5324
	private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
	{
		return transition.end == NavType.Floor;
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x0020DF14 File Offset: 0x0020C114
	private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
	{
		Facing component = reactor.GetComponent<Facing>();
		return base.transform.GetPosition().x < reactor.transform.GetPosition().x == component.GetFacing();
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x0020DF54 File Offset: 0x0020C154
	private bool ReactorIsntPartying(GameObject reactor, Navigator.ActiveTransition transition)
	{
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		return component.choreDriver.HasChore() && component.choreDriver.GetCurrentChore().choreType != Db.Get().ChoreTypes.Party;
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x000C2B1D File Offset: 0x000C0D1D
	private void ClearPasserbyReactable()
	{
		if (this.passerbyReactable != null)
		{
			this.passerbyReactable.Cleanup();
			this.passerbyReactable = null;
		}
	}

	// Token: 0x0400241F RID: 9247
	private WorkerBase.State state;

	// Token: 0x04002420 RID: 9248
	private WorkerBase.StartWorkInfo startWorkInfo;

	// Token: 0x04002421 RID: 9249
	private const float EARLIEST_REACT_TIME = 1f;

	// Token: 0x04002422 RID: 9250
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04002423 RID: 9251
	[MyCmpGet]
	private IExperienceRecipient experienceRecipient;

	// Token: 0x04002424 RID: 9252
	private float workPendingCompletionTime;

	// Token: 0x04002425 RID: 9253
	private int onWorkChoreDisabledHandle;

	// Token: 0x04002426 RID: 9254
	public object workCompleteData;

	// Token: 0x04002427 RID: 9255
	private Workable.AnimInfo animInfo;

	// Token: 0x04002428 RID: 9256
	private KAnimSynchronizer kanimSynchronizer;

	// Token: 0x04002429 RID: 9257
	private StatusItemGroup.Entry previousStatusItem;

	// Token: 0x0400242A RID: 9258
	private StateMachine.Instance smi;

	// Token: 0x0400242B RID: 9259
	private bool successFullyCompleted;

	// Token: 0x0400242C RID: 9260
	private Vector3 workAnimOffset = Vector3.zero;

	// Token: 0x0400242D RID: 9261
	public bool usesMultiTool = true;

	// Token: 0x0400242E RID: 9262
	public bool isFetchDrone;

	// Token: 0x0400242F RID: 9263
	public CellOffset[] fetchOffsets = new CellOffset[0];

	// Token: 0x04002430 RID: 9264
	private static readonly EventSystem.IntraObjectHandler<StandardWorker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<StandardWorker>(delegate(StandardWorker component, object data)
	{
		component.OnChoreInterrupt(data);
	});

	// Token: 0x04002431 RID: 9265
	private Reactable passerbyReactable;
}
