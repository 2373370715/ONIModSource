using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B78 RID: 2936
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Workable")]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x060037DC RID: 14300 RVA: 0x000C4404 File Offset: 0x000C2604
	// (set) Token: 0x060037DD RID: 14301 RVA: 0x000C440C File Offset: 0x000C260C
	public WorkerBase worker { get; protected set; }

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x000C4415 File Offset: 0x000C2615
	// (set) Token: 0x060037DF RID: 14303 RVA: 0x000C441D File Offset: 0x000C261D
	public float WorkTimeRemaining
	{
		get
		{
			return this.workTimeRemaining;
		}
		set
		{
			this.workTimeRemaining = value;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060037E0 RID: 14304 RVA: 0x000C4426 File Offset: 0x000C2626
	// (set) Token: 0x060037E1 RID: 14305 RVA: 0x000C442E File Offset: 0x000C262E
	public bool preferUnreservedCell { get; set; }

	// Token: 0x060037E2 RID: 14306 RVA: 0x000C4437 File Offset: 0x000C2637
	public virtual float GetWorkTime()
	{
		return this.workTime;
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x000C443F File Offset: 0x000C263F
	public WorkerBase GetWorker()
	{
		return this.worker;
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x000C4447 File Offset: 0x000C2647
	public virtual float GetPercentComplete()
	{
		if (this.workTimeRemaining > this.workTime)
		{
			return -1f;
		}
		return 1f - this.workTimeRemaining / this.workTime;
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x000C4470 File Offset: 0x000C2670
	public void ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)
	{
		this.multitoolContext = context;
		this.multitoolHitEffectTag = hitEffectTag;
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x00219948 File Offset: 0x00217B48
	public virtual Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			BuildingFacade buildingFacade = this.GetBuildingFacade();
			bool flag = false;
			if (buildingFacade != null && !buildingFacade.IsOriginal)
			{
				flag = buildingFacade.interactAnims.TryGetValue(base.name, out result.overrideAnims);
			}
			if (!flag)
			{
				result.overrideAnims = this.overrideAnims;
			}
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x000C4480 File Offset: 0x000C2680
	public virtual HashedString[] GetWorkAnims(WorkerBase worker)
	{
		return this.workAnims;
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x000C4488 File Offset: 0x000C2688
	public virtual KAnim.PlayMode GetWorkAnimPlayMode()
	{
		return this.workAnimPlayMode;
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x000C4490 File Offset: 0x000C2690
	public virtual HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		if (successfully_completed)
		{
			return this.workingPstComplete;
		}
		return this.workingPstFailed;
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x000C44A2 File Offset: 0x000C26A2
	public virtual Vector3 GetWorkOffset()
	{
		return Vector3.zero;
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x002199EC File Offset: 0x00217BEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().MiscStatusItems.Using;
		this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
		this.workTime = this.GetWorkTime();
		this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00219A64 File Offset: 0x00217C64
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			if (this.skillsUpdateHandle != -1)
			{
				Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			}
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		if (this.requireMinionToWork && this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		this.minionUpdateHandle = Game.Instance.Subscribe(586301400, new Action<object>(this.UpdateStatusItem));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		if (base.gameObject.HasTag(this.laboratoryEfficiencyBonusTagRequired))
		{
			this.useLaboratoryEfficiencyBonus = true;
			base.Subscribe<Workable>(144050788, Workable.OnUpdateRoomDelegate);
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		this.UpdateStatusItem(null);
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x00219B6C File Offset: 0x00217D6C
	private void RefreshRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x00219BB4 File Offset: 0x00217DB4
	private void OnUpdateRoom(object data)
	{
		if (this.worker == null)
		{
			return;
		}
		Room room = (Room)data;
		if (room != null && room.roomType == Db.Get().RoomTypes.Laboratory)
		{
			this.currentlyInLaboratory = true;
			if (this.laboratoryEfficiencyBonusStatusItemHandle == Guid.Empty)
			{
				this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, this);
				return;
			}
		}
		else
		{
			this.currentlyInLaboratory = false;
			if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
			{
				this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
				this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
			}
		}
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x00219C64 File Offset: 0x00217E64
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		component.RemoveStatusItem(this.workStatusItemHandle, false);
		if (this.worker == null)
		{
			if (this.requireMinionToWork && Components.LiveMinionIdentities.GetWorldItems(this.GetMyWorldId(), false).Count == 0)
			{
				this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.WorkRequiresMinion, null);
				return;
			}
			if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
			{
				if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId()))
				{
					StatusItem status_item = DlcManager.FeatureClusterSpaceEnabled() ? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk : Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;
					this.workStatusItemHandle = component.AddStatusItem(status_item, this.requiredSkillPerk);
					return;
				}
				this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, this.requiredSkillPerk);
				return;
			}
		}
		else if (this.workingStatusItem != null)
		{
			this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, this);
		}
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x000C44A9 File Offset: 0x000C26A9
	protected override void OnLoadLevel()
	{
		this.overrideAnims = null;
		base.OnLoadLevel();
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public virtual int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x00219D7C File Offset: 0x00217F7C
	public void StartWork(WorkerBase worker_to_start)
	{
		global::Debug.Assert(worker_to_start != null, "How did we get a null worker?");
		this.worker = worker_to_start;
		this.UpdateStatusItem(null);
		if (this.showProgressBar)
		{
			this.ShowProgressBar(true);
		}
		if (this.useLaboratoryEfficiencyBonus)
		{
			this.RefreshRoom();
		}
		this.OnStartWork(this.worker);
		if (this.worker != null)
		{
			string conversationTopic = this.GetConversationTopic();
			if (conversationTopic != null)
			{
				this.worker.Trigger(937885943, conversationTopic);
			}
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStarted);
		}
		this.numberOfUses++;
		if (this.worker != null)
		{
			if (base.gameObject.GetComponent<KSelectable>() != null && base.gameObject.GetComponent<KSelectable>().IsSelected && this.worker.gameObject.GetComponent<LoopingSounds>() != null)
			{
				this.worker.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
			else if (this.worker.gameObject.GetComponent<KSelectable>() != null && this.worker.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
			{
				base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
		}
		base.gameObject.Trigger(853695848, this);
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x00219EE8 File Offset: 0x002180E8
	public bool WorkTick(WorkerBase worker, float dt)
	{
		bool flag = false;
		if (dt > 0f)
		{
			this.workTimeRemaining -= dt;
			flag = this.OnWorkTick(worker, dt);
		}
		return flag || this.workTimeRemaining < 0f;
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x00219F28 File Offset: 0x00218128
	public virtual float GetEfficiencyMultiplier(WorkerBase worker)
	{
		float num = 1f;
		if (this.attributeConverter != null)
		{
			AttributeConverterInstance attributeConverterInstance = worker.GetAttributeConverter(this.attributeConverter.Id);
			if (attributeConverterInstance != null)
			{
				num += attributeConverterInstance.Evaluate();
			}
		}
		if (this.lightEfficiencyBonus)
		{
			int num2 = Grid.PosToCell(worker.gameObject);
			if (Grid.IsValidCell(num2))
			{
				if (Grid.LightIntensity[num2] > DUPLICANTSTATS.STANDARD.Light.NO_LIGHT)
				{
					this.currentlyLit = true;
					num += DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS;
					if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
					{
						this.lightEfficiencyBonusStatusItemHandle = worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, this);
					}
				}
				else
				{
					this.currentlyLit = false;
					if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
					{
						worker.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
					}
				}
			}
		}
		if (this.useLaboratoryEfficiencyBonus && this.currentlyInLaboratory)
		{
			num += 0.1f;
		}
		return Mathf.Max(num, this.minimumAttributeMultiplier);
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x000C44B8 File Offset: 0x000C26B8
	public virtual Klei.AI.Attribute GetWorkAttribute()
	{
		if (this.attributeConverter != null)
		{
			return this.attributeConverter.attribute;
		}
		return null;
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x0021A034 File Offset: 0x00218234
	public virtual string GetConversationTopic()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (!component.HasTag(GameTags.NotConversationTopic))
		{
			return component.PrefabTag.Name;
		}
		return null;
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x000BD956 File Offset: 0x000BBB56
	public float GetAttributeExperienceMultiplier()
	{
		return this.attributeExperienceMultiplier;
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x000C44CF File Offset: 0x000C26CF
	public string GetSkillExperienceSkillGroup()
	{
		return this.skillExperienceSkillGroup;
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x000C44D7 File Offset: 0x000C26D7
	public float GetSkillExperienceMultiplier()
	{
		return this.skillExperienceMultiplier;
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	protected virtual bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x0021A064 File Offset: 0x00218264
	public void StopWork(WorkerBase workerToStop, bool aborted)
	{
		if (this.worker == workerToStop && aborted)
		{
			this.OnAbortWork(workerToStop);
		}
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(workerToStop);
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStopped);
		}
		this.OnStopWork(workerToStop);
		if (this.resetProgressOnStop)
		{
			this.workTimeRemaining = this.GetWorkTime();
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			workerToStop.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
			this.lightEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
			this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (base.gameObject.GetComponent<KSelectable>() != null && !base.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
		{
			base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		else if (workerToStop.gameObject.GetComponent<KSelectable>() != null && !workerToStop.gameObject.GetComponent<KSelectable>().IsSelected && workerToStop.gameObject.GetComponent<LoopingSounds>() != null)
		{
			workerToStop.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		this.worker = null;
		base.gameObject.Trigger(679550494, this);
		this.UpdateStatusItem(null);
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x000BD934 File Offset: 0x000BBB34
	public virtual StatusItem GetWorkerStatusItem()
	{
		return this.workerStatusItem;
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x000BD93C File Offset: 0x000BBB3C
	public void SetWorkerStatusItem(StatusItem item)
	{
		this.workerStatusItem = item;
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x0021A1F0 File Offset: 0x002183F0
	public void CompleteWork(WorkerBase worker)
	{
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(worker);
		}
		this.OnCompleteWork(worker);
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkCompleted);
		}
		this.workTimeRemaining = this.GetWorkTime();
		this.ShowProgressBar(false);
		base.gameObject.Trigger(-2011693419, this);
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x000C44DF File Offset: 0x000C26DF
	public void SetReportType(ReportManager.ReportType report_type)
	{
		this.reportType = report_type;
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x000C44E8 File Offset: 0x000C26E8
	public ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnStopWork(WorkerBase worker)
	{
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnAbortWork(WorkerBase worker)
	{
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnPendingCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x000C44F0 File Offset: 0x000C26F0
	public void SetOffsets(CellOffset[] offsets)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new StandardOffsetTracker(offsets);
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x000C4511 File Offset: 0x000C2711
	public void SetOffsetTable(CellOffset[][] offset_table)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new OffsetTableTracker(offset_table, this);
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x000C4533 File Offset: 0x000C2733
	public virtual CellOffset[] GetOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.GetOffsets(cell);
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x000C455A File Offset: 0x000C275A
	public virtual bool ValidateOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.ValidateOffsets(cell);
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x000C4581 File Offset: 0x000C2781
	public CellOffset[] GetOffsets()
	{
		return this.GetOffsets(Grid.PosToCell(this));
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x000C458F File Offset: 0x000C278F
	public void SetWorkTime(float work_time)
	{
		this.workTime = work_time;
		this.workTimeRemaining = work_time;
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x000C459F File Offset: 0x000C279F
	public bool ShouldFaceTargetWhenWorking()
	{
		return this.faceTargetWhenWorking;
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x000C19AF File Offset: 0x000BFBAF
	public virtual Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0021A24C File Offset: 0x0021844C
	public void ShowProgressBar(bool show)
	{
		if (show)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x0021A2BC File Offset: 0x002184BC
	protected override void OnCleanUp()
	{
		this.ShowProgressBar(false);
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		if (this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		base.OnCleanUp();
		this.OnWorkableEventCB = null;
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x0021A324 File Offset: 0x00218524
	public virtual Vector3 GetTargetPoint()
	{
		Vector3 vector = base.transform.GetPosition();
		float y = vector.y + 0.65f;
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			vector = component.bounds.center;
		}
		vector.y = y;
		vector.z = 0f;
		return vector;
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x000C45A7 File Offset: 0x000C27A7
	public int GetNavigationCost(Navigator navigator, int cell)
	{
		return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x000C45B7 File Offset: 0x000C27B7
	public int GetNavigationCost(Navigator navigator)
	{
		return this.GetNavigationCost(navigator, Grid.PosToCell(this));
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x000C45C6 File Offset: 0x000C27C6
	private void TransferDiseaseWithWorker(WorkerBase worker)
	{
		if (this == null || worker == null)
		{
			return;
		}
		Workable.TransferDiseaseWithWorker(base.gameObject, worker.gameObject);
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x0021A380 File Offset: 0x00218580
	public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
	{
		if (workable == null || worker == null)
		{
			return;
		}
		PrimaryElement component = workable.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
		if (component2 == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component2.DiseaseIdx;
		invalid.count = (int)((float)component2.DiseaseCount * 0.33f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = component.DiseaseIdx;
		invalid2.count = (int)((float)component.DiseaseCount * 0.33f);
		component2.ModifyDiseaseCount(-invalid.count, "Workable.TransferDiseaseWithWorker");
		component.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
		if (invalid.count > 0)
		{
			component.AddDisease(invalid.idx, invalid.count, "Workable.TransferDiseaseWithWorker");
		}
		if (invalid2.count > 0)
		{
			component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
		}
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x0021A478 File Offset: 0x00218678
	public void SetShouldShowSkillPerkStatusItem(bool shouldItBeShown)
	{
		this.shouldShowSkillPerkStatusItem = shouldItBeShown;
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			this.skillsUpdateHandle = -1;
		}
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x0021A4EC File Offset: 0x002186EC
	public virtual bool InstantlyFinish(WorkerBase worker)
	{
		float num = worker.GetWorkable().WorkTimeRemaining;
		if (!float.IsInfinity(num))
		{
			worker.Work(num);
			return true;
		}
		DebugUtil.DevAssert(false, this.ToString() + " was asked to instantly finish but it has infinite work time! Override InstantlyFinish in your workable!", null);
		return false;
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x0021A530 File Offset: 0x00218730
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.trackUses)
		{
			Descriptor item = new Descriptor(string.Format(BUILDING.DETAILS.USE_COUNT, this.numberOfUses), string.Format(BUILDING.DETAILS.USE_COUNT_TOOLTIP, this.numberOfUses), Descriptor.DescriptorType.Detail, false);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x000C45EC File Offset: 0x000C27EC
	public virtual BuildingFacade GetBuildingFacade()
	{
		return base.GetComponent<BuildingFacade>();
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x000C45F4 File Offset: 0x000C27F4
	public virtual KAnimControllerBase GetAnimController()
	{
		return base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x000C45FC File Offset: 0x000C27FC
	[ContextMenu("Refresh Reachability")]
	public void RefreshReachability()
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.ForceRefresh();
		}
	}

	// Token: 0x040025EB RID: 9707
	public float workTime;

	// Token: 0x040025EC RID: 9708
	protected bool showProgressBar = true;

	// Token: 0x040025ED RID: 9709
	public bool alwaysShowProgressBar;

	// Token: 0x040025EE RID: 9710
	protected bool lightEfficiencyBonus = true;

	// Token: 0x040025EF RID: 9711
	protected Guid lightEfficiencyBonusStatusItemHandle;

	// Token: 0x040025F0 RID: 9712
	public bool currentlyLit;

	// Token: 0x040025F1 RID: 9713
	public Tag laboratoryEfficiencyBonusTagRequired = RoomConstraints.ConstraintTags.ScienceBuilding;

	// Token: 0x040025F2 RID: 9714
	private bool useLaboratoryEfficiencyBonus;

	// Token: 0x040025F3 RID: 9715
	protected Guid laboratoryEfficiencyBonusStatusItemHandle;

	// Token: 0x040025F4 RID: 9716
	private bool currentlyInLaboratory;

	// Token: 0x040025F5 RID: 9717
	protected StatusItem workerStatusItem;

	// Token: 0x040025F6 RID: 9718
	protected StatusItem workingStatusItem;

	// Token: 0x040025F7 RID: 9719
	protected Guid workStatusItemHandle;

	// Token: 0x040025F8 RID: 9720
	protected OffsetTracker offsetTracker;

	// Token: 0x040025F9 RID: 9721
	[SerializeField]
	protected string attributeConverterId;

	// Token: 0x040025FA RID: 9722
	protected AttributeConverter attributeConverter;

	// Token: 0x040025FB RID: 9723
	protected float minimumAttributeMultiplier = 0.5f;

	// Token: 0x040025FC RID: 9724
	public bool resetProgressOnStop;

	// Token: 0x040025FD RID: 9725
	protected bool shouldTransferDiseaseWithWorker = true;

	// Token: 0x040025FE RID: 9726
	[SerializeField]
	protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

	// Token: 0x040025FF RID: 9727
	[SerializeField]
	protected string skillExperienceSkillGroup;

	// Token: 0x04002600 RID: 9728
	[SerializeField]
	protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

	// Token: 0x04002601 RID: 9729
	public bool triggerWorkReactions = true;

	// Token: 0x04002602 RID: 9730
	public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;

	// Token: 0x04002603 RID: 9731
	[SerializeField]
	[Tooltip("What layer does the dupe switch to when interacting with the building")]
	public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;

	// Token: 0x04002604 RID: 9732
	[SerializeField]
	[Serialize]
	protected float workTimeRemaining = float.PositiveInfinity;

	// Token: 0x04002605 RID: 9733
	[SerializeField]
	public KAnimFile[] overrideAnims;

	// Token: 0x04002606 RID: 9734
	[SerializeField]
	protected HashedString multitoolContext;

	// Token: 0x04002607 RID: 9735
	[SerializeField]
	protected Tag multitoolHitEffectTag;

	// Token: 0x04002608 RID: 9736
	[SerializeField]
	[Tooltip("Whether to user the KAnimSynchronizer or not")]
	public bool synchronizeAnims = true;

	// Token: 0x04002609 RID: 9737
	[SerializeField]
	[Tooltip("Whether to display number of uses in the details panel")]
	public bool trackUses;

	// Token: 0x0400260A RID: 9738
	[Serialize]
	protected int numberOfUses;

	// Token: 0x0400260B RID: 9739
	public Action<Workable, Workable.WorkableEvent> OnWorkableEventCB;

	// Token: 0x0400260C RID: 9740
	protected int skillsUpdateHandle = -1;

	// Token: 0x0400260D RID: 9741
	private int minionUpdateHandle = -1;

	// Token: 0x0400260E RID: 9742
	public string requiredSkillPerk;

	// Token: 0x0400260F RID: 9743
	[SerializeField]
	protected bool shouldShowSkillPerkStatusItem = true;

	// Token: 0x04002610 RID: 9744
	[SerializeField]
	public bool requireMinionToWork;

	// Token: 0x04002611 RID: 9745
	protected StatusItem readyForSkillWorkStatusItem;

	// Token: 0x04002612 RID: 9746
	public HashedString[] workAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04002613 RID: 9747
	public HashedString[] workingPstComplete = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04002614 RID: 9748
	public HashedString[] workingPstFailed = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04002615 RID: 9749
	public KAnim.PlayMode workAnimPlayMode;

	// Token: 0x04002616 RID: 9750
	public bool faceTargetWhenWorking;

	// Token: 0x04002617 RID: 9751
	private static readonly EventSystem.IntraObjectHandler<Workable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Workable>(delegate(Workable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x04002618 RID: 9752
	protected ProgressBar progressBar;

	// Token: 0x02000B79 RID: 2937
	public enum WorkableEvent
	{
		// Token: 0x0400261A RID: 9754
		WorkStarted,
		// Token: 0x0400261B RID: 9755
		WorkCompleted,
		// Token: 0x0400261C RID: 9756
		WorkStopped
	}

	// Token: 0x02000B7A RID: 2938
	public struct AnimInfo
	{
		// Token: 0x0400261D RID: 9757
		public KAnimFile[] overrideAnims;

		// Token: 0x0400261E RID: 9758
		public StateMachine.Instance smi;
	}
}
