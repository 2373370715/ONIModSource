using System;
using System.Linq;
using KSerialization;

// Token: 0x0200103F RID: 4159
public class WarpReceiver : Workable
{
	// Token: 0x060054E8 RID: 21736 RVA: 0x000BC8FA File Offset: 0x000BAAFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x000D76E4 File Offset: 0x000D58E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpReceiverSMI = new WarpReceiver.WarpReceiverSM.Instance(this);
		this.warpReceiverSMI.StartSM();
		Components.WarpReceivers.Add(this);
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x0027CBB8 File Offset: 0x0027ADB8
	public void ReceiveWarpedDuplicant(WorkerBase dupe)
	{
		dupe.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this), CellAlignment.Bottom, Grid.SceneLayer.Move));
		Debug.Assert(this.chore == null);
		KAnimFile anim = Assets.GetAnim("anim_interacts_warp_portal_receiver_kanim");
		ChoreType migrate = Db.Get().ChoreTypes.Migrate;
		KAnimFile override_anims = anim;
		this.chore = new WorkChore<Workable>(migrate, this, dupe.GetComponent<ChoreProvider>(), true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, true, true, override_anims, false, true, false, PriorityScreen.PriorityClass.compulsory, 5, false, true);
		Workable component = base.GetComponent<Workable>();
		component.workLayer = Grid.SceneLayer.Building;
		component.workAnims = new HashedString[]
		{
			"printing_pre",
			"printing_loop"
		};
		component.workingPstComplete = new HashedString[]
		{
			"printing_pst"
		};
		component.workingPstFailed = new HashedString[]
		{
			"printing_pst"
		};
		component.synchronizeAnims = true;
		float num = 0f;
		KAnimFileData data = anim.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim2 = data.GetAnim(i);
			if (component.workAnims.Contains(anim2.hash))
			{
				num += anim2.totalTime;
			}
		}
		component.SetWorkTime(num);
		this.Used = true;
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x000D770E File Offset: 0x000D590E
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
		this.warpReceiverSMI.GoTo(this.warpReceiverSMI.sm.idle);
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x000D773D File Offset: 0x000D593D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.WarpReceivers.Remove(this);
	}

	// Token: 0x04003B8E RID: 15246
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04003B8F RID: 15247
	private WarpReceiver.WarpReceiverSM.Instance warpReceiverSMI;

	// Token: 0x04003B90 RID: 15248
	private Notification notification;

	// Token: 0x04003B91 RID: 15249
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04003B92 RID: 15250
	private Chore chore;

	// Token: 0x04003B93 RID: 15251
	[Serialize]
	public bool Used;

	// Token: 0x02001040 RID: 4160
	public class WarpReceiverSM : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver>
	{
		// Token: 0x060054EF RID: 21743 RVA: 0x000D7758 File Offset: 0x000D5958
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("idle");
		}

		// Token: 0x04003B94 RID: 15252
		public GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.State idle;

		// Token: 0x02001041 RID: 4161
		public new class Instance : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.GameInstance
		{
			// Token: 0x060054F1 RID: 21745 RVA: 0x000D777B File Offset: 0x000D597B
			public Instance(WarpReceiver master) : base(master)
			{
			}
		}
	}
}
