using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F94 RID: 3988
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitOutbox : StateMachineComponent<SolidConduitOutbox.SMInstance>
{
	// Token: 0x060050B4 RID: 20660 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060050B5 RID: 20661 RVA: 0x000D4A36 File Offset: 0x000D2C36
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<SolidConduitOutbox>(-1697596308, SolidConduitOutbox.OnStorageChangedDelegate);
		this.UpdateMeter();
		base.smi.StartSM();
	}

	// Token: 0x060050B6 RID: 20662 RVA: 0x000D4A74 File Offset: 0x000D2C74
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060050B7 RID: 20663 RVA: 0x000D4A7C File Offset: 0x000D2C7C
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
	}

	// Token: 0x060050B8 RID: 20664 RVA: 0x0026F674 File Offset: 0x0026D874
	private void UpdateMeter()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060050B9 RID: 20665 RVA: 0x000D4A84 File Offset: 0x000D2C84
	private void UpdateConsuming()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
	}

	// Token: 0x04003846 RID: 14406
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003847 RID: 14407
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04003848 RID: 14408
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04003849 RID: 14409
	private MeterController meter;

	// Token: 0x0400384A RID: 14410
	private static readonly EventSystem.IntraObjectHandler<SolidConduitOutbox> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<SolidConduitOutbox>(delegate(SolidConduitOutbox component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000F95 RID: 3989
	public class SMInstance : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.GameInstance
	{
		// Token: 0x060050BC RID: 20668 RVA: 0x000D4AD2 File Offset: 0x000D2CD2
		public SMInstance(SolidConduitOutbox master) : base(master)
		{
		}
	}

	// Token: 0x02000F96 RID: 3990
	public class States : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox>
	{
		// Token: 0x060050BD RID: 20669 RVA: 0x0026F6AC File Offset: 0x0026D8AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("RefreshConsuming", delegate(SolidConduitOutbox.SMInstance smi, float dt)
			{
				smi.master.UpdateConsuming();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x0400384B RID: 14411
		public StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.BoolParameter consuming;

		// Token: 0x0400384C RID: 14412
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State idle;

		// Token: 0x0400384D RID: 14413
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State working;

		// Token: 0x0400384E RID: 14414
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State post;
	}
}
