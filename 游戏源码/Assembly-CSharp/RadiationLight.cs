using System;
using UnityEngine;

// Token: 0x0200172F RID: 5935
public class RadiationLight : StateMachineComponent<RadiationLight.StatesInstance>
{
	// Token: 0x06007A39 RID: 31289 RVA: 0x000F047A File Offset: 0x000EE67A
	public void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x06007A3A RID: 31290 RVA: 0x000F04A3 File Offset: 0x000EE6A3
	public bool HasEnoughFuel()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06007A3B RID: 31291 RVA: 0x000F04B1 File Offset: 0x000EE6B1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.UpdateMeter();
	}

	// Token: 0x04005BBA RID: 23482
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005BBB RID: 23483
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005BBC RID: 23484
	[MyCmpGet]
	private RadiationEmitter emitter;

	// Token: 0x04005BBD RID: 23485
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04005BBE RID: 23486
	private MeterController meter;

	// Token: 0x04005BBF RID: 23487
	public Tag elementToConsume;

	// Token: 0x04005BC0 RID: 23488
	public float consumptionRate;

	// Token: 0x02001730 RID: 5936
	public class StatesInstance : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.GameInstance
	{
		// Token: 0x06007A3D RID: 31293 RVA: 0x00317E38 File Offset: 0x00316038
		public StatesInstance(RadiationLight smi) : base(smi)
		{
			if (base.GetComponent<Rotatable>().IsRotated)
			{
				RadiationEmitter component = base.GetComponent<RadiationEmitter>();
				component.emitDirection = 180f;
				component.emissionOffset = Vector3.left;
			}
			this.ToggleEmitter(false);
			smi.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x000F04D2 File Offset: 0x000EE6D2
		public void ToggleEmitter(bool on)
		{
			base.smi.master.operational.SetActive(on, false);
			base.smi.master.emitter.SetEmitting(on);
		}
	}

	// Token: 0x02001731 RID: 5937
	public class States : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight>
	{
		// Token: 0x06007A3F RID: 31295 RVA: 0x00317EB8 File Offset: 0x003160B8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.idle;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(RadiationLight.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.waiting.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.ready.idle, (RadiationLight.StatesInstance smi) => smi.master.operational.IsOperational);
			this.ready.EventTransition(GameHashes.OperationalChanged, this.waiting, (RadiationLight.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.ready.idle);
			this.ready.idle.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready.on, (RadiationLight.StatesInstance smi) => smi.master.HasEnoughFuel());
			this.ready.on.PlayAnim("on").Enter(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(true);
			}).EventTransition(GameHashes.OnStorageChange, this.ready.idle, (RadiationLight.StatesInstance smi) => !smi.master.HasEnoughFuel()).Exit(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(false);
			});
		}

		// Token: 0x04005BC1 RID: 23489
		public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State waiting;

		// Token: 0x04005BC2 RID: 23490
		public RadiationLight.States.ReadyStates ready;

		// Token: 0x02001732 RID: 5938
		public class ReadyStates : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State
		{
			// Token: 0x04005BC3 RID: 23491
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State idle;

			// Token: 0x04005BC4 RID: 23492
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State on;
		}
	}
}
