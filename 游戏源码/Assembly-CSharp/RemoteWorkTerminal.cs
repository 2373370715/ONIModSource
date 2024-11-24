using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001744 RID: 5956
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkTerminal")]
public class RemoteWorkTerminal : Workable
{
	// Token: 0x06007AAA RID: 31402 RVA: 0x00319994 File Offset: 0x00317B94
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_terminal_kanim")
		};
		this.InitializeWorkingInteracts();
		this.synchronizeAnims = true;
		this.kbac.onAnimComplete += this.PlayNextWorkingAnim;
	}

	// Token: 0x06007AAB RID: 31403 RVA: 0x003199EC File Offset: 0x00317BEC
	private void InitializeWorkingInteracts()
	{
		if (RemoteWorkTerminal.NUM_WORKING_INTERACTS != -1)
		{
			return;
		}
		KAnimFileData data = this.overrideAnims[0].GetData();
		RemoteWorkTerminal.NUM_WORKING_INTERACTS = 1;
		for (;;)
		{
			string anim_name = string.Format("working_loop_{0}", RemoteWorkTerminal.NUM_WORKING_INTERACTS);
			if (data.GetAnim(anim_name) == null)
			{
				break;
			}
			RemoteWorkTerminal.NUM_WORKING_INTERACTS++;
		}
	}

	// Token: 0x06007AAC RID: 31404 RVA: 0x00319A44 File Offset: 0x00317C44
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkAnims;
		}
		return RemoteWorkTerminal.normalWorkAnims;
	}

	// Token: 0x06007AAD RID: 31405 RVA: 0x00319A84 File Offset: 0x00317C84
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkPstAnim;
		}
		return RemoteWorkTerminal.normalWorkPstAnim;
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06007AAE RID: 31406 RVA: 0x000F08FA File Offset: 0x000EEAFA
	// (set) Token: 0x06007AAF RID: 31407 RVA: 0x000F090D File Offset: 0x000EEB0D
	public RemoteWorkerDock CurrentDock
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (((@ref != null) ? @ref.Get() : null) != null)
			{
				this.dock.Get().StopWorking(this);
			}
			this.dock = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06007AB0 RID: 31408 RVA: 0x000F0946 File Offset: 0x000EEB46
	// (set) Token: 0x06007AB1 RID: 31409 RVA: 0x000F0958 File Offset: 0x000EEB58
	public RemoteWorkerDock FutureDock
	{
		get
		{
			return this.future_dock ?? this.CurrentDock;
		}
		set
		{
			this.CurrentDock = value;
		}
	}

	// Token: 0x06007AB2 RID: 31410 RVA: 0x000F0961 File Offset: 0x000EEB61
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.kbac.Queue(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StartWorking(this);
	}

	// Token: 0x06007AB3 RID: 31411 RVA: 0x000F0998 File Offset: 0x000EEB98
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StopWorking(this);
	}

	// Token: 0x06007AB4 RID: 31412 RVA: 0x000F09B2 File Offset: 0x000EEBB2
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.CurrentDock == null || this.CurrentDock.OnRemoteWorkTick(dt);
	}

	// Token: 0x06007AB5 RID: 31413 RVA: 0x000F09D0 File Offset: 0x000EEBD0
	private HashedString GetWorkingLoop()
	{
		return string.Format("working_loop_{0}", UnityEngine.Random.Range(1, RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1));
	}

	// Token: 0x06007AB6 RID: 31414 RVA: 0x000F09F3 File Offset: 0x000EEBF3
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (base.worker.GetState() == WorkerBase.State.Working)
		{
			this.kbac.Play(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x04005C07 RID: 23559
	[Serialize]
	private Ref<RemoteWorkerDock> dock;

	// Token: 0x04005C08 RID: 23560
	private static int NUM_WORKING_INTERACTS = -1;

	// Token: 0x04005C09 RID: 23561
	[MyCmpReq]
	private KBatchedAnimController kbac;

	// Token: 0x04005C0A RID: 23562
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x04005C0B RID: 23563
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre"
	};

	// Token: 0x04005C0C RID: 23564
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04005C0D RID: 23565
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"working_hat_pst"
	};

	// Token: 0x04005C0E RID: 23566
	public RemoteWorkerDock future_dock;
}
