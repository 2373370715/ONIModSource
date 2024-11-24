using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016A8 RID: 5800
public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060077B7 RID: 30647 RVA: 0x000EE96F File Offset: 0x000ECB6F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060077B8 RID: 30648 RVA: 0x000C9B47 File Offset: 0x000C7D47
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04005980 RID: 22912
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04005981 RID: 22913
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005982 RID: 22914
	public SimHashes emittedElement;

	// Token: 0x04005983 RID: 22915
	public float emitRate;

	// Token: 0x020016A9 RID: 5801
	public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
	{
		// Token: 0x060077BA RID: 30650 RVA: 0x000EE98A File Offset: 0x000ECB8A
		public StatesInstance(PlantElementEmitter master) : base(master)
		{
		}

		// Token: 0x060077BB RID: 30651 RVA: 0x000EE993 File Offset: 0x000ECB93
		public bool IsWilting()
		{
			return !(base.master.wiltCondition == null) && base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}
	}

	// Token: 0x020016AA RID: 5802
	public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
	{
		// Token: 0x060077BC RID: 30652 RVA: 0x0030F170 File Offset: 0x0030D370
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.healthy;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (PlantElementEmitter.StatesInstance smi) => smi.IsWilting()).Update("PlantEmit", delegate(PlantElementEmitter.StatesInstance smi, float dt)
			{
				SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
			}, UpdateRate.SIM_4000ms, false);
			this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy, null);
		}

		// Token: 0x04005984 RID: 22916
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;

		// Token: 0x04005985 RID: 22917
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;
	}
}
