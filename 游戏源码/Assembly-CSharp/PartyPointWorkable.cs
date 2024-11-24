using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001699 RID: 5785
public class PartyPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600777C RID: 30588 RVA: 0x000AC786 File Offset: 0x000AA986
	private PartyPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600777D RID: 30589 RVA: 0x0030E298 File Offset: 0x0030C498
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnimPlayMode = KAnim.PlayMode.Loop;
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
		if (UnityEngine.Random.Range(0f, 100f) > 80f)
		{
			this.activity = PartyPointWorkable.ActivityType.Dance;
		}
		else
		{
			this.activity = PartyPointWorkable.ActivityType.Talk;
		}
		PartyPointWorkable.ActivityType activityType = this.activity;
		if (activityType == PartyPointWorkable.ActivityType.Talk)
		{
			this.workAnims = new HashedString[]
			{
				"idle"
			};
			this.workerOverrideAnims = new KAnimFile[][]
			{
				new KAnimFile[]
				{
					Assets.GetAnim("anim_generic_convo_kanim")
				}
			};
			return;
		}
		if (activityType != PartyPointWorkable.ActivityType.Dance)
		{
			return;
		}
		this.workAnims = new HashedString[]
		{
			"working_loop"
		};
		this.workerOverrideAnims = new KAnimFile[][]
		{
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
			},
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
			},
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
			}
		};
	}

	// Token: 0x0600777E RID: 30590 RVA: 0x0030E3FC File Offset: 0x0030C5FC
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x0600777F RID: 30591 RVA: 0x000EE716 File Offset: 0x000EC916
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x06007780 RID: 30592 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x06007781 RID: 30593 RVA: 0x0030E430 File Offset: 0x0030C630
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06007782 RID: 30594 RVA: 0x0030E488 File Offset: 0x0030C688
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06007783 RID: 30595 RVA: 0x0030E4DC File Offset: 0x0030C6DC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06007784 RID: 30596 RVA: 0x0030E50C File Offset: 0x0030C70C
	private void OnStartedTalking(object data)
	{
		ConversationManager.StartedTalkingEvent startedTalkingEvent = data as ConversationManager.StartedTalkingEvent;
		if (startedTalkingEvent == null)
		{
			return;
		}
		GameObject talker = startedTalkingEvent.talker;
		if (talker == base.worker.gameObject)
		{
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
				string text = startedTalkingEvent.anim;
				text += UnityEngine.Random.Range(1, 9).ToString();
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else
		{
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			}
			this.lastTalker = talker;
		}
	}

	// Token: 0x06007785 RID: 30597 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x06007786 RID: 30598 RVA: 0x000EE73D File Offset: 0x000EC93D
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04005947 RID: 22855
	private GameObject lastTalker;

	// Token: 0x04005948 RID: 22856
	public int basePriority;

	// Token: 0x04005949 RID: 22857
	public string specificEffect;

	// Token: 0x0400594A RID: 22858
	public KAnimFile[][] workerOverrideAnims;

	// Token: 0x0400594B RID: 22859
	private PartyPointWorkable.ActivityType activity;

	// Token: 0x0200169A RID: 5786
	private enum ActivityType
	{
		// Token: 0x0400594D RID: 22861
		Talk,
		// Token: 0x0400594E RID: 22862
		Dance,
		// Token: 0x0400594F RID: 22863
		LENGTH
	}
}
