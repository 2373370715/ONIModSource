using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000C8A RID: 3210
[SerializationConfig(MemberSerialization.OptIn)]
public class AlgaeDistillery : StateMachineComponent<AlgaeDistillery.StatesInstance>
{
	// Token: 0x06003DC9 RID: 15817 RVA: 0x000C817E File Offset: 0x000C637E
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04002A1B RID: 10779
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04002A1C RID: 10780
	[SerializeField]
	public float emitMass;

	// Token: 0x04002A1D RID: 10781
	[SerializeField]
	public Vector3 emitOffset;

	// Token: 0x04002A1E RID: 10782
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002A1F RID: 10783
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04002A20 RID: 10784
	[MyCmpReq]
	private Operational operational;

	// Token: 0x02000C8B RID: 3211
	public class StatesInstance : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.GameInstance
	{
		// Token: 0x06003DCB RID: 15819 RVA: 0x000C8193 File Offset: 0x000C6393
		public StatesInstance(AlgaeDistillery smi) : base(smi)
		{
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00232794 File Offset: 0x00230994
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				storage.Drop(gameObject, true).transform.SetPosition(base.transform.GetPosition() + base.master.emitOffset);
			}
		}
	}

	// Token: 0x02000C8C RID: 3212
	public class States : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery>
	{
		// Token: 0x06003DCD RID: 15821 RVA: 0x00232818 File Offset: 0x00230A18
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (AlgaeDistillery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (AlgaeDistillery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (AlgaeDistillery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (AlgaeDistillery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x04002A21 RID: 10785
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State disabled;

		// Token: 0x04002A22 RID: 10786
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State waiting;

		// Token: 0x04002A23 RID: 10787
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State converting;

		// Token: 0x04002A24 RID: 10788
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State overpressure;
	}
}
