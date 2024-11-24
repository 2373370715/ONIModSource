using System;
using KSerialization;

// Token: 0x020008F5 RID: 2293
[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineComponent<StateMachineInstanceType> : StateMachineComponent, ISaveLoadable where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000BA5F0 File Offset: 0x000B87F0
	public StateMachineInstanceType smi
	{
		get
		{
			if (this._smi == null)
			{
				this._smi = (StateMachineInstanceType)((object)Activator.CreateInstance(typeof(StateMachineInstanceType), new object[]
				{
					this
				}));
			}
			return this._smi;
		}
	}

	// Token: 0x060028A9 RID: 10409 RVA: 0x000BA629 File Offset: 0x000B8829
	public override StateMachine.Instance GetSMI()
	{
		return this._smi;
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x000BA636 File Offset: 0x000B8836
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnCleanUp");
			this._smi = default(StateMachineInstanceType);
		}
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x000BA66C File Offset: 0x000B886C
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.smi.StartSM();
		}
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x000BA68C File Offset: 0x000B888C
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnDisable");
		}
	}

	// Token: 0x04001B2D RID: 6957
	private StateMachineInstanceType _smi;
}
