using System;

// Token: 0x02000CB9 RID: 3257
public class Campfire : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>
{
	// Token: 0x06003F12 RID: 16146 RVA: 0x002364F4 File Offset: 0x002346F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.needsFuel);
		this.operational.needsFuel.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.working, new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel)).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.working.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.EnableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.needsFuel, GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Not(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel))).PlayAnim("on", KAnim.PlayMode.Loop).Exit(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission));
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x000C8FB3 File Offset: 0x000C71B3
	public static bool HasFuel(Campfire.Instance smi)
	{
		return smi.HasFuel;
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x000C8FBB File Offset: 0x000C71BB
	public static void EnableHeatEmission(Campfire.Instance smi)
	{
		smi.EnableHeatEmission();
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x000C8FC3 File Offset: 0x000C71C3
	public static void DisableHeatEmission(Campfire.Instance smi)
	{
		smi.DisableHeatEmission();
	}

	// Token: 0x04002AFF RID: 11007
	public const string LIT_ANIM_NAME = "on";

	// Token: 0x04002B00 RID: 11008
	public const string UNLIT_ANIM_NAME = "off";

	// Token: 0x04002B01 RID: 11009
	public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State noOperational;

	// Token: 0x04002B02 RID: 11010
	public Campfire.OperationalStates operational;

	// Token: 0x04002B03 RID: 11011
	public StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.BoolParameter WarmAuraEnabled;

	// Token: 0x02000CBA RID: 3258
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002B04 RID: 11012
		public Tag fuelTag;

		// Token: 0x04002B05 RID: 11013
		public float initialFuelMass;
	}

	// Token: 0x02000CBB RID: 3259
	public class OperationalStates : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State
	{
		// Token: 0x04002B06 RID: 11014
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State needsFuel;

		// Token: 0x04002B07 RID: 11015
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State working;
	}

	// Token: 0x02000CBC RID: 3260
	public new class Instance : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.GameInstance
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x000C8FDB File Offset: 0x000C71DB
		public bool HasFuel
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06003F1A RID: 16154 RVA: 0x000C8FEF File Offset: 0x000C71EF
		public bool IsAuraEnabled
		{
			get
			{
				return base.sm.WarmAuraEnabled.Get(this);
			}
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x000C9002 File Offset: 0x000C7202
		public Instance(IStateMachineTarget master, Campfire.Def def) : base(master, def)
		{
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x00236614 File Offset: 0x00234814
		public void EnableHeatEmission()
		{
			this.operational.SetActive(true, false);
			this.light.enabled = true;
			this.heater.EnableEmission = true;
			this.decorProvider.SetValues(CampfireConfig.DECOR_ON);
			this.decorProvider.Refresh();
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x00236664 File Offset: 0x00234864
		public void DisableHeatEmission()
		{
			this.operational.SetActive(false, false);
			this.light.enabled = false;
			this.heater.EnableEmission = false;
			this.decorProvider.SetValues(CampfireConfig.DECOR_OFF);
			this.decorProvider.Refresh();
		}

		// Token: 0x04002B08 RID: 11016
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04002B09 RID: 11017
		[MyCmpGet]
		public Storage storage;

		// Token: 0x04002B0A RID: 11018
		[MyCmpGet]
		public RangeVisualizer rangeVisualizer;

		// Token: 0x04002B0B RID: 11019
		[MyCmpGet]
		public Light2D light;

		// Token: 0x04002B0C RID: 11020
		[MyCmpGet]
		public DirectVolumeHeater heater;

		// Token: 0x04002B0D RID: 11021
		[MyCmpGet]
		public DecorProvider decorProvider;
	}
}
