using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001918 RID: 6424
[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060085DE RID: 34270 RVA: 0x000F7B5E File Offset: 0x000F5D5E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060085DF RID: 34271 RVA: 0x0034A2CC File Offset: 0x003484CC
	public void ReleaseAstronaut()
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x060085E0 RID: 34272 RVA: 0x000AD332 File Offset: 0x000AB532
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x0400650F RID: 25871
	[Serialize]
	private int landOffLocation;

	// Token: 0x04006510 RID: 25872
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x04006511 RID: 25873
	private float rocketSpeed;

	// Token: 0x04006512 RID: 25874
	public float exhaustEmitRate = 2f;

	// Token: 0x04006513 RID: 25875
	public float exhaustTemperature = 1000f;

	// Token: 0x04006514 RID: 25876
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04006515 RID: 25877
	private GameObject soundSpeakerObject;

	// Token: 0x04006516 RID: 25878
	private bool releasingAstronaut;

	// Token: 0x02001919 RID: 6425
	public class StatesInstance : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.GameInstance
	{
		// Token: 0x060085E2 RID: 34274 RVA: 0x000F7B9A File Offset: 0x000F5D9A
		public StatesInstance(PodLander master) : base(master)
		{
		}
	}

	// Token: 0x0200191A RID: 6426
	public class States : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander>
	{
		// Token: 0x060085E3 RID: 34275 RVA: 0x0034A348 File Offset: 0x00348548
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.landing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.flightAnimOffset = 50f;
			}).Update(delegate(PodLander.StatesInstance smi, float dt)
			{
				float num = 10f;
				smi.master.rocketSpeed = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0f, num - 2f);
				smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
				KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				Vector3 positionIncludingOffset = component.PositionIncludingOffset;
				int num2 = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num2))
				{
					SimMessages.EmitMass(num2, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				if (component.Offset.y <= 0f)
				{
					smi.GoTo(this.crashed);
				}
			}, UpdateRate.SIM_33ms, false);
			this.crashed.PlayAnim("grounded").Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				smi.master.rocketSpeed = 0f;
				smi.master.ReleaseAstronaut();
			});
		}

		// Token: 0x04006517 RID: 25879
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State landing;

		// Token: 0x04006518 RID: 25880
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State crashed;
	}
}
