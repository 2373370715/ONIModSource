using System;

// Token: 0x02001131 RID: 4401
[SkipSaveFileSerialization]
public class BlightVulnerable : StateMachineComponent<BlightVulnerable.StatesInstance>
{
	// Token: 0x06005A10 RID: 23056 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005A11 RID: 23057 RVA: 0x000DAADC File Offset: 0x000D8CDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005A12 RID: 23058 RVA: 0x000DAAEF File Offset: 0x000D8CEF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005A13 RID: 23059 RVA: 0x000DAAF7 File Offset: 0x000D8CF7
	public void MakeBlighted()
	{
		Debug.Log("Blighting plant", this);
		base.smi.sm.isBlighted.Set(true, base.smi, false);
	}

	// Token: 0x04003F8F RID: 16271
	private SchedulerHandle handle;

	// Token: 0x04003F90 RID: 16272
	public bool prefersDarkness;

	// Token: 0x02001132 RID: 4402
	public class StatesInstance : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.GameInstance
	{
		// Token: 0x06005A15 RID: 23061 RVA: 0x000DAB2A File Offset: 0x000D8D2A
		public StatesInstance(BlightVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x02001133 RID: 4403
	public class States : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable>
	{
		// Token: 0x06005A16 RID: 23062 RVA: 0x00293B54 File Offset: 0x00291D54
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.comfortable.ParamTransition<bool>(this.isBlighted, this.blighted, GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.IsTrue);
			this.blighted.TriggerOnEnter(GameHashes.BlightChanged, (BlightVulnerable.StatesInstance smi) => true).Enter(delegate(BlightVulnerable.StatesInstance smi)
			{
				smi.GetComponent<SeedProducer>().seedInfo.seedId = RotPileConfig.ID;
			}).ToggleTag(GameTags.Blighted).Exit(delegate(BlightVulnerable.StatesInstance smi)
			{
				GameplayEventManager.Instance.Trigger(-1425542080, smi.gameObject);
			});
		}

		// Token: 0x04003F91 RID: 16273
		public StateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.BoolParameter isBlighted;

		// Token: 0x04003F92 RID: 16274
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State comfortable;

		// Token: 0x04003F93 RID: 16275
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State blighted;
	}
}
