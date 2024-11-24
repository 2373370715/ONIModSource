using System;
using UnityEngine;

// Token: 0x02000D52 RID: 3410
public class ElectrobankCharger : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>
{
	// Token: 0x060042C8 RID: 17096 RVA: 0x00242954 File Offset: 0x00240B54
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noBattery;
		this.noBattery.PlayAnim("off").EventHandler(GameHashes.OnStorageChange, delegate(ElectrobankCharger.Instance smi, object data)
		{
			smi.QueueElectrobank(null);
		}).ParamTransition<bool>(this.hasElectrobank, this.charging, GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.IsTrue).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.charging, (ElectrobankCharger.Instance smi) => smi.master.GetComponent<Operational>().IsOperational);
		this.charging.QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
			smi.master.GetComponent<Operational>().SetActive(true, false);
		}).Exit(delegate(ElectrobankCharger.Instance smi)
		{
			smi.master.GetComponent<Operational>().SetActive(false, false);
		}).Update(delegate(ElectrobankCharger.Instance smi, float dt)
		{
			smi.ChargeInternal(smi, dt);
		}, UpdateRate.SIM_EVERY_TICK, false).EventTransition(GameHashes.OperationalChanged, this.inoperational, (ElectrobankCharger.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).ParamTransition<float>(this.internalChargeAmount, this.full, (ElectrobankCharger.Instance smi, float dt) => this.internalChargeAmount.Get(smi) >= 120000f);
		this.full.PlayAnim("working_pst").Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.TransferChargeToElectrobank();
		}).OnAnimQueueComplete(this.noBattery);
	}

	// Token: 0x04002DA3 RID: 11683
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State noBattery;

	// Token: 0x04002DA4 RID: 11684
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State inoperational;

	// Token: 0x04002DA5 RID: 11685
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State charging;

	// Token: 0x04002DA6 RID: 11686
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State full;

	// Token: 0x04002DA7 RID: 11687
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.FloatParameter internalChargeAmount;

	// Token: 0x04002DA8 RID: 11688
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.BoolParameter hasElectrobank;

	// Token: 0x02000D53 RID: 3411
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D54 RID: 3412
	public new class Instance : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.GameInstance
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060042CC RID: 17100 RVA: 0x000CB23D File Offset: 0x000C943D
		public Storage Storage
		{
			get
			{
				if (this.storage == null)
				{
					this.storage = base.GetComponent<Storage>();
				}
				return this.storage;
			}
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x000CB25F File Offset: 0x000C945F
		public Instance(IStateMachineTarget master, ElectrobankCharger.Def def) : base(master, def)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x000CB28C File Offset: 0x000C948C
		public void ChargeInternal(ElectrobankCharger.Instance smi, float dt)
		{
			smi.sm.internalChargeAmount.Delta(dt * 480f, smi);
			this.UpdateMeter();
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x000CB2AD File Offset: 0x000C94AD
		public void UpdateMeter()
		{
			this.meterController.SetPositionPercent(base.sm.internalChargeAmount.Get(base.smi) / 120000f);
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x000CB2D6 File Offset: 0x000C94D6
		public void TransferChargeToElectrobank()
		{
			this.targetElectrobank = Electrobank.ReplaceEmptyWithCharged(this.targetElectrobank, true);
			this.DequeueElectrobank();
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x00242B40 File Offset: 0x00240D40
		public void DequeueElectrobank()
		{
			this.targetElectrobank = null;
			base.smi.sm.hasElectrobank.Set(false, base.smi, false);
			base.smi.sm.internalChargeAmount.Set(0f, base.smi, false);
			this.UpdateMeter();
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x00242B9C File Offset: 0x00240D9C
		public void QueueElectrobank(object data = null)
		{
			if (this.targetElectrobank == null)
			{
				for (int i = 0; i < this.Storage.items.Count; i++)
				{
					GameObject gameObject = this.Storage.items[i];
					if (gameObject != null && gameObject.HasTag(GameTags.EmptyPortableBattery))
					{
						this.targetElectrobank = gameObject;
						base.smi.sm.internalChargeAmount.Set(0f, base.smi, false);
						base.smi.sm.hasElectrobank.Set(true, base.smi, false);
						break;
					}
				}
			}
			this.UpdateMeter();
		}

		// Token: 0x04002DA9 RID: 11689
		private Storage storage;

		// Token: 0x04002DAA RID: 11690
		public GameObject targetElectrobank;

		// Token: 0x04002DAB RID: 11691
		private MeterController meterController;
	}
}
