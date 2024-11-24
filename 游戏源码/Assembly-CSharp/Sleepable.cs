using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200187C RID: 6268
[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
	// Token: 0x060081AE RID: 33198 RVA: 0x000F5450 File Offset: 0x000F3650
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
		this.approachable = base.GetComponent<IApproachable>();
	}

	// Token: 0x060081AF RID: 33199 RVA: 0x000F548F File Offset: 0x000F368F
	protected override void OnSpawn()
	{
		if (this.isNormalBed)
		{
			Components.NormalBeds.Add(base.gameObject.GetMyWorldId(), this);
		}
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x060081B0 RID: 33200 RVA: 0x0033A2DC File Offset: 0x003384DC
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkAnims;
		}
		return Sleepable.normalWorkAnims;
	}

	// Token: 0x060081B1 RID: 33201 RVA: 0x0033A31C File Offset: 0x0033851C
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkPstAnim;
		}
		return Sleepable.normalWorkPstAnim;
	}

	// Token: 0x060081B2 RID: 33202 RVA: 0x0033A35C File Offset: 0x0033855C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		KAnimControllerBase animController = this.GetAnimController();
		if (animController != null)
		{
			animController.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			animController.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		}
		base.Subscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (this.operational != null)
		{
			this.operational.SetActive(true, false);
		}
		worker.Trigger(-1283701846, this);
		worker.GetComponent<Effects>().Add(this.effectName, false);
		this.isDoneSleeping = false;
	}

	// Token: 0x060081B3 RID: 33203 RVA: 0x0033A418 File Offset: 0x00338618
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.isDoneSleeping)
		{
			return Time.time > this.wakeTime;
		}
		if (this.Dreamable != null && !this.Dreamable.DreamIsDisturbed)
		{
			this.Dreamable.WorkTick(worker, dt);
		}
		if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
		{
			this.isDoneSleeping = true;
			this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
		}
		return false;
	}

	// Token: 0x060081B4 RID: 33204 RVA: 0x0033A490 File Offset: 0x00338690
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.operational != null)
		{
			this.operational.SetActive(false, false);
		}
		base.Unsubscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (worker != null)
		{
			Effects component = worker.GetComponent<Effects>();
			component.Remove(this.effectName);
			if (this.wakeEffects != null)
			{
				foreach (string effect_id in this.wakeEffects)
				{
					component.Add(effect_id, true);
				}
			}
			if (this.stretchOnWake && UnityEngine.Random.value < 0.33f)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.MorningStretch, 1, null);
			}
			if (worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value < worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
			{
				worker.Trigger(1338475637, this);
			}
		}
	}

	// Token: 0x060081B5 RID: 33205 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x060081B6 RID: 33206 RVA: 0x000F54BA File Offset: 0x000F36BA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isNormalBed)
		{
			Components.NormalBeds.Remove(base.gameObject.GetMyWorldId(), this);
		}
	}

	// Token: 0x060081B7 RID: 33207 RVA: 0x0033A5DC File Offset: 0x003387DC
	private void PlayPstAnim(object data)
	{
		WorkerBase workerBase = (WorkerBase)data;
		if (workerBase != null && workerBase.GetWorkable() != null)
		{
			KAnimControllerBase component = workerBase.GetWorkable().gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0400626D RID: 25197
	private const float STRECH_CHANCE = 0.33f;

	// Token: 0x0400626E RID: 25198
	[MyCmpGet]
	public Assignable assignable;

	// Token: 0x0400626F RID: 25199
	public IApproachable approachable;

	// Token: 0x04006270 RID: 25200
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04006271 RID: 25201
	public string effectName = "Sleep";

	// Token: 0x04006272 RID: 25202
	public List<string> wakeEffects;

	// Token: 0x04006273 RID: 25203
	public bool stretchOnWake = true;

	// Token: 0x04006274 RID: 25204
	private float wakeTime;

	// Token: 0x04006275 RID: 25205
	private bool isDoneSleeping;

	// Token: 0x04006276 RID: 25206
	public bool isNormalBed = true;

	// Token: 0x04006277 RID: 25207
	public ClinicDreamable Dreamable;

	// Token: 0x04006278 RID: 25208
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04006279 RID: 25209
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x0400627A RID: 25210
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x0400627B RID: 25211
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};
}
