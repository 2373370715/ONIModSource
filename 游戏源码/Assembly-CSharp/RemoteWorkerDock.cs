using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02001767 RID: 5991
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkDock")]
public class RemoteWorkerDock : KMonoBehaviour
{
	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06007B4A RID: 31562 RVA: 0x000F10B5 File Offset: 0x000EF2B5
	// (set) Token: 0x06007B4B RID: 31563 RVA: 0x000F10BD File Offset: 0x000EF2BD
	public RemoteWorkerSM RemoteWorker
	{
		get
		{
			return this.remoteWorker;
		}
		private set
		{
			this.remoteWorker = value;
			this.worker = ((value != null) ? new Ref<KSelectable>(value.GetComponent<KSelectable>()) : null);
		}
	}

	// Token: 0x06007B4C RID: 31564 RVA: 0x000F10E3 File Offset: 0x000EF2E3
	public WorkerBase GetActiveTerminalWorker()
	{
		if (this.terminal == null)
		{
			return null;
		}
		return this.terminal.worker;
	}

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06007B4D RID: 31565 RVA: 0x000F1100 File Offset: 0x000EF300
	public bool IsOperational
	{
		get
		{
			return this.operational.IsOperational;
		}
	}

	// Token: 0x06007B4E RID: 31566 RVA: 0x0031ADDC File Offset: 0x00318FDC
	private bool canWork(IRemoteDockWorkTarget provider)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(provider.Approachable.GetCell(), out num3, out num4);
		return num2 == num4 && Math.Abs(num - num3) <= 12;
	}

	// Token: 0x06007B4F RID: 31567 RVA: 0x000F110D File Offset: 0x000EF30D
	private void considerProvider(IRemoteDockWorkTarget provider)
	{
		if (this.canWork(provider))
		{
			this.providers.Add(provider);
		}
	}

	// Token: 0x06007B50 RID: 31568 RVA: 0x000F1124 File Offset: 0x000EF324
	private void forgetProvider(IRemoteDockWorkTarget provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x06007B51 RID: 31569 RVA: 0x0031AE24 File Offset: 0x00319024
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Components.RemoteWorkerDocks.Add(this.GetMyWorldId(), this);
		this.add_provider_binding = new Action<IRemoteDockWorkTarget>(this.considerProvider);
		this.remove_provider_binding = new Action<IRemoteDockWorkTarget>(this.forgetProvider);
		Components.RemoteDockWorkTargets.Register(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		Ref<KSelectable> @ref = this.worker;
		RemoteWorkerSM remoteWorkerSM;
		if (@ref == null)
		{
			remoteWorkerSM = null;
		}
		else
		{
			KSelectable kselectable = @ref.Get();
			remoteWorkerSM = ((kselectable != null) ? kselectable.GetComponent<RemoteWorkerSM>() : null);
		}
		this.remoteWorker = remoteWorkerSM;
		if (this.remoteWorker == null)
		{
			this.RequestNewWorker(null);
			return;
		}
		this.remoteWorkerDestroyedEventId = this.remoteWorker.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
	}

	// Token: 0x06007B52 RID: 31570 RVA: 0x0031AEFC File Offset: 0x003190FC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteWorkerDocks.Remove(this.GetMyWorldId(), this);
		Components.RemoteDockWorkTargets.Unregister(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		if (this.remoteWorker != null)
		{
			this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
		}
		if (this.newRemoteWorkerHandle.IsValid)
		{
			this.newRemoteWorkerHandle.ClearScheduler();
		}
	}

	// Token: 0x06007B53 RID: 31571 RVA: 0x0031AF74 File Offset: 0x00319174
	public void CollectChores(ChoreConsumerState duplicant_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		if (this.remoteWorker == null)
		{
			return;
		}
		ChoreConsumerState consumerState = this.remoteWorker.GetComponent<ChoreConsumer>().consumerState;
		consumerState.resume = duplicant_state.resume;
		foreach (IRemoteDockWorkTarget remoteDockWorkTarget in this.providers)
		{
			Chore remoteDockChore = remoteDockWorkTarget.RemoteDockChore;
			if (remoteDockChore != null)
			{
				remoteDockChore.CollectChores(consumerState, succeeded_contexts, incomplete_contexts, failed_contexts, false);
			}
		}
	}

	// Token: 0x06007B54 RID: 31572 RVA: 0x000F1133 File Offset: 0x000EF333
	public bool AvailableForWorkBy(RemoteWorkTerminal terminal)
	{
		return this.terminal == null || this.terminal == terminal;
	}

	// Token: 0x06007B55 RID: 31573 RVA: 0x000F1151 File Offset: 0x000EF351
	public bool HasWorker()
	{
		return this.remoteWorker != null;
	}

	// Token: 0x06007B56 RID: 31574 RVA: 0x0031B004 File Offset: 0x00319204
	public void SetNextChore(RemoteWorkTerminal terminal, Chore.Precondition.Context chore_context)
	{
		global::Debug.Assert(this.worker != null);
		global::Debug.Assert(this.terminal == null || this.terminal == terminal);
		this.terminal = terminal;
		if (this.remoteWorker != null)
		{
			this.remoteWorker.SetNextChore(chore_context);
		}
	}

	// Token: 0x06007B57 RID: 31575 RVA: 0x0031B064 File Offset: 0x00319264
	public bool StartWorking(RemoteWorkTerminal terminal)
	{
		if (this.terminal == null)
		{
			this.terminal = terminal;
		}
		if (this.terminal == terminal && this.remoteWorker != null)
		{
			this.remoteWorker.ActivelyControlled = true;
			return true;
		}
		return false;
	}

	// Token: 0x06007B58 RID: 31576 RVA: 0x000F115F File Offset: 0x000EF35F
	public void StopWorking(RemoteWorkTerminal terminal)
	{
		if (terminal == this.terminal)
		{
			this.terminal = null;
			if (this.remoteWorker != null)
			{
				this.remoteWorker.ActivelyControlled = false;
			}
		}
	}

	// Token: 0x06007B59 RID: 31577 RVA: 0x000F1190 File Offset: 0x000EF390
	public bool OnRemoteWorkTick(float dt)
	{
		return this.remoteWorker == null || (!this.remoteWorker.ActivelyWorking && !this.remoteWorker.HasChoreQueued());
	}

	// Token: 0x06007B5A RID: 31578 RVA: 0x000F11BF File Offset: 0x000EF3BF
	private void OnStorageChanged(object _)
	{
		if (this.remoteWorker == null || this.worker.Get() == null)
		{
			this.RequestNewWorker(null);
		}
	}

	// Token: 0x06007B5B RID: 31579 RVA: 0x0031B0B4 File Offset: 0x003192B4
	private void RequestNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		Tag build_MATERIAL_TAG = RemoteWorkerConfig.BUILD_MATERIAL_TAG;
		if (this.storage.FindFirstWithMass(build_MATERIAL_TAG, 200f) == null)
		{
			if (!this.activeFetch)
			{
				this.activeFetch = true;
				FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
				fetchList.Add(build_MATERIAL_TAG, null, 200f, Operational.State.None);
				fetchList.Submit(delegate
				{
					this.activeFetch = false;
					this.RequestNewWorker(null);
				}, true);
				return;
			}
		}
		else
		{
			this.MakeNewWorker(null);
		}
	}

	// Token: 0x06007B5C RID: 31580 RVA: 0x0031B140 File Offset: 0x00319340
	private void MakeNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		if (this.storage.GetAmountAvailable(RemoteWorkerConfig.BUILD_MATERIAL_TAG) < 200f)
		{
			return;
		}
		PrimaryElement elem = this.storage.FindFirstWithMass(RemoteWorkerConfig.BUILD_MATERIAL_TAG, 200f);
		if (elem == null)
		{
			return;
		}
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.storage.ConsumeAndGetDisease(elem.ElementID.CreateTag(), 200f, out num, out disease, out temperature);
		this.status_item_handle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RemoteWorkDockMakingWorker, null);
		this.newRemoteWorkerHandle = GameScheduler.Instance.Schedule("MakeRemoteWorker", 2f, delegate(object _)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(RemoteWorkerConfig.ID), this.transform.position, Grid.SceneLayer.Creatures, null, 0);
			if (this.remoteWorkerDestroyedEventId != -1 && this.remoteWorker != null)
			{
				this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
			}
			this.RemoteWorker = gameObject.GetComponent<RemoteWorkerSM>();
			this.remoteWorker.HomeDepot = this;
			this.remoteWorker.playNewWorker = true;
			gameObject.SetActive(true);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = elem.ElementID;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from construction material");
			}
			this.remoteWorkerDestroyedEventId = gameObject.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
			this.newRemoteWorkerHandle.ClearScheduler();
			this.GetComponent<KSelectable>().RemoveStatusItem(this.status_item_handle, false);
		}, null, null);
	}

	// Token: 0x04005C6F RID: 23663
	[Serialize]
	protected Ref<KSelectable> worker;

	// Token: 0x04005C70 RID: 23664
	protected RemoteWorkerSM remoteWorker;

	// Token: 0x04005C71 RID: 23665
	private int remoteWorkerDestroyedEventId = -1;

	// Token: 0x04005C72 RID: 23666
	protected RemoteWorkTerminal terminal;

	// Token: 0x04005C73 RID: 23667
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005C74 RID: 23668
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005C75 RID: 23669
	[MyCmpAdd]
	private EnterableDock enter_;

	// Token: 0x04005C76 RID: 23670
	[MyCmpAdd]
	private ExitableDock exit_;

	// Token: 0x04005C77 RID: 23671
	[MyCmpAdd]
	private WorkerRecharger recharger_;

	// Token: 0x04005C78 RID: 23672
	[MyCmpAdd]
	private WorkerGunkRemover gunk_remover_;

	// Token: 0x04005C79 RID: 23673
	[MyCmpAdd]
	private WorkerOilRefiller oil_refiller_;

	// Token: 0x04005C7A RID: 23674
	private Guid status_item_handle;

	// Token: 0x04005C7B RID: 23675
	private SchedulerHandle newRemoteWorkerHandle;

	// Token: 0x04005C7C RID: 23676
	private List<IRemoteDockWorkTarget> providers = new List<IRemoteDockWorkTarget>();

	// Token: 0x04005C7D RID: 23677
	private Action<IRemoteDockWorkTarget> add_provider_binding;

	// Token: 0x04005C7E RID: 23678
	private Action<IRemoteDockWorkTarget> remove_provider_binding;

	// Token: 0x04005C7F RID: 23679
	private bool activeFetch;
}
