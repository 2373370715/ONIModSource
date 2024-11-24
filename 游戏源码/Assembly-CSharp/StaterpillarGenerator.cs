using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000FB0 RID: 4016
public class StaterpillarGenerator : Generator
{
	// Token: 0x0600513F RID: 20799 RVA: 0x00270EF8 File Offset: 0x0026F0F8
	protected override void OnSpawn()
	{
		Staterpillar staterpillar = this.parent.Get();
		if (staterpillar == null || staterpillar.GetGenerator() != this)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.smi = new StaterpillarGenerator.StatesInstance(this);
		this.smi.StartSM();
		base.OnSpawn();
	}

	// Token: 0x06005140 RID: 20800 RVA: 0x00270F54 File Offset: 0x0026F154
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = base.GetComponent<Generator>().WattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x040038BE RID: 14526
	private StaterpillarGenerator.StatesInstance smi;

	// Token: 0x040038BF RID: 14527
	[Serialize]
	public Ref<Staterpillar> parent = new Ref<Staterpillar>();

	// Token: 0x02000FB1 RID: 4017
	public class StatesInstance : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.GameInstance
	{
		// Token: 0x06005142 RID: 20802 RVA: 0x000D4F94 File Offset: 0x000D3194
		public StatesInstance(StaterpillarGenerator master) : base(master)
		{
		}

		// Token: 0x040038C0 RID: 14528
		private Attributes attributes;
	}

	// Token: 0x02000FB2 RID: 4018
	public class States : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator>
	{
		// Token: 0x06005143 RID: 20803 RVA: 0x00270FC8 File Offset: 0x0026F1C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EventTransition(GameHashes.OperationalChanged, this.idle, (StaterpillarGenerator.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.EventTransition(GameHashes.OperationalChanged, this.root, (StaterpillarGenerator.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(StaterpillarGenerator.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
		}

		// Token: 0x040038C1 RID: 14529
		public GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State idle;
	}
}
