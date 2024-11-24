using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F05 RID: 3845
[SerializationConfig(MemberSerialization.OptIn)]
public class OxyliteRefinery : StateMachineComponent<OxyliteRefinery.StatesInstance>
{
	// Token: 0x06004D97 RID: 19863 RVA: 0x000D2632 File Offset: 0x000D0832
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x040035E4 RID: 13796
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040035E5 RID: 13797
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040035E6 RID: 13798
	public Tag emitTag;

	// Token: 0x040035E7 RID: 13799
	public float emitMass;

	// Token: 0x040035E8 RID: 13800
	public Vector3 dropOffset;

	// Token: 0x02000F06 RID: 3846
	public class StatesInstance : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.GameInstance
	{
		// Token: 0x06004D99 RID: 19865 RVA: 0x000D2647 File Offset: 0x000D0847
		public StatesInstance(OxyliteRefinery smi) : base(smi)
		{
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x002656D4 File Offset: 0x002638D4
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				Vector3 position = base.transform.GetPosition() + base.master.dropOffset;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
				storage.Drop(gameObject, true);
			}
		}
	}

	// Token: 0x02000F07 RID: 3847
	public class States : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery>
	{
		// Token: 0x06004D9B RID: 19867 RVA: 0x0026576C File Offset: 0x0026396C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OxyliteRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (OxyliteRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (OxyliteRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Transition(this.waiting, (OxyliteRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x040035E9 RID: 13801
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State disabled;

		// Token: 0x040035EA RID: 13802
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State waiting;

		// Token: 0x040035EB RID: 13803
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State converting;
	}
}
