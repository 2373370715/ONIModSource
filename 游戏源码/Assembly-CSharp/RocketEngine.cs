using System;
using KSerialization;
using STRINGS;

// Token: 0x02001928 RID: 6440
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>
{
	// Token: 0x0600862D RID: 34349 RVA: 0x0034B7B0 File Offset: 0x003499B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(FuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
		}
	}

	// Token: 0x0400654F RID: 25935
	public float exhaustEmitRate = 50f;

	// Token: 0x04006550 RID: 25936
	public float exhaustTemperature = 1500f;

	// Token: 0x04006551 RID: 25937
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04006552 RID: 25938
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04006553 RID: 25939
	public Tag fuelTag;

	// Token: 0x04006554 RID: 25940
	public float efficiency = 1f;

	// Token: 0x04006555 RID: 25941
	public bool requireOxidizer = true;

	// Token: 0x04006556 RID: 25942
	public bool mainEngine = true;

	// Token: 0x02001929 RID: 6441
	public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
	{
		// Token: 0x0600862F RID: 34351 RVA: 0x000F7E7C File Offset: 0x000F607C
		public StatesInstance(RocketEngine smi) : base(smi)
		{
		}
	}

	// Token: 0x0200192A RID: 6442
	public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
	{
		// Token: 0x06008630 RID: 34352 RVA: 0x0034B858 File Offset: 0x00349A58
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_pre").QueueAnim("launch_loop", true, null).Update(delegate(RocketEngine.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num))
				{
					SimMessages.EmitMass(num, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				int num2 = 10;
				for (int i = 1; i < num2; i++)
				{
					int num3 = Grid.OffsetCell(num, -1, -i);
					int num4 = Grid.OffsetCell(num, 0, -i);
					int num5 = Grid.OffsetCell(num, 1, -i);
					if (Grid.IsValidCell(num3))
					{
						SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num4))
					{
						SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num5))
					{
						SimMessages.ModifyEnergy(num5, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
		}

		// Token: 0x04006557 RID: 25943
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;

		// Token: 0x04006558 RID: 25944
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;

		// Token: 0x04006559 RID: 25945
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;
	}
}
