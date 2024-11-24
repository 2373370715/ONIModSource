using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000C0C RID: 3084
[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003AD2 RID: 15058 RVA: 0x000C5F32 File Offset: 0x000C4132
	public bool HasFilter()
	{
		return this.elementConverter.HasEnoughMass(this.filterTag, false);
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x000C5F46 File Offset: 0x000C4146
	public bool IsConvertable()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x000C5F54 File Offset: 0x000C4154
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003AD5 RID: 15061 RVA: 0x000AD332 File Offset: 0x000AB532
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x0400281E RID: 10270
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400281F RID: 10271
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002820 RID: 10272
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04002821 RID: 10273
	[MyCmpGet]
	private ElementConsumer elementConsumer;

	// Token: 0x04002822 RID: 10274
	public Tag filterTag;

	// Token: 0x02000C0D RID: 3085
	public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
	{
		// Token: 0x06003AD7 RID: 15063 RVA: 0x000C5F6F File Offset: 0x000C416F
		public StatesInstance(AirFilter smi) : base(smi)
		{
		}
	}

	// Token: 0x02000C0E RID: 3086
	public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
	{
		// Token: 0x06003AD8 RID: 15064 RVA: 0x00228BF8 File Offset: 0x00226DF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waiting;
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational);
			this.hasFilter.EventTransition(GameHashes.OperationalChanged, this.waiting, (AirFilter.StatesInstance smi) => !smi.master.operational.IsOperational).Enter("EnableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit("DisableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).DefaultState(this.hasFilter.idle);
			this.hasFilter.idle.EventTransition(GameHashes.OnStorageChange, this.hasFilter.converting, (AirFilter.StatesInstance smi) => smi.master.IsConvertable());
			this.hasFilter.converting.Enter("SetActive(true)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.hasFilter.idle, (AirFilter.StatesInstance smi) => !smi.master.IsConvertable());
		}

		// Token: 0x04002823 RID: 10275
		public AirFilter.States.ReadyStates hasFilter;

		// Token: 0x04002824 RID: 10276
		public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;

		// Token: 0x02000C0F RID: 3087
		public class ReadyStates : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State
		{
			// Token: 0x04002825 RID: 10277
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State idle;

			// Token: 0x04002826 RID: 10278
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;
		}
	}
}
