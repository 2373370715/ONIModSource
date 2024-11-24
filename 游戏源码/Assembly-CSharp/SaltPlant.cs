using System;

// Token: 0x020016F5 RID: 5877
public class SaltPlant : StateMachineComponent<SaltPlant.StatesInstance>
{
	// Token: 0x06007907 RID: 30983 RVA: 0x000EF9D1 File Offset: 0x000EDBD1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SaltPlant>(-724860998, SaltPlant.OnWiltDelegate);
		base.Subscribe<SaltPlant>(712767498, SaltPlant.OnWiltRecoverDelegate);
	}

	// Token: 0x06007908 RID: 30984 RVA: 0x000EF9FB File Offset: 0x000EDBFB
	private void OnWilt(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x06007909 RID: 30985 RVA: 0x000EFA0E File Offset: 0x000EDC0E
	private void OnWiltRecover(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x04005A98 RID: 23192
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWilt(data);
	});

	// Token: 0x04005A99 RID: 23193
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltRecoverDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWiltRecover(data);
	});

	// Token: 0x020016F6 RID: 5878
	public class StatesInstance : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.GameInstance
	{
		// Token: 0x0600790C RID: 30988 RVA: 0x000EFA5F File Offset: 0x000EDC5F
		public StatesInstance(SaltPlant master) : base(master)
		{
		}
	}

	// Token: 0x020016F7 RID: 5879
	public class States : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant>
	{
		// Token: 0x0600790D RID: 30989 RVA: 0x000EFA68 File Offset: 0x000EDC68
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.alive.DoNothing();
		}

		// Token: 0x04005A9A RID: 23194
		public GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.State alive;
	}
}
