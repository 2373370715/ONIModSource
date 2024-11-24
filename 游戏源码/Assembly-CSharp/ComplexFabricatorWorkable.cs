using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009BB RID: 2491
[AddComponentMenu("KMonoBehaviour/Workable/ComplexFabricatorWorkable")]
public class ComplexFabricatorWorkable : Workable
{
	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x000BD934 File Offset: 0x000BBB34
	// (set) Token: 0x06002DB3 RID: 11699 RVA: 0x000BD93C File Offset: 0x000BBB3C
	public StatusItem WorkerStatusItem
	{
		get
		{
			return this.workerStatusItem;
		}
		set
		{
			this.workerStatusItem = value;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x000BD945 File Offset: 0x000BBB45
	// (set) Token: 0x06002DB5 RID: 11701 RVA: 0x000BD94D File Offset: 0x000BBB4D
	public AttributeConverter AttributeConverter
	{
		get
		{
			return this.attributeConverter;
		}
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x000BD956 File Offset: 0x000BBB56
	// (set) Token: 0x06002DB7 RID: 11703 RVA: 0x000BD95E File Offset: 0x000BBB5E
	public float AttributeExperienceMultiplier
	{
		get
		{
			return this.attributeExperienceMultiplier;
		}
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x000BD967 File Offset: 0x000BBB67
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x170001BA RID: 442
	// (set) Token: 0x06002DB9 RID: 11705 RVA: 0x000BD970 File Offset: 0x000BBB70
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06002DBA RID: 11706 RVA: 0x000BD979 File Offset: 0x000BBB79
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!(this.fabricator != null))
			{
				return null;
			}
			return this.fabricator.CurrentWorkingOrder;
		}
	}

	// Token: 0x06002DBB RID: 11707 RVA: 0x001F1CE8 File Offset: 0x001EFEE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06002DBC RID: 11708 RVA: 0x001F1D58 File Offset: 0x001EFF58
	public override string GetConversationTopic()
	{
		string conversationTopic = this.fabricator.GetConversationTopic();
		if (conversationTopic == null)
		{
			return base.GetConversationTopic();
		}
		return conversationTopic;
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x001F1D7C File Offset: 0x001EFF7C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.fabricator.CurrentWorkingOrder != null)
		{
			this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
			this.QueueWorkingAnimations();
			return;
		}
		DebugUtil.DevAssertArgs(false, new object[]
		{
			"ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null",
			base.gameObject
		});
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x000BD996 File Offset: 0x000BBB96
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.OnWorkTickActions != null)
		{
			this.OnWorkTickActions(worker, dt);
		}
		this.UpdateOrderProgress(worker, dt);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x000BD9BD File Offset: 0x000BBBBD
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (worker != null && this.GetDupeInteract != null)
		{
			worker.GetAnimController().onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x001F1DE0 File Offset: 0x001EFFE0
	public override float GetWorkTime()
	{
		ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
		if (currentWorkingOrder != null)
		{
			this.workTime = currentWorkingOrder.time;
			return this.workTime;
		}
		return -1f;
	}

	// Token: 0x06002DC1 RID: 11713 RVA: 0x001F1E14 File Offset: 0x001F0014
	public Chore CreateWorkChore(ChoreType choreType, float order_progress)
	{
		Chore result = new WorkChore<ComplexFabricatorWorkable>(choreType, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
		return result;
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x000BD9EE File Offset: 0x000BBBEE
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.fabricator.CompleteWorkingOrder();
		this.DestroyVisualizer();
		base.OnStopWork(worker);
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x001F1E50 File Offset: 0x001F0050
	private void InstantiateVisualizer(ComplexRecipe recipe)
	{
		if (this.visualizer != null)
		{
			this.DestroyVisualizer();
		}
		if (this.visualizerLink != null)
		{
			this.visualizerLink.Unregister();
			this.visualizerLink = null;
		}
		if (recipe.FabricationVisualizer == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer, null, null);
		this.visualizer.transform.parent = this.meter.meterController.transform;
		this.visualizer.transform.SetLocalPosition(new Vector3(0f, 0f, 1f));
		this.visualizer.SetActive(true);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = this.visualizer.GetComponent<KBatchedAnimController>();
		this.visualizerLink = new KAnimLink(component, component2);
	}

	// Token: 0x06002DC4 RID: 11716 RVA: 0x001F1F20 File Offset: 0x001F0120
	private void UpdateOrderProgress(WorkerBase worker, float dt)
	{
		float workTime = this.GetWorkTime();
		float num = Mathf.Clamp01((workTime - base.WorkTimeRemaining) / workTime);
		if (this.fabricator)
		{
			this.fabricator.OrderProgress = num;
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
	}

	// Token: 0x06002DC5 RID: 11717 RVA: 0x000BDA0F File Offset: 0x000BBC0F
	private void DestroyVisualizer()
	{
		if (this.visualizer != null)
		{
			if (this.visualizerLink != null)
			{
				this.visualizerLink.Unregister();
				this.visualizerLink = null;
			}
			Util.KDestroyGameObject(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x001F1F74 File Offset: 0x001F0174
	public void QueueWorkingAnimations()
	{
		KBatchedAnimController animController = base.worker.GetAnimController();
		if (this.GetDupeInteract != null)
		{
			animController.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			animController.onAnimComplete += this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x001F1FC4 File Offset: 0x001F01C4
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (this.GetDupeInteract != null)
		{
			KBatchedAnimController animController = base.worker.GetAnimController();
			if (base.worker.GetState() == WorkerBase.State.Working)
			{
				animController.Play(this.GetDupeInteract(), KAnim.PlayMode.Once);
				return;
			}
			animController.onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x04001EAC RID: 7852
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001EAD RID: 7853
	[MyCmpReq]
	private ComplexFabricator fabricator;

	// Token: 0x04001EAE RID: 7854
	public Action<WorkerBase, float> OnWorkTickActions;

	// Token: 0x04001EAF RID: 7855
	public MeterController meter;

	// Token: 0x04001EB0 RID: 7856
	protected GameObject visualizer;

	// Token: 0x04001EB1 RID: 7857
	protected KAnimLink visualizerLink;

	// Token: 0x04001EB2 RID: 7858
	public Func<HashedString[]> GetDupeInteract;
}
